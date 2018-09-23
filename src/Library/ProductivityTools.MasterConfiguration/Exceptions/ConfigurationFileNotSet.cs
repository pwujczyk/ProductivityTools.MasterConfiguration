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
        public ConfigurationFileNotSet()
        {
        }

        public ConfigurationFileNotSet(string message) : base(message)
        {
        }

        public ConfigurationFileNotSet(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigurationFileNotSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
