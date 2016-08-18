<%@ Page Title="<%$Resources:Tokens,PendingTransferBtSavesRequests%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PendingRequestsTransferBtSaves.aspx.cs" Inherits="NewIspNL.Pages.PendingRequestsTransferBtSaves" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h1>
            <asp:Label ID="lblchan" runat="server" Text="<%$Resources:Tokens,PendingTransferBtSavesRequests %>"></asp:Label>
        </h1>
    </div>
    <div class="row">

        <div class="col-xs-12">
            <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                CssClass="table table-bordered table-condensed text-center" Style="margin-right: -18px;"
                DataKeyNames="Id"
                OnRowDataBound="grd_Requests_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:Label ID="lbl_No" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FromSave" HeaderText="<%$ Resources:Tokens,FromSave %>" />
                    <asp:BoundField DataField="ToSave" HeaderText="<%$ Resources:Tokens,ToSave %>" />
                    <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:Tokens,Amount %>" />
                    <asp:BoundField DataField="RequestMaker" HeaderText="<%$ Resources:Tokens,requestSender%>" />
                    <asp:BoundField DataField="RequestDate" HeaderText="<%$ Resources:Tokens,RequestDate %>" />
                    <asp:BoundField DataField="RequestMakerNote" HeaderText="<%$ Resources:Tokens,note %>" />

                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Approve%>">
                        <ItemTemplate>

                            <button data-id='<%#Eval("Id") %>' runat="server" id="btnapproved" data-select="approve" class="btn btn-success btn-sm" data-rel="tooltip" title="<%$Resources:Tokens,Approve %>"
                                type="button" data-request-id='<%#Eval("Id") %>'>
                                <i class="icon-ok icon-only bigger-110"></i>
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Tokens,Reject%>">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnb_Reject" runat="server" data-select="Rejectlink" data-request-id='<%#Eval("Id") %>'
                                CssClass="btn btn-danger btn-sm" Style="text-decoration: none;" data-rel="tooltip" title="<%$ Resources:Tokens,Reject %>"><span class="icon-reply icon-only bigger-110"></span></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoRequests%>"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
        <br />

        <div style="text-align: center;">
            <asp:Label ID="lbl_ProcessResult" runat="server" EnableViewState="False"
                Font-Bold="True"></asp:Label>
        </div>
    </div>

    <div id="ApproveModal" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2"
        aria-hidden="true">
        <!--data-style="upDown"-->
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button><h4 id="myModalLabel2">
                            <%=Tokens.Approve %></h4>
                </div>
                <div class="modal-body">
                    <div class="bootbox-body">
                        <asp:HiddenField runat="server" ID="ApproveId" ClientIDMode="Static" />
                        <div class="well">
                            <label>
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,Note %>"></asp:Literal></label>
                            <div>
                                 <asp:TextBox runat="server" ID="tbApproveNote" ClientIDMode="Static"
                                    TextMode="MultiLine" />
                                <%--<asp:TextBox runat="server" ClientIDMode="Static" ID="TbofferDate" data-date-picker="dp"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ValidationGroup="app" ID="RequiredFieldValidator1"
                                    ControlToValidate="TbofferDate" ErrorMessage="*">
                                </asp:RequiredFieldValidator>--%>
                            </div>
                            
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="Button1" class="btn btn-primary" type="button" runat="server"
                        onserverclick="Approved_Click">
                        <%=Tokens.Approve %></button>
                    <button class="btn btn-default" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
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
                                <asp:TextBox runat="server" ID="TbRejectReason" ValidationGroup="rej" ClientIDMode="Static"
                                    TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ValidationGroup="rej" ErrorMessage="*" ControlToValidate="TbRejectReason"
                                    ID="RR" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div id="reject-results"></div>
                    <button id="Button2" class="btn btn-primary" validationgroup="rej" type="button" causesvalidation="True"
                        runat="server" onserverclick="Rejected_Click">
                        <%=Tokens.Reject %></button>
                    <button class="btn btn-default" type="button" data-dismiss="modal" aria-hidden="true">
                        <%=Tokens.Cancel %></button>
                </div>
            </div>
        </div>
    </div>
    <%--<script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('button[data-select="approve"]').click(function () {
                var id = $(this).attr('data-request-id');
                $('#ApproveId').val(id);
                $('#ApproveModal').modal('show');
            });
            $('a[data-select="Rejectlink"]').click(function () {
                var id = $(this).attr('data-request-id');
                $('#RejectedRequestId').val(id);
                $('#rejectModal').modal('show');
            });
            $('input[data-date-picker="dp"]').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });
        });
    </script>
</asp:Content>
