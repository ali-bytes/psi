using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

using Db;

using NewIspNL.Service.Enums;
using NewIspNL.Service.Hr;


namespace NewIspNL.Service
{
    public class EmployeeService
    {
       ISPDataContext pio=new  ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

      


        public List<EmployeeReport> Getemployees()
        {


            return pio.Employes.Select(EmployeeReport.To).ToList();
          
        }

        public List<EmployeeStatesModelView> GetemployStates()
        {
            return pio.EmployeeStates.Select(EmployeeStatesModelView.To).ToList();
        }

        public HrDauesModelView SaveDayes(HrDauesModelView hrDay)
        {

            // var date = Helper.TransformDate(hrDay.Time);
            double date = 0.0;
            var da = Convert.ToDateTime(hrDay.date);
           // DateTime da = DateTime.ParseExact(hrDay.date, "dd/MM/yyyy", null);
            var hrDaies = pio.HrDayes.ToList().FirstOrDefault(e => e.EmployeeId == hrDay.EmployeeId && e.Time.Date == da.Date) ?? new HrDaye();
     
            hrDaies.EmployeeId = hrDay.EmployeeId;
            hrDaies.Code = Convert.ToInt32(pio.Employes.FirstOrDefault(x=>x.Id==hrDay.EmployeeId).Code);
            hrDaies.StateId = hrDay.StateId;
            hrDaies.Time =NewIspNL.Addons.Helpers.TransformDate(hrDay.date);
            //    var ss = string.Format("{0}.{1}", DateTime.Now.Hour, DateTime.Now.Minute);
            var ss = Convert.ToDouble(hrDay.Time);
            if (hrDay.StateId == 1&&hrDaies.Attendance==null)
            {

                hrDaies.Attendance = Convert.ToDouble(ss);
                pio.HrDayes.InsertOnSubmit(hrDaies);
                pio.SubmitChanges();
                return new HrDauesModelView
                {
                    Id = hrDaies.Id
                };
            }
            else if (hrDay.StateId == 2 && hrDaies.Attendance != null && hrDaies.Leave == null)
            {
                hrDaies.Leave = Convert.ToDouble(ss);
                pio.SubmitChanges();
                return new HrDauesModelView
                {
                    Id = hrDaies.Id
                };
            }
            else
            {

                return new HrDauesModelView
                {
                    Id = 0
                };
            }


           
          

           
        }

        public List<TravlsModelView> GetTravels(EmployeeDateReport empreport)
        {
            int employeeId = Convert.ToInt32(empreport.EmployeeId);
            var date =Addons. Helpers.TransformDate(empreport.Time);
            var travls = pio.EmployeeAssemplies.Where(e => e.EmployeeId == employeeId && e.Time.Month == date.Month).ToList();
            return travls.Select(TravlsModelView.To).ToList();
        }
       
        public HireDate Gethire(EmpIdHire empid)
        {
            int employeeId = Convert.ToInt32(empid.EmployeeId);

            var travls = pio.Employes.Where(e => e.Id == employeeId).ToList();
            return travls.Select(HireDate.To).FirstOrDefault();
        }
        public List<WithdrawalModelView> GetWithdrawals(EmployeeDateReport empreport)
        {
            int employeeId = Convert.ToInt32(empreport.EmployeeId);
            var date = Addons.Helpers.TransformDate(empreport.Time);

            var travls = pio.EmployeeDebits.ToList().Where(e => e.employeeId == employeeId && e.Time.Month == date.Month && e.Time.Year == date.Year).ToList();
            return travls.Select(WithdrawalModelView.To).ToList();
        }
        public decimal Getrewards(EmployeeDateReport empreport)
        {
            int id = Convert.ToInt32(empreport.EmployeeId);
            var date = Addons.Helpers.TransformDate(empreport.Time);
            try
            {
                decimal save = (from d in pio.Rewards
                                where d.empid == id && d.kind == "اضافي" && d.date.Month == date.Month && d.date.Year == date.Year
                                select d.value).Sum();
                return save;
            }
            catch (Exception)
            {
                const decimal save = 0;
                return save;
            }
        }
        public decimal Getdis(EmployeeDateReport empreport)
        {
            int id = Convert.ToInt32(empreport.EmployeeId);
            var date = Addons.Helpers.TransformDate(empreport.Time);
            try
            {
                decimal save = (from d in pio.Rewards
                                where d.empid == id && d.kind == "خصم" && d.date.Month == date.Month && d.date.Year == date.Year
                                select d.value).Sum();
                return save;
            }
            catch (Exception)
            {
                const decimal save = 0;
                return save;
            }
        }

