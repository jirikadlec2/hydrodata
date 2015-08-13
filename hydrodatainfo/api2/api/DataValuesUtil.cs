using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace api 
{
    public class GetValuesResult
    {
        public GetValuesResult(int nVals)
        {
            _vals = new double[nVals];
        }
        private double[] _vals;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double[] Vals { get { return _vals; } }
    }

    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool HasValues { get; set; }
    }
    
    public class DataValuesUtil
    {
        public static int VariableCodeToID(string varCode)
        {
            switch (varCode.ToUpper())
            {
                case "SRAZKY":
                    return 1;
                case "VODSTAV":
                    return 4;
                case "PRUTOK":
                    return 5;
                case "SNIH":
                    return 8;
                case "TEPLOTA":
                    return 16;
                case "TMIN":
                    return 17;
                case "TMAX":
                    return 18;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Constructs the name of the binary file
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="varCode"></param>
        /// <param name="timeStep"></param>
        /// <returns></returns>
        public static string GetBinaryFileName(int siteId, string varCode, string timeStep)
        {
            //construct fileName
            string baseDir = Helpers.GetDataDirectory();
            string stepName = "h";
            string stationCode = siteId.ToString("D4");
            float noDataValue = -9999.0f;

            string file = string.Format(@"{0}\{1}\{0}_{1}_{2}.dat", timeStep, varCode, stationCode);
            string fileName = Path.Combine(baseDir, file);
            return fileName;
        }

        /// <summary>
        /// Date range from the database for checking of values existence!
        /// </summary>
        /// <param name="siteId">Site ID</param>
        /// <param name="varCode">Variable code like teplota, prutok...</param>
        /// <param name="timeStep">The time step: "h" for hourly and "d" for daily</param>
        public static DateRange GetDateRangeFromBinary(int siteId, string varCode, string timeStep)
        {
            string fileName = GetBinaryFileName(siteId, varCode, timeStep);
            return BinaryFileHelper.BinaryFileDateRange(fileName, timeStep);
        }

        public static string GetGroupFunction(string varCode)
        {
            switch (varCode.ToUpper())
            {
                case "SRAZKY":
                    return "sum";
                case "VODSTAV":
                    return "avg";
                case "PRUTOK":
                    return "avg";
                case "SNIH":
                    return "avg";
                case "TEPLOTA":
                    return "avg";
                case "TMIN":
                    return "min";
                case "TMAX":
                    return "max";
                default:
                    return "avg";
            }
        }

        public static string GetNumberFormat(string varCode)
        {
            switch (varCode.ToUpper())
            {
                case "SRAZKY":
                    return "0.0";
                case "VODSTAV":
                    return "0.00";
                case "PRUTOK":
                    return "0.0000";
                case "SNIH":
                    return "0.0";
                case "TEPLOTA":
                    return "0.0";
                case "TMIN":
                    return "0.0";
                case "TMAX":
                    return "0.0";
                default:
                    return "0.0";
            }
        }

        

        /// <summary>
        /// Gets a regular time-series of hourly values from the DB
        /// </summary>
        /// <param name="siteId">Site ID</param>
        /// <param name="varCode">Variable Code (SNIH, SRAZKY, PRUTOK..)</param>
        /// <param name="startDateTime">start time in UTC</param>
        /// <param name="endDateTime">end time in UTC</param>
        /// <param name="timeStep">"h" for hourly or "d" for daily</param>
        /// <param name="interpolate">TRUE if NA values should be interpolated</param>
        /// <returns>array of the values in hourly time-step</returns>
        public static float[] GetValuesFromBinary(int siteId, string varCode, DateTime startDateTime, DateTime endDateTime, string timeStep, bool interpolate)
        {
            float noDataVal = -9999.0f;

            //construct fileName
            string fileName = GetBinaryFileName(siteId, varCode, timeStep);

            //fetch values
            BinaryFileData data = BinaryFileHelper.ReadBinaryFileHourly(fileName, startDateTime, endDateTime, true);
            float[] vals = data.Data;
            
            if (interpolate == true)
            {
                if (varCode.ToUpper() != "SRAZKY")
                {
                    LinearInterpolator.ReplaceNoDataValues(vals, noDataVal);
                }
                else
                {
                    LinearInterpolator.ReplaceByZero(vals, noDataVal);
                }
            }
            return vals;
        }

        public static string convertValue(object val, int varId)
        {
            var dVal = Convert.ToDouble(val);
            switch (varId)
            {
                case 1:
                    // precipitation - no data value is now displayed as zero
                    return (dVal >= 0) ? (dVal * 0.1).ToString("0.0") : "NA";
                case 4:
                    // water stag
                    return (dVal >= 0) ? dVal.ToString("0.0") : "NA";
                case 5:
                    // discharge
                    return (dVal >= 0) ? dVal.ToString("0.0000") : "NA";
                case 8:
                    // snow
                    return (dVal >= 0) ? dVal.ToString("0") : "NA";
                case 16:
                case 17:
                case 18:
                    // air temperature
                    return (dVal >= -500) ? (dVal * 0.1).ToString("0.0") : "NA";
                default:
                    return dVal >= 0 ? (dVal * 0.1).ToString("0.0") : "NA";
            }
        }

        public static double convertValue2(object val, int varId)
        {
            var dVal = Convert.ToDouble(val);
            double noDataVal = -9999.0;
            switch (varId)
            {
                case 1:
                    // precipitation - no data value is now displayed as zero
                    return (dVal >= 0) ? (dVal * 0.1) : noDataVal;
                case 4:
                    // water stag
                    return (dVal >= 0) ? dVal: noDataVal;
                case 5:
                    // discharge
                    return (dVal >= 0) ? dVal : noDataVal;
                case 8:
                    // snow
                    return (dVal >= 0) ? dVal : noDataVal;
                case 16:
                case 17:
                case 18:
                    // air temperature
                    return (dVal >= -500) ? (dVal * 0.1) : noDataVal;
                default:
                    return dVal >= 0 ? (dVal * 0.1) : noDataVal;
            }
        }
    }
}