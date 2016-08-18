<%@ Page Title="<%$Resources:Tokens,CustomerFiles%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomerFiles.aspx.cs" Inherits="NewIspNL.Pages.CustomerFiles" %>

<%@ Register Src="../WebUserControls/UserFile.ascx" TagName="UserFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr id="tr_Search" runat="server">
            <td>
                <div class="view">
                <fieldset>
                    <div class="page-header">
                        <h1><asp:Label ID="Label37" runat="server" Text="<%$Resources:Tokens,Search%>"></asp:Label></h1>
                    </div>
                    <div class="well">
                    <table width="100%">
                        <tr runat="server" id="GovBox">
                            <td style="width: 113px">
                                <asp:Label ID="Label34" runat="server" Text="<%$Resources:Tokens,Governrate%>"></asp:Label>
                            </td>
                            <td class="style1" style="width: 196px">
                                <asp:DropDownList ID="ddl_Governorates0" runat="server" Width="170px" DataTextField="GovernorateName"
                                                  DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates0" ID="RequiredFieldValidator12"
                                                            runat="server" ErrorMessage="*" ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 113px">
                                <asp:Label ID="Label31" runat="server" Text="<%$Resources:Tokens,SearchBy%>"></asp:Label>
                            </td>
                            <td class="style1" style="width: 196px">
                                <asp:RadioButtonList ID="rbl_searchType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="1" Text="<%$Resources:Tokens,Phone%>"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="<%$Resources:Tokens,Name%>"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 113px">
                                <asp:Label ID="Label36" runat="server" Text="<%$Resources:Tokens,SearchValue%>"></asp:Label>
                            </td>
                            <td class="style1" style="width: 196px">
                                <asp:TextBox ID="txt_CustomerPhone0" runat="server" Width="170px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required%>"
                                                            Text="<%$Resources:Tokens,Required%>" ControlToValidate="txt_CustomerPhone0"
                                                            ValidationGroup="SearchVG"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="2">
                                <br/>
                                <asp:Button ID="btn_search" ClientIDMode="Static" runat="server" CssClass="btn btn-primary" Text="<%$Resources:Tokens,Search%>" Width="100px"
                                            OnClick="btn_search_Click" ValidationGroup="SearchVG" />
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
                    <uc1:UserFile ID="UserFile1" runat="server" Visible="False"/>
                    <asp:Button ID="btn_Update" runat="server" Visible="False" Text="<%$Resources:Tokens,UpdateCustomer%>"
                                Width="130px" OnClick="btn_Update_Click" CssClass="btn btn-success" />
                    <asp:Label ID="lbl_InsertResult" runat="server" EnableViewState="False" Font-Bold="True"></asp:Label>
                </fieldset>
            </div>
            </td>
        </tr>
    </table>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#txt_CustomerPhone0').keypress(function (e) {
                    var key = e.which;
                    if (key === 13) {
                        $('#btn_search').click();
                        return false;
                    } else {
                        return true;
                    }
                });
            });
        </script>
</asp:Content>
