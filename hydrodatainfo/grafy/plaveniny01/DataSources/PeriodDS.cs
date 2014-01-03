using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using jk.plaveninycz;

namespace jk.plaveninycz.DataSources
{
    /// <summary>
    /// This class contains methods for accessing lists of periods
    /// </summary>
    public class PeriodDS
    {
        public static DataTable GetPeriodTable(int chId)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            string cmdText = "select start_time, end_time from periods where ch_id=" + chId;
            cmd.CommandText = cmdText;
            return DataUtils.GetTable(cmd, "PeriodDS.GetPeriodTable");
        }

        public static DataTable GetPeriodTable(int chId, DateTime start, DateTime end)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "plaveninycz.qry_periods";
            cmd.Parameters.Add(new SqlParameter("@ch_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.DateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
            cmd.Parameters["@ch_id"].Value = chId;
            cmd.Parameters["@start_time"].Value = start;
            cmd.Parameters["@end_time"].Value = end;

            return DataUtils.GetTable(cmd, string.Format("PeriodDS.GetPeriodTable(chId={0})", chId));
        }

        public static DataTable GetPeriodTable(int stId, int varId, DateTime startTime, DateTime endTime)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "plaveninycz.qry_periods";
            cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.TinyInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.DateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
            cmd.Parameters["@st_id"].Value = stId;
            cmd.Parameters["@var_id"].Value = varId;
            cmd.Parameters["@start_time"].Value = startTime;
            cmd.Parameters["@end_time"].Value = endTime;
            return DataUtils.GetTable(cmd, "PeriodDS.GetPeriodTable");
        }

        public static DataTable GetPeriodTable_povodi(int stId, DateTime startTime, DateTime endTime)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "plaveninycz.qry_periods_povodi";
            cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.DateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
            cmd.Parameters["@st_id"].Value = stId;
            cmd.Parameters["@start_time"].Value = startTime;
            cmd.Parameters["@end_time"].Value = endTime;

            return DataUtils.GetTable(cmd, "PeriodDS.GetPeriodTable_povodi");
        }



        public static DataTable GetPeriodTable(int stId, int varId)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "plaveninycz.qry_periods_all";
            cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.TinyInt));
            cmd.Parameters["@st_id"].Value = stId;
            cmd.Parameters["@var_id"].Value = varId;
            return DataUtils.GetTable(cmd, "PeriodDS.GetPeriodTable");
        }
    }

}
