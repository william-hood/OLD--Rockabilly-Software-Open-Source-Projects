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
using System.IO;
using System.Text;
using System.Threading;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind
{
	internal class TestCollection
	{

		private const string RUN_ARG = "RUN";
		private const string INCLUDE_ARG = "INCLUDE";
		private const string EXCLUDE_ARG = "EXCLUDE";
		private const string DECLARE_ARG = "DECLARE";
		private const string PORT_ARG = "PORT";
		private const string ADDRESS_ARG = "ADDRESS";


		internal Dictionary<string, TestSuite> AllTestSuites = new Dictionary<string, TestSuite>();
		internal TestSuite CurrentlyRunningSuite = null;
		//private ExecutionThread executionThread = null;
		private Thread executionThread = null;
		private MatchList exclusions = new MatchList();
		private List<Test> AllTests = null;
		internal List<string> UnprocessedArguments = new List<string>();
		internal string immediateRun = default(string);
		internal CountdownEvent block = new CountdownEvent(1);
		internal bool IsSetUp = false;


		public void WaitWhileTesting()
		{
			block.Wait();
		}

		// Create all programmatically declared test suites
		internal void SetAllTests(List<Test> testsFromProgrammer)
		{
			AllTests = testsFromProgrammer;
			foreach (Test thisTest in AllTests)
			{
				AddProgrammaticTestSuites(thisTest.TestSuiteMemberships, thisTest);
				AddProgrammaticTestSuites(thisTest.TestCategoryMemberships, thisTest);
			}
		}

		private void AddProgrammaticTestSuites(string[] memberships, Test thisTest)
		{
			if (memberships == null) return;

			foreach (string thisMembership in memberships)
			{
				string thisMembership_UpperCase = thisMembership.ToUpper();
				TestSuite tmp = null;

				try
				{
					tmp = AllTestSuites[thisMembership_UpperCase];
				}
				catch
				{
				}

				if (tmp == null) tmp = new TestSuite(thisMembership);
				tmp.Add(thisTest);
				AllTestSuites[thisMembership_UpperCase] = tmp;
				thisMembership_UpperCase = null;
				tmp = null;
			}

		}

		private Test FindTest(string identifier)
		{
			foreach (Test thisTest in AllTests)
			{
				if (thisTest.Identifier.MatchesCaseInspecific(identifier)) return thisTest;
			}
			return null;
		}

		private void DoDeclare(string[] args, int startIndex)
		{
			string suiteName = args[startIndex].ToUpper();
			TestSuite tmp = null;
			try
			{
				tmp = AllTestSuites[suiteName];
			}
			catch
			{
			}

			if (tmp == null) tmp = new TestSuite(suiteName);

			for (int index = startIndex + 1; index < args.Length; index++)
			{
				Test foundTest = FindTest(args[index]);
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
				AllTestSuites[suiteName] = tmp;
			}

			suiteName = null;
			tmp = null;
		}

		private void IncludeFile(TestProgram parent, FileInfo includedFile)
		{
			string[] args = null;

			Console.WriteLine("Handling included file " + includedFile.FullName);
			try
			{
				args = File.ReadAllLines(includedFile.FullName);
			}
			catch (IOException)
			{
				Console.WriteLine("Unable to read included file " + includedFile.FullName);
			}

			if (args != null)
			{
				ProcessConfigSet(parent, args);
			}
		}

		private void IncludeDirectory(TestProgram parent, DirectoryInfo includedDirectory)
		{
			Console.WriteLine("Handling included directory " + includedDirectory.FullName);
			foreach (FileInfo thisListing in includedDirectory.GetFiles())
			{
				HandleInclusion(parent, thisListing);
			}
		}

		private void HandleInclusion(TestProgram parent, FileInfo inclusion)
		{
			if (inclusion.Exists)
			{
				if (inclusion.Attributes.HasFlag(FileAttributes.Directory))
				{
					IncludeDirectory(parent, new DirectoryInfo(inclusion.DirectoryName));
				}
				else {
					IncludeFile(parent, inclusion);
				}
			}
			else {
				Console.WriteLine("Ignoring non-existent inclusion " + inclusion.FullName);
			}
		}

		internal void ProcessConfigSet(TestProgram parent, string[] args)
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
						exclusions.Add(args[index]);
						break;
					case INCLUDE_ARG:
						index++;
						HandleInclusion(parent, new FileInfo(args[index]));
						break;
					case DECLARE_ARG:
						DoDeclare(args, index + 1);
						return;
					case PORT_ARG:
						index++;
						if (!parent.ContinueService)
						{
							parent.WEBUI_PORT = int.Parse(args[index]);
						} // will silently ignore if serving.
						return;
					/*
			case ADDRESS_ARG:
				index++;
				if (!parent.ContinueService)
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
				*/
					default:
						UnprocessedArguments.Add(args[index]);
						break;
				}
			}
		}

		internal string DescribeAvailableSuites
		{
			get
			{
				StringBuilder result = new StringBuilder("TEST SUITES");
				result.Append(Symbols.CarriageReturnLineFeed);
				foreach (string thisTestSuite in AllTestSuites.Keys)
				{
					result.Append(Symbols.CloseAngleQuote);
					result.Append(thisTestSuite);
					result.Append(Symbols.CarriageReturnLineFeed);
				}

				return result.ToString();
			}
		}

		internal void KickOffTestSuite(TestSuite thisTestSuite)
		{

			CurrentlyRunningSuite = thisTestSuite;
			executionThread = new Thread(RunTestSuite);
			executionThread.Start();
			//executionThread.Join();
		}

		internal void RunTestSuite()
		{
			try
			{
				Console.WriteLine("Test Suite: " + CurrentlyRunningSuite.Name + Symbols.CarriageReturnLineFeed + Symbols.Divider());
				CurrentlyRunningSuite.RunTestSuite(CoarseGrind.DEFAULT_LOGGING_LEVEL, exclusions, CoarseGrind.DEFAULT_PARENT_FOLDER + Path.DirectorySeparatorChar + DateTime.Now.ToString(CoarseGrind.DateFormatString) + " " + CurrentlyRunningSuite.Name);

				Console.WriteLine();
				Console.WriteLine(Symbols.Divider());

			}
			finally
			{
				try
				{
					block.Signal();
				}
				catch { }
				CurrentlyRunningSuite = null;
			}
		}

		internal void destroy()
		{
			AllTestSuites.Clear();
			AllTests.Clear();
			UnprocessedArguments.Clear();
			AllTestSuites = null;
			AllTests = null;
			UnprocessedArguments = null;
		}
	}
}
