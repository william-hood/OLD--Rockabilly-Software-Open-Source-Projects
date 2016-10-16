package rockabilly.coarsegrind.examples.descriptions;

import java.util.Vector;

import rockabilly.coarsegrind.core.Global;
import rockabilly.coarsegrind.core.TestPriorityType;
import rockabilly.coarsegrind.core.TestResult;
import rockabilly.coarsegrind.core.TestStatus;
import rockabilly.coarsegrind.descriptions.FailureDescription;
import rockabilly.coarsegrind.descriptions.InappropriateDescriptionException;
import rockabilly.coarsegrind.descriptions.NoValueException;
import rockabilly.coarsegrind.descriptions.StringFieldTargets;
import rockabilly.common.Symbols;
import rockabilly.common.Values;

public class Descriptions__StringField extends Descriptions__Test_Case_BASE
{
	private String subNameGiven = Values.DefaultString;
	public Descriptions__StringField(String subName, StringFieldTargets target)
	{
		ExpectedFailures = new Vector<FailureDescription>();
		testCaseDescription = new TestCaseDescription();
		testCaseDescription.stringField.target = target;
		if (testCaseDescription.stringField.target == StringFieldTargets.HAPPY_PATH) priorityType = TestPriorityType.HappyPath;
		subNameGiven = subName;
	}

	@Override
	public String getTestCaseDetailedDescription()
	{
		return "Constructs an object meeting the following description:" + Symbols.NewLine + testCaseDescription.toString();
	}

	@Override
	public void performTest()
	{
		SampleObject testDatum = null;
		Global.Log.message("Constructing the object as described above...");
		try
		{
			testDatum = testCaseDescription.getDescribedObject();
			Global.Log.message("Constructed the following object:" + testDatum.toString());
		} catch (NoValueException | InappropriateDescriptionException thisException)
		{
			Global.Log.message(Global.Log.getWarningIcon(), "Exception was thrown...");
			Global.Log.showFailure(thisException);
		}
		if (secondsToWait > 0) waitSeconds(secondsToWait);

		addResult(new TestResult(TestStatus.Pass, "Test executed through to the end."));
	}

	@Override
	public String getIdentifier()
	{
		return "StringField" + subNameGiven;
	}

	@Override
	public String getName()
	{
		return "String Field - " + testCaseDescription.stringField.target.toString();
	}

}
