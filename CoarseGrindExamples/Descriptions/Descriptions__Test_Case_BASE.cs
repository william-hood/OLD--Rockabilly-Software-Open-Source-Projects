
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;

namespace Rockabilly.CoarseGrind.Examples
{
	public abstract class Descriptions__Test_Case_BASE : Test
	{
		protected List<ExceptionDescription> ExpectedFailures = null;
		protected TestDescription testDescription = null;
		protected int secondsToWait = 0;

		public override bool Setup()
		{
			return testDescription != null;
		}

		public override bool Cleanup()
		{
			return true;
		}

		public override string[] TestSuiteMemberships
		{
			get
			{
				return new string[] { "Descriptions", "All", "Example", "Generated" };
			}
		}
	}
}