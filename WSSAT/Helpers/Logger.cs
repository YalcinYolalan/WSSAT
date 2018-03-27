using System;
using System.IO;

namespace WSSAT.Helpers
{
    public class Logger
    {
        public static void Log(string path, string str, bool isError)
        {
            //try
            //{
            string fileName = getFileName(isError);
            string logPreFix = getLogMessagePrefix();
            StreamWriter sw = new StreamWriter(path + fileName, true);
            sw.WriteLine(logPreFix + str);
            sw.Flush();
            sw.Close();
            //}
            //catch { }
        }

        private static string getFileName(bool isError)
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString().PadLeft(2, '0');
            string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string errorTxt = string.Empty;
            if (isError) errorTxt = "EXCEPTION_";
            return errorTxt + year + month + day + ".log";
        }

        private static string getLogMessagePrefix()
        {
            return DateTime.Now.ToString() + " ==> ";
        }
    }
}
