using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Builders
{
    interface IBuilder
    {
        string GetValue(string key, string application, string file);

        IList<ConfigItem> GetAllValues();

        void InsertOrUpdate(ConfigItem config);

        void InsertIfNotExists(ConfigItem config);
    }
}
