using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using Db;
using NewIspNL;
using NewIspNL.Domain;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;


namespace BL.Concrete
{
    public class IspEntries
    {
        public readonly ISPDataContext Context;

        readonly ResellerDiscountsService _resellerDiscountsService;


        public IspEntries()
        {
            Context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            _resellerDiscountsService = new ResellerDiscountsService(Context);

        }


        public IspEntries(ISPDataContext context)
        {

            Context = context;
            _resellerDiscountsService = new ResellerDiscountsService(Context);


        }


        /*public IQueryable<Offer> Offers{
            get { return Context.Offers; }
        }*/
        public List<ProcessType> ProcessType
        {
            get { return Context.ProcessTypes.ToList(); }
        }
        public IQueryable<UserTracking> UserTrack
        {
            get { return Context.UserTrackings; }
        }
        public IQueryable<OptionInvoiceProvider> OptionInvoice
        {
            get { return Context.OptionInvoiceProviders; }
        }
        public IQueryable<ExtraGiga> ExtraGigas
        {
            get { return Context.ExtraGigas.OrderBy(x => x.Name); }
        }


        public ExtraGiga ExtraGigaById(int id)
        {
            return ExtraGigas.FirstOrDefault(x => x.Id == id);
        }


        public void AddExtraGiga(ExtraGiga giga)
        {
            if (giga.Id == 0)
            {
                Context.ExtraGigas.InsertOnSubmit(giga);
            }
        }


        public void DeleteExtraGiga(ExtraGiga giga)
        {
            Context.WorkOrderRequests.DeleteAllOnSubmit(giga.WorkOrderRequests);
            Context.ExtraGigas.DeleteOnSubmit(giga);
        }


        public bool UserHasPrivlidge(int userId, string privildige)
        {
            var user = GetUser(userId);
            if (user == null)
            {
                return false;
            }


            if (user.GroupID == null)
            {
                return false;
            }
            var groupId = user.GroupID.Value;
            var groupPrivileges = Context.GroupPrivileges.Where(gp => gp.Group.ID == groupId);
            return groupPrivileges.Any(x => x.privilege.Name.Equals(privildige));
        }

        public bool UserHasPrivlidge(List<GroupPrivilege> groupPrivileges, string privildige)
        {
            return groupPrivileges.Any(x => x.privilege.Name.Equals(privildige));
        }

        public bool UserHasPrivlidge(int userId, int privildigeId)
        {
            var user = GetUser(userId);
            if (user == null)
            {
                return false;
            }


            if (user.GroupID == null)
            {
                return false;
            }
            var groupId = user.GroupID.Value;
            var groupPrivileges = Context.GroupPrivileges.Where(gp => gp.Group.ID == groupId);
            return groupPrivileges.Any(x => x.privilege.ID.Equals(privildigeId));
        }


        public List<Demand> DemandsbyOrderId(int workOrderId)
        {
            return Demands().Where(x => x.WorkOrderId == workOrderId).ToList();
        }


        public IQueryable<Demand> OrderDemand(int workOrderId)
        {
            return Context.Demands.Where(x => x.WorkOrderId == workOrderId);
        }
        public IQueryable<Demand> OrderDemand2(int workOrderId, DateTime date)
        {

            IQueryable<Demand> a = Context.Demands.Where(x => x.WorkOrderId == workOrderId && x.StartAt <= date && x.EndAt >= date);

            return a;
        }
        public IQueryable<Demand> OrderDemand2Withoutdate(int workOrderId)
        {

            IQueryable<Demand> a = Context.Demands.Where(x => x.WorkOrderId == workOrderId);

            return a;
        }

        public UsersTransaction LastTransaction(int? orderId, int? resellerId, int? branchId)
        {
            if (orderId != null)
                return Context.UsersTransactions.Where(x => x.WorkOrderID == orderId).OrderByDescending(x => x.ID).FirstOrDefault();

            if (resellerId != null)
                return Context.UsersTransactions.Where(x => x.ResellerID == resellerId).OrderByDescending(x => x.ID).FirstOrDefault();

            return
                branchId != null ?
                    Context.UsersTransactions.Where(x => x.BranchID == branchId).OrderByDescending(x => x.ID).FirstOrDefault()
                    : null;
        }


