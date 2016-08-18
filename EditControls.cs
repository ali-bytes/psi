using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using BL.Concrete;
using Db;
using Microsoft.Ajax.Utilities;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Services;
using WebGrease.Css.Extensions;

namespace NewIspNL
{
    /// <summary>
    ///     Summary description for EditControls
    /// </summary>
    public class EditControls
    {
        //static ISPDataContext DataContext = new ISPDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());


        public static void AddSerialField(DataTable table)
        {
            table.Columns.Add(new DataColumn("Serial", typeof(decimal)));
            if (table.Rows.Count > 0)
            {
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    table.Rows[i]["Serial"] = i + 1;
                }
            }
        }

        readonly static ISPDataContext DataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        public static int GetStateWoCount(string url, int userId, int groupId)
        {
            var count = 0;
            if (url.Contains("Reports.aspx"))
            {
                var urlParts = url.Split('=');
                var que = QueryStringSecurity.Decrypt(urlParts[1] + "==");
                int sid = Convert.ToInt32(que);
                var user = DataContext.Users.First(usr => usr.ID == userId);
                if (user == null) return count;
                var first = user.Group.DataLevelID;
                if (first == null) return count;
                int dataLevel = first.Value;

                count = GetPendingActivationCount(userId, dataLevel, count, sid);
            }
            return count;
        }
   

        public static int GetRechargeRequestsCount(int userId,int dataLevel)
        {
            var requests = 0;
            switch (dataLevel)
            {
                case 1: requests = DataContext.RechargeRequests.Count(s => s.IsApproved == null);
                    break;
                case 2:
                    requests =
                        DataContext.RechargeRequests.Count(
                            s => s.IsApproved == null && DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(s.User.BranchID));
                    break;
                case 3:
                    requests = DataContext.RechargeRequests.Count(s => s.IsApproved == null && s.ResellerId == userId);
                    break;
            }
            //var requests = DataContext.RechargeRequests.Count(s => s.IsApproved == null);
            return requests;
        }
        public static int GetRechargeClientRequestsCount(int userId,int dataLevel)
        {
            var requests = 0;
            switch (dataLevel)
            {
                case 1:
                    requests=DataContext.RechargeClientRequests.Count(s => s.IsApproved == null);
                    break;
                case 2:
                    requests =
                        DataContext.RechargeClientRequests.Count(
                            s =>
                                s.IsApproved == null &&
                                DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(s.User.BranchID));
                    break;
                case 3:
                    requests = DataContext.RechargeClientRequests.Count(s => s.IsApproved == null && s.ResellerId == userId);
                    break;
            }
            //var requests = 
            return requests;
        }
        public static int GetRechargeRequestsCountBranch(int userId, int dataLevel)
        {
            var requests = 0;
            switch (dataLevel)
            {
                case 1:
                    requests = DataContext.RechargeRequestBranches.Count(s => s.IsApproved == null);
                    break;
                case 2:
                    requests =
                        DataContext.RechargeRequestBranches.Count(
                            s => s.IsApproved == null && DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(s.BranchId));
                    break;
                /*case 3:
                requests = DataContext.RechargeBranchRequests.Count(s => s.IsApproved == null && s.Branch.);
                break;*/
            }

            return requests;
        }

        public static int GetRechargeBranchRequestsCount(int userId, int dataLevel)
        {
            var requests = 0;
            switch (dataLevel)
            {
                case 1:
                    requests= DataContext.RechargeBranchRequests.Count(s => s.IsApproved == null);
                    break;
                case 2:
                    requests =
                        DataContext.RechargeBranchRequests.Count(
                            s => s.IsApproved == null && DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(s.BranchId));
                    break;
                /*case 3:
                requests = DataContext.RechargeBranchRequests.Count(s => s.IsApproved == null && s.Branch.);
                break;*/
            }
         
            return requests;
        }
        public static int GetResellerPprCount(int userId, int dataLevel)
        {
            int requests = 0;
            switch (dataLevel)
            {
                case 1:
                    requests = DataContext.WorkOrderRequests
                        .Count(r =>
                            r.RequestID == 11 &&
                            r.RSID == 3 &&
                            r.ProcessDate == null &&
                            r.WorkOrder.ResellerID != null);
                    break;
                case 2:
                    requests = DataContext.WorkOrderRequests
                        .Count(r =>
                            r.RequestID == 11 &&
                            r.RSID == 3 &&
                            r.ProcessDate == null &&
                            r.WorkOrder.ResellerID != null &&
                            DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(r.WorkOrder.BranchID));
                    break;
                case 3:
                    requests = DataContext.WorkOrderRequests
                        .Count(r =>
                            r.RequestID == 11 &&
                            r.RSID == 3 &&
                            r.ProcessDate == null &&
                            r.WorkOrder.ResellerID != null && r.WorkOrder.ResellerID == userId);
                    break;
            }
            return requests;

        }


