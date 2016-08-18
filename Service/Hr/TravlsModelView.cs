using System.Runtime.Serialization;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class TravlsModelView
    {

        [DataMember]
        public int EmployeeId { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public decimal Increase { get; set; }
        [DataMember]
        public string Time { get; set; }

        public static TravlsModelView To(EmployeeAssemply employee)
        {
            return new TravlsModelView
            {
                EmployeeId = employee.EmployeeId,
                Increase = employee.Increase,
                Time = employee.Time.ToShortDateString(),
                Type = employee.Type == 1 ? "سفريات" : "تركيبات"
            };
        }


    }
}
