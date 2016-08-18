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
    public partial class SupplierAccountStatement : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Activate();
            btnExportToExcel.Visible = false;
            if (IsPostBack) return;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PopulateSupplier(context);
            }
        }
        private void Activate()
        {
            GvitemData.DataBound += (o, e) => Helper.GridViewNumbering(GvitemData, "LNo");
        }
        void PopulateSupplier(ISPDataContext db)
        {
            var supplier = db.Suppliers.ToList();
            ddlSupplier.DataSource = supplier;
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

                var da = Convert.ToDateTime(datetoo.Text);
                var dateTo = da.AddHours(23).AddMinutes(59).AddSeconds(59);

                var supplierid = Convert.ToInt32(ddlSupplier.SelectedValue);
                var dateFrom = Convert.ToDateTime(datefrom.Text);
                var DebitopeningBalance = context.Accounts.Where(a => a.SupplierId == supplierid && a.Paymentdate < dateFrom).Sum(x => x.Amount);
                var CreditopeningBalance = context.InvoiceHeaders.Where(a => a.SupplierId == supplierid && a.invoiceDate < dateFrom).Sum(x => x.Net);
                var openingBalanceNet = DebitopeningBalance - CreditopeningBalance;

                var Credit = context.InvoiceHeaders.Where(a => a.SupplierId == supplierid && a.invoiceDate >= dateFrom &&
                            a.invoiceDate <= dateTo).Select(x => new AccountStatement
                            {
                                Date = (DateTime)x.invoiceDate,
                                Debit = 0,
                                Credit = decimal.Round((decimal)x.Net, 2, MidpointRounding.AwayFromZero),
                                Description = string.Format("فاتورة مشتريات رقم {0}", x.InvoiceNumber.ToString())
                            }).ToList();



                var Debit  = context.Accounts.Where(a => a.SupplierId == supplierid && a.Paymentdate >= dateFrom && a.Paymentdate <= dateTo).Select(x => new AccountStatement
                {
                    Date = (DateTime)x.Paymentdate,
                    Debit = decimal.Round((decimal)x.Amount, 2, MidpointRounding.AwayFromZero),
                    Credit = 0,
                    Description = "سداد"
                }).ToList();



                var list1 = Debit.Concat(Credit).ToList();


                // اضافة رصيد اول المدة
                if (openingBalanceNet > 0)
                {
                    list1.Insert(0, new AccountStatement() { Date = dateFrom, Debit = openingBalanceNet == null ? 0 : decimal.Round((decimal)openingBalanceNet, 2, MidpointRounding.AwayFromZero), Credit = 0, Description = "رصيد أول المدة" });

                }
                else if (openingBalanceNet < 0)
                {
                    list1.Insert(0, new AccountStatement() { Date = dateFrom, Debit = 0, Credit = openingBalanceNet == null ? 0 : decimal.Round(Math.Abs((decimal)openingBalanceNet), 2, MidpointRounding.AwayFromZero), Description = "رصيد أول المدة" });
                }
                else
                {
                    list1.Insert(0, new AccountStatement() { Date = dateFrom, Debit = 0, Credit = 0, Description = "رصيد أول المدة" });

                }


                //إضافة الاجمالى
                var lcount = list1.Count;
                var total = list1.Sum(a => a.Debit) - list1.Sum(x => x.Credit);

                List<Total> totalList = new List<Total>();
                totalList.Add(new Total() { Description = "الأجمالى", Debit = list1.Sum(a => a.Debit), Credit = list1.Sum(x => x.Credit) });
                if (total > 0)
                {
                    totalList.Add(new Total() { Description = "الرصيد النهائى", Debit = decimal.Round((decimal)total, 2, MidpointRounding.AwayFromZero), Credit = 0 });
                }
                else if (total < 0)
                {
                    totalList.Add(new Total() { Description = "الرصيد النهائى", Debit = 0, Credit = decimal.Round(Math.Abs((decimal)total), 2, MidpointRounding.AwayFromZero) });
                }
                else
                {
                    totalList.Add(new Total() { Description = "الرصيد النهائى", Debit = 0, Credit = 0 });
                }


                GvitemData.DataSource = list1.OrderBy(a => a.Date).ToList();
                GvitemData.DataBind();

                GVTotal.DataSource = totalList.ToList();
                GVTotal.DataBind();
                }
                catch (Exception)
                {

                    
                }
            }

        }
        protected void btnExport_OnClick(object sender, EventArgs e)
        {
           
            //string attachment = string.Format("attachment; filename={0}.xls", Tokens.SupplierAccountStatement);
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/ms-excel";
            //Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            //var sw = new StringWriter();
            //var htw = new HtmlTextWriter(sw);
            //GvitemData.RenderControl(htw);
            //Response.Write(sw.ToString());
            //Response.End();

            ExportTwoGrid();

        }
        void ExportTwoGrid()
        {
            string attachment = string.Format("attachment; filename={0}.xls", Tokens.SupplierAccountStatement);
            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition", attachment);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);


            Table tb = new Table();
            TableRow tr1 = new TableRow();
            TableCell cell1 = new TableCell();
            cell1.Controls.Add(GvitemData);
            tr1.Cells.Add(cell1);
            TableCell cell3 = new TableCell();
            cell3.Controls.Add(GVTotal);
            TableCell cell2 = new TableCell();
            cell2.Text = "&nbsp;";

            TableRow tr2 = new TableRow();
            tr2.Cells.Add(cell2);
            TableRow tr3 = new TableRow();
            tr3.Cells.Add(cell3);
            tb.Rows.Add(tr1);
            tb.Rows.Add(tr2);
            tb.Rows.Add(tr3);

            tb.RenderControl(hw);
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}