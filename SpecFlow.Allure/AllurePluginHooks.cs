using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlow.Allure
{
    [Binding]
    public class AllurePluginHooks
    {
        FeatureContext featureContext;
        ScenarioContext scenarioContext;
        AllureAdapter adapter = AllureAdapter.Instance;

        public AllurePluginHooks(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [BeforeFeature(Order = int.MinValue)]
        public static void BeforeFeature()
        {
        }

        [BeforeScenario(Order = int.MinValue)]
        public void BeforeScenario()
        {
            adapter.StartTestCase(featureContext.FeatureInfo, scenarioContext.ScenarioInfo);
        }

        [AfterScenario(Order = int.MaxValue)]
        public void AfterScenario()
        {
            adapter.FinishTestCase(featureContext.FeatureInfo, scenarioContext);
        }

        [AfterFeature(Order = int.MaxValue)]
        public static void AfterFeature()
        {
        }

        [AfterTestRun(Order = int.MaxValue)]
        public static void AfterTestRun()
        {
        }


    }
}
