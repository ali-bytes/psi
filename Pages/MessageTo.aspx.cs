using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class MessageTo : CustomPage
    {
      
    readonly IspEntries _ispEntries = new IspEntries();


    protected void Page_Load(object sender, EventArgs e){
        Page.Header.DataBind();  
        if(IsPostBack) return;
        if(Session["User_ID"] == null)
            Response.Redirect("defualt.aspx");
        PopulateUsers();
    }

    void PopulateUsers(){
        var userid =Convert.ToInt32(Session["User_ID"]);
        var orignalUsers = _ispEntries.Users().Where(a =>!Convert.ToBoolean(a.IsAccountStopped)).ToList();
        var use = orignalUsers.FirstOrDefault(a => a.ID == userid);
        if(use==null) return;
        if(use.GroupID == 6){
            var userwithoutResellers = orignalUsers.Where(s => s.GroupID != 6 && s.BranchID == use.BranchID);
            ddl_users.DataSource = userwithoutResellers;
            ddl_users.DataTextField = "UserName";
            ddl_users.DataValueField = "ID";
            ddl_users.DataBind();
            Helper.AddDefaultItem(ddl_users);
        }
        if (use.GroupID == 1)
        {
            var users = orignalUsers;
            ddl_users.DataSource = users;
            ddl_users.AppendDataBoundItems = true;
            ddl_users.Items.Add("All");
            ddl_users.DataTextField = "UserName";
            ddl_users.DataValueField = "ID";
            ddl_users.DataBind();
            Helper.AddDefaultItem(ddl_users);
        } 
        else if (use.GroupID == 4)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var adminbarnch = DataLevelClass.GetBranchAdminBranchIDs(userid);

                    
                var branathor = context.Users.Where(a => (!adminbarnch.Contains(a.BranchID) )&&a.GroupID==6).Select(z => new
                {
                    z.UserName,
                    z.ID
                


                }).ToList();
                var allusers = context.Users.Select(z => new
                {
                    z.UserName,
                    z.ID



                }).ToList();
                   
                var users = allusers.Except(branathor);

                ddl_users.DataSource = users;
                ddl_users.DataTextField = "UserName";
                ddl_users.DataValueField = "ID";
                ddl_users.DataBind();
                Helper.AddDefaultItem(ddl_users);
            }
        }
        else if (use.GroupID != 6)
        {
            var users = orignalUsers;
            ddl_users.DataSource = users;
            ddl_users.DataTextField = "UserName";
            ddl_users.DataValueField = "ID";
            ddl_users.DataBind();
            Helper.AddDefaultItem(ddl_users);
        }
      

    }


    protected void b_send_Click(object sender, EventArgs e){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){

            if (ToContainer.Value == "") { return;}

            if (Editor1.Content == "")
            {
                 Label4.Visible = true;


            }
            else { 
            var ids = ToContainer.Value;
            var idsArray = ids.Split(',');
            var messages = new List<Message>();
            var allusers = dataContext.Users.ToList();
            string filename = string.Empty, filename2 = string.Empty, filename3 = string.Empty;// = string.Empty;

            if (!string.IsNullOrEmpty(UploadAttachFile.FileName))
            {
                string ex = Path.GetExtension(UploadAttachFile.FileName).ToLower();
                if (ex != ".jpeg" && ex != ".png" && ex != ".jpg" && ex != ".gif" && ex != ".pdf" && ex != ".doc" && ex != ".docx" && ex != ".xls" && ex != ".xlsx")
                {
                    return;
                }
                filename = DateTime.Now.Millisecond + UploadAttachFile.FileName;
                UploadAttachFile.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename)));
            }
            if (!string.IsNullOrEmpty(FileUpload2.FileName))
            {
                string ex2 = Path.GetExtension(FileUpload2.FileName).ToLower();
                if (ex2 != ".jpeg" && ex2 != ".png" && ex2 != ".jpg" && ex2 != ".gif" && ex2 != ".pdf" && ex2 != ".doc" && ex2 != ".docx" && ex2 != ".xls" && ex2 != ".xlsx")
                {
                    return;
                }
                filename2 = DateTime.Now.Millisecond + FileUpload2.FileName;
                FileUpload2.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename2)));
            }
            if (!string.IsNullOrEmpty(FileUpload3.FileName))
            {
                string ex3 = Path.GetExtension(FileUpload3.FileName).ToLower();
                if (ex3 != ".jpeg" && ex3 != ".png" && ex3 != ".jpg" && ex3 != ".gif" && ex3 != ".pdf" && ex3 != ".doc" && ex3 != ".docx" && ex3 != ".xls" && ex3 != ".xlsx")
                {
                    return;
                }
                filename3 = DateTime.Now.Millisecond + FileUpload3.FileName;
                FileUpload3.SaveAs(Server.MapPath(string.Format("../Attachments/{0}", filename3)));
            }
            if (ddl_users.SelectedValue != "All")
            {
                for (int i = 0; i < idsArray.Length; i++)
                {
                    int userId = Convert.ToInt32(idsArray[i]);
                    var user = dataContext.Users.FirstOrDefault(u => u.ID == userId);
                    if (user == null) continue;
                    var message = new Message
                    {
                        MessageFrom = Convert.ToInt32(Session["User_ID"]),
                        MessageTo = userId,
                        Subject = tb_subject.Text,
                        Message1 = Editor1.Content,
                        Time = DateTime.Now.AddHours(),
                        DoneRead = false,
                        Attachments = filename,
                        Attachments2 = filename2,
                        Attachments3 = filename3
                    };
                    messages.Add(message);
                }
            }
            else
            {
                foreach (var user in allusers)
                {
                    int userId = user.ID;
                    var message = new Message
                    {
                        MessageFrom = Convert.ToInt32(Session["User_ID"]),
                        MessageTo = userId,
                        Subject = tb_subject.Text,
                        Message1 = Editor1.Content,
                        Time = DateTime.Now.AddHours(),
                        DoneRead = false,
                        Attachments = filename,
                        Attachments2 = filename2,
                        Attachments3 = filename3
                    };
                    messages.Add(message);
                }
            }
            dataContext.Messages.InsertAllOnSubmit(messages);
            dataContext.SubmitChanges();
            l_message.Text = Tokens.MsgSent;
            ClearForm();
            }
        }
    }


    void ClearForm(){
        ddl_users.SelectedIndex = 0;
        Editor1.Content = string.Empty;
        tb_subject.Text = string.Empty;
        ToContainer.Value = string.Empty;
    }
}
}