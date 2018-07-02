using System;

namespace TextAdventure
{
    struct StringToken
    {
        public string Source;
        public int StartIndex;
        public int Length;
        public int NextIndex;

        public StringToken(string input)
        {
            Source = input;
            StartIndex = 0;
            Length = 0;
            NextIndex = NextWord(input, 0);
            Next();
        }

        public void Next()
        {
            int endIndex = NextSpace(Source, NextIndex);

            StartIndex = NextIndex;
            Length = endIndex - StartIndex;
            NextIndex = NextWord(Source, endIndex);
        }

        public void ExtendToEnd()
        {
            Length = Source.Length - StartIndex;
            NextIndex = Source.Length;
        }

        public bool HaveNext => NextIndex < Source.Length;
        public bool IsEmpty => Length == 0;
        public int EndIndex => StartIndex + Length;

        public char this[int i] => Source[StartIndex + i];

        public override string ToString() => Source.Substring(StartIndex, Length);
        public override int GetHashCode() => ToString().GetHashCode();
        public override bool Equals(object obj) => Equals(obj.ToString());

        public bool Equals(StringToken rhs) => Length == rhs.Length && 0 == string.Compare(Source, StartIndex, rhs.Source, rhs.StartIndex, Length);
        public bool Equals(string rhs) => Length == rhs.Length && 0 == string.Compare(Source, StartIndex, rhs, 0, Length);

        public static bool operator ==(StringToken lhs, StringToken rhs) => lhs.Equals(rhs);
        public static bool operator ==(StringToken lhs, string rhs) => lhs.Equals(rhs);
        public static bool operator !=(StringToken lhs, StringToken rhs) => !lhs.Equals(rhs);
        public static bool operator !=(StringToken lhs, string rhs) => !lhs.Equals(rhs);

        private static int NextWord(string input, int startPos)
        {
            while (startPos < input.Length && input[startPos] == ' ')
                ++startPos;

            return startPos;
        }

        private static int NextSpace(string input, int startPos)
        {
            while (startPos < input.Length && input[startPos] != ' ')
                ++startPos;

            return startPos;
        }
    }
}
