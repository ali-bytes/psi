using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Services;

namespace NewIspNL.Pages
{
    public partial class ManageResellerRequests : CustomPage
    {
     
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            PopulateRequests(context);
            PopulateRequestPrivilig();
            //Bind_chl_Branchs();
            //BindSavesByBranches(context);
            Bind_ddl_Branches();
            Bind_ddl_Groups(context);
            var domain = new IspDomian(context);
            domain.PopulateGovernorates(ddlGovernorate);
            domain.PopulateUsersByDataLevel(ddlAccountManager);
        }
    }

    private void PopulateRequests(ISPDataContext context)
    {
        var requests = context.NewResellerRequests.Where(a => a.RequestStatuses == null).ToList();
        grd_Requests.DataSource = requests;
        grd_Requests.DataBind();
    }

    protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Helper.GridViewNumbering(grd_Requests, "lbl_No");
    }

    private void PopulateRequestPrivilig()
    {
        Helper.AddDefaultItem(ddlManageReuestPrivilege);
    }

    //private void Bind_chl_Branchs()
    //{
    //    chl_Branchs.DataSource = ddl_Branches.DataSource = DataLevelClass.GetUserBranches();
    //    chl_Branchs.DataBind();
    //}

    //private void BindSavesByBranches(ISPDataContext context)
    //{
    //    var beanches = DataLevelClass.GetUserBranches();
    //    var saves = new List<Save>();
    //    foreach (var branch in beanches)
    //    {
    //        var branch1 = branch;
    //        var save = context.Saves.Where(a => a.BranchId == branch1.ID).ToList();
    //        if (save.Count > 0) saves.AddRange(save);
    //    }
    //    chlSaves.DataSource = saves;
    //    chlSaves.DataBind();
    //}

    private void Bind_ddl_Branches()
    {
        ddl_Branches.SelectedValue = null;
        ddl_Branches.Items.Clear();

        ddl_Branches.AppendDataBoundItems = true;
        var userBranches = DataLevelClass.GetUserBranches();

        ddl_Branches.DataSource = userBranches;
        ddl_Branches.DataBind();

        Helper.AddDefaultItem(ddl_Branches);
    }



    private void Bind_ddl_Groups(ISPDataContext context)
    {
        if (Session["User_ID"] == null) Response.Redirect("../default.aspx");

        var user = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
        var accountManager = context.Users.Where(a => a.AccountmanagerId == user.ID).ToList();
        var groups = accountManager.Count == 0
            ? DataLevelClass.GetUserGroups()
            : context.Groups.Where(a => a.DataLevelID == 3).ToList();

        ddl_Groups.DataSource = groups;
        ddl_Groups.DataBind();
 
        Helper.AddDefaultItem(ddl_Groups);

    }

    protected void ddl_Groups_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (ddl_Groups.SelectedIndex != 0)
        {
            var groupId = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
            RowAccountManager.Visible = groupId == 6;
        }
        else
        {
            RowAccountManager.Visible = false;
        }
    }

    protected void ApproveRequest(object sender, EventArgs e)
    {
        var buttonId = hdf_ID.Value;
        if (string.IsNullOrWhiteSpace(buttonId)) return;
        var id = Convert.ToInt32(buttonId);
        if (id == 0) return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var request = context.NewResellerRequests.FirstOrDefault(a => a.Id == id);
            if (request != null)
            {
                request.RequestStatuses = true;
                var groupid = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
                var usr = new User
                {
                    UserName = request.ResellerName,
                    UserPhone = request.CompanyTelephone,
                    UserMobile = request.ResellerMobile,
                    UserAddress = request.CompanyAddress,
                    UserEmail = request.ResellerEmail,
    
                    LoginName = request.ResellerUsername,
                    LoginPassword = Security.EncodePassword(request.ResellerPassword),
                    GroupID = groupid,
                    IsAccountStopped = chb_IsAccountStopped.Checked,
                    GovernorateId = Convert.ToInt32(ddlGovernorate.SelectedItem.Value),
                    ManageRequestPrivilege = Convert.ToInt32(ddlManageReuestPrivilege.SelectedItem.Value)
                };


            
                if (ddl_Branches.SelectedItem.Value != "" && ddl_Branches.SelectedIndex != 0)
                    usr.BranchID = Convert.ToInt32(ddl_Branches.SelectedItem.Value);
                if (ddlAccountManager.SelectedIndex != -1 && RowAccountManager.Visible && groupid == 6)
                    usr.AccountmanagerId = Convert.ToInt32(ddlAccountManager.SelectedItem.Value);

                var chk = context.Users.FirstOrDefault(a => a.UserName == usr.UserName);

                if (chk!=null)
                {
                    hdfMsg.Value = "2";
                    return;
                }
                context.Users.InsertOnSubmit(usr);
                try
                {
                    context.SubmitChanges();
                    hdfMsg.Value = "1";
                }


                catch (Exception)
                {
                    hdfMsg.Value = "0";
                    return;
                }
                var cultureid = Convert.ToInt32(request.ResellerCulture);
                var culture = new CultureService();
                culture.UpdateUserCulture(cultureid, usr.ID);
                //foreach (ListItem li in chl_Branchs.Items)
                //{
                //    if (li.Selected)
                //    {
                //        var ub = new UserBranch
                //        {
                //            UserID = usr.ID,
                //            BranchID = Convert.ToInt32(li.Value)
                //        };
                //        context.UserBranches.InsertOnSubmit(ub);
                //        context.SubmitChanges();
                //    }
                //}
                //foreach (ListItem item in chlSaves.Items)
                //{
                //    if (item.Selected)
                //    {
                //        var usersave = new UserSave
                //        {
                //            UserId = usr.ID,
                //            SaveId = Convert.ToInt32(item.Value)
                //        };
                //        context.UserSaves.InsertOnSubmit(usersave);
                //        context.SubmitChanges();
                //    }
                //}
                PopulateRequests(context);
            }
        }
    }

    protected void RejectRequest(object sender, EventArgs e)
    {
        var buttonId = Convert.ToInt32(((LinkButton) sender).CommandArgument);
        if (buttonId == 0) return;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var reques = context.NewResellerRequests.FirstOrDefault(a => a.Id == buttonId);
            if(reques==null)return;
            reques.RequestStatuses = false;
            try
            {
                context.SubmitChanges();
                hdfMsg.Value = "1";
            }
            catch (Exception)
            {
                hdfMsg.Value = "0";
            }
            PopulateRequests(context);
        }
    }
}
}