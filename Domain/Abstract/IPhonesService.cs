using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IPhonesServices{
        IQueryable<Phone> GetPhonesByStateId(int stateId);
        void UpdateState(int phoneId, int newStateId);
        void UpdateNewEmployee(int phoneId, int newEmployee);
    }
}
