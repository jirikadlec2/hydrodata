<%@ Page Language="C#" MasterPageFile="~/grafy.master" AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="about" %>
<%@ MasterType VirtualPath="~/grafy.master" %>

<asp:Content ID="main_content" runat="server" ContentPlaceHolderID="cph_main">
<h1><asp:Literal ID="heading" runat="server" meta:resourcekey="heading" /></h1>
<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</asp:Content>
