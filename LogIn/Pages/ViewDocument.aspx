<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDocument.aspx.cs" Inherits="LogIn.Pages.ViewDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Image ID="imgDocument" runat="server" Visible="false" />
    <asp:Literal ID="ltPDFViewer" runat="server" Visible="false"></asp:Literal>
</asp:Content>
