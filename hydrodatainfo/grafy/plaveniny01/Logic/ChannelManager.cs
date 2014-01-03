using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;

namespace jk.plaveninycz.Bll
{

    /// <summary>
    /// Manages a station / variable combination including
    /// the information about sensor operation period
    /// </summary>
    public class ChannelManager
    {
        #region Public Methods

        public static Channel GetItemById(int channelId)
        {
            return TableToChannel(ChannelDS.GetById(channelId), true);
        }

        public static Channel GetItemByStationAndVariable(int stationId, int variableId)
        {
            return TableToChannel(ChannelDS.GetByStationAndVariable(stationId, variableId), true);
        }

        public static ChannelList GetListByStation(int stationId)
        {
            return TableToList(ChannelDS.GetByStation(stationId));
        }

        public static ChannelList GetListByVariable(int variableId)
        {
            return TableToList(ChannelDS.GetByVariable(variableId));
        }

        public static ChannelList GetListByStationAndVariable(int stationId, int variableId)
        {
            return TableToList(ChannelDS.GetByStationAndVariable(stationId, variableId));
        }
        public static ChannelList GetListByStationUrlAndVariable(string stUrl, int variableId)
        {
            return TableToList(ChannelDS.GetByStationUrlAndVariable(stUrl, variableId));
        }
        public static ChannelList GetListByStationNameAndVariable(string stationName, int variableId)
        {
            return TableToList(ChannelDS.GetByStationNameAndVariable(stationName, variableId));
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Creates a Station object from the first row in the table
        /// </summary>
        /// <param name="tbl">the data table</param>
        /// <param name="getDetails">specify if associated collections shoud be loaded</param>
        private static Channel TableToChannel(DataTable tbl, bool getDetails)
        {
            Channel ch = null;
            if (tbl.Rows.Count > 0)
            {
                ch = DataRowToChannel(tbl.Rows[0]);  
            }
            return ch;
        }

        private static Channel DataRowToChannel(DataRow r)
        {
            int id = 0;
            if (!(r.IsNull("ch_id")))
            {
                id = Convert.ToInt32(r["ch_id"]);
            }
            int stationId = Convert.ToInt32(r["st_id"]);
            int variableId = Convert.ToInt32(r["var_id"]);
            string stName = Convert.ToString(r["st_name"]);
            string stUrl = Convert.ToString(r["st_uri"]);
            long riverId = 0;
            if (!(r.IsNull("riv_id")))
            {
                riverId = Convert.ToInt64(r["riv_id"]);
            }
            string op = Convert.ToString(r["operator_name"]);

            Channel ch = new Channel(id, stationId, variableId, TimeInterval.Missing);
            Station st = new Station(stationId, stName, stUrl, riverId, op);
            Variable v = VariableManager.GetItemById(variableId);

            ch.Station = st;
            ch.Variable = v;

            //also load river for hydrologic stations
            if (riverId > 0)
            {
                st.River = RiverManager.GetItemByStation(st.Id, true);
            }

            return ch;
        }

        private static ChannelList TableToList(DataTable tbl)
        {
            ChannelList list = new ChannelList();
            foreach (DataRow r in tbl.Rows)
            {
                list.Add(DataRowToChannel(r));
            }
            return list;
        }

        #endregion
    }
}