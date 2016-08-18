using System.Runtime.Serialization;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeReport
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Rent { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string HiringDate { get; set; }
           [DataMember]
        public decimal insurance { get; set; }

        public static EmployeeReport To(Employe emp)
        {

            return new EmployeeReport
            {

                Id = emp.Id,
                Name = emp.Name,
                Rent = emp.rent,
                Email = emp.Email,
                Mobile = emp.Mobile,
                insurance=emp.insurance,
                HiringDate = emp.HiringDate.ToShortDateString()
            };
        }
    }
}
