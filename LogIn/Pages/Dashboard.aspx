<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LogIn.Pages.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <h1>Welcome Back!</h1>
    <div class="User">
        <asp:Image ID="imgProfile" CssClass="imgProfile" runat="server"  />
        <asp:Label ID="lblUserName" runat="server" CssClass="user-name-label"></asp:Label>
    </div>
</asp:Content>

