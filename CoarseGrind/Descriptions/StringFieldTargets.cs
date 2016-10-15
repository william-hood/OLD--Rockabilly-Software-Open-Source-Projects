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
namespace Rockabilly.CoarseGrind.Descriptions
{
	public enum StringFieldTargets
	{
		DEFAULT, NULL, HAPPY_PATH, EXPLICIT, EMPTY_STRING, EXCESSIVELY_LONG, PADDED_WHITESPACE, PRECEDING_WHITESPACE, TRAILING_WHITESPACE, ALL_WHITESPACE, CONTAINS_NEWLINE, CONTAINS_ASIAN_CHARACTERS, CONTAINS_NUMERIC_CHARACTERS, CONTAINS_PERIOD, CONTAINS_BACKTICK, CONTAINS_TILDE, CONTAINS_EXCLAMATION_POINT, CONTAINS_AT_SYMBOL, CONTAINS_HASH_SYMBOL, CONTAINS_DOLLAR_SIGN, CONTAINS_PERCENT_SIGN, CONTAINS_CARET_SYMBOL, CONTAINS_AMPERSAND, CONTAINS_STAR, CONTAINS_OPEN_PARENTHESIS, CONTAINS_CLOSE_PARENTHESIS, CONTAINS_OPEN_BRACE, CONTAINS_CLOSE_BRACE, CONTAINS_OPEN_BRACKET, CONTAINS_CLOSE_BRACKET, CONTAINS_DASH, CONTAINS_UNDERSCORE, CONTAINS_EQUALS_SIGN, CONTAINS_PLUS_SIGN, CONTAINS_PIPE_SYMBOL, CONTAINS_BACKSLASH, CONTAINS_FORWARD_SLASH, CONTAINS_LESS_THAN_SIGN, CONTAINS_GREATER_THAN_SIGN, CONTAINS_COLON, CONTAINS_SEMICOLON, CONTAINS_COMMA, CONTAINS_QUESTION_MARK, CONTAINS_BELL_CHARACTER, CONTAINS_TAB_CHARACTER, CONTAINS_CARRIAGE_RETURN_CHARACTER, CONTAINS_LINE_FEED_CHARACTER, CONTAINS_FORM_FEED_CHARACTER, CONTAINS_SPACE, CONTAINS_SINGLE_QUOTE, ENCLOSED_SINGLE_QUOTES, CONTAINS_DOUBLE_QUOTE, ENCLOSED_DOUBLE_QUOTES, ENCLOSED_PARENTHESES, ENCLOSED_BRACKETS, ENCLOSED_BRACES, ENCLOSED_POINTY_BRACKETS, CONTAINS_SQL, ONLY_SQL, CONTAINS_HTML, ONLY_HTML, CONTAINS_XML, ONLY_XML, CONTAINS_JAVASCRIPT, ONLY_JAVASCRIPT

	}

