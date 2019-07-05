
using System.Text;
using Rockabilly.Strings;

namespace Rockabilly.CoarseGrind.Examples
{
	public class SampleObject
	{
		public int intField = default(int);
		public float floatField = default(float);
		public string stringField = default(string);
		public bool boolField = false;
		public char charField = 'x';
		public SampleEnum enumField = default(SampleEnum);

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			result.Append("intField == " + StringUtilities.RobustGetString(intField));
			result.Append("; floatField == " + StringUtilities.RobustGetString(floatField));
			result.Append("; stringField == " + StringUtilities.RobustGetString(stringField));
			result.Append("; boolField == " + StringUtilities.RobustGetString(boolField));
			result.Append("; charField == " + StringUtilities.RobustGetString(charField));
			result.Append("; enumField == " + StringUtilities.RobustGetString(enumField));
			return result.ToString();
		}
	}
}
