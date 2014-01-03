<%@ Page Language="C#" MasterPageFile="~/grafy.master" AutoEventWireup="true" enableviewstate="false" 
         CodeFile="graf.aspx.cs" Inherits="Default2" %>
<%@ MasterType VirtualPath="~/grafy.master" %>
<%@ Reference Control="~/controls/statistics_window.ascx" %>


<asp:Content ID="main_content" runat="server" ContentPlaceHolderID="cph_main">

<h1><asp:Literal runat="server" ID="H1" Text="Display meteorological data (testing version)" /></h1>

<form id="form1" method="post" action="" runat="server" enableviewstate="false">
<fieldset>
<table>
<tr>
<td id="p_variable">    
    <label for="listbox_variable" >
        <asp:Literal runat="server" ID="lbl_VariableName" Text="<%$ Resources:global, Lbl_VariableName %>" /></label>
    <select id="listbox_variable" runat="server" tabindex="1" enableviewstate="false" />
</td>

<td>
    <label for="txt_station">
        <asp:Literal runat="server" ID="lbl_StationName" Text="<%$ Resources:global, Lbl_StationName %>" /></label>
    <input id="txt_station" type="text" size="27" runat="server" tabindex="2" enableviewstate="false"  />    
</td>

<td>
<asp:hyperlink ID="Link_StationList2" runat="server" Text="<%$ Resources:global, Link_StationList2 %>" tabindex="3" />
</td>

</tr>
    
<tr>
<td>
    <label for="listbox_year" >
        <asp:Literal runat="server" ID="lbl_start" Text="<%$ Resources:global, Lbl_Start %>" /></label><br />
    <select id="listbox_year" tabindex="5" runat="server" enableviewstate="false">
    </select>

    <select id="listbox_month" tabindex="6" runat="server" enableviewstate="false" >
    </select>    

    <select id="listbox_day" tabindex="7" runat="server" enableviewstate="false" >
    </select>
</td>

<td>
    <label for="listbox_year2" >
        <asp:Literal runat="server" ID="lbl_end" Text="<%$ Resources:global, Lbl_End %>" /></label><br />
    <select id="listbox_year2" tabindex="5" runat="server" enableviewstate="false">
    </select>

    <select id="listbox_month2" tabindex="6" runat="server" enableviewstate="false" >
    </select>    

    <select id="listbox_day2" tabindex="7" runat="server" enableviewstate="false" >
    </select>
</td>

    <td>
<span>
    <input id="Submit1" type="submit" value="<%$ Resources:global, Button_Submit %>" tabindex="8" runat="server"  enableviewstate="false" />
</span>
    </td>
</tr>
</table>
</fieldset>
</form>

<div id="graf">
    <img id="nplot_image" alt="" src="" runat="server" />
</div>

<!-- statistics window -->
<!--
<table id="statistics">
<tr><td>Pocet dnu se snehem:</td></tr>
<tr><td>Maximalni vyska snehu:</td></tr>
<tr><td>Dostupnost dat:</td></tr>
</table>
-->
<asp:PlaceHolder ID="placeholder_statistics" runat="server" />

<!--<div id="navigace">
    <h2><asp:Literal ID="heading_navigation" runat="server" Text="<%$ Resources:global, h2_Navigation %>" /></h2>
    <ul>
        <li><asp:HyperLink ID="link_start1" runat="server" NavigateUrl="<%$ Resources:global, Url_Start %>"
            Text="Start" /></li>
        <li><asp:HyperLink ID="link_graphs" runat="server" CssClass="current" 
            Text="<%$ Resources:global, link_Graphs %>" NavigateUrl="" /></li>
        <li><asp:HyperLink ID="link_stationlist1" runat="server" Text="<%$ Resources:global, Link_List_Of_Stations %>" /></li>          
        <li><asp:Hyperlink ID="link_about" runat="server" NavigateUrl="<%$ Resources:global, Url_About %>" 
            Text="<%$ Resources:global, Link_About %>" /></li>
        <li><asp:hyperlink id="link_language" runat="server" CssClass="<%$ Resources:global, CssClass_Language %>"
            Text="<%$ Resources:global, Link_Language %>" NavigateUrl="" /></li>
    </ul>
</div>-->

</asp:Content>

