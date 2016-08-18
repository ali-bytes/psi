<%@ Page Title="<%$Resources:Tokens,PPR %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PPR.aspx.cs" Inherits="NewIspNL.Pages.PPR" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.Requests %></h3>
            <asp:GridView ID="gv_customers" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                CssClass="table table-bordered table-condensed"
                OnDataBound="gv_customers_DataBound" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer %>" />
                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone %>" />
                    <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package %>"/>
                    <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Provider %>" />
                    <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status%>" />
                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governrate %>" />
                    <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                    <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                    <asp:BoundField DataField="Title" HeaderText="<%$Resources:Tokens,Offer %>"/>
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$Resources:Tokens,Request.Date %>" />
                    <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Total %>" />
                    <asp:BoundField DataField="Start" HeaderText="<%$Resources:Tokens,From %>" />
                    <asp:BoundField DataField="End" HeaderText="<%$Resources:Tokens,To %>" />
                    <asp:BoundField DataField="user" HeaderText="<%$Resources:Tokens,UserName %>" />
                    <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />
                </Columns>
                <EmptyDataTemplate>
                        <%= Tokens.NoResults %>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
        <div>
            <asp:LinkButton runat="server" ID="Export" CssClass="btn btn-primary" OnClick="Export_OnClick" ><i class="icon-file"></i>&nbsp;<%=Tokens.Export %></asp:LinkButton></div>
    </div>
</asp:Content>


