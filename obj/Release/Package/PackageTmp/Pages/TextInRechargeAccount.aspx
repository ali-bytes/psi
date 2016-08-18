<%@ Page Title="<%$Resources:Tokens,TextInRechargeAccount %>" ValidateRequest="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="TextInRechargeAccount.aspx.cs" Inherits="NewIspNL.Pages.TextInRechargeAccount" %>

 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,TextInRechargeAccount %>" runat="server" /></h1></div>
            <div class="well">
                <div>
                    <div>
                         <textarea id="Editor1" dir="rtl" ClientIDMode="Static" runat="server"></textarea>

                   <%--     <cc1:Editor ID="Editor1" runat="server" />--%>
                    </div>
                    <div>
                        <p><br/>
                            <asp:Button runat="server" ID="btnUpdateText" 
                                          Text="<%$Resources:Tokens,Update %>" onclick="btnUpdateText_Click" CssClass="btn btn-primary"/>
                        </p>
                    </div>
                                   <p><asp:Label runat="server" ID="lblMsg"></asp:Label></p>
                </div>
            </div>
        </fieldset>
    </div>
        <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/ckeditor/adapters/jquery.js"></script>
    <script>
        $(document).ready(function() {
            $("#Editor1").ckeditor();
        });
    </script>
</asp:Content>

