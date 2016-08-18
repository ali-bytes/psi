<%@ Page Title="<%$Resources:Tokens,CreatInvoices %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CreatInvoices.aspx.cs" Inherits="NewIspNL.Pages.CreatInvoices" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header"><h1>        
            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Tokens,CreateInvoice %>"></asp:Label></h1></div>
    <div class="row">

        <div class="well">
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Tokens,MenuServiceProviders %>"></asp:Label>
            <div>
                <asp:CheckBoxList ClientIDMode="Static" Width="100%" runat="server" ID="providerlist"
                    RepeatDirection="Horizontal">
                </asp:CheckBoxList>
            </div>
        </div>
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
            <p>
                <br/>
                <asp:Button Text="<%$Resources:Tokens,CreateInvoice %>" ID="Button2" CssClass="btn btn-primary"
                    runat="server" OnClick="CreateInvoice" />&nbsp;
                <asp:Button ID="BSearch" Text="<%$Resources:Tokens,Export %>" CssClass="btn btn-success" runat="server"
                            OnClick="SearchDemands" /><div><span runat="server" id="Msg"></span></div>
            </p>
        </div>
                    <fieldset>
            <h3 class="header smaller lighter blue"><asp:Label ID="Label2" runat="server" Text="<%$Resources:Tokens,Resellerhavenotdata %>"></asp:Label></h3>
            <asp:GridView CssClass="table table-bordered table-condensed"
                              runat="server" ID="GVResellers" AutoGenerateColumns="False" OnDataBound="NumberGrid">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="UserName" />
                        </Columns>
                        </asp:GridView>
        </fieldset>
    </div>
    <div class="row" runat="server" id="resellerDiv" Visible="False">
                        <fieldset>
            <h3 class="header smaller lighter blue"><asp:Label ID="Label3" runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Label></h3>
            <asp:GridView CssClass="table table-bordered table-condensed"
                              runat="server" ID="GridView1" AutoGenerateColumns="False" OnDataBound="NumberGrid">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Branch %>" DataField="Branch" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="Total" />
                        </Columns>
                        </asp:GridView>
        </fieldset>
    </div>
    <%--<div class="view white" id="resultPanel">
        <fieldset>
            <legend>
                <asp:Literal ID="Literal1" Text="<%$Resources:Tokens,Results %>" runat="server" /></legend>
           
            <div>
                <asp:GridView CssClass="table table-bordered table-hover table-condensed table-striped"
                              runat="server" ID="GvResults" AutoGenerateColumns="False" OnDataBound="NumberGrid">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LNo" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Customer %>" DataField="Customer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Phone %>" DataField="Phone" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Status %>" DataField="Status" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Service.Package %>" DataField="servicepack" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Provider %>" DataField="Provider" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Reseller %>" DataField="Reseller" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Central %>" DataField="Central" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Governorate %>" DataField="Governorate" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Offer %>" DataField="Offer" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Paid %>" DataField="TPaid" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,From %>" DataField="TStartAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,To %>" DataField="TEndAt" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Notes %>" DataField="Notes" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Amount %>" DataField="TAmount" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,ResellerDiscount %>" DataField="TResellerDiscount" />
                        <asp:BoundField HeaderText="<%$Resources:Tokens,Net %>" DataField="TResellerNet" />
                       
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <div class="span3">
                    <asp:GridView ShowHeader="False" runat="server" ID="GvReport"  CssClass="table table-bordered table-hover table-condensed table-striped white"/>
                </div>
                <div class="span9">
                    <asp:Button Text="<%$Resources:Tokens,CreateInvoice %>" ID="btnCreatInvoice" CssClass="btn btn-primary" runat="server" OnClick="CreateInvoice"/>&nbsp;<button id="Button1" class="btn btn-success"  type="button" runat="server" OnServerClick="SearchAgain"><%=Tokens.BackToSearch %></button>&nbsp;<asp:Button runat="server" OnClick="btnExport_OnClick" CssClass="btn btn-danger" Text="<%$Resources:Tokens,Export %>" ID="btnExportToExcel"/>
                </div>
            </div>
           
        </fieldset>
    </div>--%>
</asp:Content>
