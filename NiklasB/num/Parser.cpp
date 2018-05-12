#include "stdafx.h"
#include "Parser.h"
#include "Expression.h"

namespace num {

    ExpressionPtr Parser::ParseFullExpression()
    {
        auto expression = ParseExpression();

        if (lexer_.GetTokenType() != TokenType::None)
        {
            Fail("Unexpected token after expression.");
        }

        return expression;
    }

    ExpressionPtr Parser::ParseExpression()
    {
        auto expression = ParseBinaryExpression();

        if (lexer_.GetTokenType() == TokenType::Question)
        {
            lexer_.Advance();

            auto first = ParseBinaryExpression();

            if (lexer_.GetTokenType() != TokenType::Colon)
            {
                Fail("':' expected.");
            }
            lexer_.Advance();

            auto second = ParseExpression();

            expression = std::make_unique<TernaryExpression>(
                std::move(expression),
                std::move(first),
                std::move(second)
                );
        }

        return expression;
    }

    ExpressionPtr Parser::ParseBinaryExpression()
    {
        auto expression = ParseUnaryExpression();

        if (lexer_.IsBinaryOp())
        {
            expression = ParseBinaryExpression(std::move(expression), 0);
        }

        return expression;
    }

    // Recursively parse binary expressions using the precedence climbing method.
    // The top-level call should specify minPrecedence = 0.
    ExpressionPtr Parser::ParseBinaryExpression(ExpressionPtr&& leftExpr, int minPrecedence)
    {
        // Process all binary operators >= minPrecedence.
        while (lexer_.IsBinaryOp() && lexer_.GetBinaryOp()->precedence >= minPrecedence)
        {
            // Get the current operator and the right-hand expression.
            auto op = lexer_.GetBinaryOp();
            int charIndex = lexer_.GetCharIndex();
            lexer_.Advance();
            ExpressionPtr rightExpr = ParseUnaryExpression();

            // Is the next token an operator of higher precedence than the current operator?
            // If so, recursively parse it, and let the right-hand expression be the result.
            while (lexer_.IsBinaryOp() && lexer_.GetBinaryOp()->precedence > op->precedence)
            {
                rightExpr = ParseBinaryExpression(
                    std::move(rightExpr),
                    lexer_.GetBinaryOp()->precedence
                );
            }

            // Replace the left-hand expression with the binary expression.
            leftExpr = std::make_unique<BinaryExpression>(
                lexer_.GetSource(),
                charIndex,
                op,
                std::move(leftExpr),
                std::move(rightExpr)
                );
        }

        return std::move(leftExpr);
    }

    ExpressionPtr Parser::ParseUnaryExpression()
    {
        // Parse unary prefix operators if any, such that:
        //  - rootExpression is the expression for the outermost operator
        //  - unaryExpression is the expression for the innermost operator
        //  - both are nullptr if there are no prefix operators
        ExpressionPtr rootExpression;
        UnaryExpression* unaryExpression = nullptr;

        while (lexer_.GetTokenType() == TokenType::Operator)
        {
            auto op = lexer_.GetUnaryOp();
            if (op == nullptr || op->isPostfix)
            {
                Fail("The specified operator is not a unary prefix operator.");
            }
            auto expression = std::make_unique<UnaryExpression>(
                lexer_.GetSource(),
                lexer_.GetCharIndex(),
                op
                );
            lexer_.Advance();

            // The new unary expression will either become the root expression,
            // or the argument to the innermost existing unary expression.
            ExpressionPtr& ptr = unaryExpression == nullptr ?
                rootExpression :
                unaryExpression->ArgumentReference();

            unaryExpression = expression.get();

            ptr = std::move(expression);
        }

        // Parse the simple expression.
        ExpressionPtr inner = ParseSimpleExpression();

        // Parse any postfix unary operators.
        while (lexer_.IsUnaryOp() && lexer_.GetUnaryOp()->isPostfix)
        {
            // Create a unary expression for the postfix operator.
            auto postOp = std::make_unique<UnaryExpression>(
                lexer_.GetSource(),
                lexer_.GetCharIndex(),
                lexer_.GetUnaryOp()
                );
            lexer_.Advance();

            // Combine the previous inner expression with the unary expression.
            postOp->ArgumentReference() = std::move(inner);
            inner = std::move(postOp);
        }

        // The inner expression will either become the root expression,
        // or the argument to the innermost existing unary expression.
        ExpressionPtr& ptr = unaryExpression == nullptr ?
            rootExpression :
            unaryExpression->ArgumentReference();

        ptr = std::move(inner);

        // Return the root expression.
        return rootExpression;
    }

