<%@ Page Title="<%$Resources:Tokens,Purchases %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="Purchases.aspx.cs" Inherits="NewIspNL.Pages.Purchases" %>


<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">     
<style type="text/css">
    #btnAddAllInvoices {
        margin-bottom: 10px;
    }
</style>
    <div class="page-header">
        <h1><asp:Literal runat="server" Text="<%$Resources:Tokens,Purchases %>"></asp:Literal></h1>
    </div>
    
    <div class="row">

<div class="col-md-12 well">
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblNumber" Text="<%$Resources:Tokens,InvoiceNumber %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtInvoiceNumber"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtInvoiceNumber" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                <asp:Literal runat="server" ID="lblsupplier" Text="<%$Resources:Tokens,Suppliers %>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlSuppliers" DataTextField="SupplierName" DataValueField="Id"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSuppliers" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal7" Text="<%$Resources:Tokens,UserSaves %>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlSaves" DataTextField="SaveName" DataValueField="Id"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlSaves" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblDate" Text="<%$Resources:Tokens,Date %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtDate" data-x="dt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDate" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                <asp:Literal runat="server" ID="lblStore" Text="<%$Resources:Tokens,Store %>"></asp:Literal>
                <div>
                    <asp:DropDownList runat="server" ID="ddlStore" DataTextField="StoreName" 
                        DataValueField="Id" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlStore" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
           <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
        <div class="col-md-12 well">
            <div class="col-md-6" style="margin-bottom: 10px;">
                 <asp:Literal ID="Literal8"  runat="server" Text="<%$Resources:Tokens,Code %>"></asp:Literal>
           
                   <div>
                    <asp:TextBox OnTextChanged="GetItem" AutoPostBack="True" runat="server" ID="Txtcode" CssClass="required-input"></asp:TextBox>
                    </div>
            <asp:Literal runat="server" ID="lblItems" Text="<%$Resources:Tokens,ItemName %>"></asp:Literal>
            <div>
                <asp:DropDownList runat="server" ID="ddlItems" DataTextField="ItemName" DataValueField="Id" AutoPostBack="True"
                onselectedindexchanged="ddlItem_SelectedIndexChanged"/>
            </div>
            <asp:Literal runat="server" ID="lblQuantity" Text="<%$Resources:Tokens,Quantity %>"></asp:Literal>
            <div>
                <asp:TextBox runat="server" ID="txtQuantity"></asp:TextBox>
            </div>
            </div>
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblPrice" Text="<%$Resources:Tokens,Price %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtPrice"></asp:TextBox>
                </div>
            </div>
            <br/>
           <div class="col-md-12">
               <asp:Button runat="server" ID="btnAddoneInvoice" Text="<%$Resources:Tokens,Save %>" CssClass="btn btn-success" OnClick="AddOneInvoice"/>
           </div>
        </div>
        <asp:GridView runat="server" ID="GvInvoice" ClientIDMode="Static" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center col-md-12 col-sm-12">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,ItemName %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblItemName" Text='<%#Eval("ItemName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Quantity %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblQuantity" Text='<%#Eval("Quantity") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$Resources:Tokens,Price %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPrice" Text='<%#Eval("Price") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ValidationGroup='<%#Eval("ItemId") %>'  data-rel="tooltip" 
                                        ToolTip="<%$Resources:Tokens,Delete %>" OnClick="DeleteInvoice" ID="BDelete" CssClass="btn btn-xs btn-danger"><i class="icon-trash bigger-120"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
        <div class="col-md-12 well" >
            <div class="col-md-6">
                <asp:Literal runat="server" ID="lblTotal" Text="<%$Resources:Tokens,Total %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtTotal"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rquTotal" ControlToValidate="txtTotal" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal2" Text="<%$Resources:Tokens,DiscountPercent %>"></asp:Literal>
                <div>
                    
                    <asp:TextBox runat="server" ID="txtDiscoundPercent" 
                        OnTextChanged="txtDiscoundPercent_TextChanged" AutoPostBack="True"></asp:TextBox>&nbsp;<i>%</i>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDiscoundPercent" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal3" Text="<%$Resources:Tokens,Discount %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtDiscound"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDiscound" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-6">
                <asp:Literal runat="server" ID="Literal4" Text="<%$Resources:Tokens,Net %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtNet"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtNet" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal5" Text="<%$Resources:Tokens,Paid %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtPaid" OnTextChanged="txtPaid_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtPaid" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
                                <asp:Literal runat="server" ID="Literal6" Text="<%$Resources:Tokens,Remaining %>"></asp:Literal>
                <div>
                    <asp:TextBox runat="server" ID="txtRemaining"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtRemaining" ErrorMessage="<%$Resources:Tokens,Required %>" ValidationGroup="allinvoice"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:Literal runat="server" ID="lblNotes" Text="<%$Resources:Tokens,Notes %>"></asp:Literal>
            <div>
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNotes" Width="100%"></asp:TextBox>
            </div>
        </div>
         
                       

                        <asp:LinkButton runat="server" ValidationGroup="allinvoice" 
            ID="btnAddAllInvoices" CssClass="btn btn-primary btn-block" ClientIDMode="Static" 
            onclick="btnAddAllInvoices_Click"><i class="icon-ok"></i>&nbsp;<%=Tokens.Save %></asp:LinkButton>     
            <br/>                                                           <div class="alert alert-success" runat="server" id="SuccDiv" Visible="False">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Tokens,ProcessDone %>"></asp:Literal>
                        </div>
                                                <div class="alert alert-danger" runat="server" id="errorDiv" Visible="False">
                            <asp:Literal ID="lblerror" runat="server" Text="<%$Resources:Tokens,ErrorMsg %>"></asp:Literal>
                        </div>
                        
        </ContentTemplate>
        </asp:UpdatePanel></div>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[data-x="dt"]').datepicker({
                dateFormat: 'dd/mm/yy'
            });
           
            $('#footerdiv').css({ "position": "inherit" });

        }); </script>
</asp:Content>


