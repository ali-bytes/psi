using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class CenterPaymentCredit : CustomPage
    {
        
            readonly ICenterCreditRepository _creditRepository = new CenterCreditRepository();
            //readonly ISPDataContext _context;
            readonly IspDomian _domian;
            private readonly IUserSaveRepository _userSave;
            public  CenterPaymentCredit()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _domian = new IspDomian(context);
                _userSave = new UserSaveRepository();
            }


            /*void PopulateUsers(){
                var resellers = _domian.PopulateUsers(DdlUsers);
                DdlReseller.DataSource = resellers;
                DdlReseller.DataTextField = "UserName";
                DdlReseller.DataValueField = "Id";
                DdlReseller.DataBind();
                Helper.AddAllDefaultItem(DdlReseller);
            }*/


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                _domian.PopulateUsers(DdlUsers);
                Validation();
                PopulateSaves();
            }

            void PopulateSaves()
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
                int centerId = Convert.ToInt32(DdlUsers.SelectedItem.Value);
                int userId = Convert.ToInt32(Session["User_ID"]);
                decimal amount = effect == 0 ? Convert.ToDecimal(TbAmount.Text) : Convert.ToDecimal(TbAmount.Text) * -1;
                string notes = TbNotes.Text;
                DateTime time = DateTime.Now.AddHours();
                var result = _creditRepository.Save(centerId, userId, amount, notes, time);
                var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (amount > 0) _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount), "رصيد مدفوعات مراكز التحصيل", notes, context);
                }
                switch (result)
                {
                    case SaveResult.Saved:
                        Message.Text = Tokens.Saved;
                        Clear();
                        _domian.PopulateUsers(DdlUsers);
                        break;
                    case SaveResult.NoCredit:
                        Message.Text = Tokens.NotEnoughtCreditMsg;
                        break;
                }
                UpdateHistor(centerId);
            }

            void Clear()
            {
                TbAmount.Text = TbNotes.Text = string.Empty;
            }


            void UpdateHistor(int centerid)
            {
                var history = _creditRepository.CenterCredits(centerid);
                GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Id).Select(x => new
                {
                    Amount = Helper.FixNumberFormat(x.Amount),
                    Net = Helper.FixNumberFormat(x.Net),
                    Center = x.User.UserName,
                    User = x.User1.UserName,
                    Date = x.Time,//string.Format("{0:d}", x.Time),
                    x.Notes,
                    //todo: link beetween reseller credit and recharge request
                    // todo: reseller name in bote in bs
                    //link =x.RechargeRequestId !=null? string.Format("../Attachments/{0}", x.RechargeRequest.RecieptImage):Tokens.NoFiles,
                    Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                    RecieptUrl = string.Format("CenterPaymentReciept.aspx?id={0}", x.Id)
                });
                GvHistory.DataBind();
                var firstOrDefault = history.OrderByDescending(z => z.Id).FirstOrDefault();
                if (firstOrDefault != null) lblLastCredit.Text = Helper.FixNumberFormat(firstOrDefault.Net);
                else lblLastCredit.Text = string.Empty;
            }


            protected void DdlUsers_SelectedIndexChanged(object sender, EventArgs e)
            {
                var id = string.IsNullOrEmpty(DdlUsers.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlUsers.SelectedItem.Value);
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


            void Validation()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    var userid = Convert.ToInt32(Session["User_ID"]);
                    var groupid = context.Users.FirstOrDefault(u => u.ID == userid);
                    if (groupid != null && groupid.GroupID == 6)
                    {
                        ltAmount.Visible = ltEffect.Visible = ltNotes.Visible = false;
                        TbAmount.Visible = TbNotes.Visible = RblEffect.Visible = BSave.Visible = false;

                    }
                }
            }

        }
    }
 