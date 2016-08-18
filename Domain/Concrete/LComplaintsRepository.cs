using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class LComplaintsRepository : IComplaintsRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IComplaintsRepository Members


        public IQueryable<Complaint> Complaints{
            get { return _context.Complaints; }
        }


        public void Save(Complaint complaint){
            if(complaint.Id == 0){
                _context.Complaints.InsertOnSubmit(complaint);
                _context.SubmitChanges();
            }
            _context.SubmitChanges();
        }


        /*public void Delete(Complaint complaint){
            if(complaint != null){
                _context.Complaints.DeleteOnSubmit(complaint);
                _context.SubmitChanges();
            }
        }*/


        #endregion
    }
}
