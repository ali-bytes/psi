using System;
using System.Configuration;
using System.Linq;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Models;

namespace NewIspNL.Domain{
    public class WOrkOrderConverter : IWOrkOrderConverter{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        readonly IWorkOrderStatusServices _statusServices = new WorkOrderStatusServices();


        #region IWOrkOrderConverter Members


        public WorkOrderTemplate ToPreviewTemplate(WorkOrder order){
            var activationDate = _statusServices.GetStatusStartDate(order.ID, 6);
            var userId = Convert.ToInt32(order.ResellerID);
            var reseller = GetUser(userId);

            return new WorkOrderTemplate{
                Activation = Convert.ToDateTime(activationDate),
                Branch = order.Branch.BranchName,
                Central = order.Central == null ? "-" : order.Central.Name,
                Governate = order.Governorate.GovernorateName,
                Name = order.CustomerName,
                Offer = order.Offer == null ? "-" : order.Offer.Title,
                Package = order.ServicePackage.ServicePackageName,
                Phone = order.CustomerPhone,
                Provider = order.ServiceProvider.SPName,
                Reseller = reseller == null ? "-" : reseller.UserName,
                State = order.Status.StatusName
            };
        }


      

        #endregion


        User GetUser(int userId){
            return _context.Users.FirstOrDefault(r => r.ID == userId);
        }
    }
}
