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


using Rockabilly.CoarseGrind;
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;
namespace Rockabilly.CoarseGrind
{
	public abstract partial class TestProgram : HttpServer
	{
		private WebInterface statusFrame = null;
		private void renewFrame()
		{
			statusFrame = null;
			statusFrame = new WebInterface();
			statusFrame.Title = "Status Frame";
			statusFrame.RefreshIntervalSeconds = 1;
		}

		public string StatusFrameNormal
		{
			get
			{
				renewFrame();
				if (IsBusy) return ProgressBar;

				statusFrame.ControlsInOrder.Add(new Label("Stub: System Ready"));
				return statusFrame.ToString();
			}
		}

		public string StatusFrameStartTests
		{
			get
			{
				renewFrame();
				if (IsBusy) return ProgressBar;

				statusFrame.ControlsInOrder.Add(new Label("Stub: Start Custom Test"));
				return statusFrame.ToString();
			}
		}

		public string StatusFrameExit
		{
			get
			{
				renewFrame();
				if (IsBusy) return ProgressBar;

				statusFrame.ControlsInOrder.Add(new Label("Stub: Exit"));
				return statusFrame.ToString();
			}
		}


	private string ProgressBar
		{
			get
			{
				//Show progress and options to stop
				statusFrame.ControlsInOrder.Add(new CaptionedControl(new ProgressBar(Progress, 100), new Label("Progress on test suite <b><i>\"" + tests.CurrentlyRunningSuite.Name + "\"</i></b>", 150), CaptionedControlOrientation.AboveCaption));
				statusFrame.ControlsInOrder.Add(new Divider());
				statusFrame.ControlsInOrder.Add(new Label("OPTIONS", 300));
				statusFrame.ControlsInOrder.Add(new LineBreak());
				statusFrame.ControlsInOrder.Add(new RawCodeSegment("</center>"));
				statusFrame.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + STOP_TEST_CASE_PATH_PART, ICON_STOPONE), "Stop individual test <b><i>\"" + tests.CurrentlyRunningSuite.CurrentTest + "\"</i></b> and allow the remaining tests to run.", CaptionedControlOrientation.LeftOfCaption, 250));
				statusFrame.ControlsInOrder.Add(new CaptionedControl(new Link(/*remoteUrlTarget + */"/" + STOP_ALL_TESTING_PATH_PART, ICON_STOPALL), "Halt test suite <b><i>\"" + tests.CurrentlyRunningSuite.Name + "\"</i></b> completely. No more tests will run.", CaptionedControlOrientation.LeftOfCaption, 250));


				return statusFrame.ToString();
			}
		}
	}
}