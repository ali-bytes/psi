//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Security;
//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.Web;

//namespace NewIspNL.FawryService
//{
//    public class FawryViewModel2
//    {
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class ProcessRequest
//    {

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None, Namespace = "")]
//        public string BillingAcct { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string ServerDt { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Amt { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string StatusCode { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Severity { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string StatusDesc { get; set; }



//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class SignonRs
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string ServerDt { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Language { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public SignonProfile SignonProfile { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class Status
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string StatusCode { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Severity { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string StatusDesc { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class CurAmt
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Amt { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class BillSummAmt
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public CurAmt CurAmt { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class BillInfo
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public BillSummAmt BillSummAmt { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class BillRec
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string BillingAcct { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string BillTypeCode { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public BillInfo BillInfo { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class BillInqRs
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public BillRec BillRec { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class PresSvcRs
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string RqUID { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public Status Status { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public BillInqRs BillInqRs { get; set; }
//    }
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class Response
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public SignonRs SignonRs { get; set; }
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public PresSvcRs PresSvcRs { get; set; }
//        //[MessageBodyMember(ProtectionLevel = ProtectionLevel.None, Namespace = "")]
//        //public ExtensionDataObject ExtensionData { get; set; }
//    }
//    //------------------------------------------------
//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class SignonProfile
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Sender { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Receiver { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string MsgCode { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string Version { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class SignonRq
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string ClientDt { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string CustLangPref { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string SuppressEcho { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public SignonProfile SignonProfile { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class NetworkTrnInfo
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string OriginatorCode { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class MsgRqHdr
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public NetworkTrnInfo NetworkTrnInfo { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class RecCtrlIn
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string MaxRec { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class BillInqRq
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public RecCtrlIn RecCtrlIn { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string IncOpenAmt { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string BillingAcct { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string BankId { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string BillTypeCode { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string DeliveryMethod { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class PresSvcRq
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string RqUID { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string AsyncRqUID { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public MsgRqHdr MsgRqHdr { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public BillInqRq BillInqRq { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class Request
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public SignonRq SignonRq { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public string IsRetry { get; set; }

//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public PresSvcRq PresSvcRq { get; set; }
//    }

//    [MessageContract(  ProtectionLevel = ProtectionLevel.None)]
//    public class Arg0 
//    {
//        [MessageBodyMember(ProtectionLevel = ProtectionLevel.None,Namespace = "")]
//        public Request Request { get; set; }
//        //[MessageBodyMember(ProtectionLevel = ProtectionLevel.None, Namespace = "")]
//        //public ExtensionDataObject ExtensionData { get; set; }

//    }
//}