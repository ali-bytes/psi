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
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchVoiceCredit : CustomPage
    {
        
            readonly IBranchCreditVoiceRepository _creditRepository = new BranchCreditVoiceRepository();
            readonly IUserSaveRepository _userSave = new UserSaveRepository();
            void PopulateBranch()
            {
                var resellers = DataLevelClass.GetUserBranches();
                DdlBranch.DataSource = resellers;
                DdlBranch.DataTextField = "BranchName";
                DdlBranch.DataValueField = "ID";
                DdlBranch.DataBind();
                Helper.AddAllDefaultItem(DdlBranch);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateBranch();
                PopulateSvaes();
                Validation();
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

            protected void BSave_Click(object sender, EventArgs e)
            {
                var effect = RblEffect.SelectedIndex;
                int BranchId = Convert.ToInt32(DdlBranch.SelectedItem.Value);
                int userId = Convert.ToInt32(Session["User_ID"]);
                decimal amount = effect == 0 ? Convert.ToDecimal(TbAmount.Text) : Convert.ToDecimal(TbAmount.Text) * -1;
                string notes = TbNotes.Text;
                DateTime time = DateTime.Now.AddHours();
                var result = _creditRepository.SaveVoice(BranchId, userId, amount, notes, time);
                // Savestepsinsaves();
                var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (amount > 0)
                    {
                        _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount),
                            "رصيد مدفوعات الفرع للعملاء الخارجيين", notes, context);
                        //_userSave.UpdateSave(userId, saveId, Convert.ToDouble(amount), "رصيد مدفوعات الفرع للعملاء الخارجيين", notes, context);
                    }
                }
                switch (result)
                {
                    case SaveVoiceResult.Saved:
                        Message.Text = Tokens.Saved;
                        Clear();
                        PopulateBranch();
                        break;
                    case SaveVoiceResult.NoCredit:
                        Message.Text = Tokens.NotEnoughtCreditMsg;
                        break;
                }
                UpdateHistor(BranchId);
            }


            void Clear()
            {
                TbAmount.Text = TbNotes.Text = string.Empty;
            }


            void UpdateHistor(int branchId)
            {
                var history = _creditRepository.BranchCreditsVoices(branchId);
                GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Id).Select(x => new
                {
                    Amount = Helper.FixNumberFormat(x.Amount),
                    Net = Helper.FixNumberFormat(x.Net),
                    x.Branch.BranchName,
                    User = x.User.UserName,
                    Date = x.Time,//string.Format("{0:d}", x.Time),
                    x.Notes,
                    Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                    RecieptUrl = string.Format("BranchePaymentReciept.aspx?voiceid={0}", x.Id)
                });
                GvHistory.DataBind();
            }


            protected void DdlBranch_SelectedIndexChanged(object sender, EventArgs e)
            {
                var id = string.IsNullOrEmpty(DdlBranch.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlBranch.SelectedItem.Value);
                if (id == 0)
                {
                    return;
                }
                UpdateHistor(id);
            }
            void Validation()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userid = Convert.ToInt32(Session["User_ID"]);
                    var groupid = context.Users.FirstOrDefault(u => u.ID == userid);
         
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

            protected void GvHistory_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvHistory, "no");
            }
           
        }
    }
 