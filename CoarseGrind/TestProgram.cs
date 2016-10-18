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
using System.Net;
using System.Text;
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{
	public abstract class TestProgram : WebInterface_ImgsInStyleSection
	{
		private HttpListener httpServer = null;
		public TestProgram()
		{
			httpServer = new HttpListener();

				Title = HtmlTitle;
			//this.externalStyleSheetName = COARSEGRIND_EXTERNAL_CSS_NAME;
			this.RefreshIntervalSeconds = RELOAD_SECONDS;
			controlsInOrder.Add(new RawCodeSegment("<center>"));
			controlsInOrder.Add(Banner);
			controlsInOrder.Add(new Divider());
		}

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

int port = 8085;
		IPAddress address = null;

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
protected void runTestProgram(string[] args)
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

		if (! httpServer.IsListening)
		{
			if (address == null)
			{
						address = IPAddress.Parse("localhost:" + port);
			}

			if (address != null)
			{
				// Attempt to start the test server
				try
				{
							httpServer.Prefixes.Add(address.ToString());
							//setupService(port, connections, address);
							httpServer.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
							httpServer.Start();
							Console.WriteLine("Listening for connections on " + address.ToString());
				}
				catch (Exception loggedException)
				{
					Console.WriteLine(Foundation.depictFailure(loggedException));
					Console.WriteLine("Test Server is offline");
				}
			}
		}

		if (tests.immediateRun != Values.Defaultstring)
		{
			TestSuite tmp = tests.allTestSuites.get(tests.immediateRun);
			if (tmp == null)
			{
				Console.WriteLine("No test suite named \"" + tests.immediateRun + "\" exists. Nothing to run so exiting the program.");
			}
			else {
				testingContinues = false;
				tests.runTestSuite(tmp);
			}
		}

		tests.waitWhileTesting();
		tests.destroy();
		tests = null;
				GC.Collect();
	}

	if (isServing()) discontinueService();
}

	public void discontinueService() //HTTP Server
{
	if (isServing())
	{
		super.discontinueService();
		testingContinues = false;
		Console.WriteLine("TEST SERVER TAKEN OFFLINE");
	}
}

	protected HttpResponse handle(HttpRequest incomingRequest) //HTTP Server
{
	//NOTE: Requests with null URLs have somehow gotten through.
	//      How to handle?

	string remoteUrlTarget =
			incomingRequest.getURL().getProtocol() + "://"
			+ incomingRequest.getURL().getHost() + ":"
			+ incomingRequest.getURL().getPort();

	TestSuite tmp = null;
	HttpResponse result = new HttpResponse(new HttpstringPayload());

	CoarseGrindInterface ui = new CoarseGrindInterface();
	ui.setRedirectionUrl(remoteUrlTarget);
	try
	{
		result.setStatusCode(200);
	}
	catch (IllegalStatusCodeException e)
	{
		// DELIBERATE NO-OP -- SHOULDN'T BE POSSIBLE
	}

	result.httpContent = new HttpContent(HttpContent.text, HttpContent.html);
	result.setServer("Rockabilly Coarse Grind Test Server");

	string[] urlParts = incomingRequest.getURL().getPath().substring(1).split("/");

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
				tmp = tests.allTestSuites.get(urlParts[1]);
				if (tmp == null)
				{
					try
					{
						result.setStatusCode(404);
					}
					catch (IllegalStatusCodeException e)
					{
						// DELIBERATE NO-OP -- SHOULDN'T BE POSSIBLE
					}

					ui.controlsInOrder.Add(new Label("No test suite named \"" + urlParts[1] + "\" exists", 300));
					((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
					ui = null;
					return result;
				}

				//We may not support command line arguments over the web for security reasons.
				//processConfigSet(incomingRequest.body.ToString().replace("\r", "").split("\n"));

				ui.controlsInOrder.Add(new Label("Running: " + urlParts[1], 500));
				ui.setRefreshIntervalSeconds(1);
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());

				if (tmp != null)
				{
					tests.KickOffTestSuite(tmp);
				}
				ui = null;
				return result;
			case APPLY_CFG_PATH_PART:
				// For RUN and APPLY, the config parameters go in the body
				tests.processConfigSet(this, incomingRequest.payload.ToString().replace("\r", "").split("\n"));
				return result;
			case STOP_ALL_TESTING_PATH_PART:
				try
				{
					tests.CurrentlyRunningSuite.haltAllTesting();
				}
				catch (Exception assumedNullRef)
				{
					try
					{
						result.setStatusCode(417);
					}
					catch (IllegalStatusCodeException e)
					{
						// DELIBERATE NO-OP -- SHOULDN'T BE POSSIBLE
					}

					ui.controlsInOrder.Add(new Label("No test suite is currently running", 300));
					((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
					ui = null;
					return result;
				}


				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(ui.useInStyleImage(ICON_STOPALL));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new Label("Halting Test Suite: " + tests.CurrentlyRunningSuite.getName(), 300));
				ui.controlsInOrder.Add(new Label("No further tests will be run.", 200));
				ui.setRefreshIntervalSeconds(4);
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				return result;
			case STOP_TEST_CASE_PATH_PART:
				try
				{
					tests.CurrentlyRunningSuite.interruptCurrentTest();
				}
				catch (Exception assumedNullRef)
				{
					try
					{
						result.setStatusCode(417);
					}
					catch (IllegalStatusCodeException e)
					{
						// DELIBERATE NO-OP -- SHOULDN'T BE POSSIBLE
					}


					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(ui.useInStyleImage(ICON_NO));
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new Label("No test suite is currently running", 300));
					ui.setRefreshIntervalSeconds(2);
					((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
					ui = null;
					return result;
				}

				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(ui.useInStyleImage(ICON_STOPONE));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new Label("Interrupting Test Case: " + tests.CurrentlyRunningSuite.getCurrentTest(), 300));
				ui.controlsInOrder.Add(new Label("All remaining tests will still run.", 200));
				ui.setRefreshIntervalSeconds(3);
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				return result;
			case KILL_SERVICE_PATH_PART:
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(ui.useInStyleImage(ICON_EXIT));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new Label("Test Server Taken Offline", 300));
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				testingContinues = false;

				try
				{
					tests.block.countDown();
				}
				catch (Exception dontCare)
				{
					// Deliberate NO-OP
				}

				discontinueService();
				return result;
			case RETRIEVE_DATA_PATH_PART:
				try
				{
					ZipFileCreator.make(Global.DEFAULT_PARENT_FOLDER + File.separator + URLDecoder.decode(urlParts[1].replace(".zip", ""), "UTF-8"), ZIP_TEMP_FILE);
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

					ui.controlsInOrder.Add(new CaptionedControl(ui.useInStyleImage(ICON_NO), new Label("Unable to create ZIP file for transfer", 300), Orientation.LeftOfCaption));
					((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
					ui = null;
					return result;
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

					ui.controlsInOrder.Add(new CaptionedControl(ui.useInStyleImage(ICON_NO), new Label("Unable to encode ZIP file into HTTP message", 300), Orientation.LeftOfCaption));
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
				return result;
			case DELETE_DATA_PATH_PART:
				if (isBusy())
				{
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(ui.useInStyleImage(ICON_DECLINEDELETE));
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new Label("DELETE COUNTERMANDED", 400));
					ui.controlsInOrder.Add(new Label("The server has become busy. You must wait until testing is finished before trying to delete again.", 300));
					((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
					ui = null;
					return result;
				}

				Foundation.hardDelete(Global.DEFAULT_PARENT_FOLDER);
				Foundation.forceDirectoryExistence(Global.DEFAULT_PARENT_FOLDER);
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(ui.useInStyleImage(ICON_CONFIRMDELETE));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new Label("ALL TEST DATA DELETED", 300));
				ui.setRefreshIntervalSeconds(3);
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				return result;
			case LIST_DATA_PATH_PART:
				resultList.refresh();
				ui.clearRefreshInterval();
				ui.clearRedirectionUrl();
				ui.setRedirectionUrl(remoteUrlTarget + '/' + LIST_DATA_PATH_PART);
				ui.setRefreshIntervalSeconds(30);
				ui.controlsInOrder.Add(new Label("TEST RESULTS", 400));
				ui.controlsInOrder.Add(new LineBreak());

				if (resultList.resultFolders.size() < 1)
				{
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(ui.useInStyleImage(ICON_NO));
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new Label("(none)", 350));
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
					ui.controlsInOrder.Add(new LineBreak());
				}
				else {
					ControlCluster cluster = new ControlCluster();
					cluster.columns = 2;
					for (string thisTestResult : resultList.resultFolders)
					{
						WebInterfaceControl icon = null;
						// Global.DEFAULT_PARENT_FOLDER + File.separator + 
						try
						{
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
						}
						catch (IOException e)
						{
							icon = ui.useInStyleImage(ICON_LOADRESULT);
						}
						try
						{
							cluster.Add(new Link(remoteUrlTarget + '/' + RETRIEVE_DATA_PATH_PART + '/' + URLEncoder.encode(thisTestResult, "UTF-8") + ".zip", new CaptionedControl(icon, thisTestResult, ICON_TEXT_SIZE, Orientation.AboveCaption)));
						}
						catch (UnsupportedEncodingException thisException)
						{
							// NO-OP: Ignored for now.
						}
					}
					ui.controlsInOrder.Add(cluster);
					cluster = null;
				}


				ui.controlsInOrder.Add(new RawCodeSegment("</center>"));
				ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/', ui.useInStyleImage(ICON_GOBACK)), "Cancel results download and go back to the list of tests to run.", 250, Orientation.LeftOfCaption));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new Divider());
				ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + CONFIRM_DELETE_DATA_PATH_PART, ui.useInStyleImage(ICON_DELETE)), "Clear the server of all test data.", 250, Orientation.LeftOfCaption));
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				return result;
			case CONFIRM_DELETE_DATA_PATH_PART:
				ui.clearRefreshInterval();
				ui.clearRedirectionUrl();
				ui.controlsInOrder.Add(new Label("REALLY DELETE TEST DATA?<br><br>", 400));

				ui.controlsInOrder.Add(new RawCodeSegment("</center>"));
				ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + LIST_DATA_PATH_PART, ui.useInStyleImage(ICON_DECLINEDELETE)), "<b><i>NO!</i></b> What was I thinking? Do <b><i>NOT</i></b> delete any of the test data.", 250, Orientation.LeftOfCaption));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + DELETE_DATA_PATH_PART, ui.useInStyleImage(ICON_CONFIRMDELETE)), "<b><i>YES</i></b>, I really do want to delete all of the test data on the server. <b><i>I understand that it can't be undone and I will lose any data I did not download and save elsewhere.</i></b>", 250, Orientation.LeftOfCaption));
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				return result;
			case CONFIRM_KILL_SERVICE_PATH_PART:
				ui.clearRefreshInterval();
				ui.clearRedirectionUrl();
				ui.controlsInOrder.Add(new Label("REALLY TERMINATE THE SERVICE?<br><br>", 400));

				ui.controlsInOrder.Add(new RawCodeSegment("</center>"));
				ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/', ui.useInStyleImage(ICON_NO)), "<b><i>NO</i></b>, --back to testing please.", 300, Orientation.LeftOfCaption));
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new LineBreak());
				ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + KILL_SERVICE_PATH_PART, ui.useInStyleImage(ICON_YES)), "<b><i>YES</i></b>, --terminate the test server program. <b><i>The web interface will not be available until the server program is restarted manually.</i></b>", 250, Orientation.LeftOfCaption));
				((HttpstringPayload)result.payload).getContent().Append(ui.ToString());
				ui = null;
				return result;
		}
	}
	catch //(ArrayIndexOutOfBoundsException expected)
	{
		// DELIBERATE NO-OP. They want the root page.
	}


	ui.controlsInOrder.Add(new Label(this.getClass().getSimpleName(), 500));

	if (isReady())
	{
		if (isBusy())
		{
			//Show progress and options to stop
			ui.setRefreshIntervalSeconds(3);
			ui.controlsInOrder.Add(new CaptionedControl(new ProgressBar(getProgress(), 100), new Label("Progress on test suite <b><i>\"" + tests.CurrentlyRunningSuite.getName() + "\"</i></b>", 150), Orientation.AboveCaption));
			ui.controlsInOrder.Add(new Divider());
			ui.controlsInOrder.Add(new Label("OPTIONS", 300));
			ui.controlsInOrder.Add(new LineBreak());
			ui.controlsInOrder.Add(new RawCodeSegment("</center>"));
			ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + STOP_TEST_CASE_PATH_PART, ui.useInStyleImage(ICON_STOPONE)), "Stop individual test <b><i>\"" + tests.CurrentlyRunningSuite.getCurrentTest() + "\"</i></b> and allow the remaining tests to run.", 250, Orientation.LeftOfCaption));
			ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + STOP_ALL_TESTING_PATH_PART, ui.useInStyleImage(ICON_STOPALL)), "Halt test suite <b><i>\"" + tests.CurrentlyRunningSuite.getName() + "\"</i></b> completely. No more tests will run.", 250, Orientation.LeftOfCaption));
		}
		else {
			// List available tests
			ui.controlsInOrder.Add(new Label("<br>AVAILABLE TEST SUITES", 200));


			ControlCluster cluster = new ControlCluster();
			cluster.columns = 4;
			for (string thisTestSuite : tests.allTestSuites.keySet())
			{
				WebInterfaceControl icon = null;
				int size = tests.allTestSuites.get(thisTestSuite).size();
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
				cluster.Add(new Link(remoteUrlTarget + '/' + RUN_PATH_PART + '/' + thisTestSuite, new CaptionedControl(icon, thisTestSuite, ICON_TEXT_SIZE, Orientation.AboveCaption)));
				icon = null;
			}
			ui.controlsInOrder.Add(cluster);
			cluster = null;

			ui.controlsInOrder.Add(new RawCodeSegment("</center>"));
			ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + LIST_DATA_PATH_PART, ui.useInStyleImage(ICON_DOWNLOAD)), "Download individual test results or clear the server of result data.", 250, Orientation.LeftOfCaption));
			ui.controlsInOrder.Add(new LineBreak());
			ui.controlsInOrder.Add(new Divider());
			ui.controlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + CONFIRM_KILL_SERVICE_PATH_PART, ui.useInStyleImage(ICON_EXIT)), "Stop the test program completely. It will need to be restarted manually.", 250, Orientation.LeftOfCaption));
		}
	}
	else {
		ui.controlsInOrder.Add(new RawCodeSegment("</center>"));
		ui.controlsInOrder.Add(new Label("Tests are setting up...", 300));
	}

		((HttpstringPayload)result.payload).getContent().Append(ui.ToString());

	ui = null;
	return result;
}

private string HtmlTitle
	{
		get
		{
			StringBuilder result = new StringBuilder(this.GetType().Name);
			result.Append(" - ");
			if (isBusy())
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
