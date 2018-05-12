#include "stdafx.h"
#include "Lexer.h"
#include "Expression.h"

namespace num {

    Number FunctionExpression::Evaluate(ExpressionContext const& context) const
    {
        size_t const argCount = ParamCount();

        std::vector<Number> args(argCount);

        for (size_t i = 0; i < argCount; ++i)
        {
            args[i] = args_[i]->Evaluate(context);
        }

        return functionDef_->expression->Evaluate(ExpressionContext(args));
    }

    void FunctionExpression::Print() const
    {
        printf("%s(", functionDef_->name.c_str());

        size_t const argCount = args_.size();
        for (size_t i = 0; i < argCount; ++i)
        {
            if (i > 0)
            {
                fputs(", ", stdout);
            }

            args_[i]->Print();
        }

        fputc(')', stdout);
    }

    Number UnaryExpression::Evaluate(ExpressionContext const& context) const
    {
        Number value = arg_->Evaluate(context);

        if (value.isDouble || op_->intFunc == nullptr)
        {
            if (op_->doubleFunc == nullptr)
            {
                throw EvaluationException(
                    sourceExpression_,
                    charIndex_,
                    "Operation not defined for double."
                );
            }

            return op_->doubleFunc(value.doubleValue);
        }
        else
        {
            return op_->intFunc(value.intValue);
        }
    }

    void UnaryExpression::Print() const
    {
        fputs(op_->str, stdout);
        arg_->Print();
    }

    Number BinaryExpression::Evaluate(ExpressionContext const& context) const
    {
        Number left = leftArg_->Evaluate(context);
        Number right = rightArg_->Evaluate(context);

        if (left.isDouble || right.isDouble || op_->intFunc == nullptr)
        {
            if (op_->doubleFunc == nullptr)
            {
                throw EvaluationException(
                    sourceExpression_,
                    charIndex_,
                    "Operation not defined for double."
                );
            }

            return op_->doubleFunc(left.doubleValue, right.doubleValue);
        }
        else
        {
            return op_->intFunc(left.intValue, right.intValue);
        }
    }

    void BinaryExpression::Print() const
    {
        fputc('(', stdout);
        leftArg_->Print();
        printf(") %s (", op_->str);
        rightArg_->Print();
        fputc(')', stdout);
    }

    Number TernaryExpression::Evaluate(ExpressionContext const& context) const
    {
        auto test = test_->Evaluate(context);
        return test.doubleValue != 0 ? first_->Evaluate(context) : second_->Evaluate(context);
    }

    void TernaryExpression::Print() const
    {
        test_->Print();
        fputs(" ? ", stdout);
        first_->Print();
        fputs(" : ", stdout);
        second_->Print();
    }

} // end namespace num
