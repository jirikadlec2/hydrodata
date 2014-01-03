using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace jk.plaveninycz.DataSources
{
    /// <summary>
    /// This class contains methods for accessing detailed
    /// information about channels (sensor in a given station
    /// that measures a specific variable) from the database
    /// </summary>
    public class ChannelDS
    {
        public static DataTable GetById(int channelId)
        {
            return GetTable(string.Format("ch_id={0}", channelId));
        }
        
        /// <summary>
        /// Select all channels from a specified station
        /// that measure a given variable
        /// </summary>
        public static DataTable GetByStationAndVariable(int stationId, int variableId)
        {
            return GetTable
                (string.Format("ch.st_id={0} and ch.var_id={1}", stationId, variableId));
        }

        /// <summary>
        /// Select all channels at a specified station
        /// </summary>
        public static DataTable GetByStation(int StationId)
        {
            return GetTable(string.Format("ch.st_id={0}", StationId));
        }

        /// <summary>
        /// Select all channels which measure the specified variable
        /// </summary>
        public static DataTable GetByVariable(int VariableId)
        {
            return GetTable(string.Format("ch.var_id={0}", VariableId));
        }

        public static DataTable GetByStationNameAndVariable(string stName, int variableId)
        {
            return GetTable(string.Format("ch.var_id={0} and st.st_name='{1}'", variableId, stName));
        }

        public static DataTable GetByStationUrlAndVariable(string stUri, int variableId)
        {
            return GetTable(string.Format("ch.var_id={0} and st.st_uri='{1}'", variableId, stUri));
        }

        //TODO: add searching by river, searching by other criteria..

        #region private_methods

        private static void InitializeCommand(SqlCommand cmd, string filter)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("select ch.ch_id, ch.st_id, ch.var_id, ");
            cmdText.Append("st.st_name, st.riv_id, st.st_uri, ");
            cmdText.Append("op.name as 'operator_name' from plaveninycz.StationsVariables ch ");
            cmdText.Append("inner join plaveninycz.stations st on ch.st_id = st.st_id ");
            cmdText.Append("inner join plaveninycz.operator op on st.operator_id = op.id ");
            cmdText.Append("where ");
            cmdText.Append(filter);
            cmd.CommandText = cmdText.ToString();
        }

        //a filter for variable so that we can correctly return 
        //the daily precipitation channel
        private static string GetFilterForVariable(int variableId)
        {
            //if (variableId == 2)
            //{

            //}
            return " ";
        }
        
        private static DataTable GetTable(string filter)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            InitializeCommand(cmd, filter);
            return DataUtils.GetTable(cmd, "ChannelDS.GetTable (" + filter + ")");
        }

        private static IDataReader GetReader(string filter)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            InitializeCommand(cmd, filter);
            return DataUtils.GetReader(cmd, "ChannelDS.GetReader (" + filter + ")");
        }

        #endregion
    }
}
