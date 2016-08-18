<%@ Page Title="<%$Resources:Tokens,TransferBtSavesReport%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="TransferBtSavesReport.aspx.cs" Inherits="NewIspNL.Pages.TransferBtSavesReport" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="view">
        <asp:Panel ID="p_search" runat="server">
            <div class="page-header">
                <h1>
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,TransferBtSavesReport %>"></asp:Literal></h1>
            </div>
            <div class="span6">
                <div class="well">
                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Status %>"></asp:Label>
                    <div>
                        <div>
                            <asp:DropDownList ID="ddl_status" runat="server" Width="200px">
                                <asp:ListItem Text="الكل" Value="1" />
                                <asp:ListItem Text="موافقة" Value="2" />
                                <asp:ListItem Text="رفض" Value="3" />
                                 <asp:ListItem Text="Not Confirmed" Value="4" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="l_from" runat="server" Text="<%$Resources:Tokens,From %>"></asp:Label>
                        <div>
                            <asp:TextBox ID="tb_from" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="tb_from"
                                Display="Dynamic" ErrorMessage="<%$Resources:Tokens,ValidDate %>" Operator="DataTypeCheck"
                                Type="Date"></asp:CompareValidator>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="l_to" runat="server" Text="<%$Resources:Tokens,To %>"></asp:Label>
                    </div>
                    <asp:TextBox ID="tb_to" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
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
                  <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                CssClass="table table-bordered table-condensed text-center" Style="margin-right: -18px;"
                DataKeyNames="Id"
                OnRowDataBound="grd_Requests_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:Label ID="lbl_No" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FromSave" HeaderText="<%$ Resources:Tokens,FromSave %>" />
                    <asp:BoundField DataField="ToSave" HeaderText="<%$ Resources:Tokens,ToSave %>" />
                    <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Tokens,Status %>" />
                     <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount %>" />
                    <asp:BoundField DataField="RequestMaker" HeaderText="<%$ Resources:Tokens,requestSender%>" />
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$ Resources:Tokens,RequestDate %>" />
                    <asp:BoundField DataField="RequestMakerNote" HeaderText="<%$ Resources:Tokens,RequestMakerNote %>" />
                     <asp:BoundField DataField="RequestConfirmer" HeaderText="<%$ Resources:Tokens,RequestConfirmer%>" />
                     <asp:BoundField DataField="ConfirmDate" HeaderText="<%$ Resources:Tokens,ConfirmDate%>" />
                     <asp:BoundField DataField="ConfirmerNote" HeaderText="<%$ Resources:Tokens,ConfirmerNote %>" />
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoRequests%>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
            </div>
           <%-- <div class="well">
                <span>
                    <asp:Label ID="label" runat="server" Text="<%$Resources:Tokens,Total %>"></asp:Label></span>&nbsp;
                <span>
                    <asp:Label ID="l_total" runat="server" Text=""></asp:Label></span>
            </div>--%>
            <p>
                <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="<%$Resources:Tokens,Export %>"
                    OnClick="Button1_Click" Width="90px"/>
            </p>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tb_from').datepicker({ dateFormat: 'dd/mm/yy' });
            $('#tb_to').datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>
</asp:Content>
