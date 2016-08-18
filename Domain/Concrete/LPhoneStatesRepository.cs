using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class LPhoneStatesRepository : IPhoneStatesRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IPhoneStatesRepository Members


        /*public IQueryable<CallState> States(){
            return _context.CallStates;
        }*/

        public IQueryable<CallState> RejectedDataRejectedContractStates(){
            return _context.CallStates.Where(s => s.Id == 4 || s.Id == 5);
        }


        public IQueryable<CallState> EmployeeReportStates(){
            return _context.CallStates.Where(s => s.Id == 4 || s.Id == 5 || s.Id == 6);
        }


        #endregion
    }
}
