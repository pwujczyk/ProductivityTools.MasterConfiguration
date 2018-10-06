﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Directors
{
    public class BaseDirector
    {
        public string ConfigurationFileName;
        public string ApplicationName;
        protected bool CurrentDomain;

        public BaseDirector(string configurationFileName, string applicationName, bool currentDomain)
        {
            this.ConfigurationFileName = configurationFileName;
            this.ApplicationName = applicationName;
            this.CurrentDomain = currentDomain;
        }
    }
}