using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Exceptions
{
    public class KeyDeclaredMoreThanOne : Exception
    {
        public KeyDeclaredMoreThanOne()
        {
        }

        public KeyDeclaredMoreThanOne(string message) : base(message)
        {
        }

        public KeyDeclaredMoreThanOne(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyDeclaredMoreThanOne(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
