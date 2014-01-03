<%@ WebHandler Language="C#" Class="Handler2" %>

using System;
using System.Web;
using System.Drawing;
using System.IO;
using ZedGraph;
using System.Drawing.Imaging;
using System.Data;
using System.Xml;

public class Handler2 : IHttpHandler {
    
    //FOR TESTING TRY URL:
    // http://localhost:58415/SimpleExample/Handler2.ashx?startDate=2006-01-01&endDate=2012-01-01&siteCode=CZSNW:2&variableCode=CZSNW:8&serviceUrl=http://hydrodata.cz/cz_snow/cuahsi_1_1.asmx&Height=10&Width=10
    //

    string VariableName;

    string XAxisvalue, YAxisvalue;
            
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
        string Height = context.Request.Params["Height"];
        string Width = context.Request.Params["Width"];
        
              
        //create the data table
        DataTable table = GetDataValuesSOAP(start, end, siteCode, variableCode, serviceUrl);        
                
        
        System.Drawing.Image zedGraphImage = CreateGraphImage(table,Height,Width);     

        byte[] imageData;

        using (MemoryStream m = new MemoryStream())
        {
            zedGraphImage.Save(m, System.Drawing.Imaging.ImageFormat.Png);
            imageData = m.ToArray();
        }

        context.Response.ContentType = "image/png";
        
        context.Response.BinaryWrite(imageData);

    }
 
    public bool IsReusable {
        get {
            return false;
        }
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
            DataRow row = dt.NewRow();
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


    private static Image resizeImage(Image imgToResize, Size size)
    {
             
         int destWidth = (int)size.Width;
         int destHeight = (int)size.Height;

        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage((Image)b);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return (Image)b;
    }
    
    
    
    public Image CreateGraphImage(DataTable table,string Height, string Width)
    {
        DataSourcePointList source = new DataSourcePointList();
        source.DataSource = table;
        source.XDataMember = "time";
        source.YDataMember = "value";

        ZedGraph.GraphPane p = new GraphPane();

        LineItem line = p.AddCurve(VariableName, source, Color.Blue, SymbolType.None);
        
        line.Line.Width = 2f;
        
        p.XAxis.Type = AxisType.Date;

        p.XAxis.Title.Text = XAxisvalue;

        p.YAxis.Title.Text = YAxisvalue;
        
        p.AxisChange();

        Bitmap bm = new Bitmap(10,10);
        
        Graphics ge = Graphics.FromImage(bm);

        p.AxisChange(ge);

        Image img = p.GetImage();
        
        Size ss = new Size(int.Parse(Width),int.Parse(Height));

        return resizeImage(img, ss);
               
    }

}