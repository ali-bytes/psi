using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerPaymenyCredit : CustomPage
    {
     
    readonly IResellerCreditRepository _creditRepository = new ResellerCreditRepository();
    //readonly IResellerServices _resellerServices = new ResellerServices();
    readonly IUserSaveRepository _userSave=new UserSaveRepository();
    //static bool _branchPrintExcel;

    void PopulateReseller(){
        if (Session["User_ID"] == null)
        {
            Response.Redirect("default.aspx");
            return;
        }
        var resellers = _creditRepository.GetResellers(Convert.ToInt32(Session["User_ID"]));//GetResellersUponUserGroupWithCredit(Convert.ToInt32(Session["User_ID"]));
        DdlReseller.DataSource = resellers;
        DdlReseller.DataTextField = "UserName";
        DdlReseller.DataValueField = "Id";
        DdlReseller.DataBind();
        Helper.AddAllDefaultItem(DdlReseller);
    }
    void PopulateSaves()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
            {
                a.Save.Id,
                a.Save.SaveName
            });
            ddlSaves.DataBind();
            Helper.AddDefaultItem(ddlSaves);
        }
    }

    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulateReseller();
        Validation();
        PopulateSaves();
    }


    protected void BSave_Click(object sender, EventArgs e){
        var effect = RblEffect.SelectedIndex;
        int resellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
        int userId = Convert.ToInt32(Session["User_ID"]);
        decimal amount = effect == 0 ? Convert.ToDecimal(TbAmount.Text) : Convert.ToDecimal(TbAmount.Text) * -1;
        string notes = TbNotes.Text;
        DateTime time = DateTime.Now.AddHours();
        var result = _creditRepository.Save(resellerId, userId, amount, notes, time);
       
        var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
        var notes2 = " اضافة من صفحة رصيد مدفوعات موزع"+" اسم الموزع : "+DdlReseller.SelectedItem;
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (amount > 0)
            {
                _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount), notes2, notes, context);
              
            }

            switch (result)
            {
                case SaveResult.Saved:
                    Message.Text = Tokens.Saved;
                    Clear();
                    PopulateReseller();
                    if (amount > 0)
                    {
                        var reseller = context.Users.FirstOrDefault(x => x.ID == resellerId);
                        if (reseller != null &&!string.IsNullOrEmpty(reseller.UserMobile))
                        {
                            var message = SendSms.SendSmsByNotification(context, reseller.UserMobile, 6);
                            if (!string.IsNullOrEmpty(message))
                            {
                                var myscript = "window.open('" + message + "')";
                                ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript, true);
                            }
                        }
                    }
                    break;
                case SaveResult.NoCredit:
                    Message.Text = Tokens.NotEnoughtCreditMsg;
                    break;
            }
        }
        UpdateHistor(resellerId);
    }
    #region Branch Save
       
    #endregion

    void Clear(){
        TbAmount.Text = TbNotes.Text = string.Empty;
        ddlSaves.SelectedIndex = -1;
    }


    void UpdateHistor(int resellerId)
    {
        lblLastCredit.Text = "";
        var history = _creditRepository.ResellerCredits(resellerId);
       
        var firstOrDefault = history.OrderByDescending(z => z.Id).FirstOrDefault();
        if(firstOrDefault != null) lblLastCredit.Text =Helper.FixNumberFormat(firstOrDefault.Net);
    }


    protected void DdlReseller_SelectedIndexChanged(object sender, EventArgs e){
        var id = string.IsNullOrEmpty(DdlReseller.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlReseller.SelectedItem.Value);
        if(id == 0){
            return;
        }
        UpdateHistor(id);
    }

    void Validation(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var userid = Convert.ToInt32(Session["User_ID"]);
            var groupid = context.Users.FirstOrDefault(u => u.ID == userid);
            if (groupid != null && (groupid.Group.DataLevelID == 1 || groupid.Group.DataLevelID == 2))
            {
                ltAmount.Visible = ltEffect.Visible = ltNotes.Visible = ddlSaves.Visible = Literal1.Visible =
                    TbAmount.Visible = TbNotes.Visible = RblEffect.Visible = btn_Payment.Visible = true;
            }
            if (!RblEffect.Visible) return;
            var groupPrivilegeQuery =
                context.GroupPrivileges.Where(gp => gp.Group.ID == groupid.GroupID).Select(gp => gp.privilege.Name);
            var add = groupPrivilegeQuery.Contains("AddCredit");
            if (!add)
            {
                RblEffect.Items.Add(new ListItem(Tokens.Add,"0"));
            }
            else
            {
                RblEffect.Items.Add(new ListItem(Tokens.Add,"0"));
                RblEffect.Items.Add(new ListItem(Tokens.Subtract,"1"));
            }
        }
    }


   
}
}