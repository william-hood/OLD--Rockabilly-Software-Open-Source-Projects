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
	internal class CoarseGrindInterface : WebInterface_ImgsInStyleSection
	{
		internal CoarseGrindInterface(string htmlTitle, WebInterfaceControl banner, int reloadSeconds = 5)
	{
		Title = htmlTitle;
		//this.externalStyleSheetName = COARSEGRIND_EXTERNAL_CSS_NAME;
		RefreshIntervalSeconds = reloadSeconds;
		ControlsInOrder.Add(new RawCodeSegment("<center>"));
		ControlsInOrder.Add(banner);
		ControlsInOrder.Add(new Divider());
	}
}

	public abstract class TestProgram : HttpServer
	{
	internal static readonly InStyleImage ICON_COARSEGRINDLOGO = new Icon_CoarseGrindLogo();
	private static readonly InStyleImage ICON_CONFIRMDELETE = new Icon_ConfirmDelete();
	private static readonly InStyleImage ICON_DECLINEDELETE = new Icon_DeclineDelete();
	private static readonly InStyleImage ICON_DELETE = new Icon_Delete();
	private static readonly InStyleImage ICON_DOWNLOAD = new Icon_Download();
	private static readonly InStyleImage ICON_EXIT = new Icon_Exit();
	private static readonly InStyleImage ICON_FAILRESULT = new Icon_FailResult();
	private static readonly InStyleImage ICON_GOBACK = new Icon_GoBack();
	private static readonly InStyleImage ICON_INCONCLUSIVERESULT = new Icon_InconclusiveResult();
	private static readonly InStyleImage ICON_LARGETEST = new Icon_LargeTest();
	private static readonly InStyleImage ICON_LOADRESULT = new Icon_LoadResult();
	private static readonly InStyleImage ICON_MEDIUMTEST = new Icon_MediumTest();
	private static readonly InStyleImage ICON_NO = new Icon_NO();
	private static readonly InStyleImage ICON_PASSRESULT = new Icon_PassResult();
	private static readonly InStyleImage ICON_SMALLTEST = new Icon_SmallTest();
	private static readonly InStyleImage ICON_STOPALL = new Icon_StopAll();
	private static readonly InStyleImage ICON_STOPONE = new Icon_StopOne();
	private static readonly InStyleImage ICON_SUBJECTIVERESULT = new Icon_SubjectiveResult();
	private static readonly InStyleImage ICON_YES = new Icon_YES();

		// Override this to set a custom banner
protected WebInterfaceControl Banner
		{
			get
			{
				return new CoarseGrindBanner(this);
}
		}

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
	const int SMALL_SUITE_THRESHOLD = 26;
const int LARGE_SUITE_THRESHOLD = 99;
const int ICON_TEXT_SIZE = 175;
private CoarseGrindResultList resultList = new CoarseGrindResultList(CoarseGrind.DEFAULT_PARENT_FOLDER);

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
			get {
				if (tests == null) return false;
				return tests.CurrentlyRunningSuite != null;
}
		}

