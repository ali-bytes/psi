<%@ Page Title="<%$ Resources:Tokens,ReceiptsSearch %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ReceiptsSearch.aspx.cs" Inherits="NewIspNL.Pages.ReceiptsSearch" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <asp:Literal ID="Label8" runat="server" Text="<%$ Resources:Tokens,Search %>"></asp:Literal></h1>
            </div>
            <div class="well">
                <table>
                    <tr>
                        <td style="width: 87px">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,ReceiptNumber %>"></asp:Label>
                        </td>
                        <td style="width: 148px">
                            <asp:TextBox ID="txt_ReceiptNo" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txt_ReceiptNo_FilteredTextBoxExtender" runat="server"
                                                         FilterType="Numbers" TargetControlID="txt_ReceiptNo" Enabled="True">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ControlToValidate="txt_ReceiptNo" ID="RequiredFieldValidator1"
                                                        runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>" ValidationGroup="vg_Search"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 87px">
                            &nbsp;
                        </td>
                        <td style="width: 148px">
                            <br/>
                            <asp:Button ID="btn_Search" runat="server" Text="<%$ Resources:Tokens,Search %>" ClientIDMode="Static"
                                        OnClick="btn_Search_Click" CssClass="btn btn-success" Width="93px" ValidationGroup="vg_Search" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center">
                            <asp:Label ID="lbl_Message" runat="server" Font-Bold="True" Enabled="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
    <div class="view">
    <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Label9" runat="server" Text="<%$ Resources:Tokens,ReceiptDetails %>"></asp:Literal>
            </h3>
    <div class="well" id="fs_ReceiptDetails" runat="server" visible="false">
        
        
            <table class="table table-bordered table-responsive">
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,ReceiptNumber %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ReceiptNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Tokens,Date %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ReceiptDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,Amount %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ReceiptAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,Notes %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ReceiptNotes" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Customer.Phone%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_customerphone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,Customer.Name%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_customername" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 131px">
                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Tokens,UserName%>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_username" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
       
    </div> </fieldset>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txt_ReceiptNo').keypress(function (e) {
                var key = e.which;
                if (key == 13) {
                    $('#btn_Search').click();
                    return false;
                } else {
                    return true;
                }
            });
        });
    </script>

</asp:Content>

