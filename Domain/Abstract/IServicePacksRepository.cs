using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IServicePacksRepository{
        IQueryable<ServicePackage> Packages { get; }
    }
}
