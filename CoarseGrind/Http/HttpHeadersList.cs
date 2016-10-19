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
using System.IO;
using System.Text;

namespace Rockabilly.Common.Http
{
	public class HttpHeadersList
	{
		public static int WORK_AROUND_DELAY = 0;

		public readonly List<HttpHeader> headers = new List<HttpHeader>();
		public HttpContent httpContent = new HttpContent();

		protected String headersToOutgoingDataString()
		{
			StringBuilder result = new StringBuilder();

			if (httpContent.IsSet)
			{
				headers.Add(httpContent.ToHttpHeader);
			}

			if (headers.Count > 0)
			{
				foreach (HttpHeader thisHeader in headers)
				{
					result.Append(thisHeader.ToString());
					result.Append(Symbols.CarriageReturnLineFeed);
				}

				result.Append(Symbols.CarriageReturnLineFeed);
			}

			return result.ToString();
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			//if (! httpContent.toString().isEmpty()) result.append(httpContent.toHttpHeader().toString());;
			result.Append(Symbols.CarriageReturnLineFeed);
			foreach (HttpHeader thisOne in headers)
			{
				result.Append(thisOne.ToString());
				result.Append(Symbols.CarriageReturnLineFeed);
			}

			return result.ToString();
		}

		internal string PopulateHeadersFromInputStream(BufferedStream inputStream)
		{
			// This assumes we've read in far enough to begin reading the headers
			// Returns the server
			string server = default(string);

			try
			{
				// Read headers, Content Type, and Server.
				// (Date is left as a header and not a special field)
				string headerLine = Foundation.ReadLineFromInputStream(inputStream);
				while (!headerLine.IsBlank())
				{

					// DEBUG
					//System.out.println(headerLine);
					// It appears that Java itself has a race condition (thanks Oracle)
					// Need to print, or otherwise access headerLine here or you
					// might get a mysterious nullref downstream. *sigh*
					//if (WORK_AROUND_DELAY > 0) {
					//	Thread.sleep(WORK_AROUND_DELAY);
					//}

					HttpHeader thisHeader = HttpHeader.fromString(headerLine);

					if (thisHeader.Key.Equals(HttpContent.HEADER_KEY))
					{
						httpContent = HttpContent.FromHttpHeader(thisHeader);
					}
					else if (thisHeader.Key.Equals(HttpHeader.SERVER_HEADER_KEY))
					{
						server = thisHeader.Value;
					}
					else {
						headers.Add(thisHeader);
					}

					headerLine = Foundation.ReadLineFromInputStream(inputStream);
				}
			}
			catch (Exception causalException)
			{
				throw new HttpMessageParseException(causalException);
			}


			// DEBUG
			//System.out.println(FX.divider());

			return server;
		}
	}
}
