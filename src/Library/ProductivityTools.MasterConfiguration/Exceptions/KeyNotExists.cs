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
        const string message = "Given key doesn't exist in the config file: {0}";

        public KeyNotExists()
        {

        }

        public KeyNotExists(string key) : base(string.Format(message, key))
        {
            this.Data.Add("Key not found:", key);
        }

        public KeyNotExists(string key, Exception innerException) : base(string.Format(message, key), innerException)
        {
            this.Data.Add("Key not found:", key);
        }

        protected KeyNotExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
