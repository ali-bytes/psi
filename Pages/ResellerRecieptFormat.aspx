<%@ Page Title="<%$Resources:Tokens,ResellerRecieptFormat %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerRecieptFormat.aspx.cs" Inherits="NewIspNL.Pages.ResellerRecieptFormat" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit.HTMLEditor" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,ResellerRecieptFormat %>" runat="server" /></h1></div>
            <div data-select="0">
                <div class="well">
                    <div>
                        <label for="DdlBranch">
                            <%= Tokens.Reseller %></label>
                        <div>
                            <asp:DropDownList ValidationGroup="rs" runat="server" ID="DdlReseller" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RddlReseller" ErrorMessage="*" ControlToValidate="DdlReseller"
                                runat="server" />
                        </div>
                    </div>
                    <br/>
                    <button class="btn btn-success" validationgroup="rs" runat="server" id="BSearch">
                        <i class="icon-white icon-search"></i>&nbsp;
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal>
                    </button>
                </div>
            </div>
            <div data-select="1">
                  <div id="LMsgerror" Visible="False" runat="server" class="alert alert-danger">
                      <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,AddImage %>"></asp:Literal>
                </div>
                                  <div id="LMsgSuccess" Visible="False" runat="server" class="alert alert-success">
                                      <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,Saved %>"></asp:Literal>
                </div>
                <div class="well">
                    <div>
                        <asp:Label ID="Label1" Text="<%$Resources:Tokens,Logo %>" runat="server" AssociatedControlID="FLogo" />
                        <div>
                            <asp:FileUpload runat="server" ID="FLogo" />
                             <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|)$" ControlToValidate="FLogo"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %> "></asp:RegularExpressionValidator>
                   
                            <div>
                                <asp:Image runat="server" ID="ImgLogo" />
                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="Label2" Text="<%$Resources:Tokens,CompanyNameInput %>" runat="server" AssociatedControlID="TbCompanyName" />
                        <div>
                            <asp:TextBox runat="server" ID="TbCompanyName" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbCompanyName"
                                                        runat="server" />
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="Label3" Text="<%$Resources:Tokens,LogoPosition %>" runat="server" AssociatedControlID="DdlPosition" />
                        <div>
                            <asp:DropDownList runat="server" ID="DdlPosition" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="DdlPosition"
                                                        runat="server" />
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="Label4" Text="<%$Resources:Tokens,Caution %>" runat="server" AssociatedControlID="TbCaution" />
                        <div>
                            <asp:TextBox runat="server" ID="TbCaution" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbCaution"
                                                        runat="server" />
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="Label5" Text="<%$Resources:Tokens,ContactData %>" runat="server" />
                        <div>
                      <%--     <textarea id="Editor1" dir="rtl" ClientIDMode="Static" runat="server"></textarea>--%>

                          <%--  <cc1:Editor ID="Editor1" runat="server" />--%>
                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="Editor1"
                                                        runat="server" />--%>
                            
                            <cc1:Editor  ID="Editor2" runat="server" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Editor2"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                                       
                        </div>
                    </div>
                    <p>
                        <button runat="server" class="btn btn-primary" onserverclick="BSave_OnServerClick" id="BSave">
                           <i class="icon-save"></i> &nbsp;<asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Save %>" runat="server" /></button>&nbsp;
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
            $("#Editor1").ckeditor();
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


