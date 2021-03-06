﻿<%@ Page Title="<%$ Resources:Tokens,OutgoingTypes %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="OutgoingTypes.aspx.cs" Inherits="NewIspNL.Pages.OutgoingTypes" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
    <tr>
        <td>
            <div class="view">
            <fieldset>
                <div class="page-header"><h1>
                    <asp:Label ID="Label2" runat="server" 
                               Text="<%$ Resources:Tokens,OutgoingTypes%>"></asp:Label></h1>
                </div>
                <div class="well">
                <table width="100%">
                    <tr>
                        <td style="width: 149px">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Tokens,Name %>"></asp:Label>
                        </td>
                        <td style="width: 304px">
                            <asp:TextBox ID="txt_Name" runat="server" Width="300px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <td style="width: 31px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                        ErrorMessage="<%$ Resources:Tokens,Required %>" ControlToValidate="txt_Name" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btn_Insert" runat="server" Text="<%$ Resources:Tokens,Add%>" ValidationGroup="Insert" 
                                        Width="100px" onclick="btn_Insert_Click" ClientIDMode="Static" CssClass="btn btn-success" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                     
                            
                            
                            
                            
                            
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
                               Text="<%$ Resources:Tokens,Index %>"></asp:Label>
                </h3>
                <asp:GridView ID="grd" runat="server" Width="100%" 
                              AutoGenerateColumns="False" DataKeyNames="ID" 
                              onrowcancelingedit="grd_RowCancelingEdit" onrowdeleting="grd_RowDeleting" 
                              onrowediting="grd_RowEditing" onrowupdating="grd_RowUpdating" 
                              CssClass="table table-bordered table-condensed"
                              GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Tokens,Name %>">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:CommandField HeaderText="<%$ Resources:Tokens,Edit %>" ShowEditButton="True" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:CommandField HeaderText="<%$ Resources:Tokens,Delete %>" ShowDeleteButton="True" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            </div>
        </td>
    </tr>
</table>
    <script type="text/javascript">
        
        function plswait(id) {

            var type = document.getElementById('<%=txt_Name.ClientID%>').value;
         



             if (type == "" ) { return; }
             else {
                 var check2 = document.getElementById(id);
                 check2.disabled = 'true'; check2.value = 'Please wait...';
             }

         }


        $(document).ready(function () {
            $('#txt_Name').keypress(function (e) {
                var key = e.which;
                if (key == 13) {
                    $('#btn_Insert').click();
                    return false;
                } else {
                    return true;
                }
            });
        });
    </script>
</asp:Content>


