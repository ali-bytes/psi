<%@ Page Title="<%$Resources:Tokens,TicketsReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="TicketsReport.aspx.cs" Inherits="NewIspNL.Pages.TicketsReport" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <div>
            <div>
                <fieldset>
                    <div class="page-header">
                        <h1><asp:Literal Text="<%$Resources:Tokens,TicketsReport %>" runat="server" /></h1>
                    </div>
                    <div class="well">
                        <div>
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,From %>"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txt_Date1" runat="server"  data-x="dt" ClientIDMode="Static"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,ValidDate %>"
                                ControlToValidate="txt_Date1" ValidationGroup="vg_ViewTicket" Operator="DataTypeCheck"
                                Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Date1"
                                ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="vg_ViewTicket"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,To%>"></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="txt_Date2" runat="server" data-x="dt" ClientIDMode="Static"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,ValidDate %>"
                                ControlToValidate="txt_Date2" ValidationGroup="vg_ViewTicket" Operator="DataTypeCheck"
                                Type="Date"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Date2"
                                ErrorMessage="<%$ Resources:Tokens,ValidDate %>" ValidationGroup="vg_ViewTicket"></asp:RequiredFieldValidator>
                        </div>
                        <p>
                            <br/>
                            <asp:LinkButton ID="btn_ViewTicket" runat="server"
                                ValidationGroup="vg_ViewTicket" Width="100px" OnClick="btn_ViewTicket_Click" CssClass="btn btn-success"><i class="icon-search"></i>&nbsp;<%=Tokens.Search %></asp:LinkButton>
                        </p>
                    </div>
                </fieldset>
            </div>
        </div>
        <div>
            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,TicketHistory%>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd_Tickets" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" 
                    CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>" />
                            <asp:BoundField DataField="Title" HeaderText="<%$ Resources:Tokens,Ticket.Reason %>" />
                            <asp:BoundField DataField="Details" HeaderText="<%$ Resources:Tokens,Details%>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                            <asp:BoundField HeaderText="<%$ Resources:Tokens,Comment %>" DataField="Comment" />
                            <asp:BoundField DataField="ProviderTicketNo" HeaderText="<%$ Resources:Tokens,TicketNo%>" />
                            <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,NoResults %>"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
                <asp:LinkButton runat="server" ID="btnExport" CssClass="btn btn-primary" OnClick="btnExport_click"><i class="icon-file-text-alt"></i>&nbsp;<%=Tokens.Export %></asp:LinkButton>
            </div>
        </div>
    </div>
 

    <script type="text/javascript">
        $(document).ready(function () {
         
            $('input[data-x="dt"]').datepicker({
                dateFormat: 'dd-mm-yy',
                showOtherMonths: true,
                selectOtherMonths: false
            });
        });
    </script>
</asp:Content>
