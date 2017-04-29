using System;
using System.Data;
using System.Globalization;
using jk.plaveninycz;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;

namespace jk.plaveninycz.Bll
{
    public class VariableManager
    {
        #region Public Methods

        public static Variable GetItem(VariableProperty attribute, string value)
        {
            switch ( attribute )
            {
                case VariableProperty.Name:
                    return GetItemByName(value);
                case VariableProperty.ShortName:
                    return GetItemByShortName(value); 
                case VariableProperty.Url:
                    return GetItemByUrl(value);
                default:
                    return null;
            }
        }

        public static Variable GetItemById(int id)
        {
            return new Variable(selectRow("var_id=" + id));
        }

        public static Variable GetItemByEnum(VariableEnum varEnum)
        {
            return new Variable(selectRow("enum='" + varEnum + "'"));
        }

        public static Variable GetItemByShortName(string shortName)
        {
            return new Variable(selectRow("shortname='" + shortName + "'"));
        }

        public static Variable GetItemByName(string name)
        {
            return new Variable(selectRow("name='" + name + "'"));
        }

        public static Variable GetItemByUrl(string url)
        {
            return new Variable(selectRow("url='" + url + "'"));
        }

        public static Variable ChangeCulture(Variable v, CultureInfo cult)
        {
            return new Variable(selectRow(CultureHelper.CultureToShortString(cult), "var_id=" + v.Id));
        }

        public static VariableList GetListByStation(int stationId)
        {
            return makeFromTable(VariableDS.GetVariablesByStation(stationId));
        }

        public static VariableList GetCompleteList()
        {
            return makeFromTable(VariableDS.GetVariableTable());
        }

        #endregion


        #region Private methods

        private static DataRow selectRow(string filter)
        {
            return selectRow(CultureHelper.Default, filter);
        }

        private static DataRow selectRow(string lang, string filter)
        {
            DataRow[] rows = GetTableFromCache().Select("lang_abrev='" + lang + "' AND " + filter);
            if (rows.Length == 0)
            {
                return null;
            }
            else
            {
                return rows[0];
            }
        }

        /// <summary>
        /// Returns the datatable either from cache or from DB store
        /// </summary>
        /// <returns></returns>
        private static DataTable GetTableFromCache()
        {
            DataTable tbl;
            try
            {
                
                
                // check cache
                if (System.Web.HttpContext.Current.Cache["vt"] == null)
                {
                    // get data from store
                    tbl = VariableDS.GetVariableTable();
                    System.Web.HttpContext.Current.Cache.Insert("vt", tbl, null,
                            DateTime.Now.AddMinutes(10), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
                }
                else
                {
                    tbl = (DataTable)System.Web.HttpContext.Current.Cache["vt"];
                }
            }
            catch
            {
                tbl = VariableDS.GetVariableTable();
            }
            //temporary fix..
            tbl = VariableDS.GetVariableTable();
            return tbl;
        }

        private static VariableList makeFromTable(DataTable tbl)
        {
            VariableList list = new VariableList();
            foreach (DataRow r in tbl.Rows)
            {
                list.Add(new Variable(r));
            }
            return list;
        }

        #endregion
    }
}
