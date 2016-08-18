<%@ Page Title="<%$Resources:Tokens,CompanyStore%>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CompanyStore.aspx.cs" Inherits="NewIspNL.Pages.CompanyStore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header"><h1><asp:Literal ID="Literal1" Text="<%$Resources:Tokens,CompanyStore%>" runat="server" /></h1></div>
    <style>
        #message {
            color: red;
        }
    </style>
    <section>
        <div class="view">
            <fieldset>
                
            
        <div class="well">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$Resources:Tokens,Routers%>" runat="server" /></h3>
                <div>
                    <div>
                        <label>
                            <asp:Literal Text="<%$Resources:Tokens,Quantity%>" runat="server" /> : <asp:Label runat="server" ID="L_Quantity"></asp:Label></label></div>
                    <div>
                        
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="well">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$Resources:Tokens,AddSubRouters%>" runat="server" /></h3>
                <div>
                    <label for="TB_Quantity">
                        <asp:Literal Text="<%$Resources:Tokens, AddSub%>" runat="server" /></label>
                    <div>
                        <asp:TextBox runat="server" ID="TB_Quantity" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_Quantity"
                            ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TB_Quantity"
                            ErrorMessage="<%$Resources:Tokens,NumbersOnly%>" Text="<%$Resources:Tokens,NumbersOnly%>"
                            MaximumValue="99999" MinimumValue="-99999" Type="Integer">Must be a number</asp:RangeValidator>
                    </div>
                </div>
                <p><br/>
                    <asp:Button runat="server" ID="B_AddSub" CssClass="btn btn-success" Text="<%$Resources:Tokens,Save%>" OnClick="BAddSubClick" /></p>
                <div id="message">
                    <asp:Literal runat="server" ID="LI_Message"></asp:Literal></div>
            </fieldset>
        </div>
        </fieldset>
        </div>
    </section>
</asp:Content>
