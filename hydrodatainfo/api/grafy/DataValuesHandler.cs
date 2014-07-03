using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace grafy
{
    public class DataValuesHandler
    {
        public static void WriteValues(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string startDate = "1900-01-01";
            string endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string varCode = string.Empty;
            string siteCode = string.Empty;
            DateTime start = Convert.ToDateTime("1900-01-01");
            DateTime end = Convert.ToDateTime(DateTime.Now);
            string step = "day";
            string interpStr = "false";

            //siteId, varCode, startDate, endDate
            if (!context.Request.QueryString.HasKeys())
            {
                context.Response.Write("missing parameters: site, variable");
                return;
            }

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
                    case "step":
                    case "krok":
                        step = context.Request.QueryString.Get(key);
                        break;
                    case "interpolace":
                    case "interp":
                    case "fill":
                    case "doplnit":
                        interpStr = context.Request.QueryString.Get(key);
                        break;
                        
                }
            }

            //check params
            if (string.IsNullOrEmpty(siteCode))
            {
                context.Response.Write("missing parameter: site");
            }

            int siteId = Convert.ToInt32(siteCode);

            //interp param
            bool interpolateOn = false;
            switch(interpStr)
            {
                case "true":
                case "yes":
                case "ano":
                case "lin":
                case "linear":
                    interpolateOn = true;
                    break;
                default:
                    interpolateOn = false;
                    break;
            }

            DateRange range = new DateRange();
            range.Start = start;
            range.End = end;

            //special case: multiple variables!
            string[] varCodeList;
            if (string.IsNullOrEmpty(varCode))
            {
                List<VariableInfo> varList = VariableUtil.GetVariablesForSite(siteCode);
                if (varList.Count == 0)
                {
                    context.Response.Write("No data values found for site: " + siteCode);
                    return;
                }
                varCodeList = new string[varList.Count];
                for (int i = 0; i < varList.Count; i++)
                {
                    varCodeList[i] = varList[i].VariableCode;
                }
            }
            else
            {
                char[] sep = new char[] { ',', '+' };
                varCodeList = varCode.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            }

            for (int i = 0; i < varCodeList.Length; i++)
            {
                varCodeList[i] = varCodeList[i].ToLower().Trim();
                if (DataValuesUtil.VariableCodeToID(varCodeList[i]) == 0)
                {
                    context.Response.Write("variable must one of following values: SRAZKY, VODSTAV, PRUTOK, SNIH, TEPLOTA, TMIN, TMAX.");
                    return;
                }
            }
            List<DateTime> allStarts = new List<DateTime>();
            List<DateTime> allEnds = new List<DateTime>();
            bool hasTheValues = false;
            for (int i = 0; i < varCodeList.Length; i++)
            {
                DateRange rng = DataValuesUtil.GetDateRangeFromDb(siteId, varCodeList[i]);
                if (rng.HasValues)
                {
                    allStarts.Add(rng.Start);
                    allEnds.Add(rng.End);
                }
                if (hasTheValues == false && rng.HasValues == true)
                {
                    hasTheValues = true;
                }
            }
            if (!hasTheValues)
            {
                context.Response.Write("No data values found.");
                return;
            }
            allStarts.Sort();
            allEnds.Sort();
            range.Start = allStarts[0];
            range.End = allEnds[allEnds.Count - 1];

            if (!DateTime.TryParse(startDate, out start))
            {
                context.Response.Write("startDate must be in format yyyy-MM-dd");
                return;
            }
            if (!DateTime.TryParse(endDate, out end))
            {
                context.Response.Write("endDate must be in format yyyy-MM-dd");
                return;
            }
            if (step.StartsWith("h"))
            {
                step = "hour";
            }
            else
            {
                step = "day";
            }
            
            double nodata = -9998.0;
            string numberFormat = DataValuesUtil.GetNumberFormat(varCode);

            if (step == "hour")
            {                
                if (start < range.Start)
                {
                    start = range.Start.Date.AddHours(range.Start.Hour);
                }
                if (end > range.End)
                {
                    end = range.End.Date.AddHours(range.End.Hour);
                }
                if (start > end)
                {
                    context.Response.Write("no data in the selected date range.");
                    return;
                }
                
                List<double[]> valArrays = new List<double[]>();
                List<int> valArrayLengths = new List<int>();
                for (int i = 0; i < varCodeList.Length; i++)
                {
                    double[] mylist = DataValuesUtil.GetHourlyValuesFromDb(Convert.ToInt32(siteCode), varCodeList[i], start, end, interpolateOn);
                    valArrays.Add(mylist);
                    valArrayLengths.Add(mylist.Length);           
                }
                valArrayLengths.Sort();
                int maxLength = valArrayLengths[valArrayLengths.Count - 1];
                int minLength = valArrayLengths[0];
                if (maxLength == 0)
                {
                    context.Response.Write("no data in the selected date range");
                    return;
                }
                else if (valArrays.Count == 1)
                {
                    context.Response.Write(string.Format("datum\t{0}\n", varCodeList[0]));
                    double[] vals = valArrays[0];
                    for (int i = 0; i < vals.Length; i++)
                    {
                        DateTime dat = start.AddHours(i);
                        context.Response.Write(string.Format("{0}\t{1}\n", dat.ToString("yyyy-MM-dd HH:mm:ss"),
                            (vals[i] > nodata) ? vals[i].ToString(numberFormat) : "NA"));
                    }
                }
                else if (valArrays.Count > 1)
                {
                    string header = "datum";
                    for (int i = 0; i < varCodeList.Length; i++)
                    {
                        header += "\t" + varCodeList[i];
                    }
                    header += "\n";
                    context.Response.Write(header);
                    int numValArrays = valArrays.Count;
                    for (int i = 0; i < maxLength; i++)
                    {
                        DateTime dat = start.AddHours(i);
                        context.Response.Write(string.Format("{0}\t", dat.ToString("yyyy-MM-dd HH:mm:ss")));
                        for (int j=0; j< numValArrays; j++)
                        {
                            double val = valArrays[j][i];
                            context.Response.Write((val > nodata) ? val.ToString(numberFormat) : "NA");
                            if (j == numValArrays - 1)
                            {
                                context.Response.Write("\n");
                            }
                            else
                            {
                                context.Response.Write("\t");
                            }
                        }
                    }
                }
            }
            else //step is day
            {              
                if (start < range.Start)
                {
                    start = range.Start.Date;
                }
                if (end > range.End)
                {
                    end = range.End.Date;
                }
                if (start > end)
                {
                    context.Response.Write("no data in the selected date range.");
                    return;
                }

                List<double[]> valArrays = new List<double[]>();
                List<int> valArrayLengths = new List<int>();
                for (int i = 0; i < varCodeList.Length; i++)
                {
                    double[] mylist = DataValuesUtil.GetDailyValuesFromDb(Convert.ToInt32(siteCode), varCodeList[i], start, end, interpolateOn);
                    valArrays.Add(mylist);
                    valArrayLengths.Add(mylist.Length);
                }
                valArrayLengths.Sort();
                int maxLength = valArrayLengths[valArrayLengths.Count - 1];
                int minLength = valArrayLengths[0];
                if (maxLength == 0)
                {
                    context.Response.Write("no data in the selected date range");
                    return;
                }
                else if (valArrays.Count == 1)
                {
                    context.Response.Write(string.Format("datum\t{0}\n",varCodeList[0]));
                    double[] vals = valArrays[0];
                    for (int i = 0; i < vals.Length; i++)
                    {
                        DateTime dat = start.AddDays(i);
                        context.Response.Write(string.Format("{0}\t{1}\n", dat.ToString("yyyy-MM-dd"),
                            (vals[i] > nodata) ? vals[i].ToString(numberFormat) : "NA"));
                    }
                }
                else if (valArrays.Count > 1)
                {
                    string header = "datum";
                    for (int i = 0; i < varCodeList.Length; i++)
                    {
                        header += "\t" + varCodeList[i];
                    }
                    header += "\n";
                    context.Response.Write(header);
                    int numValArrays = valArrays.Count;
                    for (int i = 0; i < maxLength; i++)
                    {
                        DateTime dat = start.AddDays(i);
                        context.Response.Write(string.Format("{0}\t", dat.ToString("yyyy-MM-dd")));
                        for (int j = 0; j < numValArrays; j++)
                        {
                            double val = valArrays[j][i];
                            context.Response.Write((val > nodata) ? val.ToString(numberFormat) : "NA");
                            if (j == numValArrays - 1)
                            {
                                context.Response.Write("\n");
                            }
                            else
                            {
                                context.Response.Write("\t");
                            }
                        }
                    }
                }
            }
        }
    }
}