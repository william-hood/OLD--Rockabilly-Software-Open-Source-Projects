// Copyright (c) 2019, 2016 William Arthur Hood
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
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using Rockabilly.Common;
using Rockabilly.HtmlEffects;

namespace Rockabilly.CoarseGrind
{

	public abstract class TestProgram__DEPRECATED
	{

		// Override this to change from the default reload frequency
		protected virtual int RELOAD_SECONDS
		{
			get
			{
				return 5;
			}
		}

		TestCollection__DEPRECATED tests = null;

		protected abstract List<Test> AllTests { get; }
		protected abstract void ProcessArguments(List<string> args);

		private bool testingContinues = true;

		public bool IsReady
		{
			get
			{
				if (tests == null) return false;
				return tests.IsSetUp;
			}
		}

		public bool IsBusy
		{
			get
			{
				if (tests == null) return false;
				return tests.CurrentlyRunningSuite != null;
			}
		}

		public int Progress
		{
			get
			{
				if (!IsBusy) return 100;
				return tests.CurrentlyRunningSuite.getProgress();
			}
		}

		// Call this from the static main() entrypoint
		protected void RunTestProgram(string[] args)
		{
			while (testingContinues)
			{
				CoarseGrind.KILL_SWITCH = false;
				tests = new TestCollection__DEPRECATED();
				Console.WriteLine("Resetting the Test Collection");

				// Create all programmatically declared test suites
				tests.SetAllTests(AllTests);

				// We can't process config before adding programmatic
				// Test Suites because DECLARE requires all test
				// cases to already be defined.
				tests.ProcessConfigSet(this, args);
				ProcessArguments(tests.UnprocessedArguments);
				tests.IsSetUp = true;

				Console.WriteLine(tests.DescribeAvailableSuites);

				if (tests.immediateRun != default(string))
				{
					TestCollection tmp = null;
					testingContinues = false;

					try
					{
						tmp = tests.AllTestSuites[tests.immediateRun];
					}
					catch { }

					if (tmp == null)
					{
						Console.WriteLine("No test suite named \"" + tests.immediateRun + "\" exists. Nothing to run so exiting the program.");
						System.Environment.Exit(1);
					}
					else {
						tests.CurrentlyRunningSuite = tmp;
						tests.RunTestSuite();
						tests.WaitWhileTesting();
					}
					CoarseGrind.KILL_SWITCH = true;
					tests.destroy();
					tests = null;
					System.Environment.Exit(0);
				}

				tests.WaitWhileTesting();
				tests.destroy();
				tests = null;
				GC.Collect();
			}
		}
	}
}
