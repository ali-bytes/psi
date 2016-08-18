using Db;

namespace NewIspNL.Models{
    public class OrderDemand{
        public WorkOrder Order { get; set; }

        public Demand Demand { get; set; }
    }
}
