<%@ Page Language="C#" MasterPageFile="~/grafy.master" AutoEventWireup="true" CodeFile="start.aspx.cs" Inherits="Default2" %>
<%@ MasterType VirtualPath="~/grafy.master" %>

<asp:Content ID="main_content" runat="server" ContentPlaceHolderID="cph_main">
<h1><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:global, Heading_Start %>"/></h1>
<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</asp:Content>


