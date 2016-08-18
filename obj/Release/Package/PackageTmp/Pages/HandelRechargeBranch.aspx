<%@ Page Title="<%$Resources:Tokens,HandelRechargeBranch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="HandelRechargeBranch.aspx.cs" Inherits="NewIspNL.Pages.HandelRechargeBranch" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
        <asp:MultiView runat="server" ID="mv_container">
            <asp:View runat="server" ID="v_search">
                <asp:Panel runat="server" ID="p_resquest">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,HandelRechargeBranch %>"></asp:Literal></h1></div>
                    <div class="well">
                    <div style="padding-bottom: 5px;">
                        <asp:Label runat="server" Text="<%$Resources:Tokens,Branch %>" ID="labelBranch"></asp:Label></div>
                    <div>
                        <asp:DropDownList runat="server" ID="ddl_Branch" Width="150px" ClientIDMode="Static">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_Branch"
                                                    ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                    </div>
                    <p>
                        <br/>
                        <asp:LinkButton runat="server" ID="b_addRequest" CssClass="btn btn-success"
                        OnClientClick="hidval()"            OnClick="b_addRequest_Click"><i class="icon-search"></i><asp:Literal runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal></asp:LinkButton>
                        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hf_user" runat="server" />
                    </p>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View runat="server" ID="v_results">
                <asp:Panel runat="server" ID="p_rDetails">
                    <h3 class="header smaller lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,Requests %>"></asp:Literal></h3>
                    <div style="padding: 5px;" id="container">
                        <p>
                            <asp:Button runat="server" ID="b_changeReseller" Text="<%$Resources:Tokens,ChangeBranch %>" OnClick="b_changeBranch_Click" CssClass="btn btn-primary" /></p>
                        <asp:GridView runat="server" ID="gv_customers" AutoGenerateColumns="False" CellPadding="4"
                                      ForeColor="Black" GridLines="Horizontal" CssClass="table table-bordered table-condensed"
                                      OnDataBound="gv_customers_DataBound"
                                      ClientIDMode="Static" >
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="gv_lNumber" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch %>" />
                                <asp:BoundField DataField="BoxName" HeaderText="<%$Resources:Tokens,Box %>" />
                                <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,RequestDate%>" />
                                <asp:BoundField DataField="DepositorName" HeaderText="<%$Resources:Tokens,Depositor %>" />
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>" />
                                                        <asp:TemplateField HeaderText="<%$Resources:Tokens,DirectingBalanceTo%>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbldirection" Text='<%#Eval("CreditORVoice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Attachments %>">
                                    <ItemTemplate>
                                        <a href='<%#Eval("Url") %>' target="_blank" data-rel="tooltip" title="<%=Tokens.Download %>"><i class="icon-paper-clip blue bigger-140"></i></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        
                                        <asp:Button ID="gv_bConfirm" runat="server" Text="<%$Resources:Tokens,Confirm %>"
                                                    CommandArgument='<%# Bind("ID") %>' CssClass="btn btn-success btn-sm" OnClick="gv_bConfirm_Click" UseSubmitBehavior="false"  OnClientClick="save()" />
                                                
                                        <asp:HiddenField ID="gv_hf_id" runat="server" Value='<%# Bind("ID") %>' />
                                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-danger btn-sm" data-val="Reject" Text="<%$Resources:Tokens,Reject %>"
                                                    CommandArgument='<%# Bind("ID") %>' ToolTip='<%# Bind("ID") %>' />
                                             
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle"></RowStyle>
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
            </asp:View>
        </asp:MultiView>
        <div id="rej"class="bootbox modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
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
                        <asp:Literal runat="server" ID="lblRejectres" Text="<%$Resources:Tokens,RejectReason %>"></asp:Literal>
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
                                    Text="<%$Resources:Tokens,Save%>" ClientIDMode="Static" CssClass="btn btn-primary"
                                    ValidationGroup="x" />
&nbsp;
                    <button data-bb-handler="cancel" data-dismiss="modal" type="button" class="btn btn-default">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,Cancel %>"></asp:Literal></button>
            </div>
        </div></div></div>
        </fieldset>
    </div>
   
    <script type="text/javascript">
        
        function save() {

            var table = document.getElementById('<%=gv_customers.ClientID%>');

             if (table.rows.length > 0) {
                 //loop the gridview table
                 for (var i = 1; i < table.rows.length; i++) {
                     //get all the input elements
                     var inputs = table.rows[i].getElementsByTagName("input");
                     for (var j = 0; j < inputs.length; j++) {
                         //get the textbox1
                         if (inputs[j].id.indexOf("gv_bConfirm") > -1) {
                             $(inputs[j]).attr('disabled', 'disabled');

                         }

                     }

                 }
             }



         }


        $(document).ready(function () {
                        <%--/*var dlg = $('#rej').dialog({
                autoOpen: false,
                width: 600,
                height: 230,
                resizable: false,
                draggable: false,
                title: "<%=Tokens.RejectReason %>",
            modal: true,
            position: 'center'
        });
        dlg.parent().appendTo(jQuery("form:first"));*/--%>
            $('input[data-val="Reject"]').click(function () {
                var x = $(this).attr("Title");
                $('#hf_rejectionId').val(x);
                //$('#rej').dialog('open');
                $('#rej').modal('show');
                return false;
            });

            $('#ddl_Branch').change(function () {
                //                var selection = $('#ddl_Branch').val();
                //                $('#HiddenField1').val(selection);
                //                $('#ddl_Branch').attr('disabled', 'disabled');
            });
        });
        function hidval() {
            var selection = $('#ddl_Branch').val();
            $('#HiddenField1').val(selection);
            $('#ddl_Branch').attr('disabled', 'disabled');
        }
    </script>
</asp:Content>

