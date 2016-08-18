using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NewIspNL.Helpers;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class UpdateDemands : CustomPage
    {
        readonly IspDomian _domian;

        public UpdateDemands()
        {
            _domian = new IspDomian(IspDataContext);
        }


        protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) Button1.InnerHtml += string.Format(Tokens.UpdateDemandBefore, DateTime.Now.Date.ToShortDateString());
        var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        var demandnextmonth = context2.Options.FirstOrDefault();
        if (demandnextmonth.updatedemandnextmonth != true) { nextmonth.Visible = false; }
        if (!IsPostBack)
            {
                _domian.PopulateProviders(DdlProvider);
            }
        
        
    
    }

    void UpdateUnhandledDemands()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var now = DateTime.Now.AddHours().Date;
            var orders = context.WorkOrders.Where(x => x.RequestDate.Value.Date <= DateTime.Now.AddHours().Date&& x.WorkOrderStatusID==6).ToList();
            
            var demands = new DemandFactory(context);
            if (orders.Any())
            {
                var userId = Convert.ToInt32(Session["User_ID"]);
              
                demands.AddDemands(orders, now, userId,Page);
                msg.InnerHtml = Tokens.Saved;
                #region hashed
               
                #endregion
            }
            else
                msg.InnerHtml = Tokens.NoCustomersFoundWithoutDemands;
        }
    }

    void UpdateDemandsTothisMonth(int forReseller=0,int forDirect=0)
    {
        using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var thismonth = DateTime.Now.Month;
            var orders = new List<WorkOrder>();
            if (forReseller == 1)
            {
                orders = context2.WorkOrders.Where(a => a.RequestDate.Value.Date.Month == thismonth && a.WorkOrderStatusID == 6 && a.ResellerID !=null)
                    .ToList();
            }
            else if (forDirect==1)
            {
                orders = context2.WorkOrders.Where(a => a.RequestDate.Value.Date.Month == thismonth && a.WorkOrderStatusID == 6 && a.ResellerID == null)
                   .ToList();
            }
            else
            {
                orders = context2.WorkOrders.Where(a => a.RequestDate.Value.Date.Month == thismonth && a.WorkOrderStatusID == 6)
                   .ToList();
            }
               

            if (orders.Any())
            {
                var now = DateTime.Now.AddHours().Date;
                var userId = Convert.ToInt32(Session["User_ID"]);

                var priceServices = new PriceServices();
                var newOrders = new List<WorkOrder>();
                var demandfactory = new DemandFactory(context2);
                var ispEntries = new IspEntries(context2);
                var option = context2.OptionInvoiceProviders.ToList();
                foreach (var item in option)
                {
                    OptionInvoiceProvider item1 = item;
                    var w = orders.Where(a => a.ServiceProviderID == item1.ProviderId).ToList();
                    newOrders.AddRange(w);
                }

                if (newOrders.Any())
                {
                    foreach (var order in newOrders)
                    {
                        if (order.RequestDate == null || order.OfferStart == null) continue;
                        if (order.IpPackageID != null && order.IpPackage.IpPackageName != "0")
                        {
                            try
                            {
                                var ipAmount = Convert.ToInt32(order.IpPackage.IpPackageName)*10;
                       
                                var requestDate = Convert.ToDateTime(order.RequestDate);
                                //غيرنا بداية و تهاية المطالبة بدلالة تاريخ مطالبة العميل

                                var demand = demandfactory.CreateDemand(order, requestDate, requestDate.AddMonths(1),
                                    ipAmount, userId, paid: false,
                                    notes: "IP Package", isCommesstion: false);
                                ispEntries.AddDemands(demand);
                                ispEntries.Commit();
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        var billItem = priceServices.BillDefault(order, now.Month, now.Year, null);
            
                        var required = billItem != null ? billItem.Net : 0;
          

                        var activateProcessDemandService = new ProcessDemandsService(ispEntries,
                            new DemandFactory(ispEntries));

                        if (order.RequestDate != null && order.RequestDate.Value.Month == now.Month)
                        {
                            var monthDays = DateTime.DaysInMonth(order.RequestDate.Value.Year,
                            order.RequestDate.Value.AddMonths(1).Month);
                            var nextMonth = new DateTime(order.RequestDate.Value.Year,
                            order.RequestDate.Value.AddMonths(1).Month, Math.Min(order.RequestDate.Value.Day, monthDays));
                     
                            if (order.OfferStart != null)
                                activateProcessDemandService.CreateActivationDemand(order.ID, order.RequestDate.Value.Date,
                                    nextMonth,required,
                                    order.OfferStart.Value, false, userId);
                            msg.InnerHtml = Tokens.Saved;
                        }
                    }
                }
                else
                    msg.InnerHtml = Tokens.NoCustomersFoundWithoutDemands;
            }
            else
                msg.InnerHtml = Tokens.NoCustomersFoundWithoutDemands;
        }
    }


    protected void Update(object sender, EventArgs e){
        UpdateUnhandledDemands();
    }

    protected void UpdateThisMonth(object sender, EventArgs e)
    {
        UpdateDemandsTothisMonth();
    }

    public void UpdatenextMonth(object sender, EventArgs e)
    {
        UpdateDemandsTonextMonth();
    }
    public void UpdateThisMonthForReseller(object sender, EventArgs e)
    {
        UpdateDemandsTothisMonth(1,0);
    }

    public void UpdateThisMonthForDirectUser(object sender, EventArgs e)
    {
        UpdateDemandsTothisMonth(0, 1);
    }
    protected void Addrequestsuspend(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDaysCount.Text)) return;
                if (DdlProvider.SelectedValue == "") return;
                
                var ispEntries = new IspEntries(context);
                var demandService = new DemandService();
                
                var orders =
                    context.WorkOrders.Where(x => x.ResellerID == null && x.WorkOrderStatusID == 6 && x.ServiceProviderID==Convert.ToInt32(DdlProvider.SelectedValue)).ToList();
               
                var now = DateTime.Now.AddHours();
                
                foreach (var order in orders)
                {
                  
                    var orderId = order.ID;
                    var lastdemand = demandService.GetLastDemand(orderId);
                    if (lastdemand == null || lastdemand.Paid || lastdemand.Amount <= 0)  continue;
                    
                    var newdate = lastdemand.StartAt.AddDays(Convert.ToInt32(txtDaysCount.Text));
                    if (now.Date < newdate.Date) continue;
                    
                    if (order.WorkOrderStatusID != 6) continue;
                    var orderRequests =
                        context.WorkOrderRequests.Where(
                            woreq => woreq.WorkOrderID == orderId && woreq.RSID == 3);
                    if (orderRequests.Any()) continue;


                    var worequest = new WorkOrderRequest
                    {
                        WorkOrderID = orderId,
                        CurrentPackageID = order.ServicePackageID,
                        NewPackageID = order.ServicePackageID,
                        RequestDate = now,
                        RequestID = 2,
                        RSID = 3,
                        NewIpPackageID = order.IpPackageID,
                        SenderID = 1,
                    };
                    ispEntries.SaveRequest(worequest);
                    ispEntries.Commit();
                }
                msg.InnerHtml = Tokens.ProcessDone;
            }
            catch
            {
                msg.InnerHtml = Tokens.Error;
            }

        }
    }

   void UpdateDemandsTonextMonth()
    {
        using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
           
            var orders =
                context2.WorkOrders.Where(a => a.WorkOrderStatusID == 6 )
                    .ToList();

            if (orders.Any())
            {
                var now = DateTime.Now.AddHours().Date;
                var userId = Convert.ToInt32(Session["User_ID"]);




                var priceServices = new PriceServices();
                var newOrders = new List<WorkOrder>();
                var demandfactory = new DemandFactory(context2);
                var ispEntries = new IspEntries(context2);
                var option = context2.OptionInvoiceProviders.ToList();
                foreach (var item in option)
                {
                    OptionInvoiceProvider item1 = item;
                    var w = orders.Where(a => a.ServiceProviderID == item1.ProviderId).ToList();
                    newOrders.AddRange(w);
                }

                if (newOrders.Any())
                {
                    foreach (var order in newOrders)
                    {
                        if (order.RequestDate == null || order.OfferStart == null) continue;
                        if (order.IpPackageID != null && order.IpPackage.IpPackageName != "0")
                        {
                            try
                            {
                                var ipAmount = Convert.ToInt32(order.IpPackage.IpPackageName) * 10;

                                var requestDate = Convert.ToDateTime(order.RequestDate);
                                //غيرنا بداية و تهاية المطالبة بدلالة تاريخ مطالبة العميل

                                var demand = demandfactory.CreateDemand(order, requestDate, requestDate.AddMonths(1),
                                    ipAmount, userId, paid: false,
                                    notes: "IP Package", isCommesstion: false);
                                ispEntries.AddDemands(demand);
                                ispEntries.Commit();
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        var billItem = priceServices.BillDefault(order, now.Month, now.Year, null);

                        var required = billItem != null ? billItem.Net : 0;


                        var activateProcessDemandService = new ProcessDemandsService(ispEntries,
                            new DemandFactory(ispEntries));

                        if (order.RequestDate != null )
                        {
                            var monthDays = DateTime.DaysInMonth(order.RequestDate.Value.Year,
                            order.RequestDate.Value.AddMonths(1).Month);
                            var nextMonth = new DateTime(order.RequestDate.Value.Year,
                            order.RequestDate.Value.AddMonths(1).Month, Math.Min(order.RequestDate.Value.Day, monthDays));

                            if (order.OfferStart != null)
                                activateProcessDemandService.CreateActivationDemand(order.ID, order.RequestDate.Value.Date,
                                    nextMonth, required,
                                    order.OfferStart.Value, false, userId);
                            msg.InnerHtml = Tokens.Saved;
                        }
                    }
                }
                else
                    msg.InnerHtml = Tokens.NoCustomersFoundWithoutDemands;
            }
            else
                msg.InnerHtml = Tokens.NoCustomersFoundWithoutDemands;
        }
    }
}

}