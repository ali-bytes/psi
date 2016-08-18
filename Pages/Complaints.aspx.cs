using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Complaints : CustomPage
    {
       
            private readonly IComplaintsRepository _complaintsRepository = new LComplaintsRepository();
            protected void Page_Load(object sender, EventArgs e) { }

            protected void Button1_Click(object sender, EventArgs e)
            {
                var complaint = new Complaint
                {
                    Body = tb_body.Text,
                    Governate = tb_gov.Text,
                    Phone = tb_phone.Text,
                    Customer = tb_customer.Text,
                    Handled = false,
                    RecordDate = Convert.ToDateTime(tb_date.Text)
                };
                _complaintsRepository.Save(complaint);
                l_message.Text = Tokens.Saved;
                ClearForm();
            }

            private void ClearForm()
            {
                foreach (Control control in containerPanel.Controls)
                {
                    var input = control as TextBox;
                    if (input != null)
                    {
                        input.Text = string.Empty;
                    }
                }
            }
        }
    }
 