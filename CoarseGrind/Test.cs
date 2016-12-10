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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.Generic;
using Rockabilly.Common;
using System.IO;
using Rockabilly.Common.HtmlEffects;
using System.Text.RegularExpressions;

namespace Rockabilly.CoarseGrind
{
	public abstract class Test : TestEssentials
	{
		public static readonly CoarseGrindLogSystem Log = new CoarseGrindLogSystem();

		public const string inProgressName = "(test in progress)";
		private const string SETUP = "setup";
		private const string CLEANUP = "cleanup";
		public string parentArtifactsDirectory = default(string);
		public TestPriority Priority = TestPriority.Normal;
		private Thread executionThread = null;
		internal bool WasSetup = false;
		internal bool WasRun = false;
		internal bool WasCleanedUp = false;

		[MethodImpl(MethodImplOptions.Synchronized)]
		internal float getProgress()
		{
			float result = 0;
			if (WasSetup) result += (float).33;
			if (WasRun) result += (float).34;
			if (WasCleanedUp) result += (float).33;
			return result;
		}

		protected class SetupEnforcement
		{
			public string identifier = default(string);
			public string name = default(string);
			public int contentSize = default(int);
			public TestPriority Priority = TestPriority.Normal;

			public SetupEnforcement(Test candidate)
			{
				identifier = candidate.Identifier;
				name = candidate.Name;
				if (!(candidate is Test))
				{
					contentSize = candidate.Results.Count;
				}
				Priority = candidate.Priority;
			}

			public bool matches(SetupEnforcement candidate)
			{
				if (!identifier.Equals(candidate.identifier)) return false;
				if (!name.Equals(candidate.name)) return false;
				if (contentSize != candidate.contentSize) return false;
				if (Priority != candidate.Priority) return false;
				return true;
			}
		}

		// End User May Optionally Implement
		public virtual string CustomStyle { get { return ""; } }

		// End User Must Implement
		public abstract bool Setup();
		public abstract bool Cleanup();
		public abstract string Identifier { get; }
		public abstract string Name { get; }
		public abstract string DetailedDescription { get; }
		public abstract void PerformTest();

		public abstract string[] TestSuiteMemberships { get; }
		public abstract string[] TestCategoryMemberships { get; }

		public readonly List<TestResult> Results = new List<TestResult>();

		private string getEchelonName()
		{
			return "Test";
		}

		private string DescribeCategorization
		{
			get
			{
				StringBuilder tmp = new StringBuilder();

				foreach (string thisCategory in TestCategoryMemberships)
				{
					if (tmp.Length > 0) tmp.Append('/');
					tmp.Append(thisCategory);
				}

				return tmp.ToString();
			}
		}

		public virtual void AddResult(TestResult thisResult, LoggingLevel requestedLevel = LoggingLevel.Standard)
		{
			thisResult.Log(requestedLevel);
			Results.Add(thisResult);
		}

		public TestStatus OverallStatus
		{
			get
			{
				if ((Results.Count < 1))
					return TestStatus.Inconclusive;
				TestStatus finalValue = TestStatus.Pass;
				foreach (TestResult thisResult in Results)
				{
					finalValue = finalValue.CombineWith(thisResult.Status);
				}
				return finalValue;
			}
		}

		private void LogTestHeader()
		{
			Log.Header(IdentifiedName);
		}

