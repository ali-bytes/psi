<%@ Page Title="<%$Resources:Tokens,ResellerDownloadInvoice%>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerDownloadInvoice.aspx.cs" Inherits="NewIspNL.Pages.ResellerDownloadInvoice" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="view" id="searchPanel">
        <fieldset>
            <div class="page-header"><h1>
                <%= (Tokens.ResellerDownloadInvoice)%></h1></div>
            <div class="row-fluid" runat="server" ID="Forsearch">
                <div class="span6">
                    <div class="well">
                        <div>
                            <label for="DdlReseller">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Reseller %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlReseller" runat="server" EnableViewState="True" />
                                <asp:RequiredFieldValidator ID="rrrr" ErrorMessage="*" ControlToValidate="DdlReseller"
                                    runat="server" />
                            </div>
                        </div>
                    <%--</div>
                </div>
                </div>
            <div class="row-fluid">
                <div class="span6">
                    <div class="well">--%>
                        <div>
                            <label for="DdlYear">
                                <%=Tokens.Year %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlYear" Width="157px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="DdlYear"
                                    runat="server" />
                            </div>
                        </div>
                        <div>
                            <label for="DdlMonth">
                                <%=Tokens.Month %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlMonth" Width="157px">
                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="*" ControlToValidate="DdlMonth"
                                    runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            
            <p>
                <asp:Button ID="BSearch" Text="<%$Resources:Tokens,Search %>" CssClass="btn btn-success" runat="server"
                            OnClick="SearchDemands" />&nbsp;
            <span runat="server" ID="Msg"></span>
            </p>
           
        </fieldset>
    </div>
    <div class="view">
        <fieldset>
            <h3 class="header smaller lighter blue">
                <asp:Literal ID="Literal2" Text="<%$Resources:Tokens,Results %>" runat="server" /></h3>
           
            <div>
                <asp:GridView runat="server" ID="GvResults" AutoGenerateColumns="False" CssClass="table table-bordered table-condensed text-center">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Tokens,Reseller %>" />
                        
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,InvoiceBeforeReview%>">
                            <ItemTemplate>
                                <a id="A1" runat="server" class="icon-download-alt bigger-130 icon-only" href='<%#Eval("FileNameBeforUrl") %>' target="_blank" title='<%$Resources:Tokens,Download %>' data-rel="tooltip">
                                    </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Tokens,InvoiceAfterReview %>">
                        <ItemTemplate>
                                &nbsp;
                            <a id="A2" runat="server" class="icon-download-alt bigger-125 icon-only" data-rel="tooltip" title='<%$Resources:Tokens,Download %>' href='<%#Eval("FileNameAfterUrl") %>' target="_blank">
                                </a><%--<%= Tokens.Download %>--%>
                                </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate><%=Tokens.NoResults%></EmptyDataTemplate>
                </asp:GridView>
            </div>
           <%-- <div>
                <div class="span3">
                    <asp:GridView ShowHeader="False" runat="server" ID="GvReport"  CssClass="table table-bordered table-hover table-condensed table-striped white"/>
                </div>
                <div class="span9">
                    <asp:Button Text="<%$Resources:Tokens,CreateInvoice %>" ID="btnCreatInvoice" CssClass="btn btn-primary" runat="server" OnClick="CreateInvoice"/>&nbsp;<button id="Button1" class="btn btn-success"  type="button" runat="server" OnServerClick="SearchAgain"><%=Tokens.BackToSearch %></button>
                </div>
            </div>--%>
            
        </fieldset>
    </div>
    <asp:HiddenField runat="server" ID="HfSerched" />

    
</asp:Content>


