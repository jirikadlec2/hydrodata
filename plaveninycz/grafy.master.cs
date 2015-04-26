using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using jk.plaveninycz;
using jk.plaveninycz.DataSources;
using jk.plaveninycz.Observations;
using jk.plaveninycz.Presentation;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class grafy : System.Web.UI.MasterPage
{
    /// <summary>
    /// initializes the navigation menu list
    /// </summary>
    
    protected void Page_Init(object sender, EventArgs e)
    {
        CreateMainMenu();
    }
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        // set the graph link according to language and selected variable!
        DateTime curDate = DateTime.Now.Date;

        // in summer months, select water stage as the primary variable for links
        //int curMonth = curDate.Month;
        //if ( curMonth > 5 & curMonth < 11 )
        //{
        //    varEnum = VariableEnum.Stage;
        //}

        //VariableInfo varInfo = new VariableInfo(varEnum);
        //link_graphs.NavigateUrl = string.Format("~/{0}/{1}/", varInfo.UrlLang, varInfo.Url);
        //link_stations.NavigateUrl = string.Format("~/{0}/{1}/{2}/",
        //varInfo.UrlLang, Resources.global.Url_Stations, varInfo.Url);
        // set current time in footer!
        lbl_CurrentTime.Text = DateTime.Now.AddDays(-1).ToShortDateString();

        // bind navigation menu control to menuList
        rpt_navigace.DataSource = menuList;
        rpt_navigace.DataBind();
  
    }

    // create the main site navigation!
    private void CreateMainMenu()
    {
        menuList = new NavMenuList();
        
        menuList.Add("link_start", GetLocalResourceObject("link_start.NavigateUrl").ToString(), 
            GetLocalResourceObject("link_start.Text").ToString(), "current");
        menuList.Add("link_graphs", GetLocalResourceObject("link_graphs.NavigateUrl").ToString(),
            GetLocalResourceObject("link_graphs.Text").ToString());
        menuList.Add("link_stations", GetLocalResourceObject("link_stations.NavigateUrl").ToString(),
            GetLocalResourceObject("link_stations.Text").ToString());
        menuList.Add("link_about", GetLocalResourceObject("link_about.NavigateUrl").ToString(),
            GetLocalResourceObject("link_about.Text").ToString());
        menuList.Add("link_language", GetLocalResourceObject("link_language.NavigateUrl").ToString(),
            GetLocalResourceObject("link_language.Text").ToString(),
            GetLocalResourceObject("link_language.CssClass").ToString());
    }

    

    #region public members
    public System.Globalization.CultureInfo Culture
    {
        set 
        {      
            System.Threading.Thread.CurrentThread.CurrentCulture = value;
            System.Threading.Thread.CurrentThread.CurrentUICulture = value;
        }
    }
    public string PageTitle
    {
        set { head_title.Text = value; }
    }

    public string MetaLanguage
    {
        set { meta_language.Text = value; }
    }
    public string MetaDescription
    {
        set { meta_description.Text = value; }
    }
    public string MetaKeywords
    {
        set { meta_keywords.Text = value; }
    }

    /// <summary>
    /// specifies the current (active) selected item in main navigation menu
    /// </summary>
    public string ActiveMenuItem
    {
        set
        {
            menuList.SetCurrentItem(value);
        }
    }

    /// <summary>
    /// sets the 'graphs' navigation menu item to a specific URL
    /// </summary>
    
    public string GraphUrl
    {
        get
        {
            return menuList["link_graphs"].Url;
        }

        set
        {
            menuList["link_graphs"].Url = value;
        }
    }

    public string LanguageUrl
    {
        get
        {
            return menuList["link_language"].Url;
        }
        
        set
        {
            menuList["link_language"].Url = value;
        }

    }
    
    #endregion


    #region private
    private NavMenuList menuList;
    #endregion
}
