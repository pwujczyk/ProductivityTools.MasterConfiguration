using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Tests
{
    public class BaseTests
    {
        protected string DefaultFileName = "Configuration.xml";
        protected static string AssemblyDirectory
        {
            get
            {
                string path = Assembly.GetExecutingAssembly().Location;
                return Path.GetDirectoryName(path);
            }
        }

        public Mutex TestMutex => testMutex;

        private readonly Mutex testMutex = new Mutex(true, "MySpecificTestScenarioUniqueMutexString");

        [TestInitialize]
        public void Initialize()
        {
            testMutex.WaitOne(TimeSpan.FromSeconds(1));
        }

        [TestCleanup]
        public void Cleanup()
        {
            testMutex.ReleaseMutex();
        }

     
    }
}
