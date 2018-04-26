#pragma once

class StringRef
{
public:
    ~StringRef();
    explicit StringRef(_In_z_ char const* text);
    explicit StringRef(gsl::span<char const> text);
    StringRef(StringRef const& rhs);
    void operator=(StringRef const& rhs);

    char const* c_str() const { return data_->text_; }
    uint32_t length() const   { return data_->length_; }

private:
    struct Data
    {
        uint32_t refCount_;
        uint32_t length_;
        _Field_size_(length_ + 1) _Null_terminated_ char text_[1];
    };

    static Data* NewData(_In_reads_(length) char const* source, size_t length);
    static void AddRef(Data* data);
    static void Release(Data* data);

    Data* data_;
};


class ExpressionException
{
public:
    ExpressionException(
        StringRef const& sourceExpression,
        int charIndex,
        char const* message
        ) :
        sourceExpression_(sourceExpression),
        charIndex_(charIndex),
        message_(message)
    {
    }

    char const* GetSource() const   { return sourceExpression_.c_str(); }
    int GetCharIndex() const        { return charIndex_; }
    char const* GetMessage() const  { return message_; }

private:
    StringRef sourceExpression_;
    int charIndex_;
    char const* message_;
};
