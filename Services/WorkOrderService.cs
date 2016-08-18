using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Services{
    public class WorkOrderService{
        readonly IspDomian _ispDomian;

        readonly ISPDataContext _context;

        public WorkOrderService(ISPDataContext context){
            _ispDomian = new IspDomian(_context);
            _context = context;
        }


        

        public DateTime ? GetActivationDate(WorkOrder order){
            var workOrderStatus = _ispDomian.WorkOrderStatus.Where(s => order != null && (s.StatusID == 6 && s.WorkOrderID == order.ID)).OrderByDescending(x => x.ID).FirstOrDefault();
            return workOrderStatus != null ? workOrderStatus.UpdateDate : null;
        }


        public List<WorkOrderRequest> ConfirmedPaymentRequests(DateTime start, DateTime end){
            return _ispDomian.WorkOrderRequests.Where(r => r.RequestID == 11 && r.RSID == 1 && r.IsTransfered == null).ToList().Where(r => r.ProcessDate != null && (r.ProcessDate.Value.Date >= start.Date && r.ProcessDate.Value.Date <= end.Date)).ToList();
        }


        public List<WorkOrderRequest> ConfirmedPaymentRequests(DateTime start, DateTime end, int filter){
            switch(filter){
                case 0 :
                    return ConfirmedPaymentRequests(start, end);
                case 1 :
                    return ActiveConfirmedPaymentRequests(start, end);
                case 2 :
                    return OutOfPeriodConfirmedPaymentRequests(start, end);
                case 3 :
                    return ConfirmedPaymentRequestsBetween15(start, end);
            }
            return null;
        }



        List<WorkOrderRequest> ActiveConfirmedPaymentRequests(DateTime start, DateTime end){
            var paymentRequests = ConfirmedPaymentRequests(start, end);
            return paymentRequests.Where(request => request.WorkOrder != null)
                .Select(request => new{
                    request,
                    order = request.WorkOrder
                })
                .Where(wor => wor.order.WorkOrderStatusID == 6)
                .Select(r => r.request).ToList();
        }


        List<WorkOrderRequest> OutOfPeriodConfirmedPaymentRequests(DateTime start, DateTime end){
            var paymentRequests = ConfirmedPaymentRequests(start, end);
            return paymentRequests.Where(request => request.WorkOrder != null)
                .Where(request => PassedOverThan15AfterActivationDate(request.WorkOrder, GetActivationDate(request.WorkOrder)))
                .ToList();
        }


        List<WorkOrderRequest> ConfirmedPaymentRequestsBetween15(DateTime start, DateTime end){
            var paymentRequests = ConfirmedPaymentRequests(start, end);
            return paymentRequests.Where(request => request.WorkOrder != null)
                .Where(request => From0To15DaysAfterActivationDate(request.WorkOrder, GetActivationDate(request.WorkOrder)))
                .ToList();
        }


        public bool PassedOverThan15AfterActivationDate(WorkOrder order, DateTime ? activationDate){
            if(activationDate == null){
                return false;
            }
            var activationValue = activationDate.Value;
            var relatedMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, activationValue.Day);
            var days = (relatedMonthDate.Date - DateTime.Now.Date).Days;
            return days > 15;
        }


        public virtual bool From0To15DaysAfterActivationDate(WorkOrder order, DateTime ? activationDate){
            if(activationDate == null){
                return false;
            }
            var activationValue = activationDate.Value;
            var relatedMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, activationValue.Day);
            var days = (relatedMonthDate.Date - DateTime.Now.Date).Days;
            return days < 15 && days >= 0;
        }


        public virtual bool CustomerHasPaid(WorkOrder order, DateTime activationDate, DateTime requestDate){
            return _ispDomian.WorkOrderRequests
                .Where(r => r.WorkOrderID == order.ID && r.RSID == 1 & r.RequestID == 11)
                .ToList().Any(r => r.ProcessDate != null
                                   && r.ProcessDate.Value.Date >= activationDate.Date
                                   && r.ProcessDate.Value.Date <= requestDate.Date);
        }


        public virtual int CustomerInStateCount(DateTime start, DateTime end, int stateId, List<WorkOrder> orders,int userId,int dataLevel){
            var context = _context;

            /*var workOrderStatuses = 
                context.WorkOrderStatus
                .ToList()
                .Where(x=> 
                    x.UpdateDate != null && 
                    x.UpdateDate.Value.Date <= end.Date && 
                    x.UpdateDate.Value.Date >= start.Date)
                .ToList();*/
            var workOrderStatuses = new List<WorkOrderStatus>();
            switch (dataLevel)
            {
                case 1:
                    workOrderStatuses = context.WorkOrderStatus.Where(x=> 
                    x.UpdateDate != null && 
                    x.UpdateDate.Value.Date <= end.Date && 
                    x.UpdateDate.Value.Date >= start.Date)
                .ToList();
                    break;
                case 2:
                    workOrderStatuses = context.WorkOrderStatus.Where(x=> 
                    x.UpdateDate != null && 
                    x.UpdateDate.Value.Date <= end.Date && 
                    x.UpdateDate.Value.Date >= start.Date &&x.WorkOrder!=null&& DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(x.WorkOrder.BranchID))
                .ToList();
                    break;
                case 3:
                    workOrderStatuses = context.WorkOrderStatus.Where(x=> 
                    x.UpdateDate != null && 
                    x.UpdateDate.Value.Date <= end.Date &&
                    x.UpdateDate.Value.Date >= start.Date && x.WorkOrder != null && x.WorkOrder.ResellerID == userId)
                .ToList();
                    break;

            }
            workOrderStatuses = stateId == 0 ?
                workOrderStatuses.Where(x => x.StatusID == 6).ToList() : workOrderStatuses.Where(x => x.StatusID == stateId).ToList();

            if(stateId==0){
                workOrderStatuses = workOrderStatuses.Where(x => x.IsNew != null && x.IsNew.Value).ToList();
            }
            return workOrderStatuses.Count;
        }
        public virtual List<WorkOrder> WorkOrdersInStateCount(DateTime start, DateTime end, int stateId, List<WorkOrder> orders){
            var context = _context;
            List<WorkOrderStatus> workOrderStatuses = 
                context.WorkOrderStatus
                .ToList()
                .Where(x=> 
                    x.UpdateDate != null && 
                    x.UpdateDate.Value.Date <= end.Date && 
                    x.UpdateDate.Value.Date >= start.Date)
                .ToList();
            workOrderStatuses = stateId == 0 ?
                workOrderStatuses.Where(x => x.StatusID == 6).ToList() 
                : workOrderStatuses.Where(x => x.StatusID == stateId).ToList();

            if(stateId==0){
                workOrderStatuses = workOrderStatuses.Where(x => x.IsNew != null && x.IsNew.Value).ToList();
            }
            var newlist =new List<WorkOrderStatus>();

            foreach (var item in orders)
            {
                var data = workOrderStatuses.Where(a => a.WorkOrderID == item.ID).ToList();
                newlist.AddRange(data);
            }
            return newlist.Select(x => x.WorkOrder).ToList();

        }


        public virtual void CreateOrders(int count, int startAt, int ? offerId){
            var startCount = count + startAt;
            for(int i = startAt;i < startCount;i++){
                var tCount = string.Format("{0}", count + startAt+i);
                var wo = new WorkOrder{
                    PersonalId = tCount,
                    CustomerName = tCount,
                    CustomerGovernorateID = 1,
                    CustomerPhone = tCount,
                    CustomerAddress = tCount,
                    CustomerMobile = tCount,
                    CustomerEmail = "a@a.a",
                    ServiceProviderID = 4,
                    ServicePackageID = 34,
                    IpPackageID = 1,
                    PaymentTypeID = 1,
                    BranchID = 1,
                    VPI = "VPI",
                    VCI = "VCI",
                    UserName = "User" + startCount,
                    Password = "pass" + startCount,
                    Notes = tCount,
                    WorkOrderStatusID = 5,
                    CreationDate = DateTime.Now.AddHours(),
                    CentralId = 1,
                    OfferId = offerId,
                    ResellerID = 2
                };
                var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                context.WorkOrders.InsertOnSubmit(wo);
                context.SubmitChanges();
                var ws = new WorkOrderStatus{
                    WorkOrderID = wo.ID,
                    StatusID =5,
                    UserID = 1,
                    UpdateDate = DateTime.Now.AddHours()
                };
                context.WorkOrderStatus.InsertOnSubmit(ws);
                context.SubmitChanges();
            }
        }


        public virtual void CreateSingleOrders(int ? offerId, int stutusId,int? resellerId){
            var lastOrder = _context.WorkOrders.OrderByDescending(x => x.ID).FirstOrDefault(); //_ispDomian.WorkOrders.OrderByDescending(x => x.ID).FirstOrDefault();
            var start = lastOrder == null ? 1 : Convert.ToInt32(lastOrder.CustomerPhone) + 1;
            var tCount = string.Format("{0}", start);
            var wo = new WorkOrder{
                PersonalId = tCount,
                CustomerName = tCount,
                CustomerGovernorateID = 1,
                CustomerPhone = tCount,
                CustomerAddress = tCount,
                CustomerMobile = tCount,
                CustomerEmail = "a@a.a",
                ServiceProviderID = 4,
                ServicePackageID = 34,
                IpPackageID = 1,
                PaymentTypeID = 1,
                BranchID = 1,
                VPI = "VPI",
                VCI = "VCI",
                UserName = "User" + start,
                Password = "pass" + start,
                Notes = tCount,
                WorkOrderStatusID = stutusId,
                CreationDate = DateTime.Now.AddHours(),
                CentralId = 1,
                ResellerID = resellerId
            };
            if(offerId != null){
                wo.OfferId = offerId;
            }
            var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            context.WorkOrders.InsertOnSubmit(wo);
            context.SubmitChanges();
            var ws = new WorkOrderStatus{
                WorkOrderID = wo.ID,
                StatusID = stutusId,
                UserID = 1,
                UpdateDate = DateTime.Now.AddHours(),
            };
            context.WorkOrderStatus.InsertOnSubmit(ws);
            context.SubmitChanges();
        }
        public virtual List<WorkOrder> WorkOrdersInStateCountactive(DateTime start, DateTime end, int stateId, List<WorkOrder> orders)
        {
            var context = _context;
            List<WorkOrderStatus> workOrderStatuses =
                context.WorkOrderStatus
                .ToList()
                .Where(x =>
                    x.UpdateDate != null &&
                    x.UpdateDate.Value.Date <= end.Date &&
                    x.UpdateDate.Value.Date >= start.Date && x.IsNew == true)
                .ToList();
            workOrderStatuses = stateId == 0 ?
                workOrderStatuses.Where(x => x.StatusID == 6).ToList()
                : workOrderStatuses.Where(x => x.StatusID == 6).ToList();

            if (stateId == 0)
            {
                workOrderStatuses = workOrderStatuses.Where(x => x.IsNew != null && x.IsNew.Value).ToList();
            }
            var newlist = new List<WorkOrderStatus>();

            foreach (var item in orders)
            {
                var data = workOrderStatuses.Where(a => a.WorkOrderID == item.ID).ToList();
                newlist.AddRange(data);
            }
            return newlist.Select(x => x.WorkOrder).ToList();

        }
    }
}
