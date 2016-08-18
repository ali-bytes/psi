<%@ Page Title="<%$Resources:Tokens,AddReouter %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddRouter.aspx.cs" Inherits="NewIspNL.Pages.AddRouter" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header">
        <h1><asp:Label runat="server" Text="<%$Resources:Tokens,AddReouter %>"></asp:Label></h1>
    </div>
    <div runat="server" id="message" ></div>
    <div class="well">
        <asp:Label runat="server" Text="<%$Resources:Tokens,RouterSerial %>"></asp:Label>
        <div>
            <asp:TextBox runat="server" ID="txtRouterSerial"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRouterSerial" ID="rqu" 
            ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="ad"></asp:RequiredFieldValidator>
        </div>
                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,RouterType %>"></asp:Label>
        <div>
            <asp:TextBox runat="server" ID="txtRouterType"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRouterType" ID="RequiredFieldValidator1" 
            ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="ad"></asp:RequiredFieldValidator>
        </div>
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,AddStore %>"></asp:Label>
                    <div><asp:DropDownList runat="server" ID="ddlStores" ValidationGroup="company"/>
                    <asp:RequiredFieldValidator ControlToValidate="ddlStores" ValidationGroup="ad" ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
        <div>
            <br/>
            <asp:Button runat="server" ID="btnAddRouter" OnClick="BtnAddClick" ValidationGroup="ad" CssClass="btn btn-success" Text="<%$Resources:Tokens,Add %>"/>
        </div>
    </div>
    <a href="#addfromExcel" class="btn btn-sm btn-primary" data-toggle="collapse" title="<%=Tokens.addFromExcel%>"><i
                                                     class="icon-file"></i><%=Tokens.addFromExcel %></a>
    <div id="addfromExcel" class="collapse well">
        
     <asp:FileUpload runat="server" ID="f_sheet" multiple/>
                <asp:RequiredFieldValidator runat="server" ID="r_sheet" 
                                            ControlToValidate="f_sheet" ValidationGroup="fil" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                
          <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationGroup="fil" ValidationExpression="^.*\.(xls|XLS|xlsx|XLSX|)$" ControlToValidate="f_sheet"  ErrorMessage="<%$Resources:Tokens,uploadfilexcelerror %>"></asp:RegularExpressionValidator>
          <div class="center">
         <a href="../ExcelTemplates/AddRouters.xls" class="btn btn-default">
                        <i class="icon-cloud-download bigger-120"></i>
                        <%=Tokens.Downloadsample %>
                    </a>

        <asp:Button runat="server" ID="Button1" OnClick="BtnAddFromExcel" CssClass="btn btn-info" Text="<%$Resources:Tokens,Add %>"/>
              </div>
   <br/>
                <asp:Label runat="server" ID="l_message" Text=""></asp:Label>
         </div>
    <h3 class="header smaller lighter blue">
        <asp:Label runat="server" ID="lblres" Text="<%$Resources:Tokens,Results %>"></asp:Label>
    </h3>
    <div>
                            <asp:GridView ID="grdRouters" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        GridLines="None" CssClass="table table-bordered table-condensed text-center" OnRowDataBound="grdRouters_RowDataBound"
                        OnRowCancelingEdit="grd_RowCancelingEdit" OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing"
                                      OnRowUpdating="grd_RowUpdating" DataKeyNames="Id">
                        <Columns>
                           <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_No" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$Resources:Tokens,RouterSerial %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label31" runat="server" Text='<%# Bind("RouterSerial") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,RouterType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label32" runat="server" Text='<%# Bind("RouterType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$Resources:Tokens,AddStore %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label34" runat="server" Text='<%# Bind("StoreName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Done %>">
                                    <ItemTemplate>
                                        <asp:Label ID="Label33" runat="server" Text='<%# Bind("IsRecieved") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" data-rel="tooltip" CssClass="btn btn-xs btn-success" CommandName="Update" ToolTip="<%$Resources:Tokens,Update%>">
                                            <i class="icon-ok bigger-120"></i>
                                        </asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" data-rel="tooltip" runat="server" CausesValidation="False" CssClass="btn btn-xs btn-danger" CommandName="Cancel"
                                                              ToolTip="<%$Resources:Tokens,Cancel%>"><i class="icon-reply bigger-120"></i> </asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton3" data-rel="tooltip" CssClass="btn btn-xs btn-info" runat="server" CausesValidation="False" CommandName="Edit"
                                                        ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-edit bigger-120"></i></asp:LinkButton>
                                                         &nbsp;
                                                         <asp:LinkButton ID="BDel" data-rel="tooltip" CssClass="btn btn-xs btn-danger" runat="server" OnClick="BDel_OnClick" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm ('تاكيد حذف الراوتر؟');" ToolTip="<%$Resources:Tokens,Delete%>">
                                            <i class="icon-trash bigger-120"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Tokens,NoRequests %>"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
    </div>
    <br/>
     <hr/>
            <div>
                 <asp:GridView runat="server" ID="gv_errors" ondatabound="gv_errors_DataBound" 
                              AutoGenerateColumns="False"  
                               CellPadding="4" CssClass="table table-bordered table-condensed text-center"
                              GridLines="Vertical">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="gv_l_Number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="error" HeaderText="<%$ Resources:Tokens,SerialrouterError%>" />
                    </Columns>
                </asp:GridView>
            </div>
    <br/>
     <hr/>
            <div>
                 <asp:GridView runat="server" ID="gv_storeError"  
                              AutoGenerateColumns="False"  
                               CellPadding="4" CssClass="table table-bordered table-condensed text-center"
                              GridLines="Vertical" OnDataBound="gv_storeError_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="gv_2_Number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="error" HeaderText="<%$ Resources:Tokens,ErrorinStoreName%>" />
                    </Columns>
                </asp:GridView>
            </div>
</asp:Content>


