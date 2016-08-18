using System;
using System.Linq;
using Db;

namespace NewIspNL.Domain.Abstract{
    public interface IWorkOrderRepository{
        IQueryable<WorkOrder> WorkOrders { get; }


        WorkOrder Get(int id);


        //IQueryable<WorkOrder> WorkOrdersByCentralId(int centralId);


        DateTime ? GetActivationDate(int workOrderId, int statusId = 6);


        DateTime ? GetActivationDate(int workOrderId);


        //string GetCentral(WorkOrder order);


        bool Delete(int orderId);
    }
}
