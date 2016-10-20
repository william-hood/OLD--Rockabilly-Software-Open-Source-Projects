using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.CoarseGrind.Examples.Simple;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Examples
{
	public class ExampleProgram : TestProgram
	{


		public static void Main(string[] args)
		{
			new ExampleProgram().RunTestProgram(args);
		}


		protected override List<Test> AllTests
		{
			get
			{
				List<Test> allTests = new List<Test>();

				allTests.Add(new SimpleExample1());
				allTests.Add(new SimpleExample2());
				allTests.Add(new SimpleExample3());
				allTests.Add(new SimpleExample4());
				allTests.Add(new MultiLogExample());

				// Generate tests for the Integer field
				SubnameFactory intSubnames = new SubnameFactory(0, 3);
				foreach (ValueFieldTargets thisValue in Enum.GetValues(typeof(ValueFieldTargets)))
				{
					if (thisValue != ValueFieldTargets.NULL)
					{
						allTests.Add(new Descriptions__IntField(intSubnames.NextSubname, thisValue));
					}
				}

				// Generate tests for the String field 
				SubnameFactory stringSubnames = new SubnameFactory(0, 5);
				foreach (StringFieldTargets thisValue in Enum.GetValues(typeof(StringFieldTargets)))
				{
					allTests.Add(new Descriptions__StringField(stringSubnames.NextSubname, thisValue));
				}

				return allTests;
			}
		}

		protected override void ProcessArguments(List<string> args)
		{
			// DELIBERATE NO-OP
			//
			// A real test program might have to interpret custom arguments
			// such as a particular URL it had to know about, whether it
			// is operating in debug mode, etc.
			//
			// The args it would get are the command line arguments, in
			// order, minus any specific to coarse grind itself.
		}
	}
}
