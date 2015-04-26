<%@ Page Language="C#" MasterPageFile="~/grafy.master" AutoEventWireup="true" CodeFile="bingmap.aspx.cs" 
enableviewstate="false" Inherits="bingmap" %>
<%@ MasterType VirtualPath="~/grafy.master" %>


<asp:Content ID="main_content" runat="server" ContentPlaceHolderID="cph_main">

<h1>Bing Map Test</h1>
<form id="form1" action="" runat="server">

<asp:ListBox ID="ListBox1" runat="server" Rows="1">
    <asp:ListItem>Snow</asp:ListItem>
    <asp:ListItem>Precipitation</asp:ListItem>
    <asp:ListItem>Discharge</asp:ListItem>
</asp:ListBox>

<asp:Button ID="button1" runat="server" Text="Map Point Test.." 
    onclick="button1_Click" />

<input type="button" ID="button2" value="load maps"  runat="server" onclick="LoadPushPins();"/>
</form>
<div id="bing_map_1" style="position:relative; width:600px; height:400px;"></div>

</asp:Content>
