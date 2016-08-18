<%@ Page Title="<%$ Resources:Tokens,Requests %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="NewIspNL.Pages.Requests" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
    <tr id="tr_Insert" runat="server" visible="false">
        <td>
            <fieldset>
                <legend>
                    <asp:Label ID="Label2" runat="server" 
                               Text="<%$ Resources:Tokens,Add %>"></asp:Label>
                </legend>
                <table width="100%">
                    <tr>
                        <td style="width: 149px">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Name %>"></asp:Label>
                        </td>
                        <td style="width: 304px">
                            <asp:TextBox ID="txt_Name" runat="server" Width="300px"></asp:TextBox>
                        </td>
                        <td style="width: 31px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="<%$ Resources:Tokens,Required %>" ControlToValidate="txt_Name" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btn_Insert" runat="server" Text="<%$ Resources:Tokens,Add %>" ValidationGroup="Insert" 
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
            </fieldset>
        </td>
    </tr>
    <tr>
        <td>
            <fieldset>
                <div class="page-header"><h1>
                    <asp:Label ID="Label3" runat="server" 
                               Text="<%$ Resources:Tokens,AllRequests %>"></asp:Label></h1>
                </div>
                <asp:GridView ID="grd" runat="server" Width="100%" 
                    AutoGenerateColumns="False" DataKeyNames="ID" 
                    onrowcancelingedit="grd_RowCancelingEdit" onrowdeleting="grd_RowDeleting" 
                    onrowediting="grd_RowEditing" onrowupdating="grd_RowUpdating" 
                    CellPadding="4" GridLines="None" CssClass="table table-bordered table-responsive">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,RequestName%>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("RequestName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RequestName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:CommandField HeaderText="<%$ Resources:Tokens,Edit %>" ShowEditButton="True" Visible="False" >
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:CommandField HeaderText="<%$ Resources:Tokens,Delete %>" ShowDeleteButton="True" Visible="False" >
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </fieldset>
        </td>
    </tr>
</table>
</asp:Content>


