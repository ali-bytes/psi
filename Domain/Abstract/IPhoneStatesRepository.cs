using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IPhoneStatesRepository{
        //IQueryable<CallState> States();


        IQueryable<CallState> RejectedDataRejectedContractStates();


        IQueryable<CallState> EmployeeReportStates();
    }
}
