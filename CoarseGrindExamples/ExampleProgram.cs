// Copyright (c) 2016 William Arthur Hood
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.Collections.Generic;
using Rockabilly.CoarseGrind.Descriptions;
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
				foreach (ValueFieldTargets thisValue in Enum.GetValues(typeof(ValueFieldTargets))
				{
					if (thisValue != ValueFieldTargets.NULL)
					{
						allTests.Add(new Descriptions__IntField(intSubnames.getNextSubname(), thisValue));
					}
				}

				// Generate tests for the String field 
				SubnameFactory stringSubnames = new SubnameFactory(0, 5);
				foreach (StringFieldTargets thisValue in Enum.GetValues(typeof(StringFieldTargets))
				{
					allTests.Add(new Descriptions__StringField(stringSubnames.getNextSubname(), thisValue));
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
