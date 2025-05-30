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
        static string connectionString = string.Empty;

        public static void InitializeConfig()
        {
            connectionString= ConfigurationManager.ConnectionStrings["strCadenaConexion"].ToString();
        }
    }
}
