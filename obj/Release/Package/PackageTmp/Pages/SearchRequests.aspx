<%@ Page Title="<%$ Resources:Tokens,SearchRequests %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SearchRequests.aspx.cs" Inherits="NewIspNL.Pages.SearchRequests" %>




<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:Panel ID="p_searchInput" runat="server">
            <div class="page-header"><h1><asp:Literal ID="lblsearch" runat="server" Text="<%$Resources:Tokens,SearchOptions %>"></asp:Literal></h1></div>
            <div class="well">
                <div>
                    <label for="DdlUser">
                        <%= Tokens.User %></label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlUser" Width="178px">
                        </asp:DropDownList>
                    </div>
                </div>
                <div>
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Request.Type %>"></asp:Label>
                    <div>
                        <asp:DropDownList ID="ddl_requestsTypes" runat="server" Width="178px">
                        </asp:DropDownList>
                    </div>
                </div>
                <div>
                    <asp:Label runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlReseller" Width="178px"/>
                    </div>
                </div>
                <div>
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
                    <div>
                        <div>
                            <asp:TextBox ID="tb_from" data-select="dp" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                ControlToValidate="tb_from"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_from"
                                Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                Type="Date"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To %>"></asp:Label>
                    <div>
                        <div>
                            <asp:TextBox ID="tb_to" runat="server" data-select="dp"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                ControlToValidate="tb_to"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="tb_to"
                                Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                Type="Date"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tb_to"
                                ControlToValidate="tb_from" Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,DatePeriodMsg %>"
                                Operator="LessThan" Type="Date"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
                 <div class="checkbox" id="ShowPortalStatus" runat="server" style="padding-right: 0;" >
                        <label>
                            <input type="checkbox" class="ace" runat="server" id="CkShowPortalStatus" /><span class="lbl">&nbsp;<%= Tokens.ShowPortalStatus %></span></label>
                    </div>
            </div>
            <p>
                <asp:Button ID="b_search" runat="server" Text="<%$ Resources:Tokens,Search %>" OnClick="b_search_Click"
                    Width="90px" CssClass="btn btn-success" /></p>
        </asp:Panel>
    </div>
    <div id="result" class="view">
        <asp:Panel runat="server">
            <h3 class="header smaller lighter blue"><asp:Literal runat="server" ID="lblres" Text="<%$ Resources:Tokens,Results %>"></asp:Literal></h3>
            <div id="result-output">
                <asp:GridView ID="gv_results" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                    CssClass="table table-bordered table-condensed text-center"
                    OnDataBound="gv_results_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Name %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="Request" HeaderText="<%$ Resources:Tokens,Request %>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider %>" />
                   
                        <asp:BoundField DataField="newspeed" HeaderText="<%$ Resources:Tokens,New.Service.Package %>" />
                             <asp:BoundField DataField="newip" HeaderText="<%$ Resources:Tokens,New.IP.Package %>" />
                        <asp:BoundField DataField="extragiga" HeaderText="<%$ Resources:Tokens,Extra.Gigas %>" />
                        <asp:BoundField DataField="RequestDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Tokens,Request.Date %>" />
                         <asp:BoundField DataField="ProcessDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Tokens,ProcessDate %>" />
                        <asp:BoundField DataField="Resseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$ Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$ Resources:Tokens,User %>" />

                        <asp:BoundField DataField="status" HeaderText="<%$ Resources:Tokens,Status %>" />
                        <asp:BoundField DataField="PortalStatus" HeaderText="<%$ Resources:Tokens,PortalStatus %>" />
                        <asp:BoundField DataField="OrderStatus" HeaderText="<%$ Resources:Tokens,OrderStatus %>" />
                    </Columns>
                    <EmptyDataTemplate>
                            <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" />
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
 
    <script>

        $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });

    </script>
</asp:Content>
