using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using Db;
using System.Configuration;
using NewIspNL.Helpers;


namespace NewIspNL.Pages
{
    public partial class CustomerAccount : CustomPage
    {
    
     public IStoreAccounts Accounts;
     public CustomerAccount()
    {
        Accounts=new StoreAccounts();
    }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PopulateCustomers(context);
            }
        }
        void PopulateCustomers(ISPDataContext context)
        {
            var customers = context.Customers.ToList();
            ddlCustomer.DataSource = customers;
            ddlCustomer.DataValueField = "id";
            ddlCustomer.DataTextField = "CustomerName";
            ddlCustomer.DataBind();
            Helper.AddDefaultItem(ddlCustomer);
        }
       
        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomer.SelectedIndex != 0)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var customerId = Convert.ToInt32(ddlCustomer.SelectedItem.Value);
                    var allInvoices = context.Accounts.Where(a => a.CustomerId == customerId).Select(a => new
                    {
                        a.Amount,
                        a.Total,
                        a.User.UserName,
                        //a.Date,
                        a.Paymentdate,
                        a.PaymentComment,
                        a.Notes,
                        Remaining = a.Total == 0 ? "0" : Helper.FixNumberFormat(a.Total - a.Amount)
                    });
                    var reamin = allInvoices.Sum(a => a.Total) - allInvoices.Sum(a => a.Amount);
                    lblRemaining.Text =Math.Round(Convert.ToDecimal(Helper.FixNumberFormat(reamin)),2).ToString();
                    GvCredites.DataSource = allInvoices.OrderByDescending(o => o.Paymentdate).ToList();
                    GvCredites.DataBind();
                }
            }
        }

        protected void Pay(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPaid.Text))
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var customerId = Convert.ToInt32(ddlCustomer.SelectedItem.Value);
                    var customerName = ddlCustomer.SelectedItem.ToString();
                    //var remain = Convert.ToDecimal(txtRemaining.Text);
                    var paid = Convert.ToDecimal(txtPaid.Text);
                    //var nextRemain = Convert.ToDecimal(txtNextRemain.Text);
                    var now = DateTime.Now.AddHours();
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var note = string.Format("دفع باقى فاتورة للعميل {0} ", customerName);
                    Accounts.AddInAccount(null, customerId, Convert.ToDecimal(0),
                    Convert.ToDecimal(paid), now, "دفع من صفحة حساب عميل", userId,note, context);

                    if (ddlCustomer.SelectedIndex != 0)
                    {

                        var allInvoices = context.Accounts.Where(a => a.CustomerId == customerId).Select(a => new
                        {
                            a.Amount,
                            a.Total,
                            a.User.UserName,
                            //a.Date,
                            a.Paymentdate,
                            a.PaymentComment,
                            a.Notes,
                        });
                        var reamin = allInvoices.Sum(a => a.Total) - allInvoices.Sum(a => a.Amount);
                        lblRemaining.Text = Helper.FixNumberFormat(reamin);
                        GvCredites.DataSource = allInvoices.OrderByDescending(o => o.Paymentdate).ToList();
                        GvCredites.DataBind();
                    }
                }




            }
        }
    }
}