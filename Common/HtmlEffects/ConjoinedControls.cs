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

namespace Rockabilly.Common.HtmlEffects
{

	public enum ConjoinedControlsOrientation { AlphaAbove, AlphaLeft, AlphaRight, AlphaBelow };

	public class ConjoinedControls : WebInterfaceControl
	{
		private ConjoinedControlsOrientation orientation = ConjoinedControlsOrientation.AlphaAbove;
		WebInterfaceControl contentAlpha = null;
		WebInterfaceControl contentOmega = null;

		public ConjoinedControls(WebInterfaceControl alpha, WebInterfaceControl omega, ConjoinedControlsOrientation position)
		{
			contentAlpha = alpha;
			contentOmega = omega;
			orientation = position;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<table border=\"0\">");

			switch (orientation)
			{
				case ConjoinedControlsOrientation.AlphaAbove:
					result.Append("<tr>");
					result.Append(contentAlpha.ToString());
					result.Append("</tr><tr>");
					result.Append(contentOmega.ToString());
					result.Append("</tr>");
					break;
				case ConjoinedControlsOrientation.AlphaBelow:
					result.Append("<tr>");
					result.Append(contentOmega.ToString());
					result.Append("</tr><tr>");
					result.Append(contentAlpha.ToString());
					result.Append("</tr>");
					break;
				case ConjoinedControlsOrientation.AlphaLeft:
					result.Append("<tr><td>");
					result.Append(contentAlpha.ToString());
					result.Append("</td><td>&nbsp;</td><td>");
					result.Append(contentOmega.ToString());
					result.Append("</td></tr>");
					break;
				case ConjoinedControlsOrientation.AlphaRight:
					result.Append("<tr><td>");
					result.Append(contentOmega.ToString());
					result.Append("</td><td>&nbsp;</td><td>");
					result.Append(contentAlpha.ToString());
					result.Append("</td></tr>");
					break;
			}

			result.Append("</table>");
			return result.ToString();
		}
	}
}
