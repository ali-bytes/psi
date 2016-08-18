<%@ Page Title="<%$Resources:Tokens,NumberAttachements%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="NumberAttchments.aspx.cs" Inherits="NewIspNL.Pages.NumberAttchments" %>


<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="view">
        <fieldset>
            <div class="page-header"><h1>
                <%= Tokens.NumberAttachements %></h1></div>
            <div class="well">
                <div id="Msg" runat="server">
                </div>
                <div style="width: 33%">
                    <label for="TbPhone">
                        <asp:Literal Text="<%$Resources:Tokens,ParamterOfSearch %>" runat="server" />
                    </label>
                    <div>
                        <span class="input-icon" >
                        <asp:TextBox runat="server" ID="TbPhone"  ClientIDMode="Static"></asp:TextBox><i class="icon-search"></i></span>
                        <asp:RequiredFieldValidator ID="RTbPhone" ErrorMessage="<%$Resources:Tokens,Required %>"
                            ControlToValidate="TbPhone" runat="server" />
                    </div>
                </div>
                <div style="width: 33%">
                    <label for="TbUpload">
                        <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Upload %>" runat="server" />
                    </label>
                    <div >
                        <asp:FileUpload runat="server" ID="TbUpload" ClientIDMode="Static"></asp:FileUpload>
                         <asp:RegularExpressionValidator runat="server" ForeColor="red" ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|png|PNG|xls|XLS|xlsx|XLSX|pdf|PDF|doc|DOC|docx|DOCX)$" ControlToValidate="TbUpload"  ErrorMessage="<%$Resources:Tokens,uploadimgerror %> "></asp:RegularExpressionValidator>
                   
                    </div>
                </div>
            </div>
            <button class="btn btn-success" runat="server" id="BSearch">
                <i class="icon-search icon-only"></i>&nbsp;
                <%= Tokens.Search %>
            </button>
            &nbsp;
            <button class="btn btn-primary" runat="server" id="BAdd">
                <i class="icon-plus-sign icon-only"></i>&nbsp;
                <%= Tokens.Add %>
            </button>
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <%= Tokens.Results %></h3>
            <div>
                <asp:GridView runat="server" ID="GvResults" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:BoundField DataField="AttachmentName" HeaderText="<%$Resources:Tokens,Name %>" />
                        <asp:BoundField DataField="Number" HeaderText="<%$Resources:Tokens,Phone %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a runat="server" class="btn btn-success btn-sm" href='<%#Eval("Url") %>' target="_blank" title='<%$Resources:Tokens,Download%>' data-rel="tooltip">
                                    <i class="icon-cloud-download icon-only"></i></a>&nbsp;
                                    <asp:LinkButton runat="server" ID="btndelete" CssClass="btn btn-danger btn-sm" data-rel="tooltip" ToolTip="<%$Resources:Tokens,Delete %>" CommandArgument='<%#Eval("Id") %>' OnClick="Delete">
                                        <i class="icon-trash icon-only"></i>
                                    </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
</asp:Content>
