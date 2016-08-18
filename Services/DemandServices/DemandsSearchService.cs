using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.Discounts;

namespace NewIspNL.Services.DemandServices{
    public class DemandsSearchService{
        #region Repos


        readonly BranchDiscountCalculator _branchDiscount;

        readonly ISPDataContext _context;

        readonly Loc _loc;

        readonly ResellerDiscountCalculator _resellerDiscount;

 public DemandsSearchService(ISPDataContext context){
            _context = context;
            _loc = new Loc();
            _resellerDiscount = new ResellerDiscountCalculator(context);
            _branchDiscount = new BranchDiscountCalculator(context);
        }

        #endregion
        
        public List<Demand> SearchDemands(BasicSearchModel model){
            var results = new List<Demand>();
            if(model.Paid != null && model.Paid.Value){
                results = _context.Demands.Where(x => x.Paid).ToList();
            }

            if(model.Paid != null && !model.Paid.Value){
                results = _context.Demands.Where(x => x.Paid == false).ToList();
            }

            if(model.Paid == null){
                results = _context.Demands.ToList();
            }

            if(model.BranchId != null){
                results = results.Where(x => x.WorkOrder.BranchID == model.BranchId.Value).ToList();
            }
            if(model.CentralId != null){
                results = results.Where(x => x.WorkOrder.CentralId == model.CentralId.Value).ToList();
            }
            if(model.GovernorateId != null){
                
                results = results.Where(x => x.WorkOrder.CustomerGovernorateID == model.GovernorateId.Value).ToList();
            }
            
            if(model.ResellerId != null){
                if(model.ResellerId != 0){
                    results = results.Where(x => x.WorkOrder.ResellerID == model.ResellerId.Value).ToList();
                } else{
                    results = results.Where(x => x.WorkOrder.ResellerID == null).ToList();
                }
            }

            if(model.Year != null){
                results = results.Where(x => x.StartAt.Year == model.Year.Value).ToList();
                if(model.Month != null){
                    results = results.Where(x => x.StartAt.Month == model.Month).ToList();
                }
            }


            return results;
        }
        public List<Demand> SearchDemands(AdvancedBasicSearchModel model)
        {
            var results = new List<Demand>();
            if (model.Paid != null && model.Paid.Value)
            {
                results = _context.Demands.Where(x => x.Paid).ToList();
            }
            
            if (model.Paid != null && !model.Paid.Value)
            {
                results = _context.Demands.Where(x => x.Paid == false).ToList();
            }

            if (model.Paid == null)
            {
                results = _context.Demands.ToList();
            }

            if (model.BranchId != null)
            {
                results = results.Where(x => x.WorkOrder.BranchID == model.BranchId.Value).ToList();
            }
            if (model.CentralId != null)
            {
                results = results.Where(x => x.WorkOrder.CentralId == model.CentralId.Value).ToList();
            }
            if (model.GovernorateId != null)
            {

                results = results.Where(x => x.WorkOrder.CustomerGovernorateID == model.GovernorateId.Value).ToList();
            }

            if (model.ResellerId != null)
            {
                if (model.ResellerId != 0)
                {
                    results = results.Where(x => x.WorkOrder.ResellerID == model.ResellerId.Value).ToList();
                }
                else
                {
                    results = results.Where(x => x.WorkOrder.ResellerID == null).ToList();
                }
            }
            if(model.ProviderId != null){
                results = results.Where(r => r.WorkOrder.ServiceProviderID == model.ProviderId.Value).ToList();
            }
            if (model.PackageId != null)
            {
                results = results.Where(a => a.WorkOrder.ServicePackageID == model.PackageId.Value).ToList();
            }
            if (model.IpPackageId != null)
            {
                results = results.Where(a => a.WorkOrder.IpPackageID == model.IpPackageId.Value).ToList();
            }
            if (model.Year != null)
            {
                results = results.Where(x => x.StartAt.Year == model.Year.Value).ToList();
                if (model.Month != null)
                {
                    results = results.Where(x => x.StartAt.Month == model.Month).ToList();
                }
            }
            if (model.StatusId != null)
            {
                results = results.Where(x => x.WorkOrder.WorkOrderStatusID == model.StatusId).ToList();
            }
            if (model.PaymentTypeId != null)
            {
                results = results.Where(a => a.WorkOrder.PaymentTypeID == model.PaymentTypeId).ToList();
            }
            return results;
        }
        
        public List<Demand> PaidDemands(DateTime date,int userId=0){
            if (userId != 0){
                var user = _context.Users.FirstOrDefault(u => u.ID == userId);
                if(user == null)return null;
                var userBranches = UserServices.GetUserBranches(userId,_context);
                var demands = new List<Demand>();
                foreach(var branch in userBranches){
                   demands.AddRange(PaidDemandsForBranch(new BasicSearchModel{
                        Date = date,
                        BranchId = branch.ID
                    }));
                }
                return demands;
            }
            
            var results = 
                _context.Demands
                .Where(x => x.PaymentDate != null)
                .ToList()
                .Where(x => x.PaymentDate != null && (x.PaymentDate.Value.Date == (date.Date) && x.Paid && (x.IsRequested==false || x.IsRequested==null))).ToList();
            return results;

            
        }

