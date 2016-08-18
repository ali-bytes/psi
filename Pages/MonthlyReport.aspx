<%@ Page Title="<%$ Resources:Tokens,MonthlyReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="MonthlyReport.aspx.cs" Inherits="NewIspNL.Pages.MonthlyReport" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
               <asp:Literal runat="server" Text="<%$Resources:Tokens,MonthlyReport %>"></asp:Literal></h1></div>
            <div class="well">
                <div>
                    <label for="TbFrom">
                        <asp:Literal  Text="<%$Resources:Tokens,From %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbFrom" data-select="dp"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbFrom" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="TbFrom" runat="server" />
                    </div>
                </div>
                <div>
                    <label for="TbTo">
                        <asp:Literal  Text="<%$Resources:Tokens,To %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbTo" data-select="dp"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbTo" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                    ControlToValidate="TbTo" runat="server" />
                        
                        <asp:CompareValidator runat="server" ID="RCV" ControlToValidate="TbTo" ControlToCompare="TbFrom" Type="Date" Operator="GreaterThanEqual"> </asp:CompareValidator>
                    </div>
                </div>

            </div>
            <p>
                <button id="Button1" class="btn btn-success" runat="server" onserverclick="Search">
                    <i class="icon-white icon-search"></i>&nbsp;<%= Tokens.Search %></button></p>
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
                        <asp:BoundField DataField="PaymentDate" HeaderText="<%$Resources:Tokens,Date %>" />
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
                                <asp:Label ID="LNo" runat="server" />
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
                    <%= Tokens.BranchesPayments %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="BindBranchesNumber" AutoGenerateColumns="False"
                                  ID="GvBranches" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
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
                    <%= Tokens.ResellerPaymenttoPaymentRequest %></h3>
                <div>
                    <asp:GridView runat="server" OnDataBound="BindResellersNumber" AutoGenerateColumns="False"
                                  ID="GVRPR" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
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
                                    <asp:Label ID="LNo" runat="server" />
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
                <%= Tokens.Revenues %></h3>
            <div>
                <asp:GridView runat="server" OnDataBound="NumberRevs" AutoGenerateColumns="False"
                    ID="GvRevenues" CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
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
                                <asp:Label ID="LNo" runat="server" />
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
                <%= Tokens.Totals %>
            </h3>
            <div class="col-sm-4">
                <asp:GridView ShowHeader="False" runat="server" ID="GvResults" CssClass="table table-bordered table-condensed text-center">
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    </div>
    <div class="fc-view">
        <asp:LinkButton runat="server" ID="btnExport" CssClass="btn btn-primary"  
                    onclick="btnExport_Click"><i class="icon-file-text-alt icon-only"></i>&nbsp;<asp:Literal runat="server" Text="<%$Resources:Tokens,Export %>"></asp:Literal></asp:LinkButton>
                                        &nbsp;
                    <div class="noprint" style="float: left">
                        <button class="btn btn-success"  onclick="printDiv('divData')">
                            <i class="icon-white icon-print"></i>
                            &nbsp;
                            <%= Tokens.Print %>
                        </button>
                    </div>
    </div>
    
    <script type="text/javascript">
        $(function () {
            $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
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

