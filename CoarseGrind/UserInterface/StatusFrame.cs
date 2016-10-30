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

		private string GetStatusFrameNormal(string remoteUrlTarget)
		{
			renewFrame();
			if (IsBusy) return ProgressBar(remoteUrlTarget);

			//statusFrame.controlsInOrder.add(new Label("Stub: System Ready"));
			return statusFrame.ToString();
		}

		private string ProgressBar(string remoteUrlTarget)
		{
			//Show progress and options to stop
			ControlCluster cluster = new ControlCluster();
			cluster.columns = 1;
			cluster.Add(new RawCodeSegment("<center>"));
			ProgressBar progress = new ProgressBar(Progress, 100);
			progress.heightPixels = 20;
			progress.widthPixels = 450;
			cluster.Add(progress);
			cluster.Add(new RawCodeSegment("</center>"));
			cluster.Add(new CaptionedControl(new Link(remoteUrlTarget + "/" + STOP_TEST_CASE_PATH_PART, ICON_STOPONE, VIEW_FRAME_NAME), "Stop individual test <b><i>\"" + tests.CurrentlyRunningSuite.CurrentTest + "\"</i></b> and allow the remaining tests to run.", CaptionedControlOrientation.LeftOfCaption, 75));
			cluster.Add(new CaptionedControl(new Link(remoteUrlTarget + "/" + STOP_ALL_TESTING_PATH_PART, ICON_STOPALL, VIEW_FRAME_NAME), "Halt test suite <b><i>\"" + tests.CurrentlyRunningSuite.Name + "\"</i></b> completely. No more tests will run.", CaptionedControlOrientation.LeftOfCaption, 75));

			statusFrame.ControlsInOrder.Add(cluster);

			progress = null;
			cluster = null;
			return statusFrame.ToString();
		}
	}
}