<%@ Page Title="<%$Resources:Tokens,ReceiptFormat%>"  ValidateRequest="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RecieptFormat.aspx.cs" Inherits="NewIspNL.Pages.RecieptFormat" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit.HTMLEditor" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>







<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal Text="<%$Resources:Tokens,ReceiptFormat %>" runat="server" /></h1></div>
            <div data-select="0">
                <div class="well">
                    <div>
                        <label for="DdlBranch">
                            <%= Tokens.Branch %></label>
                        <div>
                            <asp:DropDownList ValidationGroup="s" runat="server" ID="DdlBranch" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RddlBranch" ErrorMessage="*" ControlToValidate="DdlBranch"
                                runat="server" />
                        </div>
                    </div>
                    <br/>
                    <button class="btn btn-success" validationgroup="s" runat="server" id="BSearch">
                        <i class="icon-white icon-search"></i>&nbsp;
                        <asp:Literal runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal>
                    </button>
                </div>
            </div>
            <div data-select="1">
                  <div id="LMsgerror" Visible="False" runat="server" class="alert alert-danger">
                      <asp:Literal runat="server" Text="<%$Resources:Tokens,AddImage %>"></asp:Literal>
                </div>
                                  <div id="LMsgSuccess" Visible="False" runat="server" class="alert alert-success">
                                      <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Saved %>"></asp:Literal>
                </div>
                <div class="well">
                    <div>
                        <asp:Label Text="<%$Resources:Tokens,Logo %>" runat="server" AssociatedControlID="FLogo" />
                        <div>
                            <asp:FileUpload runat="server" ID="FLogo" />
                             <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FLogo"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %> "></asp:RegularExpressionValidator>
                   
                            <div>
                                <asp:Image runat="server" ID="ImgLogo" />
                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:Label Text="<%$Resources:Tokens,CompanyNameInput %>" runat="server" AssociatedControlID="TbCompanyName" />
                        <div>
                            <asp:TextBox runat="server" ID="TbCompanyName" />
                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbCompanyName"
                                                        runat="server" />
                        </div>
                    </div>
                    <div>
                        <asp:Label Text="<%$Resources:Tokens,LogoPosition %>" runat="server" AssociatedControlID="DdlPosition" />
                        <div>
                            <asp:DropDownList runat="server" ID="DdlPosition" />
                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="DdlPosition"
                                                        runat="server" />
                        </div>
                    </div>
                    <div>
                        <asp:Label Text="<%$Resources:Tokens,Caution %>" runat="server" AssociatedControlID="TbCaution" />
                        <div>
                            <asp:TextBox runat="server" ID="TbCaution" />
                            <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbCaution"
                                                        runat="server" />
                        </div>
                    </div>
                    <div>
                        <asp:Label Text="<%$Resources:Tokens,ContactData %>" runat="server" AssociatedControlID="Editor2" />
                        <div>
                          <cc1:Editor  ID="Editor2" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Editor2"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                                   
                        </div>
                    </div>
                    <p>
                        <button runat="server" class="btn btn-primary" onserverclick="BSave_OnServerClick" id="BSave">
                           <i class="icon-save"></i> &nbsp;<asp:Literal Text="<%$Resources:Tokens,Save %>" runat="server" /></button>&nbsp;
                        <button class="btn btn-success" CausesValidation="False"  runat="server" ID="BChangeBranch">
                            <i class="icon-refresh"></i>
                            &nbsp;
                            <%= Tokens.Cancel %>
                        </button>
                    </p>
                </div>
              
                <asp:HiddenField runat="server" ID="HfId" />
            </div>
        </fieldset>
    </div>
    <input type="hidden" runat="server" id="flag" clientidmode="Static" />
  
       <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/ckeditor/adapters/jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$(document).ready(function () {
            //    $("#Editor1").ckeditor();
            //});
            /* CKEDITOR.replace('TbContactData');*/
            var $search = $('div[data-select="0"]');
            var $edit = $('div[data-select="1"]');
            var $flag = $('#flag').val();
            if ($flag === "0") {
                $search.show();
                $edit.hide();
            } else {
                $search.hide();
                $edit.show();
            }
        });

    </script>
</asp:Content>

