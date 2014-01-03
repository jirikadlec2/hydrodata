using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace grafy
{
    public static class Helpers
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["OdmConnection"].ConnectionString;
        }
    }
}