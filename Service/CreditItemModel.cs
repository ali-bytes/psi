using System.Runtime.Serialization;

namespace NewIspNL.Service{
    [DataContract]
    public class CreditItemModel{
        [DataMember]
        public int Id { get; set; }


        [DataMember]
        public string Credit { get; set; }


        [DataMember]
        public decimal CreditDecimal { get; set; }


        [DataMember]
        public string Time { get; set; }
    }
}
