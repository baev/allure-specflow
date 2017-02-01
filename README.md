# SpecFlow.Allure
SpecFlow adapter for Yandex Allure reporting system

Nuget package:
```
PM> Install-Package SpecFlow.Allure
```
### How to add attachments
Please use ScenarioContext.AddAllureAttachment extension method to add attachment to Allure test case result:
```
using SpecFlow.Allure;
using TechTalk.SpecFlow;
...
ScenarioContext scenarioContext;
scenarioContext.AddAllureAttachment(path, "Attachment Title");
```

