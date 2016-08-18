using System;
using Db;

namespace NewIspNL.Models{
    public class DemandModel{
        public int Id { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int OrderId { get; set; }

        public bool Paid { get; set; }

        public decimal Amount { get; set; }

        public decimal ResellerDiscount { get; set; }

        public decimal Percent { get; set; }

        public Demand Demand { get; set; }
    }
}