        public List<Demand> PaidDemandsForBranch(BasicSearchModel model){
            return model.BranchId == null ? null : PaidDemands(model.Date).Where(x => x.WorkOrder.BranchID == model.BranchId).ToList();
        }
        public List<Demand> PaidDemands(DateTime startAt, DateTime endAt){
            var results = _context.Demands
                .Where(x => x.PaymentDate != null).ToList()
                .Where(x => x.PaymentDate != null && (x.PaymentDate.Value.Date >= startAt.Date && x.PaymentDate.Value.Date <= endAt.Date && x.Paid)).ToList();
            return results;
        }
        public List<DemandPreviewModel> PaidDemandsTemplates(DateTime date,int userId=0){
            return PaidDemands(date, userId).Select(ToDemandPreviewModel).ToList();
        }
        public List<DemandPreviewModel> PaidDemandsForBranchTemplates(BasicSearchModel model){
            return PaidDemandsForBranch(model).Select(ToDemandPreviewModel).ToList();
        }
        public List<DemandPreviewModel> PaidDemandsTemplates(DateTime startAt, DateTime endAt){
            return PaidDemands(startAt, endAt).Select(ToDemandPreviewModel).ToList();
        }
        public List<DemandPreviewModel> SearchDemandsToPreview(BasicSearchModel basicSearchModel){
            if(basicSearchModel.WithResellerDiscount){
                return SearchDemands(basicSearchModel).Select(x => ToDemandPreviewModel(x, basicSearchModel)).ToList();
            }
            return SearchDemands(basicSearchModel).Select(ToDemandPreviewModel).ToList();
        }
    

        public List<DemandPreviewModel> AdvancedSearchDemandToPreview(AdvancedBasicSearchModel advanced){
            if(advanced.WithResellerDiscount){
                return SearchDemands(advanced).Select(x => ToDemandPreviewModel(x, advanced)).ToList();
            }
            return SearchDemands(advanced).Select(ToDemandPreviewModel).ToList();
        }


