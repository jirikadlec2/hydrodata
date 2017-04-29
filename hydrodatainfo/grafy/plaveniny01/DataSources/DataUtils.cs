using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

/// <summary>
/// A helper class for working with an SQL database
/// </summary>
namespace jk.plaveninycz.DataSources
{

    public class DataUtils
    {
        public static string GetDataDirectory()
        {
            return @"D:\Websites\448cf9624b\www\data";
            //return @"C:\temp\data";
        }

        public static string GetConnectionString()
        {
            //return @"Data Source=.\SQLEXPRESS;Initial Catalog=plaveninycz1;User Id=sa; password=2c506bbe";
           
            
            //return @"Data Source=.\SQLEXPRESS;Initial Catalog=plaveninycz1;Integrated Security=True";
           


            //return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["connstr3"].ConnectionString;

            return @"data source = sql4.aspone.cz; Initial Catalog = db1856; User Id = db1856; Password = 2c506bbe";

            // return @"data source = sql2005.dotnethosting.cz; Initial Catalog=plaveninycz1; User Id=plaveninycz1; password=Ziqwdwq1";
        }

        /// <summary>
        /// Query execution, returns a data reader object
        /// <param name="cmd">the sql command to be executed</param>
        /// <param name="errorDetails">details about a possible database error</param>
        /// a connection is added to the command if needed
        /// </summary>
        public static SqlDataReader GetReader(SqlCommand cmd, string errorDetails)
        {
            if (cmd.Connection == null)
            {
                cmd.Connection = new SqlConnection(GetConnectionString());
            }

            try
            {
                cmd.Connection.Open();
                return (cmd.ExecuteReader(CommandBehavior.CloseConnection));
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw new DataException("An exception occured when accessing database: " +
                    errorDetails + ex.Message);
            }
        }

        /// <summary>
        /// Query execution, returns a data table object
        /// <param name="cmd">the sql command to be executed</param>
        /// <param name="errorDetails">details about a possible database error</param>
        /// a connection is added to the command if needed
        /// </summary>
        public static DataTable GetTable(SqlCommand cmd, string errorDetails)
        {
            DataTable table = new DataTable();
            if (cmd.Connection == null)
            {
                cmd.Connection = new SqlConnection(GetConnectionString());
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                da.Fill(table);
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw new DataException("An exception occured when accessing database: " +
                    errorDetails + ex.Message);
            }
            return table;
        }

        /// <summary>
        /// creates the SQL command object
        /// </summary>
        /// <returns></returns>
        public static SqlCommand CreateCommand()
        {
            string str = DataUtils.GetConnectionString();
            SqlConnection cnn = new SqlConnection(str);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            return cmd;
        }

        /// <summary>
        /// Creates a new SqlConnection object
        /// </summary>
        public static SqlConnection CreateConnection()
        {
            string cnnStr = DataUtils.GetConnectionString();
            return new SqlConnection(cnnStr);
        }
    }
}
