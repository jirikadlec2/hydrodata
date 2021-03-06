﻿using System;
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
            BinaryFileData data = BinaryFileHelper.ReadBinaryFile(fileName, startDateTime, endDateTime, timeStep, true);

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

        public static float convertValue2(object val, int varId)
        {
            var dVal = Convert.ToSingle(val);
            float noDataVal = -9999.0f;
            switch (varId)
            {
                case 1:
                    // precipitation - no data value is now displayed as zero
                    return (dVal >= 0) ? (float)(dVal * 0.1) : noDataVal;
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
                    return (dVal >= -500) ? (float)(dVal * 0.1) : noDataVal;
                default:
                    return dVal >= 0 ? (float)(dVal * 0.1) : noDataVal;
            }
        }

        public static string GetTableName(int variableId)
        {
            switch (variableId)
            {
                case 1:
                    return "rain_hourly";

                case 4:
                    return "stage";

                case 5:
                    return "discharge";

                case 8:
                    return "snow";

                case 16:
                case 17:
                case 18:
                    return "temperature";

                default:
                    return "rain_hourly";
            }
        }

        public static string GetColumnName(int variableId)
        {
            switch (variableId)
            {
                case 1:
                    return "rain_mm_10";

                case 4:
                    return "stage_mm";

                case 5:
                    return "discharge_cms";

                case 8:
                    return "snow_cm";

                case 16:
                case 17:
                case 18:
                    return "temperature";

                default:
                    return "rain_mm_10";
            }
        }

        public static float[] GetDailyValuesFromDb(int siteId, string varCode, DateTime startDateTime, DateTime endDateTime, bool interpolate)
        {
            string connStr = Helpers.GetConnectionString();
            string timeStep = "day";
            float noDataVal = -9999.0f;

            //numeric variable id
            int varId = VariableCodeToID(varCode);

            string tableName = GetTableName(varId);

            int numDays = Convert.ToInt32(endDateTime.Subtract(startDateTime).TotalDays);

            //pre-fetch values
            float[] vals = new float[numDays + 1];
            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = noDataVal;
            }

            //values: get from database...
            string groupFunction = GetGroupFunction(varCode);
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("plaveninycz.new_query_observations", cnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@st_id", SqlDbType.SmallInt));
                    cmd.Parameters.Add(new SqlParameter("@var_id", SqlDbType.SmallInt));
                    cmd.Parameters.Add(new SqlParameter("@start_time", SqlDbType.SmallDateTime));
                    cmd.Parameters.Add(new SqlParameter("@end_time", SqlDbType.SmallDateTime));
                    cmd.Parameters.Add(new SqlParameter("@time_step", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@group_function", SqlDbType.VarChar));

                    cmd.Parameters["@st_id"].Value = Convert.ToInt32(siteId);
                    cmd.Parameters["@var_id"].Value = varId;
                    cmd.Parameters["@start_time"].Value = startDateTime;
                    cmd.Parameters["@end_time"].Value = endDateTime;
                    cmd.Parameters["@time_step"].Value = timeStep;
                    cmd.Parameters["@group_function"].Value = groupFunction;

                    cnn.Open();

                    SqlDataReader r = cmd.ExecuteReader();
                    int obsTimeIndex = r.GetOrdinal("obs_time");
                    int obsValueIndex = r.GetOrdinal("obs_value");
                    int valsIndex = 0;

                    DateTime curDate = startDateTime;
                    while (r.Read())
                    {
                        curDate = (Convert.ToDateTime(r[obsTimeIndex])).Date;
                        valsIndex = (int)((curDate.Subtract(startDateTime)).TotalDays);
                        //now insert the real measurement..
                        vals[valsIndex] = convertValue2(r[obsValueIndex], varId);
                    }
                }
            }
            if (interpolate == true)
            {
                if (varCode.ToUpper() != "SRAZKY")
                {
                    LinearInterpolator.ReplaceNoDataValues(vals, -9999.0f);
                }
                else
                {
                    //TODO we should probably use nearest stations or one-time DB processing
                    LinearInterpolator.ReplaceByZero(vals, -9999.0f);
                }
            }
            return vals;
        }



        /// <summary>
        /// Date range from the database for checking of values existence!
        /// </summary>
        /// <param name="siteId">Site ID</param>
        /// <param name="varCode">Variable ID</param>
        /// <param name="start">Start</param>
        /// <param name="end">End</param>
        public static DateRange GetDateRangeFromDb(int siteId, string varCode)
        {
            string connStr = Helpers.GetConnectionString();
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.MaxValue;
            int varId = VariableCodeToID(varCode);
            string tableName = GetTableName(varId);
            bool hasValues = true;
            string sql = String.Format("SELECT MIN(time_utc) AS 'begin', MAX(time_utc) AS 'end' FROM plaveninycz.{0} WHERE station_id={1}", tableName, siteId);
            using (SqlConnection cnn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cnn.Open();
                    SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleResult);
                    r.Read();
                    if (!r.HasRows)
                    {
                        hasValues = false;
                    }
                    else
                    {
                        object startObj = r["begin"];
                        object endObj = r["end"];
                        if (startObj is DBNull || endObj is DBNull)
                        {
                            hasValues = false;
                        }
                        else
                        {
                            startDateTime = Convert.ToDateTime(r["begin"]);
                            endDateTime = Convert.ToDateTime(r["end"]);
                        }
                    }
                }
            }

            DateRange range = new DateRange();
            range.Start = startDateTime;
            range.End = endDateTime;
            range.HasValues = hasValues;
            return range;
        }

    }
}