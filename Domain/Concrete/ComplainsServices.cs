using System;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class ComplainsServices : IComplaintsServices{
        readonly IComplaintsRepository _repository = new LComplaintsRepository();


        #region IComplaintsServices Members


        public IQueryable<Complaint> ComplaintsInPeriod(DateTime startDate, DateTime endDate){
            return _repository.Complaints.Where(
                                                c => c.RecordDate.Value.Date >= startDate.Date && c.Handled == false && c.RecordDate.Value.Date <= endDate.Date);
        }


        /*public IQueryable<Complaint> Complaints(){
            return _repository.Complaints;
        }*/


        /*public void Handled(Complaint complaint){
            complaint.Handled = true;
            _repository.Save(complaint);
        }*/


        #endregion
    }
}
