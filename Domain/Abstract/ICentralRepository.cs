using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface ICentralRepository{
        IQueryable<Central> Centrals { get; }


        void Save(Central central);


        bool Delete(Central central);
    }
}
