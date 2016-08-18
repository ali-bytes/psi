<%@ Page Title="<%$Resources:Tokens,RejectionReasons%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="RejectionReasons.aspx.cs" Inherits="NewIspNL.Pages.RejectionReasons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message" style="background-color:#dff0d8; color:  #468847;border-color: #d6e9c6;">
                    <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index" >
                    <div class="page-header"><h1><asp:Literal ID="lbljkh" runat="server" Text="<%$Resources:Tokens,Index%>"></asp:Literal></h1></div>
                    <div style="padding: 5px;">
                        <p>
                            <asp:Button ID="b_new" runat="server" Text="<%$Resources:Tokens,New%>" Width="90px"
                                OnClick="b_new_Click" CssClass="btn btn-success"/></p>
                        <div style="text-align: center">
                            <asp:GridView ID="gv_index" runat="server" 
                            CssClass="table table-bordered table-condensed "
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound" 
                                 ClientIDMode="Static">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="l_number" runat="server"></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Reason%>" DataField="Reason">
                                        <ItemStyle Width="400px" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                                        <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click" data-rel="tooltip"
                                                            ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-pencil icon-only bigger-130"></i></asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                        <asp:LinkButton ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' ToolTip="<%$Resources:Tokens,Delete%>"
                                                             OnClick="gvb_delete_Click" data-rel="tooltip"><i class="icon-trash icon-only bigger-130 red"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,Reason%>"></asp:Literal></h1></div>
                    <div class="well">
                        <label>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Reason%>"></asp:Label>
                        </label>
                        <div>
                            <asp:TextBox runat="server" ID="tb_reason" Height="83px" TextMode="MultiLine" Width="339px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="r_offer" ControlToValidate="tb_reason"
                                ErrorMessage="<%$Resources:Tokens,Required%>"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hf_id" runat="server" />
                        </div>
                        <p>
                            <br/>
                            <asp:Button runat="server" ID="b_save" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Save%>" OnClick="b_save_Click"/>
                        </p>
                    </div>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
    <script>
        $(document).ready(function () {
            if ($('#l_message').html() === '') {
                $('#message').css("border", "none");
            } else {
                $('#message').css("border", "silver solid 1px").css("padding", "4px")
                    .css("-moz-box-shadow", "0 0 1px #888")
                    .css("-webkit-box-shadow", " 0 0 1px#888")
                    .css("box-shadow", "0 0 1px #888");
            }
            $("#gv_index tr").not(':first').hover(function () {
                $(this).css("background-color", "rgb(243, 255, 195)");
            }, function () {
                $(this).css("background-color", "");
            });
        });
    </script>
</asp:Content>
