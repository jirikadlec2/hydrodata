using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using jk.plaveninycz.Interfaces;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;
using ZedGraph;

namespace jk.plaveninycz.Bll
{
    /// <summary>
    /// Represents the summary statistics of the observation time series
    /// </summary>
    public class StatisticsManager
    {
        #region Public Methods

        /// <summary>
        /// Creates a new statistics object from an existing time series.
        /// VariableEnum v determines which statistics should be calculated.
        /// NOTE: this method will load a PeriodList for some of the variables.
        /// </summary>
        public static Statistics CreateStatistics(Channel ch, ITimeSeries ts)
        {
            VariableEnum v = ch.Variable.VarEnum;

            if (v == VariableEnum.Stage || v == VariableEnum.Discharge)
            {
                return Statistics.FromTimeSeries(ts);
            }
            else
            {
                PeriodList p = PeriodManager.GetListByChannelAndTime(ch, ts.Start, ts.End);
                return CreateStatistics(ch, ts, p);
            }
        }

        /// <summary>
        /// Create the statistics object from existing time series and list of periods
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="v"></param>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static Statistics CreateStatistics(Channel ch, ITimeSeries ts, PeriodList periods)
        {
            Statistics stat;
            VariableEnum v = ch.Variable.VarEnum;

            if (v == VariableEnum.Precip)
            {
                stat = Statistics.FromPrecipTimeSeries(ts);
            }
            else
            {
                stat = Statistics.FromTimeSeries(ts);
            }

            if (!(v == VariableEnum.Stage || v == VariableEnum.Discharge))
            {
                stat.PercentAvailableData = periods.PercentAvailableData;
            }
            return stat;
        }

        /// <summary>
        /// Create a statistics object corresponding to an existing channel.
        /// NOTE: for precipitation, a time series will be internally loaded
        /// before creating the statistics.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static Statistics CreateStatistics(Channel ch, TimeInterval interval)
        {
            VariableEnum v = ch.Variable.VarEnum;

            if (v == VariableEnum.Stage || v == VariableEnum.Discharge)
            {
                Statistics stat = StatisticsDS.GetStatistics(ch.StationId, ch.VariableId,
                        interval.Start, interval.End);
                return stat;
            }
            else
            {
                PeriodList p = PeriodManager.GetListByChannelAndTime(ch, interval.Start, interval.End);
                return CreateStatistics(ch, interval, p);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="interval"></param>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static Statistics CreateStatistics(Channel ch, TimeInterval interval, PeriodList periods)
        {
            VariableEnum v = ch.Variable.VarEnum;

            if (v == VariableEnum.Stage || v == VariableEnum.Discharge)
            {
                Statistics stat = StatisticsDS.GetStatistics(ch.StationId, ch.VariableId,
                        interval.Start, interval.End);
                return stat;
            }
            else
            {
                if (v == VariableEnum.Precip)
                {
                    ITimeSeries ts = TimeSeriesManager.GetTimeSeries(ch, periods);
                    Statistics stat = Statistics.FromTimeSeries(ts);
                    stat.PercentAvailableData = periods.PercentAvailableData;
                    return stat;
                }
                else
                {
                    Statistics stat = StatisticsDS.GetStatistics(ch.StationId, ch.VariableId,
                        interval.Start, interval.End);
                    stat.PercentAvailableData = periods.PercentAvailableData;
                    return stat;
                }
            }
        }

        public static Statistics FromTimeSeries(ITimeSeries ts)
        {
            double maxVal = 0;
            double minVal = double.MaxValue;
            double xtimeOfMax = -1;
            double xtimeOfMin = -1;
            DateTime timeOfMax = DateTime.MaxValue;
            DateTime timeOfMin = DateTime.MinValue;
            double sum = 0;
            int count = 0;

            for (int i = 0; i < ts.Count; ++i)
            {
                PointPair pt = ts[i];
                if (!pt.IsMissing)
                {
                    if (pt.Y > maxVal)
                    {
                        maxVal = pt.Y;
                        xtimeOfMax = pt.X;
                    }
                    if (pt.Y < minVal)
                    {
                        minVal = pt.Y;
                        xtimeOfMin = pt.X;
                    }
                    if (pt.Y > 0)
                    {
                        sum += pt.Y;
                        count += 1;
                    }
                }
            }
            timeOfMax = XDate.XLDateToDateTime(xtimeOfMax);
            timeOfMin = XDate.XLDateToDateTime(xtimeOfMin);

            return new Statistics(maxVal, timeOfMax, minVal, timeOfMin, sum, count, -1);
        }

        public static Statistics FromPeriodList(PeriodList p, ITimeSeries ts)
        {
            Statistics stat = Statistics.FromTimeSeries(ts);
            stat.PercentAvailableData = p.PercentAvailableData;
            return stat;
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
