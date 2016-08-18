using System.Linq;
using Db;

namespace NewIspNL.Domain.Concrete{
    public interface IRejectionReasonsRepository{
        IQueryable<RejectionReason> Reasons { get; }


        void Save(RejectionReason rejectionReason);


        void Delete(RejectionReason rejectionReason);
    }
}
