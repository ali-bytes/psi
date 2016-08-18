<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Notifications.ascx.cs"
    Inherits="NewIspNL.WebUserControls.Notifications" %>
<%@ Import Namespace="NewIspNL" %>
<%@ Import Namespace="Resources" %>
<div class="row">
    <div class="space-6">
    </div>
    <div class="col-sm-7 infobox-container" id="counters" style="  margin-left: -4%;">
        <div style="  margin-left: 15%;">

        <div class="infobox infobox-green  ">
            <div class="infobox-icon">
                <i class="icon-comments"></i>
            </div>
            <div class="infobox-data">
                <% if (Datalevel != 0)
                   { %>
                <span class="infobox-data-number" style="font-size: 13px;">(<%= Tokens.Reseller %>&nbsp;<span><%= EditControls.GetResellerPprCount(Convert.ToInt32(Session["User_ID"]), Datalevel) %></span>)&nbsp;
                    &nbsp;<% if (Datalevel != 3)
                             { %>(<%= Tokens.Branch %>&nbsp;<span><%= EditControls.GetBranchPprCount(Convert.ToInt32(Session["User_ID"]), Datalevel) %></span>)<% } %></span>
                <% } %>
                <div class="infobox-content" style="font-size: 11px;">
                    <%=Tokens.MenuPendingPaymentRequest%></div>
            </div>
                </div>
        <div class="infobox infobox-blue  ">
            <div class="infobox-icon">
                <i class="icon-dollar"></i>
            </div>
            <div class="infobox-data">
                <% if (Datalevel != 0)
                   { %>
                <span class="infobox-data-number">
                    <%= EditControls.GetOverDaysSuspendedCustomerCount(Datalevel, Convert.ToInt32(Session["User_ID"])) %></span><% } %>
                <div class="infobox-content">
                    <%=Tokens.OverDaysSuspendedCustomers %></div>
            </div>
            
        </div>
        <div class="infobox infobox-pink  ">
            <div class="infobox-icon">
                <i class="icon-shopping-cart"></i>
            </div>
            <div class="infobox-data">
                <% if (Datalevel != 0)
                   { %>
                <%--  <span class="infobox-data-number"><%= EditControls.GetRechargeRequestsCount(Convert.ToInt32(Session["User_ID"]), Datalevel) %></span><% } %>
                --%>
                <span class="infobox-data-number" style="font-size: 13px;">(<%= Tokens.Reseller %>&nbsp;<span><%= EditControls.GetRechargeRequestsCount(Convert.ToInt32(Session["User_ID"]), Datalevel) %></span>)&nbsp;
                    &nbsp;<% if (Datalevel != 3)
                             { %>(<%= Tokens.Branch %>&nbsp;<span><%= EditControls.GetRechargeRequestsCountBranch(Convert.ToInt32(Session["User_ID"]), Datalevel)%></span>)<% } %></span>
                <% } %>
                <div class="infobox-content" style="font-size: 10px;">
                    <%=Tokens.HandelRecharge%></div>
            </div>
            <!--<div class="stat stat-important">0</div>-->
        </div>
        <div class="infobox infobox-red  ">
            <div class="infobox-icon">
                <i class="icon-bell"></i>
            </div>
            <div class="infobox-data">
                <span class="infobox-data-number">
                    <%=TicketCount %></span>
                <div class="infobox-content">
                    <%=Tokens.MenuPendingTicketing%></div>
            </div>
        </div>
        <div class="infobox infobox-orange2  ">
            <div class="infobox-icon">
                <i class="icon-fire"></i>
            </div>
            <div class="infobox-data">
                <span class="infobox-data-number" style="font-size: 13px">(<%=Tokens.Reseller %>&nbsp;<span><%=EditControls.GetRechargeClientRequestsCount(Convert.ToInt32(Session["User_ID"]), Datalevel)%></span>)
                    &nbsp;<% if (Datalevel != 3)
                             {
                    %>
                    (<%= Tokens.Branch %>&nbsp;<span><%= EditControls.GetRechargeBranchRequestsCount(Convert.ToInt32(Session["User_ID"]), Datalevel) %></span>)<% } %></span>
                <div class="infobox-content" style="font-size: 9px;">
                    <%=Tokens.RequestPaymentInvoice%></div>
            </div>
           
        </div>
    
        <div class="infobox infobox-blue2  ">
            <div class="infobox-icon">
                <i class="icon-bolt"></i>
            </div>
            <div class="infobox-data">
                <span class="infobox-data-number">
                    <%=EditControls.GetAllRequestsWoCount() %></span>
                <div class="infobox-content">
                    <%=Tokens.MenuPendingRequestsPanel%></div>
            </div>
        </div>
        <div class="space-6">
        </div>
        <a href="MessageTo.aspx" class="infobox infobox-green infobox-small infobox-dark"
            data-rel="tooltip" title="<%=Tokens.MessageTo %>" data-placement="bottom">
         
            <div class="infobox-icon">
                <i class="icon-envelope"></i>
            </div>
            <div class="infobox-data">
                <div class="infobox-content">
                    <%=Tokens.Send%></div>
                <div class="infobox-content">
                    <%=Tokens.Message %></div>
            </div>
        </a><a href="UnPaidDemands.aspx" class="infobox infobox-blue infobox-small infobox-dark"
            data-rel="tooltip" title="<%=Tokens.NumUnpaidDemaind2 %>" data-placement="bottom">
            <div class="infobox-chart">
                <span class="sparkline" data-values="3,4,2,3,4,4,2,2"></span>
            </div>
            <div class="infobox-data">
              
                <div class="infobox-content">
                    <%=Tokens.demands %></div>
                <% if (Datalevel != 0)
                   { %>
                <span class="infobox-content">
                    <%= EditControls.GetUnpaidDemandCount(Datalevel, Convert.ToInt32(Session["User_ID"])) %></span><% } %>
            </div>
        </a><a href="HandelRechargeRequestes.aspx" class="infobox infobox-grey infobox-small infobox-dark"
            data-rel="tooltip" title="<%=Tokens.HandelRechargeRequestes %>" data-placement="bottom">
            <div class="infobox-icon">
                <i class="icon-download-alt"></i>
            </div>
            <div class="infobox-data">
                <div class="infobox-content">
                    <%=Tokens.Requests%></div>
                <% if (Datalevel != 0)
                   { %>
                <div class="infobox-content">
                    <%= EditControls.GetRechargeRequestsCount(Convert.ToInt32(Session["User_ID"]), Datalevel) %></div>
                <% } %>
            </div>
        </a>
        
        </div>
        
        
        

    </div>
    <div class="vspace-sm">
    </div>
    <div class="col-sm-5" id="PendingActivationlPanel">
        
        <div class="widget-box">
            <div class="widget-header widget-header-flat widget-header-small">
                <h5>
                    <i class="icon-signal"></i>
                    <%=Tokens.Customers %>
                </h5>
            </div>
            <div class="widget-body">
                <div class="widget-main">
                    <div id="piechart-placeholder">
                    </div>
                    <div class="hr hr8 hr-double" >
                    </div>
                    <div class="clearfix">
                        <div class="grid3" style="margin: 0 29%; width: 40%;">
                            <span class="grey"><i class="icon-user icon-2x blue"></i>&nbsp;<%=Tokens.CustomersCount %>
                            </span>
                            <h4 class="bigger pull-right">
                                <%=CustomersCount %></h4>
                        </div>
                   
                    </div>
                </div>
                <!-- /widget-main -->
            </div>
            <!-- /widget-body -->
        </div>
        <!-- /widget-box -->
    </div>
          
</div>
<div class="hr hr32 hr-dotted">
</div>
<%--<%:System.Web.Optimization.Scripts.Render("~/bundles/ChartJs")%>--%>

<script type="text/javascript">


    var data = [
            { label: '<%=Active %>', data: '<%=ActiveDecimal %>', color: "#68BC31" }, //38.7
            {label: '<%=New %>', data: '<%=NewDecimal %>', color: "#6fb3e0" }, //24.5
            {label: '<%=MenuHold %>', data: '<%=MenuholdDecimal %>', color: "#2091CF" }, //5
            {label: '<%=MenuSuspend %>', data: '<%=MenuSuspendDecimal %>', color: "#AF4E96" }, //8.2
            {label: '<%=Cancel %>', data: '<%=CancelDecimal %>', color: "#DA5430" }, //18.6
            {label: '<%=SystemProblem %>', data: '<%=SystemProblemDecimal %>', color: "#FEE074"} //5
        ];

    adminScript.NotificationsMethod(data);
    adminScript.DialogNotifications();
    
 
</script>
