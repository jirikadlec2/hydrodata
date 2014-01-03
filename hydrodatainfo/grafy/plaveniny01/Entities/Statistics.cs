using System;
using System.Collections.Generic;
using System.Text;
using jk.plaveninycz.Interfaces;
using ZedGraph;

namespace jk.plaveninycz.BO
{
    public class Statistics
    {
        #region Private Fields
        private double _max;
        private double _min;
        private DateTime _timeOfMax;
        private DateTime _timeOfMin;
        private double _sum;
        private int _measurableCount;
        private double _percentAvailable;
        #endregion

        #region Constructor
        public Statistics(double max, DateTime timeOfMax, double min, DateTime timeOfMin,
            double sum, int count, int percentAvailable)
        {
            _max = max;
            _min = min;
            _timeOfMax = timeOfMax;
            _timeOfMin = timeOfMin;
            _sum = sum;
            _measurableCount = count;
            _percentAvailable = percentAvailable;
        }

        /// <summary>
        /// special calculation of statistics for precipitation data..
        /// </summary>
        public static Statistics FromPrecipTimeSeries(ITimeSeries ts)
        {
            double maxVal = 0;
            double xtimeOfMax = -1;
            DateTime timeOfMax = DateTime.MaxValue;
            double sum = 0;
            int count = 0;
            int missingCount = 0;

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

                    if (pt.Y > 0)
                    {
                        sum += pt.Y;
                        count += 1;
                    }
                }
                else
                {
                    ++missingCount;
                }
            }
            timeOfMax = XDate.XLDateToDateTime(xtimeOfMax);

            return new Statistics(maxVal, timeOfMax, 0, ts.Start, sum, count, -1);
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

            for (int i=0; i< ts.Count; ++i)
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
            stat._percentAvailable = p.PercentAvailableData;
            return stat;
        }

        #endregion

        #region Properties
        public double Max
        {
            get { return _max; }
        }

        public double Min
        {
            get { return _min; }
        }

        public DateTime TimeOfMax
        {
            get { return _timeOfMax; }
        }

        public DateTime TimeOfMin
        {
            get { return _timeOfMin; }
        }

        public double Sum
        {
            get { return _sum; }
        }

        public int MeasurableDataCount
        {
            get { return _measurableCount; }
        }

        public double PercentAvailableData
        {
            get { return _percentAvailable; }
            set { _percentAvailable = value; }
        }

        #endregion
    }
}
