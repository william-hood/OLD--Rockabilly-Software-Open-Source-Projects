// Copyright (c) 2019 William Arthur Hood
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
using System.Diagnostics;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Rockabilly.MemoirV2;
using System.Collections;

namespace Rockabilly.CoarseGrind
{
    public abstract partial class Test
    {
        protected internal class enforcer
        {
            private TestConditionalType type;
            private Test parent;

            protected internal enforcer(TestConditionalType conditionalType, Test owner)
            {
                type = conditionalType;
                parent = owner;
            }

            private void enforce(bool outcome, string explaination)
            {
                parent.AddResult(TestResult.FromConditional(type, outcome, explaination));
            }

            public void ShouldBeTrue(bool condition, string explaination = default(string))
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected boolean true. (Actual values was {condition})";
                }

                enforce(condition, explaination);
            }

            public void ShouldNotBeTrue(bool condition, string explaination = default(string))
            {
                ShouldBeFalse(condition, explaination);
            }

            public void ShouldBeFalse(bool condition, string explaination = default(string))
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected boolean false. (Actual values was {condition})";
                }

                enforce(!condition, explaination);
            }

            public void ShouldBeEqual(object candidateA, object candidateB, string explaination = default(string))
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected equal values. (Actual values were {candidateA} and {candidateB})";
                }

                enforce(candidateA.Equals(candidateB), explaination);
            }

            public void ShouldNotBeEqual(object candidateA, object candidateB, string explaination = default(string))
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected unequal values. (Actual values were {candidateA} and {candidateB})";
                }

                enforce(! candidateA.Equals(candidateB), explaination);
            }

            public void ShouldBeNull(object candidate, string explaination = default(string))
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected null. (Actual values was {candidate})";
                }

                enforce(candidate == null, explaination);
            }

            public void ShouldNotBeNull(object candidate, string explaination = default(string))
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected non-null. (Actual values was {candidate})";
                }

                enforce(candidate != null, explaination);
            }

            public void ShouldBeEmpty(Array candidate, string explaination = default(string))
            {
                enforceEmpty(candidate.Length, explaination);
            }

            public void ShouldBeEmpty(ICollection candidate, string explaination = default(string))
            {
                enforceEmpty(candidate.Count, explaination);
            }

            private void enforceEmpty(int actualCount, string explaination)
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected an empty set. (Actually had {actualCount} items)";
                }

                enforce(actualCount == 0, explaination);
            }

            public void ShouldNotBeEmpty(Array candidate, string explaination = default(string))
            {
                enforceNotEmpty(candidate.Length, explaination);
            }

            public void ShouldNotBeEmpty(ICollection candidate, string explaination = default(string))
            {
                enforceNotEmpty(candidate.Count, explaination);
            }

            private void enforceNotEmpty(int actualCount, string explaination)
            {
                if (explaination == default(string))
                {
                    explaination = $"Expected an non-empty set. (Actually had {actualCount} items)";
                }

                enforce(actualCount > 0, explaination);
            }
        }

        /// <summary>
        /// If any assertion fails the test fails.
        /// </summary>
        protected enforcer Assert;

        /// <summary>
        /// If a requirement fails the test becomes inconclusive. THIS OVERRIDES FAILURE STATUS.
        /// </summary>
        protected enforcer Require;

        /// <summary>
        /// If a consideration fails the test becomes subjective.
        /// </summary>
        protected enforcer Consider;
        // Developer's comment: Would it make more sense for a failed consideration to render the test subjective?

        /*
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
        // Make FromConditional internal rather than private
        // New order is the one or two comparators, and then an OPTIONAL desription.
        private void enforce (TestConditionalType type, bool outcome, string explaination)
        {
            TestResult result = TestResult.FromConditional(type, outcome, explaination);
            AddResult(result);
            result = null;
        }


        public virtual void Require(string conditionDescription, bool condition)
        {
            TestResult result = TestResult.FromPrerequisite(conditionDescription, condition);
            AddResult(result);
            result = null;
        }

        public virtual void Assert(string conditionDescription, bool condition)
        {
            TestResult result = TestResult.FromPassCriterion(conditionDescription, condition);
            AddResult(result);
            result = null;
        }

        public virtual void Consider(string conditionDescription, bool condition)
        {
            TestResult result = TestResult.FromCondition(conditionDescription, condition);
            AddResult(result);
            result = null;
        }
        */

        public virtual void MakeSubjective()
        {
            AddResult(new TestResult(TestStatus.Subjective, "This test case requires analysis by appropriate personnel to determine pass/fail status"));
        }
    }
}