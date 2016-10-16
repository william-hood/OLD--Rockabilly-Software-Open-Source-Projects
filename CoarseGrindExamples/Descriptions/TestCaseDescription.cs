package rockabilly.coarsegrind.examples.descriptions;

import rockabilly.coarsegrind.descriptions.FloatFieldDescription;
import rockabilly.coarsegrind.descriptions.InappropriateDescriptionException;
import rockabilly.coarsegrind.descriptions.IntFieldDescription;
import rockabilly.coarsegrind.descriptions.NoValueException;
import rockabilly.coarsegrind.descriptions.ObjectDescription;
import rockabilly.coarsegrind.descriptions.SimpleFieldDescription;
import rockabilly.coarsegrind.descriptions.StringFieldDescription;
import rockabilly.common.Foundation;

public class TestCaseDescription extends ObjectDescription<SampleObject>
{
	SimpleFieldDescription<SampleEnum> enumField = new SimpleFieldDescription<SampleEnum>(SampleEnum.THREE);
	SimpleFieldDescription<Character> charField = new SimpleFieldDescription<Character>('=');
	SimpleFieldDescription<Boolean> boolField = new SimpleFieldDescription<Boolean>((boolean)true);
	StringFieldDescription stringField = new StringFieldDescription("BASIS");
	FloatFieldDescription floatField = new FloatFieldDescription((float)10);
	IntFieldDescription intField = new IntFieldDescription(new IntFieldLimits());

	@Override
	public SampleObject getDescribedObject() throws NoValueException,
			InappropriateDescriptionException
	{
		SampleObject result = new SampleObject();
		if (! intField.isDefault()) result.intField = intField.getDescribedValue();
		if (! floatField.isDefault()) result.floatField = floatField.getDescribedValue();
		if (! stringField.isDefault()) result.stringField = stringField.getDescribedValue();
		if (! boolField.isDefault()) result.boolField = boolField.getDescribedValue();
		if (! charField.isDefault()) result.charField = charField.getDescribedValue();
		if (! enumField.isDefault()) result.enumField = enumField.getDescribedValue();
		return result;
	}
	
	@Override
	public String toString()
	{
		StringBuilder result = new StringBuilder();
		result.append("intField == " + Foundation.robustGetString(intField));
		result.append("; floatField == " + Foundation.robustGetString(floatField));
		result.append("; stringField == " + Foundation.robustGetString(stringField));
		result.append("; boolField == " + Foundation.robustGetString(boolField));
		result.append("; charField == " + Foundation.robustGetString(charField));
		result.append("; enumField == " + Foundation.robustGetString(enumField));
		return result.toString();
	}
}