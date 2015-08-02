using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api
{
    public class MapHandler
    {
        public static void WriteValues(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string startDate = "1900-01-01";
            string endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string varCode = string.Empty;
            DateTime start = Convert.ToDateTime("1900-01-01");
            DateTime end = Convert.ToDateTime(DateTime.Now);
            string stat = "avg";

            //siteId, varCode, startDate, endDate
            if (!context.Request.QueryString.HasKeys())
            {
                context.Response.Write("missing parameters: variable, startdate, enddate");
                return;
            }

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
                    case "od":
                    case "start":
                    case "begindate":
                    case "startdate":
                    case "from":
                        startDate = context.Request.QueryString.Get(key);
                        break;
                    case "do":
                    case "end":
                    case "enddate":
                    case "to":
                        endDate = context.Request.QueryString.Get(key);
                        break;
                    case "stat":
                    case "statistic":
                    case "statistika":
                    case "souhrn":
                        stat = context.Request.QueryString.Get(key);
                        break;
                }
            }

            //check params
            if (string.IsNullOrEmpty(startDate))
            {
                context.Response.Write("missing parameter: startDate");
            }

            if (string.IsNullOrEmpty(endDate))
            {
                context.Response.Write("missing parameter: endDate");
            }

            DateRange range = new DateRange();
            range.Start = start;
            range.End = end;

            List<ValueInfo> lst = MapUtil.GetMapFromDb(varCode, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), stat);

            //todo write-out response
            context.Response.Write("id\tlat\tlon\telev\tvalue\n");
            foreach (ValueInfo vi in lst)
            {
                context.Response.Write(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", vi.SiteID, vi.Lat, vi.Lon,vi.Elev, vi.DataValue));
            }
        }
    }
}