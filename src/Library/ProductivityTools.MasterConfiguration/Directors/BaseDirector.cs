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
        protected ConfigSourceLocation configSourceLocation;

        public BaseDirector(string configurationFileName, string applicationName, ConfigSourceLocation configSourceLocation) 
        {
            this.ConfigurationFileName = configurationFileName;
            this.configSourceLocation = configSourceLocation;
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
