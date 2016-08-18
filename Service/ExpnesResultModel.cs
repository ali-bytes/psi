using System;
using System.Runtime.Serialization;

namespace NewIspNL.Service
{
    [DataContract]
    public class ExpnesResultModel{
        [DataMember]
        public int Id { get; set; }


        [DataMember]
        public string Type { get; set; }


        [DataMember]
        public DateTime TimeSort { get; set; }


        [DataMember]
        public string Time { get; set; }


        [DataMember]
        public decimal Amount { get; set; }


        [DataMember]
        public string UserName { get; set; }


        [DataMember]
        public string Note { get; set; }
    }
}
