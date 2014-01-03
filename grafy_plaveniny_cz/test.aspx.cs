using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( TextBox1.Text != "" )
        {
            Label1.Text = "Zadali jste text: " + TextBox1.Text;
            Label1.CssClass = "ok";
        }
        else
        {
            Label1.Text = "Nezadali jste žádný text!";
            Label1.CssClass = "error";
        }
    }
}
