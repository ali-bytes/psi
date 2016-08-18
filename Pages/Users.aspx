<%@ Page Title="<%$ Resources:Tokens,Users %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="NewIspNL.Pages.Users" %>


<%@ Import Namespace="Resources" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">  

            <div>
                <div>
                    <fieldset>
                        <div class="page-header"><h1>
                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Tokens,New%>"></asp:Label></h1>
                        </div>
                        <div class="well">
                            <table width="100%" runat="server" ID="AllInputs">
                                <tr>
                                    <td style="width: 133px">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Tokens,Name %>"></asp:Label>
                                    </td>
                                    <td style="width: 153px">
                                        <asp:TextBox ID="txt_UserName" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td style="width: 13px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="txt_UserName" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 117px">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Tokens,Phone %>"></asp:Label>
                                    </td>
                                    <td style="width: 161px">
                                        <asp:TextBox ID="txt_UserPhone" runat="server" Width="150px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txt_UserPhone_FilteredTextBoxExtender" runat="server"
                                            FilterType="Numbers" TargetControlID="txt_UserPhone" Enabled="True">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="txt_UserPhone" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 133px">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Tokens,Mobile %>"></asp:Label>
                                    </td>
                                    <td style="width: 153px">
                                        <asp:TextBox ID="txt_UserMobile" runat="server" Width="150px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txt_UserMobile_FilteredTextBoxExtender" runat="server"
                                            FilterType="Numbers" TargetControlID="txt_UserMobile" Enabled="True">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 13px">
                                    </td>
                                    <td style="width: 117px">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Tokens,Email %>"></asp:Label>
                                    </td>
                                    <td style="width: 161px">
                                        <asp:TextBox ID="txt_UserEmail" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="txt_UserEmail" ValidationGroup="Process" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$ Resources:Tokens,ValidMail %>"
                                            ControlToValidate="txt_UserEmail" ValidationGroup="Process" Display="Dynamic"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 133px">
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Tokens,Address %>"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txt_UserAddress" runat="server" Width="457px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="txt_UserAddress" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 133px">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Tokens,Group %>"></asp:Label>
                                    </td>
                                    <td style="width: 153px">
                                        <asp:DropDownList ID="ddl_Groups" runat="server" Width="155px" DataTextField="GroupName"
                                            DataValueField="ID" AutoPostBack="True" onselectedindexchanged="ddl_Groups_SelectedIndexChanged1" 
                                          >
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 13px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="ddl_Groups" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 117px">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Tokens,Branch %>"></asp:Label>
                                    </td>
                                    <td style="width: 161px">
                                        <asp:DropDownList ID="ddl_Branches" runat="server" Width="155px" DataTextField="BranchName"
                                            DataValueField="ID">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 13px">
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="ddl_Branches" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr runat="server" id="RowAccountManager" Visible="False">
                                       <td style="width: 133px">
                                        <asp:Label ID="Label1112" runat="server" Text="<%$ Resources:Tokens,ResellerAccountManager %>"></asp:Label>
                                    </td>
                                    <td style="width: 153px">
                                       <%-- <asp:TextBox ID="txtAccountManager" runat="server" Width="150px"></asp:TextBox>--%>
                                        <asp:DropDownList runat="server" ID="ddlAccountManager" Width="155px"/>
                                        <td style="width: 13px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ControlToValidate="ddlAccountManager" ValidationGroup="Process"></asp:RequiredFieldValidator>
                                    </td>

                                    </td>
                                    <td style="width: 13px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 117px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 161px">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 133px">
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Tokens,UserName %>"></asp:Label>
                                    </td>
                                    <td style="width: 153px">
                                        <asp:TextBox ID="txt_LoginName" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td style="width: 13px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="Process" ControlToValidate="txt_LoginName"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 117px">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Tokens,Password %>"></asp:Label>
                                    </td>
                                    <td style="width: 161px">
                                        <asp:TextBox ID="txt_LoginPassword" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="Process" ControlToValidate="txt_LoginPassword"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 133px">
                                        <%= Tokens.UserIp %></td>
                                    <td style="width: 153px">
                                        <div>
                                            <label for="TbUserIp">
                                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,UserIp %>" runat="server" />
                                            </label>
                                            <div>
                                                <asp:TextBox runat="server" ID="TbUserIp" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div> </td>
                                    <td style="width: 13px">
                                        &nbsp;</td>
                                    <td style="width: 117px">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,Governorate %>"></asp:Label>
                                        </td>
                                    <td style="width: 161px">
                                       <%-- <asp:TextBox ID="TbIp" runat="server" />--%>
                                       <asp:DropDownList runat="server" ID="ddlGovernorate" Width="155px"/>
                                       <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="Process" ControlToValidate="ddlGovernorate"></asp:RequiredFieldValidator>
                                    </td>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 133px">
                                        <asp:Literal runat="server" ID="lblCulture" Text="<%$Resources:Tokens,PreferedLang %>"></asp:Literal>
                                        
                                    </td>
                                    <td style="width: 153px">
                                        <div>
                                            <div>
                                                <asp:DropDownList runat="server"  ID="ddlCulture" Width="155px"/>
                                            </div>
                                        </div> </td>
                                    <td style="width: 13px">
                                            <asp:RequiredFieldValidator Visible="False" ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="Process" ControlToValidate="ddlCulture"></asp:RequiredFieldValidator>
                                        </td>
                                    <td style="width: 117px">
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:Tokens,privilegofManageRequest %>"></asp:Literal>
                                        </td>
                                    <td style="width: 161px">
                                       <asp:DropDownList runat="server" ID="ddlManageReuestPrivilege" Width="155px" CssClass="dropdown">
                                           <asp:ListItem Value="0" Text="<%$Resources:Tokens,All %>"></asp:ListItem>
                                           <asp:ListItem Value="1" Text="<%$Resources:Tokens,Resellers %>"></asp:ListItem>
                                           <asp:ListItem Value="2" Text="<%$Resources:Tokens,DirectUsersOnly %>"></asp:ListItem>
                                       </asp:DropDownList>
                                       <td>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$ Resources:Tokens,Required %>"
                                            ValidationGroup="Process" ControlToValidate="ddlManageReuestPrivilege"></asp:RequiredFieldValidator>
                                    </td>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 133px" valign="top">
                                        <asp:Label ID="Label14" runat="server" Font-Size="Small" Text="<%$ Resources:Tokens,AccountStoped %>"></asp:Label>
                                    </td>
                                    <td style="width: 153px" valign="top">
                                        <asp:CheckBox ID="chb_IsAccountStopped" runat="server" />
                                    </td>
                                    <td style="width: 13px">
                                        &nbsp;
                                    </td>
                                    <td style="width: 117px" valign="top">
                                        <asp:Label ID="Label15" runat="server" Font-Size="Small" Text="<%$ Resources:Tokens,UsersPageNotes %>"></asp:Label>
                                    </td>
                                    <td style="width: 161px">
                                        <asp:CheckBoxList ID="chl_Branchs" runat="server" DataTextField="BranchName" DataValueField="ID">
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Tokens,sav %>"></asp:Literal></td>
                                    <td>
                                        <asp:CheckBoxList CssClass="checkbox" runat="server" ID="chlSaves" DataTextField="SaveName" DataValueField="Id">
                                        </asp:CheckBoxList>
                                    </td>
                                    <td style="width: 13px">
                                        &nbsp;
                                    </td>
                                                                        <td style="width: 117px">
                                        <asp:Label ID="Label8" runat="server" Visible="False" Font-Size="Small" Text="<%$ Resources:Tokens,ResellerProviders %>"></asp:Label>
                                    </td>
                                    <td style="width: 161px">
                                        <asp:CheckBoxList ID="chl_Providers" runat="server" DataTextField="SPName" DataValueField="ID">
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Button ID="btn_Add" runat="server" Text="<%$ Resources:Tokens,NewUser %>" ValidationGroup="Process"
                                            OnClick="btn_Add_Click" CssClass="btn btn-success" />
                                        &nbsp;<asp:Button ID="btn_Update" runat="server" Text="<%$ Resources:Tokens,Update %>"
                                            ValidationGroup="Process" OnClick="btn_Update_Click"
                                            CssClass="btn btn-primary" Visible="False" /><%--ValidationGroup="<%$ Resources:Tokens,Process %>"--%>
                                        &nbsp;<asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:Tokens,Cancel %>"
                                            CssClass="btn btn-default" OnClick="btn_Cancel_Click" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <asp:Label ID="lbl_result" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div class="view">
                    <fieldset>
                        <h3 class="header smaller lighter blue">
                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:Tokens,Users %>"></asp:Label>
                        </h3>
                        <div class="col-sm-6">
                            <div class="well">

                            <label><%=Tokens.Branch %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlBranch"/>
                            </div>
                            <label><%=Tokens.Groups %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlGroup" DataTextField="GroupName" DataValueField="ID"/>
                            </div>
                            <div class="checkbox">
                            <label><input type="checkbox" id="checkIsstoped" runat="server"/><span class="lbl">&nbsp;<%=Tokens.AccountIsStopped %></span></label></div>
