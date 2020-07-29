using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace ProductivityTools.MasterConfiguration
{
    public static class Class1
    {
        private static IConfigurationBuilder AddMasterConfigurationInternal(this IConfigurationBuilder configurationBuilder, string configName, bool force)
        {
            string aspnetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (force || aspnetCoreEnvironment == "MasterConfiguration")
            {
                string path = Environment.GetEnvironmentVariable("MasterConfigurationPath");
                if (string.IsNullOrEmpty(path))
                {
                    throw new Exception("Application uses MasterConfiguration, but MasterConfigurationPath hadn't been setup. Please setup MasterConfigurationPath environment variable which will point to the directory where master configuration is stored");
                }
                else
                {
                    string filePath = Path.Combine(path, configName);
                    configurationBuilder.AddJsonFile(filePath);
                }
            }
            return configurationBuilder;
        }

        public static IConfigurationBuilder AddMasterConfiguration(this IConfigurationBuilder configurationBuilder, string configName, bool force = false)
        {
            var r = configurationBuilder.AddMasterConfigurationInternal(configName, force);
            return r;
        }

        public static IConfigurationBuilder AddMasterConfiguration(this IConfigurationBuilder configurationBuilder, bool force = false)
        {
            var x = Assembly.GetCallingAssembly();
            var configName = x.GetName().Name;
            var r = configurationBuilder.AddMasterConfigurationInternal($"{configName}.json", force);
            return r;
        }

    }
}
