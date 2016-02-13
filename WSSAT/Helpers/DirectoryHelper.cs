using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WSSAT.Helpers
{
    public class DirectoryHelper
    {
        public static void CreateScanDirectory(string dirName)
        {
            Directory.CreateDirectory(dirName);
            //dirName += @"\Scans";
            //Directory.CreateDirectory(dirName);
            Directory.CreateDirectory(dirName + @"\Logs");
            Directory.CreateDirectory(dirName + @"\Report");
        }

        public static string GetScanDirectoryName()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString().PadLeft(2, '0');
            string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string hour = DateTime.Now.Hour.ToString().PadLeft(2, '0');
            string minute = DateTime.Now.Minute.ToString().PadLeft(2, '0');
            string second = DateTime.Now.Second.ToString().PadLeft(2, '0');
            return @"Scans\" + year + month + day + hour + minute + second;
        }
    }
}
