namespace NewIspNL.Models{
    public class PackagesDiscountModel{
        public string Provider { get; set; }

        public string Package { get; set; }

        public string Discount { get; set; }

        public string Notes { get; set; }

        public int PackageId { get; set; }

       

        public int ProviderId { get; set; }
    }
}