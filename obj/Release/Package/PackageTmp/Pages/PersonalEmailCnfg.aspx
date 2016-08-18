<%@ Page Title="<%$ Resources:Tokens,PersonalEmailCnfg%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PersonalEmailCnfg.aspx.cs" Inherits="NewIspNL.Pages.PersonalEmailCnfg" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <div id="message">
                    <asp:Label runat="server" ID="l_message"></asp:Label></div>
                <asp:Panel runat="server" ID="p_index">
                    <div class="page-header"><h1><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Tokens,Index %>"></asp:Literal></h1></div>
                    <asp:LinkButton runat="server" ID="btnAddNew" OnClick="b_new_Click" CssClass="btn btn-success"><i class="icon-white icon-plus-sign"></i>&nbsp;<%=Tokens.New %></asp:LinkButton>
                    <div style="padding: 5px;">
                        <asp:GridView ID="gv_index" runat="server"  GridLines="Horizontal"
                            AutoGenerateColumns="False" OnDataBound="gv_index_DataBound"
                            CssClass="table table-bordered table-condensed center" >
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="l_number" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PersonalMail" HeaderText="<%$ Resources:Tokens,Email %>" />
                                <asp:BoundField DataField="PersonalPassword" HeaderText="<%$Resources:Tokens,Password %>" />
                                <asp:BoundField DataField="Port" HeaderText="<%$Resources:Tokens,Port %>" />
                                <asp:BoundField DataField="Pop3" HeaderText="Pop3" />
                                <asp:BoundField DataField="SmtpClient" HeaderText="<%$Resources:Tokens,SmtpClient %>" />
                                <asp:BoundField DataField="SendPort" HeaderText="<%$Resources:Tokens,SendPort %>" />
                                <asp:BoundField DataField="SSL" HeaderText="<%$Resources:Tokens,SSL %>" />
                                <asp:BoundField DataField="Active" HeaderText="<%$Resources:Tokens,Active %>" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                                    <asp:LinkButton ID="gvb_edit" runat="server" CommandArgument='<%# Bind("Id") %>' OnClick="gvb_edit_Click" data-rel="tooltip"
                                                        ToolTip="<%$ Resources:Tokens,Edit %>"><i class="icon-pencil icon-only bigger-130"></i></asp:LinkButton>
&nbsp;
                                                     </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <asp:Panel runat="server" ID="p_add">
                    <div class="page-header"><h1><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Tokens,EMailCnfg %>"></asp:Literal></h1></div>
                    <asp:HiddenField ID="hf_id" runat="server" />
                    <div runat="server" ID="inputs" class="well">
                        <div>
                            <label>
                                <asp:Label ID="Label2" AssociatedControlID="txtEMail" runat="server" Text="<%$ Resources:Tokens,Email %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtEMail" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RFMailFrom" ControlToValidate="txtEMail"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LPassword" AssociatedControlID="txtPassword" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtPassword" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RTbPassword" ControlToValidate="txtPassword"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LSmtpClient" AssociatedControlID="txtPop3" runat="server" Text="Pop3"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtPop3" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RTbSmtpClient" ControlToValidate="txtPop3"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="LPort" AssociatedControlID="txtPort" runat="server" Text="<%$ Resources:Tokens,Port %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtPort" ClientIDMode="Static"></asp:TextBox>
                           <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtPort"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="Label1" AssociatedControlID="txtSmtpClient" runat="server" Text="<%$ Resources:Tokens,SmtpClient %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtSmtpClient" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtSmtpClient"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                                                <div>
                            <label>
                                <asp:Label ID="LUserName" AssociatedControlID="txtUserName" runat="server" Text="<%$ Resources:Tokens,UserName %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RTbUserName" ControlToValidate="txtUserName"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>
                                <asp:Label ID="Label3" AssociatedControlID="txtUserPassword" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtUserPassword" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtUserPassword"
                                                            ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                                                <div>
                            <label>
                                <asp:Label ID="Label4" AssociatedControlID="txtsendPort" runat="server" Text="<%$ Resources:Tokens,SendPort %>"></asp:Label>
                            </label>
                            <div>
                                <asp:TextBox runat="server" ID="txtsendPort" ClientIDMode="Static"></asp:TextBox>
                           <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtsendPort"
                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="pers"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <br />
                            <asp:CheckBox ID="CbSsl" Text="<%$Resources:Tokens,SSL %>" runat="server" />
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


