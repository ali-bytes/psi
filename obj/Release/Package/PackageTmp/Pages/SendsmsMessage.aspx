<%@ Page Title="<%$Resources:Tokens,SendsmsMessage%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SendsmsMessage.aspx.cs" Inherits="NewIspNL.Pages.SendsmsMessage" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="page-header">
        <h1><%=Tokens.SendsmsMessage %></h1>
    </div>
    <div Visible="False" runat="server" class="alert alert-success" id="Msg">
        <%=Tokens.MsgSent %>
    </div>
    <asp:HiddenField ID="locations" runat="server" ClientIDMode="Static" />
    <div class="row">
        <div class="col-xs-12">
            <div class="form-horizontal"> 
          
                <div class="form-group">
               <label class="col-sm-3 control-label no-padding-right"> <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Mobile %>"></asp:Literal></label>
               <div class="col-sm-9">
                <span class="input-icon">
                    <asp:TextBox runat="server" ID="txtSendTo" CssClass="required-input"></asp:TextBox><i class="icon-mobile-phone green"></i></span>
                    <asp:FilteredTextBoxExtender ID="txt_CustomerMobile_FilteredTextBoxExtender" runat="server"
                                                 FilterType="Numbers" TargetControlID="txtSendTo" Enabled="True">
                    </asp:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSendTo" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="sms"></asp:RequiredFieldValidator>
                </div></div>
                <div class="space-4"></div>
                <div class="form-group">
                <label class="col-sm-3 control-label no-padding-right"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Message %>"></asp:Literal></label>
                <div class="col-sm-9">                  
                    <asp:TextBox runat="server" ID="txtMeaageText" ClientIDMode="Static"  TextMode="MultiLine" CssClass="required-input col-xs-10 col-sm-5" Width="200px"></asp:TextBox>
  <span class="help-inline col-xs-12 col-sm-3">
												<span class="middle"><asp:Literal runat="server" Text="<%$Resources:Tokens,littersLimit %>"></asp:Literal></span>
											</span>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMeaageText" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="sms"></asp:RequiredFieldValidator>
                </div></div>
                <div class="clearfix form-actions">
										<div class="col-md-offset-3 col-md-9">
											<button class="btn btn-info" type="button" runat="server" OnServerClick="btnSendSms_Click" ValidationGroup="sms">
												<i class="icon-ok bigger-110"></i>
												<%=Tokens.Send %>
											</button>

											&nbsp; &nbsp; &nbsp;
											<button class="btn" type="reset">
												<i class="icon-undo bigger-110"></i>
												<%=Tokens.reset %>
											</button>
										</div>
									</div>
                                   
 </div></div>
            <div id="concon">
            </div>
        </div>
    <script type="text/javascript">
        $(document).ready(function () {
           

            var val = $('#hfRtl').val();
            if (val == "1") $("#txtMeaageText").attr('maxlength', '70');
            else {
                $("#txtMeaageText").attr('maxlength', '160');
            }
          
        });
    </script>
</asp:Content>

