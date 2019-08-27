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
using System.Diagnostics;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Rockabilly.MemoirV2;

namespace Rockabilly.CoarseGrind
{
    public abstract partial class Test
    {
        private const string INFO_ICON = "ℹ️";

        internal Memoir topLevelMemoir;
        internal Memoir setupMemoir;
        internal Memoir cleanupMemoir;

        public Memoir Log
        {
            get
            {
                string caller = new StackTrace().GetFrame(1).GetMethod().Name;
                switch (caller)
                {
                    case "Setup":
                        return setupMemoir;
                    case "Cleanup":
                        return cleanupMemoir;
                    default:
                        return topLevelMemoir;
                }
            }
        }

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
                identifier = candidate.identifier;
                name = candidate.name;
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

        // For readability these are now passed into the base constructor
        // and are no longer abstract properties
        private string identifier = default(string);
        private string name = default(string);
        private string detailedDescription = default(string);
        private string[] categories = { };

        protected Test(
            string Name,
            string DetailedDescription = default(string),
            string ID = default(string),
            params string[] Categories)
        {
            identifier = ID;
            name = Name;
            detailedDescription = DetailedDescription;
            categories = Categories;
            Assert = new enforcer(TestConditionalType.Assertion, this);
            Require = new enforcer(TestConditionalType.Requirement, this);
            Consider = new enforcer(TestConditionalType.Consideration, this);
    }

        // End User Must Implement
        public virtual bool Setup() { return true; }
        public virtual bool Cleanup() { return true; }
        public abstract void PerformTest();

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

                foreach (string thisCategory in categories)
                {
                    if (tmp.Length > 0) tmp.Append('/');
                    tmp.Append(thisCategory);
                }

                return tmp.ToString();
            }
        }

        public virtual void AddResult(TestResult thisResult)
        {
            thisResult.Log(topLevelMemoir);
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

        public virtual void RunTest(string rootDirectory)
        {
            if (CoarseGrind.KILL_SWITCH)
            {
                // Decline to run
            }
            else
            {
                bool setupResult = true;
                bool cleanupResult = true;

                parentArtifactsDirectory = rootDirectory;

                string expectedFileName = ArtifactsDirectory + Path.DirectorySeparatorChar + LogFileName;
                new FileInfo(expectedFileName).Directory.Create();
                topLevelMemoir = new Memoir(name, Console.Out, File.CreateText(expectedFileName), CoarseGrind.LogHeader);

                if (detailedDescription != default(string))
                {
                    topLevelMemoir.WriteToHTML(String.Format("<small><i>{0}</i></small>", detailedDescription), emoji: Constants.EMOJI_TEXT_BLANK_LINE);
                }
                topLevelMemoir.SkipLine();

                SetupEnforcement before = null;

                try
                {
                    setupMemoir = IndicateSetup();
                    before = new SetupEnforcement(this);
                    try
                    {
                        setupResult = Setup();
                    }
                    finally
                    {
                        WasSetup = true;

                        if (setupMemoir.WasUsed())
                        {
                            string style = "decaf_orange_light_roast";
                            if (setupResult) { style = "decaf_green_light_roast"; }
                            topLevelMemoir.ShowMemoir(setupMemoir, MemoirV2.Constants.EMOJI_SETUP, style);
                        }
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
                }

                if (setupResult && (CoarseGrind.KILL_SWITCH == false))
                {
                    try
                    {
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
                    }
                }
                else
                {
                    AddResult(new TestResult(TestStatus.Inconclusive,
                            "Declining to perform test case " + IdentifiedName
                                    + " because setup method failed."));
                }

                cleanupMemoir = IndicateCleanup();
                try
                {
                    cleanupResult = Cleanup();
                    WasCleanedUp = true;
                }
                catch (Exception thisFailure)
                {
                    ReportFailureInCleanup(thisFailure);
                }
                finally
                {
                    if (cleanupMemoir.WasUsed())
                    {
                        string style = "decaf_orange_light_roast";
                        if (cleanupResult) { style = "decaf_green_light_roast"; }
                        topLevelMemoir.ShowMemoir(cleanupMemoir, MemoirV2.Constants.EMOJI_CLEANUP, style);
                    }
                }

                TestStatus overall = OverallStatus;
                topLevelMemoir.WriteToHTML(String.Format("<h2>Overall Status: {0}</h2>", overall.ToString()));

                // Will need the icon here...
                topLevelMemoir.EchoPlainText(String.Format("Overall Status: {0}", overall.ToString()));

                topLevelMemoir.Conclude();
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
                tmp.Add(FilterForSummary(identifier));
                tmp.Add(FilterForSummary(name));
                tmp.Add(FilterForSummary(detailedDescription));
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
                if (identifier == default(string)) return name;
                return identifier + " - " + name;
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
            topLevelMemoir.Error(IdentifiedName + CLEANUP
                    + ":  An unanticipated failure occurred" + additionalMessage
                    + ".");
            topLevelMemoir.ShowException(thisFailure);
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

        private Memoir IndicateSetup()
        {
            Memoir result = new Memoir("Setup - " + getEchelonName() + " " + IdentifiedName, Console.Out);
            return result;
        }

        private Memoir IndicateCleanup()
        {
            Memoir result = new Memoir("Cleanup - " + getEchelonName() + " " + IdentifiedName, Console.Out);
            return result;
        }

        private Memoir IndicateBody()
        {
            Memoir result = new Memoir(getEchelonName() + " " + IdentifiedName, Console.Out);
            return result;
        }

        public void WaitSeconds(int howMany)
        {
            topLevelMemoir.Info("Waiting " + howMany + " seconds...", INFO_ICON);
            DoWait(1000 * howMany);
        }

        public void WaitMilliseconds(int howMany)
        {
            topLevelMemoir.Info("Waiting " + howMany + " milliseconds...", INFO_ICON);
            DoWait(howMany);
        }

        private static void DoWait(int howManyMilliseconds)
        {
            try
            {
                Thread.Sleep(howManyMilliseconds);
            }
            catch (ThreadInterruptedException) { }
        }
    }
}
