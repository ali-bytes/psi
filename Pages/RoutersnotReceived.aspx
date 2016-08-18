<%@ Page Title="<%$Resources:Tokens,RoutersnotReceived %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RoutersnotReceived.aspx.cs" Inherits="NewIspNL.Pages.RoutersnotReceived" %>

<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Label runat="server" ID="lblTitle" Text="<%$Resources:Tokens,RoutersnotReceived %>"></asp:Label></h1></div>
            <div class="well">
                <div>
                    <asp:Label runat="server" Text="<%$Resources:Tokens,AddStore %>"></asp:Label>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlStore" ValidationGroup="sto"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlStore" ValidationGroup="sto"></asp:RequiredFieldValidator>
                    </div>
                    
                </div>
                <br/>
                <asp:Button runat="server" Text="<%$Resources:Tokens,Search %>" ID="btnSearch" ValidationGroup="sto"
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
                            <asp:BoundField DataField="RouterType" HeaderText="<%$Resources:Tokens,RouterType %>"/>
  <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Tokens,AddStore %>"/>
                            <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,CompanyConfirmerUser %>"/>
                            <asp:BoundField DataField="CompanyProcessDate" HeaderText="<%$Resources:Tokens,CompanyProcessDate %>"/>
                </Columns>
                <EmptyDataTemplate>
                    <%=Tokens.NoResults %>
                </EmptyDataTemplate>
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>