        public static int GetBranchPprCount(int userId, int dataLevelId)
        {
            //using (var datacontex = new ISPDataContext())
            {
                var user = DataContext.Users.FirstOrDefault(a => a.ID == userId);
                int requests = 0;
                switch (dataLevelId)
                {
                    case 1:
                        requests = DataContext.WorkOrderRequests
                            .Count(r => r.RequestID == 11 &&
                                        r.RSID == 3 &&
                                        r.ProcessDate == null &&
                                        (r.WorkOrder.ResellerID == -1 || r.WorkOrder.ResellerID == null) &&
                                        r.WorkOrder.BranchID != null);
                        break;
                    case 2:
                        requests = DataContext.WorkOrderRequests
                            .Count(r => r.RequestID == 11 &&
                                        r.RSID == 3 &&
                                        r.ProcessDate == null && r.WorkOrder!=null &&
                                        (r.WorkOrder.ResellerID == -1 || r.WorkOrder.ResellerID == null) &&
                                        r.WorkOrder.BranchID != null &&
                                        DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(r.WorkOrder.BranchID));
                        break;
                    case 3:
                        requests = DataContext.WorkOrderRequests
                            .Count(r => r.RequestID == 11 &&
                                        r.RSID == 3 &&
                                        r.ProcessDate == null &&r.WorkOrder!=null &&
                                        (r.WorkOrder.ResellerID == -1 || r.WorkOrder.ResellerID == null) &&
                                        r.WorkOrder.BranchID != null && r.WorkOrder.BranchID ==Convert.ToInt32(user.BranchID));
                        break;
                }

                return requests;
            }
        }


        public static int GetHandelResellersTransfersCount(int userId, int dataLevelId)
        {
            //using (var datacontex = new ISPDataContext())
            {
                var user = DataContext.Users.FirstOrDefault(a => a.ID == userId);
                int requests = 0;
                switch (dataLevelId)
                {
                    case 1:
                        requests = DataContext.ResellerTransformationRequests
                            .Count(r => r.Status == null);
                        break;
                    case 2:
                        requests = DataContext.ResellerTransformationRequests
                            .Count(r => r.Status == null&&
                                        DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(r.User.BranchID));
                        break;
                    case 3:
                        requests = DataContext.ResellerTransformationRequests
                            .Count(r => r.Status == null &&
                                        r.User.BranchID == Convert.ToInt32(user.BranchID));
                        break;
                }

                return requests;
            }
        }



        public static int CustomerCount(int userId, int dataLevel)
        {
            var numbers = 0;
            switch (dataLevel)
            {
                case 1:
                    numbers = DataContext.WorkOrders.Count();
                    break;
                case 2:
                    numbers =
                        DataContext.WorkOrders.Count(a => DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(a.BranchID));
                    break;
                case 3:
                    numbers = DataContext.WorkOrders.Count(a => a.ResellerID == userId);
                    break;
            }
            return numbers;
        }

