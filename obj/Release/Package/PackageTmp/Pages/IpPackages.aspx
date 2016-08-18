<%@ Page Title="<%$ Resources:Tokens,IpPackages %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master"   AutoEventWireup="true" CodeBehind="IpPackages.aspx.cs" Inherits="NewIspNL.Pages.IpPackages" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="view">
                    <fieldset>
                        <div class="page-header"><h1>
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,AddIpPackage%>"></asp:Label></h1>
                        </div>
                        <div class="well">
                            <table width="100%">
                                <tr>
                                    <td style="width: 149px">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Name%>"></asp:Label>
                                    </td>
                                    <td style="width: 304px">
                                        <asp:TextBox ID="txt_Name" runat="server" Width="300px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                                     FilterType="Numbers" TargetControlID="txt_Name" Enabled="True">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 31px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                    ErrorMessage="<%$ Resources:Tokens,Required %>" ControlToValidate="txt_Name" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Insert" runat="server" CssClass="btn btn-success" Text="<%$ Resources:Tokens,Add%>" ValidationGroup="Insert" 
                                                    Width="100px" onclick="btn_Insert_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Label ID="lbl_InsertResult" Font-Bold="True" EnableViewState="False" 
                                                   runat="server"></asp:Label>
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
                            <asp:Label ID="Label3" runat="server" 
                                       Text="<%$ Resources:Tokens,AllIpPackages %>"></asp:Label>
                        </h3>
                        <asp:GridView ID="grd" runat="server" Width="100%" 
                                      AutoGenerateColumns="False" DataKeyNames="ID" 
                                      onrowcancelingedit="grd_RowCancelingEdit" onrowdeleting="grd_RowDeleting" 
                                      onrowediting="grd_RowEditing" onrowupdating="grd_RowUpdating"  GridLines="None"
                                      CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,Name%>">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("IpPackageName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("IpPackageName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="<%$ Resources:Tokens,Edit%>" ShowEditButton="True"  EditText="<%$Resources:Tokens,Edit%>" UpdateText="<%$Resources:Tokens,Update%>" CancelText="<%$Resources:Tokens,Cancel%>"/>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BDel" runat="server" OnClick="BDel_OnClick" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد الحذف ؟');" Text="<%$Resources:Tokens,Delete%>"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:CommandField HeaderText="<%$ Resources:Tokens,Delete %>" ShowDeleteButton="True" DeleteText="<%$Resources:Tokens,Delete%>"/>--%>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

