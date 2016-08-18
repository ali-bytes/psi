using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Services.ExpensesRevenues{
    public class ExpensesRevenuesSrevice{
        readonly ISPDataContext _context;


        public ExpensesRevenuesSrevice(ISPDataContext context){
            _context = context;
        }


        public List<OutgoingExpense> Expenses(DateTime time,int userId=0){
            if (userId != 0)
            {
                var user = _context.Users.FirstOrDefault(u => u.ID == userId);
                if (user == null) return null;
                var userBranches = UserServices.GetUserBranches(userId, _context);
                var demands = new List<OutgoingExpense>();
                foreach (var branch in userBranches){
                    demands.AddRange(ExpensesForBranch(new BasicSearchModel{
                        Date = time,
                        BranchId = branch.ID
                    }));
                }
                return demands;
            }
            return _context.OutgoingExpenses.Where(x => x.Date != null).ToList().Where(x => x.Date != null && x.Date.Value.Date.Equals(time.Date)).ToList();
        }
        public List<OutgoingExpense> ExpensesForBranch(BasicSearchModel model){
            if(model.BranchId==null){
                return null;
            }
            return Expenses(model.Date).Where(x => x.BranchID == model.BranchId).ToList();
        }


        public List<OutgoingExpense> Expenses(DateTime startAt, DateTime endAt){
            return _context.OutgoingExpenses.Where(x => x.Date != null)
                .ToList()
                .Where(x => x.Date != null && x.Date.Value.Date >= startAt.Date && x.Date.Value.Date <= endAt.Date).ToList();
        }


        public List<IncomingExpense> Revenues(DateTime time,int userId=0){
            if (userId != 0){
                var user = _context.Users.FirstOrDefault(u => u.ID == userId);
                if (user == null) return null;
                var userBranches = UserServices.GetUserBranches(userId, _context);
                var demands = new List<IncomingExpense>();
                foreach (var branch in userBranches){
                    demands.AddRange(RevenuesForBranch(new BasicSearchModel{
                        Date = time,
                        BranchId = branch.ID
                    }));
                }
                return demands;
            }
            return _context.IncomingExpenses.Where(x => x.Date != null).ToList().Where(x => x.Date != null && x.Date.Value.Date.Equals(time.Date)).ToList();
        }
        public List<IncomingExpense> RevenuesForBranch(BasicSearchModel searchModel){
            return Revenues(searchModel.Date).Where(x => x.BranchID == searchModel.BranchId).ToList();
        }


        public List<IncomingExpense> Revenues(DateTime startAt, DateTime endAt){
            return _context.IncomingExpenses.Where(x => x.Date != null).ToList()
                .Where(x => x.Date != null && x.Date.Value.Date >= startAt.Date && x.Date.Value.Date <= endAt.Date).ToList();
        }


        public List<ExpensesRevenuesModel> ExpensesModels(DateTime time,int userId=0){
            return Expenses(time).Select(x => ExpensesRevenuesModel.To(x, _context)).ToList();
        }
        public List<ExpensesRevenuesModel> ExpensesForBranchModels(BasicSearchModel model){
            return ExpensesForBranch(model).Select(x => ExpensesRevenuesModel.To(x, _context)).ToList();
        }
       
       


        public List<ExpensesRevenuesModel> ExpensesModels(DateTime startAt, DateTime endAt){
            return Expenses(startAt, endAt).Select(x => ExpensesRevenuesModel.To(x, _context)).ToList();
        }


        public List<ExpensesRevenuesModel> RevenuesModels(DateTime time,int userId=0){
            return Revenues(time,userId).Select(x => ExpensesRevenuesModel.To(x, _context)).ToList();
        }
        public List<ExpensesRevenuesModel> RevenuesModelsForBranch(BasicSearchModel model){
            return RevenuesForBranch(model).Select(x => ExpensesRevenuesModel.To(x, _context)).ToList();
        }


        public List<ExpensesRevenuesModel> RevenuesModels(DateTime startAt, DateTime endAt){
            return Revenues(startAt, endAt).Select(x => ExpensesRevenuesModel.To(x, _context)).ToList();
        }
    }
}