		public virtual void RunTest(LoggingLevel preferredLoggingLevel, string rootDirectory)
		{
			if (CoarseGrind.KILL_SWITCH)
			{
				// Decline to run
			}
			else
			{
				bool setupResult = true;

				Log.SetCustomStyle(CustomStyle);
				parentArtifactsDirectory = rootDirectory;
				Guid thisOutputIdentifier = Log.AddOutput(new TextOutputManager(
						ArtifactsDirectory + Path.DirectorySeparatorChar + LogFileName),
						preferredLoggingLevel, LoggingMode.Minimum);
				LogTestHeader();

				SetupEnforcement before = null;

				try
				{
					IndicateSetup();
					before = new SetupEnforcement(this);
					try
					{
						setupResult = Setup();
					}
					finally
					{
						WasSetup = true;
					}
				}
				catch (Exception thisFailure)
				{
					setupResult = false;
					AddResult(GetResultForPreclusionInSetup(thisFailure));
				}
				finally
				{
					if (!new SetupEnforcement(this).matches(before))
					{
						setupResult = false;
						AddResult(new TestResult(TestStatus.Inconclusive, "PROGRAMMING ERROR: It is illegal to change the identifier, name, or priority in Setup.  This must happen in the constructor."));
					}
					IndicateSectionEnd();
				}

				if (setupResult && (CoarseGrind.KILL_SWITCH == false))
				{
					try
					{
						IndicateBody();
						executionThread = new Thread(PerformTest);
						executionThread.Start();
						executionThread.Join();
					}
					catch (Exception thisFailure)
					{
						AddResult(GetResultForFailure(thisFailure));
					}
					finally
					{
						WasRun = true;
						executionThread = null;
						IndicateSectionEnd();
					}
				}
				else {
					AddResult(new TestResult(TestStatus.Inconclusive,
							"Declining to perform test case " + IdentifiedName
									+ " because setup method failed."));
				}

				try
				{
					IndicateCleanup();
					Cleanup();
					WasCleanedUp = true;
				}
				catch (Exception thisFailure)
				{
					ReportFailureInCleanup(thisFailure);
				}
				finally
				{
					IndicateSectionEnd();
				}

				TestStatus overall = OverallStatus;
				Log.Message("<h1>Overall Status: " + overall.ToString() + "</h1>", overall.ToLoggingLevel(), overall.ToHtmlLogIcon());
				Log.ClearSpecificOutput(thisOutputIdentifier);
			}
		}

		public void Interrupt()
		{
			try
			{
				executionThread.Interrupt();
				executionThread.Abort();
				executionThread.Abort();
				executionThread.Abort();
			}
			catch
			{
				// DELIBERATE NO-OP
			}
		}

		public virtual void CheckPrerequisite(string conditionDescription, bool condition)
		{
			TestResult result = TestResult.FromPrerequisite(conditionDescription, condition);
			AddResult(result, result.Status.ToLoggingLevel());
			result = null;
		}

		public virtual void CheckPassCriterion(string conditionDescription, bool condition)
		{
			TestResult result = TestResult.FromPassCriterion(conditionDescription, condition);
			AddResult(result, result.Status.ToLoggingLevel());
			result = null;
		}

		public virtual void CheckCondition(string conditionDescription, bool condition)
		{
			TestResult result = TestResult.FromCondition(conditionDescription, condition);
			AddResult(result, result.Status.ToLoggingLevel());
			result = null;
		}

		public virtual void MakeSubjective()
		{
			AddResult(new TestResult(TestStatus.Subjective, "This test case requires analysis by appropriate personnel to determine pass/fail status"));
		}

		private string FilterForSummary(string it)
		{
			//use regex -- will need to search the Java version for ReplaceAll() calls.
			// FIXED
			return Regex.Replace(Regex.Replace(it, "[,\r\f\b]", ""), "[\t\n]", " ");
		}

		internal List<String> SummaryDataRow
		{
			get
			{
				List<String> tmp = new List<String>();
				tmp.Add(FilterForSummary(DescribeCategorization));
				tmp.Add(FilterForSummary(Priority.ToString()));
				tmp.Add(FilterForSummary(Identifier));
				tmp.Add(FilterForSummary(Name));
				tmp.Add(FilterForSummary(DetailedDescription));
				tmp.Add(FilterForSummary(OverallStatus.ToString()));

				StringBuilder reasoning = new StringBuilder();
				foreach (TestResult thisResult in Results)
				{
					if (thisResult.Status != TestStatus.Pass)
					{
						if (reasoning.Length > 0) reasoning.Append("; ");
						reasoning.Append(thisResult.Description);

						if (thisResult.HasFailures())
						{
							foreach (Exception thisFailure in thisResult.Failures)
							{
								if (reasoning.Length > 0) reasoning.Append("; ");
								reasoning.Append(thisFailure.GetType().Name);
							}
						}
					}
				}
				tmp.Add(FilterForSummary(reasoning.ToString()));

				reasoning = null;
				return tmp;
			}
		}

