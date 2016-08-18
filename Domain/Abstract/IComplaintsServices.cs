using System;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IComplaintsServices{
        IQueryable<Complaint> ComplaintsInPeriod(DateTime startDate, DateTime endDate);


        //IQueryable<Complaint> Complaints();


        //void Handled(Complaint complaint);
    }
}
