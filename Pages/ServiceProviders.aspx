<%@ Page Title="<%$ Resources:Tokens,Service.Providers %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ServiceProviders.aspx.cs" Inherits="NewIspNL.Pages.ServiceProviders" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="view">
                <fieldset>
                    <div class="page-header"><h1>
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,Add %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table width="100%">
                        <tr>
                            <td style="width: 183px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Name %>"></asp:Label>
                            </td>
                            <td style="width: 304px">
                                <asp:TextBox ID="txt_Name" runat="server" Width="300px"></asp:TextBox>
                            </td>
                            <td style="width: 31px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                            ControlToValidate="txt_Name" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Button ID="btn_Insert" runat="server" Text="<%$ Resources:Tokens,Add %>" ValidationGroup="Insert"
                                            Width="100px" OnClick="btn_Insert_Click" CssClass="btn btn-success"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Label ID="lbl_InsertResult" Font-Bold="True" EnableViewState="False" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                </fieldset>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,Index %>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID"
                                  OnRowCancelingEdit="grd_RowCancelingEdit" OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing"
                                  OnRowUpdating="grd_RowUpdating"  GridLines="None"
                                  CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Name %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SPName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("SPName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Active %>">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CB1" runat="server" Checked='<%# Bind("Active") %>'></asp:CheckBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CB2" runat="server" Checked='<%# Bind("Active") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:CommandField HeaderText="<%$ Resources:Tokens,Edit %>" ShowEditButton="True"  EditText="<%$Resources:Tokens,Edit%>" UpdateText="<%$Resources:Tokens,Update%>" CancelText="<%$Resources:Tokens,Cancel%>"/>
                            <asp:CommandField HeaderText="<%$ Resources:Tokens,Delete %>" ShowDeleteButton="True" DeleteText="<%$Resources:Tokens,Delete%>"/>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

