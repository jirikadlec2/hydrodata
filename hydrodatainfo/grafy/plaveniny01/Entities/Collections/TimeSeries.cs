using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using ZedGraph;
using jk.plaveninycz.Interfaces;

namespace jk.plaveninycz.BO
{
    #region MeteoTimeSeries

    /// <summary>
    /// Represents a regular-spaced time-series of meteorologic
    /// (precipitation, snow, ...) data
    /// </summary>
    public class MeteoTimeSeries : ITimeSeries, IObservationList
    {
        #region Declarations

        private TimeValueList _data;
        private double _percentAvailable;

        #endregion

        #region Constructor

        public MeteoTimeSeries(DateTime start, DateTime end, TimeStep step)
        {
            _data = new TimeValueList(new TimeInterval(start, end), step, PointPair.Missing);
        }

        private MeteoTimeSeries()
        { }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private double CalcPercentAvailableData()
        {
            int missingCount = 0;
            for (int i = 0; i < _data.Count; ++i)
            {
                if (_data[i] == PointPair.Missing) ++missingCount;
            }
            return (1.0 - (double)missingCount / (double)Count) * 100.0;
        }

        #endregion



        #region ITimeSeries Members

        public DateTime Start
        {
            get { return _data.Start; }
        }

        public DateTime End
        {
            get { return _data.End; }
        }

        public double PercentAvailableData
        {
            get
            {
                if (_percentAvailable > 0)
                {
                    return _percentAvailable;
                }
                else
                {
                    _percentAvailable = CalcPercentAvailableData();
                    return _percentAvailable;
                }
            }
        }

        public ITimeSeries ShowCumulative()
        {
            MeteoTimeSeries ts2 = (MeteoTimeSeries)this.Clone();

            if (ts2.Count > 0)
            {
                if (ts2._data[0] == PointPair.Missing)
                {
                    ts2._data[0] = 0.0;
                }
            }
            
            for (int i = 1; i < ts2.Count; ++i)
            {
                if (ts2._data[i] != PointPair.Missing)
                {
                    ts2._data[i] = ts2._data[i - 1] + ts2._data[i];
                }
                else
                {
                    ts2._data[i] = ts2._data[i - 1];
                }
            }
            return ts2;
        }

        public ITimeSeries ShowUnknown(double valueToDisplay)
        {
            HydroTimeSeries ts2 = new HydroTimeSeries(_data.Start, _data.End);
            for (int i=0; i < Count; ++i)
            {
                PointPair p = this[i];
                if (p.IsMissing)
                {
                    ts2.AddObservation(XDate.XLDateToDateTime(p.X),valueToDisplay);
                }
            }
            return ts2;
        }

        #endregion

        #region IPointList Members

        public int Count
        {
            get { return _data.Count; }
        }

        public PointPair this[int index]
        {
            get 
            { 
                return new PointPair(_data.GetXLDate(index), _data[index]); 
            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            MeteoTimeSeries copy = new MeteoTimeSeries();
            copy._data = (TimeValueList)_data.Clone();
            return copy;
        }

        #endregion

        #region IObservationList Members

        public void AddObservation(DateTime time, double value)
        {
            _data[time] = value;
        }

        public void AddUnknownValue(DateTime time)
        {
            _data[time] = PointPair.Missing;
        }

        public void Clear()
        { }

        public void Sort()
        { }

        #endregion
    }

    #endregion

    #region HydroTimeSeries

    /// <summary>
    /// Represents an irregular time-series of
    /// hydrologic (stage or discharge) data
    /// </summary>
    public class HydroTimeSeries : ITimeSeries, IObservationList
    {
        #region Declarations

        private PointPairList _data;
        private DateTime _start;
        private DateTime _end;
        private double _percentAvailable;

        #endregion

        #region Constructor

        public HydroTimeSeries(DateTime start, DateTime end)
        {
            _start = start;
            _end = end;
            _data = new PointPairList();
        }

        private HydroTimeSeries()
        { }

        #endregion

        #region Private Methods

