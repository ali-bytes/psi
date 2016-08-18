using System;
using System.Configuration;
using System.Linq;
using Db;

/*public class IBoxCreditRepository
{
	public IBoxCreditRepository()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}*/


namespace NewIspNL.Domain.Abstract{
    public interface IBoxCreditRepository
    {
        decimal GetNetBoxCredit(int boxId);

        IQueryable<BoxCredit> BoxCredits(int boxid);


        BoxCredit GetCredit(int id);


        BoxCredit GetLastBoxCredit();
        bool CanSave(int boxId, decimal amount);


        //SaveBoxResult SaveBox(int boxId, int userId, decimal amount, string notes, DateTime time,int ? demandId);
        SaveBoxResult SaveBox(int boxId, int userId, decimal amount, string notes, DateTime time);

        SaveBoxResult SaveBox(int boxId, int userId, decimal amount, string notes, DateTime time, int? rechargerequestid, int? rechargeBranchId);
        //IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupWithCredit(int userId);
    }

    public class BoxCreditRepository : IBoxCreditRepository
    {
        readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        //readonly IResellerServices _resellerServices = new ResellerServices();


        #region IResellerCreditRepository Members


        public BoxCredit GetCredit(int id){
            return _context.BoxCredits.FirstOrDefault(a => a.ID == id);
        }


        public BoxCredit GetLastBoxCredit(){
            return _context.BoxCredits.OrderByDescending(a => a.ID).FirstOrDefault();
        }


        public decimal GetNetBoxCredit(int boxid){
            var lastboxCredit = _context.BoxCredits.Where(c => c.BoxId == boxid).OrderByDescending(x => x.Time).FirstOrDefault();
            if(lastboxCredit != null) if(lastboxCredit.Net != null) return  (decimal) lastboxCredit.Net;
            else{
                return 0;
            }
            return 0;
        }


       /* public decimal GetNetBoxCredit(int boxid, int resellerid){
            var lastboxCredit = _context.BoxCredits.Where(c => c.BoxId == boxid && c.UserId==resellerid).OrderByDescending(x => x.Time).FirstOrDefault();
            if (lastboxCredit != null) if (lastboxCredit.Net != null) return (decimal)lastboxCredit.Net;
                else
                {
                    return 0;
                }
            else
            {
                return 0;
            }
        }*/


        public bool CanSave(int boxId, decimal amount)
        {
            var credit = GetNetBoxCredit(boxId);
            return credit + amount >= 0;
        }



        public IQueryable<BoxCredit> BoxCredits(int boxId)
        {
            return _context.BoxCredits.Where(c => c.BoxId == boxId);
        }


        /*public IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupWithCredit(int userId)
        {
            var resellersItems = _resellerServices.GetResellersUponUserGroup(Convert.ToInt32(userId));
            return resellersItems.Select(item => new ResellerPreviewItem
            {
                Id = item.Id,
                UserName = string.Format("{0} : {1}", string.Format("{0:####.##}", GetNetCredit(item.Id)) == "" ? "0" : string.Format("{0:####.##}", GetNetCredit(item.Id)), item.UserName)
            }).ToList();
        }*/


        #endregion


        //public SaveBoxResult SaveBox(int boxId, int userId, decimal amount, string notes, DateTime time,int ? demandId)
        //{
        //    if (CanSave(boxId, amount))
        //    {
        //        var creditItem = new BoxCredit
        //        {
        //            Amount = amount,
        //            Net = GetNetBoxCredit(boxId) + amount,
        //            Notes = notes,
        //            BoxId = boxId,
        //            UserId = userId,
        //            Time = time,
        //            demandId = demandId
        //        };
        //        _context.BoxCredits.InsertOnSubmit(creditItem);
        //        _context.SubmitChanges();
        //        return SaveBoxResult.Saved;
        //    } else{
        //        var creditItem = new BoxCredit
        //        {
        //            Amount = amount,
        //            Net = GetNetBoxCredit(boxId) + amount,
        //            Notes = notes,
        //            BoxId = boxId,
        //            UserId = userId,
        //            Time = time,
        //            demandId = demandId
        //        };
        //        _context.BoxCredits.InsertOnSubmit(creditItem);
        //        _context.SubmitChanges();
        //        return SaveBoxResult.Saved;
        //    }

        //    //return SaveBoxResult.NoCredit;
        //}
        public SaveBoxResult SaveBox(int boxId, int userId, decimal amount, string notes, DateTime time)
        {
            if (CanSave(boxId, amount))
            {
                var creditItem = new BoxCredit
                {
                    Amount = amount,
                    Net = GetNetBoxCredit(boxId) + amount,
                    Notes = notes,
                    BoxId = boxId,
                    UserId = userId,
                    Time = time,
                };
                _context.BoxCredits.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveBoxResult.Saved;
            }
            else
            {
                var creditItem = new BoxCredit
                {
                    Amount = amount,
                    Net = GetNetBoxCredit(boxId) + amount,
                    Notes = notes,
                    BoxId = boxId,
                    UserId = userId,
                    Time = time,
                };
                _context.BoxCredits.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveBoxResult.Saved;
            }

            //return SaveBoxResult.NoCredit;
        }
        public SaveBoxResult SaveBox(int boxId, int userId, decimal amount, string notes, DateTime time,int? rechargereqiestid,int? rechargeBranchId)
        {
            if (CanSave(boxId, amount))
            {
                var creditItem = new BoxCredit
                {
                    Amount = amount,
                    Net = GetNetBoxCredit(boxId) + amount,
                    Notes = notes,
                    BoxId = boxId,
                    UserId = userId,
                    Time = time,
                    RechargeRequestId = rechargereqiestid,
                    RechargeBranchId = rechargeBranchId
                };
                _context.BoxCredits.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveBoxResult.Saved;
            } else{
                var creditItem = new BoxCredit
                {
                    Amount = amount,
                    Net = GetNetBoxCredit(boxId) + amount,
                    Notes = notes,
                    BoxId = boxId,
                    UserId = userId,
                    Time = time,
                    RechargeRequestId = rechargereqiestid
                };
                _context.BoxCredits.InsertOnSubmit(creditItem);
                _context.SubmitChanges();
                return SaveBoxResult.Saved;
                
            }
            //return SaveBoxResult.NoCredit;
        }
    }

    public enum SaveBoxResult
    {
        Saved,

        NoCredit
    }
}