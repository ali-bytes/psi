using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using Db;


namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeModelView
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
        public int EmployeeDayAbs { get; set; }
        [DataMember]
        public string EmployeeDayAbsValue { get; set; }
        [DataMember]
        public string EmployeeHalfAbdsValue { get; set; }
        [DataMember]
        public double EmployeeHalfAbds { get; set; }

        public static EmployeeModelView To(Employe emp)
        {
            var pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

            return new EmployeeModelView
            {

                Id = emp.Id,
                Name = emp.Name,
                Rent = emp.rent,
                Email = emp.Email,
                Mobile = emp.Mobile,
                EmployeeDayAbs = pio.EmployeeSettings.FirstOrDefault(x => x.Id == emp.Id).EmployeeDayAbs,
                EmployeeDayAbsValue = pio.EmployeeSettings.FirstOrDefault(x => x.Id == emp.Id).EmployeeDayAbsValue,
                EmployeeHalfAbds = pio.EmployeeSettings.FirstOrDefault(x => x.Id == emp.Id).EmployeeHalfAbds,
                EmployeeHalfAbdsValue = pio.EmployeeSettings.FirstOrDefault(x => x.Id == emp.Id).EmployeeHalfAbdsValue
            };
        }
    }
}
