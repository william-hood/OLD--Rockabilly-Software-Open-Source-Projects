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
using Rockabilly.Common;
using MemoirV2;

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

		public static string ToHtmlLogIcon(this TestStatus it)
		{
			switch (it)
			{
				case TestStatus.Pass:
					return MemoirV2.Constants.EMOJI_PASSING_TEST;
				case TestStatus.Fail:
					return MemoirV2.Constants.EMOJI_FAILING_TEST;
				case TestStatus.Subjective:
					return MemoirV2.Constants.EMOJI_SUBJECTIVE_TEST;
				default:
					return MemoirV2.Constants.EMOJI_INCONCLUSIVE_TEST;
			}
        }

        internal static string ToStyle(this TestStatus it)
        {
            switch (it)
            {
                case TestStatus.Pass:
                    return "decaf_green";
                case TestStatus.Fail:
                    return "decaf_orange";
                case TestStatus.Subjective:
                    return "old_parchment";
                default:
                    return "decaf_orange_light_roast";
            }
        }

        public static void Log(this TestStatus it, Memoir memoir, string message)
		{
			memoir.Info(message, it.ToHtmlLogIcon());
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
