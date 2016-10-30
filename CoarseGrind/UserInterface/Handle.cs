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


// http://www.javascriptkit.com/frame2.shtml

using System;
using System.Net;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind
{
	public abstract partial class TestProgram : HttpServer
	{
		public override string Handle(HttpListenerRequest incomingRequest) //HTTP Server
		{
			string remoteUrlTarget =
				incomingRequest.Url.Scheme + "://"
					+ incomingRequest.Url.Host + ":"
					+ incomingRequest.Url.Port;

			string[] urlParts = incomingRequest.Url.PathAndQuery.Substring(1).Split("/?=&".ToCharArray());
			//string[] urlParts = incomingRequest.Url.PathAndQuery.Substring(1).Split('/');

			string response = default(string);
			try
			{
				switch (urlParts[0])
				{
					case BANNER_FRAME_PATH:
						response = BannerFrame;
						break;
					case STATUS_FRAME_PATH:
						response = GetStatusFrameNormal(remoteUrlTarget);
						break;
					case CONTROL_FRAME_PATH:
						response = GetControlFrame(remoteUrlTarget);
						break;
					case CARRY_OUT_KILL_SERVICE_PATH:
						response = CarryOutKillService(remoteUrlTarget);
						break;
					case REQUEST_KILL_SERVICE_PATH:
						response = RequestKillService(remoteUrlTarget);
						break;
					case TEST_SUITES_PATH:
						response = GetTestSuiteLaunchPane(remoteUrlTarget);
						break;
					case TEST_RESULTS_PATH:
						response = GetUnderConstructionWarning("Test Result Browsing");
						break;
					case RUN_SUITE_PATH:
						response = RunTestSuite(remoteUrlTarget, urlParts[1]);
						break;
					case STOP_TEST_CASE_PATH_PART:
						response = StopTest(remoteUrlTarget, false);
						break;
					case STOP_ALL_TESTING_PATH_PART:
						response = StopTest(remoteUrlTarget, true);
						break;
					case SELECT_CUSTOM_SUITE_PATH:
						response = SelectIndividualTests(remoteUrlTarget);
						break;
					case RUN_CUSTOM_SUITE_PATH:
						response = runCustomTestSuite(remoteUrlTarget, incomingRequest.Url.Query.GetUrlParameters());
						break;
					default:
						// DELIBERATE NO-OP
						break;
				}
			}
			catch
			{
				// DELIBERATE NO-OP. They want the root page.
			}

			if (response.IsBlank())
			{
				response = Index_HTML;
			}

			return response;
	}
}
}