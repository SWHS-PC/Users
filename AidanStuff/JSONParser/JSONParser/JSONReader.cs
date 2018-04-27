using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    struct JSONMember
    {
        public string Name;
        public JSONNode Value;
    }

    enum JSONNodeType
    {
        Array,
        Object,
        Value
    }
    class JSONNode
    {
        public JSONNodeType NodeType;
        public List<JSONNode> Array;
        public List<JSONMember> Object;
        public object Value;
    }

    class JSONReader
    {
        TextReader reader;
        public JSONReader(TextReader read)
        {
            reader = read;
        }
        public JSONNode Parse()
        {
            return new JSONNode
            {
                NodeType = JSONNodeType.Object,
                Object = new List<JSONMember>()
            };
        }
    }
}
