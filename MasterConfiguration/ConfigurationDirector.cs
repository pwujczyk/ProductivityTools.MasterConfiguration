using ProductivityTools.MasterConfiguration.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration
{
    class ConfigurationDirector
    {
        public string ConfigurationFileName;
        private bool CurrentDomain;

        public ConfigurationDirector(string configurationFileName, bool currentDomain)
        {
            this.ConfigurationFileName = configurationFileName;
            this.CurrentDomain = currentDomain;
        }


        public string GetValue(string key)
        {
            File fileBuilder = new File(ConfigurationFileName, CurrentDomain);
            switch (fileBuilder.SourceType)
            {
                case SourceType.File:
                    return fileBuilder.GetValue(key);
                case SourceType.SqlServer:
                case SourceType.HTTP:
                case SourceType.NetPipes:
                default:
                    throw new Exception("wrong type");
            }
        }

    }
}