        public List<WorkOrder> GetWorkOrderOrdersByStatusId(int statusId)
        {
            return Context.WorkOrders.Where(x => x.WorkOrderStatusID == statusId).ToList();
        }

        public void Commit()
        {
            Context.SubmitChanges();
        }


        public WorkOrder GetWorkOrder(string phone, int governorateId)
        {
            return Context.WorkOrders.FirstOrDefault(x => x.CustomerPhone.Equals(phone) && x.CustomerGovernorateID == governorateId);
        }


        public WorkOrder GetWorkOrder(string phone)
        {
            return Context.WorkOrders.FirstOrDefault(x => x.CustomerPhone.Equals(phone));
        }



        public WorkOrder GetWorkOrder(int orderId)
        {
            return Context.WorkOrders.FirstOrDefault(x => x.ID == orderId);
        }
        public List<WorkOrder> GetWorkOrderByExtraGiga(int extraGiga)
        {
            return Context.WorkOrders.Where(x => x.ExtraGigaId == extraGiga).ToList();
        }

        public Demand GetDemand(int demandId)
        {
            return Context.Demands.FirstOrDefault(x => x.Id == demandId);
        }


        /*public decimal GetResellerDiscount(int ? resellerId, int ? serviceProviderId, int ? servicePackageId){
            if(resellerId == null || serviceProviderId == null || servicePackageId == null)
                return 0;

            return _resellerDiscountsService.GetResellerPackageDiscountPercent(resellerId.Value, serviceProviderId.Value, servicePackageId.Value, true);
        }*/


        public List<IpPackage> IpPackages()
        {
            return Context.IpPackages.ToList();
        }


        public List<WorkOrder> WorkOrdersOfReseller(int resellerId)
        {
            return Context.WorkOrders.Where(x => x.ResellerID == resellerId).ToList();
        }


        public List<Governorate> Cities()
        {
            return Context.Governorates.ToList();
        }


        public WorkOrderStatus GetLastRequest(int requestId, int orderId)
        {
            return Context.WorkOrderStatus.Where(x => x.WorkOrderID == orderId && x.StatusID == requestId).OrderByDescending(x => x.ID).FirstOrDefault();
        }


        public List<ServicePackage> ServicePackages()
        {
            return Context.ServicePackages.ToList();
        }


        public List<PaymentType> PaymentTypes()
        {
            return Context.PaymentTypes.ToList();
        }

        public PaymentType PaymentType(int paymentTypeId)
        {
            return Context.PaymentTypes.FirstOrDefault(a => a.ID == paymentTypeId);
        }


        public List<ServiceProvider> ServiceProviders()
        {
            return Context.ServiceProviders.ToList();
        }


        public List<User> Users()
        {
            return Context.Users.ToList();
        }


        public void SaveTransaction(UsersTransaction transaction)
        {
            if (transaction.ID == 0)
            {
                Context.UsersTransactions.InsertOnSubmit(transaction);
            }
        }

        public void SaveRequest(WorkOrderRequest request)
        {
            if (request.ID == 0)
            {
                Context.WorkOrderRequests.InsertOnSubmit(request);
            }
        }

        public void SendNotes(int orderId, DateTime date, string notes, int noteUserId)
        {
            var hintService = new OrderHintService(Context);
            hintService.Submit(orderId, false, 1, date, notes, noteUserId, true);
        }
        public List<WorkOrder> ClintsOfProvider(int providerId)
        {
            return Context.WorkOrders.Where(w => w.ServiceProviderID == providerId).ToList();
        }




