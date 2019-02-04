﻿using ProductivityTools.MasterConfiguration.Builders;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductivityTools.MasterConfiguration.Directors
{
    class ConfigurationDirector : BaseDirector
    {
        public ConfigurationDirector(string configurationFileName, string applicationName, ConfigSourceLocation configSourceLocation) : base(configurationFileName, applicationName, configSourceLocation)
        {
        }

        public IList<ConfigItem> GetAllValues(string category, string application, string value, string key)
        {
            IEnumerable<ConfigItem> result = GetBuilder().GetAllValues();
            Action<string, Func<ConfigItem, bool>> a = (filter, action) =>
               {
                   if (!string.IsNullOrEmpty(filter))
                   {
                       result = result.Where(action);
                   }
               };
            a(category, x => x.Category == category);
            a(application, x => x.Application == application);
            a(value, x => x.Value == value);
            a(key, x => x.Key == key);

            return result.ToList();
        }

        private IBuilder GetBuilder()
        {
            File fileBuilder = new File(ConfigurationFileName, configSourceLocation);

            switch (fileBuilder.SourceType)
            {
                case SourceType.File:
                    return fileBuilder;
                case SourceType.SqlServer:
                    return new SqlServer(fileBuilder.ConnectionString, fileBuilder.Schema, fileBuilder.Table);
                default:
                    throw new Exception("Wrong type");
            }
        }

        public string GetValue(string key, string applicationName)
        {
            File fileBuilder = new File(ConfigurationFileName, configSourceLocation);
            switch (fileBuilder.SourceType)
            {
                case SourceType.File:
                    return fileBuilder.GetValue(key, applicationName);
                case SourceType.SqlServer:
                    return new SqlServer(fileBuilder.ConnectionString, fileBuilder.Schema, fileBuilder.Table).GetValue(key, ConfigurationFileName, applicationName);
                default:
                    throw new Exception("Wrong type");
            }
        }

        public void SetValue(string key, string value, string application, string file, string category)
        {
            File fileBuilder = new File(ConfigurationFileName, configSourceLocation);
            switch (fileBuilder.SourceType)
            {
                case SourceType.File:
                    fileBuilder.SetValue(key, value, application, file, category);
                    return;
                case SourceType.SqlServer:
                    new SqlServer(fileBuilder.ConnectionString, fileBuilder.Schema, fileBuilder.Table).SetValue(key, value, application, file, category);
                    return;
                default:
                    throw new Exception("Wrong type");
            }
        }

    }
}
