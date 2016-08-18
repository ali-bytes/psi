<%@ Page Title="<%$Resources:Tokens,SearchAboutResellerInvoice %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SearchAboutInvoice.aspx.cs" Inherits="NewIspNL.Pages.SearchAboutInvoice" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,SearchAboutResellerInvoice %>" runat="server" /></h1></div>
            <div class="well">
                <label for="txtInvoiceNumber"><%=Tokens.InvoiceNumber %></label>
                <div>
                    <asp:TextBox runat="server" ID="txtInvoiceNumber" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtInvoiceNumber"
                                                ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                </div>
                <p>
                    <br/>
                    <asp:Button runat="server" Text="<%$Resources:Tokens,Search %>" ID="btnSearch" CssClass="btn btn-success" 
                        onclick="btnSearch_Click" ClientIDMode="Static"/>
                </p>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Invoice %>" runat="server" /></h3>
            <div>
                <asp:GridView runat="server" ID="GvHistory" CssClass="table table-bordered table-condensed text-center"
                    AutoGenerateColumns="False" OnDataBound="GvHistory_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,InvoiceNumber %>"/>
                        <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company%>"/>
                        <asp:BoundField DataField="ClientName" HeaderText="<%$Resources:Tokens,Customer.Name %>" />
                        <asp:BoundField DataField="ClientTelephone" HeaderText="<%$Resources:Tokens,Customer.Phone %>"/>
                        <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Date %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        <asp:BoundField DataField="RejectReason" HeaderText="<%$Resources:Tokens,RejectReason %>"/>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,RequestStatus %>">
                            <ItemTemplate>
                              <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("IsApproved") %>' ClientIDMode="Static"></asp:Label>  
                            </ItemTemplate>
                        </asp:TemplateField>
                      
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <%= Tokens.NoResults %></div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
                            <script type="text/javascript">
                                $(document).ready(function () {
                                    $('#txtInvoiceNumber').keypress(function (e) {
                                        var key = e.which;
                                        if (key == 13) {
                                            $('#btnSearch').click();
                                            return false;
                                        } else {
                                            return true;
                                        }
                                    });
                                });
    </script>
</asp:Content>


