<%@ Page Title="<%$Resources:Tokens,CustomersInStatus%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomersInStatus.aspx.cs" Inherits="NewIspNL.Pages.CustomersInStatus" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <div>
        <div class="view">
            <asp:Panel ID="p_searchInput" runat="server">
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Options%>"></asp:Literal></h1></div>
                    <div class="row-fluid">
                        <div class="col-sm-6">
                           <div class="well">
                                               <div>
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Request.Type%>"></asp:Label></div>
                    <div>
                        <asp:DropDownList ID="ddl_requestsTypes" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="ddl_requestsTypes" InitialValue=" ">Select request type</asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,From%>"></asp:Label>
                    </div>
                    <div>
                        <div>
                            <asp:TextBox ID="tb_from" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_from"
                                ErrorMessage="Enter begining date"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_from"
                                Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Date"><%= Tokens.DatePeriodMsg %></asp:CompareValidator>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,To%>"></asp:Label></div>
                    <div>
                        <div>
                            <asp:TextBox ID="tb_to" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                ControlToValidate="tb_to"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="tb_to"
                                Display="Dynamic" ErrorMessage="Enter correct date format" Operator="DataTypeCheck"
                                Type="Date">*</asp:CompareValidator></div>
                    </div>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tb_to"
                        ControlToValidate="tb_from" Display="Dynamic" ErrorMessage="CompareValidator"
                        Operator="LessThanEqual" Type="Date"><%= Tokens.DatePeriodMsg %></asp:CompareValidator>
                           </div> 
                        </div>
                        <div class="col-sm-6">
                            <div class="well" style="height: 189px;">
                                <div>
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,Reseller %>"></asp:Literal>
                                    <div>
                                        <asp:DropDownList ID="ddl_Reseller" CssClass="chosen-select" runat="server" Width="180px">
                        </asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                            ControlToValidate="ddl_Reseller" InitialValue=" "></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div>
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,Service.Provider %>"></asp:Literal>
                                    <div>
                                        <asp:DropDownList ID="ddl_Provider" CssClass="chosen-select" runat="server" Width="180px">
                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                
                <p>
                    <asp:Button ID="b_search" CssClass="btn btn-primary" runat="server" Text="<%$Resources:Tokens,Search%>"
                        OnClick="b_search_Click" /></p>
            </asp:Panel>
        </div>
        <div class="view">
            <asp:Panel ID="Panel1" runat="server">
                <h3 class="header smaller lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,Results%>"></asp:Literal></h3>
                <div id="result-output">
                    <asp:GridView ID="gv_results" runat="server" CssClass="table table-bordered table-condensed "
                        AutoGenerateColumns="False" ClientIDMode="Static" OnDataBound="gv_results_DataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer%>" />
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package%>" />
                            <asp:BoundField DataField="Date" HeaderText="<%$Resources:Tokens,Date%>" />
                            <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Governorate%>" />
                            <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider%>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch%>" />
                            <asp:BoundField DataField="Resseller" HeaderText="<%$Resources:Tokens,Reseller%>" />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:Tokens,Status%>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div style="color: blue; font-weight: bold;">
                                <%= Tokens.NoResults %></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
    </div>
     <script>

         $(document).ready(function () {
             $(".chosen-select").chosen();
            $('#tb_from').datepicker({ dateFormat: 'dd/mm/yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>
</asp:Content>
