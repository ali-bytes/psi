using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using Db;
using Microsoft.Ajax.Utilities;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using System.Linq.Dynamic;

namespace NewIspNL.Domain.SearchService{
    public class SearchEngine : ISearchEngine{
        readonly ISPDataContext _context;

        readonly WorkOrderRepository _orderRepository;


        public SearchEngine(ISPDataContext ispDataContext){
            _context = ispDataContext;
            _orderRepository = new WorkOrderRepository();
        }


        #region ISearchEngine Members

        public CanEditModel EditCustomer(int userId){
            var model = new CanEditModel();
            var user = _context.Users.FirstOrDefault(usr => usr.ID == userId);
            if(user == null){
                model.CanEdit = false;
                model.GroupId = 0;
                return model;
            }
            var id = user.GroupID;
            if(id == null){
                model.GroupId = 0;
                model.CanEdit = false;
                return model;
            }

            model.GroupId = id.Value;
            var groupPrivilegeQuery = _context.GroupPrivileges.Where(gp => gp.Group.ID == model.GroupId).Select(gp => gp.privilege.Name);
            model.CanEdit = groupPrivilegeQuery.Contains("EditCustomer.aspx");
            return model;
        }


        public List<CustomerResult> SearchByRouterSerial(string serial){
            var orders = DataLevelClass.GetUserWorkOrder(_context).Where(x => x.RouterSerial.Contains(serial)).ToList();//_context.WorkOrders.Where(x=>x.RouterSerial.Contains(serial)).ToList();
            return orders.Select(ToCustomerResult).ToList();
        }


        public CustomerResult ToCustomerResult(WorkOrder order){
            var reseller = string.Empty;
            if(order.ResellerID != null && order.ResellerID != -1){
                var resellerId = Convert.ToInt32(order.ResellerID);
                var selectedReseller = _context.Users.FirstOrDefault(u => u.ID == resellerId);
                if(selectedReseller != null){
                    reseller = selectedReseller.UserName;
                }
            }
            var package = "-";
            if(order.ServicePackage != null){
                package = order.ServicePackage.ServicePackageName;
            }

            var activation = _orderRepository.GetActivationDate(order.ID);


            return ToCustomerResult(order, reseller, package, activation);
        }


       public static CustomerResult ToCustomerResult(WorkOrder order, string reseller, string package, DateTime ? activation){
            return new CustomerResult{
                Id = order.ID,
                Branch = order.Branch == null ? string.Empty : order.Branch.BranchName,
                BranchId = order.BranchID,
                Customer = order.CustomerName,
                Phone = order.CustomerPhone,
                Mobile = order.CustomerMobile,
                ServicProvider=order.ServiceProvider.SPName,
                Reseller = reseller,
                ResellerId = order.ResellerID,
                State = order.Status == null ? string.Empty : order.Status.StatusName,
                StateId = order.WorkOrderStatusID,
                Offer = order.Offer == null ? string.Empty : order.Offer.Title,
                Central = order.Central == null ? string.Empty : order.Central.Name,
                Package = package,
                ActivationDate = activation == null ? "-" : activation.Value.ToShortDateString(),
                RouterSerial = order.RouterSerial,
                CreationDate=order.CreationDate==null?"-":order.CreationDate.Value.ToShortDateString(),
                RequestNumber = order.RequestNumber ?? "_",
                RequestDate = order.RequestDate!=null?Convert.ToDateTime(order.RequestDate):new DateTime(),
                UserName = order.UserName,
                Password = order.Password,
                PaymentType = order.PaymentType.PaymentTypeName,
                IpPackage = order.IpPackage.IpPackageName
                //Path24Month = activation==null?false:true,
                //prepaid = order.Prepaid==null?false:true,

            };
        }
        public static CustomerResult ToCustomerResult(WorkOrder order, ISPDataContext context){
            var resellerName = "";
            if(order != null && order.ResellerID != null){
                var reseller = context.Users.FirstOrDefault(u => u.ID == order.ResellerID);
                if(reseller != null){
                    resellerName = reseller.UserName;
                }
            }
            if(order != null)
                return new CustomerResult{
                    Id = order.ID,
                    Branch = order.Branch == null ? string.Empty : order.Branch.BranchName,
                    BranchId = order.BranchID,
                    Customer = order.CustomerName,
                    Phone = order.CustomerPhone,
                    Reseller = resellerName,
                    ResellerId = order.ResellerID,
                    State = order.Status == null ? string.Empty : order.Status.StatusName,
                    StateId = order.WorkOrderStatusID,
                    Offer = order.Offer == null ? string.Empty : order.Offer.Title,
                    Central = order.Central == null ? string.Empty : order.Central.Name,
                    Package = order.ServicePackage.ServicePackageName,
                    RouterSerial = order.RouterSerial,
                    CreationDate=order.CreationDate==null?"-":order.CreationDate.Value.ToShortDateString()
                };
            return null;

        }




