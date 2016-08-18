using System;

namespace NewIspNL.Models{
    public class RevenuesExpenses{
        public int TableId { get; set; }

        public string BranchName { get; set; }

        public double ? Amount { get; set; }

        public string Comment { get; set; }

        public Effect Effect { get; set; }

        public DateTime ? Date { get; set; }

        public CashBank CashBank { get; set; }
        public int  UserId { get; set; }
        public int BranchId { get; set; }
    }
}
