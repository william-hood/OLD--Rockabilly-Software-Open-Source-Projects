// Copyright (c) 2019, 2016 William Arthur Hood
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
using Rockabilly.MemoirV2;

namespace Rockabilly.CoarseGrind
{
	public enum TestConditionalType
	{
		//Unspecified, Prerequisite, PassCriterion

        Consideration, Requirement, Assertion
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

		internal static TestResult FromConditional(TestConditionalType conditionType, bool outcome, string explaination)
		{
			string prefix = string.Empty;
			if (conditionType == TestConditionalType.Assertion)
			{
				prefix = "(Assertion) ";
			}
			if (conditionType == TestConditionalType.Requirement)
			{
				prefix = "(Requirement) ";
			}

			TestResult result = new TestResult();
			if (outcome)
			{
				result.Status = TestStatus.Pass;
			}
			else {
				if (conditionType == TestConditionalType.Assertion)
				{
					result.Status = TestStatus.Fail;
                }

                if (conditionType == TestConditionalType.Consideration)
                {
                    result.Status = TestStatus.Subjective;
                }

                // Otherwise leave the default status of inconclusive in-place.
            }
			result.Description = prefix + explaination;
			return result;
		}

		public static TestResult FromPrerequisite(bool condition, string conditionDescription)
		{
			return FromConditional(TestConditionalType.Requirement, condition, conditionDescription);
		}

		public static TestResult FromPassCriterion(bool condition, string conditionDescription)
		{
			return FromConditional(TestConditionalType.Assertion, condition, conditionDescription);
		}

		public static TestResult FromConsideration(bool condition, string conditionDescription)
		{
			return FromConditional(TestConditionalType.Consideration, condition, conditionDescription);
		}

		public void Log(Memoir memoir)
		{
			Status.Log(memoir, Description);

			foreach (Exception thisFailure in Failures)
			{
                memoir.ShowException(thisFailure);
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

        /*
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
        */
	}
}
