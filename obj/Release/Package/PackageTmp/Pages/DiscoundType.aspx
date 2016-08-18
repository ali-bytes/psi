<%@ Page Title="<%$Resources:Tokens,DisType %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DiscoundType.aspx.cs" Inherits="NewIspNL.Pages.DiscoundType" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <div class="page-header" >
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,DisType %>"></asp:Literal>
                </h1>
            </div>
      <%=Tokens.distypename%> <asp:TextBox ID="typename" runat="server"> </asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"                                             
          ControlToValidate="typename" ValidationGroup="Add"></asp:RequiredFieldValidator>
          <br/> <br/>
     <div style="margin-right: 20px;">  <%=Tokens.Value%> <asp:TextBox ID="typevalue" runat="server"> </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"                                                
          ControlToValidate="typevalue" ValidationGroup="Add"></asp:RequiredFieldValidator></div>
       <asp:FilteredTextBoxExtender ID="txt_CustomerPhone_FilteredTextBoxExtender" runat="server"
                                                FilterType="Numbers, Custom" ValidChars="." TargetControlID="typevalue" Enabled="True">
                                            </asp:FilteredTextBoxExtender>
                <br/>
  <div style="margin-right: 110px;">  <asp:Button ID="Add" runat="server" 
          class="btn btn-primary" Text="<%$Resources:Tokens,Add %>" 
          ValidationGroup="Add" onclick="Add_Click" /> 
          <asp:Button ID="Button1" runat="server" Visible="False"
          class="btn btn-primary" Text="<%$Resources:Tokens,Edit %>" 
          ValidationGroup="Add" onclick="Button1_Click" />
       <asp:Button ID="cancel" runat="server" Visible="False"
          class="btn btn-danger" Text="<%$Resources:Tokens,Cancel %>" 
          ValidationGroup="Add" onclick="cancelbtn" />
           <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,AddedSuccessfully %>" Visible="False" ForeColor="green"></asp:Label>
  </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
     <div dir="rtl" style="text-align: center; ">
                      <asp:GridView runat="server" ID="GvitemData" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                              <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="TypeName" HeaderText="<%$Resources:Tokens,distypename %>"/>
                            <asp:BoundField DataField="Price" HeaderText="<%$Resources:Tokens,Value %>" />
                              <asp:TemplateField HeaderText="<%$Resources:Tokens,Edit %>">
                                    
                               <ItemTemplate>
                                        <asp:LinkButton ID="ed" data-rel="tooltip" CssClass="btn btn-xs btn-info" runat="server" CausesValidation="False" 
                                            OnClick="Ed"     CommandArgument='<%# Eval("ID") %>'     ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-edit bigger-120"></i></asp:LinkButton>
                                                      
                                                      
                                    </ItemTemplate>
                                    
                                    
                                          </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$Resources:Tokens,Delete %>">
                                            <ItemTemplate>
                                        <asp:LinkButton ID="BDel" data-rel="tooltip" CssClass="btn btn-xs btn-danger" runat="server" OnClick="Delete"  CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد حذف الفئة؟');" ToolTip="<%$Resources:Tokens,Delete%>">
                                            <i class="icon-trash bigger-120"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                      </asp:TemplateField>
                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    </div>
                    
                    
                    
         <%--           
                       <div id="multi" aria-hidden="true"  role="dialog" class="bootbox modal fade" style="margin-top: 400px;margin-right: 500px;">
                <%=Tokens.distypename%> <asp:TextBox ID="TextBox1" runat="server"> </asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"                                             
          ControlToValidate="TextBox1" ValidationGroup="edit"></asp:RequiredFieldValidator>
          <br/> <br/>
     <div style="margin-right: 33px;">  <%=Tokens.Value%> <asp:TextBox ID="TextBox2" runat="server"> </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"                                                
          ControlToValidate="TextBox2" ValidationGroup="edit"></asp:RequiredFieldValidator></div>
       
                <br/>
  <div style="margin-right: 110px;">  <asp:Button ID="Button1" runat="server" 
          class="btn btn-primary" Text="<%$Resources:Tokens,Edit %>" 
          ValidationGroup="edit" onclick="Add_Click" /> 
  </div>
            </div>--%>
</asp:Content>

