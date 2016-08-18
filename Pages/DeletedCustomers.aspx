<%@ Page Title="<%$ Resources:Tokens,DeletedCustomersReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DeletedCustomers.aspx.cs" Inherits="NewIspNL.Pages.DeletedCustomers" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
     <div class="page-header"><h1><asp:Literal runat="server" ID="lblindex" Text="<%$ Resources:Tokens,DeletedCustomersReport %>"></asp:Literal></h1></div>
           
         <asp:GridView ID="gv_customers"  runat="server" AutoGenerateColumns="False" ClientIDMode="Static" CssClass="table table-bordered table-condensed text-center" OnRowDataBound="gv_customers_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="gv_l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:Tokens,Phone%>" />
                        <asp:BoundField DataField="User" HeaderText="<%$ Resources:Tokens,User %>" />
                        <asp:BoundField DataField="DeleteDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Tokens,DeleteDate %>" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="color: blue; font-weight: bold;">
                            <asp:Literal Text="<%$ Resources:Tokens,NoResults %>" runat="server" /></div>
                    </EmptyDataTemplate>
                </asp:GridView>
    
</asp:Content>
