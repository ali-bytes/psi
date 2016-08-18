using System;

namespace NewIspNL.Domain.Abstract{
    public class RouterHistory{
        public DateTime Time { get; set; }

        public int Quantity { get; set; }

        public string Operation { get; set; }

        public string Consumer { get; set; }
    }
}
