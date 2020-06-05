using System.Collections.Generic;
using Rockabilly.CoarseGrind;

namespace CoarseGrindExampleProgram
{
    public class DataDrivenExample : TestFactory
    {
        string data =
            @"Happy Path, DD1, Douglas, Johnson, 4341 Lakeshore Drive, Canandaigua, NY, 14414
All Lower Case, DD2, happy, gilmore, 12345 combo ave, luggage, ca, 12345
All Upper Case, DD3, LOUD, MOUTHER, 999 PRIMAL SCREAM COURT, HUGENESS, TX, 99999
Foreign Characters, DD4, 國人, 外, 123 Alley 75 Lane 3, Jiufen, TW, 700";

        public override List<ManufacturedTest> Products
        {
            get
            {
                List<ManufacturedTest> result = new List<ManufacturedTest>();

                foreach (string dataline in data.Split('\n'))
                {
                    string[] fields = dataline.Trim().Split(',');
                    result.Add(new MyTest(fields[0].Trim(), "Details, details, deatils...",
                        fields[1].Trim(), fields[2].Trim(), fields[3].Trim(), fields[4].Trim(), fields[5].Trim(), fields[6].Trim(), fields[7].Trim()));
                }

                return result;
            }
        }
    }

    public class StreetAddress
    {
        public string FirstName;
        public string LastName;
        public string Address;
        public string City;
        public string State;
        public string ZipCode;
    }

    public class MyTest : ManufacturedTest
    {
        string[] testData;

        public MyTest(string Name, string DetailedDescription, string ID, params string[] dataline) : base(Name, DetailedDescription, ID, "Data Driven", "Manufactured")
        {
            testData = dataline;
        }

        public override void PerformTest()
        {
            Log.Info("Here's the raw data");
            Log.ShowObject(testData);
            Log.Info("Parsing it into a data structure");

            StreetAddress address = new StreetAddress();
            address.FirstName = testData[0];
            address.LastName = testData[1];
            address.Address = testData[2];
            address.City = testData[3];
            address.State = testData[4];
            address.ZipCode = testData[5];

            Log.Info("Here's the Object");
            Log.ShowObject(address);

            Assert.ShouldNotBeEqual(address.LastName, "", "LastName is not empty");
            Assert.ShouldBeEqual(address.ZipCode.Length, 5, "ZipCode has 5 digits");
        }
    }

}