        // ALY New Advanced Search 
        public List<CustomerResult> Search(AdvancedBasicSearchModel model, int CheckPoint, int Month24IsSelected, int isPrePaidChecked, int isTextEmpty,int userId)
        {
            List<CustomerResult> LastList = new List<CustomerResult>();
            if (CheckPoint == 1 || isTextEmpty == 1)
            {
                List<CustomerResult> olist = GetSearchResult(model,userId).ToList();
                LastList = olist.ToList();   
            }
            if (Month24IsSelected == 1 && isPrePaidChecked == 0 && CheckPoint == 0 && isTextEmpty == 0)
            {
                var now = DateTime.Now.Add9Hours();
                string qry = "UpdateDate.Value.AddMonths(24) <= DateTime.Parse(\"" + now + "\") And StatusID==6";
                LastList = GetFastResult(qry, userId, model).ToList();
                
            }
            if (isPrePaidChecked == 1 && Month24IsSelected == 0 && CheckPoint == 0 && isTextEmpty == 0)
            {
                string qry = "Prepaid > 0";

                LastList = GetFastResult(qry, userId, model).ToList();
            }
            if (isPrePaidChecked == 1 && Month24IsSelected == 1 && CheckPoint == 0 && isTextEmpty == 0)
            {
                var now = DateTime.Now.Add9Hours();
                string qry = "UpdateDate.Value.AddMonths(24) <= DateTime.Parse(\"" + now + "\") And Prepaid > 0 And StatusID==6";

                LastList = GetFastResult(qry, userId, model).ToList();
            }

            if (isPrePaidChecked == 0 && Month24IsSelected == 0 && CheckPoint == 0 && isTextEmpty == 0)
            {
                var groupId = GetGroupId(userId);
                if (groupId == 1)
                {
                    LastList = GetAllCustomerResults();
                }
                else
                {
                    string s = string.Empty;
                    LastList = GetFastResult(s, userId, model).ToList();
                }
                

            }
            return LastList.ToList();
           
        }

