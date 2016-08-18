<%@ Page Title="<%$Resources:Tokens,Emptranv%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="EmployeAssemplies.aspx.cs" Inherits="NewIspNL.Pages.EmployeAssemplies" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content">
      
                <asp:TextBox ID="txtError" runat="server" CssClass="error_box" Width="534px" 
                        Visible="False" ReadOnly="True" ></asp:TextBox>
          
        <div class="alert alert-success" style="display: <%=l_message.Text==string.Empty?"none;":"block;"%>">
            <asp:Label runat="server" ID="l_message" ClientIDMode="Static"></asp:Label>
        </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="v_index" runat="server">
                <fieldset>
                    <legend><%=Tokens.Emptranv%></legend>

                    <div>
                        <div>
                            <p>
                                <asp:Button ID="b_new" runat="server" Text="<%$Resources:Tokens,Add%>" OnClick="b_new_Click" CssClass="btn btn-primary" />
                            </p>
                            <asp:GridView style="text-align: center"  CssClass="table table-bordered table-hover" ID="GvItems" runat="server"
                                AutoGenerateColumns="False" OnDataBound="gv_index_DataBound" ClientIDMode="Static"
                                GridLines="None">
                                <AlternatingRowStyle BorderStyle="None" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="l_number" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Employee%>" DataField="Name"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Type%>" DataField="Type"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$Resources:Tokens,Value%>" DataField="Increase"></asp:BoundField>
                                    <asp:BoundField DataField="Time" HeaderText="<%$Resources:Tokens,Date%>" DataFormatString="{0:M-dd-yyyy}" />
                                    <asp:TemplateField HeaderText="<%$Resources:Tokens,update%>" >
                                        <ItemTemplate >
                                            <asp:Button ID="gvb_edit" runat="server" CommandArgument='<%#Bind("Id") %>' OnClick="gvb_edit_Click"
                                                Text="<%$Resources:Tokens,Update%>" CssClass="btn btn-primary" />
                                             </ItemTemplate>
                                    </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Tokens,Delete%>" >
                                        <ItemTemplate >
                                            <asp:Button ID="gvb_delete" runat="server" CommandArgument='<%# Bind("Id") %>' Text="<%$Resources:Tokens,Delete%>"
                                             OnClientClick="return confirm('هل تريد الحذف ؟');"   OnClick="gvb_delete_Click" CssClass="btn btn-danger" />
                                            <%--Visible='<%# Bind("CanDelete") %>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BorderStyle="None" />
                                <PagerStyle BorderStyle="None" />
                                <RowStyle BorderStyle="None" />
                            </asp:GridView>
                        </div>
                    </div>
                </fieldset>
            </asp:View>
            <asp:View ID="v_AddEdit" runat="server">
                <div>
                    <fieldset>
                        <legend><%=Tokens.Emptranv%></legend>
                        <asp:HiddenField ID="hf_id" runat="server" />
                        <div class="well">
                            <div>
                                <label for="tb_name">
                                    <%=Tokens.Name%>
                                </label>
                                <div>
                                    <asp:DropDownList runat="server" ID="tb_name" />
                                    <asp:RequiredFieldValidator runat="server" ID="Rtb_name" ControlToValidate="tb_name"
                                        ErrorMessage="*" CssClass="validation">*</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div>
                                <label for="TbType">
                                    <%=Tokens.Type%></label>
                                <div>
                                    <asp:DropDownList runat="server" ID="TbType">
                                        <asp:ListItem Text="<%$Resources:Tokens,install%> " Value="2" />
                                        <asp:ListItem Text="<%$Resources:Tokens,trav%> " Value="1" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RTbType" ControlToValidate="TbType"
                                        ErrorMessage="*" CssClass="validation">*</asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div>
                                <label for="TbIncrease">
                                    <%=Tokens.Value%></label>
                                <div>
                                    <asp:TextBox runat="server" ID="TbIncrease" ClientIDMode="Static"  type="number" />
                                    <asp:RequiredFieldValidator ID="RTbIncrease" ErrorMessage="*" ControlToValidate="TbIncrease" ValidationGroup="b"
                                        runat="server" />
                                </div>
                            </div>

                            <br/>
                            <p>
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="BSave" Text="<%$Resources:Tokens,save%>" OnClick="b_save_Click" ValidationGroup="b">
                                    <%--  <i class="icon-white icon-ok"></i>&nbsp;حفظ--%></asp:Button>
                                <span>| </span>
                                <asp:Button runat="server" ID="BBack" CssClass="btn btn-success" CausesValidation="False"
                                    OnClick="BBack_OnClick" Text="<%$Resources:Tokens,Cancel%>" />
                            </p>
                        </div>
                    </fieldset>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>

</asp:Content>

