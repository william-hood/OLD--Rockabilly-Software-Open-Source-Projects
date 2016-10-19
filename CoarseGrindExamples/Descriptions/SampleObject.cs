
using System.Text;
using Rockabilly.Common;

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
			result.Append("intField == " + Foundation.RobustGetString(intField));
			result.Append("; floatField == " + Foundation.RobustGetString(floatField));
			result.Append("; stringField == " + Foundation.RobustGetString(stringField));
			result.Append("; boolField == " + Foundation.RobustGetString(boolField));
			result.Append("; charField == " + Foundation.RobustGetString(charField));
			result.Append("; enumField == " + Foundation.RobustGetString(enumField));
			return result.ToString();
		}
	}
}
