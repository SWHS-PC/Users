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
        expression :            binary_expression ( "?" binary_expression ":" expression )?
        binary_expression :     unary_expression ( BINARY_OP unary_expression )*
        unary_expression :      PREFIX_OP* ( simple_expression POSTFIX_OP* )
        simple_expression :     NUMBER | group | variable_or_function
        group :                 "(" expression ")"
        variable_or_function :  NAME argument_list?
        argument_list :         "(" ( expression ( "," expression )* )? ")"
    */

    ExpressionPtr ParseFullExpression();

    ExpressionPtr ParseExpression();

private:
    void Fail(char const* message = "Syntax error.") const;

    ExpressionPtr ParseBinaryExpression();
    ExpressionPtr ParseBinaryExpression(ExpressionPtr&& leftExpr, int minPrecedence);
    ExpressionPtr ParseUnaryExpression();
    ExpressionPtr ParseSimpleExpression();
    ExpressionPtr ParseVariableOrFunction();
    ExpressionPtr ParseFunction(DefinitionPtr def);

    Lexer& lexer_;
    NameMap const& globals_;
    DefinitionPtr currentDefinition_;
};