        public static int GetPendingActivationCount(int userId, int dataLevel, int count, int sid, DateTime start = new DateTime(), DateTime end = new DateTime())
        {
            var orders = new List<WorkOrder>();
            var defaultDate = new DateTime(1, 1, 1);
            var acountManager = DataContext.Users.Where(a => a.AccountmanagerId == userId).ToList();
            if (acountManager.Count == 0)
            {
                switch (dataLevel)
                {
                    case 1:
                        orders = DataContext.WorkOrders.Where(wo => wo.WorkOrderStatusID == sid).ToList();
                        break;
                    case 2:
                        orders =
                            DataContext.WorkOrders.Where(
                                wo =>
                                    wo.WorkOrderStatusID == sid &&
                                    (DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(wo.BranchID) ||
                                     wo.User.AccountmanagerId == userId)).ToList();
                        break;
                    case 3:
                        orders =
                            DataContext.WorkOrders.Where(wo => wo.WorkOrderStatusID == sid && wo.ResellerID == userId)
                                .ToList();
                        break;
                }
                if (start.Date == defaultDate.Date && end.Date == defaultDate.Date)
                {
                    return orders.Count;
                }
                orders =
                    orders.Where(
                        x =>
                            x.CreationDate != null && x.CreationDate.Value.Date >= start.Date &&
                            x.CreationDate.Value.Date <= end.Date).ToList();
            }
            else
            {
                orders =
                    DataContext.WorkOrders.Where(
                        a =>
                            a.WorkOrderStatusID == sid && a.User.AccountmanagerId != null &&
                            a.User.AccountmanagerId == userId).ToList();
            }
            return orders.Count;
        }


