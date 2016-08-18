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
    public partial class ResellersTransformation : CustomPage
    {
     
    private readonly IspDomian _domian;
    public List<Target> Targets;
    readonly IResellerCreditVoiceRepository _resellervoiceCredit;
    readonly IResellerCreditRepository _resellerCredit;

    public ResellersTransformation()
    {
        _domian=new IspDomian(IspDataContext);
        _resellervoiceCredit=new ResellerCreditVoiceRepository();
        _resellerCredit=new ResellerCreditRepository();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        lbltitle.Text =Tokens.Request+@" "+ Tokens.CreditTransfer;
        PopulateList();
        _domian.PopulateResellers(ddlReseller, true);
        PopulateFrom();
    }

    void PopulateList()
    {
        Targets = new List<Target>()
        {
            new Target() {Id = null, Name = Tokens.Chose},
            new Target() {Id = 1, Name = Tokens.ResellerPaymentCredit},
            new Target() {Id = 2, Name = Tokens.ResellerVoiceCredit},
            new Target() {Id = 3, Name = Tokens.ResellerBalanceSheet}
        };
    }
    void PopulateTo(int id)
    {
        PopulateList();
        var lis = Targets.Where(a => a.Id != id).ToList();
        ddlTransferTo.DataSource = lis;
        ddlTransferTo.DataTextField = "Name";
        ddlTransferTo.DataValueField = "Id";
        ddlTransferTo.DataBind();

    }

    void PopulateFrom()
    {
        var list = Targets;
        ddlTransferFrom.DataSource = list;
        ddlTransferFrom.DataTextField = "Name";
        ddlTransferFrom.DataValueField = "Id";
        ddlTransferFrom.DataBind();
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (ddlReseller.SelectedIndex <= 0)
            {
                message.InnerHtml = Tokens.ChooseReseller;
                message.Attributes.Add("class", "alert alert-danger");
                return;
            }
            if (ddlTransferFrom.SelectedIndex <= 0) return;
            var fromId = Convert.ToInt32(ddlTransferFrom.SelectedItem.Value);
            var resellerId = Convert.ToInt32(ddlReseller.SelectedItem.Value);
            PopulateTo(fromId);
            switch (fromId)
            {
                case 1:
                    var credit = _resellerCredit.GetNetCredit(resellerId);
                    lblcredit.Text = Helper.FixNumberFormat(credit);
                    var resellerrequests = context.WorkOrderRequests.Where(a => a.WorkOrder.ResellerID == resellerId && a.RSID == 3 && a.RequestID == 11).ToList();
                    hdnOldAmount.Value = (credit - resellerrequests.Sum(a => a.Total)).ToString();
                    break;
                case 2:
                    var cred = _resellervoiceCredit.GetNetCredit(resellerId);
                    lblcredit.Text = Helper.FixNumberFormat(cred);
                    var reseRequests =
                        context.RechargeRequests.Where(a => a.ResellerId == resellerId && a.IsApproved == null).ToList();
                    var amount = reseRequests.Sum(s => s.Amount);
                    hdnOldAmount.Value = (cred - amount).ToString();
                    break;
                case 3:
                    var cre = Billing.GetLastBalance(resellerId, "Reseller");
                    var l=Helper.FixNumberFormat(cre*-1);
                    lblcredit.Text = l;
                    hdnOldAmount.Value = l;
                    break;
            }
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (Session["User_ID"] == null) Response.Redirect("default.aspx");
            if (ddlTransferFrom.SelectedIndex <= 0 || ddlReseller.SelectedIndex <= 0 || ddlTransferTo.SelectedIndex<=0 || string.IsNullOrEmpty(hdnOldAmount.Value)) return;
            var oldRequests = Convert.ToDecimal(hdnOldAmount.Value);
            if (oldRequests <= 0)
            {
                message.InnerHtml = Tokens.CreditIsntEnough;
                message.Attributes.Add("class","alert alert-danger");
                return;
            }
            var amo = Convert.ToDecimal(txtAmount.Text);
            if ((oldRequests - amo) < 0)
            {
                message.InnerHtml = Tokens.CreditIsntEnough;
                message.Attributes.Add("class", "alert alert-danger");
                return;
            }


            var fromId = Convert.ToInt32(ddlTransferFrom.SelectedItem.Value);
            var toId = Convert.ToInt32(ddlTransferTo.SelectedItem.Value);
            var resellerId = Convert.ToInt32(ddlReseller.SelectedItem.Value);
            var userId = Convert.ToInt32(Session["User_ID"]);
            var credit = Convert.ToDecimal(lblcredit.Text);
            var check_old_requests =
                context.ResellerTransformationRequests.Where(
                    x => x.ResellerId == resellerId && x.TransferFrom == fromId&&x.Status==null )
                    .Select(x => x.Amount).ToList();
            var total = check_old_requests.Sum();

            var check = (total + amo);

            if (check > credit)
            {

                message.InnerHtml = Tokens.CreditIsntEnough+" "+"بسبب وجود طلبات سابقة ";
                message.Attributes.Add("class", "alert alert-danger");
                return;

            }







            var newRequest = new ResellerTransformationRequest
            {
                ResellerId = resellerId,
                TransferFrom = fromId,
                TransferTo = toId,
                Amount = amo,
                UserId = userId,
                date = DateTime.Now.AddHours()
            };
            context.ResellerTransformationRequests.InsertOnSubmit(newRequest);
            context.SubmitChanges();
            message.InnerHtml = Tokens.Request_Added_successfully;
            message.Attributes.Add("class", "alert alert-success");
            txtAmount.Text = string.Empty;
        }
    }
}
    public class Target
{
    public int? Id { set; get; }
    public string Name { set; get; }
}
}