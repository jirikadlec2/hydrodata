<%@ OutputCache Duration="300" VaryByParam="*" %>
<%@ Page Language="C#" MasterPageFile="~/grafy.master" AutoEventWireup="true" CodeFile="station_list.aspx.cs" 
enableviewstate="false" Inherits="station_list" %>
<%@ MasterType VirtualPath="~/grafy.master" %>


<asp:Content ID="main_content" runat="server" ContentPlaceHolderID="cph_main">

<h1><asp:Literal runat="server" id="lbl_h1" /></h1>

<form id="form2" method="post" action="">
<fieldset>
<div>
    <span>
    <label id="lbl_station_type" for="select_station_type">
        <asp:Literal runat="server" id="Literal2" text="<%$ Resources:global, Lbl_VariableName %>" />
        <select id="select_station_type" runat="server">
            <option>sníh</option>
            <option>srážky</option>
            <option>vodní stav</option>
            <option>prùtok</option>
        </select>
    </label>
    </span>

    <span>
    <label id="lbl_order" for="select_order">
        <asp:Literal runat="server" id="Literal1" text="<%$ Resources:global, Lbl_SelectOrder %>" />    
    <select id="select_order" tabindex="1" runat="server">
    </select>
    </label>
    </span>
    
    <span>
    <input id="Submit2" type="submit" value="<%$ Resources:global, Button_Submit %>" tabindex="3" runat="server"  />
    </span>
</div>
</fieldset>
</form>

<table summary="Seznam stanic, ktere meri snih">
<caption><asp:Literal id="LblCaption1" runat="server" text="<%$ Resources:global, Lbl_Caption1 %>" /><asp:Literal ID="LblCaption2" runat="server" /></caption>
<tr>
<th><asp:Literal ID="th_name" text="DefaultText" runat="server" meta:resourcekey="StationListHeader" /></th>
<th><asp:Literal ID="th_location" text="DefaultText" runat="server" meta:resourcekey="LocationHeader1" /></th>
<th><asp:Literal ID="th_elevation" text="DefaultText" runat="server" meta:resourcekey="ElevationHeader" /></th>
<th><asp:Literal ID="th_time" text="DefaultText" runat="server" meta:resourcekey="TimeHeader" /></th>
</tr>

<asp:Repeater ID="StationList" runat="server" DataSourceID="StationListDataSource">
<ItemTemplate>
<tr>
<td><a href="<%#Eval("Url")%>"><%#Eval("Name")%></a></td>
<td><%#Eval("Location")%></td>
<td><%#Eval("ElevationString")%></td>
<td><%#Eval("ObservationPeriodString")%></td>
</tr>
</ItemTemplate>
</asp:Repeater>
</table>

<asp:ObjectDataSource ID="StationListDataSource" runat="server" 
TypeName="jk.plaveninycz.Geo.StationDetails" SelectMethod="LoadByVariable" >
    <SelectParameters>
        <asp:Parameter DefaultValue="2" Name="variableId" Type="Int32" />
        <asp:Parameter DefaultValue="1" Name="orderBy" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

</asp:Content>


