
using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Examples
{
	public class Descriptions__IntField : Descriptions__Test_Case_BASE
	{
		private string subNameGiven = default(string);
		public Descriptions__IntField(string subName, ValueFieldTargets target)
		{
			ExpectedFailures = new List<ExceptionDescription>();
			testCaseDescription = new TestCaseDescription();
			testCaseDescription.intField.Target = target;
			if (testCaseDescription.intField.Target == ValueFieldTargets.HAPPY_PATH) Priority = TestPriority.HappyPath;
			subNameGiven = subName;
		}

		public override string DetailedDescription
		{
			get
			{
				return "Constructs an object meeting the following description:" + Symbols.CarriageReturnLineFeed + testCaseDescription.ToString();
			}
		}

		public override void PerformTest()
		{
			SampleObject testDatum = null;
			Log.Message("Constructing the object as described above...");
			try
			{
				testDatum = testCaseDescription.DescribedObject;
				Log.Message("Constructed the following object:" + testDatum.ToString());
			}
			catch (Exception thisException)
			{
				Log.Message("Exception was thrown...", icon: Log.WarningIcon);
				Log.ShowException(thisException);
			}
			if (secondsToWait > 0) WaitSeconds(secondsToWait);

			AddResult(new TestResult(TestStatus.Pass, "Test executed through to the end."));
		}

		public override string Identifier
		{
			get
			{
				return "IntField" + subNameGiven;
			}
		}

		public override string Name
		{
			get
			{
				return "Limited Value Field - " + testCaseDescription.intField.Target.ToString();
			}
		}

	}
}