using System;
using System.IO;

namespace WSSAT.Helpers
{
    public class Logger
    {
        public static void Log(string path, string str)
        {
            //try
            //{
            string fileName = getFileName();
            string logPreFix = getLogMessagePrefix();
            StreamWriter sw = new StreamWriter(path + fileName, true);
            sw.WriteLine(logPreFix + str);
            sw.Flush();
            sw.Close();
            //}
            //catch { }
        }

        private static string getFileName()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString().PadLeft(2, '0');
            string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            return year + month + day + ".log";
        }

        private static string getLogMessagePrefix()
        {
            return DateTime.Now.ToString() + " ==> ";
        }
    }
}
