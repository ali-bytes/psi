using System.Runtime.Serialization;

namespace NewIspNL.Service{
    [DataContract]
    public class DayCreditItemModel{
        [DataMember]
        public string Day { get; set; }


        [DataMember]
        public decimal Credit { get; set; }
    }
}
