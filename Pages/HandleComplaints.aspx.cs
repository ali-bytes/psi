using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class HandleComplaints : CustomPage
    {
       
            readonly IComplaintsRepository _complaintsRepository = new LComplaintsRepository();

            readonly IComplaintsServices _complaintsServices = new ComplainsServices();


            protected void Page_Load(object sender, EventArgs e) { }

            protected void gv_items_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_items, "l_Number");
            }


            protected void b_show_Click(object sender, EventArgs e)
            {
                PopulateComplaints();
            }


            void PopulateComplaints()
            {
                var complaints = _complaintsServices.ComplaintsInPeriod(Convert.ToDateTime(tb_from.Text),
                                                                        Convert.ToDateTime(tb_to.Text));
                gv_items.DataSource = complaints.Select(c => new
                {
                    c.Id,
                    c.Customer,
                    c.Governate,
                    c.Phone,
                    complaint = c.Body,
                    Date = c.RecordDate.Value.Date.ToShortDateString(),
                    c.Handled
                });
                gv_items.DataBind();
            }

            protected void Button1_Click(object sender, EventArgs e)
            {
                var button = sender as LinkButton;
                if (button != null)
                {
                    var id = Convert.ToInt32(button.CommandArgument);
                    var complaint = _complaintsRepository.Complaints.FirstOrDefault(c => c.Id == id);
                    if (complaint != null)
                    {
                        complaint.Handled = true;
                        _complaintsRepository.Save(complaint);
                    }
                    PopulateComplaints();
                    if (complaint != null)
                        l_message.Text = string.Format(Tokens.ComplaintHandled, complaint.Customer);
                }
            }
        }
    }
 