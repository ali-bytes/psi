using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Db;
using NewIspNL.Addons;

using NewIspNL.Service.Enums;

namespace NewIspNL.Service{
    public class CreditBusiness : ICreditBusiness{

        ISPDataContext pio = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        ////public CreditBusiness(IUnitOfWork unitOfWork){
        ////    _ofWork = unitOfWork;
        ////}


        //#region ICreditBusiness Members


        public decimal? GetCredit(int saveid)
        {
            var record = pio.Saves.Select(q => q).FirstOrDefault(q => q.Id == saveid);
            return record == null ? 0 : record.Total;
        }


        public Save GetLastRecord(int id){
            return pio.Saves.Select(q => q).FirstOrDefault(q => q.Id == id);
        
        }


        public CreditOperationResult Pay(decimal amount, int saveid)
        {
           
            decimal? credit = GetCredit(saveid);
            if(amount > credit){
                return CreditOperationResult.LessCredit;
            }
            var record = (from d in pio.Saves where d.Id==saveid
                          select d).FirstOrDefault();
            if(record != null){
                record.Total = record.Total - amount;
                
                //pio.StockBanks.Add(record);
                pio.SubmitChanges();
                return CreditOperationResult.Success;
            }
            return CreditOperationResult.LessCredit;
        }


        public CreditOperationResult Receive(decimal amount, int saveid)
        {

            var record = (from d in pio.Saves
                          where d.Id == saveid
                          select d).FirstOrDefault();
            if(record != null){
                record.Total = record.Total + amount;

              
                pio.SubmitChanges();

                return CreditOperationResult.Success;
            }
      
           
            return CreditOperationResult.Success;
        }

        public DayCreditItemModel CreditOfDay(DateTime time, int saveid ){
            var operations = pio.UserSavesHistories.ToList().Where(d => d.Time <= time.Date && d.SaveId == saveid).ToList();
            var creditOfDay = new DayCreditItemModel{
                                                        Credit = (decimal) operations.Sum(x => x.amount),
                                                        Day = time.ToShortDateString()
                                                    };
            return creditOfDay;
        }


   
        public void AddTreasuryMovement(DateTime date, decimal amount, int saveid, int userId,string note, int type,string kind)
        {
            var movement = new UserSavesHistory()
            {
                                                   SaveId = saveid,
                                                   Notes = note,
                                                   Time = date,
                                                ConfirmerUserId = userId,
                                                 
                                                   amount = amount,
                                                   Notes2 = kind
                                               };
            
            pio.UserSavesHistories.InsertOnSubmit(movement);
            pio.SubmitChanges();

        }
    }
}
