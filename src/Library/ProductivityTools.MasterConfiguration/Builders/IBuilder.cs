using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Builders
{
    public interface IBuilder
    {
        string GetValue(string key);

        Dictionary<string, string> GetAllValues();

        void InsertOrUpdate(string key, string value);

        void InsertIfNotExists(string key, string value);
    }
}
