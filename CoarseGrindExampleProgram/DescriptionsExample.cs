using System;
using System.Collections.Generic;
using System.Web;
using Rockabilly.CoarseGrind;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Strings;

namespace CoarseGrindExampleProgram
{
    public class DescriptionsExample : TestFactory
    {
        public override List<ManufacturedTest> Products
        {
            get
            {
                SubnameFactory namer = new SubnameFactory();

                List<ManufacturedTest> result = new List<ManufacturedTest>();

                // If you really wanted to, you could iterate through every possibility with
                //
                //
                // foreach (StringFieldTargets target in Enum.GetValues(typeof(StringFieldTargets)))
                //
                //
                // Just remember that frivolous test cases won't help you find bugs and may even
                // leave your teammates unhappy with you.

                result.Add(new ExampleManufacturedTest(
                    StringFieldTargets.PADDED_WHITESPACE,
                    "Take a base string value and pad it with whitespace.",
                    "MD" + namer.NextSubname));

                result.Add(new ExampleManufacturedTest(
                    StringFieldTargets.CONTAINS_SINGLE_QUOTE,
                    "Take a base string value and throw in a single quote. It might mess up sloppy database programmers.",
                    "MD" + namer.NextSubname));

                result.Add(new ExampleManufacturedTest(
                    StringFieldTargets.CONTAINS_SQL,
                    "Take a base string value and stick in some SQL. Little Bobby Tables would be proud!!!",
                    "MD" + namer.NextSubname));

                result.Add(new ExampleManufacturedTest(
                    StringFieldTargets.CONTAINS_JAVASCRIPT,
                    "Take a base string value and insert some JavaScript. Don't-cha just love messin' with web programmers?",
                    "MD" + namer.NextSubname));

                result.Add(new ExampleManufacturedTest(
                    StringFieldTargets.CONTAINS_HTML,
                    "Take a base string value and insert some HTML. I once pasted the source code to a form into one of its fields. Boy did that make a mess!",
                    "MD" + namer.NextSubname));

                return result;
            }
        }
    }

    public class ExampleManufacturedTest : ManufacturedTest
    {
        private static string basisValue = "A man, a plan, a canal, Panama";
        private StringFieldDescription field = new StringFieldDescription(basisValue);
        public ExampleManufacturedTest(StringFieldTargets testCase, string DetailedDescription, string ID) : base(testCase.ToString(), DetailedDescription, ID, "Descriptions", "Manufactured")
        {
            field.Target = testCase;
        }

        public override void PerformTest()
        {
            Log.Info(string.Format("The basis value: {0}", basisValue));
            Log.Info(string.Format("The attacked value: <pre>{0}</pre>", HttpUtility.HtmlEncode(field.DescribedValue)));
            Log.Info("In a real test you'd probably try to send it into a rest call, database, or function and see if it breaks.");
            Assert("I didn't break it...", true);
        }
    }
}
