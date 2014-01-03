<%@ WebHandler Language="C#" Class="DataTableOutput" %>

using System;
using System.Web;

public class DataTableOutput : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
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

        context.Response.ContentType = "text/html";

        context.Response.Write("<html>");

        context.Response.Write("<body bgcolor=#CEECF5>");

        context.Response.Write("<h3>" + "Data Values" + "</h3>");

        context.Response.Write("<table border=1>");
        
        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Date");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write("Values");

        context.Response.Write("</td>");

        context.Response.Write("</tr>");

        for (int j = 0; j < table.Rows.Count; j++)
        {
            context.Response.Write("<tr>");

            context.Response.Write("<td>");

            context.Response.Write(table.Rows[j][0]);

            context.Response.Write("</td>");

            context.Response.Write("<td>");

            context.Response.Write(table.Rows[j][1]);

            context.Response.Write("</td>");

            context.Response.Write("</tr>");
            
        }
                    
        context.Response.Write("</table>");

        context.Response.Write("<html>");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

   
    public System.Data.DataTable GetDataValuesSOAP(string start, string end, string siteCode, string variableCode, string url)
    {
        System.Data.DataTable dt = new System.Data.DataTable();

        dt.Columns.Add(new System.Data.DataColumn("time", typeof(DateTime)));

        dt.Columns.Add(new System.Data.DataColumn("value", typeof(double)));

        System.Collections.Generic.IList<DataValues.DataValues> ListDataValue = new System.Collections.Generic.List<DataValues.DataValues>();

        XMLSerialization objXMLSerialization = new XMLSerialization();

        string xml = CallWebService(url, siteCode, variableCode, start, end);

        ListDataValue = objXMLSerialization.ParseGetValues(xml);

        var ListVariableInfo = objXMLSerialization.GetMetaData(xml);

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
            using (System.IO.Stream ReceiveStream = resp.GetResponseStream())
            {
                System.IO.StreamReader sReader = new System.IO.StreamReader(ReceiveStream);

                XML = sReader.ReadToEnd();

            }
        }
        return XML;
    }

    
}