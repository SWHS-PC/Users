using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    class JSONReader
    {
        TextReader reader;
        public JSONReader(TextReader read)
        {
            reader = read;
            NextChar();
        }
        public JSONNode Parse()
        {
            var rootNode = ParseNode();

            SkipSpaces();

            if (HaveChar)
            {
                Fail();
            }
            return rootNode;
        }

        JSONNode ParseNode()
        {
            SkipSpaces();

            switch (ch)
            {
                case '{': return ParseObject();
                case '[': return ParseArray();
                default: return ParseValue();

            }
        }
        JSONNode ParseObject()
        {
            ReadChar('{');
            SkipSpaces();

            var members = new List<JSONMember>();

            while (ch != '}' && HaveChar)
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

                JSONNode value = ParseNode();
                SkipSpaces();

                members.Add(new JSONMember { Name = name, Value = value });
            }

            ReadChar('}');

            return new JSONNode { NodeType = JSONNodeType.Object, Members = members };
        }
        JSONNode ParseArray()
        {
            ReadChar('[');

            SkipSpaces();

            var elements = new List<JSONNode>();

            while (ch != ']' && HaveChar)
            {
                if(elements.Count != 0)
                {
                    ReadChar(',');
                    SkipSpaces();
                }
                elements.Add(ParseNode());
            }

            ReadChar(']');

            return new JSONNode { NodeType = JSONNodeType.Array, Elements = elements };
        }

        JSONNode ParseValue()
        {
            object value = null;

            if (ch == '\"')
            {
                value = ParseString();
            }
            else if(ch == '-' || char.IsDigit((char)ch))
            {
                value = ParseNumber();
            }
            else
            {
                switch (ParseKeyword())
                {
                    case "true": value = true;
                        break;
                    case "false": value = false;
                        break;
                    case "null":
                        break;
                    default: Fail();
                        break;
                }
            }
            return new JSONNode { NodeType = JSONNodeType.Value, Value = value };
        }

        string ParseKeyword()
        {
            ReadChar('\"');

            var builder = new StringBuilder();

            while (ch >= 'a' && ch <= 'z')
            {
                builder.Append(ch);
                NextChar();
            }

            ReadChar('\"');

            return builder.ToString();
        }

        string ParseString()
        {
            ReadChar('\"');

            var builder = new StringBuilder();

            while(ch != '\"' && HaveChar)
            {
                if (ch != '\\')
                {
                    builder.Append((char)ch);
                }
                else
                {
                    NextChar();
                    switch (ch)
                    {
                        case '\"':
                        case '\\':
                        case '/':
                            builder.Append((char)ch);
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
                                int value = 0;
                                for(int i = 0; i <4; i++)
                                {
                                    NextChar();
                                    int digit;
                                    if(!TryParseHexDigit(ch, out digit))
                                    {
                                        Fail();
                                    }

                                    value = value * 0x10 + digit;
                                }
                                builder.Append((char)value);
                            }
                            break;

                    }
                }

                NextChar();
            }

            ReadChar('\"');

            return builder.ToString();
        }

        bool TryParseHexDigit(int ch, out int digit)
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

        bool IsNumberChar =>
            (ch >= '0' && ch <= '9') || ch == '.' || ch == '+' || ch == '-' || ch == 'e' || ch == 'E';

        object ParseNumber()
        {
            var builder = new StringBuilder();

            while (IsNumberChar)
            {
                builder.Append((char)ch);
                NextChar();
            }
            string s = builder.ToString();

            long intValue;
            if(long.TryParse(s, out intValue))
            { return intValue; }

            double doubleValue;
            if(double.TryParse(s, out doubleValue))
            { return doubleValue; }

            Fail();
            return null;
        }

        void SkipSpaces()
        {
            while (char.IsWhiteSpace((char)ch))
            {
                NextChar();
            }
        }

        void ReadChar(char expectedChar)
        {
            if (ch != expectedChar)
            {
                Fail();
            }
            NextChar();
        }

        void NextChar()
        {
            ch = reader.Read();
            charPos++;
        }

        void Fail()
        {
            throw new ApplicationException($"JSON syntax error at character {charPos}.");
        }

        bool HaveChar => ch != EofChar;
        const int EofChar = -1;
        int ch = 0;
        int charPos = 0;
    }
}
