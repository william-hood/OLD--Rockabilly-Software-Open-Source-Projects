using NUnit.Framework;
using Rockabilly.MemoirV2;
using System;
using System.IO;

namespace Tests
{
    public class Tests
    {
        Memoir topLevelLog;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            topLevelLog = new Memoir("Memoir NUnit Test", Console.Out, File.CreateText("/Users/bill/memoir_nunit_test.html"));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            topLevelLog.Conclude();
        }

        [SetUp]
        public void Setup()
        {
            Memoir setupLog = new Memoir("Set Up", Console.Out);
            setupLog.Info("Message 1");
            setupLog.Info("Message 2");
            setupLog.Info("Message 3");
            topLevelLog.ShowMemoir(setupLog, Constants.EMOJI_SETUP);
        }

        [Test]
        public void Test1()
        {
            Memoir log = new Memoir("Test 1", Console.Out);
            try
            {
                log.Info("Message 1");
                log.Info("Message 2");
                log.Info("Message 3");
                Assert.Pass();
            }
            catch (SuccessException)
            {
            }
            catch (AssertionException failedAssertion)
            {
                log.Info(failedAssertion.Message, Constants.EMOJI_FAILING_TEST);
                topLevelLog.ShowMemoir(log, Constants.EMOJI_FAILING_TEST, "failing_test_result");
                return;
            }
            catch (Exception thisException)
            {
                log.ShowException(thisException);
                topLevelLog.ShowMemoir(log, Constants.EMOJI_ERROR, "failing_test_result");
                return;
            }

            log.Info("Test Passed!", Constants.EMOJI_PASSING_TEST);
            topLevelLog.ShowMemoir(log, Constants.EMOJI_PASSING_TEST, "passing_test_result");
        }

        [Test]
        public void Test2()
        {
            Memoir log = new Memoir("Test 2", Console.Out);
            try
            {
                log.Info("Message 1");
                log.Info("Message 2");
                log.ShowObject(new NUnit.Framework.TestParameters());
                log.Info("Message 3");
                Assert.AreEqual("this", "that", "Shold have been equal");
            }
            catch (SuccessException)
            {
            }
            catch (AssertionException failedAssertion)
            {
                log.Info(failedAssertion.Message, Constants.EMOJI_FAILING_TEST);
                topLevelLog.ShowMemoir(log, Constants.EMOJI_FAILING_TEST, "failing_test_result");
                return;
            }
            catch (Exception thisException)
            {
                log.ShowException(thisException);
                topLevelLog.ShowMemoir(log, Constants.EMOJI_ERROR, "failing_test_result");
                return;
            }

            log.Info("Test Passed!", Constants.EMOJI_PASSING_TEST);
            topLevelLog.ShowMemoir(log, Constants.EMOJI_PASSING_TEST, "passing_test_result");
        }

        [Test]
        public void Test3()
        {
            Memoir log = new Memoir("Test 3", Console.Out);
            try
            {
                log.Info("Message 1");
                log.Info("Message 2");
                log.Info("Message 3");
                Assert.Greater(1, 200, "Should have been greater");
            }
            catch (SuccessException)
            {
            }
            catch (AssertionException failedAssertion)
            {
                log.Info(failedAssertion.Message, Constants.EMOJI_FAILING_TEST);
                topLevelLog.ShowMemoir(log, Constants.EMOJI_FAILING_TEST, "failing_test_result");
                return;
            }
            catch (Exception thisException)
            {
                log.ShowException(thisException);
                topLevelLog.ShowMemoir(log, Constants.EMOJI_ERROR, "failing_test_result");
                return;
            }

            log.Info("Test Passed!", Constants.EMOJI_PASSING_TEST);
            topLevelLog.ShowMemoir(log, Constants.EMOJI_PASSING_TEST, "passing_test_result");
        }

        [TearDown]
        public void TearDown()
        {
            Memoir teardownLog = new Memoir("Tear Down", Console.Out);
            teardownLog.Info("Message 1");
            teardownLog.Info("Message 2");
            teardownLog.Info("Message 3");
            topLevelLog.ShowMemoir(teardownLog, Constants.EMOJI_CLEANUP);
        }
    }
}