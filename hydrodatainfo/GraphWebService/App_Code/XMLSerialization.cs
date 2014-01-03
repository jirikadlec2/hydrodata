using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Globalization;

/// <summary>
/// Summary description for XMLSerialization
/// </summary>
public class XMLSerialization
{
	public XMLSerialization()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public IList<string> GetMetaData(string xml)
    {
        IList<string> datasource = new List<string>();

        string VariableName = "";
        string SiteName = "";
        string UnitName = "";
        string latitude = "";
        string longitude = "";

        StringReader reader1 = new StringReader(xml);

        XmlTextReader reader = new XmlTextReader(reader1);

        using (reader)
        {
            while (reader.Read())
            {
                string readerName = reader.Name.ToLower();

                if (readerName == "variablename")
                {
                   VariableName = reader.ReadString();
                   datasource.Add(VariableName);
                }

                if (readerName == "sitename")
                {
                    SiteName = reader.ReadString();
                    datasource.Add(SiteName);
                }

                if (readerName == "latitude")
                {
                    latitude = reader.ReadString();
                    datasource.Add(latitude);
                }


                if (readerName == "longitude")
                {
                    longitude = reader.ReadString();
                    datasource.Add(longitude);
                }

                if (readerName == "unitname")
                {
                    UnitName = reader.ReadString();
                    datasource.Add(UnitName);
                }
                else if (readerName == "units")
                {
                    UnitName = reader.ReadString();
                    datasource.Add(UnitName);
                }
            }
            return datasource;
        }
    }

    public IList<DataValues.DataValues> ParseGetValues(string xml)
    {
        IList<DataValues.DataValues> results = new List<DataValues.DataValues>();

        DataValues.DataValues val;

        StringReader reader1 = new StringReader(xml);

        XmlTextReader reader = new XmlTextReader(reader1);

        using (reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string readerName = reader.Name.ToLower();

                    if (readerName == "value")
                    {
                        if (reader.HasAttributes)
                        {
                            val = new DataValues.DataValues();

                            //there may be an uppercase or a lowercase dateTime attribute
                            string localDateTime = reader.GetAttribute("dateTime");
                            if (string.IsNullOrEmpty(localDateTime))
                            {
                                localDateTime = reader.GetAttribute("DateTime");
                            }

                            string strVal = reader.ReadString();
                            if (strVal != "" && localDateTime != "")
                            {
                                val.LocalDateTime = Convert.ToDateTime(localDateTime, CultureInfo.InvariantCulture);
                                val.Value = Convert.ToDouble(strVal, CultureInfo.InvariantCulture);
                                results.Add(val);
                            }
                        }
                    }
                }
            }
            return results;

        }
    }
}