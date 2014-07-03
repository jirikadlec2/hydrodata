using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace grafy
{
    public static class SiteHandler
    {
        public static void WriteSites(HttpContext context)
        {
            string varCode = string.Empty;
            string operatorCode = string.Empty;

            //process the request
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                switch (key)
                {
                    case "v":
                    case "var":
                    case "variable":
                    case "velicina":
                        varCode = context.Request.QueryString.Get(key);
                        break;
                    case "op":
                    case "operator":
                    case "zdroj":
                    case "owner":
                    case "source":
                    case "vlastnik":
                        operatorCode = context.Request.QueryString.Get(key);
                        break;
                }
            }

            context.Response.ContentType = "text/plain; charset=utf-8";

            //write your handler implementation here.
            List<SiteInfo> siteList = new List<SiteInfo>();

            string[] variableCodes = new string[]{string.Empty};
            if (!string.IsNullOrEmpty(varCode))
            {
                char[] sep = new char[] { '+', ',' };
                variableCodes = varCode.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < variableCodes.Length; i++)
                {
                    variableCodes[i] = variableCodes[i].Trim();
                    if (DataValuesUtil.VariableCodeToID(variableCodes[i]) == 0)
                    {
                        context.Response.Write("variable must one of following values: SRAZKY, VODSTAV, PRUTOK, SNIH, TEPLOTA, TMIN, TMAX.");
                        return;
                    }
                }
            }

            siteList = SiteUtil.GetSitesFromDb(variableCodes, operatorCode);

            context.Response.Write("id\tname\tlat\tlon\telev\toperator\triver\n");
            foreach (SiteInfo si in siteList)
            {
                context.Response.Write(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", si.SiteID, si.SiteName, si.Latitude, si.Longitude, si.Elevation, si.Operator, si.River));
            }
        }
    }
}