        public List<EmployeeabsenceModelView> GetEmployeeabsences(EmployeeDateReport empreport)
        {
            int employeeId = Convert.ToInt32(empreport.EmployeeId);
            DateTime dateTime = Addons.Helpers.TransformDate(empreport.Time);
            EmployeeSetting employeeSetting = pio.EmployeeSettings.FirstOrDefault((EmployeeSetting e) => e.EmplyeeId == employeeId);
            if (employeeSetting == null)
            {
                return new List<EmployeeabsenceModelView>();

            }

            string arg_9C_0 = string.Empty;
            IQueryable<HrDaye> source =
                from e in this.pio.HrDayes
                where e.EmployeeId == employeeId
                select e;
            var list = new List<EmployeeabsenceModelView>();
            int num = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            for (int i = 1; i <= num; i++)
            {
                DateTime dayTime;
                bool flag = DateTime.TryParse(string.Format("{0}/{1}/{2}", i, dateTime.Month, dateTime.Year), out dayTime);
                if (flag && dayTime.DayOfWeek != DayOfWeek.Friday)
                {
                    IQueryable<HrDaye> queryable =
                        from e in source
                        where e.Time.Day == dayTime.Day && e.Time.Month == dayTime.Month && e.Time.Year == dayTime.Year
                        select e;
                    if (!queryable.Any<HrDaye>() && Convert.ToInt32(employeeSetting.EmployeeDayAbsValue) > 0)
                    {
                        double num2 = (double)employeeSetting.EmployeeDayAbs;
                        decimal num3;
                        if (num2 == 1.0)
                        {
                            decimal d = this.pio.Employes.FirstOrDefault(x=>x.Id==employeeId).rent / 26m;
                            num3 = Convert.ToDecimal(employeeSetting.EmployeeDayAbsValue) * d;
                        }
                        else
                        {
                            num3 = Convert.ToDecimal(employeeSetting.EmployeeDayAbsValue);
                        }
                        list.Add(new EmployeeabsenceModelView
                        {
                            EmployeeId = employeeId,
                            AbsenceType = "غياب يوم كامل",
                            AbsenceValue = num3,
                            Time = dayTime.ToShortDateString()
                        });
                    }
                    else
                    {
                        foreach (HrDaye current in queryable)
                        {
                            double attendance = this.pio.Employes.FirstOrDefault(x => x.Id == employeeId).Attendance;
                            double leave = this.pio.Employes.FirstOrDefault(x => x.Id == employeeId).Leave;
                            if (!current.Attendance.HasValue)
                            {
                                current.Attendance = new double?(attendance);
                            }
                            if (!current.Leave.HasValue)
                            {
                                current.Leave = new double?(leave);
                            }
                            double currentArrendance = Convert.ToDouble(current.Attendance.ToString());
                            double currentLeave = Convert.ToDouble(current.Leave.ToString());
                            decimal num6 = Convert.ToDecimal(this.pio.Employes.FirstOrDefault(x => x.Id == employeeId).LastDiscount);
                            double num7 = leave - attendance;
                            if (currentArrendance > attendance)
                            {
                                double num8 = SetTimeHour(currentArrendance - attendance);
                                double num2 = (double)employeeSetting.EmployeeHalfAbds;
                                decimal num3;
                                if (num2 == 1.0)
                                {
                                    double value = Convert.ToDouble(this.pio.Employes.FirstOrDefault(x => x.Id == employeeId).rent) / (num7 * 26.0);
                                    num3 = Convert.ToDecimal(employeeSetting.EmployeeHalfAbdsValue) * Convert.ToDecimal(value) * Convert.ToDecimal(num8);
                                }
                                else
                                {
                                    num3 = Convert.ToDecimal(employeeSetting.EmployeeHalfAbdsValue) * Convert.ToDecimal(num8);
                                }
                                if (Convert.ToInt32(employeeSetting.EmployeeHalfAbdsValue) > 0)
                                {
                                    if (num3 > num6)
                                    {
                                        num3 = num6;
                                    }
                                    list.Add(new EmployeeabsenceModelView
                                    {
                                        EmployeeId = employeeId,
                                        AbsenceType = string.Format(" خصم تاخير {0}", num8),
                                        AbsenceValue = num3,
                                        Time = current.Time.ToShortDateString()
                                    });
                                }
                            }
                            if (currentLeave > leave)
                            {
                                if (currentLeave - SetTimeHour(leave + 0.45) > 0)
                                {
                                    double num8 = SetTimeHour((SetTimeHour(currentLeave) - leave));
                                    double num2 = (double)employeeSetting.EmployeeAdd;
                                    decimal num3;
                                    if (num2 == 1.0)
                                    {
                                        double value2 = Convert.ToDouble(this.pio.Employes.FirstOrDefault(x => x.Id == employeeId).rent) / (num7 * 26.0);
                                        num3 = Convert.ToDecimal(employeeSetting.EmployeeAddValue) * Convert.ToDecimal(value2) * Convert.ToDecimal(num8);
                                    }
                                    else
                                    {
                                        num3 = Convert.ToDecimal(employeeSetting.EmployeeAddValue) * Convert.ToDecimal(num8);
                                    }
                                    if (Convert.ToInt32(employeeSetting.EmployeeAddValue) > 0)
                                    {
                                        list.Add(new EmployeeabsenceModelView
                                        {
                                            EmployeeId = employeeId,
                                            AbsenceType = string.Format(" اضافى {0}", num8),
                                            AbsenceValue = num3 * -1m,
                                            Time = current.Time.ToShortDateString()
                                        });
                                    }
                                }
                            }
                            if (currentLeave < leave)
                            {

                                double num8 = SetTimeHour(leave - currentLeave);
                                double num2 = (double)employeeSetting.EmployeeHalfAbds;
                                decimal num3;
                                if (num2 == 1.0)
                                {
                                    double value3 = Convert.ToDouble(this.pio.Employes.FirstOrDefault(x => x.Id == employeeId).rent) / (num7 * 26.0);
                                    num3 = Convert.ToDecimal(employeeSetting.EmployeeHalfAbdsValue) * Convert.ToDecimal(value3) * Convert.ToDecimal(num8);
                                }
                                else
                                {
                                    num3 = Convert.ToDecimal(employeeSetting.EmployeeHalfAbdsValue) * Convert.ToDecimal(num8);
                                }
                                if (Convert.ToInt32(employeeSetting.EmployeeHalfAbdsValue) > 0)
                                {
                                    if (num3 > num6)
                                    {
                                        num3 = num6;
                                    }
                                    list.Add(new EmployeeabsenceModelView
                                    {
                                        EmployeeId = employeeId,
                                        AbsenceType = string.Format(" خصم انصراف {0}", num8),
                                        AbsenceValue = num3,
                                        Time = current.Time.ToShortDateString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return list.ToList<EmployeeabsenceModelView>();
        }

        private double SetTimeHour(double value)
        {

            //double setvalue=Convert.ToDouble(s);
            var hour = (int)Math.Truncate(value);
            int minite = 0;
            var stringValue = value.ToString(CultureInfo.InvariantCulture).Split('.');
            if (stringValue.Length > 1)
            {

                string s = stringValue[1].Length > 2 ? stringValue[1].Substring(0, 2) : stringValue[1];
                minite = int.Parse(s);
            }
            while (minite >= 60)
            {
                hour++;
                minite -= 60;
            }

            return Convert.ToDouble(string.Format("{0}.{1}", hour, minite));
        }
        public HrDaye GetEmpDateState(EmployeeDateReport empreport)
        {
            int employeeId = Convert.ToInt32(empreport.EmployeeId);
            var date =Addons. Helpers.TransformDate(empreport.Time);
            var absences = pio.HrDayes.ToList().FirstOrDefault(e => e.EmployeeId == employeeId && e.Time.Date == date.Date);
            return absences;
        }

        public EmployeeSalaryModelView SaveSalary(EmployeeSalaryModelView salaryModel)
        {
            int saveid = salaryModel.saveid;
            int month = Addons.Helpers.TransformDate(salaryModel.Month.ToString()).Month;
            int year = Addons.Helpers.TransformDate(salaryModel.Year.ToString()).Year;
            int empId = Convert.ToInt32(salaryModel.EmployeeId);
            int userId =Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
            var salary =
                pio.EmployeeSalaries
                    .FirstOrDefault(e => e.employeeId == empId && e.Month == month && e.Year == year);
            if (salary != null)
            {
                return new EmployeeSalaryModelView
                {
                    Id = 0,
                    Msg = "تم أضافة المرتب من قبل "
                };
            }
            salary = new EmployeeSalary
            {
                Awards = Convert.ToDecimal(salaryModel.Awards),
                Discounts = Convert.ToDecimal(salaryModel.Discounts),
                FullSalary = Convert.ToDecimal(salaryModel.FullSalary),
                employeeId = empId,
                Salary = Convert.ToDecimal(salaryModel.Salary),
                TotalAbsences = Convert.ToDecimal(salaryModel.TotalAbsences),
                TotalDebits = Convert.ToDecimal(salaryModel.TotalDebits),
                TotalSalary = Convert.ToDecimal(salaryModel.TotalSalary),
                TotalTravels = Convert.ToDecimal(salaryModel.TotalTravels),
                Year = year,
                Month = month,

            };
            pio.EmployeeSalaries.InsertOnSubmit(salary);
            pio.SubmitChanges();
            var amount = Convert.ToDecimal(salary.FullSalary);

            ICreditBusiness business = new CreditBusiness();
            var result = business.Pay(amount,saveid);
            switch (result)
            {
                case CreditOperationResult.Success:



                    business.AddTreasuryMovement(DateTime.Now, amount , saveid, userId, " مرتبات", 1," خروج من الخزينة");
                           return new EmployeeSalaryModelView
                    {
                        Id = salary.Id,
                        Msg = "Done"
                    };

                case CreditOperationResult.LessCredit:
                    return new EmployeeSalaryModelView
                    {
                        Id = salary.Id,
                        Msg = result.ToString()
                    };

                default:
                    return new EmployeeSalaryModelView
                    {
                        Id = salary.Id,
                        Msg = result.ToString()
                    };
            }

        }

        public List<EmployeeSalaryModelView> GetAllSalaries(SalarySearch salarySearch)
        {
            var empId = Convert.ToInt32(salarySearch.EmployeeId);
            var yearId = Convert.ToInt32(salarySearch.Year);
            var monthId = Convert.ToInt32(salarySearch.Month);

            List<EmployeeSalary> salaries = empId == 0 ? pio.EmployeeSalaries.Where(s => s.Year == yearId && s.Month == monthId).ToList() : pio.EmployeeSalaries.Where(s => s.Year == yearId && s.employeeId == empId && s.Month == monthId).ToList();

            return salaries.Select(EmployeeSalaryModelView.To).ToList();
        }
    }
}
