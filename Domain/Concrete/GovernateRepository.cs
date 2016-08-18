using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class GovernateRepository : IGovernateRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IGovernateRepository Members


        public IQueryable<Governorate> Governorates{
            get { return _context.Governorates; }
        }


        #endregion
    }
}
