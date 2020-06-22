using Microsoft.Extensions.Configuration;
using System;

namespace ProductivityTools.MasterConfiguration
{
    public static class Class1
    {
        public static IConfigurationBuilder AddMasterConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile(@"d:\ProductivityTools.Bank.Millenium.json");
            return configurationBuilder;
        }
    }
}
