<%@ Page Title="<%$Resources:Tokens,EmployeeIncome%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EmployeeIncome.aspx.cs" Inherits="NewIspNL.Pages.EmployeeIncome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:Panel ID="p_search" runat="server">
             <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,Search %>"></asp:Literal></h1></div>
            <div class="span6">
                <div class="well">
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Employee %>"></asp:Label>
                    <div>
                        <div>
                            <asp:DropDownList ID="ddl_employee" runat="server" Width="200px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                Display="Dynamic" ControlToValidate="ddl_employee"></asp:RequiredFieldValidator></div>
                    </div>
                    <div>
                        <asp:Label ID="l_from" runat="server" Text="<%$Resources:Tokens,From %>"></asp:Label>
                        <div>
                            <asp:TextBox ID="tb_from" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                Display="Dynamic" ControlToValidate="tb_from"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="tb_from"
                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                Type="Date"></asp:CompareValidator>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="l_to" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Label></div>
                    <asp:TextBox ID="tb_to" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                        Display="Dynamic" ControlToValidate="tb_to">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_to"
                        Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                        Type="Date">Enter valid date</asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="tb_from"
                        ControlToValidate="tb_to" Display="Dynamic" ErrorMessage="<%$Resources:Tokens,DatePeriodMsg %>"
                        Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                </div>
                <p>
                    <asp:Button ID="b_show" CssClass="btn btn-success" runat="server" Text="<%$Resources:Tokens,Show %>"
                        Width="90px" OnClick="b_show_Click" />
                </p>
            </div>
        </asp:Panel>
    </div>
    <div class="view">
        <asp:Panel runat="server" ID="p_results">
            <h3 class="header smaller lighter blue"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Tokens,Results %>"></asp:Literal></h3>
            <div runat="server" style="text-align: center" id="Print">
                <asp:GridView ID="gv_payments" runat="server" CssClass="table table-bordered table-condensed"
                    OnDataBound="gv_payments_DataBound" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="l_no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="Governorate" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <%--<asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status%>" />--%>
                        <asp:BoundField DataField="Provider" HeaderText="<%$ Resources:Tokens,Provider %>" />
                        <%--<asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller %>" />--%>
                        <asp:BoundField DataField="Central" HeaderText="<%$ Resources:Tokens,Central%>" />
                        <asp:BoundField DataField="Offer" HeaderText="<%$ Resources:Tokens,Offer %>" />
                        <asp:BoundField DataField="TStart" HeaderText="<%$ Resources:Tokens,From %>" />
                        <asp:BoundField DataField="TEnd" HeaderText="<%$ Resources:Tokens,To%>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$ Resources:Tokens,Notes%>" />
                        <asp:BoundField DataField="PaymentComment" HeaderText="<%$ Resources:Tokens,Comment%>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount%>" />
                        <%--<asp:BoundField DataField="Discount" HeaderText="<%$ Resources:Tokens,Discount%>" />--%>
                        <%--<asp:BoundField DataField="Net" HeaderText="<%$ Resources:Tokens,Net%>" />--%>
                    </Columns>
                    <EmptyDataTemplate>
                        <p>
                            <asp:Literal runat="server" ID="gv_l_message" Text="<%$Resources:Tokens,NoResults %>"></asp:Literal></p>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <div class="well">
                <span>
                    <asp:Label ID="label" runat="server" Text="<%$Resources:Tokens,Total %>"></asp:Label></span>&nbsp;
                <span>
                    <asp:Label ID="l_total" runat="server" Text=""></asp:Label></span>
            </div>
            <p>
                <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="<%$Resources:Tokens,Export %>"
                    OnClick="Button1_Click" Width="90px"/>
            </p>
        </asp:Panel>
    </div>

    <script>
        $(document).ready(function () {
            $('#tb_from').datepicker({ dateFormat: 'dd/mm/yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>
</asp:Content>
