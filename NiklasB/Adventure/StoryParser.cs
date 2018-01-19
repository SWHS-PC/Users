using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Adventure
{
    class StoryParser
    {
        enum State
        {
            None,
            Page,
            Links
        }

        string m_title = "Text Adventure"; // TODO - .title tag
        Dictionary<string, Page> m_pages = new Dictionary<string, Page>();
        Page m_startPage = null;
        Page m_currentPage = null;
        StringBuilder m_description = new StringBuilder();
        State m_state = State.None;
        int m_lineNumber = 0;

        public bool Parse(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ++m_lineNumber;
                if (!ProcessLine(line.Trim()))
                    return false;
            }

            if (m_startPage == null)
            {
                LogError("No pages specified.");
                return false;
            }

            if (m_state == State.Page)
            {
                LogError(".page with no .end.");
                return false;
            }

            // Recursively follow links from the start page to determine
            // which pages are reachable.
            var keys = new HashSet<string>();
            keys.Add(m_startPage.Name);
            FollowLinks(m_startPage.Links, keys);

            // Output warnings for unreachable pages.
            foreach (var page in m_pages.Values)
            {
                if (!keys.Contains(page.Name))
                {
                    Console.Error.WriteLine($"Warning: The {page.Name} page is not reachable from the start page.");
                }
            }

            return true;
        }

        public void WriteHtml(string outputDir)
        {
            foreach (var page in m_pages.Values)
            {
                string fileName = $"{page.Name}.html";
                if (outputDir != null)
                {
                    fileName = Path.Combine(outputDir, fileName);
                }

                try
                {
                    using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
                    {
                        WritePage(page, writer);
                    }
                }
                catch (IOException)
                {
                    Console.Error.WriteLine($"Error: Could not create {fileName}.");
                    return;
                }
            }
        }

        void WritePage(Page page, XmlWriter writer)
        {
            writer.WriteDocType("html", null, null, null);
            writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
            writer.WriteAttributeString("lang", "en");

            writer.WriteStartElement("head");
            writer.WriteStartElement("meta");
            writer.WriteAttributeString("charset", "utf-8");
            writer.WriteEndElement(); // meta
            WriteElementWithText("title", m_title, writer);
            writer.WriteStartElement("link");
            writer.WriteAttributeString("rel", "stylesheet");
            writer.WriteAttributeString("type", "text/css");
            writer.WriteAttributeString("href", "style.css");
            writer.WriteEndElement(); // link
            writer.WriteEndElement(); // head

            writer.WriteStartElement("body");
            WriteParagraphs(page, writer);
            WriteLinks(page.Links, writer);
            writer.WriteEndElement(); // body

            writer.WriteEndElement(); // html
        }

        void WriteParagraphs(Page page, XmlWriter writer)
        {
            var paragraphs = page.Description.Split(
                new string[] { "\n\n" },
                StringSplitOptions.RemoveEmptyEntries
                );

            foreach (var par in paragraphs)
            {
                WriteElementWithText("p", par.Trim('\n'), writer);
            }
        }

        void WriteElementWithText(string tag, string text, XmlWriter writer)
        {
            writer.WriteStartElement(tag);
            writer.WriteString(text);
            writer.WriteEndElement();
        }

        void WriteLinks(List<Link> links, XmlWriter writer)
        {
            switch (links.Count)
            {
                case 0:
                    // No links, so it's the end of the store.
                    WriteElementWithText("p", "THE END", writer);
                    break;

                case 1:
                    // Only one link, so write it as a paragraph.
                    writer.WriteStartElement("p");
                    WriteLink(links[0], writer);
                    writer.WriteEndElement();
                    break;

                default:
                    // Multiple links, so write a prompt followed by
                    // an unordered list of links.
                    WriteElementWithText("p", "What do you do?", writer);
                    writer.WriteStartElement("ul");

                    foreach (var link in links)
                    {
                        writer.WriteStartElement("li");
                        WriteLink(link, writer);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement(); // ul
                    break;
            }
        }

        void WriteLink(Link link, XmlWriter writer)
        {
            writer.WriteStartElement("a");
            writer.WriteAttributeString("href", $"{link.Target.Name}.html");
            writer.WriteString(link.Text);
            writer.WriteEndElement();
        }

        void FollowLinks(List<Link> links, HashSet<string> keys)
        {
            foreach (var link in links)
            {
                var target = link.Target;
                if (!keys.Contains(target.Name))
                {
                    keys.Add(target.Name);
                    FollowLinks(target.Links, keys);
                }
            }
        }

        bool ProcessLine(string line)
        {
            if (line.Length == 0)
            {
                switch (m_state)
                {
                    case State.Page:
                        m_description.Append("\n");
                        break;

                    case State.Links:
                        m_state = State.None;
                        break;
                }
            }
            else if (line[0] == '.')
            {
                var tokens = line.Split();
                switch (tokens[0])
                {
                    case ".title":
                        if (tokens.Length < 2)
                        {
                            LogError("Text expected after .title tag.");
                            return false;
                        }
                        if (m_state != State.None || m_startPage != null)
                        {
                            LogError(".title must be the first tag in the file.");
                            return false;
                        }
                        m_title = line.Substring(tokens[0].Length + 1);
                        return true;

                    case ".page":
                        if (tokens.Length != 2)
                        {
                            LogError("Expected page name after .page tag.");
                            return false;
                        }
                        return BeginPage(tokens[1]);

                    case ".end":
                        if (tokens.Length != 1)
                        {
                            LogError("Unexpected token after .end tag.");
                            return false;
                        }
                        return EndPage();

                    case ".links":
                        if (tokens.Length != 2)
                        {
                            LogError("Expected page name after .links tag.");
                            return false;
                        }
                        return BeginLinks(tokens[1]);

                    default:
                        LogError($"Unknown tag: {tokens[0]}.");
                        return false;
                }
            }
            else
            {
                switch (m_state)
                {
                    case State.Page:
                        m_description.Append(line);
                        m_description.Append("\n");
                        break;

                    case State.Links:
                        {
                            var tokens = line.Split('=');
                            if (tokens.Length != 2)
                            {
                                LogError("A link must have the form '<text>=<pageName>'.");
                                return false;
                            }

                            string text = tokens[0].Trim();
                            string targetName = tokens[1].Trim();

                            Page target;
                            if (!m_pages.TryGetValue(targetName, out target))
                            {
                                LogError($"The {targetName} page is not defined.");
                                return false;
                            }

                            m_currentPage.Links.Add(new Link(text, target));
                        }
                        break;

                    default:
                        LogError("Unexpected text outside .page or .links section.");
                        return false;
                }
            }

            return true;
        }

        bool BeginPage(string pageName)
        {
            if (m_state != State.None)
            {
                LogError(".page tag not expected in this context.");
                return false;
            }

            if (m_pages.ContainsKey(pageName))
            {
                LogError($"The {pageName} page is already defined.");
                return false;
            }

            m_currentPage = new Page(pageName);
            m_pages.Add(pageName, m_currentPage);

            if (m_startPage == null)
            {
                m_startPage = m_currentPage;
            }

            m_state = State.Page;
            return true;
        }

        bool EndPage()
        {
            if (m_state != State.Page)
            {
                LogError(".end tag with no preceding .page tag.");
                return false;
            }

            m_currentPage.Description = m_description.ToString();
            m_description.Clear();

            m_state = State.None;
            return true;
        }

        bool BeginLinks(string pageName)
        {
            if (m_state != State.None)
            {
                LogError(".links tag not expected in this context.");
                return false;
            }

            if (!m_pages.TryGetValue(pageName, out m_currentPage))
            {
                LogError($"The {pageName} page is not defined.");
                return false;
            }

            m_state = State.Links;
            return true;
        }

        void LogError(string message)
        {
            Console.Error.Write($"Error line {m_lineNumber}: {message}");
        }

        public Page StartPage => m_startPage;
    }
}
