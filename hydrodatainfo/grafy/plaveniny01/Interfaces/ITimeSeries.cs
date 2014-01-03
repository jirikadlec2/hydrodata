using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz.Interfaces
{
    /// <summary>
    /// represents a sorted list of
    /// <time, value> pairs with basic functionality
    /// for time series data manipulation.
    /// </summary>
    public interface ITimeSeries : ZedGraph.IPointList
    {
        //double MaxValue { get; }
        //double MinValue { get; }
        //DateTime MaxTime { get; }
        //DateTime MinTime { get; }

        DateTime Start { get; }
        DateTime End { get; }

        /// <summary>
        /// Percent of valid data available
        /// </summary>
        double PercentAvailableData { get; }

        /// <summary>
        /// Create a cumulative time series (integral),
        /// for example cumulative precipitation
        /// </summary>
        ITimeSeries ShowCumulative();
        
        /// <summary>
        /// Return a time series of all data points with
        /// unknown value
        /// </summary>
        ITimeSeries ShowUnknown(double valueToDisplay);
    }
}
