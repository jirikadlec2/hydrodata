using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace api
{
    public class SiteInfo
    {
        public string SiteName { get; set; }
        public int SiteID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public string Operator { get; set; }
        public string StInd { get; set; }
        public string River { get; set; }
    }

    public class DataValueInfo
    {
        public DateTime DateTimeUTC { get; set; }
        public string DataValue { get; set; }
    }

    public class SiteUtil
    {
        public static List<SiteInfo> GetSitesFromDb(string[] varCodes, string operatorCode)
        {
            List<SiteInfo> siteList = new List<SiteInfo>();

            List<int> variablesList = new List<int>();
            string varIdParam = string.Empty;
            for (int i = 0; i < varCodes.Length; i++)
            {
                int varId = DataValuesUtil.VariableCodeToID(varCodes[i]);
                varIdParam += "," + varId;
            }
            varIdParam = varIdParam.Substring(1);
            int numVariables = varCodes.Length;
            if (string.IsNullOrEmpty(varCodes[0]))
            {
                numVariables = 0;
            }
            string cnn = Helpers.GetConnectionString();
            string sql = @"SELECT s.st_id, s.st_name, s.lat, s.lon, s.altitude,
                           s.st_ind AS 'st_id2', r.riv_name AS 'river_name', o.name2 AS 'operator_name' FROM plaveninycz.stations s 
                           LEFT JOIN plaveninycz.operator o ON s.operator_id = o.ID
                           LEFT JOIN plaveninycz.river r on s.riv_id = r.riv_id ";
            if (numVariables == 0)
            {
                sql += "WHERE s.lat IS NOT NULL";
                if (!string.IsNullOrEmpty(operatorCode))
                {
                    sql += string.Format(" AND o.name2 = '{0}'", operatorCode);
                }
            }
            else if (numVariables == 1)
            {
                int varId = DataValuesUtil.VariableCodeToID(varCodes[0]);
                sql += "INNER JOIN plaveninycz.stationsvariables sv ON s.st_id = sv.st_id " + 
                    "WHERE s.lat IS NOT NULL AND sv.var_id = " + varId;
                if (!string.IsNullOrEmpty(operatorCode))
                {
                    sql += string.Format(" AND o.name2 = '{0}'", operatorCode);
                }
            }
            else // (numVariables >= 2)
            {
                string operatorSQL = "";
                if (!string.IsNullOrEmpty(operatorCode))
                {
                    sql += string.Format(" AND o.name2 = '{0}'", operatorCode);
                }
                sql += string.Format(
                    "INNER JOIN plaveninycz.stationsvariables sv ON s.st_id = sv.st_id " + 
                    "WHERE s.lat IS NOT NULL AND sv.var_id IN ({0}) {1} " +
                    "GROUP BY s.st_id, s.st_name, s.lat, s.lon, s.altitude, " +
                    " s.st_ind, r.riv_name, o.name2 " +
                    "HAVING COUNT(*) = {2}",
                    varIdParam, operatorSQL, numVariables);
            }

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        SiteInfo si = new SiteInfo();

                        if (dr["altitude"] != DBNull.Value)
                        {
                            si.Elevation = Convert.ToDouble(dr["altitude"]);
                        }
                        else
                        {
                            si.Elevation = 0;
                        }

                        si.Latitude = Convert.ToDouble(dr["Lat"]);
                        si.Longitude = Convert.ToDouble(dr["lon"]);

                        si.SiteName = Convert.ToString(dr["st_name"]);

                        si.SiteID = Convert.ToInt32(dr["st_id"]);

                        if (dr["operator_name"] != DBNull.Value)
                        {
                            si.Operator = Convert.ToString(dr["operator_name"]);
                        }
                        else
                        {
                            si.Operator = "NA";
                        }

                        if (dr["st_id2"] != DBNull.Value)
                        {
                            si.StInd = Convert.ToString(dr["st_id2"]);
                        }
                        else
                        {
                            si.StInd = "NA";
                        }

                        if (dr["river_name"] != DBNull.Value)
                        {
                            si.River = Convert.ToString(dr["river_name"]);
                        }
                        else
                        {
                            si.River = "NA";
                        }

                        siteList.Add(si);
                    }
                }
            }
            return siteList;
        }
    }
}