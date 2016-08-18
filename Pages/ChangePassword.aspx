<%@ Page Title="<%$Resources:Tokens,ChangePassword%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="NewIspNL.Pages.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header"><h1><asp:Label ID="Label4" runat="server" 
                                   Text="<%$Resources:Tokens,Users%>"></asp:Label></h1></div>
                <div class="row">
                <fieldset>
                    <div class="well">
                    <table width="100%">
                        <tr>
                            <td style="width: 121px">
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,SelectUser%>"></asp:Label>
                            </td>
                            <td style="width: 176px">
                                <asp:DropDownList ID="ddl_Users" runat="server" Style="margin-left: 0px" 
                                    Width="205px" DataTextField="LoginName" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 32px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"
                                    ControlToValidate="ddl_Users" ValidationGroup="SelectVG" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Button ID="btn_Edit" runat="server" CssClass="btn btn-success" ValidationGroup="SelectVG" Text="<%$Resources:Tokens,Edit%>" 
                                    Width="80px" onclick="btn_Edit_Click" />
                            </td>
                        </tr>
                        <tr id="tr_NewPassword" runat="server" visible="false">
                            <td style="width: 121px">
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,NewPassword%>"></asp:Label>
                            </td>
                            <td style="width: 176px">
                                <asp:TextBox ID="txt_LoginPassword" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td style="width: 32px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>"
                                    ControlToValidate="txt_LoginPassword" ValidationGroup="SelectVG"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Button ID="btn_Change" runat="server" CssClass="btn btn-primary" ValidationGroup="SelectVG" Text="<%$Resources:Tokens,Change%>"
                                    Width="80px" onclick="btn_Change_Click" />
                            &nbsp;<asp:Button ID="btn_Change0" runat="server" CssClass="btn btn-danger" Text="<%$Resources:Tokens,Cancel%>"
                                    Width="80px" onclick="btn_Change0_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Label ID="lbl_ProcessResult" runat="server" EnableViewState="False" 
                                    Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                </fieldset>
                </div>
</asp:Content>
