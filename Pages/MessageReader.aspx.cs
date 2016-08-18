using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using NewIspNL.Helpers;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class MessageReader : CustomPage
    {
     

    protected void Page_Load(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var meId = (Request.QueryString["id"]);

            //var que = Request.QueryString["Messid"];
            var id = QueryStringSecurity.Decrypt(meId);
           var messageId = id;

            if(string.IsNullOrEmpty(messageId)) Response.Redirect("Inbox.aspx");
            var message = context.Messages.FirstOrDefault(m => m.Id == Convert.ToInt32(messageId));
            if(message == null) Response.Redirect("Inbox.aspx");
            if(message == null) return;
          
            LiReplyTo.Text=l_from.Text = message.User1.UserName;
            l_body.Text = message.Message1;
            l_date.Text = message.Time.ToShortDateString();
           
            l_subject.Text = message.Subject;
       
            lblReplayBody.Text = @"<br/>______________________________________<br/><br/>" + message.Message1;

            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            if (user != null)
            {
                if (user.ID != message.MessageFrom)
                {
                    message.DoneRead = true;
                }
            }
           
           
            context.SubmitChanges();
            if (!string.IsNullOrEmpty(message.Attachments))
            {
                attachmentFiles1.Visible = true;
                attachment1.HRef = string.Format("../Attachments/{0}", message.Attachments);
            }
            if (!string.IsNullOrEmpty(message.Attachments2))
            {
                attachmentFiles2.Visible = true;
                attachment2.HRef = string.Format("../Attachments/{0}", message.Attachments2);
            }
            if (!string.IsNullOrEmpty(message.Attachments3))
            {
                attachmentFiles3.Visible = true;
                attachment3.HRef = string.Format("../Attachments/{0}", message.Attachments3);
            }
            if(!IsPostBack)
            { TbReplyTitle.Text = @"الرد على رسالة : " + message.Subject; }
        }
    }


    protected void BSend_OnClick(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            var messageId2 = (Request.QueryString["id"]);
            var messageId = QueryStringSecurity.Decrypt(messageId2);
            if(string.IsNullOrEmpty(messageId2)) Response.Redirect("Inbox.aspx");
            var receivedMessage = context.Messages.FirstOrDefault(m => m.Id == Convert.ToInt32(messageId));

            var extensions = new List<string> { ".JPG", ".GIF", ".JPEG", ".PNG", ".PDF",".DOC",".DOCX",".XLS",".XLSX" };
           
            string filename = string.Empty, filename2 = string.Empty, filename3 = string.Empty;// = string.Empty;
            if (!string.IsNullOrEmpty(UploadAttachFile.FileName))
            {
                string ex1 = Path.GetExtension(UploadAttachFile.FileName).ToUpper();
                if (!string.IsNullOrEmpty(ex1) && extensions.Any(currentExtention => currentExtention == ex1))
                {
                    filename = DateTime.Now.Millisecond + UploadAttachFile.FileName;
                    UploadAttachFile.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename)));
          
                }
            }
            if (!string.IsNullOrEmpty(FileUpload2.FileName))
            {
                string ex2 = Path.GetExtension(FileUpload2.FileName).ToUpper();
                 if (!string.IsNullOrEmpty(ex2) && extensions.Any(currentExtention => currentExtention == ex2))
                {
                    filename2 = DateTime.Now.Millisecond + FileUpload2.FileName;
                    FileUpload2.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename2)));
                }
            }
            if (!string.IsNullOrEmpty(FileUpload3.FileName))
            {
                string ex3 = Path.GetExtension(FileUpload3.FileName).ToUpper();
                 if (!string.IsNullOrEmpty(ex3) && extensions.Any(currentExtention => currentExtention == ex3))
                {
                    filename3 = DateTime.Now.Millisecond + FileUpload3.FileName;
                    FileUpload3.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename3)));
                }
            }





            if(user != null && receivedMessage != null){
                var message = new Message{
                    DoneRead = false,
                    Time = DateTime.Now.AddHours(),
                    MessageFrom = receivedMessage.MessageTo,
                    MessageTo = receivedMessage.MessageFrom,
                    Subject = TbReplyTitle.Text,
                    Message1 = Editor1.Content + lblReplayBody.Text,
                    Attachments = filename,
                        Attachments2 = filename2,
                        Attachments3 = filename3
                };
                context.Messages.InsertOnSubmit(message);
                context.SubmitChanges();
                LiMessage.Visible = true;
                LiMessage.InnerText = Tokens.MsgSent;
            }

        }
    }
}
}