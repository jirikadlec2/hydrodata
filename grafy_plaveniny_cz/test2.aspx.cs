using System;
using System.Web;

public partial class test2 : System.Web.UI.Page
{
    // tato udalost nastava pri kazdem zpracovani stranky...
    protected void Page_PreInit(object sender, EventArgs e)
    {
        // zakazeme ukladani do klientovy kese
        Response.Cache.SetNoStore();

        // overime, jestli uzivatel vyplnil formular a odeslal
        // stranku (obdoba testovani Page.IsPostBack)
        if ( Request.Form.Count > 0 )
        {
            if ( Request.Form["Textbox1"] != null )
            {
                // presmerujeme uzivatele na novou stranku s odlisnyn URL
                string newUrl = @"~/test2.aspx?text=" + Request.Form["Textbox1"];
                Response.Redirect(newUrl);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // nacteme hodnotu z dotazovaciho retezce
        string txt = Request.QueryString["text"];

        // uvodni zobrazeni stranky (prazdny dotazovaci retezec)
        if ( txt == null )
        {
            Label1.Visible = false;
            Page.Title = "Testování tlaèítka Zpìt - úvodní stránka";
        }
        else
        {
            // uzivatel nezadal zadny text
            if ( txt == "" )
            {
                txt = "Nezadali jste žádný text!";
                Label1.Text = txt;
                Label1.CssClass = "error";
                Page.Title = "Chyba: " + txt;
            }
            // uzivatel zadal text
            else
            {
                txt = Request.QueryString["text"];
                Label1.Text = "Zadali jste text: " + txt;
                Label1.CssClass = "ok";
                Page.Title = "Stránka pro zobrazení textu: " + txt;
                // Hodnotu v Textboxu musime nacist z dotazovaciho retezce,
                // jinak by byl Textbox prazdny
                TextBox1.Value = txt;
            }
        }
    }
}
