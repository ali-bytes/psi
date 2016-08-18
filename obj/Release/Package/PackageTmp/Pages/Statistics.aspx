<%@ Page Title="<%$Resources:Tokens,StatisticsInHome %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="NewIspNL.Pages.Statistics" %>

<%@ Import Namespace="Resources" %>

<%--<%@ Register Src="/WebUserControls/StatisticsChart.ascx" TagName="StatisticsChart" TagPrefix="StatisticsChart1" %>--%>
<%@ Register TagPrefix="uc1" TagName="StatisticsChart" Src="~/WebUserControls/StatisticsChart.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header"><h1><%=Tokens.StatisticsInHome%></h1></div>
    <div class="row">

        <div class="col-xs-12">
              <%--<StatisticsChart1:StatisticsChart ID="sc1" runat="server" Visible="True" CanEdit="True" />--%>
        
         <%--<uc1:StatisticsChart ID="sc2" runat="server" CanEdit="True" runat="server" Visible="True"/>--%>
            
             <uc1:StatisticsChart ID="StatisticsChart1" runat="server" CanEdit="True" runat="server" Visible="True"/>
        </div>
    </div>
</asp:Content>

