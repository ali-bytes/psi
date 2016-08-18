<%@ Page Title="<%$Resources:Tokens,Governrates %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Governorates.aspx.cs" Inherits="NewIspNL.Pages.Governorates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="view">
                    <fieldset>
                        <div class="page-header">
                            <h1><asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Governorate %>"></asp:Label></h1>
                        </div>
                        <div class="well">
                            <div>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,GovernrateName %>"></asp:Label>
                                <div>
                                    <asp:TextBox ID="txt_Name" runat="server" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                                ControlToValidate="txt_Name" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label for="TbCode">
                                    <asp:Literal Text="<%$Resources:Tokens,Code %>" runat="server" />
                                </label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbCode" ClientIDMode="Static"></asp:TextBox>
                                    
                                </div>
                            </div> 
                        </div>
                        <div>
                            <asp:Button ID="btn_Insert" CssClass="btn btn-primary" runat="server" Text="<%$Resources:Tokens,Add %>" ValidationGroup="Insert"
                                Width="100px" OnClick="btn_Insert_Click"  UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />
                            <span>
                                <asp:Label ID="lbl_InsertResult" Font-Bold="True" EnableViewState="False" runat="server"></asp:Label>
                            </span>
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
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,AllGovernorates %>"></asp:Label>
                        </h3>
                        <asp:GridView ClientIDMode="Static" ID="grd" CssClass="table table-bordered table-condensed"
                            runat="server"  AutoGenerateColumns="False" DataKeyNames="ID" OnRowCancelingEdit="grd_RowCancelingEdit"
                            OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing" OnRowUpdating="grd_RowUpdating">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,GovernrateName %>">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TbName" runat="server" ValidationGroup="g" Text='<%# Bind("GovernorateName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RTbName" ErrorMessage="*" ControlToValidate="TbName"
                                            ValidationGroup="g" runat="server"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="omak" runat="server" Text='<%# Bind("GovernorateName") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Code %>">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TbCode" runat="server" ValidationGroup="g" Text='<%# Bind("Code") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Literal ID="omak2" runat="server" Text='<%# Bind("Code") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="<%$Resources:Tokens,Edit %>" ShowEditButton="True"
                                    EditText="<%$Resources:Tokens,Edit%>" UpdateText="<%$Resources:Tokens,Update%>"
                                    ValidationGroup="g" CancelText="<%$Resources:Tokens,Cancel%>" />
                                <asp:CommandField HeaderText="<%$Resources:Tokens,Delete %>" ShowDeleteButton="True"
                                    DeleteText="<%$Resources:Tokens,Delete%>" />
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




             if (type == "") { return; }
             else {
                 var check2 = document.getElementById(id);
                 check2.disabled = 'true'; check2.value = 'Please wait...';
             }

         }




        $(function () {
            $('#grd a').addClass('btn btn-primary');
        });
    </script>
</asp:Content>
