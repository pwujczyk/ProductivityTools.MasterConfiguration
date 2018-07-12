using ProductivityTools.MasterConfiguration.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration.Builders
{
    class File : IFile
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
                var xx = Xml.Root.Element("Source");
                if (xx.Attribute("Type").Value == "File")
                {
                    return SourceType.File;
                }
                throw new Exception("not suported sourceType");
            }
        }

        public string GetValue(string key)
        {
            var valueXml = Xml.Descendants("ApplicationConfiguration").Descendants(key).ToList();
            if (valueXml.Any()==false)
            {
                throw new KeyNotExists($"Key {key} doesn't exist in the config file");
            }
            if (valueXml.Count()>1)
            {
                throw new KeyDeclaredMoreThanOne($"Key {key} is declared more than once in config file");
            }
            var query = valueXml.Single();
            return query.Value;
        }
    }
}
