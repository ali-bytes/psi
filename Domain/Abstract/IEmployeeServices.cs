using System.Collections.Generic;
using System.Linq;
using BL.Templates;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IEmployeeServices{
        IQueryable<Phone> EmployeeCustomersInState(int employeeId, int stateId);


        List<EmployeeContractsCount> ContractsCount();
    }
}
