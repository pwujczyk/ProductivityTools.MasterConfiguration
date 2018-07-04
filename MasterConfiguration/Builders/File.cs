using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MasterConfiguration.Builders
{
    class File
    {
        public string ConfigurationFile;
        private bool CurrentDomain;

        public File(string configurationFile, bool currentDomain)
        {
            this.ConfigurationFile = configurationFile;
            this.CurrentDomain = currentDomain;
        }
        private string GetAssemblyDirectory
        {
            get
            {
                if (this.CurrentDomain)
                {
                    string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
                    return baseDirectoryPath;
                }
                else
                {
                    var assemblylocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var directoryName = System.IO.Path.GetDirectoryName(assemblylocation);
                    return directoryName;
                }
            }
        }

        private string GetConfigurationPath
        {
            get
            {
                return System.IO.Path.Combine(GetAssemblyDirectory, ConfigurationFile);
            }
        }

        private XDocument Xml
        {
            get
            {
                var xml = XDocument.Load(GetConfigurationPath);
                return xml;

            }
        }

        public SourceType SourceType
        {
            get
            {
                var query = (from c in Xml.Root.Descendants("Source") select c).Single();
                if (query.Value == "ConfigurationFile")
                {
                    return SourceType.File;
                }
                throw new Exception("not suported sourceType");
            }
        }

        public string GetValue(string key)
        {
            var query = Xml.Descendants("ApplicationConfiguration").Descendants(key).Single();
            return query.Value;
        }
    }
}
