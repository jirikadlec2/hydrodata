using System;
using System.Collections;
using System.Collections.Generic;
using jk.plaveninycz.Interfaces;

namespace jk.plaveninycz.BO
{
    /// <summary>
    /// Represents a group of different time series
    /// </summary>
    public class TimeSeriesCollection : List<ITimeSeries>
    {
        public TimeSeriesCollection()
        { }
    }
}
