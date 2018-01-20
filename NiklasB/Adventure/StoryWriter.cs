using System;
using System.IO;
using System.Xml;

namespace Adventure
{
    class StoryWriter
    {
        public static void Write(Story story, string outputDir)
        {
            // If an output directory is specified, ensure that it exists.
            // Otherwise, output files will be written to the current
            // directory.
            if (outputDir != null)
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception)
                {
                    Console.Error.WriteLine($"Error: Could not create directory: {outputDir}.");
                    return;
                }
            }

            // Write each page using a PageWriter object.
            foreach (var page in story.Pages)
            {
                try
                {
                    // PageWriter implements IDisposable. The using statement
                    // ensures that the Dispose method is called when we leave
                    // the using block.
                    using (var writer = new PageWriter(story, page, outputDir))
                    {
                        writer.Write();
                    }
                }
                catch (IOException)
                {
                    Console.Error.WriteLine($"Error: Could not create file: {page.Name}.html.");
                    return;
                }
            }
        }

        // PageWriter class used to write a specific page.
        // Implement the IDisposable interface because the PageWriter
        // creates an XmlWriter, which itself implements IDisposable.
        class PageWriter : IDisposable
        {
            public PageWriter(Story story, Page page, string outputDir)
            {
                string fileName = GetFileName(page);
                if (outputDir != null)
                {
                    fileName = Path.Combine(outputDir, fileName);
                }

                Writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });
                Story = story;
                Page = page;
            }

            public void Dispose()
            {
                Writer.Dispose();
            }

            XmlWriter Writer { get; }
            Story Story { get; }
            Page Page { get; }

            public void Write()
            {
                // Write the doc type declaration and begin the html element.
                Writer.WriteDocType("html", null, null, null);
                Writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
                Writer.WriteAttributeString("lang", "en");

                // Write the head element and its contents.
                Writer.WriteStartElement("head");
                Writer.WriteStartElement("meta");
                Writer.WriteAttributeString("charset", "utf-8");
                Writer.WriteEndElement(); // meta
                WriteElementWithText("title", Story.Title);
                Writer.WriteStartElement("link");
                Writer.WriteAttributeString("rel", "stylesheet");
                Writer.WriteAttributeString("type", "text/css");
                Writer.WriteAttributeString("href", "style.css");
                Writer.WriteEndElement(); // link
                Writer.WriteEndElement(); // head

                // Write the body element and its contents.
                Writer.WriteStartElement("body");
                WriteHeading();
                WriteParagraphs();
                WriteLinks();
                Writer.WriteEndElement(); // body

                // End the html element.
                Writer.WriteEndElement(); // html
            }

            static string GetFileName(Page page)
            {
                return page.Name + ".html";
            }

            // Helper method to write the page heading as an h1 element.
            void WriteHeading()
            {
                Writer.WriteStartElement("h1");
                if (Page == Story.StartPage)
                {
                    Writer.WriteString(Story.Title);
                }
                else
                {
                    WriteLink(Story.Title, GetFileName(Story.StartPage));
                }
                Writer.WriteEndElement();
            }

            // Helper method to write the page description is a series of
            // paragraph elements.
            void WriteParagraphs()
            {
                // Split the description into paragraphs by looking for
                // double newline characters.
                var paragraphs = Page.Description.Split(
                    new string[] { "\n\n" },
                    StringSplitOptions.RemoveEmptyEntries
                    );

                // Write each paragraph as a p element.
                foreach (var par in paragraphs)
                {
                    WriteElementWithText("p", par.Trim('\n'));
                }
            }

            // Helper method to write an element that contains text.
            void WriteElementWithText(string tag, string text)
            {
                // Replace double hyphens with the Unicode em dash character.
                text = text.Replace("--", "\u2014");

                Writer.WriteStartElement(tag);
                Writer.WriteString(text);
                Writer.WriteEndElement();
            }

            // Helper method to write the links from the current page.
            void WriteLinks()
            {
                // Branch depending on the number of links.
                var links = Page.Links;
                switch (links.Count)
                {
                    case 0:
                        // No links, so it's the end of the story.
                        WriteElementWithText("p", "THE END");
                        break;

                    case 1:
                        // Only one link, so write it as a paragraph.
                        Writer.WriteStartElement("p");
                        WriteLink(links[0]);
                        Writer.WriteEndElement();
                        break;

                    default:
                        // Multiple links, so write a prompt followed by
                        // an unordered list of links.
                        WriteElementWithText("p", "What do you do?");
                        Writer.WriteStartElement("ul");

                        foreach (var link in links)
                        {
                            Writer.WriteStartElement("li");
                            WriteLink(link);
                            Writer.WriteEndElement();
                        }

                        Writer.WriteEndElement(); // ul
                        break;
                }
            }

            // Helper method to write a link given a Link object.
            void WriteLink(Link link)
            {
                WriteLink(link.Text, GetFileName(link.Target));
            }

            // Helper method to write a link given text and a file name.
            void WriteLink(string text, string targetUrl)
            {
                Writer.WriteStartElement("a");
                Writer.WriteAttributeString("href", targetUrl);
                Writer.WriteString(text);
                Writer.WriteEndElement();
            }
        }
    }
}
