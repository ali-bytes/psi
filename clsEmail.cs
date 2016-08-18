using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Db;
using NewIspNL.Domain.Concrete;

namespace NewIspNL
{
    public static class ClsEmail{
        static readonly EmailCnfgRepository CnfgRepository = new EmailCnfgRepository();


        public static void SendEmail(string strTo, string strSubject, string strMessage, bool isHtml){
            var cfg = CnfgRepository.GetActiveCnfg();
            if(cfg == null) return;

            var msg = new MailMessage{
                From = new MailAddress(cfg.MailFrom,"Smart ISP System")
            };
        
            msg.To.Add(strTo);
            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = strMessage;
            var smtp = new SmtpClient(cfg.SmtpClient){
                Credentials = new NetworkCredential(cfg.UserName, cfg.Password),
                EnableSsl = cfg.SSL,
            };
            if(cfg.Port != null) smtp.Port = Convert.ToInt32(cfg.Port);
            smtp.Send(msg);
            msg.Dispose();
        }

        public static void SendEmail(string strTo, string cc, string cc2, string strSubject, string strMessage, bool isHtml,string cc3=""){
            var cfg = CnfgRepository.GetActiveCnfg();
            if(cfg == null) return;
            var msg = new MailMessage{
                From = new MailAddress(cfg.MailFrom,"Smart ISP System")
            };
            msg.To.Add(strTo);
            msg.CC.Add(new MailAddress(cc));
            msg.CC.Add(new MailAddress(cc2));
            if (!string.IsNullOrEmpty(cc3)) msg.CC.Add(new MailAddress(cc3));
            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = strMessage;
            var smtp = new SmtpClient(cfg.SmtpClient){
                EnableSsl = cfg.SSL,
                Credentials = new NetworkCredential(cfg.UserName, cfg.Password),
            };
            if(cfg.Port != null) smtp.Port = Convert.ToInt32(cfg.Port);
            smtp.Send(msg);
            msg.Dispose();
        }
        public static void SendPersonalEmail(string strTo, string cc, string cc2, string strSubject, string strMessage, bool isHtml, int UserID)
        {

            //var cfg = confg;//CnfgRepository.GetActiveCnfg();
            //if (cfg == null) return;
            //var msg = new MailMessage
            //{
            //    From = new MailAddress(cfg.PersonalMail, cfg.User.UserName)
            //};
            //msg.To.Add(strTo);
            //if (!string.IsNullOrEmpty(cc)) msg.CC.Add(new MailAddress(cc));
            //if (!string.IsNullOrEmpty(cc2)) msg.CC.Add(new MailAddress(cc2));
            //msg.Subject = strSubject;
            //msg.IsBodyHtml = isHtml;
            //msg.Body = strMessage;
            //var smtp = new SmtpClient(cfg.SmtpClient)
            //{
            //    EnableSsl = cfg.SSL,
            //    Credentials = new NetworkCredential(cfg.SenderUserName, cfg.SenderPassWord),
            //};
            //if (cfg.Port != 0) smtp.Port = Convert.ToInt32(cfg.Port);
            //smtp.Send(msg);
            //msg.Dispose();

            //var cfg = confg;//CnfgRepository.GetActiveCnfg();
            //if (cfg == null) return;
            //PersonalEmail confg = new PersonalEmail();
            //var personalemail = "";


            PersonalEmail cfg = new PersonalEmail();
            var userName = string.Empty;
            var pesonalEmail = string.Empty;
            var smtpclint = string.Empty;
            bool enableSSL;
            var senderUserName = string.Empty;
            var senderPassWord = string.Empty;
            var sendPort = 0;


            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {


                cfg = context.PersonalEmails.FirstOrDefault(a => a.UserId == UserID);
                userName = cfg.User.UserName;
                pesonalEmail = cfg.PersonalMail;
                smtpclint = cfg.SmtpClient;
                enableSSL = cfg.SSL;
                senderUserName = cfg.SenderUserName;
                senderPassWord = cfg.SenderPassWord;
                sendPort = cfg.SendPort;
            }
            if (cfg == null) return;
            var msg = new MailMessage
            {
                From = new MailAddress(pesonalEmail, userName)
            };
            msg.To.Add(strTo);
            if (!string.IsNullOrEmpty(cc)) msg.CC.Add(new MailAddress(cc));
            if (!string.IsNullOrEmpty(cc2)) msg.CC.Add(new MailAddress(cc2));
            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = strMessage;
            
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = smtpclint;
            if (enableSSL)
            {
                smtp.EnableSsl = true;
            }
            else { smtp.EnableSsl = false; }
            smtp.Credentials = new NetworkCredential(senderUserName, senderPassWord);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (cfg.Port != 0) smtp.Port = Convert.ToInt32(sendPort);
            smtp.Send(msg);
            msg.Dispose();
             


           
        }
        public static void SendReplayEmail(string strTo, string oldMessage, string olddate, string strSubject, string strMessage, bool isHtml, int UserID)
        {
            PersonalEmail cfg = new PersonalEmail();
            var userName = string.Empty;
            var pesonalEmail = string.Empty;
            var smtpclint = string.Empty;
            bool enableSSL;
            var senderUserName = string.Empty;
            var senderPassWord = string.Empty;
            var sendPort = 0;
            StringBuilder newMessage = new StringBuilder("<b>" + strMessage + "</b>");
            newMessage.AppendLine("<hr>");
            newMessage.AppendLine(olddate + Environment.NewLine);
            newMessage.AppendLine(oldMessage);

            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {


                cfg = context.PersonalEmails.FirstOrDefault(a => a.UserId == UserID);
                userName = cfg.User.UserName;
                pesonalEmail = cfg.PersonalMail;
                smtpclint = cfg.SmtpClient;
                enableSSL = cfg.SSL;
                senderUserName = cfg.SenderUserName;
                senderPassWord = cfg.SenderPassWord;
                sendPort = cfg.SendPort;
            }
            if (cfg == null) return;
            var msg = new MailMessage
            {
                From = new MailAddress(pesonalEmail, userName)
            };
            msg.To.Add(strTo);

            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = newMessage.ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = smtpclint;
            if (enableSSL)
            {
                smtp.EnableSsl = true;
            }
            else { smtp.EnableSsl = false; }
            smtp.Credentials = new NetworkCredential(senderUserName, senderPassWord);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (cfg.Port != 0) smtp.Port = Convert.ToInt32(sendPort);
            smtp.Send(msg);
            msg.Dispose();
        }
        public static void SendEmailWithAttachment(string strTo, string strSubject, string strMessage,string attachPath, bool isHtml)
        {
            var cfg = CnfgRepository.GetActiveCnfg();
            if (cfg == null) return;

            var msg = new MailMessage
            {
                From = new MailAddress(cfg.MailFrom, "Smart ISP System")
            };

            var attachFile = new Attachment(HttpContext.Current.Server.MapPath(attachPath));

            msg.To.Add(strTo);
            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = strMessage;
            msg.Attachments.Add(attachFile);
            var smtp = new SmtpClient(cfg.SmtpClient)
            {
                Credentials = new NetworkCredential(cfg.UserName, cfg.Password),
                EnableSsl = cfg.SSL,
            };
            if (cfg.Port != null) smtp.Port = Convert.ToInt32(cfg.Port);
            smtp.Send(msg);
            msg.Dispose();
        }
        public static string Body( string message)
        {
            //<h3 style='text-align: center'>Smart ISP System</h3>
            var body = new StringBuilder();
            body.Append(
                "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
                "<html xmlns='http://www.w3.org/1999/xhtml'> <head>" +
                "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />" +
                "<title></title>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0'/>" +
                "</head><body>");
            body.Append("<table width='100%'><tr><td align='center' bgcolor='#6fb3e0' style='padding: 40px 0 30px 0;'>" +
                        " <img src='http://pit-egypt.com/fordownloads/logo.png' alt='logo'/></td></tr><tr><td bgcolor='#ffffff' style='padding: 40px 30px 40px 30px;text-align: right;'>" +
                        message +
                        "</td></tr><tr><td bgcolor='#438eb9' style='padding: 20px'><table width='100%'><tr><td>" +
                        "<p style='text-decoration: none;color: white;'>Copy Rights &copy; Pioneers 2015</p>" +
                        "</td><td style='text-align: right;color: white'>" +
                        "<a href='https://www.facebook.com/smartisp' style='background: transparent url(http://www.pioneers-solutions.com/4download/facebook.png) scroll -60px 0 no-repeat;display: block;width: 24px;height: 24px;float: right' title='facebook'><img width=' 24px' height= '24px' src='http://www.pioneers-solutions.com/4download/facebook.png' alt='logo'/></a>Contact Us at &nbsp;" +
                        "</td></tr></table></td></tr></table>");
            body.Append("</body></html>");
            return body.ToString();
        }


       
    }
}


