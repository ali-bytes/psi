<%@ Page Title="<%$Resources:Tokens,CustomersPayments%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomersPayments.aspx.cs" Inherits="NewIspNL.Pages.CustomersPayments" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .table {
            display: block;
            overflow-x: scroll;
        }
    </style>
    <div class="view">
        <fieldset>
            <div class="page-header">
                <h1>
                    <%=Tokens.Search %></h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-4">
                    <div class="well" style="height: 208px;">
                        <div>
                            <div>
                                <label for="DdlBranch">
                                    <%= Tokens.Branch %>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlBranch">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div>
                                <label for="DdlReseller">
                                    <%= Tokens.Reseller %></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlReseller">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div>
                                <label for="DdlPaymentTypes">
                                    <asp:Literal Text="<%$Resources:Tokens,PaymentType %>" runat="server" />
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlPaymentTypes" EnableViewState="True" />
                                </div>
                            </div>
                            <br />
                            <div>
                                <label>
                                    <input type="checkbox" runat="server" id="checkIsrequested" />&nbsp;<%=Tokens.RequestReimbursementPayment%></label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlGov">
                                <%= Tokens.Governorate %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlGov" AutoPostBack="True" OnSelectedIndexChanged="UpdateCentrals">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlCentral">
                                <%= Tokens.Central %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlCentral">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label><%=Tokens.Service_Provider %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlServiceProvider" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well" style="height: 208px;">
                        <div>
                            <asp:RadioButtonList runat="server" ID="RblEffect" ClientIDMode="Static" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Text="<%$Resources:Tokens,PaymentDate %>"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Tokens,InvoiceDate %>"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div>
                            <div>
                                <label for="TbFrom">
                                    <%= Tokens.From %></label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbFrom" data-select="dp" />
                                </div>
                            </div>
                            <div>
                                <label for="TbTo">
                                    <%= Tokens.To %></label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbTo" data-select="dp" />
                                    <asp:CompareValidator ID="CV" ErrorMessage="*" ControlToValidate="TbTo" runat="server"
                                        ControlToCompare="TbFrom" Operator="GreaterThanEqual" Type="Date" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <hr />
                <div>
                    <button runat="server" onserverclick="Search" type="submit" class="btn btn-success">
                        <i class="icon-search"></i>&nbsp;<%= Tokens.Search %></button>
                </div>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%=Tokens.Results +"&nbsp;"+ Tokens.CustomersPayments%></h3>
            <div>
                <div>
                    <div class="alert alert-info col-xs-12 col-md-12 ">
                        <div class="col-xs-6 col-md-4">
                            <%= Tokens.Total %>&nbsp;:&nbsp;<b runat="server" id="totaly"></b>&nbsp;&nbsp;&nbsp;&nbsp;
                        </div>

                    </div>
                </div>
                <asp:GridView runat="server" OnDataBound="NumberResults" CssClass="table table-bordered table-condensed"
                    ID="GvResults" AutoGenerateColumns="False" ClientIDMode="Static">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="No" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name %>" />
                        <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="Governorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                        <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                        <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                        <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                        <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User %>" />
                        <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes %>" />
                        <asp:BoundField DataField="TStart" HeaderText="<%$Resources:Tokens,From %>" />
                        <asp:BoundField DataField="TEnd" HeaderText="<%$Resources:Tokens,To %>" />
                        <asp:BoundField DataField="PaymentDate" HeaderText="<%$Resources:Tokens,PaymentDate %>" />
                        <asp:BoundField DataField="ActiviationDate" HeaderText="<%$Resources:Tokens,Activation.Date %>" />
                    </Columns>
                </asp:GridView>
                <p>
                    <button class="btn btn-success" type="button" runat="server" onserverclick="ToExcel">
                        <i class="icon-file-text-alt"></i>&nbsp;<%= Tokens.Export %></button>
                </p>
            </div>
        </fieldset>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
            if ($('#GvResults').width() > 1058) {
                $('#GvResults').css({
                    "font-size": "81%"
                });
            }
        });
    </script>
</asp:Content>
