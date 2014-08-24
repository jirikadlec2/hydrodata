using System;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using jk.plaveninycz.Interfaces;
using jk.plaveninycz.BO;
using jk.plaveninycz.Bll;
using jk.plaveninycz.search;
using jk.plaveninycz.graph;
using System.Xml;

namespace grafy
{
    /// <summary>
    /// The web handler to create a graph of observations
    /// using the URL parameters
    /// </summary>
    public class ChartWebHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //set this to TRUE if files are to be saved on the disk
            bool saveToDisk = false;

            //declaration of local variables
            string path = context.Request.Path.ToLower();
            System.Collections.Specialized.NameValueCollection queryParams = context.Request.QueryString;

            // process the query string parameters
            string imageName = path;
            int slashIndex = path.LastIndexOf("/");
            if (slashIndex >= 0)
            {
                imageName = path.Remove(0, slashIndex + 1);
            }

            string Name = string.Empty;
            System.Drawing.Bitmap bmp;
            ChartEngine engine = new ChartEngine();

            if (saveToDisk)
            {
                string physicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
                string parent = physicalPath.Substring(0, physicalPath.LastIndexOf("\\"));
                string TempDir = (Directory.GetParent(physicalPath)).FullName + "\\charts\\";

                string ImagePath = System.IO.Path.Combine(TempDir, imageName + ".png");

                if (File.Exists(ImagePath))
                {
                    // is the image already exists, return the existing file
                    bmp = new System.Drawing.Bitmap(ImagePath);
                }
                else
                {
                    bmp = engine.GenerateImage(imageName);
                    SaveImageToDisk(bmp, ImagePath);
                }
            }
            else
            {
                bmp = engine.GenerateImage(imageName);
            }

            //ProcessBitmap will send the bitmap as a http binary stream
            //so that it can be displayed as an image by the user's browser
            ProcessBitmap(bmp, context);
        }

        private DateTime ParseDateTime(string str)
        {
            if (str.Length >= 8)
            {
                int year = Convert.ToInt32(str.Substring(0, 4));
                int month = Convert.ToInt32(str.Substring(4, 2));
                int day = Convert.ToInt32(str.Substring(6, 2));
                return new DateTime(year, month, day);
            }
            else if (str.Length >= 6)
            {
                int year = Convert.ToInt32(str.Substring(0, 4));
                int month = Convert.ToInt32(str.Substring(4, 2));
                return new DateTime(year, month, 1);
            }
            else if (str.Length >= 4)
            {
                int year = Convert.ToInt32(str.Substring(0, 4));
                return new DateTime(year, 1, 1);
            }
            else
            {
                return new DateTime(2000, 1, 1);
            }
        }

        private void ProcessBitmap(System.Drawing.Bitmap bmp, HttpContext context)
        {
            MemoryStream PngStream = new MemoryStream();
            bmp.Save(PngStream, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
            context.Response.ClearContent();
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(PngStream.GetBuffer());
            PngStream.Close();
            context.Response.End();
        }


       

        /// <summary>
        /// Saves a backup of the bitmap to a file on disk
        /// </summary>
        /// <param name="bmp">the bitmap object</param>
        /// <param name="imagePath">the path of request</param>
        private void SaveImageToDisk(Bitmap bmp, string imagePath)
        {
            imagePath = imagePath.Replace(@"/", @"\");
            try
            {
                bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch(Exception ex)
            {
                throw new System.IO.IOException("Error saving image to " + imagePath + " " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
