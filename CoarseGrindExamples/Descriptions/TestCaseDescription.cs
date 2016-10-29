using System.Text;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Examples
{

	public class TestDescription : ObjectDescription<SampleObject>
	{
		public SimpleFieldDescription<SampleEnum> enumField = new SimpleFieldDescription<SampleEnum>(SampleEnum.THREE);
		public SimpleFieldDescription<char> charField = new SimpleFieldDescription<char>('=');
		public SimpleFieldDescription<bool> boolField = new SimpleFieldDescription<bool>(true);
		public StringFieldDescription stringField = new StringFieldDescription("BASIS");
		public FloatFieldDescription floatField = new FloatFieldDescription((float)10);
		public IntFieldDescription intField = new IntFieldDescription(new IntFieldLimits());

		public override SampleObject DescribedObject
		{
			get
			{
				SampleObject result = new SampleObject();
				if (!intField.IsDefault) result.intField = intField.DescribedValue;
				if (!floatField.IsDefault) result.floatField = floatField.DescribedValue;
				if (!stringField.IsDefault) result.stringField = stringField.DescribedValue;
				if (!boolField.IsDefault) result.boolField = boolField.DescribedValue;
				if (!charField.IsDefault) result.charField = charField.DescribedValue;
				if (!enumField.IsDefault) result.enumField = enumField.DescribedValue;
				return result;
			}
		}

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