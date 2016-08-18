using System.Configuration;
using Db;
using NewIspNL.Domain.Abstract;

namespace NewIspNL.Domain.Concrete{
    public class LPhoneDataRepository : IPhoneDataRepository{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IPhoneDataRepository Members


        /*public IQueryable<PhoneData> PhoneDatas{
            get{
                return _context
                    .PhoneDatas;
            }
        }*/


        public void Save(PhoneData phoneData){
            if(phoneData.Id == 0){
                _context.PhoneDatas.InsertOnSubmit(phoneData);
            }
            _context.SubmitChanges();
        }


        /*public void Delete(PhoneData phoneData){
            if(phoneData != null){
                _context.PhoneDatas.DeleteOnSubmit(phoneData);
            }
        }*/


        #endregion
    }
}
