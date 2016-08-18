namespace NewIspNL.Models
{
    /// <summary>
    /// Summary description for CompanyDetails
    /// </summary>
    public class CompanyDetails
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public decimal ServiceFees { get; set; }
        public string CompanyImageUrl { get; set; }
        public decimal CommissionResellerOrBranch { get; set; }
        public string HtmlImageUrl { get; set; }
    }
}