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
namespace Rockabilly.Common
{
	public static class Symbols
	{
		public static readonly string PlatformNewLine = Environment.NewLine;
		public const string CarriageReturnLineFeed = "\r\n";
		public const char Copyright = '\u00a9';
		public const char RegisteredTM = '\u00ae';
		public const char Yen = '\u00a5';
		public const char Pound = '\u00a3';
		public const char Cent = '\u00a2';
		public const char Dollar = '$';
		public const char Paragraph = '\u00b6';
		public const char OpenAngleQuote = '\u00ab';
		public const char CloseAngleQuote = '\u00bb';
		public const char Degree = '\u00b0';
		public const char Space = ' ';

		const int DEFAULT_DIVIDER_LENGTH = 79;
		const char SINGLE_DIVIDER = '_';
		const char DOUBLE_DIVIDER = '=';

		public static string Divider(DividerTypes typeToUse = DividerTypes.SINGLE, int length = DEFAULT_DIVIDER_LENGTH)
		{
			char dividerChar = DOUBLE_DIVIDER;
			string dividerString = "";

			if (typeToUse == DividerTypes.SINGLE)
			{
				dividerChar = SINGLE_DIVIDER;
			}

			for (int cursor = 0; cursor < length; cursor++)
			{
				dividerString += dividerChar;
			}

			return dividerString;
		}

		public const char ERASER = '\u0008';

	}
}
