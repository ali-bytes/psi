<%@ Page Title="<%$Resources:Tokens,EMailCnfg %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EMailCnfg.aspx.cs" Inherits="NewIspNL.Pages.EMailCnfg" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal runat="server" Text="<%$ Resources:Tokens,Index %>"></asp:Literal></h1></div>
                    <div style="padding: 5px;">
                        <%--<p>
                            <asp:LinkButton ID="b_new" runat="server"  Width="90px"
                                OnClick="b_new_Click" CssClass="btn btn-success"><i class="icon-plus-sign-alt"></i>&nbsp;<%=Tokens.New %></asp:LinkButton></p>--%>
                        <asp:GridView ID="gv_index" runat="server"  GridLines="Horizontal"
                            AutoGenerateColumns="False" OnDataBound="gv_index_DataBound"
                            CssClass="table table-bordered table-condensed" >
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="l_number" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MailFrom" HeaderText="<%$ Resources:Tokens,MailFrom %>" />
                                <asp:BoundField DataField="SmtpClient" HeaderText="<%$Resources:Tokens,SmtpClient %>" />
                                <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>" />
                                <asp:BoundField DataField="Password" HeaderText="<%$Resources:Tokens,Password %>" />
                                <asp:BoundField DataField="Port" HeaderText="<%$Resources:Tokens,Port %>" />
                                <asp:BoundField DataField="SSL" HeaderText="<%$Resources:Tokens,SSL %>" />
                                <asp:BoundField DataField="Active" HeaderText="<%$Resources:Tokens,Active %>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                                    <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click" data-rel="tooltip"
                                                        ToolTip="<%$ Resources:Tokens,Edit %>"><i class="icon-pencil icon-only bigger-130"></i></asp:LinkButton>
&nbsp;
                                                    <%--<asp:LinkButton ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' ToolTip="<%$ Resources:Tokens,Delete%>"
                                                         OnClick="gvb_delete_Click" data-rel="tooltip"><i class="icon-only icon-trash bigger-130 red"></i></asp:LinkButton>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,EMailCnfg %>"></asp:Literal></h1></div>
                    <asp:HiddenField ID="hf_id" runat="server" />
                    <div runat="server" ID="inputs" class="well">
                        <div>
                            <label>
                                <asp:Label ID="Label2" AssociatedControlID="TbMailFrom" runat="server" Text="<%$ Resources:Tokens,MailFrom %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbMailFrom" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFMailFrom" ControlToValidate="TbMailFrom"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LSmtpClient" AssociatedControlID="TbSmtpClient" runat="server" Text="<%$ Resources:Tokens,SmtpClient %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbSmtpClient" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RTbSmtpClient" ControlToValidate="TbSmtpClient"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LUserName" AssociatedControlID="TbUserName" runat="server" Text="<%$ Resources:Tokens,UserName %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbUserName" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RTbUserName" ControlToValidate="TbUserName"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LPassword" AssociatedControlID="TbPassword" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbPassword" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RTbPassword" ControlToValidate="TbPassword"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <br />
                            <asp:CheckBox ID="CbSsl" Text="<%$Resources:Tokens,SSL %>" runat="server" />
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LPort" AssociatedControlID="TbPort" runat="server" Text="<%$ Resources:Tokens,Port %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="TbPort" ClientIDMode="Static"></asp:TextBox>
                           
                            </div>
                        </div>
                        <div>
                            <br />
                            <asp:CheckBox ID="CbActive" Text="<%$Resources:Tokens,Active %>" runat="server" />
                        </div>
                    </div>
                    <p>
                        <asp:LinkButton runat="server" ID="b_save" OnClick="b_save_Click"
                           CssClass="btn btn-success"><i class="icon-ok"></i> &nbsp;<%=Tokens.Save %></asp:LinkButton>
                    </p>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
