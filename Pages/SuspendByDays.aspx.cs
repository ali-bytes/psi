using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BL.Concrete;
using Db;
using NewIspNL.Services;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class SuspendByDays : CustomPage
    {
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void AddRequest(object sender, EventArgs w)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSuspendDaysCount.Text)) return;
                var ispEntries = new IspEntries(context);
                var demandService = new DemandService();
                var providers = ispEntries.OptionInvoice;
                //var option = ispEntries.GetOption();
                var orders = new List<WorkOrder>();
                foreach (var item in providers)
                {
                    var workorder =
                        ispEntries.ClintsOfProvider(Convert.ToInt32(item.ProviderId));
                    if (workorder.Count > 0)
                    {
                        orders.AddRange(workorder);
                    }
                }
                var now = DateTime.Now.AddHours();
                var suspendproviders = context.OptionSuspendProviders.ToList();
                foreach (var order in orders)
                {
                    var orderId = order.ID;
                    var lastdemand = demandService.GetLastDemand(orderId);
                    if (lastdemand == null || lastdemand.Paid || lastdemand.Amount <= 0 || order.ResellerID != null)
                        continue;
                    var newdate = lastdemand.StartAt.AddDays(Convert.ToInt32(txtSuspendDaysCount.Text));
                    if (now.Date != newdate.Date) continue;
                    //var alertWay = option.AlertWayOfUnpaidDemand;

                    if (order.WorkOrderStatusID != 6) continue;
                    var orderRequests =
                        context.WorkOrderRequests.Where(
                            woreq => woreq.WorkOrderID == orderId && woreq.RSID == 3);
                    if (orderRequests.Any()) continue;

                    var orderprovider = order.ServiceProviderID;
                    var isfound = suspendproviders.Where(a => a.ProviderId == orderprovider);
                    if (!isfound.Any()) continue;
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
                MsgErro.Visible = false;
                MsgSuscc.Visible = true;
            }
            catch
            {
                MsgErro.Visible = true;
                MsgSuscc.Visible = false;
            }

        }
    }
}
}