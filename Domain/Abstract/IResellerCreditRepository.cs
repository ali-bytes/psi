using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.BL.Abstract;
using NewIspNL.Domain.Concrete;

namespace NewIspNL.Domain.Abstract{

    public interface IResellerCreditRepository{
        decimal GetNetCredit(int resellerId);


        IQueryable<ResellerCredit> ResellerCredits(int resellerId);

        bool CanSave(int resellerId, decimal amount);

        SaveResult Save(int resellerId, int userId, decimal amount, string notes, DateTime time, int rechargerequestId);
        SaveResult Save(int resellerId, int userId, decimal amount, string notes, DateTime time);

        IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupWithCredit(int userId);

        IEnumerable<ResellerPreviewItem> GetResellers(int userId);
    }

    public class ResellerCreditRepository : IResellerCreditRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        readonly IResellerServices _resellerServices = new ResellerServices();


        #region IResellerCreditRepository Members


        public decimal GetNetCredit(int resellerId){
            var lastCredit = _context.ResellerCredits.Where(c => c.ResellerId == resellerId).OrderByDescending(x => x.Id).FirstOrDefault();
            return lastCredit == null ? 0 : lastCredit.Net;
        }


        public bool CanSave(int resellerId, decimal amount){
            if(_context.Options.First().Allowminuscredit){
                return true;
            }
            var credit = GetNetCredit(resellerId);
            return credit + amount >= 0;
        }


        public SaveResult Save(int resellerId, int userId, decimal amount, string notes, DateTime time){
            if (CanSave(resellerId, amount))
            {
                var creditItem = new ResellerCredit
                {  
                    Amount = amount,
                    Net = GetNetCredit(resellerId) + amount,
                    Notes = notes,
                    ResellerId = resellerId,
                    UserId = userId,
                    Time = time,
                };
                _context.ResellerCredits.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveResult.Saved;
            }

            return SaveResult.NoCredit;
        }


        public IQueryable<ResellerCredit> ResellerCredits(int resellerId){
            return _context.ResellerCredits.Where(c => c.ResellerId == resellerId);
        }

        public SaveResult Save(int resellerId, int userId, decimal amount, string notes, DateTime time,int rechargerequestId){
            if(CanSave(resellerId, amount)){
                var creditItem = new ResellerCredit{
                    Amount = amount,
                    Net = GetNetCredit(resellerId) + amount,
                    Notes = notes,
                    ResellerId = resellerId,
                    UserId = userId,
                    Time = time,
                    RechargeRequestId = rechargerequestId
                };
                _context.ResellerCredits.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveResult.Saved;
            }

            return SaveResult.NoCredit;
        }

        public IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupWithCredit(int userId)
        {
            var user = _resellerServices.GetUser(userId);
            var resellersItems = _resellerServices.GetResellersUponUserGroupAccountManager(user);//_resellerServices.GetResellersUponUserGroup(Convert.ToInt32(userId));
            return resellersItems.Select(item => new ResellerPreviewItem{
                Id = item.Id,
                UserName = string.Format("{0} : {1}", string.Format("{0:####.##}", GetNetCredit(item.Id)) == "" ? "0" : string.Format("{0:####.##}", GetNetCredit(item.Id)), item.UserName)
            }).ToList();
        }


        public IEnumerable<ResellerPreviewItem> GetResellers(int userId){
            var resellersItems = _resellerServices.GetResellersUponUserGroup(Convert.ToInt32(userId));
            return resellersItems.Select(item => new ResellerPreviewItem
            {
                Id = item.Id,
                UserName = string.Format("{0}", item.UserName)
            }).ToList();
        }


        #endregion
    }

    public enum SaveResult{
        Saved,

        NoCredit
    }
}
