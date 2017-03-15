# SpecFlow.Allure [![Build status](https://ci.appveyor.com/api/projects/status/fu1b367fiiayjc5e?svg=true)](https://ci.appveyor.com/project/Bakanych/allure-specflow)
SpecFlow adapter for Yandex Allure reporting system.

### The nuget package [![NuGet Status](http://img.shields.io/nuget/v/Specflow.Allure.svg?style=flat)](https://www.nuget.org/packages/Specflow.Allure/)
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
