using ProductivityTools.MasterConfiguration.Exceptions;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration.Builders
{
    class File
    {
        private const string ApplicationConfiguration = "ApplicationConfiguration";

        public string configurationFile;
        private bool CurrentDomain;

        public string ConfigurationFile
        {
            get
            {
                if (string.IsNullOrEmpty(configurationFile))
                {
                    throw new ConfigurationFileNotSet();
                }
                return configurationFile;
            }
            private set
            {
                this.configurationFile = value;
            }
        }

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

        //public string FileName
        //{
        //    get
        //    {
        //        var s = System.IO.Path.GetFileNameWithoutExtension(this.ConfigurationFile);
        //        return s;
        //    }
        //}

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
                var sourceTypeString = SourceElement.Attribute("Type").Value;
                switch (sourceTypeString)
                {
                    case "File": return SourceType.File;
                    case "SQLServer": return SourceType.SqlServer;
                    default:
                        throw new Exception("not suported sourceType");
                }
            }
        }

        public List<ConfigItem> GetAllValues()
        {
            var applicationConfigurationNode = Xml.Root.Element(ApplicationConfiguration);
            var nameAttribute = applicationConfigurationNode.Attribute("Name");
            string applicationName = nameAttribute?.Value;

            var valueXml = applicationConfigurationNode.Descendants().ToList();
            List<ConfigItem> configItemsList = new List<ConfigItem>();
            foreach (var item in valueXml)
            {
                var config = new ConfigItem();
                config.Key = item.Name.LocalName;
                config.Value = item.Value;
                config.Application = applicationName;
                config.File = this.ConfigurationFile;
                config.Category = item.Attribute("Category")?.Value;
                configItemsList.Add(config);
            }

            return configItemsList;
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
