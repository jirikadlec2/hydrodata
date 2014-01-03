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
                TimeSeriesDS.LoadObservations(ch.StationId, ch.VariableId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Discharge)
            {
                ts = new HydroTimeSeries(interval.Start, interval.End);
                TimeSeriesDS.LoadObservationsDischarge(ch.StationId, ch.VariableId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Temperature)
            {
                ts = new HydroTimeSeries(interval.Start, interval.End);
                TimeSeriesDS.LoadObservationsTemperature(ch.StationId, ch.VariableId, interval.Start, interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.PrecipHour || v == VariableEnum.Precip || v == VariableEnum.PrecipSum)
            {
                PeriodList periods = PeriodManager.GetListByChannelAndTime(ch, interval.Start, interval.End);
                ts = new MyTimeSeries(periods, step);
                TimeSeriesDS.LoadObservationsPrecip(ch.StationId, ch.VariableId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else
            {
                PeriodList periods = PeriodManager.GetListByChannelAndTime(ch, interval.Start, interval.End);
                ts = new MyTimeSeries(periods, step);
                TimeSeriesDS.LoadObservations(ch.StationId, ch.VariableId, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            
            return ts;
        }

        /// <summary>
        /// Returns a time series corresponding to the specific channel and interval
        /// the 'missing data' (periodList) is also internally loaded for all variables
        /// except stage and discharge.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static ITimeSeries GetTimeSeries(Station st, Variable va, TimeInterval interval)
        {
            VariableEnum v = va.VarEnum;
            TimeStep step = GetDefaultTimeStep(v, interval);
            ITimeSeries ts;

            if (v == VariableEnum.Stage || v == VariableEnum.Discharge)
            {
                ts = new HydroTimeSeries(interval.Start, interval.End);
            }
            else
            {
                PeriodList periods = PeriodManager.GetListByChannelAndTime(st.Id, va.Id, interval.Start, interval.End);
                //PeriodList periods = PeriodManager.GetListByChannelAndTime(ch, interval.Start, interval.End);
                ts = new MyTimeSeries(periods, step);
            }

            if (v == VariableEnum.Precip || v == VariableEnum.PrecipHour)
            {
                TimeSeriesDS.LoadObservationsPrecip(st.Id, va.Id, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Discharge)
            {
                TimeSeriesDS.LoadObservationsDischarge(st.Id, va.Id, interval.Start,
                   interval.End, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Temperature)
            {
                TimeSeriesDS.LoadObservationsTemperature(st.Id, va.Id, interval.Start,
                   interval.End, step, (IObservationList)ts);
            }
            else
            {
                TimeSeriesDS.LoadObservations(st.Id, va.Id, interval.Start,
                    interval.End, step, (IObservationList)ts);
            }
           
            return ts;
        }

        /// <summary>
        /// Returns a time series corresponding to specific channel and interval.
        /// Use this method in case you have the period collection already available.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="interval"></param>
        /// <param name="periods"></param>
        /// <returns></returns>
        public static ITimeSeries GetTimeSeries(Channel ch, PeriodList periods)
        {
            VariableEnum v = ch.Variable.VarEnum;
            TimeStep step = GetDefaultTimeStep(v, new TimeInterval(periods.StartTime, periods.Endtime));
            ITimeSeries ts;
            if (v == VariableEnum.Stage || v == VariableEnum.Discharge)
            {
                ts = new HydroTimeSeries(periods);
            }
            else if (v == VariableEnum.Snow)
            {
                ts = new MeteoTimeSeries(periods, step);
            }
            else
            {
                ts = new MyTimeSeries(periods, step);
            }


            if (v == VariableEnum.Precip || v == VariableEnum.PrecipHour)
            {
                TimeSeriesDS.LoadObservationsPrecip(ch.StationId, ch.VariableId, periods.StartTime,
                    periods.Endtime, step, (IObservationList)ts);
            }
            else if (v == VariableEnum.Discharge)
            {
                TimeSeriesDS.LoadObservationsDischarge(ch.StationId, ch.VariableId, periods.StartTime,
                   periods.Endtime, step, (IObservationList)ts);
            }
            else
            {
                TimeSeriesDS.LoadObservations(ch.StationId, ch.VariableId, periods.StartTime,
                   periods.Endtime, step, (IObservationList)ts);
            }

            return ts;
        }

        public static ITimeSeries GetTimeSeries(int stationID, int variableID, PeriodList periods)
        {
            Variable v = VariableManager.GetItemById(variableID);
            
            //VariableEnum v = ch.Variable.VarEnum;
            TimeStep step = GetDefaultTimeStep(v.VarEnum, new TimeInterval(periods.StartTime, periods.Endtime));
            ITimeSeries ts;

            if (variableID == 4 || variableID == 5 || variableID == 6 || variableID == 7)
            {
                ts = new HydroTimeSeries(periods);
            }
            else if (variableID == 8)
            {
                ts = new MeteoTimeSeries(periods, step);
            }
            else
            {
                ts = new MyTimeSeries(periods, step);
            }
            
            if (variableID == 2 || variableID == 1)
            {
                TimeSeriesDS.LoadObservationsPrecip(stationID, variableID, periods.StartTime,
                    periods.Endtime, step, (IObservationList)ts);
            }
            else if (variableID == 5 || variableID == 6 || variableID == 7)
            {
                TimeSeriesDS.LoadObservationsDischarge(stationID, variableID, periods.StartTime,
                   periods.Endtime, step, (IObservationList)ts);
            }
            else
            {
                TimeSeriesDS.LoadObservations(stationID,variableID, periods.StartTime,
                   periods.Endtime, step, (IObservationList)ts);
            }
            return ts;
        }

        #endregion

        #region Private Methods

        //get the default time step
        //TODO: obtain the time step from 'Variable' class instead
        private static TimeStep GetDefaultTimeStep(VariableEnum v, TimeInterval interval)
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
    }
}
