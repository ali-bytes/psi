<%@ Page Title="<%$Resources:Tokens,BranchesInvoices %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="BranchesInvoices.aspx.cs" Inherits="NewIspNL.Pages.BranchesInvoices" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="view" id="searchPanel">
        <fieldset>
            <div class="page-header">
                <h1><asp:Literal runat="server" Text="<%$Resources:Tokens,BranchesInvoices %>"></asp:Literal><%--%= Tokens.Export + " " + Tokens.UnpaidBranchDemands%>--%></h1></div>
            <div class="col-sm-8" id="Forsearch">
                <div class="span6">
                    <div class="well">
                        <div>
<%--                            <label for="DdlReseller">
                                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Branch %>" runat="server" />
                            </label>
                            <div>
                                <asp:DropDownList ID="DdlBranch" runat="server" EnableViewState="True" />
                                <asp:RequiredFieldValidator ID="rrrr" ErrorMessage="*" ControlToValidate="DdlBranch"
                                    runat="server" />
                            </div>
                            <br/>--%>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,MenuServiceProviders %>"></asp:Label>
                            <div>
                                                                <asp:CheckBoxList ClientIDMode="Static" runat="server" ID="providerlist" 
                                                                    RepeatDirection="Horizontal" Width="100%">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="span6">
                    <div class="well">
                        <div>
                            <label for="DdlYear">
                                <%=Tokens.Year %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlYear">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlMonth">
                                <%=Tokens.Month %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlMonth">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <p>
                <asp:Button ID="BSearch" Text="<%$Resources:Tokens,Export %>" CssClass="btn btn-success"
                    runat="server" OnClick="SearchDemands" />&nbsp;<span runat="server" id="Msg"></span>
            </p>
        </fieldset>
    </div>
    <div class="row" runat="server" id="resellerDiv" Visible="False">
                        <fieldset>
            <h3 class="header smaller lighter blue"><asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Label></h3>
            <asp:GridView CssClass="table table-bordered table-condensed"
                              runat="server" ID="GridView1" AutoGenerateColumns="False" OnDataBound="NumberGrid" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Branch %>" DataField="Branch" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="Total" />
                        </Columns>
                        </asp:GridView>
        </fieldset>
    </div>
</asp:Content>

