<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGlobal.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SearchEngine.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="page-header text-center">
            <h1>Search Engine Project</h1>
        </div>
        <div style="max-width: 330px; margin: 0 auto;">
            <asp:TextBox Style="margin-top: 30px" CssClass="form-control" ID="TextBoxQuery" runat="server" placeholder="type your query"></asp:TextBox>
            <asp:Button Style="margin-top: 10px" CssClass="btn btn-primary btn-block" ID="ButtonGo" runat="server" Text="Go" OnClick="ButtonGo_Click" />
        </div>
    </div>
</asp:Content>
