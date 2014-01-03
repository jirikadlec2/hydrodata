using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using jk.plaveninycz.BO;

namespace jk.plaveninycz.DataSources
{
    public class StatisticsDS
    {
        #region Public Methods

        public static Statistics GetStatistics(int stId, int varId, DateTime start, DateTime end)
        {
            double maxVal, minVal, sum;
            int count = 0;
            DateTime timeOfMax, timeOfMin;
            
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "query_statistics";

            cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.TinyInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));

            cmd.Parameters.Add(new SqlParameter("@max_value", SqlDbType.Real));
            cmd.Parameters.Add(new SqlParameter("@min_value", SqlDbType.Real));
            cmd.Parameters.Add(new SqlParameter("@sum", SqlDbType.Real));
            cmd.Parameters.Add(new SqlParameter("@count", SqlDbType.Real));
            cmd.Parameters.Add(new SqlParameter("@max_time", SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@min_time", SqlDbType.SmallDateTime));

            cmd.Parameters["@st_id"].Value = stId;
            cmd.Parameters["@var_id"].Value = varId;
            cmd.Parameters["@start_time"].Value = start;
            cmd.Parameters["@end_time"].Value = end;

            cmd.Parameters["@max_value"].Direction = ParameterDirection.Output;
            cmd.Parameters["@min_value"].Direction = ParameterDirection.Output;
            cmd.Parameters["@sum"].Direction = ParameterDirection.Output;
            cmd.Parameters["@count"].Direction = ParameterDirection.Output;
            cmd.Parameters["@max_time"].Direction = ParameterDirection.Output;
            cmd.Parameters["@min_time"].Direction = ParameterDirection.Output;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                maxVal = Convert.ToDouble(cmd.Parameters["@max_value"].Value);
                minVal = Convert.ToDouble(cmd.Parameters["@min_value"].Value);
                sum = Convert.ToDouble(cmd.Parameters["@sum"].Value);
                count = Convert.ToInt32(cmd.Parameters["@count"].Value);
                timeOfMax = Convert.ToDateTime(cmd.Parameters["@max_time"].Value);
                timeOfMin = Convert.ToDateTime(cmd.Parameters["@min_time"].Value);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return new Statistics(maxVal, timeOfMax, minVal, timeOfMin, sum, count, -1);
        }

        //method for database access to return a data reader of statistics
        //('sum', 'count').
        // the date and value of maximum ('max_time', 'max_value') is returned in the second resultset.
        // the date and value of minimum ('min_time', 'min_value') is optionally returned in the 
        // third dataset.
        public SqlDataReader GetReader(int stId, int varId, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "query_statistics";
            cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.TinyInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
            cmd.Parameters["@st_id"].Value = stId;
            cmd.Parameters["@var_id"].Value = varId;
            cmd.Parameters["@start_time"].Value = startDate;
            cmd.Parameters["@end_time"].Value = endDate;

            
            return (DataUtils.GetReader(cmd, "StatisticsDS"));
        }

        #endregion
    }
}
