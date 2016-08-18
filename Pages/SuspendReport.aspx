<%@ Page Title="<%$Resources:Tokens,SuspendReport %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="SuspendReport.aspx.cs" Inherits="NewIspNL.Pages.SuspendReport" %>



<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-header">
        <h1>
            <asp:Literal runat="server" Text="<%$Resources:Tokens,SuspendReport %>"></asp:Literal>
        </h1>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <div class="well">
                                    <div>
                            <label for="DdlReseller">
                                <%= Tokens.Reseller %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlReseller" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlBranch">
                                <%= Tokens.Branch %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlBranch" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div>
                            <label for="DdlSeviceProvider">
                                <%=Tokens.Service_Provider %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlSeviceProvider" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                        </div>
        </div></div>
        <div class="col-sm-6">
            <div class="well">
                                        <div>
                            <label for="DdlYear">
                                <%=Tokens.Year %>
                            </label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlYear">
                                </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlYear"
                              ValidationGroup="se"      ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label for="DdlMonth">
                                <%=Tokens.Month %></label>
                            <div>
                                <asp:DropDownList runat="server" ID="DdlMonth">
                                </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlMonth"
                              ValidationGroup="se"      ErrorMessage="<%$Resources:Tokens,Required %>"></asp:RequiredFieldValidator>
                            </div>
                        </div>
            </div>
        </div>
      <div class="col-sm-12">  <asp:Button runat="server" ID="btnSearch"  ValidationGroup="se"    CssClass="btn btn-success" Text="<%$Resources:Tokens,Search %>" OnClick="Search"/></div>
      
      <div class="col-sm-12">
                     <h3 class="header smaller lighter blue"><asp:Literal runat="server" Text="<%$Resources:Tokens,Results %>"></asp:Literal></h3>
                          <asp:GridView ID="grd_Requests" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                    CssClass="table table-bordered table-condensed text-center" OnRowDataBound="grd_Requests_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Label ID="lbl_No" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderText="<%$ Resources:Tokens,Customer %>" />
                        <asp:BoundField DataField="CustomerPhone" HeaderText="<%$ Resources:Tokens,Phone %>"/>
                        <asp:BoundField DataField="GovernorateName" HeaderText="<%$ Resources:Tokens,Governrate %>" />
                        <asp:BoundField DataField="Central" HeaderText="<%$ Resources:Tokens,Central%>" />
                        <asp:BoundField DataField="CurrentServicePackageName" HeaderText="<%$ Resources:Tokens,CurrentSpeed %>"/>
                        <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:Tokens,Status %>"/>
                        <asp:BoundField DataField="SuspenDaysCount" HeaderText="<%$ Resources:Tokens,SuspendDaysCount %>"/>
                        <asp:BoundField DataField="SPName" HeaderText="<%$ Resources:Tokens,Provider %>"/>
                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:Tokens,Reseller %>"/>
                         <asp:BoundField DataField="BranchName" HeaderText="<%$ Resources:Tokens,Branch %>" />
                        <asp:BoundField DataField="TActivationDate" HeaderText="<%$ Resources:Tokens,Activation.Date %>" />
                        <asp:BoundField HeaderText="<%$ Resources:Tokens,Offer%>" DataField="Title" />
                        <asp:BoundField DataField="IpPackageName" HeaderText="<%$ Resources:Tokens,Ip.Package%>"/>
                        <asp:BoundField DataField="TRequestDate" HeaderText="<%$ Resources:Tokens,Request.Date %>"/>
                        <asp:BoundField DataField="PaymentType" HeaderText="<%$Resources:Tokens,PaymentType %>"/>
                        </Columns>
                        </asp:GridView>
      </div>
    </div>
    
</asp:Content>


