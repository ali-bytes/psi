<%@ Page Title="Manage ADS" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Ads.aspx.cs" Inherits="NewIspNL.Pages.Ads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
    <table width="100%">
        <tr>
            <td>
                <fieldset>
                    <div class="page-header">
                       <h1> <asp:Label ID="l_managaAds" runat="server" Text="<%$Resources:Tokens,ManageAds%>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table width="100%">
                        <tr>
                            <td style="width: 100px">
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Title%>"></asp:Label>
                            </td>
                            <td style="width: 409px">
                                <asp:TextBox ID="txt_Title" runat="server" Width="400px" MaxLength="255"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ControlToValidate="txt_Title" ValidationGroup="UpdateVG"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Details%>"></asp:Label>
                            </td>
                            <td style="width: 409px">
                                <asp:TextBox ID="txt_Details" runat="server" Rows="3" TextMode="MultiLine" 
                                             Width="400px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                            ErrorMessage="<%$Resources:Tokens,Required%>" ControlToValidate="txt_Details" 
                                                            ValidationGroup="UpdateVG"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="Button1" runat="server" Text="<%$Resources:Tokens,Update%>" Width="100px" 
                                            ValidationGroup="UpdateVG" onclick="Button1_Click" CssClass="btn btn-default"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Label ID="lbl_ProcessResult" runat="server" EnableViewState="False" 
                                           Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

