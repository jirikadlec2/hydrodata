using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace grafy
{
    public class ValueInfo
    {
        public int SiteID;
        public string DataValue;
        public double Lat;
        public double Lon;
        public double Elev;
    }
    
    public static class MapUtil
    {
        public static List<ValueInfo> GetMapFromDb(string varCode, DateTime startDateTime, DateTime endDateTime, string aggregate)
        {
            string connStr = Helpers.GetConnectionString();

            //numeric variable id

            int varId = DataValuesUtil.VariableCodeToID(varCode);

            string tableName = DataValuesUtil.GetTableName(varId);

            string columnName = DataValuesUtil.GetColumnName(varId);

            int numDays = Convert.ToInt32(endDateTime.Subtract(startDateTime).TotalDays);

            //sql query sn.{1}
            string restrict = "";
            if (varId != 16)
            {
                restrict = string.Format(" AND sn.{0} >= 0", columnName);
            }
            string sql = string.Format("select st.st_id, st.lon, st.lat, st.altitude, {0}(sn.{1}) AS 'obs_value' " +
                "FROM plaveninycz.{2} sn INNER JOIN plaveninycz.stations st ON sn.station_id = st.st_id " +
                "WHERE sn.time_utc between '{3}' and '{4}' AND st.altitude > 0 AND " +
                "st.lat > 0 AND st.lon > 0 {5} " +
                "GROUP BY st.st_id, st.lon, st.lat, st.altitude",
                aggregate,
                columnName,
                tableName,
                startDateTime.ToString("yyyy-MM-dd"),
                endDateTime.ToString("yyyy-MM-dd"),
                restrict);

            //values: get from database...
            List<ValueInfo> lst = new List<ValueInfo>();
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.CommandType = CommandType.Text;

                    cnn.Open();

                    SqlDataReader r = cmd.ExecuteReader();

                    DateTime curDate = startDateTime;
                    while (r.Read())
                    {
                        ValueInfo vi = new ValueInfo();
                        vi.DataValue = DataValuesUtil.convertValue(r["obs_value"], varId);
                        vi.Elev = Convert.ToDouble(r["altitude"]);
                        vi.Lat = Convert.ToDouble(r["lat"]);
                        vi.Lon = Convert.ToDouble(r["lon"]);
                        vi.SiteID = Convert.ToInt32(r["st_id"]);
                        lst.Add(vi);
                    }
                }
            }
            return lst;
        }
    }
}