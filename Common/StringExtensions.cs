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
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Security.Policy;

namespace Rockabilly.Common
{
	public static class StringExtensions
	{
		// From http://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
		public static string Reverse(this string text)
		{
			char[] cArray = text.ToCharArray();
			string reverse = String.Empty;
			for (int i = cArray.Length - 1; i > -1; i--)
			{
				reverse += cArray[i];
			}
			return reverse;
		}


		public static bool IsBlank(this string candidate)
		{
			if (candidate == null) return true;
			if (candidate == String.Empty) return true;
			if (candidate == "") return true;
			if (candidate.Length < 1) return true;
			return false;
		}

		public static bool Matches(this string thisString, string theOther)
		{
			if ((thisString == null) && (theOther == null)) return true;
			if ((thisString == null) && (theOther != null)) return false;
			if ((thisString != null) && (theOther == null)) return false;
			return thisString.CompareTo(theOther) == 0;
		}

		public static bool MatchesCaseInspecific(this string thisString, string theOther)
		{
			if ((thisString == null) && (theOther == null)) return true;
			if ((thisString == null) && (theOther != null)) return false;
			if ((thisString != null) && (theOther == null)) return false;
			return thisString.ToUpper().CompareTo(theOther.ToUpper()) == 0;
		}

		public static string Justify(this string message, int columns = 75)
		{
			StringBuilder justifiedMessage = new StringBuilder();

			// Before splitting the message into words, illegal characters must be
			// stripped out.
			string[] strippedMessageArray = message.Split("\n\t\r\f\b\v\a".ToCharArray());
			StringBuilder strippedMessage = new StringBuilder();
			for (int cursor = 0; cursor < strippedMessageArray.Length; cursor++)
			{
				if (strippedMessageArray[cursor].Length > 0)
				{
					if (!strippedMessageArray[cursor].EndsWith(" "))
					{
						strippedMessageArray[cursor] += ' ';
					}
				}

				strippedMessage.Append(strippedMessageArray[cursor]);
			}

			string[] parsedMessage = strippedMessage.ToString().Split(" ".ToCharArray());
			int columnIndex = 0;

			for (int cursor = 0; cursor < parsedMessage.Length; cursor++)
			{
				if (parsedMessage[cursor].Length >= columns)
				{
					parsedMessage[cursor] = parsedMessage[cursor].Substring(0, columns - 1);
				}

				parsedMessage[cursor] += ' ';
				if ((columnIndex + parsedMessage[cursor].Length) > columns)
				{
					// This word will exceed the number of columns. Add a line break
					// before the word.
					justifiedMessage.Append(Symbols.CarriageReturnLineFeed);
					columnIndex = 0;
				}

				justifiedMessage.Append(parsedMessage[cursor]);
				columnIndex += parsedMessage[cursor].Length;
			}

			return justifiedMessage.ToString();
		}

		public static string PrependEveryLineWith(this string multiLineMessage, string prependString)
		{
			return prependString + multiLineMessage.Replace(Symbols.PlatformNewLine, Symbols.PlatformNewLine + prependString);
		}

		public static string IndentEveryLineBy(this string multiLineMessage, int indentSize)
		{
			return multiLineMessage.PrependEveryLineWith(new string(' ', indentSize));
		}

		private enum PaddingSide
		{
			LEFT, RIGHT, BOTH
		};

		private static string PadEither(string input, int totalWidth, char paddingChar, PaddingSide which)
		{
			StringBuilder result = new StringBuilder();

			result.Append(input);
			while (result.Length < totalWidth)
			{
				if ((which == PaddingSide.RIGHT) || (which == PaddingSide.BOTH))
				{
					if (result.Length < totalWidth) result.Append(paddingChar);
				}

				if ((which == PaddingSide.LEFT) || (which == PaddingSide.BOTH))
				{
					if (result.Length < totalWidth) result.Insert(0, paddingChar);
				}
			}

			return result.ToString();
		}

		public static string PadSides(this string originalString, int totalSize, char paddingChar = ' ')
		{
			return PadEither(originalString, totalSize, paddingChar, PaddingSide.BOTH);
		}

		public static string PadLeft(this string originalString, int totalSize, char paddingChar = ' ')
		{
			return PadEither(originalString, totalSize, paddingChar, PaddingSide.LEFT);
		}

		public static string PadRight(this string originalString, int totalSize, char paddingChar = ' ')
		{
			return PadEither(originalString, totalSize, paddingChar, PaddingSide.RIGHT);
		}

		public static string Repeat(this string target, int count)
		{
			StringBuilder result = new StringBuilder();
			for (int index = 0; index < count; index++)
			{
				result.Append(target);
			}

			return result.ToString();
		}

		public static string MakeParenthetic(this string target)
		{
			return "(" + target + ")";
		}

		public static string MakeQuoted(this string target)
		{
			return "\"" + target + "\"";
		}

