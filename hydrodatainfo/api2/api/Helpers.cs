using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace api
{
    public static class Helpers
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["OdmConnection"].ConnectionString;
        }

        public static string GetDataDirectory()
        {
            return ConfigurationManager.ConnectionStrings["FileConnection1"].ConnectionString;
        }
    }
}