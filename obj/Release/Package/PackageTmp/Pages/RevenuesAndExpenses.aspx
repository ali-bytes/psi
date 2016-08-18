<%@ Page Title="<%$ Resources:Tokens,RevenuesAndExpenses %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RevenuesAndExpenses.aspx.cs" Inherits="NewIspNL.Pages.RevenuesAndExpenses" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-left: auto; margin-right: auto; padding: 15px;">
        <div class="view">
            <asp:Panel ID="p_options" runat="server" >
                <div class="page-header"><h1><asp:Literal ID="liter3" runat="server" Text="<%$ Resources:Tokens,SearchOptions%>"></asp:Literal></h1></div>
                <div class="well">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_from" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_from"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tb_from"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tb_to" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_to"
                                                        Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="tb_to"
                                                  Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                                  Type="Date"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                            </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                            <asp:Button ID="b_search" CssClass="btn btn-success" runat="server" OnClick="b_search_Click" Text="<%$ Resources:Tokens,Search %>" />
                        </td>
                    </tr>
                </table>
                </div>
                <div>
                </div>
            </asp:Panel>
            <asp:Panel ID="p_results" runat="server">
                <h3 class="header smaller lighter blue"><asp:Literal runat="server" ID="liter1" Text="<%$ Resources:Tokens,Results %>"></asp:Literal></h3>
                <div style="text-align: center">
                    <asp:GridView ID="gv_RevenueExpenses" runat="server" OnDataBound="gv_RevenueExpenses_DataBound"
                                  AutoGenerateColumns="False" 
                                  CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server" Text="Label"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TableId" HeaderText="<%$ Resources:Tokens,TableId %>"
                                Visible="False" />
                            <asp:BoundField DataField="BranchName" HeaderText="BranchName" />
                            <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount %>" />
                            <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:Tokens,Comment %>" />
                            <asp:BoundField DataField="Effect" HeaderText="<%$ Resources:Tokens,Revenue_Expense %>"/>
                            <asp:BoundField DataField="Date" HeaderText="<%$ Resources:Tokens,Date %>" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="CashBank" HeaderText="<%$ Resources:Tokens,Cash_Bank %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
        <div class="view">
            <asp:Panel runat="server" ID="p_report">
                <h3 class="header smaller lighter blue"><asp:Literal ID="lblliteral1" runat="server" Text="<%$ Resources:Tokens,Totals %>"></asp:Literal></h3>
                <div class="well">
                    <div class="alert alert-info">
                <div>
                    <asp:Label ID="t_Expenses" runat="server" Text="<%$ Resources:Tokens,RevenuesTotal %>"></asp:Label>
                    &nbsp;:&nbsp;
                    <asp:Label runat="server" ID="l_RevenuesTotal" Style="font-weight: 700"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="t_Revenues" runat="server" Text="<%$ Resources:Tokens,ExpensesTotal %>"></asp:Label>
                    &nbsp;:&nbsp;
                    <asp:Label runat="server" ID="l_ExpensesTotal" Style="font-weight: 700"></asp:Label>
                </div>
                </div>
                </div>
            </asp:Panel>
        </div>
    </div>
 
    <script>

        $(document).ready(function () {
            $('#tb_from').datepicker({ dateFormat: 'dd-mm-yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd-mm-yy' });
        });

    </script>
</asp:Content>
