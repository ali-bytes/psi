using System.Linq;
using Db;

namespace NewIspNL.Domain.Concrete{
    public class SMSData{
        readonly ISPDataContext _context;

        public IQueryable<SMSCnfg> SmsCnfgs
        {
            get { return _context.SMSCnfgs; }
        }

        public SMSData(ISPDataContext context)
        {
            _context = context;
        }
        //public virtual SMSCnfg smsData(){
        //    return _context.SMSCnfgs.FirstOrDefault();
        //}
        public void Save(SMSCnfg smsCnfg)
        {
            if (smsCnfg.Id == 0)
            {
                _context.SMSCnfgs.InsertOnSubmit(smsCnfg);
            }
            _context.SubmitChanges();
        }


        public void Delete(SMSCnfg smsCnfg)
        {
            if (smsCnfg == null) return;
            _context.SMSCnfgs.DeleteOnSubmit(smsCnfg);
            _context.SubmitChanges();
        }


        public SMSCnfg GetActiveCnfg(){
            return _context.SMSCnfgs.FirstOrDefault();
        }

    }
}