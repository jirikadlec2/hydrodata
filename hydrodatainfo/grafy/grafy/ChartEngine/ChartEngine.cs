using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Drawing;
using jk.plaveninycz.Interfaces;
using jk.plaveninycz.BO;
using jk.plaveninycz.DataSources;
using jk.plaveninycz.search;
using jk.plaveninycz.Validation;
using jk.plaveninycz.Bll;
using ZedGraph;
using grafy.Properties;
using System.Collections.Generic;

namespace jk.plaveninycz.graph
{

/// <summary>
/// This is the class for creating chart output.
/// given input parameters, a png image is 
/// created and can be used as response output stream
/// or saved to disk.
/// </summary>
    public class ChartEngine
    {
        #region Declarations

        int _width = 656;
        int _height = 240;
        bool _showText = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ChartEngine with default 'width' and 'height' values
        /// </summary>
        public ChartEngine()
        {
            _width = DefaultWidth;
            _height = DefaultHeight;
        }

        /// <summary>
        /// Creates a new ChartEngine, with resulting image width and height specified
        /// </summary>
        public ChartEngine(int width, int height)
        {
            _width = width; //_width = 656;  //width for hydro=696
            _height = height; //_height= 240;  //width for hydro=
        }

        #endregion


        #region Properties

        public bool ShowText
        {
            get { return ShowText; }
            set { _showText = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public static int DefaultWidth
        {
            get { return 656; }
        }

        public static int DefaultHeight
        {
            get { return 290; }
        }

        #endregion Properties


        #region Create Chart


        /// <summary>
        /// Generates a new 'chart' image to be written to http output stream.
        /// Also saves the newly generated image to file on disk.
        /// </summary>
        /// <param name="ImagePath">Http request path of the chart image</param>
        public Bitmap GenerateImage(string ImagePath)
        {
            Bitmap bmp;

            ChartEngine ChartGen = new ChartEngine();
            Width = 600;
            Height = 275;
            ShowText = true;

            QueryStringValidator QVal = new QueryStringValidator(ImagePath);

            QVal.Validate(); //the language and culture is also set in the validator

            if (QVal.IsValid)
            {
                // get query string parameters
                string lang = QVal.Culture;
                int StId = QVal.StationId;
                Variable curVar = QVal.Variable;
                DateTime startTime = QVal.StartDate;
                DateTime endTime = QVal.EndDate;


                // TODO: for longer intervals change curVar to [precip_day]

                // search for the channel corresponding to station/variable combination
                ChannelSearchResult found = ChannelSearchEngine.FindByStationIdAndVariable(
                    StId, curVar);

                if (!found.HasMatchingStations) //no matching station
                {
                    return ChartGen.CreateErrorChart(found.ErrorMessage);
                }

                if (found.HasMatchingChannels) //correct results found
                {
                    Channel ch = found.Channels[0];
                    ITimeSeries ts;
                    TimeInterval interval = new TimeInterval(startTime, endTime);
                    MessageWriter writer = new MessageWriter(ch.Station.Name, ch.Variable.Name);

                    // for precipitation, if it's hourly and a longer period,
                    // always select daily precipitation to display a readable graph
                    // otherwise, hourly precipitation will be displayed.
                    int maxHourPrecipPeriod = 10;
                    VariableEnum var = ch.Variable.VarEnum;
                    if (var == VariableEnum.PrecipHour && interval.Length.TotalDays > maxHourPrecipPeriod)
                    {
                        var = VariableEnum.Precip;
                    }

                    //here we retrieve the time series
                    ts = TimeSeriesManager.GetTimeSeries(ch, interval);
                    

                    if (ts.Count == 0) //this also means 'no data for time series'
                    {
                        bmp = ChartGen.CreateErrorChart
                            (writer.WriteErrorMessage(MessageType.NoDataForPeriod));
                    }

                    bmp = ChartGen.CreateChart(ch, ts);
                }
                else
                {
                    // incorrect input - create a 'chart' with an error message
                    bmp = ChartGen.CreateErrorChart(found.ErrorMessage);
                }
            }
            else
            {
                // incorrect input - create a 'chart' with an error message
                bmp = ChartGen.CreateErrorChart(QVal.ErrorMessage);
            }

            return bmp;
        }
        
        
        
        /// <summary>
        /// Creates a chart - plots are made for all sensors in the time series
        /// </summary>
        /// <param name="ts">The time series of data to be ploted</param>
        /// <param name="ch">The channel (contains station, variable and copyright description)
        /// </param>
        /// <returns>The chart png image</returns>
        public Bitmap CreateChart(Channel ch, ITimeSeries ts)
        {
            GraphPane pane = SetupGraphPane();
            SetupChart(ts, ch, pane);
            PlotChartForChannel(ts, ch, pane);
            return ExportGraph(pane);
        }

        private void PlotChartForChannel(ITimeSeries ts, Channel ch, GraphPane pane)
        {
            VariableEnum varEnum = ch.Variable.VarEnum;

            if (ts.End < Convert.ToDateTime("2013-01-01") && ts.PercentAvailableData < 1.0) //no data..
            {
                ShowNoDataTextBox(Resources.no_data, pane);
                ShowErrorTextBox(Resources.no_data_message, pane);
            }
            else
            {
                switch (varEnum)
                {
                    case VariableEnum.Stage:
                        PlotStageDischarge(ts, pane);
                        break;
                    case VariableEnum.Discharge:
                        PlotStageDischarge(ts, pane);
                        break;
                    case VariableEnum.Snow:
                        PlotSnow(ts, pane);
                        break;
                    case VariableEnum.SoilWater10:
                    case VariableEnum.SoilWater50:
                        PlotSoilWater(ts, pane);
                        break;
                    case VariableEnum.Precip:
                        PlotPrecip(ts, pane);
                        break;
                    case VariableEnum.PrecipHour:
                        PlotPrecipHour(ts, pane);
                        break;
                    case VariableEnum.Temperature:
                        PlotTemperature(ts, pane);
                        break;
                    default:
                        break;
                }
                
                //also plot 'no data' if necessary
                if (ch.Variable.VarEnum != VariableEnum.Stage && 
                    ch.Variable.VarEnum != VariableEnum.Discharge && 
                    ts.PercentAvailableData > 0.01)
                {
                    //PlotMissingData(ts, pane);
                }

                //add 'copyright' notice
                ShowCopyrightTextBox("data: " + ch.Station.Operator, pane);
            }
        }

        private GraphPane SetupGraphPane()
        {
            GraphPane myPane = new GraphPane(new RectangleF(0, 0, this.Width, this.Height), "no data", "", "y");
            myPane.Border.IsVisible = false;
            myPane.Legend.IsVisible = true;
            return myPane;
        }

        private void SetupChart(ITimeSeries ts, Channel ch, GraphPane pane)
        {
            Station st = ch.Station;
            Variable var = ch.Variable;
            DateTime start = ts.Start;
            DateTime end = ts.End;
            DrawTitle(pane, var, st, start, end);
            SetupAxis(pane, var, start, end);
            SetupGrid(pane, var.VarEnum);
            SetupLegend(pane);
        }

        private void DrawTitle(GraphPane myPane, Variable var, Station st, DateTime minDate, DateTime maxDate)
        {
            string title;
            string stName;
            if ( var.VarEnum == VariableEnum.Discharge || var.VarEnum == VariableEnum.Stage )
            {
                stName = string.Format("{0} ({1})", st.Name, st.River.Name);
            }
            else
            {
                stName = st.Name;
            }
            title = String.Format("{0} - {1} {2} - {3}",
                var.Name, stName, minDate.ToShortDateString(),
                maxDate.ToShortDateString());
            myPane.Title.Text = title;
            myPane.Title.FontSpec.Size = 18;
            myPane.Title.FontSpec.IsBold = false;
        }

        private void SetupAxis(GraphPane myPane, Variable var, DateTime minDate, DateTime maxDate)
        {
            
            //setup font for axis name and tick marks text
            int scaleFontSize = 16;
            int titleFontSize = 18;

            // X axis
            ZedGraph.Axis myXAxis = myPane.XAxis;
            myXAxis.Type = AxisType.Date;
            myXAxis.Scale.Min = XDate.DateTimeToXLDate(minDate);
            myXAxis.Scale.Max = XDate.DateTimeToXLDate(maxDate);            
            myXAxis.Scale.FontSpec.Size = scaleFontSize;
            myPane.XAxis.Title.FontSpec.Size = titleFontSize;

            // Y axis
            ZedGraph.Axis myYAxis = myPane.YAxis;
            if (var.VarEnum != VariableEnum.Temperature)
            {
                myYAxis.Scale.Min = 0;
            }
           
            myYAxis.Scale.FontSpec.Size = scaleFontSize;
            myYAxis.Title.FontSpec.Size = titleFontSize;

            //special Y axis for temperature
            


            // special Y axis for discharge
            if (var.VarEnum == VariableEnum.Discharge)
            {
                char exp3 = (char)179;
                myYAxis.Title.Text = string.Format("{0} (m{1}/s)", var.Name, exp3);
            }
            else
            {
                myYAxis.Title.Text = var.Name + " (" + var.Units + ")";
            }

            // Y2 axis - setup for cumulative precipitation
            if ( var.VarEnum == VariableEnum.Precip || var.VarEnum == VariableEnum.PrecipHour )
            {
                ZedGraph.Axis myY2Axis = myPane.Y2Axis;
                myY2Axis.Scale.Min = 0;
                myY2Axis.Scale.FontSpec.Size = scaleFontSize;

                myY2Axis.Title.Text = Resources.precip_sum_axis;
                myY2Axis.Title.FontSpec.Size = titleFontSize;
                myY2Axis.IsVisible = true;              
            }
        }

        private void SetupGrid(GraphPane myPane, VariableEnum v)
        {
            //first do common grid settings
            myPane.XAxis.MajorGrid.DashOn = 6.0f;
            myPane.XAxis.MajorGrid.DashOff = 2.0f;
            myPane.XAxis.MajorGrid.Color = Color.LightGray;
            myPane.XAxis.MajorGrid.IsVisible = true;
            
            myPane.YAxis.MajorGrid.Color = Color.LightGray;
            myPane.YAxis.MajorGrid.DashOn = 6.0f;
            myPane.YAxis.MajorGrid.DashOff = 2.0f;
            myPane.YAxis.MajorGrid.IsVisible = true;
            
            //special variable-specific settings
            switch ( v )
            {
                case VariableEnum.Snow:
                    myPane.XAxis.MajorGrid.IsVisible = false;
                    break;
                default:
                    break;
            }        
        }

        private void SetupLegend(GraphPane myPane)
        {
            int legendFontSize = 18;
            ZedGraph.Legend leg = myPane.Legend;
            leg.FontSpec.Size = legendFontSize;
        }

        private Bitmap ExportGraph(GraphPane pane)
        {
            Bitmap bm = new Bitmap(1, 1);
            using ( Graphics g = Graphics.FromImage(bm) )
            {
                pane.AxisChange(g);
            }

            return pane.GetImage();
        }

        // plot the stage!
        private void PlotStageDischarge(ITimeSeries ts, GraphPane myPane)
        {
            TimeInterval interval = new TimeInterval(ts.Start, ts.End);

            if ( ts.Count > 0 )
            {
                List<HydroTimeSeries> tsList = TimeSeriesManager.SplitTimeSeries(ts);
                
                foreach (HydroTimeSeries ts2 in tsList)
                {
                    LineItem myCurve = myPane.AddCurve("", ts2, Color.Blue, SymbolType.None);
                    myCurve.Line.Width = 1F;
                    myCurve.Line.Fill = new Fill(Color.FromArgb(128, Color.Blue));
                }
                //mark missing data points
                if (tsList.Count > 1)
                {
                    TimeStep step = TimeSeriesManager.GetDefaultTimeStep(VariableEnum.Stage, interval);

                    HydroTimeSeries missingTs = TimeSeriesManager.GetMissingValuesHydro(ts, ts.Start, ts.End, step);
                    PlotMissingData(missingTs, myPane);

                }
            }
        }


        /// <summary>
        /// Plot the snow !!!!
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="myPane"></param>
        private void PlotSnow(ITimeSeries ts, GraphPane myPane)
        {
            TimeInterval interval = new TimeInterval(ts.Start, ts.End);
            
            //Main creation of curve
            if ( interval.Length.TotalDays < 160 )
            {
                BarItem myCurve = myPane.AddBar("", ts, Color.Blue);
                myCurve.Bar.Border.Color = Color.Blue;
                myCurve.Bar.Border.IsVisible = true;
                myCurve.Bar.Fill.Type = FillType.Solid;
                myCurve.Bar.Fill.IsScaled = false;
            }
            else if (interval.Length.TotalDays < 200)
            {
                StickItem myCurve = myPane.AddStick("", ts, Color.Blue);
            }
            else if (interval.Length.TotalDays < 400)
            {
                BarItem myCurve = myPane.AddBar("", ts, Color.Blue);
                myCurve.Bar.Border.Color = Color.Blue;
                myCurve.Bar.Border.IsVisible = true;
                myCurve.Bar.Fill.Type = FillType.Solid;
                myCurve.Bar.Fill.IsScaled = false;
            }
            else
            {
                StickItem myCurve = myPane.AddStick("", ts, Color.Blue);
            }
            //else
            //{
            //    StickItem myCurve = myPane.AddStick("", ts, Color.Blue);
            //    //LineItem myCurve = myPane.AddCurve("", ts, Color.Blue, SymbolType.None);
            //    //myCurve.Line.Width = 0F;
            //    //myCurve.Line.Fill = new Fill(Color.Blue);
            //}
        }

        private void PlotSoilWater(ITimeSeries ts, GraphPane myPane)
        {
            if ( ts.Count > 0 )
            {
                //Main creation of curve
                LineItem myCurve = myPane.AddCurve("", ts, Color.Blue, SymbolType.None);
                myCurve.Line.Width = 2F;
            }
        }

        private void PlotTemperature(ITimeSeries ts, GraphPane myPane)
        {
            if (ts.Count > 0)
            {
                TimeInterval interv = new TimeInterval(ts.Start, ts.End);
                TimeStep step = TimeSeriesManager.GetDefaultTimeStep(VariableEnum.Temperature, interv);
                ITimeSeries ts2 = TimeSeriesManager.FillDataGaps(ts, step);
                LineItem myCurve = myPane.AddCurve("", ts2, Color.Blue, SymbolType.None);
                myCurve.Line.Width = 0.5F;
            }
        }


        /// <summary>
        /// Plot the daily precipitation !!!!
        /// </summary>
        private void PlotPrecip(ITimeSeries ts, GraphPane myPane)
        {
            TimeInterval interval = new TimeInterval(ts.Start, ts.End);
            string label = Resources.precip_label;

            if ( interval.Length.TotalDays < 100 )
            {
                BarItem myCurve = myPane.AddBar(label, ts, Color.Blue);
                myCurve.Bar.Border.Color = Color.Blue;
                myCurve.Bar.Border.IsVisible = true;
                myCurve.Bar.Fill.Type = FillType.Solid;
                myCurve.Bar.Fill.IsScaled = false;
            }
            else
            {
                StickItem myCurve = myPane.AddStick(label, ts, Color.Blue);
            }

            //cumulative precipitation..
            ITimeSeries ts2 = ts.ShowCumulative();
            if ( ts2.Count > 0 )
            {
                LineItem myCurve2 = myPane.AddCurve(Resources.precip_sum_label, 
                    ts2, Color.Red, SymbolType.None);
                myCurve2.IsY2Axis = true;

                myPane.AxisChange();
            }
            
        }

        /// <summary>
        /// Plot the hourly precipitation !!!!
        /// </summary>
        private void PlotPrecipHour(ITimeSeries ts, GraphPane myPane)
        {
            TimeInterval interval = new TimeInterval(ts.Start, ts.End);
            string varName = Resources.precip_label;

            //Main creation of curve
            if ( interval.Length.TotalDays <= 2 )
            {
                BarItem myCurve = myPane.AddBar(varName, ts, Color.Blue);
                myCurve.Bar.Border.Color = Color.Blue;
                myCurve.Bar.Border.IsVisible = true;
                myCurve.Bar.Fill.Type = FillType.Solid;
                myCurve.Bar.Fill.IsScaled = false;
            }
            else
            {
                StickItem myCurve = myPane.AddStick(varName, ts, Color.Blue);
            }

            //cumulative precipitation..
            ITimeSeries ts2 = ts.ShowCumulative();
            LineItem myCurve2 = myPane.AddCurve(Resources.precip_sum_label, 
                ts2, Color.Red, SymbolType.None);
            myCurve2.IsY2Axis = true;
            myPane.AxisChange();
        }

        /// <summary>
        /// puts little "crosses" on missing data points
        /// </summary>
        private void PlotMissingData(ITimeSeries ts, GraphPane myPane)
        {
            string noDataText = Resources.no_data;

            float crossSize = 10F;
            //double yAxisH = myPane.YAxis.Scale.ReverseTransform(crossSize);
            double yAxisH = myPane.YAxis.Scale.Max / 25.0;

            //ITimeSeries ts2 = ts.ShowUnknown(yAxisH);
            for (int i = 0; i < ts.Count; i++ )
            {
                ts[i].Y = yAxisH;
            }

            if ( ts != null )
            {
                if ( ts.Count > 0 )
                { 
                    LineItem missingCurve = myPane.AddCurve(noDataText, ts, Color.Red, SymbolType.XCross);
                    missingCurve.Line.IsVisible = false;
                    missingCurve.Symbol.Size = crossSize;
                    missingCurve.Symbol.IsVisible = true;
                    myPane.AxisChange();
                }
            }
        }

        private void ShowCopyrightTextBox(string text, GraphPane myPane)
        {
            TextObj obj = new TextObj(text, 0.025F, 0.05F, CoordType.ChartFraction);
            obj.FontSpec = new FontSpec("MS Sans Serif", 20, Color.DarkBlue, false, false, false);
            obj.Location.AlignH = AlignH.Left;
            obj.Location.AlignV = AlignV.Top;
            obj.FontSpec.Border.Color = Color.Green;
            obj.FontSpec.Border.IsVisible = false;
            //obj.FontSpec.IsItalic = true;
            obj.FontSpec.Fill.IsVisible = true;
            
            myPane.GraphObjList.Add(obj);
        }

        private void ShowNoDataTextBox(string noDataText, GraphPane myPane)
        {
            //show a 'no data' box  
            TextObj noDataObj = new TextObj(noDataText.ToUpper(), 0.5, 0.5, CoordType.PaneFraction);
            noDataObj.Location.AlignH = AlignH.Center;
            noDataObj.Location.AlignV = AlignV.Center;
            noDataObj.FontSpec.Size = 40f;
            noDataObj.IsVisible = true;
            noDataObj.IsClippedToChartRect = true;
            myPane.GraphObjList.Add(noDataObj);
            myPane.AxisChange();   
        }

        private void ShowErrorTextBox(string errorMessage, GraphPane myPane)
        {
            string HeadingText = Resources.ChartError_Heading;
            TextObj HeadingObj = new TextObj(HeadingText, 0.1F, 0.2F, CoordType.PaneFraction);
            HeadingObj.FontSpec = new FontSpec("Arial", 40, Color.Red, true, false, false);
            HeadingObj.Location.AlignH = AlignH.Left;
            HeadingObj.Location.AlignV = AlignV.Top;
            myPane.GraphObjList.Add(HeadingObj);

            TextObj ErrorObj = new TextObj(errorMessage, 0.1F, 0.4F);
            ErrorObj.Location.CoordinateFrame = CoordType.PaneFraction;
            //ErrorObj.Location.Height = 0.2;
            ErrorObj.Location.AlignH = AlignH.Left;
            ErrorObj.Location.AlignV = AlignV.Top;
            //ErrorObj.Text = errorMessage;
            
            //TextObj ErrorObj = new TextObj(errorMessage, 0.75, 0.5, CoordType.PaneFraction);
            ErrorObj.FontSpec = new FontSpec("Arial", 20, Color.Black, false, false, false);
            ErrorObj.FontSpec.Border.IsVisible = false;
            ErrorObj.FontSpec.StringAlignment = StringAlignment.Near;
            //ErrorObj.FontSpec.StringAlignment = StringAlignment.Center;
            
            myPane.GraphObjList.Add(ErrorObj);

            myPane.AxisChange();
        }

        //generate chart with error message
        // and write it directly to output stream
        public System.Drawing.Bitmap CreateErrorChart(string ErrorMessage)
        {
            GraphPane pane = SetupGraphPane();
            ShowErrorTextBox(ErrorMessage, pane);
            return ExportGraph(pane);
        }

        #endregion
    }
}
