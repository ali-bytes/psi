using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using BL.Concrete;
using Db;

using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;

namespace NewIspNL.Services{
    // Refactor if u have time :)
    public class DemandService{
        readonly CancelProcessDemandService _cancelProcessDemandService;

        readonly DemandFactory _demandFactory;

        readonly IspEntries _ispEntries;

        readonly ReportsDemandService _reportsDemandService;

        readonly UpDownGradeProcessDemandService _upDownService;


        public DemandService(){
            _ispEntries = new IspEntries();
            var priceServices = new PriceServices();
            _demandFactory = new DemandFactory(_ispEntries);
            _cancelProcessDemandService = new CancelProcessDemandService( _demandFactory);
            _upDownService = new UpDownGradeProcessDemandService(_demandFactory, priceServices);
            _reportsDemandService = new ReportsDemandService(_ispEntries);
        }


        public DemandService(ISPDataContext context)
        {
                _ispEntries = new IspEntries(context);
                var priceServices = new PriceServices();
                _demandFactory = new DemandFactory(_ispEntries);
                _cancelProcessDemandService = new CancelProcessDemandService(_demandFactory);
                _upDownService = new UpDownGradeProcessDemandService(_demandFactory, priceServices);
                _reportsDemandService = new ReportsDemandService(_ispEntries);
        }


        public virtual List<DemandModel> ResellerDemands(int resellerId){
            return _demandFactory.ResellerDemands(resellerId);
        }


        public DemandsReportContainer HandleCancelRequest(int orderId, DateTime date, int userId, int workOrderStatusId, bool previewOnly = false, int cancelOption = -1){
            return _cancelProcessDemandService.HandleCancelRequest(orderId, date, userId, workOrderStatusId, previewOnly : previewOnly, cancelOption : cancelOption);
        }



        public DemandsReportContainer ProcessUpDownGradeDemand(int orderId, DateTime date, int newPackageId, int userId, bool previewOnly , bool createTowInvoice,int method=5)
        {
            switch(method){
                    
                case 0:
                    return _upDownService.HandleUpDownGradeFirstChoise(date, orderId, newPackageId, userId, previewOnly,
                        createTowInvoice);
                case  1:
                    return _upDownService.HandleUpDownGradeSecoundChoise(date, orderId, newPackageId, userId, previewOnly, createTowInvoice);
                    
                case 2:
                    return _upDownService.HandleUpDownGradeThirdChoice(date, orderId, newPackageId, userId, previewOnly, createTowInvoice);
                    
                case 3:
                    return _upDownService.HandleUpDownGradeFourthChoice(date, orderId, newPackageId, userId, previewOnly, createTowInvoice);
                default:
                    return null;
            }
            //return _upDownService.HandleUpDownGrade(date, orderId, newPackageId, userId, previewOnly, createTowInvoice,method);
        }


        


        public OrderDemand GetDemandWithOrder(int orderId, DateTime date){
            return _demandFactory.GetDemandWithOrder(orderId, date);
        }



        public virtual List<Demand> CustomerDemands(int workOrderId){
            return _ispEntries.DemandsbyOrderId(workOrderId).OrderBy(x=>x.Id).ToList();
        }
        public virtual List<Demand> CustomerDemands(int workOrderId,ISPDataContext context)
        {
            return context.Demands.Where(x => x.WorkOrderId == workOrderId).OrderBy(x => x.Id).ToList();
           
        }

        public virtual List<WorkOrder> DemandWorkOrders(DateTime date){
            List<WorkOrder> workOrderOrdersByStatusId = _ispEntries.GetWorkOrderOrdersByStatusId(6);
            return workOrderOrdersByStatusId.Where(x => x.RequestDate != null && x.RequestDate.Value.Date.Equals(date.Date)).ToList();
        }


        public virtual DemandsReportContainer UpdateRequestDate(int orderId, DateTime time, int fromStatusId, int daysCount=0, bool previewOnly = false,int postponed = 0){
            return _reportsDemandService.UpdateRequestDate(orderId, time, fromStatusId, daysCount, previewOnly, postponed);
        }
        

        public virtual void AddDemands(List<WorkOrder> orders, DateTime date, int userId,Page page,bool redirect=true){
            _demandFactory.AddDemands(orders, date, userId,page,redirect);
        }


        public virtual bool DayProcessed(DateTime date){
            return _ispEntries.Demands().Any(x => x.StartAt.Date == date.Date);
        }


        public virtual void ProcessDemands(DateTime date,Page page ,int userId=1){
           // if(DayProcessed(date)) return;
           var orders = DemandWorkOrders(date);
            AddDemands(orders, date, userId,page,false);
        }

