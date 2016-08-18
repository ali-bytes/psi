<%@ Page Title="<%$Resources:Tokens,MenuInvoiceByCentral%>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="InvoiceByCentral.aspx.cs" Inherits="NewIspNL.Pages.InvoiceByCentral" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal runat="server" ID="lititle1" Text="<%$ Resources:Tokens,Search%>"></asp:Literal></h1></div>
            <div class="well">
                <div>
                    <div class="row">
                        <div class="col-md-4 col-sm-6 col-lg-4 col-xs-12">
                    <label for="ddl_centrals">
                        <asp:Literal runat="server" ID="lt_cen" Text="<%$ Resources:Tokens,Central%>"></asp:Literal>
                    </label>
                             <asp:DropDownList runat="server" ClientIDMode="Static" CssClass="width-60 chosen-select" Width="200px" ID="ddl_centrals" >
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_centrals"
                            Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        <div class="col-md-4 col-sm-6 col-lg-4 col-xs-12">
                         <label for="DdlProvider">
                                <asp:Literal Text="<%$Resources:Tokens,Provider %>" runat="server" />
                            </label>
                             <asp:DropDownList runat="server" ID="DdlProvider" CssClass="width-60 chosen-select"  />
                            </div>
                        <div class="col-md-4 col-sm-6 col-lg-4 col-xs-12">
                         <label for="DdlReseller">
                                <asp:Literal Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </label>
                             <asp:DropDownList ID="DdlReseller" CssClass="width-60 chosen-select"  runat="server" />
                            </div>
                        </div>
                   
                    <div>
                        <label for="ddl_months">
                            <asp:Literal runat="server" ID="Literal1" Text="<%$ Resources:Tokens,Month %>"></asp:Literal>
                        </label>
                        <div>
                            <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddl_months" Width="200px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddl_months"
                                Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div>
                        <label for="ddl_year">
                            <asp:Literal runat="server" ID="Literal2" Text="<%$ Resources:Tokens,Year %>"></asp:Literal>
                        </label>
                        <div>
                            <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddl_year" Width="200px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddl_year"
                                Display="Dynamic" ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <p>
                <asp:LinkButton runat="server" CssClass="btn btn-success" ID="b_search" OnClick="bSave_Click"
                    Width="90px"><i class="icon-search icon-only"></i>&nbsp;<%=Tokens.Search %></asp:LinkButton>
            </p>
        </fieldset></div> 
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal runat="server" ID="lititlw2" Text="<%$ Resources:Tokens,Details %>"></asp:Literal></h3>
            <div>
                <asp:GridView ID="gv_invoices" ClientIDMode="Static" runat="server" CssClass="table table-bordered table-condensed"
                              AutoGenerateColumns="False" OnDataBound="gv_invoices_DataBound">
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
                        <asp:BoundField DataField="TStart"  HeaderText="<%$ Resources:Tokens,From %>" />
                        <asp:BoundField DataField="TEnd" HeaderText="<%$ Resources:Tokens,To%>" />
                        <asp:BoundField DataField="Amount"  HeaderText="<%$ Resources:Tokens,Amount%>" />
                        <%--<asp:BoundField DataField="Discount" HeaderText="<%$ Resources:Tokens,Discount%>" />--%>
                        <%--<asp:BoundField DataField="Net" HeaderText="<%$ Resources:Tokens,Net%>" />--%>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <div class="alert alert-info">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal Text="<%$ Resources:Tokens,Net %>" runat="server" />
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label ID="net" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </fieldset>
        <button type="button" class="btn btn-primary" id="btnexport" runat="server" OnServerClick="Exportgrid"><i class="icon-file-text icon-only"></i>&nbsp;<%= Tokens.Export %></button><!--onclick="tableToExcel('gv_invoices', 'W3C Example Table')"-->
    </div>
 
      <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>
      <script type="text/javascript">
     $(document).ready(function () {
            $(".chosen-select").chosen();
     });   </script>
</asp:Content>
