using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using BL.Concrete;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using WorkOrderCredit = NewIspNL.Domain.Abstract.WorkOrderCredit;
using Resources;
 


namespace NewIspNL.Services.DemandServices{
    public class DemandFactory{
        readonly IspEntries _ispEntries;

        readonly ResellerServices _resellerService;
        private readonly IWorkOrderCredit _orderCredit=new WorkOrderCredit();

        public DemandFactory(IspEntries ispEntries){
            _ispEntries = ispEntries;
            _resellerService = new ResellerServices();
            _orderCredit = new WorkOrderCredit();
        }


        public DemandFactory(ISPDataContext context)
        {
            _ispEntries = new IspEntries(context);
            _resellerService = new ResellerServices();
        }


        public virtual Demand CreateDemand(WorkOrder order, DateTime startAt, DateTime endAt, decimal amount, int userId, DateTime? paymentdate, bool paid = false, string notes = "", string paymentComment = "")
        {
            var lastCredit = _orderCredit.GetLastCredit(order.ID, _ispEntries.Context);
            if(lastCredit!=0)lastCredit*=-1;
            var demand = new Demand{
                Amount = amount,
                EndAt = endAt,
                StartAt = startAt,
                Paid = lastCredit>=amount,
                WorkOrder = order,
                OfferId = order.OfferId,
                Notes = notes,
                UserId = userId,
                PaymentComment = paymentComment,
                PaymentDate = paymentdate
            };
            if (!demand.Paid) return demand;
            var credit = lastCredit - amount;
            if (lastCredit > amount) credit *= -1;
            _orderCredit.AddCredit(userId, order.ID, credit, "دفع فاتورة", DateTime.Now.AddHours(), _ispEntries.Context);
            if (paymentdate == null) demand.PaymentDate = DateTime.Now.AddHours();
            return demand;
        }

        public virtual Demand firstCreateDemand(WorkOrder order, DateTime startAt, DateTime endAt, decimal amount, int userId, DateTime? paymentdate, bool paid = false, string notes = "", string paymentComment = "")
        {
            //var lastCredit = _orderCredit.GetLastCredit(order.ID, _ispEntries.Context);
            //if (lastCredit != 0) lastCredit *= -1;
            var demand = new Demand
            {
                Amount = amount,
                EndAt = endAt,
                StartAt = startAt,
                Paid = paid,
                WorkOrder = order,
                OfferId = order.OfferId,
                Notes = notes,
                UserId = userId,
                PaymentComment = paymentComment,
                PaymentDate = paymentdate
            };
            if (!demand.Paid) return demand;
            
               if (paymentdate == null) demand.PaymentDate = DateTime.Now.AddHours();
            return demand;
        }


        public virtual Demand CreateDemand(WorkOrder order, DateTime startAt, DateTime endAt, decimal amount, int userId, bool paid = false, string notes = "", string paymentComment = "",bool isCommesstion=true)
        {

            var lastCredit = _orderCredit.GetLastCredit(order.ID, _ispEntries.Context);
            if (lastCredit != 0) lastCredit *= -1;
            if (amount > 0)
            {
                var demand = new Demand
                {
                    Amount = amount,
                    StartAt = startAt,
                    EndAt = endAt,
                    Paid = lastCredit >= amount,
                    WorkOrder = order,
                    OfferId = order.OfferId,
                    Notes = notes,
                    UserId = userId,
                    PaymentComment = paymentComment,
                    IsResellerCommisstions = isCommesstion,
                };
                if (!demand.Paid) return demand;
                var credit = lastCredit - amount;
                if (lastCredit > amount) credit *= -1;
                _orderCredit.AddCredit(userId, order.ID, credit, "دفع فاتورة", DateTime.Now.AddHours(),
                    _ispEntries.Context);
                demand.PaymentDate = DateTime.Now.AddHours();
                return demand;
            }
            else
            {
                var demand = new Demand
                {
                    Amount = amount,
                    StartAt = startAt,
                    EndAt = endAt,
                    Paid = paid,
                    WorkOrder = order,
                    OfferId = order.OfferId,
                    Notes = notes,
                    UserId = userId,
                    PaymentComment = paymentComment,
                    IsResellerCommisstions = isCommesstion,
                };
                if (!demand.Paid) return demand;
                
                demand.PaymentDate = DateTime.Now.AddHours();
                return demand;


            }
        }

