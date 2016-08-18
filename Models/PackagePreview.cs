namespace NewIspNL.Models{
    public class PackagePreview{
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public bool CanDelete { get; set; }
        public string Notes { get; set; }
        public double Price { get; set; }
        public string TPrice { get; set; }
        public string Active { get; set; }
        public string PurchasePrice { get; set; }
    }
}
