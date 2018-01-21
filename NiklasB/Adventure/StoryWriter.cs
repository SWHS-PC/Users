using System;
using System.IO;
using System.Xml;

namespace Adventure
{
    class StoryWriter
    {
        public static void Write(Story story, string outputFileName)
        {
            try
            {
                using (var writer = XmlWriter.Create(outputFileName, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
                {
                    // Write the doc type declaration and begin the html element.
                    writer.WriteDocType("html", null, null, null);
                    writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
                    writer.WriteAttributeString("lang", "en");

                    WriteHead(writer, story);
                    WriteBody(writer, story);

                    writer.WriteEndElement(); // html
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Error: Could not open output file: {outputFileName}.");
            }
        }

        static void WriteHead(XmlWriter writer, Story story)
        {
            // Write the head element and its contents.
            writer.WriteStartElement("head");

            writer.WriteStartElement("meta");
            writer.WriteAttributeString("charset", "utf-8");
            writer.WriteEndElement(); // meta

            WriteElementWithText(writer, "title", story.Title);

            writer.WriteStartElement("link");
            writer.WriteAttributeString("rel", "stylesheet");
            writer.WriteAttributeString("type", "text/css");
            writer.WriteAttributeString("href", "style.css");
            writer.WriteEndElement(); // link

            WriteElementWithText(
                writer, 
                "script",
                "\n" +
                $"var currentId = '{story.StartPage.Name}';\n" +
                "function show(id)\n" +
                "{\n" +
                "    document.getElementById(currentId).style = 'display:none';\n" +
                "    document.getElementById(id).style = 'display:block';\n" +
                "    currentId = id;\n" +
                "}\n"
                );

            writer.WriteEndElement(); // head
        }

        static void WriteBody(XmlWriter writer, Story story)
        {
            writer.WriteStartElement("body");

            writer.WriteStartElement("h1");
            WriteLink(writer, story.Title, story.StartPage.Name);
            writer.WriteEndElement(); // h1

            foreach (var page in story.Pages)
            {
                writer.WriteStartElement("div");
                writer.WriteAttributeString("id", page.Name);
                if (page != story.StartPage)
                {
                    writer.WriteAttributeString("style", "display:none");
                }

                WriteParagraphs(writer, page);
                WriteLinks(writer, page);

                writer.WriteEndElement(); // div
            }

            writer.WriteEndElement(); // body
        }

        static void WriteParagraphs(XmlWriter writer, Page page)
        {
            // Split the description into paragraphs by looking for
            // double newline characters.
            var paragraphs = page.Description.Split(
                new string[] { "\n\n" },
                StringSplitOptions.RemoveEmptyEntries
                );

            // Write each paragraph as a p element.
            foreach (var par in paragraphs)
            {
                WriteElementWithText(writer, "p", par.Trim('\n'));
            }
        }

        static void WriteLinks(XmlWriter writer, Page page)
        {
            // Branch depending on the number of links.
            var links = page.Links;
            switch (links.Count)
            {
                case 0:
                    // No links, so it's the end of the story.
                    WriteElementWithText(writer, "p", "THE END");
                    break;

                case 1:
                    // Only one link, so write it as a paragraph.
                    writer.WriteStartElement("p");
                    WriteLink(writer, links[0]);
                    writer.WriteEndElement();
                    break;

                default:
                    // Multiple links, so write a prompt followed by
                    // an unordered list of links.
                    WriteElementWithText(writer, "p", "What do you do?");
                    writer.WriteStartElement("ul");

                    foreach (var link in links)
                    {
                        writer.WriteStartElement("li");
                        WriteLink(writer, link);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement(); // ul
                    break;
            }
        }

        static void WriteLink(XmlWriter writer, Link link)
        {
            WriteLink(writer, link.Text, link.Target.Name);
        }

        static void WriteLink(XmlWriter writer, string text, string pageName)
        {
            writer.WriteStartElement("a");
            writer.WriteAttributeString("href", $"javascript:show('{pageName}')");
            writer.WriteString(text);
            writer.WriteEndElement();
        }

        // Helper method to write an element that contains text.
        static void WriteElementWithText(XmlWriter writer, string tag, string text)
        {
            writer.WriteStartElement(tag);
            writer.WriteString(text);
            writer.WriteEndElement();
        }
   }
}
