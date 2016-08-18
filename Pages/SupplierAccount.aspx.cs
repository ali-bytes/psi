using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Microsoft.Ajax.Utilities;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class SupplierAccount :CustomPage
    {
       

    public IStoreAccounts Accounts;
    public SupplierAccount()
    {
        Accounts=new StoreAccounts();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            PopulateSupplier(context);
        }
    }
    void PopulateSupplier(ISPDataContext db)
    {
        var supplier = db.Suppliers.ToList();
        ddlSupplier.DataSource = supplier;
        ddlSupplier.DataBind();
        Helper.AddDefaultItem(ddlSupplier);
    }

    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSupplier.SelectedIndex != 0)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var supplierId = Convert.ToInt32(ddlSupplier.SelectedItem.Value);
                var allInvoices = context.Accounts.Where(a => a.SupplierId == supplierId).Select(a=>new
                {
                    a.Amount,
                   a.Total,
                   a.User.UserName,
                   //a.Date,
                   a.Paymentdate,
                   a.PaymentComment,
                   a.Notes,
                    Remaining = a.Total==0? "0" : Helper.FixNumberFormat(a.Total - a.Amount) 
                });
                var reamin = allInvoices.Sum(a => a.Total) - allInvoices.Sum(a => a.Amount);
                txtRemaining.Text = Helper.FixNumberFormat(reamin);
                GvCredites.DataSource = allInvoices.OrderByDescending(o=>o.Paymentdate).ToList();
                GvCredites.DataBind();
            }
        }
    }
    protected void txtPaid_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtPaid.Text) && !string.IsNullOrWhiteSpace(txtRemaining.Text))
        {
            var remain = Convert.ToDecimal(txtRemaining.Text);
            var paid = Convert.ToDecimal(txtPaid.Text);
            txtNextRemain.Text = remain >= paid ? Helper.FixNumberFormat(remain - paid) : "0";

        }
    }

    protected void Pay(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtNextRemain.Text)&&!string.IsNullOrWhiteSpace(txtPaid.Text))
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var supplierId = Convert.ToInt32(ddlSupplier.SelectedItem.Value);
                var spplierName = ddlSupplier.SelectedValue;
                var remain = Convert.ToDecimal(txtRemaining.Text);
                var paid = Convert.ToDecimal(txtPaid.Text);
                var nextRemain = Convert.ToDecimal(txtNextRemain.Text);
                var now = DateTime.Now.AddHours();
                var userId = Convert.ToInt32(Session["User_ID"]);
                var note = string.Format("دفع باقى فاتورة للمورد {0} ", spplierName);
                Accounts.AddInAccount(supplierId, null, Convert.ToDecimal(0),
                    Convert.ToDecimal(paid), now, "دفع من صفحة حساب مورد", userId,
                    note, context);


                if (ddlSupplier.SelectedIndex != 0)
                {
                   
                        
                        var allInvoices = context.Accounts.Where(a => a.SupplierId == supplierId).Select(a => new
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
                        txtRemaining.Text = Helper.FixNumberFormat(reamin);
                        GvCredites.DataSource = allInvoices.OrderByDescending(o => o.Paymentdate).ToList();
                        GvCredites.DataBind();
                    }
                }




            }
        }
    }
}
