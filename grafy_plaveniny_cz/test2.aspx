<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Testovací stránka: tlaèítko zpìt</title>
    <link rel="stylesheet" type="text/css" href="styl_test.css" />
</head>
<body>
    <form id="form1" action="" method="post">
    <div>
        <label for="TextBox1">Zadej text:</label>
        <input type="text" id="TextBox1" runat="server" />
        <input type="submit" id="Button1" value="Odeslat!" runat="server" />
    </div>
    </form>
    <asp:Label ID="Label1" runat="server"></asp:Label>
</body>
</html>
