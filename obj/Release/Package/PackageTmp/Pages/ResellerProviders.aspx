<%@ Page Title="<%$Resources:Tokens,ResellerProviders%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerProviders.aspx.cs" Inherits="NewIspNL.Pages.ResellerProviders" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">#GvResults { width: 100%; }#GvResults td, #GvResults th { text-align: center; }</style>
    <div>
        <div class="view">
            <fieldset>
                <div class="page-header"><h1>
                    <asp:Literal Text="<%$Resources:Tokens,ResellerProviders %>" runat="server" /></h1></div>
                <div class="well">
                    
                        <label><asp:Label Text="<%$Resources:Tokens,Reseller %>" runat="server" AssociatedControlID="DdlResellers" /></label><div>
                        <asp:DropDownList runat="server" ID="DdlResellers" ValidationGroup="x">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ErrorMessage="<%$Resources:Tokens,Required %>" ControlToValidate="DdlResellers"
                                                    runat="server" ValidationGroup="x" />
                    </div>
                    <p>
                        <br/>
                        <button runat="server" onserverclick="Search" validationgroup="x" class="btn btn-success">
                           <i class="icon-search icon-only"></i>&nbsp; <asp:Literal Text="<%$Resources:Tokens,Search %>" runat="server" /></button>
                    </p>
                </div>
            </fieldset>
        </div>
        <div class="view">
            <fieldset>
                <h3 class="header smaller lighter blue">
                    <asp:Literal Text="<%$Resources:Tokens,Provider %>" runat="server" /></h3>
                <div>
                    <asp:GridView runat="server" ID="GvResults" AutoGenerateColumns="False" ClientIDMode="Static"
                    CssClass="table table-bordered table-condensed"
                                  OnDataBound="UpdateNumbers"  GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:Literal ID="LNo" runat="server"></asp:Literal>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Provider %>" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="chec" type="checkbox" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hfId" Value='<%#Eval("ID") %>' />
                                    <asp:CheckBox data-select="cb" runat="server" ID="CbProvider" Checked='<%#Eval("IsChecked") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <p>
                        <button runat="server" onserverclick="Save" validationgroup="x" class="btn btn-primary">
                           <i class="icon-save icon-only"></i>&nbsp; <asp:Literal Text="<%$Resources:Tokens,Save %>" runat="server" /></button>
                    </p>
                    <div runat="server" id="Msg">
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
    <script src="../Content/app/reselerProv.js" type="text/javascript"> </script>
</asp:Content>