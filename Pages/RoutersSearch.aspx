<%@ Page Title="<%$Resources:Tokens,RoutersSearch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RoutersSearch.aspx.cs" Inherits="NewIspNL.Pages.RoutersSearch" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,RoutersSearch %>"></asp:Literal></h1></div>
            <div class="well">
                <div>
                    <label><asp:Literal runat="server" Text="<%$Resources:Tokens,AddStore %>"></asp:Literal></label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlStore" ValidationGroup="ro"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlStore" ValidationGroup="ro"></asp:RequiredFieldValidator>
                    </div>
                </div><br/>
                <asp:Button runat="server" Text="<%$Resources:Tokens,Search %>" ValidationGroup="ro" ID="btnSearch"
                 CssClass="btn btn-success" OnClick="Search"/>
            </div>
        </fieldset>
    </div>
        <div class="view">
        <fieldset>
            <asp:GridView ID="GVRouters" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center"
            OnDataBound="GVRouters_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RouterSerial" HeaderText="<%$Resources:Tokens,RouterSerial %>"/>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>"/>
                            <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Tokens,AddStore %>"/>
                            <asp:BoundField DataField="CompanyUserName" HeaderText="<%$Resources:Tokens,CompanyConfirmerUser %>"/>
                            <asp:BoundField DataField="CompanyProcessDate" HeaderText="<%$Resources:Tokens,CompanyProcessDate %>"/>
                            <asp:BoundField DataField="CustomerUserName" HeaderText="<%$Resources:Tokens,CustomerConfirmerUser %>"/>
                            <asp:BoundField DataField="CustomerProcessDate" HeaderText="<%$Resources:Tokens,CustomerProcessDate %>"/>
                            <asp:BoundField DataField="IsRecieved" HeaderText="<%$Resources:Tokens,CustomerConfirmerUser %>"/>
                </Columns>
                <EmptyDataTemplate>
                    <%=Tokens.NoResults %>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>


