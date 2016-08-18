using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class PhonesServices : IPhonesServices{
        readonly IPhonesRepository _phonesRepository = new LPhonesRepository();


        #region IPhonesServices Members


        public IQueryable<Phone> GetPhonesByStateId(int stateId){
            var phones = _phonesRepository.Phones.Where(p => p.CallStateId == stateId);
            return phones;
        }


        public void UpdateState(int phoneId, int newStateId){
            var phone = _phonesRepository.Phones.FirstOrDefault(p => p.Id == phoneId);
            if(phone == null) return;
            phone.CallStateId = newStateId;
            _phonesRepository.Save(phone);
        }
        public void UpdateNewEmployee(int phoneId,int newEmployee)
        {
            var phone = _phonesRepository.Phones.FirstOrDefault(p => p.Id == phoneId);
            if (phone == null) return;
            phone.EmployeeId = newEmployee;
            _phonesRepository.Save(phone);
        }

        #endregion
    }
}
