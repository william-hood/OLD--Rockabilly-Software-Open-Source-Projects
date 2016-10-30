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

using System.Text;
using Rockabilly.Common;
using Rockabilly.Common.HtmlEffects;

// http://www.w3schools.com/tags/tag_iframe.asp

namespace Rockabilly.CoarseGrind
{
	public abstract partial class TestProgram : HttpServer
	{
		private static string index_Html = default(string);
		public string Index_HTML
		{
			get
			{
				if (index_Html == default(string))
				{
					WebInterface ui = new WebInterface();
					ui.Title = "Coarse Grind Test Server - " + this.GetType().Name;

					StringBuilder code = new StringBuilder();

					code.Append("<div class=\"");
					code.Append(BANNER_FRAME_NAME);
					code.Append("\"><iframe src=\"");
					code.Append(BANNER_FRAME_PATH);
					code.Append("\" height=\"200\" width=\"49%\" align=\"left\" name=\"");
					code.Append(BANNER_FRAME_NAME);
					code.Append("\" style=\"border:none;\"></iframe></div>");

					code.Append("<div class=\"");
					code.Append(STATUS_FRAME_NAME);
					code.Append("\"><iframe src=\"");
					code.Append(STATUS_FRAME_PATH);
					code.Append("\" height=\"200\" width=\"49%\" align=\"right\" name=\"");
					code.Append(STATUS_FRAME_NAME);
					code.Append("\" style=\"border:none;\"></iframe></div>");

					code.Append(new LineBreak());

					code.Append("<div class=\"");
					code.Append(CONTROL_FRAME_NAME);
					code.Append("\"><iframe src=\"");
					code.Append(CONTROL_FRAME_PATH);
					code.Append("\" height=\"800\" width=\"24%\" align=\"left\" name=\"");
					code.Append(CONTROL_FRAME_NAME);
					code.Append("\" style=\"border:none;\"></iframe></div>");

					code.Append("<div class=\"");
					code.Append(VIEW_FRAME_NAME);
					code.Append("\"><iframe src=\"");
					code.Append(TEST_SUITES_PATH);
					code.Append("\" height=\"800\" width=\"75%\" align=\"right\" name=\"");
					code.Append(VIEW_FRAME_NAME);
					code.Append("\" style=\"border:1px solid black;\"></iframe></div>");


					ui.ControlsInOrder.Add(new RawCodeSegment(code.ToString()));
					index_Html = ui.ToString();

					ui = null;
				}

				return index_Html;
			}
		}
	}
}