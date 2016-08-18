using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
    public partial class BranchDirectCustomers : CustomPage
    {

        //static bool BranchPrintExcel; //it should be static


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User_ID"] == null)
                return;

            if (!IsPostBack)
            {
                Bind_ddl_Branch();
            }
        }


        void Bind_ddl_Branch()
        {
            ddl_Branch.SelectedValue = null;
            ddl_Branch.Items.Clear();

            ddl_Branch.AppendDataBoundItems = true;
            var query = DataLevelClass.GetUserBranches();
            ddl_Branch.DataSource = query;
            ddl_Branch.DataBind();
            Helper.AddDefaultItem(ddl_Branch);
        }


        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var users = DataLevelClass.GetUserWorkOrder(dataContext);
                var query = from wo in users
                            where
                                wo.BranchID == Convert.ToInt32(ddl_Branch.SelectedValue)
                                && wo.ResellerID == null
                            select new
                            {
                                wo.CustomerName,
                                wo.CustomerPhone,
                                wo.Governorate.GovernorateName,
                                wo.ServicePackage.ServicePackageName,
                                wo.ServiceProvider.SPName,
                                wo.Status.StatusName,
                                UpdateDate = wo.CreationDate
                            };
                grd_Customers.DataSource = query;
                grd_Customers.DataBind();
            }
        }


        protected void Exprot_Click(object sender, EventArgs e)
        {
            //BranchPrintExcel = true;
            string attachment = string.Format("attachment; filename={0}", Tokens.ExcelTityleDirectCustomers);
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grd_Customers.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            //don't throw any exception!
        }

        protected void grd_Customers_DataBound1(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grd_Customers.Rows)
            {
                var label = row.FindControl("gv_l_number") as Label;
                if (label != null) label.Text = (row.RowIndex + 1).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
