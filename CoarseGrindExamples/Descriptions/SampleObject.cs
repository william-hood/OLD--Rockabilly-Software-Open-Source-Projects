package rockabilly.coarsegrind.examples.descriptions;

import rockabilly.common.Foundation;
import rockabilly.common.Values;

public class SampleObject
{
	int intField = Values.DefaultInt;
	float floatField = Values.DefaultFloat;
	String stringField = Values.DefaultString;
	boolean boolField = false;
	char charField = 'x';
	SampleEnum enumField = null;
	
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
