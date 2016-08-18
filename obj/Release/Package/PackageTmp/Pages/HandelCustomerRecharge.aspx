<%@ Page Title="<%$Resources:Tokens,HandelCustomerRecharge %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HandelCustomerRecharge.aspx.cs" Inherits="NewIspNL.Pages.HandelCustomerRecharge" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div>
        <asp:MultiView runat="server" ID="mv_container">
            <asp:View runat="server" ID="v_search">
                <div class="view">
                    <fieldset>
                        <div class="page-header"><h1>
                            <%=Tokens.HandelCustomerRecharge %></h1>
                        </div>
                        <asp:Panel runat="server" ID="p_resquest">
                            <div class="well">
                            <div style="padding-bottom: 5px;">
                                <asp:Label runat="server" Text="<%$Resources:Tokens,Reseller %>" ID="labelReseller"></asp:Label></div>
                            <div>
                                <asp:DropDownList runat="server" ID="ddl_reseller" Width="150px" ClientIDMode="Static">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_reseller"
                                                            ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                            <p>
                                <br/>
                                <asp:LinkButton runat="server" ID="b_addRequest"
                                  OnClientClick="hidval()"          OnClick="b_addRequest_Click" CssClass="btn btn-success"><i class="icon-search"></i>&nbsp;<asp:Literal runat="server" ID="lblsearchtxt" Text="<%$Resources:Tokens,Search %>"></asp:Literal></asp:LinkButton>
                                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hf_user" runat="server" />
                            </p>
                            </div>
                        </asp:Panel>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View runat="server" ID="v_results">
                                <div class="view">
                <fieldset>
                <asp:Panel runat="server" ID="p_rDetails">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Requests %>"></asp:Literal></h1></div>
                    <div style="padding: 5px;" id="container">
                        <p>
                            <asp:Button runat="server" ID="b_changeReseller" CssClass="btn btn-primary" Text="<%$Resources:Tokens,ChangeReseller %>" OnClick="b_changeReseller_Click" /></p>
                        <asp:GridView runat="server" ID="gv_customers" AutoGenerateColumns="False"
                                      ForeColor="Black" GridLines="Horizontal" CssClass="table table-bordered table-condensed"
                                      OnDataBound="gv_customers_DataBound" Width="100%"
                                      ClientIDMode="Static" >
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>" />
                                <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,InvoiceNumber %>"/>
                                <asp:BoundField DataField="ClientName" HeaderText="<%$Resources:Tokens,Customer.Name %>" />
                                <asp:BoundField DataField="ClientTelephone" HeaderText="<%$Resources:Tokens,Customer.Phone %>" />
                                <asp:BoundField DataField="CompanyName" HeaderText="<%$Resources:Tokens,Company %>"/>
                                <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,RequestDate%>" />
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="gv_bConfirm" runat="server" CssClass="btn btn-success btn-sm" Text="<%$Resources:Tokens,Confirm %>"
                                                    CommandArgument='<%# Bind("ID") %>' data-fruit='<%#Eval("Amount")%>' ToolTip='<%# Bind("ID") %>' data-val="Approv"  /><%--OnClick="gv_bConfirm_Click--%>
                                                
                                        <asp:HiddenField ID="gv_hf_id" runat="server" Value='<%# Bind("ID") %>' />
                                        <asp:Button ID="Button1" CssClass="btn btn-danger btn-sm" runat="server" data-val="Reject" Text="<%$Resources:Tokens,Reject %>"
                                                    CommandArgument='<%# Bind("ID") %>' ToolTip='<%# Bind("ID") %>' />
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <EmptyDataTemplate>
                                <%=Tokens.NoResults %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div>
                        </div>
                        <div style="text-align: center">
                            <asp:Label runat="server" ID="l_message"></asp:Label></div>
                    </div>
                </asp:Panel>
                                </fieldset>
                </div>
            </asp:View>
        </asp:MultiView>
                <div id="rej" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
           <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button><h4 id="myModalLabel" class="modal-title">
                            <%=Tokens.RejectReason %></h4>
                </div>
                        <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="bootbox-form">
                    <table width="500">
                        <tr>
                            <td colspan="2">
                                <asp:Literal runat="server" Text="<%$Resources:Tokens,RejectReason %>"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 125px">
                                <asp:TextBox ID="txt_RejectReason" runat="server" Rows="3" TextMode="MultiLine" Width="356px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_RejectReason"
                                                            Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="x"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    </div></div></div>
                    <div class="modal-footer">
                                                        <asp:HiddenField ID="hf_rejectionId" runat="server" ClientIDMode="Static" />
                                <asp:Button aria-disabled="false" role="button" ID="btn_reject" runat="server" OnClick="btn_reject_Click"
                                            Text="<%$Resources:Tokens,Save%>"  ClientIDMode="Static" CssClass="btn btn-primary"
                                            ValidationGroup="x" />
