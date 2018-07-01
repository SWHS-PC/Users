using System;
using System.Collections.Generic;
using System.Text;

namespace TextAdventure
{
    class CommandParserTest
    {
        struct TestCase
        {
            public string Input;
            public string ExpectedResult;
        }

        public static void Run()
        {
            // Examples of format strings:
            var formatStrings = new string[]
            {
                "h[elp]",
                "t[ake] <object>",
                "unlock <object> with <object>"
            };

            var testCases = new TestCase[]
            {
                new TestCase
                {
                    Input = "h",
                    ExpectedResult = "0,True"
                },
                new TestCase
                {
                    Input = "help foo",
                    ExpectedResult = "0,False"
                },
                new TestCase
                {
                    Input = "foo",
                    ExpectedResult = "-1,False"
                },
                new TestCase
                {
                    Input = "t golden key",
                    ExpectedResult = "1,True,golden key"
                },
                new TestCase
                {
                    Input = "take golden key",
                    ExpectedResult = "1,True,golden key"
                },
                new TestCase
                {
                    Input = "unlock silver box",
                    ExpectedResult = "2,False"
                },
                new TestCase
                {
                    Input = "unlock silver box with small jeweled key",
                    ExpectedResult = "2,True,silver box,small jeweled key"
                }
            };

            var parser = new CommandParser(formatStrings);
            var args = new List<string>();

            foreach (var testCase in testCases)
            {
                int commandIndex;
                bool isValid = parser.ParseCommand(testCase.Input, args, out commandIndex);

                var builder = new StringBuilder();
                builder.Append($"{commandIndex},{isValid}");

                if (isValid)
                {
                    foreach (var arg in args)
                    {
                        builder.Append($",{arg}");
                    }
                }

                string result = builder.ToString();

                if (result != testCase.ExpectedResult)
                {
                    Console.Error.WriteLine("Error: CommandParserTest: {0}", testCase.Input);
                    Console.Error.WriteLine("       Expected Result:   {0}", testCase.ExpectedResult);
                    Console.Error.WriteLine("       Actual Result:     {0}", result);
                }
            }
        }
    }
}
