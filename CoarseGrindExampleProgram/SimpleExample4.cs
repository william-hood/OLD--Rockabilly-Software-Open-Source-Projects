
using System;

namespace Rockabilly.CoarseGrind.Examples
{
	public class SimpleExample4 : Test
    {
        public SimpleExample4() : base(
            Name: "Sample Test FOUR",
            DetailedDescription: "This is the detailed description for SimpleExample3.  Use this field to describe what the test does and what its pass criteria are. Commas, \tTabs, \rCarriage Returns, \nLine Feeds, and \fForm Feed chars will be filtered out.",
            ID: "ST4",

            // Categories
            "Simple", "All", "Example")
        { }

        public override void PerformTest()
		{
			// This would be where you actually perform the test.  What a surprise, huh?
			// If setup fails, it will NOT be run.

			// You should log every little thing the test does.  That lessens the chance of needing to re-run when a bug happens.
			Log.Info("Deliberately throwing an exception");

			try
			{

				Log.Info("Calling doSomethingTwo()");
				//int return2 = doSomethingTwo();

				throw new NullReferenceException("This is a FAKE null pointer exception. Nothing's actually wrong.", new ArgumentException("Also a FAKE exception. Carry on."));
			}
			catch (Exception loggedException)
			{
				AddResult(GetResultForFailure(loggedException));

				// If you don't want to actually add a TestResult containing
				// the exception you can simply log it with the line below instead.
				// Log.ShowException(loggedException);
			}
		}

		public override bool Cleanup()
		{
			// This will ALWAYS be run, even if setup failed.
			return true;
		}

		public override bool Setup()
		{
			// Perform any test case specific setup.
			// If this does not return true, performTest() will never be run.
			// You may wish to make an abstract class that extends Test.
			// Lots of testcases could extend that and share the same setup(),
			// cleanup(), and other common methods.
			return true;
		}

	}
}