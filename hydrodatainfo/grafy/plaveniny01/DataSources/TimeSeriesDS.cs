using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using jk.plaveninycz.Interfaces;
using jk.plaveninycz.BO;

namespace jk.plaveninycz.DataSources
{
    /// <summary>
    /// this class is used for retrieving raw observation time series data 
    /// for a given time period from a particular sensor 
    /// </summary>

    public class TimeSeriesDS
    {
        #region Public Methods

        /// <summary>
        /// Retrieves a time series of observations from the database
        /// (only measured, non-zero values are retrieved)
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="variableId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="observations"></param>
        public static void LoadObservations(int stationId, int variableId, DateTime start, DateTime end,
            TimeStep step, IObservationList observations)
        {
            //observations.Clear();
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = "plaveninycz.new_query_observations";
            cmd.CommandType = CommandType.StoredProcedure;
            SetCmdParameters(cmd, stationId, variableId, start, end, step);
            SqlDataReader rdr;
            double val;
            DateTime t;

            try
            {
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(1))
                    {
                        t = Convert.ToDateTime(rdr[0]);
                        val = Convert.ToDouble(rdr[1]);
                        if (val > 0)
                        {
                            observations.AddObservation(t, val);
                        }
                        else
                        {
                            observations.AddUnknownValue(t);
                        }
                    }
                }
                rdr.Close();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }


        /// <summary>
        /// Retrieves a time series of observations from the database
        /// (only measured, non-zero values are retrieved)
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="variableId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="observations"></param>
        public static void LoadObservationsPrecip(int stationId, int variableId, DateTime start, DateTime end,
            TimeStep step, IObservationList observations)
        {
            //observations.Clear();
            SqlCommand cmd = DataUtils.CreateCommand();
            if (step == TimeStep.Hour)
            {
                cmd.CommandText = "plaveninycz.new_query_observations";
                cmd.CommandType = CommandType.StoredProcedure;
                SetCmdParameters(cmd, stationId, variableId, start, end, step);
            }
            else
            {
                cmd.CommandText = "plaveninycz.new_query_precipday";
                cmd.CommandType = CommandType.StoredProcedure;
                SetCmdParameters2(cmd, stationId, start, end);
            }
            SqlDataReader rdr;
            double val;
            DateTime t;

            try
            {
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(1))
                    {
                        t = Convert.ToDateTime(rdr[0]);
                        val = Convert.ToDouble(rdr[1]);
                        if (val > 0)
                        {
                            observations.AddObservation(t, (val / 10.0));
                        }
                        else
                        {
                            observations.AddUnknownValue(t);
                        }
                    }
                }
                rdr.Close();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public static void LoadObservationsTemperature(int stationId, int variableId, DateTime start, DateTime end,
            TimeStep step, IObservationList observations)
        {
            //observations.Clear();
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = "plaveninycz.new_query_observations";
            cmd.CommandType = CommandType.StoredProcedure;
            SetCmdParameters(cmd, stationId, variableId, start, end, step);
            SqlDataReader rdr;
            double val;
            DateTime t;

            try
            {
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(1))
                    {
                        t = Convert.ToDateTime(rdr[0]);
                        val = Convert.ToDouble(rdr[1]) * 0.1;
                        if (val > -9999)
                        {
                            observations.AddObservation(t, val);
                        }
                        else
                        {
                            observations.AddUnknownValue(t);
                        }
                    }
                }
                rdr.Close();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public static void LoadObservationsSnow(int stationId, int variableId, DateTime start, DateTime end,
            TimeStep step, IObservationList observations)
        {
            //observations.Clear();
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = "plaveninycz.new_query_observations";
            cmd.CommandType = CommandType.StoredProcedure;
            SetCmdParameters(cmd, stationId, variableId, start, end, step);
            SqlDataReader rdr;
            double val;
            DateTime t;

            try
            {
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(1))
                    {
                        t = Convert.ToDateTime(rdr[0]);
                        val = Convert.ToDouble(rdr[1]);
                        if (val > -9999)
                        {
                            observations.AddObservation(t, val);
                        }
                        else
                        {
                            observations.AddUnknownValue(t);
                        }
                    }
                }
                rdr.Close();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Retrieves a time series of discharge observations from the database
        /// (only measured, non-zero values are retrieved)
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="variableId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="observations"></param>
        public static void LoadObservationsDischarge(int stationId, int variableId, DateTime start, DateTime end,
            TimeStep step, IObservationList observations)
        {
            //observations.Clear();
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = "plaveninycz.new_query_observations";

            cmd.CommandType = CommandType.StoredProcedure;
            SetCmdParameters(cmd, stationId, variableId, start, end, step);
            SqlDataReader rdr;
            double val;
            DateTime t;

            try
            {
                cmd.Connection.Open();
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(1))
                    {
                        t = Convert.ToDateTime(rdr[0]);
                        val = Convert.ToDouble(rdr[1]);
                        //val = Math.Pow(2.0, (val / 1000.0));
                        if (val > 0)
                        {
                            observations.AddObservation(t, val);
                        }
                        else
                        {
                            observations.AddUnknownValue(t);
                        }
                    }
                }
                rdr.Close();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        #endregion

        #region Private Methods

        private static void SetCmdParameters(SqlCommand cmd, int stId, int varId, DateTime start, DateTime end,
            TimeStep step)
        {
            cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.TinyInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@time_step", SqlDbType.VarChar));

            cmd.Parameters["@st_id"].Value = stId;
            cmd.Parameters["@var_id"].Value = varId;
            cmd.Parameters["@start_time"].Value = start;
            cmd.Parameters["@end_time"].Value = end;

            if (step == TimeStep.Day)
            {
                cmd.Parameters["@time_step"].Value = "day";
            }
            else
            {
                cmd.Parameters["@time_step"].Value = "hour";
            }
        }

        private static void SetCmdParameters2(SqlCommand cmd, int stId, DateTime start, DateTime end)
        {
            cmd.Parameters.Add(new SqlParameter("@station_id", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));

            cmd.Parameters["@station_id"].Value = stId;
            cmd.Parameters["@start_time"].Value = start;
            cmd.Parameters["@end_time"].Value = end;
        }

        #endregion
    }
}
