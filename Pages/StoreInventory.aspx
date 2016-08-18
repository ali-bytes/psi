<%@ Page Title="<%$Resources:Tokens,StoreInventory %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="StoreInventory.aspx.cs" Inherits="NewIspNL.Pages.StoreInventory" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header"><h1><asp:Literal runat="server" ID="lblTitle" Text="<%$Resources:Tokens,Search %>"></asp:Literal></h1></div>
    <div class="well">
        <asp:Literal runat="server" ID="lblstores" Text="<%$Resources:Tokens,Store %>"></asp:Literal>
        <div>
            <asp:DropDownList runat="server" ID="ddlStore" DataTextField="StoreName" 
                        DataValueField="Id" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlStore" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="sea"></asp:RequiredFieldValidator>
        </div><br/>
        <asp:Button runat="server" ID="btnSearch" Text="<%$Resources:Tokens,Search %>" CssClass="btn btn-success" ValidationGroup="sea" OnClick="Search"/>
    </div>
    
            <asp:GridView runat="server" ID="GvSearch" ClientIDMode="Static" 
        AutoGenerateColumns="False" 
        CssClass="table table-bordered table-condensed text-center col-md-12 col-sm-12" 
        ondatabound="GvSearch_DataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Store %>">
                            <ItemTemplate>
                                <asp:Label ID="lblStore" runat="server" Text='<%#Eval("StoreName") %>' ToolTip='<%#Eval("StoreId") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Itemes %>">
                            <ItemTemplate>
                                <asp:Label ID="lblItem" runat="server" Text='<%#Eval("ItemName") %>' ToolTip='<%#Eval("ItemId") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$Resources:Tokens,Quantity %>">
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        </Columns>
                        </asp:GridView>
    <br />
     <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="<%$ Resources:Tokens,Export %>"
                    Width="150px" CssClass="btn btn-primary"/>

</asp:Content>


