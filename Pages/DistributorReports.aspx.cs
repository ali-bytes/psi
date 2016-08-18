using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;
namespace NewIspNL.Pages
{
    public partial class DistributorReports : CustomPage
    {
        private readonly IspDomian _domian;

        public DistributorReports()
        {
            _domian = new IspDomian(IspDataContext);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            _domian.PopulateResellers(DdlReseller);
            _domian.PopulateBranches(DdlBranch, true);
        }
        void Search()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                lblTotal.Text = "";
                //IQueryable<DistributorPaymentRecord> demands = Enumerable.Empty<DistributorPaymentRecord>().AsQueryable();
                var demands = new List<DistributorPaymentRecord>();
                demands = context.DistributorPaymentRecords.ToList();
                if (DdlReseller.SelectedIndex > 0)
                {
                    demands = demands.Where(a => a.UserId == Helper.GetDropValue(DdlReseller)).ToList();
                }
                if (DdlBranch.SelectedIndex > 0)
                {

                    demands = demands.Where(a => a.WorkOrder!=null && a.WorkOrder.BranchID.HasValue && a.WorkOrder.BranchID == Helper.GetDropValue(DdlBranch)).ToList();
                }
                if (ddlCustomerType.SelectedIndex == 1)
                {
                    demands = demands.Where(a => a.WorkOrderId != null && a.WorkOrderId != 0).ToList();
                }
                if (ddlCustomerType.SelectedIndex == 2)
                {
                    demands = demands.Where(a => a.WorkOrderId == null || a.WorkOrderId == 0).ToList();
                }
                if (!string.IsNullOrWhiteSpace(txtFrom.Text))
                {
                    var d = Convert.ToDateTime(txtFrom.Text).Date;
                    demands = demands.Where(a => a.PaidDate.HasValue && a.PaidDate.Value.Date >= d).ToList();
                }
                if (!string.IsNullOrWhiteSpace(txtTo.Text))
                {
                    demands = demands.Where(a => a.PaidDate.Value.Date <= Convert.ToDateTime(txtTo.Text)).ToList();
                }

                GvResults.DataSource = demands.Select(a => new
                {
                    a.Id,
                    User= a.User.LoginName,
                    Date= a.PaidDate,
                    a.Amount,
                    Customer = a.CustomerName,
                    Phone = a.CustomerPhone,
                    CustomerType = a.WorkOrderId != null && a.WorkOrderId > 0 ? "داخلى": "خارجى",
                }).ToList();
                GvResults.DataBind();
               
                if (!demands.Any())
                {
                    return;

                }
                else
                {
                    if (GvResults.DataSource == null) return;
                    if (demands.Any())
                        lblTotal.Text = demands.Sum(a => a.Amount).ToString();
                    divTotal.Visible = true;
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        protected void NumberGrid(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvResults, "LNo");
        }
        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            string attachment = string.Format("attachment; filename={0}.xls", "DistributorInvoices");
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            GvResults.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();


        }
    }
}