﻿using ProductivityTools.MasterConfiguration.Directors;
using ProductivityTools.MasterConfiguration.Extensions;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration
{
    public class MConfiguration
    {
        public string ConfigurationFileName { get; private set; }
        public string ConfiguratoinFileDirectory { get; private set; }
        private string ApplicationName = string.Empty;
        private string EnvironmentVariableName;

        private static ConfigSourceLocation ConfigSourceLocation = ConfigSourceLocation.CurrentDomainBaseDirectory;



        public string this[string key]
        {
            get
            {
                var director = new ConfigurationDirector(ConfigurationFileName, ApplicationName, ConfigSourceLocation);
                return director.GetValue(key, ApplicationName);
            }
        }

        public void SetConfigurationFileName(string configurationFileName)
        {
            ConfigurationFileName = configurationFileName;
        }

        public void SetConfigurationFileDirectory(string configurationFileDirectory)
        {
            this.ConfiguratoinFileDirectory = configurationFileDirectory;
        }

        public void SetApplicationName(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public void SetEnvironmentVariableName(string environmentVariableName)
        {
            EnvironmentVariableName = environmentVariableName;
        }

        public void SetConfigFileConfiguration(ConfigSourceLocation configSourceLocation)
        {
            ConfigSourceLocation = configSourceLocation;
        }

        public void MigrateConfiguration(bool ovverideExistingOnes)
        {
            var director = new MigrationDirector(ConfigurationFileName, ApplicationName, ConfigSourceLocation);
            director.Migrate(ovverideExistingOnes);
        }

        public IList<ConfigItem> GetValues(string application = null, string category = null, string value = null, string key = null)
        {
            var director = new ConfigurationDirector(ConfigurationFileName, ApplicationName, ConfigSourceLocation);
            var result = director.GetAllValues(category, application, value, key);
            return result;
        }

        /// <summary>
        /// It adds or updates value in the configuration source
        /// </summary>
        /// <param name="key">Key which together with file and application identify item</param>
        /// <param name="value">Value</param>
        /// <param name="application">Application which should use given item, if not provided empty string value will be used</param>
        /// <param name="file">File for given application it allows to create different configurations for different environment for the same applicatin. If not provided empty string value will be used</param>
        /// <param name="category">Category for item, it is just for organisation purpose. If not provided empty string value will be used</param>
        public void SetValue(string key, string value, string application = null, string file = "", string category = "")
        {
            var applicationunion = application ?? ApplicationName;
            var director = new ConfigurationDirector(ConfigurationFileName, ApplicationName, ConfigSourceLocation);
            director.SetValue(key, value, applicationunion, file.NormalizeString(), category.NormalizeString());
        }
    }
}
