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
using Rockabilly.Common.HtmlEffects;

namespace Rockabilly.CoarseGrind
{
	public enum TestStatus
	{
		Inconclusive, Fail, Subjective, Pass
	}

	public static class TestStatusExtensions
	{

		public static bool IsPassing(this TestStatus it)
		{
			return it == TestStatus.Pass;
		}

		public static bool IsFailing(this TestStatus it)
		{
			return it == TestStatus.Fail;
		}

		public static bool IsInconclusive(this TestStatus it)
		{
			return it == TestStatus.Inconclusive;
		}

		public static InlineImage ToHtmlLogIcon(this TestStatus it)
		{
			switch (it)
			{
				case TestStatus.Pass:
					return Test.Log.PassIcon;
				case TestStatus.Fail:
					return Test.Log.FailIcon;
				case TestStatus.Subjective:
					return Test.Log.InfoIcon;
				default:
					return Test.Log.StopSignIcon;
			}
		}

		public static LoggingLevel ToLoggingLevel(this TestStatus it)
		{
			if (it == TestStatus.Pass)
				return LoggingLevel.Standard;
			return LoggingLevel.Critical;
		}

		public static void Log(this TestStatus it, String message)
		{
			it.Log(message, it.ToLoggingLevel());
		}

		public static void Log(this TestStatus it, string message, LoggingLevel requestedLevel)
		{
			Test.Log.Message(message, requestedLevel, it.ToHtmlLogIcon());
		}

		public static TestStatus CombineWith(this TestStatus it, TestStatus candidate)
		{
			switch (it)
			{
				case TestStatus.Subjective:
					return TestStatus.Subjective;
				case TestStatus.Inconclusive:
					if (candidate == TestStatus.Subjective) return TestStatus.Subjective;
					return TestStatus.Inconclusive;
				case TestStatus.Fail:
					if (candidate == TestStatus.Subjective) return TestStatus.Subjective;
					if (candidate == TestStatus.Inconclusive) return TestStatus.Inconclusive;
					return TestStatus.Fail;
				default:
					if (candidate == TestStatus.Subjective) return TestStatus.Subjective;
					if (candidate == TestStatus.Inconclusive) return TestStatus.Inconclusive;
					if (candidate == TestStatus.Fail) return TestStatus.Fail;
					return TestStatus.Pass;
			}
		}
	}
}
