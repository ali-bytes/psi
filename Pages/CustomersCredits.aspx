<%@ Page Title="<%$Resources:Tokens,cuscredit %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="CustomersCredits.aspx.cs" Inherits="NewIspNL.Pages.CustomersCredits" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div class="modal-content">
                <div class="modal-header">
    <%=Tokens.cuscredit %>
  </div>
          </div>
        
        
              <div style="width: 100%">
                    <asp:GridView runat="server" ID="GvCustomerData" AutoGenerateColumns="False" OnDataBound="payhis_OnDataBound" CssClass="table table-bordered table-condensed center" Font-Size="12px">
                        <Columns>
                             <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="<%$Resources:Tokens,Customer.Name%>" />
                            <asp:BoundField DataField="CustomerPhone" HeaderText="<%$Resources:Tokens,Phone%>" />
                            <asp:BoundField DataField="StatusName" HeaderText="<%$Resources:Tokens,State%>" />
                            <asp:BoundField DataField="ServicePackageName" HeaderText="<%$Resources:Tokens,Package%>" />
                            <asp:BoundField DataField="BranchName" HeaderText="<%$Resources:Tokens,Branch%>" />
                            <asp:BoundField DataField="SPName" HeaderText="<%$Resources:Tokens,Service.Provider %>"/>
                            <asp:BoundField DataField="OfferTitle" HeaderText="<%$Resources:Tokens,Offer%>" />
                            <asp:BoundField DataField="CentralName" HeaderText="<%$Resources:Tokens,Central%>" />
                            <asp:BoundField DataField="CreditAmount" HeaderText="<%$Resources:Tokens,Amount%>" />

                               </Columns>
                        <EmptyDataTemplate>
                            <div class="alert">
                                <%= Tokens.NoResults %>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
        
</asp:Content>

