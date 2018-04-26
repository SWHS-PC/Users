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
        public JSONWriter(TextWriter write)
        {
            writer = write;
        }

        public void Write(JSONNode node)
        {

        }


    }
}
