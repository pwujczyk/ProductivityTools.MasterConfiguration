using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeString(this string s)
        {
            return s ?? string.Empty;
        }
    }
}
