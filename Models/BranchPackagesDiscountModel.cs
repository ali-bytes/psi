namespace NewIspNL.Models{
    public class BranchPackagesDiscountModel : PackagesDiscountModel{
        public string Branch { get; set; }
        public int ResellerId { get; set; }

        public int BranchId { get; set; }
    }
}