        public static int GetTicketCount(string url, int userId, int groupId)
        {

            string[] urlParts = url.Split('=');
            var que = QueryStringSecurity.Decrypt(urlParts[1] + "==");
            var ticketStatusId = Convert.ToInt32(que);

            var first = DataContext.Users.Where(usr => usr.ID == userId).Select(usr => usr.Group.DataLevelID).First();
            if (first == null) return 0;
            int dataLevel = first.Value;
            var i = DataContext.Users.Where(usr => usr.ID == userId).Select(usr => usr.BranchID).First();
            if (i == null) return 0;
            int userBranchId = i.Value;
            switch (dataLevel)
            {
                case 1:
                {
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
                    var cmd = new SqlCommand("PROC_GET_TICKETS", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = ticketStatusId;
                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;

                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    connection.Close();
                    return table.Rows.Count;
                }

                case 2:
                {
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
                    var cmd = new SqlCommand("PROC_GET_TICKETS", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = ticketStatusId;
                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = userBranchId;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = userId;

                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    connection.Close();
                    return table.Rows.Count;
                }

                case 3:
                {
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
                    var cmd = new SqlCommand("PROC_GET_TICKETS", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = ticketStatusId;
                    cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = userId;
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;
                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    connection.Close();
                    return table.Rows.Count;
                }
            }
            return 0;
        }

        public static int GetInboxCount(string url, int userId)
        {
            var count = DataContext.Messages.Count(x => x.MessageTo == userId & !Convert.ToBoolean(x.DoneRead));
            return count;
        }

        public static int GetRequestWoCount(string url, int userId, int groupId)
        {
            if (url.Contains("ManageRequests.aspx"))
            {
                string[] urlParts = url.Split('=');
                var decrypted = QueryStringSecurity.Decrypt(urlParts[1] + "==");
                var rid = Convert.ToInt32(decrypted);

                var query = DataLevelClass.GetCountNonCofirmedWoRequests(rid);
                //DataLevelClass.GetUserNonCofirmedWoRequests(rid);
                return query;
            }
            else if (url.Contains("ManageOfferRequest.aspx"))
            {
                var offerquery = DataLevelClass.GetCountNonCofirmedWoRequests(12);
                return offerquery;
            }
            else if (url.Contains("ManageResellerRequests.aspx"))
            {
                return GetNewResellerRequests();
            }
            return 0;
        }


        public static int GetAllRequestsWoCount()
        {
            int count = 0;
            for (int i = 1; i < 10; i++)
            {
                var l = DataLevelClass.GetCountNonCofirmedWoRequests(i);//DataLevelClass.GetUserNonCofirmedWoRequests(i);
                count += l;
            }
            return count;
        }


        public static int GetUnpaidDemandCount(int dataLevel,int userId)
        {
            var demands = new List<Demand>();
            switch (dataLevel)
            {
                case 1:
                    demands = DataContext.Demands.Where(o => o.Paid == false && o.WorkOrder.ResellerID == null).ToList();
                    break;
                case 2:
                    demands = DataContext.Demands.Where(o => o.Paid == false && o.WorkOrder.ResellerID == null && DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(o.WorkOrder.BranchID)).ToList();
                    break;
                case 3:
                    demands = DataContext.Demands.Where(o => o.Paid == false&&o.WorkOrder.ResellerID==userId).ToList();
                    break;
            }

            // var query = DataContext.Demands.Where(a => a.Paid == false).ToList();
            return demands.Count;
        }

        public static int GetNewResellerRequests()
        {
            var count = DataContext.NewResellerRequests.Count(a => a.RequestStatuses == null);
            return count;
        }
        public static int GetOverDaysSuspendedCustomerCount(int dataLevel, int userId)
        {
            if (userId == 0) return 0;
            WorkOrderRepository _orderRepository = new WorkOrderRepository();
            IspEntries _ispEntries = new IspEntries();
            //var userWorkOrders = _orderRepository.GetUserWorkOrders(userId);

            //var suspendedCustomers = userWorkOrders
            //   .Where(o => o.Status.ID == 11 && o.WorkOrderStatus.Any() && o.WorkOrderStatus.Where(x => x.UpdateDate.Value.Date.AddDays()))
            //   .Select(w => _ispEntries.DaysForCustomerAtStatus(w.ID, 11)).ToList();



            //var suspendedCustomers = userWorkOrders
            //    .Where(o => o.Status.ID == 11 && o.WorkOrderStatus.Any())
            //    .Select(w => _ispEntries.DaysForCustomerAtStatus(w.ID, 11)).ToList();
            //            //{
                            //var user = w.User;
                            //var dateTime = w.WorkOrderStatus.Max(s => s.UpdateDate);
                            //if (dateTime != null)
                                //return new
                                //{
                                    //w.ID,
                                    //w.CustomerName,
                                    //w.CustomerPhone,
                                    //w.Governorate.GovernorateName,
                                    //Provider = w.ServiceProvider.SPName,
                                    //Branch = w.Branch.BranchName,
                                    //Resseller = user == null ? "-" : user.UserName,
                                    //Requestdate =
                                    //    dateTime.Value.Date.ToShortDateString(),

                                    //Days = 
                                    //_ispEntries.DaysForCustomerAtStatus(w.ID, 11)
                            //    };
                            //return null;
                        //}
                        //)
                //.ToList();
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var option = OptionsService.GetOptions(context, true);
                if (option != null)
                {
                    //var userWorkOrders = _orderRepository.GetUserWorkOrders(userId);

                    //var suspendedCustomers = context.WorkOrders
                    //   .Where(o => o.Status.ID == 11 && o.WorkOrderStatus.Any()).AsQueryable().ToList();

                   //var total= (context.WorkOrderStatus.Where(x => x.Status.ID == 11)
                   //    .GroupBy(x => x.WorkOrderID).Select(a => a.OrderByDescending(x => x.UpdateDate).First())).Where(x => x.UpdateDate.Value.Date < (DateTime.Now.AddHours().AddDays(-option.SuspendDaysCount))).ToList();

                    var total = (context.WorkOrderStatus.Where(x => x.Status.ID == 11 && x.WorkOrder.WorkOrderStatusID == 11)
                       .GroupBy(x => x.WorkOrderID).Select(a => a.OrderByDescending(x => x.UpdateDate).First())).ToList().Where(x => x.UpdateDate.Value.AddDays(option.SuspendDaysCount) < DateTime.Now.AddHours()).ToList();

                    return total.Count();
                    //var total = context.WorkOrderStatus.Where(x => x.Status.ID == 11).OrderByDescending(x => x.UpdateDate).DistinctBy(d=>d.WorkOrderID).ToList();



                    //&& o.WorkOrderStatus..UpdateDate.Value.Date.AddDays(option.SuspendDaysCount) < DateTime.Now.AddHours())
                    //.Select(w => _ispEntries.DaysForCustomerAtStatus(w.ID, 11)).ToList();



                    //var overRangeCustomers = suspendedCustomers.Where(o => o.Days > option.SuspendDaysCount).ToList();
                    //if (overRangeCustomers.Any(s => s.Days > option.SuspendDaysCount))
                    //{
                    //    return overRangeCustomers.Count(s => s.Days > option.SuspendDaysCount);

                    //}
                }
            }

            return 0;
        }
    }
}
