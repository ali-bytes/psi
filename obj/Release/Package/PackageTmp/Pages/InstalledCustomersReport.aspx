<%@ Page Title="<%$Resources:Tokens,InstalledCustomersReport%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="InstalledCustomersReport.aspx.cs" Inherits="NewIspNL.Pages.InstalledCustomersReport" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.InstalledCustomersReport%></h1>
            </div>
            <div class="well">
                <div>
                    <label for="TbStartAt">
                        <asp:Literal Text="<%$Resources:Tokens,From %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbStartAt" data-select="dp"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbFrom" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbStartAt" runat="server" />
                    </div>
                </div>
                <div>
                    <label for="TbTo">
                        <asp:Literal Text="<%$Resources:Tokens,To %>" runat="server" />
                    </label>
                    <div>
                        <asp:TextBox runat="server" ID="TbTo" data-select="dp"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RTbTo" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbTo" runat="server" />
                    </div>
                </div>
                <p>
                    <br/>
                    <button class="btn btn-primary" runat="server" id="bSearch">
                       <i class="icon-search icon-only">&nbsp;</i> <%= Tokens.Search %></button>
                </p>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.Results %></h3>
            <div>
                <asp:GridView CssClass="table table-bordered table-condensed" runat="server" ID="GvResults" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone %>" />
                        <asp:BoundField DataField="Govornorate" HeaderText="<%$Resources:Tokens,Governorate %>" />
                        <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central %>" />
                        <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State %>" />
                        <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="Provider" HeaderText="<%$Resources:Tokens,Provider %>" />
                        <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package %>" />
                        <asp:BoundField DataField="ActivationDate" HeaderText="<%$Resources:Tokens,Activation.Date %>" />
                        <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer %>" />
                        <asp:BoundField DataField="Installer" HeaderText="<%$Resources:Tokens,Installer %>" />
                        <asp:BoundField DataField="InstallationTime" HeaderText="<%$Resources:Tokens,InstallationTime %>" />
                        <asp:BoundField DataField="Note" HeaderText="<%$Resources:Tokens,Notes %>" />
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
 
    <script type="text/javascript">
        $(function () {
            $('input[data-select="dp"]').datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
</asp:Content>
