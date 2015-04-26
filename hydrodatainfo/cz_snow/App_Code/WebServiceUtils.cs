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
using WaterOneFlowImpl.geom;

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
                    string sqlSites = "SELECT Stations.st_id, st_name, altitude, location_id FROM Stations  WHERE Stations.st_id IN (SELECT st_id FROM StationsVariables WHERE var_id=8 OR var_id=1)";
                    string sql = "SELECT st.st_id, st_name, lat, lon, altitude FROM " +
                        "(" + sqlSites + ") st INNER JOIN Locations ON st.location_id = Locations.loc_id";
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        SiteInfoResponseTypeSite newSite = new SiteInfoResponseTypeSite();
                        SiteInfoType si = new SiteInfoType();

                        if (dr["altitude"] != DBNull.Value)
                        {
                            si.elevation_m = Convert.ToDouble(dr["altitude"]);
                            si.elevation_mSpecified = true;
                        }
                        else
                        {
                            si.elevation_m = 0;
                            si.elevation_mSpecified = true;
                        }
                        si.geoLocation = new SiteInfoTypeGeoLocation();
                        
                        LatLonPointType latLon = new LatLonPointType();
                        latLon.latitude = Convert.ToDouble(dr["Lat"]);
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
                        si.note = new NoteType[1];
                        si.note[0] = new NoteType();
                        si.note[0].title = "my note";
                        si.note[0].type = "custom";
                        si.note[0].Value = "CZSNW";
                        si.verticalDatum = "Unknown";

                        si.siteCode = new SiteInfoTypeSiteCode[1];
                        si.siteCode[0] = new SiteInfoTypeSiteCode();
                        si.siteCode[0].network = serviceCode;
                        si.siteCode[0].siteID = Convert.ToInt32(dr["st_id"]);
                        si.siteCode[0].siteIDSpecified = true;
                        si.siteCode[0].Value = Convert.ToString(dr["st_id"]);

                        si.siteName = Convert.ToString(dr["st_name"]);

                        newSite.siteInfo = si;
                        siteList.Add(newSite);
                    }
                }
            }
            return siteList.ToArray();
        }

        /// <summary>
        /// Gets the sites, in XML format [test for SNOW]
        /// </summary>
        public static SiteInfoResponseTypeSite[] GetSitesByBox(box queryBox, bool includeSeries)
        {
            List<SiteInfoResponseTypeSite> siteList = new List<SiteInfoResponseTypeSite>();

            string cnn = GetConnectionString();
            string serviceCode = ConfigurationManager.AppSettings["network"];

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sqlSites = "SELECT Stations.st_id, st_name, altitude, location_id FROM Stations INNER JOIN StationsVariables stv ON Stations.st_id = stv.st_id WHERE var_id=8 OR (var_id=1 AND operator_id=1)";
                    string sql = "SELECT st.st_id, st_name, lat, lon, altitude FROM " +
                        "(" + sqlSites + ") st INNER JOIN Locations ON st.location_id = Locations.loc_id";
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        SiteInfoResponseTypeSite newSite = new SiteInfoResponseTypeSite();
                        SiteInfoType si = new SiteInfoType();

                        if (dr["altitude"] != DBNull.Value)
                        {
                            si.elevation_m = Convert.ToDouble(dr["altitude"]);
                            si.elevation_mSpecified = true;
                        }
                        else
                        {
                            si.elevation_m = 0;
                            si.elevation_mSpecified = true;
                        }
                        si.geoLocation = new SiteInfoTypeGeoLocation();

                        LatLonPointType latLon = new LatLonPointType();
                        latLon.latitude = Convert.ToDouble(dr["Lat"]);
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
                        si.note = new NoteType[1];
                        si.note[0] = new NoteType();
                        si.note[0].title = "my note";
                        si.note[0].type = "custom";
                        si.note[0].Value = "CZSNW";
                        si.verticalDatum = "Unknown";

                        si.siteCode = new SiteInfoTypeSiteCode[1];
                        si.siteCode[0] = new SiteInfoTypeSiteCode();
                        si.siteCode[0].network = serviceCode;
                        si.siteCode[0].siteID = Convert.ToInt32(dr["st_id"]);
                        si.siteCode[0].siteIDSpecified = true;
                        si.siteCode[0].Value = Convert.ToString(dr["st_id"]);

                        si.siteName = Convert.ToString(dr["st_name"]);

                        newSite.siteInfo = si;
                        siteList.Add(newSite);
                    }
                }
            }
            return siteList.ToArray();
        }

        public static seriesCatalogTypeSeries GetSeriesCatalogFromDb(int siteId, int variableId)
        {
            seriesCatalogTypeSeries s = new seriesCatalogTypeSeries();

            s.dataType = "Average";
            s.generalCategory = "Climate";

            //method
            s.method = new MethodType();
            if (variableId == 8)
            {
                s.method.methodCode = "1";
                s.method.methodID = 1;
                s.method.methodDescription = "Snow measured at 6:00Z on open ground";
                s.method.methodLink = "hydro.chmi.cz/hpps";
            }
            else
            {
                s.method.methodCode = "2";
                s.method.methodID = 2;
                s.method.methodDescription = "Precipitation measured by tipping-bucket raingauge and aggregated to daily";
                s.method.methodLink = "hydro.chmi.cz/hpps";
            }

            //qc level
            s.qualityControlLevel = new QualityControlLevelType();
            s.qualityControlLevel.definition = "raw data";
            s.qualityControlLevel.explanation = "raw data";
            s.qualityControlLevel.qualityControlLevelCode = "1";
            s.qualityControlLevel.qualityControlLevelID = 1;
            s.qualityControlLevel.qualityControlLevelIDSpecified = true;

            //source - accommodate for POH
            int operatorId = 1;
            string connStr = GetConnectionString();
            string sqlSource = string.Format(Resources.SqlQueries.query_operatorid, siteId);
            {
                using (SqlConnection con01 = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd01 = new SqlCommand(sqlSource, con01))
                    {
                        con01.Open();
                        object obj = cmd01.ExecuteScalar();
                        if (obj != null)
                        {
                            operatorId = Convert.ToInt32(obj);
                        }
                    }
                }
            }
            if (variableId == 8)
            {
                s.sampleMedium = "Snow";
            }
            else
            {
                s.sampleMedium = "Precipitation";
            }
            s.source = new SourceType();

            if (operatorId == 1)
            {
                s.source.citation = "CHMI";
                s.source.organization = "CHMI";
                s.source.sourceCode = "1";
                if (variableId == 8)
                    s.source.sourceDescription = "snow depth measured by CHMI professional stations";
                else
                    s.source.sourceDescription = "CHMI automated stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }
            else if (operatorId == 3)
            {
                s.source.citation = "POH";
                s.source.organization = "Ohře Watershed Authority";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "measured by Ohře watershed authority stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }
            else if (operatorId == 2)
            {
                s.source.citation = "PVL";
                s.source.organization = "Povodí Vltavy";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "measured by Vltava watershed authority professional stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }
            else if (operatorId == 4)
            {
                s.source.citation = "PLA";
                s.source.organization = "Povodí Labe";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "measured by Labe watershed authority professional stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }
            else if (operatorId == 5)
            {
                s.source.citation = "PMO";
                s.source.organization = "Povodí Moravy";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "measured by Morava watershed authority professional stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }
            else if (operatorId == 6)
            {
                s.source.citation = "POD";
                s.source.organization = "Povodí Odry";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "measured by Odra watershed authority professional stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }
            else
            {
                s.source.citation = "DWD";
                s.source.organization = "Deutscher Wetterdienst";
                s.source.sourceCode = "1";
                s.source.sourceDescription = "snow depth measured by Deutscher Wetterdienst professional stations";
                s.source.sourceID = operatorId;
                s.source.sourceIDSpecified = true;
            }

            //value count, begin time, end time
            string sql = string.Format(Resources.SqlQueries.query_seriescatalog, siteId, variableId);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        s.valueCount = new seriesCatalogTypeSeriesValueCount();
                        s.valueCount.Value = Convert.ToInt32(dr["ValueCount"]);
                        s.valueType = "Field Observation";

                        s.variableTimeInterval = new TimeIntervalType();
                        s.variableTimeInterval.beginDateTime = Convert.ToDateTime(dr["BeginDate"]);
                        s.variableTimeInterval.beginDateTimeUTC = s.variableTimeInterval.beginDateTime.AddHours(-1);
                        s.variableTimeInterval.beginDateTimeUTCSpecified = true;

                        s.variableTimeInterval.endDateTime = Convert.ToDateTime(dr["EndDate"]);
                        s.variableTimeInterval.endDateTimeUTC = s.variableTimeInterval.endDateTime.AddHours(-1);
                        s.variableTimeInterval.endDateTimeUTCSpecified = true;
                    }
                }

            }

            //variable
            if (variableId == 8)
            {
                s.variable = GetVariableInfoFromDb("CZSNW:8");
            }
            else
            {
                s.variable = GetVariableInfoFromDb("CZSNW:1");
            }

            return s;
        }

        public static VariableInfoType[] GetVariablesFromDb()
        {
            List<VariableInfoType> variablesList = new List<VariableInfoType>();

            string cnn = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sqlVariables = "select var_id, var_name, var_units, interval_h from variables where var_id=8 OR var_id=1";
                    cmd.CommandText = sqlVariables;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        VariableInfoType varInfo = new VariableInfoType();

                        int var_id = Convert.ToInt32(dr["var_id"]);

                        if (var_id == 1)
                        {

                            varInfo.dataType = "Incremental";
                            varInfo.generalCategory = "Climate";
                            varInfo.metadataTimeSpecified = false;
                            varInfo.noDataValue = -9999.0;
                            varInfo.noDataValueSpecified = true;
                            varInfo.sampleMedium = "Precipitation";
                            varInfo.speciation = "Not Applicable";

                            //time support and time unit
                            varInfo.timeScale = new VariableInfoTypeTimeScale();
                            varInfo.timeScale.isRegular = true;
                            varInfo.timeScale.timeSpacingSpecified = false;
                            varInfo.timeScale.timeSupport = 0.0f;
                            varInfo.timeScale.timeSupportSpecified = true;
                            varInfo.timeScale.unit = new UnitsType();
                            varInfo.timeScale.unit.unitAbbreviation = "d";
                            varInfo.timeScale.unit.unitCode = "104";
                            varInfo.timeScale.unit.unitID = 0;
                            varInfo.timeScale.unit.unitName = "day";
                            varInfo.timeScale.unit.unitType = "Time";

                            //variable unit
                            varInfo.unit = new UnitsType();
                            varInfo.unit.unitAbbreviation = "mm";
                            varInfo.unit.unitCode = "54";
                            varInfo.unit.unitDescription = "millimeter";
                            varInfo.unit.unitID = 1;
                            varInfo.unit.unitIDSpecified = true;
                            varInfo.unit.unitName = "millimeter";
                            varInfo.unit.unitType = "Length";

                            //variable code
                            varInfo.valueType = "Field Observation";

                            varInfo.variableCode = new VariableInfoTypeVariableCode[1];
                            varInfo.variableCode[0] = new VariableInfoTypeVariableCode();
                            varInfo.variableCode[0].@default = true;
                            varInfo.variableCode[0].defaultSpecified = true;
                            varInfo.variableCode[0].Value = Convert.ToString(dr["var_id"]);
                            varInfo.variableCode[0].vocabulary = ConfigurationManager.AppSettings["vocabulary"];
                            varInfo.variableCode[0].variableID = Convert.ToInt32(dr["var_id"]);

                            varInfo.variableName = "Precipitation";
                        }
                        else
                        {
                            varInfo.dataType = "Continuous";
                            varInfo.generalCategory = "Climate";
                            varInfo.metadataTimeSpecified = false;
                            varInfo.noDataValue = -9999.0;
                            varInfo.noDataValueSpecified = true;
                            varInfo.sampleMedium = "Snow";
                            varInfo.speciation = "Not Applicable";

                            //time support and time unit
                            varInfo.timeScale = new VariableInfoTypeTimeScale();
                            varInfo.timeScale.isRegular = true;
                            varInfo.timeScale.timeSpacingSpecified = false;
                            varInfo.timeScale.timeSupport = 0.0f;
                            varInfo.timeScale.timeSupportSpecified = true;
                            varInfo.timeScale.unit = new UnitsType();
                            varInfo.timeScale.unit.unitAbbreviation = "d";
                            varInfo.timeScale.unit.unitCode = "104";
                            varInfo.timeScale.unit.unitID = 0;
                            varInfo.timeScale.unit.unitName = "day";
                            varInfo.timeScale.unit.unitType = "Time";

                            //variable unit
                            varInfo.unit = new UnitsType();
                            varInfo.unit.unitAbbreviation = "cm";
                            varInfo.unit.unitCode = "47";
                            varInfo.unit.unitDescription = "centimeter";
                            varInfo.unit.unitID = 1;
                            varInfo.unit.unitIDSpecified = true;
                            varInfo.unit.unitName = "centimeter";
                            varInfo.unit.unitType = "Length";

                            //variable code
                            varInfo.valueType = "Field Observation";

                            varInfo.variableCode = new VariableInfoTypeVariableCode[1];
                            varInfo.variableCode[0] = new VariableInfoTypeVariableCode();
                            varInfo.variableCode[0].@default = true;
                            varInfo.variableCode[0].defaultSpecified = true;
                            varInfo.variableCode[0].Value = Convert.ToString(dr["var_id"]);
                            varInfo.variableCode[0].vocabulary = ConfigurationManager.AppSettings["vocabulary"];
                            varInfo.variableCode[0].variableID = Convert.ToInt32(dr["var_id"]);

                            varInfo.variableName = "Snow depth";
                        }
                        
                        variablesList.Add(varInfo);
                    }
                }
            }
            return variablesList.ToArray();
        }

        internal static VariableInfoType GetVariableInfoFromDb(string VariableParameter)
        {
            VariableInfoType varInfo = new VariableInfoType();
            string cnn = GetConnectionString();
            string variableId = VariableParameter.Substring(VariableParameter.LastIndexOf(":") + 1);

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sqlVariables = string.Format(
                        "select var_id, var_name, var_units, interval_h from variables where var_id={0}", variableId);
                    cmd.CommandText = sqlVariables;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();

                        int var_id = Convert.ToInt32(dr["var_id"]);
                        if (var_id == 1)
                        {
                            varInfo.dataType = "Incremental";
                            varInfo.generalCategory = "Climate";
                            varInfo.metadataTimeSpecified = false;
                            varInfo.noDataValue = -9999.0;
                            varInfo.noDataValueSpecified = true;
                            varInfo.sampleMedium = "Precipitation";
                            varInfo.speciation = "Not Applicable";

                            //time support and time unit
                            varInfo.timeScale = new VariableInfoTypeTimeScale();
                            varInfo.timeScale.isRegular = true;
                            varInfo.timeScale.timeSpacingSpecified = false;
                            varInfo.timeScale.timeSupport = 0.0f;
                            varInfo.timeScale.timeSupportSpecified = true;
                            varInfo.timeScale.unit = new UnitsType();
                            varInfo.timeScale.unit.unitAbbreviation = "d";
                            varInfo.timeScale.unit.unitCode = "104";
                            varInfo.timeScale.unit.unitID = 0;
                            varInfo.timeScale.unit.unitName = "day";
                            varInfo.timeScale.unit.unitType = "Time";

                            //variable unit
                            varInfo.unit = new UnitsType();
                            varInfo.unit.unitAbbreviation = "mm";
                            varInfo.unit.unitCode = "54";
                            varInfo.unit.unitDescription = "millimeter";
                            varInfo.unit.unitID = 1;
                            varInfo.unit.unitIDSpecified = true;
                            varInfo.unit.unitName = "millimeter";
                            varInfo.unit.unitType = "Length";

                            //variable code
                            varInfo.valueType = "Field Observation";

                            varInfo.variableCode = new VariableInfoTypeVariableCode[1];
                            varInfo.variableCode[0] = new VariableInfoTypeVariableCode();
                            varInfo.variableCode[0].@default = true;
                            varInfo.variableCode[0].defaultSpecified = true;
                            varInfo.variableCode[0].Value = Convert.ToString(dr["var_id"]);
                            varInfo.variableCode[0].vocabulary = ConfigurationManager.AppSettings["vocabulary"];
                            varInfo.variableCode[0].variableID = Convert.ToInt32(dr["var_id"]);

                            varInfo.variableName = "Precipitation";
                        }
                        else
                        {
                            varInfo.dataType = "Average";
                            varInfo.generalCategory = "Climate";
                            varInfo.metadataTimeSpecified = false;
                            varInfo.noDataValue = -9999.0;
                            varInfo.noDataValueSpecified = true;
                            varInfo.sampleMedium = "Snow";
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
                            varInfo.unit.unitAbbreviation = Convert.ToString(dr["var_units"]);
                            varInfo.unit.unitCode = "47";
                            varInfo.unit.unitDescription = "centimeter";
                            varInfo.unit.unitID = 1;
                            varInfo.unit.unitIDSpecified = true;
                            varInfo.unit.unitName = "centimeter";
                            varInfo.unit.unitType = "Length";

                            //variable code
                            varInfo.valueType = "Field Observation";

                            varInfo.variableCode = new VariableInfoTypeVariableCode[1];
                            varInfo.variableCode[0] = new VariableInfoTypeVariableCode();
                            varInfo.variableCode[0].@default = true;
                            varInfo.variableCode[0].defaultSpecified = true;
                            varInfo.variableCode[0].Value = Convert.ToString(dr["var_id"]);
                            varInfo.variableCode[0].vocabulary = ConfigurationManager.AppSettings["vocabulary"];
                            varInfo.variableCode[0].variableID = Convert.ToInt32(dr["var_id"]);

                            varInfo.variableName = "Snow depth";
                        }
                    }
                }
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
                    string sqlSite = string.Format(Resources.SqlQueries.query_sitebyid2, siteId);

                    cmd.CommandText = sqlSite;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    dr.Read();
                    if (dr.HasRows)
                    {       
                        si.geoLocation = new SiteInfoTypeGeoLocation();
                        LatLonPointType latLon = new LatLonPointType();
                        latLon.latitude = Convert.ToDouble(dr["Lat"]);
                        latLon.longitude = Convert.ToDouble(dr["lon"]);
                        latLon.srs = "EPSG:4326";
                        si.geoLocation.geogLocation = latLon;
                        si.siteCode = new SiteInfoTypeSiteCode[1];
                        si.siteCode[0] = new SiteInfoTypeSiteCode();
                        si.siteCode[0].network = serviceCode;
                        si.siteCode[0].siteID = Convert.ToInt32(dr["st_id"]);
                        si.siteCode[0].siteIDSpecified = true;
                        si.siteCode[0].Value = Convert.ToString(dr["st_id"]);
                        si.siteName = Convert.ToString(dr["st_name"]);
                    }
                }
            }
            return si;
        }

        public static SiteInfoResponseTypeSite GetSiteFromDb(string siteId, bool includeSeriesCatalog)
        {
            SiteInfoResponseTypeSite newSite = new SiteInfoResponseTypeSite();

            newSite.siteInfo = GetSiteFromDb2(siteId);

            //to add the catalog
            if (includeSeriesCatalog)
            {
                List<int> variableIdList = GetVariablesForSite(Convert.ToInt32(siteId));
                int numVariables = variableIdList.Count;
                
                newSite.seriesCatalog = new seriesCatalogType[1];
                newSite.seriesCatalog[0] = new seriesCatalogType();
                newSite.seriesCatalog[0].series = new seriesCatalogTypeSeries[numVariables];

                for (int i = 0; i < numVariables; i++)
                {
                    newSite.seriesCatalog[0].series[i] = GetSeriesCatalogFromDb(Convert.ToInt32(siteId), variableIdList[i]);
                }
            }

            return newSite;
        }

        private static List<int> GetVariablesForSite(int siteId)
        {
            string cnn = GetConnectionString();
            string serviceCode = ConfigurationManager.AppSettings["network"];
            List<int> variableIdList = new List<int>();

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT var_id FROM StationsVariables WHERE st_id=" + siteId;

                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            variableIdList.Add(Convert.ToInt32(dr["var_id"]));
                        }
                    }
                }
            }
            return variableIdList;
        }

        /// <summary>
        /// Get the values, from the Db
        /// </summary>
        /// <param name="siteId">site id (local database id)</param>
        /// <param name="variableId">variable id (local database id)</param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        internal static TsValuesSingleVariableType GetValuesFromDb(string siteId, string variableId, DateTime startDateTime, DateTime endDateTime)
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
            int varId = Convert.ToInt32(variableId);
            if (varId == 8)
            {
                s.method[0].methodCode = "1";
                s.method[0].methodID = 1;
                s.method[0].methodDescription = "Snow measured at 6:00Z on open ground";
                s.method[0].methodLink = "hydro.chmi.cz/hpps";
            }
            else if (varId == 1)
            {
                s.method[0].methodCode = "2";
                s.method[0].methodID = 2;
                s.method[0].methodDescription = "Precipitation measured by tipping bucket";
                s.method[0].methodLink = "hydro.chmi.cz/hpps";
            }

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
            s.source[0].citation = "CHMI";
            s.source[0].organization = "CHMI";
            s.source[0].sourceCode = "1";
            s.source[0].sourceDescription = " measured by CHMI professional stations";
            s.source[0].sourceID = 1;
            s.source[0].sourceIDSpecified = true;

            //time units
            s.units = new UnitsType();
            s.units.unitAbbreviation = "d";
            s.units.unitCode = "d";
            s.units.unitID = 0;
            s.units.unitName = "day";
            s.units.unitType = "Time";

            //values: get from database...
            string connStr = GetConnectionString();
            List<ValueSingleVariable> valuesList = new List<ValueSingleVariable>();
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("qry_observations", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
                    cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.SmallInt));
                    cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
                    cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
                    cmd.Parameters.Add(new SqlParameter("@time_step", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@group_function", SqlDbType.VarChar));

                    cmd.Parameters["@st_id"].Value = Convert.ToInt32(siteId);
                    cmd.Parameters["@var_id"].Value = Convert.ToInt32(variableId);
                    cmd.Parameters["@start_time"].Value = startDateTime;
                    cmd.Parameters["@end_time"].Value = endDateTime;
                    cmd.Parameters["@time_step"].Value = "day";
                    cmd.Parameters["@group_function"].Value = "max";

                    cnn.Open();

                    SqlDataReader r = cmd.ExecuteReader();
                    if (varId == 8)
                    {
                        while (r.Read())
                        {
                            ValueSingleVariable v = new ValueSingleVariable();
                            v.censorCode = "nc";
                            v.dateTime = Convert.ToDateTime(r["obs_time"]);
                            v.dateTimeUTC = v.dateTime.AddHours(-1);
                            v.dateTimeUTCSpecified = true;
                            v.methodCode = "1";
                            v.methodID = "1";
                            v.offsetValueSpecified = false;
                            v.qualityControlLevelCode = "1";
                            v.sourceCode = "1";
                            v.sourceID = "1";
                            v.timeOffset = "01:00";
                            v.Value = Convert.ToDecimal(r["obs_value"]);
                            if (v.Value == -1) v.Value = 0.1M;
                            if (v.Value == -2) v.Value = 0.2M;
                            valuesList.Add(v);
                        }
                    }
                    else
                    {
                        while (r.Read())
                        {
                            ValueSingleVariable v = new ValueSingleVariable();
                            v.censorCode = "nc";
                            v.dateTime = Convert.ToDateTime(r["obs_time"]).AddHours(-19);
                            //v.dateTimeUTC = v.dateTime.AddHours(-1);
                            v.dateTimeUTCSpecified = true;
                            v.methodCode = "2";
                            v.methodID = "2";
                            v.offsetValueSpecified = false;
                            v.qualityControlLevelCode = "1";
                            v.sourceCode = "1";
                            v.sourceID = "1";
                            v.timeOffset = "01:00";
                            v.Value = Convert.ToDecimal(Convert.ToDouble(r["obs_value"]) * 0.1);
                            if (v.Value == -1) v.Value = 0.1M;
                            if (v.Value == -2) v.Value = 0.2M;
                            valuesList.Add(v);
                        }
                    }
                }
            }

            //convert list to array
            DateTime beginDate = valuesList[0].dateTime;
            DateTime endDate = valuesList[valuesList.Count - 1].dateTime;
            int numDays = endDate.Subtract(beginDate).Days;
            ValueSingleVariable[] valuesArray = new ValueSingleVariable[numDays];
            int valueIndex = 0;

            DateTime curDate = beginDate;
            foreach(ValueSingleVariable val in valuesList)
            {
                if (valueIndex >= valuesArray.Length) break;
                
                while (curDate < val.dateTime)
                {
                    valuesArray[valueIndex] = CreateNoDataValue(curDate);
                    curDate = curDate.AddDays(1);
                    valueIndex++;
                }

                if (valueIndex >= valuesArray.Length) break;
                valuesArray[valueIndex] = val;
                curDate = val.dateTime.AddDays(1);
                valueIndex++;
            }

            s.value = valuesArray;
            return s;
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
