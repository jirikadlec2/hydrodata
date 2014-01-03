using System;
using System.Globalization;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using jk.plaveninycz;
using jk.plaveninycz.DataSources;
using jk.plaveninycz.BO;
using jk.plaveninycz.Bll;

public partial class station_list : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {

        //Response.Cache.SetNoStore();
        
        string lang = "en";
        string cult = "en-US";
        VariableEnum varEnum = VariableEnum.Stage;
        //VariableInfo varInfo = new VariableInfo(varEnum);
        Variable var = VariableManager.GetItemByEnum(varEnum);
        if ( Context.Request.QueryString["lang"] != null )
        {
            lang = Context.Request.QueryString["lang"];
            switch ( lang )
            {
                case "cz":
                    cult = "cs-CZ";
                    break;
                default:
                    cult = "en-US";
                    break;
            }
        }
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cult);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cult);

        // this code is invoked, when the form is posted back.
        // (after pressing the "OK" button)
        // The page is then redirected to a different URL
        if ( Context.Request.Form.Count > 0 )
        {
            //set culture according to the language of request
            string requestPath = Context.Request.UrlReferrer.AbsolutePath;
            if ( requestPath.IndexOf("/cz/") >= 0 )
            {
                cult = "cs-CZ";
            }
            else
            {
                cult = "en-US";
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cult);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cult);
            
            //string ctlHeading = @"ctl00$cph_main$";
            string varName = Context.Request.Form[@"ctl00$cph_main$select_station_type"];
            string orderBy = Context.Request.Form[@"ctl00$cph_main$select_order"];

            if ( varName == "discharge" ) varName = "flow"; //special case..
            var = VariableManager.GetItemByShortName(varName);

            string varUrl = var.Url;
            int orderId = Convert.ToInt32(orderBy) + 1;

            string newUrl = String.Format(@"~/{0}/{1}/{2}/",
                var.UrlLang, Resources.global.Url_Stations, varUrl);

            if ( orderId > 1 )
            {
                newUrl = String.Format(@"{0}o{1}.aspx", newUrl, orderId);
            }
           
            // redirect the browser to the new page!
            Context.Response.Redirect(newUrl);
        }

    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // local variables
        VariableEnum varEnum = VariableEnum.Stage;
        Variable varInfo = VariableManager.GetItemByEnum(varEnum);
        string cultureStr = Thread.CurrentThread.CurrentCulture.ToString();
        int order = 1;
        DateTime t = DateTime.Now.Date;

        if ( !( Page.IsPostBack ) )
        {        
            // resolve query string parameters
            this.resolveVariable(varInfo);
            varEnum = varInfo.VarEnum;
            order = this.resolveOrder();
            // initialize control values and set page metadata
            this.initialize(varEnum);
            this.setMetaData(varEnum);
            // set value of select_station_type control
            this.selectVariable(varInfo);
            this.selectOrder(order);
            // create the table of stations!
            this.createStationTable(varInfo, order);

            // set the correct page title!
            Master.PageTitle = Resources.global.Link_List_Of_Stations + " - " + varInfo.Name;
            
            // set right url of the language hyperlink
            Master.LanguageUrl = CreateLanguageLink(varInfo);
        }
    }


    /// <summary>
    /// generates a correct URL to be used in the 'language' link
    /// </summary>
    /// <param name="varInfo"></param>
    /// <returns></returns>
    private string CreateLanguageLink(Variable varInfo)
    {
        string LangPath = Context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
        string LangPathStart;
        string lang = Thread.CurrentThread.CurrentCulture.ToString();
        string tempLang = lang; //the different culture string(to be changed)
        //resolve language of the link
        switch ( lang )
        {
            case "en-US":
                tempLang = "cs-CZ";
                break;
            case "cs-CZ":
                tempLang = "en-US";
                break;
            default:
                tempLang = "cs-CZ";
                break;
        }
        //temporarily change the culture
        CultureInfo pageCulture = Thread.CurrentThread.CurrentUICulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo(tempLang);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(tempLang);

        VariableManager.ChangeCulture(varInfo, Thread.CurrentThread.CurrentUICulture);
        LangPathStart = "/" + varInfo.UrlLang + "/" + Resources.global.Url_Stations + "/" + varInfo.Url + "/";

        //switch back to original culture
        Thread.CurrentThread.CurrentCulture = pageCulture;
        Thread.CurrentThread.CurrentUICulture = pageCulture;

        if ( LangPath.EndsWith("/default.aspx") )
        {
            LangPath = LangPath.Remove(LangPath.IndexOf("/default.aspx"));
        }

        //string LangPath2 = System.Text.RegularExpressions.Regex.Replace
        //(LangPath, "(/[czen]{2}/[a-z_-]+/[a-z_-]*)", LangPathStart, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if ( LangPath.EndsWith(".aspx"))
        {
            LangPathStart = LangPathStart + LangPath.Substring(LangPath.LastIndexOf("/"));
        }
        if ( !( LangPathStart.EndsWith(".aspx") || LangPathStart.EndsWith("/") ) )
        {
            LangPathStart = LangPathStart + "/";
        }
        return "~/" + LangPathStart;
    }


    // sets initial values to dropdownlists in the page form
    private void initialize(VariableEnum varEnum)
    {
        string cultStr = Thread.CurrentThread.CurrentCulture.ToString();

        SetNavigationMenu();

        string[] variableList;
        variableList = new string[4] { Resources.global.Var_Snow, Resources.global.Var_Precip, Resources.global.Var_Stage, Resources.global.Var_Discharge };
       
        int orderListCount = 4;
        Dictionary<int,string> orderList = new Dictionary<int,string>();

        for ( int i = 0; i < orderListCount; ++i )
        {
            // special case: recognize river/territory
            if ( i == 1 & ( varEnum == VariableEnum.Stage | varEnum == VariableEnum.Discharge ) )
            {
                orderList.Add(i, GetLocalResourceObject("StationOrderType_River").ToString());
            }
            else
            {
                orderList.Add(i, GetLocalResourceObject("StationOrderType_" + ( i + 1 ).ToString()).ToString());
            }
        }
        
        select_station_type.DataSource = variableList;
        select_station_type.DataBind();
        select_order.DataSource = orderList;
        select_order.DataTextField = "Value";
        select_order.DataValueField = "Key";
        select_order.DataBind();

        if ( varEnum == VariableEnum.Stage | varEnum == VariableEnum.Discharge )
        {
            th_location.Text = GetLocalResourceObject("LocationHeader2.Text").ToString();
        }
    }

    // set the active (current) menu item in master page
    // also create a correct language url link
    private void SetNavigationMenu()
    {
        Master.ActiveMenuItem = "link_stations";
        
        //Master.LanguageUrl = GetLocalResourceObject("LanguageLink.NavigateUrl").ToString();

    }

    private void resolveVariable(Variable varInfo)
    {
        string varName;
        if ( Request.QueryString["var"] != null )
        {
            varName = Request.QueryString["var"];
            varInfo = VariableManager.GetItemByName(varName);
        }
    }
    
    // returns the index indicating the order of
    // displayed stations
    private int resolveOrder()
    {
        int order = 1;
        if ( Request.QueryString["order"] != null )
        {
            order = Convert.ToInt32(Request.QueryString["order"]);
        }
        return order;
    }

    // selects the variable in select_station_type DropDownList
    // also sets correct variable to page title, heading and graph
    // hyperlink
    private void selectVariable(Variable varInfo)
    {
        VariableEnum varEnum = varInfo.VarEnum;
        
        // first, set the table caption label
        string varName = varInfo.Name;
        LblCaption2.Text = varName;
        // second, set the main page heading
        lbl_h1.Text = GetLocalResourceObject("PageTitle1.Text").ToString() + ": " + varName;
        // third, set correct variable for the graph hyperlink
        Master.GraphUrl = Resources.global.Url_Graphs + "/" + varInfo.Url + "/";
        //link_graphs.NavigateUrl = Resources.global.Url_Graphs + "/" + varInfo.Url + "/";
        // third, select the right index in the variable listbox
        int index = 0;
        switch ( varEnum )
        {
            case VariableEnum.Snow:
                index = 0;        
                break;
            case VariableEnum.Precip:
                index = 1;          
                break;
            case VariableEnum.Stage:
                index = 2;   
                break;
            case VariableEnum.Discharge:
                index = 3;
                break;
            default:
                break;
        }
        select_station_type.SelectedIndex = index;
    }
    
    // set the caption and headers of station table!
    //private void StationList_OnItemDataBound()

    // select the order in select_order DropDownList
    private void selectOrder(int order)
    {
        if ( order < 1 | order > 4 )
        {
            order = 1;
        }
        select_order.SelectedIndex = order - 1;
    }

    // the main function: fill the table of stations with data!
    private void createStationTable(Variable varInfo, int order)
    {
        int varId = varInfo.Id; 
        StationListDataSource.SelectParameters["variableId"].DefaultValue = varId.ToString();
        StationListDataSource.SelectParameters["orderBy"].DefaultValue = order.ToString();
    }


    // set the metadata in page header from local resource file!
    private void setMetaData(VariableEnum varEnum)
    {
        string varName = varEnum.ToString() + ".text";
        // meta_language
        Master.MetaLanguage = String.Format("<meta http-equiv='content-language' content='{0}' />",
            Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName); 
        // meta_description
        Master.MetaDescription = "<meta name='description' content='" + 
            GetLocalResourceObject("MetaDescription_" + varName) + "' />";
        // meta_keywords
        Master.MetaKeywords = "<meta name='keywords' content='" +
            GetLocalResourceObject("MetaKeywords_" + varName) + "' />";
    }
}
