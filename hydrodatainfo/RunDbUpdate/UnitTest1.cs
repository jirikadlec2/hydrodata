using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Data.SqlClient;

namespace RunDbUpdate
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RunUpdateFiles()
        {
            string filesDir = @"E://dev/HydroDataInfo/teploty/";
            string sql = "Update plaveninycz.temperature " +
                "set temperature = @temperature where time_utc = @time AND station_id = @st";

            string dbConn =
        @"data source=.\SQLEXPRESS; Initial Catalog=plaveninycz1; User Id=sa; password=2c506bbe";

            dbConn = "Data Source=sql2005.dotnethosting.cz;Initial Catalog=plaveninycz1;User Id=plaveninycz1;Password=Ziqwdwq1;";


            SqlConnection cnn = new SqlConnection(dbConn);
            SqlCommand cmd = new SqlCommand(dbConn);
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@temperature",System.Data.SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@time", System.Data.SqlDbType.SmallDateTime));
            cmd.Parameters.Add(new SqlParameter("@st", System.Data.SqlDbType.SmallInt));
            cmd.Connection = cnn;

            foreach (string fn in Directory.GetFiles(filesDir))
            {
                if (fn.Contains("fixed_"))
                {
                    string siteNo1 = fn.Substring(fn.IndexOf("fixed_") + 6);
                    string siteNo = siteNo1.Substring(0, siteNo1.IndexOf("."));
                    int siteId = Convert.ToInt32(siteNo);
                    Console.WriteLine(fn);
                    
                    //read file
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        string line = sr.ReadLine();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] res = line.Split(new char[] { '\t' });
                            DateTime dt = Convert.ToDateTime(res[0]);
                            if (res[1] != "NA")
                            {
                                int tp = Convert.ToInt32(Convert.ToDouble(res[1]) * 10.0);

                                try
                                {
                                    cnn.Open();
                                    cmd.Parameters["@temperature"].Value = tp;
                                    cmd.Parameters["@st"].Value = siteId;
                                    cmd.Parameters["@time"].Value = dt;
                                    cmd.ExecuteNonQuery();
                                    cnn.Close();
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
