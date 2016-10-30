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
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{

	public abstract partial class TestProgram : HttpServer
	{

		// Override this to change from the default reload frequency
		protected virtual int RELOAD_SECONDS
		{
			get
			{
				return 5;
			}
		}

		TestCollection tests = null;

		protected abstract List<Test> AllTests { get; }
		protected abstract void ProcessArguments(List<string> args);

		private const string RUN_SUITE_PATH = "run";
		private const string STOP_TEST_CASE_PATH_PART = "interrupt";
		private const string STOP_ALL_TESTING_PATH_PART = "halt";
		private const string REQUEST_KILL_SERVICE_PATH = "confirm_kill_service";
		private const string CARRY_OUT_KILL_SERVICE_PATH = "kill_service";
		private const string BANNER_FRAME_PATH = "banner";
		private const string STATUS_FRAME_PATH = "status";
		private const string CONTROL_FRAME_PATH = "controls";
		private const string TEST_SUITES_PATH = "suites";
		private const string TEST_RESULTS_PATH = "results";
		private const string SELECT_CUSTOM_SUITE_PATH = "select_custom";
		private const string RUN_CUSTOM_SUITE_PATH = "run_custom";


		private const string BANNER_FRAME_NAME = "BANNER_FRAME";
		private const string STATUS_FRAME_NAME = "STATUS_FRAME";
		private const string CONTROL_FRAME_NAME = "CONTROLS_FRAME";
		private const string VIEW_FRAME_NAME = "VIEW_FRAME";

		internal const int SMALL_SUITE_THRESHOLD = 26;
		internal const int LARGE_SUITE_THRESHOLD = 99;
		internal const int ICON_TEXT_SIZE = 175;

		internal int WEBUI_PORT = 8085;

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
				tests = new TestCollection();
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


				if (!ContinueService)
				{
					// Attempt to start the test server
					try
					{
						SetupService(WEBUI_PORT);
						Console.WriteLine(DescribeService());
					}
					catch (Exception loggedException)
					{
						Console.WriteLine(Foundation.DepictException(loggedException));
						Console.WriteLine("Test Server is offline");
					}
				}

				if (tests.immediateRun != default(string))
				{
					TestSuite tmp = null;

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
						testingContinues = false;
						tests.CurrentlyRunningSuite = tmp;
						tests.RunTestSuite();
					}
				}

				tests.WaitWhileTesting();
				tests.destroy();
				tests = null;
				GC.Collect();
			}

			if (ContinueService) DiscontinueService();
		}

		public override void DiscontinueService() //HTTP Server
		{
			if (ContinueService)
			{
				testingContinues = false;
				new Thread(effectDiscontinue).Start();
			}
		}

		private void effectDiscontinue()
		{
			Thread.Sleep(2000);
			base.DiscontinueService();
			Console.WriteLine("TEST SERVER TAKEN OFFLINE");
		}
	}
}
