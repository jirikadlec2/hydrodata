using System;
using System.Globalization;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using jk.plaveninycz;
using jk.plaveninycz.DataSources;
using jk.plaveninycz.Observations;
using jk.plaveninycz.Geo;
using jk.plaveninycz.Validation;
using jk.plaveninycz.Presentation;
using jk.plaveninycz.Web;

public partial class Default2 : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {       
        string lang = "en";
       
        if ( Context.Request.QueryString["lang"] != null )
        {
            lang = Context.Request.QueryString["lang"];
            switch ( lang )
            {
                case "cz":
                case "cs":
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs-CZ");
                    break;
                default:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    break;
            }
        }
        else
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
        
        // this code is invoked, when the form is posted back.
        // (after pressing the "OK" button)
        // The page is then redirected to a different URL
        if ( Context.Request.Form.Count > 0 )
        {
            Variable varInfo = new Variable(4);
            string ctlHeading = @"ctl00$cph_main$";
            string varName = Context.Request.Form[ctlHeading + "listbox_variable"];
            string stName = Context.Request.Form[ctlHeading + "txt_station"];
            
            //string periodStr = Context.Request.Form[ctlHeading + "listbox_period"];

            string yearStr = Context.Request.Form[ctlHeading + "listbox_year"];
            string monthStr = Context.Request.Form[ctlHeading + "listbox_month"];
            string dayStr = Context.Request.Form[ctlHeading + "listbox_day"];

            string yearStr2 = Context.Request.Form[ctlHeading + "listbox_year2"];
            string monthStr2 = Context.Request.Form[ctlHeading + "listbox_month2"];
            string dayStr2 = Context.Request.Form[ctlHeading + "listbox_day2"];

            string varUrl = ProcessVariable(varName, varInfo);
            string stUrl = ProcessStation(stName, varInfo);
            //string dateUrl = ProcessDate(yearStr, monthStr, dayStr);
            string periodUrl = ProcessPeriod(yearStr, monthStr, dayStr, yearStr2, monthStr2, dayStr2);

            // generate url of the new page!
            string newUrl =
                String.Format(@"~/{0}/{1}/{2}/{3}.aspx",
                lang, varUrl, stUrl, periodUrl);

            // redirect the browser to the new page!
            Context.Response.Redirect(newUrl);
        }
        
    }
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // local variables
        int DtDays = 365;
        DateTime endDate = DateTime.Now.Date;
        DateTime startDate = endDate.AddDays(-DtDays);
        
        string chartUrl;
        string chartTitle;

        if ( !( Page.IsPostBack ) )
        {        
            // initialize control values
            this.initialize();

            DataManager manager = new DataManager();

            // validate user input
            PageUrlValidator validator = new PageUrlValidator(Context.Request.QueryString, manager);
            validator.Validate();

            endDate = this.ResolveDate();
            startDate = this.SetPeriod(endDate);
            TimeInterval interval = new TimeInterval(startDate, endDate);

            //input is valid - get station and variable
            if ( validator.IsValidVariable )
            {
                SetupVariable(validator.Variable);
                // set values of StationListLink URL
                this.SetStationListUrl(validator.Variable);

                if ( validator.IsValidStation )
                {
                    SetupStation(validator.Station);
                }
            }

            //chart title..
            chartTitle = this.createChartTitle(validator, startDate, endDate);
            Master.PageTitle = chartTitle;

            // get station and variable (might not be valid)
            Variable varInfo = validator.Variable;
            Station stInfo = validator.Station;

            // set right url of the language hyperlink
            string LangPath = this.CreateLanguageLink(varInfo);
            Master.LanguageUrl = LangPath;

            // decide if we should use hourly or daily precipitation
            if ( varInfo.VarEnum == VariableEnum.Precip && interval.Length.TotalDays < 11)
            {
                
                if ( CheckHourlyPrecipAvailable(varInfo, stInfo, startDate, endDate, manager) )
                {
                    varInfo.VarEnum = VariableEnum.PrecipHour;
                }
            }

            // assign image url to the chart image
            chartUrl = this.createChartUrl(varInfo.ShortName, stInfo.StationId, startDate, endDate);
            nplot_image.Attributes["src"] = chartUrl;
            
            nplot_image.Attributes["alt"] = chartTitle;
            nplot_image.Attributes["title"] = chartTitle;


            // create the "statistics" window
            if ( validator.IsValid )
            {
                CreateStatistics(varInfo, stInfo, startDate, endDate, manager);
            }
        }
    }

    private bool CheckHourlyPrecipAvailable(Variable var, Station st, DateTime start, DateTime end, DataManager manager)
    {
        Variable tmpVar = new Variable(VariableEnum.PrecipHour);
        PeriodCollection periods = new PeriodCollection(manager);
        TimeInterval interval = new TimeInterval(start, end);
        periods.Load(st.StationId, tmpVar.Id, interval);
        if ( periods.Count > 0 )
        {
            return periods.HasObservations(interval);
        }
        else
        {
            return false;
        }
    }

    // obtains the variable code from Request.Form object
    // returns the variable URL, sets the right variable info
    private string ProcessVariable(string VariableName, Variable varInfo)
    {
        varInfo.Name = VariableName;
        if ( varInfo.IsValid )
        {
            return varInfo.Url;
        }
        else
        {
            return varInfo.Name;
        }
    }
    

    // processes the station name - creates readable station URL!
    private string ProcessStation(string stName, Variable var)
    {
        DataManager manager = new DataManager();

        //improved code - first try to load a valid station-variable combination
        Station st = new Station(manager);
        //st.Load

        StationCollection stColl = new StationCollection(manager);
        VariableEnum varEnum = var.VarEnum;
        int varId = var.Id;

        //stColl.LoadByName(stName);
        stColl.LoadByName(stName, var.Id); //try loading a valid station-variable combination

        Station StInfo = new Station(manager);
        int StId = StInfo.StationId;
        string StUri = StInfo.Url;

        if ( stColl.Count == 0 )
        {
            //try to load it only by name
            stColl.LoadByName(stName);
            StUri = stName.Replace(" ", "_");
        }

        if ( stColl.Count == 0 ) //still no stations loaded..No station exists
        {
            StUri = stName.Replace(" ", "_"); //this is in fact a non-existent uri
        }

        else
        {
            StInfo = stColl[0];
            StUri = StInfo.Url;
            if ( varEnum != VariableEnum.Discharge && varEnum != VariableEnum.Stage )
            {
                if ( StUri.IndexOf("/") >= 0 )
                {
                    StUri = StUri.Remove(0, StUri.IndexOf("/"));
                }
            }
        }
        return StUri;
    }

    // processes the date - ensures correct datatime computation
    private DateTime ProcessDate(string yearStr, string monthStr, string dayStr)
    {
        int SelYear = Convert.ToInt32(yearStr);
        int SelMonth = Convert.ToInt32(monthStr);
        
        int SelDay = Convert.ToInt32(dayStr);
        if ( SelDay > DateTime.DaysInMonth(SelYear, SelMonth) )
        {
            SelDay = DateTime.DaysInMonth(SelYear, SelMonth);
        }
        DateTime SelDate = new DateTime(SelYear, SelMonth, SelDay);
        return SelDate;
    }

    private string ProcessPeriod(string year1, string month1, string day1, string year2, string month2, string day2)
    {
        DateTime startDate = ProcessDate(year1, month1, day1);
        DateTime endDate = ProcessDate(year2, month2, day2);
        int periodDays = 1;

        if ( startDate > endDate )
        {
            DateTime tmpDate = startDate;
            startDate = endDate;
            endDate = tmpDate;
        }

        periodDays = (int) ( (TimeSpan) ( endDate.Subtract(startDate) ) ).TotalDays;

        string DateString = string.Format(@"{0}{1}{2}/{3}d",
            endDate.Year.ToString("0000"), endDate.Month.ToString("00"), endDate.Day.ToString("00"), periodDays);
        return DateString;
    }

    // sets initial values to dropdownlists in the page form
    // Culture string specifies the CultureInfo (cs-CZ, en-US,..)
    private void initialize()
    {
        // set the active (current) menu item in master page
        // also create a correct language url link
        setNavigationMenu();
        
        // initialize listbox_year
        int i = 0;
        int y1 = 2005;
        int y2 = DateTime.Now.Year;
        int numYears = y2 - y1 + 1;
        int[] yearList = new int[numYears];
        for ( i = 0; i < numYears; ++i )
        {
            yearList[i] = y2 - i;
        }
        listbox_year.DataSource = yearList;
        listbox_year.DataBind();

        listbox_year2.DataSource = yearList;
        listbox_year2.DataBind();

        // initialize listbox_month
        Dictionary<int, string> months = new Dictionary<int, string>(12);

        CultureInfo cult = Thread.CurrentThread.CurrentCulture;
        string[] monthNames = cult.DateTimeFormat.MonthNames;
        for ( i = 0; i < 12; ++i )
        {
            months.Add(i + 1, monthNames[i]);
        }
        listbox_month.DataSource = months;
        listbox_month.DataTextField = "Value";
        listbox_month.DataValueField = "Key";
        listbox_month.DataBind();

        listbox_month2.DataSource = months;
        listbox_month2.DataTextField = "Value";
        listbox_month2.DataValueField = "Key";
        listbox_month2.DataBind();

        // initialize listbox_day
        int[] dayList = new int[31];
        for ( i = 0; i < 31; ++i )
        {
            dayList[i] = i + 1;
        }
        listbox_day.DataSource = dayList;
        listbox_day.DataBind();

        listbox_day2.DataSource = dayList;
        listbox_day2.DataBind();

        string[] variableList;
        string CultureString = cult.ToString();
        switch ( CultureString )
        {
            //TODO: populate these values from a variable collection
            case "cs-CZ":
            variableList = new string[4] {"sníh","srážky","vodní stav","průtok"};
                break;
            default:
                variableList = new String[4] { "snow","precipitation","water stage","flow"};
                break;
        }
        listbox_variable.DataSource = variableList;
        listbox_variable.DataBind();

        // in case of no parameters provided, set controls to default values
        if ( Context.Request.QueryString["var"] == null )
        {
            listbox_variable.SelectedIndex = 2;
        }
        if ( Context.Request.QueryString["st"] == null )
        {
            txt_station.Value = @"Ústí nad Labem";
        }
        
        if ( Context.Request.QueryString["t"] == null )
        {
            listbox_year2.SelectedIndex = 0;
            listbox_month2.SelectedIndex = DateTime.Now.Month - 1;
            listbox_day2.SelectedIndex = DateTime.Now.Day - 1;
        }
        if ( Context.Request.QueryString["dt"] == null )
        {
            DateTime start = DateTime.Now.AddYears(-1);
            listbox_year.Value = start.Year.ToString();
            listbox_month.SelectedIndex = start.Month - 1;
            listbox_day.Value = start.Day.ToString();
        }

        // initialize the form's action attribute
        string path = Context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
        if ( path.EndsWith("default.aspx") )
        {
            form1.Attributes["action"] = "default.aspx";
        }
    }

    /// <summary>
    /// Setup vales of the 'variable' selection list
    /// </summary>    
    private void SetupVariable(Variable var)
    {   
        VariableEnum varEnum = var.VarEnum;
        int selIndex = 0;
        switch ( varEnum )
        {
            case VariableEnum.Snow:
                selIndex = 0;
                break;
            case VariableEnum.Precip:
                selIndex = 1;
                break;
            case VariableEnum.Stage:
                selIndex = 2;
                break;
            case VariableEnum.Discharge:
                selIndex = 3;
                break;
            default:
                selIndex = 3;
                break;
        }
        listbox_variable.SelectedIndex = selIndex;
    }

    /// <summary>
    /// Setup the 'station' input text box
    /// </summary>
    private void SetupStation(Station st)
    {
        if ( st.Loaded )
        {
            txt_station.Value = st.Name;
        }
        else
        {
            txt_station.Value = "unknown station";
        }
    }


    // creates the correct station list URL, given the variable enum
    private void SetStationListUrl(Variable varInfo)
    {
        string navUrl;
        navUrl = string.Format("~/{0}/{1}/{2}/", 
            varInfo.UrlLang, Resources.global.Url_Stations, varInfo.Url);
        
        link_stationlist1.NavigateUrl = navUrl;
        Link_StationList2.NavigateUrl = navUrl;
    }

    // sets up the statistics control
    private void CreateStatistics(Variable var, Station st, DateTime start, DateTime end, DataManager manager)
    {
        //TODO: eliminate double loading of periodCollection!!!
        PeriodCollection periods = new PeriodCollection(manager);
        periods.Load(var.Id, st.StationId);
        bool hasObservations = periods.HasObservations(new TimeInterval(start, end));

        if (hasObservations)
        {  
            MeasuredTimeSeries ts = TimeSeriesFactory.CreateTimeSeries(new Sensor(st, var, start, end));
            ts.LoadObservations(manager);
            Statistics stats = new Statistics(ts);
            
            controls_statistics_window statControl = (controls_statistics_window) LoadControl("~/controls/statistics_window.ascx");
            statControl.Statistics = stats;
            statControl.Variable = var;
            statControl.Update();
            placeholder_statistics.Controls.Add(statControl);
        }
    }

    // resolves the observation date (end of period) and
    // sets values in the date selection list boxes
    private DateTime ResolveDate()
    {
        DateTime t = DateTime.Now.Date;
        int curYr = t.Year;
        if ( Context.Request.QueryString["t"] != null )
        {
            string str = Context.Request.QueryString["t"];
            int year = Convert.ToInt32(str.Substring(0, 4));
            int month = Convert.ToInt32(str.Substring(4, 2));
            int day = Convert.ToInt32(str.Substring(6, 2));

            if ( year < 2005 ) year = 2005;
            if ( year > curYr ) year = curYr;
            if ( month < 1 ) month = 1;
            if ( month > 12 ) month = 12;
            if ( day < 1 ) day = 1;
            if ( day > DateTime.DaysInMonth(year,month) )
            {
                day = DateTime.DaysInMonth(year,month);
            }
            t = new DateTime(year, month, day);

            // set control values
            listbox_year2.Value = year.ToString();
            listbox_month2.SelectedIndex = month - 1;
            listbox_day2.Value = day.ToString();
        }
        return t;
    }

    // set the observation period length
    // returns the start date of observation period
    private DateTime SetPeriod(DateTime endDate)
    {
        string dt = "52w";
        int dtDays;

        if ( Context.Request.QueryString["dt"] != null )
        {
            dt = Context.Request.QueryString["dt"];
        }
        string dtNum = dt.Substring(0, dt.Length - 1);

        if ( dt.EndsWith("w") )
        {
            dtDays = Convert.ToInt32(dtNum) * 7;
        }
        else
        {
            dtDays = Convert.ToInt32(dtNum);
        }

        DateTime startDate = endDate.AddDays(-dtDays);
        listbox_year.Value = startDate.Year.ToString();
        listbox_month.SelectedIndex = startDate.Month - 1;
        listbox_day.Value = startDate.Day.ToString();

        return startDate;
    }

    // creates the chart url
    private string createChartUrl(string varStr, int st, DateTime start, DateTime end)
    {
        string chartBaseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["chart_url"];
        
        string lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        System.Text.StringBuilder chartUrl = new System.Text.StringBuilder(chartBaseUrl,32);
        chartUrl.AppendFormat(@"/{0}-{1}-{2}-{3}-{4}.ashx",
            lang,
            varStr, st.ToString("000"), 
            start.ToString("yyyyMMdd"),
            end.ToString("yyyyMMdd"));
            
        return chartUrl.ToString();
    }

    // creates the language url
    private string CreateLanguageLink(Variable varInfo)
    {
        string LangPath = Context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
        string LangPathStart;
        string lang = Thread.CurrentThread.CurrentCulture.ToString();
        
        switch ( lang )
        {
            case "en-US":
                varInfo.Culture = new CultureInfo("cs-CZ");
                LangPathStart = "/cz/" + varInfo.Url;
                break;
            case "cs-CZ":
                varInfo.Culture = new CultureInfo("en-US");
                LangPathStart = "/en/" + varInfo.Url;
                break;
            default:
                varInfo.Culture = new CultureInfo("cs-CZ");
                LangPathStart = "/cz/" + varInfo.Url;
                break;
        }
        
        string LangPath2 = System.Text.RegularExpressions.Regex.Replace
        (LangPath, "(/[czen]{2}/[a-z_-]+)", LangPathStart, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if ( LangPath2.EndsWith("/default.aspx") )
        {
            LangPath2 = LangPath2.Remove(LangPath2.IndexOf("/default.aspx"));
        }

        if ( !( LangPath2.EndsWith(".aspx") || LangPath2.EndsWith("/") ) )
        {
            LangPath2 = LangPath2 + "/";
        }
        return LangPath2;
    }

    // creates the string for page title and chart alternative text and meta-description
    private string createChartTitle
        (PageUrlValidator validator, DateTime start, DateTime end)
    {
        string varStr = "";
        string outStr = "";
        string noStationVariable = "";
        string errorMsg = "";
        CultureInfo cultInfo = Thread.CurrentThread.CurrentCulture;
        
        if ( validator.IsValidStation )
        {
            Station stInfo = validator.Station;
            Variable varInfo = validator.Variable;
            string stFullName = stInfo.Name;
            string stFullName2 = stInfo.Name;
            try
            {
                if (stInfo.River != null)
                {
                    if (stInfo.River.Name.Length > 0)
                    {
                        stFullName = stInfo.River + " - " + stFullName;
                        stFullName2 = stInfo.River + ", " + stFullName2;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                stFullName2 = stFullName;
                //this is an exception
            }
            varStr = varInfo.Name;

            //check if it's a valid station-variable combination
            
            if (!validator.IsValidStationVariable)
            {
                noStationVariable = ( cultInfo.Name == "cs-CZ" ? " (chybí mìøení)" : " (no data)" );
                errorMsg = validator.ErrorMessage;
            }

            outStr = String.Format(@"{0}: {1}, {2} - {3} {4}",
                    varStr, stFullName, start.ToString(cultInfo.DateTimeFormat.ShortDatePattern,
                    cultInfo.DateTimeFormat),
                    end.ToString(cultInfo.DateTimeFormat.ShortDatePattern,
                    cultInfo.DateTimeFormat),
                    noStationVariable);

            // create main page heading ...
            H1.Text = string.Format("{0} - {1} {2}", varStr, stFullName2, noStationVariable);

            string stMetaName = ( cultInfo.Name == "cs-CZ" ? "ve stanici" : "in" );
            string riverName = "";
            string riverMetaName = "";
            if ( varInfo.VarEnum == VariableEnum.Stage | varInfo.VarEnum == VariableEnum.Discharge )
            {
                if ( stInfo.River != null )
                {
                    riverName = stInfo.River.Name;
                    riverMetaName = ( cultInfo.Name == "cs-CZ" ? "na øece " : "on the river " ) + riverName;
                }
            }

            // add meta content-language tag...
            Master.MetaLanguage =
                String.Format("\n<meta http-equiv='content-language' content='{0}' />\n",
                cultInfo.TwoLetterISOLanguageName);

            // add the meta description ...
            Master.MetaDescription =
                String.Format("<meta name='description' content='{0} {3} {1} {2}' />\n",
                varStr, stMetaName, stInfo.Name, riverMetaName);

            // add meta keywords ...
            Master.MetaKeywords =
                String.Format("<meta name='keywords' content='{0}, {1}, {2}' />\n",
                varStr, stInfo.Name, riverName);
        }
        else if ( validator.IsValidVariable )
        {
            // UNKNOWN station - STATION NOT FOUND .......
            
            errorMsg = validator.ErrorMessage;
            // create main page heading ...
            varStr = validator.Variable.Name;
            // create main page heading ...
            string noStationFound = ( cultInfo.Name == "cs-CZ" ? " (stanice nebyla nalezena)" : " (no station found)" );
            H1.Text = string.Format("{0} {1}", varStr, noStationVariable);
            
            outStr = String.Format(@"{0}: {1} {2}",
                    varStr, validator.Station.Name, noStationFound);

            // add meta content-language tag...
            Master.MetaLanguage =
                String.Format("\n<meta http-equiv='content-language' content='{0}' />\n",
                cultInfo.TwoLetterISOLanguageName);

            // add the meta description ...
            Master.MetaDescription =
                String.Format("<meta name='description' content='{0} {1} {2}' />\n",
                varStr, validator.Station.Name, noStationFound);

            // add meta keywords ...
            Master.MetaKeywords =
                String.Format("<meta name='keywords' content='{0}, {1}' />\n",
                varStr, noStationFound);

        }
        else
        {
            H1.Text = "ERROR - Bad variable specified!";
            outStr = validator.ErrorMessage;
        }
        return outStr;
    }

    // set the active (current) menu item in master page
    // also create a correct language url link
    private void setNavigationMenu()
    {
        Master.ActiveMenuItem = "link_graphs";
        //Master.LanguageUrl = GetLocalResourceObject("LanguageLink.NavigateUrl").ToString();

    }
}
