package rockabilly.coarsegrind.examples.descriptions;

import rockabilly.coarsegrind.descriptions.IntLimitsDescription;

public class IntFieldLimits extends IntLimitsDescription
{
	@Override
	// In most cases the field may need to retrieve the limit
	// from an external source, rather than hard-code it.
	public Integer getUpperLimit()
	{
		return 250;
	}

	@Override
	// In most cases the field may need to retrieve the limit
	// from an external source, rather than hard-code it.
	public Integer getLowerLimit()
	{
		return -250;
	}
}
