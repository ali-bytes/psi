using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SupplierMovement : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("default.aspx");
                return;
            }
            Activate();
            btnExportToExcel.Visible = false;
            if (IsPostBack) return;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PopulateSuppliers(context);
            }
        }
        private void Activate()
        {
            GvitemData.DataBound += (o, e) => Helper.GridViewNumbering(GvitemData, "LNo");
        }
        void PopulateSuppliers(ISPDataContext context)
        {
            var suppliers = context.Suppliers.ToList();
            ddlSupplier.DataSource = suppliers;
            ddlSupplier.DataValueField = "Id";
            ddlSupplier.DataTextField = "SupplierName";
            ddlSupplier.DataBind();
            Helper.AddDefaultItem(ddlSupplier);
        }
        protected void Search(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                try
                {

               
                btnExportToExcel.Visible = true;
                var dateFrom = Convert.ToDateTime(datefrom.Text);
                var da = Convert.ToDateTime(datetoo.Text);
                var dateTo = da.AddHours(23).AddMinutes(59).AddSeconds(59);
                var SupplierId = Convert.ToInt32(ddlSupplier.SelectedValue);

                var list =
                    context.InvoiceDetails.Where(
                        x =>
                             x.InvoiceHeader.SupplierId == SupplierId && x.InvoiceHeader.invoiceDate >= dateFrom &&
                            x.InvoiceHeader.invoiceDate <= dateTo).Select(x => new
                            {
                                x.InvoiceHeader.invoiceDate,
                                x.InvoiceHeader.InvoiceNumber,
                                x.InvoiceHeader.Supplier.SupplierName,
                                x.InvoiceHeader.Total,
                                x.InvoiceHeader.Discound,
                                x.InvoiceHeader.Net
                            }
                        ).ToList();
               
                GvitemData.DataSource = list;
                GvitemData.DataBind();
                }
                catch (Exception)
                {

                }
            }

        }
        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            
            string attachment = string.Format("attachment; filename={0}.xls", Tokens.SupplierMovement);
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            GvitemData.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}