using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DroughtAnalysis
{
    /// <summary>
    /// Contains methods for interpolating series
    /// </summary>
    public class DataGapInterpolator
    {
        /// <summary>
        /// Synchronizes the series so that all values have the same DateTime
        /// </summary>
        /// <param name="seriesList">the list of series</param>
        /// <returns>data table with synchronized values</returns>
        public DataTable SynchronizeSeries(IList<CustomDataSeries> seriesList)
        {
            //find common start date
            DateTime startDate = seriesList[0].Values.First().Time;
            DateTime endDate = seriesList[0].Values.Last().Time;

            foreach (CustomDataSeries ds in seriesList)
            {
                if (ds.Values.First().Time > startDate)
                    startDate = ds.Values.First().Time;
                if (ds.Values.Last().Time < endDate)
                    endDate = ds.Values.Last().Time;
            }
            //for each data value populate table column
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Datum", typeof(DateTime)));
            foreach (CustomDataSeries ds in seriesList)
            {
                table.Columns.Add(new DataColumn(ds.SeriesMetadata.Variable.Name, typeof(double)));
            }
            //for each data series get the index
            List<int> firstValueIndexes = new List<int>();
            foreach (CustomDataSeries ds in seriesList)
            {
                for (int i=0; i< ds.Values.Count; i++)
                {
                    if (ds.Values[i].Time >= startDate)
                    {
                        firstValueIndexes.Add(i);
                        break;
                    }
                }
            }

            int numValues = (int)((endDate.Subtract(startDate)).TotalDays) + 1;
            
            //setup empty data table
            for (int i = 0; i < numValues; i++)
            {
                DataRow row = table.NewRow();
                row[0] = startDate.AddDays(i);
                table.Rows.Add(row);
            }
            //for each data value add row
            int columnIndex = 1;
            foreach(CustomDataSeries ds in seriesList)
            {
                int rowIndex = 0;
                foreach(TimeValuePair tp in ds.Values)
                {
                    if (tp.Time >= startDate && tp.Time <= endDate)
                    {
                        table.Rows[rowIndex][columnIndex] = tp.Value;
                        rowIndex++;
                    }
                }
                columnIndex++;
            }

            return table;
        }
        
        /// <summary>
        /// Replaces the 'No Data' values by interpolating 
        /// </summary>
        /// <param name="values">the values</param>
        /// <param name="noDataVal">the noData value to be replaced by linear interpol</param>
        public void ReplaceNoDataValues(IList<TimeValuePair> values, double noDataVal)
        {
            int nextValueIndex = -1;
            int lastValueIndex = -1;
            double lastValue = values[0].Value;
            double nextValue = values[values.Count - 1].Value;
            double slope = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Value == noDataVal)
                {
                    if (nextValueIndex < 0)
                    {
                        lastValueIndex = i - 1;
                        lastValue = (i > 0) ? values[lastValueIndex].Value : 0.0;
                        
                        nextValueIndex = FindNextValueIndex(values, i, noDataVal);
                        nextValue = (nextValueIndex < values.Count) ? values[nextValueIndex].Value : lastValue;
                        slope = (nextValue - lastValue) / (nextValueIndex - lastValueIndex);
                    }
                    //interpolate
                    values[i].Value = lastValue + slope * (i - lastValueIndex);
                }
                else
                {
                    nextValueIndex = -1;
                }
            }
        }

        private int FindNextValueIndex(IList<TimeValuePair> values, int curIndex, double noDataVal)
        {
            int nextValueIndex = curIndex;

            if (nextValueIndex >= values.Count)
                return nextValueIndex;

            double val = values[nextValueIndex].Value;
            while (val == noDataVal)
            {              
                nextValueIndex++;
                if (nextValueIndex >= values.Count)
                    return nextValueIndex;
                val = values[nextValueIndex].Value;
            }
            return nextValueIndex;
        }

        //interpolates the data gaps.
        //works OK with 'nodata' and with actual gaps.
        /// <summary>
        /// Replaces data gaps in irregular time series by no data values
        /// </summary>
        /// <param name="times">times</param>
        /// <param name="values">values</param>
        /// <param name="timeStepDay">time step in days</param>
        /// <param name="noDataVal">no data value</param>
        /// <returns>The interpolated list of time value pairs</returns>
        public IList<TimeValuePair> InterpolateDataGaps(DateTime[] times, double[] values, int timeStepDay, double noDataVal)
        {
            //(1) prepare the times array
            DateTime startDate = times[0].Date;
            DateTime endDate = times[times.Length-1].Date;
            double startValue = values[0];

            int index1 = 0;
            int index2 = 0;

            int numDays = (int)endDate.Subtract(startDate).TotalDays;

            List<TimeValuePair> regularValues = new List<TimeValuePair>(numDays);
            //prepopulate the regular dates
            double oaDate = startDate.ToOADate();
            for (int i = 0; i < numDays; i++)
            {
                regularValues.Add(new TimeValuePair(DateTime.FromOADate(oaDate + i),noDataVal));
            }

            for (int i = 0; i < times.Length; i++)
            {
                if (times[index1].Date == regularValues[index2].Time)
                {
                    regularValues[index2].Value = values[index1];
                    index1++;
                    index2++;
                }

                if (index2 == numDays) break;

                while (regularValues[index2].Time < times[index1].Date)
                {
                    index2++;
                }
            }
            return regularValues;
        }
    }
}
