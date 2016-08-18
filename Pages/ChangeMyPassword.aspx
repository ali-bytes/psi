<%@ Page Title="<%$Resources:Tokens,ChangeMyPassword%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ChangeMyPassword.aspx.cs" Inherits="NewIspNL.Pages.ChangeMyPassword" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header"><h1><asp:Label ID="Label7" runat="server" Text="<%$Resources:Tokens,ChangeMyPassword %>"></asp:Label></h1></div>
<div class="row">
            <fieldset>
                <div class="well">
                <table width="100%">
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,UserName%>"></asp:Label>
                        </td>
                        <td style="width: 218px">
                            <asp:Label ID="lbl_UserName" runat="server" ></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,LoginName%>"></asp:Label>
                        </td>
                        <td style="width: 218px">
                            <asp:Label ID="lbl_LoginName" runat="server" ></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,PreferedLang%>"></asp:Label>
                        </td>
                        <td style="width: 218px">
                            <asp:DropDownList runat="server" ID="ddlCulture"/>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ControlToValidate="ddlCulture" ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>" ValidationGroup="UpdateVG"></asp:RequiredFieldValidator>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Password%>"></asp:Label>
                        </td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txt_LoginPassword" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ControlToValidate="txt_LoginPassword" ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="<%$Resources:Tokens,Required%>" Text="<%$Resources:Tokens,Required%>" ValidationGroup="UpdateVG"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                                        
                    <tr>
                        <td colspan="3" align="center">
                            <asp:LinkButton ID="btn_UpdatePassword" runat="server"
                                ValidationGroup="UpdateVG" onclick="btn_UpdatePassword_Click" CssClass="btn btn-success"><i class="icon-wrench"></i>&nbsp;<%=Tokens.ChangeSettings %></asp:LinkButton>
                        </td>
                        
                    </tr>
                     <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblProcessResult" runat="server" EnableViewState="False" 
                                Font-Bold="True"></asp:Label>
                         </td>
                        
                    </tr>
                </table>
                </div>
            </fieldset>
            </div>
</asp:Content>