public int Progress
		{ get {
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

		if (! ContinueService)
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
			TestSuite tmp = tests.AllTestSuites[tests.immediateRun];
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

	public override string Handle(HttpListenerRequest incomingRequest) //HTTP Server
{
			StringBuilder result = new StringBuilder();
	//NOTE: Requests with null URLs have somehow gotten through.
	//      How to handle?
			/*
			string remoteUrlTarget =
				incomingRequest.getURL().getProtocol() + "://"
				+ incomingRequest.getURL().getHost() + ":"
				+ incomingRequest.getURL().getPort();
			*/

	TestSuite tmp = null;

			CoarseGrindInterface ui = new CoarseGrindInterface(HtmlTitle, Banner);
	//ui.RedirectionUrl = remoteUrlTarget;


	string[] urlParts = incomingRequest.RawUrl.Split('/');

	try
	{
		switch (urlParts[0])
		{
			//case COARSEGRIND_EXTERNAL_CSS_NAME:
			//result.httpContent.subtype = "css";
			//result.httpContent = null;
			//result.headers.Add(CSS_LAST_MODIFIED_HEADER);
			//result.headers.Add(CSS_EXPIRES_HEADER);
			//result.headers.Add(CSS_CACHE_CONTROL);
			//((HttpstringPayload)result.payload).getContent().Append(ui.getCssStyle());
			//return result;
			case RUN_PATH_PART:
				tmp = tests.AllTestSuites[urlParts[1]];
				if (tmp == null)
				{
					ui.ControlsInOrder.Add(new Label("No test suite named \"" + urlParts[1] + "\" exists", 300));
					result.Append(ui.ToString());
					ui = null;
							return result.ToString();
				}

				//We may not support command line arguments over the web for security reasons.
				//processConfigSet(incomingRequest.body.ToString().replace("\r", "").split("\n"));

				ui.ControlsInOrder.Add(new Label("Running: " + urlParts[1], 500));
				ui.RefreshIntervalSeconds = 1;
				result.Append(ui.ToString());

				if (tmp != null)
				{
					tests.KickOffTestSuite(tmp);
				}
				ui = null;
					return result.ToString();
			case APPLY_CFG_PATH_PART:
				// For RUN and APPLY, the config parameters go in the body
						tests.ProcessConfigSet(this, new StreamReader(incomingRequest.InputStream).ReadToEnd().Replace("\r", "").Split('\n'));
				return "ACK";
			case STOP_ALL_TESTING_PATH_PART:
				try
				{
							tests.CurrentlyRunningSuite.HaltAllTesting();
				}
				catch (Exception assumedNullRef)
				{
					ui.ControlsInOrder.Add(new Label("No test suite is currently running", 300));
					result.Append(ui.ToString());
					ui = null;
							return result.ToString();
				}


				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_STOPALL));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new Label("Halting Test Suite: " + tests.CurrentlyRunningSuite.Name, 300));
				ui.ControlsInOrder.Add(new Label("No further tests will be run.", 200));
				ui.RefreshIntervalSeconds = 4;
				result.Append(ui.ToString());
				ui = null;
					return result.ToString();
			case STOP_TEST_CASE_PATH_PART:
				try
				{
					tests.CurrentlyRunningSuite.InterruptCurrentTest();
				}
				catch (Exception)
				{
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_NO));
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new Label("No test suite is currently running", 300));
					ui.RefreshIntervalSeconds = 2;
					result.Append(ui.ToString());
					ui = null;
					return result.ToString();
				}

				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_STOPONE));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new Label("Interrupting Test Case: " + tests.CurrentlyRunningSuite.CurrentTest, 300));
				ui.ControlsInOrder.Add(new Label("All remaining tests will still run.", 200));
				ui.RefreshIntervalSeconds = 3;
				result.Append(ui.ToString());
				ui = null;
					return result.ToString();
			case KILL_SERVICE_PATH_PART:
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_EXIT));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new Label("Test Server Taken Offline", 300));
				result.Append(ui.ToString());
				ui = null;
				testingContinues = false;

				try
				{
					tests.block.Signal();
				}
				catch (Exception dontCare)
				{
					// Deliberate NO-OP
				}

				DiscontinueService();
					return result.ToString();
			case RETRIEVE_DATA_PATH_PART:
						ui.ControlsInOrder.Add(new LineBreak());
						ui.ControlsInOrder.Add(new LineBreak());
						ui.ControlsInOrder.Add(new LineBreak());
						ui.ControlsInOrder.Add(new LineBreak());
						ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_NO));
						ui.ControlsInOrder.Add(new LineBreak());
						ui.ControlsInOrder.Add(new Label("NOT YET IMPLEMENTED", 300));
						result.Append(ui.ToString());
						ui = null;
						/*
						
				try
				{
							ZipFile.CreateFromDirectory(CoarseGrind.DEFAULT_PARENT_FOLDER + Path.DirectorySeparatorChar + URLDecoder.decode(urlParts[1].replace(".zip", ""), "UTF-8"), ZIP_TEMP_FILE, CompressionLevel.Optimal, true);
				}
				catch
				{
					ui.ControlsInOrder.Add(new CaptionedControl(ui.useInStyleImage(ICON_NO), new Label("Unable to create ZIP file for transfer", 300), CaptionedControlOrientation.LeftOfCaption));
					result.Append(ui.ToString());
					ui = null;
							return result.ToString();
				}
				result.headers.clear();
				result.httpContent = new HttpContent(HttpContent.application, HttpContent.zip);

				File file = new File(ZIP_TEMP_FILE);
				byte[] bytes = new byte[(int)file.length()];

				try
				{
					DataInputStream dataInputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(ZIP_TEMP_FILE)));
					dataInputStream.readFully(bytes);
					dataInputStream.close();
				}
				catch (IOException thisException)
				{
					try
					{
						result.setStatusCode(500);
					}
					catch (IllegalStatusCodeException e)
					{
						// DELIBERATE NO-OP -- SHOULDN'T BE POSSIBLE
					}

					ui.ControlsInOrder.Add(new CaptionedControl(ui.useInStyleImage(ICON_NO), new Label("Unable to encode ZIP file into HTTP message", 300), Orientation.LeftOfCaption));
					((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
					ui = null;
					return result;
				}
				finally
				{
							Directory.Delete(ZIP_TEMP_FOLDER, true);
				}

				result.payload = null;
				result.payload = new HttpBinaryPayload();
				((HttpBinaryPayload)result.payload).setContent(bytes);
				//result.binaryBody = bytes;
					*/
						return result.ToString();
			case DELETE_DATA_PATH_PART:
						if (IsBusy)
				{
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_DECLINEDELETE));
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new Label("DELETE COUNTERMANDED", 400));
					ui.ControlsInOrder.Add(new Label("The server has become busy. You must wait until testing is finished before trying to delete again.", 300));
					result.Append(ui.ToString());
					ui = null;
							return result.ToString();
				}

						Directory.Delete(CoarseGrind.DEFAULT_PARENT_FOLDER, true);
						Directory.CreateDirectory(CoarseGrind.DEFAULT_PARENT_FOLDER);
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_CONFIRMDELETE));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new Label("ALL TEST DATA DELETED", 300));
				ui.RefreshIntervalSeconds = 3;
				result.Append(ui.ToString());
				ui = null;
					return result.ToString();
			case LIST_DATA_PATH_PART:
				resultList.refresh();
				ui.ClearRefreshInterval();
				ui.ClearRedirectionUrl();
				ui.RedirectionUrl = /*remoteUrlTarget + '/' + */LIST_DATA_PATH_PART;
				ui.RefreshIntervalSeconds = 30;
				ui.ControlsInOrder.Add(new Label("TEST RESULTS", 400));
				ui.ControlsInOrder.Add(new LineBreak());

						if (resultList.resultFolders.Count < 1)
				{
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(ui.useInStyleImage(ICON_NO));
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new Label("(none)", 350));
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
				}
				else {
					ControlCluster cluster = new ControlCluster();
					cluster.columns = 2;
					foreach (DirectoryInfo thisTestResult in resultList.resultFolders)
					{
						WebInterfaceControl icon = null;
						// Global.DEFAULT_PARENT_FOLDER + File.separator + 
						try
						{
									/*
							string demeanor = new string(Files.readAllBytes(Paths.get(Global.DEFAULT_PARENT_FOLDER + File.separator + thisTestResult + File.separator + Global.SUMMARY_TEXTFILE_NAME)));
							switch (demeanor.toUpperCase().charAt(0))
							{
								case 'P':
									icon = ui.useInStyleImage(ICON_PASSRESULT);
									break;
								case 'F':
									icon = ui.useInStyleImage(ICON_FAILRESULT);
									break;
								case 'S':
									icon = ui.useInStyleImage(ICON_SUBJECTIVERESULT);
									break;
								default:
									icon = ui.useInStyleImage(ICON_INCONCLUSIVERESULT);
							}
							*/
									icon = ui.useInStyleImage(ICON_INCONCLUSIVERESULT);
						}
						catch (IOException e)
						{
							icon = ui.useInStyleImage(ICON_LOADRESULT);
						}

									cluster.Add(new Link(/*remoteUrlTarget + '/' + */RETRIEVE_DATA_PATH_PART + '/' + /*URLEncoder.encode(thisTestResult, "UTF-8") + */ "NotImplemented.zip", new CaptionedControl(icon, thisTestResult.Name, CaptionedControlOrientation.AboveCaption, ICON_TEXT_SIZE)));
					}
					ui.ControlsInOrder.Add(cluster);
					cluster = null;
				}


				ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
				ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" ,ui.useInStyleImage(ICON_GOBACK)), "Cancel results download and go back to the list of tests to run.", CaptionedControlOrientation.LeftOfCaption, 250));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new Divider());
				ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + CONFIRM_DELETE_DATA_PATH_PART, ui.useInStyleImage(ICON_DELETE)), "Clear the server of all test data.", CaptionedControlOrientation.LeftOfCaption, 250));
				result.Append(ui.ToString());
				ui = null;
					return result.ToString();
			case CONFIRM_DELETE_DATA_PATH_PART:
				ui.ClearRefreshInterval();
				ui.ClearRedirectionUrl();
				ui.ControlsInOrder.Add(new Label("REALLY DELETE TEST DATA?<br><br>", 400));

				ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
				ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + LIST_DATA_PATH_PART, ui.useInStyleImage(ICON_DECLINEDELETE)), "<b><i>NO!</i></b> What was I thinking? Do <b><i>NOT</i></b> delete any of the test data.", CaptionedControlOrientation.LeftOfCaption, 250));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + DELETE_DATA_PATH_PART, ui.useInStyleImage(ICON_CONFIRMDELETE)), "<b><i>YES</i></b>, I really do want to delete all of the test data on the server. <b><i>I understand that it can't be undone and I will lose any data I did not download and save elsewhere.</i></b>", CaptionedControlOrientation.LeftOfCaption, 250));
				result.Append(ui.ToString());
				ui = null;
					return result.ToString();
			case CONFIRM_KILL_SERVICE_PATH_PART:
				ui.ClearRefreshInterval();
				ui.ClearRedirectionUrl();
				ui.ControlsInOrder.Add(new Label("REALLY TERMINATE THE SERVICE?<br><br>", 400));

				ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
				ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/", ui.useInStyleImage(ICON_NO)), "<b><i>NO</i></b>, --back to testing please.", CaptionedControlOrientation.LeftOfCaption, 300));
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + KILL_SERVICE_PATH_PART, ui.useInStyleImage(ICON_YES)), "<b><i>YES</i></b>, --terminate the test server program. <b><i>The web interface will not be available until the server program is restarted manually.</i></b>", CaptionedControlOrientation.LeftOfCaption, 250));
				result.Append(ui.ToString());
				ui = null;
					return result.ToString();
		}
	}
	catch //(ArrayIndexOutOfBoundsException expected)
	{
		// DELIBERATE NO-OP. They want the root page.
	}


			ui.ControlsInOrder.Add(new Label(this.GetType().Name, 500));

	if (IsReady)
	{
		if (IsBusy)
		{
			//Show progress and options to stop
			ui.RefreshIntervalSeconds = 3;
			ui.ControlsInOrder.Add(new CaptionedControl(new ProgressBar(Progress, 100), new Label("Progress on test suite <b><i>\"" + tests.CurrentlyRunningSuite.Name + "\"</i></b>", 150), CaptionedControlOrientation.AboveCaption));
			ui.ControlsInOrder.Add(new Divider());
			ui.ControlsInOrder.Add(new Label("OPTIONS", 300));
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + STOP_TEST_CASE_PATH_PART, ui.useInStyleImage(ICON_STOPONE)), "Stop individual test <b><i>\"" + tests.CurrentlyRunningSuite.CurrentTest + "\"</i></b> and allow the remaining tests to run.", CaptionedControlOrientation.LeftOfCaption, 250));
			ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + STOP_ALL_TESTING_PATH_PART, ui.useInStyleImage(ICON_STOPALL)), "Halt test suite <b><i>\"" + tests.CurrentlyRunningSuite.Name + "\"</i></b> completely. No more tests will run.", CaptionedControlOrientation.LeftOfCaption, 250));
		}
		else {
			// List available tests
			ui.ControlsInOrder.Add(new Label("<br>AVAILABLE TEST SUITES", 200));


			ControlCluster cluster = new ControlCluster();
			cluster.columns = 4;
					foreach (string thisTestSuite in tests.AllTestSuites.Keys)
			{
				WebInterfaceControl icon = null;
				int size = tests.AllTestSuites[thisTestSuite].Count;
				if (size < SMALL_SUITE_THRESHOLD)
				{
					icon = ui.useInStyleImage(ICON_SMALLTEST);
				}
				else if (size > LARGE_SUITE_THRESHOLD)
				{
					icon = ui.useInStyleImage(ICON_LARGETEST);
				}
				else {
					icon = ui.useInStyleImage(ICON_MEDIUMTEST);
				}
				cluster.Add(new Link(/*remoteUrlTarget + */"/" + RUN_PATH_PART + '/' + thisTestSuite, new CaptionedControl(icon, thisTestSuite, CaptionedControlOrientation.AboveCaption, ICON_TEXT_SIZE)));
				icon = null;
			}
			ui.ControlsInOrder.Add(cluster);
			cluster = null;

			ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + LIST_DATA_PATH_PART, ui.useInStyleImage(ICON_DOWNLOAD)), "Download individual test results or clear the server of result data.", CaptionedControlOrientation.LeftOfCaption, 250));
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new Divider());
			ui.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + CONFIRM_KILL_SERVICE_PATH_PART, ui.useInStyleImage(ICON_EXIT)), "Stop the test program completely. It will need to be restarted manually.", CaptionedControlOrientation.LeftOfCaption, 250));
		}
	}
	else {
		ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
		ui.ControlsInOrder.Add(new Label("Tests are setting up...", 300));
	}

		result.Append(ui.ToString());

	ui = null;
			return result.ToString();
}

private string HtmlTitle
	{
		get
		{
			StringBuilder result = new StringBuilder(this.GetType().Name);
			result.Append(" - ");
			if (IsBusy)
			{
				result.Append(tests.CurrentlyRunningSuite.Name + " " + Progress + "%");
			}
			else {
				result.Append("Ready");
			}
			return result.ToString();
}
		}
}
}
