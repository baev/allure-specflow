using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlow.Allure.Tests
{
    [Binding]
    public class Hooks
    {
        FeatureContext featureContext;
        ScenarioContext scenarioContext;
        public Hooks(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [BeforeFeature(tags: "BeforeFeatureFailed")]
        public static void BeforeFeature()
        {
            throw new Exception();
        }

        [BeforeScenario(tags: "BeforeScenarioFailed")]
        public void BeforeScenario()
        {
            throw new Exception();
        }

        [BeforeStep(tags: "BeforeStepFailed")]
        public void BeforeStep()
        {
            throw new Exception();

        }

        [AfterStep(tags: "AfterStepFailed")]
        public void AfterStep()
        {
            throw new Exception();
        }

        [AfterScenario(tags: "Attachment")]
        public void PutAttachmentIntoContext()
        {
            var types = new HashSet<string>() { "txt", "xml", "html", "png", "jpg", "json" };
            foreach (var item in types)
            {
                var path = $"test.{item}";
                File.WriteAllText(path, "test text");
                scenarioContext.Add($"allure-attachment{item}", path);
            }

            throw new Exception();
        }

    }
}
