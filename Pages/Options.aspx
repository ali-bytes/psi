<%@ Page Title="<%$Resources:Tokens,Options%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Options.aspx.cs" Inherits="NewIspNL.Pages.Options" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        #errmsg {
            color: red;
        }

        .marg {
            margin-right: 20px;
        }
    </style>
    <div class="row">
        <fieldset>
            <div class="page-header">
                <h1>
                    <%= Tokens.Options %></h1>
            </div>
            <div class="form-horizontal">
                <div class="control-group">
                    <div class="well">
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CbIncludeGov" /><span class="lbl">&nbsp;<%= Tokens.SearchByGovPhone %></span></label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CbMergeGovPhone" /><span class="lbl">&nbsp;<%= Tokens.MergeGorvoernorateCodeWithPhoneNumberWhileAddingNewCustomer%></span></label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CheckDiscound" /><span class="lbl"><%= Tokens.DiscoundFromBranchCredit %></span></label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CheckDiscoundfrombranchandReseller" /><span
                                    class="lbl"><%= Tokens.DiscoundFromBranchAndReseller %></span></label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="checkUpload" /><span class="lbl"><%=Tokens.UploadFileToNewCustomer %></span>
                            </label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CheckCounter" /><span class="lbl"><%=Tokens.CountersinMenu %></span>
                            </label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="Checkminuscredit" /><span class="lbl"><%=Tokens.Allowminuscredit %></span>
                            </label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CheckStatistic" /><span class="lbl"><%= Tokens.ShowStatistic %></span></label>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" class="ace" runat="server" id="CheckSendMessage" /><span class="lbl"><%=Tokens.SendMessageOrNot %></span>
                            </label>
                        </div>
                        <div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="checkInstallationDiscound" /><span
                                        class="lbl"><%=Tokens.checkIntallationDiscound%></span>
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="CheckShowRequestInSearch" /><span
                                        class="lbl"><%=Tokens.ShowRequestsInSearch%></span>
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="CheckActivatephone" /><span
                                        class="lbl"><%=Tokens.ActivatCustomerPhoneNumbersValidation%></span>
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="CheckAllDemand" /><span class="lbl"><%=Tokens.ShowAlldemandOfPaymentRequest %></span>
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="AutoSusCustomerUnderReseller" /><span class="lbl"><%=Tokens.AutoSuspendtoCustomerUnderReseller %></span>
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="chkbxShowDedWithFix" /><span class="lbl"><%=Tokens.ShowDeductionWithFixedRequestDate %></span>
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="chkbxPreventUnsus" /><span class="lbl"><%=Tokens.PreventUnsusForCustomerHasIndebtedness%></span>
                                </label>
                            </div>
                            
                              <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="cbSusAfterMonth" /><span class="lbl"><%=Tokens.Suspend_after_month_from_reActivate%></span>
                                </label>
                            </div>
                            

                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="CheckFawry" disabled="disabled" /><span class="lbl"><%=Tokens.ActivateFawry %></span>
                                </label>
                                <span class="lbl" style="color: red">لتفعيل خدمة فورى . الرجاء التواصل مع الإدارة  </span>
                                <br />

                                <div style="margin-right: 10%">
                                    <asp:CheckBoxList Enabled="false" ClientIDMode="Static" Width="100%" runat="server" ID="checkfawrytype"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="FirstDemandUnPaid">اول مطالبة غير مدفوعة</asp:ListItem>
                                        <asp:ListItem Value="AllDemad">جميع المطالبات غير المدفوعة</asp:ListItem>
                                        <asp:ListItem Value="LastDemondUnPaid">اخر مطالبة غير مدفوعة</asp:ListItem>
                                    </asp:CheckBoxList>

                                </div>
                            </div>
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,DemandReciept %>"></asp:Label>
                            <div class="radio-inline">
                                <label>
                                    <input type="radio" runat="server" id="CheckReciepit" class="ace" /><span class="lbl"><%= Tokens.Reciept2%></span></label>
                                <label>
                                    <input type="radio" runat="server" id="CheckHalfReciepit" class="ace" /><span class="lbl"><%= Tokens.HalfReciept%></span></label>
                            </div>
                            <div>
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,PortalTedata %>">
                                </asp:Label><span class="lbl" style="color: red">  <%= Tokens.PortalCation %>  </span>
                                <asp:CheckBoxList ClientIDMode="Static" Width="100%" runat="server" ID="portalTeData"
                                    RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </div>
                            <br />
                            <div>
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,ServiceProviderstoPaymentRequest %>"></asp:Label>
                                <asp:CheckBoxList ClientIDMode="Static" Width="100%" runat="server" ID="providerlist"
                                    RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </div>
                            <br />
                            <div>
                                <asp:Label runat="server" Text="<%$Resources:Tokens,ServiceProviderstoInvoicePaymentRequest %>"></asp:Label>
                                <asp:CheckBoxList runat="server" ClientIDMode="Static" Width="100%" ID="InvoiceProviderList"
                                    RepeatDirection="Horizontal" />
                            </div>
                            <br />
                            <%--todo:new task 28-4-2015--%>
                            <div>
                                <asp:Label runat="server" Text="<%$Resources:Tokens,SPforResellerAcoount %>"></asp:Label>
                                <asp:CheckBoxList runat="server" ClientIDMode="Static" Width="100%" ID="CheckBoxList1"
                                    RepeatDirection="Horizontal" />
                            </div>
                            <br />
                            <%-- <fieldset>
                                <legend>Portal Option</legend>
                                 <div class="row">
                                <div class="col-md-2">
                                     <label class="label label-xlg label-warning" for="DdTypesFrom">
                                        <%= Tokens.Suspend %>
                                    </label>
                                    </div>
                                     <div class="col-md-4">
                                 <asp:RadioButtonList ID="RadioSuspend" ClientIDMode="Static" runat="server"
                                    Width="100%" CssClass="radio-inline" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text="<%$Resources:Tokens,Confirm %>"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="<%$Resources:Tokens,ConfirmWithLiquidation %>"></asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                     </div>
                                     </div>
                                 <hr />
                            </fieldset>--%>
                            <hr />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="DdTypesFrom">
                                        <%= Tokens.WhenConversionFromPackageType %>
                                    </label>
                                    <asp:DropDownList runat="server" ID="DdTypesFrom" ClientIDMode="Static" Width="178px">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="DdTypesTo">
                                        <%= Tokens.ToPackageType %>
                                    </label>
                                    <asp:DropDownList runat="server" ID="DdTypesTo" ClientIDMode="Static" Width="178px">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="txtPrice">
                                        <%= Tokens.Addthevalueofthedebt %>
                                    </label>
                                    <asp:TextBox runat="server" ID="txtPrice" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <hr />
                            <br />
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <label for="txtPortalRelayDays">
                                        <%= Tokens.PortalRelayDays %>
                                    </label>&nbsp;
                                    <asp:TextBox runat="server" ID="txtPortalRelayDays" ClientIDMode="Static" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                             <hr />
                            <br />
                            <div>
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" class="ace" runat="server" id="checkDuration" onclick="DisableText()" /><span
                                            class="lbl"><%=Tokens.RecevingRequestesDuration %></span></label>
                                </div>
                                <div>
                                    <table id="durationtable">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblDuration" Text="<%$Resources:Tokens,From %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtFrom"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label2" Text="<%$Resources:Tokens,To %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTo"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <label for="TbSuspendDaysCount">
                                <asp:Literal Text="<%$Resources:Tokens,SuspendDaysCountForSuspendPage %>" runat="server" />
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbSuspendDaysCount" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RTbSus" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="TbSuspendDaysCount" runat="server" />
                                <asp:CompareValidator runat="server" ID="xxx" ControlToValidate="TbSuspendDaysCount"
                                    Type="Integer" Operator="DataTypeCheck" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"></asp:CompareValidator>
                            </div>
                            <div>
                                <label for="TbTimeDifference">
                                    <%=Tokens.TimeDifference%>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbTimeDifference" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="<%$Resources:Tokens,Required %>"
                                        ControlToValidate="TbTimeDifference" runat="server" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div>
                            <div class="col-md-6">
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,UnpaidDemandDaysCountOption %>"></asp:Literal>
                                <br />
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,DaysCount %>"></asp:Literal>
                                :
                                <asp:TextBox runat="server" ID="txtDaysCount"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ControlToValidate="txtDaysCount" runat="server" />
                                <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="txtDaysCount"
                                    Type="Integer" Operator="DataTypeCheck" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"></asp:CompareValidator>
                            </div>
                            <div class="col-md-6">
                                <asp:LinkButton runat="server" ID="btnAddRequests" CssClass="btn btn-success" OnClick="AddRequest">
                        <i class="icon-arrow-right icon-on-right"></i>&nbsp;&nbsp;<%=Tokens.Add_Request %>&nbsp;<%=Tokens.Suspend %>
                                </asp:LinkButton>
                            </div>
                            <div>
                                <asp:RadioButtonList ID="checkUnpaidDemand" ClientIDMode="Static" runat="server"
                                    Width="100%" CssClass="radio-inline" RepeatDirection="Horizontal" OnSelectedIndexChanged="checkUnpaidDemand_SelectedIndexChanged">

                                    <asp:ListItem Value="1" Text="<%$Resources:Tokens,SuspendRequest %>"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="<%$Resources:Tokens,AddToCustomerNotes %>"></asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                <div id="ReminderText">
                                    <asp:Label runat="server" Text="<%$Resources:Tokens,User %>"></asp:Label>
                                    <div>
                                        <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlUser" />

                                        <asp:Label runat="server" ID="lblReminder" Text="<%$Resources:Tokens,Reminder %>"></asp:Label>
                                        <div>
                                            <asp:TextBox runat="server" ID="txtRemindertext" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <div id="suspendoption">
                                    <br />
                                    <asp:CheckBoxList ClientIDMode="Static" Width="100%" runat="server" ID="CheckProviderSuspend"
                                        RepeatDirection="Horizontal" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div>

                            <%--<div class="checkbox">--%>

                            <span class="">
                                <input type="checkbox" class="ace" runat="server" id="ChkboxResellerdebt" clientidmode="Static" /><span class="lbl">&nbsp;<%= Tokens.Resellerdebt%></span>
                            </span>

                            <%--</div>--%>
                            <%--  <a href="#Resellerdebt" class="btn btn-sm btn-primary" data-toggle="collapse" title="<%=Tokens.Resellerdebt%>"><i
                                class="icon-cog"></i><%=Tokens.Resellerdebt %></a>--%>
                            <br />
                            <div id="Resellerdebt" runat="server" clientidmode="Static" class="collapse row">

                                <div class="row">
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Day1%></span>
                                        <asp:TextBox ID="txtday1" runat="server" class="form-control" TextMode="Number" min="0" max="31" step="1" ClientIDMode="Static" placeholder="<%$Resources:Tokens,Day1%>" aria-describedby="sizing-addon2" ToolTip="ضع اليوم فقط"></asp:TextBox>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Percentage%></span>
                                        <asp:TextBox ID="txtpercentage1" runat="server" class="form-control" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="100" step="1"></asp:TextBox>
                                        <span class="input-group-addon label-warning" id="sizing-addon2">%</span>
                                    </div>

                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Day2%></span>
                                        <asp:TextBox ID="txtday2" runat="server" class="form-control" TextMode="Number" min="0" max="31" step="1" ClientIDMode="Static" placeholder="<%$Resources:Tokens,Day4%>" aria-describedby="sizing-addon2" ToolTip="ضع اليوم فقط"></asp:TextBox>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Percentage%></span>
                                        <asp:TextBox ID="txtpercentage2" runat="server" class="form-control" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="100" step="1"></asp:TextBox>
                                        <span class="input-group-addon label-info" id="sizing-addon2">%</span>
                                    </div>

                                </div>
                                <div class="row">
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="31" ControlToValidate="txtday1"
                                        ErrorMessage="<%$Resources:Tokens,MonthValueBetween0And31%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="100" ControlToValidate="txtpercentage1"
                                        ErrorMessage="<%$Resources:Tokens,PercentageValueBetween0And100%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="31" ControlToValidate="txtday2"
                                        ErrorMessage="<%$Resources:Tokens,MonthValueBetween0And31%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="100" ControlToValidate="txtpercentage2"
                                        ErrorMessage="<%$Resources:Tokens,PercentageValueBetween0And100%>" />
                                </div>
                                <br />
                                <div class="row">
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Day3%></span>
                                        <asp:TextBox ID="txtday3" runat="server" class="form-control" placeholder="<%$Resources:Tokens,Day3%>" TextMode="Number" min="0" max="31" step="1" aria-describedby="sizing-addon2" ToolTip="ضع اليوم فقط"></asp:TextBox>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Percentage%></span>
                                        <asp:TextBox ID="txtpercentage3" runat="server" class="form-control" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="100" step="1"></asp:TextBox>
                                        <span class="input-group-addon label-warning" id="sizing-addon2">%</span>
                                    </div>

                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Day4%></span>
                                        <asp:TextBox ID="txtday4" runat="server" class="form-control" TextMode="Number" min="0" max="31" step="1" ClientIDMode="Static" placeholder="<%$Resources:Tokens,Day5%>" aria-describedby="sizing-addon2" ToolTip="ضع اليوم فقط"></asp:TextBox>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Percentage%></span>
                                        <asp:TextBox ID="txtpercentage4" runat="server" class="form-control" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="100" step="1"></asp:TextBox>
                                        <span class="input-group-addon label-info" id="sizing-addon2">%</span>
                                    </div>
                                </div>
                                <div class="row">
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="31" ControlToValidate="txtday3"
                                        ErrorMessage="<%$Resources:Tokens,MonthValueBetween0And31%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="100" ControlToValidate="txtpercentage3"
                                        ErrorMessage="<%$Resources:Tokens,PercentageValueBetween0And100%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="31" ControlToValidate="txtday4"
                                        ErrorMessage="<%$Resources:Tokens,MonthValueBetween0And31%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="100" ControlToValidate="txtpercentage4"
                                        ErrorMessage="<%$Resources:Tokens,PercentageValueBetween0And100%>" />
                                </div>
                                <br />
                                <div class="row">
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Day5%></span>
                                        <asp:TextBox ID="txtday5" runat="server" class="form-control" placeholder="<%$Resources:Tokens,Day3%>" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="31" step="1" ToolTip="ضع اليوم فقط"></asp:TextBox>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Percentage%></span>
                                        <asp:TextBox ID="txtpercentage5" runat="server" class="form-control" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="100" step="1"></asp:TextBox>
                                        <span class="input-group-addon label-warning" id="sizing-addon2">%</span>
                                        <%--<input type="text" class="form-control" placeholder="Username" aria-describedby="sizing-addon2">--%>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Day6%></span>
                                        <asp:TextBox ID="txtday6" runat="server" class="form-control" TextMode="Number" min="0" max="31" step="1" ClientIDMode="Static" placeholder="<%$Resources:Tokens,Day6%>" aria-describedby="sizing-addon2" ToolTip="ضع اليوم فقط"></asp:TextBox>
                                    </div>
                                    <div class="input-group col-lg-3 col-md-3 col-sm-6 col-xs-6">
                                        <span class="input-group-addon" id="sizing-addon2"><%=Tokens.Percentage%></span>
                                        <asp:TextBox ID="txtpercentage6" runat="server" class="form-control" aria-describedby="sizing-addon2" TextMode="Number" min="0" max="100" step="1"></asp:TextBox>
                                        <span class="input-group-addon label-info" id="sizing-addon2">%</span>
                                    </div>

                                </div>
                                <div class="row">
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="31" ControlToValidate="txtday5"
                                        ErrorMessage="<%$Resources:Tokens,MonthValueBetween0And31%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="100" ControlToValidate="txtpercentage5"
                                        ErrorMessage="<%$Resources:Tokens,PercentageValueBetween0And100%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="31" ControlToValidate="txtday6"
                                        ErrorMessage="<%$Resources:Tokens,MonthValueBetween0And31%>" />
                                    <asp:RangeValidator runat="server" Type="Integer" CssClass="marg"
                                        MinimumValue="0" MaximumValue="100" ControlToValidate="txtpercentage6"
                                        ErrorMessage="<%$Resources:Tokens,PercentageValueBetween0And100%>" />
                                </div>
                                <div>
                                    <asp:Label runat="server" Text="<%$Resources:Tokens,BranchesImplementInstallments %>"></asp:Label>
                                    <asp:CheckBoxList runat="server" ClientIDMode="Static" Width="100%" ID="ChkListBranchesForInstallments"
                                        RepeatDirection="Horizontal" />
                                </div>
                            </div>


                            <span class="">
                                <input type="checkbox" class="ace" runat="server" id="ckPendingRequestsAll" clientidmode="Static" /><span class="lbl">&nbsp;<%= Tokens.PendingRequestsOptions%></span>
                            </span>
                            <div id="pendingRequestsDiv" runat="server" clientidmode="Static" class="collapse row">
                                <div class="container">
                                    <h2><%= Tokens.PendingRequestsOptions%></h2>
                                    <%--<p>The panel-group class clears the bottom-margin. Try to remove the class and see what happens.</p>--%>
                                    <div class="panel-group">
                                        <div class="panel panel-primary col-md-4">
                                            <div class="panel-heading"><%= Tokens.Upgrade_Downgrade%></div>
                                            <div class="panel-body">
                                                <asp:CheckBoxList runat="server" ID="CkUpDwonOptions">
                                                    <asp:ListItem Value="فاتورة جزئية" Text="<%$Resources:Tokens,PartialInvoiceForOldSpeed %>" />
                                                    <asp:ListItem Value="دمج السرعين" Text="<%$Resources:Tokens,MergeSpeedsInOneInvoice %>" />
                                                    <asp:ListItem Value="شهر كامل" Text="<%$Resources:Tokens,MonthB4MonthAfter %>" />
                                                    <asp:ListItem Value="شهر كامل2" Text="<%$Resources:Tokens,Monthb4CurrentMonthRest %>" />
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="panel panel-success col-md-4">
                                            <div class="panel-heading"><%= Tokens.Unsuspend%></div>
                                            <div class="panel-body">
                                                <asp:CheckBoxList runat="server" ID="CkUnSuspend">
                                                    <asp:ListItem Value="ترحيل ايام السسبند" Text="<%$Resources:Tokens,PostponeSuspendDays %>" />
                                                    <asp:ListItem Value="تخصيم مع ثبات تاريخ المطالبة" Text="<%$Resources:Tokens,DeductionWithFixedRequestDate %>" />
                                                    <asp:ListItem Value="فاتورة كاملة" Text="<%$Resources:Tokens,CompleteInvoice %>" />
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                        <div class="panel panel-warning col-md-4">
                                            <div class="panel-heading"><%= Tokens.Suspend%></div>
                                            <div class="panel-body">
                                                <asp:CheckBoxList runat="server" ID="CkSuspend">
                                                    <asp:ListItem Value="تاكيد" Text="<%$Resources:Tokens,Confirm %>" />
                                                    <asp:ListItem Value="تأكيد مع التصفية" Text="<%$Resources:Tokens,ConfirmWithLiquidation %>" />
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>



                                    </div>
                                </div>
                            </div>

                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="autoreseller" /><span class="lbl">&nbsp;<%= Tokens.AutoResellerPayment %></span></label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" class="ace" runat="server" id="autobranch" /><span class="lbl">&nbsp;<%= Tokens.AutoBranchPayment%></span></label>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <button runat="server" id="BSave" class="btn btn-primary">
                <i class="icon-save icon-only"></i>&nbsp;<%= Tokens.Save %></button><div runat="server"
                    id="Msg">
                </div>
        </fieldset>
    </div>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = DisableText;
        check = document.getElementById('<%=checkDuration.ClientID %>');
        from = document.getElementById('<%=txtFrom.ClientID %>');
        to = document.getElementById('<%=txtTo.ClientID %>');
        to.onblur = validation;
        from.onblur = validationDays;
        function DisableText() {
            if (check.checked == true) {
                from.disabled = false;
                to.disabled = false;
            } else {
                from.disabled = true;
                from.value = "";
                to.value = "";
                to.disabled = true;
            }
        }


        function validation() {
            if (parseInt(from.value) > parseInt(to.value)) {
                alert('الفترة يجب ان تكون من يوم اقل الى يوم أكبر');
                to.value = "";
            }
            if (parseInt(from.value) > 31 || parseInt(to.value) > 31) {
                alert('عدد ايام الشهر اقل من 31 يوم');
                to.value = from.value = "";
            }
        }

        function validationDays() {
            if (parseInt(from.value) > 31 || parseInt(to.value) > 31) {
                alert('عدد ايام الشهر اقل من 31 يوم');
                to.value = from.value = "";
            }
        }

        $(document).ready(function () {
            $('#ReminderText').hide(); $('#suspendoption').hide();
            var val = $('#checkUnpaidDemand_1').attr('checked');
            if (val) {
                $('#ReminderText').show();
                $('#suspendoption').hide();
            } else {
                $('#ReminderText').hide();
                $('#suspendoption').show();
            }

            $('#checkUnpaidDemand_1').click(function () {
                $('#ReminderText').show();
                $('#suspendoption').hide();
            });
            $('#checkUnpaidDemand_0').click(function () {
                $('#ReminderText').hide();
                $('#suspendoption').show();
            });
            $('tr td input[type=checkbox]').addClass("ace");
            $('tr td input[type=radio]').addClass("ace");
            $('tr td label').addClass("lbl");
        });

        $(document).ready(function () {
            $('#ChkboxResellerdebt').change(function () {
                if (this.checked)
                    $('#Resellerdebt').fadeIn('slow');
                else
                    $('#Resellerdebt').fadeOut('slow');

            });

            $('#ckPendingRequestsAll').change(function () {
                if (this.checked)
                    $('#pendingRequestsDiv').fadeIn('slow');
                else {
                    $('#pendingRequestsDiv').fadeOut('slow');
                    $("INPUT[id^='ctl00_ContentPlaceHolder1_CkUpDwonOptions_']").removeAttr('checked');
                    $("INPUT[id^='ctl00_ContentPlaceHolder1_CkUnSuspend_']").removeAttr('checked');
                    $("INPUT[id^='ctl00_ContentPlaceHolder1_CkSuspend_']").removeAttr('checked');
                }

            });
        });

    </script>
</asp:Content>

