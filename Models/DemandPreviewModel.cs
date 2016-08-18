using System;
using Db;

namespace NewIspNL.Models{
  
    public class DemandPreviewModel{
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Phone { get; set; }
        public string Governorate { get; set; }
        public string Offer { get; set; }
        public string Reseller { get; set; }
        public string Provider { get; set; }
        public string servicepack { get; set; }
        public string Central { get; set; }
        public decimal Amount { get; set; }
        public string TAmount { get; set; }
        public DateTime StartAt { get; set; }
        public string TStartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string TEndAt { get; set; }
        public bool Paid { get; set; }
        public string TPaid { get; set; }
        public string Notes { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public int ? StatusId { get; set; }
        public string PaymentComment { get; set; }
        public string PaymentDate { get; set; }

        public string Branch { get; set; }
        public decimal ResellerNet { get; set; }
        public string TResellerNet { get; set; }
        public decimal ResellerDiscount { get; set; }
        public string TResellerDiscount { get; set; }
        public decimal BranchNet { get; set; }
        public string TBranchNet { get; set; }
        public decimal BranchDiscount { get; set; }
        public string TBranchDiscount { get; set; }
        public DateTime RequestDate { get; set; }
        public Demand Demand { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int BranchId { get; set; }
        public string PaymentType { get; set; }
        public int? WorkOrderId { get; set; }
    }
    [Serializable]
    public class DemandPreviewModelToExel
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Phone { get; set; }
        public string Governorate { get; set; }
        public string Offer { get; set; }
        public string Reseller { get; set; }
        public string Provider { get; set; }
        public string servicepack { get; set; }
        public string Central { get; set; }
        public decimal Amount { get; set; }
        public string TAmount { get; set; }
        public DateTime StartAt { get; set; }
        public string TStartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string TEndAt { get; set; }
        public bool Paid { get; set; }
        public string TPaid { get; set; }
        public string Notes { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public string PaymentComment { get; set; }
        public string PaymentDate { get; set; }

        public string Branch { get; set; }
        public decimal ResellerNet { get; set; }
        public string TResellerNet { get; set; }
        public decimal ResellerDiscount { get; set; }
        public string TResellerDiscount { get; set; }
        public decimal BranchNet { get; set; }
        public string TBranchNet { get; set; }
        public decimal BranchDiscount { get; set; }
        public string TBranchDiscount { get; set; }
        public DateTime RequestDate { get; set; }
        //public Demand Demand { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int BranchId { get; set; }
        public string PaymentType { get; set; }
        public int? WorkOrderId { get; set; }
    }
}