	public static class StringFieldTargets_Extensions
	{
		public static string ToString(this StringFieldTargets it)
		{
			switch (it)
			{
				case StringFieldTargets.ALL_WHITESPACE:
					return "All Whitespace";
				case StringFieldTargets.CONTAINS_AMPERSAND:
					return "Contains Ampersand";
				case StringFieldTargets.CONTAINS_ASIAN_CHARACTERS:
					return "Contains Asian Characters";
				case StringFieldTargets.CONTAINS_AT_SYMBOL:
					return "Contains At Symbol";
				case StringFieldTargets.CONTAINS_BACKSLASH:
					return "Contains Backslash Character";
				case StringFieldTargets.CONTAINS_BACKTICK:
					return "Contains Backtick Character";
				case StringFieldTargets.CONTAINS_BELL_CHARACTER:
					return "Contains Bell Character";
				case StringFieldTargets.CONTAINS_CARET_SYMBOL:
					return "Contains Caret Symbol";
				case StringFieldTargets.CONTAINS_CLOSE_BRACE:
					return "Contains Closing Brace";
				case StringFieldTargets.CONTAINS_CLOSE_BRACKET:
					return "Contains Closing Bracket";
				case StringFieldTargets.CONTAINS_CLOSE_PARENTHESIS:
					return "Contains Closing Parenthesis";
				case StringFieldTargets.CONTAINS_COLON:
					return "Contains Colon";
				case StringFieldTargets.CONTAINS_COMMA:
					return "Contains Comma";
				case StringFieldTargets.CONTAINS_DASH:
					return "Contains Dash";
				case StringFieldTargets.CONTAINS_DOLLAR_SIGN:
					return "Contains Dollar Sign";
				case StringFieldTargets.CONTAINS_EQUALS_SIGN:
					return "Contains Equals Sign";
				case StringFieldTargets.CONTAINS_EXCLAMATION_POINT:
					return "Contains Exclamation Point";
				case StringFieldTargets.CONTAINS_FORWARD_SLASH:
					return "Contains Forward Slash Character";
				case StringFieldTargets.CONTAINS_GREATER_THAN_SIGN:
					return "Contains Greater Than Sign";
				case StringFieldTargets.CONTAINS_HASH_SYMBOL:
					return "Contains Hash Symbol";
				case StringFieldTargets.CONTAINS_HTML:
					return "Contains Inserted HTML";
				case StringFieldTargets.CONTAINS_JAVASCRIPT:
					return "Contains Inserted JavaScript";
				case StringFieldTargets.CONTAINS_LESS_THAN_SIGN:
					return "Contains Less Than Sign";
				case StringFieldTargets.CONTAINS_CARRIAGE_RETURN_CHARACTER:
					return "Contains Carriage Return Character";
				case StringFieldTargets.CONTAINS_LINE_FEED_CHARACTER:
					return "Contains Line Feed Character";
				case StringFieldTargets.CONTAINS_FORM_FEED_CHARACTER:
					return "Contains Form Feed Character";
				case StringFieldTargets.CONTAINS_SPACE:
					return "Contains Inserted Space";
				case StringFieldTargets.CONTAINS_NEWLINE:
					return "Contains Newline Character";
				case StringFieldTargets.CONTAINS_NUMERIC_CHARACTERS:
					return "Contains Numeric Characters";
				case StringFieldTargets.CONTAINS_OPEN_BRACE:
					return "Contains Opening Brace";
				case StringFieldTargets.CONTAINS_OPEN_BRACKET:
					return "Contains Opening Bracket";
				case StringFieldTargets.CONTAINS_OPEN_PARENTHESIS:
					return "Contains Opening Parenthesis";
				case StringFieldTargets.CONTAINS_PERCENT_SIGN:
					return "Contains Percent Sign";
				case StringFieldTargets.CONTAINS_PERIOD:
					return "Contains Period";
				case StringFieldTargets.CONTAINS_PIPE_SYMBOL:
					return "Contains Pipe Symbol";
				case StringFieldTargets.CONTAINS_PLUS_SIGN:
					return "Contains Plus Sign";
				case StringFieldTargets.CONTAINS_QUESTION_MARK:
					return "Contains Question Mark";
				case StringFieldTargets.CONTAINS_SEMICOLON:
					return "Contains Semicolon";
				case StringFieldTargets.CONTAINS_SINGLE_QUOTE:
					return "Contains Single Quote";
				case StringFieldTargets.CONTAINS_DOUBLE_QUOTE:
					return "Contains Double Quote";
				case StringFieldTargets.CONTAINS_SQL:
					return "Contains Inserted SQL";
				case StringFieldTargets.CONTAINS_STAR:
					return "Contains Star Character";
				case StringFieldTargets.CONTAINS_TAB_CHARACTER:
					return "Contains Tab Character";
				case StringFieldTargets.CONTAINS_TILDE:
					return "Contains Tilde";
				case StringFieldTargets.CONTAINS_UNDERSCORE:
					return "Contains Underscore";
				case StringFieldTargets.CONTAINS_XML:
					return "Contains Inserted XML";
				case StringFieldTargets.EMPTY_STRING:
					return "Empty String";
				case StringFieldTargets.ENCLOSED_BRACES:
					return "Enclosed in Braces";
				case StringFieldTargets.ENCLOSED_BRACKETS:
					return "Enclosed in Brackets";
				case StringFieldTargets.ENCLOSED_DOUBLE_QUOTES:
					return "Enclosed in Double Quotes";
				case StringFieldTargets.ENCLOSED_PARENTHESES:
					return "Enclosed in Parentheses";
				case StringFieldTargets.ENCLOSED_POINTY_BRACKETS:
					return "Enclosed in Pointy Brackets";
				case StringFieldTargets.ENCLOSED_SINGLE_QUOTES:
					return "Enclosed in Single Quotes";
				case StringFieldTargets.EXCESSIVELY_LONG:
					return "Excessively Long";
				case StringFieldTargets.EXPLICIT:
					return "Explicit Value";
				case StringFieldTargets.HAPPY_PATH:
					return "Happy Path";
				case StringFieldTargets.NULL:
					return "Explicit Null";
				case StringFieldTargets.ONLY_HTML:
					return "Purely HTML";
				case StringFieldTargets.ONLY_JAVASCRIPT:
					return "Purely JavaScript";
				case StringFieldTargets.ONLY_SQL:
					return "Purely SQL";
				case StringFieldTargets.ONLY_XML:
					return "Purely XML";
				case StringFieldTargets.PADDED_WHITESPACE:
					return "Padded Whitespace Both Sides";
				case StringFieldTargets.PRECEDING_WHITESPACE:
					return "Has Preceding Whitespace";
				case StringFieldTargets.TRAILING_WHITESPACE:
					return "Has Trailing Whitespace";
			}

			return "Left Default";
		}

		public static bool IsHappyOrExplicit(this StringFieldTargets it)
		{
			if (it == StringFieldTargets.HAPPY_PATH)
				return true;
			if (it == StringFieldTargets.EXPLICIT)
				return true;
			return false;
		}

