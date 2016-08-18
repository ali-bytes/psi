using System;
using System.Configuration;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IWorkOrderStatusServices{
        DateTime ? GetStatusStartDate(int workOrderId, int statusId);
    }

    public class WorkOrderStatusServices : IWorkOrderStatusServices{
        readonly ISPDataContext _context =
            new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


        #region IWorkOrderStatusServices Members


        public DateTime ? GetStatusStartDate(int workOrderId, int statusId){
            var order =
                _context.WorkOrderStatus.Where(r => r.WorkOrderID == workOrderId && r.StatusID == statusId).
                    OrderByDescending(x => x.UpdateDate).FirstOrDefault();
            return order != null ? order.UpdateDate : null;
        }


        #endregion
    }
}
