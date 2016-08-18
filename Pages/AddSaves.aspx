<%@ Page Title="<%$Resources:Tokens,Saves %>"  Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="AddSaves.aspx.cs" Inherits="NewIspNL.Pages.AddSaves" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Saves %>"></asp:Literal></h1></div>
            <div id="Msgsuccess" class="alert alert-success" Visible="False" runat="server">
                <asp:Literal runat="server" Text="<%$Resources:Tokens,Saved %>"></asp:Literal>
            </div>
            <div id="MsgError" runat="server" class="alert alert-danger" Visible="False">
                <asp:Literal runat="server" ID="lblerror" Text="<%$Resources:Tokens,ErrorMsg %>"></asp:Literal>
            </div>
            <div class="well">
                <asp:Literal runat="server" Text="<%$Resources:Tokens,Name %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" CssClass="text" ID="txtSaveName" ValidationGroup="save"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSaveName" ErrorMessage="*"
                    ValidationGroup="save"></asp:RequiredFieldValidator>
                </div>
                <asp:Literal runat="server" Text="<%$Resources:Tokens,Branches %>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlBranches" ValidationGroup="save" Width="177px"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBranches" ErrorMessage="*"
                   ValidationGroup="save"></asp:RequiredFieldValidator>
                </div>
               <asp:Literal runat="server" Text="<%$Resources:Tokens,Credit %>"></asp:Literal>
               <div>
                   <asp:TextBox runat="server" ID="txtSaveCredit" ValidationGroup="save"></asp:TextBox>
                   <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSaveCredit" ErrorMessage="*"
                   ValidationGroup="save"></asp:RequiredFieldValidator>
               </div>
               <br/>
               <asp:Button runat="server" ID="btnAdd" Text="<%$Resources:Tokens,Save %>" ValidationGroup="save"
               CssClass="btn btn-primary" OnClick="btnAdd_Click"/>
            </div>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <legend><asp:Literal runat="server" Text="<%$Resources:Tokens,Results%>"></asp:Literal></legend>
                                  <asp:GridView runat="server" ID="GvSaves" AutoGenerateColumns="False" DataKeyNames="Id"  OnDataBound="GvBox_OnDataBound"
                                      CssClass="table table-bordered  table-condensed"
                                      ClientIDMode="Static" OnRowCancelingEdit="grd_RowCancelingEdit" OnRowDeleting="grd_RowDeleting" OnRowEditing="grd_RowEditing"
                                      OnRowUpdating="grd_RowUpdating">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Name %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl1" Text='<%#Bind("SaveName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Branch %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl3" Text='<%#Bind("BranchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Credit %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl2" Text='<%#Bind("Total") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Edit %>" ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-xs btn-success" CommandName="Update" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Update%>"><i class="icon-ok bigger-120"></i></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CssClass="btn btn-xs btn-danger" CommandName="Cancel"
                                                    data-rel="tooltip" ToolTip="<%$Resources:Tokens,Cancel%>"><i class="icon-undo bigger-120"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                            data-rel="tooltip" CssClass="btn btn-xs btn-info"  ToolTip="<%$Resources:Tokens,Edit%>"><i class="icon-edit bigger-120"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Delete %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BDel" CssClass="btn btn-xs btn-danger" CommandName="Delete" runat="server"  OnClientClick="return confirm ('تاكيد حذف الخزنة؟');"
                                       data-rel="tooltip"  ToolTip="<%$Resources:Tokens,Delete%>"><i class="icon-trash bigger-120"></i></asp:LinkButton>
                                    </ItemTemplate><%--OnClick="BDel_OnClick" CommandArgument='<%# Eval("ID") %>'--%>
                                </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,NoResults %>"></asp:Literal>
                                </EmptyDataTemplate>
                                </asp:GridView>
        </fieldset>
    </div>
</asp:Content>

