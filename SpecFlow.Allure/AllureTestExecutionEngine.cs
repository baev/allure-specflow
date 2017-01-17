using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.BindingSkeletons;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.ErrorHandling;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Tracing;
using TechTalk.SpecFlow.UnitTestProvider;

namespace SpecFlow.Allure
{
    public class AllureTestExecutionEngine : TestExecutionEngine, ITestExecutionEngine
    {
        AllureAdapter adapter = AllureAdapter.Instance;
        public AllureTestExecutionEngine(IStepFormatter stepFormatter, ITestTracer testTracer, IErrorProvider errorProvider, IStepArgumentTypeConverter stepArgumentTypeConverter, RuntimeConfiguration runtimeConfiguration, IBindingRegistry bindingRegistry, IUnitTestRuntimeProvider unitTestRuntimeProvider, IStepDefinitionSkeletonProvider stepDefinitionSkeletonProvider, IContextManager contextManager, IStepDefinitionMatchService stepDefinitionMatchService, IDictionary<string, IStepErrorHandler> stepErrorHandlers, IBindingInvoker bindingInvoker) : base(stepFormatter, testTracer, errorProvider, stepArgumentTypeConverter, runtimeConfiguration, bindingRegistry, unitTestRuntimeProvider, stepDefinitionSkeletonProvider, contextManager, stepDefinitionMatchService, stepErrorHandlers, bindingInvoker)
        {
        }

        void ITestExecutionEngine.OnAfterLastStep()
        {
            try
            {
                base.OnAfterLastStep();
            }
           
            catch (Exception ex)
            {
                var stepDefEx = new MissingStepDefinitionException();
                if (ex.Message.StartsWith(stepDefEx.Message))
                    adapter.PendingTestCase(stepDefEx);

                throw;
            }
        }


    }
}
