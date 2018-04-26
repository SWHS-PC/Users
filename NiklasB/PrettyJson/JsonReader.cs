using System;
using System.IO;
using System.Collections.Generic;

namespace PrettyJson
{
    enum JsonNodeType
    {
        Array,
        Object,
        Value
    }

    class JsonNode
    {
        public JsonNodeType NodeType;
        public List<JsonNode> Array;
        public List<JsonMember> Object;
        public object Value;
    }

    struct JsonMember
    {
        public string Name;
        public JsonNode Value;
    }

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
            return new JsonNode
            {
                NodeType = JsonNodeType.Object,
                Object = new List<JsonMember>()
            };
        }

        TextReader _reader;
    }
}
