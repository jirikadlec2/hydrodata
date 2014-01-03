<%@ WebHandler Language="C#" Class="Statistical" %>

using System;
using System.Web;
using System.IO;
using System.Data;

public class Statistical : IHttpHandler {

    string VariableName;

    string XAxisvalue, YAxisvalue;

    Decimal max = 0;
    Decimal maxinter = 0;
    Decimal min = 0;
    Decimal minInter = 0;
    Decimal avg = 0;
    Decimal avgInter = 0;

    /*
    ?startDate=2006-01-01&endDate=2012-01-01&siteCode=CZSNW:2&variableCode=CZSNW:8&serviceUrl=http://hydrodata.cz/cz_snow/cuahsi_1_1.asmx&Height=10&Width=10
       */

    public void ProcessRequest(HttpContext context)
    {
        //needs some validation like 2012-12-31
        //read the parameters
        if (context.Request.Params["startDate"] == null)
        {
            context.Response.Write("error: startDate parameter is missing.");
            return;
        }

        string start = context.Request.Params["startDate"];
        string end = context.Request.Params["endDate"];
        string siteCode = context.Request.Params["siteCode"];
        string variableCode = context.Request.Params["variableCode"];
        string serviceUrl = context.Request.Params["serviceUrl"];

        //create the data table
        System.Data.DataTable table = GetDataValuesSOAP(start, end, siteCode, variableCode, serviceUrl);

        System.Collections.Generic.List<string> ListstatResult = StatResult(table);
        
        
        context.Response.ContentType = "text/html";

        context.Response.Write("<html>");

        context.Response.Write("<body bgcolor=#CEECF5>");

        context.Response.Write("<h3>" + "Statistical Result" + "</h3>");

        context.Response.Write("<table>");

        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Maxium:" );

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(ListstatResult[0]);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");

        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Minimum");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(ListstatResult[1]);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");

        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Average:");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(ListstatResult[2]);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");

        context.Response.Write("</table>");

        context.Response.Write("<html>");

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public System.Collections.Generic.List<string> StatResult(DataTable dt)
    {
        System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();
        
        int j = 0;

        try
        {
            for (int i = 1; i < dt.Rows.Count - 1; i++)
            {

                avgInter += Convert.ToDecimal(dt.Rows[i]["value"].ToString());
                j += 1;

                max = Convert.ToDecimal(dt.Rows[i]["value"].ToString());
                maxinter = Convert.ToDecimal(dt.Rows[i + 1]["value"].ToString());

                if (max < maxinter)
                {
                    max = maxinter;
                }

                min = Convert.ToDecimal(dt.Rows[i]["value"].ToString());
                minInter = Convert.ToDecimal(dt.Rows[i + 1]["value"].ToString());

                if (min > minInter)
                {
                    min = minInter;
                }

            }

            avg = (Decimal)(avgInter / j);

            result.Add(max.ToString());
            result.Add(min.ToString());
            result.Add(avg.ToString());

        }
        catch (Exception ex)   { }
         
        return result;
        
    }

    public DataTable GetDataValuesSOAP(string start, string end, string siteCode, string variableCode, string url)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("time", typeof(DateTime)));
        
        dt.Columns.Add(new DataColumn("value", typeof(double)));

        System.Collections.Generic.IList<DataValues.DataValues> ListDataValue = new System.Collections.Generic.List<DataValues.DataValues>();

        XMLSerialization objXMLSerialization = new XMLSerialization();

        string xml = CallWebService(url, siteCode, variableCode, start, end);

        ListDataValue = objXMLSerialization.ParseGetValues(xml);

        var ListVariableInfo = objXMLSerialization.GetMetaData(xml);

        if (ListVariableInfo.Count > 0)
            VariableName = ListVariableInfo[0].ToString();
        else
            VariableName = "unknown variable";

        if (ListVariableInfo.Count > 1)
            YAxisvalue = ListVariableInfo[1].ToString();
        else
            XAxisvalue = "unknown units";

        if (ListVariableInfo.Count > 2)
            XAxisvalue = ListVariableInfo[2].ToString();
        else
            XAxisvalue = "unknown units";


        for (int i = 0; i < ListDataValue.Count; i++)
        {
            System.Data.DataRow row = dt.NewRow();
            row["time"] = ListDataValue[i].LocalDateTime;
            row["value"] = ListDataValue[i].Value;
            dt.Rows.Add(row);
        }
        return dt;
    }

    /// <summary>
    /// Calls any SOAP web service
    /// </summary>
    /// <param name="url">for example "http://hydrodata.info/cz_snow/cuahsi_1_1.asmx"</param>
    /// <param name="siteCode"></param>
    /// <param name="variableCode"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public string CallWebService(string url, string siteCode, string variableCode, string startTime, string endTime)
    {
        System.Net.HttpWebRequest req = SOAPParsing.CreateGetValuesRequest(url, siteCode, variableCode, startTime, endTime);

        string XML;

        using (var resp = (System.Net.HttpWebResponse)req.GetResponse())
        {
            // we will read data via the response stream
            using (Stream ReceiveStream = resp.GetResponseStream())
            {
                StreamReader sReader = new StreamReader(ReceiveStream);

                XML = sReader.ReadToEnd();

            }
        }
        return XML;
    }

}