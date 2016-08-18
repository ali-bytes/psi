using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class UpdatedRBSPrivilages : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateGroups();
        }




        void PopulateGroups()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var query = context.Groups.Select(grp => grp);
                ddlGroups.DataSource = query;
                ddlGroups.DataBind();
                Helper.AddDefaultItem(ddlGroups);
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var gid = Helper.GetDropValue(ddlGroups);
                var check = context.UpdatedResellerBSPrivilages.Where(a => a.GroupId == gid);
                if (!check.Any())
                {
                    AddPrivilages(gid);
                    lblMsg.Text = Tokens.Saved;
                    Clear();
                }
                else
                {
                    DeletePrivilage(gid);
                    AddPrivilages(gid);
                    lblMsg.Text = Tokens.Saved;
                    Clear();
                }
            }
        }


        void DeletePrivilage(int? groupId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (groupId != null)
                {
                    var allexistes = context.UpdatedResellerBSPrivilages.Where(a => a.GroupId == groupId).ToList();
                    foreach (var thi in allexistes)
                    {
                        context.UpdatedResellerBSPrivilages.DeleteOnSubmit(thi);
                    }
                    context.SubmitChanges();
                }
            }
        }


        void AddPrivilages(int? groupid)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (groupid != null)
                {
                    foreach (ListItem item in CheckPrivilages.Items)
                    {
                        if (item.Selected)
                        {
                            var txt = Convert.ToInt32(item.Value);
                            var text = string.Empty;
                            switch (txt)
                            {
                                case 0:
                                    text = "تعديل";
                                    break;
                                case 1:
                                    text = "حذف";
                                    break;
                                case 2:
                                    text = "طباعه";
                                    break;
                                case 3:
                                    text = "اضافة فواتير الموزع";
                                    break;
                                case 4:
                                    text = "اضافة مدفوعات الموزع";
                                    break;
                            }
                            var add = new UpdatedResellerBSPrivilage
                            {
                                GroupId = groupid,
                                ItemName = text//item.Text
                            };
                            context.UpdatedResellerBSPrivilages.InsertOnSubmit(add);
                            context.SubmitChanges();
                        }
                    }
                }
            }
        }


        void Clear()
        {
            //ddlGroups.SelectedIndex = 0;
            foreach (ListItem item in CheckPrivilages.Items)
            {
                item.Selected = false;
            }
        }


        void PopulatePrivilages(int? groupid)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                Clear();
                if (groupid != null)
                {
                    var getPriv = context.UpdatedResellerBSPrivilages.Where(a => a.GroupId == groupid).Select(x => new
                    {
                        x.ItemName,
                        x.Group.GroupName
                    }).ToList();
                    if (getPriv.Count != 0)
                    {
                        foreach (var item in getPriv)
                        {
                            if (item.ItemName == "تعديل")
                                CheckPrivilages.Items[0].Selected = true;
                            if (item.ItemName == "حذف")
                                CheckPrivilages.Items[1].Selected = true;
                            if (item.ItemName == "طباعه")
                                CheckPrivilages.Items[2].Selected = true;
                            if (item.ItemName == "اضافة فواتير الموزع")
                                CheckPrivilages.Items[3].Selected = true;
                            if (item.ItemName == "اضافة مدفوعات الموزع")
                                CheckPrivilages.Items[4].Selected = true;
                        }
                    }
                }
            }
        }


        protected void ddlGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gid = Helper.GetDropValue(ddlGroups);
            PopulatePrivilages(gid);
        }
    }
}