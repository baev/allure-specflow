using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Allure
{
    public static class Config
    {
        static AppSettingsSection appsettings;
        static string resultsPath;

        public static readonly string AttachmentKey = "55B8549C-1CE3-473B-A8A4-52FD2AEA58F7";
        public static string ResultsPath => resultsPath;

        static Config()
        {
            try
            {
                appsettings = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(AllureAdapter)).Location).AppSettings;
                resultsPath = appsettings?.Settings["resultsPath"]?.Value;
                if (resultsPath == null)
                    throw new Exception("Cannot read 'resultsPath' key");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during read Specflow.Allure plugin config: {ex}");
            }
        }

    }
}