        private double CalcPercentAvailableData()
        {
            double missingSpan = 0.0;

            if (!_data.Sorted)
            {
                _data.Sort();
            }

            if (_data.Count > 1)
            {
                //missing data after start
                double diff = _data[0].X - XDate.DateTimeToXLDate(_start);
                if (diff > 1.8)
                {
                    missingSpan += diff;
                }

                for (int i = 1; i < _data.Count; ++i)
                {
                    diff = _data[i].X - _data[i - 1].X;
                    if (diff > 1.5)
                    {
                        missingSpan += diff;
                    }
                }
                diff = XDate.DateTimeToXLDate(_end) - _data[Count - 1].X;
                if (diff > 1.8)
                {
                    missingSpan += diff;
                }
                return 100 * (1.0 - missingSpan / (_end.ToOADate() - _start.ToOADate()));
            }
            else
            {
                return 100.0;
            }                       
        }

        #endregion

        #region ITimeSeries Members

        public DateTime Start
        {
            get { return _start; }
        }

        public DateTime End
        {
            get { return _end; }
        }

        public double PercentAvailableData
        {
            get
            {
                if (_percentAvailable > 0)
                {
                    return _percentAvailable;
                }
                else
                {
                    _percentAvailable = CalcPercentAvailableData();
                    return _percentAvailable;
                }
            }
        }

        public ITimeSeries ShowCumulative()
        {
            HydroTimeSeries ts2 = (HydroTimeSeries)this.Clone();

            if (ts2.Count > 0)
            {
                if (ts2[0].IsMissing)
                {
                    ts2[0].Y = 0.0;
                }
            }

            for (int i = 1; i < ts2.Count; ++i)
            {
                if (! ts2[i].IsMissing)
                {
                    ts2[i].Y = ts2[i - 1].Y + ts2[i].Y;
                }
                else
                {
                    ts2[i].Y = ts2[i - 1].Y;
                }
            }
            return ts2;
        }

        public ITimeSeries ShowUnknown(double valueToDisplay)
        {
            HydroTimeSeries ts2 = new HydroTimeSeries(_start, _end);
            foreach (PointPair p in ts2._data)
            {
                if (p.IsMissing)
                {
                    ts2._data.Add(p.X, valueToDisplay);
                }
            }
            return ts2;
        }

        #endregion

        #region IPointList Members

        public int Count
        {
            get { return _data.Count; }
        }

        public PointPair this[int index]
        {
            get { return _data[index]; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            HydroTimeSeries copy = new HydroTimeSeries();
            copy._start = _start;
            copy._end = _end;
            copy._data = _data.Clone();
            return copy;
        }

        #endregion

        #region IObservationList Members

        public void AddObservation(DateTime time, double value)
        {
            _data.Add(new PointPair(time.ToOADate(), value));
        }

        public void AddUnknownValue(DateTime time)
        {
            _data.Add(new PointPair(time.ToOADate(), PointPair.Missing));
        }

        public void Clear()
        {
            _data.Clear();
        }

        public void Sort()
        {
            if (!_data.Sorted) _data.Sort();
        }

        #endregion
    }

    #endregion


    #region MyTimeSeries

    /// <summary>
    /// Represents a time series with irregular-spaced data
    /// points. Only 'Missing' and non-zero values are stored
    /// in this time series.
    /// </summary>
    public class MyTimeSeries : ITimeSeries, IObservationList
    {
        #region Declarations

        private PointPairList _data;
        private DateTime _start;
        private DateTime _end;
        private TimeStep _step;
        private double _percentAvailable;

        #endregion

        #region Constructor

        public MyTimeSeries(DateTime start, DateTime end, TimeStep step)
        {
            _step = step;
            _start = start;
            _end = end;
            _data = new PointPairList();
        }

        
        private MyTimeSeries()
        { }

        #endregion

        #region Private Methods

        

