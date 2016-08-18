using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Domain.Concrete{
    public class WorkOrderRepository : IWorkOrderRepository{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        public WorkOrderRepository() {}


        public WorkOrderRepository(ISPDataContext context){
            _context = context;
        }


        #region IWorkOrderRepository Members


        public WorkOrder Get(int id){
            
            return _context.WorkOrders.FirstOrDefault(w => w.ID == id);
        }


        public IQueryable<WorkOrder> WorkOrders{
            get { return _context.WorkOrders; }
        }


        /*public IQueryable<WorkOrder> WorkOrdersByCentralId(int centralId){
            return _context.WorkOrders.Where(w => w.CentralId == centralId);
        }*/


        public DateTime ? GetActivationDate(int workOrderId, int statusId = 6){
            var firstStatusRecord =
                _context.WorkOrderStatus.Where(s => s.WorkOrderID == workOrderId && s.StatusID == statusId).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
            return firstStatusRecord == null ? null : firstStatusRecord.UpdateDate;
        }


        public DateTime ? GetActivationDate(int workOrderId){
            var firstStatusRecord =
                _context.WorkOrderStatus.Where(s => s.WorkOrderID == workOrderId && s.StatusID==6).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
            return firstStatusRecord == null ? null : firstStatusRecord.UpdateDate;
        }


        /*public string GetCentral(WorkOrder order){
            return order.Central == null ? "-" : order.Central.Name == null ? "-" : order.Central.Name;
        }*/


        public bool Delete(int orderId){
            var order = Get(orderId);
            if(order == null){
                return true;
            }

            if(order.WorkOrderRequests.Any(x => x.RSID == 3)){
                return true;
            }

           
            _context.ResellerRouters.DeleteAllOnSubmit(order.ResellerRouters);
            _context.SubmitChanges();

            _context.ResellerRouters.DeleteAllOnSubmit(order.ResellerRouters);
            _context.SubmitChanges();

            _context.Routers.DeleteAllOnSubmit(order.Routers);
            _context.SubmitChanges();

            _context.Tickets.DeleteAllOnSubmit(order.Tickets);
            _context.SubmitChanges();

            foreach(var t in order.UsersTransactions){
                _context.Receipts.DeleteAllOnSubmit(t.Receipts);
            }
            _context.SubmitChanges();

            _context.UsersTransactions.DeleteAllOnSubmit(order.UsersTransactions);
            _context.SubmitChanges();

            _context.WorkOrderFiles.DeleteAllOnSubmit(order.WorkOrderFiles);
            _context.SubmitChanges();
            _context.WorkOrderHistories.DeleteAllOnSubmit(order.WorkOrderHistories);
            _context.SubmitChanges();
            _context.WorkOrderRequests.DeleteAllOnSubmit(order.WorkOrderRequests);
            _context.SubmitChanges();

            _context.WorkOrderRouters.DeleteAllOnSubmit(order.WorkOrderRouters);
            _context.SubmitChanges();

            _context.WorkOrderStatus.DeleteAllOnSubmit(order.WorkOrderStatus);
            _context.SubmitChanges();

            _context.WorkOrderNotes.DeleteAllOnSubmit(order.WorkOrderNotes);
            _context.SubmitChanges();

            
            //-----
            var dm = _context.Demands.Where(x => x.WorkOrderId == order.ID).ToList();
            foreach (var d in dm)
            {
                var dlt = _context.PayingCustomersResellers.FirstOrDefault(x => x.DemandId == d.Id);
                if (dlt != null)
                {
                    _context.PayingCustomersResellers.DeleteOnSubmit(dlt);
                    _context.SubmitChanges();
                }

            }
            //-----/----/-
            foreach (var tt in order.Demands)
            {
                _context.DebtsInvoices.DeleteAllOnSubmit(tt.DebtsInvoices);
            }
            _context.SubmitChanges();
            _context.RequestsNotitfications.DeleteAllOnSubmit(order.RequestsNotitfications);
            _context.SubmitChanges();
            foreach (var tt in order.Demands)
            {
                _context.ResellerUnpaidDemandComments.DeleteAllOnSubmit(tt.ResellerUnpaidDemandComments);
           
            
            }
            _context.SubmitChanges();


            foreach (var tt in order.Demands)
            {
                _context.BranchInvoiceComments.DeleteAllOnSubmit(tt.BranchInvoiceComments);


            }
            _context.SubmitChanges();



            _context.Demands.DeleteAllOnSubmit(order.Demands);
            _context.SubmitChanges();

            _context.RequestDateHistories.DeleteAllOnSubmit(order.RequestDateHistories);
            _context.SubmitChanges();

            _context.CallMessages.DeleteAllOnSubmit(order.CallMessages);
            _context.SubmitChanges();
            //-----
            _context.WorkOrderCredits.DeleteAllOnSubmit(order.WorkOrderCredits);
            _context.SubmitChanges();

            _context.RecieveRouters.DeleteAllOnSubmit(order.RecieveRouters);
            _context.SubmitChanges();
          
            //--/--/-
            _context.WorkOrders.DeleteOnSubmit(order);
            _context.SubmitChanges();

            var hist = new DeletedCustomersHistory()
            {
                DeleteDate = DateTime.Now.AddHours(),
                Phone = order.CustomerPhone,
                UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"])
            };
            _context.DeletedCustomersHistories.InsertOnSubmit(hist);
            _context.SubmitChanges();
        
            
            return false;
        }


        #endregion


        public List<WorkOrder> GetUserWorkOrders(int userId){
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            if(user == null){
                return null;
            }
            if(user.GroupID == null){
                return null;
            }
            switch(user.GroupID.Value){
                case 1 :
                    return _context.WorkOrders.ToList();
                case 4 :
                    var branches = user.UserBranches.ToList();
                    var orders = new List<WorkOrder>();
                    if (branches.Count>0)
                    {
                    foreach(var branch in branches){
                        var branch1 = branch;
                        orders.AddRange(_context.WorkOrders.Where(x => x.BranchID == branch1.BranchID));
                    }
                    }
                    else
                    {
                        orders = _context.WorkOrders.Where(x => x.BranchID == user.BranchID.Value).ToList();
                    }
                    return orders;
                case 6 :
                    return _context.WorkOrders.Where(x => x.ResellerID == userId).ToList();
                default :
                    return _context.WorkOrders.Where(x => x.BranchID == user.BranchID.Value).ToList();
            }
        }


        public static DateTime ? GetActivationDate(int workOrderId, ISPDataContext context){
            var firstStatusRecord =
                context.WorkOrderStatus.Where(s => s.WorkOrderID == workOrderId).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
            return firstStatusRecord == null ? null : firstStatusRecord.UpdateDate;
        }


        public static OrderBasicData GetOrderBasicData(WorkOrder order, ISPDataContext context){
            if(order != null){
                var resellerId = 0;
                var resellerName = "";
                if(order.ResellerID != null){
                    var reseller = context.Users.FirstOrDefault(u => u.ID == order.ResellerID);
                    if(reseller != null){
                        resellerName = reseller.UserName;
                        resellerId = reseller.ID;
                    }
                }
                var activationDate = GetActivationDate(order.ID, context);

                var orderBasicData = new OrderBasicData{
                    Id = order.ID,
                    Branch = order.Branch == null ? "-" : order.Branch.BranchName,
                    BranchId = order.BranchID == null ? 0 : order.BranchID.Value,
                    Central = order.Central == null ? "-" : order.Central.Name,
                    CentralId = order.CentralId == null ? 0 : order.CentralId.Value,
                    Govornorate = order.Governorate.GovernorateName,
                    GovornorateId = order.CustomerGovernorateID != null ? order.CustomerGovernorateID.Value : 0,
                    Reseller = resellerName,
                    ResellerId = resellerId,
                    ActivationDate = activationDate == null ? default(DateTime) : activationDate.Value,
                    TActivationDate = activationDate == null ? "-" : activationDate.Value.ToShortDateString(),
                    Offer = order.Offer == null ? "-" : order.Offer.Title,
                    Package = order.ServicePackage == null ? "-" : order.ServicePackage.ServicePackageName,
                    Provider = order.ServiceProvider == null ? "-" : order.ServiceProvider.SPName,
                    State = order.Status == null ? "-" : order.Status.StatusName,
                    Installer = order.User1 == null ? "-" : order.User1.UserName,

                    Customer = order.CustomerName,
                    Phone = order.CustomerPhone,
                    InstallationTime = order.InstallationTime != null ? order.InstallationTime.Value.ToDateTime() : "",
                    Note = order.Notes
                };
                return orderBasicData;
            }
            return null;
        }


        public List<WorkOrder> OrderbyActivationDate(List<WorkOrder> list, DateTime activationDate){
            var inDay = new List<WorkOrder>();
            foreach(var order in list){
                var activeAt = GetActivationDate(order.ID, _context);
                if(activeAt == null) continue;
                if(activeAt.Value.Date.Equals(activationDate.Date)){
                    inDay.Add(order);
                }
            }
            return inDay;
        }
    }



    public class OrderBasicData{
        public string Reseller { get; set; }

        public int ? ResellerId { get; set; }

        public int BranchId { get; set; }

        public string Branch { get; set; }

        public string Central { get; set; }

        public int CentralId { get; set; }

        public int GovornorateId { get; set; }

        public string Govornorate { get; set; }

        public DateTime ActivationDate { get; set; }

        public string TActivationDate { get; set; }

        public int Id { get; set; }

        public string Offer { get; set; }

        public string Package { get; set; }

        public string Provider { get; set; }

        public string State { get; set; }

        public string Installer { get; set; }

        public string InstallationTime { get; set; }

        public string Customer { get; set; }

        public string Phone { get; set; }
        public string Note { get; set; }
    }
}
