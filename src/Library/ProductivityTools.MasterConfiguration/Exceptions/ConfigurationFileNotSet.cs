using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Exceptions
{
    public class ConfigurationFileNotSet : Exception
    {
        private static string Mesage = "Please setup configuration file using MConfiguration.SetConfigurationFileName(DefaultFileName);";

        private static Func<string,string> exception = (message) => string.IsNullOrEmpty(message) ? Mesage : message;
        public ConfigurationFileNotSet() : base(Mesage)
        {
        }

        public ConfigurationFileNotSet(string message) : base(exception(message))
        {
        }

        public ConfigurationFileNotSet(string message, Exception innerException) : base(exception(message) , innerException)
        {
        }

        protected ConfigurationFileNotSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
