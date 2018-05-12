#pragma once
#include "Number.h"
#include "StringRef.h"

namespace num {

    class ExpressionContext
    {
    public:
        explicit ExpressionContext(gsl::span<Number const> paramValues = {}) :
            paramValues_(paramValues)
        {
        }

        Number GetParam(size_t index) const
        {
            return paramValues_[index];
        }

    private:
        gsl::span<Number const> paramValues_;
    };

    class IExpression
    {
    public:
        virtual ~IExpression()
        {
        }

        virtual Number Evaluate(ExpressionContext const& context) const = 0;
        virtual void Print() const = 0;
    };

    using ExpressionPtr = std::unique_ptr<IExpression>;

    struct Definition
    {
        std::string name;
        std::vector<std::string> paramNames;
        bool isFunction = false;
        ExpressionPtr expression;
    };

    using DefinitionPtr = std::shared_ptr<Definition>;
    using NameMap = std::map<std::string, DefinitionPtr>;

    struct DefinitionList
    {
        std::vector<DefinitionPtr> vec;
        NameMap map;
    };

    class EvaluationException : public ExpressionException
    {
    public:
        EvaluationException(StringRef const& sourceExpression, int charIndex, char const* message) :
            ExpressionException(sourceExpression, charIndex, message)
        {
        }
    };
} // end namespace num
