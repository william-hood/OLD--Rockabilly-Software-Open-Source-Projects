using System;
using System.IO;
using MemoirV2;

namespace MemoirExample
{
    class TestStruct
    {
        public string name = "Hi";
        public int value = 7;
        public double otherValue = 42.9;
        public TestStruct child = null;
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Memoir memoir = new Memoir("Memoir Test", Console.Out, File.CreateText("memoir_test.html"));

            try
            {
                memoir.Info("This is a test");

                memoir.Debug("Debug message here!");

                TestStruct check = new TestStruct();
                check.child = new TestStruct();

                memoir.ShowObject(check, "check");

                memoir.Error("Uh oh!");

                memoir.Info("Here's one with a custom emoji!", "🎶");
                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");

                StyleTest(memoir, "decaf_green");

                memoir.Info("This is a test");
                memoir.Info("This is a test");

                StyleTest(memoir, "decaf_orange");

                memoir.Info("This is a test");

                StyleTest(memoir, "decaf_green_light_roast");

                memoir.Info("This is a test");
                memoir.Info("This is a test");

                StyleTest(memoir, "decaf_orange_light_roast");

                memoir.Info("This is a test");
                memoir.Info("This is a test");

                StyleTest(memoir, "desert_horizon");

                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");

                Memoir subLog = new Memoir("Sub Log", Console.Out);
                subLog.Info("First line of the sub log");
                subLog.Info("Second line of the sub log");


                Memoir justSomeTest = new Memoir("Test Something", Console.Out);
                justSomeTest.Info("This happened");
                justSomeTest.Info("Then this");
                justSomeTest.Info("Also this");
                justSomeTest.Info("This check passed", Constants.EMOJI_PASSING_TEST);
                justSomeTest.Info("So did this", Constants.EMOJI_PASSING_TEST);
                justSomeTest.Info("But not this", Constants.EMOJI_FAILING_TEST);

                subLog.LogMemoir(justSomeTest, Constants.EMOJI_FAILING_TEST, "failing_test_result");


                subLog.Debug("Third line of the sub log");
                subLog.Info("Fourth line of the sub log");
                memoir.LogMemoir(subLog);

                Memoir passingTest = new Memoir("Passing Test", Console.Out);
                passingTest.Info("This happened");
                passingTest.Info("Then this");
                passingTest.Info("Also this");
                passingTest.Info("This check passed", Constants.EMOJI_PASSING_TEST);
                passingTest.Info("So did this", Constants.EMOJI_PASSING_TEST);
                memoir.LogMemoir(passingTest, Constants.EMOJI_PASSING_TEST, "passing_test_result");

                Memoir inconclusiveTest = new Memoir("Inconclusive Test", Console.Out);
                inconclusiveTest.Info("This happened");
                inconclusiveTest.Info("Then this");
                inconclusiveTest.Info("Also this");
                inconclusiveTest.Info("This check passed", Constants.EMOJI_PASSING_TEST);
                inconclusiveTest.Info("This check proved we're wasting our time. Dang it!", Constants.EMOJI_INCONCLUSIVE_TEST);
                inconclusiveTest.Info("This check also passed", Constants.EMOJI_PASSING_TEST);
                memoir.LogMemoir(inconclusiveTest, Constants.EMOJI_INCONCLUSIVE_TEST, "inconclusive_test_result");

                memoir.Info("This is a test");
                memoir.Info("This is a test");

                StyleTest(memoir, "old_parchment");

                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");

                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");
                memoir.Info("This is a test");

                StyleTest(memoir, "plate");

                memoir.Info("Second to last line...");

                memoir.Info("Final Line");

                passingTest.ShowObject(passingTest);
            }
            catch (Exception fakeException)
            {
                memoir.ShowException(fakeException);
            }
            finally
            {
                memoir.Conclude();
            }
        }

        public static void StyleTest(Memoir memoir, string style)
        {

            Memoir styleTest = new Memoir("This is " + style, Console.Out);
            styleTest.Info("This happened");
            styleTest.Info("Then this");
            styleTest.Info("Also this");
            styleTest.Info("This check passed", Constants.EMOJI_PASSING_TEST);
            memoir.LogMemoir(styleTest, null, style);
        }
    }
}
