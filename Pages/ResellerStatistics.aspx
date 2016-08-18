<%@ Page Title="<%$Resources:Tokens,CustomerStatisticsForReseller %>" Language="C#" MasterPageFile="~/Pages/AwesomeMaster.Master" AutoEventWireup="true" CodeBehind="ResellerStatistics.aspx.cs" Inherits="NewIspNL.Pages.ResellerStatistics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="view">
         <div class="page-header"><h1>
                    <asp:Label ID="Label38" runat="server" 
                               Text="<%$ Resources:Tokens,CustomerStatisticsForReseller %>"></asp:Label></h1>
                </div>
         
          <asp:GridView CssClass="table table-bordered table-condensed center" ID="grd_Statistics" runat="server" OnDataBound="GvBox_OnDataBound" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="LNo" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:BoundField DataField="ResellerName" HeaderText="<%$ Resources:Tokens,Reseller %>"/>
                            <asp:BoundField DataField="NewCustomer" HeaderText="<%$ Resources:Tokens,MenuNewCustomers %>"/>
                            <asp:BoundField DataField="PendingTe" HeaderText="<%$ Resources:Tokens,MenuPendingTE %>"/>
                            <asp:BoundField DataField="PendingWo" HeaderText="<%$ Resources:Tokens,MenuPendingWO %>"/>
                            <asp:BoundField DataField="PendingSplitting" HeaderText="<%$ Resources:Tokens,MenuPendingSplitting %>"/>
                            <asp:BoundField DataField="PendingInstallation" HeaderText="<%$ Resources:Tokens,MenuPendingInstallation %>" />
                            <asp:BoundField DataField="Active" HeaderText="<%$ Resources:Tokens,MenuActive %>" />
                            <asp:BoundField DataField="SystemProblem" HeaderText="<%$ Resources:Tokens,SystemProblem %>" />
                            <asp:BoundField DataField="CancellationProcess" HeaderText="<%$ Resources:Tokens,MenuCancellationProcess %>" />
                            <asp:BoundField DataField="Cancelled" HeaderText="<%$ Resources:Tokens,MenuCancelled %>" />
                            <asp:BoundField DataField="Hold" HeaderText="<%$ Resources:Tokens,MenuHold %>" />
                            <asp:BoundField DataField="Suspend" HeaderText="<%$ Resources:Tokens,MenuSuspend %>" />
                            <asp:BoundField DataField="AutoSuspend" HeaderText="<%$ Resources:Tokens,AutoSuspend %>" />
                        </Columns>
                    </asp:GridView>

     </div>
    <script type="text/javascript">
        $("tr:odd").addClass("warning");
    </script>
</asp:Content>
