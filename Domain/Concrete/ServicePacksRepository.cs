using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class ServicePacksRepository : IServicePacksRepository{
        readonly ISPDataContext _context;


        public ServicePacksRepository(ISPDataContext context){
            _context = context;
        }


        public ServicePacksRepository(){
            _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        }


        #region IServicePacksRepository Members


        public IQueryable<ServicePackage> Packages{
            get { return _context.ServicePackages; }
        }


        #endregion
    }
}
