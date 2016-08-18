<%@ Page Title="<%$ Resources:Tokens,Reports %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="NewIspNL.Pages.Reports" %>




<%@ Import Namespace="Resources" %>
<%@ Import Namespace="NewIspNL.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <style type="text/css">
        .tablescroll {
            display: block;
    overflow-x: scroll;

}
    </style>

    <asp:HiddenField ID="Hfmessage" runat="server" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="HfFlag" />
            <link rel="stylesheet" href="../Content/ace-assest/css/chosen.css" type="text/css"/>

    <!--header title-->
    <div class="page-header">
        <h1>
            <asp:Label ID="lbl_Legend" runat="server"></asp:Label>
        </h1>
    </div>        
    <div class="row">
        <fieldset>
            <div class="row">
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <%= Tokens.Reseller %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlReseller" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlBranch">
                                <%= Tokens.Branch %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlBranch" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                        <div>
                            <label for="DdlGovernorate">
                                <%= Tokens.Governorate %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlGovernorate" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlCentral">
                                <%= Tokens.Central %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlCentral" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="well">
                        <div >
                            <label for="DdlSeviceProvider">
                                <%=Tokens.Service_Provider %></label>
                            <div>
                                <asp:DropDownList runat="server" CssClass="width-60 chosen" ID="DdlSeviceProvider" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                    <label for="txtCustomerPhone">
                                <%=Tokens.Phone %></label>
                            <div>
                        <asp:TextBox ID="txtCustomerPhone" ClientIDMode="Static" runat="server" Width="170px"></asp:TextBox>
                         </div>
                </div>
            </div>
            <p>
                <button validationgroup="foo" class="btn btn-success" runat="server" id="BSearch"
                    OnServerClick="FilterResults"  >
                    <i class="icon-white icon-search"></i>&nbsp;
                    <%= Tokens.Search %>
                </button>
            </p>
        </fieldset>

            
        <div id="all-result-items">

            
            
            
             <asp:GridView ID="grd_wo" runat="server" CssClass="table table-bordered table-condensed text-center tablescroll col-sm-12 col-xs-12"  
                ClientIDMode="Static" AutoGenerateColumns="False" DataKeyNames="ID" OnRowDataBound="grd_Requests_RowDataBound" 
               PageSize="500"  AllowPaging="True" OnPageIndexChanging="grdData_PageIndexChanging" ><%--PageSize="10"AllowPaging="True"--%>
                <PagerSettings Position="TopAndBottom"></PagerSettings>
                 <HeaderStyle  HorizontalAlign="Center" />
                <Columns>
                   <asp:TemplateField>
                        <ItemTemplate>
                            <div style="display: none;">
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Serial") %>'></asp:Label></div>
                         <asp:Label ID="l_number" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>"/>
                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>"/>
                    <asp:BoundField HeaderText="<%$Resources:Tokens,RequestNumber %>" DataField="RequestNumber" />
                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate%>"/>
                    <asp:BoundField DataField="CentralName" HeaderText="<%$ Resources:Tokens,Central%>"/>
                   
                       
                     <asp:BoundField DataField="ServicePackageName" HeaderText="<%$ Resources:Tokens,Package %>"/>
                    <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,State%>"/>
                    <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>"/>
                    <asp:Boundfield DataField="PaymentTypeName" HeaderText="<%$Resources:Tokens,PaymentType %>"/>
                    <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:Tokens,Branch %>"/>
                    <asp:BoundField DataField="ResellerName" HeaderText="<%$ Resources:Tokens,Reseller %>"/>
                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,UserName %>"/>
                    <asp:BoundField DataField="Notes" HeaderText="<%$ Resources:Tokens,Notes %>"/>
                    <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:Tokens,CreationDate %>"/>
                    <asp:BoundField DataField="OfferName" HeaderText="<%$ Resources:Tokens,Offer%>"/>
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$ Resources:Tokens,InvoiceDueDate%>"/>
                   
                        
                    <asp:TemplateField>
                   

                        <ItemTemplate>
                            <asp:HiddenField ID="hf_id" runat="server" Value='<%# Bind("ID") %>' />
                                    <div style="width: 85px">
                                       
                               <% if (EditCustomer)
                                  { %> 
                                        <asp:LinkButton ID="LinkBtnEdit" runat="server"  CssClass="btn btn-primary btn-xs" CommandArgument='<%# Bind("ID") %>' OnCommand="LinkBtnEdit_Command" OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 500);"  ><i class="icon-edit icon-only bigger-120"></i></asp:LinkButton>
                                       <%-- <a class="btn btn-primary btn-xs" id="HyperLink1"  onclick="edit_click"
                                    title="<%= Tokens.Edit %>" data-rel="tooltip" target="_blank"><i class="icon-edit icon-only bigger-120"></i></a>--%> <% } %>
                                &nbsp;
                                        <asp:LinkButton ID="LinkBtnDetails" runat="server" CssClass="btn btn-info btn-xs" CommandArgument='<%# Bind("ID") %>' OnCommand="LinkBtnDetails_Command" OnClientClick="var originalTarget = document.forms[0].target; document.forms[0].target = '_blank'; setTimeout(function () { document.forms[0].target = originalTarget; }, 500);" ><i class="icon-building icon-only bigger-120"></i></asp:LinkButton>
                                         <%--<a class="btn btn-info btn-xs" id="HyperLink2"  target="_blank"
                                    href='<%#string.Format("CustomerDetails.aspx?WOID={0}",QueryStringSecurity.Encrypt(Eval("ID").ToString())) %>' title="<%=Tokens.Details%>" data-rel="tooltip"><i class="icon-building icon-only bigger-120"></i></a>--%>
                                    </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <a class="btn btn-danger btn-xs" id="lnb_Reject" runat="server" data-select="reject"
                                data-woid='<%# Eval("ID") %>' title="<%$ Resources:Tokens,Reject %>" data-rel="tooltip" ><i class="icon-reply icon-only bigger-120"></i></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Select %>">
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </EditItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox ID="SelectHeader" runat="server" AutoPostBack="True" OnCheckedChanged="SelectHeader_CheckedChanged" ClientIDMode="Static"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="SelectItem" runat="server" data-select="check" />
                            <button runat="server" data-id='<%#Eval("ID") %>' data-select="approve" class="btn btn-success btn-xs"
                                type="button" title="<%$Resources:Tokens,Approve %>" data-rel="tooltip">
                                <i class="icon-ok icon-white bigger-120"></i>
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                                                <HeaderTemplate>
                            <input id="selectall"  type="checkbox"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="selecttoapprove" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
             
                <EmptyDataTemplate>
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,NoResults %>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
            

            <asp:HiddenField runat="server" ID="hf_woid" ClientIDMode="Static"/>
            <div id="div_Approve" runat="server" style="text-align: center" visible="false">
               
                &nbsp;
                <asp:Button ID="btn_ApproveSelected" ClientIDMode="Static" runat="server" Text="<%$ Resources:Tokens,ApproveSelected%>"
                    OnClick="btn_ApproveSelected_Click" data-select="btn-approve" CssClass="btn btn-success"/>
                                    &nbsp;
                <input id="btn_approveselectedActive" clientidmode="Static" type="button" value="<%=Tokens.ApproveSelected %>"
                       class="btn btn-success" />
                &nbsp;
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="<%$ Resources:Tokens,Export %>"
                    Width="150px" CssClass="btn btn-primary"/>
            </div>
            <div id="div_Reject" class="bootbox modal fade" tabindex="-1" role="dialog"aria-labelledby="myModalLabel"
        aria-hidden="true">
                        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H1">
                        <%= Tokens.RejectReason %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                <div style="margin: 10px auto;">
                    <table>
                        <tr>
                            <td style="width: 125px">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,RejectReason %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_RejectReason" runat="server" Rows="3" TextMode="MultiLine" Width="290px"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_RejectReason"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="v"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                </div>
                </div>
                <div class="modal-footer">
                                                    <asp:Button ID="btn_reject" runat="server" Text="<%$ Resources:Tokens,Reject %>"
                                     OnClick="btn_reject_Click" ValidationGroup="v" CssClass="btn btn-info"/>
                                &nbsp;<button ID="btn_Cancel" runat="server" data-dismiss="modal" aria-hidden="true"
                                     class="btn btn-default"><i class="icon-white icon-arrow-left"></i>
                                    <%= Tokens.Cancel %></button>
                </div>
                </div>
                </div>
            </div>
            <center>
                <asp:Label ID="lbl_ProcessResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
            </center>
            <asp:Label ID="lbl_QSStatus" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
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
                        <%= Tokens.CustomerInvoices %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="HfSelected" ClientIDMode="Static" />
                        <div>
                            <div runat="server" id="DateArea">
                                <div class="well">
                                    <asp:RadioButtonList ID="RblByActivation" runat="server" data-select="radios" ClientIDMode="Static"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Text="<%$Resources:Tokens,ByActivationDate %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Tokens,FilterFirstMonthAndTheRestOfMonthsAreComplete %>"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Tokens,FilterToAHalfMonth%>"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12">
                                        <div class="well">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div>
                                                            <asp:Label ID="Label4" Text="<%$Resources:Tokens,Activation.Date %>" runat="server"
                                                                AssociatedControlID="TbCreationDate" /><div>
                                                                    <asp:TextBox runat="server" ID="TbCreationDate" data-x="dt" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="<%$Resources:Tokens,Required  %>"
                                                                        ControlToValidate="TbCreationDate" runat="server" data-required="post" />
                                                                </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <label for="TbSerial">
                                                                <%= Tokens.SerialNumber %></label>
                                                            <div>
                                                                <asp:TextBox runat="server" ID="TbSerial" ClientIDMode="Static"/>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div>
                                                            <label for="TbUserName">
                                                                <%= Tokens.UserName %></label>
                                                            <div>
                                                                <asp:TextBox runat="server" ID="TbUserName" ClientIDMode="Static"/>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div>
                                                            <label for="TbPassword">
                                                                <%= Tokens.Password %></label>
                                                            <div>
                                                                <asp:TextBox runat="server" ID="TbPassword" ClientIDMode="Static"/>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="row-fluid">
                                    <div class="span12" id="offer-flag">
                                        <div class="well">
                                            <div>
                                                <asp:RadioButtonList ID="RblInOffer" RepeatDirection="Horizontal" ClientIDMode="Static"
                                                    runat="server" data-select="radios">
                                                    <asp:ListItem Selected="True" Text="<%$Resources:Tokens,InOffer %>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$Resources:Tokens,OutOfOffer %>"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">
                        <i class="icon-white icon-arrow-left"></i>
                        <%= Tokens.Cancel %></button>
                    <button class="btn btn-primary" type="button" causesvalidation="True" data-check="post"
                        runat="server" id="ModalBtnApprove" OnServerClick="ModalBtnApproveSave" clientidmode="Static">
                        <i class="icon-white icon-ok"></i>
                        <%= Tokens.Approve %></button>
                                                <button class="btn btn-primary" type="button" causesvalidation="True" data-check="post"
                        runat="server" id="btnApproveToactive" OnServerClick="btn_ApproveSelectToActive_Click" clientidmode="Static">
                        <i class="icon-only icon-ok"></i>
                        <%= Tokens.Save %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="statemodal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="stateSelector"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="stateSelector">
                        <%= Tokens.NewStatus %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="RejectedRequestId" ClientIDMode="Static" />
                        <div class="well">
                            <label for="DdlNewState">
                                <%= Tokens.NewStatus %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" style="width: 300px !important" ClientIDMode="Static" ID="DdlNewState">
                                </asp:DropDownList>
                            </div>
                            <div id="problem" style="display: none;">
                                <label for="TbProblem">
                                    <%= Tokens.SystemProblem %>
                                </label>
                                <asp:TextBox runat="server" ID="TbProblem" ClientIDMode="Static" TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="Button2" class="btn btn-primary" type="button" causesvalidation="True"
                        onserverclick="SaveMulti" runat="server">
                        <%= Tokens.Save %></button>
                    <button class="btn btn-default" type="button" data-dismiss="modal" aria-hidden="true">
                        <%= Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <div id="modalToSplititing" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="stateSelector"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button>
                    <h4 id="H2">
                        <%= Tokens.MenuPendingSplitting %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="HiddenField1" ClientIDMode="Static" />
                        <div class="well">
                            <label for="txtPortNumber">
                                <%= Tokens.PortNumber %>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtPortNumber"></asp:TextBox>
                            </div>
                            
                                <label for="txtBlockNumber">
                                    <%= Tokens.Block %>
                                </label><div>
                                <asp:TextBox runat="server" ID="txtBlockNumber" /></div>
                                                                <label for="txtDslamNumber">
                                    <%= Tokens.Dslam %>
                                </label><div>
                                <asp:TextBox runat="server" ID="txtDslamNumber" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="Button3" class="btn btn-primary" type="button" CausesValidation="True"
                        OnServerClick="ToSpiliting" runat="server">
                        <%= Tokens.Save %></button>
                    <button class="btn btn-default" type="button" data-dismiss="modal" aria-hidden="true">
                        <%= Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <span runat="server" id="direction" style="display: none" clientidmode="Static">
    </span>
    <asp:HiddenField runat="server" ID="hdnUrl" ClientIDMode="Static"/>

    <script type="text/javascript">
        $(document).ready(function () {

            

            //$(".chosen-select").chosen();
            var flag = $('#HfFlag').val();
            if (flag === "5") {
                $('button[data-select="approve"]').show();
                $('input[data-select="btn-approve"]').hide();
                $('span[data-select="check"]').hide();
                $('#SelectHeader').hide();
                $('#btn_approveselectedActive').show();
            } else {
                $('button[data-select="approve"]').hide();
                $('input[data-select="btn-approve"]').show();
                $('span[data-select="check"]').show();
                $('#SelectHeader').show();
                $('#btn_approveselectedActive').hide();
            }
            var direction = $('#direction').html();
            $('table[data-select="radios"] input').css({ 'float': direction === "1" ? "right" : "left", 'margin': '3px' });
            $('#ctl00_ContentPlaceHolder1_RblInOffer_1')
                .parent('td')
                .css("width", "150px");
            $('button[data-select="approve"]').click(function () {
                $('#btnApproveToactive').hide();
                $('#ModalBtnApprove').show();
                $('#TbSerial').prop('disabled', false); $('#TbUserName').prop('disabled', false); $('#TbPassword').prop('disabled', false);
                var id = $(this).attr("data-id");
                $('#HfSelected').val(id);
                $('#myModal').modal('show');
            });
            $('input[id="btn_approveselectedActive"]').click(function () {
                //buttons in modal
                $('#btnApproveToactive').show(); $('#ModalBtnApprove').hide();
                //textboxes in modal
                $('#TbSerial').prop('disabled', true); $('#TbUserName').prop('disabled', true); $('#TbPassword').prop('disabled', true);
                $('#myModal').modal('show');
            });
            
            $('input[data-x="dt"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });
            $('#offer-flag').hide();
            $($('#RblByActivation input[type="radio"]')[2]).click(function () {
                $('#offer-flag').fadeIn();
            });
            $($('#RblByActivation input[type="radio"]')[1]).click(function () {
                $('#offer-flag').fadeIn();
            });
            $($('#RblByActivation input[type="radio"]')[0]).click(function () {
                $('#offer-flag').fadeOut();
            });

            $('#RblInOffer input[type="radio"]:last').parent('td').css('width', '400px');
            $('#RblInOffer input[type="radio"]:first').parent('td').css('width', '100px');

            $('#btn_ApproveSelected').click(function () {
                var requestType = $("#hdnUrl").val();
                if (requestType === "1" || requestType === "2") {
                    $('#statemodal').modal('show');
                    return false;
                } else if (requestType === "3") {
                    $('#modalToSplititing').modal('show');
                    return false;
                }
                else {
                    return true;
                }
            });
            $('#DdlNewState').change(function () {
                var selectedValue = $('#DdlNewState').val();
                if (selectedValue === "7") {
                    $('#problem').show();
                } else {
                    $('#problem').hide();
                }
            });
            
            if ($('#grd_wo').width() > 950) {
                $('#all-result-items').css({
                    "overflow-y": "auto"
                });
                $('#grd_wo').css({
                    "font-family": "serif",
                    "font-size": "12px"
                });
            }
            
            $('a[data-select="reject"]').click(function () {
                var id = $(this).attr("data-woid");
                $('#hf_woid').val(id);
                $('#div_Reject').modal('show');
            });

            $('#selectall').click(function (event) {  //on click 
                if (this.checked) { // check select status
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes with class "checkbox1"               
                    });
                } else {
                    $('input[type="checkbox"]').each(function () { //loop through each checkbox
                        this.checked = false; //deselect all checkboxes with class "checkbox1"                       
                    });
                }
            });
            $("#grd_wo:last td table").addClass("pagination");
            $("#grd_wo:last td table tbody tr td").addClass("btn btn-xs");
            $("#grd_wo:last td table tbody tr td a").addClass("btn btn-xs btn-light");
            $("#grd_wo:last td table tbody tr td a").parent().addClass("btn btn-xs btn-light");
            $("#grd_wo:last td table tbody tr td span").addClass("btn btn-xs btn-info");
            $("#grd_wo:last td table tbody tr td span").parent().addClass("btn btn-xs btn-info");
        });

        jQuery(document).ready(function () {
            jQuery(".chosen").chosen();
        });
    </script>
</asp:Content>

