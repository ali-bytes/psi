using System;

namespace NewIspNL.Models{
    public class CustomerResult{
        public int Id { get; set; }

        public string Customer { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }

        public int ? StateId { get; set; }

        public string State { get; set; }

        public int ? BranchId { get; set; }

        public string Branch { get; set; }

        public int ? ResellerId { get; set; }

        public string Reseller { get; set; }

        public string Offer { get; set; }

        public string Central { get; set; }

        public string Package { get; set; }

        public string RequestNumber { get; set; }

        public string ServicProvider{get; set; }
        public DateTime RequestDate { get; set; }
        public string ActivationDate { get; set; }
        public string RouterSerial { get; set; }
        public string CreationDate { get; set; }
        public bool Path24Month { get; set; }
        public bool Prepaid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PaymentType { get; set; }
        public string IpPackage { get; set; }
    }
}
