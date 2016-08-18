using System;


namespace BL.Concrete{
    /// <summary>
    ///     Template for invoices usage
    /// </summary>
    public class Invoice{
        public string Phone { get; set; }
        public double Amount { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool ResellerCommission{ get; set; }
        public string Note { get; set; }
    }
}
