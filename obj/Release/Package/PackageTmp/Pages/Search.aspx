<%@ Page Title="<%$ Resources:Tokens,Search %>" EnableEventValidation="false" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="NewIspNL.Pages.Search" %>

<%@ Import Namespace="Resources" %>
<%@ Register Src="../WebUserControls/UserFile.ascx" TagName="UserFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Content/airview.min.css" rel="stylesheet" />
    <script src="../Scripts/airview.min.js"></script>
    <style type="text/css">
        .allbutt button, .allbutt input {
            max-width: 127px;
        }

        .btn-custom {
            background-color: hsl(195, 79%, 53%) !important;
            background-repeat: repeat-x;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#28b6e5", endColorstr="#28b6e5");
            background-image: -khtml-gradient(linear, left top, left bottom, from(#28b6e5), to(#28b6e5));
            background-image: -moz-linear-gradient(top, #28b6e5, #28b6e5);
            background-image: -ms-linear-gradient(top, #28b6e5, #28b6e5);
            background-image: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #28b6e5), color-stop(100%, #28b6e5));
            background-image: -webkit-linear-gradient(top, #28b6e5, #28b6e5);
            background-image: -o-linear-gradient(top, #28b6e5, #28b6e5);
            background-image: linear-gradient(#28b6e5, #28b6e5);
            border-color: #28b6e5 #28b6e5 hsl(195, 79%, 53%);
            color: #333 !important;
            text-shadow: 0 1px 1px rgba(255, 255, 255, 0.00);
            -webkit-font-smoothing: antialiased;
        }

        .btn-p {
            background-color: hsl(145, 62%, 78%) !important;
            background-repeat: repeat-x;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr="#a4e9c1", endColorstr="#a4e9c1");
            background-image: -khtml-gradient(linear, left top, left bottom, from(#a4e9c1), to(#a4e9c1));
            background-image: -moz-linear-gradient(top, #a4e9c1, #a4e9c1);
            background-image: -ms-linear-gradient(top, #a4e9c1, #a4e9c1);
            background-image: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #a4e9c1), color-stop(100%, #a4e9c1));
            background-image: -webkit-linear-gradient(top, #a4e9c1, #a4e9c1);
            background-image: -o-linear-gradient(top, #a4e9c1, #a4e9c1);
            background-image: linear-gradient(#a4e9c1, #a4e9c1);
            border-color: #a4e9c1 #a4e9c1 hsl(145, 62%, 78%);
            color: #333 !important;
            text-shadow: 0 1px 1px rgba(255, 255, 255, 0.00);
            -webkit-font-smoothing: antialiased;
        }

        .srouter {
	-moz-box-shadow: 0px 10px 14px -7px #3e7327;
	-webkit-box-shadow: 0px 10px 14px -7px #3e7327;
	box-shadow: 0px 10px 14px -7px #3e7327;
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #77b55a), color-stop(1, #72b352));
	background:-moz-linear-gradient(top, #77b55a 5%, #72b352 100%);
	background:-webkit-linear-gradient(top, #77b55a 5%, #72b352 100%);
	background:-o-linear-gradient(top, #77b55a 5%, #72b352 100%);
	background:-ms-linear-gradient(top, #77b55a 5%, #72b352 100%);
	background:linear-gradient(to bottom, #77b55a 5%, #72b352 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#77b55a', endColorstr='#72b352',GradientType=0);
	background-color:#77b55a;
	-moz-border-radius:4px;
	-webkit-border-radius:4px;
	border-radius:4px;
	border:1px solid #4b8f29;
	display:inline-block;
	cursor:pointer;
	color:#ffffff;
	font-family:Arial;
	font-size:13px;
	font-weight:bold;
	padding:6px 12px;
	text-decoration:none;
	text-shadow:0px 1px 0px #5b8a3c;
}
.srouter:hover {
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #72b352), color-stop(1, #77b55a));
	background:-moz-linear-gradient(top, #72b352 5%, #77b55a 100%);
	background:-webkit-linear-gradient(top, #72b352 5%, #77b55a 100%);
	background:-o-linear-gradient(top, #72b352 5%, #77b55a 100%);
	background:-ms-linear-gradient(top, #72b352 5%, #77b55a 100%);
	background:linear-gradient(to bottom, #72b352 5%, #77b55a 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#72b352', endColorstr='#77b55a',GradientType=0);
	background-color:#72b352;
}
.srouter:active {
	position:relative;
	top:1px;
}
.reciever {
	-moz-box-shadow: 0px 10px 14px -7px #276873;
	-webkit-box-shadow: 0px 10px 14px -7px #276873;
	box-shadow: 0px 10px 14px -7px #276873;
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #599bb3), color-stop(1, #408c99));
	background:-moz-linear-gradient(top, #599bb3 5%, #408c99 100%);
	background:-webkit-linear-gradient(top, #599bb3 5%, #408c99 100%);
	background:-o-linear-gradient(top, #599bb3 5%, #408c99 100%);
	background:-ms-linear-gradient(top, #599bb3 5%, #408c99 100%);
	background:linear-gradient(to bottom, #599bb3 5%, #408c99 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#599bb3', endColorstr='#408c99',GradientType=0);
	background-color:#599bb3;
	-moz-border-radius:4px;
	-webkit-border-radius:4px;
	border-radius:4px;
	border:1px solid #29668f;
	display:inline-block;
	cursor:pointer;
	color:#ffffff;
	font-family:Arial;
	font-size:13px;
	font-weight:bold;
	padding:6px 12px;
	text-decoration:none;
	text-shadow:0px 1px 0px #3d768a;
}
.reciever:hover {
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #408c99), color-stop(1, #599bb3));
	background:-moz-linear-gradient(top, #408c99 5%, #599bb3 100%);
	background:-webkit-linear-gradient(top, #408c99 5%, #599bb3 100%);
	background:-o-linear-gradient(top, #408c99 5%, #599bb3 100%);
	background:-ms-linear-gradient(top, #408c99 5%, #599bb3 100%);
	background:linear-gradient(to bottom, #408c99 5%, #599bb3 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#408c99', endColorstr='#599bb3',GradientType=0);
	background-color:#408c99;
}
.reciever:active {
	position:relative;
	top:1px;
}


    </style>
    <div>
        <div id="tr_Search" runat="server">
            <div class="view">
                <fieldset>
                    <div class="page-header">
                        <h1>
                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Tokens,Search %>"></asp:Label></h1>
                    </div>
                    <div id="Div1" runat="server"></div>
                    <br />
                    <div id="Div2" runat="server"></div>
                    <div>
                        <div class="well">
                            <div>
                                <div>
                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Tokens,SearchOptions %>"></asp:Label>
                                    <div>
                                        <asp:RadioButtonList ID="rbl_searchType" runat="server" CellPadding="3" CellSpacing="5" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Value="1" Text="<%$ Resources:Tokens,Phone %>"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="<%$ Resources:Tokens,Mobile %>"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="<%$ Resources:Tokens,Name %>"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="<%$ Resources:Tokens,UserName %>"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <div style="width: 113px">
                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,SearchValue %>"></asp:Label>
                                </div>
                                <div>
                                    <asp:TextBox ID="txt_CustomerPhone0" ClientIDMode="Static" runat="server" Width="170px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_CustomerPhone0" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div runat="server" id="GovDiv">
                                <label for="DdlReseller">
                                    <%= Tokens.Governorate %></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlGovernorate">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RddlGovernorate" ErrorMessage="*" ControlToValidate="DdlGovernorate"
                                        runat="server" />
                                </div>
                            </div>
                            <p>
                                <br />
                                <asp:Button ID="btn_search" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Tokens,Search %>"
                                    Width="100px" OnClick="btn_search_Click" ValidationGroup="SearchVG" ClientIDMode="Static" />
                            </p>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:Tokens,Results %>"></asp:Label>
                </h3>
                <table class="table table-bordered table-condensed" id="get-result" clientidmode="Static">
                    <thead>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <br />
                <asp:Button ID="external_data" runat="server"
                    CssClass="btn btn-primary" Text="Portal data" OnClick="external_data_Click"></asp:Button>
                <br />
                <asp:GridView ID="grd_externalData" CssClass="table table-bordered table-condensed" runat="server" ItemStyle-HorizontalAlign="Left">
                </asp:GridView>
                <br />
                <asp:GridView ID="grd_usage" CssClass="table table-bordered table-condensed" runat="server" HorizontalAlign="Left">
                </asp:GridView>
                <br />
                <div id="grd_resellerData" runat="server">
                    <h3 class="header smaller lighter blue">
                        <%=Tokens.Reseller %></h3>
                    <asp:GridView ID="grd_resellerData1" CssClass="table table-bordered table-condensed" runat="server" HorizontalAlign="Left">
                    </asp:GridView>
                </div>
                <br />
                <asp:GridView ID="grd_wo" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed center"
                    ClientIDMode="Static" OnDataBound="grd_wo_DataBound" OnRowDataBound="grd_wo_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="no" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Governrate %>">
                            <ItemTemplate>
                                <asp:Label ID="l_governate" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Central %>">
                            <ItemTemplate>
                                <asp:Label ID="central" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Package %>">
                            <ItemTemplate>
                                <asp:Label ID="l_speed" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Provider %>">
                            <ItemTemplate>
                                <asp:Label ID="l_provider" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ResellerName" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Branch %>">
                            <ItemTemplate>
                                <asp:Label ID="l_branch" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PayType" HeaderText="<%$ Resources:Tokens,PaymentType %>" />
                        <asp:BoundField DataField="IpPackageName" HeaderText="<%$Resources:Tokens,Ip.Package %>" />
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                        <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />

                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Activation.Date %>">
                            <ItemTemplate>
                                <asp:Label ID="l_activation" runat="server"></asp:Label>
                                <asp:HiddenField ID="hf_id" runat="server" Value='<%# Bind("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Offer %>">
                            <ItemTemplate>
                                <asp:Label ID="l_offer" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,InvoiceDueDate %>">
                            <ItemTemplate>
                                <asp:Label ID="lRequestDate" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="lastStatusDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="<%$ Resources:Tokens,LastStatusDate %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="width: 121px">
                                    <% if (CanEdit)
                                       { %>
                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="btn_DirectEdit" runat="server"
                                        CssClass="btn btn-primary btn-xs" data-rel="tooltip" ToolTip="<%$ Resources:Tokens,Edit %>"
                                        OnClick="btn_DirectEdit_Click"><i class="icon-edit bigger-120"></i></asp:LinkButton>&nbsp;<% } %><asp:LinkButton ID="lnb_Edit" CssClass="btn btn-success btn-xs" runat="server" CommandArgument='<%# Eval("ID") %>'
                                            OnClick="lnb_Edit_Click" data-rel="tooltip" ToolTip='<%$ Resources:Tokens,Details %>'><i class="icon-building bigger-120"></i></asp:LinkButton>




                                    <a data-rel="tooltip" title="<%=Tokens.Call%>" id="lnkcall" class="btn btn-info btn-xs"
                                        data-woid='<%# Eval("ID") %>'><i class="icon-phone bigger-120"></i></a>
                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="btn_Directcon" runat="server"
                                        CssClass="btn btn-info btn-xs" data-rel="tooltip" ToolTip="<%$ Resources:Tokens,Contract %>"
                                        OnClientClick="aspnetForm.target ='_blank';" OnClick="btn_Directcan_Click">
                                        <i class="icon-only icon-print bigger-120">
                                            </i></asp:LinkButton>


                                    <% if (CanDelete)
                                       { %>
                                    <asp:LinkButton CssClass="btn btn-danger btn-xs" ID="btn_Delete" OnClientClick="return areyousure()"
                                        runat="server" ToolTip="<%$Resources:Tokens,Delete %>" OnClick="btn_Delete_Click"
                                        CausesValidation="False" data-rel="tooltip" CommandArgument='<%# Eval("ID") %>'><i class="icon-trash icon-only bigger-120"></i></asp:LinkButton><% } %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label19" runat="server" Font-Bold="True" ForeColor="#FF3300" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button CssClass="btn btn-success" runat="server" ID="toExcel" Text="<%$ Resources:Tokens,Export %>"
                            OnClick="b2_OnClick" Visible="False" />&nbsp;
                        <asp:Button ID="linkRouter" runat="server" Visible="False" OnClick="linkRouter_Click"
                            CssClass="btn btn-primary" Text="اقرار استلام الراوتر"></asp:Button><%--onclick="showDiv('MyDiv2')"--%>
                        &nbsp;
                        <asp:Button ID="linkRunOrderTEdata" runat="server" Visible="False" Text="أمر تشغيل TE Data"
                            OnClick="linkRunOrderTEdata_Click" CssClass="btn btn-danger" />
                        &nbsp;
                        <asp:Button ID="linkRunOrderEtisalat" runat="server" Visible="False" Text="أمر تشغيل Etisalat"
                            CssClass="btn btn-success" OnClick="linkRunOrderEtisalate_Click" />
                        <% if (AddTicket)
                           {%>
                        <asp:Button ID="btn_AddTicket" CssClass="btn btn-danger" runat="server" Text="<%$ Resources:Tokens,Add.Ticket %>"
                            OnClick="btn_AddTicket_Click" Width="120px" Visible="False" /><% }%>
                        &nbsp;
                        <% if (CustomerDemand)
                           { %>
                        <a runat="server" id="ADemanedsLink" class="btn btn-info" target="_blank" visible="False">
                            <%= Tokens.CustomerDemands %></a><% } %>
                         &nbsp;
                        <asp:Button ID="btnNotDirect" runat="server" CssClass="btn btn-purple" Text="<%$ Resources:Tokens,ResellerData %>" OnClick="btnNotDirect_Click" />
                    </div>

                </div>
                <div>
                    <h3 class="header smaller lighter blue">
                        <%=Tokens.Add_Request %></h3>
                    <p runat="server" id="divRequests" visible="False" class="allbutt">
                        <button id="btnUpDoenGrade" class="btn btn-warning" type="button" runat="server" clientidmode="Static">
                            <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Tokens,MenuUpgradeDowngrade %>"></asp:Literal></button>
                        <asp:Button runat="server" ID="btnSuspend" OnClientClick="return areyousure()" CssClass="btn btn-danger"
                            OnClick="Send" Text="<%$Resources:Tokens,MenuSuspend %>" />
                        <asp:Button runat="server" ID="btnUnSuspend" OnClientClick="return areyousure()"
                            CssClass="btn btn-inverse" OnClick="Send" Text="<%$Resources:Tokens,MenuUnsuspend %>" />
                        <asp:Button runat="server" ID="btnCancel" OnClientClick="return areyousure()" CssClass="btn btn-pink"
                            OnClick="Send" Text="<%$Resources:Tokens,MenuCancel %>" />
                        <button id="reactiveModal" type="button" class="btn btn-purple">
                            <asp:Literal ID="Literal18" runat="server" Text="<%$Resources:Tokens,MenuReactivate %>"></asp:Literal></button>
                        <asp:Button runat="server" ID="btnHold" OnClientClick="return areyousure()" CssClass="btn btn-yellow"
                            OnClick="Send" Text="<%$Resources:Tokens,MenuCustomerHold %>" />
                        <asp:Button runat="server" ID="btnUnHold" OnClientClick="return areyousure()" CssClass="btn btn-grey"
                            Text="<%$Resources:Tokens,MenuUnhold %>" OnClick="Send" />
                        <button id="btnchangeIP" class="btn btn-light" type="button" runat="server" clientidmode="Static">
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,MenuChangeIPPackage %>" ID="lblchangeIP"></asp:Literal></button>
                        <button id="btnExtraGiga" class="btn btn-info" type="button" runat="server" clientidmode="Static">
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,MenuRequestExtraGiga%>" ID="lblxtra"></asp:Literal></button>
                        <button id="btnChangeOffer" class="btn btn-default" type="button" runat="server" clientidmode="Static">
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,ChangeOfferRequest %>" ID="lblchangeoffer"></asp:Literal></button>
                        <a runat="server" id="btnChangeRequestDate" class="btn btn-custom" target="_blank">
                            <%= Tokens.ChangeRequestDate %></a>
                        <a runat="server" id="btnChangeActivationDate" class="btn btn-p" target="_blank">
                            <%= Tokens.ChangeActivationDate %></a>

                        <%--  <button id="btnChangeActivationDate" class="btn btn-p" type="button" runat="server" clientidmode="Static">
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,ChangeActivationDate %>" ID="lblChangeActivationDate"></asp:Literal></button>--%>
                    </p>

                    <div style="margin: 0px; padding: 0px; clear: both; height: 6px">
                        &nbsp;
                    </div>
                    <p>
                        <a runat="server" id="SendRouters" class="srouter" clientidmode="Static" target="_blank">
                            <%= Tokens.SendRouters %></a>
                        <a runat="server" id="reciverouters" class="reciever" clientidmode="Static" target="_blank">
                            <%= Tokens.reciverouters %></a>
                    </p>
                </div>
                <asp:Label runat="server" ID="lbl_InsertResult"></asp:Label><br />

                <div runat="server" id="divRejectRequest" visible="False">
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="lblrequest" runat="server"></asp:Label>
                    </h3>
                    <asp:GridView ID="GridView2" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-condensed text-center" Style="margin-right: -18px;"
                        DataKeyNames="ID" OnRowDataBound="grd_Requests_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_No" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>" />
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>" />
                            <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$ Resources:Tokens,Central%>" />
                            <asp:BoundField DataField="CurrentServicePackageName" HeaderText="<%$ Resources:Tokens,CurrentSpeed %>" />
                            <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>" />
                            <asp:BoundField DataField="SuspenDaysCount" HeaderText="<%$ Resources:Tokens,SuspendDaysCount %>" />
                            <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>" />
                            <asp:BoundField DataField="SenderName" HeaderText="<%$Resources:Tokens,SenderName %>" />
                            <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="TActivationDate" HeaderText="<%$ Resources:Tokens,Activation.Date %>" />
                            <asp:BoundField HeaderText="<%$ Resources:Tokens,Offer%>" DataField="Title" />
                            <asp:BoundField DataField="NewServicePackageName" HeaderText="<%$ Resources:Tokens,New.Service.Package %>" />
                            <asp:BoundField DataField="IpPackageName" HeaderText="<%$ Resources:Tokens,New.IP.Package %>" />
                            <asp:BoundField DataField="ExtraGiga" HeaderText="<%$ Resources:Tokens,Extra.Gigas%>" />
                            <asp:BoundField DataField="TRequestDate" HeaderText="<%$ Resources:Tokens,Request.Date %>" />
                            <asp:BoundField DataField="RejectReason" HeaderText="<%$ Resources:Tokens,RejectReason %>" />
                            <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:Tokens,PaymentType %>" />

                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Reject%>">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnb_Reject" runat="server" data-select="toRevect" data-request-id='<%#Eval("ID") %>'
                                        CssClass="btn btn-danger btn-sm" Style="text-decoration: none;" data-rel="tooltip"
                                        title="<%$ Resources:Tokens,Reject %>"><span class="icon-remove icon-only bigger-110"></span></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label runat="server" Text="<%$Resources:Tokens,NoRequests%>" ID="lblnoData"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div id="tr_CustomerDetails" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label40" runat="server" Text="<%$ Resources:Tokens,Customer.Details %>"></asp:Label>
                    </h3>
                    <table class="table table-bordered table-condensed">
                        <tr>
                            <td colspan="4">
                                <%=Tokens.CustomerDetails %>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Tokens,Customer %>"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbl_CustomerName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Tokens,Address %>"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbl_CustomerAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Tokens,Governrate %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_GovernorateName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Tokens,Central%>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="l_central" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Tokens,Phone %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_CustomerPhone" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Tokens,Ip.Package %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_IpPackageName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Tokens,Mobile1 %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_CustomerMobile" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Tokens,Mobile2 %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_Mobil2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 147px">
                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Tokens,Customer.Email %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_CustomerEmail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Literal ID="Literal2" Text="<%$ Resources:Tokens,IdNumber %>" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lPersonalId" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,LineOwner%>"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lblLineOwner"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Literal ID="Literal4" Text="<%$ Resources:Tokens,Prepaid %>" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblprepaid" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,InstallationCost%>"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="lblinstallationcost"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Literal ID="Literal6" Text="<%$ Resources:Tokens,ContractingCost %>" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblcontractingcost" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,CreationDate %>"></asp:Literal>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCreationDate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <%=Tokens.ServiceDetails %>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Tokens,Service.Provider %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_SPName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Tokens,Service.Package %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_ServicePackageName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Tokens,Reseller %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_ResellerName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Tokens,Branch%>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_BranchName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Tokens,VPI %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_VPI" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:Tokens,VCI %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_VCI" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Tokens,UserName %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_Client_UserName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:Tokens,Customer.Status %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_StatusName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_Password" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Tokens,Extra.Gigas %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lbl_ExtraGigas" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,PaymentType %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lbl_PaymentType" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:Tokens,Offer %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="l_offer" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <%= Tokens.FirstDateForOffer %>
                            </td>
                            <td style="width: 162px">
                                <label runat="server" id="LOfferStart">
                                </label>
                            </td>
                            <td style="width: 147px">
                                <%= Tokens.LastDateForOffer %>
                            </td>
                            <td style="width: 167px">
                                <label runat="server" id="LOfferEnd">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Literal Text="<%$Resources:Tokens,RequestNumber %>" runat="server" />
                            </td>
                            <td style="width: 162px">
                                <asp:Literal ID="LRequestNumber" runat="server" />
                            </td>
                            <td style="width: 147px">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,RouterSerial %>" runat="server" />
                            </td>
                            <td style="width: 167px">
                                <asp:Literal ID="LRouterSerial" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <%= Tokens.Installer %>
                            </td>
                            <td style="width: 162px">
                                <span runat="server" id="SpanInstaller"></span>
                            </td>
                            <td style="width: 147px">
                                <%= Tokens.InstallationTime %>
                            </td>
                            <td style="width: 167px">
                                <span runat="server" id="SpanInstallTime"></span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,WorkorderNumber %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lblWorkorderNumber" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,WorkorderDate %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lblWorkorderDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px; color: red">
                                <asp:Literal Text="<%$Resources:Tokens,SuspendDaysCount %>" runat="server" />
                            </td>
                            <td style="color: red" colspan="3">
                                <asp:Literal ID="LSuspendedDaysCount" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <%=Tokens.CentralDetails %>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,PortNumber %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lblPortNumber" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,Block %>"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="lblBlock" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,Dslam %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:Label ID="lblDslam" runat="server"></asp:Label>
                            </td>
                            <td style="width: 147px">
                                <asp:Label ID="Label9" runat="server"></asp:Label>
                            </td>
                            <td style="width: 167px">
                                <asp:Label ID="Label10" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 138px">
                                <asp:Label ID="lbl_Notes_Head" runat="server" Text="<%$ Resources:Tokens,Notes %>"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbl_Notes" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
        <div class="view" runat="server" visible="False" id="router">
            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">اقرار استلام راوتر                    اقرار استلام راوتر</h3>

                    <div id="MyDiv1" dir="rtl">

                        <table style="width: 100%; height: 150px;" dir="rtl">
                            <tr>
                                <td colspan="2">
                                    <h1 align="center">
                                        <asp:Literal ID="LCompany" runat="server" /></h1>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <strong>التاريخ / </strong>
                                </td>
                                <td class="style3">
                                    <asp:Label ID="lblDate" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    <strong>رقم الهاتف /</strong></td>
                                <td class="style5">
                                    <asp:Label ID="lblPhone" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <strong>اسم العميل/</strong></td>
                                <td class="style7">
                                    <asp:Label ID="lblName" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1" colspan="2">
                                    <strong>اقرار استلام راوتر</strong></td>
                            </tr>
                        </table>


                        <table style="width: 100%; height: 150px;" dir="rtl" frame="hsides"
                            border="2px">
                            <tr>
                                <td class="style2">
                                    <strong>الصنف</strong></td>
                                <td class="style2">
                                    <strong>العدد</strong></td>
                                <td class="style8">
                                    <strong>متواجد ( نعم - لا )</strong></td>
                            </tr>
                            <tr>
                                <td class="style17">Network cable</td>
                                <td class="style17">1</td>
                                <td class="style17">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style10">Bags</td>
                                <td class="style10">1</td>
                                <td class="style10">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style18">Splitter</td>
                                <td class="style18">1</td>
                                <td class="style18">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style17">Adaptor</td>
                                <td class="style17">1</td>
                                <td class="style17">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style12">Telephone cable</td>
                                <td class="style12">1</td>
                                <td class="style12">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style14">Serial</td>
                                <td class="style14">&nbsp;</td>
                                <td class="style14">
                                    <asp:Label ID="lblSerial" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%; height: 150px;" dir="rtl" frame="hsides">
                            <tr>
                                <td class="style15">
                                    <strong>استلمت انا /                 استلمت انا /</strong></td>
                                <td class="style8">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    <strong>رقم قومي /</strong><asp:Label ID="lblNationalNumber" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td class="style5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style16" style="text-align: right">
                                    <strong>و المقيم /</strong><asp:Label ID="address" runat="server"
                                        Style="font-size: large"></asp:Label></td>
                                <td class="style10">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="style2" colspan="2">الجهاز بالمواصفات الموضحة بعاليه&nbsp; بحالة جيدة و سليمة و صالحة للاستعمال
                                    <br />
                                    و هذا اقرار مني و ايصال بالاستلام.</td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    <strong>توقيع العميل/</strong></td>
                                <td class="style17" style="text-align: right">
                                    <strong>توقيع المهندس/</strong></td>
                            </tr>
                        </table>


                    </div>
                    <div align="center">
                        <input id="Button1" class="btn btn-success" type="button" onclick="Print('MyDiv1');"
                            value="طباعة" />
                    </div>
                </fieldset>
            </div>
        </div>
        <div runat="server" visible="False" id="RunOrderTEData" class="view">
            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">أمر تشغيل TE Data</h3>
                    <div id="MyDiv2" dir="rtl">
                        <table style="width: 100%; font-size: large;" dir="rtl">
                            <tr>
                                <td colspan="4">
                                    <h1 align="center">TEData</h1>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="width: 237px"></td>
                                <td style="font-size: large">التاريخ :
                                </td>
                                <td>
                                    <asp:Label ID="lblDateTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="font-size: x-large;">
                                    <strong>بيانات العميل</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: large">رقم ال ADSL :
                                </td>
                                <td>
                                    <asp:Label ID="lblADSLTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="font-size: large">اسم العميل :
                                </td>
                                <td style="font-size: large">
                                    <asp:Label ID="lblNameTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>اسم صاحب الخط :
                                </td>
                                <td>
                                    <asp:Label ID="lblonerNameTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>المحمول :
                                </td>
                                <td>
                                    <asp:Label ID="lblMobileTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>العنوان :
                                </td>
                                <td>
                                    <asp:Label ID="lblAddressTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="4" style="font-size: x-large">
                                    <strong>بيانات الخدمة</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>السرعة المتعاقد عليها :
                                </td>
                                <td>
                                    <asp:Label ID="lblSpeedTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblUserNameTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>:UserName
                                </td>
                            </tr>
                            <tr>
                                <td>نوع العرض :
                                </td>
                                <td>
                                    <asp:Label ID="lblOfferTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblPassTEdata" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>:Password
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>:Ping
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="font-size: x-large">
                                    <strong>الخدمات المقدمة</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>تصفية الشهر:
                                </td>
                                <td></td>
                                <td>سلك نت :
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>حساب الشهر:
                                </td>
                                <td></td>
                                <td>سلك تليفون :
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>زيارة التركيب :
                                </td>
                                <td>
                                    <asp:Label ID="lblInstallationfeesTedata" runat="server"></asp:Label>
                                </td>
                                <td>أرجيه نت:
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>الاجمالى :
                                </td>
                                <td></td>
                                <td>الروتر:
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>المبلغ المدفوع تحت الحساب :
                                </td>
                                <td>
                                    <asp:Label ID="lblprePaidTeData" runat="server"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>المبلغ المطلوب :
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>الاجمالى بعد الاضافة :
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">طلبات العميل
                                </td>
                            </tr>
                            <tr>
                                <td>1:
                                </td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td>2:
                                </td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td colspan="4">تقيم أداء الدعم الفنى و مدى رضاء العميل
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">:
                                </td>
                            </tr>
                            <tr>
                                <td style="direction: ltr">اسم العميل :
                                </td>
                                <td></td>
                                <td>مسئول الدعم الفنى :
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>التوقيع :
                                </td>
                                <td></td>
                                <td>مساعد الدعم الفنى :
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>العنوان
                                </td>
                                <td colspan="3">
                                    <asp:Literal ID="LAddress" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4">
                                    <h3 align="center"></h3>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div align="center">
                        <input id="Button2" type="button" class="btn btn-success" onclick="Print('MyDiv2')"
                            value="طباعة" />
                    </div>
                </fieldset>
            </div>
        </div>
        <div runat="server" class="view" visible="False" id="RunOrderEtisalat">
            <div>
                <fieldset>
                    <legend>أمر تشغيل Etisalat</legend>
                    <div id="MyDiv3" dir="rtl">
                        <table style="width: 100%; font-size: large;" dir="rtl">
                            <tr>
                                <td colspan="4">
                                    <h1 align="center" runat="server" id="Company2"></h1>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="width: 237px"></td>
                                <td style="font-size: large">التاريخ :
                                </td>
                                <td>
                                    <asp:Label ID="lblDateEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="font-size: x-large;">
                                    <strong>بيانات العميل</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: large">رقم ال ADSL :
                                </td>
                                <td>
                                    <asp:Label ID="lblADSLEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="font-size: large">اسم العميل :
                                </td>
                                <td style="font-size: large">
                                    <asp:Label ID="lblNameEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>رقم البطاقة :
                                </td>
                                <td>
                                    <asp:Label ID="lblNationalIDEtisalat" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>اسم صاحب الخط :
                                </td>
                                <td>
                                    <asp:Label ID="lblOwnerEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>المحمول :
                                </td>
                                <td>
                                    <asp:Label ID="lblMobileEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>العنوان :
                                </td>
                                <td>
                                    <asp:Label ID="lblAdreessEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="4" style="font-size: x-large">
                                    <strong>بيانات الخدمة</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>السرعة المتعاقد عليها :
                                </td>
                                <td>
                                    <asp:Label ID="lblSpeedEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblUsernameEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>:UserName
                                </td>
                            </tr>
                            <tr>
                                <td>نوع العرض :
                                </td>
                                <td>
                                    <asp:Label ID="lblOfferEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblPassEtisalat" runat="server" Style="font-size: large"></asp:Label>
                                </td>
                                <td>:Password
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>:Ping
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="font-size: x-large">
                                    <strong>الخدمات المقدمة</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>زيارة التركيب :
                                </td>
                                <td>
                                    <asp:Label ID="lblInstallationFees" runat="server"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>سلك نت :
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>سلك تليفون :
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>الاجمالى :
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>المبلغ المدفوع تحت الحساب :
                                </td>
                                <td>
                                    <asp:Label ID="lblPrePaidEtisalat" runat="server"></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>المبلغ المطلوب :
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>الاجمالى بعد الاضافة :
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">طلبات العميل
                                </td>
                            </tr>
                            <tr>
                                <td>1:
                                </td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td>2:
                                </td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td colspan="4">تقيم أداء الدعم الفنى و مدى رضاء العميل
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">:
                                </td>
                            </tr>
                            <tr>
                                <td>اسم العميل :
                                </td>
                                <td></td>
                                <td>مسئول الدعم الفنى :
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>التوقيع :
                                </td>
                                <td></td>
                                <td>مساعد الدعم الفنى :
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>العنوان
                                </td>
                                <td colspan="3">
                                    <asp:Literal ID="LAddress2" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3"></td>
                            </tr>
                        </table>
                    </div>
                    <div align="center">
                        <input id="Button3" class="btn btn-success" type="button" onclick="Print('MyDiv3')"
                            value="طباعة" />
                    </div>
                </fieldset>
            </div>
        </div>
        <div id="tr_Files" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <uc1:UserFile ID="UserFile1" runat="server" CanEdit="False" />
                </fieldset>
            </div>
        </div>
        <div id="tr_Status" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:Tokens,CustomerStatuseHistory %>"></asp:Label>
                    </h3>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        GridLines="None" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>"></asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Admin_Employee%>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Date_Time %>">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UpdateDate") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </div>
        </div>
        <div id="tr_Requests" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:Tokens,CustomerRequestsHistory %>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd_Requests" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        GridLines="None" CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:BoundField DataField="RequestName" HeaderText="<%$ Resources:Tokens,Request.Type %>" />
                            <asp:BoundField DataField="RequestDate" HeaderText="<%$ Resources:Tokens,Request.Date %>" />
                            <asp:BoundField DataField="ProcessDate" HeaderText="<%$Resources:Tokens,ProcessDate %>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,WhoOrder %>" />
                            <asp:BoundField DataField="UserName2" HeaderText="<%$ Resources:Tokens,WhoManage%>" />
                            <asp:BoundField DataField="ServicePackageName" HeaderText="<%$ Resources:Tokens,OldPackage %>" />
                            <asp:BoundField DataField="ServicePackageName2" HeaderText="<%$ Resources:Tokens,New.Service.Package %>" />
                            <asp:BoundField DataField="IpPackageName" HeaderText="<%$ Resources:Tokens,New.IP.Package %>" />
                            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Tokens,Extra.Gigas%>" />
                            <asp:BoundField DataField="Total" HeaderText="<%$Resources:Tokens,Amount %>" />
                            <asp:BoundField DataField="RSName" HeaderText="<%$ Resources:Tokens,RequestStatus %>" />
                            <asp:BoundField DataField="RejectReason" HeaderText="<%$ Resources:Tokens,RejectReason %>" />
                            <asp:BoundField DataField="Notes" HeaderText="<%$ Resources:Tokens,Notes %>" />

                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,NoRequests %>"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
            </div>
        </div>
        <div id="tr_Tickets" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label43" runat="server" Text="<%$ Resources:Tokens,CustomerTicketHistory %>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd_Tickets" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>"></asp:BoundField>
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>"></asp:BoundField>
                            <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>"></asp:BoundField>
                            <asp:BoundField DataField="Title" HeaderText="<%$ Resources:Tokens,Ticket.Reason %>"></asp:BoundField>
                            <asp:BoundField DataField="Details" HeaderText="<%$ Resources:Tokens,Details %>"></asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>"></asp:BoundField>
                            <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:Tokens,Comment %>"></asp:BoundField>
                            <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>"></asp:BoundField>
                            <asp:BoundField DataField="TicketDate" HeaderText="<%$ Resources:Tokens,OpenDate %>" />
                            <asp:BoundField DataField="CommentDate" HeaderText="<%$ Resources:Tokens,SolvedDate %>" />
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,DaysCount %>">
                                <ItemTemplate>
                                    <asp:Literal ID="DaysCount" runat="server" Text='<%#Eval("ID") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,NoTickects %>"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
            </div>
        </div>
        <%  if (IsNotReseller)
            { %>
        <div id="tr_woInfo" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label44" runat="server" Text="<%$ Resources:Tokens,CustomerInfoHistory%>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd_Info" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        GridLines="None" CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                            <asp:BoundField DataField="Reseller" HeaderText="<%$ Resources:Tokens,Reseller%>" />
                            <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>" />
                            <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:Tokens,Branch %>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Manager %>" />
                            <asp:BoundField DataField="UpdateDate" HeaderText="<%$ Resources:Tokens,UpdateDate %>" />
                            <asp:BoundField DataField="Offer" HeaderText="<%$ Resources:Tokens,Offer %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoUpdates%>"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
            </div>
            <td></td>
        </div>
        <%  } %>
        <div id="RequestDateHistory" runat="server" visible="False">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%=Tokens.ChangeRequestDate %></h3>
                <div>
                    <asp:GridView runat="server" ID="GVRequestDateHistory" AutoGenerateColumns="False"
                        CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,User %>" />
                            <asp:BoundField DataField="newRequestDate" HeaderText="<%$ Resources:Tokens,newRequestDate %>" />
                            <asp:BoundField DataField="oldRequestDate" HeaderText="<%$ Resources:Tokens,oldRequestDate %>" />
                            <asp:BoundField DataField="ChangeDate" HeaderText="<%$ Resources:Tokens,ProcessDate %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div id="customerRouter" runat="server" visible="False">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <%= Tokens.Routers %></h3>
                    <div>
                        <asp:GridView runat="server" ID="GVRouter" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed"
                            DataKeyNames="Id" OnDataBound="GVRouter_DataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RouterSerial" HeaderText="<%$Resources:Tokens,RouterSerial %>" />
                                <asp:BoundField DataField="StoreName" HeaderText="<%$Resources:Tokens,AddStore %>" />
                                <asp:BoundField DataField="CompanyUserName" HeaderText="<%$Resources:Tokens,RecieveFromCompany %>" />
                                <asp:BoundField DataField="CustomerUserName" HeaderText="<%$Resources:Tokens,CustomerConfirmerUser %>" />
                                <asp:BoundField DataField="CompanyDate" HeaderText="<%$Resources:Tokens,CompanyProcessDate %>" />
                                <asp:BoundField DataField="CustomerDate" HeaderText="<%$Resources:Tokens,CustomerProcessDate %>" />
                                <asp:BoundField DataField="IsRecieved" HeaderText="<%$Resources:Tokens,Done %>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a href='<%#Eval("Attach") %>' runat="server" class="btn btn-success btn-xs" target="_blank"
                                            id="link" visible="False"><i class="icon-only icon-download"></i></a>&nbsp;
                                        <a href='<%#Eval("Attach2") %>' runat="server" class="btn btn-primary btn-xs" target="_balnk"
                                            id="link2" visible="False"><i class="icon-only icon-download"></i></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
        <div id="CNotes" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <%= Tokens.CutomerNotes %></h3>
                    <div>
                        <asp:GridView runat="server" ID="GvNotes" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Text" HeaderText="<%$Resources:Tokens,CustomerNote %>" />
                                <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Time %>" />
                                <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,User %>" />
                                <asp:BoundField DataField="Done" HeaderText="<%$Resources:Tokens,Done %>" />
                                <asp:BoundField DataField="Employee" HeaderText="<%$Resources:Tokens,Employee %>" />
                                <asp:BoundField DataField="Comment" HeaderText="<%$Resources:Tokens,Comment %>" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
        <div id="divCallMessages" runat="server" visible="false">
            <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <%= Tokens.CallMessages %></h3>
                    <div>
                        <asp:GridView runat="server" ID="Grd_CallMessages" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MessageText" HeaderText="<%$Resources:Tokens,Message %>" />
                                <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,User %>" />
                                <%--<asp:BoundField DataField="Date" DataFormatString="{0:dd/MM/yyyy hh:mm}" HeaderText="<%$Resources:Tokens,Date %>" />--%>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Date %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Tokens,NoResults%>"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
        <div id="CallModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <!-- -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="myModalLabel" class="modal-title">
                            <%=Tokens.Call %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <label>
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,CallText %>"></asp:Literal>
                                </label>
                                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtCallMassage"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCallMassage" ErrorMessage="<%$Resources:Tokens,Required %>"
                                    ValidationGroup="call"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnCallMessage" ValidationGroup="call" CssClass="btn btn-info"
                            Text="<%$Resources:Tokens,Save %>" OnClick="btnCallMessage_Click" />
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                    </div>
                    <asp:HiddenField runat="server" ID="hf_Woid" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div id="updowngradeModal" class="bootbox modal fade" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <!-- -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="H1" class="modal-title">
                            <%=Tokens.Upgrade_Downgrade %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <label>
                                    <asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,New.Service.Package %>"></asp:Literal>
                                </label>
                                <asp:DropDownList ID="ddl_ServicePackage" runat="server" Width="155px" DataTextField="ServicePackageName"
                                    DataValueField="ID" ValidationGroup="updown">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddl_ServicePackage"
                                    ErrorMessage="RequiredFieldValidator" ValidationGroup="updown">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnmodalupdown" ValidationGroup="updown" CssClass="btn btn-info"
                            Text="<%$Resources:Tokens,Save %>" OnClick="Send" />
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <asp:Literal ID="Literal9" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                    </div>
                    <asp:HiddenField runat="server" ID="HiddenField1" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div id="changeIPModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <!-- -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="H2" class="modal-title">
                            <%=Tokens.MenuChangeIPPackage %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <label>
                                    <asp:Literal ID="Literal10" runat="server" Text="<%$Resources:Tokens,New.IP.Package %>"></asp:Literal>
                                </label>
                                <asp:DropDownList ID="ddl_IpPackage" runat="server" Width="155px" DataTextField="IpPackageName"
                                    DataValueField="ID" ValidationGroup="ip">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddl_IpPackage"
                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="ip">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnchangeIpModal" ValidationGroup="ip" CssClass="btn btn-info"
                            Text="<%$Resources:Tokens,Save %>" OnClick="Send" />
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                    </div>
                    <asp:HiddenField runat="server" ID="HiddenField2" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div id="ExtraGigaModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <!-- -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="H3" class="modal-title">
                            <%=Tokens.MenuRequestExtraGiga %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <label>
                                    <asp:Literal ID="Literal12" runat="server" Text="<%$Resources:Tokens,Extra.Gigas %>"></asp:Literal>
                                </label>
                                <asp:DropDownList runat="server" ID="DdlExtraGigas" ValidationGroup="giga">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="DdlExtraGigas"
                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="giga"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnExtraModal" ValidationGroup="giga" CssClass="btn btn-info"
                            Text="<%$Resources:Tokens,Save %>" OnClick="Send" />
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <asp:Literal ID="Literal13" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                    </div>
                    <asp:HiddenField runat="server" ID="HiddenField3" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div id="changeofferModal" class="bootbox modal fade" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <!-- -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="H4" class="modal-title">
                            <%=Tokens.ChangeOfferRequest %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <label>
                                    <asp:Literal ID="Literal14" runat="server" Text="<%$Resources:Tokens,Offer %>"></asp:Literal>
                                </label>
                                <asp:DropDownList ID="ddlOffers" runat="server" Width="155px" ValidationGroup="offer">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlOffers"
                                    ErrorMessage="RequiredFieldValidator" ValidationGroup="offer">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnChangeOfferModal" ValidationGroup="offer" CssClass="btn btn-info"
                            Text="<%$Resources:Tokens,Save %>" OnClick="Send" />
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <asp:Literal ID="Literal15" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                    </div>
                    <asp:HiddenField runat="server" ID="HiddenField4" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div id="rejectModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel4"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                            ×</button>
                        <h4 id="myModalLabel4">
                            <%=Tokens.RejectReason %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <asp:HiddenField runat="server" ID="RejectedRequestId" ClientIDMode="Static" />
                            <div>
                                <label for="TbRejectReason">
                                    <%=Tokens.RejectReason %>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbRejectReason" ValidationGroup="rr" ClientIDMode="Static"
                                        TextMode="MultiLine" />
                                    <asp:RequiredFieldValidator ValidationGroup="rr" ErrorMessage="*" ControlToValidate="TbRejectReason"
                                        ID="RR" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div id="reject-results">
                        </div>
                        <button id="Button4" class="btn btn-primary" validationgroup="rr" type="button" causesvalidation="True"
                            runat="server" onserverclick="RejectSelectedRequest">
                            <%=Tokens.Reject %></button>
                        <button class="btn btn-default" type="button" data-dismiss="modal" aria-hidden="true">
                            <%=Tokens.Cancel %></button>
                    </div>
                </div>
            </div>
        </div>
        <div id="reActivModal" class="bootbox modal fade" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <!-- -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 id="H1" class="modal-title">
                            <%=Tokens.Reactivate %></h4>
                    </div>
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <div class="bootbox-form">
                                <label>
                                    <asp:Literal ID="Literal16" runat="server" Text="<%$Resources:Tokens,Note %>"></asp:Literal>
                                </label>
                                <textarea id="notes" runat="server" cols="50" rows="5"></textarea>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="notes"
                                    ErrorMessage="RequiredFieldValidator" ValidationGroup="notes">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnReactive" ValidationGroup="notes" CssClass="btn btn-info"
                            Text="<%$Resources:Tokens,Save %>" OnClick="Send" />
                        <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                            <asp:Literal ID="Literal17" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
                    </div>
                    <asp:HiddenField runat="server" ID="HiddenField5" ClientIDMode="Static" />
                </div>
            </div>
        </div>
    </div>
    <div id="CustomerModel" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="modelheader"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="modelheader">
                        <%= Tokens.RecieveToCustomer %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div>
                            <div>
                                <div>
                                    <asp:Label ID="Label34" AssociatedControlID="ddlRouters" runat="server" Text="<%$ Resources:Tokens,Routers %>"></asp:Label>
                                    <div>

                                        <asp:DropDownList ID="ddlRouters" ClientIDMode="Static" runat="server" Width="150px" CssClass="width-60">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ControlToValidate="ddlRouters" ID="RequiredFieldValidator5"
                                            runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="customer"></asp:RequiredFieldValidator>

                                    </div>
                                    <br />
                                </div>

                                <asp:Label runat="server" Text="<%$Resources:Tokens,AttachFile %>"></asp:Label>
                                <div>
                                    <asp:FileUpload runat="server" ID="fileUpload1" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                        ControlToValidate="fileUpload1" ValidationGroup="customer"></asp:RequiredFieldValidator>
                                </div>
                                <div>
                                    <asp:FileUpload runat="server" ID="fileUpload2" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnRecieveToCustomer"
                        ValidationGroup="customer" Text="<%$Resources:Tokens,Save %>"
                        OnClick="btnRecieveToCustomer_Click" />
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="multi" class="bootbox modal fade" tabindex="-1" style="margin-right: 200px;" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div style="background-color: white; width: 300px; margin-right: 500px; margin-top: 300px;">
            <div class="modal-content">
                <div class="modal-header">
                    <div style="margin-right: 20px;">


                        <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <br />



                        <div class="modal-body">
                            <asp:Label runat="server" Text="<%$Resources:Tokens,RouterSerial %>"></asp:Label>
                            <div>
                                <asp:TextBox runat="server" ID="txtRouterSerial"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRouterSerial" ID="rqu"
                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="ad"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,RouterType %>"></asp:Label>
                            <div>
                                <asp:TextBox runat="server" ID="txtRouterType"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRouterType" ID="RequiredFieldValidator6"
                                    ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="ad"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Tokens,AddStore %>"></asp:Label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlStores" ValidationGroup="company" />
                                <asp:RequiredFieldValidator ControlToValidate="ddlStores" ValidationGroup="ad" ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <br />
                                <asp:Button runat="server" ID="btnAddRouter" OnClick="btnRecieveFromCompany_Click" ValidationGroup="ad" CssClass="btn btn-success" Text="<%$Resources:Tokens,Add %>" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%:System.Web.Optimization.Scripts.Render("~/bundles/tablesorter")%>
    <script type="text/javascript">



        $(document).ready(function () {
            $('#SendRouters').click(function () {
                $('#CustomerModel').modal('show');
            });

            $('#reciverouters').click(function () {
                $('#multi').modal('show');
            });

            $('img').each(function () {
                $(this).airview({

                    width: 500,
                    container: 'body',
                    trigger: 'hover',
                    //error: 'Sorry! No Image found',
                    //template: '<div class="airview" role="tooltip"><div class="airview-arrow"></div><div class="airview-inner"><div class="airview-loader"></div><img /></div></div>',
                    placement: 'right',
                    //html: true,
                    //animation: true,
                    url: false
                    //content: function () {
                    //    return '<img src="' + $(this).data('img') + '" />';
                    //}

                });
            });



            $("#grd_wo tbody tr").filter(':first').appendTo('#get-result thead').attr("title", "press and wait for sorting");
            $("#grd_wo tbody tr").appendTo('#get-result tbody');


            $('#txt_CustomerPhone0').keypress(function (e) {
                var key = e.which;
                if (key == 13) {
                    $('#btn_search').click();
                    return false;
                } else {
                    return true;
                }
            });
            $('#lnkcall').click(function () {
                var id = $(this).attr('data-woid');
                $('#hf_Woid').val(id);
                $('#CallModal').modal('show');
            });

            $('#reactiveModal').click(function () {

                $('#reActivModal').modal('show');
            });
            $('#btnUpDoenGrade').click(function () {

                $('#updowngradeModal').modal('show');
            });
            $('#btnchangeIP').click(function () {

                $('#changeIPModal').modal('show');
            });
            $('#btnExtraGiga').click(function () {

                $('#ExtraGigaModal').modal('show');
            });
            $('#btnChangeOffer').click(function () {

                $('#changeofferModal').modal('show');
            });

            $('a[data-select="toRevect"]').click(function () {
                var id = $(this).attr('data-request-id');
                $('#RejectedRequestId').val(id);
                $('#rejectModal').modal('show');
                return false;
            });

        });
        function Print(div) {

            var printContent = document.getElementById(div);
            var windowUrl = '';  /*'about:blank';*/
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'right=50000,top=50000,width=0,height=0');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

        } function areyousure() {
            return confirm('<%= Tokens.AlertRUS %>');
        }
        function showDiv(showdiv) {
            document.getElementById(showdiv).style.display = "block";
        }
    </script>
</asp:Content>
