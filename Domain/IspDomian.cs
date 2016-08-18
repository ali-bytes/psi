using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Domain{
    public class IspDomian{
        readonly ISPDataContext _context;

        readonly IOfferRepository _offerRepository = new OfferRepository();


        public IspDomian(ISPDataContext context){
            _context = context;
        }


        /*public IQueryable<AutomatedProcess> AutomatedProcesses{
            get { return _context.AutomatedProcesses; }
        }*/



        public IQueryable<Status> Statuses{
            get { return _context.Status; }
        }


        public IQueryable<Status> StatusesActiveNewCustomer{
            get { return _context.Status.Where(s => s.ID == 1 || s.ID == 6); }
        }



        public IQueryable<Central> Centrals{
            get { return _context.Centrals; }
        }


        public IQueryable<Governorate> Governorates{
            get { return _context.Governorates; }
        }


        public IQueryable<Offer> Offers{
            get { return _context.Offers; }
        }


        public IQueryable<WorkOrder> WorkOrders{
            get { return _context.WorkOrders; }
        }


        public IQueryable<User> Users{
            get { return _context.Users; }
        }



        public virtual List<User> Resellers{
            get { return DataLevelClass.GetUserReseller(); }
        }


        public IQueryable<UsersTransaction> UsersTransactions{
            get { return _context.UsersTransactions; }
        }


        public IQueryable<WorkOrderRequest> WorkOrderRequests{
            get { return _context.WorkOrderRequests; }
        }


        public IQueryable<WorkOrderStatus> WorkOrderStatus{
            get { return _context.WorkOrderStatus; }
        }


        public IQueryable<IpPackage> IpPackages{
            get { return _context.IpPackages; }
        }


        public IQueryable<Branch> Branches{
            get { return _context.Branches; }
        }


        public IQueryable<ServiceProvider> Providers{
            get { return _context.ServiceProviders; }
        }


        public IQueryable<ServicePackage> Packages{
            get { return _context.ServicePackages; }
        }


        public IQueryable<ExtraGiga> ExtraGigas{
            get { return _context.ExtraGigas.OrderBy(x => x.Name); }
        }


        public virtual void PopulateResellers(DropDownList dropDownList, bool isUserRelated){
            dropDownList.AppendDataBoundItems = true;
            //Helper.AddDropDownItem(dropDownList, dropDownList.Items.Count, "Direct User");//
            dropDownList.DataSource = DataLevelClass.GetUserReseller();
            dropDownList.DataTextField = "UserName";
            dropDownList.DataValueField = "ID";
           
            dropDownList.DataBind();
            Helper.AddDefaultItem(dropDownList);
        }
        public virtual void PopulateResellerswithDirectUser(DropDownList dropDownList, bool isUserRelated)
        {
            dropDownList.AppendDataBoundItems = true;
            Helper.AddDropDownItem(dropDownList, dropDownList.Items.Count, "Direct User");//
            dropDownList.DataSource = DataLevelClass.GetUserReseller();
            dropDownList.DataTextField = "UserName";
            dropDownList.DataValueField = "ID";

            dropDownList.DataBind();
            Helper.AddDefaultItem(dropDownList);
        }

        public virtual void PopulateExtraGigas(DropDownList dropDownList)
        {
            dropDownList.DataSource = ExtraGigas;//_context.ExtraGigas.OrderBy(x => x.Name);
            dropDownList.DataTextField = "Name";
            dropDownList.DataValueField = "Id";
            dropDownList.DataBind();
            Helper.AddDefaultItem(dropDownList);
            
        }


        public virtual void PopulateBoxes(DropDownList dropDown){
            dropDown.DataSource = _context.Boxes;
            dropDown.DataTextField = "BoxName";
            dropDown.DataValueField = "ID";
            dropDown.DataBind();
            Helper.AddDefaultItem(dropDown);
        }


       /* public decimal GetServicePackageDiscount(int offerId, int packageId){
            return 0;
        }*/


        public virtual void PopulateIpPackages(DropDownList ddl, string defaultItem="")
        {
            ddl.DataSource = IpPackages;
            ddl.DataTextField = "IpPackageName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            if (defaultItem.Equals(""))
            {
                Helper.AddDefaultItem(ddl);
            }
            else
            {
                Helper.AddDefaultItem(ddl, defaultItem);
            }
        }


        public virtual void PopulateOffers(DropDownList ddl, int userId){
            ddl.DataSource = _offerRepository.GetOffersByUser(userId);
            ddl.DataTextField = "Title";
            ddl.DataValueField = "Id";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }


        public virtual void PopulateStatuses(DropDownList ddl){
            ddl.DataSource = Statuses;
            ddl.DataTextField = "StatusName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }


        public virtual void PopulateBranches(object ddl){
            var dropDownList = ddl as DropDownList;
            if(dropDownList != null){
                dropDownList.DataSource = Branches;
                dropDownList.DataTextField = "BranchName";
                dropDownList.DataValueField = "ID";
                dropDownList.DataBind();
                Helper.AddDefaultItem(dropDownList);
            }

            var rbl = ddl as CheckBoxList;
            if(rbl != null){
                rbl.DataSource = Branches;
                rbl.DataTextField = "BranchName";
                rbl.DataValueField = "ID";
                rbl.DataBind();
            }
        }



        public virtual void PopulateBranches(DropDownList ddl, bool checkLevel){

            ddl.DataSource = DataLevelClass.GetUserBranches();
            ddl.DataTextField = "BranchName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }


        public virtual void PopulateGovernorates(DropDownList ddl){
            ddl.DataSource = Governorates;
            ddl.DataTextField = "GovernorateName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }


        public virtual void PopulatePackages(DropDownList ddl, string defaultItem="")
        {
            ddl.DataSource = Packages;
            ddl.DataTextField = "ServicePackageName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            if (defaultItem.Equals(""))
            {
                Helper.AddDefaultItem(ddl);
            }
            else
            {
                Helper.AddDefaultItem(ddl, defaultItem);
            }
        }


        public virtual void PopulateProviders(DropDownList dropDownList){
            dropDownList.DataSource = Providers;
            dropDownList.DataTextField = "SPName";
            dropDownList.DataValueField = "ID";
            dropDownList.DataBind();
            Helper.AddDefaultItem(dropDownList);
        }
        public virtual void PopulateProviders(DropDownList ddl, string defaultItem = "")
        {
            ddl.DataSource = Providers;
            ddl.DataTextField = "SPName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            if (defaultItem.Equals(""))
            {
                Helper.AddDefaultItem(ddl);
            }
            else
            {
                Helper.AddDefaultItem(ddl, defaultItem);
            }
        }

        public virtual string GetUserName(int userId, string defaultName = "-"){
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            return user == null ? defaultName : user.UserName;
        }



       /* public virtual UsersTransaction ResellerLastTransaction(int ? resellerId){
            if(resellerId == null){
                return null;
            }
            int reseller = Convert.ToInt32(resellerId);
            return _context.UsersTransactions.Where(t => t.ResellerID == reseller).OrderByDescending(
                                                                                                     t => t.ID).FirstOrDefault();
        }


        public void Customer_Pay(int orderId, double amount, int userId, string notes, string description = "payment"){
            var userTransaction = new UsersTransaction{
                CreationDate = DateTime.Now.AddHours(),
                DepitAmmount = 0,
                CreditAmmount = amount,
                IsInvoice = false,
                WorkOrderID = orderId,
                Notes = notes,
                UserId = userId,
                Total = Billing.GetLastBalance(orderId, "WorkOrder") - amount,
                Description = description
            };
            _context.UsersTransactions.InsertOnSubmit(userTransaction);
        }*/



        public virtual void PopulateCentrals(DropDownList ddl){
            ddl.DataSource = Centrals;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "Id";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }



        /*public virtual void PopulateStatusesActiveNewCustomer(DropDownList ddl){
            ddl.DataSource = StatusesActiveNewCustomer;
            ddl.DataTextField = "StatusName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }*/



        public virtual List<int ?> GetBranchAdminBranchIDs(int userId){
            //List<int ?>
            var userBranchs = _context.UserBranches.Where(ub => ub.UserID == userId).Select(ub => ub.BranchID).ToList();
            if(userBranchs.Count <= 0)
                userBranchs = _context.Users.Where(u => u.ID == userId).Select(u => u.BranchID).ToList();
            return userBranchs;
        }


        public virtual List<User> ResellersOfUser(int userId){
            var resellerList = new List<User>();
            var user = _context.Users.FirstOrDefault(usr => usr.ID == userId);
            if (user == null) return resellerList;
            var id = user.Group.DataLevelID;
            if (id == null) return resellerList;
            var level = id.Value;
            var i = _context.Users.Where(usr => usr.ID == userId).Select(usr => usr.BranchID).First();
            if(i == null){} else{
                var userBranchId = i.Value;
                switch(level){
                    case 1 :
                        resellerList = _context.Users.Where(usr => usr.GroupID == 6).ToList();
                        break;
                    case 2 :
                        resellerList = _context.Users.Where(usr => usr.GroupID == 6 && GetBranchAdminBranchIDs(userId).Contains(usr.BranchID)).ToList();
                        break;
                    case 3 :
                        resellerList = _context.Users.Where(usr => usr.GroupID == 6 && usr.BranchID == userBranchId && usr.ID == userId).ToList();
                        break;
                }
            }
            return resellerList;
        }


        public virtual void PopulateOffers(DropDownList ddl,string defaultItem=""){
            ddl.DataSource = Offers;
            ddl.DataTextField = "Title";
            ddl.DataValueField = "Id";
            ddl.DataBind();
            if (defaultItem.Equals(""))
            {
                Helper.AddDefaultItem(ddl);
            } else{
                Helper.AddDefaultItem(ddl,defaultItem);
            }
          
        }



        public virtual void PopulateResellersOfUser(int userId, DropDownList ddl){
            ddl.DataSource = ResellersOfUser(userId);
            ddl.DataTextField = "UserName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            Helper.AddDefaultItem(ddl);
        }



        public virtual int ? GetUserGroup(int userId){
            var user = Users.FirstOrDefault(u => u.ID == userId);
            return user != null ? user.GroupID : null;
        }


        /*public virtual void SaveTransaction(UsersTransaction transaction){
            if(transaction.ID == 0){
                _context.UsersTransactions.InsertOnSubmit(transaction);
            }
        }


        public virtual void SaveAutmoatedProcess(AutomatedProcess process){
            if(process.Id == 0){
                _context.AutomatedProcesses.InsertOnSubmit(process);
            }
        }

        public virtual List<CustomerModel> SearchCustomers(SearchCustomersModel model){
            var customers = _context.WorkOrders.Where(o => o.WorkOrderStatusID == 6 || o.WorkOrderStatusID == 1).ToList();

            if(!string.IsNullOrEmpty(model.From)){
                var fromDate = Convert.ToDateTime(model.From);
                customers = customers.Where(c => c.CreationDate != null && c.CreationDate.Value.Date >= fromDate.Date).ToList();
            }

            if(!string.IsNullOrEmpty(model.To)){
                var toDate = Convert.ToDateTime(model.To);
                customers = customers.Where(c => c.CreationDate != null && c.CreationDate.Value.Date <= toDate.Date).ToList();
            }

            if(model.CentralId != null){
                customers = customers.Where(c => c.CentralId == model.CentralId).ToList();
            }

            if(model.OfferId != null){
                customers = customers.Where(c => c.OfferId == model.OfferId).ToList();
            }

            if(model.RepId != null){
                customers = customers.Where(c => c.RepresentativeId == model.RepId).ToList();
            }

            if(model.ResellerId != null){
                customers = customers.Where(c => c.ResellerID == model.ResellerId).ToList();
            }


            if(model.StateId != null){
                customers = customers.Where(c => c.WorkOrderStatusID == model.StateId).ToList();
            }



            return customers.Select(TransformWorkOrderToCustomerModel).OrderBy(c => c.Name).ToList();
        }


        public virtual CustomerModel TransformWorkOrderToCustomerModel(WorkOrder order){
            var reseller = order.ResellerID != null ? _context.Users.FirstOrDefault(u => u.ID == order.ResellerID) : null;
            return new CustomerModel{
                                        Creation = order.CreationDate,
                                        Activation = order.CreationDate == null ? string.Empty : order.CreationDate.Value.ToShortDateString(),
                                        Branch = order.Branch == null ? string.Empty : order.Branch.BranchName,
                                        BranchId = order.BranchID,
                                        Name = order.CustomerName,
                                        Id = order.ID,
                                        Offer = order.Offer == null ? string.Empty : order.Offer.Title,
                                        OfferId = order.OfferId,
                                        Package = order.ServicePackage == null ? string.Empty : order.ServicePackage.ServicePackageName,
                                        PackageId = order.ServicePackageID,
                                        Phone = order.CustomerPhone,
                                        Reseller = reseller == null ? string.Empty : reseller.UserName,
                                        ResellerId = order.ResellerID,
                                        Provider = order.ServiceProvider == null ? string.Empty : order.ServiceProvider.SPName,
                                        ProviderId = order.ServiceProviderID,
                                        State = order.Status == null ? string.Empty : order.Status.StatusName,
                                        StateID = order.WorkOrderStatusID,
                                        CentralId = order.CentralId,
                                        Central = order.Central == null ? string.Empty : order.Central.Name
                                    };
        }
*/


        public virtual void Commit(){
            _context.SubmitChanges();
        }


        public virtual void PopulateResellers(DropDownList dropDownList){
            dropDownList.AppendDataBoundItems = true;
            dropDownList.DataSource = Resellers;
            dropDownList.DataTextField = "UserName";
            dropDownList.DataValueField = "ID";
            dropDownList.DataBind();
            Helper.AddAllDefaultItem(dropDownList);
        }

        /*public virtual void PopulateResellerswithDirectUser(DropDownList dropDownList)
        {
            dropDownList.AppendDataBoundItems = true;
            Helper.AddDropDownItem(dropDownList, dropDownList.Items.Count, "Direct User");
            dropDownList.DataSource = Resellers;
            dropDownList.DataTextField = "UserName";
            dropDownList.DataValueField = "ID";
            dropDownList.DataBind();
            Helper.AddAllDefaultItem(dropDownList);
        }

        public WorkOrder GetWorkOrder(string phone, int governorateId){
            return _context.WorkOrders.FirstOrDefault(w => w.CustomerPhone == phone && w.CustomerGovernorateID == governorateId);
        }*/


        public WorkOrder GetWorkOrder(int orderId){
            return _context.WorkOrders.FirstOrDefault(w => w.ID == orderId);
        }


        /*public bool CustomerIsWithReseller(WorkOrder order, int resellerId){
            var reseller = GetReseller(resellerId);
            if(reseller == null){
                return false;
            }
            return order.ResellerID == resellerId;
        }*/


        public User GetReseller(int resellerId){
            return _context.Users.FirstOrDefault(r => r.ID == resellerId);
        }


        public string GetReseller(int ? resellerId){
            if(resellerId == null || resellerId == -1){
                return string.Empty;
            }

            var reseller = GetReseller(Convert.ToInt32(resellerId));
            return reseller != null ? reseller.UserName : string.Empty;
        }


        /*public void SaveFile(FileUpload fileUpload, string fullPath){
            fileUpload.SaveAs(fullPath);
        }



        public void SaveWorkOrderRequests(IEnumerable<WorkOrderRequest> workOrderRequests){
            _context.WorkOrderRequests.InsertAllOnSubmit(workOrderRequests);
        }


        public void SaveWorkOrderRequest(WorkOrderRequest request){
            if(request.ID == 0){
                _context.WorkOrderRequests.InsertOnSubmit(request);
            }
        }



        public Branch GetResellerBranch(int userId){
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            var branch = _context.Branches.FirstOrDefault(b => b.ID == user.BranchID);
            return branch;
        }



        public List<WorkOrderRequest> CustomerWithPreviousPaymentRequests(int resellerId){
            return _context.WorkOrderRequests.Where(
                                                    r => r.RequestID == 11 && r.RSID == 3 && r.WorkOrder.ResellerID == resellerId).ToList();
        }


        public void SaveWorkOrderStatus(WorkOrderStatus status){
            if(status.ID == 0){
                _context.WorkOrderStatus.InsertOnSubmit(status);
            }
        }*/


        public void PopulatePaymentTypes(DropDownList dropDownList){
            var paymentTypes = _context.PaymentTypes;
            dropDownList.DataSource = paymentTypes;
            dropDownList.DataTextField = "PaymentTypeName";
            dropDownList.DataValueField = "ID";
            dropDownList.DataBind();
            Helper.AddAllDefaultItem(dropDownList);
        }


        public void PopulateCentrals(DropDownList ddlCentral, int governorateId){
                var centrals = _context.Centrals.Where(c => c.GovernateId == governorateId);
                Helper.PopulateDrop(centrals, ddlCentral, "Id", "Name");

        }


        public Offer Offer(int offerId){
            return _context.Offers.FirstOrDefault(o => o.Id == offerId);
        }


        public List<ProviderPackages> OfferProvidersPackages(int offerId){
            var offer = Offer(offerId);
            if(offer == null){
                return null;
            }
            var providers = Providers.ToList();
            var items = new List<ProviderPackages>();
            foreach(var provider in providers){
                var packages = provider.ServicePackages.ToList();
                foreach(var package in packages){
                    items.Add(new ProviderPackages{
                        Package = package,
                        Provider = provider,
                        Checked = offer.OfferProviderPackages.Any(x => x.OfferId == offerId && x.PackageId == package.ID)
                    });
                }
            }
            return items;
        }


        /*public List<Offer> ProviderOffers(int providerId,User user){
            var provider = _context.ServiceProviders.FirstOrDefault(p => p.ID.Equals(providerId));
            return provider == null ? null : ProviderOffers(provider, user);
        }*/


        public List<Offer> ProviderOffers(ServiceProvider provider, User user){
            var packages = provider.ServicePackages.ToList();
            var offers = new List<Offer>();
            foreach(var package in packages){
                var offerProviderPackages = package.OfferProviderPackages.ToList();
                foreach(var providerPackage in offerProviderPackages){
                    if(offers.Any(x => x.Id == providerPackage.OfferId))
                        continue;
                    offers.Add(providerPackage.Offer);
                }
            }
            
            if(user == null || user.GroupID == null) return null;
            if(user.GroupID==1){
                return offers;
            }
            if (user.GroupID == 6)
            {
                offers = offers.Where(x => x.OfferResellers.Any(m => m.UserId.Equals(user.ID))).ToList();
                return offers;
            }
            if (user.BranchID!=null){
                offers = offers.Where(x => x.OfferBranches.Any(m => m.BranchId.Equals(user.BranchID.Value))).ToList();
                return offers;
            }
           

            return null;

        }



        public User User(int userId ){
            return _context.Users.FirstOrDefault(u=>u.ID==userId);
        }


        /*public virtual List<WorkOrder> UnistalledCustomers()
        {
            return WorkOrders.Where(w => w.Installed == false).ToList();
        }

        public virtual List<WorkOrder> UnistalledCustomers(List<WorkOrder>orders )
        {
            return orders.Where(w => w.Installed == false ).ToList();
        }*/


        public void PopulateUsers(object ddl)
        {
            var dropDownList = ddl as DropDownList;
            if (dropDownList != null)
            {
                dropDownList.DataSource = Users;
                dropDownList.DataTextField = "UserName";
                dropDownList.DataValueField = "ID";
                dropDownList.DataBind();
                Helper.AddDefaultItem(dropDownList);
            }
            var rbl = ddl as CheckBoxList;
            if(rbl == null) return;
            rbl.DataSource = Users;
            rbl.DataTextField = "UserName";
            rbl.DataValueField = "ID";
            rbl.DataBind();
        }

        public void PopulateUsersWithOutResellers(DropDownList ddl)
        {

            if (ddl != null)
            {
                ddl.DataSource = Users.Where(a=>a.GroupID != 6).ToList();
                ddl.DataTextField = "UserName";
                ddl.DataValueField = "ID";
                ddl.DataBind();
                Helper.AddDefaultItem(ddl);
            }
        }

        public void PopulateUsersByDataLevel(object ddl)
        {
            var dropDownList = ddl as DropDownList;
            var users = DataLevelClass.GetListUsersByDataLevel();
            if (dropDownList != null)
            {
                dropDownList.DataSource = users.Where(a => a.GroupId != 1 && a.GroupId != 4 && a.GroupId != 6);
                dropDownList.DataTextField = "UserName";
                dropDownList.DataValueField = "ID";
                dropDownList.DataBind();
                Helper.AddDefaultItem(dropDownList);
            }
            var rbl = ddl as CheckBoxList;
            if (rbl == null) return;
            rbl.DataSource = users;
            rbl.DataTextField = "UserName";
            rbl.DataValueField = "ID";
            rbl.DataBind();
        }
    }
}
