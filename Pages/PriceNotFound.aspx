<%@ Page Title="Price not found" MasterPageFile="~/Pages/AwesomeMaster.master" Language="C#" AutoEventWireup="true" CodeBehind="PriceNotFound.aspx.cs" Inherits="NewIspNL.Pages.PriceNotFound" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
        <div class="view">
        <fieldset>
            <div class="page-header"><h1>Price not found</h1>
                </div>
            <div class="alert alert-danger">
                <asp:Literal runat="server" ID="li_message"></asp:Literal>
            </div>
            
        </fieldset>
    </div>
</asp:Content>

