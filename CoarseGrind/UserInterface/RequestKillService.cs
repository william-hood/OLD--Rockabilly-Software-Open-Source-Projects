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


using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{
	public abstract partial class TestProgram : HttpServer
	{
		private string RequestKillService(string remoteUrlTarget)
		{
			WebInterface ui = new WebInterface();
			ui.ControlsInOrder.Add(new RawCodeSegment("<center>"));

			ui.ControlsInOrder.Add(new Label("Do you want to stop the test server?<br><br>", 300));

			ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			ui.ControlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + TEST_SUITES_PATH, ICON_NO), "<b><i>NO</i></b>, --back to testing please.", CaptionedControlOrientation.LeftOfCaption, 250));
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new LineBreak());
			ui.ControlsInOrder.Add(new CaptionedControl(new Link(remoteUrlTarget + '/' + CARRY_OUT_KILL_SERVICE_PATH, ICON_YES, "_top"), "<b><i>YES</i></b>, --terminate the test server program. <b><i>The web interface will not be available until the server program is restarted manually.</i></b>", CaptionedControlOrientation.LeftOfCaption, 200));

			return ui.ToString();
		}
	}
}