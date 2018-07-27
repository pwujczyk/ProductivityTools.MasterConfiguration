﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            //    new DatabaseSetup().DropDatabase();
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
                var x = MConfiguration.Configuration["randomnotexistskey"];
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
            SetSQLConfigurationDefaultFileName(DefaultFileName);
            var configItem = new ConfigItem() { Key = "examplekey124", Value = "exampleValue123", Application = "TestApplication" };
            new SQLAccess().InsertValueIfNotExists(DatabaseSetup.ConnectionString, DatabaseSetup.Schema, DatabaseSetup.Table, configItem);
            MConfiguration.ResetConfiguraiton();
            var x = MConfiguration.Configuration[configItem.Key];
            Assert.AreEqual(configItem.Value, x, "Example value from database");

        }
    }
}
