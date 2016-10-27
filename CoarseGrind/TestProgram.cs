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
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{

	public abstract partial class TestProgram : HttpServer
	{

		// Override this to change from the default reload frequency
		protected int RELOAD_SECONDS
		{
			get
			{
				return 5;
			}
		}

		TestCollection tests = null;

		protected abstract List<Test> AllTests { get; }
		protected abstract void ProcessArguments(List<string> args);

		private const string APPLY_CFG_PATH_PART = "apply";
		private const string RUN_PATH_PART = "run";
		private const string STOP_TEST_CASE_PATH_PART = "interrupt";
		private const string STOP_ALL_TESTING_PATH_PART = "halt";
		private const string DELETE_DATA_PATH_PART = "delete_data";
		private const string CONFIRM_DELETE_DATA_PATH_PART = "confirm";
		private const string RETRIEVE_DATA_PATH_PART = "get_data";
		private const string LIST_DATA_PATH_PART = "list_data";
		private const string CONFIRM_KILL_SERVICE_PATH_PART = "confirm_kill_service";
		private const string KILL_SERVICE_PATH_PART = "kill_service";

		private static readonly string ZIP_TEMP_FOLDER = Foundation.UserHomeFolder + Path.DirectorySeparatorChar + "TEMP_DELTHIS";
		private const string ZIP_TEMP_NAME = "TEMP_DELTHIS.zip";
		private static readonly string ZIP_TEMP_FILE = ZIP_TEMP_FOLDER + Path.DirectorySeparatorChar + ZIP_TEMP_NAME;
		const int ICON_TEXT_SIZE = 175;
		private CoarseGrindResultList resultList = new CoarseGrindResultList(CoarseGrind.DEFAULT_PARENT_FOLDER);

		internal int WEBUI_PORT = 8085;

		private bool testingContinues = true;

		private CoarseGrindInterface NewUiInstance
		{
			get
			{

				//CoarseGrindInterface ui = NewUiInstance;
				//ui.useInStyleImage(TestProgram.ICON_COARSEGRINDLOGO);
				return new CoarseGrindInterface("Obsolete", null);
			}
		}

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


				// DEBUG: GET RID OF THIS
				ContinueService = false;
				Console.WriteLine("Declining to run the Web interface. Console use ONLY.");

				/*
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
				*/

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
				base.DiscontinueService();
				testingContinues = false;
				Console.WriteLine("TEST SERVER TAKEN OFFLINE");
			}
		}
	}
}
