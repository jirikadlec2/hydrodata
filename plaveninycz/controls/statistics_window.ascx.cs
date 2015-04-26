using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using jk.plaveninycz;
using jk.plaveninycz.Observations;

namespace jk.plaveninycz.Presentation
{

    public partial class controls_statistics_window : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //LoadStatList(_ts);
            //repeater1.DataSource = _statList;
            //repeater1.DataBind();
        }

        public void Update()
        {
            if ( _stats != null && _var != null )
            {
                LoadStatList();
                repeater1.DataSource = _statList;
                repeater1.DataBind();
            }
        }

        public Variable Variable
        {
            get { return _var; }
            set { _var = value; }
        }

        public Statistics Statistics
        {
            get { return _stats; }
            set { _stats = value; }
        }

        private void LoadStatList()
        {
            _statList = new StatisticsList(_stats, _var);            
        }

        //protected void repeater1_ItemDataBound(object source,
        //                                    RepeaterItemEventArgs e)
        //{
        //    if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
        //    {
        //        Literal lblName = (Literal) e.Item.FindControl("stat_name");
        //        Literal lblValue = (Literal) e.Item.FindControl("stat_value");
        //        DictionaryEntry entry = (DictionaryEntry) e.Item.DataItem;
        //        lblName.Text = (string) GetLocalResourceObject(entry.Key.ToString());
        //        lblValue.Text = entry.Value.ToString();
        //    }

        //}

        private Statistics _stats;
        private Variable _var;
        private StatisticsList _statList;
    }
}
