<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="db_update.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="label1" runat="server" />
<asp:DataGrid runat="server" id="grid1" Font-Name="Verdana"
    AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee"
    HeaderStyle-BackColor="Navy" HeaderStyle-ForeColor="White"
    HeaderStyle-Font-Size="15pt" HeaderStyle-Font-Bold="True">
  <Columns>
    <asp:HyperLinkColumn DataNavigateUrlField="Url" DataTextField="Name" 
           HeaderText="File Name" />
    <asp:BoundColumn DataField="LastWriteTime" HeaderText="Last Write Time"
        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
    <asp:BoundColumn DataField="Length" HeaderText="File Size"
		ItemStyle-HorizontalAlign="Right" 
		DataFormatString="{0:#,### bytes}" />
  </Columns>
</asp:DataGrid> 
    </div>
    </form>
</body>
</html>
