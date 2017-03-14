using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.ErrorHandling;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;

namespace SpecFlow.Allure
{
    class AllureBindingInvoker : BindingInvoker
    {
        public AllureBindingInvoker(RuntimeConfiguration runtimeConfiguration, IErrorProvider errorProvider) : base(runtimeConfiguration, errorProvider)
        {
        }

        public override object InvokeBinding(IBinding binding, IContextManager contextManager, object[] arguments, ITestTracer testTracer, out TimeSpan duration)
        {
            var hook = binding as HookBinding;
            var step = binding as StepDefinitionBinding;
            if (hook != null)
            {
                try
                {
                    if (hook.HookType == HookType.BeforeFeature && hook.HookOrder == int.MinValue)
                        AllureAdapter.Instance.StartSuite(contextManager.FeatureContext.FeatureInfo);

                    if (hook.HookType == HookType.AfterFeature && hook.HookOrder == int.MaxValue)
                        AllureAdapter.Instance.FinishSuite(contextManager.FeatureContext.FeatureInfo);

                    return base.InvokeBinding(hook, contextManager, arguments, testTracer, out duration);
                }
                catch (Exception ex)
                {
                    switch (hook.HookType)
                    {
                        case HookType.BeforeTestRun:
                        case HookType.BeforeFeature:
                        case HookType.BeforeScenarioBlock:
                            var scenarioInfo = new ScenarioInfo(hook.HookType.ToString());
                            AllureAdapter.Instance.FailTestSuite(contextManager.FeatureContext.FeatureInfo, scenarioInfo, ex);
                            break;

                        case HookType.BeforeScenario:
                            AllureAdapter.Instance.CancelTestCase(ex);
                            break;

                        case HookType.AfterScenario:
                        case HookType.AfterStep:
                            AllureAdapter.Instance.FinishTestCase(contextManager.ScenarioContext, ex);
                            break;

                        case HookType.BeforeStep:
                        case HookType.AfterScenarioBlock:
                        case HookType.AfterFeature:
                        case HookType.AfterTestRun:
                        default:
                            break;
                    }
                    throw;
                }

            }

            return base.InvokeBinding(binding, contextManager, arguments, testTracer, out duration);
        }
    }
}
