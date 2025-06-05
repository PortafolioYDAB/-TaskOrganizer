using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskOrganizer.Utilities
{
    static public class Log
    {
        private static bool startLog = false;
        public static string logName = "Log_"+InterfaceConfig.nameLog;


        public static void initializeLog()
        {
            logName = InterfaceConfig.logPath + "/Logs" + InterfaceConfig.nameLog + Environment.Version + "_" + DateTime.Now.ToString("dd-MM-yyyy");
            using (StreamWriter w = File.AppendText(logName + ".txt"))
            {
                w.Write($"-------------------------------------------------------");
                w.Write($"\r \n Log TaskOrganizer Version {Environment.Version}");
                w.Write($"{DateTime.Now.ToLongDateString}");
                w.Write($"-------------------------------------------------------");
            }
            startLog = true;
        } 

        public static void recordLog(string message)
        {
            logName = InterfaceConfig.logPath + "/Logs" + InterfaceConfig.nameLog + Environment.Version + "_" + DateTime.Now.ToString("dd-MM-yyyy");
            if (!startLog) initializeLog();
            using (StreamWriter w = File.AppendText(logName + ".txt"))
            {
                w.WriteLine($"{DateTime.Now} -----> : {message}");
            }
        }


    }
}
