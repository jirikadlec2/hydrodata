using System;
using System.Data;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;

namespace jk.plaveninycz.Bll
{
    public class StationManager
    {
        #region Public_methods

        /// <summary>
        /// Returns a station with specified id
        /// </summary>
        public static Station GetItemById(int stationId, bool getStationDetails)
        {
            DataTable tbl = StationDS.GetById(stationId);
            return TableToStation(tbl, getStationDetails);
        }

        /// <summary>
        /// Creates a new station object from the data store given the
        /// station url. in case the url is non-unique, the first match
        /// is returned.
        /// </summary>
        public static Station GetItemByUrl(string url, bool getStationDetails)
        {
            DataTable tbl = StationDS.GetByUrl(url);
            return TableToStation(tbl, getStationDetails);
        }

        /// <summary>
        /// Creates a new station object, from the data store given the variable id
        /// and the station url (there should be unique id-url combination)
        /// </summary>
        public static Station GetItemByUrlAndVariable(string url, int variableId, bool getStationDetails)
        {
            DataTable tbl = StationDS.GetByUrlAndVariable(url, variableId);
            return TableToStation(tbl, getStationDetails);
        }

        /// <summary>
        /// Creates a station which has the specified channel
        /// </summary>
        public static Station GetItemByChannel(int channelId, bool getStationDetails)
        {
            DataTable tbl = StationDS.GetByChannel(channelId);
            return TableToStation(tbl, getStationDetails);
        }

        /// <summary>
        /// Creates a complete list of all stations
        /// </summary>
        /// <returns></returns>
        public static StationList GetList()
        {
            return TableToList(StationDS.GetAll());
        }

        /// <summary>
        /// Select a list of all stations which match the given name
        /// </summary>
        public static StationList GetListByName(string stationName)
        {
            return TableToList(StationDS.GetByName(stationName));
        }

        /// <summary>
        /// Select a list of all stations which measure the specified variable
        /// </summary>
        public static StationList GetListByVariable(int variableId)
        {
            return TableToList(StationDS.GetByVariable(variableId));
        }
        /// <summary>
        /// Select a list of all stations which measure the specified variable.
        /// Order this list by the orderBy parameter
        /// </summary>
        /// <param name="variableId">the variable Id</param>
        /// <param name="orderBy">ordering parameter</param>
        /// <returns></returns>
        public static StationDetailList GetDetailListByVariableOrdered(int variableId, int orderBy)
        {
            return TableToDetailList(StationDS.GetDetailsByVariable(variableId, orderBy));
        }

        /// <summary>
        /// Select a list of all stations which match the given name and measure the variable
        /// </summary>
        public static StationList GetListByNameAndVariable(string stationName, int variableId)
        {
            return TableToList(StationDS.GetByNameAndVariable(stationName, variableId));
        }

        /// <summary>
        /// Select a list of all stations which match the specified URL
        /// </summary>
        public static StationList GetListByUrl(string url)
        {
            return TableToList(StationDS.GetByUrl(url));
        }

        /// <summary>
        /// Select a list of all stations which are situated on the
        /// specified river
        /// </summary>
        public static StationList GetListByRiver(int riverId)
        {
            return TableToList(StationDS.GetByRiver(riverId));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a Station object from the first row in the table
        /// </summary>
        /// <param name="tbl">the data table</param>
        /// <param name="getDetails">specify if associated collections shoud be loaded</param>
        private static Station TableToStation(DataTable tbl, bool getDetails)
        {
            Station st = null;
            if (tbl.Rows.Count > 0)
            {
                st = DataRowToStation(tbl.Rows[0]);
                if (getDetails)
                {
                    AddStationDetails(st);
                }
                AddStationLocation(st);
            }
            return st;
        }

        private static Station DataRowToStation(DataRow r)
        {
            int id = Convert.ToInt32(r["st_id"]);
            string name = Convert.ToString(r["st_name"]);
            string url = Convert.ToString(r["st_uri"]);
            string op = Convert.ToString(r["operator_name"]);
            long riverId = -1;
            if (!r.IsNull("riv_id"))
            {
                riverId = Convert.ToInt64(r["riv_id"]);
            }
            int locationId = -1;
            if (!r.IsNull("location_id"))
            {
                locationId = Convert.ToInt32(r["location_id"]);
            }

            Station st = new Station(id, name, url, riverId, op);
            st.LocationId = locationId;
            return st;
        }

        private static StationDetail DataRowToStationDetail(DataRow r)
        {
            StationDetail sd = new StationDetail();
            sd.Name = Convert.ToString(r["station"]);
            sd.ElevationString = Convert.ToString(r["elevation"]);
            sd.Url = Convert.ToString(r["url"]);
            sd.RiverName = Convert.ToString(r["river"]);

            return sd;
        }

        private static StationList TableToList(DataTable tbl)
        {
            StationList list = new StationList();
            foreach (DataRow r in tbl.Rows)
            {
                Station st = DataRowToStation(r);
                AddStationLocation(st);
                list.Add(st);
            }
            return list;
        }

        private static StationDetailList TableToDetailList(DataTable tbl)
        {
            StationDetailList list = new StationDetailList();
            foreach (DataRow r in tbl.Rows)
            {
                StationDetail stDetail = DataRowToStationDetail(r);
                list.Add(stDetail);
            }
            return list;
        }

        private static void AddStationLocation(Station st)
        {
            if (st.LocationId > 0)
            {
                DataTable locTable = StationDS.GetLocationTable((int)st.LocationId);
                if (locTable.Rows.Count > 0)
                {
                    st.Latitude = Convert.ToDouble(locTable.Rows[0][0]);
                    st.Longitude = Convert.ToDouble(locTable.Rows[0][1]);
                }
            }
        }

        /// <summary>
        /// Loads the station, variables and channels info for the station
        /// </summary>
        /// <param name="st"></param>
        private static void AddStationDetails(Station st)
        {
            st.River = RiverManager.GetItemByStation(st.Id, true);
            st.Variables = VariableManager.GetListByStation(st.Id);
            st.Channels = ChannelManager.GetListByStation(st.Id);
        }

        #endregion
    }
}