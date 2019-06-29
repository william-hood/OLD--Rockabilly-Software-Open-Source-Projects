using System;
using System.IO;
using MemoirV2;

namespace MemoirExample
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Memoir memoir = new Memoir("Memoir Test", Console.Out, File.CreateText("memoir_test.html"));

            memoir.LogInfo("This is a test");

            memoir.LogDebug("Debug message here!");

            memoir.LogError("Uh oh!");

            memoir.LogInfo("Here's one with a custom emoji!", "🎶");

            Memoir subLog = new Memoir("Sub Log", Console.Out);
            subLog.LogInfo("First line of the sub log");
            subLog.LogInfo("Second line of the sub log");

            Memoir justSomeTest = new Memoir("Test Something", Console.Out);
            justSomeTest.LogInfo("This happened");
            justSomeTest.LogInfo("Then this");
            justSomeTest.LogInfo("Also this");
            justSomeTest.LogInfo("This check passed", Constants.EMOJI_PASSING_TEST);
            justSomeTest.LogInfo("So did this", Constants.EMOJI_PASSING_TEST);
            justSomeTest.LogInfo("But not this", Constants.EMOJI_FAILING_TEST);

            subLog.LogMemoir(justSomeTest, Constants.EMOJI_FAILING_TEST, "failing_test_result");


            subLog.LogDebug("Third line of the sub log");
            subLog.LogInfo("Fourth line of the sub log");
            memoir.LogMemoir(subLog);

            Memoir passingTest = new Memoir("Passing Test", Console.Out);
            passingTest.LogInfo("This happened");
            passingTest.LogInfo("Then this");
            passingTest.LogInfo("Also this");
            passingTest.LogInfo("This check passed", Constants.EMOJI_PASSING_TEST);
            passingTest.LogInfo("So did this", Constants.EMOJI_PASSING_TEST);
            memoir.LogMemoir(passingTest, Constants.EMOJI_PASSING_TEST, "passing_test_result");

            Memoir inconclusiveTest = new Memoir("Inconclusive Test", Console.Out);
            inconclusiveTest.LogInfo("This happened");
            inconclusiveTest.LogInfo("Then this");
            inconclusiveTest.LogInfo("Also this");
            inconclusiveTest.LogInfo("This check passed", Constants.EMOJI_PASSING_TEST);
            inconclusiveTest.LogInfo("This check proved we're wasting our time. Dang it!", Constants.EMOJI_INCONCLUSIVE_TEST);
            inconclusiveTest.LogInfo("This check also passed", Constants.EMOJI_PASSING_TEST);
            memoir.LogMemoir(inconclusiveTest, Constants.EMOJI_INCONCLUSIVE_TEST, "inconclusive_test_result");

            memoir.LogInfo("Second to last line...");

            memoir.LogInfo("Final Line");

            memoir.Conclude();
        }
    }
}
