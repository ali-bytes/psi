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
    public partial class HandelRechargeBranch : CustomPage
    {
      
            //readonly ISPDataContext _context;
            readonly IBoxCreditRepository _creditRepository;

            readonly IBranchCreditVoiceRepository _branchrvoiceCredit;

            readonly BranchCreditRepository _branchCredit;
            readonly IspDomian _domian;
            public  HandelRechargeBranch()
            {
                //_context = IspDataContext;
                _domian = new IspDomian(IspDataContext);
                _creditRepository = new BoxCreditRepository();
                _branchrvoiceCredit = new BranchCreditVoiceRepository();
                _branchCredit = new BranchCreditRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);
                //BuildUponUserType();
                if (IsPostBack) return;
                //_domian.PopulateResellers(ddl_reseller, true);
                CheckOnBranch();
            }


            void CheckOnBranch()
            {
                using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var beanches = DataLevelClass.GetUserBranches();
                    var BranchHasRequest = new List<Branch>();
                    foreach (var item in beanches)
                    {
                        var check = _context.RechargeRequestBranches.FirstOrDefault(ch => ch.BranchId == item.ID && ch.IsApproved == null);
                        if (check != null)
                        {
                            BranchHasRequest.Add(item);
                        }
                    }
                    ddl_Branch.DataSource = BranchHasRequest;
                    ddl_Branch.DataTextField = "BranchName";
                    ddl_Branch.DataValueField = "ID";
                    ddl_Branch.DataBind();
                    Helper.AddDefaultItem(ddl_Branch);

                }
            }


            protected void b_addRequest_Click(object sender, EventArgs e)
            {
                FindRequests();
            }
            protected void gv_customers_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_customers, "gv_lNumber");

                if (hf_user.Value == "6")
                {
                    var cols = gv_customers.Columns;
                    cols[6].Visible = false;
                    b_changeReseller.Visible = false;
                }
                var rows = gv_customers.Rows;
                foreach (GridViewRow item in rows)
                {
                    var lbl = item.FindControl("lbldirection") as Label;
                    if (lbl != null)
                    {
                        switch (lbl.Text)
                        {
                            case "0":
                                lbl.Text = Tokens.BranchVoiceCredit;
                                break;
                            case "1":
                                lbl.Text = Tokens.BranchPaymentCredit;
                                break;
                            case "2":
                                lbl.Text = Tokens.AddToBranchBalanceSheet;
                                break;
                            default:
                                lbl.Text = @"-";
                                break;
                        }
                    }
                }
            }


            void FindRequests()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var requests = context.RechargeRequestBranches
                        .Where(r =>
                            r.IsApproved == null && r.BranchId == Convert.ToInt32(HiddenField1.Value))
                        .Select(x => new
                        {
                            x.Id,
                            x.BoxId,
                            x.Box.BoxName,
                            x.BranchId,
                            x.Branch.BranchName,
                            x.DepositorName,
                            x.Amount,
                            x.Time,
                            Url = string.Format("../Attachments/{0}", x.RecieptImage),
                            x.CreditOrVoice
                        }).ToList();
                    gv_customers.DataSource = requests;
                    gv_customers.DataBind();
                    mv_container.SetActiveView(v_results);
                    //p_rDetails.GroupingText = Tokens.Requests + "&nbsp;" + "(" + gv_customers.Rows.Count + ")";
                }
            }


            protected void b_changeBranch_Click(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);
                //BuildUponUserType();
                //_domian.PopulateResellers(ddl_reseller, true);
                gv_customers.DataSource = null;
                gv_customers.DataBind();
            }


            protected void gv_bConfirm_Click(object sender, EventArgs e)
            {
                using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var confirmButton = sender as Button;
                    if (confirmButton == null) return;
                    var id = Convert.ToInt32(confirmButton.CommandArgument);
                    var request = _context.RechargeRequestBranches.FirstOrDefault(r => r.Id == id);
                    if (request != null)
                    {
                        //var boxCredit = Convert.ToDouble(_creditRepository.GetNetBoxCredit(Convert.ToInt32(request.BoxId)));
                        //if (boxCredit >= Convert.ToDouble(request.Amount)){
                        request.IsApproved = true;
                        request.RejectReason = string.Empty;
                        switch (request.CreditOrVoice)
                        {
                            case 0:
                                _branchrvoiceCredit.SaveVoice(Convert.ToInt32(request.BranchId), Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(request.Amount), "Recharge Branch Requests", DateTime.Now.AddHours());
                                break;
                            case 1:
                                _branchCredit.Save(Convert.ToInt32(request.BranchId), Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(request.Amount), "Recharge Branch Requests", DateTime.Now.AddHours());
                                break;
                            case 2:
                                var userTransaction = new UsersTransaction
                                {
                                    CreationDate = DateTime.Now.AddHours(),
                                    DepitAmmount = 0,
                                    CreditAmmount = Convert.ToDouble(request.Amount),
                                    IsInvoice = false,
                                    BranchID = Convert.ToInt32(request.BranchId),
                                    Total = Billing.GetLastBalance(Convert.ToInt32(request.BranchId), "Branch") - Convert.ToDouble(request.Amount),
                                    Description = "Recharge Branch Requests",
                                    UserId = Convert.ToInt32(Session["User_ID"])
                                };
                                _context.UsersTransactions.InsertOnSubmit(userTransaction);
                                _context.SubmitChanges();
                                break;
                        }

                        var userid = Convert.ToInt32(Session["User_ID"]);
                        var user = _context.Users.FirstOrDefault(a => a.ID == userid);
                        var notesinbox = user != null ? "Recharge Branch Requests" + Tokens.br + Tokens.Branch + " : " + request.Branch.BranchName + Tokens.br + Tokens.UserName + ": " + user.UserName : "Recharge Branch Requests" + Tokens.Reseller + " : " + request.Branch.BranchName;
                        var result = _creditRepository.SaveBox(Convert.ToInt32(request.BoxId), Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(request.Amount), notesinbox, DateTime.Now.AddHours(), null, request.Id);

                        switch (result)
                        {
                            case SaveBoxResult.Saved:
                                //if(SaveResult.Saved)return;
                                l_message.Text = Tokens.Saved;
                                _context.SubmitChanges();
                                if (request.Branch != null && !string.IsNullOrEmpty(request.Branch.Mobile1))
                                {
                                    var message = SendSms.SendSmsByNotification(_context, request.Branch.Mobile1, 11);
                                    if (!string.IsNullOrEmpty(message))
                                    {
                                        var myscript = "window.open('" + message + "')";
                                        ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                                    }
                                }
                                break;
                            case SaveBoxResult.NoCredit:
                                l_message.Text = Tokens.NotEnoughtCreditMsg;
                                break;
                        }
                        FindRequests();

                    }
                }
            }


            protected void btn_reject_Click(object sender, EventArgs e)
            {
                using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var request = _context.RechargeRequestBranches.FirstOrDefault(r => r.Id == Convert.ToInt32(hf_rejectionId.Value));
                    if (request != null)
                    {
                        request.RejectReason = txt_RejectReason.Text;
                        request.IsApproved = false;
                    }

                    _context.SubmitChanges();
                    FindRequests();
                    mv_container.SetActiveView(v_results);
                    txt_RejectReason.Text = string.Empty;
                    l_message.Text = Tokens.Rejected;
                }
            }
        }
    }
 