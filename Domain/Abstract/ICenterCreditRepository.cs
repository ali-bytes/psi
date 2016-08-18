using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
  
    public interface ICenterCreditRepository{
        decimal GetNetCredit(int centerId);
        IQueryable<CenterCredit> CenterCredits(int centerId);
        bool CanSave(int centerId, decimal amount);
        SaveResult Save(int centerId, int userId, decimal amount, string notes, DateTime time);
    }

    public class CenterCreditRepository:ICenterCreditRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        public decimal GetNetCredit(int centerId){
            var lastCredit = _context.CenterCredits.Where(c => c.CenterUserId == centerId).OrderByDescending(x => x.Id).FirstOrDefault();
            return lastCredit == null ? 0 : Convert.ToDecimal(lastCredit.Net);
        }


        public IQueryable<CenterCredit> CenterCredits(int centerId){
            return _context.CenterCredits.Where(c => c.CenterUserId == centerId);
        }


        public bool CanSave(int centerId, decimal amount){
            if (_context.Options.First().Allowminuscredit)
                return true;
            var credit = GetNetCredit(centerId);
            return credit + amount >= 0;
        }


        public SaveResult Save(int centerId, int userId, decimal amount, string notes, DateTime time){
            if (!CanSave(centerId, amount)) return SaveResult.NoCredit;
            var creditItem = new CenterCredit
            {
                Amount = amount,
                Net = GetNetCredit(centerId) + amount,
                Notes = notes,
                CenterUserId = centerId,
                UserId = userId,
                Time = time,
            };
            _context.CenterCredits.InsertOnSubmit(creditItem);
            _context.SubmitChanges();
            return SaveResult.Saved;
        }


        /*public IEnumerable<CenterPreviewItem> GetCenterUponUserGroupWithCredit(int userId){
            var resellersItems = _resellerServices.GetResellersUponUserGroup(Convert.ToInt32(userId));
            return resellersItems.Select(item => new ResellerPreviewItem
            {
                Id = item.Id,
                UserName = string.Format("{0} : {1}", string.Format("{0:####.##}", GetNetCredit(item.Id)) == "" ? "0" : string.Format("{0:####.##}", GetNetCredit(item.Id)), item.UserName)
            }).ToList();
        }*/


        /*public IEnumerable<CenterPreviewItem> GetCenter(int userId){
            var resellersItems = _resellerServices.GetResellersUponUserGroup(Convert.ToInt32(userId));
            return resellersItems.Select(item => new ResellerPreviewItem
            {
                Id = item.Id,
                UserName = string.Format("{0}", item.UserName)
            }).ToList();
        }*/
    }
}