using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using OpenPop.Pop3;
using Resources;

namespace NewIspNL.Pages
{
    public partial class EmailMessages : CustomPage
    {

        protected List<Email> Emails
        {
            get { return (List<Email>)ViewState["Emails"]; }
            set { ViewState["Emails"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Read_Emails();
            }
        }

        PersonalEmail GetConfig()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null)
                {
                    Response.Redirect("~/Pages/default.aspx");
                    return null;
                }
                var userId = Convert.ToInt32(Session["User_ID"]);
                return context.PersonalEmails.FirstOrDefault(a => a.UserId == userId);
            }
        }

        private void Read_Emails()
        {
            try
            {
                Pop3Client pop3Client;
                if (Session["Pop3Client"] == null)
                {
                    var config = GetConfig();
                    if (config == null) return;
                    pop3Client = new Pop3Client();
                    pop3Client.Connect(config.Pop3, config.Port, true);
                    pop3Client.Authenticate(config.PersonalMail, config.PersonalPassword, AuthenticationMethod.TryBoth);
                    Session["Pop3Client"] = pop3Client;
                }
                else
                {
                    pop3Client = (Pop3Client)Session["Pop3Client"];
                }
                var count = pop3Client.GetMessageCount();
                Emails = new List<Email>();
                var counter = 0;
                var m = 1;
                for (var i = count; i >= 1; i--, m++)
                {
                    try
                    {
                        var message = pop3Client.GetMessage(i);
                        var email = new Email()
                        {
                            MessageNumber = m,
                            Subject = message.Headers.Subject,
                            DateSent = message.Headers.DateSent,
                            From =
                                string.Format("<a href = 'mailto:{1}'>{0}</a>", message.Headers.From.DisplayName,
                                    message.Headers.From.Address),
                            Address = message.Headers.From.Address
                        };
                        var body = message.FindFirstHtmlVersion();
                        if (body != null)
                        {
                            email.Body = body.GetBodyAsText();
                            
                            
                        }
                        else
                        {
                            body = message.FindFirstPlainTextVersion();
                            if (body != null)
                            {
                                email.Body = body.GetBodyAsText();
                                
                            }
                        }
                        
                        var attachments = message.FindAllAttachments();

                        foreach (var attachment in attachments)
                        {
                            email.Attachments.Add(new Attachment
                            {
                                FileName = attachment.FileName,
                                ContentType = attachment.ContentType.MediaType,
                                Content = attachment.Body
                            });
                        }
                        Emails.Add(email);
                        counter++;
                        if (counter > count)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                
                gvEmails.DataSource = Emails;
                gvEmails.DataBind();
               
            }
            catch (Exception ex)
            {

            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmails.DataSource = Emails;
            gvEmails.PageIndex = e.NewPageIndex;
            gvEmails.DataBind();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            if (e.Row == null || gvEmails.DataKeys[e.Row.RowIndex] == null) return;
            var rptAttachments = (e.Row.FindControl("rptAttachments") as Repeater);
            var index = gvEmails.DataKeys[e.Row.RowIndex];
            if (index == null) return;
            var ema = Emails.FirstOrDefault(email => email.MessageNumber == Convert.ToInt32(index.Value));
            if (ema == null) return;
            var attachments = ema.Attachments;
            if (rptAttachments == null) return;
            rptAttachments.DataSource = attachments;
            rptAttachments.DataBind();
        }

        protected void Download(object sender, EventArgs e)
        {
            var lnkAttachment = (sender as LinkButton);
            if (lnkAttachment != null)
            {
                var row = (lnkAttachment.Parent.Parent.NamingContainer as GridViewRow);
                if (row == null || gvEmails.DataKeys[row.RowIndex] == null) return;
                var firstOrDefault = Emails.FirstOrDefault(email => email.MessageNumber == Convert.ToInt32(gvEmails.DataKeys[row.RowIndex].Value));
                if (firstOrDefault != null)
                {
                    var attachments = firstOrDefault.Attachments;
                    var attachment = attachments.FirstOrDefault(a => a.FileName == lnkAttachment.Text);
                    if (attachment != null)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=" + attachment.FileName);
                        Response.ContentType = attachment.ContentType;
                        Response.BinaryWrite(attachment.Content);
                    }
                }
            }
            Response.End();
        }
        protected void b_send_Click(object sender, EventArgs e)
        {
            //using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))

            //{
                //var ids = ToContainer.Value;
                //var idsArray = ids.Split(',');
                //var cfg = GetConfig();
                //if (!idsArray.Any())
                //{
                //    lblMessgae.InnerHtml = @"أدخل المرسل اليه";
                //    lblMessgae.Attributes.Add("class", "alert alert-danger");
                //    return;
                //}
                //var to1 = idsArray[0];
                //var to2 = string.Empty;
                //var to3 = string.Empty;
                //if (idsArray.Count() > 1) to2 = idsArray[1] ?? "";
                //if (idsArray.Count() > 2) to3 = idsArray[2] ?? "";
                //ClsEmail.SendPersonalEmail(to1, to2, to3, txtSubject.Text, tb_message.Value, true, cfg);
                //lblMessgae.InnerHtml = Tokens.MsgSent;
                //lblMessgae.Attributes.Add("class", "alert alert-success");
                //ClearForm();
            //}

            if (Session["User_ID"] == null)
            {
                Response.Redirect("~/Pages/default.aspx");
                
            }

            int userId = Convert.ToInt32(Session["User_ID"]);

            var to1 = txtTo.Text;
            var cfg = GetConfig();
            if (string.IsNullOrEmpty(txtTo.Text))
            {
                lblMessgae.InnerHtml = @"أدخل المرسل اليه";
                lblMessgae.Attributes.Add("class", "alert alert-danger");
                return;
            }
           
            var to2 =  "";
            var to3 =  "";
            //ClsEmail.SendPersonalEmail(to1, to2, to3, txtSubject.Text, tb_message.Value, true, cfg);
            ClsEmail.SendPersonalEmail(to1, to2, to3, txtSubject.Text, tb_message.Value, true, userId);

            lblMessgae.InnerHtml = Tokens.MsgSent;
            lblMessgae.Attributes.Add("class", "alert alert-success");
            ClearForm();
        }


        void ClearForm()
        {
            //ddl_users.SelectedIndex = 0;
            //txtTo.Text=
            tb_message.Value =
            txtSubject.Text = string.Empty;
            //ToContainer.Value = string.Empty;
        }

        protected void btnReplay_Click(object sender, EventArgs e)
        {

            string subject = "Re: " + mail_subject.Value.ToString(); ;
            string message = string.Empty;
            message = replay_message.Value;
            int mailnum = Convert.ToInt32(mail_num.Value);
            var mail = Emails.FirstOrDefault(email => email.MessageNumber == mailnum);
            string oldMessage = string.Empty;
            string oldDate = string.Empty;
            oldDate = old_date.Value;
            oldMessage = old_message.Value;
            if (Session["User_ID"] == null)
            {
                Response.Redirect("~/Pages/default.aspx");

            }
            var userId = Convert.ToInt32(Session["User_ID"]);

            ClsEmail.SendReplayEmail(mail.Address, oldMessage, oldDate, subject, message, true, userId);
            lblMessgae.InnerHtml = Tokens.MsgSent;
            lblMessgae.Attributes.Add("class", "alert alert-success");
            replay_message.Value = string.Empty;
        }


    }

    [Serializable]
    public class Email
    {
        public Email()
        {
            Attachments = new List<Attachment>();
        }
        public int MessageNumber { get; set; }
        public string From { get; set; }
        public string Address { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    [Serializable]
    public class Attachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }

}