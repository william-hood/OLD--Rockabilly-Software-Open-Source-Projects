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
		private static string controlFrame = default(string);
		public string ControlFrame
		{
			get
			{
				if (controlFrame == default(string))
				{
					WebInterface ui = new WebInterface();
					ControlCluster cluster = new ControlCluster();
					cluster.columns = 1;
					ui.Title = "Control Frame";

					// These will have to be made into links
					cluster.Add(ICON_SUITES);
					cluster.Add(ICON_ALLTESTS);
					cluster.Add(ICON_RESULTS);
					cluster.Add(ICON_EXIT);

					ui.ControlsInOrder.Add(cluster);
					controlFrame = ui.ToString();

					cluster = null;
					ui = null;
				}

				return controlFrame;
			}
		}
	}
}