using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PrettyJson
{
    /// <summary>
    /// Parses JSON into a tree of JsonNode according to
    /// the grammer defined at http://json.org.
    /// </summary>
    class JsonReader
    {
        public JsonReader(TextReader reader)
        {
            // Save the reader.
            _reader = reader;

            // Read the first character and make it the current character.
            // In other words, advance to the first character.
            NextChar();
        }

        public JsonNode Parse()
        {
            // Parse the root node.
            var rootNode = ParseNode();

            SkipSpaces();

            if (HaveChar)
                Fail();

            return rootNode;
        }

        JsonNode ParseNode()
        {
            SkipSpaces();

            switch (_ch)
            {
                case '{': return ParseObject();
                case '[': return ParseArray();
                default: return ParseValue();
            }
        }

        JsonNode ParseObject()
        {
            ReadChar('{');
            SkipSpaces();

            var members = new List<JsonMember>();

            // Read the members
            while (_ch != '}' && HaveChar)
            {
                if (members.Count != 0)
                {
                    ReadChar(',');
                    SkipSpaces();
                }

                string name = ParseString();
                SkipSpaces();

                ReadChar(':');
                SkipSpaces();

                JsonNode value = ParseNode();
                SkipSpaces();

                members.Add(new JsonMember { Name = name, Value = value });
            }

            ReadChar('}');

            return new JsonNode { NodeType = JsonNodeType.Object, Members = members };
        }

        JsonNode ParseArray()
        {
            ReadChar('[');
            SkipSpaces();

            var elements = new List<JsonNode>();

            while (_ch != ']' && HaveChar)
            {
                if (elements.Count != 0)
                {
                    ReadChar(',');
                    SkipSpaces();
                }

                elements.Add(ParseNode());

                SkipSpaces();
            }

            ReadChar(']');

            return new JsonNode { NodeType = JsonNodeType.Array, Elements = elements };
        }

        JsonNode ParseValue()
        {
            object value = null;

            if (_ch == '\"')
            {
                value = ParseString();
            }
            else if (_ch == '-' || char.IsDigit((char)_ch))
            {
                value = ParseNumber();
            }
            else
            {
                switch (ParseKeyword())
                {
                    case "true":
                        value = true;
                        break;

                    case "false":
                        value = false;
                        break;

                    case "null":
                        // value is already null
                        break;

                    default:
                        Fail();
                        break;
                }
            }

            return new JsonNode { NodeType = JsonNodeType.Value, Value = value };
        }

        bool IsNumberChar =>
            (_ch >= '0' && _ch <= '9') ||
            _ch == '.' || _ch == '+' || _ch == '-' || _ch == 'e' || _ch == 'E';

        object ParseNumber()
        {
            var builder = new StringBuilder();

            while (IsNumberChar)
            {
                builder.Append((char)_ch);
                NextChar();
            }

            string s = builder.ToString();

            long intValue;
            if (long.TryParse(s, out intValue))
                return intValue;

            double doubleValue;
            if (double.TryParse(s, out doubleValue))
                return doubleValue;

            Fail();
            return null;
        }

        string ParseString()
        {
            ReadChar('\"');

            var builder = new StringBuilder();

            while (_ch != '\"' && HaveChar)
            {
                if (_ch != '\\')
                {
                    // Not an escape sequence: just append the character.
                    builder.Append((char)_ch);
                    }
                else
                {
                    // Get the next character after the '\\'.
                    NextChar();
                    switch (_ch)
                    {
                        case '\"':
                        case '\\':
                        case '/':
                            builder.Append((char)_ch);
                            break;

                        case 'b':
                            builder.Append('\b');
                            break;

                        case 'f':
                            builder.Append('\f');
                            break;

                        case 'n':
                            builder.Append('\n');
                            break;

                        case 'r':
                            builder.Append('\r');
                            break;

                        case 't':
                            builder.Append('\t');
                            break;

                        case 'u':
                            {
                                // We expect four hexadecimal digits after \u.
                                int value = 0;
                                for (int i = 0; i < 4; ++i)
                                {
                                    NextChar();

                                    int digit;
                                    if (!TryParseHexDigit(_ch, out digit))
                                    {
                                        Fail();
                                    }

                                    value = value * 0x10 + digit;
                                }
                                builder.Append((char)value);
                            }
                            break;

                        default:
                            Fail();
                            break;
                    }
                }
                NextChar();
            }

            ReadChar('\"');

            return builder.ToString();
        }

        static bool TryParseHexDigit(int ch, out int digit)
        {
            if (ch >= '0' && ch <= '9')
            {
                digit = ch - '0';
                return true;
            }
            else if (ch >= 'a' && ch <= 'f')
            {
                digit = ch - ('a' - 10);
                return true;
            }
            else if (ch >= 'A' && ch <= 'F')
            {
                digit = ch - ('A' - 10);
                return true;
            }
            else
            {
                digit = 0;
                return false;
            }
        }

        string ParseKeyword()
        {
            // There's no explicit concept of "keyword" in JSON, but
            // this method is used to read sequences of characters that
            // correspond to the special named values "true", "false", 
            // and "null".
            var builder = new StringBuilder();

            while (_ch >= 'a' && _ch <= 'z')
            {
                builder.Append((char)_ch);
                NextChar();
            }

            return builder.ToString();
        }

        void SkipSpaces()
        {
            while (char.IsWhiteSpace((char)_ch))
            {
                NextChar();
            }
        }

        /// <summary>
        /// Verifies that the current character is the specified value
        /// and advances past it.
        /// </summary>
        void ReadChar(char expectedChar)
        {
            if (_ch != expectedChar)
                Fail();

            NextChar();
        }

        void NextChar()
        {
            _ch = _reader.Read();
            _charPos++;
        }

        bool HaveChar => _ch != EofChar;

        void Fail()
        {
            throw new ApplicationException($"JSON syntax error at character {_charPos}.");
        }

        TextReader _reader;
        int _ch = 0;            // current character value or EofChar
        int _charPos = 0;       // character position in stream

        const int EofChar = -1; // TextReader::Read returns this on end of file
    }
}
