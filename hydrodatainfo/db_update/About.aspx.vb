Public Class About
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        label1.Text = Server.MapPath("/Radar/")
    End Sub

End Class