using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.MasterConfiguration.SQL;
using ProductivityTools.MasterConfiguration.Tests.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Tests
{ 
    [TestClass]
    public class FileToSqlTest: BaseTests
    {

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var sql = new SQLAccess();
            sql.CreateDatabaseIfNotExists(DatabaseSetup.DataSourceConnectionString, DatabaseSetup.DatbaseName);
            sql.CreateConfigurationTableIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table);
        }

        private void SetSQLConfigurationToMigrate(string name)
        {
            string resultDirectory = $"{AssemblyDirectory}\\{name}";
            Tools.LogToFile("Save SetSqlFile to disk");
            string text = $@"<Configuration>
                                <Source Type=""SQLServer"">
                                    <ConnectionString Value=""{DatabaseSetup.ConnectionString}""/>
                                    <Schema>{DatabaseSetup.Schema}</Schema>
                                    <Table>{DatabaseSetup.Table}</Table>
                                </Source>
                                <ApplicationConfiguration Name=""Application1"">
                                    <Key1 Category=""Category1"">Value1</Key1>
                                    <Key2 Category=""Category1"">Value2</Key2>
                                </ApplicationConfiguration>
                            </Configuration>";
            System.IO.File.WriteAllText(resultDirectory, text);

            Tools.LogToFile("Saved config");
            Tools.LogToFile(text);
            Tools.WriteFile("SaveConfig from file", resultDirectory);
        }

        private void SetSQLConfigurationToMigrateSecondOne(string name)
        {
            string resultDirectory = $"{AssemblyDirectory}\\{name}";
            Tools.LogToFile("Save SetSqlFile to disk");
            string text = $@"<Configuration>
                                <Source Type=""SQLServer"">
                                    <ConnectionString Value=""{DatabaseSetup.ConnectionString}""/>
                                    <Schema>{DatabaseSetup.Schema}</Schema>
                                    <Table>{DatabaseSetup.Table}</Table>
                                </Source>
                                <ApplicationConfiguration Name=""Application1"">
                                    <Key1 Category=""Category1"">Value11</Key1>
                                    <Key2 Category=""Category2"">Value22</Key2>
                                    <Key3 Category=""Category3"">Value33</Key3>
                                </ApplicationConfiguration>
                            </Configuration>";
            System.IO.File.WriteAllText(resultDirectory, text);

            Tools.LogToFile("Saved config");
            Tools.LogToFile(text);
            Tools.WriteFile("SaveConfig from file", resultDirectory);
        }

        [TestMethod]
        public void PerformOneTimeMigration()
        {
            SetSQLConfigurationToMigrate(DefaultFileName);
            MasterConfiguration.MConfiguration.MigrateConfiguration(ovverideExistingOnes:true);
            var result=MasterConfiguration.MConfiguration.Configuration["Key1"];
            Assert.AreEqual(result, "Value1");
        }

        [TestMethod]
        public void PerformMigrationAndSecondOneWithOverrideExistingOnes()
        {
            SetSQLConfigurationToMigrate(DefaultFileName);

            MConfiguration.SetConfigurationFileName(DefaultFileName);
            MConfiguration.SetApplicationName(ApplicationName);
            MConfiguration.MigrateConfiguration(ovverideExistingOnes: true);

            var value1result = MasterConfiguration.MConfiguration.Configuration["Key1"];
            Assert.AreEqual(value1result, "Value1");
            var value2result = MasterConfiguration.MConfiguration.Configuration["Key2"];
            Assert.AreEqual(value2result, "Value2");

            SetSQLConfigurationToMigrateSecondOne(DefaultFileName);
            MConfiguration.MigrateConfiguration(ovverideExistingOnes: true);

            value1result = MasterConfiguration.MConfiguration.Configuration["Key1"];
            Assert.AreEqual(value1result, "Value11");
            value2result = MasterConfiguration.MConfiguration.Configuration["Key2"];
            Assert.AreEqual(value2result, "Value22");
            var value3result = MasterConfiguration.MConfiguration.Configuration["Key3"];
            Assert.AreEqual(value3result, "Value33");
        }

        [TestMethod]
        public void PerformMigrationAndSecondOneWithAddNewOnes()
        {
            SetSQLConfigurationToMigrate(DefaultFileName);


            MConfiguration.SetConfigurationFileName(DefaultFileName);
            MConfiguration.SetApplicationName(ApplicationName);
            MConfiguration.MigrateConfiguration(ovverideExistingOnes: true);
            var value1result = MasterConfiguration.MConfiguration.Configuration["Key1"];
            Assert.AreEqual(value1result, "Value1");
            var value2result = MasterConfiguration.MConfiguration.Configuration["Key2"];
            Assert.AreEqual(value2result, "Value2");

            SetSQLConfigurationToMigrateSecondOne(DefaultFileName);
            MasterConfiguration.MConfiguration.MigrateConfiguration(ovverideExistingOnes: false);

            value1result = MasterConfiguration.MConfiguration.Configuration["Key1"];
            Assert.AreEqual(value1result, "Value1");
            value2result = MasterConfiguration.MConfiguration.Configuration["Key2"];
            Assert.AreEqual(value2result, "Value2");
            var value3result = MasterConfiguration.MConfiguration.Configuration["Key3"];
            Assert.AreEqual(value3result, "Value33");
        }
    }
}
