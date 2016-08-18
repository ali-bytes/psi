<%@ Page Title="<%$Resources:Tokens,Branches%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Branches.aspx.cs" Inherits="NewIspNL.Pages.Branches" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
                       <h1> <asp:Label ID="l_t_AddBranche" runat="server" Text="<%$Resources:Tokens,Branches %>"></asp:Label></h1>
                    </div>
    <div class="col-sm-12" runat="server" id="ser">
            <div class="widget-box">
                <div class="widget-header">
                    <h4>
                        <%=Tokens.AddBranch%></h4>
                    <span class="widget-toolbar" style="padding: 12px"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                    </i></a><a href="#" data-action="reload"><i class="icon-refresh"></i></a><a href="#"
                        data-action="close"><i class="icon-remove"></i></a></span>
                </div>
                <div class="widget-body">
                    <div class="widget-main no-padding">
                        <div>
                            <!-- <legend>Form</legend> -->
                            <fieldset style="padding: 16px;">
                                <div class="span4">
                                    <div class="view">
                <fieldset>
                    
                    <div>
                    <table width="100%">
                        <tr>
                            <td style="width: 129px">
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,BranchName %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:TextBox ID="txt_BranchName" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 31px">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txt_BranchName" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 140px">
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,BranchAddress %>"></asp:Label>
                            </td>
                            <td style="width: 158px">
                                <asp:TextBox ID="txt_BranchAddress" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txt_BranchAddress" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px">
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Phone1 %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:TextBox ID="txt_Phone1" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="txt_Phone1" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 140px">
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Phone2 %>"></asp:Label>
                            </td>
                            <td style="width: 158px">
                                <asp:TextBox ID="txt_Phone2" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px">
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Tokens,Mobile1 %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:TextBox ID="txt_Mobile1" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td style="width: 140px">
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Tokens,Mobile1 %>"></asp:Label>
                            </td>
                            <td style="width: 158px">
                                <asp:TextBox ID="txt_Mobile2" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 129px">
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Tokens,Fax %>"></asp:Label>
                            </td>
                            <td style="width: 162px">
                                <asp:TextBox ID="txt_Fax" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td style="width: 140px">
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Tokens,AdministratorName %>"></asp:Label>
                            </td>
                            <td style="width: 158px">
                                <asp:DropDownList ID="ddl_Admins" runat="server" Width="155px" DataTextField="UserName"
                                                  DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"
                                                            ControlToValidate="ddl_Admins" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <asp:Label ID="lbl_InsertResult" Font-Bold="True" EnableViewState="False" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                    
                </fieldset>
                </div>
                                </div>
                            </fieldset>
                            <div class="form-actions center" style="margin: 0;">
                              <%-- <asp:LinkButton ID="btn_Insert" runat="server" CssClass="btn btn-success" ValidationGroup="Insert"
                                            Width="100px" OnClick="btn_Insert_Click" ><i class="icon-save icon-only"></i>&nbsp;<%=Tokens.Add %></asp:LinkButton>
                                --%>
                                
                                  <asp:Button ID="Button1" runat="server" ClientIDMode="Static" Text="<%$ Resources:Tokens,Add%>" ValidationGroup="Insert" 
                                        Width="100px" onclick="btn_Insert_Click" CssClass="btn btn-success" UseSubmitBehavior="false" OnClientClick="plswait(this.id)" />

                                

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    
    
    

    <table width="100%">
        <tr>
            <td>
                <div class="view">
                <fieldset>
                    <h3 class="header smaller lighter blue">
                        <asp:Label ID="l_t_AllBranches" runat="server" Text="<%$Resources:Tokens,Branches%>"></asp:Label>
                    </h3>
                    <asp:GridView ID="grd" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID,UserID"
                                  OnRowCancelingEdit="grd_RowCancelingEdit" OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing"
                                  OnRowUpdating="grd_RowUpdating" 
                                  CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,BranchName %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("BranchName") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,BranchAddress %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("BranchAddress") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label10" runat="server" Text='<%# Bind("BranchAddress") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Phone1 %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Phone1") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label11" runat="server" Text='<%# Bind("Phone1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Phone2 %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Phone2") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label12" runat="server" Text='<%# Bind("Phone2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Mobile1 %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox5" Width="80px" runat="server" Text='<%# Bind("Mobile1") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label13" runat="server" Text='<%# Bind("Mobile1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Mobile2 %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox6" Width="80px" runat="server" Text='<%# Bind("Mobile2") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label14" runat="server" Text='<%# Bind("Mobile2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Fax %>">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox7" Width="80px" runat="server" Text='<%# Bind("Fax") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label15" runat="server" Text='<%# Bind("Fax") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Tokens,AdministratorName %>">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_Users_Grd" runat="server" DataTextField="UserName" Width="100px"
                                                      DataValueField="ID">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label16" runat="server" Text='<%# Bind("Admin") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Update" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Update%>"><i class="icon-ok-circle icon-only green bigger-120"></i></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                          data-rel="tooltip" ToolTip="<%$Resources:Tokens,Cancel%>"><i class="icon-undo icon-only red bigger-120"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Edit" 
                                                   data-rel="tooltip" ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-pencil icon-only bigger-120"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="BDel" runat="server" OnClick="BDel_OnClick" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد حذف الفرع؟');" 
                                    ToolTip="<%$Resources:Tokens,Delete%>" data-rel="tooltip"><i class="icon-trash icon-only red bigger-120"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <%-- <asp:CommandField HeaderText="<%$Resources:Tokens,Delete%>" ShowDeleteButton="True" 
                                              DeleteText="<%$Resources:Tokens,Delete%>" />--%>
                        </Columns>
                        
                    </asp:GridView>
                </fieldset>
                </div>
            </td>
        </tr>
    </table>
    
    <script type="text/javascript">

        function plswait(id) {

            var type = document.getElementById('<%=txt_BranchName.ClientID%>').value;
              var amo = document.getElementById('<%=txt_Phone1.ClientID%>').value;

              var type2 = document.getElementById('<%=txt_BranchAddress.ClientID%>').value;
              var com = document.getElementById('<%=ddl_Admins.ClientID%>').value;



              if (type == "" || amo == "" || type2 == "" || com == "") { return; }
              else {
                  var check2 = document.getElementById(id);
                  check2.disabled = 'true'; check2.value = 'Please wait...';
              }

          }     </script>
</asp:Content>