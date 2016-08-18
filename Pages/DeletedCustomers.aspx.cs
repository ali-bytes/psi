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
    public partial class DeletedCustomers : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateGrd();

            }
        }

        private void PopulateGrd()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var Customers = context.DeletedCustomersHistories.Select(x=>new
                {
                    x.Phone,
                    User=x.User.LoginName,
                    x.DeleteDate
                }).ToList();
                gv_customers.DataSource = Customers;
                gv_customers.DataBind();
            }
        }

        protected void gv_customers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(gv_customers, "gv_l_number");
        }
    }
}