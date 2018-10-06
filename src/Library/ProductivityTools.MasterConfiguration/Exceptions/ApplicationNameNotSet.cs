using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Exceptions
{
    public class ApplicationNameNotSet : Exception
    {
        private static string Mesage = "Please setup application name using MConfiguration.SetApplicationName(applicationName);";

        private static Func<string, string> exception = (message) => string.IsNullOrEmpty(message) ? Mesage : message;
        public ApplicationNameNotSet() : base(Mesage)
        {
        }

        public ApplicationNameNotSet(string message) : base(exception(message))
        {
        }

        public ApplicationNameNotSet(string message, Exception innerException) : base(exception(message), innerException)
        {
        }

        protected ApplicationNameNotSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
