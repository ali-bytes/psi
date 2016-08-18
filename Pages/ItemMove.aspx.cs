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
    public partial class ItemMove : CustomPage
    {
       
            // ISPDataContext db=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            protected void Page_Load(object sender, EventArgs e)
            {
                // using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    if (Session["User_ID"] == null)
                    {
                        Response.Redirect("default.aspx");
                        return;
                    } Activate();
                    btnExportToExcel.Visible = false;
                    if (IsPostBack) return;
                    FillItems();

                }
            }
            private void Activate()
            {
                GvitemData.DataBound += (o, e) => Helper.GridViewNumbering(GvitemData, "LNo");
            }
            void FillItems()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var allitemInStore =
                        context.StoreTransactions.Select(a => new
                        {
                            a.Item.ItemName,
                            a.ItemId
                        }).ToList();
                    ddlItems.DataSource = allitemInStore.Distinct().ToList();
                    ddlItems.DataBind();
                    Helper.AddDefaultItem(ddlItems);
                }
            }

            protected void Search(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    btnExportToExcel.Visible = true;
                    var dateFrom = Convert.ToDateTime(datefrom.Text);
                    var da = Convert.ToDateTime(datetoo.Text);
                    var dateTo = da.AddHours(23).AddMinutes(59).AddSeconds(59);
                    var itemid = Convert.ToInt32(ddlItems.SelectedValue);

                    var list =
                        context.InvoiceDetails.Where(
                            x =>
                                x.ItemId == itemid && x.InvoiceHeader.invoiceDate >= dateFrom &&
                                x.InvoiceHeader.invoiceDate <= dateTo).Select(x => new
                                {
                                    x.Quantity,
                                    x.itemprice,
                                    total = x.Quantity * x.itemprice,
                                    x.InvoiceHeader.Supplier.SupplierName,
                                    x.InvoiceHeader.Customer.CustomerName,
                                    x.InvoiceHeader.invoiceDate,
                                    x.InvoiceHeader.Store.StoreName,
                                    x.InvoiceHeader.InvoiceType.TypeName,
                                    x.InvoiceHeader.Notes,
                                    x.Item.ItemName

                                }
                            ).ToList();

                    GvitemData.DataSource = list;
                    GvitemData.DataBind();
                }

            }
            protected void btnExport_OnClick(object sender, EventArgs e)
            {
                //BranchPrintExcel = true;
                string attachment = string.Format("attachment; filename={0}.xls", Tokens.ItemMove);
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
 