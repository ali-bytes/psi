using System;
using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract
{
    public interface IBranchCreditVoiceRepository
    {
        decimal GetNetCredit(int branchId);


        IQueryable<BranchCreditVoice> BranchCreditsVoices(int branchId);


        bool CanSave(int branchId, decimal amount);


        SaveVoiceResult SaveVoice(int branchId, int userId, decimal amount, string notes, DateTime time);


        //IEnumerable<ResellerPreviewItem> GetBrancheUponUserGroupWithCredit(int BranchId);
    }

    public class BranchCreditVoiceRepository: IBranchCreditVoiceRepository
    {
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        public decimal GetNetCredit(int branchId)
        {
            var lastCredit = _context.BranchCreditVoices.Where(c => c.BranchId == branchId).OrderByDescending(x => x.Id).FirstOrDefault();
            return lastCredit == null ? 0 : Convert.ToDecimal(lastCredit.Net);
        }

        public IQueryable<BranchCreditVoice> BranchCreditsVoices(int branchId){
            return _context.BranchCreditVoices.Where(a => a.BranchId == branchId);
        }

        public bool CanSave(int branchId, decimal amount)
        {
            if (_context.Options.First().Allowminuscredit)
            {
                return true;
            }
            var credit = GetNetCredit(branchId);
            return credit + amount >= 0;
        }

        public SaveVoiceResult SaveVoice(int branchId, int userId, decimal amount, string notes, DateTime time)
        {
            if (CanSave(branchId, amount))
            {
                var creditItem = new BranchCreditVoice
                {
                    Amount = amount,
                    Net = GetNetCredit(branchId) + amount,
                    Notes = notes,
                    BranchId = branchId,
                    UserId = userId,
                    Time = time,
                };
                _context.BranchCreditVoices.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveVoiceResult.Saved;
            }

            return SaveVoiceResult.NoCredit;
        }
    }
}