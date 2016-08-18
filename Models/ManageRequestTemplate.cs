using System;

namespace NewIspNL.Models{
    [Serializable]
    public class ManageRequestTemplate{
        public int ID { get; set; }
        public string Note { get; set; }
        public bool? ProviderRequest { get; set; }
        public int CurrentPackageID { get; set; }
        public string ExtraGiga { get; set; }
        public string IpPackageName { get; set; }
        public int NewIpPackageID { get; set; }
        public int NewPackageID { get; set; }
        public DateTime RequestDate { get; set; }
        public string TRequestDate { get; set; }
        public string RSName { get; set; }
        public int RSID { get; set; }
        public string CurrentServicePackageName { get; set; }
        public string NewServicePackageName { get; set; }
        public int WorkOrderID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string SPName { get; set; }
        public string GovernorateName { get; set; }
        public string UserName { get; set; }
        public string UserName2 { get; set; }
        public int woid { get; set; }
        public string RejectReason { get; set; }
        public string StatusName { get; set; }
        public string Title { get; set; }
        public string BranchName { get; set; }
        public DateTime ActivationDate { get; set; }
        public string RequestDate2 { get; set; }
        public string Central { get; set; }

        public string TActivationDate { get; set; }
        public int ? RequestTypeId { get; set; }

        public string  SenderName { get; set; }
        public string ResultHtml { get; set; }
        //public string CurrentOffer { get; set; }
        public string NewOffer { get; set; }
        public string PaymentType { get; set; }
        public int SuspenDaysCount { get; set; }
        public int? PaymentTypeId { get; set; }
        public string WRequestDate { get; set; }
    }
}
