using System;
using System.Data;
using System.Data.SqlClient;

namespace jk.plaveninycz.DataSources
{
    /// <summary>
    /// Data access class for obtaining information about variables from the
    /// pernament data store.
    /// the temporary variables table will be stored in the cache internally.
    /// </summary>
    public class VariableDS
    {
        #region Public methods
        /// <summary>
        /// initializes the variable data table from the database store
        /// </summary>
        public static DataTable GetVariableTable()
        {
            SqlCommand cmd = new SqlCommand("plaveninycz.query_variables");
            cmd.CommandType = CommandType.StoredProcedure;
            return ( DataUtils.GetTable(cmd, "variableDS.GetVariableTable") );
        }

        /// <summary>
        /// loads a data reader with all idS of variables, given the station id
        /// in case of wrong id, the collection will be empty.
        /// </summary>
        public static DataTable GetVariablesByStation(int stationId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT v.var_id, v.shortname, v.var_name as 'name', v.shortname, v.var_units as 'units', v.interval_h, v.scalefactor " +
                "FROM variables v INNER JOIN stationsvariables stv " +
                "ON v.var_id = stv.var_id WHERE stv.st_id = @st_id";
            cmd.Parameters.Add(new SqlParameter("@st_id",stationId));

            return DataUtils.GetTable(cmd, "VariableDS.GetVariablesByStation, station id=" + stationId.ToString());
        }
        #endregion


    }

}

