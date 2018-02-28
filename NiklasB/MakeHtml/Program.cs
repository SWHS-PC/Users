using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MakeHtml
{
    class Program
    {
        const string m_usage = "MakeHtml <input-file.txt> <output-file.html>";

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine(m_usage);
                return;
            }

            try
            {
                // Read the input text file.
                var lines = ReadTextFile(args[0]);

                // Write the output HTML file.
                WriteHtml(lines, args[1]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        static List<string> ReadTextFile(string fileName)
        {
            var lines = new List<string>();

            // Create a stream reader; the using statement
            // ensures the file will be closed.
            using (var reader = new StreamReader(fileName))
            {
                // Read each line and add it to the list.
                string line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }

            return lines;
        }

        static void WriteHtml(List<string> lines, string fileName)
        {
            using (var writer = XmlWriter.Create(
                fileName,
                new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }
                ))
            {
                // Write the doc type declaration and HTML start tag.
                writer.WriteDocType("html", null, null, null);
                writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
                writer.WriteAttributeString("lang", "en");

                // Write the head element.
                writer.WriteStartElement("head");
                writer.WriteStartElement("title");
                writer.WriteString(fileName);
                writer.WriteEndElement(); // title
                writer.WriteEndElement(); // head

                // Begin the body element.
                writer.WriteStartElement("body");

                // Loop over the input lines.
                bool inPara = false;
                foreach (var line in lines)
                {
                    var text = line.Trim();
                    if (text.Length == 0)
                    {
                        // It's a blank line, so end the current paragraph if we're in one.
                        if (inPara)
                        {
                            writer.WriteEndElement();
                            inPara = false;
                        }
                    }
                    else
                    {
                        // It's not a blank line. Write a newline character if we're already
                        // in a paragraph; otherwise begin a paragraph.
                        if (inPara)
                        {
                            writer.WriteString("\n");
                        }
                        else
                        {
                            writer.WriteStartElement("p");
                            inPara = true;
                        }

                        // Write the text.
                        writer.WriteString(text);
                    }
                }

                // End the last paragraph element if we're in one.
                if (inPara)
                {
                    writer.WriteEndElement();
                }

                // End the body and HTML elements.
                writer.WriteEndElement(); // body
                writer.WriteEndElement(); // html
            }
        }
    }
}
