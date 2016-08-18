using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class EmployeeIncome : CustomPage
    {
       
            //static bool _branchPrintExcel; //it should be static


            //readonly ISPDataContext _context;

            readonly DemandSearch _demandSearch;

            readonly IspEntries _ispEntries;


            public  EmployeeIncome()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
                _demandSearch = new DemandSearch();
                _ispEntries = new IspEntries(context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateEmployees();
            }


            void PopulateEmployees()
            {
                var activeUserId = Convert.ToInt32(Session["User_ID"]);
                var employees = _ispEntries.ActiveUserUsers(activeUserId);
                ddl_employee.DataSource = employees;
                ddl_employee.DataTextField = "UserName";
                ddl_employee.DataValueField = "ID";
                ddl_employee.DataBind();
                Helper.AddDefaultItem(ddl_employee);
            }


            public override void VerifyRenderingInServerForm(Control control) { }


            protected void Button1_Click(object sender, EventArgs e)
            {
                //_branchPrintExcel = true;
                const string attachment = "attachment; filename=Contacts.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                Print.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }


            protected void b_show_Click(object sender, EventArgs e)
            {
                var userId = Convert.ToInt32(ddl_employee.SelectedItem.Value);
                var demandsByUser = _demandSearch.SearchDemandsByUser(userId, Convert.ToDateTime(tb_from.Text).Date, Convert.ToDateTime(tb_to.Text).Date, true);
                gv_payments.DataSource = demandsByUser;
                gv_payments.DataBind();
                var totalPayments = demandsByUser.Sum(i => i.DAmount);
                l_total.Text = Helper.FixNumberFormat(totalPayments);
            }


            protected void gv_payments_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_payments, "l_no");//.AddNumberToGridViewLabel(gv_payments, "l_no");
            }
        }
    }
