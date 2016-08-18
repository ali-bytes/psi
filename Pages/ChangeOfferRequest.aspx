<%@ Page Title="<%$Resources:Tokens,ChangeOfferRequest %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ChangeOfferRequest.aspx.cs" Inherits="NewIspNL.Pages.ChangeOfferRequest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="page-header">
            <h1>
                <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,ChangeOfferRequest %>"></asp:Literal>
            </h1>
        </div>
        <div class="col-sm-12" runat="server" id="ser">
            <div class="widget-box">
                <div class="widget-header">
                    <h4>
                        <%=Tokens.Search%></h4>
                    <span class="widget-toolbar" style="padding: 12px"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                    </i></a><a href="#" data-action="reload"><i class="icon-refresh"></i></a><a href="#"
                        data-action="close"><i class="icon-remove"></i></a></span>
                </div>
                <div class="widget-body">
                    <div class="widget-main no-padding">
                        <div style="padding: 16px">
                                                <asp:UpdatePanel ID="up_phone" runat="server">
                                                    <ContentTemplate>
                                                        <div>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Customer.Phone %>"></asp:Label>
                                                            <div>
                                                                <asp:TextBox ID="txt_CustomerPhone" runat="server" Width="150px" ClientIDMode="Static"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div runat="server" id="GovBox">
                                                            <label for="DdlGovernorate">
                                                                <%= Tokens.Governorate %></label>
                                                            <div>
                                                                <asp:DropDownList runat="server" ID="DdlGovernorate" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RddlGovernorate" ErrorMessage="*" ControlToValidate="DdlGovernorate"
                                                                    runat="server" />
                                                            </div>
                                                        </div>
                                                        <asp:FilteredTextBoxExtender ID="txt_CustomerPhone_FilteredTextBoxExtender" runat="server"
                                                            FilterType="Numbers" TargetControlID="txt_CustomerPhone" Enabled="True">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                    ControlToValidate="txt_CustomerPhone" ValidationGroup="uph"></asp:RequiredFieldValidator>
                                            </div>
                                                                        <div class="form-actions center" style="margin: 0;">
                                <p class="carry">
                                    <asp:Button CssClass="btn btn-success" ID="btn_Search" runat="server" Text="<%$Resources:Tokens,Search %>"
                                        ValidationGroup="uph"  ClientIDMode="Static" OnClick="btn_Search_Click"/>
                                </p>
                                <div>
                                    <asp:Label ID="lbl_SearchResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                                    <asp:HiddenField ID="hf_woid" runat="server" />
                                    <asp:Label ID="lbl_InsertResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                                </div>
                                <%--<asp:Button CausesValidation="True" Text="<%$Resources:Tokens,Search %>" ID="BSearch"
                            CssClass="btn btn-primary" OnClick="BSearch_OnClick" runat="server" ClientIDMode="Static"/>
                                <button type="submit" class="btn btn-sm btn-success" clientidmode="Static" runat="server"
                                    id="BSearch" onserverclick="BSearch_OnClick">
                                    <%=Tokens.Search %>
                                    <i class="icon-search icon-on-right bigger-110"></i>
                                </button>
                                <asp:Literal ID="LMessage" runat="server" />--%>
                            </div>
                    </div>
                    </div>
                    </div>
                    </div>
        <div class="col-xs-12 col-sm-6" id="tr_Request" visible="false" runat="server">
            <div class="widget-box">
                <div class="widget-header">
                    <h4>
                        <asp:Label ID="lblrequesttypr" runat="server" Text="<%$Resources:Tokens,Request %>"></asp:Label></h4>
                    <span class="widget-toolbar" style="padding: 12px"><a href="#" data-action="reload"><i class="icon-refresh">
                    </i></a><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a><a
                        href="#" data-action="close"><i class="icon-remove"></i></a></span>
                </div>
                <div class="widget-body ">
                    <div class="widget-main no-padding">
                        <div style="padding: 10px;">
                            <fieldset>
                                <div id="tr_Offers" runat="server" style="padding: 5px;">
                                    <label style="width: 156px">
                                        <asp:Label ID="Label29" runat="server" Text="<%$Resources:Tokens,Offers %>"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="ddlOffers" runat="server" Width="155px"  ValidationGroup="ugh">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlOffers"
                                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="form-actions center" style="margin: 0;">
                        <button id="btn_AddRequest" runat="server"  class="btn btn-primary" ValidationGroup="ugh" OnServerClick="btn_AddRequest_Click">
                            <i class="icon-arrow-right icon-on-right bigger-110"></i>&nbsp;<%=Tokens.Add_Request %></button>
                        <%--<asp:Button   Text="$Resources:Tokens,Add.Request "
                                            Width="112px" CssClass= OnClick= />--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6" id="tr_Details" visible="false" runat="server">
            <div class="widget-box">
                <div class="widget-header">
                    <h4>
                        <asp:Label ID="l_CustomerDetails" runat="server" Text="<%$Resources:Tokens,Customer.Details %>"></asp:Label></h4>
                    <span class="widget-toolbar" style="padding: 12px"><a href="#" data-action="reload"><i class="icon-refresh">
                    </i></a><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a><a
                        href="#" data-action="close"><i class="icon-remove"></i></a></span>
                </div>
                <div class="widget-body">
                    <table class="table table-bordered table-condensed" style="margin: 0;">
                        <tr>
                            <td style="padding-top: 0;">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Tokens,Customer.Name %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CustomerName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Tokens,Customer.Phone %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CustomerPhone" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label25" runat="server" Text="<%$Resources:Tokens,Branch %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_BranchName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label26" runat="server" Text="<%$Resources:Tokens,Request.Date %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_RequestDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" Text="<%$Resources:Tokens,Service.Package %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ServicePackageName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="<%$Resources:Tokens,Ip.Package %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_IpPackageName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label33" runat="server" Text="<%$Resources:Tokens,Customer.Status %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CustomerStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
                    </div>
     <script type="text/javascript">
         $(document).ready(function () {
             $(document).keypress(function (e) {
                 var key = e.which;
                 if (key === 13) {
                     $('#btn_Search').click();
                     return false;
                 } else {
                     return true;
                 }
             });

        });
    </script>
</asp:Content>

