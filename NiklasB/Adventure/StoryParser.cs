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

        Story m_story = new Story();
        Dictionary<string, Page> m_pages = new Dictionary<string, Page>();
        Page m_currentPage = null;
        StringBuilder m_description = new StringBuilder();
        State m_state = State.None;
        int m_lineNumber = 0;

        Page StartPage => m_story.StartPage;

        static public Story Parse(string fileName)
        {
            Story story = null;

            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    story = new StoryParser().ParseInternal(reader);
                }
            }
            catch (IOException)
            {
                Console.Error.Write($"Error: Could not open {fileName}.");
            }

            return story;
        }

        Story ParseInternal(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ++m_lineNumber;
                if (!ProcessLine(line.Trim()))
                    return null;
            }

            if (StartPage == null)
            {
                LogError("No pages specified.");
                return null;
            }

            if (m_state == State.Page)
            {
                LogError(".page with no .end.");
                return null;
            }

            // Recursively follow links from the start page to determine
            // which pages are reachable.
            var keys = new HashSet<string>();
            keys.Add(StartPage.Name);
            FollowLinks(StartPage.Links, keys);

            // Output warnings for unreachable pages.
            foreach (var page in m_pages.Values)
            {
                if (!keys.Contains(page.Name))
                {
                    Console.Error.WriteLine($"Warning: The {page.Name} page is not reachable from the start page.");
                }
            }

            return m_story;
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
                        if (m_state != State.None || StartPage != null)
                        {
                            LogError(".title must be the first tag in the file.");
                            return false;
                        }
                        m_story.Title = line.Substring(tokens[0].Length + 1);
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

            // Create the new page.
            var page = new Page(pageName);

            // Add the page to the story, and make it the start
            // page if there isn't one already.
            m_story.Pages.Add(page);
            if (m_story.StartPage == null)
            {
                m_story.StartPage = page;
            }

            // Add the page to the dictionary.
            m_pages.Add(pageName, page);

            // Set the current page and the state.
            m_currentPage = page;
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
    }
}
