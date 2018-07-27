using ProductivityTools.MasterConfiguration.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration.Builders
{
    class File //: IFile
    {
        private const string ApplicationConfiguration = "ApplicationConfiguration";

        public string ConfigurationFile;
        private bool CurrentDomain;

        public string ConnectionString
        {
            get
            {

                var connectionStringNode = SourceElement.Element("ConnectionString");
                var connectionString = connectionStringNode.Attribute("Value");
                return connectionString.Value;
            }
        }


        public string Schema
        {
            get
            {
                var schema = SourceElement.Element("Schema");
                return schema.Value;
            }
        }

        public string Table
        {
            get
            {
                var tableName = SourceElement.Element("Table");
                return tableName.Value;
            }
        }

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

        private XElement SourceElement
        {
            get
            {
                var source = Xml.Root.Element("Source");
                return source;
            }
        }

        public SourceType SourceType
        {
            get
            {
                //pw:change it

                var sourceTypeString = SourceElement.Attribute("Type").Value;
                if (sourceTypeString == "File")
                {
                    return SourceType.File;
                }
                if (sourceTypeString == "SQLServer")
                {
                    return SourceType.SqlServer;
                }
                throw new Exception("not suported sourceType");
            }
        }

        private void GetConnectionString(XElement xx)
        {
        }

        //pw:todo
        public Dictionary<string, string> GetAllValues()
        {
            var valueXml = Xml.Descendants(ApplicationConfiguration).Descendants().ToList();
            Dictionary<string, string> x = new Dictionary<string, string>();
            foreach (var item in valueXml)
            {
                string key = item.Name.LocalName;
                string value = item.Value;
                x.Add(key, value);
            }

            return x;

        }

        public string GetValue(string key)
        {
            var valueXml = Xml.Descendants(ApplicationConfiguration).Descendants(key).ToList();
            if (valueXml.Any() == false)
            {
                throw new KeyNotExists(key);
            }
            if (valueXml.Count() > 1)
            {
                throw new KeyDeclaredMoreThanOne($"Key {key} is declared more than once in config file");
            }
            var query = valueXml.Single();
            return query.Value;
        }
    }
}
