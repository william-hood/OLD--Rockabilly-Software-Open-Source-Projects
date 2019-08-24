using System;
using Rockabilly.CoarseGrind;

namespace CoarseGrindExampleProgram
{
    public class SampleTest4x : Test
    {
        public SampleTest4x() : base(
                Name: "Sample Test Four-X",
                DetailedDescription: "Just takin' it for a spin...",
                ID: "ST4X",

                // Categories
                "Simple", "All", "Example")
        { }

        public override void PerformTest()
        {
            Log.Info("Whelp, that was fun!");
        }
    }
}
