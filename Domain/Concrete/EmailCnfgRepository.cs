using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Concrete{
    public class EmailCnfgRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        public IQueryable<EmailCnfg> EmailCnfgs{
            get { return _context.EmailCnfgs; }
        }


        public void Save(EmailCnfg emailCnfg){
            if(emailCnfg.Id == 0){
                _context.EmailCnfgs.InsertOnSubmit(emailCnfg);
            }
            _context.SubmitChanges();
        }


        public void Delete(EmailCnfg emailCnfg){
            if(emailCnfg == null) return;
            _context.EmailCnfgs.DeleteOnSubmit(emailCnfg);
            _context.SubmitChanges();
        }


        public EmailCnfg GetActiveCnfg(){
            return _context.EmailCnfgs.FirstOrDefault(c => c.Active);
        }
    }
}
