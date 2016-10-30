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



using System.Collections.Generic;
using System.Text;
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{
	public abstract partial class TestProgram : HttpServer
	{
		private Test findTest(string identifier)
		{
			foreach (Test thisTest in AllTests)
			{
				if (thisTest.Identifier.MatchesCaseInspecific(identifier)) return thisTest;
			}
			return null;
		}

		private string runCustomTestSuite(string remoteUrlTarget, List<KeyValuePair<string, string>> selectedTests)
		{
			WebInterface ui = new WebInterface();
			ui.RedirectionUrl = remoteUrlTarget + "/" + TEST_SUITES_PATH;
			ui.RefreshIntervalSeconds = 3;
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new RawCodeSegment("<center>"));

			if (IsBusy)
			{
				ui.ControlsInOrder.Add(InProgressWarning);
				ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			}
			else {
				TestSuite tmp = new TestSuite("Custom");
				StringBuilder warnings = new StringBuilder();

				foreach (KeyValuePair<string, string> thisTest in selectedTests)
				{
					Test foundTest = findTest(thisTest.Value);
					if (foundTest == null)
					{
						warnings.Append("<li>No test named " + thisTest + " exists; Not adding to " + tmp.Name);
					}
					else {
						tmp.Add(foundTest);
					}
				}

				tests.KickOffTestSuite(tmp);
				ui.ControlsInOrder.Add(new CaptionedControl(ICON_ALLTESTS, "Running custom suite of selected tests", CaptionedControlOrientation.LeftOfCaption, 300));
				ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));

				if (warnings.Length > 0)
				{
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new LineBreak());
					ui.ControlsInOrder.Add(new Label("<u>WARNINGS</u>", 200));
					ui.ControlsInOrder.Add(new Label(warnings.ToString(), 100));
				}
			}

			return ui.ToString();
		}
	}

}