using System.Linq;
using Db;

namespace NewIspNL.Domain{
    public class RecieptCnfgRepository{
        readonly ISPDataContext _context;


        public RecieptCnfgRepository(ISPDataContext context){
            _context = context;
        }


        public virtual ReceiptCnfg GetResellerCnfg(int resellerId){
            return _context.ReceiptCnfgs.FirstOrDefault(x => x.ResellerId == resellerId);
        }


        public virtual ReceiptCnfg GetBranchCnfg(int branchId){
            return _context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == branchId);
        }


        /*public virtual ReceiptCnfg GetCnfgById(int id){
            return _context.ReceiptCnfgs.FirstOrDefault(x => x.Id == id);
        }*/



        public virtual ReceiptCnfg GetCnfg(int userId){
            var domian = new IspDomian(_context);
            var user = domian.Users.FirstOrDefault(x => x.ID == userId);
            if(user == null) return null;
            if(user.GroupID != null && user.GroupID == 6)
                return GetResellerCnfg(userId);
            return user.BranchID != null ? GetBranchCnfg(user.BranchID.Value) : null;
        }

       

        public void Save(ReceiptCnfg cnfg){
            if(cnfg.Id == 0)
                _context.ReceiptCnfgs.InsertOnSubmit(cnfg);
            _context.SubmitChanges();
        }
    }
}
