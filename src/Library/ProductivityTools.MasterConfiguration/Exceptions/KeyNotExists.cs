using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Exceptions
{
    public class KeyNotExists : Exception
    {
        public KeyNotExists()
        {
        }

        public KeyNotExists(string message) : base(message)
        {

        }

        public KeyNotExists(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyNotExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
