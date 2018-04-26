#pragma once
#include "Number.h"
#include "StringRef.h"

enum class TokenType
{
    None,
    Number,         // integer or floating point literal
    Name,           // function or variable name
    StartGroup,     // "("
    EndGroup,       // ")"
    Comma,          // ","
    Question,       // "?"
    Colon,          // ":"
    Lamda,          // "=>"
    Operator        // binary or unary operator
};

template<typename T> using UnaryFunc = Number(*)(T arg);
template<typename T> using BinaryFunc = Number(*)(T leftArg, T rightArg);

struct UnaryOp
{
    char const* str;
    bool isPostfix;
    UnaryFunc<int64_t> intFunc;
    UnaryFunc<double> doubleFunc;
};

struct BinaryOp
{
    char const* str;
    int precedence;
    BinaryFunc<int64_t> intFunc;
    BinaryFunc<double> doubleFunc;
};

class Lexer
{
public:
    explicit Lexer(StringRef const& expression) : expression_(expression)
    {
        pos_ = expression.c_str();
        tokenPos_ = expression.c_str();
        Advance();
    }

    void Advance();

    StringRef const& GetSource() const  { return expression_; }
    int GetCharIndex() const            { return static_cast<int>(tokenPos_ - expression_.c_str()); }
    TokenType GetTokenType() const      { return tokenType_; }

    Number GetNumber() const            { return number_; }
    std::string const& GetName() const  { return name_; }
    BinaryOp const* GetBinaryOp() const { return binaryOp_; }
    UnaryOp const* GetUnaryOp() const   { return unaryOp_; }

    bool IsBinaryOp() const             { return binaryOp_ != nullptr; }
    bool IsUnaryOp() const              { return unaryOp_ != nullptr; }

    void Fail(char const* message = "Invalid token.") const;

private:
    StringRef expression_;
    _Field_z_ char const* tokenPos_;
    _Field_z_ char const* pos_;
    TokenType tokenType_ = TokenType::None;
    Number number_;
    std::string name_;
    UnaryOp const* unaryOp_ = nullptr;
    BinaryOp const* binaryOp_ = nullptr;

    bool TryReadNumber();
    bool TryReadName();
};

class ParserException : public ExpressionException
{
public:
    ParserException(Lexer const& lexer, char const* message) :
        ExpressionException(lexer.GetSource(), lexer.GetCharIndex(), message)
    {
    }
};
