using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class UsersSaveMovments :  CustomPage
    {
       

    private readonly IUserSaveRepository _userSave;

    public UsersSaveMovments()
    {
        _userSave=new UserSaveRepository();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        PopulateSaves();
    }

    void PopulateSaves()
    {
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(a => a.ID == userId);
            if (user == null) return;
            var savesList = new List<Save>();
            switch (user.GroupID)
            {
                case 1:
                    savesList = context.Saves.ToList();
                    break;
                case 4:
                    savesList =
                        context.Saves.Where(a => DataLevelClass.GetBranchAdminBranchIDs(userId).Contains(a.BranchId))
                            .ToList();
                        //context.Saves.Where(a => a.BranchId == user.BranchID).ToList();
                    break;
                    /*default:
                        var savlist = _userSave.SavesOfUser(userId, context);
                        break;*/

            }
            if (savesList.Count > 0){
                ddlSaves.DataSource = savesList.Select(a => new
                {
                    a.SaveName,
                    a.Id
                });
                ddlSaves.DataBind();
                Helper.AddDefaultItem(ddlSaves);
            }else{
                ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
                {
                    a.Save.SaveName,
                    a.Save.Id
                }).ToList();
                ddlSaves.DataBind();
                Helper.AddDefaultItem(ddlSaves);
            }
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
            var history = context.UserSavesHistories.Where(a => a.SaveId == saveId &&
                                                                a.Time.Value.Date >=
                                                                Convert.ToDateTime(txtFrom.Text).Date &&
                                                                a.Time.Value.Date <= Convert.ToDateTime(txtTo.Text)).Select(a=>new
                                                                {
                                                                    a.Time,
                                                                    a.Save.SaveName,
                                                                    Amount=Helper.FixNumberFormat(a.amount),
                                                                    a.User.UserName,
                                                                    a.Notes,
                                                                    a.Notes2,
                                                                    Total=Helper.FixNumberFormat(a.Save.Total),
                                                                })
                .ToList();
            gv_Results.DataSource = history;
            gv_Results.DataBind();
            lblTotal.Text =history.Count>0&&history.FirstOrDefault()!=null? history.FirstOrDefault().Total:"0";
            lblPeriodTotal.Text = (history.Count > 0 && history.FirstOrDefault() != null ? history.Where(x => x.Amount != null).Sum(a => Convert.ToDecimal(a.Amount)) :
            0).ToString();
        }
    }
}
}