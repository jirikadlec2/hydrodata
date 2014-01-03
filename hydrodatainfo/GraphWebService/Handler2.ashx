<%@ WebHandler Language="C#" Class="Handler2" %>

using System;
using System.Web;
using System.Drawing;
using System.IO;
using ZedGraph;
using System.Drawing.Imaging;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;

using HydroDesktop.Interfaces.ObjectModel;
using HydroDesktop.WebServices;
using HydroDesktop.WebServices.WaterOneFlow;
using GraphWebService;

    public class Handler2 : IHttpHandler
    {

        //FOR TESTING TRY URL:
        // http://localhost:58415/SimpleExample/Handler2.ashx?startDate=2006-01-01&endDate=2012-01-01&siteCode=CHMI-D:SNIH&variableCode=CHMI-D:8&serviceUrl=http://hydrodata.info/chmi-d/cuahsi_1_1.asmx&Height=300&Width=600
        //



        public void ProcessRequest(HttpContext context) 
    {

        //TODO: needs some validation like 2012-12-31
        //read the parameters
        if (context.Request.Params["startDate"] == null)
        {
            context.Response.Write("error: startDate parameter is missing.");
            return;
        }

        if (context.Request.Params["Width"] == null)
        {
            context.Response.Write("error: Width parameter is missing.");
            return;
        }

        if (context.Request.Params["Width"] == null)
        {
            context.Response.Write("error: Height parameter is missing.");
            return;
        }
        
        
        string start = context.Request.Params["startDate"];
        string end = context.Request.Params["endDate"];
        string siteCode = context.Request.Params["siteCode"];
        string variableCode = context.Request.Params["variableCode"];
        string serviceUrl = context.Request.Params["serviceUrl"];
        string Height = context.Request.Params["Height"];
        string Width = context.Request.Params["Width"];


        //this the old way of calling the web service..
        //SimpleSoapClient cli = new SimpleSoapClient();
        //DataTable table = cli.GetDataValuesSOAP(start, end, siteCode, variableCode, serviceUrl);        
        //System.Drawing.Image zedGraphImage = cli.CreateGraphImage(table,Height,Width);  
        
              
        //create the data table from SOAP web service
        HydroDesktop.Interfaces.ObjectModel.Series myDataSeries = GetSoapData(start, end, siteCode, variableCode, serviceUrl);

        int graphWidth = 600;
        int.TryParse(Width, out graphWidth);
        int graphHeight = 400;
        int.TryParse(Height, out graphHeight);

        ChartCreator myChartCreator = new ChartCreator();
        System.Drawing.Image zedGraphImage = myChartCreator.CreateGraphFromWaterML(myDataSeries, graphWidth, graphHeight);   

        byte[] imageData;

        using (MemoryStream m = new MemoryStream())
        {
            zedGraphImage.Save(m, System.Drawing.Imaging.ImageFormat.Png);
            imageData = m.ToArray();
        }

        context.Response.ContentType = "image/png";
        context.Response.BinaryWrite(imageData);
    }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public Series GetSoapData(string start, string end, string siteCode, string variableCode, string url)
        {
            //parse parameters
            DateTime startTime = Convert.ToDateTime(start, CultureInfo.InvariantCulture);
            DateTime endTime = Convert.ToDateTime(end, CultureInfo.InvariantCulture);

            WaterOneFlowClient myClient = new WaterOneFlowClient(url);
            myClient.SaveXmlFiles = false;
            myClient.AllInOneRequest = true;
            IList<Series> seriesData = myClient.GetValues(siteCode, variableCode, startTime, endTime);

            if (seriesData.Count > 0)
                return seriesData[0];
            else
                return null;
        }
    }