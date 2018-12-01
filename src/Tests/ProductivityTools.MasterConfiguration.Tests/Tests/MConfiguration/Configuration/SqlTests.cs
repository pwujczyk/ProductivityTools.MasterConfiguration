using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.MasterConfiguration.Models;
using ProductivityTools.MasterConfiguration.SQL;
using ProductivityTools.MasterConfiguration.Tests.Management;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Tests
{
    [TestClass]
    public class SqlTests : BaseTests
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var sql = new SQLAccess();
            sql.CreateDatabaseIfNotExists(DatabaseSetup.DataSourceConnectionString, DatabaseSetup.DatbaseName);
            sql.CreateConfigurationTableIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table);
        }

        [ClassCleanup()]
        public static void AssemblyCleanup()
        {
            new DatabaseSetup().DropDatabase();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            new DatabaseSetup().Truncate();
        }

        private void SetSQLConfigurationDefaultFileName(string name)
        {
            string resultDirectory = $"{AssemblyDirectory}\\{name}";
            Tools.LogToFile("Save SetSqlFile to disk");
            string text = $@"<Configuration>
                                <Source Type=""SQLServer"">
                                    <ConnectionString Value=""{DatabaseSetup.ConnectionString}""/>
                                    <Schema>{DatabaseSetup.Schema}</Schema>
                                    <Table>{DatabaseSetup.Table}</Table>
                                </Source>
                            </Configuration>";
            System.IO.File.WriteAllText(resultDirectory, text);

            Tools.LogToFile("Saved config");
            Tools.LogToFile(text);
            Tools.WriteFile("SaveConfig from file", resultDirectory);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductivityTools.MasterConfiguration.Exceptions.KeyNotExists))]
        public void GetSqlValueWhenNotExists()
        {
            try
            {

                SetSQLConfigurationDefaultFileName(DefaultFileName);

                MConfiguration configuration = new MConfiguration();
                configuration.SetConfigurationFileName(DefaultFileName);
                configuration.SetApplicationName(ApplicationName);
                var x = configuration["randomnotexistskey"];
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                throw;
            }
        }

        [TestMethod]
        public void GetSqlExampleValue()
        {
           // MConfiguration.ResetConfiguration();//previous test can change default file name
           // SetSQLConfigurationDefaultFileName(DefaultFileName);
            string applicationName = "TestApplication";
            var configItem = new ConfigItem() { Key = "examplekey124", Value = "exampleValue123", Application = applicationName, File = DefaultFileName };
            new SQLAccess().InsertValueIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table, configItem);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetApplicationName(applicationName);
            var x = configuration[configItem.Key];
            Assert.AreEqual(configItem.Value, x, "Example value from database");
        }

        [TestMethod]
        public void TwoApplicationsInSql()
        {

            string productionFileXml = "production.xml";
            string testFileXml = "test.xml";
            string applicationName = "TestApplication";
            SetSQLConfigurationDefaultFileName(productionFileXml);
            var configItem = new ConfigItem() { Key = "examplekey124", Value = "exampleValue123", Application = applicationName, File= productionFileXml };
            new SQLAccess().InsertValueIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table, configItem);

            configItem = new ConfigItem() { Key = "examplekey124", Value = "exampleValue123", Application = "TestApplication1", File = testFileXml };
            new SQLAccess().InsertValueIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table, configItem);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(productionFileXml);
            configuration.SetApplicationName(applicationName);
            var x = configuration[configItem.Key];
        }

        [TestMethod]
        public void GetValues()
        {
            string applicationName = "TestApplication";
            var configItem = new ConfigItem() { Key = "examplekey123", Value = "exampleValue123", Application = applicationName, File = DefaultFileName, Category="Category1" };
            new SQLAccess().InsertValueIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table, configItem);

            configItem = new ConfigItem() { Key = "examplekey124", Value = "exampleValue124", Application = applicationName, File = DefaultFileName, Category = "Category2" };
            new SQLAccess().InsertValueIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table, configItem);

            MConfiguration configuration = new MConfiguration();
            configuration.SetConfigurationFileName(DefaultFileName);
            configuration.SetApplicationName(applicationName);

            var result =configuration.GetValues();
            Assert.AreEqual(result.Count, 2);

            result = configuration.GetValues(category: "Category2");
            Assert.AreEqual(result.Single().Value, "exampleValue124");
        }
    }
}
