using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace jk.plaveninycz.DataSources
{
    /// <summary>
    /// This class contains methods for accessing detailed
    /// information about rivers from the database
    /// </summary>
    public class RiverDS
    {
        /// <summary>
        /// Select a river by unique Id
        /// </summary>
        public static DataTable GetById(long riverId)
        {
            return getTable(string.Format("riv_id={0}", riverId));
        }

        /// <summary>
        /// Select a river by Url
        /// </summary>
        public static DataTable GetByUrl(string url)
        {
            return getTable(string.Format("riv_url='{0}'", url));
        }

        /// <summary>
        /// Select all rivers with the specified name
        /// </summary>
        public static DataTable GetByName(string riverName)
        {
            return getTable(string.Format("riv_name like '%{0}%'", riverName));
        }

        /// <summary>
        /// Select all rivers which are tributaries of the specified recipient 
        /// </summary>
        public static DataTable GetByRecipientId(long recipientId)
        {
            return getTable(string.Format("recip_id={0}", recipientId));
        }

        public static DataTable GetAll()
        {
            return getTable("1=1");
        }

        /// <summary>
        /// Select a river which flows at the specified station
        /// </summary>
        public static DataTable GetByStationId(int stationId)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("select ri.riv_id, ri.riv_name, ri.riv_url, ri.recip_id ");
            cmdText.Append("from plaveninycz.river ri inner join plaveninycz.stations st ");
            cmdText.Append("on ri.riv_id = st.riv_id ");
            cmdText.Append(string.Format("where st.st_id={0}", stationId));
            cmd.CommandText = cmdText.ToString();
            return DataUtils.GetTable(cmd, "RiverDS.GetByStationId");
        }

        #region private_methods
        
        private static DataTable getTable(string filter)
        {
            SqlCommand cmd = DataUtils.CreateCommand();
            cmd.CommandText = 
                @"select ri.riv_id, ri.riv_name, ri.riv_url, ri.recip_id from plaveninycz.river ri where " + filter;
            return DataUtils.GetTable(cmd, "RiverDS.GetTable");
        }
        #endregion
    }

}