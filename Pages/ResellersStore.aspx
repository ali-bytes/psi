<%@ Page Title="<%$ Resources:Tokens,ResellersStore %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellersStore.aspx.cs" Inherits="NewIspNL.Pages.ResellersStore" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        #message {
            color: red;
        }
    </style>
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal Text="<%$ Resources:Tokens,ResellersStore %>" runat="server" /></h1>
                </div>
        <div class="well">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$ Resources:Tokens,Companystore %>" runat="server" /></h3>
                <div>
                    <label>
                        <asp:Literal runat="server" ID="LAvailableQuantityLabel" Text="<%$ Resources:Tokens,AvailableQuantity%>"></asp:Literal> : <asp:Literal runat="server" ID="LAvailableQuantity"></asp:Literal></label>
                </div>
            </fieldset>
        </div>
        <div class="well">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$ Resources:Tokens,Transfertoreseller %>" runat="server" /></h3>
                <div>
                    <div>
                        <label for="DResellers">
                            <asp:Literal Text="<%$ Resources:Tokens,Reseller %>" runat="server" /></label>
                        <div>
                            <asp:DropDownList runat="server" ID="DResellers" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DResellers"
                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div>
                        <label for="TbQuantity">
                            <asp:Literal Text="<%$ Resources:Tokens,Quantity %>" runat="server" /></label>
                        <div>
                            <asp:TextBox runat="server" ID="TbQuantity" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbQuantity"
                                ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TbQuantity"
                                ErrorMessage="<%$ Resources:Tokens,NumbersOnly %>" MaximumValue="99999" MinimumValue="-99999"
                                Type="Integer"></asp:RangeValidator>
                        </div>
                    </div>
                    <p>
                        <br/>
                        <asp:Button runat="server" CssClass="btn btn-success" Text="<%$ Resources:Tokens,Save %>" ID="BSave" OnClick="BSave_OnClick" />
                    </p>
                    <div id="message">
                        <asp:Literal runat="server" ID="LMessage"></asp:Literal>
                    </div>
                </div>
            </fieldset>
        </div>
        </fieldset>
    </div>
</asp:Content>

