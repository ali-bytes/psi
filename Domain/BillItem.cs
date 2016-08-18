using System;

namespace NewIspNL.Domain{
    public class BillDetails{
        public double Price { get; set; }

        public double PriceAfterDisc { get; set; }

        public decimal Discount { get; set; }

        public decimal MainPrice { get; set; }

        public decimal Net { get; set; }

        public decimal ResellerDiscount { get; set; }

        public string PackageName { get; set; }
    }

    public class BillItem{
        public string CustomerName { get; set; }

        public string GovernorateName { get; set; }

        public string CustomerPhone { get; set; }

        public string SpName { get; set; }

        public string ServicePackageName { get; set; }

        public string StatusName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Price { get; set; }

        public double PriceAfterDisc { get; set; }

        public string Offer { get; set; }

        public decimal Discount { get; set; }

        public decimal MainPrice { get; set; }

        public decimal ResellerDiscount { get; set; }

        public decimal Net { get; set; }

        public string Reseller { get; set; }

        public string Branch { get; set; }

        public string ActivationDate { get; set; }

        public string Central { get; set; }

        public string TEndDate { get; set; }
        public string TStartDate { get; set; }
    }
}
