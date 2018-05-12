#include "stdafx.h"
#include "Lexer.h"

namespace num {
    enum Precedence
    {
        PrecedenceBitwiseOr,
        PrecedenceBitwiseXor,
        PrecedenceBitwiseAnd,
        PrecedenceEquality,
        PrecedenceInequality,
        PrecedenceShift,
        PrecedencePlusMinus,
        PrecedenceMultiplyDivide,
        PrecedencePower,
    };

    int64_t Power(int64_t leftArg, int64_t rightArg)
    {
        int64_t value = 1;
        for (int64_t i = 0; i < rightArg; ++i)
        {
            value *= leftArg;
        }
        return value;
    }

    UnaryOp const g_unaryPlus =
    {
        "+",
        false,
        [](int64_t arg) { return MakeNumber(arg); },
        [](double arg) { return MakeNumber(arg); }
    };
    BinaryOp const g_binaryPlus =
    {
        "+",
        PrecedencePlusMinus,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg + rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg + rightArg); }
    };
    UnaryOp const g_unaryMinus =
    {
        "-",
        false,
        [](int64_t arg) { return MakeNumber(-arg); },
        [](double arg) { return MakeNumber(-arg); }
    };
    BinaryOp const g_binaryMinus =
    {
        "-",
        PrecedencePlusMinus,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg - rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg - rightArg); }
    };
    UnaryOp const g_unaryBitwiseNot =
    {
        "~",
        false,
        [](int64_t arg) { return MakeNumber(~arg); },
        nullptr
    };
    BinaryOp const g_binaryPower =
    {
        "**",
        PrecedencePower,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(Power(leftArg, rightArg)); },
        [](double leftArg, double rightArg) { return MakeNumber(pow(leftArg, rightArg)); }
    };
    BinaryOp const g_binaryTimes =
    {
        "*",
        PrecedenceMultiplyDivide,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg * rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg * rightArg); }
    };
    BinaryOp const g_binaryDivide =
    {
        "/",
        PrecedenceMultiplyDivide,
        [](int64_t leftArg, int64_t rightArg) { return rightArg == 0 ? MakeNaN() : MakeNumber(leftArg / rightArg); },
        [](double leftArg, double rightArg) { return rightArg == 0 ? MakeNaN() : MakeNumber(leftArg / rightArg); }
    };
    BinaryOp const g_binaryModulo =
    {
        "%",
        PrecedenceMultiplyDivide,
        [](int64_t leftArg, int64_t rightArg) { return rightArg == 0 ? MakeNaN() : MakeNumber(leftArg % rightArg); },
        [](double leftArg, double rightArg) { return rightArg == 0 ? MakeNaN() : MakeNumber(fmod(leftArg, rightArg)); }
    };
    BinaryOp const g_binaryAnd =
    {
        "&",
        PrecedenceBitwiseAnd,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg & rightArg); },
        nullptr
    };
    BinaryOp const g_binaryXor =
    {
        "^",
        PrecedenceBitwiseXor,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg ^ rightArg); },
        nullptr
    };
    BinaryOp const g_binaryOr =
    {
        "|",
        PrecedenceBitwiseOr,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg | rightArg); },
        nullptr
    };
    BinaryOp const g_leftShift =
    {
        "<<",
        PrecedenceShift,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg << rightArg); },
        nullptr
    };
    BinaryOp const g_rightShift =
    {
        ">>",
        PrecedenceBitwiseOr,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg >> rightArg); },
        nullptr
    };
    BinaryOp const g_lessEqual =
    {
        "<=",
        PrecedenceInequality,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg <= rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg <= rightArg); }
    };
    BinaryOp const g_greaterEqual =
    {
        ">=",
        PrecedenceInequality,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg >= rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg >= rightArg); }
    };
    BinaryOp const g_less =
    {
        "<",
        PrecedenceInequality,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg < rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg < rightArg); }
    };
    BinaryOp const g_greater =
    {
        ">",
        PrecedenceInequality,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg > rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg > rightArg); }
    };
    BinaryOp const g_equal =
    {
        "=",
        PrecedenceEquality,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg == rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg == rightArg); }
    };
    BinaryOp const g_notEqual =
    {
        "!=",
        PrecedenceEquality,
        [](int64_t leftArg, int64_t rightArg) { return MakeNumber(leftArg != rightArg); },
        [](double leftArg, double rightArg) { return MakeNumber(leftArg != rightArg); }
    };

    void Lexer::Advance()
    {
        // Advance past whitespace.
        while (*pos_ == ' ')
        {
            ++pos_;
        }

        // Save the position of the token.
        tokenPos_ = pos_;

        // Reset the token information.
        tokenType_ = TokenType::None;
        number_ = Number();
        name_.clear();
        binaryOp_ = nullptr;
        unaryOp_ = nullptr;

        // End of string or comment?
        if (*pos_ == '\0' || *pos_ == ';')
            return;

        if (TryReadNumber())
        {
            return;
        }

        if (TryReadName())
        {
            return;
        }

        switch (*pos_)
        {
        case '(':
            tokenType_ = TokenType::StartGroup;
            ++pos_;
            return;

        case ')':
            tokenType_ = TokenType::EndGroup;
            ++pos_;
            return;

        case ',':
            tokenType_ = TokenType::Comma;
            ++pos_;
            return;

        case '?':
            tokenType_ = TokenType::Question;
            ++pos_;
            return;

        case ':':
            tokenType_ = TokenType::Colon;
            ++pos_;
            return;

        case '=':
            if (pos_[1] == '>')
            {
                tokenType_ = TokenType::Lamda;
                pos_ += 2;
            }
            else
            {
                binaryOp_ = &g_equal;
                tokenType_ = TokenType::Operator;
                ++pos_;
            }
            return;

        case '!':
            if (pos_[1] == '=')
            {
                binaryOp_ = &g_notEqual;
                tokenType_ = TokenType::Operator;
                pos_ += 2;
                return;
            }
            break;

        case '+':
            unaryOp_ = &g_unaryPlus;
            binaryOp_ = &g_binaryPlus;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '-':
            unaryOp_ = &g_unaryMinus;
            binaryOp_ = &g_binaryMinus;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '~':
            unaryOp_ = &g_unaryBitwiseNot;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '*':
            if (pos_[1] == '*')
            {
                binaryOp_ = &g_binaryPower;
                tokenType_ = TokenType::Operator;
                pos_ += 2;
            }
            else
            {
                binaryOp_ = &g_binaryTimes;
                tokenType_ = TokenType::Operator;
                ++pos_;
            }
            return;

        case '/':
            binaryOp_ = &g_binaryDivide;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '%':
            binaryOp_ = &g_binaryModulo;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '&':
            binaryOp_ = &g_binaryAnd;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '^':
            binaryOp_ = &g_binaryXor;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '|':
            binaryOp_ = &g_binaryXor;
            tokenType_ = TokenType::Operator;
            ++pos_;
            return;

        case '<':
            switch (pos_[1])
            {
            case '<':
                binaryOp_ = &g_leftShift;
                tokenType_ = TokenType::Operator;
                pos_ += 2;
                return;
            case '=':
                binaryOp_ = &g_lessEqual;
                tokenType_ = TokenType::Operator;
                pos_ += 2;
                return;
            default:
                binaryOp_ = &g_less;
                tokenType_ = TokenType::Operator;
                pos_++;
                return;
            }

        case '>':
            switch (pos_[1])
            {
            case '>':
                binaryOp_ = &g_rightShift;
                tokenType_ = TokenType::Operator;
                pos_ += 2;
                return;
            case '=':
                binaryOp_ = &g_greaterEqual;
                tokenType_ = TokenType::Operator;
                pos_ += 2;
                return;
            default:
                binaryOp_ = &g_greater;
                tokenType_ = TokenType::Operator;
                pos_++;
                return;
            }
        }

        Fail();
    }

    void Lexer::Fail(char const* message) const
    {
        throw ParserException(*this, message);
    }


    inline bool IsDecimalDigit(char ch)
    {
        return ch >= '0' && ch <= '9';
    }

    inline _Success_(return) bool IsHexDigit(char ch, _Out_ int* value)
    {
        if (ch >= '0' && ch <= '9')
        {
            *value = ch - '0';
            return true;
        }
        else if (ch >= 'a' && ch <= 'f')
        {
            *value = ch - ('a' - 10);
            return true;
        }
        else if (ch >= 'A' && ch <= 'F')
        {
            *value = ch - ('A' - 10);
            return true;
        }
        else
        {
            return false;
        }
    }

    _Success_(return) bool ParseHexNumber(
        char const* p,
        _Out_ char const** end,
        _Out_ int64_t* result
    )
    {
        int digit;

        if (*p == '0' && p[1] == 'x' && IsHexDigit(p[2], &digit))
        {
            int64_t value = digit;
            p += 2;
            while (IsHexDigit(*p, &digit))
            {
                value = (value << 4) + digit;
                ++p;
            }

            *end = p;
            *result = value;
            return true;
        }
        else
        {
            return false;
        }
    }

    _Success_(return) bool ParseDecimalNumber(
        char const* p,
        _Out_ char const** end,
        _Out_ Number* result
    )
    {
        bool isNegative = false;

        if (*p == '-')
        {
            isNegative = true;
            ++p;
        }

        char const* start = p;

        int64_t value = 0;
        double numerator = 0;
        double denominator = 0;

        while (IsDecimalDigit(*p))
        {
            value = (value * 10) + (*p - '0');
            ++p;
        }

        if (*p == '.' && IsDecimalDigit(p[1]))
        {
            numerator = p[1] - '0';
            denominator = 10;
            p += 2;

            while (IsDecimalDigit(*p))
            {
                numerator = (numerator * 10) + (*p - '0');
                denominator *= 10;
                ++p;
            }
        }

        if (p > start)
        {
            if (isNegative)
            {
                value = -value;
                numerator = -numerator;
            }

            *result = (denominator == 0) ?
                MakeNumber(value) :
                MakeNumber(value + (numerator / denominator));

            *end = p;
            return true;
        }
        else
        {
            return false;
        }
    }

    inline bool IsNameStartChar(char ch)
    {
        return (ch >= 'a' && ch <= 'z') ||
            (ch >= 'A' && ch <= 'Z') ||
            (ch == '_');
    }

    inline bool IsNameChar(char ch)
    {
        return IsNameStartChar(ch) || IsDecimalDigit(ch);
    }

    bool Lexer::TryReadNumber()
    {
        Number value;
        char const* endPos = pos_;
        bool haveNumber = false;

        if (ParseHexNumber(pos_, &endPos, &value.intValue))
        {
            haveNumber = true;
        }
        else if (ParseDecimalNumber(pos_, &endPos, &value))
        {
            Number exponent;
            if (*endPos == 'e' && ParseDecimalNumber(endPos + 1, &endPos, &exponent))
            {
                value = MakeNumber(value.doubleValue * pow(10.0, exponent.doubleValue));
            }
            haveNumber = true;
        }

        if (haveNumber && !IsNameChar(*endPos))
        {
            tokenType_ = TokenType::Number;
            number_ = value;
            pos_ = endPos;
            return true;
        }

        return false;
    }

    bool Lexer::TryReadName()
    {
        if (IsNameStartChar(*pos_))
        {
            char const* start = pos_++;

            while (IsNameChar(*pos_))
            {
                ++pos_;
            }

            name_.assign(start, pos_);
            tokenType_ = TokenType::Name;
            return true;
        }
        return false;
    }

} // end namespace num
