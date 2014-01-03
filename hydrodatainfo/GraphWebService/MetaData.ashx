<%@ WebHandler Language="C#" Class="MetaData" %>

using System;
using System.Web;
using System.IO;
using System.Data;

public class MetaData : IHttpHandler {

    string VariableName;

    string SiteName;

    string UnitName;

    string Latitude;

    string Longitude;
          

    /*
    ?startDate=2006-01-01&endDate=2012-01-01&siteCode=CZSNW:2&variableCode=CZSNW:8&serviceUrl=http://hydrodata.cz/cz_snow/cuahsi_1_1.asmx&Height=10&Width=10
       */
    
    public void ProcessRequest (HttpContext context) {
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
        GetDataValuesSOAP(start, end, siteCode, variableCode, serviceUrl);

        context.Response.ContentType = "text/html";

        context.Response.Write("<html>");

        context.Response.Write("<body bgcolor=#CEECF5>");

        context.Response.Write("<h3>" + "Metadata Information" + "</h3>");
             
        context.Response.Write("<table>");

        context.Response.Write("<tr>");

        context.Response.Write("<td>");
                
        context.Response.Write("SiteName:");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(SiteName);
            
        context.Response.Write("</td>");

        context.Response.Write("</tr>");

        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("VariableName");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(VariableName);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");

        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Measurement Unit:");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(UnitName);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");
        
        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Latitude:");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(Latitude);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");
        
        context.Response.Write("<tr>");

        context.Response.Write("<td>");

        context.Response.Write("Longitude:");

        context.Response.Write("</td>");

        context.Response.Write("<td>");

        context.Response.Write(Longitude);

        context.Response.Write("</td>");

        context.Response.Write("</tr>");
                
        context.Response.Write("</table>");
                    
        context.Response.Write("<html>");

        }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public void GetDataValuesSOAP(string start, string end, string siteCode, string variableCode, string url)
    {
             
        XMLSerialization objXMLSerialization = new XMLSerialization();

        string xml = CallWebService(url, siteCode, variableCode, start, end);

         var ListVariableInfo = objXMLSerialization.GetMetaData(xml);

        if (ListVariableInfo.Count > 0)
            SiteName = ListVariableInfo[0].ToString();
        else
            SiteName = "unknown variable";

        if (ListVariableInfo.Count > 1)
            Latitude = ListVariableInfo[1].ToString();
        else
            Latitude = "unknown units";

        if (ListVariableInfo.Count > 2)
            Longitude = ListVariableInfo[2].ToString();
        else
            Longitude = "unknown units";

        if (ListVariableInfo.Count > 3)
            VariableName = ListVariableInfo[3].ToString();
        else
            VariableName = "unknown units";

        if (ListVariableInfo.Count > 4)
            UnitName = ListVariableInfo[4].ToString();
        else
            UnitName = "unknown units";
      
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