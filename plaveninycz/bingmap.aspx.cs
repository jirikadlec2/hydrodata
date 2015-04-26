using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using jk.plaveninycz.Geo;
using jk.plaveninycz.DataSources;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class bingmap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "getMap", "bing_map_functions.js");


        if (!Page.IsPostBack)
        {
            //fillData();
        }

        //if (!Master.Page.ClientScript.IsStartupScriptRegistered("myBingMapScript"))
        //{
        //    Master.Page.ClientScript.RegisterStartupScript
        //        (this.GetType(), "myBingMapScript", "insideJS();", true);
        //}

        //if (!Master.Page.ClientScript.IsStartupScriptRegistered("myBingMapScript"))
        //{
        //    Master.Page.ClientScript.RegisterStartupScript
        //        (this.GetType(), "myBingMapScript", "getMap();", true);
        //}              

    }
    protected void button1_Click(object sender, EventArgs e)
    {
        //get the variable code
        string varCode;
        int varId;
        DateTime startDate, endDate;
        switch (ListBox1.SelectedItem.Text)
        {
            case "Snow":
                varCode = "snw";
                varId = 8;
                startDate = DateTime.Now.Date.AddYears(-1);
                endDate = DateTime.Now.Date;
                break;
            case "Discharge":
                varCode = "flw";
                varId = 5;
                startDate = DateTime.Now.Date.AddDays(-28);
                endDate = DateTime.Now.Date;
                break;
            case "Precipitation":
                varCode = "p1h";
                varId = 1;
                startDate = DateTime.Now.Date.AddDays(-6);
                endDate = DateTime.Now.Date;
                break;
            default:
                varCode = "snw";
                varId = 8;
                startDate = DateTime.Now.Date.AddYears(-1);
                endDate = DateTime.Now.Date;
                break;
        }      
        
        string cnn = DataUtils.ConnectionString;
        StringBuilder sb = new StringBuilder();
        using (SqlConnection conn = new SqlConnection(cnn))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                string sqlSites = "SELECT Stations.st_id, st_name, location_id FROM Stations INNER JOIN StationsVariables stv ON Stations.st_id = stv.st_id WHERE var_id=" + varId;
                string sql = "SELECT st.st_id, st_name, lat, lon FROM " +
                    "(" + sqlSites + ") st INNER JOIN Locations ON st.location_id = Locations.loc_id";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                
                sb.Append("var variableCode='" + varCode + "'; ");
                sb.AppendFormat("var startDate='{0}'; var endDate='{1}'; ", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
                sb.Append("var ids = new Array(); var lats = new Array(); var longs = new Array(); var names=new Array();");
                sb.AppendLine();

                int i = 0;

                while (dr.Read())
                {
                    sb.AppendFormat("ids[{0}]={1};lats[{0}]={2};longs[{0}]={3};names[{0}]='{4}';", i, dr["st_id"], dr["Lat"], dr["lon"], dr["st_name"]);
                    i++;
                }
            }
        }
        //button2.Attributes["onclick"] += "LoadPushPins();";
        //ClientScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PointArrays", sb.ToString(), true);
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterClientScriptBlock(this.GetType(), "PointArrays", sb.ToString(), true);
        
    }
}