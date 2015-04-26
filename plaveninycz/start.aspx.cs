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

public partial class Default2 : System.Web.UI.Page
{
    
    protected override void InitializeCulture()
    {
        string lang = "en";
        string cult = "en-US";
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
        //Master.Culture = cult;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // set the page body user control (start.ascx) to Czech or English!
        string UserControlName = GetLocalResourceObject("UserControlName.Text").ToString();
        UserControl uc = (System.Web.UI.UserControl) LoadControl(UserControlName);
        PlaceHolder1.Controls.Add(uc);

        // set page metadata!
        setMetaData();
    }

    // set the metadata and title in Masterpage header from local resource file!
    private void setMetaData()
    {
        
        // meta_language
        Master.MetaLanguage = String.Format("<meta http-equiv='content-language' content='{0}' />",
            Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        // meta_description
        Master.MetaDescription = "<meta name='description' content='" +
            GetLocalResourceObject("MetaDescription") + "' />";
        // meta_keywords
        Master.MetaKeywords = "<meta name='keywords' content='" +
            GetLocalResourceObject("MetaKeywords") + "' />";
        // page title
        Master.PageTitle = GetLocalResourceObject("PageTitle").ToString();
    }

    
}
