using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Concrete{
    public class LEmployeeRepository : IEmployeeRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IEmployeeRepository Members


        public IQueryable<User> Employees{
            get { return _context.Users; }
        }


        #endregion
    }

    public interface IEmployeeRepository{
        IQueryable<User> Employees { get; }
    }
}