        public DateTime? GetActivationDate(WorkOrder order, ISPDataContext context)
        {
            var workOrderStatusOfctivationDate = context.WorkOrderStatus.Where(w
                =>
                w.WorkOrderID == order.ID
                && w.StatusID == 6).OrderBy(x => x.UpdateDate).FirstOrDefault();
            return workOrderStatusOfctivationDate != null ? workOrderStatusOfctivationDate.UpdateDate : null;
        }


        public DateTime? GetNextMonthRequestDate(WorkOrder order, ISPDataContext context)
        {
            var activationDate = GetActivationDate(order, context);
            if (activationDate != null)
            {
                var nextMonth = new DateTime(DateTime.Now.AddHours().Year, DateTime.Now.AddHours().AddMonths(1).Month, activationDate.Value.Day);
                return nextMonth;
            }
            return null;
        }


        public User GetUser(int userId)
        {
            return Context.Users.FirstOrDefault(u => u.ID == userId);
        }


        /* public List<User> ResellersByUserId(int userId){
             var id = userId;
             var user = Context.Users.FirstOrDefault(u => u.ID == id);
             if(user != null){
                 switch(user.GroupID){
                     case 1 : // admin
                         return GetResellers(null);

                     case 6 : // reseller
                         return Context.Users.Where(u => u.ID == id).ToList();

                     default :
                         return GetResellers(id);
                 }
             }
             return null;
         }*/

        public List<RejectionReason> RejectionReasons()
        {
            return Context.RejectionReasons.ToList();
        }


        IQueryable<Governorate> GetGovernorates()
        {
            return Context.Governorates;
        }



        public void AddDemands(List<Demand> demands)
        {
            Context.Demands.InsertAllOnSubmit(demands);
        }


        public void AddDemands(Demand demands)
        {
            if (demands.Id == 0)
            {
                Context.Demands.InsertOnSubmit(demands);
            }
        }


        public WorkOrderRequest GetLastOrderRequest(int requestId, int orderId)
        {
            return Context.WorkOrderRequests.Where(x => x.WorkOrderID == orderId && x.RequestID == requestId).OrderByDescending(x => x.ID).FirstOrDefault();
        }

        public IQueryable<WorkOrderRequest> GetOrderRequest(int workOrrderId)
        {
            return Context.WorkOrderRequests.Where(a => a.WorkOrderID == workOrrderId && a.RSID == 3);
        }
        public List<Demand> DemandsbyOrderId(int workOrderId, bool? paid = null)
        {
            var list = Context.Demands.Where(x => x.WorkOrderId == workOrderId).ToList();
            return paid == null ? list : (paid == true ? list.Where(x => x.Paid).ToList() : list.Where(x => !x.Paid).ToList());
        }


        public List<Demand> Demands()
        {
            return Context.Demands.ToList();
        }

        public Option GetOption()
        {
            return Context.Options.FirstOrDefault();
        }

