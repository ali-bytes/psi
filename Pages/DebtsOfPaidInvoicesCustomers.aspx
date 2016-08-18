<%@ Page Title="<%$Resources:Tokens,DebtsOfPaidInvoicesCustomers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DebtsOfPaidInvoicesCustomers.aspx.cs" Inherits="NewIspNL.Pages.DebtsOfPaidInvoicesCustomers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
            <div>
                <fieldset>
                    <div class="page-header"><h1>
                        <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Unpaid %>" runat="server" /></h1></div>
                    <div>
                        <asp:GridView runat="server" ID="GvUnpaid" AutoGenerateColumns="False" OnDataBound="GvUnpaid_OnDataBound"
                            CssClass="table table-bordered table-condensed">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer%>" />
                                <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                                <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                                <asp:BoundField DataField="From" HeaderText="<%$Resources:Tokens,From%>" />
                                <asp:BoundField DataField="To" HeaderText="<%$Resources:Tokens,To%>" />
                                <asp:BoundField DataField="ForMonth" HeaderText="<%$Resources:Tokens,Month%>" />
                                <asp:BoundField DataField="ForYear" HeaderText="<%$Resources:Tokens,Year%>" />
                                <%--<asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />--%>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes%>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Notes") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount%>" />
                                <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User%>" />
                                <asp:BoundField DataField="PaymentComment" HeaderText="<%$Resources:Tokens,Comment %>"/>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <button class="btn btn-success btn-sm" id="confirmPay" type="button"
                                            runat="server" CausesValidation="False" data-app="cancelPay" title='<%$Resources:Tokens,Pay %>'
                                             data-id='<%#Eval("Id") %>' data-rel="tooltip"><i class="icon-dollar icon-only"></i></button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
            <asp:HiddenField runat="server" ID="hdfId" ClientIDMode="Static"/>
                    <div id="cancelPayModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
                          <div class="modal-dialog">
            <div class="modal-content">
                            <div class="modal-header">
            <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h4 id="myModalLabel">
                <%= Tokens.Pay %></h4>
        </div>
                            <div class="modal-body">
                                <div class="bootbox-body">
            <div class="well">
                <div>
                    <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlSavesPay" DataTextField="SaveName" DataValueField="Id" ValidationGroup="cancelp"/>
                    <asp:RequiredFieldValidator ControlToValidate="ddlSavesPay" ValidationGroup="cancelp" ID="RequiredFieldValidator15" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            </div>
        </div>
                            <div class="modal-footer">
            <asp:Button runat="server" ID="btnRecieveFromCompany" ValidationGroup="cancelp" 
                CssClass="btn btn-primary" Text="<%$Resources:Tokens,Save %>" 
                onclick="ConfirmPayDemand"/>
            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
        </div>
        </div>
        </div>
                </div>
 
                <script type="text/javascript">
                    $(function () {
                        $('button[data-app="cancelPay"]').click(function () {
                            var id = $(this).attr('data-id');
                            $('#hdfId').val(id);
                            $('#cancelPayModal').modal('show');
                        });
                    });
                </script>
</asp:Content>

