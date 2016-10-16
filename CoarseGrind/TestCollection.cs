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
using System.Text;
using System.Threading;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind
{
	// Inner class ExecutionThread removed for now

	internal class TestCollection{

	private const string RUN_ARG = "RUN";
	private const string INCLUDE_ARG = "INCLUDE";
	private const string EXCLUDE_ARG = "EXCLUDE";
	private const string DECLARE_ARG = "DECLARE";
	private const string PORT_ARG = "PORT";
	private const string ADDRESS_ARG = "ADDRESS";


	Dictionary<string, TestSuite> allTestSuites = new Dictionary<string, TestSuite>();
	TestSuite currentlyRunningSuite = null;
	//private ExecutionThread executionThread = null;
	private MatchList exclusions = new MatchList();
	private List<Test> allTests = null;
	List<string> unprocessedArguments = new List<string>();
	string immediateRun = default(string);
	CountdownEvent block = new CountdownEvent(1);
	bool isSetUp = false;


	public void waitWhileTesting()
	{
			block.Wait();
	}

	// Create all programmatically declared test suites
	void setAllTests(List<Test> testCasesFromProgrammer)
	{
		allTests = testCasesFromProgrammer;
		foreach (Test thisTest in allTests)
		{
			addProgrammaticTestSuites(thisTest.TestSuiteMemberships, thisTest);
			addProgrammaticTestSuites(thisTest.TestCategoryMemberships, thisTest);
		}
	}

	private void addProgrammaticTestSuites(string[] memberships, Test thisTest)
	{
		if (memberships == null) return;

		foreach (string thisMembership in memberships)
		{
			string thisMembership_UpperCase = thisMembership.ToUpper();
			TestSuite tmp = allTestSuites[thisMembership_UpperCase];
			if (tmp == null) tmp = new TestSuite(thisMembership);
			tmp.Add(thisTest);
			allTestSuites[thisMembership_UpperCase] = tmp;
			thisMembership_UpperCase = null;
			tmp = null;
		}

	}

	private Test findTest(string identifier)
	{
		foreach (Test thisTest in allTests)
		{
				if (thisTest.Identifier.MatchesCaseInspecific(identifier)) return thisTest;
		}
		return null;
	}

	private void doDeclare(string[] args, int startIndex)
	{
		string suiteName = args[startIndex].ToUpper();
		TestSuite tmp = allTestSuites[suiteName];
		if (tmp == null) tmp = new TestSuite(suiteName);

			for (int index = startIndex + 1; index < args.Length; index++)
		{
			Test foundTest = findTest(args[index]);
			if (foundTest == null)
			{
				Console.WriteLine("No test case named " + args[index] + " exists. Not adding to " + suiteName);
			}
			else {
				tmp.Add(foundTest);
			}
		}

			if (tmp.Count < 1)
		{
			Console.WriteLine("Declining to add empty test suite " + suiteName);
		}
		else {
			Console.WriteLine("Added declared test suite " + suiteName);
			allTestSuites[suiteName] = tmp;
		}

		suiteName = null;
		tmp = null;
	}

	private void includeFile(TestProgram parent, File includedFile)
	{
		string[] args = null;

		Console.WriteLine("Handling included file " + includedFile.getAbsolutePath());
		try
		{
			args = Files.readAllLines(Paths.get(includedFile.getAbsolutePath())).toArray(new string[0]);
		}
		catch (IOException dontCare)
		{
			Console.WriteLine("Unable to read included file " + includedFile.getAbsolutePath());
		}

		if (args != null)
		{
			processConfigSet(parent, args);
		}
	}

	private void includeDirectory(TestProgram parent, File includedDirectory)
	{
		Console.WriteLine("Handling included directory " + includedDirectory.getAbsolutePath());
		for (File thisListing : includedDirectory.listFiles())
		{
			handleInclusion(parent, thisListing);
		}
	}

	private void handleInclusion(TestProgram parent, File inclusion)
	{
		if (inclusion.exists())
		{
			if (inclusion.isDirectory())
			{
				includeDirectory(parent, inclusion);
			}
			else {
				includeFile(parent, inclusion);
			}
		}
		else {
			Console.WriteLine("Ignoring non-existent inclusion " + inclusion.getAbsolutePath());
		}
	}

	void processConfigSet(TestProgram parent, string[] args)
	{
		for (int index = 0; index < args.Length; index++)
		{
			switch (args[index].ToUpper())
			{
				case RUN_ARG:
					index++;
					immediateRun = args[index].ToUpper();
					break;
				case EXCLUDE_ARG:
					index++;
					exclusions.add(args[index]);
					break;
				case INCLUDE_ARG:
					index++;
					handleInclusion(parent, new File(args[index]));
					break;
				case DECLARE_ARG:
					doDeclare(args, index + 1);
					return;
				case PORT_ARG:
					index++;
					if (!parent.isServing())
					{
							parent.port = int.Parse(args[index]);
					} // will silently ignore if serving.
					return;
				case ADDRESS_ARG:
					index++;
					if (!parent.isServing())
					{
						try
						{
							parent.address = InetAddress.getByName(args[index]);
						}
						catch (UnknownHostException e)
						{
							Console.WriteLine("Unable to set local address to " + args[index]);
						}
					} // will silently ignore if serving.
					return;
				default:
					unprocessedArguments.Add(args[index]);
						break;
			}
		}
	}

	string describeAvailableSuites()
	{
		StringBuilder result = new StringBuilder("TEST SUITES");
			result.Append(Symbols.CarriageReturnLineFeed);
		foreach (string thisTestSuite in allTestSuites.Keys)
		{
			result.Append(Symbols.CloseAngleQuote);
			result.Append(thisTestSuite);
			result.Append(Symbols.CarriageReturnLineFeed);
		}

		return result.ToString();
	}

	void kickOffTestSuite(TestSuite thisTestSuite)
	{
		executionThread = new ExecutionThread(thisTestSuite);
		executionThread.start();
	}

	void runTestSuite(TestSuite thisTestSuite)
	{
		currentlyRunningSuite = thisTestSuite;
			Console.WriteLine("Test Suite: " + thisTestSuite.Name + Symbols.CarriageReturnLineFeed + Symbols.Divider());
			thisTestSuite.RunTestSuite(CoarseGrind.DEFAULT_LOGGING_LEVEL, exclusions, CoarseGrind.DEFAULT_PARENT_FOLDER + File.separator + CoarseGrind.DateFormat.format(new Date()) + " " + thisTestSuite.Name);

		Console.WriteLine();
		Console.WriteLine(Symbols.Divider());

		currentlyRunningSuite = null;
			block.Signal();
	}

	void destroy()
	{
		allTestSuites.Clear();
		allTests.Clear();
		unprocessedArguments.Clear();
		allTestSuites = null;
		allTests = null;
		unprocessedArguments = null;
	}
}
}
