<%@ Page Title="<%$ Resources:Tokens,ResellersRoutersCredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellersRoutersCredit.aspx.cs" Inherits="NewIspNL.Pages.ResellersRoutersCredit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        #GResellersStocks {
            width: 500px;
            text-align: center;
        }
    </style>
    <div class="view">
        <fieldset>
        <div class="page-header"><h1>
                <asp:Literal Text="<%$ Resources:Tokens,Stocks %>" runat="server" /></h1>
        </div>
        <section>

                <div>
                    <asp:GridView runat="server" ID="GResellersStocks" ClientIDMode="Static" AutoGenerateColumns="False"
                        OnDataBound="GResellersStocks_OnDataBound" 
                        CssClass="table table-bordered table-condensed">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <asp:Label ID="LNo" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Tokens,Name %>" />
                            <asp:BoundField DataField="Stock" HeaderText="<%$ Resources:Tokens,Stock%>" />
                        </Columns>
                    </asp:GridView>
                </div>
        </section>
        </fieldset>
    </div>
</asp:Content>

