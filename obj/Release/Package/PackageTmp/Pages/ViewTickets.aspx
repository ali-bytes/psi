<%@ Page Title="<%$ Resources:Tokens,Tickets %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ViewTickets.aspx.cs" Inherits="NewIspNL.Pages.ViewTickets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/1.10.9/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="http://cdn.datatables.net/tabletools/2.2.4/css/dataTables.tableTools.css" />
    <table width="100%" id="tb_all">
        <tr>
            <td>
                <div class="view">
                    <fieldset>
                        <div class="page-header">
                            <h1>
                                <asp:Label ID="lblTitle" runat="server"></asp:Label></h1>
                        </div>
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="well">
                                    <div>
                                        <label for="DdlTicketReason">
                                            <%= Tokens.TicketReasons%></label>
                                        <div>
                                            <asp:DropDownList runat="server" ID="ddl_TicketReasons" ClientIDMode="Static" DataTextField="Title" 
                                        DataValueField="ID" >
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tabl">
                            <asp:GridView ID="grd_Tickets" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                                DataKeyNames="ID,LastCommentID" CssClass="table table-bordered table-condensed text-center"
                                OnRowCancelingEdit="grd_Tickets_RowCancelingEdit" OnRowEditing="grd_Tickets_RowEditing"
                                OnRowUpdating="grd_Tickets_RowUpdating" OnRowDataBound="grd_Tickets_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <EditItemTemplate>
                                            <asp:Label ID="lbl_No" runat="server"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_No" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,Serial %>" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>"
                                        ReadOnly="True"></asp:BoundField>
                                    
                                    <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="Title" HeaderText="<%$ Resources:Tokens,Ticket.Reason %>"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="Details" HeaderText="<%$ Resources:Tokens,Details %>"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,SenderName %>"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:TemplateField FooterText="Comment" HeaderText="<%$ Resources:Tokens,Comment %>">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Comment") %>'></asp:TextBox>
                                        </EditItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,TicketNo %>">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ProviderTicketNo") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("ProviderTicketNo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TicketDate" HeaderText="<%$ Resources:Tokens,OpenDate %>"></asp:BoundField>
                                    <asp:BoundField DataField="CommentDate" HeaderText="<%$ Resources:Tokens,SolvedDate %>"></asp:BoundField>
                                    <asp:BoundField DataField="Employee" HeaderText="<%$ Resources:Tokens,Employee %>"></asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Add %>" ShowHeader="False">
                                        <ItemTemplate>
                                            <span runat="server" data-rel="tooltip" title="<%$Resources:Tokens,Add %>" class="btn btn-success btn-sm" data-select="edit" data-value='<%#Eval("ID") %>'>
                                                <i class="icon-pencil"></i></span>
                                            <%--      <asp:LinkButton ID="lnb_AddTicket" runat="server" CausesValidation="False" OnClick="lnb_AddTicket_Click"
                                                        Text="<%$ Resources:Tokens,Add %>"></asp:LinkButton>
                                            --%>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Reject %>" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnb_Reject" CssClass="btn btn-danger btn-sm" runat="server" CausesValidation="False" OnClick="lnb_Reject_Click"
                                                ToolTip="<%$ Resources:Tokens,Reject %>" data-rel="tooltip"><i class="icon-remove icon-only"></i></asp:LinkButton>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:CommandField HeaderText="<%$Resources:Tokens,Update %>" ShowEditButton="True"
                                        EditText="<%$Resources:Tokens,Edit %>" CancelText="<%$Resources:Tokens,Cancel %>"
                                        DeleteText="<%$Resources:Tokens,Delete %>" UpdateText="<%$Resources:Tokens,Update %>">
                                        <ControlStyle ForeColor="Blue" />


                                    </asp:CommandField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Solved %>" ShowHeader="False">
                                        <ItemTemplate>
                                            <button type="button" id="btnSolve" data-rel="tooltip" title='<%=Tokens.Add %>' class="btn btn-success btn-sm" data-select="solve" data-value='<%#Eval("ID") %>'>
                                                <i class="icon-plus-sign icon-only"></i>
                                            </button>
                                            <%--  <asp:LinkButton ID="lnb_Solved" runat="server" CausesValidation="False" OnClick="lnb_Solved_Click"
                                            Text="<%$ Resources:Tokens,Solved %>"></asp:LinkButton>--%>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoTickects %>"></asp:Label>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
    <div style="text-align: center">
        <asp:Label ID="lbl_Link" EnableViewState="False" runat="server" Font-Bold="True"></asp:Label>
    </div>
    <%--    <asp:ModalPopupExtender ID="mpe_AddTicket" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="div_AddTicket" TargetControlID="Label1" Drag="True" DynamicServicePath=""
        Enabled="True">
    </asp:ModalPopupExtender>
    <div id="div_AddTicket" runat="server" class="modalPopup">
    </div>--%>
    <asp:ModalPopupExtender ID="mpe_Reject" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="div_Reject" TargetControlID="Label1" Drag="True" DynamicServicePath=""
        Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Panel ID="div_Reject" runat="server" class="modalPopup">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="submit" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true" runat="server" causesvalidation="False">
                        &times;</button>
                    <h4 id="H1">
                        <%= Tokens.Reject %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <table>
                            <tr>
                                <td style="width: 93px">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,Comment %>"></asp:Label>
                                </td>
                                <td style="width: 408px">
                                    <asp:TextBox ID="txt_Comment2" runat="server" Width="400px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_Comment2" ValidationGroup="vg_Reject"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btn_Reject" runat="server" Text="<%$ Resources:Tokens,Reject %>"
                        ValidationGroup="vg_Reject" Width="100px" OnClick="btn_Reject_Click" CssClass="btn btn-primary" />
                    &nbsp;
                    <asp:Button ID="btn_Cancel2" runat="server" CausesValidation="False" Text="<%$ Resources:Tokens,Cancel %>"
                        Width="100px" CssClass="btn btn-default" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <%--    <asp:ModalPopupExtender ID="mpe_Solved" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="div_Solve" TargetControlID="Label1" Drag="True" DynamicServicePath=""
        Enabled="True">
    </asp:ModalPopupExtender>
    <div id="div_Solve" runat="server" class="modalPopup">
    </div>--%>
    <div id="addTicket" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel5"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 id="myModalLabel5">
                        <%= Tokens.Ticket %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,TicketNo %>" Width="77px"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_ProviderTicketNo" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_ProviderTicketNo" ValidationGroup="t"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,Comment %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_Comment1" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                        ControlToValidate="txt_Comment1" ValidationGroup="t"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="cancelTicket" runat="server" causesvalidation="True" onserverclick="AddTickect"
                        validationgroup="t" class="btn btn-primary">
                        <%= Tokens.Save %></button>
                    <span id="cancel" class="btn">
                        <%= Tokens.Cancel %></span>
                </div>
            </div>
        </div>
    </div>
    <div id="solveTicket" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel5h4"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 id="myModalLabel5h4">
                        <%= Tokens.Solve %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="well">
                            <div>
                                <div>
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Comment %>"></asp:Label>
                                    <div>
                                        <asp:TextBox ID="txt_Comment3" runat="server" ValidationGroup="s"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="txt_Comment3" ValidationGroup="s"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div>
                                <label>
                                    <input type="radio" name="withDiscount" runat="server" id="RbWithout" checked="True"
                                        clientidmode="Static" />&nbsp;<%= Tokens.DontApplyDiscountForDemand %></label>
                                <label>
                                    <input type="radio" name="withDiscount" runat="server" id="RbWith" clientidmode="Static" />&nbsp;<%= Tokens.ApplyDiscountForDemand %></label>
                                <label>
                                    <input type="radio" name="withDiscount" runat="server" id="RbDeport"
                                        clientidmode="Static" />&nbsp;<%= Tokens.DeportDaysCompensation %></label>
                            </div>
                            <div id="dicount-div" style="display: none;">
                                <div>
                                    <label for="TbDiscount">
                                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Discount %>" runat="server" />
                                    </label>
                                    <div>
                                        <asp:TextBox ValidationGroup="s" runat="server" ID="TbDiscount" Text="0" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ValidationGroup="s" ID="RTbDiss" ErrorMessage="<%$Resources:Tokens,Required %>"
                                            ControlToValidate="TbDiscount" runat="server" />
                                        <asp:CompareValidator ValidationGroup="s" runat="server" ID="CV" ControlToValidate="TbDiscount"
                                            Operator="DataTypeCheck" Type="Double" ErrorMessage="<%$Resources:Tokens,NumbersOnly %>"></asp:CompareValidator>
                                    </div>
                                </div>
                            </div>

                            <div id="deportDaysTxt" style="display: none;">
                                <div>
                                    <label for="txtdeportDays">
                                        <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,DaysCount %>" runat="server" />
                                    </label>
                                    <div>
                                        <asp:TextBox ValidationGroup="dd" runat="server" ID="txtdeportDays" Text="0" TextMode="Number" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ValidationGroup="dd" ID="RequiredFieldValidator5" ErrorMessage="<%$Resources:Tokens,Required %>"
                                            ControlToValidate="txtdeportDays" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="solveTicketbtn" runat="server" causesvalidation="True" onserverclick="btn_Solved_Click"
                        validationgroup="s" class="btn btn-primary">
                        <%= Tokens.Save %></button>
                    <span id="cancelSovle" class="btn">
                        <%= Tokens.Cancel %></span>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" runat="server" id="HfTicketId" clientidmode="Static" />
    <input type="hidden" runat="server" id="HfTicketIdToSolve" clientidmode="Static" />
    <script type="text/javascript">
        $(function () {
            'user strict';
            $('.table a').addClass('btn btn-primary btn-sm').css('color', 'white');
            //$('.table a').attr("title", "<%=Tokens.Edit %>");
            if ($('#tabl').width() > 1200) {
                $('#tabl').css({
                    "overflow-y": "auto",
                    "width": "1048px"
                });
            }
            /*$('#grd_Tickets').css({
                    "font-family": "serif",
                    "font-size": "12px"
                });*/
            $('span[data-select="edit"]').click(function () {
                var id = $(this).attr('data-value');
                $('#HfTicketId').val(id);
                $('#addTicket').modal('show');
            });

            $('button[data-select="solve"]').click(function () {
                var id = $(this).attr('data-value');
                $('#HfTicketIdToSolve').val(id);
                $('#solveTicket').modal('show');
            });

            $('#cancel').click(function () {
                $('#addTicket').modal('hide');
            });


            $('#cancelSovle').click(function () {
                $('#solveTicket').modal('hide');
            });

            $('#RbWithout').click(function () {
                Show(false);
                ShowDeport(false);
            });

            $('#RbWith').click(function () {
                Show(true);
                ShowDeport(false);
            });
            $('#RbDeport').click(function () {
                ShowDeport(true);
                Show(false);
            });
            function Show(show) {
                if (show) {
                    $('#dicount-div').show();
                } else {
                    $('#dicount-div').hide();
                }
            }
            function ShowDeport(show) {
                if (show) {
                    $('#deportDaysTxt').show();
                } else {
                    $('#deportDaysTxt').hide();
                }
            }

        });
    </script>
    <script type="text/javascript" src="http://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="http://cdn.datatables.net/tabletools/2.2.4/js/dataTables.tableTools.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('table').each(function () {
                $(this).prepend('<thead></thead>');
                $(this).find('thead').append($(this).find("tr:eq(0)"));
            });

            var table = $('#grd_Tickets').dataTable({
                "iDisplayLength": 50
            });
            var tableTools = new $.fn.dataTable.TableTools(table, {
                'aButtons': [
                    {
                        'sExtends': 'xls',
                        'sButtonText': 'Save to Excel',
                        'sFileName': 'ViewTickets.xls'
                    },
                    {
                        'sExtends': 'print',
                        'bShowAll': true
                    },
                    'copy'
                ],
                'sSwfPath': '//cdn.datatables.net/tabletools/2.2.4/swf/copy_csv_xls_pdf.swf'
            });
            $(tableTools.fnContainer()).insertBefore('#grd_Tickets_wrapper');

            $('#ddl_TicketReasons').change(function () {

                
                table.fnFilter($('#ddl_TicketReasons :selected').text());

            });
        });


    </script>
</asp:Content>
