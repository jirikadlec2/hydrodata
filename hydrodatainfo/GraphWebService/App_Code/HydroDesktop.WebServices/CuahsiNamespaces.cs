using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydroDesktop.WebServices
{
    public static class CuahsiNamespaces
    {
        //<data name="CUAHSI_1_0_Namespace" xml:space="preserve">
    //<value>http://www.cuahsi.org/his/1.0/ws/</value>
  //</data>
        public static string CUAHSI_1_0_Namespace
        {
            get { return "http://www.cuahsi.org/his/1.0/ws/"; }
        }
  //<data name="CUAHSI_1_1_Namespace" xml:space="preserve">
    //<value>http://www.cuahsi.org/his/1.1/ws/</value>
  //</data>
        public static string CUAHSI_1_1_Namespace
        {
            get { return "http://www.cuahsi.org/his/1.1/ws/"; }
        }

        
  //<data name="HISCentral_Site" xml:space="preserve">
    //<value>http://hiscentral.cuahsi.org</value>
  //</data>
        public static string HISCentral_Site
        {
            get { return "http://hiscentral.cuahsi.org"; }
        }
    }
}