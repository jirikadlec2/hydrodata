using jk.plaveninycz.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace jk.plaveninycz.Logic
{
    public class BinaryFileHelper
    {
        public static void ReadBinaryFile(string fileName, DateTime startTime, DateTime endTime, TimeStep timeStep, 
            bool includeNA, IObservationList observations)
        {
            if (timeStep == TimeStep.Day)
            {
                ReadBinaryFileDaily(fileName, startTime, endTime, includeNA, observations);
            }
            else
            {
                ReadBinaryFileHourly(fileName, startTime, endTime, includeNA, observations);
            }
        }
        
        public static void ReadBinaryFileHourly(string fileName, DateTime startTime, DateTime endTime, 
            bool includeNA, IObservationList observations)
        {
            int SIZEOF_FLOAT = 4;
            int SIZEOF_LONG = 8;

            //clear out any existing obs.
            observations.Clear();
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //read the startDate
                    byte[] startDateBytes = new byte[SIZEOF_LONG];
                    stream.Read(startDateBytes, 0, startDateBytes.Length);
                    long[] startDateBinary = new long[1];
                    Buffer.BlockCopy(startDateBytes, 0, startDateBinary, 0, SIZEOF_LONG);
                    DateTime startDateFromFile = DateTime.FromBinary(startDateBinary[0]);

                    //check start time
                    if (startTime < startDateFromFile)
                    {
                        startTime = startDateFromFile;
                    }

                    //find position of query start time
                    int startTimePositionHours = (int)((startTime - startDateFromFile).TotalHours);
                    if (startTimePositionHours < 0)
                        return;
                    int numHoursInFile = (int)((stream.Length - SIZEOF_LONG) / SIZEOF_FLOAT);
                    DateTime endDateFromFile = startDateFromFile.AddHours(numHoursInFile);

                    if (endTime < startDateFromFile)
                        return;
                    if (startTime > endDateFromFile)
                        return;

                    long startTimePositionInBytes = SIZEOF_LONG + startTimePositionHours * SIZEOF_FLOAT;
                    int numHoursStartEnd = (int)((endTime - startTime).TotalHours);
                    long numBytesStartEnd = numHoursStartEnd * SIZEOF_FLOAT;
                    if (startTimePositionInBytes + numBytesStartEnd > stream.Length)
                    {
                        numBytesStartEnd = stream.Length - startTimePositionInBytes;
                        numHoursStartEnd = (int)(numBytesStartEnd / SIZEOF_FLOAT);
                    }
                    long endTimePositionInBytes = startTimePositionInBytes + numBytesStartEnd;

                    byte[] resultBytes = new byte[numBytesStartEnd];


                    stream.Seek(startTimePositionInBytes, SeekOrigin.Begin);
                    stream.Read(resultBytes, 0, resultBytes.Length);

                    float[] result = new float[numHoursStartEnd];
                    Buffer.BlockCopy(resultBytes, 0, result, 0, resultBytes.Length);

                    DateTime curTime = startTime;
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (result[i] == -9999.0)
                        {
                            if (includeNA)
                            {
                                observations.AddUnknownValue(curTime);
                            }
                        }
                        else
                        {
                            observations.AddObservation(curTime, result[i]);
                        }
                        curTime = curTime.AddHours(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ReadBinaryFileDaily(string fileName, DateTime startTime, DateTime endTime, 
            bool includeNA, IObservationList observations)
        {
            int SIZEOF_FLOAT = 4;
            int SIZEOF_LONG = 8;

            //clear out any existing obs.
            observations.Clear();
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //read the startDate
                    byte[] startDateBytes = new byte[SIZEOF_LONG];
                    stream.Read(startDateBytes, 0, startDateBytes.Length);
                    long[] startDateBinary = new long[1];
                    Buffer.BlockCopy(startDateBytes, 0, startDateBinary, 0, SIZEOF_LONG);
                    DateTime startDateFromFile = DateTime.FromBinary(startDateBinary[0]);

                    //check start time
                    if (startTime < startDateFromFile)
                    {
                        startTime = startDateFromFile;
                    }

                    //find position of query start time
                    int startTimePositionDays = (int)((startTime - startDateFromFile).TotalDays);
                    if (startTimePositionDays < 0)
                        return;
                    int numDaysInFile = (int)((stream.Length - SIZEOF_LONG) / SIZEOF_FLOAT);
                    DateTime endDateFromFile = startDateFromFile.AddDays(numDaysInFile);

                    if (endTime < startDateFromFile)
                        return;
                    if (startTime > endDateFromFile)
                        return;

                    long startTimePositionInBytes = SIZEOF_LONG + startTimePositionDays * SIZEOF_FLOAT;
                    int numDaysStartEnd = (int)((endTime - startTime).TotalDays);
                    long numBytesStartEnd = numDaysStartEnd * SIZEOF_FLOAT;
                    if (startTimePositionInBytes + numBytesStartEnd > stream.Length)
                    {
                        numBytesStartEnd = stream.Length - startTimePositionInBytes;
                        numDaysStartEnd = (int)(numBytesStartEnd / SIZEOF_FLOAT);
                    }
                    long endTimePositionInBytes = startTimePositionInBytes + numBytesStartEnd;

                    byte[] resultBytes = new byte[numBytesStartEnd];


                    stream.Seek(startTimePositionInBytes, SeekOrigin.Begin);
                    stream.Read(resultBytes, 0, resultBytes.Length);

                    float[] result = new float[numDaysStartEnd];
                    Buffer.BlockCopy(resultBytes, 0, result, 0, resultBytes.Length);

                    DateTime curTime = startTime;
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (result[i] == -9999.0)
                        {
                            if (includeNA)
                            {
                                observations.AddUnknownValue(curTime);
                            }
                        }
                        else
                        {
                            observations.AddObservation(curTime, result[i]);
                        }
                        curTime = curTime.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
