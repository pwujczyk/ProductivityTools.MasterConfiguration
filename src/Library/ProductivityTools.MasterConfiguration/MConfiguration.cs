using ProductivityTools.MasterConfiguration.Directors;
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
        //private static string DefaultConfigurationFileName = "Configuration.xml";
        private static string ConfigurationFileName;
        private static string ApplicationName;

        private static bool CurrentDomain = false;

        static MConfiguration()
        {
           // ConfigurationFileName = DefaultConfigurationFileName;
        }

        public static MConfiguration Configuration { get; } = new MConfiguration();

        //public static void ResetConfiguration()
        //{
        //    ConfigurationFileName = DefaultConfigurationFileName;
        //}

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

        public static void SetCurrentDomainPath(bool currentDomain)
        {
            CurrentDomain = currentDomain;
        }

        public static void MigrateConfiguration(bool ovverideExistingOnes)
        {
            var director = new MigrationDirector(ConfigurationFileName, ApplicationName, CurrentDomain);
            director.Migrate(ovverideExistingOnes);
        }
    }
}