        public virtual Demand CreateDemand(WorkOrder order, DateTime startAt, DateTime endAt, decimal amount, int userId,
            bool branchPaid, bool paid = false, string notes = "", string paymentComment = "", bool isCommesstion = true)
        {
            var lastCredit = _orderCredit.GetLastCredit(order.ID, _ispEntries.Context);
            if (lastCredit != 0) lastCredit *= -1;
            if (amount > 0)
            {
                var demand = new Demand
                {
                    Amount = amount,
                    EndAt = endAt,
                    StartAt = startAt,
                    Paid = lastCredit >= amount,
                    WorkOrder = order,
                    OfferId = order.OfferId,
                    Notes = notes,
                    UserId = userId,
                    PaymentComment = paymentComment,
                    IsResellerCommisstions = isCommesstion,
                    BranchPaid = branchPaid
                };
                if (!demand.Paid) return demand;
                var credit = lastCredit - amount;
                if (lastCredit > amount) credit *= -1;
                AddCredit(userId, order.ID, credit, "دفع فاتورة", DateTime.Now.AddHours(),
                    _ispEntries.Context);
                demand.PaymentDate = DateTime.Now.AddHours();
                return demand;
            }
            else
            {
                var demand = new Demand
                {
                    Amount = amount,
                    EndAt = endAt,
                    StartAt = startAt,
                    Paid = paid,
                    WorkOrder = order,
                    OfferId = order.OfferId,
                    Notes = notes,
                    UserId = userId,
                    PaymentComment = paymentComment,
                    IsResellerCommisstions = isCommesstion,
                    BranchPaid = branchPaid
                };
                if (!demand.Paid) return demand;

                demand.PaymentDate = DateTime.Now.AddHours();
                return demand;
            }
        }

        private void AddCredit(int confirmUserId, int orderId, decimal amount, string note, DateTime date, ISPDataContext context)
        {
            //var lastCredit = GetLastCredit(orderId, context);
            var newCredit = new global::Db.WorkOrderCredit
            {
                UserId = confirmUserId,
                WorkOrderId = orderId,
                Notes = note,
                Time = date,
                CreditAmount = amount//(amount<0 && lastCredit>(amount*-1))?  lastCredit+amount:amount+lastCredit
            };
            context.WorkOrderCredits.InsertOnSubmit(newCredit);
            //context.SubmitChanges();
        }
        public OrderDemand GetDemandWithOrder(int orderId, DateTime date){
            var demandWithOrder = new OrderDemand{
                Demand = GetDemand(orderId, date),
                Order = _ispEntries.GetWorkOrder(orderId)
            };
            return demandWithOrder.Demand == null || demandWithOrder.Order == null ? null : demandWithOrder;
        }


        public Demand GetDemand(int orderId, DateTime date){
            return _ispEntries.DemandsbyOrderId(orderId).FirstOrDefault( /*x => x.StartAt.Date <= date.Date && x.EndAt.Date >= date.Date*/);
        }


        public Demand ToFakeDemand(Demand demand, WorkOrder order){
            return new Demand{
                Amount = demand.Amount,
                EndAt = demand.EndAt,
                Paid = demand.Paid,
                StartAt = demand.StartAt,
                WorkOrder = order,
                WorkOrderId = order.ID,
                UserId = demand.UserId,
                User = demand.User,
                Notes = demand.Notes
            };
        }


        public virtual List<DemandModel> ResellerDemands(int resellerId){
            var demands = new List<DemandModel>();
            var orders = _ispEntries.WorkOrdersOfReseller(resellerId);
            if(!orders.Any()){
                return demands;
            }

            foreach(var order in orders){
                var orderDemands = _ispEntries.DemandsbyOrderId(order.ID, false);
                foreach(var orderDemand in orderDemands){
                    var model = new DemandModel{
                        Demand = orderDemand,
                        Amount = orderDemand.Amount,
                        EndAt = orderDemand.EndAt,
                        StartAt = orderDemand.StartAt,
                        Paid = orderDemand.Paid,
                        Id = orderDemand.Id,
                        OrderId = orderDemand.WorkOrderId,
                    };
                    model.Amount = _resellerService.DeductDemandResellerDiscount(orderDemand);
                    demands.Add(model);
                }
            }
            return demands;
        }


        public virtual void AddDemands(List<WorkOrder> orders, DateTime date, int userId, Page page, bool redirect = true)
        {
            if(!orders.Any()) return;
            var priceServices = new PriceServices();
            var newOrders = new List<WorkOrder>();
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var option = context.OptionInvoiceProviders.ToList();
                foreach (var item in option)
                {
                    var item1 = item;
                    var w = orders.Where(a => a.ServiceProviderID == item1.ProviderId).ToList();
                    newOrders.AddRange(w);
                }
            }

