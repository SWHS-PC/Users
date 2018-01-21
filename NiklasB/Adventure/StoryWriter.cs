using System;
using System.IO;
using System.Xml;

namespace Adventure
{
    class StoryWriter : IDisposable
    {
        public static void Write(Story story, string outputFileName)
        {
            try
            {
                if (story.IsOneFile)
                {
                    using (var storyWriter = new StoryWriter(story, outputFileName, null))
                    {
                        storyWriter.WriteInternal();
                    }
                }
                else
                {
                    foreach (var page in story.Pages)
                    {
                        using (var storyWriter = new StoryWriter(story, outputFileName, page))
                        {
                            storyWriter.WriteInternal();
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Error: Could not open output file: {outputFileName}.");
            }
        }

        StoryWriter(Story story, string outputFileName, Page page)
        {
            m_story = story;
            m_outputFileName = outputFileName;
            m_page = page;

            m_writer = XmlWriter.Create(
                GetPageFileName(page != null ? page : story.StartPage), 
                new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }
                );
        }

        Story m_story;
        string m_outputFileName;
        Page m_page;
        XmlWriter m_writer;

        void IDisposable.Dispose()
        {
            m_writer.Dispose();
        }

        string GetPageFileName(Page page)
        {
            if (page == m_story.StartPage)
            {
                return m_outputFileName;
            }
            else
            {
                string baseName = Path.GetFileNameWithoutExtension(m_outputFileName);
                string ext = Path.GetExtension(m_outputFileName);
                return $"{baseName}_{page.Name}{ext}";
            }
        }

        string GetPageUrl(Page target)
        {
            return m_page != null ?
                GetPageFileName(target) :
                $"javascript:show('{target.Name}')";
        }

        void WriteInternal()
        {
            // Write the doc type declaration and begin the html element.
            m_writer.WriteDocType("html", null, null, null);
            m_writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
            m_writer.WriteAttributeString("lang", "en");

            WriteHead();
            WriteBody();

            m_writer.WriteEndElement(); // html
        }

        void WriteHead()
        {
            // Write the head element and its contents.
            m_writer.WriteStartElement("head");

            m_writer.WriteStartElement("meta");
            m_writer.WriteAttributeString("charset", "utf-8");
            m_writer.WriteEndElement(); // meta

            WriteElementWithText("title", m_story.Title);

            m_writer.WriteStartElement("link");
            m_writer.WriteAttributeString("rel", "stylesheet");
            m_writer.WriteAttributeString("type", "text/css");
            m_writer.WriteAttributeString("href", "style.css");
            m_writer.WriteEndElement(); // link

            if (m_page == null)
            {
                WriteElementWithText(
                    "script",
                    "\n" +
                    $"var currentId = '{m_story.StartPage.Name}';\n" +
                    "function show(id)\n" +
                    "{\n" +
                    "    document.getElementById(currentId).style = 'display:none';\n" +
                    "    document.getElementById(id).style = 'display:block';\n" +
                    "    currentId = id;\n" +
                    "}\n"
                    );
            }

            m_writer.WriteEndElement(); // head
        }

        void WriteBody()
        {
            m_writer.WriteStartElement("body");

            m_writer.WriteStartElement("h1");
            if (m_page != m_story.StartPage)
            {
                WriteLink(m_story.Title, m_story.StartPage);
            }
            else
            {
                m_writer.WriteString(m_story.Title);
            }
            m_writer.WriteEndElement(); // h1

            if (m_page != null)
            {
                // This file contains only one page, so write its content.
                WriteParagraphs(m_page);
                WriteLinks(m_page);
            }
            else
            {
                // We're writing all the pages as one file, so write a div
                // element for each page.
                foreach (var page in m_story.Pages)
                {
                    m_writer.WriteStartElement("div");
                    m_writer.WriteAttributeString("id", page.Name);
                    if (page != m_story.StartPage)
                    {
                        m_writer.WriteAttributeString("style", "display:none");
                    }

                    WriteParagraphs(page);
                    WriteLinks(page);

                    m_writer.WriteEndElement(); // div
                }
            }

            m_writer.WriteEndElement(); // body
        }

        void WriteParagraphs(Page page)
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
                WriteElementWithText("p", par.Trim('\n'));
            }
        }

        void WriteLinks(Page page)
        {
            // Branch depending on the number of links.
            var links = page.Links;
            switch (links.Count)
            {
                case 0:
                    // No links, so it's the end of the m_story.
                    WriteElementWithText("p", "THE END");
                    break;

                case 1:
                    // Only one link, so write it as a paragraph.
                    m_writer.WriteStartElement("p");
                    WriteLink(links[0]);
                    m_writer.WriteEndElement();
                    break;

                default:
                    // Multiple links, so write a prompt followed by
                    // an unordered list of links.
                    WriteElementWithText("p", "What do you do?");
                    m_writer.WriteStartElement("ul");

                    foreach (var link in links)
                    {
                        m_writer.WriteStartElement("li");
                        WriteLink(link);
                        m_writer.WriteEndElement();
                    }

                    m_writer.WriteEndElement(); // ul
                    break;
            }
        }

        void WriteLink(Link link)
        {
            WriteLink(link.Text, link.Target);
        }

        void WriteLink(string text, Page target)
        {
            m_writer.WriteStartElement("a");

            m_writer.WriteAttributeString("href", GetPageUrl(target));
            m_writer.WriteString(text);
            m_writer.WriteEndElement();
        }

        // Helper method to write an element that contains text.
        void WriteElementWithText(string tag, string text)
        {
            m_writer.WriteStartElement(tag);
            m_writer.WriteString(text);
            m_writer.WriteEndElement();
        }
    }
}
