using System.Runtime.Serialization;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class WithdrawalModelView
    {
        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public decimal Debit { get; set; }
        [DataMember]
        public string Time { get; set; }

        public static WithdrawalModelView To(EmployeeDebit employee)
        {
            return new WithdrawalModelView
            {
                EmployeeId = employee.employeeId,
                Debit = employee.Debit,
                Time = employee.Time.ToShortDateString(),

            };
        }

    }
}
