using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Allure.Tests.Scenarios
{
    [TestFixture]
    public class IntegrationFixture
    {
        [OneTimeSetUp]
        public void Init()
        {
            var dir = Path.GetDirectoryName(typeof(IntegrationFixture).Assembly.Location);
            Environment.CurrentDirectory = dir;

            DirectoryInfo directoryInfo = new DirectoryInfo(Config.ResultsPath);

            if (directoryInfo.Exists)
                directoryInfo.Delete(true);

            var process = Process.Start(@"..\..\runtests.cmd");
            process.WaitForExit();

        }


        [TestCase("Total: 18")]
        [TestCase("Succeeded: 8")]
        [TestCase("Ignored: 0")]
        [TestCase("Pending: 2")]
        [TestCase("Skipped: 0")]
        [TestCase("Failed: 8")]
        public void CheckNumberOfScenariosByOutcome(string text)
        {
            var log = File.ReadLines("specrun.log");

            Assert.That(log.Contains(text));
        }
    }
}
