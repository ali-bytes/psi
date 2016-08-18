using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class SavesBalance : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateSaves();
        }
        private void PopulateSaves()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                int userId = Convert.ToInt32(Session["User_ID"]);
                var user = context.Users.FirstOrDefault(x => x.ID == userId);
                List<SavesList> savesList = new List<SavesList>();

                if (user != null && user.GroupID == 1)
                {
                    var saves = context.Saves.ToList();
                    foreach (var us in saves)
                    {
                        SavesList save = new SavesList();

                        if (us != null)
                        {
                            save.SaveName = us.SaveName;
                            save.Total = us.Total;
                            savesList.Add(save);
                        }

                    }
                }
                else
                {
                    var userSaves = context.UserSaves.Where(x => x.UserId == userId).ToList();
                    foreach (var us in userSaves)
                    {
                        SavesList save = new SavesList();
                        var saves = context.Saves.FirstOrDefault(a => a.Id == us.SaveId);
                        if (saves != null)
                        {
                            save.SaveName = saves.SaveName;
                            save.Total = saves.Total;
                            savesList.Add(save);
                        }
                    }
                }
                var grangataotal = savesList.Sum(x => x.Total);
                total.InnerHtml += grangataotal.ToString() ?? "";
                GvSaves.DataSource = savesList;
                GvSaves.DataBind();
            }
        }
        protected void GvBox_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvSaves, "LNo");
        }
    }

    public class SavesList
    {
        public string SaveName { get; set; }
        public decimal? Total { get; set; }
    }
}