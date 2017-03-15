using AllureCSharpCommons.AllureModel;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpecFlow.Allure.Tests.Scenarios
{
    [TestFixture]
    public class IntegrationFixture
    {
        const string SCENARIO_PATTERN = "Scenario:";
        const string FEATURE_PATTERN = "Feature:";

        HashSet<string> scenarioTitles = new HashSet<string>();
        XmlSerializer serializer = new XmlSerializer(typeof(testsuiteresult));
        ConcurrentDictionary<string, testsuiteresult> specflowSuites = new ConcurrentDictionary<string, testsuiteresult>();
        ConcurrentDictionary<string, testsuiteresult> allureSuites = new ConcurrentDictionary<string, testsuiteresult>();


        [OneTimeSetUp]
        public void Init()
        {
            // setup current folder for nUnit engine
            var dir = Path.GetDirectoryName(typeof(IntegrationFixture).Assembly.Location);
            Environment.CurrentDirectory = dir;

            ParseSpecFlowFeatures();

            // delete allure results folder
            DirectoryInfo allureResultsDir = new DirectoryInfo(Config.ResultsPath);
            if (allureResultsDir.Exists)
                allureResultsDir.Delete(true);

            // run SpecFlow scenarios using SpecRun runner
            var process = Process.Start(@"..\..\runtests.cmd");
            process.WaitForExit();

            // parse allure suites
            ParseAllureSuites(allureResultsDir);
        }


        [TestCase("Total: 18")]
        [TestCase("Succeeded: 8")]
        [TestCase("Ignored: 0")]
        [TestCase("Pending: 2")]
        [TestCase("Skipped: 0")]
        [TestCase("Failed: 8")]
        public void SpecRunLog(string text)
        {
            var log = File.ReadLines("specrun.log");
            Assert.That(log.Contains(text));
        }

        [Test]
        public void SuiteTitles()
        {
            Assert.That(allureSuites.Keys, Is.EquivalentTo(specflowSuites.Keys));
        }

        [Test]
        public void ScenarioTitles()
        {
            Assert.That(allureSuites.Keys, Is.EquivalentTo(specflowSuites.Keys));
        }

        [TestCase(status.passed, 8)]
        [TestCase(status.failed, 1)]
        [TestCase(status.broken, 5)]
        [TestCase(status.canceled, 2)]
        [TestCase(status.pending, 2)]
        public void AssertOutcome(status status, int count)
        {
            var actualCount = allureSuites.Values.SelectMany(x => x.testcases)
                .Where(x => x.status == status).Count();

            Assert.That(actualCount, Is.EqualTo(count));
        }
        private void ParseAllureSuites(DirectoryInfo allureResultsDir)
        {
            var allureSuiteFiles = allureResultsDir.GetFiles("*-testsuite.xml");
            foreach (var suiteFile in allureSuiteFiles)
            {
                using (var stream = new FileStream(suiteFile.FullName, FileMode.Open))
                {
                    var suite = serializer.Deserialize(stream) as testsuiteresult;
                    allureSuites.AddOrUpdate(suite.title, suite, (k, oldvalue) =>
                    {
                        oldvalue.testcases = oldvalue.testcases.Concat(suite.testcases).ToArray();
                        return oldvalue;
                    });
                }
            }
        }
        private void ParseSpecFlowFeatures()
        {
            var featuresDir = new DirectoryInfo(@"..\..\Test Features");
            var featureFiles = featuresDir.GetFiles("*.feature");

            foreach (var fileinfo in featureFiles)
            {
                var file = File.ReadLines(fileinfo.FullName);

                var featureTitle = file
                    .First(x => x.StartsWith(FEATURE_PATTERN))
                    .Replace(FEATURE_PATTERN, string.Empty).Trim();

                var testcases = file
                    .Where(x => x.StartsWith(SCENARIO_PATTERN))
                    .Select(x => new testcaseresult()
                    {
                        title = x.Replace(SCENARIO_PATTERN, string.Empty).Trim()
                    });

                var testsuite = new testsuiteresult()
                {
                    title = featureTitle,
                    testcases = testcases.ToArray()
                };

                specflowSuites.AddOrUpdate(featureTitle, testsuite, (k, v) => testsuite);
            }
        }
    }
}
