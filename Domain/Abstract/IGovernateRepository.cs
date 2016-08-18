using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IGovernateRepository{
        IQueryable<Governorate> Governorates { get; }
    }
}
