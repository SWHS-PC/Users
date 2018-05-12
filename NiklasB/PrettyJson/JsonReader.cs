using System;
using System.IO;
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
            _reader = reader;
        }

        public JsonNode Parse()
        {
            // TODO
            var node = new JsonNode
            {
                NodeType = JsonNodeType.Object,
                Members = new List<JsonMember>()
            };

            node.Members.Add(new JsonMember
            {
                Name = "foo",
                Value = new JsonNode
                {
                    NodeType = JsonNodeType.Value,
                    Value = "Hello"
                }
            });

            var elements = new List<JsonNode>();
            elements.Add(new JsonNode { NodeType = JsonNodeType.Value, Value = 1 });
            elements.Add(new JsonNode { NodeType = JsonNodeType.Value, Value = 2.00001 });
            elements.Add(new JsonNode { NodeType = JsonNodeType.Value, Value = "element 3" });

            node.Members.Add(new JsonMember
            {
                Name = "fred",
                Value = new JsonNode
                {
                    NodeType = JsonNodeType.Array,
                    Elements = elements
                }
            });

            return node;
        }

        TextReader _reader;
    }
}
