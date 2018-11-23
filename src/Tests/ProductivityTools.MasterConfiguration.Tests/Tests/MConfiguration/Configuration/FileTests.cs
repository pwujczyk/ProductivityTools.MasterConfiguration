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

        [TestMethod]
        [ExpectedException(typeof(KeyNotExists))]
        public void GetNonExistsFileValue()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetApplicationName(ApplicationName);
            MConfiguration.SetConfigurationFileName(DefaultFileName);
            var x = MConfiguration.Configuration["NotExists"];
        }

        [TestMethod]
        [ExpectedException(typeof(KeyDeclaredMoreThanOne))]
        public void GetValueDeclaredTwice()
        {
            SetFileConfigurationWithTwoSameValues(DefaultFileName);

            MConfiguration.SetApplicationName(ApplicationName);
            MConfiguration.SetConfigurationFileName(DefaultFileName);
            var x = MConfiguration.Configuration["Key1"];
        }

        [TestMethod]
        public void GetFileValue()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetConfigurationFileName(DefaultFileName);
            MConfiguration.SetApplicationName(ApplicationName);
            var x = MConfiguration.Configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }

        [TestMethod]
        public void GetValues()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetConfigurationFileName(DefaultFileName);
            var result = MConfiguration.GetValues();
            Assert.AreEqual(result.Count, 2);

            result = MConfiguration.GetValues(category: "Category2");
            Assert.AreEqual(result.Single().Value, "Value2");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetFileValueFromDifferentConfigurationFileAnfFileMissing()
        {
            ClearDirectoryFromConfigs();

            MConfiguration.SetConfigurationFileName("Pawel.xml");
            var x = MConfiguration.Configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }

        [TestMethod]
        public void GetFileValueFromDifferentConfigurationFile()
        {
            ClearDirectoryFromConfigs();
            string differentFileName = "Pawel.xml";
            SetFileConfiguration(differentFileName);

            MConfiguration.SetConfigurationFileName(differentFileName);
            var x = MConfiguration.Configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }

        [TestMethod]
        public void SetValueTest()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetApplicationName(ApplicationName);
            MConfiguration.SetConfigurationFileName(DefaultFileName);
            MConfiguration.SetValue("KeySetValue1", "Value1");

            var x = MConfiguration.Configuration["KeySetValue1"];
            Assert.AreEqual("Value1", x);
        }

        [TestMethod]
        public void SetValueUpdateTest()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetApplicationName(ApplicationName);
            MConfiguration.SetConfigurationFileName(DefaultFileName);
            MConfiguration.SetValue("Key1", "XXX");

            var x = MConfiguration.Configuration["Key1"];
            Assert.AreEqual("XXX", x);
        }


        [TestMethod]
        public void SetValueTestForNotExistsApplication()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetApplicationName("NotExistsApplication");
            MConfiguration.SetConfigurationFileName(DefaultFileName);
            MConfiguration.SetValue("KeySetValue1", "Value1","NotExistsApplication");

            var x = MConfiguration.Configuration["KeySetValue1"];
            Assert.AreEqual("Value1", x);
        }
    }
}
