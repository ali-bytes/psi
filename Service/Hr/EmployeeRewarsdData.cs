using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Db;

namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeRewarsdData
    {
        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public decimal value { get; set; }
        [DataMember]
        public string Time { get; set; }
        public static EmployeeRewarsdData To(EmployeeDebit employee)
        {
            return new EmployeeRewarsdData
            {
                EmployeeId = employee.employeeId,
                value = employee.Debit,
                Time = employee.Time.ToShortDateString(),

            };
        }
    }
}