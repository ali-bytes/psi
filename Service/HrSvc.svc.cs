using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using Db;

using NewIspNL.Service;
using NewIspNL.Service.Hr;

using System.Web.UI;

namespace NewIspNL.Service
{
    [ServiceBehavior(UseSynchronizationContext = false,
         ConcurrencyMode = ConcurrencyMode.Multiple,
         InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HrSvc : IHrSvc
    {
        readonly ISPDataContext pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        private readonly EmployeeService _employeeService;

        public HrSvc()
        {
            _employeeService=new EmployeeService();
        }
        public List<EmployeeReport> Getemployees()
        {
            return _employeeService.Getemployees();
        }

        //public DateTime HireDate(int id)
        //{
        //    DateTime a = (from d in pio.Employes
        //                   where d.Id == id
        //                   select d.HiringDate).FirstOrDefault();

        //    return a;
        //}

        public List<UserSave_ViewModel> Getsaves()
        {
            int id = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);

            List<UserSave_ViewModel> save = (from d in pio.UserSaves
                       where d.UserId == id
                                    select d).Select(r=>new UserSave_ViewModel
                                    {
                                        id = r.Id,
                                        Save_id = r.SaveId,
                                        savename = r.Save.SaveName,
                                        User_id = r.UserId 
                                    }).ToList();
            return save;
        }


        public decimal Getemprewards(EmployeeDateReport empreport)
        {

            return _employeeService.Getrewards(empreport);
        }
        public decimal Getempdis(EmployeeDateReport empreport)
        {

            return _employeeService.Getdis(empreport);
        }

        public List<EmployeeStatesModelView> GetemployStates()
        {
            return _employeeService.GetemployStates();
        }

        public HrDauesModelView SaveDayes(HrDauesModelView hrDay)
        {
            return _employeeService.SaveDayes(hrDay);
        }

        public List<TravlsModelView> GetTravels(EmployeeDateReport empreport)
        {

            return _employeeService.GetTravels(empreport);
        }

        public List<WithdrawalModelView> GetWithdrawals(EmployeeDateReport empreport)
        {

            return _employeeService.GetWithdrawals(empreport);
        }

        public HrDauesModelView GetEmpDateState(EmployeeDateReport empreport)
        {
            var empSatae = _employeeService.GetEmpDateState(empreport) ?? new HrDaye();
            return HrDauesModelView.To(empSatae);
        }
        public List<EmployeeabsenceModelView> GetEmployeeabsences(EmployeeDateReport empreport)
        {
            return _employeeService.GetEmployeeabsences(empreport);
        }
        public EmployeeSalaryModelView SaveSalaries(EmployeeSalaryModelView salaryModel)
        {
           
            return _employeeService.SaveSalary(salaryModel);
        }

        public List<EmployeeSalaryModelView> GetAllSalaries(SalarySearch salarySearch)
        {

            return _employeeService.GetAllSalaries(salarySearch);
        }
    }
}
