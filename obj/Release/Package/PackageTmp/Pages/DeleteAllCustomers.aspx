<%@ Page Title="<%$Resources:Tokens,DeleteAllCustomers%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="DeleteAllCustomers.aspx.cs" Inherits="NewIspNL.Pages.DeleteAllCustomers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <br/>
        <br/><br/>
        <button runat="server" onserverclick="DeleteCustomers" class="btn btn-danger btn-block">
           <i class="icon-trash bigger-130"></i>&nbsp; <asp:Literal Text="<%$Resources:Tokens,DeleteAllCustomers %>" runat="server" /></button>
           <br/>
    </div>
    <div>
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
            <div>
                <asp:GridView runat="server" ID="GvUndeleted" AutoGenerateColumns="False" 
                    OnDataBound="GvUndeleted_OnDataBound" CssClass="table table-bordered table-condensed" CellPadding="4" GridLines="Horizontal">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="l_number" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$Resources:Tokens,Phone %>" />
                        
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    <p runat="server" id="msg">
    </p>
</asp:Content>
