using System;
using System.Linq;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Models{
    public class PaymentTemplate{
        public string TAmount { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string TDate { get; set; }

        public int ? ResellerId { get; set; }

        public string Reseller { get; set; }

        public int ? BranchId { get; set; }

        public string Branch { get; set; }

        public string User { get; set; }


        public static PaymentTemplate To(UsersTransaction t, ISPDataContext context){
            var amount = Convert.ToDecimal(t.CreditAmmount != null ? t.CreditAmmount.Value : 0);
            var dateTime = t.CreationDate == null ? new DateTime() : t.CreationDate.Value;
            var template = new PaymentTemplate{
                Amount = amount,
                TAmount = Helper.FixNumberFormat(amount),
                TDate = dateTime.Equals(new DateTime()) ? "" : dateTime.ToString(),
                Date = dateTime,
                User = t.User1 == null ? "-" : t.User1.UserName,
            };
            if(t.ResellerID != null && t.ResellerID > 0){
                var user = context.Users.FirstOrDefault(x => x.ID == t.ResellerID.Value);
                if(user != null){
                    template.ResellerId = user.ID;
                    template.Reseller = user.UserName;
                }
            }

            if(t.Branch != null){
                template.BranchId = t.Branch.ID;
                template.Branch = t.Branch.BranchName;
            }
            return template;
        }
    }
}