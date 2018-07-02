using System;
using System.Collections.Generic;

namespace TextAdventure
{
    class CommandParser
    {
        IList<string> m_formatStrings;
        Dictionary<string, int> m_verbToCommandMap = new Dictionary<string, int>();

        public CommandParser(IList<string> formatStrings)
        {
            m_formatStrings = formatStrings;

            for (int commandIndex = 0; commandIndex < formatStrings.Count; commandIndex++)
            {
                var token = new StringToken(formatStrings[commandIndex]);
                token.Next();
                string verb = new StringToken(formatStrings[commandIndex]).ToString();

                string shortVerb = null;
                int shortLength = verb.IndexOf('[');
                if (shortLength > 0)
                {
                    int startIndex = shortLength + 1;
                    int endIndex = verb.IndexOf(']', startIndex);
                    if (endIndex < 0)
                        throw new ArgumentException($"Missing ']' in: {verb}.");

                    shortVerb = verb.Substring(0, shortLength);
                    verb = shortVerb + verb.Substring(startIndex, endIndex - startIndex);
                }

                m_verbToCommandMap.Add(verb, commandIndex);

                if (shortVerb != null)
                {
                    m_verbToCommandMap.Add(shortVerb, commandIndex);
                }
            }
        }

        public bool ParseCommand(string input, IList<string> args, out int commandIndex)
        {
            commandIndex = -1;
            args.Clear();

            var inputToken = new StringToken(input);

            int i;
            if (!m_verbToCommandMap.TryGetValue(inputToken.ToString(), out i))
                return false;

            commandIndex = i;

            var formatToken = new StringToken(m_formatStrings[i]);

            // Iterate over all tokens of the format string.
            while (formatToken.HaveNext)
            {
                // Fail if there's no input to match the format token.
                if (!inputToken.HaveNext)
                    return false;

                // Advance both tokens.
                inputToken.Next();
                formatToken.Next();

                if (formatToken[0] != '<')
                {
                    // The format token is a literal word rather than a placeholder.
                    // Make sure the input token matches.
                    if (inputToken != formatToken)
                        return false;
                }
                else
                {
                    // The format token is a placeholder, so we need to determine the range
                    // if input tokens that match. First, skip over "the" if present in the input.
                    if (inputToken == "the")
                    {
                        inputToken.Next();
                    }

                    // Is the placeholder token at the end of the format string?
                    if (!formatToken.HaveNext)
                    {
                        // It's the last token, so match the rest of the input string.
                        inputToken.ExtendToEnd();
                        args.Add(inputToken.ToString());
                    }
                    else
                    {
                        // Remember the input range and advance both tokens.
                        int inputStartIndex = inputToken.StartIndex;
                        int inputEndIndex = inputToken.EndIndex;
                        formatToken.Next();
                        inputToken.Next();

                        // The current format token is expected to be a literal marker
                        // word that follows the placeholder. Scan ahead until we find
                        // an input token that matches the marker word.
                        while (inputToken != formatToken)
                        {
                            // Fail if we reach the end without finding the marker word.
                            if (!inputToken.HaveNext)
                                return false;

                            // Remember the input end position and advance the input token.
                            inputEndIndex = inputToken.EndIndex;
                            inputToken.Next();
                        }

                        // Add the input substring that corresponds to the placeholder.
                        args.Add(inputToken.Source.Substring(inputStartIndex, inputEndIndex - inputStartIndex));
                    }
                }
            }

            // Make sure there's no additional unexpected input.
            return !inputToken.HaveNext;
        }
    }
}
