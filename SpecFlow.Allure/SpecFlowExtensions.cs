using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlow.Allure
{
    public static class SpecFlowExtensions
    {
        public static void AddAllureAttachment(this ScenarioContext scenarioContext, string filePath, string title = null)
        {
            title = title ?? Guid.NewGuid().ToString();
            scenarioContext.Add(Config.AttachmentKey + Guid.NewGuid().ToString(), Tuple.Create(title, filePath));
        }
    }
}
