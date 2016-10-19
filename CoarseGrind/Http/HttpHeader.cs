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
using System;
using System.Collections.Generic;

namespace Rockabilly.Common.Http
{
	public class HttpHeader
	{
		private KeyValuePair<String, String> holdings = default(KeyValuePair<String, String>);

		private const string httpDateFormat = "EEE, dd MMM yyyy HH:mm:ss z";
		public const string DATE_HEADER_KEY = "Date";
		public const string SERVER_HEADER_KEY = "Server";

		//static {
		//	httpDateFormat.setTimeZone(TimeZone.getTimeZone("GMT"));
		//}

		public static string getHttpDate()
		{
			return DateTime.Now.ToString(httpDateFormat);
		}

		public HttpHeader(string key, string value)
		{
			holdings = new KeyValuePair<string, string>(key, value);
		}

		public string Key
		{
			get
			{
				return holdings.Key;
			}
		}

		public string Value
		{
			get
			{
				return holdings.Value;
			}
		}

		public static HttpHeader fromString(String headerLine)
		{
			int split = headerLine.IndexOf(':');
			if (split == -1) return new HttpHeader(headerLine, default(string));
			return new HttpHeader(headerLine.Substring(0, split), headerLine.Substring(split + 2));
		}

		public static HttpHeader getDateHeader()
		{
			return new HttpHeader(DATE_HEADER_KEY, getHttpDate());
		}

		public override string ToString()
		{
			return Key + ": " + Value;
		}
	}
}
