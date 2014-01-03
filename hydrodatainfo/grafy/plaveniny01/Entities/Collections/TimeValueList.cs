using System;
using System.Collections.Generic;
using System.Text;
using jk.plaveninycz;
using ZedGraph;

namespace jk.plaveninycz.BO
{
    /// <summary>
    /// Represents a custom list of <time, value>
    /// observations spaced at regular time step
    /// </summary>
    internal class TimeValueList : ICloneable
    {
        #region Declarations

        private double _start;
        private int _timeStepFactor;      
        private double[] _data;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new regular-spaced list and sets all values to initialValue
        /// </summary>
        /// <param name="period"></param>
        /// <param name="step"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public TimeValueList (TimeInterval period, TimeStep step, double initialValue)
        {
            _timeStepFactor = (step == TimeStep.Hour) ? 24 : 1;
            _start = period.Start.ToOADate();
            
            switch(TimeStep)
            {
                case TimeStep.Hour:
                    _data = new double[(int) period.Length.TotalHours];
                    break;
                case TimeStep.Day:
                    _data = new double[(int) period.Length.TotalDays];
                    break;
                default:
                    throw new ArgumentException("'step' parameter must be 'Hour' or 'Day'");
            }

            if (initialValue != 0.0)
            {
                // initialize array values...
                for (int i = 0; i < _data.Length; ++i)
                {
                    _data[i] = initialValue;
                }
            }
        }

        private TimeValueList()
        { }


        #endregion

        #region Properties

        public TimeStep TimeStep
        {
            get { return _timeStepFactor == 24 ? TimeStep.Hour : TimeStep.Day; }
        }

        public DateTime Start
        {
            get { return XDate.XLDateToDateTime(_start); }
        }

        public DateTime End
        {
            get 
            {
                return GetTime(_data.Length - 1);
            }
        }

        #endregion

        public DateTime GetTime(int index)
        {
            return XDate.XLDateToDateTime(GetXLDate(index));
        }

        public double GetXLDate(int index)
        {
            //the time step is one day
            return _start + (index / _timeStepFactor);
        }

        public int Count
        {
            get { return _data.Length; }
        }

        public double this[int index]
        {
            get 
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;
            }
        }

        public double this[DateTime time]
        {
            get
            {
                int index = FindIndex(time);
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException
                        (string.Format("[time] index must be between {0} and {1}.",
                        Start, End));
                }
                return _data[index];
            }

            set
            {
                int index = FindIndex(time);
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException
                        (string.Format("[time] index must be between {0} and {1}.",
                        Start, End));
                }
                _data[index] = value;
            }
        }

        public int FindIndex(DateTime time)
        {
            double oadate = time.ToOADate();
            double diff = oadate - _start;
            double diff2 = diff * _timeStepFactor;
            int idx = (int)(Math.Round(diff2));
            return idx;
            //return (int)((time.ToOADate() - _start) * _timeStepFactor);
        }

        #region ICloneable Members

        public object Clone()
        {
            TimeValueList copy = new TimeValueList();
            copy._start = this._start;
            copy._timeStepFactor = this._timeStepFactor;
            copy._data = (double[])(_data.Clone());
            return (TimeValueList)copy;
        }

        #endregion
    }
}
