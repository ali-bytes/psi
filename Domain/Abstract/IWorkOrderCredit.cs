using System;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract
{
    /// <summary>
    /// Summary description for IWorkOrderCredit
    /// </summary>

    public interface IWorkOrderCredit
    {
        decimal GetLastCredit(int orderId, ISPDataContext context);
        global::Db.WorkOrderCredit GetCredit(int creditId, ISPDataContext context);
        global::Db.WorkOrderCredit GetLastRow(int orderId, ISPDataContext context);
        void AddCredit(int confirmUserId, int orderId, decimal amount, string note, DateTime date, ISPDataContext context);
    }
    public class WorkOrderCredit:IWorkOrderCredit
    {

        public decimal GetLastCredit(int orderId, ISPDataContext context)
        {
            var workOrderCredit = context.WorkOrderCredits.Where(a => a.WorkOrderId == orderId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();
            decimal lastCredit=0;
            if (workOrderCredit != null)
            {
                 lastCredit =
                    workOrderCredit.CreditAmount;
            }
            return lastCredit;
        }

        public global::Db.WorkOrderCredit GetLastRow(int orderId, ISPDataContext context)
        {
            var cred =
                context.WorkOrderCredits.Where(a => a.WorkOrderId == orderId)
                    .OrderByDescending(a => a.Id)
                    .FirstOrDefault();
            return cred;
        }

        public global::Db.WorkOrderCredit GetCredit(int creditId, ISPDataContext context)
        {
            var credit = context.WorkOrderCredits.FirstOrDefault(a => a.Id == creditId);
            return credit;
        }

        public void AddCredit(int confirmUserId, int orderId, decimal amount, string note, DateTime date, ISPDataContext context)
        {
            //var lastCredit = GetLastCredit(orderId, context);
            var newCredit = new global::Db.WorkOrderCredit
            {
                UserId = confirmUserId,
                WorkOrderId = orderId,
                Notes = note,
                Time = date,
                CreditAmount =amount//(amount<0 && lastCredit>(amount*-1))?  lastCredit+amount:amount+lastCredit
            };
            context.WorkOrderCredits.InsertOnSubmit(newCredit);
            context.SubmitChanges();
        }
    }
}