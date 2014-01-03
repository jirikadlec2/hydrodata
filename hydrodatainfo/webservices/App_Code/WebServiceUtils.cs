using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using WaterOneFlow.Schema.v1_1;
using WaterOneFlow;
using WaterOneFlowImpl.v1_1;
using WaterOneFlowImpl;
using System.Data;

namespace WaterOneFlow.odws
{
    /// <summary>
    /// The web service utilities
    /// </summary>
    public class WebServiceUtils
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["OdmConnection"].ConnectionString;
        }
        
        public static string GetBaseUri()
        {
            string uri = HttpContext.Current.Request.Url.ToString();
            if (uri.EndsWith(".asmx"))
            {
                return uri;
            }
            else
            {
                return uri.Remove(uri.IndexOf(".asmx")) + ".asmx";
            }
        }
        
        public static QueryInfoType CreateQueryInfo(string webMethodName)
        {
            QueryInfoType queryInfo = new QueryInfoType();
            queryInfo.creationTime = DateTime.Now;
            queryInfo.creationTimeSpecified = true;
            queryInfo.criteria = new QueryInfoTypeCriteria();
            queryInfo.criteria.locationParam = String.Empty;
            queryInfo.criteria.MethodCalled = webMethodName;
            queryInfo.criteria.variableParam = String.Empty;
            queryInfo.queryURL = @"http://example.com";
            
            return queryInfo;
        }

        /// <summary>
        /// Gets the sites, in XML format [test for SNOW]
        /// </summary>
        public static SiteInfoResponseTypeSite[] GetSitesFromDb()
        {
           
                List<SiteInfoResponseTypeSite> siteList = new List<SiteInfoResponseTypeSite>();

                string cnn = GetConnectionString();
                string serviceCode = ConfigurationManager.AppSettings["network"];

                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sql = "SELECT st_id, st_name, wmo_id, lat, lon, elev, country FROM st " +
                                     "WHERE st.st_id IN (SELECT st_id FROM series WHERE value_count > 1000) ORDER BY country, st_id";

                        cmd.CommandText = sql;
                        cmd.Connection = conn;
                        conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            SiteInfoResponseTypeSite newSite = new SiteInfoResponseTypeSite();
                            SiteInfoType si = new SiteInfoType();

                            if (dr["elev"] != DBNull.Value)
                            {
                                si.elevation_m = Convert.ToDouble(dr["elev"]);
                                si.elevation_mSpecified = true;
                            }
                            else
                            {
                                si.elevation_m = 0;
                                si.elevation_mSpecified = true;
                            }
                            si.geoLocation = new SiteInfoTypeGeoLocation();

                            LatLonPointType latLon = new LatLonPointType();
                            latLon.latitude = Convert.ToDouble(dr["lat"]);
                            latLon.longitude = Convert.ToDouble(dr["lon"]);
                            latLon.srs = "EPSG:4326";
                            si.geoLocation.geogLocation = latLon;
                            si.geoLocation.localSiteXY = new SiteInfoTypeGeoLocationLocalSiteXY[1];
                            si.geoLocation.localSiteXY[0] = new SiteInfoTypeGeoLocationLocalSiteXY();
                            si.geoLocation.localSiteXY[0].X = latLon.longitude;
                            si.geoLocation.localSiteXY[0].Y = latLon.latitude;
                            si.geoLocation.localSiteXY[0].ZSpecified = false;
                            si.geoLocation.localSiteXY[0].projectionInformation = si.geoLocation.geogLocation.srs;
                            si.metadataTimeSpecified = false;
                            //si.oid = Convert.ToString(dr["st_id"]);
                            si.note = new NoteType[2];
                            si.note[0] = new NoteType();
                            si.note[0].title = "Country";
                            si.note[0].type = "custom";
                            si.note[0].Value = dr["country"].ToString();

                            si.note[1] = new NoteType();
                            si.note[1].title = "db_id";
                            si.note[1].type = "custom";
                            si.note[1].Value = dr["st_id"].ToString();

                            si.verticalDatum = "Unknown";

                            si.siteCode = new SiteInfoTypeSiteCode[1];
                            si.siteCode[0] = new SiteInfoTypeSiteCode();
                            si.siteCode[0].network = serviceCode;
                            si.siteCode[0].siteID = Convert.ToInt32(dr["wmo_id"]);
                            si.siteCode[0].siteIDSpecified = true;
                            si.siteCode[0].Value = Convert.ToString(dr["wmo_id"]);

                            si.siteName = Convert.ToString(dr["st_name"]);

                            newSite.siteInfo = si;
                            siteList.Add(newSite);
                        }
                    }
                }
                return siteList.ToArray();        
        }

        public static void WriteLog(string logMessage)
        {
            string cmdText = "INSERT INTO log_messages(log_time, log_msg) VALUES(@p1,@p2)";
            using(SqlConnection cnn = new SqlConnection(GetConnectionString()))
            {
                using(SqlCommand cmd = new SqlCommand(cmdText, cnn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p1", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@p2", SqlDbType.VarChar));
                    cmd.Parameters["@p1"].Value = DateTime.Now;
                    cmd.Parameters["@p2"].Value = logMessage;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                
            }
        }

        public static seriesCatalogTypeSeries GetSeriesCatalogFromDb(int dbSiteId, int variableId)
        {
            WriteLog("Executing GetSeriesCatalogFromDB(" + dbSiteId + ", " + variableId + ")");
            try
            {
                seriesCatalogTypeSeries s = new seriesCatalogTypeSeries();

                //method
                s.method = new MethodType();
                s.method.methodCode = "1";
                s.method.methodID = 1;
                s.method.methodDescription = "Global Climate Data";
                s.method.methodLink = "http://eca.knmi.nl";

                //qc level
                s.qualityControlLevel = new QualityControlLevelType();
                s.qualityControlLevel.definition = "Quality Controlled Data";
                s.qualityControlLevel.explanation = "Quality Controlled Data";
                s.qualityControlLevel.qualityControlLevelCode = "1";
                s.qualityControlLevel.qualityControlLevelID = 1;
                s.qualityControlLevel.qualityControlLevelIDSpecified = true;

                //value count, begin time, end time
                string sql = string.Format(Resources.SqlQueries.query_seriescatalog, dbSiteId, variableId);
                string connStr = GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        SqlDataReader dr = cmd.ExecuteReader();

                        dr.Read();

                        s.valueCount = new seriesCatalogTypeSeriesValueCount();
                        s.valueCount.Value = Convert.ToInt32(dr["value_count"]);

                        s.variableTimeInterval = new TimePeriodType();
                        s.variableTimeInterval.beginDateTime = Convert.ToDateTime(dr["start_date"]);
                        s.variableTimeInterval.beginDateTimeUTC = s.variableTimeInterval.beginDateTime;
                        s.variableTimeInterval.beginDateTimeUTCSpecified = true;

                        s.variableTimeInterval.endDateTime = Convert.ToDateTime(dr["end_date"]);
                        s.variableTimeInterval.endDateTimeUTC = s.variableTimeInterval.endDateTime;
                        s.variableTimeInterval.endDateTimeUTCSpecified = true;
                    }

                }

                //variable
                s.variable = GetVariableInfoByID(variableId);
                s.dataType = s.variable.dataType;
                s.generalCategory = s.variable.generalCategory;
                s.valueType = s.variable.valueType;

                //source
                s.sampleMedium = s.variable.sampleMedium;
                s.source = new SourceType();
                s.source.citation = "NCDC World Climate Data Online";
                s.source.organization = "NCDC";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "NCDC Climate Data Online";
                s.source.sourceID = 1;
                s.source.sourceIDSpecified = true;
                s.source.sourceLink = new string[] { "http://www7.ncdc.noaa.gov/rest" };

                return s;
            }
            catch (Exception ex)
            {
                WriteLog("GetSeriesCatalogFromDb - " + ex.Message);
                return null;
            }
        }

        

        public static VariableInfoType[] GetVariablesFromDb()
        {
            List<VariableInfoType> variablesList = new List<VariableInfoType>();

            string cnn = GetConnectionString();

            List<string> variableCodes = new List<string>();

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sqlVariables = "SELECT VariableCode from variables WHERE VariableCode in (SELECT var_code FROM series)";
                    cmd.CommandText = sqlVariables;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string code = dr["VariableCode"].ToString();
                        variableCodes.Add("HCLIMATE:" + code);
                        
                        VariableInfoType varInfo = new VariableInfoType();
                    }
                    conn.Close();
                }
            }
            foreach (string varCode in variableCodes)
            {
                VariableInfoType vi = GetVariableInfoFromDb(varCode);
                variablesList.Add(vi);
            }
            return variablesList.ToArray();
        }

        internal static VariableInfoType GetVariableInfoByID(int variableID)
        {
            string variableCode = "HCLIMATE:PRCP";
            switch(variableID)
            {
                case 1:
                    variableCode = "HCLIMATE:PRCP";
                    break;
                case 2:
                    variableCode = "HCLIMATE:TMIN";
                    break;
                case 3:
                    variableCode = "HCLIMATE:TAVG";
                    break;
                case 4:
                    variableCode = "HCLIMATE:TMAX";
                    break;
                default:
                    break;
            }
            return GetVariableInfoFromDb(variableCode);
        }

        internal static VariableInfoType GetVariableInfoFromDb(string VariableParameter)
        {
            WriteLog("Executing GetVariableInfoFromDb(" + VariableParameter + ")");
            
            VariableInfoType varInfo = new VariableInfoType();
            
            try
            {
            
            string cnn = GetConnectionString();
            string variableCode = VariableParameter.Substring(VariableParameter.LastIndexOf(":") + 1);

                using (SqlConnection conn = new SqlConnection(cnn))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sqlVariables = string.Format(
                            "SELECT var_id, VariableCode, VariableName, UnitsName, UnitsAbbrev, DataType, ValueType, " +
                            "NoDataValue, SampleMedium, UnitsID FROM variables where VariableCode='{0}'", variableCode);
                        //WriteLog("variableCode:" + variableCode);
                        cmd.CommandText = sqlVariables;
                        cmd.Connection = conn;
                        conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        dr.Read();

                        varInfo.dataType = dr["DataType"].ToString();
                        varInfo.generalCategory = "Climate";
                        varInfo.metadataTimeSpecified = false;
                        varInfo.noDataValue = -9999.0;
                        varInfo.noDataValueSpecified = true;
                        varInfo.sampleMedium = dr["SampleMedium"].ToString();
                        varInfo.speciation = "Not Applicable";

                        //time support and time unit
                        varInfo.timeScale = new VariableInfoTypeTimeScale();
                        varInfo.timeScale.isRegular = true;
                        varInfo.timeScale.timeSpacingSpecified = false;
                        varInfo.timeScale.timeSupport = 1.0f;
                        varInfo.timeScale.timeSupportSpecified = true;
                        varInfo.timeScale.unit = new UnitsType();
                        varInfo.timeScale.unit.unitAbbreviation = "d";
                        varInfo.timeScale.unit.unitCode = "104";
                        varInfo.timeScale.unit.unitID = 0;
                        varInfo.timeScale.unit.unitName = "day";
                        varInfo.timeScale.unit.unitType = "Time";

                        //variable unit
                        varInfo.unit = new UnitsType();
                        varInfo.unit.unitAbbreviation = Convert.ToString(dr["UnitsAbbrev"]);
                        varInfo.unit.unitCode = dr["UnitsID"].ToString();
                        varInfo.unit.unitDescription = dr["UnitsName"].ToString();
                        varInfo.unit.unitID = 1;
                        varInfo.unit.unitIDSpecified = true;
                        varInfo.unit.unitName = dr["UnitsName"].ToString();
                        varInfo.unit.unitType = "Length";

                        //variable code
                        varInfo.valueType = dr["ValueType"].ToString();

                        varInfo.variableCode = new VariableInfoTypeVariableCode[1];
                        varInfo.variableCode[0] = new VariableInfoTypeVariableCode();
                        varInfo.variableCode[0].@default = true;
                        varInfo.variableCode[0].defaultSpecified = true;
                        varInfo.variableCode[0].Value = Convert.ToString(dr["VariableCode"]);
                        varInfo.variableCode[0].vocabulary = ConfigurationManager.AppSettings["vocabulary"];
                        varInfo.variableCode[0].variableID = Convert.ToInt32(dr["var_id"]);

                        varInfo.variableName = Convert.ToString(dr["VariableName"]);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("GetVariableInfoFromDb - " + ex.Message);
            }
            return varInfo;
        }

        public static SiteInfoType GetSiteFromDb2(string siteId)
        {
            string cnn = GetConnectionString();
            string serviceCode = ConfigurationManager.AppSettings["network"];
            SiteInfoType si = new SiteInfoType();

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sqlSite = string.Format(Resources.SqlQueries.query_sitebyid, siteId);

                    cmd.CommandText = sqlSite;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    dr.Read();
                    if (dr.HasRows)
                    {       
                        si.geoLocation = new SiteInfoTypeGeoLocation();
                        LatLonPointType latLon = new LatLonPointType();
                        latLon.latitude = Math.Round(Convert.ToDouble(dr["lat"]),3);
                        latLon.longitude = Math.Round(Convert.ToDouble(dr["lon"]),3);
                        latLon.srs = "EPSG:4326";
                        si.geoLocation.geogLocation = latLon;
                        si.siteCode = new SiteInfoTypeSiteCode[1];
                        si.siteCode[0] = new SiteInfoTypeSiteCode();
                        si.siteCode[0].network = serviceCode;
                        si.siteCode[0].siteID = Convert.ToInt32(dr["wmo_id"]);
                        si.siteCode[0].siteIDSpecified = true;
                        si.siteCode[0].Value = Convert.ToString(dr["wmo_id"]);
                        si.siteName = Convert.ToString(dr["st_name"]);
                        si.elevation_m = Convert.ToDouble(dr["elev"]);
                        si.verticalDatum = "Unknown";

                        si.note = new NoteType[2];
                        si.note[0] = new NoteType();
                        si.note[0].title = "Country";
                        si.note[0].type = "custom";
                        si.note[0].Value = dr["country"].ToString();
 
                        si.note[1] = new NoteType();
                        si.note[1].title = "db_id";
                        si.note[1].type = "custom";
                        si.note[1].Value = dr["st_id"].ToString();
                    }
                }
            }
            return si;
        }

        public static SiteInfoResponseTypeSite GetSiteFromDb(string siteId, bool includeSeriesCatalog)
        {
            WriteLog("Executing GetSiteFromDb(" + siteId + ", " + includeSeriesCatalog + ")");
            
            try
            {
                SiteInfoResponseTypeSite newSite = new SiteInfoResponseTypeSite();

                newSite.siteInfo = GetSiteFromDb2(siteId);

                //db site id
                int dbSiteId = Convert.ToInt32(newSite.siteInfo.note[1].Value);
                WriteLog("DbSiteID: " + dbSiteId);

                //to add the catalog
                if (includeSeriesCatalog)
                {
                    //first, get the variables for the site
                    string sqlVariables = String.Format("SELECT var_id FROM series WHERE st_id = {0} AND value_count > 1000", dbSiteId);
                    List<int> variableIDs = new List<int>();

                    string connStr = GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlVariables, conn))
                        {
                            conn.Open();

                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                int varID = Convert.ToInt32(dr["var_id"]);
                                variableIDs.Add(varID);
                            }
                            conn.Close();
                        }
                    }

                    int numVariables = variableIDs.Count;
                    WriteLog("numVariables: " + numVariables);

                    newSite.seriesCatalog = new seriesCatalogType[1];
                    newSite.seriesCatalog[0] = new seriesCatalogType();
                    newSite.seriesCatalog[0].series = new seriesCatalogTypeSeries[numVariables];

                    for (int i = 0; i < numVariables; i++)
                    {
                        newSite.seriesCatalog[0].series[i] = GetSeriesCatalogFromDb(dbSiteId, variableIDs[i]);
                    }
                }
                return newSite;

            }
            catch (Exception ex)
            {
                WriteLog("WebServiceUtils.GetSiteFromDb " + ex.Message);
                return null;
            }

        }

        private static int GetDbSiteId(int wmoId)
        {
            string connStr = GetConnectionString();
            int dbId = -1;
            string sql = String.Format("SELECT st_id FROM st WHERE wmo_id={0}", wmoId);
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    dbId = Convert.ToInt32(obj);
                }
            }
            return dbId;
        }



        /// <summary>
        /// Get the values, from the Db
        /// </summary>
        /// <param name="siteId">site code (networkPrefix:SiteCode)</param>
        /// <param name="variableId">variable code (vocabularyPrefix:VariableCode)</param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        internal static TsValuesSingleVariableType GetValuesFromDb(string siteId, string variableId, DateTime startDateTime, DateTime endDateTime)
        {
            WriteLog(String.Format("GetValuesFromDb({0},{1},{2},{3}", siteId, variableId, startDateTime, endDateTime));
            
            try
            {

                //to get values, from the db
                TsValuesSingleVariableType s = new TsValuesSingleVariableType();
                s.censorCode = new CensorCodeType[1];
                s.censorCode[0] = new CensorCodeType();
                s.censorCode[0].censorCode = "nc";
                s.censorCode[0].censorCodeDescription = "not censored";
                s.censorCode[0].censorCodeID = 1;
                s.censorCode[0].censorCodeIDSpecified = true;

                //method
                s.method = new MethodType[1];
                s.method[0] = new MethodType();
                s.method[0].methodCode = "1";
                s.method[0].methodID = 1;
                s.method[0].methodDescription = "NCDC METAR observation";
                s.method[0].methodLink = "http://www7.ncdc.noaa.gov";

                //qc level
                s.qualityControlLevel = new QualityControlLevelType[1];
                s.qualityControlLevel[0] = new QualityControlLevelType();
                s.qualityControlLevel[0].definition = "raw data";
                s.qualityControlLevel[0].explanation = "raw data";
                s.qualityControlLevel[0].qualityControlLevelCode = "1";
                s.qualityControlLevel[0].qualityControlLevelID = 1;
                s.qualityControlLevel[0].qualityControlLevelIDSpecified = true;

                //source
                s.source = new SourceType[1];
                s.source[0] = new SourceType();
                s.source[0].citation = "NCDC";
                s.source[0].organization = "NCDC";
                s.source[0].sourceCode = "1";
                s.source[0].sourceDescription = "NCDC Climate Data Online Web Service";
                s.source[0].sourceID = 1;
                s.source[0].sourceIDSpecified = true;

                //time units
                s.units = new UnitsType();
                s.units.unitAbbreviation = "d";
                s.units.unitCode = "d";
                s.units.unitID = 0;
                s.units.unitName = "day";
                s.units.unitType = "Time";

                //get db site id
                int dbSiteId = GetDbSiteId(Convert.ToInt32(siteId));

                //values: get from database...
                string connStr = GetConnectionString();

                string sqlQuery = "SELECT obs_date, obs_value FROM pcp WHERE st_id = @p1 AND obs_date > @p2 AND obs_date < @p3 ORDER BY obs_date";

                switch (variableId)
                {
                    case "TMIN":
                        sqlQuery = "SELECT obs_date, tmin FROM tmp WHERE st_id=@p1 AND obs_date > @p2 AND obs_date < @p3 ORDER BY obs_date";
                        break;
                    case "TAVG":
                        sqlQuery = "SELECT obs_date, tavg FROM tmp WHERE st_id=@p1 AND obs_date > @p2 AND obs_date < @p3 ORDER BY obs_date";
                        break;
                    case "TMAX":
                         sqlQuery = "SELECT obs_date, tmax FROM tmp WHERE st_id=@p1 AND obs_date > @p2 AND obs_date < @p3 ORDER BY obs_date";
                        break;
                }


                List<ValueSingleVariable> valuesList = new List<ValueSingleVariable>();
                using (SqlConnection cnn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, cnn))
                    {
                        cmd.Parameters.Add("@p1", SqlDbType.SmallInt);
                        cmd.Parameters.Add("@p2", SqlDbType.Date);
                        cmd.Parameters.Add("@p3", SqlDbType.Date);
                        cmd.Parameters["@p1"].Value = dbSiteId;
                        cmd.Parameters["@p2"].Value = startDateTime;
                        cmd.Parameters["@p3"].Value = endDateTime;
                        
                        cnn.Open();

                        SqlDataReader r = cmd.ExecuteReader();
                        while (r.Read())
                        {
                            ValueSingleVariable v = new ValueSingleVariable();
                            v.censorCode = "nc";
                            v.dateTime = Convert.ToDateTime(r["obs_date"]).AddHours(7);
                            v.dateTimeUTC = v.dateTime.AddHours(-1);
                            v.dateTimeUTCSpecified = true;
                            v.methodCode = "1";
                            v.methodID = "1";
                            v.offsetValueSpecified = false;
                            v.qualityControlLevelCode = "1";
                            v.sourceCode = "1";
                            v.sourceID = "1";
                            v.timeOffset = "01:00";
                            double iValue = Convert.ToDouble(r[1]);
                            if (iValue > -8000 && iValue < 2000)
                            {
                                v.Value = Convert.ToDecimal(Math.Round(iValue / 10.0, 1));
                            }
                            else
                            {
                                v.Value = Convert.ToDecimal(-9999.0);
                            }
                            valuesList.Add(v);
                        }
                    }
                }

                //convert list to array
                ValueSingleVariable[] valuesArray = FillValuesList(valuesList);

                //DateTime beginDate = valuesList[0].dateTime;
                //DateTime endDate = valuesList[valuesList.Count - 1].dateTime;
                //int numDays = endDate.Subtract(beginDate).Days;
                //ValueSingleVariable[] valuesArray = new ValueSingleVariable[numDays];
                //int valueIndex = 0;

                //DateTime curDate = beginDate;
                //foreach (ValueSingleVariable val in valuesList)
                //{
                //    if (valueIndex >= valuesArray.Length) break;

                //    while (curDate < val.dateTime)
                //    {
                //        valuesArray[valueIndex] = CreateNoDataValue(curDate);
                //        curDate = curDate.AddDays(1);
                //        valueIndex++;
                //    }

                //    if (valueIndex >= valuesArray.Length) break;
                //    valuesArray[valueIndex] = val;
                //    curDate = val.dateTime.AddDays(1);
                //    valueIndex++;
                //}

                s.value = valuesArray;
                return s;

            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return new TsValuesSingleVariableType();
            }
        }

        //This function fills any gaps in the ValuesList
        private static ValueSingleVariable[] FillValuesList(List<ValueSingleVariable> valuesList)
        {
            DateTime startDate = valuesList[0].dateTime;
            DateTime endDate = valuesList[valuesList.Count - 1].dateTime;
            int numDays = (int)(Math.Round((endDate - startDate).TotalDays));
            ValueSingleVariable[] result = new ValueSingleVariable[numDays];

            //1-add the valid dates
            foreach (ValueSingleVariable val in valuesList)
            {
                int index = (int)(val.dateTime.Subtract(startDate).TotalDays);
                if (index >= result.Length) break;
                result[index] = val;
            }
            //2-add the other dates
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == null)
                {
                    DateTime t = startDate.AddDays(i);
                    result[i] = CreateNoDataValue(t);
                }
            }
            return result;
        }

        private static ValueSingleVariable CreateNoDataValue(DateTime time)
        {
            ValueSingleVariable v = new ValueSingleVariable();
            v.censorCode = "nc";
            v.dateTime = Convert.ToDateTime(time);
            v.dateTimeUTC = v.dateTime.AddHours(-1);
            v.dateTimeUTCSpecified = true;
            v.methodCode = "1";
            v.methodID = "1";
            v.offsetValueSpecified = false;
            v.qualityControlLevelCode = "1";
            v.sourceCode = "1";
            v.sourceID = "1";
            v.timeOffset = "01:00";
            v.Value = 0.0M;
            return v;
        }
    }
}
