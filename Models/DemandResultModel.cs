using System;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using NewIspNL.Services.OfferServices;

namespace NewIspNL.Models{
    public class DemandResultModel2
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Governorate { get; set; }
        public string Central { get; set; }
        public string Provider { get; set; }
        public string Package { get; set; }
        public string Offer { get; set; }
        public int Offerid { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime TStart { get; set; }
        public DateTime TEnd { get; set; }
        public int Id { get; set; }
        public bool Paid { get; set; }
        public decimal DAmount { get; set; }
        public bool Isrequested { get; set; }
        public int WorkorderId { get; set; }
        public int StatusId { get; set; }
  
    
    }

    public class DemandResultModel{
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Governorate { get; set; }
        public string Central { get; set; }
        public string Provider { get; set; }
        public string Package { get; set; }
        public string Offer { get; set; }
        public string Amount { get; set; }
        public string User { get; set; }
        public string Notes { get; set; }
        public string TStart { get; set; }
        public string TEnd { get; set; }
        public decimal DAmount { get; set; }
        public bool Isrequested { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }  
        public bool Paid { get; set; }
        public string PaymentDate { get; set; }
        public string ActiviationDate { get; set; }
        public string Branch { get; set; }
        public string Reseller { get; set; }
        public int? ResellerId { get; set; }
        public int WorkorderId { get; set; }
        public string UserName { get; set; }
        public static DemandResultModel To(Demand demand){
             IWorkOrderStatusServices statusServices = new WorkOrderStatusServices();
            var order = demand.WorkOrder;
            var perchus = order.ServicePackage.PurchasePrice;
            decimal discound = 0;
            if(demand.WorkOrder.Offer!=null) discound = OfferPricingServices.GetOfferPrice(demand.WorkOrder.Offer, perchus,perchus);
            var gains = demand.Amount - (perchus - discound); //demand.Amount-order.ServicePackage.PurchasePrice;
            var statusStartDate = statusServices.GetStatusStartDate(order.ID,6);
                var demandResultModel = new DemandResultModel{
                    Id=demand.Id,
                    Name = order.CustomerName,
                    WorkorderId = order.ID,
                    Phone = order.CustomerPhone,
                    Notes = demand.Notes,
                    Amount = Helper.FixNumberFormat(demand.Amount),
                    Central = order.Central == null ? "-" : order.Central.Name,
                    Governorate = order.Governorate.GovernorateName,
                    Offer = demand.Offer == null ? "-" : demand.Offer.Title,
                    Package = order.ServicePackage.ServicePackageName,
                    Provider = order.ServiceProvider.SPName,
                    User = demand.User.UserName,
                    TStart = demand.StartAt.ToShortDateString(),
                    TEnd = demand.EndAt.ToShortDateString(),
                    Gains=gains,
                    TGains=Helper.FixNumberFormat(gains),
                    DAmount = demand.Amount,
                    PaymentComment = demand.PaymentComment,
                    Status=order.Status.StatusName,
                    StatusId = order.WorkOrderStatusID,
                    Isrequested = (demand.IsRequested.HasValue) && demand.IsRequested.Value,//Convert.ToBoolean(demand.IsRequested)
                    Paid = demand.Paid,
                    PaymentDate= demand.PaymentDate!=null?demand.PaymentDate.ToString():"-",
                    Branch = order.Branch.BranchName,
                    Reseller = (order.User == null || order.User.UserName == null) ? "-" : order.User.UserName,
                    ActiviationDate =statusStartDate!=null? statusStartDate.Value.ToShortDateString():"",
                    ResellerId = order.ResellerID,
                    UserName = order.UserName??"-"
                };
                return demandResultModel;
           
        }
        public static DemandMoreData To2(Demand demand)
        {
            var order = demand.WorkOrder;
            var perchus = order.ServicePackage.PurchasePrice;
            decimal discound = 0;
            if (demand.WorkOrder.Offer != null) discound = OfferPricingServices.GetOfferPrice(demand.WorkOrder.Offer, perchus,perchus);
            var gains = demand.Amount - (perchus - discound); //demand.Amount-order.ServicePackage.PurchasePrice;
            var demandResultModel = new DemandMoreData
            {
                Id = demand.Id,
                Name = order.CustomerName,
                WorkorderId = order.ID,
                Phone = order.CustomerPhone,
                Notes = demand.Notes,
                Amount = Helper.FixNumberFormat(demand.Amount),
                Central = order.Central == null ? "-" : order.Central.Name,
                Governorate = order.Governorate.GovernorateName,
                Offer = demand.Offer == null ? "-" : demand.Offer.Title,
                Package = order.ServicePackage.ServicePackageName,
                Provider = order.ServiceProvider.SPName,
                User = demand.User.UserName,
                TStart = demand.StartAt.ToShortDateString(),
                TEnd = demand.EndAt.ToShortDateString(),
                Gains = gains,
                TGains = Helper.FixNumberFormat(gains),
                DAmount = demand.Amount,
                PaymentComment = demand.PaymentComment,
                Status = order.Status.StatusName,
                StatusId = order.WorkOrderStatusID,
                Isrequested = (demand.IsRequested.HasValue) && demand.IsRequested.Value,//Convert.ToBoolean(demand.IsRequested)
                Paid = demand.Paid,
                PaymentDate = demand.PaymentDate != null ? demand.PaymentDate.ToString() : "-",
                Branch = order.Branch.BranchName,
                Reseller = (order.User == null || order.User.UserName == null) ? "-" : order.User.UserName,
                Mobile = order.CustomerMobile,
                Address = order.CustomerAddress
            };
            return demandResultModel;
        }


        public string PaymentComment { get; set; }


        public int Id { get; set; }


        public string TGains { get; set; }


        public decimal Gains { get; set; }
    }

    public class DemandMoreData:DemandResultModel
    {
        public string Address { get; set; }
        public string Mobile { get; set; }

    }
}