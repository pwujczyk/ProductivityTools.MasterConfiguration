using ProductivityTools.MasterConfiguration.Builders;
using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Directors
{
    public class MigrationDirector : BaseDirector
    {
        public MigrationDirector(string configurationFileName, string applicationName, bool currentDomain) : base(configurationFileName, applicationName, currentDomain) { }

        public void Migrate(bool ovverideExistingOnes)
        {
            File fileBuilder = new File(ConfigurationFileName, CurrentDomain);
            var fileValues = fileBuilder.GetAllValues();

            IMigrationBuilder targetBuilder = new SqlServer(fileBuilder.ConnectionString, fileBuilder.Schema, fileBuilder.Table);

            Action<ConfigItem> ActionToPerform;
            if (ovverideExistingOnes)
            {
                ActionToPerform = targetBuilder.InsertOrUpdate;
            }
            else
            {
                ActionToPerform = targetBuilder.InsertIfNotExists;
            }

            foreach (var value in fileValues)
            {
                ActionToPerform(value);
            }
        }
    }
}