        private List<CustomerResult> GetAllCustomerResults()
        {
            var part1 = _context.WorkOrders.Join(
               _context.WorkOrderStatus.OrderByDescending(a => a.UpdateDate),
                w => w.ID,
                s => s.WorkOrderID,
                (w, s) =>
                  new
                  {
                      w.ID,
                      w.Branch.BranchName,
                      w.BranchID,
                      w.CustomerName,
                      w.UserName,
                      w.CustomerPhone,
                      w.CustomerMobile,
                      w.ServiceProviderID,
                      w.ServiceProvider.SPName,
                      w.ResellerID,
                      ResName = w.User.UserName,
                      w.Status.StatusName,
                      w.WorkOrderStatusID,
                      w.PaymentTypeID,
                      w.Offer.Title,
                      w.OfferId,
                      w.Central.Name,
                      w.CentralId,
                      w.ServicePackage.ServicePackageName,
                      w.ServicePackageID,
                      w.RouterSerial,
                      w.CreationDate,
                      w.RequestNumber,
                      w.RequestDate,
                      w.Password,
                      w.Prepaid,
                      w.VPI,
                      w.VCI,
                      w.CustomerGovernorateID,
                      w.IpPackage,
                      w.IpPackageID,
                      w.CustomerEmail,
                      
                      s.UpdateDate,
                      s.StatusID

                  }).AsEnumerable()
                  .Select(w => new CustomerResult
                  {
                      Id = w.ID,
                      Branch = w.BranchName,
                      BranchId = w.BranchID,
                      Customer = w.CustomerName,
                      Phone = w.CustomerPhone,
                      Mobile = w.CustomerMobile,
                      ServicProvider = w.SPName,
                      Reseller = w.ResName,
                      ResellerId = w.ResellerID,
                      State = w.StatusName,
                      StateId = w.WorkOrderStatusID,
                      Offer = w.Title,
                      Central = w.Name,
                      Package = w.ServicePackageName,
                      //ActivationDate = w.UpdateDate.ToString(),
                      RouterSerial = w.RouterSerial,
                      CreationDate = w.CreationDate == null ? "-" : w.CreationDate.Value.ToShortDateString(),
                      RequestNumber = w.RequestNumber ?? "_",
                      RequestDate = w.RequestDate != null ? Convert.ToDateTime(w.RequestDate) : new DateTime(),
                      UserName = w.UserName,
                      Password = w.Password,
                      Prepaid = w.Prepaid > 0 ? true : false
                      
                  });




            var part2 = _context.WorkOrders.Join(
               _context.WorkOrderStatus.Where(s => s.StatusID == 6).OrderByDescending(a => a.UpdateDate),
                w => w.ID,
                s => s.WorkOrderID,
                (w, s) =>
                  new
                  {
                      w.ID,
                      w.Branch.BranchName,
                      w.BranchID,
                      w.CustomerName,
                      w.UserName,
                      w.CustomerPhone,
                      w.CustomerMobile,
                      w.ServiceProviderID,
                      w.ServiceProvider.SPName,
                      w.ResellerID,
                      ResName = w.User.UserName,
                      w.Status.StatusName,
                      w.WorkOrderStatusID,
                      w.PaymentTypeID,
                      w.Offer.Title,
                      w.OfferId,
                      w.Central.Name,
                      w.CentralId,
                      w.ServicePackage.ServicePackageName,
                      w.ServicePackageID,
                      w.RouterSerial,
                      w.CreationDate,
                      w.RequestNumber,
                      w.RequestDate,
                      w.Password,
                      w.Prepaid,
                      w.VPI,
                      w.VCI,
                      w.CustomerGovernorateID,
                      w.IpPackage,
                      w.IpPackageID,
                      w.CustomerEmail,
                      w.PaymentType.PaymentTypeName,
                      s.UpdateDate,
                      s.StatusID

                  }).AsEnumerable()
                  .Select(w => new CustomerResult
                  {
                      Id = w.ID,
                      Branch = w.BranchName,
                      BranchId = w.BranchID,
                      Customer = w.CustomerName,
                      Phone = w.CustomerPhone,
                      Mobile = w.CustomerMobile,
                      ServicProvider = w.SPName,
                      Reseller = w.ResName,
                      ResellerId = w.ResellerID,
                      State = w.StatusName,
                      StateId = w.WorkOrderStatusID,
                      Offer = w.Title,
                      Central = w.Name,
                      Package = w.ServicePackageName,
                      ActivationDate = w.UpdateDate == null ? "-" : w.UpdateDate.Value.ToShortDateString(),
                      RouterSerial = w.RouterSerial,
                      CreationDate = w.CreationDate == null ? "-" : w.CreationDate.Value.ToShortDateString(),
                      RequestNumber = w.RequestNumber ?? "_",
                      RequestDate = w.RequestDate != null ? Convert.ToDateTime(w.RequestDate) : new DateTime(),
                      UserName = w.UserName,
                      Password = w.Password,
                      Prepaid = w.Prepaid > 0 ? true : false,
                      PaymentType = w.PaymentTypeName
                  });


            var full = part2.Union(part1).DistinctBy(a => a.Phone).ToList();

            return full.ToList();
        }

