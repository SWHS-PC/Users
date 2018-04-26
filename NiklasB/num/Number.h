#pragma once

struct Number
{
    bool isDouble = false;
    int64_t intValue = 0;       // valid only if !isDouble
    double doubleValue = 0;     // always valid
};

inline Number MakeNumber(int64_t value)
{
    return Number{ false, value, static_cast<double>(value) };
}

inline Number MakeNumber(bool value)
{
    return MakeNumber(static_cast<int64_t>(value));
}

inline Number MakeNumber(double value)
{
    return Number{ true, 0, value };
}

inline Number MakeNaN()
{
    return MakeNumber(std::numeric_limits<double>::quiet_NaN());
}

void PrintNumber(Number value);
