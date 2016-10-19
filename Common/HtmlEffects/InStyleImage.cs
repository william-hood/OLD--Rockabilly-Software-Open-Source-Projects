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

namespace Rockabilly.Common.HtmlEffects
{

	// These need to be treated separately because they go in the style section.
	public abstract class InStyleImage : WebInterfaceControl, WebImage
	{
		public abstract string Base64ImageData { get; }
		public abstract string ImageType { get; }

		protected abstract string CssClassName { get; }

		public string ToCssClassString()
		{
			StringBuilder result = new StringBuilder("img.");
			result.Append(CssClassName);
			result.Append(" {");
			result.Append(Symbols.CarriageReturnLineFeed);
			result.Append("content:");
			result.Append(Symbols.CarriageReturnLineFeed);
			result.Append("url(data:img/");
			result.Append(ImageType);
			result.Append(";base64,");
			result.Append(Base64ImageData);
			result.Append(");");
			result.Append(Symbols.CarriageReturnLineFeed);
			result.Append("}");
			return result.ToString();
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<img class=\"");
			result.Append(CssClassName);
			result.Append("\">");
			return result.ToString();
		}
	}
}
