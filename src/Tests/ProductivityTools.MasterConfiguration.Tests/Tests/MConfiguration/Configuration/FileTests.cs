using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.MasterConfiguration.Exceptions;
using ProductivityTools.MasterConfiguration;

namespace ProductivityTools.MasterConfiguration.Tests
{
    [TestClass]
    public class FileUnitTests : BaseTests
    {
        private const string ApplicationName = "ApplicationName";

        private void ClearDirectoryFromConfigs()
        {
            var files = Directory.GetFiles(AssemblyDirectory);
            foreach (string file in files.Where(item => item.EndsWith(".xml")))
            {
                File.Delete(file);
            }
        }



        private void SetFileConfiguration(string name)
        {
            Tools.LogToFile("SetFile");
            string text = @"<Configuration>
                                <Source Type=""File""></Source>
                                <ApplicationConfiguration Name=""ApplicationName"">
                                    <Key1>Value1</Key1>
                                    <Key2 Category=""Category2"">Value2</Key2>
                                </ApplicationConfiguration>
                            </Configuration>";
            System.IO.File.WriteAllText($"{AssemblyDirectory}\\{name}", text);
        }

        private void SetFileConfigurationWithTwoSameValues(string name)
        {
            Tools.LogToFile("SetFile");
            string text = @"<Configuration>
                                <Source Type=""File""></Source>
                                <ApplicationConfiguration Name=""ApplicationName"">
                                    <Key1 Category=""Category1"">Value11</Key1>
                                    <Key1 Category=""Category2"">Value11</Key1>
                                    <Key3 Category=""Category3"">Value33</Key3>
                                </ApplicationConfiguration>
                            </Configuration>";
            System.IO.File.WriteAllText($"{AssemblyDirectory}\\{name}", text);
        }

        private void SetFileConfigurationWithTwoApplications(string name)
        {
            Tools.LogToFile("SetFile");
            string text = @"<Configuration>
                                <Source Type=""File""></Source>
                                <ApplicationConfiguration Name=""ApplicationName1"">
                                    <App11 Category=""Category1"">Value11</App11>
                                    <App12 Category=""Category2"">Value11</App12>
                                    <App13 Category=""Category3"">Value33</App13>
                                </ApplicationConfiguration>
                                <ApplicationConfiguration Name=""ApplicationName2"">
                                    <App21 Category=""app2"">Value11</App21>
                                    <App22 Category=""Category2"">Value11</App22>
                                    <App23 Category=""Category3"">Value33</App23>
                                </ApplicationConfiguration>
                            </Configuration>";
            System.IO.File.WriteAllText($"{AssemblyDirectory}\\{name}", text);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotExists))]
        public void GetNonExistsFileValue()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetApplicationName(ApplicationName);
            configuration.SetConfigurationFileName(DefaultFileName);
            var x = configuration["NotExists"];
        }

        [TestMethod]
        [ExpectedException(typeof(KeyDeclaredMoreThanOne))]
        public void GetValueDeclaredTwice()
        {
            SetFileConfigurationWithTwoSameValues(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetApplicationName(ApplicationName);
            configuration.SetConfigurationFileName(DefaultFileName);
            var x = configuration["Key1"];
        }

        [TestMethod]
        public void GetFileValue()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetApplicationName(ApplicationName);
            var x = configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }

        [TestMethod]
        public void GetValues()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(DefaultFileName);
            var result = configuration.GetValues();
            Assert.AreEqual(result.Count, 2);

            result = configuration.GetValues(category: "Category2");
            Assert.AreEqual(result.Single().Value, "Value2");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetFileValueFromDifferentConfigurationFileAnfFileMissing()
        {
            ClearDirectoryFromConfigs();

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName("Pawel.xml");
            var x = configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }

        [TestMethod]
        public void GetFileValueFromDifferentConfigurationFile()
        {
            ClearDirectoryFromConfigs();
            string differentFileName = "Pawel.xml";
            SetFileConfiguration(differentFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(differentFileName);
            configuration.SetApplicationName(ApplicationName);
            var x = configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }

        [TestMethod]
        public void GetFileValueFromSecondApplication()
        {
            ClearDirectoryFromConfigs();
            SetFileConfigurationWithTwoApplications(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(DefaultFileName);
            var x = configuration.GetValues("ApplicationName2");
            Assert.AreEqual(x[0].Value, "Value11");
        }

        [TestMethod]
        public void SetValueTest()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetApplicationName(ApplicationName);
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetValue("KeySetValue1", "Value1");

            var x = configuration["KeySetValue1"];
            Assert.AreEqual("Value1", x);
        }

        [TestMethod]
        public void SetValueUpdateTest()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetApplicationName(ApplicationName);
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetValue("Key1", "XXX");

            var x = configuration["Key1"];
            Assert.AreEqual("XXX", x);
        }

        [TestMethod]
        public void SetValueTestForNotExistsApplication()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetApplicationName("NotExistsApplication");
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetValue("KeySetValue1", "Value1", "NotExistsApplication");

            var x = configuration["KeySetValue1"];
            Assert.AreEqual("Value1", x);
        }

        [TestMethod]
        public void SetValueWithNullfields()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetValue("KeySetValue1", "Value1", null, null, null);
            

            var x = configuration["KeySetValue1"];
            Assert.AreEqual("Value1", x);
        }
    }
}
