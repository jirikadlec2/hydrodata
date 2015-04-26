using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;

namespace WebServiceTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            /*textBox1.Text = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" +
  "<soap:Body>" +
    @"<GetValues xmlns=""http://www.cuahsi.org/his/1.0/ws/"">" +
      "<location>LittleBearRiver:USU-LBR-Mendon</location>" +
      "<variable>LBR:USU4</variable>" +
      "<startDate>2009-05-01</startDate>" +
      "<endDate>2009-06-01</endDate>" +
      "<authToken></authToken>" +
    "</GetValues>" +
  "</soap:Body>" +
"</soap:Envelope>";*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //get the user entered values
            string siteCode = txtSiteCode.Text;
            string variableCode = txtVariableCode.Text;
            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            string authToken = "";
            
            //Construct the SOAP envelope
            StringBuilder soapEnv = new StringBuilder();
            soapEnv.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            soapEnv.AppendLine(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">");
            soapEnv.AppendLine("<soap:Body>");
            soapEnv.AppendLine(@"<GetValues xmlns=""http://www.cuahsi.org/his/1.0/ws/"">");
            soapEnv.AppendLine("<location>" + siteCode + "</location>");
            soapEnv.AppendLine("<variable>" + variableCode + "</variable>");
            soapEnv.AppendLine("<startDate>" +startDate + "</startDate>");
            soapEnv.AppendLine("<endDate>" + endDate + "</endDate>");
            soapEnv.AppendLine("<authToken>" + authToken + "</authToken>");
            soapEnv.AppendLine("</GetValues>");
            soapEnv.AppendLine("</soap:Body>");
            soapEnv.AppendLine("</soap:Envelope>");

            //display the soap envelope in the textBox
            textBox1.Text = soapEnv.ToString();

            //call the Http Soap request
            HttpSOAPRequest(textBox1.Text, null);
        }


        void HttpSOAPRequest(String xmlfile, string proxy)
        {
            //string url = "http://water.sdsc.edu/hiscentral/webservices/hiscentral.asmx";

            //string url = "http://icewater.usu.edu/littlebearriver/cuahsi_1_0.asmx";

            string url = txtWSDL.Text.Trim().ToLower();
            //for http request, we need to remove the ?WSDL part from the url
            if (url.EndsWith("?wsdl"))
            {
                url = url.Replace("?wsdl", "");
            }

            //send the SOAP envelope to the service as a xml document
            XmlDocument doc = new XmlDocument();
            doc.Load(new System.IO.StringReader(textBox1.Text));
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            if (proxy != null) req.Proxy = new WebProxy(proxy, true);
            // if SOAPAction header is required, add it here...

            //this is the valid SoapAction header for GetValues web method
            req.Headers.Add("SOAPAction", @"http://www.cuahsi.org/his/1.0/ws/GetValues");

            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";
            Stream stm = req.GetRequestStream();
            doc.Save(stm);
            stm.Close();
            WebResponse resp = req.GetResponse();
            stm = resp.GetResponseStream();
            StreamReader r = new StreamReader(stm);
            
            // process SOAP return doc here
            string result = r.ReadToEnd();

            //extract the soap body result
            XmlDocument resultDoc = new XmlDocument();
            resultDoc.LoadXml(result);
            string soapBody = resultDoc.DocumentElement.ChildNodes[1].InnerText;

            //extract the dataSet
            DataSet ds = new DataSet();
            XmlReader reader = XmlReader.Create(new StringReader(soapBody));
            ds.ReadXml(reader);

            dataGridView1.DataSource = ds.Tables["Value"];
        }

    }
}
