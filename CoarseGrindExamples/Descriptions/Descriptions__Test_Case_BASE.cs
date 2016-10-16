package rockabilly.coarsegrind.examples.descriptions;

import java.util.Vector;

import rockabilly.coarsegrind.core.TestCase;
import rockabilly.coarsegrind.descriptions.FailureDescription;

public abstract class Descriptions__Test_Case_BASE extends TestCase
{	
	protected Vector<FailureDescription> ExpectedFailures = null;
	protected TestCaseDescription testCaseDescription = null;
	protected int secondsToWait = 1;
		
	@Override
	public boolean setup()
	{
		return testCaseDescription != null;
	}

	@Override
	public boolean cleanup()
	{
		return true;
	}

	@Override
	public String[] getTestSuiteMemberships() {
		return new String[] {"Descriptions", "All"};
	}

	@Override
	public String[] getTestCategoryMemberships() {
		return new String[] {"Example", "Generated"};
	}
}
