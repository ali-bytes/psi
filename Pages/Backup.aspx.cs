using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Ionic.Zip;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class Backup : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LMessage.Text = string.Empty;
            FetchBackups();
        }

        private void FetchBackups()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var backUps = context.Backups.OrderByDescending(x => x.Id).Select(x => new
                {
                    x.Id,
                    x.Time,
                    x.Url
                }).ToList();
                gv_index.DataSource = backUps;
                gv_index.DataBind();
            }
        }

        protected void BBackup_OnClick(object sender, EventArgs e)
        {
            var backup = new Domain.Backup();

            LMessage.Text = backup.StartBackup();
            FetchBackups();
        }


        protected void delBtu_Click(object sender, EventArgs e)
        {
            LMessage.Text = string.Empty;
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var id = Convert.ToInt32(button.CommandArgument);
                        var item = context.Backups.FirstOrDefault(x=>x.Id==id);
                        if (item != null)
                        {
                            context.Backups.DeleteOnSubmit(item);
                            context.SubmitChanges();
                            string destination = Server.MapPath("~/DbBackup");
                            if (destination != null && !Directory.Exists(destination))
                            {
                                LMessage.Text = "الملف غير موجود";
                                return;
                            }
                            string completePath = Server.MapPath("~/DbBackup/" + id + ".BAK");
                            string zipcompletePath = Server.MapPath("~/DbBackup/" + id + ".zip");
                            if (File.Exists(completePath))
                            {
                                File.Delete(completePath);
                            }
                            if (File.Exists(zipcompletePath))
                            {
                                File.Delete(zipcompletePath);
                            }
                        }
                    }
                }
                LMessage.Text = "تم الحذف";
                FetchBackups();
            }
            catch (Exception)
            {
                FetchBackups();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LMessage.Text = string.Empty;
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    var id = Convert.ToInt32(button.CommandArgument);
                    string completePath = Server.MapPath("~/DbBackup/" + id + ".zip");

                    if (File.Exists(completePath))
                    {
                        
                    }else
                    {
                        using (var zip = new ZipFile(Server.MapPath(string.Format("~/DbBackup/{0}.zip", id))))
                        {
                            zip.Encryption = EncryptionAlgorithm.None;
                            var fileName = Server.MapPath(string.Format("~/DbBackup/{0}.Bak", id));
                            zip.AddFile(fileName, string.Empty);
                            zip.Save();
                        }
                    }
                   
                    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var user = context.Users.FirstOrDefault(x => x.ID == 1);
                        if (user == null || user.UserEmail == null)
                        {
                            return;
                        }
                       
                        var fPath = string.Format("~/DbBackup/{0}.zip", id);
                        var bckup = context.Backups.FirstOrDefault(x => x.Id == id);
                        if (bckup!=null)
                        {
                            ClsEmail.SendEmailWithAttachment(user.UserEmail, "Smart Isp Backup", "نسخة احتياطية من قاعدة البيانات بتاريخ" + bckup.Time, fPath, true);
                        }

                    }
                }
                LMessage.Text = "تم الارسال";
                FetchBackups();
            }
            catch (Exception)
            {
                FetchBackups();
            }
        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
             var button = sender as Button;
            if (button != null)
            {
                var id = Convert.ToInt32(button.CommandArgument);

                #region Response
                HttpResponse Response = HttpContext.Current.Response;
                Response.Clear();
                Response.BufferOutput = false;
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "inline; filename=\"" + id + "\".zip");
                #endregion

                if (File.Exists(Server.MapPath(string.Format("~/DbBackup/{0}.zip", id))))
                {
                    using (var zip = new ZipFile(Server.MapPath(string.Format("~/DbBackup/{0}.zip", id))))
                    {
                        zip.Encryption = EncryptionAlgorithm.None;
                        var fileName = Server.MapPath(string.Format("~/DbBackup/{0}.Bak", id));

                        zip.Save(Response.OutputStream);

                    }
                }
                else
                {
                    using (var zip = new ZipFile(Server.MapPath(string.Format("~/DbBackup/{0}.zip", id))))
                    {
                        zip.Encryption = EncryptionAlgorithm.None;
                        var fileName = Server.MapPath(string.Format("~/DbBackup/{0}.Bak", id));
                        zip.AddFile(fileName, string.Empty);
                        zip.Save(Response.OutputStream);
                    }
                }
                Response.End();
            }
        }


        protected void btnrestore_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var id = Convert.ToInt32(button.CommandArgument);
                var backup = new Domain.Backup();

                LMessage.Text = backup.StartRestore(id.ToString());

            }
        }
    }
}