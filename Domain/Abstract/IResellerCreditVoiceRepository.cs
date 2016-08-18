using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.BL.Abstract;
using NewIspNL.Domain.Concrete;

namespace NewIspNL.Domain.Abstract
{
    public interface IResellerCreditVoiceRepository
    {
        decimal GetNetCredit(int resellerId);


        IQueryable<ResellerCreditsVoice> ResellerCreditsVoices(int resellerId);


        bool CanSave(int resellerId, decimal amount);


        SaveVoiceResult SaveVoice(int resellerId, int userId, decimal amount, string notes, DateTime time);
        SaveVoiceResult SaveVoice(int resellerId, int userId, decimal amount, string notes, DateTime time, int requestId);

    }

    public class ResellerCreditVoiceRepository: IResellerCreditVoiceRepository
    {
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        readonly IResellerServices _resellerServices = new ResellerServices();


        #region IResellerCreditVoiceRepository Members


        public decimal GetNetCredit(int resellerId)
        {
            var lastCredit = _context.ResellerCreditsVoices.Where(c => c.ResellerId == resellerId).OrderByDescending(x => x.Id).FirstOrDefault();
            return lastCredit == null ? 0 : lastCredit.Net;
        }


        public bool CanSave(int resellerId, decimal amount)
        {
            if (_context.Options.First().Allowminuscredit)
            {
                return true;
            }
            var credit = GetNetCredit(resellerId);
            return credit + amount >= 0;
        }


        public SaveVoiceResult SaveVoice(int resellerId, int userId, decimal amount, string notes, DateTime time,int requestId)
        {
            if (CanSave(resellerId, amount))
            {
                var creditItem = new ResellerCreditsVoice
                {
                    Amount = amount,
                    Net = GetNetCredit(resellerId) + amount,
                    Notes = notes,
                    ResellerId = resellerId,
                    UserId = userId,
                    Time = time,
                    RechargeRequestId = requestId
                };
                _context.ResellerCreditsVoices.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveVoiceResult.Saved;
            }

            return SaveVoiceResult.NoCredit;
        }
        public SaveVoiceResult SaveVoice(int resellerId, int userId, decimal amount, string notes, DateTime time)
        {
            if (CanSave(resellerId, amount))
            {
                var creditItem = new ResellerCreditsVoice
                {
                    Amount = amount,
                    Net = GetNetCredit(resellerId) + amount,
                    Notes = notes,
                    ResellerId = resellerId,
                    UserId = userId,
                    Time = time,
                };
                _context.ResellerCreditsVoices.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveVoiceResult.Saved;
            }

            return SaveVoiceResult.NoCredit;
        }
       
        #endregion


        public IQueryable<ResellerCreditsVoice> ResellerCreditsVoices(int resellerId)
        {
            return _context.ResellerCreditsVoices.Where(c => c.ResellerId == resellerId);
        }
    }

    public enum SaveVoiceResult
    {
        Saved,

        NoCredit
    }
}
