using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;

namespace NewIspNL.Pages
{
    public partial class CallCenter : CustomPage
    {
    
            //static bool _branchPrintExcel; //it should be static

            readonly LtsCallRepository _repository = new LtsCallRepository();


            public override void VerifyRenderingInServerForm(Control control) { }


            protected void Page_Load(object sender, EventArgs e)
            {
                // if(_branchPrintExcel)
                //_branchPrintExcel=false;
            }



            protected void b_show_Click(object sender, EventArgs e)
            {
                SearchCallsByDate();
            }


            void SearchCallsByDate()
            {
                List<Call> results =
                    _repository.Calls
                        .Where(c =>
                                   c.CallDate.Value.Date >= Convert.ToDateTime(tb_from.Text)
                                       && c.CallDate.Value.Date <= Convert.ToDateTime(tb_to.Text))
                        .ToList();
                gv_calls.DataSource = results;
                gv_calls.DataBind();
            }


            protected void gv_lb_edit_Click(object sender, EventArgs e)
            {
                var linkButton = sender as LinkButton;
                if (linkButton == null) return;
                int id = Convert.ToInt32(linkButton.CommandArgument);
                Response.Redirect(string.Format("~/pages/CallCenterEntry.aspx?id={0}", id));
            }



            protected void gv_lb_delete_Click(object sender, EventArgs e)
            {
                var linkButton = sender as LinkButton;
                if (linkButton != null)
                {
                    int id = Convert.ToInt32(linkButton.CommandArgument);
                    Call call = _repository.Calls.FirstOrDefault(c => c.Id == id);
                    _repository.DeleteCall(call);
                }

                SearchCallsByDate();
            }





            protected void Button1_Click(object sender, EventArgs e)
            {
                //_branchPrintExcel=true;
                const string attachment = "attachment; filename=Contacts.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";

                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                Print.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
    }
 