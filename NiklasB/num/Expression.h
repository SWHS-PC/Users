#pragma once
#include "IExpression.h"

namespace num {

    class NumberExpression : public IExpression
    {
    public:
        explicit NumberExpression(Number value) : value_(value)
        {
        }

        Number Evaluate(ExpressionContext const& context) const override
        {
            return value_;
        }

        void Print() const override
        {
            PrintNumber(value_);
        }

    private:
        Number value_;
    };

    class VariableExpression : public IExpression
    {
    public:
        explicit VariableExpression(DefinitionPtr&& def) : def_(std::move(def))
        {
        }

        Number Evaluate(ExpressionContext const& context) const override
        {
            return def_->expression->Evaluate(context);
        }

        void Print() const override
        {
            fputs(def_->name.c_str(), stdout);
        }

    private:
        DefinitionPtr def_;
    };

    class FunctionExpression : public IExpression
    {
    public:
        explicit FunctionExpression(
            DefinitionPtr functionDef,
            std::vector<std::unique_ptr<IExpression>>&& args
        ) :
            functionDef_(std::move(functionDef)),
            args_(std::move(args))
        {
            assert(args_.size() == functionDef_->paramNames.size());
        }

        size_t ParamCount() const
        {
            return args_.size();
        }

        Number Evaluate(ExpressionContext const& context) const override;

        void Print() const override;

    private:
        DefinitionPtr functionDef_;
        std::vector<std::unique_ptr<IExpression>> args_;
    };

    class ParamExpression : public IExpression
    {
    public:
        ParamExpression(DefinitionPtr functionDef, size_t paramIndex) :
            functionDef_{ std::move(functionDef) },
            paramIndex_{ paramIndex }
        {
        }

        Number Evaluate(ExpressionContext const& context) const override
        {
            return context.GetParam(paramIndex_);
        }

        void Print() const override
        {
            fputs(functionDef_->paramNames[paramIndex_].c_str(), stdout);
        }

    private:
        DefinitionPtr functionDef_;
        size_t paramIndex_;
    };

    struct UnaryOp;

    class UnaryExpression : public IExpression
    {
    public:
        UnaryExpression(StringRef const& sourceExpression, int charIndex, UnaryOp const* op) :
            sourceExpression_(sourceExpression),
            charIndex_(charIndex),
            op_(op)
        {
        }

        ExpressionPtr& ArgumentReference() { return arg_; }

        Number Evaluate(ExpressionContext const& context) const override;

        void Print() const override;

    private:
        StringRef sourceExpression_;
        int charIndex_;
        UnaryOp const* op_;
        ExpressionPtr arg_;
    };

    class BinaryExpression : public IExpression
    {
    public:
        BinaryExpression(
            StringRef const& sourceExpression,
            int charIndex,
            BinaryOp const* op,
            ExpressionPtr&& leftArg,
            ExpressionPtr&& rightArg
        ) :
            sourceExpression_(sourceExpression),
            charIndex_(charIndex),
            op_(op),
            leftArg_(std::move(leftArg)),
            rightArg_(std::move(rightArg))
        {
        }

        Number Evaluate(ExpressionContext const& context) const override;

        void Print() const override;

    private:
        StringRef sourceExpression_;
        int charIndex_;
        BinaryOp const* op_;
        ExpressionPtr leftArg_;
        ExpressionPtr rightArg_;
    };

    class TernaryExpression : public IExpression
    {
    public:
        TernaryExpression(ExpressionPtr&& test, ExpressionPtr&& first, ExpressionPtr&& second) :
            test_{ std::move(test) },
            first_{ std::move(first) },
            second_{ std::move(second) }
        {
        }

        Number Evaluate(ExpressionContext const& context) const override;

        void Print() const override;

    private:
        ExpressionPtr test_;
        ExpressionPtr first_;
        ExpressionPtr second_;
    };
} // end namespace num
