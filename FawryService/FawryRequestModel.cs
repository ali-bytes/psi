using System.Runtime.Serialization;

namespace NewIspNL.FawryService
{
    public class FawryRequestModel
    {
    }



    //[DataContract]
    //public class SignonProfile
    //{
    //    [DataMember]
    //    public string Sender { get; set; }

    //    [DataMember]
    //    public string Receiver { get; set; }

    //    [DataMember]
    //    public string MsgCode { get; set; }

    //    [DataMember]
    //    public string Version { get; set; }
    //}

    //[DataContract]
    //public class SignonRq
    //{
    //    [DataMember]
    //    public string ClientDt { get; set; }

    //    [DataMember]
    //    public string CustLangPref { get; set; }

    //    [DataMember]
    //    public string SuppressEcho { get; set; }

    //    [DataMember]
    //    public SignonProfile SignonProfile { get; set; }
    //}

    //[DataContract]
    //public class NetworkTrnInfo
    //{
    //    [DataMember]
    //    public string OriginatorCode { get; set; }
    //}

    //[DataContract]
    //public class MsgRqHdr
    //{
    //    [DataMember]
    //    public NetworkTrnInfo NetworkTrnInfo { get; set; }
    //}

    //[DataContract]
    //public class RecCtrlIn
    //{
    //    [DataMember]
    //    public string MaxRec { get; set; }
    //}

    //[DataContract]
    //public class BillInqRq
    //{
    //    [DataMember]
    //    public RecCtrlIn RecCtrlIn { get; set; }

    //    [DataMember]
    //    public string IncOpenAmt { get; set; }

    //    [DataMember]
    //    public string BillingAcct { get; set; }

    //    [DataMember]
    //    public string BankId { get; set; }

    //    [DataMember]
    //    public string BillTypeCode { get; set; }

    //    [DataMember]
    //    public string DeliveryMethod { get; set; }
    //}

    //[DataContract]
    //public class PresSvcRq
    //{
    //    [DataMember]
    //    public string RqUID { get; set; }

    //    [DataMember]
    //    public string AsyncRqUID { get; set; }

    //    [DataMember]
    //    public MsgRqHdr MsgRqHdr { get; set; }

    //    [DataMember]
    //    public BillInqRq BillInqRq { get; set; }
    //}

    //[DataContract]
    //public class Request
    //{
    //    [DataMember]
    //    public SignonRq SignonRq { get; set; }

    //    [DataMember]
    //    public string IsRetry { get; set; }

    //    [DataMember]
    //    public PresSvcRq PresSvcRq { get; set; }
    //}

    //[DataContract]
    //public class Arg0 : IExtensibleDataObject
    //{
    //    [DataMember]
    //    public Request Request { get; set; }

    //    public ExtensionDataObject ExtensionData { get; set; }
    //}
}