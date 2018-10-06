using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Exceptions
{
    public class ConfigurationFileNameNotSet : Exception
    {
        private static string Mesage = "Please setup configuration file using MConfiguration.SetConfigurationFileName(DefaultFileName);";

        private static Func<string,string> exception = (message) => string.IsNullOrEmpty(message) ? Mesage : message;
        public ConfigurationFileNameNotSet() : base(Mesage)
        {
        }

        public ConfigurationFileNameNotSet(string message) : base(exception(message))
        {
        }

        public ConfigurationFileNameNotSet(string message, Exception innerException) : base(exception(message) , innerException)
        {
        }

        protected ConfigurationFileNameNotSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
