using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace grafy
{
    public class VariableInfo
    {
        public string VariableCode { get; set; }
        public string UnitsName { get; set; }
    }
    
    public class VariableUtil
    {
        public static string VariableIdToCode(int varId)
        {
            switch (varId)
            {
                case 1:
                case 2:
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
                    return "TMAX";
                case 18:
                    return "TMIN";
                default:
                    return "NA";
            }
        }       
        
        /// <summary>
        /// Gets the variables
        /// </summary>
        public static List<VariableInfo> GetVariablesFromDb()
        {
            List<VariableInfo> variableList = new List<VariableInfo>();

            string cnn = Helpers.GetConnectionString();
            
            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT var_id, var_units FROM plaveninycz.variables WHERE var_id IN(1,4,5,8,16)";
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        VariableInfo vi = new VariableInfo();

                        int varId = Convert.ToInt32(dr["var_id"]);
                        vi.UnitsName = Convert.ToString(dr["var_units"]);
                        vi.VariableCode = VariableIdToCode(varId);
                        variableList.Add(vi);
                    }
                }
            }
            VariableInfo tmaxvi = new VariableInfo();
            tmaxvi.VariableCode = "TMAX";
            tmaxvi.UnitsName = "°C";
            variableList.Add(tmaxvi);
            VariableInfo tminvi = new VariableInfo();
            tminvi.VariableCode = "TMIN";
            tminvi.UnitsName = "°C";
            variableList.Add(tminvi);
            return variableList;
        }

        public static List<VariableInfo> GetVariablesForSite(string siteCode)
        {
            List<VariableInfo> variableList = new List<VariableInfo>();
            int siteId = 0;
            if (!int.TryParse(siteCode, out siteId))
            {
                return variableList;
            }

            string cnn = Helpers.GetConnectionString();
            bool hasTemperature = false;

            using (SqlConnection conn = new SqlConnection(cnn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT v.var_id, v.var_units FROM plaveninycz.variables v " +
                        "INNER JOIN plaveninycz.stationsvariables stv ON v.var_id = stv.var_id " +
                        string.Format("WHERE stv.st_id = {0} and v.var_id IN(1,4,5,8,16)", siteId);
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        VariableInfo vi = new VariableInfo();

                        int varId = Convert.ToInt32(dr["var_id"]);
                        vi.UnitsName = Convert.ToString(dr["var_units"]);
                        vi.VariableCode = VariableIdToCode(varId);
                        if (varId == 16)
                        {
                            hasTemperature = true;
                        }
                        variableList.Add(vi);
                    }
                }
            }
            if (hasTemperature)
            {
                VariableInfo tmaxvi = new VariableInfo();
                tmaxvi.VariableCode = "TMAX";
                tmaxvi.UnitsName = "°C";
                variableList.Add(tmaxvi);
                VariableInfo tminvi = new VariableInfo();
                tminvi.VariableCode = "TMIN";
                tminvi.UnitsName = "°C";
                variableList.Add(tminvi);
            }
            return variableList;
        }
    }
}