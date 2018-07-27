using ProductivityTools.MasterConfiguration.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Directors
{
    public class MigrationDirector : BaseDirector
    {
        public MigrationDirector(string configurationFileName, bool currentDomain) : base(configurationFileName, currentDomain)
        {
        }

        public void Migrate(bool ovverideExistingOnes)
        {
            File fileBuilder = new File(ConfigurationFileName, CurrentDomain);
            var fileValues = fileBuilder.GetAllValues();

            IBuilder targetBuilder = new SqlServer(fileBuilder.ConnectionString, fileBuilder.Schema, fileBuilder.Table);

            Action<string,string> ActionToPerform;
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
                ActionToPerform(value.Key, value.Value);
            }
        }
    }
}
