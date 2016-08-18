using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class InstalledCustomersReport : CustomPage
    {
     
            /*readonly ISPDataContext _context;
            public Pages_InstalledCustomersReport(){
                _context = IspDataContext;
            }*/
            protected void Page_Load(object sender, EventArgs e)
            {
                Activate();
                if (IsPostBack) return;
                Helper.AssignFirstAndLastDateOfMonth(TbStartAt, TbTo, DateTime.Now.AddHours());
                /* var now = DateTime.Now.AddHours();
                 var first = new DateTime(now.Year, now.Month, 1);
                 var end = first.AddMonths(1).AddDays(-1);
                 TbStartAt.Text = first.ToShortDateString();
                 TbTo.Text = end.ToShortDateString();*/
            }

            void Search()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var startAt = Convert.ToDateTime(TbStartAt.Text);
                    var endAt = Convert.ToDateTime(TbTo.Text);
                    var orders = DataLevelClass.GetUserWorkOrder(context).Where(x => x.Installed != null && x.Installed.Value).ToList().Where(x => x.InstallationTime != null && x.InstallationTime.Value >= startAt.Date && x.InstallationTime.Value <= endAt.Date).ToList().Select(x => WorkOrderRepository.GetOrderBasicData(x, context)).ToList();
                    GvResults.DataSource = orders;
                    GvResults.DataBind();
                }
            }
            void Activate()
            {
                bSearch.ServerClick += (o, e) => Search();
                GvResults.DataBound += (o, e) => Helper.GridViewNumbering(GvResults, "LNo");
            }
        }
    }
 