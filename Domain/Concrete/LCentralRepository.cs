using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class LCentralRepository : ICentralRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region ICentralRepository Members


        public IQueryable<Central> Centrals{
            get { return _context.Centrals; }
        }


        public void Save(Central central){
            if(central.Id == 0){
                _context.Centrals.InsertOnSubmit(central);
            }
            _context.SubmitChanges();
        }


        public bool Delete(Central central){
            if(central == null) return false;
            var orders = _context.WorkOrders.Where(a => a.CentralId == central.Id);
            if (orders.Any())return false;
            _context.Centrals.DeleteOnSubmit(central);
            _context.SubmitChanges();
            return true;
        }


        #endregion
    }
}