        public void ManageFawry()
        {
            try
            {
                var option = GetOption();
                if (!Convert.ToBoolean(option.FawryService)) return;
                var fawryList = Context.Fawries.Where(a => a.IsNew == false || a.IsNew == null).ToList();
                var now = DateTime.Now.AddHours();

                foreach (var item in fawryList)
                {
                    try
                    {
                        var orderId = new int();
                        if (item.FK_CustomerID == null || item.FK_CustomerID == 0)
                        {
                            orderId = Context.WorkOrders.FirstOrDefault(x => x.CustomerPhone == item.BillingAccount).ID;
                        }
                        else
                        {
                            orderId = Convert.ToInt32(item.FK_CustomerID);
                        }

                        //var orderId = Convert.ToInt32(item.FK_CustomerID);
                        var oldRequests = GetOrderRequest(orderId);
                        var order = GetWorkOrder(orderId);
                        switch (order.WorkOrderStatusID)
                        {
                            case 11:
                                // if suspend, create Ususpend Request
                                if (oldRequests.Any(a => a.RequestID == 3))
                                    continue; //if he has any old Requests return

                                //-------------------- send un suspend to portal
                                var portalList = Context.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                                var woproviderList = Context.WorkOrders.FirstOrDefault(z => z.ID == order.ID);
                                if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                                {
                                    try
                                    {
                                        var note = "";
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
                                                    if (custStatus == "enable")
                                                    {
                                                        // "هذا العميل مفعل بالفعل على البورتال";
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        var worNote = Tedata.SendTedataUnSuspendRequest(username,
                                                            cookiecon, pagetext);
                                                        if (worNote == 2)
                                                        {
                                                            //"تعذر الوصول الى البورتال";
                                                            note =
                                                                "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                                            //ينزل الطلب معلق فى اى اس بى
                                                        }
                                                        else
                                                        {
                                                            //ينزل الطلب متوافق علية فى اى اس بى

                                                            var work = new WorkOrderRequest
                                                            {
                                                                WorkOrderID = order.ID,
                                                                CurrentPackageID = order.ServicePackageID,
                                                                NewPackageID = order.ServicePackageID,
                                                                ExtraGigaId = order.ExtraGigaId,
                                                                RequestDate = now,
                                                                RequestID = 3,
                                                                RSID = 1,
                                                                SenderID = 1,
                                                                ConfirmerID = 1,
                                                                ProcessDate = DateTime.Now.AddHours(),
                                                                Notes = "تم تشغيل العميل بعد اتمام عملية دفع من فورى"
                                                            };
                                                            Context.WorkOrderRequests.InsertOnSubmit(work);
                                                            Context.SubmitChanges();
                                                            
                                                            //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                            var current =
                                                                Context.WorkOrders.FirstOrDefault(x => x.ID == order.ID);

                                                            if (current != null)
                                                            {
                                                                current.WorkOrderStatusID = 6;

                                                                global::Db.WorkOrderStatus wos = new global::Db.
                                                                    WorkOrderStatus
                                                                {
                                                                    WorkOrderID = current.ID,
                                                                    StatusID = 6,
                                                                    UserID = 1,
                                                                    UpdateDate = DateTime.Now.AddHours(),
                                                                };
                                                                Context.WorkOrderStatus.InsertOnSubmit(wos);
                                                            }
                                                            Context.SubmitChanges();
                                                            // "تم إرسال الطلب الى البورتال بنجاح";     

                                                            // ترحيل ايام السسبند
                                                            int daysCount = DaysForCustomerAtStatus(order.ID, 11);
                                                            if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                                            {
                                                                order.RequestDate.Value.AddDays(daysCount);
                                                                Commit();
                                                            }

                                                            break;
                                                        }
                                                    }

                                                }


                                            }

                                        }


                                    }
                                    catch
                                    {
                                      continue;

                                    }
                                }

                                try
                                {

                                //-------------------------
                                var wor = new WorkOrderRequest
                                {
                                    WorkOrderID = order.ID,
                                    CurrentPackageID = order.ServicePackageID,
                                    NewPackageID = order.ServicePackageID,
                                    ExtraGigaId = order.ExtraGigaId,
                                    RequestDate = now,
                                    RequestID = 3,
                                    RSID = 3,
                                    SenderID = 1,
                                    Notes = "تم عمل هذا الطلب من السيستم بعد اتمام عملية دفع من فورى بسبب عدم إستجابة البورتال"
                                };
                                Context.WorkOrderRequests.InsertOnSubmit(wor);
                                Context.SubmitChanges();
                                break;
                                }
                                catch
                                {
                                    continue;

                                }
                            case 8:
                            case 9:
                                try
                                {

                               
                                //if Cancellation Process and Cancelled, Create ReActive Request
                                if (oldRequests.Any(a => a.RequestID == 7)) continue;
                                var wor2 = new WorkOrderRequest
                                {
                                    WorkOrderID = order.ID,
                                    CurrentPackageID = order.ServicePackageID,
                                    NewPackageID = order.ServicePackageID,
                                    ExtraGigaId = order.ExtraGigaId,
                                    RequestDate = now,
                                    RequestID = 7,
                                    RSID = 3,
                                    SenderID = 1,
                                    Notes = "تم عمل هذا الطلب من السيستم بعد اتمام عملية دفع من فورى"
                                };
                                Context.WorkOrderRequests.InsertOnSubmit(wor2);
                                Context.SubmitChanges();
                                break;
                                }
                                catch
                                {
                                    continue;

                                }
                            case 6:
                                try
                                {

                              
                                //if Active and have suspend request, reject it
                                if (oldRequests.Any())
                                {
                                    var suspendRequest = oldRequests.FirstOrDefault(a => a.RequestID == 2);
                                    if (suspendRequest != null)
                                    {
                                        const string msg = "تم رفض طلب السسبند للعميل بعد عملية دفع من فورى";
                                        suspendRequest.RSID = 2;
                                        suspendRequest.RejectReason = msg;
                                        suspendRequest.ConfirmerID = 1;
                                        suspendRequest.ProcessDate = now;
                                        Context.SubmitChanges();

                                        var workOrderStatuses =
                                            Context.WorkOrderStatus.Where(
                                                wost => wost.WorkOrderID == suspendRequest.WorkOrderID).ToList();
                                        var statusId = workOrderStatuses.Last().StatusID;
                                        if (statusId != null)
                                        {
                                            var lastStatusId = statusId.Value;
                                            var currentWorkOrder =
                                                Context.WorkOrders.FirstOrDefault(
                                                    wo => wo.ID == suspendRequest.WorkOrderID);
                                            if (currentWorkOrder != null)
                                            {
                                                currentWorkOrder.WorkOrderStatusID = lastStatusId;
                                                //var option = GetOptions();
                                                if (Convert.ToBoolean(option.SendMessageAfterOperations))
                                                {
                                                    CenterMessage.SendRequestReject(currentWorkOrder, msg,
                                                        suspendRequest.Request.RequestName, 1);
                                                }
                                            }
                                        }
                                        Context.SubmitChanges();
                                    }
                                }
                                break;
                                }
                                catch
                                {
                                    continue;

                                }

                        }

                        var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                        var type = context2.Options.FirstOrDefault();

                        if (type.FawryType == "AllDemad")
                        {
                            //get all unpaid Demand to this user 
                            var unpaiddemonds =
                            Context.Demands.Where(z => z.WorkOrderId == orderId && z.Paid != true)
                                .Select(z => z)
                                .ToList();
                            foreach (var unpaiddemond in unpaiddemonds)
                            {
                                unpaiddemond.Paid = true;
                                unpaiddemond.PaymentDate = now;
                                unpaiddemond.PaymentComment = "تم الدفع من خلال خدمة فورى";
                                unpaiddemond.UserId = 1;
                                item.IsNew = true;
                            }
                            Context.SubmitChanges();

                        }
                        else if (type.FawryType == "FirstDemandUnPaid")
                        {
                            //get First unpaid Demand to this user 
                            var unpaidFirstdemonds =
                            Context.Demands.Where(z => z.WorkOrderId == orderId && z.Paid != true)
                                .Select(z => z)
                                .FirstOrDefault();
                            if (unpaidFirstdemonds == null) continue;
                            unpaidFirstdemonds.Paid = true;
                            unpaidFirstdemonds.PaymentDate = now;
                            unpaidFirstdemonds.PaymentComment = "تم الدفع من خلال خدمة فورى";
                            unpaidFirstdemonds.UserId = 1;
                            item.IsNew = true;

                            Context.SubmitChanges();

                        }

                        else if (type.FawryType == "LastDemondUnPaid")
                        {
                            //get last unpaid Demand to this user 
                            var unpaidLastdemonds1 =
                           Context.Demands.Where(z => z.WorkOrderId == orderId && z.Paid != true)
                               .Select(z => z)
                               .ToList();
                            var unpaidLastdemonds = unpaidLastdemonds1.LastOrDefault();
                            if (unpaidLastdemonds == null) continue;
                            unpaidLastdemonds.Paid = true;
                            unpaidLastdemonds.PaymentDate = now;
                            unpaidLastdemonds.PaymentComment = "تم الدفع من خلال خدمة فورى";
                            unpaidLastdemonds.UserId = 1;
                            item.IsNew = true;

                            Context.SubmitChanges();
                        }















                        //get the last Demand to this user 
                        //var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                        //var cmd = new SqlCommand("PROC_GETDemand", connection)
                        //{
                        //    CommandType = CommandType.StoredProcedure
                        //};
                        //cmd.Parameters.Add(new SqlParameter("@CustomerPhone", SqlDbType.NVarChar)).Value =
                        //    order.CustomerPhone;


                        //connection.Open();
                        //var table = new DataTable();
                        //table.Load(cmd.ExecuteReader());
                        //connection.Close();
                        //var lastDemand = table.AsEnumerable().Select(dr => new
                        //{
                        //    WorkOrderId = dr.Field<int>("ID"),
                        //    CustomerName = dr.Field<string>("CustomerName"),
                        //    CustomerPhone = dr.Field<string>("CustomerPhone"),
                        //    Amount = dr.Field<decimal>("Amount"),
                        //    StartAt = dr.Field<DateTime>("StartAt"),
                        //    EndAt = dr.Field<DateTime>("EndAt"),
                        //    Paid = dr.Field<bool>("Paid"),
                        //    demandId = dr.Field<int>("Expr1"),
                        //}).FirstOrDefault();
                        //if (lastDemand == null) continue;
                        //var demand = GetDemand(Convert.ToInt32(lastDemand.demandId));
                        //if (demand == null) continue;
                        //demand.Paid = true;
                        //demand.PaymentDate = now;
                        //demand.PaymentComment = "تم الدفع من خلال خدمة فورى";
                        //demand.UserId = 1;
                        //item.IsNew = true;
                        //Commit();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public WorkOrderStatus LastWorkOrderStatus(int orderId, int requestId, DateTime? startAt, DateTime? endAt)
        {
            var requests = Context.WorkOrderStatus.Where(x => x.WorkOrderID == orderId && x.StatusID == requestId).OrderByDescending(x => x.ID).ToList();
            if (startAt != null)
            {
                requests = requests.Where(x => x.UpdateDate != null && x.UpdateDate.Value.Date >= startAt.Value.Date).ToList();
            }
            if (endAt != null)
            {
                requests = requests.Where(x => x.UpdateDate != null && x.UpdateDate.Value.Date <= endAt.Value.Date).ToList();
            }
            return requests.Any() ? requests.OrderByDescending(x => x.ID).FirstOrDefault() : null;
        }


        public List<WorkOrderRequest> GetOrderRequests(int orderId)
        {
            var workOrderRequests = Context.WorkOrderRequests.Where(x => x.WorkOrderID == orderId).ToList();
            return workOrderRequests;
        }


        public List<ExtraGigaDetailModel> ExtraGigasToPreview()
        {
            return ExtraGigas.Select(ToExtraGigaDetail).ToList();
        }


        ExtraGigaDetailModel ToExtraGigaDetail(ExtraGiga arg)
        {
            return new ExtraGigaDetailModel
            {
                Name = arg.Name,
                Price = Helper.FixNumberFormat(arg.Price),
                CanDelete = !arg.WorkOrders.Any(),
                Id = arg.Id
            };
        }


        public void PopulateExtraGigas(DropDownList ddl)
        {
            ddl.DataSource = Context.ExtraGigas.OrderBy(x => x.Name);
            ddl.DataTextField = "Name";
            ddl.DataValueField = "Id";
            ddl.DataBind();
            /*Helper.AddDefaultItem(ddl);*/
        }


        public void AddStatus(WorkOrderStatus status)
        {
            Context.WorkOrderStatus.InsertOnSubmit(status);
        }


        public void AddDemand(Demand demand)
        {
            Context.Demands.InsertOnSubmit(demand);
        }


        public Status GetStatus(int i)
        {
            return Context.Status.FirstOrDefault(x => x.ID == i);
        }


        public bool DeleteDemand(int demandId, bool updateOrderRequestDate = false)
        {
            try
            {
                var demand = Context.Demands.FirstOrDefault(x => x.Id == demandId);

                if (demand != null)
                {
                    //-----
                    AddUserTrack(new UserTracking()
                    {
                        WorkOrderId = demand.WorkOrderId,
                        Date = DateTime.Now.AddHours(),
                        // process type 4 for delete demand from ProcessType table
                        ProcessTypeId = 4,
                        UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]),
                        Note = " حذف فاتورة قيمة الفاتورة " + demand.Amount + " تاريخ بداية المطالبة " + demand.StartAt + " تاريخ انتهاء المطالبة  " + demand.EndAt 
                    });

                    var dlt = Context.PayingCustomersResellers.FirstOrDefault(x => x.DemandId == demand.Id);
                        if (dlt != null)
                        {
                            Context.PayingCustomersResellers.DeleteOnSubmit(dlt);
                            Context.SubmitChanges();
                        }

                   
                    //-----/----/-
                    if (demand.WorkOrder.Demands.Count > 1)
                    {
                        if (updateOrderRequestDate)
                        {
                            demand.WorkOrder.RequestDate = demand.StartAt;
                        }
                        Context.Demands.DeleteOnSubmit(demand);


                       
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public List<User> ActiveUserUsers(int activeUserId)
        {
            var user = GetUser(activeUserId);
            if (user == null || user.GroupID == null) return null;
            var branchUsers = new List<User>();
            switch (user.GroupID.Value)
            {
                case 1:
                    return Context.Users.ToList();
                case 4:
                    var branches = user.UserBranches.ToList();
                    if (branches.Count == 0)
                    {
                        return Context.Users.Where(x => x.BranchID == user.BranchID).ToList();
                    }
                    foreach (var branch in branches)
                    {
                        branchUsers.AddRange(branch.Branch.Users);
                    }
                    return branchUsers;
                default:
                    branchUsers.Add(user);
                    return branchUsers;
            }
        }


        public int DaysForCustomerAtStatus(int orderId, int statusId)
        {
            var status = Context.WorkOrderStatus
                .Where(x => x.WorkOrderID == orderId && x.StatusID == statusId)
                .OrderByDescending(x => x.ID).FirstOrDefault();
            if (status == null)
            {
                return 0;
            }
            if (status.UpdateDate == null)
            {
                return 0;
            }
            return (DateTime.Now.AddHours() - status.UpdateDate.Value.Date).Days;
        }
        public Dictionary<int, string> ListOfDaysForCustomerAtStatus(int statusId, List<ManageRequestTemplate> requests)
        {
            /* var status = Context.WorkOrderStatus
                 .Where(x => x.StatusID == statusId)
                 .OrderByDescending(x => x.ID).ToList();*/
            var status = requests;
            var numbers = new Dictionary<int, string>();
            foreach (var workOrderStatuse in status)
            {
                /* var n = (DateTime.Now.AddHours() - workOrderStatuse.UpdateDate.Value.Date).Days;*/
                var n = DaysForCustomerAtStatus(workOrderStatuse.WorkOrderID, 11);
                var value = n.ToString(CultureInfo.InvariantCulture);
                if (!numbers.ContainsValue(value)) numbers.Add(workOrderStatuse.WorkOrderID, value);
            }
            return numbers;

        }
        public void AddUserTrack(UserTracking track)
        {
            if (track.Id == 0)
            {
                Context.UserTrackings.InsertOnSubmit(track);
            }
        }

        /*public class DaysCount
        {
            public int Number { get; set; }
            public int Index { get; set; }
        }*/
    }
}
