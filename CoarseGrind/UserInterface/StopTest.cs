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
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{
	public abstract partial class TestProgram : HttpServer
	{
		private string StopTest(string remoteUrlTarget, bool entireSuite)
		{
			WebInterface ui = new WebInterface();
			ui.ControlsInOrder.Add(new RawCodeSegment("<center>"));
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			try
			{
				if (entireSuite)
				{
					tests.CurrentlyRunningSuite.HaltAllTesting();

					ui.ControlsInOrder.Add(
					new ConjoinedControls(ICON_STOPALL,
							new ConjoinedControls(new Label("Halting Test Suite: " + tests.CurrentlyRunningSuite.Name, 150),
									new Label("No further tests will be run.", 100),
									ConjoinedControlsOrientation.AlphaAbove),
							ConjoinedControlsOrientation.AlphaLeft
							)
					);
				}
				else {
					tests.CurrentlyRunningSuite.InterruptCurrentTest();

					ui.ControlsInOrder.Add(
					new ConjoinedControls(ICON_STOPONE,
							new ConjoinedControls(new Label("Interrupting Test: " + tests.CurrentlyRunningSuite.CurrentTest, 150),
									new Label("All remaining tests will still run.", 100),
									ConjoinedControlsOrientation.AlphaAbove),
							ConjoinedControlsOrientation.AlphaLeft
							)
					);
				}

				ui.RefreshIntervalSeconds = 4;
				ui.RedirectionUrl = remoteUrlTarget + "/" + TEST_SUITES_PATH;
			}
			catch
			{
				ui.ControlsInOrder.Add(ICON_NO);
				ui.ControlsInOrder.Add(new LineBreak());
				ui.ControlsInOrder.Add(new Label("No test suite is currently running", 300));
				ui.RefreshIntervalSeconds = 2;
				ui.RedirectionUrl = remoteUrlTarget + "/" + TEST_SUITES_PATH;
			}

			ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			return ui.ToString();
		}
	}

}