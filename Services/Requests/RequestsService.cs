using System;
using System.Linq;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Services.Requests{
    public class RequestsService{
        readonly ISPDataContext _context;


        public RequestsService(ISPDataContext context){
            _context = context;
        }


        public WorkOrder GetOrder(int orderId){
            return _context.WorkOrders.FirstOrDefault(x => x.ID == orderId);
        }


        public bool SuspendByOrderId(int orderId, int userId, int suspendRequestId = 2, int suspendStatusId = 11,
                                        DateTime requestDate = default(DateTime)){
            var order = GetOrder(orderId);
            if(order == null) return false;
            if(requestDate.Equals(default(DateTime))) requestDate = DateTime.Now.AddHours();
            var servicePackageId = order.ServicePackageID;
            var wor = CreateSuspendRequest(orderId, userId, suspendRequestId, requestDate, servicePackageId, order);
            _context.WorkOrderRequests.InsertOnSubmit(wor);
            Commit();
            var wos = CreateSuspendStatus(userId, suspendStatusId, requestDate, order);
            _context.WorkOrderStatus.InsertOnSubmit(wos);
            order.WorkOrderStatusID = suspendStatusId;
            Commit();
            return true;
        }



        public WorkOrderStatus CreateSuspendStatus(int userId, int suspendStatusId, DateTime requestDate,
                                                    WorkOrder order){
            return new WorkOrderStatus{
                WorkOrder = order,
                StatusID = suspendStatusId,
                UserID = userId,
                UpdateDate = requestDate
            };
        }


        public WorkOrderRequest CreateSuspendRequest(int orderId, int userId, int suspendRequestId, DateTime requestDate, int ? servicePackageId, WorkOrder order){
            return new WorkOrderRequest{
                WorkOrderID = orderId,
                CurrentPackageID = servicePackageId,
                NewPackageID = order.IpPackageID,
                NewIpPackageID = servicePackageId,
                RequestDate = requestDate,
                RequestID = suspendRequestId,
                RSID = 1,
                SenderID = userId,
                ConfirmerID = userId,
                ProcessDate = requestDate,
            };
        }


        public void Commit(){
            _context.SubmitChanges();
        }
    }
}
