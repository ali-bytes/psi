using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.BL.Abstract;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerVoiceCredit : CustomPage
    {
   
    private readonly IResellerCreditVoiceRepository _creditRepository;
    private readonly IUserSaveRepository _userSave;
    private readonly IResellerServices _resellerServices;
    public ResellerVoiceCredit()
    {
        _creditRepository=new ResellerCreditVoiceRepository();
        _resellerServices=new ResellerServices();
        _userSave=new UserSaveRepository();
    }
    

    void PopulateReseller()
    {
        var resellers = _resellerServices.GetResellersUponUserGroup(Convert.ToInt32(Session["User_ID"]));
        DdlReseller.DataSource = resellers;
        DdlReseller.DataTextField = "UserName";
        DdlReseller.DataValueField = "Id";
        DdlReseller.DataBind();
        Helper.AddAllDefaultItem(DdlReseller);
    }
    void PopulateSvaes()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
            {
                a.Save.SaveName,
                a.Save.Id
            });
            ddlSaves.DataBind();
            Helper.AddDefaultItem(ddlSaves);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        PopulateReseller();
        PopulateSvaes();
        Validation();
    }


    protected void BSave_Click(object sender, EventArgs e)
    {
        var effect = RblEffect.SelectedIndex;
        int resellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
        int userId = Convert.ToInt32(Session["User_ID"]);
        decimal amount = effect == 0 ? Convert.ToDecimal(TbAmount.Text) : Convert.ToDecimal(TbAmount.Text) * -1;
        string notes = TbNotes.Text;
        DateTime time = DateTime.Now.AddHours();
        var result = _creditRepository.SaveVoice(resellerId, userId, amount, notes, time);
       
        var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            
            if (amount > 0)
            {
                var reseller = context.Users.First(x => x.ID == resellerId);
                var note = "اضافة رصيد لموزع" + " اسم الموزع : " + reseller.UserName;
                _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount), notes,
                    " من صفحة رصيد مدفوعات الموزع للعملاء الخارجيين " + note, context);
              
            }
        }
        switch (result)
        {
            case SaveVoiceResult.Saved:
                Message.Text = Tokens.Saved;
                Clear();
                PopulateReseller();
                break;
            case SaveVoiceResult.NoCredit:
                Message.Text = Tokens.NotEnoughtCreditMsg;
                break;
        }
        UpdateHistor(resellerId);
    }


    void Clear()
    {
        TbAmount.Text = TbNotes.Text = string.Empty;
    }


    void UpdateHistor(int resellerId)
    {
        var history = _creditRepository.ResellerCreditsVoices(resellerId);
        GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Id).Select(x => new
        {
            Amount = Helper.FixNumberFormat(x.Amount),
            Net = Helper.FixNumberFormat(x.Net),
            Reseller = x.User.UserName,
            User = x.User1.UserName,
            Date = x.Time,
            x.Notes,
            Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
            RecieptUrl = string.Format("ResellerPaymentReciept.aspx?Voiceid={0}", x.Id),
            link = x.RechargeRequestId != null ? string.Format("../Attachments/{0}", x.RechargeRequest.RecieptImage) : Tokens.NoFiles,
            ifAttchment = x.RechargeRequestId != null
        });
        GvHistory.DataBind();
    }


    protected void DdlReseller_SelectedIndexChanged(object sender, EventArgs e)
    {
        var id = string.IsNullOrEmpty(DdlReseller.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlReseller.SelectedItem.Value);
        if (id == 0)
        {
            return;
        }
        UpdateHistor(id);
    }


    protected void GvHistory_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvHistory, "no");
    }
    #region Branch Save
       
    #endregion
    void Validation()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userid = Convert.ToInt32(Session["User_ID"]);
            var groupid = context.Users.FirstOrDefault(u => u.ID == userid);
            if (groupid != null && (groupid.Group.DataLevelID == 1))
            {
                Literal3.Visible = Literal4.Visible = Literal5.Visible =lblsave.Visible=ddlSaves.Visible=
                TbAmount.Visible = TbNotes.Visible = RblEffect.Visible = btn_Payment.Visible = true;

            }
            if (!RblEffect.Visible) return;
            var groupPrivilegeQuery =
                context.GroupPrivileges.Where(gp => gp.Group.ID == groupid.GroupID).Select(gp => gp.privilege.Name);
            var add = groupPrivilegeQuery.Contains("AddCredit");
            if (!add)
            {
                RblEffect.Items.Add(new ListItem(Tokens.Add, "0"));
            }
            else
            {
                RblEffect.Items.Add(new ListItem(Tokens.Add, "0"));
                RblEffect.Items.Add(new ListItem(Tokens.Subtract, "1"));
            }
        }
    }
}
}