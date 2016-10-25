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
using System.Collections.Generic;
using Rockabilly.Common.HtmlEffects;
using System.Text.RegularExpressions;

namespace Rockabilly.Common
{
	public enum LoggingLevel
	{
		Debug = 0,
		Warning = 1,
		Standard = 2,
		Important = 3,
		Critical = 4

	}


	public enum LoggingMode
	{
		Exclusive, Minimum
	}

	public abstract class HtmlLogSystem
	{
		public abstract string GetHtmlLogHeader(string headline);

		public abstract InlineImage CriticalFailureIcon { get; }
		public abstract InlineImage PassIcon { get; }
		public abstract InlineImage FailIcon { get; }
		public abstract string HtmlStyle { get; }
		public abstract List<InlineImage> AllInlineImages { get; }

		public const int HTML_LOG_JUSTIFY_SIZE = 120;

		public static readonly string HTML__LINE_BREAK = new LineBreak().ToString() + Symbols.CarriageReturnLineFeed;
		public static readonly string HTML__DIVIDER = new Divider().ToString();
		public const string HTML__TABLE_BEGIN_DATA_CELL = "<td style=\"text-align:left\">";
		public const string HTML__TABLE_BEGIN_RIGHT_ALIGN_DATA_CELL = "<td style=\"text-align:right\">";
		public const string HTML__TABLE_BEGIN_WIDE_DATA_CELL = "<td style=\"text-align:left\" colspan=\"5\">";
		public const string HTML__TABLE_END_DATA_CELL = "</td>";
		public const string HTML__TABLE_DIVISION = HTML__TABLE_END_DATA_CELL + HTML__TABLE_BEGIN_DATA_CELL;
		public const string HTML__ROW_START = "<tr>";
		public const string HTML__ROW_END = "</tr>";
		public const string HTML__TABLE_START = "<table class=rockabilly_log_system>" + HTML__ROW_START;
		public const string HTML__EXCEPTION_TABLE_START = "<table class=rockabilly_logged_exception>";
		public static readonly string HTML__TABLE_END = HTML__ROW_END + "</table>" + HTML__LINE_BREAK;
		public const string HTML__EXCEPTION_TABLE_END = "</table>";
		public const string HTML__SPACE = "&nbsp;";



		private static string LogDateFormatstring = "[yyyy-MM-dd] [hh:mm:ss.ffff]";
		//private static SimpleDateFormat LogDateFormat = new SimpleDateFormat(LogDateFormatstring);

		public bool SHOW_TIME_STAMPS = true;

		private class output
		{
			internal readonly WebInterface OutputWebInterface = new WebInterface();
			public TextOutputManager OutputWriter;
			private LoggingLevel OutputLevel;
			private LoggingMode OutputMode;

			private Guid identifier = Guid.NewGuid();

			public Guid Identifier
			{
				get
				{
					return identifier;
				}
			}

			private output()
			{
			} // EXPLICTLY FORBID USE OF THE DEFAULT CONSTRUCTOR

			public output(HtmlLogSystem parent, string title, TextOutputManager newOutputWriter, LoggingLevel newLevel, LoggingMode newMode)
			{
				OutputWebInterface.Title = title;
				OutputWebInterface.CssStyles = parent.HtmlStyle;

				OutputWriter = newOutputWriter;
				OutputLevel = newLevel;
				OutputMode = newMode;

				OutputWriter.WriteLine(OutputWebInterface.HtmlHeader);
			}

			public bool Allows(LoggingLevel requestedLevel)
			{
				if (OutputMode == LoggingMode.Minimum)
					return requestedLevel >= OutputLevel;
				return requestedLevel == OutputLevel;
			}
		}

		private bool autoFlush = true;

		public void SetManualFlush()
		{
			autoFlush = false;
		}

		public void SetAutomaticFlush()
		{
			autoFlush = true;
		}

		private List<output> outputs = new List<output>();

		public Guid AddOutput(TextOutputManager newOutputWriter, LoggingLevel newLevel = LoggingLevel.Standard, LoggingMode newMode = LoggingMode.Minimum)
		{
			output newOutput = new output(this, Foundation.TimeStamp, newOutputWriter, newLevel, newMode);
			outputs.Add(newOutput);
			return newOutput.Identifier;
		}

		public Guid AddHeadlinedOutput(string headLine, TextOutputManager newOutputWriter, LoggingLevel newLevel = LoggingLevel.Standard, LoggingMode newMode = LoggingMode.Minimum)
		{
			Guid result = AddOutput(newOutputWriter, newLevel, newMode);
			Header(result, headLine);
			return result;
		}

		public Guid AddDefaultOutput()
		{
			return AddOutput(new TextOutputManager(), LoggingLevel.Standard, LoggingMode.Minimum);
		}

