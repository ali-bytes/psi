<%@ Page Title="<%$Resources:Tokens,UserSaves %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SavesBalance.aspx.cs" Inherits="NewIspNL.Pages.SavesBalance" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="view">
      
            <div class="page-header"><h1><asp:Literal runat="server" Text="<%$Resources:Tokens,UserSaves %>"></asp:Literal></h1></div>
           
    
    <asp:GridView runat="server" ID="GvSaves" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed" OnDataBound="GvBox_OnDataBound" ClientIDMode="Static">
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
                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Credit %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl2" Text='<%#Bind("Total") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Literal runat="server" Text="<%$Resources:Tokens,NoResults %>"></asp:Literal>
                                </EmptyDataTemplate>
                                </asp:GridView>
        <div id="total" runat="server" class="alert alert-info" style="text-align: center"><span style="color: red">Total : </span></div>
        </div>
</asp:Content>
