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
                string verb;
                int charPos = GetToken(formatStrings[commandIndex], 0, out verb);

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

            string verb;
            int inputCharPos = GetToken(input, 0, out verb);

            int i;
            if (!m_verbToCommandMap.TryGetValue(verb, out i))
                return false;

            commandIndex = i;

            string formatString = m_formatStrings[i];
            int formatCharPos = SkipToken(formatString, 0);

            // Iterate over all tokens of the format string.
            while (formatCharPos < formatString.Length)
            {
                // Fail if there's no input to match the format token.
                if (inputCharPos == input.Length)
                    return false;

                // Get the format token.
                string formatToken;
                formatCharPos = GetToken(formatString, formatCharPos, out formatToken);

                // Remember the input position and get an input token.
                int inputStartPos = inputCharPos;
                string inputToken;
                inputCharPos = GetToken(input, inputCharPos, out inputToken);

                if (formatToken[0] != '<')
                {
                    // It's a literal word; make sure the input matches.
                    if (inputToken != formatToken)
                        return false;
                }
                else if (formatCharPos == formatString.Length)
                {
                    // It's a placeholder token at the end of the format string, so it
                    // matches the rest of the input string.
                    args.Add(input.Substring(inputStartPos));
                    inputCharPos = input.Length;
                }
                else
                {
                    // It's a placeholder token in the middle of the format string, so get
                    // the next format token. The next format token is assumed to be a literal
                    // word that follows the argument.
                    formatCharPos = GetToken(formatString, formatCharPos, out formatToken);

                    // Scan ahead for an input token that matches the format token.
                    // Fail if we reach the end of the input without finding one.
                    int inputEndPos = inputCharPos;
                    inputCharPos = GetToken(input, inputCharPos, out inputToken);

                    while (inputToken != formatToken)
                    {
                        if (inputCharPos == input.Length)
                            return false;

                        inputEndPos = inputCharPos;
                        inputCharPos = GetToken(input, inputCharPos, out inputToken);
                    }

                    // Add the input substring that corresponds to the placeholder.
                    inputEndPos = TrimBack(input, inputStartPos, inputEndPos);

                    args.Add(input.Substring(inputStartPos, inputEndPos - inputStartPos));
                }
            }

            // Make sure there's no additional unexpected input.
            return inputCharPos == input.Length;
        }

        public static int SkipToken(string input, int startPos)
        {
            int wordPos = NextWord(input, startPos);
            int wordEnd = NextSpace(input, wordPos);
            return NextWord(input, wordEnd);
        }

        public static int GetToken(string input, int startPos, out string token)
        {
            int wordPos = NextWord(input, startPos);
            int wordEnd = NextSpace(input, wordPos);
            token = input.Substring(wordPos, wordEnd - wordPos);
            return NextWord(input, wordEnd);
        }

        static int NextWord(string input, int startPos)
        {
            while (startPos < input.Length && input[startPos] == ' ')
                ++startPos;

            return startPos;
        }

        static int NextSpace(string input, int startPos)
        {
            while (startPos < input.Length && input[startPos] != ' ')
                ++startPos;

            return startPos;
        }

        static int TrimBack(string input, int startPos, int endPos)
        {
            while (endPos > startPos && input[endPos - 1] == ' ')
                --endPos;

            return endPos;
        }
    }
}
