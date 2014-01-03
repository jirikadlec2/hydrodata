using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.IO;

namespace GraphWebService.App_Code
{
    /// <summary>
    /// This class contains the methods used in older version
    /// of the graph web service
    /// </summary>
    public class SimpleSoapClient
    {
        public string VariableName { get; set; }

        public string XAxisvalue { get; set; }

        public string YAxisvalue { get; set; }
        
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
        /// <param name="url">for example "http://hydrodata.info/chmi-d/cuahsi_1_1.asmx"</param>
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
}