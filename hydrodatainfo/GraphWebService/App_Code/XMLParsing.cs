using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for XMLParsing
/// </summary>
public class XMLParsing
{
	public XMLParsing()
	{
		//
		// TODO: Add constructor logic here
		//
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
        HttpWebRequest req = SOAPParsing.CreateGetValuesRequest(url, siteCode, variableCode, startTime, endTime);

        string XML;

        using (var resp = (HttpWebResponse)req.GetResponse())
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