		public virtual string LogFileName
		{
			get
			{
				return IdentifiedName + " Log.html";
			}
		}


		public virtual string ArtifactsDirectory
		{
			get
			{
				return parentArtifactsDirectory + Path.DirectorySeparatorChar + inProgressName;
			}
		}

		public virtual string IdentifiedName
		{
			get
			{
				return Identifier + " - " + Name;
			}
		}

		public virtual TestResult GetResultForFailure(Exception thisFailure, string section = "")
		{
			return GetResultForIncident(TestStatus.Fail, section, thisFailure);
		}

		public virtual void ReportFailureInCleanup(Exception thisFailure)
		{
			ReportFailureInCleanup("", thisFailure);
		}

		public virtual void ReportFailureInCleanup(string additionalMessage, Exception thisFailure)
		{
			if (additionalMessage.Length > 0)
				additionalMessage = " " + additionalMessage;
			Log.Message(IdentifiedName + CLEANUP
					+ ":  An unanticipated failure occurred" + additionalMessage
					+ ".", LoggingLevel.Warning, Log.WarningIcon);
			Log.ShowException(thisFailure);
		}

		public virtual TestResult GetResultForPreclusionInSetup(Exception thisPreclusion)
		{
			return GetResultForIncident(TestStatus.Inconclusive, SETUP, thisPreclusion);
		}

		public virtual TestResult GetResultForPreclusion(Exception thisPreclusion)
		{
			return GetResultForIncident(TestStatus.Inconclusive, "", thisPreclusion);
		}

		private TestResult GetResultForIncident(TestStatus status, string section, Exception thisFailure)
		{
			if (section.Length > 0)
				section = " (" + section + ")";
			TestResult result = new TestResult(status, IdentifiedName
					+ section + ":  An unanticipated failure occurred.");
			result.Failures.Add(thisFailure);
			return result;
		}

		public virtual string PrefixedName
		{
			get
			{
				return OverallStatus + " - " + IdentifiedName;
			}
		}

		private string sectionIndicator = default(string);

		internal void IndicateSetup()
		{
			string tmpIndicator = "Setup - " + getEchelonName() + " " + IdentifiedName;
			IndicateSectionStart(Log.InfoIcon, "<b>" + tmpIndicator + "</b><br><small><i>" + DetailedDescription + "</i></small>");
			sectionIndicator = tmpIndicator;
			tmpIndicator = null;
		}

		private void IndicateSectionStart(InlineImage icon, string indicator)
		{
			sectionIndicator = indicator;
			Log.Message(sectionIndicator
					+ HtmlLogSystem.HTML__LINE_BREAK
					+ Log.HTML__SECTIONSTART, LoggingLevel.Critical, icon);
		}

		internal void IndicateSectionEnd()
		{
			Log.Message(Log.HTML__SECTIONEND
					+ HtmlLogSystem.HTML__LINE_BREAK
					+ sectionIndicator);
			Log.SkipLine();
		}

		internal void IndicateCleanup()
		{
			IndicateSectionStart(null, "Cleanup - " + getEchelonName() + " " + IdentifiedName);
		}

		internal void IndicateBody()
		{
			IndicateSectionStart(null, getEchelonName() + " " + IdentifiedName);
		}

		public static void WaitSeconds(int howMany, LoggingLevel requestedLevel = LoggingLevel.Standard)
		{
			Log.Message("Waiting " + howMany + " seconds...", requestedLevel, Log.InfoIcon);
			DoWait(1000 * howMany, requestedLevel);
		}

		public static void WaitMilliseconds(int howMany, LoggingLevel requestedLevel = LoggingLevel.Standard)
		{
			Log.Message("Waiting " + howMany + " milliseconds...", requestedLevel, Log.InfoIcon);
			DoWait(howMany, requestedLevel);
		}

		private static void DoWait(int howManyMilliseconds, LoggingLevel requestedLevel = LoggingLevel.Standard)
		{
			try
			{
				Thread.Sleep(howManyMilliseconds);
			}
			catch (ThreadInterruptedException) { }
		}
	}
}
