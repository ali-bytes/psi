using System.Collections.Generic;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IPhonesRepository{
        IQueryable<Phone> Phones { get; }


        void Save(Phone phone);


        void SaveMany(List<Phone> phones);


        //void Delete(Phone phone);
    }
}
