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
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Rockabilly.Common;
using MemoirV2;

namespace Rockabilly.CoarseGrind
{
    internal class TestSuite : List<Test>
    {
        private const string FOLDER_FOR_ALL_TESTS = "All Tests";
        private Test currentTest = null;
        private string currentArtifactsDirectory = default(string);
        private Thread executionThread = null;
        private float currentCount = float.MaxValue;
        private string name = default(string);

        void Reset()
        {
            currentCount = float.MaxValue;
            currentTest = null;
            if (executionThread != null)
            {
                try
                {
                    executionThread.Abort();
                    executionThread.Abort();
                    executionThread.Abort();
                }
                catch { }
            }
            executionThread = null;
            currentArtifactsDirectory = default(string);
        }

        public TestSuite(string suiteName)
        {
            name = suiteName;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string CurrentTest
        {
            get
            {
                if (currentTest == null) return default(string);
                return currentTest.IdentifiedName;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int getProgress()
        {
            if (Count < 1) return 100;
            if (currentCount >= Count) return 100;

            float effectiveCount = currentCount;

            try
            {
                effectiveCount += (1 * currentTest.getProgress());
            }
            catch
            {
                // DELIBERATE NO-OP
            }

            return (int)Math.Round((effectiveCount / Count) * 100);
        }

        private void CopyResultsToCategories()
        {
            foreach (string thisCategory in currentTest.TestSuiteMemberships)
            {
                try
                {
                    Foundation.CopyCompletely(currentTest.ArtifactsDirectory, currentArtifactsDirectory + Path.DirectorySeparatorChar + thisCategory + Path.DirectorySeparatorChar + currentTest.PrefixedName);
                }
                catch (Exception loggedException)
                {
                    Console.WriteLine(Foundation.DepictException(loggedException));
                }
            }

            try
            {
                Foundation.CopyCompletely(currentTest.ArtifactsDirectory, currentArtifactsDirectory + Path.DirectorySeparatorChar + currentTest.OverallStatus.ToString() + Path.DirectorySeparatorChar + currentTest.PrefixedName);
            }
            catch (Exception loggedException)
            {
                Console.WriteLine(Foundation.DepictException(loggedException));
            }

            try
            {
                Foundation.CopyCompletely(currentTest.ArtifactsDirectory, currentArtifactsDirectory + Path.DirectorySeparatorChar + currentTest.Priority.ToString() + Path.DirectorySeparatorChar + currentTest.PrefixedName);
            }
            catch (Exception loggedException)
            {
                Console.WriteLine(Foundation.DepictException(loggedException));
            }

            try
            {
                Foundation.CopyCompletely(currentTest.ArtifactsDirectory, currentArtifactsDirectory + Path.DirectorySeparatorChar + FOLDER_FOR_ALL_TESTS + Path.DirectorySeparatorChar + currentTest.PrefixedName);
            }
            catch (Exception loggedException)
            {
                Console.WriteLine(Foundation.DepictException(loggedException));
            }

            Directory.Delete(currentTest.ArtifactsDirectory, true);
        }

        public void RunTestSuite(LoggingLevel preferredLoggingLevel, MatchList exclusions, string rootDirectory)
        {
            currentArtifactsDirectory = rootDirectory;
            string expectedFileName = currentArtifactsDirectory + Path.DirectorySeparatorChar + "All tests.html";
            new FileInfo(expectedFileName).Directory.Create();
            Memoir overLog = new Memoir(Name, null, File.CreateText(expectedFileName), CoarseGrind.LogHeader);
            currentCount = 0;
            for (currentCount = 0; currentCount < Count; currentCount++)
            {
                if (CoarseGrind.KILL_SWITCH)
                {
                    // Decline to run
                    break;
                }
                else
                {
                    currentTest = this[(int)currentCount];
                    if (!exclusions.MatchesCaseInspecific(currentTest.IdentifiedName))
                    {
                        try
                        {
                            executionThread = new Thread(Run);
                            executionThread.Start();
                            executionThread.Join();
                        }
                        catch (Exception thisFailure)
                        {
                            currentTest.AddResult(currentTest.GetResultForPreclusionInSetup(thisFailure));
                        }
                        finally
                        {
                            executionThread = null;
                            CopyResultsToCategories();
                            overLog.ShowMemoir(currentTest.topLevelMemoir, currentTest.OverallStatus.ToHtmlLogIcon(), currentTest.OverallStatus.ToStyle());
                        }
                    }
                }
            }

            overLog.Conclude();

            CreateSummaryReport(rootDirectory);
        }

        public void Run()
        {
            currentTest.RunTest(currentArtifactsDirectory);
        }

        public void InterruptCurrentTest()
        {
            currentTest.Interrupt();
        }

        public void HaltAllTesting()
        {
            CoarseGrind.KILL_SWITCH = true;

            currentCount = float.MaxValue;
            InterruptCurrentTest();

            try
            {
                executionThread.Interrupt();
                executionThread.Abort();
                executionThread.Abort();
                executionThread.Abort();
            }
            catch { }
            finally
            {
                executionThread = null;
            }
        }

        static string[] SUMMARY_HEADERS = { "Categorization", "Test Priority", "Test ID", "Name", "Description", "Status", "Reasons" };

        public void CreateSummaryReport(string rootDirectory)
        {
            Console.WriteLine("Creating Test Suite Summary Report");

            string fullyQulaifiedSummaryFileName = rootDirectory + Path.DirectorySeparatorChar + CoarseGrind.SUMMARY_FILE_NAME;
            DelimitedDataManager<string> summaryReport = null;

            try
            {
                summaryReport = DelimitedDataManager<string>.FromFile(fullyQulaifiedSummaryFileName, new StringParser());
            }
            catch
            {
                summaryReport = new DelimitedDataManager<string>(columnNames: SUMMARY_HEADERS);
            }

            foreach (Test thisCase in this)
            {
                //if (thisCase.wasRun) summaryReport.addDataRow(thisCase.getSummaryDataRow());
                if (thisCase.WasSetup) summaryReport.addDataRow(thisCase.SummaryDataRow);
            }

            summaryReport.ToFile(fullyQulaifiedSummaryFileName);

            string fullyQulaifiedSummaryTextFileName = rootDirectory + Path.DirectorySeparatorChar + CoarseGrind.SUMMARY_TEXTFILE_NAME;
            try
            {
                TextOutputManager textFile = new TextOutputManager(fullyQulaifiedSummaryTextFileName);
                textFile.WriteLine(OverallStatus.ToString());
                textFile.Flush();
                textFile.Close();
            }
            catch
            {
                // DELIBERATE NO-OP
            }
        }

        public TestStatus OverallStatus
        {
            get
            {
                int tally = 0;
                TestStatus finalValue = TestStatus.Pass;
                foreach (Test thisTest in this)
                {
                    if (thisTest.WasRun)
                    {
                        tally++;
                        finalValue = finalValue.CombineWith(thisTest.OverallStatus);
                    }
                }
                if (tally < 1) return TestStatus.Inconclusive;
                return finalValue;
            }
        }

    }
}
