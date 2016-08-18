using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerStatistics : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(Session["User_ID"]);
            if (id<=0)
            {
                return;
            }
            List<StatisticList> statisticList = new List<StatisticList>();
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var check = context.Users.FirstOrDefault(wo => wo.ID == id);
                if (check != null && check.GroupID!=1)
                {
                    return;
                }
                var resellers = context.Users.Where(wo => wo.GroupID == 6).ToList();
                var status = context.Status.ToList();
                foreach (var res in resellers)
                {
                    StatisticList statistic = new StatisticList();
                    statistic.ResellerName = res.UserName;
                    foreach (var state in status)
                    {
                        var orders = context.WorkOrders.Where(wo => wo.WorkOrderStatusID == state.ID && wo.ResellerID == res.ID).ToList();
                        if (state.ID==1)
                        {
                            statistic.NewCustomer = orders.Count;
                        }
                        if (state.ID == 2)
                        {
                            statistic.PendingTe = orders.Count;
                        }
                        if (state.ID == 3)
                        {
                            statistic.PendingWo = orders.Count;
                        }
                        if (state.ID == 4)
                        {
                            statistic.PendingSplitting = orders.Count;
                        }
                        if (state.ID == 5)
                        {
                            statistic.PendingInstallation = orders.Count;
                        }
                        if (state.ID == 6)
                        {
                            statistic.Active = orders.Count;
                        }
                        if (state.ID == 7)
                        {
                            statistic.SystemProblem = orders.Count;
                        }
                        if (state.ID == 8)
                        {
                            statistic.CancellationProcess = orders.Count;
                        }
                        if (state.ID == 9)
                        {
                            statistic.Cancelled = orders.Count;
                        }
                        if (state.ID == 10)
                        {
                            statistic.Hold = orders.Count;
                        }
                        if (state.ID == 11)
                        {
                            statistic.Suspend = orders.Count;
                        }
                        if (state.ID == 12)
                        {
                            statistic.AutoSuspend = orders.Count;
                        }
                    }
                    statisticList.Add(statistic);
                }
              
            }

            grd_Statistics.DataSource = statisticList.ToList();
            grd_Statistics.DataBind();
        }
        protected void GvBox_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(grd_Statistics, "LNo");
        }
    }
    public class StatisticList
    {
        public string ResellerName { get; set; }
        public int? NewCustomer { get; set; }
        public int? PendingTe { get; set; }
        public int? PendingWo { get; set; }
        public int? PendingSplitting { get; set; }
        public int? PendingInstallation { get; set; }
        public int? Active { get; set; }
        public int? SystemProblem { get; set; }
        public int? CancellationProcess { get; set; }
        public int? Cancelled { get; set; }
        public int? Hold { get; set; }
        public int? Suspend { get; set; }
        public int? AutoSuspend { get; set; }

    }
}