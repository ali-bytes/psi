using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.BL.Abstract;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using NewIspNL.Services.DemandServices;


namespace BL.Concrete{
   public class InvoiceService : IInvoiceService{
        readonly ISPDataContext _context;
        private readonly IUserSaveRepository _userSave;

       // readonly IUserTransactionsService _transactionsService;

       readonly DemandFactory _demandFactory;

       readonly IspEntries _entries;

       public InvoiceService(){
           _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
           //_transactionsService = new UserTransactionsService();
           _entries=new IspEntries();
           _demandFactory = new DemandFactory(_entries);
           _userSave = new UserSaveRepository();
       }


       public List<string> SaveDemands(List<Invoice> invoices, int userId){
           var errors = new List<string>();
           var demands = new List<Demand>();
           foreach(var invoice in invoices){
               var order = _entries.GetWorkOrder(invoice.Phone);
               
               if(order == null){
                  errors.Add(invoice.Phone);
                   continue;
               }

               var demand = _demandFactory.CreateDemand(order, invoice.StartAt, invoice.EndAt,
                   Convert.ToDecimal(invoice.Amount), userId, false, false, invoice.Note,"",invoice.ResellerCommission);
               demands.Add(demand);
              
           }
           _entries.AddDemands(demands);
          _entries.Commit();
           return errors;

       }


      


       public List<string> CreateManaulInvoices(List<Invoice> invoices, int userId,int saveId){
            var errors = new List<string>();
            var idAmounts = new List<IdAmount>();
            foreach(var invoice in invoices){
                var workOrder = _context.WorkOrders.FirstOrDefault(w => w.CustomerPhone == invoice.Phone);
                if(workOrder == null){
                    continue;
                }
                var idAmount = new IdAmount{
                    Id = workOrder.ID,
                    Amount = invoice.Amount,
                    StartAt = invoice.StartAt.Date,
                    EndAt = invoice.EndAt.Date
                };
                idAmounts.Add(idAmount);
            }

            /*var ids=invoices.Select(x => new{
                Id=_context.WorkOrders.FirstOrDefault(w => w.CustomerPhone == x.Phone) == null 
                ? -1 
                : _context.WorkOrders.FirstOrDefault(w => w.CustomerPhone == x.Phone).ID,
                x.Amount
            });*/

            invoices.Where(i => _context.WorkOrders.FirstOrDefault(w => w.CustomerPhone == i.Phone) == null).ToList().ForEach(x => errors.Add(x.Phone));
            foreach(var id in idAmounts){
                if(id.Id == -1){
                    continue;
                }
                var time = DateTime.Now.AddHours();
                var id1 = id;
                var decimalamount = Convert.ToDecimal(id1.Amount);
                var startdate = id1.StartAt.ToShortDateString();
                var enddate = id1.EndAt.ToShortDateString();
                var demand =
                    _context.Demands.FirstOrDefault(
                        a =>
                            a.WorkOrderId == id1.Id && a.Amount == decimalamount && (a.Paid == false) &&
                            a.StartAt.Date == Convert.ToDateTime(startdate).Date && a.EndAt.Date == Convert.ToDateTime(enddate).Date);
                //
                if(demand==null)continue;
                demand.Paid = true;
                demand.PaymentDate = time;
                demand.PaymentComment = "سداد المطالبة من صفحة دفع يدوى";
                demand.UserId = userId;
                _context.SubmitChanges();
                var notes2 =demand.WorkOrder.CustomerName + " - " +
                                demand.WorkOrder.CustomerPhone + " - " + " فاتورة شهر  " + "(" + demand.StartAt.Month +
                                " - " + demand.StartAt.Year + ")";
                using (var context7 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(id1.Amount),
                        "دفع مطالبة من صفحة دفع يدوى",
                        notes2, context7);
                }
                /*var userTransaction = new UsersTransaction{
                    CreationDate = time,
                    DepitAmmount = 0,
                    CreditAmmount = id.Amount,
                    IsInvoice = false,
                    WorkOrderID = id.Id,
                    Total =
                        Billing.GetLastBalance(id.Id, "WorkOrder") - id.Amount,
                    UserId = userId,
                    Notes = "",
                    Description = "payment"
                };
                _context.UsersTransactions.InsertOnSubmit(userTransaction);
                _context.SubmitChanges();*/
            }
            return errors;
        }


        #region Nested type: IdAmount


        class IdAmount{
            public int Id { get; set; }
            public double Amount { get; set; }
            public DateTime StartAt { get; set; }
            public DateTime EndAt { get; set; }
        }


        #endregion
    }
}