        public DemandPreviewModel ToDemandPreviewModel(Demand demand, BasicSearchModel searchModel){
            var workOrder = demand.WorkOrder;
                var model = new DemandPreviewModel{
                    Amount = demand.Amount,
                    Central = workOrder.CentralId == null ? "-" : workOrder.Central.Name,
                    Customer = workOrder.CustomerName,
                    EndAt = demand.EndAt,
                    TEndAt = demand.EndAt.ToShortDateString(),
                    Governorate = workOrder.Governorate.GovernorateName,
                    Id = demand.Id,
                    Offer = demand.Offer == null ? "-" : demand.Offer.Title,
                    Phone = workOrder.CustomerPhone,
                    servicepack = workOrder.ServicePackage.ServicePackageName,
                    Provider = workOrder.ServiceProvider.SPName,
                    Reseller = workOrder.User == null ? " " : workOrder.User.UserName,
                    StartAt = demand.StartAt,
                    TAmount = Helper.FixNumberFormat(demand.Amount),
                    TStartAt = demand.StartAt.ToShortDateString(),
                    Paid = demand.Paid,
                    TPaid = _loc.IterateResource(demand.Paid.ToString()),
                    Notes = demand.Notes,
                    User = demand.User.UserName,
                    Status = workOrder.Status.StatusName,
                    StatusId = workOrder.WorkOrderStatusID,
                    PaymentDate = demand.PaymentDate == null ? "-" : demand.PaymentDate.Value.ToShortDateString(),
                    Branch = workOrder.Branch == null ? "-" : workOrder.Branch.BranchName,
                    Demand = demand,
                    RequestDate =Convert.ToDateTime(workOrder.RequestDate),
                    WorkOrderId = demand.WorkOrderId
                    //workOrder.RequestDate!=null? Convert.ToDateTime(workOrder.RequestDate):new DateTime()
                };
                if(searchModel.WithResellerDiscount){
                    var order = demand.WorkOrder;
                    if(demand.WorkOrder == null) return model;
                    ApplyResellerDiscount(demand, order, model);
                    return model;
                }
                if(searchModel.WithBranchDiscount){
                    var order = demand.WorkOrder;
                    if(demand.WorkOrder == null) return model;
                    ApplyBranchDiscount(demand, order, model);
                    return model;
                }
                return model;
            
        }
        void ApplyResellerDiscount(Demand demand, WorkOrder order, DemandPreviewModel model){
            if(order.ResellerID == null || order.ServiceProviderID == null || order.ServicePackageID == null|| demand.IsResellerCommisstions==false)
            {
                var amount = demand.Amount;
                model.ResellerNet = amount;
                model.TResellerNet = Helper.FixNumberFormat(amount);
                return;
            }
            var discountData = _resellerDiscount.CalculateDiscount(order.ResellerID.Value, order.ServiceProviderID.Value, order.ServicePackageID.Value, demand.Amount);
            var resellerDiscount = discountData.Discount;
            model.ResellerDiscount = resellerDiscount;
            model.TResellerDiscount = Helper.FixNumberFormat(resellerDiscount);
            var resellerNet = discountData.Net;
            model.ResellerNet = resellerNet;
            model.TResellerNet = Helper.FixNumberFormat(resellerNet);
        }
        void ApplyBranchDiscount(Demand demand, WorkOrder order, DemandPreviewModel model){
            if (order.BranchID == null || order.ServiceProviderID == null || order.ServicePackageID == null || demand.IsResellerCommisstions == false)
            {
                var amount = demand.Amount;
                model.BranchNet = amount;
                model.TBranchNet = Helper.FixNumberFormat(amount);
                return;

            }
            var discountData = _branchDiscount.CalculateDiscount(order.BranchID.Value, order.ServiceProviderID.Value, order.ServicePackageID.Value, demand.Amount);
            var resellerDiscount = discountData.Discount;
            model.BranchDiscount = resellerDiscount;
            model.TBranchDiscount = Helper.FixNumberFormat(resellerDiscount);
            var branchNet = discountData.Net;
            model.BranchNet = branchNet;
            model.TBranchNet = Helper.FixNumberFormat(branchNet);
        }
        public DemandPreviewModel ToDemandPreviewModel(Demand demand){
            var workOrder = demand.WorkOrder;
            var model = new DemandPreviewModel{
                Amount = demand.Amount,
                Central = workOrder.CentralId == null ? "-" : workOrder.Central.Name,
                Customer = workOrder.CustomerName,
                EndAt = demand.EndAt,
                TEndAt = demand.EndAt.ToShortDateString(),
                Governorate = workOrder.Governorate.GovernorateName,
                Id = demand.Id,
                Offer = demand.Offer == null ? "-" : demand.Offer.Title,
                Phone = workOrder.CustomerPhone,
                servicepack = workOrder.ServicePackage.ServicePackageName,
                Provider = workOrder.ServiceProvider.SPName,
                Reseller = workOrder.User == null ? " " : workOrder.User.UserName,
                StartAt = demand.StartAt,
                TAmount = Helper.FixNumberFormat(demand.Amount),
                TStartAt = demand.StartAt.ToShortDateString(),
                Paid = demand.Paid,
                TPaid = _loc.IterateResource(demand.Paid.ToString()),
                Notes = demand.Notes,
                User = demand.User.UserName,
                Status = workOrder.Status.StatusName,
                StatusId = workOrder.WorkOrderStatusID,
                PaymentDate = demand.PaymentDate == null ? "-" : demand.PaymentDate.Value.ToString(CultureInfo.InvariantCulture),
                Branch = workOrder.Branch == null ? "-" : workOrder.Branch.BranchName,
                PaymentComment = demand.PaymentComment,
                RequestDate = Convert.ToDateTime(workOrder.RequestDate).Date,
                PaymentMethod = workOrder.PaymentType.PaymentTypeName,
                Address = workOrder.CustomerAddress,
                Mobile = workOrder.CustomerMobile,
                BranchId = workOrder.Branch==null?0:Convert.ToInt32(workOrder.BranchID),
                UserName = workOrder.UserName,
                Password = workOrder.Password,
                WorkOrderId = demand.WorkOrderId
            };

            return model;
        }
        public List<Demand> BranchUnpaidDemands(BasicSearchModel model){
            var results = _context.Demands.ToList();
            if (model.Paid != null)
                results = results.Where(a => a.Paid == model.Paid.Value).ToList();//_context.Demands.Where(x => x.Paid == model.Paid.Value).ToList();


            results = results.Where(x => x.BranchPaid == null || x.BranchPaid == false).ToList();

            if(model.BranchId != null)
                results = results.Where(x => x.WorkOrder.BranchID == model.BranchId.Value).ToList();

            if(model.Year != null){
                results = results.Where(x => x.StartAt.Year == model.Year.Value).ToList();
                if(model.Month != null)
                    results = results.Where(x => x.StartAt.Month == model.Month).ToList();
            }
            return results;
        }
        public List<DemandPreviewModel> BranchUnpaidDemandsPreview(BasicSearchModel model){
            if(model.WithBranchDiscount){
                return BranchUnpaidDemands(model).Select(x => ToDemandPreviewModel(x, model)).ToList();
            }
            return BranchUnpaidDemands(model).Select(ToDemandPreviewModel).ToList();
        }

        public User GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(a => a.ID == userId);
        }

        public void Commit()
        {
            _context.SubmitChanges();
        }
    }
}
