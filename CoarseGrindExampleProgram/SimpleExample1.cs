
using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Examples
{
	public class SimpleExample1 : Test
	{
        public SimpleExample1() : base(
                Name: "Sample Test ONE",
                DetailedDescription: "This is the detailed description for SimpleExample1.  Use this field to describe what the test does and what its pass criteria are. Commas, \tTabs, \rCarriage Returns, \nLine Feeds, and \fForm Feed chars will be filtered out.",
                ID: "ST1",

                // Categories
                "Simple", "All", "Example")
        { }


		public override void PerformTest()
		{
			// This would be where you actually perform the test.  What a surprise, huh?
			// If setup fails, it will NOT be run.

			int percentSetup = 100;
            Require.ShouldBeTrue(percentSetup == 100, "Verifying that everything is setup.");

			// Coarse Grind does not interrupt the test if something fails.
			// This is by-design to support multiple points-of-failure.
			// If inconclusive or failing status makes it pointless to proceed, you must explicitly enforce that.
			if (this.OverallStatus != TestStatus.Inconclusive)
			{
				// You should log every little thing the test does.  That lessens the chance of needing to re-run when a bug happens.
				Log.Info("Calling doSomethingOne()");
				//int return1 = doSomethingOne();
				int return1 = 42;

				Log.Info("Calling doSomethingTwo()");
				//int return2 = doSomethingTwo();
				int return2 = 42;

				Log.Info("Calling doSomethingThree()");
				//doSomethingThree();

                Assert.ShouldBeEqual(42, return1, "First function returned 42.");
                Assert.ShouldBeEqual(42, return2, "Second function also returned 42.");
            }

			// This test should pass.
			// Note that the test subject will only be passing if there are NO failing or inconclusive tests in its contents.
		}

		public override bool Cleanup()
		{
			// This will ALWAYS be run, even if setup failed.
            // You will not see a cleanup section in the log unless you log something.
			return true;
		}

		public override bool Setup()
		{
            // Perform any test case specific setup.
            // If this does not return true, performTest() will never be run.
            // You may wish to make an abstract class that extends Test.
            // Lots of testcases could extend that and share the same setup(),
            // cleanup(), and other common methods.


            // You will not see a setup section in the log unless you log something.

            return true;
		}

	}
}