
using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;
using MemoirV2;

namespace Rockabilly.CoarseGrind.Examples
{

	public class Descriptions__StringField : Descriptions__Test_Case_BASE
	{
		private string subNameGiven = default(string);
		public Descriptions__StringField(string subName, StringFieldTargets target)
		{
			ExpectedFailures = new List<ExceptionDescription>();
			testDescription = new TestDescription();
			testDescription.stringField.Target = target;
			if (testDescription.stringField.Target == StringFieldTargets.HAPPY_PATH) Priority = TestPriority.HappyPath;
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
			Log.LogInfo("Constructing the object as described above...");
			try
			{
				testDatum = testDescription.DescribedObject;
				Log.LogInfo("Constructed the following object:" + testDatum.ToString());
			}
			catch (Exception thisException)
			{
				Log.LogError("Exception was thrown...");
				Log.LogException(thisException);
			}
			if (secondsToWait > 0) WaitSeconds(secondsToWait);

			AddResult(new TestResult(TestStatus.Pass, "Test executed through to the end."));
		}

		public override string Identifier
		{
			get
			{
				return "StringField" + subNameGiven;
			}
		}

		public override string Name
		{
			get
			{
				return "String Field - " + testDescription.stringField.Target.ToString();
			}
		}

	}
}
