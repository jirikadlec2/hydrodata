<%@ Control Language="C#" AutoEventWireup="true" CodeFile="statistics_window.ascx.cs" 
Inherits="jk.plaveninycz.Presentation.controls_statistics_window" %>
<div id="statistics">
<asp:repeater ID="repeater1" runat="server">
<ItemTemplate>
<p><%# ((jk.plaveninycz.Presentation.StatisticsItem)Container.DataItem).Label %>:&nbsp; 
   <%# ((jk.plaveninycz.Presentation.StatisticsItem)Container.DataItem).Value %></p>
</ItemTemplate>
</asp:repeater>
</div>
