using ProductivityTools.MasterConfiguration.Builders;
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
        public ConfigurationDirector(string configurationFileName, bool currentDomain) : base(configurationFileName, currentDomain)
        {
        }


        public string GetValue(string key)
        {
            Tools.LogToFile(ConfigurationFileName);
            Tools.WriteFile("ConfigurationDirectorGedValue", ConfigurationFileName);
            File fileBuilder = new File(ConfigurationFileName, CurrentDomain);
            Tools.LogToFile($"SourceType {fileBuilder.SourceType}");
            Tools.WriteFile("GetValueFirst", ConfigurationFileName);
            switch (fileBuilder.SourceType)
            {
                case SourceType.File:
                    return fileBuilder.GetValue(key);
                case SourceType.SqlServer:
                    return new SqlServer(fileBuilder.ConnectionString, fileBuilder.Schema, fileBuilder.Table).GetValue(key);
                case SourceType.HTTP:
                case SourceType.NetPipes:
                default:
                    throw new Exception("wrong type");
            }
        }

    }
}