		public static string MakeSingleQuoted(this string target)
		{
			return "'" + target + "'";
		}

		public static string FilterCarriageReturns(this string message)
		{
			return message.Replace("\r", "");
		}

		public static string PadVertical(this string message, int totalRows, VerticalJustification justification = VerticalJustification.CENTER, bool addHorizontalSpaces = false)
		{
			string paddedMessage = message;

			message = message.FilterCarriageReturns();
			string[] splitMessage = message.Split('\n');

			if (totalRows <= splitMessage.Length)
			{
				return paddedMessage;
			}

			string emptyLine = Symbols.CarriageReturnLineFeed;
			if (addHorizontalSpaces)
			{
				int maxHorz = 0;
				for (int cursor = 0; cursor < splitMessage.Length; cursor++)
				{
					if (splitMessage[cursor].Length > maxHorz)
					{
						maxHorz = splitMessage[cursor].Length;
					}
				}

				emptyLine = new String(' ', maxHorz) + Symbols.CarriageReturnLineFeed;
			}

			int topPadding = (totalRows - splitMessage.Length) / 2;
			paddedMessage = "";

			switch (justification)
			{
				case VerticalJustification.BOTTOM:
					topPadding = totalRows - splitMessage.Length;
					break;
				case VerticalJustification.TOP:
					topPadding = 0;
					break;
				default:
					topPadding = (totalRows - splitMessage.Length) / 2;
					break;
			}

			int pads = topPadding;

			for (int cursor = 0; cursor < pads; cursor++)
			{
				paddedMessage += emptyLine;
			}

			for (int cursor = 0; cursor < splitMessage.Length; cursor++)
			{
				paddedMessage += splitMessage[cursor] + Symbols.CarriageReturnLineFeed;
				pads++;
			}

			while (pads < totalRows)
			{
				paddedMessage += emptyLine;
				pads++;
			}
			// Deliberate
			// for (pads = pads; pads < totalRows; pads++)
			// {
			// paddedMessage += emptyLine;
			// }

			return paddedMessage;
		}

		//use regex
		// FIXED
		private const string REPLACER = "\u25a2";
		public static string FilterOutNonPrintables(this string candidate)
		{
			return Regex.Replace(candidate, @"[^\u0000-\u007F]+", REPLACER);
		}


		public static string ShortFileName(this string completeFilePath)
		{
			string baseExt = completeFilePath.Substring(completeFilePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			if (completeFilePath.Contains("."))
			{
				return baseExt.Substring(0, baseExt.LastIndexOf('.'));
			}

			return baseExt;
		}

		public static bool ContainsMatch(this List<string> checkList, string candidate)
		{
			foreach (string thisString in checkList)
			{
				if (thisString.Matches(candidate)) return true;
			}

			return false;
		}

		public static bool ContainsMatchCaseInspecific(this List<string> checkList, string candidate)
		{
			foreach (string thisString in checkList)
			{
				if (thisString.MatchesCaseInspecific(candidate)) return true;
			}

			return false;
		}

		public static bool ContainsMatch(this string[] checkList, string candidate)
		{
			return new List<string>(checkList).ContainsMatch(candidate);
		}

		public static bool ContainsMatchCaseInspecific(this string[] checkList, string candidate)
		{
			return new List<string>(checkList).ContainsMatchCaseInspecific(candidate);
		}

		public static List<string> SortMarkupTags(this string target)
		{
			List<string> result = new List<string>();
			StringBuilder thisString = new StringBuilder();
			int levelsIn = 0;
			for (int index = 0; index < target.Length; index++)
			{
				char thisChar = target[index];
				if (levelsIn > 0)
				{
					if (thisChar == '<') { levelsIn++; }
					else if (thisChar == '>') { levelsIn--; }

					thisString.Append(thisChar);

					if (levelsIn == 0)
					{
						if (thisString.Length > 0)
						{
							result.Add(thisString.ToString());
							thisString = new StringBuilder();
						}
					}
				}
				else {
					if (thisChar == '<')
					{
						if (thisString.Length > 0)
						{
							result.Add(thisString.ToString());
							thisString = new StringBuilder();
						}
						levelsIn = 1;
					}

					thisString.Append(thisChar);
				}
			}

			if (thisString.Length > 0)
			{
				result.Add(thisString.ToString());
				thisString = new StringBuilder();
			}

			return result;
		}



		// Based on http://stackoverflow.com/questions/13592236/parse-a-uri-string-into-name-value-collection
		public static List<KeyValuePair<string, string>> GetUrlParameters(this string urlQuery)
		{
			List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
			string[] parameters = urlQuery.Split("?&".ToCharArray());

			foreach (string thisParam in parameters)
			{
				if (thisParam.Contains("="))
				{
					string[] keyValuePair = thisParam.Split('=');
					result.Add(new KeyValuePair<string, string>(keyValuePair[0], keyValuePair[1]));
				}
			}

			return result;
		}
	}
}
