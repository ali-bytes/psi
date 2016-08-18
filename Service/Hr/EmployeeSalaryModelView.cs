using System.Runtime.Serialization;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeSalaryModelView
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int saveid { get; set; }

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public decimal TotalAbsences { get; set; }
        [DataMember]
        public decimal TotalDebits { get; set; }
        [DataMember]
        public decimal TotalSalary { get; set; }
        [DataMember]
        public decimal Salary { get; set; }
        [DataMember]
        public decimal TotalTravels { get; set; }
        [DataMember]
        public decimal Awards { get; set; }
        [DataMember]
        public decimal FullSalary { get; set; }
        [DataMember]
        public decimal Discounts { get; set; }
        [DataMember]
        public string Year { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public string Msg { get; set; }



        public static EmployeeSalaryModelView To(EmployeeSalary salary)
        {
            return new EmployeeSalaryModelView
            {
                Awards = salary.Awards,
                Discounts = salary.Discounts,
                EmployeeId = salary.employeeId,
                FullSalary = salary.FullSalary,
                Id = salary.Id,
                Month = salary.Month.ToString(),
                Salary = salary.Salary,
                TotalAbsences = salary.TotalAbsences,
                TotalDebits = salary.TotalDebits,
                TotalSalary = salary.TotalSalary,
                TotalTravels = salary.TotalTravels,
                Year = salary.Year.ToString()
            };
        }
    }
}
