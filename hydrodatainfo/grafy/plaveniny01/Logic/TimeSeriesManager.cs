using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using jk.plaveninycz.Interfaces;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;
using ZedGraph;

/// <summary>
/// Manager for time series
/// </summary>
namespace jk.plaveninycz.Bll
{
    public class TimeSeriesManager
    {
        #region Public Methods

        /// <summary>
        /// Returns a time series corresponding to the specific channel and interval
        /// the 'missing data' (periodList) is also internally loaded for all variables
        /// except stage and discharge.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static ITimeSeries GetTimeSeries(Channel ch, TimeInterval interval)
        {
            VariableEnum v = ch.Variable.VarEnum;
            TimeStep step = GetDefaultTimeStep(v, interval);
            ITimeSeries ts;

            if (v == VariableEnum.Stage)
            { 
                ts = new HydroTimeSeries(interval.Start, interval.End);
                TimeSeriesDS.LoadObservationsStage2(ch.StationId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Discharge)
            {
                ts = new HydroTimeSeries(interval.Start, interval.End);
                TimeSeriesDS.LoadObservationsDischarge2(ch.StationId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Temperature)
            {
                ts = new HydroTimeSeries(interval.Start, interval.End);
                //step = TimeStep.Hour;
                TimeSeriesDS.LoadObservationsTemperature2(ch.StationId, interval.Start, interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Snow)
            {
                ts = new HydroTimeSeries(interval.Start, interval.End);
                TimeSeriesDS.LoadObservationsSnow(ch.StationId, ch.VariableId, interval.Start, interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.PrecipHour || v == VariableEnum.Precip || v == VariableEnum.PrecipSum)
            {
                ts = new MyTimeSeries(interval.Start, interval.End, step);
                TimeSeriesDS.LoadObservationsPrecip2(ch.StationId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else
            {
                //snow
                ts = new HydroTimeSeries(interval.Start, interval.End);
                TimeSeriesDS.LoadObservationsSnow(ch.StationId, ch.VariableId, interval.Start, interval.End, step, (IObservationList)ts);
            }
            return ts;
        }

        /// <summary>
        /// Fills the data gaps inside the time-series by setting them to 'no data' vals
        /// </summary>
        /// <param name="ts"></param>
        public static ITimeSeries FillDataGaps(ITimeSeries ts, TimeStep step)
        {
            ITimeSeries myTs = MakeRegularTimeSeries(ts.Start, ts.End, step);
            if (step == TimeStep.Day)
            {
                double tStart = myTs[0].X;
                for (int i = 0; i < ts.Count - 1; i++)
                {
                    double index = (ts[i].X - tStart);
                    int ix = (int)index;
                    myTs[ix].Y = ts[i].Y;
                }
            }
            else
            {
                double tStart = myTs[0].X * 24;
                for (int i = 0; i < ts.Count - 1; i++)
                {
                    double index = (ts[i].X * 24 - tStart);
                    int ix = (int)index;
                    myTs[ix].Y = ts[i].Y;
                }
            }
            return myTs;
        }

        /// <summary>
        /// Makes a regular time-series with all of the data being equal to NO-DATA
        /// </summary>
        /// <param name="start">start time</param>
        /// <param name="end">end time</param>
        /// <param name="step">the time step: TimeStep.Day or TimeStep.Hour</param>
        /// <returns>The time-series object with No-Data at all data points</returns>
        public static ITimeSeries MakeRegularTimeSeries(DateTime start, DateTime end, TimeStep step)
        {
            HydroTimeSeries myTs = new HydroTimeSeries(start, end);
            if (step == TimeStep.Day)
            {
                int nd = (int)end.Subtract(start).TotalDays;
                for (int i = 0; i < nd; i++)
                {
                    DateTime newTime = start.AddDays(i);
                    myTs.AddUnknownValue(newTime);
                }
            }
            else
            {
                int nh = (int)end.Subtract(start).TotalHours;
                for (int i = 0; i < nh; i++)
                {
                    DateTime newTime = start.AddHours(i);
                    myTs.AddUnknownValue(newTime);
                }
            }
            return myTs;
        }

        public static HydroTimeSeries GetMissingValuesHydro(ITimeSeries ts, DateTime start, DateTime end, TimeStep step)
        {
            HydroTimeSeries missingTs = new HydroTimeSeries(start, end);
            List<int> breakIx = GetDataBreaks(ts);

            //TODO missing points before series start
            foreach (int begIndex in breakIx)
            {
                if (begIndex < ts.Count - 1)
                {
                    DateTime begDate = DateTime.FromOADate(ts[begIndex].X);
                    DateTime endDate = DateTime.FromOADate(ts[begIndex + 1].X);
                    if (step == TimeStep.Day)
                    {
                        int nd = (int)endDate.Subtract(begDate).TotalDays;
                        for (int i = 0; i < nd; i++)
                        {
                            DateTime newTime = begDate.AddDays(i);
                            missingTs.AddUnknownValue(newTime);
                        }
                    }
                    else
                    {
                        int nh = (int)endDate.Subtract(begDate).TotalHours;
                        for (int i = 0; i < nh; i++)
                        {
                            DateTime newTime = begDate.AddHours(i);
                            missingTs.AddUnknownValue(newTime);
                        }
                    }
                }
            }
            return missingTs;
        }

        
        /// <summary>
        /// Splits a regular time-series with data gaps into two or more sub-series
        /// The maximum allowed gap size is determined by GetDefaultTimeStep()
        /// Use this function for stage and discharge
        /// </summary>
        /// <param name="ts">The time series</param>
        /// <returns>the list of splitted regular time-series</returns>
        public static List<HydroTimeSeries> SplitTimeSeries(ITimeSeries ts)
        {
            List<HydroTimeSeries> hsList = new List<HydroTimeSeries>();
            List<int> breakIx = GetDataBreaks(ts);

            if (breakIx.Count > 0)
            {
                //for the final segment
                if (breakIx[breakIx.Count - 1] != ts.Count - 1)
                {
                    breakIx.Add(ts.Count - 1);
                }
                
                int begIndex = 0;
                for (int i = 0; i < breakIx.Count; i++)
                {
                    //all data before each break
                    int endIndex = breakIx[i];
                    DateTime begDate = DateTime.FromOADate(ts[begIndex].X);
                    DateTime endDate = DateTime.FromOADate(ts[endIndex].X);
                    HydroTimeSeries newTs = new HydroTimeSeries(begDate, endDate);
                    for (int j = begIndex; j <= endIndex; j++)
                    {
                        newTs.AddObservation(DateTime.FromOADate(ts[j].X), ts[j].Y);
                    }
                    begIndex = endIndex + 1;
                    if (newTs.Count > 0)
                    {
                        hsList.Add(newTs);
                    }
                }
            }
            else
            {
                hsList.Add((HydroTimeSeries)ts);
            }
            return hsList;
        }

        
        //get the default time step
        //TODO: obtain the time step from 'Variable' class instead
        public static TimeStep GetDefaultTimeStep(VariableEnum v, TimeInterval interval)
        {
            int periodLength = (int)interval.Length.TotalDays;
            
            switch (v)
            {
                case VariableEnum.Stage:
                case VariableEnum.Discharge:
                    return (periodLength > 300) ? TimeStep.Day : TimeStep.Hour;
                case VariableEnum.Temperature:
                    return (periodLength > 31) ? TimeStep.Day : TimeStep.Hour;
                case VariableEnum.PrecipHour:
                    return TimeStep.Hour;
                default:
                    return TimeStep.Day;
            }
        }

        #endregion

        private static List<int> GetDataBreaks(ITimeSeries ts)
        {
            double breakTresholdDays = 1.1;
            List<int> breakIx = new List<int>();

            for (int i = 0; i < ts.Count - 1; i++)
            {
                double x1 = ts[i].X;
                double x2 = ts[i + 1].X;
                if (x2 - x1 > breakTresholdDays)
                {
                    breakIx.Add(i);
                }
            }
            return breakIx;
        }

    }
}