		public static bool IsEmpty(this StringFieldTargets it)
		{
			if (it == StringFieldTargets.ALL_WHITESPACE)
				return true;
			if (it == StringFieldTargets.EMPTY_STRING)
				return true;
			if (it == StringFieldTargets.NULL)
				return true;
			if (it == StringFieldTargets.DEFAULT)
				return true;
			return false;
		}

		public static bool IsWhitespaceCase(this StringFieldTargets it)
		{
			if (it == StringFieldTargets.ALL_WHITESPACE)
				return true;
			if (it == StringFieldTargets.PADDED_WHITESPACE)
				return true;
			if (it == StringFieldTargets.PRECEDING_WHITESPACE)
				return true;
			if (it == StringFieldTargets.TRAILING_WHITESPACE)
				return true;
			if (it == StringFieldTargets.CONTAINS_SPACE)
				return true;
			return false;
		}

		public static bool IsScriptAttack(this StringFieldTargets it)
		{
			if (it == StringFieldTargets.CONTAINS_SQL)
				return true;
			if (it == StringFieldTargets.ONLY_SQL)
				return true;
			if (it == StringFieldTargets.CONTAINS_HTML)
				return true;
			if (it == StringFieldTargets.ONLY_HTML)
				return true;
			if (it == StringFieldTargets.CONTAINS_XML)
				return true;
			if (it == StringFieldTargets.ONLY_XML)
				return true;
			if (it == StringFieldTargets.CONTAINS_JAVASCRIPT)
				return true;
			if (it == StringFieldTargets.ONLY_JAVASCRIPT)
				return true;
			return false;
		}

		public static bool IsNonstandardCharacterCase(this StringFieldTargets it)
		{
			switch (it)
			{
				case StringFieldTargets.CONTAINS_AMPERSAND:
				case StringFieldTargets.CONTAINS_ASIAN_CHARACTERS:
				case StringFieldTargets.CONTAINS_AT_SYMBOL:
				case StringFieldTargets.CONTAINS_BACKSLASH:
				case StringFieldTargets.CONTAINS_BACKTICK:
				case StringFieldTargets.CONTAINS_BELL_CHARACTER:
				case StringFieldTargets.CONTAINS_CARET_SYMBOL:
				case StringFieldTargets.CONTAINS_CLOSE_BRACE:
				case StringFieldTargets.CONTAINS_CLOSE_BRACKET:
				case StringFieldTargets.CONTAINS_CLOSE_PARENTHESIS:
				case StringFieldTargets.CONTAINS_COLON:
				case StringFieldTargets.CONTAINS_COMMA:
				case StringFieldTargets.CONTAINS_DASH:
				case StringFieldTargets.CONTAINS_DOLLAR_SIGN:
				case StringFieldTargets.CONTAINS_EQUALS_SIGN:
				case StringFieldTargets.CONTAINS_EXCLAMATION_POINT:
				case StringFieldTargets.CONTAINS_FORWARD_SLASH:
				case StringFieldTargets.CONTAINS_GREATER_THAN_SIGN:
				case StringFieldTargets.CONTAINS_HASH_SYMBOL:
				case StringFieldTargets.CONTAINS_LESS_THAN_SIGN:
				case StringFieldTargets.CONTAINS_CARRIAGE_RETURN_CHARACTER:
				case StringFieldTargets.CONTAINS_LINE_FEED_CHARACTER:
				case StringFieldTargets.CONTAINS_FORM_FEED_CHARACTER:
				case StringFieldTargets.CONTAINS_SPACE:
				case StringFieldTargets.CONTAINS_NEWLINE:
				case StringFieldTargets.CONTAINS_NUMERIC_CHARACTERS:
				case StringFieldTargets.CONTAINS_OPEN_BRACE:
				case StringFieldTargets.CONTAINS_OPEN_BRACKET:
				case StringFieldTargets.CONTAINS_OPEN_PARENTHESIS:
				case StringFieldTargets.CONTAINS_PERCENT_SIGN:
				case StringFieldTargets.CONTAINS_PERIOD:
				case StringFieldTargets.CONTAINS_PIPE_SYMBOL:
				case StringFieldTargets.CONTAINS_PLUS_SIGN:
				case StringFieldTargets.CONTAINS_QUESTION_MARK:
				case StringFieldTargets.CONTAINS_SEMICOLON:
				case StringFieldTargets.CONTAINS_SINGLE_QUOTE:
				case StringFieldTargets.CONTAINS_DOUBLE_QUOTE:
				case StringFieldTargets.CONTAINS_STAR:
				case StringFieldTargets.CONTAINS_TAB_CHARACTER:
				case StringFieldTargets.CONTAINS_TILDE:
				case StringFieldTargets.CONTAINS_UNDERSCORE:
				case StringFieldTargets.ENCLOSED_BRACES:
				case StringFieldTargets.ENCLOSED_BRACKETS:
				case StringFieldTargets.ENCLOSED_DOUBLE_QUOTES:
				case StringFieldTargets.ENCLOSED_PARENTHESES:
				case StringFieldTargets.ENCLOSED_POINTY_BRACKETS:
				case StringFieldTargets.ENCLOSED_SINGLE_QUOTES:
					return true;
			}
			return false;
		}
	}
}