            foreach (var order in newOrders)
            {
                if (order.RequestDate == null || order.OfferStart == null) continue;
                if (order.IpPackageID != null && order.IpPackage.IpPackageName != "0")
                {
                    try
                    {
                        var ipAmount = Convert.ToInt32(order.IpPackage.IpPackageName)*10;
                        var requestDate = Convert.ToDateTime(order.RequestDate);//غيرنا بداية و تهاية المطالبة بدلالة تاريخ مطالبة العميل
                        var demand = CreateDemand(order, requestDate, requestDate.AddMonths(1), ipAmount, userId, false, "IP Package", isCommesstion: false);
                        _ispEntries.AddDemands(demand);
                        _ispEntries.Commit();
                    }
                    catch
                    {
                        continue;
                    }
                }
                var billItem = priceServices.BillDefault(order, date.Month, date.Year, null, redirect);
               
                var required = billItem != null ? billItem.Net : 0;
              

                var activateProcessDemandService = new ProcessDemandsService(_ispEntries, new DemandFactory(_ispEntries));
               
                if (order.RequestDate != null && order.RequestDate.Value.Date == date.Date)
                {
                    var monthDays = DateTime.DaysInMonth(date.Year, date.AddMonths(1).Month);
                    var nextMonth = new DateTime(date.Year, date.AddMonths(1).Month, Math.Min(date.Day, monthDays));
                    activateProcessDemandService.CreateActivationDemand(order.ID, date, nextMonth, required,
                        order.OfferStart.Value, false, userId);
                }
                else if (order.RequestDate != null && order.RequestDate.Value.Date < date.Date)
                {
                    var monthDays = DateTime.DaysInMonth(order.RequestDate.Value.Year, order.RequestDate.Value.AddMonths(1).Month);
                    var nextMonth = new DateTime(order.RequestDate.Value.Year, order.RequestDate.Value.AddMonths(1).Month, Math.Min(order.RequestDate.Value.Day, monthDays));
                    
                    activateProcessDemandService.CreateActivationDemand(order.ID, order.RequestDate.Value.Date, nextMonth, required,
                        order.OfferStart.Value, false, userId);
                }
                try
                {
                    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var smsdata = context.SMSCnfgs.FirstOrDefault();
                        var mobile = order.CustomerMobile;
                        var messagetext = context.SMSCaseNotifications.FirstOrDefault(a => a.Id == 1);
                        if (Convert.ToBoolean(smsdata.sendsms))
                        {
                            if (smsdata != null && mobile != null && mobile != string.Empty && messagetext != null &&
                                Convert.ToBoolean(messagetext.Send))
                            {
                                var msg = messagetext.Message;
                                var message = SendSms.Send(smsdata.UserName, smsdata.Password, mobile, msg,
                                    smsdata.Sender, smsdata.UrlAPI);
                                var myscript = "window.open('" + message + "')";
                                if (page != null && page.Application != null && page.Title == Tokens.UpdateDemands)
                                    page.ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript,
                                        true);
                                    //System.Web.UI.ScriptManager.RegisterStartupScript();
                                else page.ClientScript.RegisterClientScriptBlock(GetType(), "myscript", myscript, true);
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
        }


        public void PayResellerDemands(List<int> demandsIds, int ? resellerId, int ? userId){
            decimal total = 0;
            foreach(var id in demandsIds){
                var demand = _ispEntries.GetDemand(id);
                if(demand == null) continue;
                total += _resellerService.DeductDemandResellerDiscount(demand);
                demand.Paid = true;
            }
            _ispEntries.Commit();
            if(resellerId == null || userId == null) return;
            _resellerService.CreateTransaction(total, Convert.ToInt32(resellerId.Value), userId.Value);
        }


        public Demand GetLastDemand(int orderId){
            return _ispEntries.OrderDemand(orderId).OrderByDescending(x => x.Id).FirstOrDefault();
        }


        public static Demand CreateNewDemandObject(Demand demand){
            return new Demand{
                Id = demand.Id,
                WorkOrder = demand.WorkOrder,
                Amount = demand.Amount,
                EndAt = demand.EndAt,
                Notes = demand.Notes,
                Offer = demand.Offer,
                OfferId = demand.OfferId,
                Paid = demand.Paid,
                PaymentComment = demand.PaymentComment,
                StartAt = demand.StartAt,
                User = demand.User,
                UserId = demand.UserId,
                WorkOrderId = demand.WorkOrderId
            };
        }


        public static Demand CreateNewDemandObject(Demand demand, WorkOrder order){
            return new Demand{
                Id = demand.Id,
                WorkOrder = order,
                Amount = demand.Amount,
                EndAt = demand.EndAt,
                Notes = demand.Notes,
                Offer = demand.Offer,
                OfferId = demand.OfferId,
                Paid = demand.Paid,
                PaymentComment = demand.PaymentComment,
                StartAt = demand.StartAt,
                User = demand.User,
                UserId = demand.UserId,
                WorkOrderId = order.ID
            };
        }
    }
}
