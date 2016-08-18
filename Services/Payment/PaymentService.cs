using System;
using System.Collections.Generic;
using System.Linq;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Services.Payment{
    public class PaymentData{
        public decimal TAmount { get; set; }
        public string Reseller { get; set; }
        public string User { get; set; }
        public DateTime TDate { get; set; }
    }

    public class PaymentService{
        readonly ISPDataContext _context;


        public PaymentService(ISPDataContext context){
            _context = context;
        }


        public List<UsersTransaction> ResellersPayments(DateTime date,int userId=0){
            if (userId != 1)
            {
                var user = _context.Users.FirstOrDefault(u => u.ID == userId);
                if (user == null) return null;
                var userBranches = UserServices.GetUserBranches(userId, _context);
                var transactions = new List<UsersTransaction>();
                foreach (var branch in userBranches){
                    transactions.AddRange(ResellersPaymentsForBranch(new BasicSearchModel
                    {
                        Date = date,
                        BranchId = branch.ID
                    }));
                }
                return transactions;
            }
            return _context.UsersTransactions
                .Where(x => x.ResellerID != null && x.ResellerID > 0 && x.CreationDate != null).ToList()
                .Where(x => x.CreationDate != null && x.CreationDate.Value.Date == date.Date && x.Description == "payment").ToList();
        }

        // modified by ashraf to publish
        public List<PaymentData> ResellerCredits(DateTime date, int userid){
            var user = _context.Users.FirstOrDefault(a => a.ID == userid);
            if(user == null) return null;
            var credit = _context.ResellerCredits.Where(a => a.Time.Date == date.Date && a.Amount > 0).Select(x=>new PaymentData{
               TAmount= x.Amount,
                Reseller=x.User.UserName,
               User=x.User1.UserName,
               TDate=x.Time
            }).ToList();
            return credit;
        }
        //mdified by Ahmed Saied
       // مدفوعات موزع لطلبات السداد فى صفحة التقرير الشهرى
        public List<PaymentData> ResellerCredits(DateTime startAt, DateTime endAt)
        {
            //var user = _context.Users.FirstOrDefault(a => a.ID == userid);
            //if (user == null) return null;
            var credit = _context.ResellerCredits.Where(a =>a.Time!=null && a.Time.Date >= startAt.Date &&a.Time.Date<=endAt.Date && a.Amount > 0).Select(x => new PaymentData
            {
                TAmount = x.Amount,
                Reseller = x.User.UserName,
                User = x.User1.UserName,
                TDate = x.Time
            }).ToList();
            return credit;
        }
        //مدفوعات موزع لطلبات السداد فى التقرير اليومى
        public List<PaymentData> ResellerCredits(BasicSearchModel model)
        {
            if (model.BranchId == null)
            {
                return null;
            }
            var credit = _context.ResellerCredits.Where(a => a.Time.Date == model.Date && a.Amount > 0 &&a.User1.BranchID==model.BranchId).Select(x => new PaymentData
            {
                TAmount = x.Amount,
                Reseller = x.User.UserName,
                User = x.User1.UserName,
                TDate = x.Time
            }).ToList();
            return credit;
        }
       
        // modified by ashraf to publish
        //مدفوعات فرع لطلبات السداد فى التقرير اليومى 
        public List<PaymentData> BranchCredits(DateTime date, int userid)
        {
            var user = _context.Users.FirstOrDefault(a => a.ID == userid);
            if (user == null) return null;
            var credit = _context.BranchCredits.Where(a => a.Time.Date == date && a.Amount > 0).Select(x => new PaymentData
            {
                TAmount = x.Amount,
                Reseller = x.Branch.BranchName,
                User = x.User.UserName,
                TDate = x.Time
            }).ToList();
            return credit;
        }
        //مدفوعات فرع لطلبات السداد فى صفحة التقرير الشهرى
        public List<PaymentData> BranchCredits(DateTime startAt, DateTime endAt)
        {
            //var user = _context.Users.FirstOrDefault(a => a.ID == userid);
            //if (user == null) return null;
            var credit = _context.BranchCredits.Where(a =>a.Time!=null&& a.Time.Date >= startAt.Date && a.Time.Date<=endAt.Date && a.Amount > 0).Select(x => new PaymentData
            {
                TAmount = x.Amount,
                Reseller = x.Branch.BranchName,
                User = x.User.UserName,
                TDate = x.Time
            }).ToList();
            return credit;
        }
        //مدفوعات فرع لطلبات السداد فى تقرير يومى للفرع
        public List<PaymentData> BranchCredits(BasicSearchModel model)
        {
            if (model.BranchId == null)
            {
                return null;
            }
            var credit = _context.BranchCredits.Where(a => a.Time.Date == model.Date && a.Amount > 0 &&a.BranchId==model.BranchId).Select(x => new PaymentData
            {
                TAmount = x.Amount,
                Reseller = x.Branch.BranchName,
                User = x.User.UserName,
                TDate = x.Time
            }).ToList();
            return credit;
        }
       
        
        public List<UsersTransaction> ResellersPaymentsForBranch(BasicSearchModel model){

            if(model.BranchId==null){
                return null;
            }
            return _context.UsersTransactions
                .Where(x => x.User.BranchID == model.BranchId && x.ResellerID != null && x.ResellerID > 0 && x.CreationDate != null).ToList()
                .Where(x => x.CreationDate != null && x.CreationDate.Value.Date == model.Date && x.Description == "payment").ToList();
        }


        public List<UsersTransaction> ResellersPayments(DateTime startAt, DateTime endAt){
            return _context.UsersTransactions
                .Where(x => x.ResellerID != null && x.ResellerID > 0 && x.CreationDate != null).ToList()
                .Where(x => x.CreationDate != null && x.CreationDate.Value.Date >= startAt.Date && x.CreationDate.Value.Date <= endAt.Date && x.Description == "payment").ToList();
        }


        public List<PaymentTemplate> ResellersPaymentsTemplates(DateTime date,int userId){
            return ResellersPayments(date,userId).Select(x => PaymentTemplate.To(x, _context)).ToList();
        }
        public List<PaymentTemplate> ResellersPaymentsForBranchTemplates(BasicSearchModel model){
            return ResellersPaymentsForBranch(model).Select(x => PaymentTemplate.To(x, _context)).ToList();
        }


        public List<PaymentTemplate> ResellersPaymentsTemplates(DateTime startAt, DateTime endAt){
            return ResellersPayments(startAt, endAt).Select(x => PaymentTemplate.To(x, _context)).ToList();
        }


        public List<UsersTransaction> BranchesPayments(DateTime date,int userId=0){
            if (userId != 0){
                var user = _context.Users.FirstOrDefault(u => u.ID == userId);
                if (user == null) return null;
                var userBranches = UserServices.GetUserBranches(userId, _context);
                var demands = new List<UsersTransaction>();
                foreach (var branch in userBranches){
                    demands.AddRange(BranchesPaymentsForBranch(new BasicSearchModel{
                        Date = date,
                        BranchId = branch.ID
                    }));
                }
                return demands;
            }

            return _context.UsersTransactions
                .Where(x => x.BranchID != null && x.BranchID > 0 && x.CreationDate != null).ToList()
                .Where(x => x.CreationDate != null && x.CreationDate.Value.Date == date.Date && x.Description == "payment").ToList();
        }
        
        public List<UsersTransaction> BranchesPaymentsForBranch(BasicSearchModel model){
            if(model.BranchId == null) return null;
            var payments = BranchesPayments(model.Date);
            var forBranch = payments.Where(x => x.BranchID != null && x.BranchID == model.BranchId).ToList();
            return forBranch;
        }
        
        public List<UsersTransaction> BranchesPayments(DateTime startAt, DateTime endAt){
            return 
                _context.UsersTransactions
                .Where(x => x.BranchID != null && x.BranchID > 0 && x.CreationDate != null)
                .ToList()
                .Where(x => x.CreationDate != null && x.CreationDate.Value.Date >= startAt.Date && x.CreationDate.Value.Date <= endAt.Date && x.Description == "payment")
                .ToList();
        }
        
        public List<PaymentTemplate> BranchesPaymentsTemplates(DateTime date,int userId=0){
            return BranchesPayments(date).Select(x => PaymentTemplate.To(x, _context)).ToList();
        }
        public List<PaymentTemplate> BranchesPaymentsForBranchTemplates(BasicSearchModel model){
            return BranchesPaymentsForBranch(model).Select(x => PaymentTemplate.To(x, _context)).ToList();
        }


        public List<PaymentTemplate> BranchesPaymentsTemplates(DateTime startAt, DateTime endAt){
            return BranchesPayments(startAt, endAt).Select(x => PaymentTemplate.To(x, _context)).ToList();
        }
    }
}
