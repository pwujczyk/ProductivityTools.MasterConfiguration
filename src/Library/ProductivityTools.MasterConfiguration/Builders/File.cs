﻿using ProductivityTools.MasterConfiguration.Exceptions;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration.Builders
{
    class File : IBuilder
    {

        public string configurationFile;
        private ConfigSourceLocation ConfigSourceLocation;

        public string ConfigurationFile
        {
            get
            {
                if (string.IsNullOrEmpty(configurationFile))
                {
                    throw new ConfigurationFileNameNotSet();
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

        public File(string configurationFile, ConfigSourceLocation configSourceLocation)
        {
            this.ConfigurationFile = configurationFile;
            this.ConfigSourceLocation = configSourceLocation;
        }

        private string GetAssemblyDirectory
        {
            get
            {
                switch (this.ConfigSourceLocation)
                {
                    case ConfigSourceLocation.ExecutingAssemblyLocation:
                        var executingAssemblylocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        var executingDirectoryName = System.IO.Path.GetDirectoryName(executingAssemblylocation);
                        return executingDirectoryName;
                    case ConfigSourceLocation.CallingAssemblyLocation:
                        var callingAssemblylocation = System.Reflection.Assembly.GetCallingAssembly().Location;
                        var callingDirectoryName = System.IO.Path.GetDirectoryName(callingAssemblylocation);
                        return callingDirectoryName;
                    case ConfigSourceLocation.CurrentDomainBaseDirectory:
                    default:
                        string baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
                        return baseDirectoryPath;
                }
            }
        }

        private string ConfigurationPath
        {
            get
            {
                return System.IO.Path.Combine(GetAssemblyDirectory, ConfigurationFile);
            }
        }

        XDocument xml;
        private XDocument Xml
        {
            get
            {
                this.xml = XDocument.Load(ConfigurationPath);
                return this.xml;
            }
            set
            {
                this.xml.Save(ConfigurationPath);
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

        public IList<ConfigItem> GetAllValues()
        {
            var applicationConfigurationNode = Xml.Root.Elements(ApplicationConfiguration);
            List<ConfigItem> configItemsList = new List<ConfigItem>();
            foreach (var application in applicationConfigurationNode)
            {
                var nameAttribute = application.Attribute("Name");
                string applicationName = nameAttribute?.Value;

                var valueXml = application.Descendants().ToList();

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
            }
            return configItemsList;
        }

        public string GetValue(string key, string application)
        {
            var valueXml = Xml.Descendants(ApplicationConfiguration)
                .Single(x => x.Attribute("Name").Value == application)
                .Descendants("Config").Where(x=>x.Attribute("Key").Value==key).ToList();
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

        private void SaveXml()
        {
            Xml.Save(ConfigurationPath);
        }

        private const string ApplicationKey = "Application";
        private const string ConfigurationKey = "Configuration";
        private const string ApplicationConfiguration = "ApplicationConfiguration";

        public void ValidateApplicationConfigurationNode(string applicationName)
        {
            XDocument document = Xml;
            bool exist = document.Element(ConfigurationKey)
                .Descendants(ApplicationConfiguration)
                .Any(x => x.Attribute("Name").Value == applicationName);
            if (exist == false)
            {
                document.Element(ConfigurationKey).Add
                    (
                      new XElement
                         (
                             ApplicationConfiguration, new XAttribute("Name", applicationName)
                         )
                    );
                this.Xml = document;
                SaveXml();
            }
        }

        public void ValidateKeyNode(string applicationName, string key, string value, string category)
        {
            var document = Xml;
            var applicationNode = document.Element(ConfigurationKey)
                .Elements(ApplicationConfiguration).Single(x => x.Attribute("Name").Value == applicationName)
                .Elements("Config").Where(x => x.Attribute("Key").Value == key).SingleOrDefault();
            if (applicationNode == null)
            {
                document.Element(ConfigurationKey)
                .Elements(ApplicationConfiguration)
                .Single(x => x.Attribute("Name").Value == applicationName)
                .Add
                (
                     new XElement
                         (
                             "Config", value, new XAttribute("Key",key), new XAttribute("Category", category)
                         )
                  );
            }
            else
            {
                applicationNode.Value = value;

            }
            Xml = document;
            SaveXml();
        }



        public void SetValue(string key, string value, string application, string file, string category)
        {
            ValidateApplicationConfigurationNode(application);
            ValidateKeyNode(application, key, value, category);
        }

        public string GetValue(string key, string application, string file)
        {
            throw new NotImplementedException();
        }
    }
}
