using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IComplaintsRepository{
        IQueryable<Complaint> Complaints { get; }


        void Save(Complaint complaint);


        //void Delete(Complaint complaint);
    }
}
