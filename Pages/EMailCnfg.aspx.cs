using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class EMailCnfg : CustomPage
    {
      
            readonly EmailCnfgRepository _cnfgRepository = new EmailCnfgRepository();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                MultiView1.SetActiveView(v_index);
                PopulateCnfgs();
                l_message.Text = "";
            }


            void PopulateCnfgs()
            {
                var emailCnfgs = _cnfgRepository.EmailCnfgs;
                gv_index.DataSource = emailCnfgs;
                gv_index.DataBind();
            }


            protected void b_new_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
            }


            protected void b_save_Click(object sender, EventArgs e)
            {
                EmailCnfg emailCnfg;
                if (hf_id.Value == string.Empty)
                {
                    emailCnfg = new EmailCnfg
                    {
                        MailFrom = TbMailFrom.Text,
                        SmtpClient = TbSmtpClient.Text,
                        UserName = TbUserName.Text,
                        Password = TbPassword.Text,
                        SSL = CbSsl.Checked,
                        Active = CbActive.Checked,

                    };
                    if (!string.IsNullOrWhiteSpace(TbPort.Text)) emailCnfg.Port = Convert.ToInt32(Convert.ToInt32(TbPort.Text));
                }
                else
                {
                    emailCnfg = _cnfgRepository.EmailCnfgs.FirstOrDefault(o => o.Id == Convert.ToInt32(hf_id.Value));
                    if (emailCnfg != null)
                    {
                        emailCnfg.MailFrom = TbMailFrom.Text;
                        emailCnfg.SmtpClient = TbSmtpClient.Text;
                        emailCnfg.UserName = TbUserName.Text;
                        emailCnfg.Password = TbPassword.Text;
                        emailCnfg.SSL = CbSsl.Checked;
                        emailCnfg.Active = CbActive.Checked;
                        if (!string.IsNullOrWhiteSpace(TbPort.Text)) emailCnfg.Port = Convert.ToInt32(Convert.ToInt32(TbPort.Text));
                    }
                }
                _cnfgRepository.Save(emailCnfg);
                PopulateCnfgs();
                if (emailCnfg != null) l_message.Text = string.Format(Tokens.Saved);
                hf_id.Value = string.Empty;
                Clear();
                MultiView1.SetActiveView(v_index);
            }


            void Clear()
            {
                foreach (var control in inputs.Controls)
                {
                    var tb = control as TextBox;
                    if (tb != null)
                    {
                        tb.Text = string.Empty;
                    }
                    else
                    {
                        var cb = control as CheckBox;
                        if (cb != null)
                        {
                            cb.Checked = false;
                        }
                    }
                }
            }


            protected void gv_index_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_index, "l_number");
            }


            protected void gvb_edit_Click(object sender, EventArgs e)
            {
                MultiView1.SetActiveView(v_AddEdit);
                var buttonSender = sender as LinkButton;
                if (buttonSender == null) return;
                var id = Convert.ToInt32(buttonSender.CommandArgument);
                var emailCnfg = _cnfgRepository.EmailCnfgs.FirstOrDefault(o => o.Id == id);
                if (emailCnfg == null) return;
                TbMailFrom.Text = emailCnfg.MailFrom;
                TbSmtpClient.Text = emailCnfg.SmtpClient;
                TbUserName.Text = emailCnfg.UserName;
                TbPassword.Text = emailCnfg.Password;
                CbSsl.Checked = emailCnfg.SSL;
                CbActive.Checked = emailCnfg.Active;
                TbPort.Text = string.Format("{0}", emailCnfg.Port == null ? string.Empty : emailCnfg.Port.ToString());
                hf_id.Value = emailCnfg.Id.ToString(CultureInfo.InvariantCulture);
            }


            protected void gvb_delete_Click(object sender, EventArgs e)
            {
                var cnfg = _cnfgRepository.EmailCnfgs.FirstOrDefault(o => o.Id == Convert.ToInt32((sender as LinkButton).CommandArgument));
                if (cnfg == null) return;
                l_message.Text = string.Format(Tokens.Saved);
                _cnfgRepository.Delete(cnfg);
                PopulateCnfgs();
            }
        }
    }
 