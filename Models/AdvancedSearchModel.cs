using System;
using System.Collections.Generic;

namespace NewIspNL.Models{
    public class BasicSearchModel{
        public int? GovernorateId { get; set; }
        public int? ResellerId { get; set; }

        public int? BranchId { get; set; }
        public int? CentralId { get; set; }
        public bool? Paid { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public bool BranchPaid { get; set; }

        public bool WithResellerDiscount { get; set; }
        public bool WithBranchDiscount { get; set; }

        public DateTime Date { get; set; }

        public int? PaymentTypeId { get; set; }

        public bool ? Isrequested { get; set; }
        public int? ProviderId { get; set; }
        public int? DateSearchType { get; set; }
    }

    public class AdvancedBasicSearchModel : BasicSearchModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Vpi { get; set; }

        public string Vci { get; set; }

        public string UserName { get; set; }

       

        public int ? IpPackageId { get; set; }

        //public int ? ProviderId { get; set; }

        public int ? PackageId { get; set; }

        public int ? StatusId { get; set; }

        public int ? OfferId { get; set; }

       

        public List<int> Resellers { get; set; }

        public List<int> Branches { get; set; }

        public bool IsSystemAdmin { get; set; }
        public bool Path24Month { get; set; }
        public bool PrePaid { get; set; }
        public string PaymentType { get; set; }
        public int? WorkOrderId { get; set; }
    }
}
