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
    public partial class UserTrack : CustomPage
    {
        
            readonly IspEntries _ispEntries;

            public UserTrack()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture));
             
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

            var pType = _ispEntries.ProcessType.ToList();
            ddlProcess.DataSource = pType;
            ddlProcess.DataTextField = "Name";
            ddlProcess.DataValueField = "Id";
            ddlProcess.DataBind();
            Helper.AddDefaultItem(ddlProcess);
        }
        protected void b_show_Click(object sender, EventArgs e)
        {
            var pType = _ispEntries.UserTrack.ToList();
            if (ddl_employee.SelectedIndex > 0 )
            {
                var userId = Convert.ToInt32(ddl_employee.SelectedItem.Value);
                pType = pType.Where(x => x.UserId == userId).ToList();

            }
            if (ddlProcess.SelectedIndex > 0)
            {
                var processId = Convert.ToInt32(ddlProcess.SelectedItem.Value);
                pType = pType.Where(x => x.ProcessTypeId == processId).ToList();

            }
            if (!string.IsNullOrEmpty(tb_from.Text))
            {
                pType = pType.Where(x => x.Date.Value.Date >= Convert.ToDateTime(tb_from.Text).Date).ToList();

            }
            if (!string.IsNullOrEmpty(tb_to.Text))
            {
                pType = pType.Where(x => x.Date.Value.Date <= Convert.ToDateTime(tb_to.Text).Date).ToList();

            }
            gv_results.DataSource = pType.Select(x=>new UserTracks
            {
                Customer = x.WorkOrder == null ?"": x.WorkOrder.CustomerName,
                CustomerPhone = x.WorkOrder == null ? "" : x.WorkOrder.CustomerPhone,
                Process = x.ProcessType.Name,
                Date = x.Date == null?"":x.Date.ToString(),
                Note = x.Note,
                User = x.User.LoginName
            });
            gv_results.DataBind();
          
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
           
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
        protected void gv_results_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(gv_results, "l_no");
        }
         public class UserTracks
        {
             public string User { get; set; }
             public string Process { get; set; }
             public string Date { get; set; }
             public string Note { get; set; }
            public string Customer { get; set; }
            public string CustomerPhone { get; set; }
        }
    }
}