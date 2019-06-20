// Copyright (c) 2019, 2016 William Arthur Hood
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

	public class Link : WebInterfaceControl
	{
		private string content = default(string);
		private string destUrl = default(string);
		private string targetFrameName = default(string);

		public Link(string destinationUrl, string text, string targetFrame = default(string))
		{
			destUrl = destinationUrl;
			content = text;
			targetFrameName = targetFrame;
		}

		public Link(string destinationUrl, WebInterfaceControl innerControl, string targetFrame = default(string)) : this(destinationUrl, innerControl.ToString(), targetFrame)
		{
		}

		public string getStyle()
		{
			return "text-decoration: none";
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<a ");
			result.Append("href=\"");
			result.Append(destUrl);
			result.Append("\"");

			if (targetFrameName != default(string))
			{
				result.Append(" target=\"");
				result.Append(targetFrameName);
				result.Append("\"");
			}

			string style = getStyle();
			if (style != default(string))
			{
				result.Append(" style=\"");
				result.Append(style);
				result.Append(";\"");
			}

			result.Append('>');
			result.Append(content);
			result.Append("</a>");
			return result.ToString();
		}
	}
}
