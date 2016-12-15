using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.BindingSkeletons;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.Tracing;

namespace SpecFlow.Allure
{
    public class AllureTestTracer : TestTracer, ITestTracer
    {
        AllureAdapter adapter = AllureAdapter.Instance;

        public AllureTestTracer(ITraceListener traceListener, IStepFormatter stepFormatter, IStepDefinitionSkeletonProvider stepDefinitionSkeletonProvider, RuntimeConfiguration runtimeConfiguration) : base(traceListener, stepFormatter, stepDefinitionSkeletonProvider, runtimeConfiguration)
        {
        }

        void ITestTracer.TraceStep(StepInstance stepInstance, bool showAdditionalArguments)
        {
            base.TraceStep(stepInstance, showAdditionalArguments);

            var title = $"{stepInstance.Keyword} {stepInstance.Text}";
            adapter.StartStep(title);
        }

        void ITestTracer.TraceStepDone(BindingMatch match, object[] arguments, TimeSpan duration)
        {
            base.TraceStepDone(match, arguments, duration);
            adapter.FinishStep();
        }

        void ITestTracer.TraceError(Exception ex)
        {
            base.TraceError(ex);
            adapter.FailStep(ex);

        }
        void ITestTracer.TraceStepSkipped()
        {
            base.TraceStepSkipped();
            adapter.CancelStep();
        }

        void ITestTracer.TraceStepPending(BindingMatch match, object[] arguments)
        {
            base.TraceStepPending(match, arguments);
            adapter.CancelStep();

        }
    }
}