    ExpressionPtr Parser::ParseSimpleExpression()
    {
        ExpressionPtr expression;

        switch (lexer_.GetTokenType())
        {
        case TokenType::Number:
            expression = std::make_unique<NumberExpression>(lexer_.GetNumber());
            lexer_.Advance();
            break;

        case TokenType::StartGroup:
            lexer_.Advance();
            expression = ParseExpression();
            if (lexer_.GetTokenType() != TokenType::EndGroup)
                Fail("Expected ')'.");
            lexer_.Advance();
            break;

        case TokenType::Name:
            expression = ParseVariableOrFunction();
            break;

        default:
            Fail();
        }

        return expression;
    }

    ExpressionPtr Parser::ParseVariableOrFunction()
    {
        std::string const& name = lexer_.GetName();

        // Is the expression we're parsing part of a function definition?
        if (currentDefinition_ != nullptr && currentDefinition_->isFunction)
        {
            // Determine if the name matches a parameter name.
            auto& paramNames = currentDefinition_->paramNames;
            auto param = std::find(paramNames.begin(), paramNames.end(), name);
            if (param != paramNames.end())
            {
                lexer_.Advance();
                size_t paramIndex = param - paramNames.begin();
                return std::make_unique<ParamExpression>(currentDefinition_, paramIndex);
            }

            // Determine if the name matches the current function, in which
            // case this is a recursive function call.
            if (name == currentDefinition_->name)
            {
                return ParseFunction(currentDefinition_);
            }
        }

        // Look up the name in the global name map.
        auto global = globals_.find(name);
        if (global == globals_.end())
        {
            Fail("Name not found.");
        }

        DefinitionPtr def = global->second;

        if (def->isFunction)
        {
            return ParseFunction(def);
        }
        else
        {
            lexer_.Advance();
            return std::make_unique<VariableExpression>(std::move(def));
        }
    }

    ExpressionPtr Parser::ParseFunction(DefinitionPtr def)
    {
        assert(lexer_.GetName() == def->name);
        lexer_.Advance();

        if (lexer_.GetTokenType() != TokenType::StartGroup)
        {
            Fail("Expected '(' after function name.");
        }
        lexer_.Advance();

        size_t const paramCount = def->paramNames.size();

        std::vector<std::unique_ptr<IExpression>> args;
        args.reserve(paramCount);

        if (lexer_.GetTokenType() == TokenType::EndGroup)
        {
            lexer_.Advance();
        }
        else
        {
            for (;;)
            {
                args.push_back(ParseExpression());

                if (lexer_.GetTokenType() == TokenType::EndGroup)
                {
                    lexer_.Advance();
                    break;
                }

                if (lexer_.GetTokenType() != TokenType::Comma)
                {
                    Fail("Expected ',' or ')'.");
                }
                lexer_.Advance();
            }
        }

        if (args.size() != paramCount)
        {
            Fail(
                args.size() < paramCount ?
                "Too few arguments to function" :
                "Too many arguments to function."
            );
        }

        return std::make_unique<FunctionExpression>(def, std::move(args));
    }

    void Parser::Fail(char const* message) const
    {
        lexer_.Fail(message);
    }
} // end namespace num
