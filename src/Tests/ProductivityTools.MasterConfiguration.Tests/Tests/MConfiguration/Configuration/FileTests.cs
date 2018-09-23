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
                                <ApplicationConfiguration>
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
                                <ApplicationConfiguration>
                                    <Key1>Value1</Key1>
                                    <Key1>Value1</Key1>
                                    <Key2>Value2</Key2>
                                </ApplicationConfiguration>
                            </Configuration>";
            System.IO.File.WriteAllText($"{AssemblyDirectory}\\{name}", text);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotExists))]
        public void GetNonExistsFileValue()
        {
            SetFileConfiguration(DefaultFileName);
            var x = MConfiguration.Configuration["NotExists"];
        }

        [TestMethod]
        [ExpectedException(typeof(KeyDeclaredMoreThanOne))]
        public void GetValueDeclaredTwice()
        {
            SetFileConfigurationWithTwoSameValues(DefaultFileName);
            var x = MConfiguration.Configuration["Key1"];
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationFileNotSet))]
        public void GetFileValueThrowsException()
        {
            var x = MConfiguration.Configuration["Key1"];
        }

        [TestMethod]
        public void GetFileValue()
        {
            SetFileConfiguration(DefaultFileName);

            MConfiguration.SetConfigurationFileName(DefaultFileName);
            var x = MConfiguration.Configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
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
    }
}
