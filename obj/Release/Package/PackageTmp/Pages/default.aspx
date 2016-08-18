<%@ Page Title="Login" Language="C#" MasterPageFile="~/Pages/Login.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="NewIspNL.Pages.Default" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .signup-box {
            display: none;
        }
    </style>

    <div class="position-relative ">
        <div id="login-box" class="login-box visible widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <h4 class="header blue lighter bigger center">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Login %>"></asp:Literal>
                        <i class="icon-coffee green"></i>
                    </h4>
                    <div class="space-6">
                    </div>
                    <div>
                        <fieldset>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" placeholder='<%$Resources:Tokens,UserName %>'
                                        id="txtUsername" runat="server" clientidmode="Static" />
                                    <i class="icon-user"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtPassword" runat="server"
                                        ClientIDMode="Static" placeholder="<%$Resources:Tokens,Password %>" />
                                    <i class="icon-lock"></i></span>
                            </label>
                            <div class="space">
                            </div>
                            <div class="clearfix">
                                <%--<label class="inline">
                                    <%--<input type="checkbox" class="ace" runat="server" id="CheckMemberMe" />
                                    <span class="lbl">
                                        <%=Tokens.RememberMe %></span>
                                </label>--%>
                                <button type="button" style="margin-right: 95px;" class="width-35 pull-right btn btn-sm btn-primary" id="btnLogin">
                                    <i class="icon-key"></i>
                                    <%=Tokens.Login %>
                                </button>
                                <button runat="server" clientidmode="Static" id="btnbehind" onserverclick="Login1_Authenticate" />
                                <strong style="color: red; font-size: 14pt;">
                                    <%=Session["loginIpError"]%></strong>
                            </div>
                            <div class="space-4">
                            </div>
                            <div class="alert alert-danger" visible="False" runat="server" id="lblError">
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="toolbar clearfix">
                    <div class="text-left">
                        <a href="#" id="forgot" class="forgot-password-link">
                            <i class="icon-arrow-left"></i>
                             <%=Tokens.ForgotPassword %>
                        </a>
                    </div>
                    <div class="text-right">
                        <a href="#" onclick="show_box('signup-box'); return false;" class="user-signup-link">
                            <!--I want to be Reseller-->

                            <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,NewResellerRequest %>"></asp:Literal>
                            <i class="icon-arrow-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
        <div id="signup-box" class="signup-box widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <h4 class="header green lighter bigger" style="text-align: right">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,NewResellerRequest %>"></asp:Literal>&nbsp; <i class="icon-group blue"></i>
                    </h4>
                    <div class="space-6">
                    </div>
                    <p style="direction: rtl">
                        <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,CompanyData %>"></asp:Literal>
                        :
                    </p>
                    <div id="myform">
                        <fieldset>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" id="txtArabicName" runat="server" clientidmode="Static" class="form-control"
                                        placeholder="<%$Resources:Tokens,CompanyNameArabic %>" title="<%$Resources:Tokens,CompanyNameArabic %>" />
                                    <i class="icon-home"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" id="txtEnglishName" runat="server" clientidmode="Static" class="form-control"
                                        placeholder="<%$Resources:Tokens,EnglishCompanyName %>" title="<%$Resources:Tokens,EnglishCompanyName %>" />
                                    <i class="icon-home"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <asp:DropDownList runat="server" ID="ddlCompanyType" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Selected="True" Text="<%$Resources:Tokens,CompanyType %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Tokens,Company %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Tokens,office %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Tokens,market %>"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--                                    <select class="form-control" id="ddlCompanyType" clientidmode="Static" runat="server"
                                        title="نوع المنشأة">
                                        <option value="0">نوع المنشأة</option>
                                        <option>شركة</option>
                                        <option>مكتب</option>
                                        <option>محل</option>
                                    </select>--%>
                                </span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <asp:DropDownList runat="server" ID="ddlEmployeeNumbers" ClientIDMode="Static" CssClass="form-control">
                                        <asp:ListItem Selected="True" Text="<%$Resources:Tokens,EmployeeNumbers %>"></asp:ListItem>
                                        <asp:ListItem Text="1-5"></asp:ListItem>
                                        <asp:ListItem Text="6-10"></asp:ListItem>
                                        <asp:ListItem Text="11-20"></asp:ListItem>
                                        <asp:ListItem Text="أكثر من 20"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--                                    <select class="form-control" id="ddlEmployeeNumbers" clientidmode="Static" runat="server"
                                        title="عدد الموظفين">
                                        <option value="0">عدد الموظفين</option>
                                        <option>1 - 5</option>
                                        <option>6 - 10</option>
                                        <option>11 - 20</option>
                                        <option>أكثر من 20</option>
                                    </select>--%>
                                </span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <textarea class="form-control" runat="server" clientidmode="Static" id="txtCompanyActivity"
                                        placeholder="<%$Resources:Tokens,CompanyActivites %>" title="<%$Resources:Tokens,CompanyActivites %>"></textarea>
                                    <%--<i class="icon-envelope"></i>--%>
                                </span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtComapnyAddress" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,Address %>" title="<%$Resources:Tokens,Address %>" />
                                    <i class="glyphicon glyphicon-map-marker"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtCompanyTele" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,Phone %>" title="<%$Resources:Tokens,Phone %>" />
                                    <i class="icon-phone"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtfax" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,Fax %>" title="<%$Resources:Tokens,Fax %>" />
                                    <i class="icon-inbox"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtCompanyEmail" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,CompanyEmail %>" title="<%$Resources:Tokens,CompanyEmail %>" />
                                    <i class="icon-envelope"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtCompanyMobile" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,CompanyMobile %>" title="<%$Resources:Tokens,CompanyMobile %>" />
                                    <i class="icon-mobile-phone"></i></span>
                            </label>
                            <%--style="text-align: right;border-bottom: 1px solid #cce2c1;"--%>
                            <h4 class="header green lighter bigger">
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,ResellerData %>"></asp:Literal></h4>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtResellerName" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,Reseller %>" title="<%$Resources:Tokens,Reseller %>" />
                                    <i class="icon-user"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtNationalNumber" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,NationalId %>" title="<%$Resources:Tokens,NationalId %>" />
                                    <i class="icon-credit-card"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtResellerMobile" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,Mobile %>" title="<%$Resources:Tokens,Mobile %>" />
                                    <i class="icon-mobile-phone"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtResellerEmail" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,Email %>" title="<%$Resources:Tokens,Email %>" />
                                    <i class="icon-envelope"></i></span>
                            </label>
                            <p style="text-align: right; border-bottom: 1px solid #cce2c1;">
                                <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,RequriedDatainResellerRequest %>"></asp:Literal>
                            </p>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <input type="text" class="form-control" id="txtResellerUsername" runat="server" clientidmode="Static"
                                        placeholder="<%$Resources:Tokens,UserName %>" title="<%$Resources:Tokens,UserName %>" />
                                    <i class="icon-user"></i></span>
                            </label>
                            <label class="block clearfix">
                                <span class="block input-icon input-icon-right">
                                    <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtResellerPassword"
                                        runat="server" ClientIDMode="Static" placeholder="<%$Resources:Tokens,Password %>" ToolTip="<%$Resources:Tokens,Password %>" />
                                    <i class="icon-lock"></i></span>
                            </label>
                            <p>
                                <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,languageRequired %>"></asp:Literal>
                            </p>

                            <div class="control-group">
                                <label class="block">
                                    <label>
                                        <input type="radio" runat="server" clientidmode="Static" class="ace" id="radioArabic"
                                            value="1" />
                                        <span class="lbl"><%=Tokens.arabic %> </span>
                                    </label>
                                    <br />
                                    <label>
                                        <input type="radio" runat="server" clientidmode="Static" class="ace" id="radioEnglish"
                                            value="2" />
                                        <span class="lbl"><%=Tokens.english %> </span>
                                    </label>
                                    <input type="hidden" id="hdfCulture" />
                                </label>
                            </div>
                            <div class="alert" style="display: none" id="msg">
                            </div>

                            <div class="space-24">
                            </div>
                            <div class="clearfix">
                                <button type="reset" class="width-30 pull-left btn btn-sm">
                                    <i class="icon-refresh"></i><%=Tokens.reset %>
                                </button>
                                <button type="button" class="width-65 pull-right btn btn-sm btn-success" id="btnRegister" onclick="alert(' تم التسجيل بنجاح   ');"
                                    runat="server" onserverclick="BtnSave" clientidmode="Static">
                                    <%=Tokens.Send %> <i class="icon-arrow-right icon-on-right"></i>
                                </button>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="toolbar center">
                    <a href="#" onclick="hide_box('signup-box'); return false;" class="back-to-login-link">
                        <i class="icon-arrow-left"></i><%=Tokens.Back %></a>
                </div>
            </div>
            <!-- /widget-body -->
        </div>
    </div>
    <div class="center" style="position: relative; margin-left: auto; margin-right: auto;">
        <h1>
            <i class="icon-leaf green"></i><span class="white">Smart</span> <span class="red">ISP</span>
            <span class="white">System</span>
        </h1>

    </div>
    <div id="ForgetPasswordModal" class="bootbox modal" tabindex="-1" role="dialog" aria-labelledby="H1"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H4">
                        <%=Tokens.ForgotPassword %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="form-group">
                            <label for="ForgotPassEmail"><%=Tokens.Email %></label>
                            <input type="text" runat="server" clientidmode="Static" class="form-control" id="ForgotPassEmail" placeholder="Email"/>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ForgotPassEmail"
                                        ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="forgot">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="ForgotPassUserName"><%=Tokens.UserName %></label>
                            <input type="text" runat="server" clientidmode="Static" class="form-control" id="ForgotPassUserName" placeholder="User Name"/>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ForgotPassUserName"
                                        ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="forgot">*</asp:RequiredFieldValidator>
                        </div>
                          <div id="forgotMsg" runat="server"></div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="updown-results"></div>
                    <button class="btn btn-primary" type="button" validationgroup="forgot" runat="server"
                        onserverclick="btn_ForgotPass_Click">
                      <%--<button id="sendpass" class="btn btn-info" type="button">--%>
                        <%=Tokens.SendEmail %></button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
       
      <script type="text/javascript" >
          $(document).ready(function() {
              $('#forgot').click(function() {
                  $('#ForgetPasswordModal').modal('show');
              });

          });
      </script>
    <%--<script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script>--%>
</asp:Content>
