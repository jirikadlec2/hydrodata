using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZedGraph;
using HydroDesktop.WebServices;
using HydroDesktop.Interfaces.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace GraphWebService
{
    public class ChartCreator
    {
        private GraphPane SetupGraphPane(Series mySeries, int width, int height)
        {
            string graphTitle = "No Data";
            string xAxisTitle = "Time";
            string yAxisTitle = "Data Value";

            if (mySeries != null)
            {
                graphTitle = mySeries.Site.Name;
                xAxisTitle = "Time";
                yAxisTitle = string.Format("{0} [{1}]", mySeries.Variable.Name, mySeries.Variable.VariableUnit.Abbreviation);
            }

            GraphPane myPane = new GraphPane(new RectangleF(0f, 0f, (float)width, (float)height), graphTitle, xAxisTitle, yAxisTitle);
            myPane.Border.IsVisible = false;
            myPane.Legend.IsVisible = true;

            return myPane;
        }

        public Image CreateGraphFromWaterML(Series mySeries, int widthPixels, int heightPixels)
        {
            ZedGraph.GraphPane p = SetupGraphPane(mySeries, widthPixels, heightPixels);

            if (mySeries != null)
            {
                if (mySeries.DataValueList != null)
                {
                    DataSourcePointList source = new DataSourcePointList();
                    source.DataSource = mySeries.DataValueList;
                    source.XDataMember = "LocalDateTime";
                    source.YDataMember = "Value";

                    LineItem line = p.AddCurve(mySeries.Variable.Name, source, Color.Blue, SymbolType.None);

                    line.Line.Width = 2f;

                    p.XAxis.Type = AxisType.Date;

                    //adjust x axis range
                    p.XAxis.Scale.Min = XDate.DateTimeToXLDate(mySeries.BeginDateTime);
                    p.XAxis.Scale.Max = XDate.DateTimeToXLDate(mySeries.EndDateTime);

                    p.AxisChange();

                    //Bitmap bm = new Bitmap(10, 10);
                    //Graphics ge = Graphics.FromImage(bm);
                    //p.AxisChange(ge);
                }
            }

            Image img = p.GetImage();
            return img;
        }

        public Image resizeImage(Image imgToResize, Size size)
        {
            int destWidth = (int)size.Width;
            int destHeight = (int)size.Height;

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}