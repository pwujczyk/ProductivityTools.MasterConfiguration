using ProductivityTools.MasterConfiguration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Builders
{
    public interface IMigrationBuilder
    {
        void InsertOrUpdate(ConfigItem config);

        void InsertIfNotExists(ConfigItem config);
    }
}