        //get resault when user select branch
        public IEnumerable<CustomerResult> GetFastResult(string query, int userId,AdvancedBasicSearchModel model)
        {
            // for get current user group
            var groupId = GetGroupId(userId);
            //For Resseller
            if (groupId == 6)
            {

                if (string.IsNullOrEmpty(query))
                {
                    query = "ResellerID==" + userId + "";
                }
                else
                {
                    query += " And ResellerID==" + userId + "";
                }
            }
            //for BranchAdmin
            else if (groupId == 4)
            {
                List<int> userBr = new List<int>();
                userBr = GetUserBranches(userId);
                string Bquery = string.Empty;
                if (model.BranchId!=null)
                {
                    
                }
                else
                {
                    foreach (var u in userBr)
                    {

                        if (string.IsNullOrEmpty(Bquery))
                        {
                            if (string.IsNullOrEmpty(query))
                            {
                                Bquery = "BranchID==" + u + "";
                            }
                            else
                            {
                                Bquery = " And ( BranchID==" + u + "";
                            }
                        }
                        else
                        {
                            Bquery += " OR BranchID==" + u + "";
                        }

                    }
                    if (Bquery.Contains("("))
                    {
                        Bquery += ")";
                    }
                    query += Bquery;
                }
               
            }
            else if (groupId == 1)
            {
                //For Admin
            }
            else
            {
                //FOR Any ONe Else
                var user1 = _context.Users.FirstOrDefault(usr => usr.ID == userId);
                if (user1 != null)
                {
                    var id = user1.BranchID;
                    if (id != 0)
                    {
                        if (string.IsNullOrEmpty(query))
                        {
                            query = "BranchID==" + id + "";
                        }
                        else
                        {
                            query += " And BranchID==" + id + "";
                        }
                    }
                }
            }
            return GetFastResult(query, userId);
        }

        public IEnumerable<CustomerResult> GetFastResult(string query, int userId)
        {
           

            var part1 = _context.WorkOrders.Join(
               _context.WorkOrderStatus.OrderByDescending(a => a.UpdateDate),
                w => w.ID,
                s => s.WorkOrderID,
                (w, s) =>
                  new
                  {
                      
                      w.ID,
                      w.Branch.BranchName,
                      w.BranchID,
                      w.CustomerName,
                      w.UserName,
                      w.CustomerPhone,
                      w.CustomerMobile,
                      w.ServiceProviderID,
                      w.ServiceProvider.SPName,
                      w.ResellerID,
                      ResName = w.User.UserName,
                      w.Status.StatusName,
                      w.WorkOrderStatusID,
                      w.PaymentTypeID,
                      w.Offer.Title,
                      w.OfferId,
                      w.Central.Name,
                      w.CentralId,
                      w.ServicePackage.ServicePackageName,
                      w.ServicePackageID,
                      w.RouterSerial,
                      w.CreationDate,
                      w.RequestNumber,
                      w.RequestDate,
                      w.Password,
                      w.Prepaid,
                      w.VPI,
                      w.VCI,
                      w.CustomerGovernorateID,
                      w.IpPackage,
                      w.IpPackageID,
                      w.CustomerEmail,
                      
                      s.UpdateDate,
                      s.StatusID

                  }).Where(query).AsEnumerable()
                  .Select(w => new CustomerResult
                  {
                      Id = w.ID,
                      Branch = w.BranchName,
                      BranchId = w.BranchID,
                      Customer = w.CustomerName,
                      Phone = w.CustomerPhone,
                      Mobile = w.CustomerMobile,
                      ServicProvider = w.SPName,
                      Reseller = w.ResName,
                      ResellerId = w.ResellerID,
                      State = w.StatusName,
                      StateId = w.WorkOrderStatusID,
                      Offer = w.Title,
                      Central = w.Name,
                      Package = w.ServicePackageName,
                      //ActivationDate = w.UpdateDate.ToString(),
                      RouterSerial = w.RouterSerial,
                      CreationDate = w.CreationDate == null ? "-" : w.CreationDate.Value.ToShortDateString(),
                      RequestNumber = w.RequestNumber ?? "_",
                      RequestDate = w.RequestDate != null ? Convert.ToDateTime(w.RequestDate) : new DateTime(),
                      UserName = w.UserName,
                      Password = w.Password,
                      Prepaid = w.Prepaid > 0 ? true : false
                     
                  });




            var part2 = _context.WorkOrders.Join(
               _context.WorkOrderStatus.Where(s => s.StatusID == 6).OrderByDescending(a => a.UpdateDate),
                w => w.ID,
                s => s.WorkOrderID,
                (w, s) =>
                  new
                  {
                      w.ID,
                      w.Branch.BranchName,
                      w.BranchID,
                      w.CustomerName,
                      w.UserName,
                      w.CustomerPhone,
                      w.CustomerMobile,
                      w.ServiceProviderID,
                      w.ServiceProvider.SPName,
                      w.ResellerID,
                      ResName = w.User.UserName,
                      w.Status.StatusName,
                      w.WorkOrderStatusID,
                      w.PaymentTypeID,
                      w.Offer.Title,
                      w.OfferId,
                      w.Central.Name,
                      w.CentralId,
                      w.ServicePackage.ServicePackageName,
                      w.ServicePackageID,
                      w.RouterSerial,
                      w.CreationDate,
                      w.RequestNumber,
                      w.RequestDate,
                      w.Password,
                      w.Prepaid,
                      w.VPI,
                      w.VCI,
                      w.CustomerGovernorateID,
                      w.IpPackage,
                      w.IpPackageID,
                      w.CustomerEmail,
                      w.PaymentType.PaymentTypeName,
                      s.UpdateDate,
                      s.StatusID

                  }).Where(query).AsEnumerable()
                  .Select(w => new CustomerResult
                  {
                      Id = w.ID,
                      Branch = w.BranchName,
                      BranchId = w.BranchID,
                      Customer = w.CustomerName,
                      Phone = w.CustomerPhone,
                      Mobile = w.CustomerMobile,
                      ServicProvider = w.SPName,
                      Reseller = w.ResName,
                      ResellerId = w.ResellerID,
                      State = w.StatusName,
                      StateId = w.WorkOrderStatusID,
                      Offer = w.Title,
                      Central = w.Name,
                      Package = w.ServicePackageName,
                      ActivationDate = w.UpdateDate == null ? "-" : w.UpdateDate.Value.ToShortDateString(),
                      RouterSerial = w.RouterSerial,
                      CreationDate = w.CreationDate == null ? "-" : w.CreationDate.Value.ToShortDateString(),
                      RequestNumber = w.RequestNumber ?? "_",
                      RequestDate = w.RequestDate != null ? Convert.ToDateTime(w.RequestDate) : new DateTime(),
                      UserName = w.UserName,
                      Password = w.Password,
                      Prepaid = w.Prepaid > 0 ? true : false,
                      PaymentType = w.PaymentTypeName
                      
                  });


            var full = part2.Union(part1).DistinctBy(a => a.Phone).ToList();

            return full.ToList();
               
        }

       
        #endregion

