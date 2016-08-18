using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class CustomersInStatus : CustomPage
    {
        
            readonly IspDomian _ispDomian;
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateRequests();
                var userId = Convert.ToInt32(Session["User_ID"]);
                //_ispDomian.PopulateResellers(ddl_Reseller);
                _ispDomian.PopulateProviders(ddl_Provider);
                _ispDomian.PopulateResellersOfUser(userId, ddl_Reseller);
            }

            public  CustomersInStatus()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _ispDomian = new IspDomian(context);
            }

            void PopulateRequests()
            {
                using (var context =
                    new ISPDataContext(
                        ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture)))
                {


                    var requestTypes = context.Status.Where(r => r.ID == 6 || r.ID == 7 || r.ID == 1).ToList();

                    ddl_requestsTypes.DataTextField = "StatusName";
                    ddl_requestsTypes.DataValueField = "ID";
                    ddl_requestsTypes.DataSource = requestTypes;

                    ddl_requestsTypes.DataBind();
                    Helper.AddDefaultItem(ddl_requestsTypes);
                    //AddDefaultItem(ddl_requestsTypes);
                }
            }
            protected void b_search_Click(object sender, EventArgs e)
            {
                using (var context =
                    new ISPDataContext(
                        ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture)))
                {
                    var statusid = Convert.ToInt32(ddl_requestsTypes.SelectedItem.Value);
                    List<global::Db.WorkOrderStatus> query = new List<global::Db.WorkOrderStatus>();
                    switch (statusid)
                    {
                        case 1:
                            query = context.WorkOrderStatus.Where(r =>
                                r.StatusID == statusid &&
                                r.WorkOrder.CreationDate != null &&
                                r.WorkOrder.CreationDate.Value.Date <= Convert.ToDateTime(tb_to.Text)
                                && r.WorkOrder.CreationDate.Value.Date >= Convert.ToDateTime(tb_from.Text)).ToList();
                            break;
                        case 6:
                        case 7:
                            query =
                                context.WorkOrderStatus.Where(r =>
                                    r.StatusID == statusid &&
                                    r.UpdateDate != null &&
                                    r.UpdateDate.Value.Date <= Convert.ToDateTime(tb_to.Text)
                                    && r.UpdateDate.Value.Date >= Convert.ToDateTime(tb_from.Text)).ToList();
                            break;
                    }
                    /*var query =
                        context.WorkOrderStatus.Where(
                            r =>
                                r.StatusID == Convert.ToInt32(ddl_requestsTypes.SelectedItem.Value) &&
                                r.WorkOrder.CreationDate != null &&
                                r.WorkOrder.CreationDate.Value.Date <= Convert.ToDateTime(tb_to.Text)
                                && r.WorkOrder.CreationDate.Value.Date >= Convert.ToDateTime(tb_from.Text)).ToList();
            
                    var results = context
                        .WorkOrderStatus
                        .Where(r => r.StatusID == Convert.ToInt32(ddl_requestsTypes.SelectedItem.Value))
                        .ToList().Where(r => r.UpdateDate != null &&
                                             r.UpdateDate.Value.Date <= Convert.ToDateTime(tb_to.Text)
                                             && r.UpdateDate.Value.Date >= Convert.ToDateTime(tb_from.Text)
                                             && r.WorkOrder.WorkOrderStatus.Any())*/
                    var results = query.Where(r => r.WorkOrder.WorkOrderStatus.Any())
                        .Select(w =>
                        {
                            //var user = context.Users.FirstOrDefault(u => u.ID == w.WorkOrder.ResellerID);
                            return new
                            {
                                w.WorkOrder.CustomerName,
                                w.WorkOrder.CustomerPhone,
                                Package = w.WorkOrder.ServicePackage.ServicePackageName,
                                Date = w.UpdateDate == null ? "" : w.UpdateDate.Value.Date.ToShortDateString(),
                                w.WorkOrder.CustomerMobile,
                                w.WorkOrder.CustomerEmail,
                                w.WorkOrder.Governorate.GovernorateName,
                                Provider = w.WorkOrder.ServiceProvider.SPName,
                                Branch = w.WorkOrder.Branch.BranchName,
                                Resseller = w.WorkOrder.ResellerID == null ? "-" : w.WorkOrder.User.UserName,//user == null ? "-" : user.UserName,
                                Status = w.Status.StatusName,
                                ResellerId = w.WorkOrder.ResellerID,
                                ProviderId = w.WorkOrder.ServiceProvider.ID,
                                w.WorkOrderID
                            };
                        }).ToList();
                    var list = new List<SearchResult>();
                    foreach (var w in results)
                    {
                        var result = new SearchResult
                        {
                            CustomerName = w.CustomerName,
                            CustomerPhone = w.CustomerPhone,
                            Package = w.Package,
                            Date = w.Date,
                            CustomerMobile = w.CustomerMobile,
                            CustomerEmail = w.CustomerEmail,
                            GovernorateName = w.GovernorateName,
                            Provider = w.Provider,
                            Branch = w.Branch,
                            Resseller = w.Resseller,
                            Status = w.Status,
                            ResellerId = w.ResellerId,
                            ProviderId = w.ProviderId,
                            WorkOrdersId = w.WorkOrderID
                        };
                        list.Add(result);
                    }

                    if (ddl_Reseller.SelectedIndex != 0)
                    {
                        list = list.Where(a => a.ResellerId == Convert.ToInt32(ddl_Reseller.SelectedItem.Value)).ToList();
                    }
                    if (ddl_Provider.SelectedIndex != 0)
                    {
                        list = list.Where(a => a.ProviderId == Convert.ToInt32(ddl_Provider.SelectedItem.Value)).ToList();
                    }
                    var limitedWo = DataLevelClass.GetUserWorkOrder(context);
                    var dataLevellist = new List<SearchResult>();
                    foreach (var liWo in limitedWo)
                    {
                        var id = liWo.ID;
                        var res = list.Where(a => a.WorkOrdersId == id);
                        dataLevellist.AddRange(res);
                    }
                    gv_results.DataSource = dataLevellist;
                    gv_results.DataBind();
                }
            }


            protected void gv_results_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_results, "gv_l_number");
            }
            public class SearchResult
            {
                public string CustomerName { get; set; }
                public string CustomerPhone { get; set; }
                public string Package { get; set; }
                public string Date { get; set; }
                public string CustomerMobile { get; set; }
                public string CustomerEmail { get; set; }
                public string GovernorateName { get; set; }
                public string Provider { get; set; }
                public int? ProviderId { get; set; }
                public string Branch { get; set; }
                public string Resseller { get; set; }
                public int? ResellerId { get; set; }
                public string Status { get; set; }
                public int? WorkOrdersId { get; set; }
            }
        }
    }
