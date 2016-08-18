<%@ Page Title="<%$Resources:Tokens,BranchDailyReport%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchDailyReport.aspx.cs" Inherits="NewIspNL.Pages.BranchDailyReport" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
    <div class="view">
        <fieldset>
            <div class="page-header">
                <h1><%= Tokens.BranchDailyReport%></h1></div>
            <div class="well">
                <div>
                    <div>
                        <label for="TbDate">
                            <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Date %>" runat="server" />
                        </label>
                        <div>
                            <asp:TextBox runat="server" ID="TbDate" data-select="dp"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RTDate" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                        ControlToValidate="TbDate" runat="server" />
                        </div>
                    </div>
                    <div>
                        <label for="DdlReseller">
                            <%= Tokens.Branch %></label>
                        <div>
                            <asp:DropDownList runat="server" ID="DdlBranch">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RddlBranch" ErrorMessage="*" ControlToValidate="DdlBranch"
                                                        runat="server" />
                        </div>
                    </div>
                    <br/>
                    <button id="Button1" class="btn btn-success" runat="server" onserverclick="Search">
                        <i class="icon-search icon-only"></i>&nbsp;<%= Tokens.Search %></button>
                </div>
            </div>
        </fieldset>
    </div>
    <div id="divData">
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.CustomersPayments %>
                </h3>
                <div>
                    <asp:GridView runat="server" ID="GvCustomers" OnDataBound="BindCustomersNumber" CssClass="table table-bordered table-condensed text-center"
                                  AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                            <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                            <asp:BoundField DataField="Governorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:Tokens,Status %>" />
                            <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                            <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.ResellersPayments %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="BindResellersNumber" AutoGenerateColumns="False"
                                  ID="GvResellers" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                            <asp:BoundField DataField="TDate" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.ResellerPaymenttoPaymentRequest %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="BindResellersNumber" AutoGenerateColumns="False"
                                  ID="GVRPR" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                            <asp:BoundField DataField="TDate" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.BranchPaymenttoPaymentRequest %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="BindResellersNumber" AutoGenerateColumns="False"
                                  ID="GVBPR" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Branch   %>" />
                            <asp:BoundField DataField="TDate" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.BranchesPayments %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="BindBranchesNumber" AutoGenerateColumns="False"
                                  ID="GvBranches" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="TDate" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.Revenues %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="NumberRevs" AutoGenerateColumns="False"
                                  ID="GvRevenues" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TDate" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                            <asp:BoundField DataField="Comment" HeaderText="<%$Resources:Tokens,Comment %>" />
                            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.Expenses %></h3>
                <div>
                    <asp:GridView runat="server" AutoGenerateColumns="False" ID="GvExpenses" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Type" HeaderText="<%$Resources:Tokens,Type %>" />
                            <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="TDate" HeaderText="<%$Resources:Tokens,Date %>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="TAmount" HeaderText="<%$Resources:Tokens,Amount %>" />
                            <asp:BoundField DataField="Comment" HeaderText="<%$Resources:Tokens,Comment %>" />
                            <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.anotherCustomersPayments %>
                </h3>
                <div class="span4">
                        <asp:GridView 
                        CssClass="table table-bordered table-condensed text-center" ID="grd_Transactions" runat="server"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Time" HeaderText="<%$ Resources:Tokens,Date %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BoxName" HeaderText="<%$ Resources:Tokens,Boxname %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BoxAmount" HeaderText="<%$ Resources:Tokens,BoxAmount %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="InvoiceAmount" HeaderText="<%$ Resources:Tokens,invoiceAmount %>">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,User %>" />
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>" />
                            <asp:BoundField DataField="CustomerTelephone" HeaderText="<%$Resources:Tokens,Customer.Phone %>" />
                            <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company %>" />
                        </Columns>
                        <EmptyDataTemplate><%=Tokens.NoResults %></EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.Totals %>
                </h3>
                <div class="span4">
                    <asp:GridView ShowHeader="False" runat="server" ID="GvResults" Width="25%"
                    CssClass="table table-bordered table-condensed text-center">
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
     
    </div>

    <div class="fc-view">
        <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary"  
                    Text="<%$Resources:Tokens,Export %>" onclick="btnExport_Click"/>
                                                            &nbsp;
        <div class="noprint" style="float: left">
            <button class="btn btn-success"  onclick="printDiv('divData')">
                <i class="icon-print icon-only"></i>
                &nbsp;
                <%= Tokens.Print %>
            </button>
        </div>

    </div></div>

    <script type="text/javascript">
        $(function () {
            $('input[data-select="dp"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });
        });
        function printDiv(divID) {
            //Get the HTML of div
            var divElements = document.getElementById(divID).innerHTML;
            //Get the HTML of whole page
            var oldPage = document.body.innerHTML;
            //Reset the page's HTML with div's HTML only
            document.body.innerHTML =
              "<html><head><title></title></head><body>" +
              divElements + "</body>";
            //Print Page
            window.print();
            //Restore orignal HTML
            document.body.innerHTML = oldPage;
        }
    </script>
</asp:Content>
