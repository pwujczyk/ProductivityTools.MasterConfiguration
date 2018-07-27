using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.MasterConfiguration
{
   public static class Tools
    {
        public static void LogToFile(string s)
        {
            //System.IO.File.AppendAllText($"d:\\log.txt", $"{DateTime.Now} {s}{Environment.NewLine}");
        }

        public static void WriteFile(string id, string filePath)
        {
            string tt1 = System.IO.File.ReadAllText(filePath);
            Tools.LogToFile(string.Format($"{id} - {tt1}"));
        }
    }
}