        public void ProcessSuspend(DateTime date)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var ispEntries = new IspEntries(context);
                var suspendOrders = ispEntries.GetWorkOrderOrdersByStatusId(11);
                foreach (var order in suspendOrders)
                {
                    try
                    {
                        
                        if(order.OfferStart==null || order.RequestDate==null)continue;
                        var suspendData = order.WorkOrderStatus.OrderByDescending(a => a.UpdateDate).FirstOrDefault();
                        if (suspendData != null && suspendData.UpdateDate != null)
                        {
                            var nextMonth = date.AddMonths(1);
                            var suspendPeriod = (date.Date - suspendData.UpdateDate.Value.Date).Days;
                            if (suspendPeriod == 90 && order.RequestDate != null )
                            {
                               
                                var lastDemand = context.Demands.Where(x => x.WorkOrderId == order.ID).ToList();
                                var lastDemand1 = lastDemand.LastOrDefault();
                                if (lastDemand1.Notes == "غرامة 30 جنية لان عدد ايام السسبند اصبح 90 يوم")
                                {
                                   continue;
                                }
                                    var demand = _demandFactory.CreateDemand(order, date, nextMonth, 30, 1, false,
                                        "غرامة 30 جنية لان عدد ايام السسبند اصبح 90 يوم");
                                    demand.IsResellerCommisstions = false;
                                    ispEntries.AddDemand(demand);
                                    ispEntries.Commit();
                                
                            }
                            //delete offer
                            if (order.Offer != null && order.Offer.SuspendPenalty>0&& suspendPeriod >= order.Offer.SuspendPenalty)
                            {
                                var oldoffer = string.Format(" تم الغاء العرض {0}" +
                                                             " الخاص بالعميل{1} " +
                                                             "لتجاوزه عدد الايام السسبند المسموح بها", order.Offer.Title,
                                    order.CustomerName);
                                order.Offer = null;
                                ispEntries.Commit();
                                if (order.ResellerID != null && order.ResellerID != -1)
                                {
                                    var option = OptionsService.GetOptions(context, true);
                                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                                    {
                                        const int userId = 1;
                                        CenterMessage.SendPublicMessageReport(order, oldoffer, userId);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
            }


        }

        public void ProcessUnpaidDemands(DateTime date)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var ispEntries = new IspEntries(context);
                var providers = ispEntries.OptionInvoice;
                var option = ispEntries.GetOption();
                var orders = new List<WorkOrder>();


                var autoSusOption = context.Options.FirstOrDefault();
                var autoSus = false;
                if (autoSusOption!=null)
                {
                   autoSus = autoSusOption.AutoSuspendCustomersUnderReseller??false;
                }


                foreach (var item in providers)
                {
                    var workorder =
                        ispEntries.ClintsOfProvider(Convert.ToInt32(item.ProviderId));
                    if (workorder.Count > 0)
                    {
                        orders.AddRange(workorder);
                    }
                }
                foreach (var order in orders)
                {
                    try
                    {

                        var orderId = order.ID;
                        var lastdemand = GetLastDemand(orderId);
                        //if (lastdemand == null || lastdemand.Paid || lastdemand.Amount <= 0 || order.ResellerID != null)
                        //    continue;
                        if (lastdemand == null || lastdemand.Paid || lastdemand.Amount <= 0)
                            continue;

                        if (!autoSus && order.ResellerID != null) continue;

                        var newdate = lastdemand.StartAt.AddDays(Convert.ToInt32(option.DaysOfUnpaidDemandsLimit));
                        if (date.Date != newdate.Date) continue;
                        var alertWay = option.AlertWayOfUnpaidDemand;
                        switch (alertWay)
                        {
                                #region sendmailhashed

                                /*case 0:
                                    try
                                    {

                                            var active = context.EmailCnfgs.FirstOrDefault();
                                            if (active != null && active.Active)
                                            {
                                                var messagetext = Tokens.Customer_Name + ":" + order.CustomerName +
                                                                  Tokens.br +
                                                                  Tokens.Customer_Phone + ":" + order.CustomerPhone +
                                                                  Tokens.br +
                                                                  " الرجاء التوجة الى فرع الشركة لدفع فاتورة خدمة الانترنت ";
                                                ClsEmail.SendEmail(order.CustomerEmail,
                                                    ConfigurationManager.AppSettings["InstallationEmail"]
                                                    , ConfigurationManager.AppSettings["CC2Email"],
                                                    "Internet Adsl Invoice", messagetext, true);
                                            }
                                        

                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                    break;*/

                                #endregion

                            case 1:
                                if (order.WorkOrderStatusID == 6)
                                {
                                    var orderRequests =
                                        context.WorkOrderRequests.Where(
                                            woreq => woreq.WorkOrderID == orderId && woreq.RSID == 3);
                                    if (!orderRequests.Any())
                                    {

                                        var tickets =
                                            context.Tickets.Where(
                                                z => z.WorkOrderID == order.ID && (z.StatusID == 1 || z.StatusID == 2))
                                                .ToList();
                                        if(tickets.Count>0) continue;
           
                                        var suspendproviders = context.OptionSuspendProviders.ToList();
                                        var orderprovider = order.ServiceProviderID;
                                        var isfound = suspendproviders.Where(a => a.ProviderId == orderprovider);
                                        if (isfound.Any())
                                        {

                                            //portal suspend
                                            var wor = new WorkOrderRequest();
                                            var portalList = context.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                                            var woproviderList = context.WorkOrders.FirstOrDefault(z => z.ID == orderId);
                                            if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                                            {
                                                if (woproviderList.UserName != null)
                                                {
                                                    var username = woproviderList.UserName;
                                                    CookieContainer cookiecon = new CookieContainer();
                                                    cookiecon = Tedata.Login();
                                                    if (cookiecon != null)
                                                    {
                                                        var pagetext = Tedata.GetSearchPage(username, cookiecon);
                                                        if (pagetext != null)
                                                        {
                                                             var searchPage = Tedata.CheckSearchPage(pagetext);
                                                            if (searchPage)
                                                            {
                                                                var custStatus = Tedata.CheckCustomerStatus(pagetext);
                                                                if (custStatus == "disable")
                                                                {
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    var worNote =
                                                                        Tedata.SendTedataSuspendRequest(username,
                                                                            cookiecon, pagetext);
                                                                    if (worNote != 2)
                                                                    {
                                                                        //فى حالة نجاح الارسال الى البورتال ننزل الطلب متوافق علية فى اى اس بى
                                                                        wor.WorkOrderID = order.ID;
                                                                        wor.RequestID = 2;
                                                                        wor.ConfirmerID = 1;
                                                                        wor.RequestDate = DateTime.Now.AddHours();
                                                                        wor.ProcessDate = DateTime.Now.AddHours();
                                                                        wor.RSID = 1;
                                                                        wor.SenderID = 1;
                                                                        wor.CurrentPackageID = order.ServicePackageID;
                                                                        wor.NewPackageID = order.ServicePackageID;
                                                                        wor.NewIpPackageID = order.IpPackageID;
                                                                        context.WorkOrderRequests.InsertOnSubmit(wor);
                                                                        //context.SubmitChanges();

                                                                        //تغيير الحالة الى suspend
                                                                        var current =
                                                                            context.WorkOrders.FirstOrDefault(
                                                                                x => x.ID == orderId);

                                                                        if (current != null)
                                                                        {
                                                                            current.WorkOrderStatusID = 11;

                                                                            global::Db.WorkOrderStatus wos = new global
                                                                                ::Db.WorkOrderStatus
                                                                            {
                                                                                WorkOrderID = current.ID,
                                                                                StatusID = 11,
                                                                                UserID = 1,
                                                                                UpdateDate = DateTime.Now.AddHours(),
                                                                            };
                                                                            context.WorkOrderStatus.InsertOnSubmit(wos);
                                                                        }

                                                                        context.SubmitChanges();
                                                                        continue;
                                                                    }
                                                                }

                                                            }


                                                        }
                                                        
                                                    }
                                                   
                                                }
                                            }


                                           
                                            var worequest = new WorkOrderRequest
                                            {
                                                WorkOrderID = orderId,
                                                CurrentPackageID = order.ServicePackageID,
                                                NewPackageID = order.ServicePackageID,
                                                RequestDate = date,
                                                RequestID = 2,
                                                RSID = 3,
                                                NewIpPackageID = order.IpPackageID,
                                                SenderID = 1,
                                                Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال "
                                            };
                                            ispEntries.SaveRequest(worequest);
                                            ispEntries.Commit();
                                        }
                                    }
                                }


                                break;
                            case 2:
                                ispEntries.SendNotes(orderId, date, option.ReminderMessage,
                                    Convert.ToInt32(option.ReminderToUserId));
                                break;
                        }

                    }
                    catch
                    {
                        continue;
                    }
                }


            }
        }

        public void FawryProcess()
        {
            _ispEntries.ManageFawry();
        }
        //public void Pay(int demandId, int userId, string comment = "",DateTime? paymentTime=null){
        //    var demand = _ispEntries.GetDemand(demandId);
        //    if(demand == null)return;
        //    demand.Paid = true;
        //    demand.PaymentComment = comment;
        //    demand.UserId = userId;
        //    if(paymentTime==null){
        //        demand.PaymentDate = DateTime.Now.AddHours();
        //    } else{
        //        demand.PaymentDate = paymentTime;
        //    }
        //    _ispEntries.Commit();
        //}
        public void Pay(int demandId, int userId, string note, string comment = "", DateTime? paymentTime = null)
        {
            var demand = _ispEntries.GetDemand(demandId);
            if (demand == null) return;
            demand.Paid = true;
            demand.Notes = note;
            demand.PaymentComment = comment;
            demand.UserId = userId;
            if (paymentTime == null)
            {
                demand.PaymentDate = DateTime.Now.AddHours();
            }
            else
            {
                demand.PaymentDate = paymentTime;
            }
            _ispEntries.Commit();

        }

        public void PayResellerDemands(List<int> demandsIds, int ? resellerId, int ? userId){
            _demandFactory.PayResellerDemands(demandsIds,resellerId,userId);
        }


        public void EditDemand(int demandId, DateTime startAt, DateTime endAt, decimal amount, string notes,bool isCommession){
            var demand = _ispEntries.GetDemand(demandId);
            if (demand == null) return;
            _ispEntries.AddUserTrack(new UserTracking()
            {
                WorkOrderId = demand.WorkOrderId,
                Date = DateTime.Now.AddHours(),
                // process type 3 for edit demand from ProcessType table
                ProcessTypeId = 3,
                UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]),
                Note = " تعديل قيمة الفاتورة من " + demand.Amount + " الى " + amount + " تعديل تاريخ بداية المطالبة من " + demand.StartAt + " الى " + startAt + " تعديل تاريخ انتهاء المطالبة من  " + demand.EndAt + " الى " + endAt
            });

            demand.StartAt = startAt;
            demand.EndAt = endAt;
            demand.Amount = amount;
            demand.Notes = notes;
            demand.IsResellerCommisstions = isCommession;
            
            _ispEntries.Commit();
        }


