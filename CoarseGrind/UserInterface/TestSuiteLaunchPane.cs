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
		public string GetTestSuiteLaunchPane(string remoteUrlTarget)
		{
			WebInterface testSuiteLaunchPane = new WebInterface();
			testSuiteLaunchPane = null;
			testSuiteLaunchPane = new WebInterface();
			testSuiteLaunchPane.Title = "Test Suites";
			testSuiteLaunchPane.ControlsInOrder.Add(new RawCodeSegment("<center>"));
			if (this.IsReady)
			{

				// List available tests
				testSuiteLaunchPane.ControlsInOrder.Add(new Label("AVAILABLE TEST SUITES", 200));


				ControlCluster cluster = new ControlCluster();
				cluster.columns = 3;
				foreach (string thisTestSuite in tests.AllTestSuites.Keys)
				{
					WebInterfaceControl icon = null;
					int size = tests.AllTestSuites[thisTestSuite].Count;
					if (size < SMALL_SUITE_THRESHOLD)
					{
						icon = ICON_SMALLTEST;
					}
					else if (size > LARGE_SUITE_THRESHOLD)
					{
						icon = ICON_LARGETEST;
					}
					else {
						icon = ICON_MEDIUMTEST;
					}
					cluster.Add(new Link(remoteUrlTarget + "/" + RUN_SUITE_PATH + '/' + thisTestSuite, new CaptionedControl(icon, thisTestSuite, CaptionedControlOrientation.AboveCaption, ICON_TEXT_SIZE).ToString()));
					icon = null;
				}
				testSuiteLaunchPane.ControlsInOrder.Add(cluster);
				cluster = null;
			}
			else {
				testSuiteLaunchPane.RefreshIntervalSeconds = 3;
				testSuiteLaunchPane.ControlsInOrder.Add(new Label("Tests are setting up...", 300));
			}


			testSuiteLaunchPane.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			return testSuiteLaunchPane.ToString();
		}
	}
}