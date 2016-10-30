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
	public enum ButtonTypes
	{
		button, submit, reset
	}

	public class Button : WebInterfaceControl
	{
		private string content = default(string);
		public ButtonTypes type = ButtonTypes.button;

		public Button(string text, ButtonTypes buttonType = ButtonTypes.button)
		{
			content = text;
			type = buttonType;
		}

		public string CssStyle
		{
			get
			{
				return "";
			}
		}


		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<button ");
			result.Append("type=\"");
			result.Append(type);
			result.Append("\"");

			string style = CssStyle;
			if (style != "")
			{
				result.Append(" style=\"");
				result.Append(style);
				result.Append(";\"");
			}

			result.Append('>');
			result.Append(content);
			result.Append("</button>");
			return result.ToString();
		}
	}
}
