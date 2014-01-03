using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz.BO
{
    /// <summary>
    /// Represents a list of measurement periods
    /// from a specific measuring channel
    /// </summary>
    public class PeriodList : List<TimeInterval>
    {
        #region Declarations
        private DateTime _start;
        private DateTime _end;
        #endregion

        public PeriodList(DateTime start, DateTime end)
        {
            _start = start;
            _end = end;
        }

        #region Properties

        public DateTime StartTime
        {
            get
            {   
                return _start;
            }            
        }

        public DateTime Endtime
        {
            get
            {
                return _end;
            }
        }

        public DateTime StartOfFirstPeriod
        {
            get
            {
                if (Count > 0)
                {
                    return this[0].Start;
                }
                else
                {
                    return TimeInterval.Unknown;
                }
            }
        }

        public DateTime EndOfLastPeriod
        {
            get
            {
                if (Count > 0)
                {
                    return this[Count - 1].End;
                }
                else
                {
                    return TimeInterval.Unknown;
                }
            }
        }

        public double PercentAvailableData
        {
            get
            {
                TimeSpan length = new TimeSpan(0);
                TimeSpan totalLength = (TimeSpan)(this.Endtime - this.StartTime);
                foreach (TimeInterval ti in this)
                {
                    length = length.Add(ti.Length);
                }
                double perc = length.TotalSeconds / totalLength.TotalSeconds;
                return perc;
            }
        }

        public bool HasObservations
        {
            get
            {
                return (Count > 0);
            }
        }

        #endregion

        #region Public Methods

        public bool HasObservationsInInterval(TimeInterval interval)
        {
            foreach (TimeInterval ti in this)
            {
                if (ti.ContainsPartOfInterval(interval))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get all times with an available observation
        /// </summary>
        public TimeList GetObservationTimes(TimeStep step)
        {
            DateTime start = this.StartTime;
            TimeList list = new TimeList();
            DateTime curTime;
            int timeStepHours = (step == TimeStep.Hour) ? 1 : 24;

            foreach (TimeInterval ti in this)
            {
                if (step == TimeStep.Day)
                {
                    curTime = ti.Start.Date;
                }
                else
                {
                    curTime = ti.Start;
                }

                while (curTime <= ti.End)
                {
                    list.Add(curTime);
                    curTime = curTime.AddHours(timeStepHours);
                }
            }
            return list;
        }

        /// <summary>
        /// Returns a list of all times where the observation is 'missing'
        /// </summary>
        public TimeList GetMissingTimes(TimeStep step)
        {
            DateTime start = this.StartTime;
            DateTime curTime = start;
            TimeList list = new TimeList();
            int timeStepHours = (step == TimeStep.Hour) ? 1 : 24;

            foreach (TimeInterval ti in this)
            {
                // check the interval between two observation periods
                while (curTime < ti.Start)
                {
                    list.Add(curTime);
                    curTime = curTime.AddHours(timeStepHours);
                }

                // skip the period
                if (step == TimeStep.Day)
                {
                    curTime = ti.End.Date.AddDays(1);
                }
                else
                {
                    curTime = ti.End.AddHours(1);
                }
            }

            // check the interval after the end of last period
            while (curTime < this.Endtime)
            {
                list.Add(curTime);
                curTime = curTime.AddHours(timeStepHours);
            }

            return list;
        }

        #endregion
    }
}
