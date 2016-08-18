using System;
using System.Collections.Generic;
using System.Linq;
using BL.Concrete;
using Db;
using NewIspNL.Models;

namespace NewIspNL.Domain.Abstract
{
    public interface IRequestNotifiy
    {
        bool AddNotification(int orderId, int requestId, bool status, DateTime date, int userId,ISPDataContext context);
        bool AddNotification(int orderId, int requestId, bool status, DateTime date, int userId, ISPDataContext context, string extraName);

        bool UpdateNotification(int id,bool status,ISPDataContext context);
        List<NotificationRequests> GetNotificationRequestses(bool status, ISPDataContext context);
        List<NotificationRequests> GetNotificationRequestses(bool status,int requestId, ISPDataContext context);
    }
    public class RequestNotifiy:IRequestNotifiy
    {
        //add Request Notification from Customer Demand Only
        public bool AddNotification(int orderId, int requestId, bool status, DateTime date, int userId, ISPDataContext context)
        {
            var notification = new RequestsNotitfication
            {
                WorkOrderId = orderId,
                RequestId = requestId,
                Status = status,
                ProccessDate = date,
                UserId = userId
                
                
            };
            try
            {
                context.RequestsNotitfications.InsertOnSubmit(notification);
                context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddNotification(int orderId, int requestId, bool status, DateTime date, int userId, ISPDataContext context,string extraName)
        {
            var notification = new RequestsNotitfication
            {
                WorkOrderId = orderId,
                RequestId = requestId,
                Status = status,
                ProccessDate = date,
                UserId = userId,
                Notes = extraName

            };
            try
            {
                context.RequestsNotitfications.InsertOnSubmit(notification);
                context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateNotification(int id, bool status, ISPDataContext context)
        {
            var data = context.RequestsNotitfications.FirstOrDefault(a => a.Id == id);
            if (data != null)
            {
                data.Status = status;
                context.SubmitChanges();
                return true;
            }
            return false;
        }

        public List<NotificationRequests> GetNotificationRequestses(bool status, ISPDataContext context)
        {
             var ispEntries=new IspEntries();
            var requests =
                context.RequestsNotitfications.Where(a => a.Status == status).Select(a => new NotificationRequests
                {
                    
                    CustomerName = a.WorkOrder.CustomerName,
                    CustomerPhone = a.WorkOrder.CustomerPhone,
                    GovernorateName = a.WorkOrder.Governorate.GovernorateName,
                    Central = a.WorkOrder.Central.Name,
                    CurrentServicePackageName = a.WorkOrder.ServicePackage.ServicePackageName,
                    StatusName = a.WorkOrder.Status.StatusName,
                    SuspenDaysCount =
                        a.WorkOrder.WorkOrderStatusID == 11
                            ? ispEntries.DaysForCustomerAtStatus(Convert.ToInt32(a.WorkOrderId), 11)
                            : 0,
                    SPName = a.WorkOrder.ServiceProvider.SPName,
                    UserName = a.WorkOrder.User == null ? "" : a.WorkOrder.User.UserName,
                    SenderName = a.User.UserName,
                    BranchName = a.WorkOrder.Branch.BranchName,
                    WorkOrderID = a.WorkOrderId,
                    Title = a.WorkOrder.Offer.Title,
                    IpPackageName = a.WorkOrder.IpPackage.IpPackageName,
                   //ExtraGiga = a.WorkOrder.ExtraGiga == null ? "-" : a.WorkOrder.ExtraGiga.Name,
                    PaymentType = a.WorkOrder.PaymentType.PaymentTypeName,
                    

                    ProcessDate = Convert.ToDateTime(a.ProccessDate),
                    Status =Convert.ToBoolean(a.Status),
                    ProccessName = a.Request.RequestName,
                    ProccessId = a.RequestId,
                    RequestNotifiId = a.Id,
                    Notes = a.Notes
                    
                }).ToList();
            foreach (var resultItem in requests)
            {
                var item = resultItem;
                var order = context.WorkOrderStatus.Where(r => r.WorkOrderID == item.WorkOrderID && r.StatusID == 6).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                if (order != null)
                {
                    resultItem.ActivationDate = Convert.ToDateTime(order.UpdateDate);
                }

                //var o = context.WorkOrderRequests.FirstOrDefault(a => a.WorkOrderID == resultItem.WorkOrderID);
                //var ex = context.ExtraGigas.Where(a => a.Id == Convert.ToInt32(o)).Select(x=>x.Name).FirstOrDefault();
                //if (o!=null)
                //{
                //    resultItem.ExtraGiga = o.ExtraGiga.Name;
                //}
               
            }

            return requests;
        }

        public List<NotificationRequests> GetNotificationRequestses(bool status, int requestId, ISPDataContext context)
        {
            var ispEntries = new IspEntries();
            var requests =
                context.RequestsNotitfications.Where(a => a.Status == status&&a.RequestId==requestId).Select(a => new NotificationRequests
                {

                    CustomerName = a.WorkOrder.CustomerName,
                    CustomerPhone = a.WorkOrder.CustomerPhone,
                    GovernorateName = a.WorkOrder.Governorate.GovernorateName,
                    Central = a.WorkOrder.Central.Name,
                    CurrentServicePackageName = a.WorkOrder.ServicePackage.ServicePackageName,
                    StatusName = a.WorkOrder.Status.StatusName,
                    SuspenDaysCount =
                        a.WorkOrder.WorkOrderStatusID == 11
                            ? ispEntries.DaysForCustomerAtStatus(Convert.ToInt32(a.WorkOrderId), 11)
                            : 0,
                    SPName = a.WorkOrder.ServiceProvider.SPName,
                    UserName = a.WorkOrder.User == null ? "" : a.WorkOrder.User.UserName,
                    SenderName = a.User.UserName,
                    BranchName = a.WorkOrder.Branch.BranchName,
                    WorkOrderID = a.WorkOrderId,
                    Title = a.WorkOrder.Offer.Title,
                    IpPackageName = a.WorkOrder.IpPackage.IpPackageName,
                    ExtraGiga = a.WorkOrder.ExtraGiga.Name,
                    PaymentType = a.WorkOrder.PaymentType.PaymentTypeName,


                    ProcessDate = Convert.ToDateTime(a.ProccessDate),
                    Status = Convert.ToBoolean(a.Status),
                    ProccessName = a.Request.RequestName,
                    ProccessId = a.RequestId,
                    RequestNotifiId = a.Id,
                     Notes = a.Notes
                }).ToList();
            return requests;
        }
    }

    public class NotificationRequests : ManageRequestTemplate
    {
        public int RequestNotifiId { set; get; }
        public bool Status { set; get; }
        public DateTime ProcessDate { set; get; }
        public string ProccessName { set; get; }
        public int ProccessId { set; get; }
        public string Notes { set; get; }
    }
}