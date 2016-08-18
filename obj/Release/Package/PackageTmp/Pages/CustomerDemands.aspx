<%@ Page Title="<%$Resources:Tokens,CustomerDemands%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerDemands.aspx.cs" Inherits="NewIspNL.Pages.CustomerDemands" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="NewIspNL.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        
         @media only screen and (max-width: 800px) {
             .table {
                 display: block;
                 overflow-x: scroll !important;
                 width: 320px;
                 overflow-y: hidden !important;
             }
         }
    </style>
    <div runat="server" id="Mother">
        <span runat="server" visible="False" id="Msg" class="alert alert-success"></span>
        <span runat="server" visible="False" id="Msg2" class="alert alert-danger"></span>
        <div id="portalRequest" runat="server"></div>
        <div class="view">
            <fieldset>
                <div class="page-header">
                    <h1>
                        <asp:Literal Text="<%$Resources:Tokens,CustomerDemands%>" runat="server" ID="lblTitle" /></h1>
                </div>
                <div class="well">
                    <div>
                        <div runat="server" id="GovBox">
                            <div>
                                <label for="DdGovernorates">
                                    <asp:Literal Text="<%$Resources:Tokens,Governrate %>" runat="server" /></label>
                            </div>
                            <div>
                                <asp:DropDownList runat="server" ID="DdGovernorates">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdGovernorates"
                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <div>
                                <label for="TPhone">
                                    <asp:Literal Text="<%$Resources:Tokens,Phone%>" runat="server" /></label>
                            </div>
                            <div>
                                <asp:TextBox runat="server" ID="TPhone" ClientIDMode="Static"> </asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TPhone"
                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <button class="btn btn-success" type="submit" runat="server" id="bSearch" clientidmode="Static" onserverclick="BSearch_OnServerClick">
                                        <i class="icon-white icon-search"></i>&nbsp;<%=Tokens.Search %></button>
                                </td>
                                <td>
                                    <%--  <span id="go-to-details" class="btn btn-success"><i class="icon-white icon-question-sign">
                                    </i>&nbsp;<%=Tokens.Details %></span></td>--%>

                                    <asp:Button ID="Button3" class="btn btn-success" OnClick="Direct" OnClientClick="check()" runat="server" Text="<%$Resources:Tokens,Details %>" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <button type="button" id="addDemand" class="btn btn-success" runat="server" clientidmode="Static">
                                        <i class="icon-white icon-plus-sign"></i>&nbsp;<%=Tokens.NewDemand %></button>
                                </td>
                                <td>
                                    <span id="BNextD2" runat="server"><span class="btn btn-success" id="BAddNextDemand" runat="server" clientidmode="Static">
                                        <%= Tokens.AddNextMonthDemand %></span> <span style="display: none;">
                                            <asp:Button class="btn btn-success" runat="server" ID="BNextD" OnClick="NextMonthDemand"
                                                ClientIDMode="Static" Text="<%$Resources:Tokens,AddNextMonthDemand %>"></asp:Button></span>
                                    </span>
                                    <% if (CanAddNext)
                                       { %>
                                    <button id="btnAddPrepaid" class="btn btn-success" type="button"><i class="icon-white icon-dollar"></i>&nbsp;<%=Tokens.Prepay %></button>
                                    <% } %>
                                    <button id="btnPrepayReceipt" class="btn btn-success" type="button"><i class="icon-white icon-file-text"></i>&nbsp;<%=Tokens.PrepayReceipt %></button>

                                    <button class="btn btn-success" runat="server" onclick="showhis()" id="history" type="button"><i class="icon-white icon-globe"></i>&nbsp;<%=Tokens.payhistory %></button>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="HfCustomerId" Value="0" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="HFcheck" Value="0" ClientIDMode="Static" />
                    </div>
                </div>
            </fieldset>
        </div>


        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <%= Tokens.Customer %></h3>
                <div class="alert alert-danger" id="suspendMsg" runat="server">
                </div>
                <asp:HiddenField runat="server" ID="hdfDaysOfSuspend" ClientIDMode="Static" />
                <p runat="server" id="handleCanSuspend">
                    <button class="btn btn-primary" runat="server" id="Suspsend" visible="False" onserverclick="SuspendOrder">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.Suspend %></button>
                    <button class="btn btn-danger" id="btnExtragiga" type="button"><i class="icon-white icon-globe"></i>&nbsp;<%=Tokens.ExtraGigas %></button>
                    <button class="btn btn-primary" id="btnUpDown" runat="server" type="button" clientidmode="Static"><i class="icon-white icon-sort"></i>&nbsp;<%=Tokens.Upgrade_Downgrade %></button>
                    <button class="btn btn-yellow" id="btnChangeIPPackage" runat="server" type="button" clientidmode="Static"><i class="icon-white icon-flag"></i>&nbsp;<%=Tokens.MenuChangeIPPackage %></button>

                </p>
                <p id="handelcanActive" runat="server">
                    <button class="btn btn-success" id="btnActive" type="button">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.MenuUnsuspend %></button><%--<%= Tokens.ToActiveCustomer %></button>--%>
                    <button class="btn btn-success" id="btnlimitActive" type="button">
                        <i class="icon-white icon-ok"></i>&nbsp;<%= Tokens.MenulimitUnsuspend %></button><%--<%= Tokens.ToActiveCustomer %></button>--%>
                </p>
                <div>
                    <asp:GridView runat="server" ID="GvCustomerData" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed center">
                        <Columns>
                            <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer%>" />
                            <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:BoundField DataField="Package" HeaderText="<%$Resources:Tokens,Package%>" />
                            <asp:BoundField DataField="IpPackage" HeaderText="<%$Resources:Tokens,Ip.Package%>" />
                            <asp:BoundField DataField="Branch" HeaderText="<%$Resources:Tokens,Branch%>" />
                            <asp:BoundField DataField="ServicProvider" HeaderText="<%$Resources:Tokens,Service.Provider %>" />
                            <asp:BoundField DataField="Reseller" HeaderText="<%$Resources:Tokens,Reseller%>" />
                            <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:Tokens,PaymentType%>" />
                            <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                             <asp:BoundField DataField="RequestDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="<%$Resources:Tokens,InvoiceDueDate%>" />
                            <asp:BoundField DataField="Central" HeaderText="<%$Resources:Tokens,Central%>" />
                            <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                            <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Literal Text="<%$Resources:Tokens,Unpaid %>" runat="server" /></h3>
                    <div>
                        <asp:GridView runat="server" ID="GvUnpaid" AutoGenerateColumns="False" OnDataBound="GvUnpaid_OnDataBound"
                            CssClass="table table-bordered table-condensed center" Font-Size="11px">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer%>" />
                                <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                                <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                                <asp:BoundField DataField="From" HeaderText="<%$Resources:Tokens,From%>" />
                                <asp:BoundField DataField="To" HeaderText="<%$Resources:Tokens,To%>" />
                                <asp:BoundField DataField="ForMonth" HeaderText="<%$Resources:Tokens,Month%>" />
                                <asp:BoundField DataField="ForYear" HeaderText="<%$Resources:Tokens,Year%>" />
                                <%--<asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />--%>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Notes%>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Notes") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount%>" />
                                <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User%>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton data-comment="comment" ID="BPay" CssClass="btn btn-success btn-sm"
                                            runat="server" data-fruit='<%#Eval("Amount") %>' CommandArgument='<%#Eval("Id") %>' data-demand='<%#Eval("Id") %>'
                                            ToolTip="<%$Resources:Tokens,Pay %>" data-rel="tooltip"><i class="icon-dollar icon-only"></i></asp:LinkButton>
                                        &nbsp;<asp:LinkButton runat="server" ID="EditBtn" CommandArgument='<%#Eval("Id") %>' CssClass="btn btn-primary btn-sm"
                                            ToolTip="<%$Resources:Tokens,Edit %>" data-demand='<%#Eval("Id") %>' data-from='<%#Eval("From") %>' data-comm='<%#Eval("Commisstion") %>'
                                            data-to='<%#Eval("To") %>' data-notes='<%#Eval("Notes") %>' data-amount='<%#Eval("Amount") %>'
                                            data-select="for-edit" data-rel="tooltip"><i class="icon-edit icon-only"></i></asp:LinkButton>





                                        &nbsp;<asp:LinkButton runat="server" ID="DeleteBtn" OnClientClick="return areyousure()" data-rel="tooltip" CssClass="btn btn-danger btn-sm"
                                            CommandArgument='<%#Eval("Id") %>' OnClick="DeleteCurrentDemand" ToolTip="<%$Resources:Tokens,Delete %>">
                                            <i class="icon-trash icon-only"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                    <div class="row alert alert-info" runat="server" id="divCredits" visible="False">
                        <%--                        <table>
                <tr>
                    <td><%=Tokens.Total %></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td></td>
                </tr>
            </table>--%>
                        <div class="col-xs-3">
                            <%=Tokens.Credit %> :
                            <label runat="server" id="lblCredit" clientidmode="Static"></label>
                            <asp:HiddenField runat="server" ID="hdnCredit" />
                        </div>
                        <div class="col-xs-3">
                            <%=Tokens.Net %> :
                            <label runat="server" id="lblTotalUnpaid" clientidmode="Static"></label>
                        </div>

                    </div>
                </fieldset>
            </div>
        </div>
        <div class="view">

            <div>
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Paid %>" runat="server" /></h3>
                    <div>
                        <asp:GridView runat="server" ID="GvPaid" OnDataBound="GvPaid_OnDataBound" AutoGenerateColumns="False"
                            CssClass="table table-bordered table-condensed center">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Customer" HeaderText="<%$Resources:Tokens,Customer%>" />
                                <asp:BoundField DataField="Phone" HeaderText="<%$Resources:Tokens,Phone%>" />
                                <asp:BoundField DataField="Offer" HeaderText="<%$Resources:Tokens,Offer%>" />
                                <asp:BoundField DataField="From" HeaderText="<%$Resources:Tokens,From%>" />
                                <asp:BoundField DataField="To" HeaderText="<%$Resources:Tokens,To%>" />
                                <asp:BoundField DataField="ForMonth" HeaderText="<%$Resources:Tokens,Month%>" />
                                <asp:BoundField DataField="ForYear" HeaderText="<%$Resources:Tokens,Year%>" />
                                <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount%>" />
                                <asp:BoundField DataField="PaymentComment" HeaderText="<%$Resources:Tokens,Comment%>" />
                                <asp:BoundField DataField="User" HeaderText="<%$Resources:Tokens,User%>" />
                                <asp:BoundField DataField="PaymentDate" HeaderText="<%$Resources:Tokens,PaymentDate%>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <a runat="server" data-work='<%#Eval("Id") %>'
                                            data-app="cancelPay" id="Unpay" class="btn btn-danger btn-sm" clientidmode="Static" title='<%$Resources:Tokens,CancelPayment %>' data-rel="tooltip"><i class="icon-undo icon-only"></i></a>
                                        <%--<asp:Button CssClass="btn btn-danger" runat="server" ID="Unpay" CommandArgument='<%#Eval("Id") %>'
                                                    Text="<%$Resources:Tokens,CancelPayment %>" OnClick="CancelPayment"></asp:Button>--%>
                                        &nbsp;<%--<% if (WidthOption.WidthOfReciept == false)
                                                 { %><a target="_blank" class="btn btn-primary btn-sm" href='<%# string.Format("DemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString())) %>' data-rel="tooltip" title='<%=Tokens.Print %>'><i class="icon-print icon-only"></i></a><% }
                                                 else
                                                 { %><a target="_blank" class="btn btn-primary btn-sm" href='<%#string.Format("smallDemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString()))  %>' data-rel="tooltip" title="<%=Tokens.Print %>"><i class="icon-print icon-only"></i></a><% } %>
                                        --%>
                                        <a target="_blank" class="btn btn-primary btn-sm" href='<%# string.Format("DemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString())) %>' data-rel="tooltip" title='<%=Tokens.HalfReciept %>'><i class="icon-print icon-only"></i></a>
                                        <a target="_blank" class="btn btn-info btn-sm" href='<%#string.Format("smallDemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString()))  %>' data-rel="tooltip" title="<%=Tokens.Reciept2 %>"><i class="icon-print icon-only"></i></a>

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert">
                                    <%=Tokens.NoResults %>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <%-- <span style="display: none" id="get-url" data-url="<%=DetailsUrl %>"></span>--%>
    <div id="x-dialog" style="display: none">
        <div>
            <div>
                <div>
                    <label for="">
                        <asp:Literal Text="<%$Resources:Tokens,MonthsCount %>" runat="server" />
                    </label>
                    <div>
                        <asp:DropDownList runat="server" ID="DdlMonths">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div id="demand-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H5">
                        <%=Tokens.AddDemand %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Literal Text="<%$Resources:Tokens,Customer %>" runat="server" />&nbsp;:&nbsp;
                                    </td>
                                    <td>
                                        <span runat="server" id="SCustomer"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal Text="<%$Resources:Tokens,Phone %>" runat="server" />&nbsp;:&nbsp;
                                    </td>
                                    <td>
                                        <span runat="server" id="SPhone"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal Text="<%$Resources:Tokens,Offer %>" runat="server" />&nbsp;:&nbsp;
                                    </td>
                                    <td>
                                        <span runat="server" id="SOffer"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator runat="server" ValidationGroup="dd" ID="RF" ControlToValidate="TbFrom"
                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal Text="<%$Resources:Tokens,From %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ValidationGroup="dd" ID="TbFrom" data-select="dt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ValidationGroup="dd" runat="server" ID="RT" ControlToValidate="TbTo"
                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal Text="<%$Resources:Tokens,To %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TbTo" data-select="dt" ValidationGroup="dd" />
                                        <asp:CompareValidator ID="CompareValidator1" ValidationGroup="dd" runat="server"
                                            ErrorMessage="<%$Resources:Tokens,DatePeriodMsg %>" ControlToCompare="TbFrom"
                                            ControlToValidate="TbTo" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <%--                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlSaves"
                            ErrorMessage="RequiredFieldValidator" ValidationGroup="dd">*</asp:RequiredFieldValidator>
                            <asp:Literal runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal>
                    </td>
                    <td>
                                            <div>
                        <asp:DropDownList runat="server" ID="ddlSaves" ValidationGroup="dd" ClientIDMode="Static" DataTextField="SaveName" DataValueField="Id" />
                        
                    </div>
                    </td>
                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ValidationGroup="dd" runat="server" ID="RequiredFieldValidator3"
                                            ControlToValidate="TbAmount" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal Text="<%$Resources:Tokens,Amount %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TbAmount" ValidationGroup="dd" />
                                        <asp:CompareValidator ID="CompareValidator2" ValidationGroup="dd" runat="server"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" ControlToValidate="TbAmount"
                                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <%--                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:CheckBox ID="CbPaid" Text="<%$Resources:Tokens,PaidDone %>" runat="server" />&nbsp;
                        
                    </td>
                </tr>--%>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbCommission" Text="<%$Resources:Tokens,ResellerCommission %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ValidationGroup="dd" runat="server" ID="RequiredFieldValidator4"
                                            ControlToValidate="TbAmount" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Notes %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TbNotes" ValidationGroup="dd" />
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <p>
                        <button runat="server" clientidmode="Static" id="BSaveDemand" onserverclick="SaveDemand"
                            validationgroup="dd" type="button" class="btn btn-primary">
                            <i class="icon-white icon-ok"></i>&nbsp;<%=Tokens.Save %></button>&nbsp;|&nbsp;
                <button id="cancel-dialog" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True">
                    <i class="icon-white icon-remove"></i>&nbsp;<%=Tokens.Cancel %></button>&nbsp;
                    </p>
                </div>
            </div>
        </div>
    </div>
    <div id="comment-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <asp:HiddenField runat="server" ID="HfDemandId" ClientIDMode="Static" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H1">
                        <%=Tokens.PaymentComment %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <label><%=Tokens.Box %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlbox" ValidationGroup="comment" ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="ddlbox"
                                    runat="server" ID="RequiredFieldValidator11" ValidationGroup="comment" />
                            </div>
                            <label><%=Tokens.DiscoundFromBox %></label>
                            <div>
                                <asp:TextBox runat="server" ID="txtDiscoundBox" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="txtDiscoundBox"
                                    runat="server" ID="RequiredFieldValidator10" ValidationGroup="comment" />
                            </div>
                            <label for="TbAmountincomment">
                                <%=Tokens.Amount %>
                            </label>
                            <div>
                                <input type="text" runat="server" id="TbAmountincomment" clientidmode="Static" />
                                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbAmountincomment"
                                    runat="server" ID="RequiredFieldValidator9" ValidationGroup="comment" />
                                <span style="float: left;" id="lblAmount"></span>
                            </div>
                            <label for="ddlSavesPay">
                                <%=Tokens.sav %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlSavesPay" DataTextField="SaveName" DataValueField="Id" ValidationGroup="comment" />
                                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="ddlSavesPay"
                                    runat="server" ID="RequiredFieldValidator13" ValidationGroup="comment" />
                            </div>

                            <div>
                                <asp:CheckBox runat="server" ID="CheckAddRequestPayment" Text="<%$Resources:Tokens,AddRequestPayment %>" />
                            </div>
                            <label for="TbComment">
                                <%=Tokens.Comment %></label>
                            <div>
                                <asp:TextBox runat="server" ID="TbComment" TextMode="MultiLine" ValidationGroup="comment" />
                                <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="TbComment"
                                    runat="server" ID="RComment" ValidationGroup="comment" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <p>
                        <asp:HiddenField runat="server" ID="hf_boxId" ClientIDMode="Static" />
                        <asp:Button Text="<%$Resources:Tokens,Pay %>" runat="server" ValidationGroup="comment" ID="PDialogPay" OnClick="PayDemand" CssClass="btn btn-primary" UseSubmitBehavior="false" OnClientClick="save()" />
                        &nbsp;
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                    </p>
                </div>
            </div>
        </div>

    </div>
    <div id="edit-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <asp:HiddenField runat="server" ID="HfEditDemandId" ClientIDMode="Static" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H2">
                        <%=Tokens.Edit %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator runat="server" ValidationGroup="edit" ID="RequiredFieldValidator5"
                                            ControlToValidate="TbEFrom" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal Text="<%$Resources:Tokens,From %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ValidationGroup="edit" ClientIDMode="Static" ID="TbEFrom" data-select="dt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ValidationGroup="edit" runat="server" ID="RequiredFieldValidator6"
                                            ControlToValidate="TbETo" ErrorMessage="<%$Resources:Tokens,Required %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:Literal ID="Literal7" Text="<%$Resources:Tokens,To %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TbETo" data-select="dt" ValidationGroup="edit" ClientIDMode="Static" />
                                        <asp:CompareValidator ID="CompareValidator3" ValidationGroup="edit" runat="server"
                                            ErrorMessage="<%$Resources:Tokens,DatePeriodMsg %>" ControlToCompare="TbEFrom"
                                            ControlToValidate="TbETo" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ValidationGroup="edit" runat="server" ID="RequiredFieldValidator7"
                                            ControlToValidate="TbEAmount" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal Text="<%$Resources:Tokens,Amount %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TbEAmount" ValidationGroup="edit" ClientIDMode="Static" />
                                        <asp:CompareValidator ID="CompareValidator4" ValidationGroup="edit" runat="server"
                                            ErrorMessage="<%$Resources:Tokens,NumbersOnly %>" ControlToValidate="TbEAmount"
                                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:CheckBox runat="server" ClientIDMode="Static" ID="CBResellerCommession" Text="<%$Resources:Tokens,ResellerCommission %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ValidationGroup="edit" runat="server" ID="RequiredFieldValidator8"
                                            ControlToValidate="TbENotes" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                                        <asp:Literal Text="<%$Resources:Tokens,Notes %>" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TbENotes" ValidationGroup="edit" ClientIDMode="Static" />
                                        <label>إزالة كود html إن وجد</label>
                                    </td>

                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <p>
                        <asp:Button Text="<%$Resources:Tokens,Edit %>" runat="server" CausesValidation="True" ID="PEditDemand" ValidationGroup="edit"
                            OnClick="EditUnpaidDemand" CssClass="btn btn-success" />
                        <button type="button" class="btn btn-danger" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                    </p>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfId" ClientIDMode="Static" />
    <div id="cancelPayModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="myModalLabel">
                        <%= Tokens.CancelPayment %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlSavesCancelPay" DataTextField="SaveName" DataValueField="Id" ValidationGroup="cancelp" />
                                    <asp:RequiredFieldValidator ControlToValidate="ddlSavesCancelPay" ValidationGroup="cancelp" ID="RequiredFieldValidator15" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnRecieveFromCompany" ValidationGroup="cancelp"
                        CssClass="btn btn-primary" Text="<%$Resources:Tokens,Save %>"
                        OnClick="CancelPayment" UseSubmitBehavior="false" OnClientClick="save2()" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="unsuspendModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H3">
                        <%=Tokens.MenuUnsuspend %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <%--            <asp:HiddenField runat="server" ID="UnsuspendId" ClientIDMode="Static" />--%>

                        <div>
                            <div>
                                <label for="TbDaysCount">
                                    <%= Tokens.DaysCountofLiqudation %></label>
                                <div>
                                    <asp:TextBox runat="server" ValidationGroup="ded" ClientIDMode="Static" ID="TbDaysCount" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="TbDaysCount" ID="sdfsd"
                                        ValidationGroup="ded" runat="server" />
                                </div>
                            </div>
                            <div runat="server" id="UnsesuspendContainer">
                                <label for="TbUnsuspendDate">
                                    <%= Tokens.Date %>
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbUnsuspendDate" data-select="dt"></asp:TextBox>
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
                    <%-- <asp:HiddenField runat="server" ID="ClickedBtn" Value="0" />--%>
                    <%--<asp:HiddenField runat="server" ClientIDMode="Static" ID="btnclickallornot" />--%>
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
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="ExtraGigModal" class="bootbox modal" tabindex="-1" role="dialog" aria-labelledby="H1"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H4">
                        <%=Tokens.Extra_Gigas %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div>
                            <label>
                                <asp:Label ID="Label31" runat="server" Text="<%$Resources:Tokens,Extra.Gigas %>"></asp:Label>
                            </label>
                            <asp:DropDownList runat="server" ID="DdlExtraGigas">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="DdlExtraGigas"
                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="Extra"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="one">
                         
                        <button class="btn btn-primary" runat="server" id="BtnSave2"
                            validationgroup="Extra" type="button" onserverclick="AddExtraGiga">
                            <i class="icon-ok icon-white"></i><%=Tokens.Confirm %>
                        </button>
                        <button type="button" id="cancel" class="btn btn-danger" data-dismiss="modal" data-bb-handler="cancel">
                            <i class="icon-white icon-remove"></i>&nbsp;
            <asp:Literal ID="Literal5" Text="<%$Resources:Tokens,Cancel %>" runat="server" /></button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="addCredit" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H6">
                        <%= Tokens.Prepay %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlPrePay" DataTextField="SaveName" DataValueField="Id" ValidationGroup="btnPrePay" />
                                    <asp:RequiredFieldValidator ControlToValidate="ddlPrePay" ValidationGroup="btnPrePay" ID="RequiredFieldValidator16" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Amount %>"></asp:Label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtPrePayAmount"></asp:TextBox>
                                    <asp:RequiredFieldValidator ControlToValidate="txtPrePayAmount" ValidationGroup="btnPrePay" ID="RequiredFieldValidator17" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                                <asp:Label runat="server" Text="<%$Resources:Tokens,Notes %>"></asp:Label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ControlToValidate="txtNotes" ValidationGroup="btnPrePay" ID="RequiredFieldValidator12" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="Button1" ValidationGroup="btnPrePay"
                        CssClass="btn btn-primary" Text="<%$Resources:Tokens,Save %>"
                        OnClick="PrePayment" />
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="searchCredit" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H7">
                        <%= Tokens.Search %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <%--                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Label>
                    <div>
                        <asp:DropDownList runat="server" ID="DropDownList1" DataTextField="SaveName" DataValueField="Id" ValidationGroup="btnPrePay"/>
                    <asp:RequiredFieldValidator ControlToValidate="ddlPrePay" ValidationGroup="btnPrePay" ID="RequiredFieldValidator12" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>--%>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Tokens,ReceiptNumber %>"></asp:Label>
                                <div>
                                    <asp:TextBox runat="server" ID="txtReceiptNumber"></asp:TextBox>
                                    <asp:RequiredFieldValidator ControlToValidate="txtReceiptNumber" ValidationGroup="btnSearchPrePay"
                                        ID="RequiredFieldValidator18" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="Button2" ValidationGroup="btnSearchPrePay"
                        CssClass="btn btn-primary" Text="<%$Resources:Tokens,Search %>"
                        OnClick="SearchPrePaidReceipt" />
                    <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>


    <div id="his-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

        <div style="background-color: white; margin-top: 100px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H5">
                        <%=Tokens.payhistory %></h4>
                </div>

                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">


                            <asp:GridView runat="server" ID="payhis" OnDataBound="payhis_OnDataBound" AutoGenerateColumns="False"
                                CssClass="table table-bordered table-condensed center">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:Label ID="LNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Id" HeaderText="<%$Resources:Tokens,ReceiptNumber%>" />
                                    <asp:BoundField DataField="CreditAmount" HeaderText="<%$Resources:Tokens,Amount%>" />
                                    <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName%>" />
                                    <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Time%>" />
                                    <asp:BoundField DataField="Notes" HeaderText="<%$Resources:Tokens,Notes%>" />


                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert">
                                        <%=Tokens.NoResults %>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="UpDownModal" class="bootbox modal" tabindex="-1" role="dialog" aria-labelledby="H1"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H4">
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
                         <div>
                            <label>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,New.Service.Package %>"></asp:Label>
                            </label>
                            <asp:DropDownList runat="server" ID="ddl_ServicePackage" ValidationGroup="UpDown">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddl_ServicePackage"
                                ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="UpDown"></asp:RequiredFieldValidator>
                        </div>

               
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="updown-results"></div>
                    <button class="btn btn-primary" type="button" validationgroup="UpDown" runat="server"
                        onserverclick="btn_ApproveSelected_Click">
                        <%=Tokens.Upgrade_Downgrade %></button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="ChangeIPPackageModal" class="bootbox modal" tabindex="-1" role="dialog" aria-labelledby="H1"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H4">
                        <%=Tokens.New_IP_Package %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div id="tr_NewIPPackage" runat="server" style="padding: 5px;">
                            <label style="width: 100px">
                                <asp:Label ID="Label30" runat="server" Text="<%$Resources:Tokens,New.IP.Package %>"></asp:Label>
                            </label>
                            <asp:DropDownList ID="ddl_IpPackage" CssClass="width-60" runat="server" Width="155px" ValidationGroup="x">
                            </asp:DropDownList>
                            &nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ddl_IpPackage"
                                        ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="x">*</asp:RequiredFieldValidator>
                        </div>
                        <div runat="server" id="MiscTbContainer">
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <div id="xcntnt">
                    </div>
                    <div id="one">
                        <button class="btn btn-primary" runat="server" id="BtnSaveIpPack" clientidmode="Static"
                            validationgroup="x" type="button" onserverclick="BtnSaveIpPack_OnServerClick">
                            <i class="icon-ok icon-white"></i>&nbsp;<asp:Literal ID="Literal4" Text="<%$Resources:Tokens,Confirm %>"
                                runat="server" />
                        </button>
                     
                        <button type="button" id="cancel" class="btn btn-danger" data-dismiss="modal" data-bb-handler="cancel">
                            <i class="icon-white icon-remove"></i>&nbsp;
                        <asp:Literal Text="<%$Resources:Tokens,Cancel %>" runat="server" /></button>
                    </div>
                   
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="woid" runat="server" />

    -              
    
    
    
    
    
    
    
      <div id="limitedunsus" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

          <div style="background-color: white; margin-top: 100px; margin-right: 20%; width: 40%">
              <div class="modal-content">
                  <div class="modal-header">
                      <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h4 id="H5">
                          <%=Tokens.MenulimitUnsuspend %></h4>
                  </div>

                  <div class="modal-body">
                      <div class="bootbox-body">
                          <div class="well">


                              <asp:DropDownList ID="unsusduration" runat="server"></asp:DropDownList>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="unsusduration"
                                  ValidationGroup="unsus" ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                          </div>
                          <div class="modal-footer">
                              <asp:Button runat="server" ID="Button4" ValidationGroup="unsus"
                                  OnClick="Unsuslimited" CssClass="btn btn-primary" Text="<%$Resources:Tokens,MenulimitUnsuspend %>" />
                              <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                          </div>
                      </div>
                  </div>
              </div>
          </div>
      </div>
    <div id="print-dialog" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 id="H5">
                        <%=Tokens.Print %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <table>
                                <tr>
                                    <td>
                                        <a target="_blank" class="btn btn-primary btn-sm" href='<%# string.Format("DemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString())) %>' data-rel="tooltip" title='<%=Tokens.HalfReciept %>'><i class="icon-print icon-only"></i><%=Tokens.HalfReciept %></a>
                                    </td>
                                    <td>
                                        <a target="_blank" class="btn btn-primary btn-sm" href='<%#string.Format("smallDemandReciept.aspx?d={0}",QueryStringSecurity.Encrypt(Eval("Id").ToString()))  %>' data-rel="tooltip" title="<%=Tokens.Reciept2 %>"><i class="icon-print icon-only"></i><%=Tokens.Reciept2 %></a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="True"><%=Tokens.Cancel %></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>















    <script type="text/javascript">

        function showhis() {
            $("#his-dialog").modal('show');
           <%-- var ph = document.getElementById('<%=woid.ClientID%>');
            $.ajax({
                type: "Post",
                contentType: "application/json; charset=utf-8",
                url: "CustomerDemands.aspx/fill_payhisGV",
                dataType: "json",
                data: "{'id':'" + ph.value + "'}",
                success: function () {
                 
                  

                            },
                     error: function (result) {

                         alert(result);
                     }
                 });--%>
        }

        function save() {
            var amount = document.getElementById('<%=ddlbox.ClientID%>').value;
            var res = document.getElementById('<%=txtDiscoundBox.ClientID%>').value;
            var box = document.getElementById('<%=TbAmountincomment.ClientID%>').value;
            var recep = document.getElementById('<%=ddlSavesPay.ClientID%>').value;
            var re = document.getElementById('<%=TbComment.ClientID%>').value;
            if (amount == "" || res == "" || box == "" || recep == "" || re == "") {
                return;
            } else {
                var check2 = document.getElementById('<%=PDialogPay.ClientID%>');
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }
        }
        function save2() {
            var amount = document.getElementById('<%=ddlSavesCancelPay.ClientID%>').value;

            if (amount == "") {
                return;
            } else {
                var check2 = document.getElementById('<%=btnRecieveFromCompany.ClientID%>');
                check2.disabled = 'true'; check2.value = 'Please wait...';
            }
        }

        function areyousure() {

            return confirm('<%= Tokens.Delete %>');
        }
        function check() {
            var customerId = $('#HFcheck').val();
            if (customerId === "0") {
                alert("<%=Tokens.SearchCustomerFirst %>");
                return;
            } else {

            }
        };

        $(document).ready(function () {
            // var customerId = $('#HfCustomerId').val();

            var customerId = $('#HFcheck').val();

            $('#addDemand').click(function () {
                if (parseInt(customerId) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    return true;
                } else {
                    $('#demand-dialog').modal('show');
                    return false;
                }
            });

            $('#BAddNextDemand').click(function () {
                if (parseInt(customerId) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                } else {
                    $('#BNextD').click();
                }
            });
            $('a[data-comment="comment"]').click(function () {
                if (parseInt(customerId) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    //$('#ddlbox').val('-1');
                    return true;
                } else {

                    //$(commentDialog).dialog('open');

                    var demandId = $(this).attr("data-demand");
                    $('#HfDemandId').val(demandId);
                    var realAmount = $(this).attr('data-fruit');
                    var v = parseFloat($(this).attr('data-fruit'));
                    var credit = parseFloat($("#hdnCredit").val());
                    var pluscredit = credit;
                    if (credit < 0) pluscredit = credit * (-1);
                    var minusV = v;
                    if (v < 0) minusV = v * (-1);
                    var l = 0;
                    if (minusV < pluscredit) {
                        l = pluscredit - minusV;
                    } else {
                        l = v;
                    }
                    if (v < 0) $("#txtDiscoundBox").val(realAmount);
                    else $("#txtDiscoundBox").val(l);
                    $("#TbAmountincomment").val(realAmount);
                    $("#comment-dialog").modal('show');
                    return false;
                }
            });
            $('#ddlbox').change(function () {
                var s = $('#ddlbox').val();
                $('#hf_boxId').val(s);
            });
            $('a[data-select="for-edit"]').click(function () {
                if (parseInt(customerId) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    return true;
                } else {

                    $('#edit-dialog').modal('show');
                    var demandId = $(this).attr("data-demand");
                    var from = $(this).attr("data-from");
                    var to = $(this).attr("data-to");
                    var commission = $(this).attr("data-comm");
                    
                    $('#HfEditDemandId').val(demandId);
                    $('#TbEFrom').val(from);
                    $('#TbETo').val(to);
                    if (commission === "False") {
                        $('#CBResellerCommession').prop('checked', false);
                    } else if (commission === "True") {
                        $('#CBResellerCommession').prop('checked', true);
                    }
                  
                  
                    $('#TbENotes').val($(this).attr("data-notes"));
                    $('#TbEAmount').val($(this).attr("data-amount"));
                    return false;
                }
            });

            //$('input[data-select="dt"]').datepicker({ dateFormat: 'dd/mm/yy' });
            $('input[data-select="dt"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });
            $('#cancel-dialog').click(function () {
                $(dlg).dialog('close');

            });

                                             <%--   $('#go-to-details').click(function () {
                if (customerId === "0") {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    return;
                } else {
                    window.location = $('#get-url').attr("data-url") + customerId;
                }
            });--%>
            $('#btnActive').click(function () {
                var suspenddays = $('#hdfDaysOfSuspend').val();
                $('#TbDaysCount').val(suspenddays);
                $('#unsuspendModal').modal('show');
            });
            $('#btnExtragiga').click(function () {
                $('#ExtraGigModal').modal('show');
            });

            $('#btnlimitActive').click(function () {

                $('#limitedunsus').modal('show');
            });

            $('#btnUpDown').click(function () {
                $('#UpDownModal').modal('show');
            });

            $('#btnChangeIPPackage').click(function () {
                $('#ChangeIPPackageModal').modal('show');
            });

            var msgSpan = $("#Msg");
            if (msgSpan.html() === "") {
                msgSpan.slideUp();
            } else {
                msgSpan.slideDown();
            }

        });
        window.addEventListener('load', doit);
        txt = document.getElementById('<%=TbAmountincomment.ClientID %>');
        lbl = document.getElementById('lblAmount');
        firstvalu = txt.value;
        function doit() {
            txt.onblur = checkamount;
        }

        function checkamount() {
            if (parseFloat(firstvalu) > parseFloat(txt.value)) {
                lbl.innerText = "الباقى : ";
                lbl.innerText += parseFloat(firstvalu) - parseFloat(txt.value);
            } else if (parseFloat(firstvalu) < parseFloat(txt.value)) {
                txt.value = "";
                lbl.innerText = "";
            } else {
                lbl.innerText = "";
            }
        }
    </script>
    <script type="text/javascript">

        $('#TbDaysCount').attr('readonly', 'readonly');


        $(document).ready(function () {

            $('input[data-date-picker="dp"]').datepicker({

                dateFormat: 'dd/mm/yy'
            });

            $("#TPhone").keypress(function (e) {
                var key = e.which;
                if (key === 13) {
                    $("#bSearch").click();
                    return false;
                } else {
                    return true;
                }
            });
            $('a[data-app="cancelPay"]').click(function () {
                var id = $(this).attr('data-work');
                $('#hdfId').val(id);
                $('#cancelPayModal').modal('show');
            });
            $("#btnAddPrepaid").click(function () {
                var customerId2 = $('#HFcheck').val();
                if (parseInt(customerId2) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    return true;
                } else {
                    $("#addCredit").modal("show");
                    return false;
                }
            });
            $("#btnPrepayReceipt").click(function () {

                var customerId3 = $('#HFcheck').val();
                if (parseInt(customerId3) === 0) {
                    alert("<%=Tokens.SearchCustomerFirst %>");
                    return true;
                } else {
                    $("#searchCredit").modal("show");
                    return false;
                }
            });<%--            $("#lblNotes").css("height", "auto");
            $('button[data-print]').click(function () {
                var ele = $('#receptmodal').prop('outerHTML');
                //var ele = $("#div_Receipt").prop("outerHTML");
                var mywindow = window.open('', 'new div');
                mywindow.document.write('<html><head><title> إيصال دفع مقدم</title>');
                /*optional stylesheet mywindow.document.write('<link rel="stylesheet" href="../Content/ace-assest/css/ace-rtl.min.css" />');*/ //
                mywindow.document.write('</head><body >');
                mywindow.document.write(ele);
                mywindow.document.write('</body></html>');
                mywindow.document.target = '_blank';
                $('table img').src($('#imgSite').src);
                mywindow.print();
                mywindow.close();
                return true;
            });
            $("#btnCancel").click(function () {
                $("#receptmodal").css("display", "none");
            });
            <% } %>--%>

        });
    </script>
</asp:Content>
