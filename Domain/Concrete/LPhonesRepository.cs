using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class LPhonesRepository : IPhonesRepository{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IPhonesRepository Members


        public IQueryable<Phone> Phones{
            get { return _context.Phones; }
        }


        public void Save(Phone phone){
            if(phone.Id == 0){
                _context.Phones.InsertOnSubmit(phone);
            }
            _context.SubmitChanges();
        }


        public void SaveMany(List<Phone> phones){
            foreach(var phone in phones){
                Save(phone);
            }
        }


       /* public void Delete(Phone phone){
            if(phone != null){
                _context.Phones.DeleteOnSubmit(phone);
            }
        }*/


        #endregion
    }
}
