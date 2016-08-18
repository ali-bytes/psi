using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using Microsoft.Ajax.Utilities;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class AdminSavesMovements : CustomPage
    {
       
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                Bind_ddl_Branchs();
                PopulateSaves();
                PopulateEmployee();
            }
            void Bind_ddl_Branchs()
            {
                ddl_Branchs.DataSource = DataLevelClass.GetUserBranches();
                ddl_Branchs.DataBind();
                Helper.AddDefaultItem(ddl_Branchs);
                //ddl_Branchs.SelectedValue = "-1";
            }
            void PopulateSaves()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var allSaves = context.Saves.ToList();
                    ddlSaves.DataSource = allSaves;
                    ddlSaves.DataBind();
                    Helper.AddDefaultItem(ddlSaves);
                }
            }

            void PopulateEmployee()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var alluser = context.Users.Where(a => a.GroupID != 6).ToList();
                    ddlEmployee.DataSource = alluser;
                    ddlEmployee.DataBind();
                    Helper.AddDefaultItem(ddlEmployee);
                }
            }

            protected void btnSearch_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var allhistory = context.UserSavesHistories.ToList();
                    if (ddlSaves.SelectedIndex != 0)
                    {
                        allhistory = allhistory.Where(a => a.SaveId == Convert.ToInt32(ddlSaves.SelectedItem.Value)).ToList();
                    }
                    if (ddl_Branchs.SelectedIndex != 0)
                    {
                        var branchSavesId = context.Saves.Where(x => x.BranchId == Convert.ToInt32(ddl_Branchs.SelectedItem.Value)).Select(a => a.Id).ToList();
                        allhistory = allhistory.Where(a => branchSavesId.Contains(a.SaveId)).ToList();
                    }

                    if (ddlEmployee.SelectedIndex != 0)
                    {
                        allhistory = allhistory.Where(a => a.ConfirmerUserId == Convert.ToInt32(ddlEmployee.SelectedItem.Value)).ToList();
                    }
                    if (!string.IsNullOrEmpty(txtFrom.Text))
                    {
                        allhistory = allhistory.Where(a => a.Time.Value.Date >= Convert.ToDateTime(txtFrom.Text).Date).ToList();
                    }
                    if (!string.IsNullOrEmpty(txtTo.Text))
                    {
                        allhistory = allhistory.Where(a => a.Time.Value.Date <= Convert.ToDateTime(txtTo.Text).Date).ToList();
                    }
                    gv_Results.DataSource = allhistory.Select(a => new
                    {
                        a.Id,
                        a.Save.SaveName,
                        a.User.UserName,
                        a.Notes,
                        a.Notes2,
                        Amount = Helper.FixNumberFormat(a.amount),
                        a.Time
                    });
                    gv_Results.DataBind();
                    var list1 = allhistory.DistinctBy(a => a.SaveId).ToList();
                    var tot = list1.Sum(a => a.Save.Total);
                    lblTotal.Text = allhistory.Count > 0 && allhistory.FirstOrDefault() != null ? tot.ToString() : "0";
                    lblPeriodTotal.Text = (allhistory.Count > 0 && allhistory.FirstOrDefault() != null ? allhistory.Where(x => x.amount != null).Sum(a => a.amount) :0).ToString();
                }
            }
        }
    }
 