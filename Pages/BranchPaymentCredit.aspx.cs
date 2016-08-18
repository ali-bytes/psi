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
    public partial class BranchPaymentCredit : CustomPage
    {
       
            readonly BranchCreditRepository _creditRepository = new BranchCreditRepository();
            readonly IUserSaveRepository _userSave = new UserSaveRepository();
            void PopulateBranches()
            {
                var branches = DataLevelClass.GetUserBranches();
                DdlReseller.DataSource = branches;
                DdlReseller.DataTextField = "BranchName";
                DdlReseller.DataValueField = "ID";
                DdlReseller.DataBind();
                Helper.AddAllDefaultItem(DdlReseller);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateBranches();
                Validation();
                PopulateSvaes();
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
                int branchId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
                int userId = Convert.ToInt32(Session["User_ID"]);
                decimal amount = effect == 0 ? Convert.ToDecimal(TbAmount.Text) : Convert.ToDecimal(TbAmount.Text) * -1;
                string notes = TbNotes.Text;
                var time = DateTime.Now.AddHours();
                //Savestepsinsaves();
                var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (amount > 0)
                    {
                        var cussrentuser = context.Users.FirstOrDefault(x => x.ID == userId);
                        //if (cussrentuser == null) return;
                        var branchName = context.Branches.FirstOrDefault(a => a.ID == branchId);
                        if (branchName == null) return;
                        _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount),
                            " اضافة رصيد للفرع من صفحة رصيد مدفوعات الفرع " + branchName.BranchName, TbNotes.Text, context);
                        //_userSave.UpdateSave(userId, saveId,Convert.ToDouble(amount), " من صفحة رصيد مدفوعات الفرع ", TbNotes.Text,context );
                    }

                    var result = _creditRepository.Save(branchId, userId, amount, notes, time);
                    switch (result)
                    {
                        case SaveResult.Saved:
                            Message.Text = Tokens.Saved;
                            Clear();
                            PopulateBranches();
                            PopulateSvaes();
                            if (amount > 0)
                            {
                                var branch = context.Branches.FirstOrDefault(x => x.ID == branchId);
                                if (branch != null && !string.IsNullOrEmpty(branch.Mobile1))
                                {
                                    var message = SendSms.SendSmsByNotification(context, branch.Mobile1, 7);
                                    if (!string.IsNullOrEmpty(message))
                                    {
                                        var myscript = "window.open('" + message + "')";
                                        ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                                    }
                                }
                            }
                            break;
                        case SaveResult.NoCredit:
                            Message.Text = Tokens.NotEnoughtCreditMsg;
                            break;
                    }
                }
                //UpdateHistor(branchId);
            }
            #region Save Branch
           
            #endregion

            void Clear()
            {
                TbAmount.Text = TbNotes.Text = string.Empty;
            }


            void UpdateHistor(int resellerId)
            {
                var history = _creditRepository.BranchCredits(resellerId);
                //GvHistory.DataSource = history.ToList().OrderByDescending(x => x.Id).Select(x => new
                //{
                //    Amount = Helper.FixNumberFormat(x.Amount),
                //    Net = Helper.FixNumberFormat(x.Net),
                //    Branch = x.Branch.BranchName,
                //    User = x.User.UserName,
                //    Date = x.Time,//string.Format("{0:d}", x.Time),
                //    x.Notes,
                //    Type = x.Amount < 0 ? Tokens.Subtract : Tokens.Add,
                //    RecieptUrl = string.Format("BranchPaymentReciept.aspx?id={0}", x.Id)
                //});
                //GvHistory.DataBind();
                var firstOrDefault = history.OrderByDescending(z => z.Id).FirstOrDefault();
                if (firstOrDefault != null) lblLastCredit.Text = Helper.FixNumberFormat(firstOrDefault.Net);
            }


            protected void DdlReseller_SelectedIndexChanged(object sender, EventArgs e)
            {
                var id = string.IsNullOrEmpty(DdlReseller.SelectedItem.Value) ? 0 : Convert.ToInt32(DdlReseller.SelectedItem.Value);
                if (id == 0) return;
                UpdateHistor(id);
            }


            //protected void GvHistory_DataBound(object sender, EventArgs e)
            //{
            //    Helper.GridViewNumbering(GvHistory, "no");
            //}


            void Validation()
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userid = Convert.ToInt32(Session["User_ID"]);
                    var groupid = context1.Users.FirstOrDefault(u => u.ID == userid);
                    if (groupid != null && (groupid.Group.DataLevelID == 1))
                    {
                        lblAmount.Visible = lblEffect.Visible = lblNotes.Visible = lblsave.Visible = ddlSaves.Visible =
                        TbAmount.Visible = TbNotes.Visible = RblEffect.Visible = btn_Payment.Visible = true;

                    }
                }
            }
        }
    }
 