using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Directors
{
    public class BaseDirector
    {
        public string ConfigurationFileName;
        protected bool CurrentDomain;

        public BaseDirector(string configurationFileName, bool currentDomain)
        {
            this.ConfigurationFileName = configurationFileName;
            this.CurrentDomain = currentDomain;
        }
    }
}
