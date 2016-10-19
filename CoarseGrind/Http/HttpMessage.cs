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
using System.IO;
using System.Text;

namespace Rockabilly.Common.Http
{
	public abstract class HttpMessage : HttpHeadersList
	{
		public static HttpVerb HttpVerbFromString(string candidate)
		{
			foreach (HttpVerb thisVerb in Enum.GetValues(typeof(HttpVerb)))
			{
				if (candidate.ToUpper().Equals(thisVerb.ToString().ToUpper())) return thisVerb;
			}

			return HttpVerb.GET;
		}

	public const string PROTOCOL = "HTTP/1.1";
	
	// Server is here because this is where the headers are parsed and we need a place to put it.
	// It is only accessible from response, not from request.
	// If determined to be a request while reading from input stream, a Server header will be added.
	protected string server = "Rockabilly Common HTTP Server";
	protected bool skipReadingHeaders = false;


	public HttpPayload<T> payload = null;

	public abstract void SendToOutgoingStream(BinaryWriter outputStream);



	internal void PopulateFromInputStream(BufferedStream inputStream) {
		// This assumes we've read in far enough to begin reading the headers
		if (skipReadingHeaders) {
			server = "UNKNOWN";
		} else {
			server = PopulateHeadersFromInputStream(inputStream);

// Skip the line after the headers
Foundation.ReadLineFromInputStream(inputStream);
		}
		
		if (this is HttpRequest) {
	headers.Add(new HttpHeader(HttpHeader.SERVER_HEADER_KEY, server));
}
		
		try {
	if (httpContent.IsMultipart)
	{
		payload = new HttpMultipartPayload();
		payload.PopulateFromInputStream(inputStream, httpContent.MultipartBoundary);
	}
	else if (httpContent.IsText)
	{
		payload = new HttpStringPayload();
		payload.PopulateFromInputStream(inputStream, "");
	}
	else { //Assuming binary
		payload = new HttpBinaryPayload();
		payload.PopulateFromInputStream(inputStream, "");
	}
} catch (IOException rethrownException) {
	throw rethrownException;
} catch (Exception causalException) {
	throw new HttpMessageParseException(causalException);
}
}

	public override string ToString()
{
	StringBuilder result = new StringBuilder(base.ToString());

			result.Append(Symbols.Divider());
			result.Append(Symbols.CarriageReturnLineFeed);
	result.Append(Symbols.CarriageReturnLineFeed);

	result.Append(payload.ToString());

	return result.ToString();
}
}
}
