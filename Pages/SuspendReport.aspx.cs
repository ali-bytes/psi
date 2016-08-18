using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Pages
{
    public partial class SuspendReport : CustomPage
    {
       
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var domian = new IspDomian(context);
            domian.PopulateResellerswithDirectUser(DdlReseller, true); //_domian.PopulateResellers(DdlReseller,true);
            domian.PopulateBranches(DdlBranch, true);
            PopulateProviders(domian,context);
            var currentYear = DateTime.Now.Year;
            Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear).OrderByDescending(x => x), DdlYear);
            Helper.PopulateMonths(DdlMonth);
        }

    }
    private void PopulateProviders(IspDomian domian, ISPDataContext db)
    {

        var userId = Convert.ToInt32(Session["User_ID"]);
        var user = db.Users.FirstOrDefault(a => a.ID == userId);
        if (user != null && user.GroupID == 6)
        {
            var providers = db.UserProviders.Where(a => a.UserId == userId).Select(x => new
            {
                x.ServiceProvider.SPName,
                x.ServiceProvider.ID,
            }).ToList();
            DdlSeviceProvider.DataSource = providers;
            DdlSeviceProvider.DataTextField = "SPName";
            DdlSeviceProvider.DataValueField = "ID";
            DdlSeviceProvider.DataBind();
            Helper.AddDefaultItem(DdlSeviceProvider);
        }
        else
        {
            domian.PopulateProviders(DdlSeviceProvider);
        }
    }

    protected void Search(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {

            var customers = context.WorkOrderStatus.Where(a=>a.StatusID==11).ToList();
            if (DdlReseller.SelectedIndex > 0)
            {
                var resellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
                customers = customers.Where(a => a.WorkOrder.ResellerID == resellerId).ToList();
            }
            if (DdlBranch.SelectedIndex > 0)
            {
                var branchId = Convert.ToInt32(DdlBranch.SelectedItem.Value);
                customers = customers.Where(a => a.WorkOrder.BranchID == branchId).ToList();
            }
            if (DdlSeviceProvider.SelectedIndex > 0)
            {
                var providerId = Convert.ToInt32(DdlSeviceProvider.SelectedItem.Value);
                customers = customers.Where(a => a.WorkOrder.ServiceProviderID == providerId).ToList();
            }
            if (DdlYear.SelectedIndex > 0)
            {
                var year = Convert.ToInt32(DdlYear.SelectedItem.Value);
                customers = customers.Where(a =>a.UpdateDate!=null && a.UpdateDate.Value.Year == year).ToList();
            }
            if (DdlMonth.SelectedIndex > 0)
            {
                var month = Convert.ToInt32(DdlMonth.SelectedItem.Value);
                customers = customers.Where(a => a.UpdateDate != null && a.UpdateDate.Value.Month == month).ToList();
            }
            var request = new List<ManageRequestTemplate>();
            foreach (var item in customers)
            {
                var mon = Convert.ToInt32(DdlMonth.SelectedItem.Value);
                var yea = Convert.ToInt32(DdlYear.SelectedItem.Value);
                var orderId = item.WorkOrderID;
                var find = request.Where(a => a.ID == orderId);
                if(find.Any())continue;
                var order = customers.Where(a => a.WorkOrderID == orderId).OrderByDescending(a => a.ID).FirstOrDefault();
                var sus = context.WorkOrderRequests.FirstOrDefault(x => x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 2 &&
                                                                        x.ProcessDate.Value.Month == mon && x.ProcessDate.Value.Year == yea);
               
                var unsus = context.WorkOrderRequests.FirstOrDefault(x => x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 2 &&
                                                                          x.ProcessDate.Value.Month == mon && x.ProcessDate.Value.Year == yea);


                if (sus == null || sus.ProcessDate > unsus.ProcessDate)
                {
             
                }
                else
                {
                    request.Add(Custome(order, context, Convert.ToInt32(DdlMonth.SelectedItem.Value), Convert.ToInt32(DdlYear.SelectedItem.Value)));
                }
                
                
                
            }
            grd_Requests.DataSource = request;
            grd_Requests.DataBind();
          
        }
    }




        public static int DaysForCustomerAtStatus(int orderId, int statusId,int mon, int yea)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                int dayes = 0;

                var daysofmn = DateTime.DaysInMonth(yea, mon);
                var allappsus =
                    context.WorkOrderRequests.Where(
                        x =>
                            x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 2 &&
                            x.ProcessDate.Value.Month == mon && x.ProcessDate.Value.Year == yea)
                        .Select(x => x.ProcessDate.Value.Day)
                        .ToList();
                var allappunsus =
                    context.WorkOrderRequests.Where(
                        x =>
                            x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 3 &&
                            x.ProcessDate.Value.Month == mon && x.ProcessDate.Value.Year == yea)
                        .Select(x => x.ProcessDate.Value.Day)
                        .ToList();

                if (allappsus.Count == 0 && allappunsus.Count == 0)
                {

                    dayes = 0;
                }
                else
                {
                    for (int xx = 0; xx < allappsus.Count; xx++)
                    {
                        var sus = allappsus[xx];
                        try
                        {

                            var unsus = allappunsus[xx];
                            dayes += Convert.ToInt32(unsus) - Convert.ToInt32(sus);

                        }
                        catch
                        {

                            if (DateTime.Now.Month == mon)
                            {
                                dayes += DateTime.Now.Day - Convert.ToInt32(sus);
                            }
                            else
                            {
                                dayes += daysofmn - Convert.ToInt32(sus);
                            }

                            continue;

                        }
                    }
                }
                return dayes;
            }











            // int dayes = 0;
               
               // var daysofmn = DateTime.DaysInMonth( yea,mon);
               // var allappsus =
               //     context.WorkOrderRequests.Where(
               //         x =>
               //             x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 2 &&
               //             x.ProcessDate.Value.Month == mon && x.ProcessDate.Value.Year == yea)
               //         .Select(x => x.ProcessDate.Value.Day)
               //         .ToList();
               //var   allappunsus =
               //     context.WorkOrderRequests.Where(
               //         x =>
               //             x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 3 &&
               //             x.ProcessDate.Value.Month == mon && x.ProcessDate.Value.Year == yea)
               //         .Select(x => x.ProcessDate.Value.Day)
               //         .ToList();


               //var allappsuslastmon =
               //   context.WorkOrderRequests.Where(
               //       x =>
               //           x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 2 &&
               //           x.ProcessDate.Value.Month == mon-1 && x.ProcessDate.Value.Year == yea)
               //       .Select(x => x.ProcessDate.Value.Day)
               //       .ToList();
               //var allappunsuslastmon =
               //     context.WorkOrderRequests.Where(
               //         x =>
               //             x.WorkOrderID == orderId && x.RSID == 1 && x.RequestID == 3 &&
               //             x.ProcessDate.Value.Month == mon-1 && x.ProcessDate.Value.Year == yea)
               //         .Select(x => x.ProcessDate.Value.Day)
               //         .ToList();



               // if (allappsus.Count == 0&&allappunsus.Count==0&&allappsuslastmon.Count == 0 && allappunsuslastmon.Count==0)
               // {

               //     return 0;
               // }
               //   if (allappsus.Count == 0 && allappunsus.Count == 0&& allappsuslastmon.Count == 1 && allappunsuslastmon.Count==0)
               // {
               //     dayes += DateTime.Now.Day ;
                   
               // }
               //   else if (allappsus.Count == 0 && allappunsus.Count > 0 && allappsuslastmon.Count == 1 && allappunsuslastmon.Count==0)
               // {
               //     dayes += allappunsus[0];

               // }
               //   else if (allappunsus.Count > allappsus.Count)
               //   {
               //       dayes += allappunsus[0];
               //       for (int xx = 1; xx < allappunsus.Count; xx++)
               //           {
                         
               //               var unsus = allappunsus[xx];
               //       try
               //       {

               //           var sus = allappsus[xx];
                             
               //               dayes += Convert.ToInt32(unsus) - Convert.ToInt32(sus);
                          
               //       }
               //       catch
               //       {

               //           if (DateTime.Now.Month == mon)
               //           {
               //               dayes += DateTime.Now.Day - Convert.ToInt32(unsus);
               //           }
               //           else
               //           {
               //               dayes += daysofmn - Convert.ToInt32(unsus);
               //           }

               //           continue;

               //       }}
               //   }
               //   else if (allappunsus.Count == allappsus.Count &&
               //            allappunsus.FirstOrDefault() < allappsus.FirstOrDefault())
               //   {
               //       dayes += allappunsus[0];
               //       for (int xx = 1; xx < allappunsus.Count; xx++)
               //       {
               //           var sus = allappsus[xx];
               //           var unsus = allappunsus[xx];
               //           dayes += Convert.ToInt32(unsus) - Convert.ToInt32(sus);
               //       }

               //   }
               //  else { 

               // for (int xx = 0; xx < allappsus.Count; xx++)
               // {
                    
                    
               //         var sus = allappsus[xx];

               //         if (allappunsus.Count == 0)
               //         {
               //             if (DateTime.Now.Month == mon)
               //             {
               //                 dayes += DateTime.Now.Day - Convert.ToInt32(sus);
               //             }
               //             else
               //             {
               //                 dayes += daysofmn - Convert.ToInt32(sus);
               //             }
               //         }
                      
               //         else
               //         {
               //             try
               //             {
                           
               //                     var unsus = allappunsus[xx];
               //                     dayes += Convert.ToInt32(unsus) - Convert.ToInt32(sus);
                              
               //             }
               //             catch
                               
               //             {

               //                 if (DateTime.Now.Month == mon)
               //                 {
               //                     dayes += DateTime.Now.Day - Convert.ToInt32(sus);
               //                 }
               //                 else
               //                 {
               //                     dayes += daysofmn - Convert.ToInt32(sus);
               //                 }

               //                continue;

               //             }

               //         }
               //         }
               //  }
 
               // return dayes;

       

        }




        private static ManageRequestTemplate Custome(global::Db.WorkOrderStatus orderStatus, ISPDataContext db,int mon,int yea)
    {
        var order = orderStatus.WorkOrder;
        var ispEntries = new IspEntries(db);
        IWorkOrderRepository workOrderRepository = new WorkOrderRepository();
        var activationDate = workOrderRepository.GetActivationDate(order.ID, 6);
        var now = DateTime.Now.AddHours();
        var monthDays = DateTime.DaysInMonth(now.Year, now.Month);
        var requestDate = Convert.ToDateTime(order.RequestDate);
        var day = new DateTime(now.Year, now.Month, Math.Min(DateTime.DaysInMonth(now.Year, now.Month), requestDate.Day));
        var trequestDate = order.RequestDate == null ? "" : day.ToShortDateString();
        var template = new ManageRequestTemplate
        {
            ID = order.ID,
            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone,
            Central = order.Central.Name,
            CurrentServicePackageName = order.ServicePackage.ServicePackageName,
            StatusName = order.Status.StatusName,
            SuspenDaysCount = DaysForCustomerAtStatus(Convert.ToInt32(order.ID), 11,mon,yea),
            UserName = order.User == null ? "" : order.User.UserName,
            BranchName = order.Branch.BranchName,
            PaymentType = order.PaymentType.PaymentTypeName,
            Title =order.Offer!=null? order.Offer.Title:"",
            IpPackageName = order.IpPackage.IpPackageName,
            TRequestDate = trequestDate,
            TActivationDate = activationDate == null ? "" : new DateTime(now.Year, now.Month, Math.Min(activationDate.Value.Day, monthDays)).ToShortDateString()
        };
        return template;
    }

    protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Helper.GridViewNumbering(grd_Requests, "lbl_No");
    }
}
}