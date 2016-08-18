using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class TransferBtSavesReport : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
          
        }
      
        protected void b_show_Click(object sender, EventArgs e)
        {
            var status = Convert.ToInt32(ddl_status.SelectedItem.Value);
            var from = tb_from.Text;
            var to = tb_to.Text;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var requests = context.TransferBetweenSavesRequests.Select(s => new
                {
                    id = s.Id,
                    FromSave = s.Save.SaveName,
                    ToSave = s.Save1.SaveName,
                    s.Status,
                    s.Amount,
                    RequestMaker = s.User1.UserName,
                    s.RequestDate,
                    s.RequestMakerNote,
                    RequestConfirmer = s.User.UserName,
                    ConfirmDate = s.ApprovedDate,
                    s.ConfirmerNote

                }).ToList();
               
                if (status == 2)
                {
                    requests = requests.Where(x => x.Status == true).ToList();
                }
                else if (status == 3)
                {
                    requests = requests.Where(x => x.Status == false).ToList();
                }
                else if (status == 4)
                {
                    requests = requests.Where(x => x.Status == null).ToList();
                }
                if (!string.IsNullOrEmpty(from))
                {
                    requests = requests.Where(x => x.RequestDate.Value.Date >= Convert.ToDateTime(from)).ToList();
                }
                if (!string.IsNullOrEmpty(to))
                {
                    requests = requests.Where(x => x.RequestDate.Value.Date <= Convert.ToDateTime(to)).ToList();
                }
                grd_Requests.DataSource = requests;
                grd_Requests.DataBind();
            }
        }
        protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(grd_Requests, "lbl_No");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = "<b>" + e.Row.Cells[3].Text + "</b>";
                if (e.Row.Cells[3].Text.Trim() == "<b>True</b>")
                {
                    e.Row.Cells[3].Text = "تمت الموافقة";
                    e.Row.Cells[3].ForeColor = Color.Green;
                }
                if (e.Row.Cells[3].Text.Trim() == "<b>False</b>")
                {
                    e.Row.Cells[3].Text = "تم الرفض";
                    e.Row.Cells[3].ForeColor = Color.OrangeRed;
                }
                if (e.Row.Cells[3].Text == "<b>&nbsp;</b>")
                {
                    e.Row.Cells[3].Text = "انتظار";
                    e.Row.Cells[3].ForeColor = Color.CornflowerBlue;
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            const string attachment = "attachment; filename=Transfer_Between_Saves.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grd_Requests.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}