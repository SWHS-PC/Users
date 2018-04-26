#pragma once
#include "Lexer.h"
#include "IExpression.h"

class Parser
{
public:
    Parser(
        Lexer& lexer, 
        NameMap const& globals,
        DefinitionPtr currentDefinition = {}
        ) :
        lexer_{ lexer },
        globals_{ globals },
        currentDefinition_{ currentDefinition }
    {
    }

    /*
    Grammar:

        expression              =>  binary_expression ( "?" binary_expression ":" expression )?
        binary_expression       =>  unary_expression ( BINARY_OP unary_expression )*
        unary_expression        =>  PREFIX_OP* ( simple_expression POSTFIX_OP* )
        simple_expression       =>  NUMBER | group | variable_or_function
        group                   =>  "(" expression ")"
        variable_or_function    =>  NAME argument_list?
        argument_list           =>  "(" ( expression ( "," expression )* )? ")"

        Note that the grammer for binary_expression does not take into account operator
        precedence. The ParseBinaryExpression method uses the precedence climbing method
        to construct an expression tree that, for example, correctly distinguishes 
        between the following two cases:

        Expression     Syntax Tree

        5 * 2 + 3           +
                           / \
                          *   3
                         / \
                        5   2

        5 + 2 * 3           +
                           / \
                          5   *
                             / \
                            2   3
    */

    // ParseFullExpression parses an expression and verifies that it is not followed
    // by any additional tokens.
    ExpressionPtr ParseFullExpression();

    // ParseExpression parses an expression according to the above grammar. The
    // expression may be followed by additional tokens, as in the case where it's
    // part of a larger expression.
    ExpressionPtr ParseExpression();

private:
    // Helper method to throw a parser exception.
    void Fail(char const* message = "Syntax error.") const;

    // Methods for parsing elements of the grammer.
    ExpressionPtr ParseBinaryExpression();
    ExpressionPtr ParseBinaryExpression(ExpressionPtr&& leftExpr, int minPrecedence);
    ExpressionPtr ParseUnaryExpression();
    ExpressionPtr ParseSimpleExpression();
    ExpressionPtr ParseVariableOrFunction();
    ExpressionPtr ParseFunction(DefinitionPtr def);

    // Lexer used to read input tokens.
    Lexer& lexer_;

    // Global name map used to resolve variable and parameter names.
    NameMap const& globals_;

    // Definition pointer used to resolve parameter names if the current
    // expression is part of a function definition; can be null.
    DefinitionPtr currentDefinition_;
};