		public void ClearSpecificOutput(Guid specificOutputIdentifier)
		{
			bool notFound = true;
			Flush();

			foreach (output thisOutput in outputs)
			{
				if (thisOutput.Identifier == specificOutputIdentifier)
				{
					thisOutput.OutputWriter.WriteLine(HTML__TABLE_END);
					thisOutput.OutputWriter.WriteLine(thisOutput.OutputWebInterface.getHtmlFooter());
					thisOutput.OutputWriter.Close();
					thisOutput.OutputWriter = null;
					outputs.Remove(thisOutput);
					notFound = false;
					break;
				}
			}

			if (notFound)
				Message("LogSystem - NOTHING REMOVED:  Did not find output having Guid==" + specificOutputIdentifier, LoggingLevel.Critical, CriticalFailureIcon);

			GC.Collect();
		}

		public void ClearOutputs()
		{
			Flush();

			foreach (output thisOutput in outputs)
			{
				thisOutput.OutputWriter.Close();
				thisOutput.OutputWriter = null;
			}

			outputs.Clear();
			GC.Collect();
		}

		public void Header(Guid outputIdentifier, string headLine)
		{
			WriteLine(outputIdentifier, GetHtmlLogHeader(headLine), LoggingLevel.Critical);
		}

		public void Header(string headLine)
		{
			WriteLine(GetHtmlLogHeader(headLine), LoggingLevel.Critical);
		}

		protected void WriteLine(string newstring, LoggingLevel requestedLevel = LoggingLevel.Standard)
		{
			if (outputs.Count < 1)
				AddDefaultOutput();

			foreach (output thisOutput in outputs)
			{
				if (thisOutput.Allows(requestedLevel))
				{
					thisOutput.OutputWriter.WriteLine(newstring);
				}
			}

			if (autoFlush) Flush();
		}

		protected void WriteLine(Guid outputIdentifier, string newstring, LoggingLevel requestedLevel = LoggingLevel.Standard)
		{
			foreach (output thisOutput in outputs)
			{
				if (thisOutput.Identifier.Equals(outputIdentifier))
				{
					if (thisOutput.Allows(requestedLevel))
					{
						thisOutput.OutputWriter.WriteLine(newstring);
					}
				}
			}
		}

		public void SkipLine(LoggingLevel logLevel = LoggingLevel.Critical)
		{
			Message(HTML__SPACE, logLevel);
		}

		public void Flush()
		{
			if (outputs.Count < 1)
				AddDefaultOutput();

			foreach (output thisOutput in outputs)
			{
				thisOutput.OutputWriter.Flush();
			}
		}

		protected static string PlainTimeStamp
		{
			get
			{
				return DateTime.Now.ToString(LogDateFormatstring).Replace("[", "").Replace("]", "");
			}
		}

		protected static string HtmlTimeStamp
		{
			get
			{
				return DateTime.Now.ToString(LogDateFormatstring).Replace(" ", HTML__TABLE_DIVISION + HTML__TABLE_DIVISION).Replace("[", "<small>").Replace("]", "</small>");
			}
		}

		private string NormalLine(InlineImage icon, string message)
		{
			StringBuilder result = new StringBuilder(HTML__ROW_START + HTML__TABLE_BEGIN_DATA_CELL);
			if (SHOW_TIME_STAMPS)
			{
				result.Append(HTML__TABLE_BEGIN_DATA_CELL);
				result.Append(HtmlTimeStamp);
				result.Append(HTML__TABLE_END_DATA_CELL);
			}
			result.Append(HTML__TABLE_BEGIN_DATA_CELL);
			if (icon == null)
			{
				result.Append(HTML__SPACE);
			}
			else {
				result.Append(icon);
			}
			result.Append(HTML__TABLE_DIVISION);

			// DEBUG
			// List<string> messageSorted = Foundation.sortMarkupTags(message);

			// For loop below is to properly handle embedded HTML tags.
			foreach (string thisString in message.SortMarkupTags())
			{
				if (thisString.StartsWith("<"))
				{
					if (thisString.Length > 60)
					{
						result.Append(Symbols.CarriageReturnLineFeed + thisString + Symbols.CarriageReturnLineFeed);
					}
					else {
						result.Append(thisString);
					}
				}
				else {
					//use regex
					// FIXED
					result.Append(Regex.Replace(thisString.Justify(HTML_LOG_JUSTIFY_SIZE), Symbols.CarriageReturnLineFeed, "<br>" + Symbols.CarriageReturnLineFeed));
				}
			}

			result.Append(HTML__TABLE_END_DATA_CELL);
			result.Append(HTML__ROW_END);
			return result.ToString();
		}

		public void Message(string message, LoggingLevel requestedLevel = LoggingLevel.Standard, InlineImage icon = null)
		{
			WriteLine(NormalLine(icon, message), requestedLevel);
		}

		public bool ShowAssertion(string message, bool mustBeTrue, LoggingLevel requestedLevel = LoggingLevel.Critical)
		{
			if (mustBeTrue)
			{
				Message(message, requestedLevel, PassIcon);
			}
			else {
				Message(message, requestedLevel, FailIcon);
				Flush();
			}

			return mustBeTrue;
		}

