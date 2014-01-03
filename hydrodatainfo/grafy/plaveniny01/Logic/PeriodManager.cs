using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;

namespace jk.plaveninycz.Bll
{
    /// <summary>
    /// Manages a collection of periods in an observation
    /// time series
    /// </summary>
    public class PeriodManager
    {
        #region Public Methods

        public static PeriodList GetCompleteList(int channelId)
        {
            DataTable tbl = PeriodDS.GetPeriodTable(channelId);
            int count = 0;
            if ((count = tbl.Rows.Count) > 0)
            {
                DateTime start = Convert.ToDateTime(tbl.Rows[0]["start_time"]);
                DateTime end = Convert.ToDateTime(tbl.Rows[count]["end_time"]);
                PeriodList list = new PeriodList(start, end);
                foreach (DataRow row in tbl.Rows)
                {
                    list.Add(DataRowToTimeInterval(row));
                }
                return list;
            }
            else
            {
                return new PeriodList(TimeInterval.Unknown, TimeInterval.Unknown);
            }
        }

        public static PeriodList GetCompleteList(int stationId, int variableId)
        {
            //DataTable tbl = PeriodDS.GetPeriodTable(channelId);
            DataTable tbl = PeriodDS.GetPeriodTable(stationId, variableId);
            int count = 0;
            if ((count = tbl.Rows.Count) > 0)
            {
                DateTime start = Convert.ToDateTime(tbl.Rows[0]["start_time"]);
                DateTime end = Convert.ToDateTime(tbl.Rows[count]["end_time"]);
                PeriodList list = new PeriodList(start, end);
                foreach (DataRow row in tbl.Rows)
                {
                    list.Add(DataRowToTimeInterval(row));
                }
                return list;
            }
            else
            {
                return new PeriodList(TimeInterval.Unknown, TimeInterval.Unknown);
            }
        }

        /// <summary>
        /// Returns a 'period list' for the given channel and time period
        /// </summary>
        public static PeriodList GetListByChannelAndTime(int stationId, int variableId, DateTime start, DateTime end)
        {
            Station st = StationManager.GetItemById(stationId, false);
            
            //if (st.Operator.ToLower().StartsWith("povod"))
            //{
            //    //for 'povodi' we need a special function..
            //    return GetPeriods_povodi(stationId, variableId, start, end);
            //}
            //else
            //{
                DataTable tbl = PeriodDS.GetPeriodTable(stationId, variableId, start, end);
                PeriodList list = new PeriodList(start, end);
                DateTime curTime = list.StartTime;

                TimeInterval wholeInterval = new TimeInterval(start, end);
                int count = 0;

                foreach (DataRow row in tbl.Rows)
                {
                    list.Add(DataRowToTimeInterval(row));
                }

                // Change the start of first period and 
                // end of last period so that the periods don't
                // overlap the start and end of the whole interval
                if ((count = list.Count) > 0)
                {
                    if (list[0].Start < start)
                    {
                        list[0] = new TimeInterval(start, list[0].End);
                    }

                    if (list[count - 1].End > end)
                    {
                        list[count - 1] = new TimeInterval(list[count - 1].Start, end);
                    }
                }
                return list;
            //}
        }
        
        /// <summary>
        /// Returns a 'period list' for the given channel and time period
        /// </summary>
        public static PeriodList GetListByChannelAndTime(Channel ch, DateTime start, DateTime end)
        {
            //if (ch.Station.Operator.ToLower().StartsWith("povod"))
            //{
            //    //for 'povodi' we need a special function..
            //    return GetPeriods_povodi(ch, start, end);
            //}
            //else
            //{
                DataTable tbl = PeriodDS.GetPeriodTable(ch.StationId, ch.VariableId, start, end);

                //special case-hourly pcp summarized by day
                if (tbl.Rows.Count == 0 && ch.VariableId == 2)
                {
                    tbl = PeriodDS.GetPeriodTable(ch.StationId, 1, start, end);
                }

                PeriodList list = new PeriodList(start, end);
                DateTime curTime = list.StartTime;

                TimeInterval wholeInterval = new TimeInterval(start, end);
                int count = 0;

                foreach (DataRow row in tbl.Rows)
                {
                    list.Add(DataRowToTimeInterval(row));
                }

                // Change the start of first period and 
                // end of last period so that the periods don't
                // overlap the start and end of the whole interval
                if ((count = list.Count) > 0)
                {
                    if (list[0].Start < start)
                    {
                        list[0] = new TimeInterval(start, list[0].End);
                    }

                    if (list[count - 1].End > end)
                    {
                        list[count - 1] = new TimeInterval(list[count - 1].Start, end);
                    }
                }
                return list;
            //}
        }

        private static PeriodList GetPeriods_povodi(Channel ch, DateTime start, DateTime end)
        {
            int stId = ch.StationId;
            int varId = 1;
            DataTable tbl = PeriodDS.GetPeriodTable_povodi(stId, start, end);
            PeriodList list = new PeriodList(start, end);
            TimeInterval interval;
            DateTime missingEnd = new DateTime(2008, 8, 12);

            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    interval = DataRowToTimeInterval(row);

                    if (interval.ContainsTime(missingEnd))
                    {
                        varId = Convert.ToInt32(row["var_id"]);
                        if (varId == 2)
                        {
                            interval = new TimeInterval(interval.Start, missingEnd);
                        }
                        else
                        {
                            interval = new TimeInterval(missingEnd, interval.End);
                        }
                    }
                    list.Add(interval);
                }

                // Change the start of first period and 
                // end of last period so that the periods don't
                // overlap the start and end of the whole interval
                int count = 0;
                if ((count = list.Count) > 0)
                {
                    if (list[0].Start < start)
                    {
                        list[0] = new TimeInterval(start, list[0].End);
                    }

                    if (list[count - 1].End > end)
                    {
                        list[count - 1] = new TimeInterval(list[count - 1].Start, end);
                    }
                }
            }
            return list;
        }

        private static PeriodList GetPeriods_povodi(int stationId, int variableId, DateTime start, DateTime end)
        {
            int stId = stationId;
            int varId = 1;
            DataTable tbl = PeriodDS.GetPeriodTable_povodi(stationId, start, end);
            PeriodList list = new PeriodList(start, end);
            TimeInterval interval;
            DateTime missingEnd = new DateTime(2008, 8, 12);

            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    interval = DataRowToTimeInterval(row);

                    if (interval.ContainsTime(missingEnd))
                    {
                        varId = Convert.ToInt32(row["var_id"]);
                        if (varId == 2)
                        {
                            interval = new TimeInterval(interval.Start, missingEnd);
                        }
                        else
                        {
                            interval = new TimeInterval(missingEnd, interval.End);
                        }
                    }
                    list.Add(interval);
                }

                // Change the start of first period and 
                // end of last period so that the periods don't
                // overlap the start and end of the whole interval
                int count = 0;
                if ((count = list.Count) > 0)
                {
                    if (list[0].Start < start)
                    {
                        list[0] = new TimeInterval(start, list[0].End);
                    }

                    if (list[count - 1].End > end)
                    {
                        list[count - 1] = new TimeInterval(list[count - 1].Start, end);
                    }
                }
            }
            return list;
        }

        #endregion

        #region Private Methods

        private static TimeInterval DataRowToTimeInterval(DataRow r)
        {
            DateTime start = Convert.ToDateTime(r["start_time"]);
            DateTime end = Convert.ToDateTime(r["end_time"]);
            
            return new TimeInterval(start, end);
        }

        #endregion
    }
}
