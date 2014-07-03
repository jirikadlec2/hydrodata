using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace grafy
{
    public static class LinearInterpolator
    {
        public static void ReplaceByZero(double[] values, double noDataVal)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == noDataVal)
                {
                    values[i] = 0.0;
                }
            }
        }
        
        /// <summary>
        /// Replaces the 'No Data' values by interpolating 
        /// </summary>
        /// <param name="values">the values</param>
        /// <param name="noDataVal">the noData value to be replaced by linear interpol</param>
        public static void ReplaceNoDataValues(double[] values, double noDataVal)
        {
            int nextValueIndex = -1;
            int lastValueIndex = -1;
            double lastValue = values[0];
            double nextValue = values[values.Length - 1];
            double slope = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == noDataVal)
                {
                    if (nextValueIndex < 0)
                    {
                        lastValueIndex = i - 1;
                        lastValue = (i > 0) ? values[lastValueIndex] : 0.0;

                        nextValueIndex = FindNextValueIndex(values, i, noDataVal);
                        nextValue = (nextValueIndex < values.Length) ? values[nextValueIndex] : lastValue;
                        slope = (nextValue - lastValue) / (nextValueIndex - lastValueIndex);
                    }
                    //interpolate
                    values[i] = lastValue + slope * (i - lastValueIndex);
                }
                else
                {
                    nextValueIndex = -1;
                }
            }
        }

        private static int FindNextValueIndex(double[] values, int curIndex, double noDataVal)
        {
            int nextValueIndex = curIndex;

            if (nextValueIndex >= values.Length)
                return nextValueIndex;

            double val = values[nextValueIndex];
            while (val == noDataVal)
            {
                nextValueIndex++;
                if (nextValueIndex >= values.Length)
                    return nextValueIndex;
                val = values[nextValueIndex];
            }
            return nextValueIndex;
        }
    }
}