        #region NewCode

        private int GetGroupId(int userId)
        {
            var user = _context.Users.FirstOrDefault(usr => usr.ID == userId);
            if (user != null)
            {
                var id = user.GroupID;
                if (id != 0)
                {
                    int groupId = id ?? 0;
                    return groupId;
                }
            }
            return 0;
        }

        private List<int> GetUserBranches(int userId)
        {
            List<int> userBr = new List<int>();

            var user = _context.UserBranches.Where(usr => usr.UserID == userId).Select(a=>a.BranchID).ToList();
            //return user;
            if (user.Count > 0)
            {
                foreach (var u in user)
                {
                    userBr.Add(u.Value);
                }

            }
           
            return userBr.ToList();
        }

        public IEnumerable<CustomerResult> GetSearchResult(AdvancedBasicSearchModel model, int userId)
        {
            string query = string.Empty;

            if (!string.IsNullOrEmpty(model.Name))
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "CustomerName.Contains(\"" + model.Name.ToString() + "\")";
                }

            }

            if (model.GovernorateId != null)
            {
                var id = Convert.ToInt32(model.GovernorateId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "CustomerGovernorateID==" + id + "";
                }
                else
                {
                    query += " And CustomerGovernorateID==" + id + "";
                }

            }

            if (!string.IsNullOrEmpty(model.Phone))
            {
                string ph = model.Phone.ToString();
                if (string.IsNullOrEmpty(query))
                {
                    
                    query = "CustomerPhone.Contains(\"" + ph + "\")";
                }
                else
                {
                    query += " And CustomerPhone.Contains(\"" + ph + "\")";
                }
            }

            if (model.IpPackageId != null)
            {
                var id = Convert.ToInt32(model.IpPackageId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "IpPackageID==" + id + "";
                }
                else
                {
                    query += " And IpPackageID==" + id + "";
                }
            }

