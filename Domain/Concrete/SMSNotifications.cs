using System.Collections.Generic;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Concrete
{
    /// <summary>
    /// Summary description for SMSNotifications
    /// </summary>
    public class SMSNotifications
    {
        readonly ISPDataContext _context;

        public IQueryable<SMSCaseNotification> SmsNotification
        {
            get { return _context.SMSCaseNotifications; }
        }
        public SMSNotifications(ISPDataContext context)
        {
            _context = context;
        }
        public void Save(SMSCaseNotification sms)
        {
            if (sms.Id == 0)
            {
                _context.SMSCaseNotifications.InsertOnSubmit(sms);
            }
            _context.SubmitChanges();
        }


        public void Delete(SMSCaseNotification sms)
        {
            if (sms == null) return;
            _context.SMSCaseNotifications.DeleteOnSubmit(sms);
            _context.SubmitChanges();
        }


        /*public SMSCnfg GetActiveCnfg()
    {
        return _context.SMSCnfgs.FirstOrDefault();
    }*/

        public List<SMSCaseNotification> GetList()
        {
            return _context.SMSCaseNotifications.ToList();
        }  
    }
}