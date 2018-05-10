using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    class JSONWriter
    {
        TextWriter writer;
        int nestLevel = 0;
        bool isNewLine = true;

        public JSONWriter(TextWriter write)
        {
            writer = write;
        }

        public bool IsFormatted
        {
            get; set;
        }

        public void Write(JSONNode node)
        {
            WriteNode(node);
        }

        void WriteNode(JSONNode node)
        {
            switch (node.NodeType)
            {
                case JSONNodeType.Object:
                    WriteObject(node.Members);
                    break;

                case JSONNodeType.Array:
                    WriteArray(node.Elements);
                    break;

                case JSONNodeType.Value:
                    WriteValue(node.Value);
                    break;
            }
        }

        void WriteObject(IList<JSONMember> members)
        {
            Indent();
            writer.Write('{');
            nestLevel++;

            int amount = members.Count;
            for (int i = 0; i < amount; i++)
            {
                if(i != 0)
                {
                    writer.Write(", ");
                }

                EndLine();
                Indent();

                WriteString(members[i].Name);
                writer.Write(": ");

                var node = members[i].Value;
                if (node.NodeType == JSONNodeType.Value)
                {
                    WriteValue(node.Value);
                }
                else
                {
                    nestLevel++;
                    EndLine();
                    Indent();
                    WriteNode(members[i].Value);
                    nestLevel--;
                }
            }

            nestLevel--;
            EndLine();
            Indent();
            writer.Write('}');

        }
        void WriteArray(IList<JSONNode> elements)
        {
            Indent();
            int amount = elements.Count;
            writer.Write('[');
            nestLevel++;

            for (int i = 0; i < amount; i++)
            {
                if (i != 0)
                {
                    writer.Write(", ");
                }

                EndLine();
                Indent();

                WriteNode(elements[i]);
            }
            nestLevel--;
            EndLine();
            Indent();
            writer.Write(']');

        }
        void WriteString(string value)
        {
            writer.Write($"\"{value}\"");
        }
        void WriteValue(object value)
        {
            if(value == null)
            {
                writer.Write("null");
            }
            else if(value is string)
            {
                WriteString((string)value);
            }
            else if(value is bool)
            {
                writer.Write((bool)value ? "true" : "false");
            }
            else
            {
                writer.Write(value);
            }
        }
        void Indent()
        {
            if(IsFormatted && isNewLine)
            {
                for(int i = 0; i < nestLevel; i++)
                {
                    writer.Write("    ");
                }
                isNewLine = false;
            }
        }
        void EndLine()
        {
            if(IsFormatted && !isNewLine)
            {
                writer.WriteLine();
                isNewLine = true;
            }
        }
    }
}
