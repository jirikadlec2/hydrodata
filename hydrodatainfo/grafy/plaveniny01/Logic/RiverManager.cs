using System;
using System.Data;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;

namespace jk.plaveninycz.Bll
{
    /// <summary>
    /// This class manages the river objects
    /// </summary>
    public class RiverManager
    {
        #region Public_methods

        /// <summary>
        /// Loads the river with specified Id
        /// </summary>
        public static River GetItemById(long riverId, bool GetRecipient)
        {
            River riv = null;
            DataTable tbl = RiverDS.GetById(riverId);
            if (tbl.Rows.Count > 0)
            {
                riv = DataRowToRiver(tbl.Rows[0]);
                if (GetRecipient == true)
                {
                    riv.Recipient = GetItemById(riv.Id, false);
                }
            }
            return riv;
        }

        /// <summary>
        /// Creates a new river object, from the data store given the
        /// river url (there should be an unique url)
        /// </summary>
        public static River GetItemByUrl(string url, bool getRecipient)
        {
            River riv = null;
            DataTable tbl = RiverDS.GetByUrl(url);
            if (tbl.Rows.Count > 0)
            {
                riv = DataRowToRiver(tbl.Rows[0]);
                if (getRecipient)
                {
                    riv.Recipient = GetItemById(riv.Id, false);
                }
            }
            return riv;
        }

        /// <summary>
        /// creates a new river object corresponding to the specified
        /// station
        /// </summary>
        public static River GetItemByStation(int stationId, bool getRecipient)
        {
            River riv = null;
            DataTable tbl = RiverDS.GetByStationId(stationId);
            if (tbl.Rows.Count > 0)
            {
                riv = DataRowToRiver(tbl.Rows[0]);
                if (getRecipient)
                {
                    riv.Recipient = GetItemById(riv.Id, false);
                }
            }
            return riv;
        }

        /// <summary>
        /// Selects a list of all rivers which are tributaries of
        /// the specified recipient
        /// </summary>
        public static RiverList GetListByRecipient(int recipId)
        {
            return TableToList(RiverDS.GetByRecipientId(recipId));
        }

        /// <summary>
        /// Select a list of all rivers which match the specified name
        /// </summary>
        public static RiverList GetListByName(string riverName)
        {
            return TableToList(RiverDS.GetByName(riverName));
        }

        /// <summary>
        /// Select a complete list of rivers associated
        /// with any of the stations
        /// </summary>
        public static RiverList GetCompleteList()
        {
            return TableToList(RiverDS.GetAll());
        }

        #endregion


        #region private_methods

        private static River DataRowToRiver(DataRow r)
        {
            return new River(Convert.ToInt64(r["riv_id"]),
                    Convert.ToString(r["riv_name"]),
                    Convert.ToString(r["riv_url"]),
                    Convert.ToInt64(r["recip_id"]));
        }

        private static RiverList TableToList(DataTable tbl)
        {
            RiverList list = new RiverList();
            foreach (DataRow r in tbl.Rows)
            {
                list.Add(DataRowToRiver(r));
            }
            return list;
        }

        #endregion
    }
}