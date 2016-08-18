using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Concrete{
    public class LRejectionReasonsRepository : IRejectionReasonsRepository{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IRejectionReasonsRepository Members


        public IQueryable<RejectionReason> Reasons{
            get { return _context.RejectionReasons; }
        }


        public void Save(RejectionReason rejectionReason){
            if(rejectionReason.Id == 0){
                _context.RejectionReasons.InsertOnSubmit(rejectionReason);
            }
            _context.SubmitChanges();
        }


        public void Delete(RejectionReason rejectionReason){
            if(rejectionReason != null){
                _context.RejectionReasons.DeleteOnSubmit(rejectionReason);
                _context.SubmitChanges();
            }
        }


        #endregion
    }
}
