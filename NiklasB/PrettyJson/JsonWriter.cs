using System;
using System.Collections.Generic;
using System.IO;

namespace PrettyJson
{
    class JsonWriter
    {
        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public bool IsFormatted
        {
            get;
            set;
        }

        public void Write(JsonNode node)
        {
            WriteNode(node);
        }

        void WriteNode(JsonNode node)
        {
            switch (node.NodeType)
            {
                case JsonNodeType.Object:
                    WriteObject(node.Members);
                    break;

                case JsonNodeType.Array:
                    WriteArray(node.Elements);
                    break;

                case JsonNodeType.Value:
                    WriteValue(node.Value);
                    break;
            }
        }

        void WriteObject(IList<JsonMember> members)
        {
            Indent();
            _writer.Write('{');
            _nestLevel++;

            int count = members.Count;
            for (int i = 0; i < count; i++)
            {
                // Write a comma separator before all but the first member.
                if (i != 0)
                {
                    _writer.Write(',');
                }

                EndLine();
                Indent();

                // Write the member name followed by a colon.
                WriteString(members[i].Name);
                _writer.Write(": ");

                // Write simple value members on the same line and
                // complex values (arrays and objects) on a new line.
                var node = members[i].Value;
                if (node.NodeType == JsonNodeType.Value)
                {
                    // Simple value.
                    WriteValue(node.Value);
                }
                else
                {
                    // Complex value; indent and move to a new line.
                    ++_nestLevel;
                    EndLine();
                    Indent();
                    WriteNode(node);
                    --_nestLevel;
                }
            }

            _nestLevel--;
            EndLine();
            Indent();
            _writer.Write('}');
        }

        void WriteArray(IList<JsonNode> elements)
        {
            Indent();
            _writer.Write('[');
            _nestLevel++;

            int count = elements.Count;

            for (int i = 0; i < count; i++)
            {
                // Write a comma separator before all but the first element.
                if (i != 0)
                {
                    _writer.Write(", ");
                }
                EndLine();
                Indent();

                // Write the element itself.
                WriteNode(elements[i]);
            }

            _nestLevel--;
            EndLine();
            Indent();
            _writer.Write(']');
        }

        void WriteValue(object value)
        {
            if (value == null)
            {
                _writer.Write("null");
            }
            else if (value is string)
            {
                WriteString((string)value);
            }
            else if (value is bool)
            {
                _writer.Write((bool)value ? "true" : "false");
            }
            else
            {
                _writer.Write(value);
            }
        }

        void WriteString(string value)
        {
            // TODO - escape special characters
            _writer.Write($"\"{value}\"");
        }

        void EndLine()
        {
            if (IsFormatted && !_isNewLine)
            {
                _writer.WriteLine();
                _isNewLine = true;
            }
        }

        void Indent()
        {
            if (IsFormatted && _isNewLine)
            {
                for (int i = 0; i < _nestLevel; i++)
                {
                    _writer.Write("    ");
                }
                _isNewLine = false;
            }
        }

        TextWriter _writer;
        int _nestLevel = 0;
        bool _isNewLine = true;
    }
}
