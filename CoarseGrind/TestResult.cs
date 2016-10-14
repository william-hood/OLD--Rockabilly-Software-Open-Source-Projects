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
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind
{
	internal enum TestConditionalType
	{
		Unspecified, Prerequisite, PassCriterion
	}

	public class TestResult
	{
		public string Description = default(string);
		public TestStatus Status = TestStatus.Inconclusive;
		public List<Exception> Failures = new List<Exception>();
		public List<object> Artifacts = new List<object>();

		public TestResult(TestStatus status = TestStatus.Inconclusive, string description = default(string), params Exception[] associatedFailures)
		{
			foreach (Exception thisFailure in associatedFailures)
			{
				Failures.Add(thisFailure);
			}

			Status = status;
			Description = description;
		}

		public bool HasArtifacts()
		{
			if (Artifacts == null)
				return false;
			return Artifacts.Count > 0;
		}

		public bool HasFailures()
		{
			if (Failures == null)
				return false;
			return Failures.Count > 0;
		}

		private static TestResult FromCondition(string conditionDescription, bool condition, TestConditionalType conditionType)
		{
			string prefix = string.Empty;
			if (conditionType == TestConditionalType.PassCriterion)
			{
				prefix = "(Pass Criterion) ";
			}
			if (conditionType == TestConditionalType.Prerequisite)
			{
				prefix = "(Prerequisite) ";
			}

			TestResult result = new TestResult();
			if (condition)
			{
				result.Status = TestStatus.Pass;
			}
			else {
				if (conditionType != TestConditionalType.Prerequisite)
				{
					result.Status = TestStatus.Fail;
				}
			}
			result.Description = prefix + conditionDescription;
			return result;
		}

		public static TestResult FromPrerequisite(string conditionDescription, bool condition)
		{
			return FromCondition(conditionDescription, condition, TestConditionalType.Prerequisite);
		}

		public static TestResult FromPassCriterion(String conditionDescription, bool condition)
		{
			return FromCondition(conditionDescription, condition, TestConditionalType.PassCriterion);
		}

		public static TestResult FromCondition(String conditionDescription, bool condition)
		{
			return FromCondition(conditionDescription, condition, TestConditionalType.Unspecified);
		}

		public void Log()
		{
			Log(Status.ToLoggingLevel());
		}

		public void Log(LoggingLevel requestedLevel)
		{
			Status.Log(Description, requestedLevel);

			foreach (Exception thisFailure in Failures)
			{
				Test.Log.ShowException(thisFailure, requestedLevel);
			}
		}

		private static string[] SUMMARY_HEADERS = { "Criterion", "Status", "Artifacts", "Failures" };

		public string[] SummaryDataHeaders
		{
			get
			{
				return SUMMARY_HEADERS;
			}
		}

		public static bool CheckCollection(List<TestResult> Results, bool shouldLog = true)
		{
			bool isPassing = true;
			foreach (TestResult thisResult in Results)
			{
				if (shouldLog) thisResult.Log(LoggingLevel.Standard);
				isPassing &= thisResult.Status.IsPassing();
			}
			return isPassing;
		}
	}
}
