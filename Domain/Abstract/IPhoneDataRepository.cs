using Db;


namespace NewIspNL.Domain.Abstract{
    public interface IPhoneDataRepository{
        //IQueryable<PhoneData> PhoneDatas { get; }


        void Save(PhoneData phoneData);


        //void Delete(PhoneData phoneData);
    }
}
