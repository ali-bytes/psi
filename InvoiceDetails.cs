namespace NewIspNL
{
    /// <summary>
    /// Summary description for InvoiceDetails
    /// </summary>
    public class InvoiceDetailsClass
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string ItemPrice { get; set; }
        public decimal Price { get; set; }
    }
}