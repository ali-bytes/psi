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
    public partial class BillShow : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Gvbill_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(Gvbill, "LNo");
          
        }

        protected void search_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var billnum = txtnum.Text;


                if (billkind.SelectedValue == "0") return;
                if (billkind.SelectedValue == "1")
                {
                    Literal3.Visible = true;
                    Literal1.Visible = true;
                    var bill =
                        context.InvoiceDetails.Where(
                            z => z.InvoiceHeader.TypeId == 1 && z.InvoiceHeader.InvoiceNumber == billnum)
                            .Select(z => new
                            {
                                z.Item.ItemName,
                                z.Quantity,
                                z.itemprice,


                            }).ToList();
                    Gvbill.DataSource = bill;
                    Gvbill.DataBind();


                    var bill2 =
                       context.InvoiceDetails.Where(
                           z => z.InvoiceHeader.TypeId == 1 && z.InvoiceHeader.InvoiceNumber == billnum)
                           .Select(z => new
                           {
                               z.InvoiceHeader.InvoiceNumber,
                               SupplierName = z.InvoiceHeader.Supplier.SupplierName ?? "_",
                               z.InvoiceHeader.invoiceDate,
                               z.InvoiceHeader.Total,
                               z.InvoiceHeader.Discound,
                               z.InvoiceHeader.Net,
                               z.InvoiceHeader.Paid,
                               z.InvoiceHeader.Remaining,
                               z.InvoiceHeader.Store.StoreName,
                               z.InvoiceHeader.Notes,
                               CustomerName = z.InvoiceHeader.Customer.CustomerName ?? "_"

                           }).ToList();
                    GridView1.DataSource = bill2;
                    GridView1.DataBind();



                }
                else if (billkind.SelectedValue == "2")
                {
                    Literal3.Visible = true;
                    Literal1.Visible = true;
                    var bill =
                       context.InvoiceDetails.Where(
                           z => z.InvoiceHeader.TypeId == 2 && z.InvoiceHeader.InvoiceNumber == billnum)
                           .Select(z => new
                           {
                               z.Item.ItemName,
                               z.Quantity,
                               z.itemprice,


                           }).ToList();
                    Gvbill.DataSource = bill;
                    Gvbill.DataBind();


                    var bill2 =
                      context.InvoiceDetails.Where(
                          z => z.InvoiceHeader.TypeId == 2 && z.InvoiceHeader.InvoiceNumber == billnum)
                          .Select(z => new
                          {
                              z.InvoiceHeader.InvoiceNumber,
                           SupplierName=   z.InvoiceHeader.Supplier.SupplierName != null ? z.InvoiceHeader.Supplier.SupplierName :"_",
                              z.InvoiceHeader.invoiceDate,
                              z.InvoiceHeader.Total,
                              z.InvoiceHeader.Discound,
                              z.InvoiceHeader.Net,
                              z.InvoiceHeader.Paid,
                              z.InvoiceHeader.Remaining,
                              z.InvoiceHeader.Store.StoreName,
                              z.InvoiceHeader.Notes,
                           CustomerName=   z.InvoiceHeader.Customer.CustomerName!= null ?z.InvoiceHeader.Customer.CustomerName : "_"

                          }).ToList();
                    GridView1.DataSource = bill2;
                    GridView1.DataBind();
                }
                Reciept.Visible = true;

                Recipet(context,billnum);




            }
        }
        private void Recipet(ISPDataContext db,string invid2)
        {
           // var invid2 = Convert.ToInt32(Session["invid"]);
            var invv2 = (from g in db.InvoiceDetails where g.InvoiceHeader.InvoiceNumber == invid2.ToString() select g).FirstOrDefault();
            var userId2 = Convert.ToInt32(Session["User_ID"]);
            var username2 = db.Users.Where(x => x.ID == userId2).Select(x => x.UserName).FirstOrDefault();
            user2.Text = username2;


            cusname2.Text = invv2.InvoiceHeader.Customer.CustomerName;
            date2.Text = invv2.InvoiceHeader.invoiceDate.Value.ToShortDateString();
            comp2.Text = db.ReceiptCnfgs.Select(x => x.CompanyName).FirstOrDefault();
            Image2.ImageUrl = "../PrintLogos/" + db.ReceiptCnfgs.Select(x => x.LogoUrl).FirstOrDefault();
            billnum2.Text = invid2.ToString();

            total2.Text = invv2.InvoiceHeader.Total.Value.ToString("#.##");
            dis2.Text = invv2.InvoiceHeader.Discound.Value.ToString("#.##");
            totalafterdis2.Text = invv2.InvoiceHeader.Net.Value.ToString("#.##");
            note2.Text = invv2.InvoiceHeader.Notes;
            repaid2.Text = invv2.InvoiceHeader.Paid.Value.ToString("#.##");
            var inv2 = invid2.ToString();
            var griddata2 = (from g in db.InvoiceDetails
                             where g.InvoiceHeader.InvoiceNumber == inv2
                             select new
                             {

                                 g.Item.ItemName,
                                 g.Quantity,
                                 g.itemprice,
                                 total = g.Quantity * g.itemprice.Value


                             }).ToList();

            GridView2.DataSource = griddata2;
            GridView2.DataBind();
        }
    }
}