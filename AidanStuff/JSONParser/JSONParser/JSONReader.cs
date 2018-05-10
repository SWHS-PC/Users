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
        }
        public JSONNode Parse()
        {
            return new JSONNode
            {
                NodeType = JSONNodeType.Object,
                Members = new List<JSONMember>()
            };
        }
    }
}