		public void ShowException(Exception loggedException, LoggingLevel requestedLevel = LoggingLevel.Critical, InlineImage icon = null)
		{
			string currentLine = default(string);
			try
			{
				StringBuilder result = new StringBuilder(HTML__EXCEPTION_TABLE_START);
				string[] lines = Foundation.DepictException(loggedException).Replace("\r", "").Split('\n');

				for (int cursor = 0; cursor < lines.Length; cursor++)
				{
					currentLine = lines[cursor].Trim();

					if (currentLine != null)
					{
						if (currentLine.Trim().Length > 0)
						{
							result.Append(HTML__ROW_START);

							if (currentLine.StartsWith("at"))
							{
								string[] parts = Regex.Replace(currentLine.Trim().Replace("at ", ""), "([a-z]):", "").Replace("line", "").Replace(':', '|').Replace(" in ", "|").Split('|');

								if (parts.Length > 2)
								{
									result.Append(HTML__TABLE_BEGIN_DATA_CELL);
									result.Append("<small>");
									result.Append(HTML__SPACE);
									result.Append(HTML__SPACE);
									result.Append(parts[0].Trim());
									result.Append("()</small>");
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__TABLE_BEGIN_DATA_CELL);
									result.Append(HTML__SPACE);
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__TABLE_BEGIN_DATA_CELL);
									result.Append("<b>");
									result.Append(parts[1].Trim());
									result.Append("</b>");
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__TABLE_BEGIN_DATA_CELL);
									result.Append(HTML__SPACE);
									result.Append("<small><i>at line</i></small>");
									result.Append(HTML__SPACE);
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__TABLE_BEGIN_RIGHT_ALIGN_DATA_CELL);
									result.Append("<b>");
									result.Append(parts[2].Trim());
									result.Append(HTML__SPACE);
									result.Append(HTML__SPACE);
									result.Append("</b>");
								}
								else if (parts.Length > 1)
								{
									result.Append(HTML__TABLE_BEGIN_DATA_CELL);
									result.Append("<small>");
									result.Append(HTML__SPACE);
									result.Append(HTML__SPACE);
									result.Append(parts[0].Trim());
									result.Append("()</small>");
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__TABLE_BEGIN_DATA_CELL);
									result.Append(HTML__SPACE);
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
									result.Append("<b>");
									result.Append(parts[1].Trim());
									result.Append("</b>");
									result.Append(HTML__TABLE_END_DATA_CELL);
								}
								else {
									result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
									result.Append(singleFailureLine(currentLine.Replace("at ", HTML__SPACE + HTML__SPACE)));
								}

							}
							else if (currentLine.ToLower().StartsWith("inner"))
							{
								string[] parts = currentLine.Replace("Inner ", "").Split(':');

								if (parts.Length > 1)
								{
									result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
									result.Append(HTML__SPACE);
									result.Append(HTML__TABLE_END_DATA_CELL);
									result.Append(HTML__ROW_END);
									result.Append(HTML__ROW_START);
									result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
									result.Append("<small>...caused by...</small><center><b>");
									result.Append(parts[0].Trim());
									result.Append("</b>");
									if (parts.Length > 1)
									{
										result.Append("<br><small><i>");
										result.Append(parts[1].Trim());
										result.Append("</i></small>");
									}
									result.Append("</center><br>");
								}
								else {
									result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
									result.Append(singleFailureLine(currentLine));
								}
							}
							else if (currentLine.StartsWith("..."))
							{
								result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
								result.Append("<small><i>(");
								result.Append(currentLine.Replace("... ", ""));
								result.Append(")</i></small>");
							}
							else {
								result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
								string[] parts = currentLine.Split(':');
								if (currentLine.Contains(loggedException.GetType().Name))
								{
									result.Append("<center><h3>");
									result.Append(parts[0]);
									result.Append("</h3>");

									if (parts.Length > 1)
									{
										result.Append("<small><i>");
										result.Append(parts[1].Trim());
										result.Append("</i></small>");
									}
									result.Append("</center><br>");
								}
								else {
									//use regex
									// FIXED
									result.Append(singleFailureLine(Regex.Replace(currentLine, Symbols.CarriageReturnLineFeed, " ")));
								}
							}

							result.Append(HTML__TABLE_END_DATA_CELL);
							result.Append(HTML__ROW_END);
						}
					}
				}

				result.Append(HTML__TABLE_BEGIN_WIDE_DATA_CELL);
				result.Append("<small>");
				result.Append(HTML__SPACE);
				result.Append("</small>");
				result.Append(HTML__TABLE_END_DATA_CELL);
				result.Append(HTML__EXCEPTION_TABLE_END);
				Message(result.ToString(), requestedLevel, icon);
				Flush();
			}
			catch (Exception thisException)
			{
				Console.Out.WriteLine();
				Console.Out.WriteLine("Programming Error in showFailure():");
				Console.Out.WriteLine(Foundation.DepictException(thisException));
				Console.Out.WriteLine();
				Console.Out.WriteLine("Current line was:");
				Console.Out.WriteLine(currentLine);
				Console.Out.WriteLine();
				Console.Out.WriteLine("Failure to log the following exception:");
				Console.Out.WriteLine(Foundation.DepictException(loggedException));
				Console.Out.WriteLine();
			}
		}

		private string singleFailureLine(string currentLine)
		{
			StringBuilder result = new StringBuilder();
			result.Append("<li>");
			result.Append(currentLine);
			return result.ToString();
		}
	}
}
