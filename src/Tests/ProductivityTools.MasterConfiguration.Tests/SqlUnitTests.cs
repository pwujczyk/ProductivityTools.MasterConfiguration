using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Tests
{
    public class SqlUnitTests : BaseTests
    {

        private void SetSQLConfigurationDefaultFileName(string name)
        {
            string text = @"<Configuration>
                                <Source Type=""SQLServer"">
                                    <ConnectionString Value="">
                                </Source>
                            </Configuration>";
            System.IO.File.WriteAllText($"{AssemblyDirectory}\\{name}", text);
        }


        [TestMethod]
        public void GetSqlValue()
        {
            SetSQLConfigurationDefaultFileName(DefaultFileName);
            var x = MConfiguration.Configuration["Key1"];
            Assert.AreEqual("Value1", x, "Value form key");
        }
    }
}
