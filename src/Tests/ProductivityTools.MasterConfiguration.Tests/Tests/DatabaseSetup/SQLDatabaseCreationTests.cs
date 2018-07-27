using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.MasterConfiguration.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Tests.SQL
{
    [TestClass]
    public class SQLDatabaseCreationTests
    {
        [TestMethod]
        [DataRow("master", true)]
        [DataRow(DatabaseSetup.Table, false)]
        
        public void CheckIfDatabseExists(string tableName, bool exists)
        {
            var r = DatabaseSetup.CheckIfDatabaseExists(DatabaseSetup.DataSourceConnectionString, tableName);
            Assert.AreEqual(r, exists);
        }

        //[TestMethod]
        //// [Ignore]
        //public void CreateDatabaseIfNotExists_Test()
        //{
        //    SQLAccess s = new SQLAccess();
        //    var notExists = DatabaseSetup.CheckIfDatabaseExists(DatabaseSetup.DataSourceConnectionString, DatabaseSetup.DatbaseName);
        //    Assert.IsFalse(notExists, "FirstAssert in CreateDatabaseIfNotExists_Test");
        //    s.CreateDatabaseIfNotExists(DatabaseSetup.DataSourceConnectionString, DatabaseSetup.DatbaseName);
        //    var exists = DatabaseSetup.CheckIfDatabaseExists(DatabaseSetup.DataSourceConnectionString, DatabaseSetup.DatbaseName);
        //    Assert.IsTrue(exists);
        //    new DatabaseSetup().DropDatabase();
        //    notExists = DatabaseSetup.CheckIfDatabaseExists(DatabaseSetup.DataSourceConnectionString, DatabaseSetup.DatbaseName);
        //    Assert.IsFalse(notExists, "Second asserti in CreateDatabaseIfNotExists_Test");
        //}
    }
}
