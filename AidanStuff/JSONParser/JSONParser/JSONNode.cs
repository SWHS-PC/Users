using System;
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
        public List<JSONNode> Elements;
        public List<JSONMember> Members;
        public object Value;
    }
}
