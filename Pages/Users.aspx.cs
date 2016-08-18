using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Users : CustomPage
    {
       

    //readonly ISPDataContext _context;

    readonly IspDomian _domian;
    private readonly CultureService _culture;
    public Users()
    {
        var context = IspDataContext;
        _domian = new IspDomian(context);
        _culture=new CultureService();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Activate();
        if (IsPostBack) return;
        Bind_ddl_Branches();
        BindSavesByBranches();
        Bind_ddl_Groups();
        Bind_grd_users(false);
        Bind_chl_Branchs();
        BindDdlBranches();
        //BindProviders();
        _domian.PopulateGovernorates(ddlGovernorate);
        _domian.PopulateGovernorates(ddlfilterGovernrate);
        //_domian.PopulateUsersByDataLevel(ddlAccountManager);
        PopulateAccountManager();
        _domian.PopulateUsersByDataLevel(ddlfilterAccountManager);
        PopulateCultures();
        PopulateRequestPrivilig();
    }

    private void PopulateAccountManager()
    {
        var users = DataLevelClass.GetListUsersByDataLevel();
        ddlAccountManager.DataSource = users.Where(a => a.GroupId != 1 && a.GroupId != 4 && a.GroupId != 6 );
        ddlAccountManager.DataTextField = "UserName";
        ddlAccountManager.DataValueField = "ID";
        ddlAccountManager.DataBind();
        Helper.AddDefaultItem(ddlAccountManager);
    }
    private void PopulateCultures()
    {
        var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        var cultures = context.Cultures;
        ddlCulture.DataSource = cultures;
        ddlCulture.DataTextField = "Name";
        ddlCulture.DataValueField = "Id";
        ddlCulture.DataBind();
        Helper.AddDefaultItem(ddlCulture);
    }

    void PopulateRequestPrivilig()
    {
        Helper.AddDefaultItem(ddlManageReuestPrivilege);
    }
    void Bind_chl_Branchs()
    {
        chl_Branchs.DataSource = ddl_Branches.DataSource = DataLevelClass.GetUserBranches();
        chl_Branchs.DataBind();
    }

    void BindSavesByBranches()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var beanches = DataLevelClass.GetUserBranches();
            var saves = new List<Save>();
            foreach (var branch in beanches)
            {
                var branch1 = branch;
                var save = context.Saves.Where(a => a.BranchId == branch1.ID).ToList();
                if(save.Count>0)saves.AddRange(save);
            }
            chlSaves.DataSource = saves;
            chlSaves.DataBind();
        }
    }

    void BindProviders()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var providers = context.ServiceProviders.ToList();
            chl_Providers.DataSource = providers;
            chl_Providers.DataBind();
        }
    }

    void Bind_ddl_Branches()
    {
        ddl_Branches.SelectedValue = null;
        ddl_Branches.Items.Clear();

        ddl_Branches.AppendDataBoundItems = true;
        var userBranches = DataLevelClass.GetUserBranches();

        ddl_Branches.DataSource = userBranches;
        ddl_Branches.DataBind();

        Helper.AddDefaultItem(ddl_Branches);
    }


    void BindDdlBranches()
    {
        _domian.PopulateBranches(ddlBranch, true);
    }


    void Bind_ddl_Groups()
    {
        if(Session["User_ID"] == null) Response.Redirect("../default.aspx");
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var user = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
            var accountManager = context.Users.Where(a => a.AccountmanagerId == user.ID).ToList();
            var groups = accountManager.Count == 0 ? DataLevelClass.GetUserGroups() : context.Groups.Where(a => a.DataLevelID == 3).ToList();
            ddlGroup.DataSource = ddl_Groups.DataSource = groups;
            ddl_Groups.DataBind();
            ddlGroup.DataBind();
            Helper.AddDefaultItem(ddl_Groups);
            Helper.AddDefaultItem(ddlGroup);
        }
    }


    protected void btn_Add_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (context.Users.Any(x => x.UserName.ToLower().Equals(txt_UserName.Text.ToLower()) || x.LoginName.ToLower().Equals(txt_LoginName.Text.ToLower())))
            {
                lbl_result.Text = Tokens.UserNameAlreadyExist;
                lbl_result.ForeColor = Color.Red;
                return;
            }
            var groupid = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
            var usr = new User
            {
                UserName = txt_UserName.Text.Trim(),
                UserPhone = txt_UserPhone.Text.Trim(),
                UserMobile = txt_UserMobile.Text.Trim(),
                UserAddress = txt_UserAddress.Text.Trim(),
                UserEmail = txt_UserEmail.Text.Trim(),
                Ip = TbUserIp.Text,
                LoginName = txt_LoginName.Text.Trim(),
                LoginPassword = Security.EncodePassword(txt_LoginPassword.Text),
                GroupID = groupid,//Convert.ToInt32(ddl_Groups.SelectedItem.Value),
                IsAccountStopped = chb_IsAccountStopped.Checked,
                GovernorateId = Convert.ToInt32(ddlGovernorate.SelectedItem.Value),
                ManageRequestPrivilege = Convert.ToInt32(ddlManageReuestPrivilege.SelectedItem.Value)
            };
            
            
            if (!string.IsNullOrWhiteSpace(TbUserIp.Text))
            {
                usr.Ip = TbUserIp.Text;
            }
            if (ddl_Branches.SelectedItem.Value != "" && ddl_Branches.SelectedIndex != 0)
                usr.BranchID = Convert.ToInt32(ddl_Branches.SelectedItem.Value);
            if (ddlAccountManager.SelectedIndex != -1 && RowAccountManager.Visible && groupid == 6)
                usr.AccountmanagerId = Convert.ToInt32(ddlAccountManager.SelectedItem.Value);
            context.Users.InsertOnSubmit(usr);
            try
            {
                context.SubmitChanges();
            }
            

            catch (Exception)
            {
                lbl_result.Text = Tokens.UserNameAlreadyExist;
                lbl_result.ForeColor = Color.Red;
                return;
            }
            var cultureid = Convert.ToInt32(ddlCulture.SelectedItem.Value);
            _culture.UpdateUserCulture(cultureid,usr.ID);
            foreach (ListItem li in chl_Branchs.Items)
            {
                if (li.Selected)
                {
                    var ub = new UserBranch
                    {
                        UserID = usr.ID,
                        BranchID = Convert.ToInt32(li.Value)
                    };
                    context.UserBranches.InsertOnSubmit(ub);
                    context.SubmitChanges();
                }
            }
            foreach (ListItem item in chlSaves.Items)
            {
                if (!item.Selected) continue;
                var usersave = new UserSave
                {
                    UserId = usr.ID,
                    SaveId = Convert.ToInt32(item.Value)
                };
                context.UserSaves.InsertOnSubmit(usersave);
                context.SubmitChanges();
            }
            if (usr.GroupID == 6)
            {
                /*if (chl_Providers.Items.Count > 0)
                {
                    if (usr.UserProviders != null)
                    {
                        context.UserProviders.DeleteAllOnSubmit(usr.UserProviders);
                        context.SubmitChanges();
                    }
                }*/
                foreach (ListItem item in chl_Providers.Items)
                {
                    if (!item.Selected) continue;
                    var resellerProvider = new UserProvider
                    {
                        UserId = usr.ID,
                        Provider = Convert.ToInt32(item.Value)
                    };
                    context.UserProviders.InsertOnSubmit(resellerProvider);
                    context.SubmitChanges();
                }
            }
            lbl_result.Text = Tokens.Saved;
            lbl_result.ForeColor = Color.Green;

            Clear();

            Bind_grd_users(false);
        }
    }


    public class SearchUsers
    {
        public int? BranchId { get; set; }

        public bool? IsStopped { get; set; }
        public int ?GroupId { get; set; }
        public int? AccountManagerId { get; set; }
        public int? GovernrateId { get; set; }
    }

    void Clear()
    {
        txt_UserName.Text = TbUserIp.Text =
    txt_UserPhone.Text =
        txt_UserMobile.Text =
            txt_UserAddress.Text =
                txt_UserEmail.Text =
                    txt_LoginName.Text =
                        txt_LoginPassword.Text = string.Empty;
        ddl_Branches.SelectedIndex = ddl_Groups.SelectedIndex = ddlGovernorate.SelectedIndex = ddlCulture.SelectedIndex = ddlManageReuestPrivilege.SelectedIndex = -1;
        chb_IsAccountStopped.Checked = false;
        foreach (ListItem li in chl_Branchs.Items)
        {
            li.Selected = false;
        }
        foreach (ListItem item in chlSaves.Items)
        {
            item.Selected = false;
        }
        foreach (ListItem ite in chl_Providers.Items)
        {
            ite.Selected = false;
        }
    }

    private void Bind_grd_users(bool stoped)
    {
        if (Session["User_ID"] == null)
            Response.Redirect("../default.aspx");
        var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        var userId = Convert.ToInt32(Session["User_ID"]);
        var user = context.Users.Where(a => a.AccountmanagerId == userId).ToList();
        List<DataLevelClass.UsersClass> users;
        if (!user.Any())
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
             users = DataLevelClass.GetListUsersByDataLevel();
        }
        else
        {
            users = user.Select(usr=>
            {
                var firstOrDefault = context.Users.FirstOrDefault(a => a.ID == usr.AccountmanagerId);
                var orDefault = context.Governorates.FirstOrDefault(a => a.ID == usr.GovernorateId);
                if (orDefault != null)
                    return firstOrDefault != null ? new DataLevelClass.UsersClass
                    {
                        Id = usr.ID,
                        UserName = usr.UserName,
                        UserPhone = usr.UserPhone,
                        UserEmail = usr.UserEmail,
                        GroupName = usr.Group.GroupName,
                        GroupId = usr.GroupID,
                        BranchName = usr.Branch.BranchName,
                        BranchId = usr.BranchID,
                        IsAccountStopped = usr.IsAccountStopped,
                        GovernerateId = usr.GovernorateId,
                        AccountManagerId = usr.AccountmanagerId,
                        Governerate = usr.GovernorateId!=null?orDefault.GovernorateName:" ",
                        AccountManager =usr.AccountmanagerId!=null?firstOrDefault.UserName:" "
                    } : null;
                return null;

            }).ToList();
        }


        //users.Provider.CreateQuery()
        var data = new SearchUsers
        {
            BranchId = Helper.GetDropValue(ddlBranch),
            IsStopped = checkIsstoped.Checked,
            GroupId = Helper.GetDropValue(ddlGroup),
            GovernrateId = Helper.GetDropValue(ddlfilterGovernrate),
            AccountManagerId = Helper.GetDropValue(ddlfilterAccountManager)
        };
        if (data.BranchId != null)
        {
            users = users.Where(x => x.BranchId == data.BranchId).ToList(); //(x=>x.BranchID==data.BranchId).ToList();
        }
        if (stoped && data.IsStopped!=null)
        {
            users = users.Where(x => x.IsAccountStopped == data.IsStopped).ToList();
        }
        if (data.GroupId != null)
        {
            users = users.Where(a => a.GroupId == data.GroupId).ToList();
        }
        if (data.AccountManagerId != null)
        {
            users = users.Where(a => a.AccountManagerId == data.AccountManagerId).ToList();
        }
        if (data.GovernrateId != null)
        {
            users = users.Where(a => a.GovernerateId == data.GovernrateId).ToList();
        }
        grd_users.DataSource = users.OrderBy(a=>a.Id);
        grd_users.DataBind();
    }
    protected void grd_users_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (e.Row.Cells[8].Text.Trim() == "False")
            {
                e.Row.Cells[8].Text = "مفعل";
                e.Row.Cells[8].ForeColor = Color.Green;
            }
            if (e.Row.Cells[8].Text.Trim() == "True")
            {
                e.Row.Cells[8].Text = "موقوف";
                e.Row.Cells[8].ForeColor = Color.OrangeRed;
            }
           
        }
    } 

    protected void grd_users_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var dataKey = grd_users.DataKeys[e.RowIndex];
            if (dataKey == null) return;
            var id = Convert.ToInt32(dataKey["ID"]);
            var user = context.Users.FirstOrDefault(usr => usr.ID == id);
            if (user == null) return;
            if (user.WorkOrders.Any())
            {
                lbl_result.Text = string.Format(Tokens.CantDelete);
                lbl_result.ForeColor = Color.Red;
                return;
            }
            if (user.GroupID != null && user.GroupID.Value == 6)
            {
                var checkuser = context.Branches.Where(a => a.AdminID == user.ID);
                var accountManager = context.Users.Where(a => a.AccountmanagerId == user.ID);
                if (checkuser.Any()||accountManager.Any())
                {

                    lbl_result.Text = string.Format(Tokens.CannotDeleteReseller, user.UserName, user.WorkOrders.Count);
                    lbl_result.ForeColor = Color.Red;
                    return;
                }
                try
                {

                    List<Receipt>addrec=new List<Receipt>();
                    var userinres = context.UsersTransactions.Where(x => x.ResellerID == user.ID).ToList();
                    foreach (var usersTransaction in userinres)
                    {
                        var resrec =
                            context.Receipts.FirstOrDefault(x => x.UserTransationID == usersTransaction.ID);

                        if (resrec != null) addrec.Add(resrec);
                    }

                    context.Receipts.DeleteAllOnSubmit(addrec);

                    context.SubmitChanges();

                    context.OfferResellers.DeleteAllOnSubmit(user.OfferResellers);
                    context.UserProviders.DeleteAllOnSubmit(user.UserProviders);
                    context.UserCultures.DeleteAllOnSubmit(user.UserCultures);
                    context.Messages.DeleteAllOnSubmit(user.Messages);
                    context.UserBranches.DeleteAllOnSubmit(user.UserBranches);
                    context.ResellerCredits.DeleteAllOnSubmit(user.ResellerCredits);
                    context.ResellerRouters.DeleteAllOnSubmit(user.ResellerRouters);
                    context.WorkOrderRouters.DeleteAllOnSubmit(user.WorkOrderRouters);
                  
                    context.ResellerPackagesDiscounts.DeleteAllOnSubmit(user.ResellerPackagesDiscounts);
                    context.ReceiptCnfgs.DeleteAllOnSubmit(user.ReceiptCnfgs);
                  
                    context.ResellerAttachments.DeleteAllOnSubmit(user.ResellerAttachments);
                    context.ResellerCreditsVoices.DeleteAllOnSubmit(user.ResellerCreditsVoices);
                  
                    context.UpdatedResellerPayments.DeleteAllOnSubmit(user.UpdatedResellerPayments);
                  
                    context.RechargeClientRequests.DeleteAllOnSubmit(user.RechargeClientRequests);
                   
                    context.PayingCustomersResellers.DeleteAllOnSubmit(user.PayingCustomersResellers);
                    context.UsersTransactions.DeleteAllOnSubmit(user.UsersTransactions);
                  
                    context.Reminders.DeleteAllOnSubmit(user.Reminders);
                 
                    context.RechargeRequests.DeleteAllOnSubmit(user.RechargeRequests);
                    context.CustomerPayments.DeleteAllOnSubmit(user.CustomerPayments);
                    context.WorkOrderHistories.DeleteAllOnSubmit(user.WorkOrderHistories);
                    context.WorkOrderStatus.DeleteAllOnSubmit(user.WorkOrderStatus);
                 
                    context.Routers.DeleteAllOnSubmit(user.Routers);
                    context.RequestsNotitfications.DeleteAllOnSubmit(user.RequestsNotitfications);
                    context.UserTrackings.DeleteAllOnSubmit(user.UserTrackings);

                    context.Users.DeleteOnSubmit(user);
                    context.SubmitChanges();
                    lbl_result.Text = Tokens.Deleted;
                    lbl_result.ForeColor = Color.Green;
                    Bind_grd_users(false);
                    ClearControls(this);
                    ViewState["ID"] = null;
                }
                catch (Exception exception)
                {
                    lbl_result.Text = Tokens.CantDelete + exception.Message;
                }
            }
            else
            {
                try
                {
                    if (user.ResellerCredits.Any() || user.BranchCredits.Any() || user.BranchCreditVoices.Any() ||
                        user.BoxCredits.Any() || user.UserSaves.Any() ||
                        user.UsersTransactions.Any() || user.IncomingExpenses.Any() || user.OutgoingExpenses.Any() ||
                        user.Demands.Any())
                    {
                        lbl_result.Text = string.Format(Tokens.CantDelete);
                        lbl_result.ForeColor = Color.Red;
                        return;
                    }
                    //context.UserCultures.DeleteAllOnSubmit(user.UserCultures);
                    //context.UserBranches.DeleteAllOnSubmit(user.UserBranches);
                    context.OfferResellers.DeleteAllOnSubmit(user.OfferResellers);
                    context.UserProviders.DeleteAllOnSubmit(user.UserProviders);
                    context.UserCultures.DeleteAllOnSubmit(user.UserCultures);
                    context.Messages.DeleteAllOnSubmit(user.Messages);
                    context.UserBranches.DeleteAllOnSubmit(user.UserBranches);
                    //context.ResellerCredits.DeleteAllOnSubmit(user.ResellerCredits);
                    context.ResellerRouters.DeleteAllOnSubmit(user.ResellerRouters);
                    context.WorkOrderRouters.DeleteAllOnSubmit(user.WorkOrderRouters);
                    //context.BranchCredits.DeleteAllOnSubmit(user.BranchCredits);
                    context.ResellerPackagesDiscounts.DeleteAllOnSubmit(user.ResellerPackagesDiscounts);
                    context.ReceiptCnfgs.DeleteAllOnSubmit(user.ReceiptCnfgs);
                    //new tables
                    context.RecieveRouters.DeleteAllOnSubmit(user.RecieveRouters);
                    //context.BranchCreditVoices.DeleteAllOnSubmit(user.BranchCreditVoices);
                    context.ResellerAttachments.DeleteAllOnSubmit(user.ResellerAttachments);
                    context.ResellerCreditsVoices.DeleteAllOnSubmit(user.ResellerCreditsVoices);
                    //context.BoxCredits.DeleteAllOnSubmit(user.BoxCredits);
                    context.UpdatedResellerPayments.DeleteAllOnSubmit(user.UpdatedResellerPayments);
                    context.WorkOrderNotes.DeleteAllOnSubmit(user.WorkOrderNotes);
                    //context.UserSaves.DeleteAllOnSubmit(user.UserSaves);
                    context.RechargeClientRequests.DeleteAllOnSubmit(user.RechargeClientRequests);
                   
                    context.PayingCustomersResellers.DeleteAllOnSubmit(user.PayingCustomersResellers);
                    //context.UsersTransactions.DeleteAllOnSubmit(user.UsersTransactions);
                    context.Phones.DeleteAllOnSubmit(user.Phones);
                    context.Options.DeleteAllOnSubmit(user.Options);
                    context.Reminders.DeleteAllOnSubmit(user.Reminders);
                    context.RequestDateHistories.DeleteAllOnSubmit(user.RequestDateHistories);
                    //context.OutgoingExpenses.DeleteAllOnSubmit(user.OutgoingExpenses);
                    //context.IncomingExpenses.DeleteAllOnSubmit(user.IncomingExpenses);
                    context.Accounts.DeleteAllOnSubmit(user.Accounts);
                    context.CallMessages.DeleteAllOnSubmit(user.CallMessages);
                    context.RechargeRequests.DeleteAllOnSubmit(user.RechargeRequests);
                    context.CustomerPayments.DeleteAllOnSubmit(user.CustomerPayments);
                    context.WorkOrderHistories.DeleteAllOnSubmit(user.WorkOrderHistories);
                    context.WorkOrderStatus.DeleteAllOnSubmit(user.WorkOrderStatus);
                   // context.Demands.DeleteAllOnSubmit(user.Demands);
                    context.Routers.DeleteAllOnSubmit(user.Routers);
                    context.RequestsNotitfications.DeleteAllOnSubmit(user.RequestsNotitfications);
                    context.CenterCredits.DeleteAllOnSubmit(user.CenterCredits);
                    context.UserTrackings.DeleteAllOnSubmit(user.UserTrackings);

                    context.Users.DeleteOnSubmit(user);
                    context.SubmitChanges();
                    Bind_grd_users(false);
                    lbl_result.Text = string.Format(Tokens.Deleted);
                    lbl_result.ForeColor = Color.Green;
                }
                catch (Exception exception)
                {
                    lbl_result.Text = string.Format("{0}, Details:{1}",Tokens.CantDelete,exception.Message);
                    lbl_result.ForeColor = Color.Red;
                }


            }
        }
    }


    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        btn_Add.Visible = true;
        btn_Update.Visible = false;
        btn_Cancel.Visible = false;
        ClearControls(this);
        ViewState["ID"] = null;
        RequiredFieldValidator7.Enabled = true;
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            int id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            var query = context.Users.Where(usr => usr.ID == id);
            User user = query.First();
            txt_UserName.Text = user.UserName;
            txt_UserPhone.Text = user.UserPhone;
            txt_UserMobile.Text = user.UserMobile;
            txt_UserAddress.Text = user.UserAddress;
            txt_UserEmail.Text = user.UserEmail;
            //TbUserIp.Text = user.Ip;

            txt_LoginName.Text = user.LoginName;
            chb_IsAccountStopped.Checked = user.IsAccountStopped != null && user.IsAccountStopped.Value;
            //txt_LoginPassword.Text = Editusr.LoginPassword;
            ddl_Groups.SelectedValue = user.GroupID.ToString();
            if (user.BranchID != null)
                ddl_Branches.SelectedValue = user.BranchID.ToString();
            if (user.GovernorateId != null)
                ddlGovernorate.SelectedValue = user.GovernorateId.ToString();
            if (user.AccountmanagerId != null)
                ddlAccountManager.SelectedValue = user.AccountmanagerId.ToString();
            if (user.ManageRequestPrivilege != null)
                ddlManageReuestPrivilege.SelectedValue = user.ManageRequestPrivilege.ToString();
            if (user.UserCultures != null)
                ddlCulture.SelectedValue = _culture.GetUserCultureName(user.ID);//user.CultureId.ToString();
                

            TbUserIp.Text = user.Ip;

            List<int?> userBranchIds = context.UserBranches.Where(ub => ub.UserID == id).Select(ub => ub.BranchID).ToList();
            foreach (ListItem li in chl_Branchs.Items)
            {
                if (userBranchIds.Contains(Convert.ToInt32(li.Value)))
                    li.Selected = true;
                else
                    li.Selected = false;
            }
            List<int?> userSaves = context.UserSaves.Where(a => a.UserId == id).Select(a => a.SaveId).ToList();
            foreach (ListItem save in chlSaves.Items)
            {
                save.Selected = userSaves.Contains(Convert.ToInt32(save.Value));
            }
            if (user.GroupID == 6)
            {
                RowAccountManager.Visible=Label8.Visible = true;
                BindProviders();
                var userProviders = context.UserProviders.Where(a => a.UserId == id).Select(a => a.Provider).ToList();
                foreach (ListItem prov in chl_Providers.Items)
                {
                    prov.Selected = userProviders.Contains(Convert.ToInt32(prov.Value));
                }
            }

            ViewState["ID"] = id;
            btn_Add.Visible = false;
            btn_Update.Visible = true;
            btn_Cancel.Visible = true;
            RequiredFieldValidator7.Enabled = false;
        }
    }


    protected void btn_Update_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (ViewState["ID"] == null)
            {
                lbl_result.Text = Tokens.SelectUser;
                lbl_result.ForeColor = Color.Red;
                return;
            }
            int id = Convert.ToInt32(ViewState["ID"]);
            var query = context.Users.Where(usr => usr.ID == id);
            var updateusr = query.First();
            updateusr.UserName = txt_UserName.Text.Trim();
            updateusr.UserPhone = txt_UserPhone.Text.Trim();
            updateusr.UserMobile = txt_UserMobile.Text.Trim();
            updateusr.UserAddress = txt_UserAddress.Text.Trim();
            updateusr.UserEmail = txt_UserEmail.Text.Trim();
            updateusr.Ip = TbUserIp.Text;

            updateusr.LoginName = txt_LoginName.Text.Trim();
            if (txt_LoginPassword.Text != "")
                updateusr.LoginPassword = Security.EncodePassword(txt_LoginPassword.Text);
            updateusr.GroupID = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
            if (ddl_Branches.SelectedIndex != 0 & ddl_Branches.SelectedItem.Value!="")
                updateusr.BranchID = Convert.ToInt32(ddl_Branches.SelectedItem.Value);
            updateusr.IsAccountStopped = chb_IsAccountStopped.Checked;
            if (ddlGovernorate.SelectedIndex != 0 && ddlGovernorate.SelectedValue != "-1")
                updateusr.GovernorateId = Convert.ToInt32(ddlGovernorate.SelectedItem.Value);
            if (ddlManageReuestPrivilege.SelectedItem.Value != Tokens._minusOne && ddlManageReuestPrivilege.SelectedIndex!=0)
                updateusr.ManageRequestPrivilege = Convert.ToInt32(ddlManageReuestPrivilege.SelectedItem.Value);
            if (ddlCulture.SelectedIndex != 0 && ddlCulture.SelectedValue != "-1")
                _culture.UpdateUserCulture(Convert.ToInt32(ddlCulture.SelectedItem.Value),updateusr.ID);
            if (ddlAccountManager.SelectedIndex != -1 && RowAccountManager.Visible && updateusr.GroupID == 6)
                updateusr.AccountmanagerId = Convert.ToInt32(ddlAccountManager.SelectedItem.Value);
            context.SubmitChanges();

            List<UserBranch> ubList = context.UserBranches.Where(u => u.UserID == updateusr.ID).ToList();
            context.UserBranches.DeleteAllOnSubmit(ubList);
            context.SubmitChanges();
            foreach (ListItem li in chl_Branchs.Items)
            {
                if (li.Selected)
                {
                    var ub = new UserBranch();
                    ub.UserID = updateusr.ID;
                    ub.BranchID = Convert.ToInt32(li.Value);
                    context.UserBranches.InsertOnSubmit(ub);
                    context.SubmitChanges();
                }
            }
            var userSaves = context.UserSaves.Where(s => s.UserId == updateusr.ID).ToList();
            context.UserSaves.DeleteAllOnSubmit(userSaves);
            foreach (ListItem item in chlSaves.Items)
            {
                if (item.Selected)
                {
                    var newsavs = new UserSave
                    {
                        UserId = updateusr.ID,
                        SaveId = Convert.ToInt32(item.Value)
                    };
                    context.UserSaves.InsertOnSubmit(newsavs);
                    context.SubmitChanges();
                }
            }
            if (updateusr.GroupID == 6)
            {
                if (chl_Providers.Items.Count > 0)
                {
                    if (updateusr.UserProviders != null)
                    {
                        context.UserProviders.DeleteAllOnSubmit(updateusr.UserProviders);
                        context.SubmitChanges();
                    }
                }
                foreach (ListItem item in chl_Providers.Items)
                {
                    if (!item.Selected) continue;
                    var resellerProvider = new UserProvider
                    {
                        UserId = updateusr.ID,
                        Provider = Convert.ToInt32(item.Value)
                    };
                    context.UserProviders.InsertOnSubmit(resellerProvider);
                    context.SubmitChanges();
                }
            }
            context.SubmitChanges();
            lbl_result.Text = Tokens.UserUpdatedSuccess;
            lbl_result.ForeColor = Color.Green;
            Clear();
            //btn_Add.Visible = true;
            //btn_Update.Visible = false;
            //btn_Cancel.Visible = false;
            Bind_grd_users(false);
        }
    }


    void ClearControls(Control ctr)
    {
        if (ctr.HasControls())
        {
            foreach (Control tmpctr in ctr.Controls)
            {
                if (ctr.HasControls())
                    ClearControls(tmpctr);
                else if (ctr is TextBox)
                    ((TextBox)ctr).Text = "";
                else if (ctr is DropDownList)
                {
                    ((DropDownList)ctr).SelectedValue = null;
                }
            }
        }
        else if (ctr is TextBox)
            ((TextBox)ctr).Text = "";
        else if (ctr is DropDownList)
        {
            ((DropDownList)ctr).SelectedValue = null;
        }
    }


   
    void Activate()
    {
        grd_users.DataBound += (o, b) => Helper.GridViewNumbering(grd_users, "LNo");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_grd_users(true);
    }

    protected void ddl_Groups_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (ddl_Groups.SelectedIndex != 0)
        {
            var groupId = Convert.ToInt32(ddl_Groups.SelectedItem.Value);
            RowAccountManager.Visible = groupId == 6;
            if (groupId != 6) return;
            Label8.Visible = true;
            BindProviders();
        }
        else
        {
            Label8.Visible=RowAccountManager.Visible = false;
        }
    }
}

}