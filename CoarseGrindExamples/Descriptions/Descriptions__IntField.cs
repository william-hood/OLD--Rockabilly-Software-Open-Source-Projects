
using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;
using MemoirV2;

namespace Rockabilly.CoarseGrind.Examples
{
	public class Descriptions__IntField : Descriptions__Test_Case_BASE
	{
		private string subNameGiven = default(string);
		public Descriptions__IntField(string subName, ValueFieldTargets target)
		{
			ExpectedFailures = new List<ExceptionDescription>();
			testDescription = new TestDescription();
			testDescription.intField.Target = target;
			if (testDescription.intField.Target == ValueFieldTargets.HAPPY_PATH) Priority = TestPriority.HappyPath;
			subNameGiven = subName;
		}

		public override string DetailedDescription
		{
			get
			{
				return "Constructs an object meeting the following description:" + Symbols.CarriageReturnLineFeed + testDescription.ToString();
			}
		}

		public override void PerformTest()
		{
			SampleObject testDatum = null;
			Log.Info("Constructing the object as described above...");
			try
			{
				testDatum = testDescription.DescribedObject;
				Log.Info("Constructed the following object:" + testDatum.ToString());
			}
			catch (Exception thisException)
			{
				Log.Error("Exception was thrown...");
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
				return "Limited Value Field - " + testDescription.intField.Target.ToString();
			}
		}

	}
}