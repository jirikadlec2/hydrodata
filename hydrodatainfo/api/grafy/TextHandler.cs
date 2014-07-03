using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace grafy
{
    public class TextHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            
            string p = context.Request.CurrentExecutionFilePath;
            if (p.IndexOf("/") >= 0)
            {
                int index = p.LastIndexOf("/");
                if (index < p.Length - 1)
                {
                    p = p.Substring(index + 1);
                }
                else
                {
                    p = "";
                }
            }

            switch (p)
            {
                case "sites":
                case "stanice":
                case "station":
                case "stations":
                case "site":
                    SiteHandler.WriteSites(context);
                    break;
                case "vars":
                case "variables":
                case "variable":
                case "veliciny":
                case "velicina":
                    VariablesHandler.WriteVariables(context);
                    break;
                case "values":
                case "vals":
                case "hodnoty":
                case "mereni":
                case "data":
                    DataValuesHandler.WriteValues(context);
                    break;
                case "map":
                case "mapa":
                    MapHandler.WriteValues(context);
                    break;
                case "":
                    string appPath = context.Request.RawUrl;
                    if (!appPath.EndsWith("/"))
                    {
                        appPath = appPath + "/";
                    }
                    HttpResponse resp = context.Response;
                    resp.ContentType = "text/html; charset=utf-8";
                    resp.Write("<html><head><title>hydrodata.cz API</title></head><body>");
                    resp.Write("<h1>hydrodata.cz API for R</h1>");
                    DocHelper h = new DocHelper(resp, appPath);
                    h.Write("sites");
                    h.Write("variables");
                    h.Write("values");
                    h.Write("map");
                    resp.Write(string.Format("<h2>Examples</h2>", appPath));
                    h.Write("stanice");
                    h.Write("stanice?var=snih");
                    h.Write("stanice?var=teplota&operator=POH");
                    h.Write("veliciny");
                    h.Write("veliciny?st=4");
                    h.Write("hodnoty?st=4&od=2013-07-01&do=2013-07-31");
                    h.Write("hodnoty?st=4&var=teplota,srazky&od=2013-01-01");
                    h.Write("hodnoty?st=4&var=teplota,srazky&od=2013-01-01&interp=true");
                    h.Write("hodnoty?st=4&var=srazky&od=2013-06-01&do=2013-06-30&krok=h");
                    h.Write("hodnoty?st=123&od=2012-07-01&do=2012-07-31&v=prutok&krok=h&interp=false");
                    h.Write("hodnoty?st=123&od=2012-07-01&do=2012-07-31&v=prutok&krok=h&interp=true");
                    h.Write("mapa?var=snih&od=2013-02-01&do=2013-02-02&stat=max");
                    h.Write("mapa?var=teplota&od=2012-08-20&do=2012-08-21&stat=avg");
                    context.Response.Write("<h2>Example R Script</h2>\n");
                    context.Response.Write("\n<pre>\n");
                    context.Response.Write(Properties.Resources.sample_r_script);
                    context.Response.Write("\n</pre>\n");
                    context.Response.Write("</body></html>");
                    break;
                default:
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    context.Response.Write("Bad Request. Use /sites, /variables or /values");
                    break;
            }

        }

        #endregion
    }
}
