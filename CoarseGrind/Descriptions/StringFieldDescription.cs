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
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Descriptions
{
	public class StringFieldDescription : FieldDescription<string>
	{
		public StringFieldTargets Target = StringFieldTargets.HAPPY_PATH;
		private const string HTML_HACK = "<html><head><title>TITLE</title></head><body>Howdy</body></html>";
		private const string SQL_HACK = "SELECT 'Hello, World!';";
		private const string JAVASCRIPT_HACK = "function displayDate(){document.getElementById(\"demo\").innerHTML=Date();}";
		private const string XML_HACK = "<?xml version=\"1.0\"?><soap:Envelope xmlns:soap=\"http://www.w3.org/2001/12/soap-envelope\" soap:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding\"><soap:Body xmlns:m=\"http://www.example.org/stock\"><m:GetStockPrice><m:StockName>IBM</m:StockName></m:GetStockPrice></soap:Body></soap:Envelope>";

		public StringFieldDescription(string basisValue = default(string)) : base()
		{
			BasisValue = basisValue;
		}

		public override bool HasSpecificHappyValue
		{
			get
			{
				return ((Target == StringFieldTargets.HAPPY_PATH) && BasisValue != null);
			}
		}

		public override bool IsExplicit
		{
			get
			{
				return Target == StringFieldTargets.EXPLICIT;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return Target == StringFieldTargets.DEFAULT;
			}
		}

		private int MiddleIndex
		{
			get
			{
				return (int)Math.Round((decimal)(BasisValue.Length / 2));
			}
		}

		private string FirstHalf
		{
			get
			{
				return BasisValue.Substring(0, MiddleIndex);
			}
		}

		private string SecondHalf
		{
			get
			{
				return BasisValue.Substring(MiddleIndex + 1);
			}
		}

		public override string DescribedValue
		{
			get
			{
				switch (Target)
				{
					case StringFieldTargets.DEFAULT:
						throw new NoValueException();
					case StringFieldTargets.HAPPY_PATH:
					case StringFieldTargets.EXPLICIT:
						if (BasisValue == null)
							throw new InappropriateDescriptionException();
						return BasisValue;
					case StringFieldTargets.ALL_WHITESPACE:
						return "             ";
					case StringFieldTargets.CONTAINS_AMPERSAND:
						return FirstHalf + "&" + SecondHalf;
					case StringFieldTargets.CONTAINS_ASIAN_CHARACTERS:
						return FirstHalf + "\u60a8\u597d\u5927\u5bb6" /*"您好大家"*/ + SecondHalf;
					case StringFieldTargets.CONTAINS_AT_SYMBOL:
						return FirstHalf + "@" + SecondHalf;
					case StringFieldTargets.CONTAINS_BACKSLASH:
						return FirstHalf + "\\" + SecondHalf;
					case StringFieldTargets.CONTAINS_BACKTICK:
						return FirstHalf + "`" + SecondHalf;
					case StringFieldTargets.CONTAINS_BELL_CHARACTER:
						return FirstHalf + "\u2407" + SecondHalf;
					case StringFieldTargets.CONTAINS_CARET_SYMBOL:
						return FirstHalf + "^" + SecondHalf;
					case StringFieldTargets.CONTAINS_CLOSE_BRACE:
						return FirstHalf + "}" + SecondHalf;
					case StringFieldTargets.CONTAINS_CLOSE_BRACKET:
						return FirstHalf + "]" + SecondHalf;
					case StringFieldTargets.CONTAINS_CLOSE_PARENTHESIS:
						return FirstHalf + ")" + SecondHalf;
					case StringFieldTargets.CONTAINS_COLON:
						return FirstHalf + ":" + SecondHalf;
					case StringFieldTargets.CONTAINS_COMMA:
						return FirstHalf + "," + SecondHalf;
					case StringFieldTargets.CONTAINS_DASH:
						return FirstHalf + "-" + SecondHalf;
					case StringFieldTargets.CONTAINS_DOLLAR_SIGN:
						return FirstHalf + "$" + SecondHalf;
					case StringFieldTargets.CONTAINS_EQUALS_SIGN:
						return FirstHalf + "=" + SecondHalf;
					case StringFieldTargets.CONTAINS_EXCLAMATION_POINT:
						return FirstHalf + "!" + SecondHalf;
					case StringFieldTargets.CONTAINS_FORWARD_SLASH:
						return FirstHalf + "/" + SecondHalf;
					case StringFieldTargets.CONTAINS_GREATER_THAN_SIGN:
						return FirstHalf + ">" + SecondHalf;
					case StringFieldTargets.CONTAINS_HASH_SYMBOL:
						return FirstHalf + "#" + SecondHalf;
					case StringFieldTargets.CONTAINS_HTML:
						return FirstHalf + " " + HTML_HACK + " " + SecondHalf;
					case StringFieldTargets.CONTAINS_JAVASCRIPT:
						return FirstHalf + " " + JAVASCRIPT_HACK + " " + SecondHalf;
					case StringFieldTargets.CONTAINS_LESS_THAN_SIGN:
						return FirstHalf + "<" + SecondHalf;
					case StringFieldTargets.CONTAINS_CARRIAGE_RETURN_CHARACTER:
						return FirstHalf + "\r" + SecondHalf;
					case StringFieldTargets.CONTAINS_LINE_FEED_CHARACTER:
						return FirstHalf + "\n" + SecondHalf;
					case StringFieldTargets.CONTAINS_FORM_FEED_CHARACTER:
						return FirstHalf + "\f" + SecondHalf;
					case StringFieldTargets.CONTAINS_SPACE:
						return FirstHalf + " " + SecondHalf;
					case StringFieldTargets.CONTAINS_NEWLINE:
						return FirstHalf + Symbols.CarriageReturnLineFeed + SecondHalf;
					case StringFieldTargets.CONTAINS_NUMERIC_CHARACTERS:
						return FirstHalf + "1234567890" + SecondHalf;
					case StringFieldTargets.CONTAINS_OPEN_BRACE:
						return FirstHalf + "{" + SecondHalf;
					case StringFieldTargets.CONTAINS_OPEN_BRACKET:
						return FirstHalf + "[" + SecondHalf;
					case StringFieldTargets.CONTAINS_OPEN_PARENTHESIS:
						return FirstHalf + "(" + SecondHalf;
					case StringFieldTargets.CONTAINS_PERCENT_SIGN:
						return FirstHalf + "%" + SecondHalf;
					case StringFieldTargets.CONTAINS_PERIOD:
						return FirstHalf + "." + SecondHalf;
					case StringFieldTargets.CONTAINS_PIPE_SYMBOL:
						return FirstHalf + "|" + SecondHalf;
					case StringFieldTargets.CONTAINS_PLUS_SIGN:
						return FirstHalf + "+" + SecondHalf;
					case StringFieldTargets.CONTAINS_QUESTION_MARK:
						return FirstHalf + "?" + SecondHalf;
					case StringFieldTargets.CONTAINS_SEMICOLON:
						return FirstHalf + ";" + SecondHalf;
					case StringFieldTargets.CONTAINS_SINGLE_QUOTE:
						return FirstHalf + "'" + SecondHalf;
					case StringFieldTargets.CONTAINS_DOUBLE_QUOTE:
						return FirstHalf + "\"" + SecondHalf;
					case StringFieldTargets.CONTAINS_SQL:
						return FirstHalf + " " + SQL_HACK + " " + SecondHalf;
					case StringFieldTargets.CONTAINS_STAR:
						return FirstHalf + "*" + SecondHalf;
					case StringFieldTargets.CONTAINS_TAB_CHARACTER:
						return FirstHalf + "\t" + SecondHalf;
					case StringFieldTargets.CONTAINS_TILDE:
						return FirstHalf + "~" + SecondHalf;
					case StringFieldTargets.CONTAINS_UNDERSCORE:
						return FirstHalf + "_" + SecondHalf;
					case StringFieldTargets.CONTAINS_XML:
						return FirstHalf + " " + XML_HACK + " " + SecondHalf;
					case StringFieldTargets.EMPTY_STRING:
						return "";
					case StringFieldTargets.ENCLOSED_BRACES:
						return "{" + BasisValue + "}";
					case StringFieldTargets.ENCLOSED_BRACKETS:
						return "[" + BasisValue + "]";
					case StringFieldTargets.ENCLOSED_DOUBLE_QUOTES:
						return "\"" + BasisValue + "\"";
					case StringFieldTargets.ENCLOSED_PARENTHESES:
						return "(" + BasisValue + ")";
					case StringFieldTargets.ENCLOSED_POINTY_BRACKETS:
						return "<" + BasisValue + ">";
					case StringFieldTargets.ENCLOSED_SINGLE_QUOTES:
						return "'" + BasisValue + "'";
					case StringFieldTargets.EXCESSIVELY_LONG:
						return "fdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjffdagshjafguyagyuwefuyabvubmcxvvFHWGYDISFIDFI4637285462850qzmglpbotrekwisirutjf";
					case StringFieldTargets.NULL:
						return null;
					case StringFieldTargets.ONLY_HTML:
						return HTML_HACK;
					case StringFieldTargets.ONLY_JAVASCRIPT:
						return JAVASCRIPT_HACK;
					case StringFieldTargets.ONLY_SQL:
						return SQL_HACK;
					case StringFieldTargets.ONLY_XML:
						return XML_HACK;
					case StringFieldTargets.PADDED_WHITESPACE:
						return "     " + BasisValue + "      ";
					case StringFieldTargets.PRECEDING_WHITESPACE:
						return "     " + BasisValue;
					case StringFieldTargets.TRAILING_WHITESPACE:
						return BasisValue + "      ";
				}

				return null;
			}
		}

		public override string ToString()
		{
			if (Target == StringFieldTargets.EXPLICIT)
				return Target.ToString() + " (" + BasisValue + ")";
			return Target.ToString();
		}

		public override void SetToExplicitValue(string value)
		{
			BasisValue = value;
			Target = StringFieldTargets.EXPLICIT;
		}
	}
}
