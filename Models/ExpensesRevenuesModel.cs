using System;
using System.Globalization;
using System.Linq;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Models{
    public class ExpensesRevenuesModel{
        public string Type { get; set; }

        public string Branch { get; set; }

        public decimal Amount { get; set; }

        public string TAmount { get; set; }

        public string Comment { get; set; }

        public string Notes { get; set; }

        public string User { get; set; }

        public string TDate { get; set; }

        public DateTime Date { get; set; }


        public static ExpensesRevenuesModel To(IncomingExpense eOut, ISPDataContext context){
            var amount = eOut.Value == null ? 0 : Convert.ToDecimal(eOut.Value);
            var userName = "";

            if(eOut.UserId != null){
                var user = context.Users.FirstOrDefault(u => u.ID == eOut.UserId);
                if(user != null){
                    userName = user.UserName;
                }
            }

            return new ExpensesRevenuesModel{
                Date = eOut.Date != null ? eOut.Date.Value : new DateTime(),
                TDate = eOut.Date != null ? eOut.Date.Value.ToString(CultureInfo.InvariantCulture) : "-",
                Amount = amount,
                TAmount = Helper.FixNumberFormat(amount),
                Branch = eOut.BranchID == null ? "-" : eOut.Branch.BranchName,
                Comment = eOut.Comment,
                Notes = eOut.Notes,
                User = userName
            };
        }


        public static ExpensesRevenuesModel To(OutgoingExpense eOut, ISPDataContext context){
            var amount = eOut.Value == null ? 0 : Convert.ToDecimal(eOut.Value);
            var userName = "";

            if(eOut.UserId != null){
                var user = context.Users.FirstOrDefault(u => u.ID == eOut.UserId);
                if(user != null){
                    userName = user.UserName;
                }
            }

            return new ExpensesRevenuesModel{
                Date = eOut.Date != null ? eOut.Date.Value : new DateTime(),
                TDate = eOut.Date != null ? eOut.Date.Value.ToString(CultureInfo.InvariantCulture) : "-",
                Amount = amount,
                TAmount = Helper.FixNumberFormat(amount),
                Branch = eOut.BranchID == null ? "-" : eOut.Branch.BranchName,
                Comment = eOut.Comment,
                Notes = eOut.Notes,
                Type = eOut.OutgoingTypeID == null ? "-" : eOut.OutgoingType.Name,
                User = userName
            };
        }
    }
}