&nbsp;
                    <button data-bb-handler="cancel" data-dismiss="modal" type="button" class="btn btn-default">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
            </div>
                    </div></div>
                </div>
                <div id="aprov" class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="H1"
        aria-hidden="true">
           <div class="modal-dialog">
            <div class="modal-content">
            <div class="modal-header">
                    <button type="button" class="bootbox-close-button close" data-dismiss="modal">
                        ×</button><h4 id="H1" class="modal-title">
                            <%=Tokens.Approve %></h4>
                </div>
                        <div class="modal-body">
                    <div class="bootbox-body">
                        <div class="bootbox-form">
            <table width="500">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <label><%=Tokens.Box %></label><div>
                        <asp:DropDownList runat="server" ID="ddlBox" ClientIDMode="Static"/>
                        </div>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlBox"
                                                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><label><%=Tokens.DiscoundFromBox %></label><div>
                        <asp:TextBox runat="server" ID="txtDiscoundBox" ClientIDMode="Static"></asp:TextBox></div></td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDiscoundBox"
                                                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                    </td>
                </tr>
               <%-- <tr>
                    <td><label><%=Tokens.DiscoundFromReseller %></label>
                    <asp:TextBox runat="server" ID="txtDiscoundReseller" ClientIDMode="Static"></asp:TextBox></td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDiscoundBox"
                                                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                    </td>
                </tr>--%>
                <tr>
                    
                    <td style="width: 125px">
                        <label><%=Tokens.Comment %></label><div>
                        <asp:TextBox ID="txtApprovComment" runat="server" Rows="3" TextMode="MultiLine" Width="356px"></asp:TextBox></div>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="hf_Approved" runat="server" ControlToValidate="txtApprovComment"
                                                    Display="Dynamic" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="y"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            </div></div></div>
            <div class="modal-footer">
                                        <asp:HiddenField ID="hf_ApprovedId" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hf_boxId" ClientIDMode="Static"/>
                        <asp:Button aria-disabled="false" role="button"  ID="Button2" runat="server" OnClick="gv_bConfirm_Click"
                            Text="<%$Resources:Tokens,Save%>" ClientIDMode="Static" CssClass="btn btn-primary"
                            ValidationGroup="y"  UseSubmitBehavior="false" OnClientClick="plswait(this.id) " />
&nbsp;
                    <button data-bb-handler="cancel" data-dismiss="modal" type="button" class="btn btn-default">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
            </div>
            </div></div>
        </div>
    </div>

    <script type="text/javascript">
        
        function plswait(id) {

            var type = document.getElementById('<%=ddlBox.ClientID%>').value;
              var amo = document.getElementById('<%=txtDiscoundBox.ClientID%>').value;
              var type2 = document.getElementById('<%=txtApprovComment.ClientID%>').value;

             

              if (type == "" || amo == "" || type2 == "" ) { return; }
              else {
                  var check2 = document.getElementById(id);
                  check2.disabled = 'true'; check2.value = 'Please wait...';
              }

          }






        $(document).ready(function () {
            /*var dlg = $('#rej').dialog({
                autoOpen: false,
                width: 600,
                height: 230,
                resizable: false,
                draggable: false,
                title: "<%=Tokens.RejectReason %>",
            modal: true,
            position: 'center'
        });
        dlg.parent().appendTo(jQuery("form:first"));*/
            $('input[data-val="Reject"]').click(function () {
                var x = $(this).attr("Title");
                $('#hf_rejectionId').val(x);
                $('#rej').modal('show');//.dialog('open');
                return false;
            });
            /*var pop = $('#aprov').dialog({
            autoOpen: false,
            width: 600,
            height: "auto",
            resizable: false,
            draggable: false,
            title: "<%=Tokens.Approve %>",
        modal: true,
            position: 'center'
        });
        pop.parent().appendTo(jQuery("form:first"));*/
            $('input[data-val="Approv"]').click(function () {
                var a = $(this).attr('Title');
                $('#hf_ApprovedId').val(a);
                var v = $(this).attr('data-fruit');
                $('#txtDiscoundBox').val(v);
                /*$('#txtDiscoundReseller').val(v);*/
                $('#aprov').modal('show');//dialog('open');
                return false;
            });
            $('#ddl_reseller').change(function () {
                //                var selection = $('#ddl_reseller').val();
                //                $('#HiddenField1').val(selection);
                //                $('#ddl_reseller').attr('disabled', 'disabled');
            });
            $('#ddlBox').change(function () {
                var s = $('#ddlBox').val();
                $('#hf_boxId').val(s);
            });
        });
        function hidval() {
            var selection = $('#ddl_reseller').val();
            $('#HiddenField1').val(selection);
            $('#ddl_reseller').attr('disabled', 'disabled');
        }
    </script>
</asp:Content>

