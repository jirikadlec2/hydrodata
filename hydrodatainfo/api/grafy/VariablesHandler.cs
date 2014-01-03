using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace grafy
{
    public static class VariablesHandler
    {
        public static void WriteVariables(HttpContext context)
        {
            string siteCode = string.Empty;

            //process the request
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                switch (key)
                {
                    case "st":
                    case "stanice":
                    case "site":
                        siteCode = context.Request.QueryString.Get(key);
                        break;
                }
            }
            
            context.Response.ContentType = "text/plain";

            List<VariableInfo> mylist = new List<VariableInfo>();
            //write your handler implementation here.
            if (string.IsNullOrEmpty(siteCode))
            {
                mylist = VariableUtil.GetVariablesFromDb();
            }
            else
            {
                mylist = VariableUtil.GetVariablesForSite(siteCode);
            }
            if (mylist.Count == 0)
            {
                context.Response.Write(string.Format("Site with code '{0}' not found.", siteCode));
                return;
            }

            context.Response.Write("code\tunits\n");
            foreach (VariableInfo vi in mylist)
            {
                context.Response.Write(String.Format("{0}\t{1}\n", vi.VariableCode, vi.UnitsName));
            }
        } 
    }
}