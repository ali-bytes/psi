<%@ Page Title="<%$Resources:Tokens,AddDisCard %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DisCard.aspx.cs" Inherits="NewIspNL.Pages.DisCard" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="page-header" >
                <h1><asp:Literal ID="Literal8" runat="server" Text="<%$Resources:Tokens,AddDisCard %>"></asp:Literal>
                </h1>
            </div>
      
                                            <div style="margin-right: 30px;">    <%=Tokens.Type%>     
                                                <asp:DropDownList ID="ddl_Governorates" CssClass="required-input" runat="server"
                                        DataTextField="GovernorateName" DataValueField="ID" Height="30px" Width="158px"  >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ControlToValidate="ddl_Governorates" ID="RequiredFieldValidator7"
                                        runat="server" ErrorMessage="*" ValidationGroup="Add"></asp:RequiredFieldValidator>
                </div> 
          <br/> 
     <div style="margin-right: 20px;">  <%=Tokens.Quantity%> <asp:TextBox ID="typevalue" runat="server"> </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Tokens,Required %>"                                                
          ControlToValidate="typevalue" ValidationGroup="Add"></asp:RequiredFieldValidator></div>
       <asp:FilteredTextBoxExtender ID="txt_CustomerPhone_FilteredTextBoxExtender" runat="server"
                                                FilterType="Numbers" TargetControlID="typevalue" Enabled="True">
                                            </asp:FilteredTextBoxExtender>
                <br/>
  <div style="margin-right: 110px;">  
      <asp:Button ID="Add" runat="server" 
          class="btn btn-primary" Text="<%$Resources:Tokens,Add %>" 
          ValidationGroup="Add" onclick="Add_Click"  /> 
           <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,addtype %>" Visible="False" ForeColor="green"></asp:Label>
  </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
  <br/>
    <div style="margin-right: 400px;">
        <%=Tokens.SearchBy%> 
                                                    <asp:DropDownList ID="searchtype" 
            CssClass="required-input" runat="server"
                                         Height="30px" Width="158px" 
            AutoPostBack="True" onselectedindexchanged="searchtype_SelectedIndexChanged"  >
                                    </asp:DropDownList>   <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Cardnum %>" Visible="False" ForeColor="green"></asp:Label><asp:TextBox ID="cardnum" AutoPostBack="True"
            runat="server" Visible="False" ontextchanged="cardnum_TextChanged"></asp:TextBox>
         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                FilterType="Numbers" TargetControlID="cardnum" Enabled="True">
                                            </asp:FilteredTextBoxExtender>
                                    </div>  <br/>
     <div dir="rtl" style="text-align: center; ">
                      <asp:GridView runat="server" ID="GvitemData" AutoGenerateColumns="False"  CssClass="table table-bordered table-condensed center" Width="100%" >
                        <Columns >
                              <asp:TemplateField HeaderText="#">
                            <ItemTemplate >
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,Cardnum %>" />
                         <asp:BoundField DataField="TypeName" HeaderText="<%$Resources:Tokens,distypename %>" />
                            <asp:BoundField DataField="Price" HeaderText="<%$Resources:Tokens,Value %>"  />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:Tokens,State %>"  />
                              <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>"  />
                               <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Customer.Phone %>"  />
                                <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,UserName %>"  />
                                 <asp:BoundField DataField="op_date" HeaderText="<%$Resources:Tokens,Date%>"  />
                             <%-- <asp:TemplateField HeaderText="<%$Resources:Tokens,Edit %>">
                               <ItemTemplate>
                                        <asp:LinkButton ID="ed" data-rel="tooltip" CssClass="btn btn-xs btn-info" runat="server" CausesValidation="False" 
                                                CommandArgument='<%# Eval("ID") %>'     ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-edit bigger-120"></i></asp:LinkButton>     
                                    </ItemTemplate>
                                          </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$Resources:Tokens,Delete %>">
                                            <ItemTemplate>
                                        <asp:LinkButton ID="BDel" data-rel="tooltip" CssClass="btn btn-xs btn-danger" runat="server"   CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm ('تاكيد حذف الفئة؟');" ToolTip="<%$Resources:Tokens,Delete%>">
                                            <i class="icon-trash bigger-120"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                      </asp:TemplateField>--%>

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


