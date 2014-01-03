using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

namespace jk.plaveninycz.DataSources
{

    /// <summary>
    /// This class contains methods for accessing detailed
    /// information about stations from the database
    /// from the database about a given station
    /// </summary>
    public class StationDS
    {
        #region Public Methods

        /// <summary>
        /// Selects a station by id
        /// </summary>
        public static DataTable GetById(int stationId)
        {
            return GetTable(string.Format("st_id={0}", stationId));
        }

        /// <summary>
        /// Gets all stations
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {
            return GetTable();
        }

        public static DataTable GetByChannel(int channelId)
        {
            string filter = string.Format("channel.ch_id={0}");
            return GetTableIncludingChannels(filter);
        }

        /// <summary>
        /// returns a station from the database, given the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static DataTable GetByUrl(string url)
        {
            return GetTable(string.Format("st_uri='{0}'", url));
        }

        /// <summary>
        /// select a station from the database, 
        /// given the url string and variable id. returns empty table when station 
        /// does not exist for the given variable
        /// </summary>
        public static DataTable GetByUrlAndVariable(string url, int variableId)
        {
            string filter = string.Format("st.st_uri='{0}' and stv.var_id={1}",
                url, variableId);
            return GetTableIncludingChannels(filter);          
        }

        /// <summary>
        /// Select all stations which match the specified name. 
        /// returns an empty table when no stations found 
        /// </summary>
        public static DataTable GetByName(string stationName)
        {
            return GetTable(string.Format("st_name Like '%{0}%'", stationName));
        }

        /// <summary>
        /// Select all stations which match the specified name and measure the 
        /// specified variable. returns empty table when no stations found 
        /// </summary>
        public static DataTable GetByNameAndVariable(string stationName, int variableId)
        {
            string filter = string.Format("stv.var_id={0} and st.st_name Like '%{1}%'",
                variableId, stationName);
            return GetTableIncludingChannels(filter); 
        }

        /// <summary>
        /// Select all stations which measure the specified variable. 
        /// Returns an empty table if no stations were found
        /// </summary>
        public static DataTable GetByVariable(int variableId)
        {
            string filter = string.Format("stv.var_id={0}", variableId);
            return GetTableIncludingChannels(filter); 
        }

        /// <summary>
        /// This method returns a list (data reader) of stations which are situated
        /// on the specified river. The stations are ordered downstream.
        /// </summary>
        public static DataTable GetByRiver(long riverId)
        {
            return GetTable(string.Format("riv_id={0}", riverId));
        }

        //TODO: change the station_table aspx pages
        //public IDataReader GetDetailsByVariable(int variableId)
        //{
        //    return GetDetailsByVariable(variableId, 1);
        //}

        public static DataTable GetDetailsByVariable(int variableId, int orderBy)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = "plaveninycz.new_query_stationsbyvariable";
 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.TinyInt));
            cmd.Parameters.Add(new SqlParameter("@order", SqlDbType.TinyInt));
            cmd.Parameters["@var_id"].Value = variableId;
            cmd.Parameters["@order"].Value = orderBy;

            return (DataUtils.GetTable(cmd, "GetDetailsByVariable"));
        }

        #endregion

        #region Private Methods

        public static DataTable GetLocationTable(int locationId)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = "select lat, lon from plaveninycz.locations where loc_id = " + locationId;
            return DataUtils.GetTable(cmd, "StationDS.GetLocation");
        }

        private static DataTable GetTable()
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText =
                "select st.st_id, st.st_name, st.st_uri, st.riv_id, st.location_id, op.name as 'operator_name' " +
                "from plaveninycz.stations st " +
                "inner join plaveninycz.operator op on st.operator_id = op.id ";
                
            return DataUtils.GetTable(cmd, "StationDS.GetTable");
        }

        /// <summary>
        /// Returns a table of stations matching the specified criteria
        /// </summary>
        /// <param name="filter">the filter (SQL where) expression</param>
        private static DataTable GetTable(string filter)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText =
                "select st.st_id, st.st_name, st.st_uri, st.riv_id, st.location_id, op.name as 'operator_name' " +
                " from plaveninycz.stations st " +
                "inner join plaveninycz.operator op on st.operator_id = op.id where " + filter;
            return DataUtils.GetTable(cmd, "StationDS.GetTable");
        }

        private static DataTable GetTableIncludingChannels(string filter)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("select st.st_id, st.st_name, st.st_uri, st.riv_id, st.location_id, op.name as 'operator_name' ");
            cmdText.Append("from plaveninycz.stations st ");
            cmdText.Append("inner join plaveninycz.operator op on st.operator_id = op.id ");
            cmdText.Append("inner join plaveninycz.StationsVariables stv on st.st_id = stv.st_id ");
            cmdText.Append("where ");
            cmdText.Append(filter);

            cmd.CommandText = cmdText.ToString();
            return DataUtils.GetTable(cmd, "StationDS.GetTableIncludingChannels (" + filter + ")");
        }

        private static IDataReader getReader(string filter)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText =
               "select st.st_id, st.st_name, st.st_uri, st.riv_id, st.location_id, op.name as 'operator_name' " +
                "from plaveninycz.stations st " +
                "inner join plaveninycz.operator op on st.operator_id = op.id where " + filter;
            return DataUtils.GetReader(cmd, "StationDS.GetReader (" + filter + ")");
        }

        #endregion
    }

}
