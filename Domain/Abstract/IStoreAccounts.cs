using System;
using Db;

namespace NewIspNL.Domain.Abstract
{
    public interface IStoreAccounts
    {
        bool AddInAccount(int? supplierId, int? customerId, decimal total, decimal amount, DateTime date,
            string paymentComment, int userId, string note, ISPDataContext context);
    }

    public class StoreAccounts:IStoreAccounts
    {
        public bool AddInAccount(int? supplierId, int? customerId, decimal total, decimal amount, DateTime date, string paymentComment,
            int userId, string note,ISPDataContext context)
        {
            try
            {
                var newmony = new Account
                {
                    SupplierId = supplierId,
                    CustomerId = customerId,
                    Total = total,
                    Amount = amount,
                    PaymentComment = paymentComment,
                    Paymentdate = date,
                    UserId = userId,
                    Notes = note,
                };
                context.Accounts.InsertOnSubmit(newmony);  
                context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}