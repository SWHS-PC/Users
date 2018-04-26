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

        public void Write(JsonNode node)
        {
            // TODO
        }

        TextWriter _writer;
    }
}
