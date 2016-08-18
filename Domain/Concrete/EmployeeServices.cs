using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BL.Templates;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class EmployeeServices : IEmployeeServices{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IEmployeeServices Members


        public IQueryable<Phone> EmployeeCustomersInState(int employeeId, int stateId){
            var phones = _context.Phones
                .Where(p => p.EmployeeId == employeeId && p.CallStateId == stateId);
            return phones;
        }


        public List<EmployeeContractsCount> ContractsCount(){
            var emps = _context.Users.ToList().Select(em => new{
                em.ID,
                em.UserName,
                em.UserPhone,
                Count = em.Phones.Count(p => p.CallStateId == 6)
            }).OrderByDescending(x => x.Count);
            var counts = emps.Select(emp => new EmployeeContractsCount{
                Count = emp.Count,
                Name = emp.UserName,
                Phone = emp.UserPhone,
                Id = emp.ID
            }).ToList();
            return counts;
        }


        #endregion
    }
}
