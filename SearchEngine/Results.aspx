<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGlobal.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="SearchEngine.Results" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="page-header">
            <h3 class="text-muted">Showing Search Results For: <%=Keyword %></h3>
            <asp:Label CssClass="text-info" ID="LabelTime" runat="server" Text=""></asp:Label>
        </div>
        <div style="margin-bottom:100px;">
            <%=Result %>
        </div>
    </div>
</asp:Content>
