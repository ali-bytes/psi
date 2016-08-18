using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using System.Web.Services;

namespace NewIspNL.Pages
{
    public partial class Inbox : CustomPage
    {
            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["User_ID"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                if (IsPostBack) return;
                GetValue(Convert.ToInt32(Session["User_ID"]));
        
            }
            protected void dtlCat_ItemCommand(object source, DataListCommandEventArgs e)
            {
                if (e.CommandName == "View")
                {
                    // Here "i" will get the imageid(CommandArgument='<%# Eval("ImageID") %>') that was given inside the image button tag
                    string i = e.CommandArgument.ToString();
                    var id = QueryStringSecurity.Encrypt(i);
                    Response.Redirect("MessageReader.aspx?Messid=" + id);


                }



            }
       

            void GetValue(int userId)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var allMessages = context.Messages.Where(m => m.MessageTo == userId)
                            .OrderByDescending(m => m.Time).
                            Select(m =>
                                new //Mydata
                                {
                                    DoneRead = Convert.ToBoolean(m.DoneRead),
                                    Id = m.Id,
                                    Message = m.Message1,
                                    From = context.Users.FirstOrDefault(u => u.ID == m.MessageFrom).UserName,
                                    Subject = m.Subject,
                                    Time = m.Time.ToShortTimeString(),
                                    Date = m.Time.Date.ToShortDateString(),
                                    CssClass = Convert.ToBoolean(m.DoneRead) ? "l" : "message-unread",
                                    CssAttachment = (m.Attachments == null || m.Attachments == string.Empty) ? "" : "<i class='icon-paper-clip'></i>"
                                }).ToList();
                    var inbox = allMessages.Where(x => !Convert.ToBoolean(x.DoneRead)).ToList();//context.Messages.Where(x => x.MessageTo == userId & !Convert.ToBoolean(x.DoneRead)).ToList();
                    lblInboxcount.Text = inbox.Count.ToString(CultureInfo.InvariantCulture);
                    lbltotal.Text = allMessages.Count.ToString(CultureInfo.InvariantCulture);
                   
                    DataList1.DataSource = allMessages;
                    DataList1.DataBind();
                }
            }

            protected void SaveRead_OnServerClick(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    foreach (DataListItem item in DataList1.Items)
                    {
                        var btn = item.FindControl("HyperLink1") as HtmlAnchor;
                        var cb = item.FindControl("readIt") as CheckBox;
                        if (btn == null || cb == null) continue;
                        var id = Convert.ToInt32(btn.Style.Value);
                        var msg = context.Messages.FirstOrDefault(m => m.Id == id);
                        if (msg == null) continue;
                        if (cb.Checked) msg.DoneRead = cb.Checked;
                        context.SubmitChanges();
                    }

                    GetValue(Convert.ToInt32(Session["User_ID"]));
                }
            }
            protected void UnRead_OnServerClick(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    foreach (DataListItem item in DataList1.Items)
                    {
                        var btn = item.FindControl("HyperLink1") as HtmlAnchor;
                        var cb = item.FindControl("readIt") as CheckBox;
                        if (btn == null || cb == null) continue;
                        var id = Convert.ToInt32(btn.Style.Value);
                        var msg = context.Messages.FirstOrDefault(m => m.Id == id);
                        if (msg == null) continue;
                        if (cb.Checked) msg.DoneRead = !(cb.Checked);
                        context.SubmitChanges();
                    }

                    GetValue(Convert.ToInt32(Session["User_ID"]));
                }
            }
            protected void DeleteMsg_OnServerClick(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    foreach (DataListItem item in DataList1.Items)
                    {
                        var btn = item.FindControl("HyperLink1") as HtmlAnchor;
                        var cb = item.FindControl("readIt") as CheckBox;
                        if (btn == null || cb == null) continue;
                        var id = Convert.ToInt32(btn.Style.Value);
                        var msg = context.Messages.FirstOrDefault(m => m.Id == id);
                        if (msg == null) continue;
                        if (cb.Checked) context.Messages.DeleteOnSubmit(msg);
                        //msg.DoneRead = cb.Checked;
                        context.SubmitChanges();
                    }

                    GetValue(Convert.ToInt32(Session["User_ID"]));
                }
            }



            [WebMethod]
            public static string Directclick(string mid)
            {
                var id = QueryStringSecurity.Encrypt(mid);
                return id;
            }

    }
    }
 