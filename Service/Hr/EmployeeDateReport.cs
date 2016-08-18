using System.Runtime.Serialization;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeDateReport
    {

        [DataMember]
        public string EmployeeId { get; set; }

        [DataMember]
        public string Time { get; set; }
    }
}
