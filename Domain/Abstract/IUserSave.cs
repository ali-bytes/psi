using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Domain.Abstract
{
    public interface IUserSaveRepository
    {
        void InsertInsave(UserSavesHistory save, ISPDataContext context);
        SaveResult UpdateSave(int confirmUserId, int saveId, double amount, string movementNote, string note, ISPDataContext context);
        List<UserSave> SavesOfUser(int userId, ISPDataContext context);
        SaveResult BranchAndUserSaves(int saveId, int userId, double amount, string note1, string note2, ISPDataContext db6);
        bool CanSave(int saveId, decimal amount, ISPDataContext context);
        bool CanSaveBranch(int saveId, decimal amount, ISPDataContext context);

        bool AddPendingTransferRequest(int fromSaveId, int toSaveId, decimal amount, int userId, string note,
            ISPDataContext context);

        List<int?> SavesIdsOfUser(int userId, ISPDataContext context);
    }
    public class UserSaveRepository : IUserSaveRepository
    {

        public void InsertInsave(UserSavesHistory save, ISPDataContext context)
        {
            if (save.Id == 0)
            {
                context.UserSavesHistories.InsertOnSubmit(save);
            }
            context.SubmitChanges();
        }
        public bool CanSave(int saveId, decimal amount, ISPDataContext context)
        {
            var save = context.Saves.FirstOrDefault(a => a.Id == saveId);//GetNetCredit(resellerId);
            if (save != null)
            {
                var credit = save.Total;
                return credit + amount >= 0;
            }
            return false;
        }
        public bool CanSaveBranch(int branchId, decimal amount, ISPDataContext context)
        {
            var save = context.BranchesSaves.FirstOrDefault(a => a.BranchID == branchId);//GetNetCredit(resellerId);
            if (save != null)
            {
                var credit = Convert.ToDecimal(save.SaveValue);
                return credit + amount >= 0;
            }
            return false;
        }
        public SaveResult UpdateSave(int confirmuserId, int saveId, double amount, string movementNote, string note, ISPDataContext context)
        {

            var amont = Convert.ToDecimal(amount);
            if (amont < 0)
            {
                if (!CanSave(saveId, amont, context))
                {
                    return SaveResult.NoCredit;
                }
            }
            var save = context.Saves.FirstOrDefault(v => v.Id == saveId);
            if (save != null)
            {
                //var saveHistory = context.UserSavesHistories.FirstOrDefault(a => a.SaveId == save.Id);
                save.Total += amont;
                //if (user != null && save != null)
                InsertInsave(new UserSavesHistory
                {
                    SaveId = save.Id,
                    amount = amont,
                    ConfirmerUserId = confirmuserId,
                    Time = DateTime.Now.AddHours(),
                    Notes = movementNote,
                    Notes2 = note
                }, context);
                return SaveResult.Saved;

            }

            return SaveResult.NoCredit;
        }

        public List<UserSave> SavesOfUser(int userId, ISPDataContext context)
        {
            var saves = context.UserSaves.Where(a => a.UserId == userId).ToList();
            return saves;
        }
        public List<int?> SavesIdsOfUser(int userId, ISPDataContext context)
        {
            var saves = context.UserSaves.Where(a => a.UserId == userId).Select(a=>a.SaveId).ToList();
            return saves;
        }

        public SaveResult BranchAndUserSaves(int saveId, int userId, double amount, string note1, string note2, ISPDataContext db6)
        {
            try
            {
                var cussrentuser = db6.Users.FirstOrDefault(x => x.ID == userId);
                //if(cussrentuser==null){}
                if (cussrentuser != null)
                {
                    //var branchId = Convert.ToInt32(cussrentuser.BranchID);
                    if (amount < 0)
                    {
                        if (!CanSave(saveId, Convert.ToDecimal(amount), db6))
                        { return SaveResult.NoCredit; }
                    }


                    UpdateSave(userId, saveId, amount, note1, note2, db6);
                    return SaveResult.Saved;
                }
                return SaveResult.NoCredit;
            }
            catch
            {
                return SaveResult.NoCredit;
            }
            //return SaveResult.NoCredit;
        }

        public bool AddPendingTransferRequest(int fromSaveId, int toSaveId, decimal amount, int userId, string note, ISPDataContext context)
        {
            try
            {
                TransferBetweenSavesRequest req = new TransferBetweenSavesRequest()
                {
                    FromSave = fromSaveId,
                    ToSave = toSaveId,
                    Amount = amount,
                    RequestMakerNote = note,
                    RequestDate = DateTime.Now.AddHours(),
                    RequestMakerId = userId
                };
                context.TransferBetweenSavesRequests.InsertOnSubmit(req);
                context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

       
    }
    public class PendingRequestsTfBtSaves
    {
        public string SaveName { get; set; }
        public int SaveId { get; set; }
    }
}