            if (!string.IsNullOrEmpty(model.Mobile))
            {
                string mb = model.Mobile.ToString();
                if (string.IsNullOrEmpty(query))
                {
                    query = "CustomerMobile.Contains(\"" + mb + "\")";
                }
                else
                {
                    query += " And CustomerMobile.Contains(\"" + mb + "\")";
                }
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "CustomerEmail.Contains(\"" + model.Email.ToString() + "\")";
                }
                else
                {
                    query += " And CustomerEmail.Contains(\"" + model.Email.ToString() + "\")";
                }
            }

            if (model.ProviderId != null)
            {
                var id = Convert.ToInt32(model.ProviderId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "ServiceProviderID==" + id + "";
                }
                else
                {
                    query += " And ServiceProviderID==" + id + "";
                }
            }
            if (model.PackageId != null)
            {
                var id = Convert.ToInt32(model.PackageId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "ServicePackageID==" + id + "";
                }
                else
                {
                    query += " And ServicePackageID==" + id + "";
                }
            }
            if (model.IsSystemAdmin)
            {
                if (model.ResellerId != null)
                {
                    var id = Convert.ToInt32(model.ResellerId);
                    if (id != 0)
                    {
                        if (string.IsNullOrEmpty(query))
                        {
                            query = "ResellerID==" + id + "";
                        }
                        else
                        {
                            query += " And ResellerID==" + id + "";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(query))
                        {
                            query = "ResellerID== null";
                        }
                        else
                        {
                            query += " And ResellerID== null";
                        }
                    }
                   
                }
                if (model.BranchId != null)
                {
                    var id = Convert.ToInt32(model.BranchId);
                    if (string.IsNullOrEmpty(query))
                    {
                        query = "BranchID==" + id + "";
                    }
                    else
                    {
                        query += " And BranchID==" + id + "";
                    }

                }
            }
            else
            {
                if (model.ResellerId != null)
                {
                    var id = Convert.ToInt32(model.ResellerId);

                    if (id != 0)
                    {
                        if (string.IsNullOrEmpty(query))
                        {
                            query = "ResellerID==" + id + "";
                        }
                        else
                        {
                            query += " And ResellerID==" + id + "";
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(query))
                        {
                            query = "ResellerID== null";
                        }
                        else
                        {
                            query += " And ResellerID== null";
                        }
                    }
                   
                    
                }
               

                if (model.BranchId != null)
                {
                    var id = Convert.ToInt32(model.BranchId);

                    if (string.IsNullOrEmpty(query))
                    {
                        query = "BranchID==" + id + "";
                    }
                    else
                    {
                        query += " And BranchID==" + id + "";
                    }
                }
                
            }
            if (!string.IsNullOrEmpty(model.Vpi))
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "VPI.Contains(\"" + model.Vpi.ToString() + "\")";
                }
                else
                {
                    query += " And VPI.Contains(\"" + model.Vpi.ToString() + "\")";
                }
            }
            if (!string.IsNullOrEmpty(model.Vci))
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "VCI.Contains(\"" + model.Vci.ToString() + "\")";
                }
                else
                {
                    query += " And VCI.Contains(\"" + model.Vci.ToString() + "\")";
                }
            }
            if (!string.IsNullOrEmpty(model.UserName))
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "UserName.Contains(\"" + model.UserName.ToString() + "\")";
                }
                else
                {
                    query += " And UserName.Contains(\"" + model.UserName.ToString() + "\")";
                }
            }
            if (model.StatusId != null)
            {
                var id = Convert.ToInt32(model.StatusId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "WorkOrderStatusID==" + id + "";
                }
                else
                {
                    query += " And WorkOrderStatusID==" + id + "";
                }

            }
            if (model.OfferId != null)
            {
                var id = Convert.ToInt32(model.OfferId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "OfferId==" + id + "";
                }
                else
                {
                    query += " And OfferId==" + id + "";
                }
            }
            if (model.CentralId != null)
            {
                var id = Convert.ToInt32(model.CentralId);
                if (string.IsNullOrEmpty(query))
                {
                    query = "CentralId==" + id + "";
                }
                else
                {
                    query += " And CentralId==" + id + "";
                }
            }
            if (model.Path24Month)
            {
                var now = DateTime.Now.Add9Hours();
                if (string.IsNullOrEmpty(query))
                {
                    query = "UpdateDate.Value.AddMonths(24) <= DateTime.Parse(\"" + now + "\") And StatusID==6";
                }
                else
                {
                    query += " And UpdateDate.Value.AddMonths(24) <= DateTime.Parse(\"" + now + "\") And StatusID==6";
                }
               
            
            }
            if (model.PrePaid)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "Prepaid>0";
                }
                else
                {
                    query += " And Prepaid>0";
                }

            }
            if (model.PaymentTypeId != null)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "PaymentTypeID==" + model.PaymentTypeId + "";
                }
                else
                {
                    query += " And PaymentTypeID==" + model.PaymentTypeId + "";
                }
            }

            if (model.From != null)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "RequestDate>= DateTime.Parse(\"" + model.From + "\")";
                }
                else
                {
                    query += " And RequestDate>=DateTime.Parse(\"" + model.From + "\")";
                }
            }

            if (model.To != null)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = "RequestDate<=DateTime.Parse(\"" + model.To + "\")";
                }
                else
                {
                    query += " And RequestDate<=DateTime.Parse(\"" + model.To + "\")";
                }
            }

            if (string.IsNullOrEmpty(query))
            {
                return null;
            }



            return GetFastResult(query,userId,model).ToList();

            //var fullData = _context.WorkOrders.Join(
            //    _context.WorkOrderStatus,
            //     w => w.ID,
            //     s => s.WorkOrderID,
            //     (w, s) =>
            //       new
            //       {
            //           w.ID,
            //           w.Branch.BranchName,
            //           w.BranchID,
            //           w.CustomerName,
            //           w.CustomerPhone,
            //           w.CustomerMobile,
            //           w.ServiceProvider.SPName,
            //           w.ResellerID,
            //           w.Status.StatusName,
            //           w.WorkOrderStatusID,
            //           w.Offer.Title,
            //           w.OfferId,
            //           w.Central.Name,
            //           w.CentralId,
            //           w.ServicePackage.ServicePackageName,
            //           w.ServicePackageID,
            //           w.RouterSerial,
            //           w.CreationDate,
            //           w.RequestNumber,
            //           w.RequestDate,
            //           w.UserName,
            //           w.Password,
            //           w.Prepaid,
            //           s.UpdateDate
            //       }).Where(query).AsEnumerable()
            //       .Select(w => new CustomerResult
            //       {
            //           Id = w.ID,
            //           Branch = w.BranchName,
            //           BranchId = w.BranchID,
            //           Customer = w.CustomerName,
            //           Phone = w.CustomerPhone,
            //           Mobile = w.CustomerMobile,
            //           ServicProvider = w.SPName,
            //           Reseller = w.UserName,
            //           ResellerId = w.ResellerID,
            //           State = w.StatusName,
            //           StateId = w.WorkOrderStatusID,
            //           Offer = w.Title,
            //           Central = w.Name,
            //           Package = w.ServicePackageName,
            //           ActivationDate = w.UpdateDate.ToString(),
            //           RouterSerial = w.RouterSerial,
            //           CreationDate = w.CreationDate == null ? "-" : w.CreationDate.Value.ToShortDateString(),
            //           RequestNumber = w.RequestNumber ?? "_",
            //           RequestDate = w.RequestDate != null ? Convert.ToDateTime(w.RequestDate) : new DateTime(),
            //           UserName = w.UserName,
            //           Password = w.Password,
            //           Prepaid = w.Prepaid > 0 ? true : false
            //       });
            //return fullData.ToList();



        }

        #endregion
       
        public List<WorkOrder> Getbyactivation(List<WorkOrder> orders)
        {
            var listorders = new List<WorkOrder>();
            foreach (var order in orders)
            {
                var dateTime = _orderRepository.GetActivationDate(order.ID);
                if (dateTime != null)
                {
                    var activationDate = dateTime.Value.AddMonths(24);
                    {
                        var now = DateTime.Now.Add9Hours();
                        if (now >= activationDate)
                        {
                            listorders.Add(order);
                        }
                    
                    }
                }
            }
            return listorders;
        }
   
        public List<WorkOrder> GetbyprePaid(List<WorkOrder> orders)
        {
            var listorders = new List<WorkOrder>();
            foreach (var ord in orders)
            {
                if (ord.Prepaid > 0)
                {
                    listorders.Add(ord);
                }
            }
            return listorders;
        }
       
    }

    public class CanEditModel{
        public bool CanEdit { get; set; }

        public int GroupId { get; set; }

        public bool CanDelete { get; set; }
    }

    public class SearchPrivilage : CanEditModel
    {
        public bool AddTicket { get; set; }
        public bool CustomerDemand { get; set; }
    }
}
