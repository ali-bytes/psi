<%@ Page Title="<%$Resources:Tokens,AdvancedSearch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="AdvancedSearch.aspx.cs" Inherits="NewIspNL.Pages.AdvancedSearch" %>


<%@ Import Namespace="System.Linq.Dynamic" %>
<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="Microsoft.Ajax.Utilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
    <style type="text/css">
        .table {
            display: block;
            overflow-x: scroll;
        }
    </style>
    <div class="page-header">
        <h1>
            <asp:Literal ID="Literal8" Text="<%$Resources:Tokens,Search %>" runat="server" /></h1>
    </div>
    <div class="row">
    <div id="Msg" runat="server"></div>
        <div class="col-sm-12" runat="server" id="ser">
            <div class="row">
                <div class="widget-box">
                    <div class="widget-header">
                        <h4>
                            <%=Tokens.SearchOptions%></h4>
                        <span class="widget-toolbar" style="padding: 12px"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a><a href="#" data-action="reload"><i class="icon-refresh"></i></a><a href="#"
                            data-action="close"><i class="icon-remove"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main no-padding">
                            <div>
                                <!-- <legend>Form</legend> -->
                                <fieldset style="padding: 16px;">
                                    <div>
                                        <table class="search">
                                            <tr>
                                                <td>
                                                    <label for="TbName">
                                                        <asp:Literal Text="<%$Resources:Tokens,Name %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbName" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="DdlGovernorate">
                                                        <asp:Literal Text="<%$Resources:Tokens,Governorate %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlGovernorate" EnableViewState="True"
                                                        OnSelectedIndexChanged="DdlGovernorate_SelectedIndexChanged" AutoPostBack="True" Width="180px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="TbPhone">
                                                        <asp:Literal Text="<%$Resources:Tokens,Phone %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbPhone" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="DdlIpPackages">
                                                        <asp:Literal Text="<%$Resources:Tokens,Ip.Package %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlIpPackages" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="TbMobile">
                                                        <asp:Literal Text="<%$Resources:Tokens,Mobile %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbMobile" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="TbEmail">
                                                        <asp:Literal Text="<%$Resources:Tokens,Email %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbEmail" ClientIDMode="Static" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="DdlProvider">
                                                        <asp:Literal Text="<%$Resources:Tokens,Provider %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlProvider" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="DdlPackage">
                                                        <asp:Literal Text="<%$Resources:Tokens,Package %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlPackage" EnableViewState="True" Width="220px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="DdlReseller">
                                                        <asp:Literal Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlReseller" CssClass="chosen" runat="server" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="DdlBranchs">
                                                        <asp:Literal Text="<%$Resources:Tokens,Branch %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DdlBranchs" CssClass="chosen" runat="server" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="TbVPI">
                                                        <asp:Literal Text="<%$Resources:Tokens,VPI %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbVPI" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="TbVCI">
                                                        <asp:Literal Text="<%$Resources:Tokens,VCI %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbVCI" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="TbUserName">
                                                        <asp:Literal Text="<%$Resources:Tokens, UserName%>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="TbUserName" EnableViewState="True" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="DdlStatus">
                                                        <asp:Literal Text="<%$Resources:Tokens, Status%>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlStatus" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label for="DdlOffer">
                                                        <asp:Literal Text="<%$Resources:Tokens,Offer %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlOffer" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                                <td>
                                                    <label for="DdlCentral">
                                                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Central %>" runat="server" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlCentral" EnableViewState="True" Width="180px" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        <asp:Literal runat="server" Text="<%$Resources:Tokens,PaymentType %>" ID="lblpaymentType"></asp:Literal>
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="chosen" ID="DdlPaymentType" DataTextField="PaymentTypeName"
                                                        DataValueField="ID" Width="180px" /></td>
                                                <td></td>
                                                <td>
                                                    <asp:Literal runat="server" ID="lblFromRequestDate"></asp:Literal></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFromRequestDate" data-calender="db"></asp:TextBox></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3"></td>
                                                <td>
                                                    <asp:Literal runat="server" ID="lblToRequestDate" Text="<%$Resources:Tokens,To %>"></asp:Literal></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToRequestDate" data-calender="db"></asp:TextBox></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div>
                                                        <label>
                                                            <input type="checkbox" runat="server" id="Check24Month" class="ace" /><span class="lbl">&nbsp;<%= Tokens._24MonthPathed %></span></label>
                                                    </div>
                                                </td>
                                                <td></td>

                                                <td colspan="2">
                                                    <div>
                                                        <label>
                                                            <input type="checkbox" runat="server" id="CheckPrePaid" class="ace" /><span class="lbl">&nbsp;<%= Tokens.Prepaid %></span></label>
                                                    </div>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <input type="checkbox" runat="server" id="ChcekFullData" class="ace" /><span class="lbl">&nbsp;<%=Tokens.ShowCount %></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                                <div class="form-actions center" style="margin: 0;">
                                    <button type="button" id="btn_Search" clientidmode="Static" runat="server" onserverclick="BSearch_OnClick" class="btn btn-success">
                                        <i class="icon-search icon-on-right bigger-110"></i>&nbsp;<asp:Literal Text="<%$Resources:Tokens,Search %>"
                                            runat="server" /></button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <% if (Results != null && Results.Any())
           { %>

        <div class="col-sm-12">
            <div class="row">
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$Resources:Tokens,Results %>" runat="server" />
                    &nbsp; (<%= DynamicQueryable.Count(Results.DistinctBy(a=>a.Id)) %>)</h3>
                <div class="row">
                <div id="Changeset" runat="server" class="col-md-1">
                    <button class="btn btn-primary" type="button" id="changeSettings">
                        <i class="icon-cog icon-only"></i>&nbsp;<%= Tokens.ChangeSettings %></button>
                </div>
                    <div class="col-md-1">
                <button id="Button1" data-send="sms" class="btn btn-success" title="<%=Tokens.SendsmsMessage %>" data-rel="tooltip"><%=Tokens.SendsmsMessage %>&nbsp;<i class="icon-mobile-phone bigger-120"></i></button>
                </div>
                    </div>
                <br />
                <% if (!ChcekFullData.Checked)
                   { %>
                <asp:ListView ID="ListView1" runat="server" ItemPlaceholderID="content">
                    <LayoutTemplate>
                        <table id="advancetbl" class="table table-bordered table-condensed " border="1px" style="text-align: right">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>
                                        <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Customer %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal3" Text="<%$Resources:Tokens,Phone %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal13" Text="<%$Resources:Tokens,Mobile %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal19" Text="<%$Resources:Tokens,UserName %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal runat="server" ID="litera" Text="<%$Resources:Tokens,RequestNumber %>"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Offer %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,State %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal6" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal7" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal18" Text="<%$Resources:Tokens,PaymentType %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal runat="server" ID="lblProvider" Text="<%$Resources:Tokens,Service.Provider %>"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal12" Text="<%$Resources:Tokens,Central %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal14" Text="<%$Resources:Tokens,Package %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal15" Text="<%$Resources:Tokens,Activation.Date %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal16" Text="<%$Resources:Tokens,CreationDate %>" runat="server" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal17" Text="<%$Resources:Tokens,RequestDate %>" runat="server" />
                                    </th>
                                    <th></th>
                                    <th>
                                        <input type="checkbox" id="selectall" style="display: <%= GroupId == 1 ? "block" : "none" %>;" />
                                    </th>
                                </tr>
                            </thead>
                            <asp:PlaceHolder ID="myItemPlaceHolder" runat="server">

                                <tbody runat="server" class="tbody" id="content"></tbody>
                            </asp:PlaceHolder>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class="tablerow">
                            <td></td>
                            <td>
                                <%# Eval("Customer") %>
                            </td>
                            <td>
                                <%# Eval("Phone") %>
                            </td>
                            <td>
                                <%# Eval("Mobile")%>
                            </td>
                            <td>
                                <%# Eval("UserName")%>
                            </td>
                            <td>
                                <%# Eval("RequestNumber") %>
                            </td>
                            <td>
                                <%# Eval("Offer") %>
                            </td>
                            <td>
                                <%# Eval("State") %>
                            </td>
                            <td>
                                <%# Eval("Branch") %>
                            </td>
                            <td>
                                <%# Eval("Reseller") %>
                            </td>
                            <td>
                                <%# Eval("PaymentType") %>
                            </td>
                            <td>
                                <%# Eval("ServicProvider") %>
                            </td>
                            <td>
                                <%# Eval("Central") %>
                            </td>
                            <td>
                                <%# Eval("Package") %>
                            </td>
                            <td>
                                <%# Eval("ActivationDate") %>
                            </td>
                            <td>
                                <%# Eval("CreationDate") %>
                            </td>
                            <td>
                                <%# Eval("RequestDate") %>
                            </td>

                            <td style="width: 100px;">
                                <% if (CanEdit)%>
                                <%{ %>
                                <asp:LinkButton ID="LinkBtnEdit" runat="server" CssClass="btn btn-primary btn-xs" CommandArgument='<%# Bind("Id") %>' OnCommand="LinkBtnEdit_Command" OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 500);"><i class="icon-edit icon-only bigger-120"></i></asp:LinkButton>

                                <%--<a style="text-decoration: none" class="btn btn-primary btn-xs" data-rel="tooltip" title='<%= Tokens.Edit %>' href='<%# Eval("Id","EditCustomer.aspx?NID={0}") %>' target="_blank">
                                        <i class="icon-edit icon-only bigger-120"></i>
                                    </a>--%>&nbsp;&nbsp;
                                <%} %>
                                <asp:LinkButton ID="LinkBtnDetails" runat="server" CssClass="btn btn-success btn-xs" CommandArgument='<%# Bind("Id") %>' OnCommand="LinkBtnDetails_Command" OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 500);"><i class="icon-building icon-only bigger-120"></i></asp:LinkButton>

                                <%-- <a style="text-decoration: none" class="btn btn-success btn-xs" data-rel="tooltip" title='<%= Tokens.Details %>' href='<%# Eval("Id","CustomerDetails.aspx?NID={0}")%>' target="_blank">
                                    <i class="icon-building icon-only bigger-120"></i>
                                </a>--%>
                            </td>
                            <td>
                                <input type="checkbox" name="ck" value="<%# Eval("Id") %>" data-val="<%# Eval("Id") %>" selector="cb" />
                            </td>
                        </tr>

                    </ItemTemplate>
                </asp:ListView>

                <%-- <asp:Button CssClass="btn btn-success" runat="server" ID="Export" Visible="False" Text="<%$Resources:Tokens,Export %>"
                        OnClick="Export_OnClick" />--%>
            </div>
        </div>
        <% }
       }
           else
           { %>
        <label>
            <asp:Literal ID="Literal10" Text="<%$Resources:Tokens,NoResults %>" runat="server" />
        </label>
        <% } %>

        <asp:HiddenField runat="server" ID="HfIds" ClientIDMode="Static" />
        <div class="bootbox modal fade" id="sets">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="myModalLabel" class="modal-title">
                            <%=Tokens.ChangeSettings%></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <div class="well">
                                    <div>
                                        <div>
                                            <label for="DdlReseller2">
                                                <asp:Literal Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList ID="DdlReseller2" runat="server" EnableViewState="True" />
                                            </div>
                                        </div>
                                        <div>
                                            <label for="DdlBranchs2">
                                                <asp:Literal Text="<%$Resources:Tokens,Branch %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList ID="DdlBranchs2" runat="server" EnableViewState="True" />
                                            </div>
                                        </div>
                                        <div>
                                            <div>
                                                <label for="DdlGovernorate2">
                                                    <asp:Literal Text="<%$Resources:Tokens,Governorate %>" runat="server" />
                                                </label>
                                                <div>
                                                    <asp:DropDownList runat="server" ID="DdlGovernorate2" EnableViewState="True" />
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <label for="DdlProvider2">
                                                <asp:Literal Text="<%$Resources:Tokens,Provider %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList runat="server" ID="DdlProvider2" EnableViewState="True" />
                                            </div>
                                        </div>
                                        <div>
                                            <label for="DdlPackage2">
                                                <asp:Literal Text="<%$Resources:Tokens,Package %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList runat="server" ID="DdlPackage2" EnableViewState="True" />
                                            </div>
                                        </div>
                                        <div>
                                            <label for="DdlCentral2">
                                                <asp:Literal Text="<%$Resources:Tokens,Central %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList runat="server" ID="DdlCentral2" EnableViewState="True" />
                                            </div>
                                        </div>
                                        <div>
                                            <label for="DdlPaymentTypes">
                                                <asp:Literal Text="<%$Resources:Tokens,PaymentType %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:DropDownList runat="server" ID="DdlPaymentTypes" EnableViewState="True" />
                                            </div>
                                        </div>
                                        <div>
                                            <label>
                                                <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Tokens,Offer %>"></asp:Literal></label>
                                            <div>
                                                <asp:DropDownList runat="server" ID="DdlOffersets" EnableViewState="True" /></div>
                                        </div>
                                        <label>
                                            <input style="display: initial" type="checkbox" runat="server" id="ToDirctCustomer" /><%= Tokens.DirectCustomer %></label>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="Button1" class="btn btn-primary" runat="server" onserverclick="SaveNewSettings">
                            <asp:Literal ID="Literal9" Text="<%$Resources:Tokens,Save %>" runat="server" />
                        </button>
                        &nbsp;
                    <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                    </div>
                </div>
            </div>
        </div>
        <div id="SmsModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="H1">
                            <%=Tokens.SendsmsMessage%></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div>
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,Message %>" ID="lblMessabe"></asp:Literal>
                                <div>
                                    <asp:TextBox runat="server" ID="txtMessageText" ClientIDMode="Static" TextMode="MultiLine" CssClass="required-input col-xs-10 col-sm-5" Width="200px"></asp:TextBox>
                                    <span class="help-inline col-xs-12 col-sm-3">
                                        <span class="middle">
                                            <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,littersLimit %>"></asp:Literal></span>
                                    </span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMessageText" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="sms"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="Button2" class="btn btn-info" type="button" runat="server" onserverclick="SendsmsSelected" validationgroup="sms">
                            <i class="icon-ok bigger-110"></i>
                            &nbsp;<asp:Literal runat="server" ID="Literal20" Text="<%$Resources:Tokens,Send %>"></asp:Literal>
                        </button>
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <i class="icon-white icon-arrow-left"></i>
                            <%=Tokens.Cancel %></button>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript" src="~/Content/ace-assest/js/chosen.jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $(document).keypress(function (e) {
                var key = e.which;
                if (key === 13) {
                    $('#btn_Search').click();
                    return false;
                } else {
                    return true;
                }
            });

            jQuery(".chosen").chosen();
            $('input[data-calender="db"]').datepicker({ dateFormat: 'dd-mm-yy' });
        });
        $(document).ready(function () {
            var table = $('#advancetbl').dataTable({
                "iDisplayLength": 50,
                "aLengthMenu": [[10, 25, 50, 100, 250, 500, -1], [10, 25, 50, 100, 250, 500, "All"]]
            });
            var tableTools = new $.fn.dataTable.TableTools(table, {
                'aButtons': [
                    {
                        'sExtends': 'xls',
                        'sButtonText': 'Save to Excel',
                        'sFileName': 'Data.xls'
                    },
                    {
                        'sExtends': 'print',
                        'bShowAll': true
                    },
                    'copy'
                ],
                'sSwfPath': '//cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf'
            });
            $(tableTools.fnContainer()).insertBefore('#advancetbl_wrapper');
        });
        $(document).ready(function () {
            $('.tablerow').each(function (i) {
                $("td:first", this).html(i + 1);
            });

            var ids = [];
            var inputs = $('input[selector="cb"]');

            var dlg = $('#sets');

            $('#changeSettings').click(function () {
                if (ids.length < 1) {
                    alert('<%=Tokens.ChoseCustomers %>');
                    return true;
                }
                //$(dlg).dialog('open');
                dlg.modal('show');
                $('#HfIds').val(ids.join());
                return false;
            });


            $('#changeSettings').bind('click', function () {
                console.log(ids);
            });

            $(inputs).bind('click', function () {
                updateIds(inputs);
                console.log(ids.join());

            });

            $('button[data-send="sms"]').click(function () {

                var totalRows = $("#advancetbl tr").length;
                if (totalRows > 0) {
                    var checks = $('#advancetbl input[type="checkbox"]');

                    for (var i = 0; i < checks.length; i++) {
                        if ($(checks[i]).is(':checked')) {
                            $('#SmsModal').modal('show');
                            return false;
                        }
                    }
                    alert('<%=Tokens.SelectDemands %>');
                    return false;
                 }
                alert('<%=Tokens.NoResults %>');
                return true;
            });

            $('#selectall').click(function () {
                if (this.checked) {
                    $('input[selector="cb"]').each(function () {
                        this.checked = true;
                    });
                } else {
                    $('input[selector="cb"]').each(function () {
                        this.checked = false;
                    });
                }
                updateIds(inputs);
            });
            function updateIds(inputs) {
                ids = [];
                for (var i = 0; i < inputs.length; i++) {
                    if ($(inputs[i]).is(':checked')) {
                        ids.push($(inputs[i]).attr("data-val"));
                    }
                }
            }

            //pagination
            $("table.table").each(function () {
                var currentPage = 0;
                var numPerPage = 250;
                var $table = $(this);
                $table.bind('repaginate', function () {
                    $table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
                });
            });
            //    $table.trigger('repaginate');
            //    var numRows = $table.find('tbody tr').length;
            //    var numPages = Math.ceil(numRows / numPerPage);
            //    var $pager = $("<ul class='pagination'></ul>");
            //    for (var page = 0; page < numPages; page++) {
            //        var num = page + 1;
            //        $("<li></li>").html("<a>"+num+"</a>").bind('click', {
            //            newPage: page
            //        }, function(event) {
            //            currentPage = event.data['newPage'];
            //            $table.trigger('repaginate');
            //            $(this).addClass('active').siblings().removeClass('active');
            //        }).appendTo($pager); //.addClass('clickable');
            //    }
            //    $pager.insertBefore($table).find('span.page-number:first').addClass('active');
            //});

        });


    </script>
</asp:Content>
