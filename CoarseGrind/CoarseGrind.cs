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
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind
{
	public static class CoarseGrind
	{
		internal const LoggingLevel DEFAULT_LOGGING_LEVEL = LoggingLevel.Warning;
		internal static string DateFormatString = "yyyy-MM-dd HH-mm-ss.ffff";
		internal const string SUMMARY_FILE = "SUMMARY";
		internal const string SUMMARY_FILE_EXTENSION = ".csv";
		internal const string SUMMARY_FILE_NAME = SUMMARY_FILE + SUMMARY_FILE_EXTENSION;
		internal static readonly string SUMMARY_TEXTFILE_NAME = '.' + SUMMARY_FILE;
		internal static bool KILL_SWITCH = false;



		private static string defaultParentFolder = default(string);
		internal static string DEFAULT_PARENT_FOLDER// = Foundation.UserHomeFolder + Path.DirectorySeparatorChar + "Documents" + Path.DirectorySeparatorChar + "Test Results";
		{
			get
			{
				if (defaultParentFolder == default(string))
				{
					string rootShortName = Path.DirectorySeparatorChar + "Test Results";
					string rootPath = Foundation.UserHomeFolder + Path.DirectorySeparatorChar + "Documents";

					if (Directory.Exists(rootPath))
					{
						// Deliberate NO-OP
					}
					else
					{
						rootPath = Foundation.UserHomeFolder;
					}

					defaultParentFolder = rootPath + rootShortName;
				}

				return defaultParentFolder;
			}
		}

		public static TestPriority TestPriorityFromString(String statusString)
		{
			if (statusString.ToUpper().StartsWith("N"))
				return TestPriority.Normal;
			if (statusString.ToUpper().StartsWith("A"))
				return TestPriority.Ancillary;
			if (statusString.ToUpper().StartsWith("C"))
				return TestPriority.Critical;
			return TestPriority.HappyPath;
		}

		public static TestStatus TestStatusFromString(String statusString)
		{
			if (statusString.ToUpper().StartsWith("P"))
				return TestStatus.Pass;
			if (statusString.ToUpper().StartsWith("F"))
				return TestStatus.Fail;
			return TestStatus.Inconclusive;
		}
	}
}
