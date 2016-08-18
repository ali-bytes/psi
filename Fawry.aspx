<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Fawry.aspx.cs" Inherits="NewIspNL.Fawry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
        <br/>
        <asp:Button runat="server" ID="btnSearch" Text="Search" 
            onclick="btnSearch_Click"/>
    </div>
    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                            DataKeyNames="ID" CssClass="table table-bordered table-condensed text-center">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_No" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="<%$Resources:Tokens,Serial %>"/>
                                <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name %>"/>
                                <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Customer.Phone %>"/>
                                <asp:BoundField DataField="Amount" HeaderText="<%$Resources:Tokens,Amount %>"/>
                                <asp:BoundField DataField="StartAt" HeaderText="<%$Resources:Tokens,From %>"/>
                                <asp:BoundField DataField="EndAt" HeaderText="<%$Resources:Tokens,To%>"/>
                                <asp:BoundField DataField="Paid" HeaderText="<%$Resources:Tokens,Paid %>"/>
                                <asp:BoundField DataField="Id" HeaderText="<%$Resources:Tokens,demands %>"/>

                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="<%$ Resources:Tokens,NoResults %>"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
</asp:Content>
