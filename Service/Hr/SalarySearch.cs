using System.Runtime.Serialization;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class SalarySearch
    {
        [DataMember]
        public string EmployeeId { get; set; }
        [DataMember]
        public string Year { get; set; }
        [DataMember]
        public string Month { get; set; }
    }
}
