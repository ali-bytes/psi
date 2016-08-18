using System.Collections.Generic;
using System.Linq;
using Db;

namespace NewIspNL.Services{
    public class UserServices{
        public static User Get(int userId, ISPDataContext context){
            return context.Users.FirstOrDefault(u => u.ID == userId);
        }


        public static List<Branch> GetUserBranches(int userId, ISPDataContext context){
            var user = Get(userId, context);
            if(user == null || user.Group == null) return null;
            var level = user.Group.DataLevelID;
            if(user.BranchID == null) return null;
            switch(level){
                case 1 :
                    return context.Branches.ToList();
                case 2 :
                    return
                        context.Branches.Where(b => DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(b.ID)).ToList();
                //GetBranchAdminBranchIDs(userId, context).Contains(b.ID)).ToList();
                case 3 :
                    return context.Branches.Where(brnch => brnch.ID == user.BranchID).ToList();
            }
            return null;
        }


        public static List<int ?> GetBranchAdminBranchIDs(int userId, ISPDataContext context){
            var branchs = context.UserBranches
                .Where(ub => ub.UserID == userId).Select(ub => ub.BranchID).ToList();
            if(branchs.Count<=0)
                branchs = context.Users.Where(u => u.ID == userId).Select(u => u.BranchID).ToList();
            return branchs;
        }


        public static bool UserIs(int userId, ISPDataContext context, int groupId){
            var user = Get(userId,context);
            return user != null && user.GroupID != null && user.GroupID.Value == groupId;
        }
    }
}
