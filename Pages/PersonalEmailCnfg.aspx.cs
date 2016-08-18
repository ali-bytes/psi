using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class PersonalEmailCnfg : CustomPage
    {
    

    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        MultiView1.SetActiveView(v_index);
        PopulateCnfgs();
        l_message.Text = "";
    }


    void PopulateCnfgs(){
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }
            var userId = Convert.ToInt32(Session["User_ID"]);
            var emailCnfgs = context.PersonalEmails.Where(a=>a.UserId==userId);
            gv_index.DataSource = emailCnfgs;
            gv_index.DataBind();
            if (emailCnfgs.Any())
            {
                btnAddNew.Visible = false;
            }
        }
    }


    protected void b_new_Click(object sender, EventArgs e){
        MultiView1.SetActiveView(v_AddEdit);
    }


    protected void b_save_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }
            var userId = Convert.ToInt32(Session["User_ID"]);
            PersonalEmail emailCnfg;
            if (hf_id.Value == string.Empty)
            {
                var exist = context.PersonalEmails.FirstOrDefault(a => a.UserId == userId);
                if (exist != null)
                {
                    l_message.Text = @"هذا المستخدم له إعدادت فعلا";
                    hf_id.Value = string.Empty;
                    Clear();
                    MultiView1.SetActiveView(v_index);
                    return;
                }
                emailCnfg = new PersonalEmail
                {
                    PersonalMail = txtEMail.Text,
                    PersonalPassword = txtPassword.Text,
                    Pop3 = txtPop3.Text,
                    UserId = userId,
                    SmtpClient = txtSmtpClient.Text,
                    SSL = CbSsl.Checked,
                    Active = CbActive.Checked,
                    SenderUserName = txtUserName.Text,
                    SenderPassWord = txtUserPassword.Text,
                    
                   
                };
                if (!string.IsNullOrWhiteSpace(txtPort.Text))
                    emailCnfg.Port = Convert.ToInt32(Convert.ToInt32(txtPort.Text));
                if (!string.IsNullOrWhiteSpace(txtsendPort.Text))
                    emailCnfg.SendPort = Convert.ToInt32(Convert.ToInt32(txtsendPort.Text));
                context.PersonalEmails.InsertOnSubmit(emailCnfg);
            }
            else
            {
                emailCnfg = context.PersonalEmails.FirstOrDefault(a => a.Id == Convert.ToInt32(hf_id.Value));//_cnfgRepository.EmailCnfgs.FirstOrDefault(o => o.Id == );
                if (emailCnfg != null)
                {
                    emailCnfg.PersonalMail = txtEMail.Text;
                    emailCnfg.PersonalPassword = txtPassword.Text;
                    emailCnfg.Pop3 = txtPop3.Text;
                    emailCnfg.UserId = userId;
                    emailCnfg.SmtpClient = txtSmtpClient.Text;
                    emailCnfg.SSL = CbSsl.Checked;
                    emailCnfg.Active = CbActive.Checked;
                    emailCnfg.SenderUserName = txtUserName.Text;
                    emailCnfg.SenderPassWord = txtUserPassword.Text;
                    if (!string.IsNullOrWhiteSpace(txtPort.Text))
                        emailCnfg.Port = Convert.ToInt32(Convert.ToInt32(txtPort.Text));
                    if (!string.IsNullOrWhiteSpace(txtsendPort.Text))
                        emailCnfg.SendPort = Convert.ToInt32(Convert.ToInt32(txtsendPort.Text));
                }
            }
            context.SubmitChanges();
          
            PopulateCnfgs();
            if (emailCnfg != null) l_message.Text = string.Format(Tokens.Saved);
            hf_id.Value = string.Empty;
            Clear();
            MultiView1.SetActiveView(v_index);
        }
    }


    void Clear(){
        foreach(var control in inputs.Controls){
            var tb = control as TextBox;
            if(tb != null){
                tb.Text = string.Empty;
            } else{
                var cb = control as CheckBox;
                if(cb != null){
                    cb.Checked = false;
                }
            }
        }
    }


    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
    }


    protected void gvb_edit_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            MultiView1.SetActiveView(v_AddEdit);
            var buttonSender = sender as LinkButton;
            if (buttonSender == null) return;
            var id = Convert.ToInt32(buttonSender.CommandArgument);
            var emailCnfg = context.PersonalEmails.FirstOrDefault(a => a.Id == id);
            if (emailCnfg == null) return;
            txtEMail.Text = emailCnfg.PersonalMail;
            txtPassword.Text = emailCnfg.PersonalPassword;
            
            txtPop3.Text = emailCnfg.Pop3;
            txtSmtpClient.Text = emailCnfg.SmtpClient;
            CbSsl.Checked = emailCnfg.SSL;
            CbActive.Checked = emailCnfg.Active;
            txtUserName.Text = emailCnfg.SenderUserName;
            txtUserPassword.Text = emailCnfg.SenderPassWord;
            txtPort.Text = string.Format("{0}", emailCnfg.Port);
            txtsendPort.Text = string.Format("{0}", emailCnfg.SendPort);
            hf_id.Value = emailCnfg.Id.ToString(CultureInfo.InvariantCulture);
        }
    }


    protected void gvb_delete_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var cnfg =
                context.PersonalEmails.FirstOrDefault(
                    o => o.Id == Convert.ToInt32((sender as LinkButton).CommandArgument));
            if (cnfg == null) return;
            l_message.Text = string.Format(Tokens.Saved);
            context.PersonalEmails.DeleteOnSubmit(cnfg);
            context.SubmitChanges();
            PopulateCnfgs();
        }
    }

}
}