        /// <summary>
        /// Calculated the time step expressed as fraction of days
        /// (hour --> 1/24, day --> 1)
        private double CalcTimeStepFactor(TimeStep step)
        {
            if (step == TimeStep.Day)
            {
                return 1.0;
            }
            else
            {
                return (1.0 / 24.0);
            }
        }

        private double CalcPercentAvailableData()
        {
            int missingCount = 0;
            for (int i = 0; i < _data.Count; ++i)
            {
                if (_data[i].IsMissing) ++missingCount;
            }
            double factor = 1.0;
            if (_step == TimeStep.Hour)
            {
                factor = 24.0;
            }
            double span = (_end.ToOADate() - _start.ToOADate()) * factor;

            return 100.0 * (1.0 - (double)missingCount / span);

        }

        #endregion

        #region ITimeSeries Members

        public DateTime Start
        {
            get { return _start; }
        }

        public DateTime End
        {
            get { return _end; }
        }

        public double PercentAvailableData
        {
            get
            {
                if (_percentAvailable > 0)
                {
                    return _percentAvailable;
                }
                else
                {
                    _percentAvailable = CalcPercentAvailableData();
                    return _percentAvailable;
                }
            }
        }

        public ITimeSeries ShowCumulative()
        {
            MyTimeSeries ts2 = new MyTimeSeries(_start, _end, _step);
            double last_x = _start.ToOADate();
            double sum = 0.0;
            //the time step in fractions of day
            double timeStepFactor = CalcTimeStepFactor(_step);

            //approximate time step to ensure correct comparison results
            double AproxFactor = timeStepFactor + 0.01; 
            
            if (!_data.Sorted)
            {
                _data.Sort();
            }

            foreach (PointPair pt in _data)
            {
                if (!pt.IsMissing)
                {
                    if (pt.X - last_x > AproxFactor)
                    {
                        //the two points are more than one time step apart
                        ts2._data.Add(new PointPair(pt.X - timeStepFactor, sum));
                    }
                    sum += pt.Y;
                    ts2._data.Add(new PointPair(pt.X, sum));
                }
            }

            //add last point..
            ts2._data.Add(new PointPair(End.ToOADate(), sum));

            return ts2;
        }

        public ITimeSeries ShowUnknown(double valueToDisplay)
        {
            MyTimeSeries ts2 = new MyTimeSeries(_start, _end, _step);
            foreach (PointPair p in _data)
            {
                if (p.IsMissing)
                {
                    ts2._data.Add(p.X, valueToDisplay);
                }
            }
            return ts2;
        }

        #endregion

        #region IPointList Members

        public int Count
        {
            get { return _data.Count; }
        }

        public PointPair this[int index]
        {
            get { return _data[index]; }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            MyTimeSeries copy = new MyTimeSeries();
            copy._start = _start;
            copy._end = _end;
            copy._data = _data.Clone();
            return copy;
        }

        #endregion

        #region IObservationList Members

        public void AddObservation(DateTime time, double value)
        {
            _data.Add(new PointPair(time.ToOADate(), value));
        }

        public void AddUnknownValue(DateTime time)
        {
            _data.Add(new PointPair(time.ToOADate(), PointPair.Missing));
        }

        public void Clear()
        {
            _data.Clear();
        }

        public void Sort()
        {
            if (!_data.Sorted) _data.Sort();
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// Represents helper functions for time series manipulation
    /// </summary>
    public class TimeSeriesHelper
    {
        public static ITimeSeries MakeCumulative(ITimeSeries ts)
        {
            HydroTimeSeries ts2 = new HydroTimeSeries(ts.Start, ts.End);
            double sum = 0.0;
            PointPair curPt;
            for ( int i = 0; i < ts.Count; ++i )
            {
                curPt = ts[i];
                if ( curPt.IsMissing )
                {
                    ts2.AddUnknownValue(XDate.XLDateToDateTime(curPt.X));
                }
                else
                {
                    if (curPt.Y > 0)
                    {
                        sum += curPt.Y;
                    }
                    ts2.AddObservation(XDate.XLDateToDateTime(curPt.X), sum);
                }
            }
            return ts2;
        }
    }
}
