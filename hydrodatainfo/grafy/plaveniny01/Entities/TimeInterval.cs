using System;

namespace jk.plaveninycz.BO
{
    /// <summary>
    /// represents a time interval
    /// with a start and end time
    /// </summary>
    public class TimeInterval
    {
        /// <summary>
        /// Creates a new time interval and
        /// ensures the start and end times are in
        /// correct order.
        /// </summary>
        public TimeInterval(DateTime start, DateTime end)
        {
            if (end < start)
            {
                SwapTimes(ref start, ref end);
            }
            _start = start;
            _end = end;
        }

        #region Private Fields
        private DateTime _start;
        private DateTime _end;
        #endregion

        #region Properties
        public static DateTime Unknown
        {
            get { return DateTime.MaxValue; }
        }

        public DateTime Start
        {
            get { return _start; }
        }
        public DateTime End
        {
            get { return _end; }
        }

        public TimeSpan Length
        {
            get { return End.Subtract(Start); }
        }

        public TimeInterval NextInterval
        {
            get
            {
                return new TimeInterval(End, End.Add(Length));
            }
        }

        public TimeInterval PreviousInterval
        {
            get
            {
                return new TimeInterval(Start.Subtract(Length), Start);
            }
        }

        /// <summary>
        /// Represents a non-existent time interval
        /// </summary>
        public static TimeInterval Missing
        {
            get
            {
                return new TimeInterval(DateTime.MaxValue, DateTime.MaxValue);
            }
        }

        public static bool IsMissing(TimeInterval interval)
        {
            return (interval.Start == DateTime.MaxValue && interval.End == DateTime.MaxValue);
        }

        #endregion

        #region Private Methods
        private void SwapTimes(ref DateTime t1, ref DateTime t2)
        {
            DateTime temp = t1;
            t1 = t2;
            t2 = temp;
        }
        #endregion

        #region Public Methods
        public bool ContainsTime(DateTime t)
        {
            return (t >= Start && t <= End);
        }
        public bool ContainsPartOfInterval(TimeInterval ti)
        {
            return (!(ti.Start > End || ti.End < Start));
        }

        public bool ContainsWholeInterval(TimeInterval ti)
        {
            return (ti.Start >= Start && ti.End <= End);
        }

        public override string ToString()
        {
            return Start.ToString() + "-" + End.ToString();
        }

        //public TimeInterval PreviousInterval
        //{

        //}
        #endregion
    }
}
