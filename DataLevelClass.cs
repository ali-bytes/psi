using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using BL.Concrete;
using Db;
using Microsoft.Ajax.Utilities;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL
{
    public static class DataLevelClass{
        static readonly ISPDataContext Context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        static readonly IWorkOrderRepository WorkOrderRepository = new WorkOrderRepository();

        //static readonly DemandService DemandService = new DemandService();

        static readonly IspEntries IspEntries=new IspEntries();
        public static List<Branch> GetUserBranches(){
            var branchsList = new List<Branch>();
            if(HttpContext.Current.Session["User_ID"] == null){
                HttpContext.Current.Response.Redirect("../default.aspx");
                return branchsList;
            }
            var first = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if(first == null) return null;
            int dataLevel = first.Value;
            var i = (Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID)).First();
            if(i == null) return null;
            int userBranchId = i.Value;
            switch(dataLevel){
                case 1 :
                    branchsList = Context.Branches.Select(brnch => brnch).ToList();
                    break;
                case 2 :
                    branchsList = Context.Branches.Where(brnch => GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"])).
                        Contains(brnch.ID)).ToList();
                    break;
                case 3 :
                    branchsList = Context.Branches.Where(brnch => brnch.ID == userBranchId).ToList();
                    break;
            }
            return branchsList;
        }


        public static List<User> GetUserReseller(){
            if(HttpContext.Current.Session["User_ID"] == null)
                HttpContext.Current.Response.Redirect("../default.aspx");
            var first = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if(first == null) return null;
            int dataLevel = first.Value;
            var i = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID).First();
            if(i == null) return null;
            int userBranchId = i.Value;
            var resellerList = new List<User>();
            switch(dataLevel){
                case 1 :
                    resellerList = Context.Users
                        .Where(usr => usr.GroupID == 6).ToList();
                    break;
                case 2 :
                    resellerList = Context.Users
                        .Where(usr =>
                            usr.GroupID == 6 && 
                            GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Contains(usr.BranchID)).ToList();
                    break;
                case 3 :
                    resellerList = Context.Users
                        .Where(usr => usr.GroupID == 6
                                      && usr.BranchID == userBranchId
                                      && usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).ToList();
                    break;
            }
            return resellerList;
        }


        public static List<WorkOrder> GetUserWorkOrder(int userId, ISPDataContext context)
        {
            if (userId == 0) return null;
            var first = context.Users.Where(usr => usr.ID ==userId).Select(usr => usr.Group.DataLevelID).First();
            if(first == null)return null;
            int dataLevel = first.Value;
            var i = context.Users.Where(usr => usr.ID == userId).Select(usr => usr.BranchID).First();
            if(i == null) return null;
            int userBranchId = i.Value;
            var workOrderList = new List<WorkOrder>();
            switch(dataLevel){
                case 1 :
                    workOrderList = context.WorkOrders.Select(wo => wo).ToList();
                    break;
                case 2 :
                    workOrderList = context.WorkOrders.Where(wo => GetBranchAdminBranchIDs(userId).Contains(wo.BranchID)).ToList();
                    break;
                case 3 :
                    workOrderList = context.WorkOrders.Where(wo => wo.BranchID == userBranchId && wo.ResellerID == userId).ToList();
                    break;
            }
            return workOrderList;
        } 
    
        public static List<WorkOrder> GetUserWorkOrder(){
            if(HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
            var first = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if(first == null){
                return null;
            }
            int dataLevel = first.Value;
            var i = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID).First();
            if(i == null) return null;
            int userBranchId = i.Value;
            var workOrderList = new List<WorkOrder>();
            switch(dataLevel){
                case 1 :
                    workOrderList = Context.WorkOrders.Select(wo => wo).ToList();
                    break;
                case 2 :
                    workOrderList = Context.WorkOrders.Where(wo => GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Contains(wo.BranchID)).ToList();
                    break;
                case 3 :
                    workOrderList = Context.WorkOrders.Where(wo => wo.BranchID == userBranchId && wo.ResellerID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).ToList();
                    break;
            }
            return workOrderList;
        }
        public static List<WorkOrder> GetUserWorkOrder(ISPDataContext context){
            if(HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
            var first = context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if(first == null){
                return null;
            }
            int dataLevel = first.Value;
            var i = context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID).First();
            if(i == null) return null;
            int userBranchId = i.Value;
            var workOrderList = new List<WorkOrder>();
            switch(dataLevel){
                case 1 :
                    workOrderList = context.WorkOrders.Select(wo => wo).ToList();
                    break;
                case 2 :
                    workOrderList = context.WorkOrders.Where(wo => GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Contains(wo.BranchID)).ToList();
                    break;
                case 3 :
                    workOrderList = context.WorkOrders.Where(wo => wo.BranchID == userBranchId && wo.ResellerID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).ToList();
                    break;
            }
            return workOrderList;
        }

        public static List<WorkOrder> GetUserWorkOrderByAccountManager(int userId,ISPDataContext context)
        {
            var accountManagerUsers =
                context.WorkOrders.Where(a => a.User.AccountmanagerId != null && a.User.AccountmanagerId == userId).ToList();
            return accountManagerUsers;
        }

        public static List<Ticket> GetUserTickets(){
            if(HttpContext.Current.Session["User_ID"] == null)
                HttpContext.Current.Response.Redirect("../default.aspx");
            var first = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if(first == null) return null;
            int dataLevel = first.Value;
            var uid = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.BranchID).First();
            if(uid == null) return null;
            int userBranchId = uid.Value;
            var ticketList = new List<Ticket>();
            switch(dataLevel){
                case 1 :
                    ticketList = Context.Tickets.Select(tick => tick).ToList();
                    break;
                case 2 :
                    ticketList = Context.Tickets.Where(tick => GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Contains(
                        tick.WorkOrder.BranchID)).ToList();
                    break;
                case 3 :
                    ticketList = Context.Tickets.Where(tick => tick.WorkOrder.BranchID == userBranchId
                                                               &&
                                                               tick.WorkOrder.ResellerID ==
                                                               Convert.ToInt32(HttpContext.Current.Session["User_ID"])).ToList();
                    break;
            }
            return ticketList;
        }


        public static List<ManageRequestTemplate> GetUserNonCofirmedWoRequests(int requestId){
            if(HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
            var Cnt = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            var first = Context.Users.First(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]));//Select(usr => usr.Group.DataLevelID)

            if(first != null && first.Group.DataLevelID!=null)
            {
                var resultItems = new List<ManageRequestTemplate>();
                var isAcountManager = Context.Users.Where(a => a.AccountmanagerId == first.ID).ToList();
                if (isAcountManager.Count == 0)
                {
                    var dataLevel = first.Group.DataLevelID.Value;
                    switch (dataLevel)
                    {
                        case 1:
                           
                            var ss = Cnt.WorkOrderRequests
                                .Where(wor => wor.RequestID == requestId && wor.RSID == 3).ToList();

                            //resultItems = ss.Select(ToManageRequestTemplate).ToList();
                            var now = DateTime.Now.AddHours();
                             var monthDays = DateTime.DaysInMonth(now.Year, now.Month);
                            resultItems = ss.Select(x=>new ManageRequestTemplate
                            {
                                //TActivationDate = WorkOrderRepository.GetActivationDate(x.WorkOrder.ID, 6) == null ? "" : WorkOrderRepository.GetActivationDate(x.WorkOrder.ID, 6).Value.ToShortDateString(),
                                //ActivationDate = x.WorkOrder.WorkOrderStatus.Where(s => s.WorkOrderID == x.ID && s.StatusID == 6).Max(a => a.UpdateDate) ?? new DateTime(),
                                TActivationDate = x.WorkOrder.WorkOrderStatus != null ? x.WorkOrder.WorkOrderStatus.Where(s => s.WorkOrderID == x.WorkOrder.ID && s.StatusID == 6).Max(a => a.UpdateDate).ToString() ?? "-" : "-",
                                //TActivationDate = "-",
                                ID = x.ID,
                                Note = x.Notes,
                                ProviderRequest = x.IsProviderRequest ?? true,
                                UserName2 = x.WorkOrder.UserName,
                                CurrentPackageID = Convert.ToInt32(x.CurrentPackageID),
                                ExtraGiga = x.ExtraGiga == null ? "" : x.ExtraGiga.Name,
                                IpPackageName = x.IpPackage != null ? x.IpPackage.IpPackageName : string.Empty,
                                NewIpPackageID = Convert.ToInt32(x.NewIpPackageID),
                                NewPackageID = Convert.ToInt32(x.NewPackageID),
                                RequestDate = Convert.ToDateTime(x.RequestDate ?? new DateTime()),

                                TRequestDate = x.RequestDate == null ? "" : new DateTime(now.Year, now.Month, Math.Min(DateTime.DaysInMonth(now.Year, now.Month), Convert.ToDateTime(x.RequestDate ?? new DateTime()).Day)).ToShortDateString(),
                               
                                RSName = x.RequestStatus.RSName,
                                RSID = Convert.ToInt32(x.RSID ?? 0),
                                CurrentServicePackageName = x.ServicePackage.ServicePackageName ?? "-",
                                NewServicePackageName = x.ServicePackage1 != null ? x.ServicePackage1.ServicePackageName : string.Empty,
                                WorkOrderID = Convert.ToInt32(x.WorkOrderID ?? 0),
                                CustomerName = x.WorkOrder.CustomerName,
                                CustomerPhone = x.WorkOrder.CustomerPhone,
                                SPName = x.WorkOrder.ServiceProvider.SPName,
                                GovernorateName = x.WorkOrder.Governorate.GovernorateName,
                                UserName = x.WorkOrder.User == null ? "-" : x.WorkOrder.User.UserName,
                                woid = x.WorkOrder.ID,
                                RejectReason = x.RejectReason ?? "",
                                StatusName = x.WorkOrder.Status.StatusName,
                                Title = x.WorkOrder.Offer == null ? "" : x.WorkOrder.Offer.Title,
                                BranchName = x.WorkOrder.Branch.BranchName,
                                Central = x.WorkOrder.Central == null ? "-" : x.WorkOrder.Central.Name,
                                //RequestDate2 = WorkOrderRepository.GetActivationDate(x.WorkOrder.ID, 6) == null ? "" : new DateTime(now.Year, now.Month, Math.Min(WorkOrderRepository.GetActivationDate(x.WorkOrder.ID, 6).Value.Day, monthDays)).ToShortDateString(),
                                //RequestDate2 = x.WorkOrder.WorkOrderStatus.Where(s => s.WorkOrderID == x.WorkOrder.ID && s.StatusID == 6).Max(a => a.UpdateDate).Value.ToShortDateString() == null ? "" : new DateTime(now.Year, now.Month, Math.Min(x.WorkOrder.WorkOrderStatus.Where(s => s.WorkOrderID == x.WorkOrder.ID && s.StatusID == 6).Max(a => a.UpdateDate).Value.Day, monthDays)).ToShortDateString(),
                                SenderName =  x.SenderID==null?"":x.User.UserName,
                                //CurrentOffer = x.WorkOrder.Offer == null ? " " : x.WorkOrder.Offer.Title,
                                NewOffer = x.Offer == null ? "" : x.Offer.Title,
                                PaymentType = x.WorkOrder.PaymentType == null ? "-" : x.WorkOrder.PaymentType.PaymentTypeName,
                                PaymentTypeId = x.WorkOrder.PaymentTypeID ?? 0,
                                SuspenDaysCount = x.WorkOrder.WorkOrderStatusID == 11 ? IspEntries.DaysForCustomerAtStatus(Convert.ToInt32(x.WorkOrderID), 11) : 0,
                                //SuspenDaysCount = 0,
                                WRequestDate = x.WorkOrder.RequestDate == null ? "-" : x.WorkOrder.RequestDate.Value.ToShortDateString() ?? "-"

                            }).ToList();
                                
                                break;
                        case 2:
                            resultItems =
                                Cnt
                                    .WorkOrderRequests
                                    .Where(
                                        wor =>
                                            wor.RequestID == requestId && wor.RSID == 3 &&
                                            GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"]))
                                                .Contains(wor.WorkOrder.BranchID))
                                    .Select(ToManageRequestTemplate).ToList();
                            break;
                        case 3:
                            resultItems = Cnt.WorkOrderRequests.Where(wor => wor.RequestID == requestId
                                                                                 && wor.RSID == 3
                                                                                 &&
                                                                                 wor.WorkOrder.User.ID ==
                                                                                 Convert.ToInt32(
                                                                                     HttpContext.Current.Session["User_ID"]))
                                .ToList()
                                .Select(ToManageRequestTemplate).ToList();
                            break;
                    }
                }
                else
                {
                    foreach (var user in isAcountManager)
                    {
                        var data = Cnt.WorkOrderRequests.Where(wor => wor.RequestID == requestId
                                                                          && wor.RSID == 3
                                                                          &&
                                                                          wor.WorkOrder.User.ID==user.ID)
                            .ToList()
                            .Select(ToManageRequestTemplate).ToList();
                        if (data.Count != 0)
                            resultItems.AddRange(data);
                    }
                }
                //foreach(var resultItem in resultItems){
                //    var item = resultItem;
                //    var order = Cnt.WorkOrderStatus.Where(r => r.WorkOrderID == item.ID && r.StatusID == 6).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                //    if(order != null){
                //        resultItem.ActivationDate = Convert.ToDateTime(order.UpdateDate);
                //    }
                //}
                var index =Convert.ToInt32(first.ManageRequestPrivilege);
                switch (index)
                {
                    case 0:
                        //resultItems = resultItems;
                        break;
                    case 1:
                        resultItems =
                            resultItems.Where(a =>  !string.IsNullOrEmpty(a.UserName)).ToList();
                        break;
                    case 2:
                        resultItems = resultItems.Where(a => string.IsNullOrEmpty(a.UserName)).ToList();
                        break;
                }
                return resultItems;
            }
            return null;
        }
        public static List<ManageRequestTemplate> GetNonCofirmedRequestsToUser(int workOrderId)
        {
            if (HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");

            var first = Context.Users.First(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]));//Select(usr => usr.Group.DataLevelID)

            if (first != null && first.Group.DataLevelID != null)
            {
                var dataLevel = first.Group.DataLevelID.Value;
                var resultItems = new List<ManageRequestTemplate>();

                switch (dataLevel)
                {
                    case 1:
                        resultItems = Context.WorkOrderRequests
                            .Where(wor => wor.WorkOrderID == workOrderId && wor.RSID == 3).ToList()
                            .Select(ToManageRequestTemplate).ToList();
                        break;
                    case 2:
                        resultItems =
                            Context
                                .WorkOrderRequests
                                .Where(wor => wor.WorkOrderID == workOrderId && wor.RSID == 3 && GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Contains(wor.WorkOrder.BranchID))
                                .Select(ToManageRequestTemplate).ToList();
                        break;
                    case 3:
                        resultItems = Context.WorkOrderRequests.Where(wor => wor.WorkOrderID == workOrderId
                                                                             && wor.RSID == 3
                                                                             && wor.WorkOrder.User.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).ToList()
                            .Select(ToManageRequestTemplate).ToList();
                        break;
                }
                foreach (var resultItem in resultItems)
                {
                    var item = resultItem;
                    var order = Context.WorkOrderStatus.Where(r => r.WorkOrderID == item.ID && r.StatusID == 6).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                    if (order != null)
                    {
                        resultItem.ActivationDate = Convert.ToDateTime(order.UpdateDate);
                    }
                }
                var index = Convert.ToInt32(first.ManageRequestPrivilege);
                switch (index)
                {
                    case 0:
                        //resultItems = resultItems;
                        break;
                    case 1:
                        resultItems =
                            resultItems.Where(a => !string.IsNullOrEmpty(a.UserName)).ToList();
                        break;
                    case 2:
                        resultItems = resultItems.Where(a => string.IsNullOrEmpty(a.UserName)).ToList();
                        break;
                }
                return resultItems;
            }
            return null;
        }
        public static int GetCountNonCofirmedWoRequests(int requestId)
        {
            if (HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
            var userId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
            var first = Context.Users.Where(usr => usr.ID == userId).
                Select(usr => usr.Group.DataLevelID).First();
            int count = 0;
            if (first != null)
            {
                var accountManager = Context.Users.Where(a => a.AccountmanagerId == userId).ToList();
                if (accountManager.Count == 0)
                {
                    var dataLevel = first.Value;
                    //new List<ManageRequestTemplate>();

                    switch (dataLevel)
                    {
                        case 1:
                            count = Context.WorkOrderRequests.Count(wor => wor.RequestID == requestId && wor.RSID == 3);
                            break;
                        case 2:
                            count =
                                Context
                                    .WorkOrderRequests
                                    .Count(
                                        wor =>
                                            wor.RequestID == requestId && wor.RSID == 3 &&
                                            GetBranchAdminBranchIDs(userId)
                                                .Contains(wor.WorkOrder.BranchID));
                            break;
                        case 3:
                            count = Context.WorkOrderRequests.Count(wor => wor.RequestID == requestId
                                                                           && wor.RSID == 3
                                                                           &&
                                                                           wor.WorkOrder.User.ID == userId);
                            break;
                    }
                    /*foreach (var resultItem in count)
            {
                var item = resultItem;
                var order = _context.WorkOrderStatus.Where(r => r.WorkOrderID == item.ID && r.StatusID == 6).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                if (order != null)
                {
                    resultItem.ActivationDate = Convert.ToDateTime(order.UpdateDate);
                }
            }*/

                }
                else
                {
                    foreach (var user in accountManager)
                    {
                        var data=Context.WorkOrderRequests.Count(wor => wor.RequestID == requestId
                                                                        && wor.RSID == 3
                                                                        &&
                                                                        wor.WorkOrder.User.ID==user.ID);
                        if (data > 0) count += data;
                    }
                }
            }
            return count;
        }
        public static List<WorkOrderRequest> GetCofirmedWoRequests()
        {
            if (HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
            var userId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
            var first = Context.Users.Where(usr => usr.ID == userId).
                Select(usr => usr.Group.DataLevelID).First();
            var requests = new List<WorkOrderRequest>();//= null;
            if (first == null) return requests;
            var accountManager = Context.Users.Where(a => a.AccountmanagerId == userId).ToList();
            if (accountManager.Count == 0)
            {
                var dataLevel = first.Value;
                //new List<ManageRequestTemplate>();

                switch (dataLevel)
                {
                    case 1:
                        requests = Context.WorkOrderRequests.Where(wor => wor.RequestID != 11 && wor.RSID == 1).ToList();
                        break;
                    case 2:
                        requests =
                            Context
                                .WorkOrderRequests
                                .Where(
                                    wor =>
                                        wor.RequestID != 11 && wor.RSID == 1 &&
                                        GetBranchAdminBranchIDs(userId)
                                            .Contains(wor.WorkOrder.BranchID)).ToList();
                        break;
                    case 3:
                        requests = Context.WorkOrderRequests.Where(wor => wor.RequestID != 11
                                                                          && wor.RSID == 1
                                                                          &&
                                                                          wor.WorkOrder.User.ID == userId).ToList();
                        break;
                }
                /*foreach (var resultItem in count)
            {
                var item = resultItem;
                var order = _context.WorkOrderStatus.Where(r => r.WorkOrderID == item.ID && r.StatusID == 6).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                if (order != null)
                {
                    resultItem.ActivationDate = Convert.ToDateTime(order.UpdateDate);
                }
            }*/

            }
            else
            {
                //var c = 0;
                foreach (var user in accountManager)
                {
                    var user1 = user;
                    var data = Context.WorkOrderRequests.Where(wor => wor.RequestID != 11
                                                                      && wor.RSID == 1
                                                                      &&
                                                                      wor.WorkOrder.User.ID == user1.ID).ToList();
                    if (data.Count > 0) requests.AddRange(data);
                }
            }
            return requests;
        }

        static ManageRequestTemplate ToManageRequestTemplate(WorkOrderRequest x){
            var activationDate = WorkOrderRepository.GetActivationDate(x.WorkOrder.ID, 6);
            var note = x.Notes;
            var request = x.IsProviderRequest == null ? true : x.IsProviderRequest; 
            var now = DateTime.Now.AddHours();
            var monthDays = DateTime.DaysInMonth(now.Year, now.Month);
            var requestDate2 = activationDate == null ? "" : new DateTime(now.Year, now.Month, Math.Min(activationDate.Value.Day, monthDays)).ToShortDateString();
            var rejectReason = x.RejectReason ?? "";
            var userName = x.WorkOrder.User == null ? "-" : x.WorkOrder.User.UserName;
            var senderName = x.SenderID==null?"":x.User.UserName;
            var requestDate = Convert.ToDateTime(x.RequestDate)/*.AddMonths(1)*/;
            var day = new DateTime(now.Year, now.Month, Math.Min(DateTime.DaysInMonth(now.Year, now.Month), requestDate.Day));
            var trequestDate = x.RequestDate == null ? "": day/*.AddMonths(1)*/.ToShortDateString();
            var username2 = x.WorkOrder.UserName;
            //var ispEntries = new IspEntries();
            var demand = IspEntries.OrderDemand(Convert.ToInt32(x.WorkOrderID)).OrderByDescending(a => a.Id).FirstOrDefault();
            var template = ToManageRequestTemplate(x, activationDate, requestDate, trequestDate, userName, rejectReason, requestDate2, senderName, username2, note, request);
            if(x.RequestID == null) return template;
            template.RequestTypeId = x.RequestID;
            if(demand!=null)template.ResultHtml = demand.Notes; //HandleResultTemplate(x);
            return template;
        }


        static ManageRequestTemplate ToManageRequestTemplate(WorkOrderRequest x, DateTime? activationDate, DateTime requestDate, string trequestDate, string userName, string rejectReason, string requestDate2, string sendername, string username2, string note, bool? prorequest)
        {
            var template = new ManageRequestTemplate{
                TActivationDate = activationDate == null ? "" : activationDate.Value.ToShortDateString(),
                ID = x.ID,
                Note = note,
                ProviderRequest = prorequest,
                UserName2  =username2,
                CurrentPackageID = Convert.ToInt32(x.CurrentPackageID),
                ExtraGiga = x.ExtraGiga == null ? "" : x.ExtraGiga.Name,
                IpPackageName = x.IpPackage != null ? x.IpPackage.IpPackageName : string.Empty,
                NewIpPackageID = Convert.ToInt32(x.NewIpPackageID),
                NewPackageID = Convert.ToInt32(x.NewPackageID),
                RequestDate = requestDate,
                TRequestDate = trequestDate,
                RSName = x.RequestStatus.RSName,
                RSID = Convert.ToInt32(x.RSID),
                CurrentServicePackageName = x.ServicePackage.ServicePackageName,
                NewServicePackageName = x.ServicePackage1 != null ? x.ServicePackage1.ServicePackageName : string.Empty,
                WorkOrderID = Convert.ToInt32(x.WorkOrderID),
                CustomerName = x.WorkOrder.CustomerName,
                CustomerPhone = x.WorkOrder.CustomerPhone,
                SPName = x.WorkOrder.ServiceProvider.SPName,
                GovernorateName = x.WorkOrder.Governorate.GovernorateName,
                UserName = userName,
                woid = x.WorkOrder.ID,
                RejectReason = rejectReason,
                StatusName = x.WorkOrder.Status.StatusName,
                Title = x.WorkOrder.Offer == null ? "" : x.WorkOrder.Offer.Title,
                BranchName = x.WorkOrder.Branch.BranchName,
                Central = x.WorkOrder.Central == null ? "-" : x.WorkOrder.Central.Name,
                RequestDate2 = requestDate2,
                SenderName = sendername,
                //CurrentOffer = x.WorkOrder.Offer == null ? " " : x.WorkOrder.Offer.Title,
                NewOffer =x.Offer==null?"": x.Offer.Title,
                PaymentType = x.WorkOrder.PaymentType.PaymentTypeName,
                PaymentTypeId = x.WorkOrder.PaymentTypeID??0,
                SuspenDaysCount = x.WorkOrder.WorkOrderStatusID==11?IspEntries.DaysForCustomerAtStatus(Convert.ToInt32(x.WorkOrderID), 11):0,
                WRequestDate = x.WorkOrder.RequestDate.Value.ToShortDateString()??"-"
            };
        
            return template;
        }

        public static List<Group> GetUserGroups(){
            if(HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
            var first = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if(first == null) return null;
            int dataLevel = first.Value;

            var groupList = new List<Group>();
            switch(dataLevel){
                case 1 :
                    groupList =  Context.Groups.ToList();
                    break;
                case 2 :
                    groupList = (from gp in Context.Groups
                        where gp.DataLevelID == dataLevel
                              ||
                              gp.DataLevelID == dataLevel + 1
                        select gp).ToList();
                    break;
                case 3 :
                    groupList = (Context.Groups.Where(gp => gp.ID == (Context.Users.First(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).GroupID.Value))).ToList();
                    break;
            }
            return groupList;
        }

        public static List<UsersClass> GetListUsersByDataLevel()
        {
            if (HttpContext.Current.Session["User_ID"] == null)
                HttpContext.Current.Response.Redirect("../default.aspx");
            var first = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
            if (first == null) return null;
            var dataLevel = first.Value;

            switch (dataLevel)
            {
                case 1:
                {
                    var query = Context.Users.Select(usr => new UsersClass
                    {
                        Id = usr.ID,
                        UserName = usr.UserName,
                        UserPhone = usr.UserPhone,
                        UserEmail = usr.UserEmail,
                        GroupName = usr.Group.GroupName,
                        GroupId = usr.GroupID,
                        BranchName = usr.Branch.BranchName,
                        BranchId = usr.BranchID,
                        IsAccountStopped = usr.IsAccountStopped,
                        GovernerateId = usr.GovernorateId,
                        AccountManagerId = usr.AccountmanagerId,
                        Governerate= Context.Governorates.FirstOrDefault(a=>a.ID==usr.GovernorateId).GovernorateName,
                        AccountManager = Context.Users.FirstOrDefault(a=>a.ID==usr.AccountmanagerId).UserName
                        
                    }).ToList();
                    return query;
                }
                case 2:
                {
                    var userId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
                    var query = Context.Users.Where(usr => GetBranchAdminBranchIDs(userId).
                        Contains(usr.BranchID)  ).Select(usr => new UsersClass
                                                           {
                                                               Id = usr.ID,
                                                               UserName = usr.UserName,
                                                               UserPhone = usr.UserPhone,
                                                               UserEmail = usr.UserEmail,
                                                               GroupName = usr.Group.GroupName,
                                                               GroupId = usr.GroupID,
                                                               BranchName = usr.Branch.BranchName,
                                                               BranchId = usr.BranchID,
                                                               IsAccountStopped = usr.IsAccountStopped,
                                                               GovernerateId = usr.GovernorateId,
                                                               AccountManagerId = usr.AccountmanagerId,
                                                               Governerate = Context.Governorates.FirstOrDefault(a => a.ID == usr.GovernorateId).GovernorateName,
                                                               AccountManager = Context.Users.FirstOrDefault(a => a.ID == usr.AccountmanagerId).UserName
                                                           }).ToList();
                    return query;
                }
                case 3:
                {
                    var query = Context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => new UsersClass
                    {
                        Id = usr.ID,
                        UserName = usr.UserName,
                        UserPhone = usr.UserPhone,
                        UserEmail = usr.UserEmail,
                        GroupName = usr.Group.GroupName,
                        GroupId = usr.GroupID,
                        BranchName = usr.Branch.BranchName,
                        BranchId = usr.BranchID,
                        IsAccountStopped = usr.IsAccountStopped,
                        GovernerateId = usr.GovernorateId,
                        AccountManagerId = usr.AccountmanagerId,
                        Governerate = Context.Governorates.FirstOrDefault(a => a.ID == usr.GovernorateId).GovernorateName,
                        AccountManager = Context.Users.FirstOrDefault(a => a.ID == usr.AccountmanagerId).UserName
                    }).ToList();
                    return query;
                }
                default:
                    return null;
            }
        }
        public class UsersClass{
            public int Id { get; set; }

            public string UserName { get; set; }

            public string UserPhone { get; set; }

            public string UserEmail { get; set; }

            public string GroupName { get; set; }

            public int? GroupId { get; set; }

            public string BranchName { get; set; }

            public int? BranchId { get; set; }

            public bool? IsAccountStopped { get; set; }
            public string Governerate { get; set; }
            public int? GovernerateId { get; set; }
            public string AccountManager { get; set; }
            public int? AccountManagerId { get; set; }

        }

        public static List<int ?> GetBranchAdminBranchIDs(int userId){
            if(HttpContext.Current.Session["User_ID"] == null)
                HttpContext.Current.Response.Redirect("../default.aspx");
            var userBranchs = Context.UserBranches.Where(ub => ub.UserID == userId).Select(ub => ub.BranchID).ToList();
            if(userBranchs.Count <= 0)
                userBranchs = Context.Users.Where(u => u.ID == userId).Select(u => u.BranchID).ToList();
            return userBranchs;
        }
    }
}
