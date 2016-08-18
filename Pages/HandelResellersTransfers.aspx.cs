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
    public partial class HandelResellersTransfers : CustomPage
    {
        
            readonly IResellerCreditVoiceRepository _resellervoiceCredit;
            readonly IResellerCreditRepository _resellerCredit;

            public  HandelResellersTransfers()
            {
                _resellervoiceCredit = new ResellerCreditVoiceRepository();
                _resellerCredit = new ResellerCreditRepository();
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                CheckOnReseller();
            }

            protected void Search_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    PopulateRequests(context);
                }
            }

            void PopulateRequests(ISPDataContext context)
            {
                if (ddlReseller.SelectedIndex < 0) return;
                var resellerId = Helper.GetDropValue(ddlReseller);
                var requests =
                    context.ResellerTransformationRequests.Where(a => a.ResellerId == resellerId && a.Status == null)
                        .Select(a => new
                        {
                            Reseller = a.User.UserName,
                            a.User1.UserName,
                            a.TransferFrom,
                            a.TransferTo,
                            RequestDate = a.date,
                            Amount = Helper.FixNumberFormat(a.Amount),
                            a.Id
                        }).ToList();
                gvRequests.DataSource = requests;
                gvRequests.DataBind();
            }
            protected void Confirm_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (Session["User_ID"] == null) Response.Redirect("default.aspx");
                    var confirmButton = sender as Button;
                    if (confirmButton == null) return;
                    var id = Convert.ToInt32(confirmButton.CommandArgument);
                    var userId = Convert.ToInt32(Session["User_Id"]);
                    var request = context.ResellerTransformationRequests.FirstOrDefault(a => a.Id == id);
                    if (request == null) return;
                    request.Status = true;
                    var fromText = PassText(request.TransferFrom);
                    var toText = PassText(request.TransferTo);
                    PutAmountIn(request.TransferFrom, toText, request.ResellerId, (request.Amount * -1), userId, context);
                    PutAmountIn(request.TransferTo, fromText, request.ResellerId, request.Amount, userId, context);
                    context.SubmitChanges();
                    message.InnerHtml = Tokens.Done;
                    message.Attributes.Add("class", "alert alert-success");
                    PopulateRequests(context);
                    try
                    {
                        var mes = string.Format("تم تحويل رصيد للموزع من {0} الى {1} , و قيمة التحويل {2}", fromText, toText, request.Amount);
                        CenterMessage.SendPublic(request.User.UserName, request.User.UserPhone, mes, userId, request.ResellerId);
                    }
                    catch (Exception ex)
                    {
                        message.InnerHtml = ex.Message;
                        message.Attributes.Add("class", "alert alert-danger");
                    }
                }
            }
            protected void Reject_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    //var confirmButton = sender as LinkButton;
                    //if (confirmButton == null) return;
                    var id = Convert.ToInt32(hf_rejectionId.Value);
                    var request = context.ResellerTransformationRequests.FirstOrDefault(a => a.Id == id);
                    if (request == null) return;
                    request.Status = false;
                    context.SubmitChanges();
                    message.InnerHtml = Tokens.Done;
                    message.Attributes.Add("class", "alert alert-success");
                    PopulateRequests(context);
                    try
                    {
                        var userId = Convert.ToInt32(Session["User_Id"]);
                        var fromText = PassText(request.TransferFrom);
                        var toText = PassText(request.TransferTo);
                        var mes = string.Format("تم رفض تحويل رصيد للموزع من {0} الى {1} " + "<br/>"+"سبب الرفض :" + txt_RejectReason.Text, fromText, toText);
                        CenterMessage.SendPublic(request.User.UserName, request.User.UserPhone, mes, userId, request.ResellerId);
                    }
                    catch (Exception ex)
                    {
                        message.InnerHtml = ex.Message;
                        message.Attributes.Add("class", "alert alert-danger");
                    }
                }
            }

            void PutAmountIn(int target, string targetName, int resellerId, decimal amount, int userId, ISPDataContext context)
            {
                switch (target)
                {
                    case 1:
                        if (amount < 0)
                        {
                            var credit = _resellerCredit.GetNetCredit(resellerId);
                            var resellerrequests =
                                context.WorkOrderRequests.Where(
                                    a => a.WorkOrder.ResellerID == resellerId && a.RSID == 3 && a.RequestID == 11).ToList();
                            var endResult = (credit - resellerrequests.Sum(a => a.Total));
                            if (endResult <= 0)
                            {
                                message.InnerHtml = Tokens.CreditIsntEnough;
                                message.Attributes.Add("class", "alert alert-danger");
                                break;
                            }
                        }
                        _resellerCredit.Save(Convert.ToInt32(resellerId), userId, amount, "Reseller Transformation Request :" + targetName,
                            DateTime.Now.AddHours());
                        break;
                    case 2:
                        if (amount < 0)
                        {
                            var cred = _resellervoiceCredit.GetNetCredit(resellerId);
                            var reseRequests =
                                context.RechargeRequests.Where(a => a.ResellerId == resellerId && a.IsApproved == null).ToList();
                            var amou = reseRequests.Sum(s => s.Amount);
                            var enres = (cred - amou);
                            if (enres <= 0)
                            {
                                message.InnerHtml = Tokens.CreditIsntEnough;
                                message.Attributes.Add("class", "alert alert-danger");
                                break;
                            }
                        }
                        _resellervoiceCredit.SaveVoice(Convert.ToInt32(resellerId), userId, amount,
                            "Reseller Transformation Request :" + targetName, DateTime.Now.AddHours());
                        break;

                    case 3:
                        var oldCredit = Billing.GetLastBalance(resellerId, "Reseller");
                        //if (oldCredit <= 0)
                        //{
                        //    message.InnerHtml = Tokens.CreditIsntEnough;
                        //    message.Attributes.Add("class", "alert alert-danger");
                        //    break;
                        //}
                        var userTransaction = new UsersTransaction
                        {
                            CreationDate = DateTime.Now.AddHours(),
                            IsInvoice = false,
                            ResellerID = resellerId,
                            Description = "Reseller Transformation Request :" + targetName,
                            UserId = userId
                        };
                        if (amount > 0)
                        {
                            userTransaction.DepitAmmount = 0;
                            userTransaction.CreditAmmount = Convert.ToDouble(amount);
                            userTransaction.Total = (oldCredit) - Convert.ToDouble(amount);
                        }
                        else
                        {
                            userTransaction.DepitAmmount = Convert.ToDouble(amount * -1);
                            userTransaction.CreditAmmount = 0;
                            userTransaction.Total = oldCredit - Convert.ToDouble(amount);
                        }
                        context.UsersTransactions.InsertOnSubmit(userTransaction);
                        context.SubmitChanges();
                        break;
                }
            }
            void CheckOnReseller()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var resellers = DataLevelClass.GetUserReseller();
                    var resellerHasRequest = new List<User>();
                    foreach (var item in resellers)
                    {
                        var item1 = item;
                        var check = context.ResellerTransformationRequests.FirstOrDefault(ch => ch.ResellerId == item1.ID && ch.Status == null);
                        if (check != null)
                        {
                            resellerHasRequest.Add(item);
                        }
                    }
                    ddlReseller.DataSource = resellerHasRequest;
                    ddlReseller.DataTextField = "UserName";
                    ddlReseller.DataValueField = "ID";
                    ddlReseller.DataBind();
                    Helper.AddDefaultItem(ddlReseller);
                }
            }

            protected void gv_customers_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gvRequests, "gv_lNumber");
                var rows = gvRequests.Rows;
                foreach (GridViewRow item in rows)
                {
                    var lblfr = item.FindControl("lblFrom") as Label;
                    var lblto = item.FindControl("lblTo") as Label;
                    if (lblfr != null && lblto != null)
                    {
                        lblfr.Text = PassText(Convert.ToInt32(lblfr.Text));
                        lblto.Text = PassText(Convert.ToInt32(lblto.Text));
                    }
                }
            }

            static string PassText(int tar)
            {
                var text = string.Empty;
                switch (tar)
                {
                    case 1:
                        text = Tokens.ResellerPaymentCredit;
                        break;
                    case 2:
                        text = Tokens.ResellerVoiceCredit;
                        break;
                    case 3:
                        text = Tokens.ResellerBalanceSheet;
                        break;
                }
                return text;
            }
        }
    }
 