        public void CancelPayment(int demandId){
            var demand = _ispEntries.GetDemand(demandId);
            if (demand == null) return;
            demand.Paid = false;
            demand.PaymentDate = null;
            demand.Notes = "هذة الفاتورة تم الغاء دفعها";
            _ispEntries.Commit();
        }


        public Demand GetLastDemand(int orderId){
            return _ispEntries.DemandsbyOrderId(orderId).OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public Demand GetLastDemand2(int orderId, DateTime date)
        {

            var dem = _ispEntries.OrderDemand2(orderId, date).ToList();
            var demand2 = dem.LastOrDefault();



            return demand2;
        }
        public DemandsReportContainer AddDemandForWorkOrderService(int orderId, string notes, decimal amount, int userId, bool iscommisstion, bool previewOnly = false)
        {
            var demandsReportContainer = new DemandsReportContainer
            {
                After = new List<Demand>()
            };
            var lasdDemand = GetLastDemand(orderId);
            if (lasdDemand != null)
            {
                var serviceDemand = _demandFactory.CreateDemand(lasdDemand.WorkOrder, lasdDemand.StartAt, lasdDemand.EndAt, amount, userId, paid: false, notes: notes, isCommesstion: iscommisstion);
                if (!previewOnly)
                {
                    _ispEntries.AddDemands(serviceDemand);
                    _ispEntries.Commit();
                }
                if (previewOnly)
                {
                    demandsReportContainer.After.Add(serviceDemand);
                    serviceDemand.WorkOrder = _ispEntries.GetWorkOrder(orderId);
                }
            }
            return demandsReportContainer;
        }
        public DemandsReportContainer AddDemandForWorkOrderService(DateTime date,int orderId, string notes, decimal amount, int userId, bool iscommisstion, bool previewOnly = false)
        {
            var demandsReportContainer = new DemandsReportContainer
                    {
                        After = new List<Demand>()
                    };
            var lasdDemand = GetLastDemand2(orderId, date);
            if(lasdDemand != null){
                var serviceDemand = _demandFactory.CreateDemand(lasdDemand.WorkOrder,lasdDemand.StartAt,lasdDemand.EndAt,amount,userId,paid : false,notes : notes,isCommesstion:iscommisstion);
                if(!previewOnly){
                    _ispEntries.AddDemands(serviceDemand);
                    _ispEntries.Commit();
                }
                if(previewOnly){
                    demandsReportContainer.After.Add(serviceDemand);
                    serviceDemand.WorkOrder = _ispEntries.GetWorkOrder(orderId);
                }
            }
            return demandsReportContainer;
        }
    }
}
