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
        private static bool CurrentDomain = false;
        static MConfiguration configuration = new MConfiguration();

        static MConfiguration()
        {
            ConfigurationFileName = "Configuration.dll.config";
        }

        public static MConfiguration Configuration
        {
            get
            {
                return configuration;
            }
        }

        public string this[string key]
        {
            get
            {
                var director = new ConfigurationDirector(ConfigurationFileName, CurrentDomain);
                return director.GetValue(key);
            }
        }

        public static void SetConfigurationName(string configurationFileName)
        {
            ConfigurationFileName = configurationFileName;
        }

        public static void SetCurrentDomainPath(bool currentDomain)
        {
            CurrentDomain = currentDomain;
        }
    }
}
