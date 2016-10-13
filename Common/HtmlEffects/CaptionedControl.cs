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
using System.Text;

namespace Rockabilly.Common.HtmlEffects
{

	public enum CaptionedControlOrientation { AboveCaption, LeftOfCaption, RightOfCaption, BelowCaption }

	public class CaptionedControl : WebInterfaceControl
	{
		private CaptionedControlOrientation orientation = CaptionedControlOrientation.AboveCaption;
		private Label text = null;
		internal WebInterfaceControl content = null;
		private HorizontalJustification textAlignment = HorizontalJustification.CENTER;

		public CaptionedControl(WebInterfaceControl control, Label caption, CaptionedControlOrientation position)
		{
			content = control;
			text = caption;
			orientation = position;
			GuessJustification();
		}

		public CaptionedControl(WebInterfaceControl control, string caption, CaptionedControlOrientation position, int fontSize = 100) : this(control, new Label(caption, fontSize), position)
		{
		}

		public Label Caption { get { return text; } }

		private void GuessJustification()
		{
			switch (orientation)
			{
				case CaptionedControlOrientation.AboveCaption:
					textAlignment = HorizontalJustification.CENTER;
					break;
				case CaptionedControlOrientation.BelowCaption:
					textAlignment = HorizontalJustification.CENTER;
					break;
				case CaptionedControlOrientation.LeftOfCaption:
					textAlignment = HorizontalJustification.LEFT;
					break;
				case CaptionedControlOrientation.RightOfCaption:
					textAlignment = HorizontalJustification.RIGHT;
					break;
			}
		}

		private HorizontalJustification ContentAlignment
		{
			get
			{
				if (textAlignment == HorizontalJustification.LEFT) return HorizontalJustification.RIGHT;
				if (textAlignment == HorizontalJustification.RIGHT) return HorizontalJustification.LEFT;
				return textAlignment;
			}
		}

		private string RenderedLabel
		{
			get
			{
				StringBuilder result = new StringBuilder("<td><div style=\"text-align:");
				result.Append(textAlignment.ToString().ToLower());
				result.Append(";\">");
				result.Append(text.ToString());
				result.Append("</div></td>");
				return result.ToString();
			}
		}

		private string RenderedContent
		{
			get
			{
				StringBuilder result = new StringBuilder("<td><div style=\"text-align:");
				result.Append(ContentAlignment.ToString().ToLower());
				result.Append(";\">");
				result.Append(content.ToString());
				result.Append("</div></td>");
				return result.ToString();
			}
		}


		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<table border=\"0\">");

			switch (orientation)
			{
				case CaptionedControlOrientation.AboveCaption:
					result.Append("<tr>");
					result.Append(RenderedContent);
					result.Append("</tr><tr>");
					result.Append(RenderedLabel);
					result.Append("</tr>");
					break;
				case CaptionedControlOrientation.BelowCaption:
					result.Append("<tr>");
					result.Append(RenderedLabel);
					result.Append("</tr><tr>");
					result.Append(RenderedContent);
					result.Append("</tr>");
					break;
				case CaptionedControlOrientation.LeftOfCaption:
					result.Append("<tr>");
					result.Append(RenderedContent);
					result.Append("<td>&nbsp;</td>");
					result.Append(RenderedLabel);
					result.Append("</tr>");
					break;
				case CaptionedControlOrientation.RightOfCaption:
					result.Append("<tr>");
					result.Append(RenderedLabel);
					result.Append("<td>&nbsp;</td>");
					result.Append(RenderedContent);
					result.Append("</tr>");
					break;
			}

			result.Append("</table>");
			return result.ToString();
		}
	}
}