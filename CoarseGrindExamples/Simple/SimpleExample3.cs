
using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Examples
{
	public class SimpleExample3 : Test
	{

		public override string DetailedDescription
		{
			get
			{
				return "This is the detailed description for SimpleExample3.  Use this field to describe what the test does and what its pass criteria are. Commas, \tTabs, \rCarriage Returns, \nLine Feeds, and \fForm Feed chars will be filtered out.";
			}
		}

		public override void PerformTest()
		{
			// This would be where you actually perform the test.  What a surprise, huh?
			// If setup fails, it will NOT be run.

			// The two lines below should render this test (and thus the whole test subject) inconclusive.
			int percentSetup = 76;
			CheckPrerequisite("Verifying that everything is setup.", percentSetup == 100);

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

				CheckPassCriterion("Everything adds up nicely.", return1 + return2 == 84);
			}
		}

		public override string Identifier
		{
			get
			{
				// This can be anything but is typically a number generated by a test case management system.
				// It is generally easier to say that "Test ST7 failed" than "The string-based selection upper limit test failed."
				return "ST3";
			}
		}

		public override string Name
		{
			get
			{
				// This should be a human-readable name that describes the test in-brief
				return "Sample Test THREE";
			}
		}

		public override string[] TestSuiteMemberships
		{
			get
			{
				return new String[] { "Simple", "All", "Example" };
			}
		}

	}
}