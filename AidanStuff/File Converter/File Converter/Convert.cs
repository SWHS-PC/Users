using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace File_Converter
{
    public class Convert
    {
        public static void TextToHtml(string[] args)
        {
            const string mUsage = "HTML <inputFile> <outputFile>";
            if (args.Length != 3)
            {
                Console.Error.WriteLine(mUsage);
                return;
            }

            WriteHTML(ReadTextFile(args[1]), args[2]);
        }
        static List<string> ReadTextFile(string fileName)
        {
            var lines = new List<string>();

            using (var read = new StreamReader(fileName))
            {
                string line = read.ReadLine();
                while(line != null)
                {
                    lines.Add(line);
                    line = read.ReadLine();

                }
            }

            return lines;
        }
        static void WriteHTML(List<string> lines, string filename)
        {
            using (var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteDocType("html", null, null, null);
                writer.WriteStartElement("html", "");
                writer.WriteAttributeString("lang", "en");

                writer.WriteStartElement("head");

                writer.WriteStartElement("title");
                writer.WriteString("MyTitle");
                writer.WriteEndElement();//title

                writer.WriteEndElement();//head

                writer.WriteStartElement("body");

                writer.WriteStartElement("div");
                writer.WriteStartElement("pre");
                foreach (var line in lines)
                {
                    writer.WriteString(line);
                    writer.WriteString("\n");
                }
                writer.WriteEndElement();//pre
                writer.WriteEndElement();//div

                writer.WriteEndElement();//body

                writer.WriteEndElement();//html
            }
        }
    }
}