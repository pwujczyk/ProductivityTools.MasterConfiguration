using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Directors
{
    public class BaseDirector
    {
        protected string ConfigurationFileName;
        protected bool CurrentDomain;

        public BaseDirector(string configurationFileName, string applicationName, bool currentDomain) 
        {
            this.ConfigurationFileName = configurationFileName;
            this.CurrentDomain = currentDomain;
        }

        protected string ConfigurationFilePath
        {
            get
            {
                if (Path.IsPathRooted(ConfigurationFileName))
                {
                    return ConfigurationFileName;
                }
                else
                {
                    string path = Path.Combine(Environment.CurrentDirectory, ConfigurationFileName);
                    return path;
                }
            }
        }
    }
}
