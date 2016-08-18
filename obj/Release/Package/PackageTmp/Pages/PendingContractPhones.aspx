<%@ Page Title="<%$Resources:Tokens,PendingContractPhones%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PendingContractPhones.aspx.cs" Inherits="NewIspNL.Pages.PendingContractPhones" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <script src="../Content/ace-assest/js/jquery-ui-1.10.3.full.min.js" type="text/javascript"></script>
    <div class="view">
        <div id="message" style="background-color: #dff0d8; color: #468847;border-color: #d6e9c6;
            margin: 5px;">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
        <div>
            <asp:Panel runat="server" ID="containerPanel" ClientIDMode="Static">
                <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,PendingContractPhones%>"></asp:Literal></h1></div>
                <div style="padding: 15px;">
                    <asp:GridView runat="server" ID="gv_items" 
                    CssClass="table table-bordered table-responsive text-center"
                         ClientIDMode="Static" OnDataBound="gv_items_DataBound" AutoGenerateColumns="False" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="l_Number" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Phone1" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="Name" HeaderText="<%$Resources:Tokens,Name%>" />
                            <asp:BoundField DataField="Governate" HeaderText="<%$Resources:Tokens,Governrate%>" />
                            <asp:BoundField DataField="Offer1" HeaderText="<%$Resources:Tokens,Offer1%>" />
                            <asp:BoundField DataField="Offer2" HeaderText="<%$Resources:Tokens,Offer2%>" />
                            <asp:BoundField DataField="State" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:BoundField DataField="Employee" HeaderText="<%$Resources:Tokens,Employee%>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="b_approve" runat="server" data-val="Approve" Text="<%$Resources:Tokens,Approve%>"
                                                    CommandArgument='<%# Bind("Id") %>' CssClass="btn btn-success" OnClick="b_approve_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="b_reject" runat="server" Text="<%$Resources:Tokens,Reject%>" data-val="Reject"
                                                    CommandArgument='<%# Bind("Id") %>' CssClass="btn btn-danger" ToolTip='<%# Bind("Id") %>' ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div>
                                <%=Tokens.NoResults %></div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <div>
                <div id="reject" style="padding: 15px;">
                    <fieldset>
                        <legend>
                            <asp:Label runat="server" Text="<%$Resources:Tokens,Reject%>" ID="Label13"></asp:Label></legend>
                        <div>
                            <div>
                                <asp:Label runat="server" ID="Label14" Text="<%$Resources:Tokens,Reason%>"></asp:Label></div>
                            <div>
                                <asp:DropDownList ID="ddl_reject2" runat="server" Width="209px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="ddl_reject2"
                                    ErrorMessage="<%$Resources:Tokens,Required%>" ValidationGroup="reject"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hf_rejectionId" runat="server" ClientIDMode="Static" />
                            </div>
                            <p>
                                <asp:Button runat="server" Text="<%$Resources:Tokens,Save%>" ID="b_saveReject" OnClick="b_saveReject_Click"
                                    Width="90px" ValidationGroup="reject" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only ui-state-hover" /></p>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message')
                    .css("border", "silver solid 1px")
                    .css("padding", "4px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", " 0 0 1px#888")
                    .css("box-shadow", "0 0 1px #888");
            }

            var dlg = $('#reject').dialog({
                autoOpen: false,
                width: 600,
                height: 230,
                resizable: false,
                draggable: false,
                title: 'Reject reason',
                modal: true,
                position: 'center'
            });
            dlg.parent().appendTo(jQuery("form:first"));

            $('input[data-val="Reject"]').click(function () {
                var x = $(this).attr("Title");
                $('#hf_rejectionId').val(x);
                $('#reject').dialog('open');
                return false;
            });

            $("#gv_items tr")
                .not(':first')
                .hover(function () {
                    $(this)
                        .css("background-color", "rgb(243, 255, 195)");
                },
                    function () {
                        $(this)
                            .css("background-color", "");
                    });
        });
    </script>
</asp:Content>

