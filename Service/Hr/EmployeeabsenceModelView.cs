using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using Db;


namespace NewIspNL.Service.Hr
{
    [DataContract]
    public class EmployeeabsenceModelView
    {

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public string AbsenceType { get; set; }

        [DataMember]
        public decimal AbsenceValue { get; set; }
        [DataMember]
        public string Time { get; set; }


        public static EmployeeabsenceModelView To(HrDaye emp)
        {

            var pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

          
            string absenceType = string.Empty;

            decimal dayValue = 0;
            double statetype;
            var state = pio.EmployeeSettings.FirstOrDefault(e => e.EmplyeeId == emp.EmployeeId);
            if (state != null)
            {

                switch (emp.StateId)
                {

                    case 2:
                        statetype = state.EmployeeDayAbs;

                        if (statetype == 1)
                        {
                            var day = pio.Employes.FirstOrDefault(x => x.Id == emp.EmployeeId).rent / 30;
                            dayValue = Convert.ToDecimal(state.EmployeeDayAbsValue) * day;
                        }
                        else
                        {
                            dayValue = Convert.ToDecimal(state.EmployeeDayAbsValue);

                        }
                        absenceType = "غياب يوم كامل";
                        break;
                    case 3:
                        statetype = state.EmployeeHalfAbds;
                        if (statetype == 1)
                        {
                            var day = (pio.Employes.FirstOrDefault(x => x.Id == emp.EmployeeId).rent / 30);
                            dayValue = Convert.ToDecimal(state.EmployeeHalfAbdsValue) * day;
                        }
                        else
                        {
                            dayValue = Convert.ToDecimal(state.EmployeeHalfAbdsValue);

                        }

                        absenceType = "غياب نصف يوم ";
                        break;
                    default:
                        break;
                }
            }


            return new EmployeeabsenceModelView
            {
                EmployeeId = emp.EmployeeId,
                AbsenceType = absenceType,
                AbsenceValue = dayValue,
                Time = emp.Time.ToShortDateString()
            };

        }

    }
}
