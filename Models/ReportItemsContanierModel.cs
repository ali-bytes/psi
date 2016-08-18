using System.Collections.Generic;

namespace NewIspNL.Models{
    public class ReportItemsContanierModel{
        public List<int> Values { get; set; }
        public List<string> Names { get; set; } 
        public string TName { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}