using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.ServiceModel.Activation;
using System.Text;
using NewIspNL.Service.Hr;


namespace NewIspNL.Service
{
    [ServiceBehavior(UseSynchronizationContext = false,
         ConcurrencyMode = ConcurrencyMode.Multiple,
         InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
 
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "hiredateser" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select hiredateser.svc or hiredateser.svc.cs at the Solution Explorer and start debugging.
    public class hiredateser : Ihiredateser
    { 
        
    
        private readonly EmployeeService _employeeService;
   
      
               public HireDate Getdate(EmpIdHire empid)
        {
            //int id = empid.Id;
            //var ser = (from d in pio.Employes
            //           where d.Id == id
            //           select d.HiringDate).FirstOrDefault();
            //string a = ser.ToString();

            //return a;
            return _employeeService.Gethire(empid);


        }
      
    }
}
