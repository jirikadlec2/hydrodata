﻿using System;
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
                    string sql = "SELECT DISTINCT st.st_id, st.st_name, st.lat, st.lon, st.altitude FROM plaveninycz.Stations st " +
"INNER JOIN plaveninycz.stationsvariables stv ON st.st_id = stv.st_id WHERE st.lat is not NULL AND stv.var_id IN (1, 4, 5, 8, 16);";

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
                        //si.note = new NoteType[1];
                        //si.note[0] = new NoteType();
                        //si.note[0].title = "my note";
                        //si.note[0].type = "custom";
                        //si.note[0].Value = "CHMI-D";
                        si.verticalDatum = "MSL";

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
                    string sqlSites = "SELECT Stations.st_id, st_name, altitude, location_id FROM Stations INNER JOIN StationsVariables stv ON Stations.st_id = stv.st_id WHERE var_id in (1, 8, 5, 16) AND operator_id=1";
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
                        si.note[0].Value = "CHMI-D";
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

        public static string VariableIDToCode(int variableID) 
        {
            string prefix = "CHMI-D:";
            return prefix + VariableIDToShortCode(variableID);
        }

        public static string VariableIDToShortCode(int variableID) {
            switch (variableID)
            {
                case 1:
                    return "SRAZKY";
                case 4:
                    return "VODSTAV";
                case 5:
                    return "PRUTOK";
                case 8:
                    return "SNIH";
                case 16:
                    return "TEPLOTA";
                case 17:
                    return "TMIN";
                case 18:
                    return "TMAX";
                default:
                    return "UNKNOWN";
            }
        }

        public static int VariableCodeToID(string variableCode) 
        {
            string prefix = "CHMI-D:";
            string varCode = variableCode;
            if (variableCode.StartsWith(prefix))
            {
                varCode = varCode.Substring(prefix.Length);
            }
            switch (varCode)
            {
                case "SRAZKY":
                    return 1;
                case "VODSTAV":
                    return 4;
                case "PRUTOK":
                    return 5;
                case "SNIH":
                    return 8;
                case "TEPLOTA":
                    return 16;
                case "TMIN":
                    return 17;
                case "TMAX":
                    return 18;
                default:
                    return 0;
            }
        }

        private static string GetTableName(int variableId)
        {
            string tableName = "rain_hourly";
            switch(variableId)
            {
                case 1:
                    return "rain_hourly";
                    
                case 4:
                    return "stage";
                    
                case 5:
                    return "discharge";
                    
                case 8:
                    return "snow";
                    
                case 16:
                case 17:
                case 18:
                    return "temperature";
                    
                default:
                    return "rain_hourly";
            }
        }

        internal static SourceType GetSourceForSite(int siteId)
        {
            SourceType s = new SourceType();
            s.citation = "CHMI";
            s.organization = "CHMI";
            s.sourceCode = "1";
            s.sourceDescription = " measured by CHMI professional stations";
            s.sourceID = 1;
            s.sourceIDSpecified = true;

            string sql = "SELECT op.id, op.name, op.url FROM plaveninycz.operator op " +
                String.Format("INNER JOIN plaveninycz.stations s ON op.id = s.operator_id WHERE s.st_id = {0}", siteId);
            string connStr = GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        s.citation = Convert.ToString(dr["name"]);
                        s.organization = Convert.ToString(dr["name"]);
                        s.sourceCode = Convert.ToString(dr["id"]);
                        s.sourceLink = new string[1];
                        s.sourceLink[0] = Convert.ToString(dr["url"]);
                        s.sourceID = Convert.ToInt32(dr["id"]);
                    }
                }
            }
            return s;
        }

        public static seriesCatalogTypeSeries GetSeriesCatalogFromDb(int siteId, int variableId)
        {
            seriesCatalogTypeSeries s = new seriesCatalogTypeSeries();
            string connStr = GetConnectionString();

            s.generalCategory = "Climate";
            s.valueType = "Field Observation";

            //method
            s.method = GetMethodForVariable(variableId);

            //qc level
            s.qualityControlLevel = new QualityControlLevelType();
            s.qualityControlLevel.definition = "raw data";
            s.qualityControlLevel.explanation = "raw data";
            s.qualityControlLevel.qualityControlLevelCode = "1";
            s.qualityControlLevel.qualityControlLevelID = 1;
            s.qualityControlLevel.qualityControlLevelIDSpecified = true;

            //source
            s.source = GetSourceForSite(siteId);

            //table name
            //foldername
            string variableFolder = "prutok";
            switch (variableId)
            {
                case 1:
                    variableFolder = "srazky";
                    break;
                case 2:
                    variableFolder = "srayky";
                    break;
                case 4:
                    variableFolder = "vodstav";
                    break;
                case 5:
                    variableFolder = "prutok";
                    break;
                case 8:
                    variableFolder = "snih";
                    break;
                case 16:
                case 17:
                case 18:
                    variableFolder = "teplota";
                    break;
            }

            //value count, begin time, end time
            DateRange beginEndTime = new DateRange();

            if (variableFolder == "snih")
            {
                beginEndTime = GetDateRangeFromDb(siteId, 8);
            }
            else
            {
                string binFileName = BinaryFileHelper.GetBinaryFileName(siteId, variableFolder, "d");
                beginEndTime = BinaryFileHelper.BinaryFileDateRange(binFileName, "d");
            }
            if (beginEndTime.Start == null || beginEndTime.End == null)
            {
                return null;
            }

            s.variableTimeInterval = new TimeIntervalType();
            s.variableTimeInterval.beginDateTime = beginEndTime.Start;
            s.variableTimeInterval.beginDateTimeUTC = s.variableTimeInterval.beginDateTime.AddHours(-1);
            s.variableTimeInterval.beginDateTimeUTCSpecified = true;

            s.variableTimeInterval.endDateTime = beginEndTime.End;
            s.variableTimeInterval.endDateTimeUTC = s.variableTimeInterval.endDateTime.AddHours(-1);
            s.variableTimeInterval.endDateTimeUTCSpecified = true;

            double totalDays = (s.variableTimeInterval.endDateTime.Subtract(s.variableTimeInterval.beginDateTime)).TotalDays;
            s.valueCount = new seriesCatalogTypeSeriesValueCount();
            s.valueCount.Value = (int)(Math.Round(totalDays));

            //if no values --> series doesn't exist
            if (s.valueCount.Value == 0)
            {
                return null;
            }

            //variable
            s.variable = GetVariableInfoFromDb(VariableIDToCode(variableId));

            //data type, sample medium
            s.dataType = s.variable.dataType;
            s.sampleMedium = s.variable.sampleMedium;
            s.generalCategory = s.variable.generalCategory;
            return s;
        }

        public static DateRange GetDateRangeFromDb(int siteId, int varId)
        {
            string connStr = GetConnectionString();
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.MaxValue;

            string tableName = GetTableName(varId);
            bool hasValues = true;
            string sql = String.Format("SELECT MIN(time_utc) AS 'begin', MAX(time_utc) AS 'end' FROM plaveninycz.{0} WHERE station_id={1}", tableName, siteId);
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cnn.Open();
                    SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);
                    r.Read();
                    if (!r.HasRows)
                    {
                        hasValues = false;
                    }
                    else
                    {
                        object startObj = r["begin"];
                        object endObj = r["end"];
                        if (startObj is DBNull || endObj is DBNull)
                        {
                            hasValues = false;
                        }
                        else
                        {
                            startDateTime = Convert.ToDateTime(r["begin"]);
                            endDateTime = Convert.ToDateTime(r["end"]);
                        }
                    }
                }
            }

            DateRange range = new DateRange();
            range.Start = startDateTime;
            range.End = endDateTime;
            range.HasValues = hasValues;
            return range;
        }

        public static VariableInfoType[] GetVariablesFromDb()
        {
            List<VariableInfoType> variablesList = new List<VariableInfoType>();

            int[] variableIDs = { 1, 4, 5, 8, 16, 17, 18 };

            string cnn = GetConnectionString();
            foreach (int var_id in variableIDs)
            {
                VariableInfoType varInfo = new VariableInfoType();
                SetVariableProperties(var_id, varInfo);
                variablesList.Add(varInfo);
            }
            
            return variablesList.ToArray();
        }

        internal static void SetVariableProperties(int var_id, VariableInfoType varInfo)
        {
            //time support and time unit (same for all variables here)
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

            //variable code (same computation for all variables here)
            varInfo.valueType = "Field Observation";

            varInfo.variableCode = new VariableInfoTypeVariableCode[1];
            varInfo.variableCode[0] = new VariableInfoTypeVariableCode();
            varInfo.variableCode[0].@default = true;
            varInfo.variableCode[0].defaultSpecified = true;
            varInfo.variableCode[0].Value = VariableIDToShortCode(var_id);
            varInfo.variableCode[0].vocabulary = "CHMI-D";
            varInfo.variableCode[0].variableID = var_id;
            
            switch(var_id)
            {
                case 1:
                    //precipitation
                    varInfo.dataType = "Incremental";
                    varInfo.generalCategory = "Climate";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Precipitation";
                    varInfo.speciation = "Not Applicable";

                    //variable unit
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "mm";
                    varInfo.unit.unitCode = "54";
                    varInfo.unit.unitDescription = "millimeter";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "millimeter";
                    varInfo.unit.unitType = "Length";
                    varInfo.variableName = "Precipitation";
                    break;
                case 4:
                    //water level
                    varInfo.dataType = "Average";
                    varInfo.generalCategory = "Hydrology";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Water";
                    varInfo.speciation = "Not Applicable";

                    //variable unit - cubic meter per second
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "cm";
                    varInfo.unit.unitCode = "47";
                    varInfo.unit.unitDescription = "centimeter";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "centimeter";
                    varInfo.unit.unitType = "Length";

                    //variable value type and name
                    varInfo.valueType = "Field Observation";
                    varInfo.variableName = "Gage height";
                    break;
                case 5:
                    //discharge
                    varInfo.dataType = "Average";
                    varInfo.generalCategory = "Hydrology";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Water";
                    varInfo.speciation = "Not Applicable";

                    //variable unit - cubic meter per second
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "m^3/s";
                    varInfo.unit.unitCode = "36";
                    varInfo.unit.unitDescription = "cubic meters per second";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "cubic meters per second";
                    varInfo.unit.unitType = "Flow";
                    
                    //variable value type and name
                    varInfo.valueType = "Field Observation";
                    varInfo.variableName = "Discharge";
                    break;
                case 8:
                    //snow
                    varInfo.dataType = "Average";
                    varInfo.generalCategory = "Climate";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Snow";
                    varInfo.speciation = "Not Applicable";

                    //variable unit
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "cm";
                    varInfo.unit.unitCode = "47";
                    varInfo.unit.unitDescription = "centimeter";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "centimeter";
                    varInfo.unit.unitType = "Length";

                    //variable value type and name
                    varInfo.valueType = "Field Observation";
                    varInfo.variableName = "Snow depth";
                    break;
                case 16:
                    //temperature
                    varInfo.dataType = "Average";
                    varInfo.generalCategory = "Climate";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Air";
                    varInfo.speciation = "Not Applicable";

                    //variable unit
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "degC";
                    varInfo.unit.unitCode = "96";
                    varInfo.unit.unitDescription = "degree celsius";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "degree celsius";
                    varInfo.unit.unitType = "Temperature";

                    //variable code
                    varInfo.valueType = "Field Observation";
                    varInfo.variableName = "Temperature";
                    break;
                case 17:
                    //temperature min
                    varInfo.dataType = "Minimum";
                    varInfo.generalCategory = "Climate";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Air";
                    varInfo.speciation = "Not Applicable";

                    //variable unit
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "degC";
                    varInfo.unit.unitCode = "96";
                    varInfo.unit.unitDescription = "degree celsius";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "degree celsius";
                    varInfo.unit.unitType = "Temperature";

                    //variable code
                    varInfo.valueType = "Field Observation";
                    varInfo.variableName = "Temperature";
                    break;
                case 18:
                    //temperature
                    varInfo.dataType = "Maximum";
                    varInfo.generalCategory = "Climate";
                    varInfo.metadataTimeSpecified = false;
                    varInfo.noDataValue = -9999.0;
                    varInfo.noDataValueSpecified = true;
                    varInfo.sampleMedium = "Air";
                    varInfo.speciation = "Not Applicable";

                    //variable unit
                    varInfo.unit = new UnitsType();
                    varInfo.unit.unitAbbreviation = "degC";
                    varInfo.unit.unitCode = "96";
                    varInfo.unit.unitDescription = "degree celsius";
                    varInfo.unit.unitID = 1;
                    varInfo.unit.unitIDSpecified = true;
                    varInfo.unit.unitName = "degree celsius";
                    varInfo.unit.unitType = "Temperature";

                    //variable code
                    varInfo.valueType = "Field Observation";
                    varInfo.variableName = "Temperature";
                    break;
            }
        }

        internal static VariableInfoType GetVariableInfoFromDb(string VariableParameter)
        {
            VariableInfoType varInfo = new VariableInfoType();
            int var_id = VariableCodeToID(VariableParameter);
            SetVariableProperties(var_id, varInfo);

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

                List<seriesCatalogTypeSeries> seriesCatalogList = new List<seriesCatalogTypeSeries>();
                
                for (int i = 0; i < numVariables; i++)
                {
                    seriesCatalogTypeSeries cat = GetSeriesCatalogFromDb(Convert.ToInt32(siteId), variableIdList[i]);
                    if (cat != null)
                    {
                        seriesCatalogList.Add(cat);
                    }
                }
 
               newSite.seriesCatalog[0].series = seriesCatalogList.ToArray();
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
                    string sql = "SELECT var_id FROM plaveninycz.StationsVariables WHERE st_id=" + siteId + "AND var_id in (1, 4, 5, 8, 16)";

                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            int varId = Convert.ToInt32(dr["var_id"]);
                            if (!variableIdList.Contains(varId))
                                variableIdList.Add(varId);
                            if (varId == 16) //if temperature is measured also add tmin and tmax
                            {
                                variableIdList.Add(17);
                                variableIdList.Add(18);
                            }
                        }
                    }
                }
            }
            return variableIdList;
        }

        internal static MethodType GetMethodForVariable(int variableId) 
        {
            MethodType m = new MethodType();
            switch (variableId)
            {
                case 8:
                    m.methodCode = "1";
                    m.methodID = 1;
                    m.methodDescription = "Snow measured at 6:00Z on open ground";
                    m.methodLink = "hydro.chmi.cz/hpps";
                    break;
                case 1:
                    m.methodCode = "2";
                    m.methodID = 2;
                    m.methodDescription = "Precipitation measured by tipping-bucket raingauge and aggregated to daily";
                    m.methodLink = "hydro.chmi.cz/hpps";
                    break;
                case 4:
                    m.methodCode = "5";
                    m.methodID = 5;
                    m.methodDescription = "Water level measured by pressure sensor";
                    m.methodLink = "hydro.chmi.cz/hpps";
                    break;
                case 5:
                    m.methodCode = "4";
                    m.methodID = 4;
                    m.methodDescription = "Water level measured by pressure sensor. Discharge computed from water level using rating curve";
                    m.methodLink = "hydro.chmi.cz/hpps";
                    break;
                default:
                    m.methodCode = "3";
                    m.methodID = 3;
                    m.methodDescription = "Temperature sensor in 2 meters on automated meteorological station";
                    m.methodLink = "hydro.chmi.cz/hpps";
                    break;
            }
            return m;
        }

        /// <summary>
        /// Get the values, from the Db
        /// </summary>
        /// <param name="siteId">site id (local database id)</param>
        /// <param name="variableId">variable id (local database id)</param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        internal static TsValuesSingleVariableType GetValuesFromDb(string siteId, string variableCode, DateTime startDateTime, DateTime endDateTime)
        {
            //numeric variable id
            int varId = VariableCodeToID(variableCode);
            
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
            s.method[0] = GetMethodForVariable(varId);
            string timeStep = "day";

            //time units
            s.units = new UnitsType();
            s.units.unitAbbreviation = "d";
            s.units.unitCode = "104";
            s.units.unitID = 104;
            s.units.unitName = "day";
            s.units.unitType = "Time";
            timeStep = "day";

            //method
            //TODO: re-factor into a new file
            s.method[0] = GetMethodForVariable(varId);

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

            string variableFolder = "prutok";
            switch (varId)
            {
                case 1:
                    variableFolder = "srazky";
                    break;
                case 2:
                    variableFolder = "srazky";
                    break;
                case 4:
                    variableFolder = "vodstav";
                    break;
                case 5:
                    variableFolder = "prutok";
                    break;
                case 8:
                    variableFolder = "snih";
                    break;
                case 16:
                    variableFolder = "teplota";
                    break;
                case 17:
                    variableFolder = "tmin";
                    break;
                case 18:
                    variableFolder = "tmax";
                    break;
            }

            //special case for SNOW: read values from database
            if (varId == 8)
            {
                //values: get from database...
                string connStr = GetConnectionString();
                List<ValueSingleVariable> valuesList = new List<ValueSingleVariable>();
                using (SqlConnection cnn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("plaveninycz.new_query_observations", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
                        cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.SmallInt));
                        cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
                        cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
                        cmd.Parameters.Add(new SqlParameter("@time_step", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@group_function", SqlDbType.VarChar));

                        cmd.Parameters["@st_id"].Value = Convert.ToInt32(siteId);
                        cmd.Parameters["@var_id"].Value = varId;
                        cmd.Parameters["@start_time"].Value = startDateTime;
                        cmd.Parameters["@end_time"].Value = endDateTime;
                        cmd.Parameters["@time_step"].Value = timeStep;
                        cmd.Parameters["@group_function"].Value = "sum";

                        cnn.Open();

                        SqlDataReader r = cmd.ExecuteReader();
                        int obsTimeIndex = r.GetOrdinal("obs_time");
                        int obsValueIndex = r.GetOrdinal("obs_value");
                        while (r.Read())
                        {
                            ValueSingleVariable v = new ValueSingleVariable();
                            v.censorCode = "nc";
                            v.dateTime = Convert.ToDateTime(r["obs_time"]);
                            v.dateTimeUTC = v.dateTime.AddHours(-1);
                            v.dateTimeUTCSpecified = true;
                            v.methodCode = s.method[0].methodCode;
                            v.methodID = v.methodCode;
                            v.offsetValueSpecified = false;
                            v.qualityControlLevelCode = "1";
                            v.sourceCode = "1";
                            v.sourceID = "1";
                            v.timeOffset = "01:00";
                            v.Value = convertValue(r["obs_value"], varId);
                            valuesList.Add(v);
                        }
                    }
                }

                //convert list to array - for precip, snow, discharge, stage
                DateTime beginDate = valuesList[0].dateTime;
                DateTime endDate = valuesList[valuesList.Count - 1].dateTime;
                int numDays = endDate.Subtract(beginDate).Days;
                ValueSingleVariable[] valuesArray = new ValueSingleVariable[numDays];
                int valueIndex = 0;

                DateTime curDate = beginDate;
                foreach (ValueSingleVariable val in valuesList)
                {
                    if (valueIndex >= valuesArray.Length) break;

                    while (curDate < val.dateTime)
                    {
                        valuesArray[valueIndex] = CreateNoDataValue(curDate, s, varId);
                        curDate = curDate.AddDays(1);
                        valueIndex++;
                    }

                    if (valueIndex >= valuesArray.Length) break;
                    valuesArray[valueIndex] = val;
                    curDate = val.dateTime.AddDays(1);
                    valueIndex++;
                }
                s.value = valuesArray;
            }
            else
            {
                //values: get from binary file ...
                string binFileName = BinaryFileHelper.GetBinaryFileName(Convert.ToInt32(siteId), variableFolder, "d");
                BinaryFileData dataValues = BinaryFileHelper.ReadBinaryFileDaily(binFileName, startDateTime, endDateTime, true);

                List<ValueSingleVariable> valuesList = new List<ValueSingleVariable>();
                int N = dataValues.Data.Length;
                DateTime startValueDate = dataValues.BeginDateTime;
                for (int i = 0; i < N; i++)
                {
                    ValueSingleVariable v = new ValueSingleVariable();
                    v.censorCode = "nc";
                    v.dateTime = startValueDate.AddDays(i);
                    v.dateTimeUTC = v.dateTime.AddHours(-1);
                    v.dateTimeUTCSpecified = true;
                    v.methodCode = s.method[0].methodCode;
                    v.methodID = v.methodCode;
                    v.offsetValueSpecified = false;
                    v.qualityControlLevelCode = "1";
                    v.sourceCode = "1";
                    v.sourceID = "1";
                    v.timeOffset = "01:00";
                    v.Value = convertValue(dataValues.Data[i], varId);
                    valuesList.Add(v);
                }

                s.value = valuesList.ToArray();
            }
            return s;
        }

        private static Decimal convertValue(object val, int varId)
        {
            var dVal = Convert.ToDouble(val);
            switch (varId)
            {
                case 1:
                    // precipitation - no data value is now displayed as zero
                    return Convert.ToDecimal(Math.Round(dVal, 1));
                case 4:
                    // water stag
                    return Convert.ToDecimal(Math.Round(dVal, 4));
                case 5:
                    // discharge
                    return Convert.ToDecimal(Math.Round(dVal, 4));
                case 8:
                    // snow
                    return Convert.ToDecimal(Math.Round(dVal, 1));
                case 16:
                case 17:
                case 18:
                    // air temperature
                    return Convert.ToDecimal(Math.Round(dVal, 1));
                default:
                    return Convert.ToDecimal(Math.Round(dVal, 4));
            }
        }

        private static ValueSingleVariable CreateNoDataValue(DateTime time, TsValuesSingleVariableType s, int variableId)
        {
            ValueSingleVariable v = new ValueSingleVariable();
            v.censorCode = "nc";
            v.dateTime = Convert.ToDateTime(time);
            v.dateTimeUTC = v.dateTime.AddHours(-1);
            v.dateTimeUTCSpecified = true;
            v.methodCode = s.method[0].methodCode;
            v.methodID = v.methodCode;
            v.offsetValueSpecified = false;
            v.qualityControlLevelCode = "1";
            v.sourceCode = "1";
            v.sourceID = "1";
            v.timeOffset = "01:00";

            switch (variableId)
            {
                case 1:
                    //for precipitation, set 'no data' to zero
                    v.Value = 0.0M;
                    break;
                case 4:
                    v.Value = -9999.0M;
                    break;
                case 5:
                    v.Value = -9999.0M;
                    break;
                case 8:
                    v.Value = 0.0M;
                    break;
                case 16:
                    v.Value = -9999.0M;
                    break;
                default:
                    v.Value = -9999.0M;
                    break;
            }
            return v;
        }
    }
}
