using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NewIspNL.FawryService
{
    public class PayingViewModel
    {
    }
     [DataContract]
	public class PmtTransId {
         [DataMember(EmitDefaultValue = false)]
		public string PmtId { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string PmtIdType { get; set; }
        [DataMember(EmitDefaultValue = false)]
		public string CreatedDt { get; set; }
	}

     [DataContract]
	public class PmtStatus {
         [DataMember(EmitDefaultValue = false)]
		public string PmtStatusCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
		public string EffDt { get; set; }
	}

     [DataContract]
	public class PmtInfo {
         [DataMember(EmitDefaultValue = false)]
		public string BillingAcct { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string BillerId { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string BillTypeCode { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string BankId { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string PmtType { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string DeliveryMethod { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public CurAmt CurAmt { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string PrcDt { get; set; }
	}

     [DataContract]
	public class PmtRec {
        [DataMember(EmitDefaultValue = false)]
		public List<PmtTransId> PmtTransId { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public PmtStatus PmtStatus { get; set; }
        [DataMember(EmitDefaultValue = false)]
		public PmtInfo PmtInfo { get; set; }
	}

     [DataContract]
	public class PmtNotifyRq {
       [DataMember(EmitDefaultValue = false)]
		public PmtRec PmtRec { get; set; }
	}

     [DataContract]
	public class PaySvcRq {
         [DataMember(EmitDefaultValue = false)]
		public string RqUID { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public string AsyncRqUID { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public MsgRqHdr MsgRqHdr { get; set; }
         [DataMember(EmitDefaultValue = false)]
		public PmtNotifyRq PmtNotifyRq { get; set; }
	}
    // response
     [DataContract]
     public class PmtStatusRec
     {
         [DataMember(EmitDefaultValue = false)]
         public PmtTransId PmtTransId { get; set; }
          [DataMember(EmitDefaultValue = false)]
         public Status status { get; set; }
     }
     [DataContract]
     public class PmtNotifyRs
     {
         [DataMember(EmitDefaultValue = false)]
         public PmtStatusRec PmtStatusRec { get; set; }
     }
     [DataContract]
     public class PaySvcRs
     {
         [DataMember(EmitDefaultValue = false)]
         public string RqUID { get; set; }
          [DataMember(EmitDefaultValue = false)]
         public string AsyncRqUID { get; set; }
          [DataMember(EmitDefaultValue = false)]
         public Status Status { get; set; }
          [DataMember(EmitDefaultValue = false)]
         public PmtNotifyRs PmtNotifyRs { get; set; }
     }

}

