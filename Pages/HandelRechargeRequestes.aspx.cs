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
    public partial class HandelRechargeRequestes : CustomPage
    {
        
            readonly IBoxCreditRepository _creditRepository;

            readonly IResellerCreditVoiceRepository _resellervoiceCredit;

            readonly IResellerCreditRepository _resellerCredit;
            public  HandelRechargeRequestes()
            {
                _creditRepository = new BoxCreditRepository();
                _resellervoiceCredit = new ResellerCreditVoiceRepository();
                _resellerCredit = new ResellerCreditRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                mv_container.SetActiveView(v_search);
                if (IsPostBack) return;
                CheckOnReseller();
            }


            void CheckOnReseller()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var resellers = DataLevelClass.GetUserReseller();
                    var resellerHasRequest = new List<User>();
                    foreach (var item in resellers)
                    {
                        var check = context.RechargeRequests.FirstOrDefault(ch => ch.ResellerId == item.ID && ch.IsApproved == null);
                        if (check != null)
                        {
                            resellerHasRequest.Add(item);
                        }
                    }
                    ddl_reseller.DataSource = resellerHasRequest;
                    ddl_reseller.DataTextField = "UserName";
                    ddl_reseller.DataValueField = "ID";
                    ddl_reseller.DataBind();
                    Helper.AddDefaultItem(ddl_reseller);

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
                                lbl.Text = Tokens.ResellerVoiceCredit;
                                break;
                            case "1":
                                lbl.Text = Tokens.ResellerPaymentCredit;
                                break;
                            case "2":
                                lbl.Text = Tokens.AddToResellerBalanceSheet;
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
                    var requests = context.RechargeRequests
                        .Where(r =>
                            r.IsApproved == null && r.ResellerId == Convert.ToInt32(HiddenField1.Value))
                        .Select(x => new
                        {
                            x.ID,
                            x.BoxId,
                            x.Box.BoxName,
                            x.ResellerId,
                            x.User.UserName,
                            x.DepositorName,
                            x.Amount,
                            x.Time,
                            Url = string.Format("../Attachments/{0}", x.RecieptImage),
                            x.CreditORVoice
                        }).ToList();
                    gv_customers.DataSource = requests;
                    gv_customers.DataBind();
                    mv_container.SetActiveView(v_results);
                }
            }


            protected void b_changeReseller_Click(object sender, EventArgs e)
            {
                CheckOnReseller();
                mv_container.SetActiveView(v_search);
                gv_customers.DataSource = null;
                gv_customers.DataBind();
            }
          

            protected void gv_bConfirm_Click(object sender, EventArgs e)
            {
               
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var confirmButton = sender as Button;
                    if (confirmButton == null) return;
                    var id = Convert.ToInt32(confirmButton.CommandArgument);
                    var request = context.RechargeRequests.FirstOrDefault(r => r.ID == id);
                    if (request != null)
                    {
                        request.IsApproved = true;
                        request.RejectReason = string.Empty;
                        switch (request.CreditORVoice)
                        {
                            case 0:

                                _resellervoiceCredit.SaveVoice(Convert.ToInt32(request.ResellerId), Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(request.Amount), "Recharge Reseller Requests", DateTime.Now.AddHours(), request.ID);
                                break;
                            case 1:

                                _resellerCredit.Save(Convert.ToInt32(request.ResellerId), Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(request.Amount), "Recharge Reseller Requests", DateTime.Now.AddHours(), request.ID);
                                break;
                            case 2:
                                var userTransaction = new UsersTransaction
                                {
                                    CreationDate = DateTime.Now.AddHours(),
                                    DepitAmmount = 0,
                                    CreditAmmount = Convert.ToDouble(request.Amount),
                                    IsInvoice = false,
                                    ResellerID = Convert.ToInt32(request.ResellerId),
                                    Total = Billing.GetLastBalance(Convert.ToInt32(request.ResellerId), "Reseller") - Convert.ToDouble(request.Amount),
                                    Description = "Recharge Reseller Requests",
                                    UserId = Convert.ToInt32(Session["User_ID"])
                                };
                                context.UsersTransactions.InsertOnSubmit(userTransaction);
                                context.SubmitChanges();
                                break;
                        }

                        var userid = Convert.ToInt32(Session["User_ID"]);
                        var user = context.Users.FirstOrDefault(a => a.ID == userid);
                        var notesinbox = user != null ? "Recharge Reseller Requests" + " " + Tokens.Reseller + " : " + request.User.UserName + " " + Tokens.UserName + ": " + user.UserName : "Recharge Reseller Requests" + Tokens.Reseller + " : " + request.User.UserName;
                        var result = _creditRepository.SaveBox(Convert.ToInt32(request.BoxId), Convert.ToInt32(Session["User_ID"]), Convert.ToDecimal(request.Amount), notesinbox, DateTime.Now.AddHours(), request.ID, null);

                        switch (result)
                        {
                            case SaveBoxResult.Saved:
                                l_message.Text = Tokens.Saved;
                                context.SubmitChanges();
                                var option = context.Options.FirstOrDefault();
                                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                                {
                                    CenterMessage.SendRequestApproval(request.User.UserName, request.User.UserPhone,
                                        request.User.UserAddress, Convert.ToInt32(request.ResellerId),
                                        Tokens.RechargeAccountReseller, userid);
                                }
                                if (request.User != null && !string.IsNullOrEmpty(request.User.UserMobile))
                                {
                                    var message = SendSms.SendSmsByNotification(context, request.User.UserMobile, 10);
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
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var request = context.RechargeRequests.FirstOrDefault(r => r.ID == Convert.ToInt32(hf_rejectionId.Value));
                    if (request != null)
                    {
                        var userid = Convert.ToInt32(Session["User_ID"]);
                        request.RejectReason = txt_RejectReason.Text;
                        request.IsApproved = false;
                        var option = context.Options.FirstOrDefault();
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestReject(request.User.UserName, request.User.Phones.ToString(),
                                request.User.UserAddress, Convert.ToInt32(request.ResellerId), Tokens.RechargeAccountReseller,
                                txt_RejectReason.Text, userid);
                        }
                    }

                    context.SubmitChanges();
                    FindRequests();
                    mv_container.SetActiveView(v_results);
                    txt_RejectReason.Text = string.Empty;
                    l_message.Text = Tokens.Rejected;
                }
            }
        }
    }
 