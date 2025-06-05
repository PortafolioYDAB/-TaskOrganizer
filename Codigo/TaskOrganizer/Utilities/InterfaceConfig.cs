using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOrganizer.Utilities
{
    public static class InterfaceConfig
    {
        public static string connectionString = string.Empty;
        public static string nameLog = string.Empty;
        public static string logPath = string.Empty;

        public static void InitializeConfig()
        {
            connectionString= ConfigurationManager.ConnectionStrings["strCadenaConexion"]!.ToString();
            nameLog = ConfigurationManager.AppSettings["nameLog"]!.ToString();
            logPath = ConfigurationManager.AppSettings["logPath"]!.ToString();
        }
    }
}
