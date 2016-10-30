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
		private string SelectIndividualTests(string remoteUrlTarget)
		{
			WebInterface ui = new WebInterface();
			ui.ControlsInOrder.Add(new RawCodeSegment("<form action=\"" + remoteUrlTarget + '/' + RUN_CUSTOM_SUITE_PATH + "\" method=\"get\">"));

			ui.ControlsInOrder.Add(new RawCodeSegment("<table border=\"0\"><tr>"));
			foreach (Test thisTest in AllTests)
			{
				ui.ControlsInOrder.Add(new RawCodeSegment("<tr>"));
				ui.ControlsInOrder.Add(new RawCodeSegment("<td>"));
				//ui.ControlsInOrder.Add(new Checkbox("test", thisTest.getIdentifier(), "-webkit-appearance:none; width:35px; height:35px; border-radius:5px; border:1px solid black;"));
				ui.ControlsInOrder.Add(new Checkbox("test", thisTest.Identifier, "-ms-transform: scale(2); -moz-transform: scale(2); -webkit-transform: scale(2); -o-transform: scale(2); transform: scale(2);"));
				ui.ControlsInOrder.Add(new RawCodeSegment("</td><td>" + HtmlLogSystem.HTML__SPACE + "</td><td>" + HtmlLogSystem.HTML__SPACE + "</td><td>"));
				ui.ControlsInOrder.Add(new Label(thisTest.Identifier, 300));
				ui.ControlsInOrder.Add(new RawCodeSegment("</td><td>"));
				ui.ControlsInOrder.Add(new ConjoinedControls(new Label("<u>" + thisTest.Name + "</u>", 150),
						new Label(thisTest.DetailedDescription, 100),
						ConjoinedControlsOrientation.AlphaAbove));
				ui.ControlsInOrder.Add(new RawCodeSegment("</td>"));

				ui.ControlsInOrder.Add(new RawCodeSegment("</tr><tr><td>" + HtmlLogSystem.HTML__SPACE + "</td><td>" + HtmlLogSystem.HTML__SPACE + "</td></tr>"));
			}

			ui.ControlsInOrder.Add(new RawCodeSegment("</table>"));

			ui.ControlsInOrder.Add(new RawCodeSegment("<center>"));
			ui.ControlsInOrder.Add(new Button(new Label("BEGIN TESTING", 200).ToString() + BUTTON_BEGIN.ToString(), ButtonTypes.submit));
			ui.ControlsInOrder.Add(new RawCodeSegment("</center>"));
			ui.ControlsInOrder.Add(new RawCodeSegment("</form>"));

			return ui.ToString();
		}
	}

}