</div>
                            </div>
                            <div class="col-sm-6">
                            <div class="well" style="height: 193px">
                            <label> <asp:Literal ID="Literal4" runat="server" Text="<%$Resources:Tokens,ResellerAccountManager %>"></asp:Literal></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlfilterAccountManager" DataValueField="ID" DataTextField="UserName"/>
                            </div>
                            <label><asp:Literal ID="Literal5" runat="server" Text="<%$Resources:Tokens,Governrate %>"></asp:Literal></label>
                            <div>
                                <asp:DropDownList runat="server" ID="ddlfilterGovernrate"/>
                            </div></div></div>
                            <asp:LinkButton runat="server" ID="btnSearch"  
                                onclick="btnSearch_Click" CssClass="btn btn-success"><i class="icon-search"></i><asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Tokens,Search %>"></asp:Literal></asp:LinkButton>
                        <br/><br/>
                        <asp:GridView ID="grd_users" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="table table-bordered table-condensed text-center"
                            DataKeyNames="ID" OnRowDeleting="grd_users_RowDeleting" OnRowDataBound="grd_users_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,UserName %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserPhone" HeaderText="<%$ Resources:Tokens,Phone %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserEmail" HeaderText="<%$ Resources:Tokens,Email %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GroupName" HeaderText="<%$ Resources:Tokens,Group %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Governerate" HeaderText="<%$Resources:Tokens,Governrate %>"/>
                                <asp:BoundField DataField="AccountManager" HeaderText="<%$Resources:Tokens,ResellerAccountManager %>"/>
                                  <asp:BoundField DataField="IsAccountStopped" HeaderText="<%$ Resources:Tokens,Status %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,Edit %>">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-primary btn-sm" runat="server" CommandArgument='<%# Eval("ID") %>' data-rel="tooltip"
                                            ToolTip="<%$Resources:Tokens,Edit%>" OnClick="LinkButton1_Click"><i class="icon-edit icon-only bigger-125"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Tokens,Delete %>" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" CssClass="btn btn-danger btn-sm" runat="server" CausesValidation="False"
                                            CommandName="Delete" OnClientClick="return areyousure()" data-rel="tooltip"
                                            ToolTip="<%$Resources:Tokens,Delete%>"><i class="icon-trash icon-only bigger-125"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </div>
            </div>

    <script type="text/javascript">
        $(function () {
            $('#ctl00_ContentPlaceHolder1_chl_Branchs label').css({ display: "initial", padding: "5px" });
            $('#ctl00_ContentPlaceHolder1_chlSaves').css({ display: "initial", padding: "5px" });
        });
        function areyousure() {
            return confirm('<%= Tokens.AlertRUS%>');
        }
    </script>
</asp:Content>
