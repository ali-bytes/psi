using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace NewIspNL.FawryService
{
    public class FawryViewModel2
    {
    }
    [DataContract]
    public class ProcessRequest
    {

        [DataMember(EmitDefaultValue = false)]
        public string BillingAcct { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ServerDt { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Amt { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string StatusCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Severity { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string StatusDesc { get; set; }



    }
    [DataContract]
    public class SignonRs
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string ClientDt { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string CustLangPref { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string ServerDt { get; set; }
        [DataMember(Order = 4, EmitDefaultValue = false)]
        public string Language { get; set; }
        [DataMember(Order = 5, EmitDefaultValue = false)]
        public SignonProfile SignonProfile { get; set; }
    }
    [DataContract]
    public class Status
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string StatusCode { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Severity { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string StatusDesc { get; set; }
        [DataMember(Order = 4,EmitDefaultValue = false)]
        public AdditionalStatus AdditionalStatus { get; set; }
    }
    [DataContract]
    public class CurAmt
    {
        [DataMember(EmitDefaultValue = false)]
        public string Amt { get; set; }
         [DataMember(EmitDefaultValue = false)]
        public string CurCode { get; set; }
    }
    [DataContract]
    public class BillSummAmt
    {
        [DataMember(EmitDefaultValue = false)]
        public CurAmt CurAmt { get; set; }
    }
    [DataContract]
    public class BillInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public BillSummAmt BillSummAmt { get; set; }
    }
    [DataContract]
    public class BillRec
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string BillingAcct { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string BillTypeCode { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public BillInfo BillInfo { get; set; }
    }
    [DataContract]
    public class BillInqRs
    {
        [DataMember(EmitDefaultValue = false)]
        public BillRec BillRec { get; set; }
    }
    [DataContract]
    public class PresSvcRs
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string RqUID { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public Status Status { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public BillInqRs BillInqRs { get; set; }
    }
      [DataContract]
    public class Response
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public SignonRs SignonRs { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public PresSvcRs PresSvcRs { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public PaySvcRs PaySvcRs { get; set; }
    }
    [MessageContract]
    public class processRequestResponse
    {
        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None, Namespace = "",Name = "return")]
        public Return Return { get; set; }
    }
      [DataContract]
    public class Return
    {
        [DataMember]
        public Response Response { get; set; }
    }

    //------------------------------------------------
    [DataContract]
    public class SignonProfile
    {
        [DataMember(Order = 1,EmitDefaultValue = false)]
        public string Sender { get; set; }

        [DataMember(Order = 4, EmitDefaultValue = false)]
        public string Receiver { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string MsgCode { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string Version { get; set; }
    }

    [DataContract]
    public class SignonRq
    {
        [DataMember(EmitDefaultValue = false)]
        public string ClientDt { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CustLangPref { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string SuppressEcho { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SignonProfile SignonProfile { get; set; }
    }

    [DataContract]
    public class NetworkTrnInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public string OriginatorCode { get; set; }
    }

    [DataContract]
    public class MsgRqHdr
    {
        [DataMember(EmitDefaultValue = false)]
        public NetworkTrnInfo NetworkTrnInfo { get; set; }
    }

    [DataContract]
    public class RecCtrlIn
    {
        [DataMember(EmitDefaultValue = false)]
        public string MaxRec { get; set; }
    }

    [DataContract]
    public class BillInqRq
    {
        [DataMember(EmitDefaultValue = false)]
        public RecCtrlIn RecCtrlIn { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string IncOpenAmt { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BillingAcct { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BankId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BillTypeCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DeliveryMethod { get; set; }
    }

    [DataContract]
    public class PresSvcRq
    {
        [DataMember(EmitDefaultValue = false)]
        public string RqUID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string AsyncRqUID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MsgRqHdr MsgRqHdr { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public BillInqRq BillInqRq { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember(EmitDefaultValue = false)]
        public SignonRq SignonRq { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string IsRetry { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public PresSvcRq PresSvcRq { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public PaySvcRq PaySvcRq { get; set; }
    }

     [DataContract]
    public class Arg0
    {
         [DataMember]
        public Request Request { get; set; }
        //[DataMember]
        //public ExtensionDataObject ExtensionData { get; set; }

    }
    [MessageContract(ProtectionLevel = ProtectionLevel.None)]
    public class processRequest
    {
         [MessageBodyMember(ProtectionLevel = ProtectionLevel.None, Namespace = "")]
        public Arg0 arg0 { get; set; }
    }
      [DataContract]
    public class AdditionalStatus
    {
        [DataMember(Order = 1,EmitDefaultValue = false)]
        public string StatusCode { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Severity { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string StatusDesc { get; set; }
    }
}