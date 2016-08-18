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
using System.Web.Services;

namespace NewIspNL.Pages
{
    public partial class SentMail : CustomPage
    {

    public List<Mydata> AllMessages;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        GetValue(Convert.ToInt32(Session["User_ID"]));
       
    }

   
    void GetValue(int userId)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            AllMessages = context.Messages.Where(m => m.MessageFrom == userId)
                .OrderByDescending(m => m.Time).
                Select(m =>
                    new Mydata
                    {
                        DoneRead = Convert.ToBoolean(m.DoneRead),
                        Id = m.Id,
                        Message = m.Message1,
                        From = m.User.UserName, 
                        Subject = m.Subject,
                        Time = m.Time.ToShortTimeString(),
                        Date = m.Time.Date.ToShortDateString(),
                        CssClass = Convert.ToBoolean(m.DoneRead) ? "l" : "message-unread",
                        CssAttachment =
                            (m.Attachments == null || m.Attachments == string.Empty)
                                ? ""
                                : "<i class='icon-paper-clip'></i>"
                    }).ToList();
            var inbox = AllMessages.Where(x => !Convert.ToBoolean(x.DoneRead)).ToList();//context.Messages.Where(x => x.MessageTo == userId & !Convert.ToBoolean(x.DoneRead)).ToList();
            lblInboxcount.Text = inbox.Count.ToString(CultureInfo.InvariantCulture);
            lbltotal.Text = AllMessages.Count.ToString(CultureInfo.InvariantCulture);

        }
    }
    public class Mydata
    {
        public bool DoneRead;
        public int Id;
        public string Message;
        public string From;
        public string Subject;
        public string Time;
        public string Date;
        public string CssClass;
        public string CssAttachment;
    }

        public string name;
        public int mesid;
         [WebMethod]
        public static string Directclick(string mid)
    {

      
    

       
        var id = QueryStringSecurity.Encrypt(mid);
        return id;


        }

    


    protected void SaveRead_OnServerClick(object sender, EventArgs e)
    {
       
    }
}
}