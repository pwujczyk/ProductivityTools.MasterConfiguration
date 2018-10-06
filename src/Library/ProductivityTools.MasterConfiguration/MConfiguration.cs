using ProductivityTools.MasterConfiguration.Directors;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration
{
    public class MConfiguration
    {
        private static string ConfigurationFileName;
        private static string ApplicationName;
        private static string EnvironmentVariableName;

        private static bool CurrentDomain = false;

        static MConfiguration()
        {
        }

        public static MConfiguration Configuration { get; } = new MConfiguration();

        public string this[string key]
        {
            get
            {
                var director = new ConfigurationDirector(ConfigurationFileName, ApplicationName, CurrentDomain);
                return director.GetValue(key);
            }
        }

        public static void SetConfigurationFileName(string configurationFileName)
        {
            ConfigurationFileName = configurationFileName;
        }

        public static void SetApplicationName(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public static void SetEnvironmentVariableName(string environmentVariableName)
        {
            EnvironmentVariableName = environmentVariableName;
        }

        public static void SetCurrentDomainPath(bool currentDomain)
        {
            CurrentDomain = currentDomain;
        }

        public static void MigrateConfiguration(bool ovverideExistingOnes)
        {
            var director = new MigrationDirector(ConfigurationFileName, ApplicationName, CurrentDomain);
            director.Migrate(ovverideExistingOnes);
        }

        public static IList<ConfigItem> GetValues(string category=null, string application=null, string file=null, string value=null, string key=null)
        {
            var director = new ConfigurationDirector(ConfigurationFileName, ApplicationName, CurrentDomain);
            var result= director.GetAllValues(category,application,file,value,key);
            return result;
        }
    }
}
