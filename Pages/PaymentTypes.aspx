<%@ Page Title="<%$ Resources:Tokens,PaymentTypes%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="PaymentTypes.aspx.cs" Inherits="NewIspNL.Pages.PaymentTypes" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="view">
                <fieldset>
                    <div class="page-header"><h1>
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,Add %>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table width="100%">
                        <tr>
                            <td style="width: 149px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Name%>"></asp:Label>
                            </td>
                            <td style="width: 304px">
                                <asp:TextBox ID="txt_PaymentTypeName" runat="server" Width="300px"></asp:TextBox>
                            </td>
                            <td style="width: 44px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                            ControlToValidate="txt_PaymentTypeName" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 149px">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,Amount%>"></asp:Label>
                            </td>
                            <td style="width: 304px">
                                <asp:TextBox ID="txt_Amount" runat="server" Width="300px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txt_Amount_FilteredTextBoxExtender" runat="server"
                                                             FilterType="Numbers" TargetControlID="txt_Amount" Enabled="True">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 44px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                            ControlToValidate="txt_Amount" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,L.E %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Tokens,Add%>" ValidationGroup="Insert"
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
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,Index %>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID"
                                  OnRowCancelingEdit="grd_RowCancelingEdit" OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing"
                                  OnRowUpdating="grd_RowUpdating" GridLines="None"
                                  CssClass="table table-bordered table-condensed text-center">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Name %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PaymentTypeName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PaymentTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Tokens,Amount%>">
                                <EditItemTemplate>
                                    <table width="150">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Amount") %>'></asp:TextBox><asp:FilteredTextBoxExtender
                                                                                                                                         ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBox2" FilterType="Numbers"
                                                                                                                                         Enabled="True">
                                                                                                                                     </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                                                            ControlToValidate="TextBox2" ValidationGroup="Update"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="<%$ Resources:Tokens,Edit %>" ShowEditButton="True"
                                              ValidationGroup="Update"  EditText="<%$Resources:Tokens,Edit%>" UpdateText="<%$Resources:Tokens,Update%>" CancelText="<%$Resources:Tokens,Cancel%>"/>
                            <asp:CommandField HeaderText="<%$ Resources:Tokens,Delete %>" ShowDeleteButton="True" DeleteText="<%$Resources:Tokens,Delete%>"/>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
