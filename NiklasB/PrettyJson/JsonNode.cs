using System;
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
        public List<JsonNode> Elements;     // if JsonNodeType.Array
        public List<JsonMember> Members;    // if JsonNodeType.Object
        public object Value;                // if JsonNodeType.Value
    }

    struct JsonMember
    {
        public string Name;
        public JsonNode Value;
    }

}
