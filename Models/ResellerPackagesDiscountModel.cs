namespace NewIspNL.Models{
    public class ResellerPackagesDiscountModel : PackagesDiscountModel{
        public string Reseller { get; set; }
        public int ResellerId { get; set; }
    }
}