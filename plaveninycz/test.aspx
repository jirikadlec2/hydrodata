<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Testovací stránka: tlaèítko zpìt</title>
    <link rel="stylesheet" type="text/css" href="styl_test.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label for="TextBox1">Zadej text:</label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1"
            runat="server" Text="Zobrazit!" />
    </div>
    </form>
    <asp:Label ID="Label1" runat="server" CssClass="error" Text="Nezadali jste žádný text."></asp:Label>
</body>
</html>
