using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BL.Concrete;
using Db;
using NewIspNL.BL.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Domain.Concrete{
    public class ResellerServices : IResellerServices{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        readonly IspEntries _entries;


        public ResellerServices(IspEntries entries){
            _entries = entries;
        }


        public ResellerServices(){
            _entries = new IspEntries();
        }


        #region IResellerServices Members


        /*public string GetResellerName(int ? userId){
            if(userId == null){
                return "-";
            }
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            return user == null ? "-" : user.UserName;
        }*/


        public IEnumerable<ResellerPreviewItem> GetResellersUponUserGroup(User user)
        {
            if (user == null) return null;
            List<User> resellers;
            switch (user.GroupID)
            {
                    // admin 
                case 1:
                    resellers = GetResellers(null);
                    return resellers.Select(x => new ResellerPreviewItem
                    {
                        Id = x.ID,
                        UserName = x.UserName
                    });


                    // reseller
                case 6:
                    resellers = _context.Users.Where(u => u.ID == user.ID).ToList();

                    return resellers.Select(x => new ResellerPreviewItem
                    {
                        Id = x.ID,
                        UserName = x.UserName
                    });

                    //Branch
                default:
                    var templates = new List<ResellerPreviewItem>();
                    var branches = _context.UserBranches.Where(b => b.UserID == user.ID);
                    if (branches.Any())
                    {
                        foreach (var branch in branches)
                        {
                            var current = GetResellersByBranchId(Convert.ToInt32(branch.BranchID));

                            foreach (var reseller in current)
                            {
                                templates.Add(new ResellerPreviewItem
                                {
                                    Id = reseller.ID,
                                    UserName = branch.Branch.BranchName + " / " + reseller.UserName
                                });
                            }
                        }
                        return templates;
                    }
                    resellers = GetResellers(user.ID);
                    return resellers.Select(x => new ResellerPreviewItem
                    {
                        Id = x.ID,
                        UserName = x.UserName
                    });

                //default:
                //    resellers = GetResellers(user.ID);
                //    return resellers.Select(x => new ResellerPreviewItem
                //    {
                //        Id = x.ID,
                //        UserName = x.UserName
                //    });
            }
        }


        public IEnumerable<ResellerPreviewItem> GetResellersUponUserGroupAccountManager(User user)
        {
            List<User> resellers;
            var acountManager = _context.Users.Where(a => a.AccountmanagerId == user.ID).ToList();
            if (acountManager.Count == 0)
            {
                switch (user.GroupID)
                {
                    // admin 
                    case 1:
                        resellers = GetResellers(null);
                        return resellers.Select(x => new ResellerPreviewItem
                        {
                            Id = x.ID,
                            UserName = x.UserName
                        });


                    // reseller
                    case 6:
                        resellers = _context.Users.Where(u => u.ID == user.ID).ToList();

                        return resellers.Select(x => new ResellerPreviewItem
                        {
                            Id = x.ID,
                            UserName = x.UserName
                        });

                    //Branch
                    case 4:
                        var templates = new List<ResellerPreviewItem>();
                        var branches = _context.UserBranches.Where(b => b.UserID == user.ID);
                        if (branches.Any())
                        {
                            foreach (var branch in branches)
                            {
                                var current = GetResellersByBranchId(Convert.ToInt32(branch.BranchID));

                                foreach (var reseller in current)
                                {
                                    templates.Add(new ResellerPreviewItem
                                    {
                                        Id = reseller.ID,
                                        UserName = branch.Branch.BranchName + " / " + reseller.UserName
                                    });
                                }
                            }
                            return templates;
                        }
                        resellers = GetResellers(user.ID);
                        return resellers.Select(x => new ResellerPreviewItem
                        {
                            Id = x.ID,
                            UserName = x.UserName
                        });

                    default:
                        resellers = GetResellers(user.ID);
                        return resellers.Select(x => new ResellerPreviewItem
                        {
                            Id = x.ID,
                            UserName = x.UserName
                        });
                }
            }
            resellers = acountManager;
            return resellers.Select(x => new ResellerPreviewItem
            {
                Id = x.ID,
                UserName = x.UserName
            });
        }


        public IEnumerable<ResellerPreviewItem> GetResellersUponUserGroup(int userId){
            return GetResellersUponUserGroup(GetUser(userId));
        }


        public User GetUser(int userId){
            return _context.Users.FirstOrDefault(u => u.ID == userId);
        }


        #endregion


        List<User> GetResellers(int ? id){
            if(id != null){
                var user = _context.Users.FirstOrDefault(x => x.ID == id);
                if(user == null){
                    return null;
                }
                var resellers = _context.Users.Where(g => g.GroupID == 6 && g.BranchID == user.BranchID).ToList();
                return resellers;
            }
            return _context.Users.Where(x => x.GroupID == 6).ToList();
        }


        IEnumerable<User> GetResellersByBranchId(int id){
            {
                var resellers = _context.Users.Where(g => g.GroupID == 6 && g.BranchID == id).ToList();
                return resellers;
            }
        }


        public virtual decimal DeductDemandResellerDiscount(Demand demand){
           /* var resellerDiscount = _entries.GetResellerDiscount(demand.WorkOrder.ResellerID, demand.WorkOrder.ServiceProviderID, demand.WorkOrder.ServicePackageID.Value);
            var net = demand.Amount - (demand.Amount * resellerDiscount);*/
            return 0;
        }


        public void CreateTransaction(decimal total, int resellerId, int userId){
            var transaction = _entries.LastTransaction(null, resellerId, null);
            var doubleTotal = Convert.ToDouble(total);
            if(transaction != null){
                transaction.Total -= doubleTotal;
            } else{
                transaction = new UsersTransaction{
                    CreationDate = DateTime.Now.AddHours(),
                    CreditAmmount = doubleTotal,
                    ResellerID = resellerId,
                    DepitAmmount = 0,
                    IsInvoice = false,
                    Description = "Demands payment",
                    UserId = userId
                };
                _entries.SaveTransaction(transaction);
            }
            _entries.Commit();
        }
    }
}
