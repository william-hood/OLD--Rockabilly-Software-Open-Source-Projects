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
using System.Collections.Generic;
using Rockabilly.Common;

namespace Rockabilly.Common.HtmlEffects
{

	public class WebInterface
	{

		public const string HTML_BODY_START = "\t<body>";
		public const string HTML_BODY_END = "\t</body>";

		protected string styles = default(string);

		private int refreshInterval = default(int);
		private string pageTitle = "Web Interface";
		private string redirectionUrl = default(string);

		public readonly List<WebInterfaceControl> controlsInOrder = new List<WebInterfaceControl>();
		public string externalStyleSheetName = default(string);

		public int RefreshIntervalSeconds
		{
			get
			{
				return refreshInterval;
			}
			set
			{
				refreshInterval = value;
				if (value < 1) ClearRefreshInterval();
			}
		}

		public void ClearRefreshInterval()
		{
			RefreshIntervalSeconds = default(int);
		}

		public bool HasRefreshInterval
		{
			get
			{
				return refreshInterval != default(int);
			}
		}

		public string Title
		{
			get
			{
				return pageTitle;
			}
			set
			{
				pageTitle = value;
			}
		}

		protected virtual string getCssStyle()
		{
			StringBuilder result = new StringBuilder();
			if (styles != default(string))
			{
				result.Append(styles);
				result.Append(Symbols.CarriageReturnLineFeed);
			}

			return result.ToString();
		}

		public string CssStyles
		{
			get
			{
				return getCssStyle();
			}
			set
			{
				styles = value;
			}
		}

		public string RedirectionUrl
		{
			get
			{
				return redirectionUrl;
			}
			set
			{
				redirectionUrl = value;
			}
		}

		public void ClearRedirectionUrl()
		{
			RedirectionUrl = default(string);
		}

		public bool HasRedirectionUrl()
		{
			return redirectionUrl != default(string);
		}

		protected virtual bool HasCssStyle()
		{
			return styles != default(string);
		}

		public string HtmlHeader
		{
			get
			{
				StringBuilder result = new StringBuilder("<!DOCTYPE html>");
				result.Append(Symbols.CarriageReturnLineFeed);
				result.Append("<html>");
				result.Append(Symbols.CarriageReturnLineFeed);
				result.Append("\t<head>");
				result.Append(Symbols.CarriageReturnLineFeed);
				result.Append("\t\t<title>");
				result.Append(pageTitle);
				result.Append("</title>");
				result.Append(Symbols.CarriageReturnLineFeed);
				// charset="utf-8"

				result.Append("\t\t<META charset=\"utf-8\" ");
				if (HasRefreshInterval)
				{
					result.Append("HTTP-EQUIV=\"refresh\" CONTENT=\"");
					result.Append(refreshInterval);

					if (HasRedirectionUrl())
					{
						result.Append(";URL='");
						result.Append(redirectionUrl);
						result.Append("'");
					}

					result.Append("\"");
				}
				result.Append(">");
				result.Append(Symbols.CarriageReturnLineFeed);

				if (HasCssStyle())
				{

					if (externalStyleSheetName == default(string))
					{
						result.Append("<style type=\"text/css\">");
						result.Append(Symbols.CarriageReturnLineFeed);
						result.Append(CssStyles);
						result.Append("</style>");
					}
					else {
						result.Append("<link rel = \"stylesheet\"");
						result.Append("type = \"text/css\"");
						result.Append("href = \"");
						result.Append(externalStyleSheetName);
						result.Append("\" />");
					}

				}

				result.Append("\t</head>");

				result.Append(Symbols.CarriageReturnLineFeed);
				result.Append(HTML_BODY_START);
				result.Append(Symbols.CarriageReturnLineFeed);
				return result.ToString();
			}
		}

		public string getHtmlFooter()
		{
			return HTML_BODY_END + Symbols.CarriageReturnLineFeed + "</html>";
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder(HtmlHeader);

			for (int index = 0; index < controlsInOrder.Count; index++)
			{
				result.Append("\t\t");
				result.Append(controlsInOrder[index].ToString());
				result.Append("<br>");
				result.Append(Symbols.CarriageReturnLineFeed);
			}

			result.Append(getHtmlFooter());
			return result.ToString();
		}

	}
}