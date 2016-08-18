using System;
using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public class BranchCreditRepository{
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);



        public decimal GetNetCredit(int branchId){
            var lastCredit = _context.BranchCredits.Where(c => c.BranchId == branchId).OrderByDescending(x => x.Id).FirstOrDefault();
            return lastCredit == null ? 0 : lastCredit.Net;
        }


        public bool CanSave(int branchId, decimal amount){
            if(_context.Options.First().Allowminuscredit){
                return true;
            }
            var credit = GetNetCredit(branchId);
            return credit + amount >= 0;
        }


        public IQueryable<BranchCredit> BranchCredits(int branchId)
        {
            return _context.BranchCredits.Where(c => c.BranchId == branchId);
        }


        public SaveResult Save(int branchId, int userId, decimal amount, string notes, DateTime time){
            if (!CanSave(branchId, amount)) return SaveResult.NoCredit;
            var creditItem = new BranchCredit{
                Amount = amount,
                Net = GetNetCredit(branchId) + amount,
                Notes = notes,
                BranchId = branchId,
                UserId = userId,
                Time = time,
            };
            _context.BranchCredits.InsertOnSubmit(creditItem);
            _context.SubmitChanges();
            return SaveResult.Saved;
        }


       /* public IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupWithCredit(int userId){
            return DataLevelClass.GetUserBranches().Select(item => new ResellerPreviewItem{
                Id = item.ID,
                UserName = string.Format("{0} : {1}", string.Format("{0:####.##}", GetNetCredit(item.ID)) == "" ? "0" : string.Format("{0:####.##}", GetNetCredit(item.ID)), item.BranchName)
            }).ToList();
        }*/
    }
}