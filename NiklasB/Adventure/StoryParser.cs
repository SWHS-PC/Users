using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            return true;
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

            m_currentPage = new Page();
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
