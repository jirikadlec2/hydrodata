using System;
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class about : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {

        //Response.Cache.SetNoStore();

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
    }
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // set the language and other links!
        string UserControlName = GetLocalResourceObject("UserControlName.Text").ToString();
        UserControl uc = (System.Web.UI.UserControl) LoadControl(UserControlName);
        PlaceHolder1.Controls.Add(uc);
        setMetaData();
        setNavigationMenu();
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

    // set the active (current) menu item in master page
    // also create a correct language url link
    private void setNavigationMenu()
    {
        Master.ActiveMenuItem = "link_about";
        Master.LanguageUrl = GetLocalResourceObject("LanguageLink.NavigateUrl").ToString();
        
    }

    
}
