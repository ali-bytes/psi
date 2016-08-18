<%@ Page Title="<%$ Resources:Tokens,ManageRequests%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ManageRequests.aspx.cs" Inherits="NewIspNL.Pages.ManageRequests" %>

<%@ Import Namespace="Resources" %>
<%@ Import Namespace="NewIspNL.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
    <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
    <style type="text/css">
        .tablescroll {
            display: block;
            overflow-x: scroll;
        }
    </style>

    <asp:HiddenField runat="server" ID="SelectedStuff" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HfIsUnsuspend" ClientIDMode="Static" />
     <asp:HiddenField runat="server" ID="EditRequestDate" ClientIDMode="Static" />
    <div class="page-header">
        <h1>
            <asp:Label ID="lbl_Gridlabel" runat="server"></asp:Label>
        </h1>
    </div>
    <div class="row">
        <%--<fieldset>view view-scroll --%>
        <div class="col-xs-12">
            <div class="col-md-3">
                <label><%=Tokens.PaymentType %></label>
                <div>
                   <%-- <asp:DropDownList runat="server" ID="ddlPayType" ClientIDMode="Static" AutoPostBack="True" OnSelectedIndexChanged="ddlPayType_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                     <asp:DropDownList runat="server" ID="ddlPayType" ClientIDMode="Static" >
                    </asp:DropDownList>
                </div>
            </div>
          
            <div runat="server" id="suspendStatus" visible="False">
                <label><%=Tokens.SuspendDaysCount %></label>
                <div>
                    <asp:DropDownList runat="server" ID="ddlSuspendCount" OnSelectedIndexChanged="ddlSuspendDays_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
             <div style="margin: 0px; padding:0px; clear: both; height: 6px">
         &nbsp;
       </div>
            <br />
            <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                CssClass="table table-bordered table-condensed text-center tablescroll col-sm-12 col-xs-12" Style="margin-right: -18px;"
                DataKeyNames="ID" 
                OnRowDataBound="grd_Requests_RowDataBound" >
                <PagerSettings Position="Bottom"></PagerSettings>
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:Label ID="lbl_No" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </EditItemTemplate>
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
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$ Resources:Tokens,Request.Date %>" />
                    <asp:BoundField DataField="UserName2" HeaderText="<%$ Resources:Tokens,UserName %>" />
                    <asp:BoundField DataField="RejectReason" HeaderText="<%$ Resources:Tokens,RejectReason %>" />
                    <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:Tokens,PaymentType %>" />
                    <asp:BoundField DataField="Note" HeaderText="<%$Resources:Tokens,Notes %>" />
                    <asp:BoundField DataField="WRequestDate" HeaderText="<%$Resources:Tokens,InvoiceDueDate %>" />

                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,isprorequest%>">
                        <ItemTemplate>
                            <%--   <button type="button" ID="lnb_prorequest" runat="server" onclick='proreq(<%#Eval("ID") %>)'  Visible='<%#Eval("ProviderRequest") %>'     OnServerClick="PeoviderRequestSent"    class="btn btn-info btn-sm" style="text-decoration: none;"  title="<%$ Resources:Tokens,isprorequest %>"><span class="icon-reply icon-only icon-plus-sign"></span></button>
                
                               <a href="#" onclick="res(100)" clientidmode="Static" class="btn btn-info btn-sm"  runat="server" OnServerClick="PeoviderRequestSent"  Visible='<%#Eval("ProviderRequest") %>' title="<%$ Resources:Tokens,isprorequest %>" ><span class="icon-reply icon-only icon-plus-sign"></span></a>
                            <asp:Button ID="Button1"  Visible='<%#Eval("ProviderRequest") %>' class="btn btn-info btn-sm" style="text-decoration: none;" runat="server" Text="<%$ Resources:Tokens,isprorequest %>"   OnClick="PeoviderRequestSent" CommandArgument="<%#Eval("ID") %>" />  

                        
                                
                                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="PeoviderRequestSent" />
                               
                            --%>
                            <%--<asp:Button ID="Button2"  runat="server" Text="Button" OnClick="PeoviderRequestSent" />--%>

                            <asp:LinkButton CssClass="btn btn-info" ID="LinkButton1" runat="server"
                                CommandArgument='<%#Eval("ID") %>' Visible='<%#Eval("ProviderRequest") %>' OnClick="PeoviderRequestSent" ValidationGroup="none"><span class="icon-reply icon-only icon-plus-sign"></span></asp:LinkButton>


                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Approve%>">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" Value='<%#Eval("woid") %>' ID="lblWoid" />
                            <div x-template="x" style="display: none;">
                                <asp:Literal ID="Literal1" Text='<%#Eval("ResultHtml") %>' runat="server" />
                            </div>
                            <a id="lnb_Approve" runat="server" data-id='<%# Eval("ID") %>' style="text-decoration: none;" clientidmode="Static"
                                data-woid='<%# Eval("woid") %>' class="btn btn-success btn-sm"
                                selector="dialog" onclick="return addDialog(this);" 
                                title="<%$ Resources:Tokens,Approve %>" data-rel="tooltip"><i class="icon-ok icon-only bigger-110"></i></a><%--CommandArgument='<%# Eval("ID") %>'OnClick="lnb_Approve_Click" ValidationGroup='<%# Eval("woid") %>'--%>
                            <button type="button" runat="server" clientidmode="Static" data-id='<%#Eval("ID") %>' data-select="approve" onclick="approv(this)" id="btnapproved" class="btn btn-success btn-sm">
                                <i class="icon-ok icon-only bigger-110"></i>
                            </button>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Reject%>">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnb_Reject" runat="server" data-select="toRevect" data-request-id='<%#Eval("ID") %>' ClientIDMode="Static" onclick="rej(this)"
                                CssClass="btn btn-danger btn-sm" Style="text-decoration: none;" data-rel="tooltip" title="<%$ Resources:Tokens,Reject %>"><span class="icon-reply icon-only bigger-110"></span></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Select %>">
                        <HeaderTemplate>

                            <input type="checkbox" id="selecctall" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="SelectItem" runat="server" data-check="select" ClientIDMode="Static" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoRequests%>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>


        </div>
        <br />
        <table width="100%" id="tbl_Control" runat="server">
            <tr>
                <td align="center">
                    <asp:LinkButton CssClass="btn btn-success" ID="btn_ApproveSelected" runat="server" ClientIDMode="Static"  OnClientClick="addDialog(this); return false;"  Text="<%$ Resources:Tokens,ApproveSelected %>"
                        Width="200px" OnClick="btn_ApproveSelected_Click" ValidationGroup="none" />
                    &nbsp;
                        <asp:LinkButton ID="btn_RejectSelected" CssClass="btn btn-danger" data-select="toRevect" runat="server" Text="<%$ Resources:Tokens,RejectSelected%>"
                            Width="200px" OnClick="btn_RejectSelected_Click" ValidationGroup="none" />
                    &nbsp;
                         <%--   <asp:LinkButton CssClass="btn btn-info" ID="btnproviderrequest2" runat="server"   Text="<%$ Resources:Tokens,isprorequest %>"
                            Width="200px" OnClick="btn_PeoviderRequestSentSelected_Click" ValidationGroup="none"/>
                        &nbsp;--%>
                        
                </td>
                <td align="center">
                    <asp:LinkButton ID="provrequestall" ValidationGroup="none" OnClick="btn_PeoviderRequestSentSelected_Click" CssClass="btn btn-info" runat="server"><%=Tokens.isprorequest %> </asp:LinkButton>
                </td>
            </tr>
        </table>

        <div style="text-align: center;">
            <asp:Label ID="lbl_ProcessResult" runat="server" EnableViewState="False"
                Font-Bold="True"></asp:Label>
        </div>
        <div runat="server" id="Div1"></div>
        <%--</fieldset>--%>
    </div>
    <asp:HiddenField runat="server" ID="hdnreq" ClientIDMode="Static" />
    <!--suspend & ip Package & unhold-->
    <div id="xdialog" class="bootbox modal" tabindex="-1" role="dialog" aria-labelledby="H1"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H1">
                        <%=Tokens.Confirm %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div runat="server" id="MiscTbContainer">

                            <label for="TbMiscDate">
                                <%= Tokens.Date %>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbMiscDate" ClientIDMode="Static" data-date-picker="dp"
                                    ValidationGroup="cd"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ValidationGroup="x" ID="vId1" ControlToValidate="TbMiscDate"
                                    ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="xcntnt">
                    </div>
                    <div id="one">
                        <button class="btn btn-primary" runat="server" id="BtnSave" clientidmode="Static"
                            validationgroup="x" type="button" onserverclick="BtnSave_OnServerClick">
                            <i class="icon-ok icon-white"></i>&nbsp;<asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Confirm %>"
                                runat="server" />
                        </button>
                        <% var l = QueryStringSecurity.Decrypt(Request.QueryString["rid"]); if (l == "2")
                           { %>
                        <button class="btn btn-success" runat="server" id="BtnSaveWithLiquidation" clientidmode="Static"
                            validationgroup="x" type="button" onserverclick="BtnSaveWithLiquidation_Click">
                            <i class="icon-ok icon-white"></i>&nbsp;<asp:Literal ID="Literal3" Text="<%$Resources:Tokens,ConfirmWithLiquidation %>"
                                runat="server" />
                        </button>
                        <% } %>
                        <button type="button" id="cancel" class="btn btn-danger" data-dismiss="modal" data-bb-handler="cancel">
                            <i class="icon-white icon-remove"></i>&nbsp;
            <asp:Literal Text="<%$Resources:Tokens,Cancel %>" runat="server" /></button>
                    </div>
                    <div id="multi">
                        <button class="btn btn-success" runat="server" id="btnsaveall" clientidmode="Static"
                            validationgroup="x" type="button" onserverclick="btn_SuspendSelected_Click">
                            <%=Tokens.Confirm %>
                        </button>
                        <% if (l == "2")
                           { %>
                        <button type="button" class="btn btn-success" runat="server"
                            id="btnsaveAndLiqudationall" clientidmode="Static" validationgroup="x" onserverclick="btn_SuspendwithLiquedationSelected_Click">
                            <%=Tokens.ConfirmWithLiquidation %>
                        </button>
                        <% } %>
                        <button type="button" id="cancel2" class="btn btn-danger" data-dismiss="modal" data-bb-handler="cancel">
                            <%=Tokens.Cancel %>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="myModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="myModalLabel">
                        <%=Tokens.MenuUnsuspend %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="UnsuspendId" ClientIDMode="Static" />

                        <div>
                            <div>
                                <label for="TbDaysCount">
                                    <%= Tokens.DaysCountofLiqudation %></label>
                                <div>
                                    <asp:TextBox runat="server" ValidationGroup="ded" ClientIDMode="Static" ID="TbDaysCount"></asp:TextBox>
                                    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="TbDaysCount" ID="sdfsd"
                                        ValidationGroup="ded" runat="server" />
                                </div>
                            </div>
                            <div runat="server" id="UnsesuspendContainer">
                                <label for="TbUnsuspendDate">
                                    <%= Tokens.Date %>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbUnsuspendDate" data-date-picker="dp"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="sdffsder" ValidationGroup="ded" ControlToValidate="TbUnsuspendDate"
                                        ErrorMessage="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="unsuspend-result">
                    </div>
                    <asp:HiddenField runat="server" ID="ClickedBtn" Value="0" />

                    <table id="tbl">

                        <tr>
                            <td>
                                <button class="btn btn-primary" type="button" runat="server" id="PostponeSuspendDays"
                                    onserverclick="UnsuspendWithPostponeSuspendDays" validationgroup="ded" data-bb-handler="confirm">
                                    <%=Tokens.PostponeSuspendDays %></button>

                            </td>
                            <td>
                                <button class="btn btn-danger" type="button" data-dismiss="modal" data-bb-handler="cancel">
                                    <%=Tokens.Cancel %></button>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <button class="btn btn-primary" type="button" causesvalidation="True" runat="server"
                                    onserverclick="UnsusPendWithDeductionWithFixedRequestDate" id="DeductionWithFixedRequestDate"
                                    validationgroup="ded" data-bb-handler="confirm">
                                    <%=Tokens.DeductionWithFixedRequestDate %></button>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <button class="btn btn-primary" type="button" causesvalidation="False" runat="server" id="ModalBtnApprove"
                                    onserverclick="UnsusPendWithCompleteInvoice" data-bb-handler="confirm">
                                    <%=Tokens.CompleteInvoice %></button>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" ID="RBLUnsuspend">
                                    <asp:ListItem Selected="true" Text="<%$Resources:Tokens,PostponeSuspendDays %>" />
                                    <asp:ListItem Text="<%$Resources:Tokens,DeductionWithFixedRequestDate %>" />
                                    <asp:ListItem Text="<%$Resources:Tokens,CompleteInvoice %>" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Button runat="server" ValidationGroup="ded" ID="btnunsuspendselectall" OnClick="btn_ApproveSelected_Click" CssClass="btn btn-success" Text="<%$Resources:Tokens,Confirm %>" data-bb-handler="confirm" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="upDwon" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2"
        aria-hidden="true">
        <!--data-style="upDown"-->
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button><h4 id="myModalLabel2">
                            <%=Tokens.Upgrade_Downgrade %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <asp:RadioButtonList runat="server" ID="RblUpDwonOptions">
                                <asp:ListItem Value="فاتورة جزئية" Text="<%$Resources:Tokens,PartialInvoiceForOldSpeed %>" />
                                <asp:ListItem  Value="دمج السرعين" Selected="True" Text="<%$Resources:Tokens,MergeSpeedsInOneInvoice %>" />
                                <asp:ListItem  Value="شهر كامل" Text="<%$Resources:Tokens,MonthB4MonthAfter %>" />
                                <asp:ListItem Value="شهر كامل2" Text="<%$Resources:Tokens,Monthb4CurrentMonthRest %>" />
                            </asp:RadioButtonList>
                        </div>
                        <div runat="server" id="containerForUpDown">
                            <label for="TbUnsuspendDate">
                                <%= Tokens.Date %>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ClientIDMode="Static" ID="TbUpDwonDate" data-date-picker="dp"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ValidationGroup="upDown" ID="RequiredFieldValidator1"
                                    ControlToValidate="TbUpDwonDate" ErrorMessage="*">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <%--           <div id="updown-results">
            </div>--%>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="updown-results"></div>
                    <button class="btn btn-primary" type="button" validationgroup="upDown" runat="server"
                        onserverclick="UpGradeDwon">
                        <%=Tokens.Upgrade_Downgrade %></button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="cancelModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel3"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="myModalLabel3">
                        <%=Tokens.Cancel %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <asp:RadioButtonList runat="server" ID="RblCancelOptions" ClientIDMode="Static">
                                    <asp:ListItem Selected="True" Text="<%$Resources:Tokens,FilterCustomerInvoice %>" />
                                    <asp:ListItem Text="<%$Resources:Tokens,CancelDept %>" />
                                    <asp:ListItem Text="<%$Resources:Tokens,PayDept %>" />
                                    <asp:ListItem Text="<%$Resources:Tokens,CompleteInvoice %>" />
                                </asp:RadioButtonList>
                            </div>
                            <div runat="server" id="containerForCancel">
                                <label for="TbCancelDate">
                                    <%= Tokens.Date %>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbCancelDate" ClientIDMode="Static" data-date-picker="dp"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="upp" ID="vId2" ControlToValidate="TbCancelDate"
                                        ErrorMessage="*">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="cancel-results">
                    </div>
                    <button class="btn btn-primary" validationgroup="upp" type="button" runat="server"
                        onserverclick="CancelProcess" id="btncancelmodal" clientidmode="Static">
                        <%=Tokens.Cancel %></button>
                    <button class="btn btn-default" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Back %></button>
                </div>
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
                        <div class="well">
                            <label for="TbRejectReason">
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
                    <div id="reject-results"></div>
                    <button class="btn btn-primary" validationgroup="rr" type="button" causesvalidation="True"
                        runat="server" onserverclick="RejectSelectedRequest">
                        <%=Tokens.Reject %></button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="reactivateModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel5"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        &times;</button>

                    <h4 id="myModalLabel5">
                        <%= Tokens.MenuReactivate %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <label class="radio">
                                    <input runat="server" type="radio" name="reactivate" id="RbToNew" clientidmode="Static" checked="True" />
                                    <%= Tokens.ToNewCustomer %>
                                </label>
                                <label class="radio">
                                    <input runat="server" type="radio" name="reactivate" id="RbToActive" clientidmode="Static" />
                                    <%= Tokens.ToActiveCustomer %>
                                </label>
                            </div>
                            <div runat="server" id="ReactivateDateBox">
                                <label for="TbReactiveDate">
                                    <%= Tokens.Date %>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbReactiveDate" ValidationGroup="reactivate" ClientIDMode="Static"
                                        data-date-picker="dp"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="fistick1" ValidationGroup="reactivate"
                                        ControlToValidate="TbReactiveDate" ErrorMessage="<%$Resources:Tokens,Required%>">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div>
                                        <label><%=Tokens.Service_Provider %></label>
                                        <div>
                                            <asp:DropDownList runat="server" ID="ddlServiceProvider" OnSelectedIndexChanged="ddlServiceProvider_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="True">
                                            </asp:DropDownList><!--ddlServiceProvider_SelectedIndexChanged-->
                                        </div>
                                    </div>
                                    <div>
                                        <label for="DdlOffer">
                                            <%= Tokens.Offer %></label>
                                        <div>
                                            <asp:DropDownList runat="server" ID="DdlOffer" OnSelectedIndexChanged="DdlOffer_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RddlOffer" ErrorMessage="*" ControlToValidate="DdlOffer"
                                                runat="server" />
                                        </div>
                                    </div>
                                    <div>
                                        <label for="DdlPackage">
                                            <%= Tokens.Package %></label>
                                        <div>
                                            <asp:DropDownList runat="server" ID="DdlPackage" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlServiceProvider" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DdlOffer" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div>
                                <label for="DdlIpPackage">
                                    <%= Tokens.Ip_Package %></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="DdlIpPackage" ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>
                            </div>


                        </div>
                        <div class="well" id="rblFlag">
                            <asp:RadioButtonList runat="server" Width="100%" ClientIDMode="Static"
                                RepeatDirection="Horizontal" CssClass="radio" ID="RblDemand">
                                <asp:ListItem Value="0" Text="<%$Resources:Tokens,CompleteInvoice %>"></asp:ListItem>
                                <asp:ListItem Value="1" Text="<%$Resources:Tokens,filterTostartMonth %>"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="reactivate-results">
                    </div>
                    <button runat="server" causesvalidation="True" onserverclick="CancelProcess" validationgroup="reactivate"
                        class="btn btn-primary">
                        <%= Tokens.MenuReactivate %></button>
                    <span id="cancelReactivate" class="btn btn-danger" data-dismiss="modal" aria-hidden="true">
                        <%= Tokens.Cancel %></span>
                </div>
            </div>
        </div>

    </div>

    <span runat="server" id="direction" style="display: none" clientidmode="Static"></span>


    <script type="text/javascript">
       $(document).ready(function () {
            $('table').each(function () {
                $(this).prepend('<thead></thead>');
                $(this).find('thead').append($(this).find("tr:eq(0)"));
                //$(this).append('<tfoot><tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr></tfoot>');
            });

            var table = $('#grd_Requests').dataTable({
                "iDisplayLength": 50,
                "aLengthMenu": [[10, 25, 50, 100, 250, 500, -1], [10, 25, 50, 100, 250, 500, "All"]]
            });
           
           // $('#grd_Requests tfoot th').each(function () {
           //     var title = $('#grd_Requests thead th').eq($(this).index()).text();
           //     $(this).html('<input type="text" placeholder="Search ' + title + '" />');
           // });

           //// DataTable
           

           //// Apply the search
           // table.columns().every(function () {
           //     var that = this;

           //     $('input', this.footer()).on('keyup change', function () {
           //         if (that.search() !== this.value) {
           //             that
           //                 .search(this.value)
           //                 .draw();
           //         }
           //     });
           // });
            //table.columnFilter({
            //    aoColumns: [{ type: "select", values: ['Gecko', 'Trident', 'KHTML', 'Misc', 'Presto', 'Webkit', 'Tasman'] },
            //             { type: "text" },
            //             null,
            //             { type: "number" },
            //     { type: "select" }
            //    ]

            //});
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
            $(tableTools.fnContainer()).insertBefore('#grd_Requests_wrapper');
            $('#ddlPayType').change(function () {
                table.fnFilter($('#ddlPayType :selected').text());
            });
            $('#ddlSuspendCount').change(function () {
                //table.columns(6).search($('#ddlSuspendCount :selected').text()).draw();
                //var n = "n";
                //table.fnFilter($('#ddlSuspendCount :selected').text());
            });
            
            window.addDialog = function (t) {
                var requestId = $("#hdnreq").val();
                var btnall = $(t).attr("id");
                var template = $(t).parent().children(':first').clone();
                var template1 = template.removeAttr('onclick');

                if (btnall === '<%=btn_ApproveSelected.ClientID %>') {
                    $('.btn.btn-primary').hide();
                    $("#<%=BtnSaveWithLiquidation.ClientID %>").hide();
                    $('#updown-results').parent().children(':first').show();
                    $('#cancel-results').parent().children(':first').show();
                    $('#btncancelmodal').hide(); //hide small btn of cancel
                    $('#reactivate-results').parent().children(':first').show();
                    $('#unsuspend-result').parent().children(':first').show();
                    $('#<%=RBLUnsuspend.ClientID %>').show();

                    $('#<%=btnunsuspendselectall.ClientID %>').show();
                    $('#multi').show();
                    $('#xcntnt').hide();
                    $('#one').hide();
                    $('#TbDaysCount').val(0);
                   
                } else {
                    //$('#<%=btn_ApproveSelected.ClientID %>').hide();
                    $("#<%=BtnSaveWithLiquidation.ClientID %>").show();
                    $('.btn.btn-primary').show();
                    $('#updown-results').parent().children(':first').hide();
                    $('#cancel-results').parent().children(':first').hide();
                    $('#btncancelmodal').show(); //show small btn of cancel
                    $('#reactivate-results').parent().children(':first').hide();
                    $('#unsuspend-result').parent().children(':first').hide();
                    $('#multi').hide();
                    $('#one').show();

                }
                var id = $(t).attr("data-id");
                $('#UnsuspendId').val(id);
                if (requestId === "1") {
                    //var template = $(t).parent().children(':first').clone();
                    //var template1 = template.removeAttr('onclick');
                    $('#updown-results').children().remove();
                    $('#updown-results').append($(template1).css('display', 'block'));
                    $('#upDwon').modal('show');
                    return false;
                }
                if (requestId === "6") {
                    var canceltemplate = $(t).parent().children(':first').clone();
                    $("#cancel-results").children().remove();
                    $("#cancel-results").append($(template1).css('display', 'block'));
                    $("#cancelModal").modal("show");
                    return false;
                }
                if (requestId === "7") {
                    var reactivateTemplate = $(t).parent().children(':first').clone();
                    $('#reactivate-results').children().remove();
                    $('#reactivate-results').append($(template1).css('display', 'block'));
                    $('#reactivateModal').modal('show');
                    return false;
                }
                if (requestId === "3") {
                    var UnsuspendTemplate = $(t).parent().children(':first').clone();
                    $('#unsuspend-results').children().remove();
                    $('#unsuspend-results').append($(template1).css('display', 'block'));
                    var ids = $(t).attr("data-id");
                    $('#UnsuspendId').val(ids);
                    $('#myModal').modal('show');
                    return false;
                }

                var me = $(t);
                var holder = $('#SelectedStuff');
                var allAttrs = $(me).attr('data-id') + ',' + $(me).attr('data-woid');
                var content = $(t).parent().children(':first').clone();
                $('#xcntnt').children().remove();
                $('#xcntnt').append($(template1).css('display', 'block'));
                $(holder).val(allAttrs);

                $('#xdialog').modal('show');
                $('.ui-widget-overlay').removeClass('ui-front');
                return false;
            }

            window.rej = function (it) {
                var id = $(it).attr('data-request-id');
                $('#RejectedRequestId').val(id);
                var btnrej = $(it).attr("id");
                if (btnrej === '<%=btn_RejectSelected.ClientID %>') {
                    $('.btn.btn-primary').hide();
                    var rej = $(it).parent().children(':last').clone();
                    $('#reject-results').children().remove();
                    $('#reject-results').append($(rej).css('display', 'block'));
                    $('#rejectModal').modal('show');
                    return false;
                } else {
                    $('<%=btn_RejectSelected.ClientID %>').hide();
                    $('.btn.btn-primary').show();
                    $('#reject-results').children().remove();
                    $('#rejectModal').modal('show');

                }

            }

           window.approv=function(t) {
               $('#<%=RBLUnsuspend.ClientID %>').hide();

               $('#<%=btnunsuspendselectall.ClientID %>').hide();

               $('.btn.btn-primary').show();
               var id = $(t).attr("data-id");
               $('#UnsuspendId').val(id);
               $('#TbDaysCount').val(0);

               $('#myModal').modal('show');
           }
        });

             
       

        $(function () {
            if ($("#EditRequestDate").val() === "1") {
                $('input[data-date-picker="dp"]').attr("readonly", "readonly");
                $('input[data-date-picker="dp"]').removeAttr("data-date-picker");
            }

            $('input[data-date-picker="dp"]').datepicker({

                dateFormat: 'dd/mm/yy'
            });


            $('a[data-select="toRevect"]').click(function () {
                var id = $(this).attr('data-request-id');
                $('#RejectedRequestId').val(id);
                var btnrej = $(this).attr("id");
                if (btnrej === '<%=btn_RejectSelected.ClientID %>') {
                    $('.btn.btn-primary').hide();
                    var rej = $(this).parent().children(':last').clone();
                    $('#reject-results').children().remove();
                    $('#reject-results').append($(rej).css('display', 'block'));
                    $('#rejectModal').modal('show');
                    return false;
                } else {
                    $('<%=btn_RejectSelected.ClientID %>').hide();
                    $('.btn.btn-primary').show();
                    $('#reject-results').children().remove();
                    $('#rejectModal').modal('show');

                }

            });

            var requestId = $("#hdnreq").val();
            var direction = $('#direction').html();
            $('input[type="radio"]')
                .css({ 'float': direction === "1" ? "right" : "left", 'margin': '3px' });
            $('#BtnSave').bind('click', function () {
                $(this).attr('disabled', 'disabled');
            });

         <%--  $('a[selector="dialog"]').click(function () {
                var btnall = $(this).attr("id");
                if (btnall === '<%=btn_ApproveSelected.ClientID %>') {
                $('.btn.btn-primary').hide();
                $("#<%=BtnSaveWithLiquidation.ClientID %>").hide();
                    $('#updown-results').parent().children(':first').show();
                    $('#cancel-results').parent().children(':first').show();
                    $('#btncancelmodal').hide(); //hide small btn of cancel
                    $('#reactivate-results').parent().children(':first').show();
                    $('#unsuspend-result').parent().children(':first').show();
                    $('#<%=RBLUnsuspend.ClientID %>').show();

                    $('#<%=btnunsuspendselectall.ClientID %>').show();
                $('#multi').show();
                $('#xcntnt').hide();
                $('#one').hide();
                $('#TbDaysCount').val(0);
            } else {
                //$('#<%=btn_ApproveSelected.ClientID %>').hide();
                $("#<%=BtnSaveWithLiquidation.ClientID %>").show();
                $('.btn.btn-primary').show();
                $('#updown-results').parent().children(':first').hide();
                $('#cancel-results').parent().children(':first').hide();
                $('#btncancelmodal').show();//show small btn of cancel
                $('#reactivate-results').parent().children(':first').hide();
                $('#unsuspend-result').parent().children(':first').hide();
                $('#multi').hide();
                $('#one').show();

            }
            var id = $(this).attr("data-id");
            $('#UnsuspendId').val(id);
            if (requestId === "1") {
                var template = $(this).parent().children(':first').clone();
                $('#updown-results').children().remove();
                $('#updown-results').append($(template).css('display', 'block'));
                $('#upDwon').modal('show');
                return false;
            }
            if (requestId === "6") {
                var canceltemplate = $(this).parent().children(':first').clone();
                $("#cancel-results").children().remove();
                $("#cancel-results").append($(canceltemplate).css('display', 'block'));
                $("#cancelModal").modal("show");
                return false;
            }
            if (requestId === "7") {
                var reactivateTemplate = $(this).parent().children(':first').clone();
                $('#reactivate-results').children().remove();
                $('#reactivate-results').append($(reactivateTemplate).css('display', 'block'));
                $('#reactivateModal').modal('show');
                return false;
            }
            if (requestId === "3") {
                var UnsuspendTemplate = $(this).parent().children(':first').clone();
                $('#unsuspend-results').children().remove();
                $('#unsuspend-results').append($(UnsuspendTemplate).css('display', 'block'));
                var ids = $(this).attr("data-id");
                $('#UnsuspendId').val(ids);
                $('#myModal').modal('show');
                return false;
            }

            var me = $(this);
            var holder = $('#SelectedStuff');
            var allAttrs = $(me).attr('data-id') + ',' + $(me).attr('data-woid');
            var content = $(this).parent().children(':first').clone();
            $('#xcntnt').children().remove();
            $('#xcntnt').append($(content).css('display', 'block'));
            $(holder).val(allAttrs);

            $('#xdialog').modal('show');
            $('.ui-widget-overlay').removeClass('ui-front');
            return false;
        });--%>
            var isunsuspend = $('#HfIsUnsuspend').val();
            if (isunsuspend == "3") {
                $('a[selector="dialog"]').hide();
                $('a[id="<%=btn_ApproveSelected.ClientID %>"]').show();
                $('button[data-select="approve"]').show();

                $('input[data-check="select"]').hide();
            } else {
                $('a[selector="dialog"]').show();
                $('button[data-select="approve"]').hide();
                //$('#SelectHeader').show();
                $('input[data-check="select"]').show();
            }


           <%-- $('button[data-select="approve"]').click(function () {
                $('#<%=RBLUnsuspend.ClientID %>').hide();

                $('#<%=btnunsuspendselectall.ClientID %>').hide();


                var id = $(this).attr("data-id");
                $('#UnsuspendId').val(id);
                $('#TbDaysCount').val(0);

                $('#myModal').modal('show');

            });--%>


            $('#RblCancelOptions td').css('width', '250px');

            $('#cancelReactivate').on('click', function () {
                $('#reactivateModal').modal('hide');
            });


            $('#rblFlag').hide();
            $('#RbToNew').click(function () {
                $('#rblFlag').fadeOut();
            });
            $('#RbToActive').click(function () {
                $('#rblFlag').fadeIn();
            });
            if ($('#grd_Requests').width() > 1058) {
                $('#grd_Requests').css({
                    //"font-family": "serif",
                    "font-size": "11px"
                });
            }

        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#selecctall').click(function () {  //on click 
                if (this.checked) { // check select status
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes
                    });
                } else {
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = false; //deselect all checkboxes 
                    });
                }
            });

            //$("#grd_Requests:last td table").addClass("pagination");
            //$("#grd_Requests:last td table tbody tr td").addClass("btn btn-xs");
            //$("#grd_Requests:last td table tbody tr td a").addClass("btn btn-xs btn-light");
            //$("#grd_Requests:last td table tbody tr td a").parent().addClass("btn btn-xs btn-light");
            //$("#grd_Requests:last td table tbody tr td span").addClass("btn btn-xs btn-info");
            //$("#grd_Requests:last td table tbody tr td span").parent().addClass("btn btn-xs btn-info");

        });



    </script>
    <style type="text/css">
        .well {
            max-width: 100%;
            padding: 10px;
        }

        .ui-corner-all {
            padding: 10px;
        }
        /* ui-helper-clearfix*/
    </style>
</asp:Content>

