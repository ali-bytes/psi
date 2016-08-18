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
    public partial class BranchesTransformation : CustomPage
    {
        private readonly IspDomian _domian;
    public List<Target> Targets;
    readonly IBranchCreditVoiceRepository _branchvoiceCredit;
    readonly BranchCreditRepository _branchCredit;

    public BranchesTransformation()
    {
        _domian=new IspDomian(IspDataContext);
        _branchvoiceCredit = new BranchCreditVoiceRepository();
        _branchCredit = new BranchCreditRepository();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        lbltitle.Text =Tokens.Request+@" "+ Tokens.CreditTransfer;
        PopulateList();
        _domian.PopulateBranches(ddlBranches, true);
        PopulateFrom();
    }

    void PopulateList()
    {
        Targets = new List<Target>()
        {
            new Target() {Id = null, Name = Tokens.Chose},
            new Target() {Id = 1, Name = Tokens.BranchPaymentCredit},
            new Target() {Id = 2, Name = Tokens.BranchVoiceCredit},
            new Target() {Id = 3, Name = Tokens.BranchBalanceSheet}
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
            if (ddlBranches.SelectedIndex <= 0)
            {
                message.InnerHtml = "أختر فرع";
                message.Attributes.Add("class", "alert alert-danger");
                return;
            }
            if (ddlTransferFrom.SelectedIndex <= 0) return;
            var fromId = Convert.ToInt32(ddlTransferFrom.SelectedItem.Value);
            var brancheId = Convert.ToInt32(ddlBranches.SelectedItem.Value);
            PopulateTo(fromId);
            switch (fromId)
            {
                case 1:
                    var credit = _branchCredit.GetNetCredit(brancheId);
                    lblcredit.Text = Helper.FixNumberFormat(credit);
                    var branchrequests = context.WorkOrderRequests.Where(a => a.WorkOrder.BranchID == brancheId && a.RSID == 3 && a.RequestID == 11).ToList();
                    hdnOldAmount.Value = (credit - branchrequests.Sum(a => a.Total)).ToString();
                    break;
                case 2:
                    var cred = _branchvoiceCredit.GetNetCredit(brancheId);
                    lblcredit.Text = Helper.FixNumberFormat(cred);
                    var reseRequests =
                        context.RechargeRequestBranches.Where(a => a.BranchId == brancheId && a.IsApproved == null).ToList();
                    var amount = reseRequests.Sum(s => s.Amount);
                    hdnOldAmount.Value = (cred - amount).ToString();
                    break;
                case 3:
                    var cre = Billing.GetLastBalance(brancheId, "Branch");
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
            if (ddlTransferFrom.SelectedIndex <= 0 || ddlBranches.SelectedIndex <= 0 || ddlTransferTo.SelectedIndex <= 0 || string.IsNullOrEmpty(hdnOldAmount.Value)) return;
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
            var BrancheId = Convert.ToInt32(ddlBranches.SelectedItem.Value);
            var userId = Convert.ToInt32(Session["User_ID"]);
            var credit = Convert.ToDecimal(lblcredit.Text);
            var check_old_requests =
                context.BranchTransformationRequests.Where(
                    x => x.BranchId == BrancheId && x.TransferFrom == fromId && x.Status == null)
                    .Select(x => x.Amount).ToList();
            var total = check_old_requests.Sum();

            var check = (total + amo);

            if (check > credit)
            {

                message.InnerHtml = Tokens.CreditIsntEnough+" "+"بسبب وجود طلبات سابقة ";
                message.Attributes.Add("class", "alert alert-danger");
                return;

            }


            var newRequest = new BranchTransformationRequest
            {
                BranchId = BrancheId,
                TransferFrom = fromId,
                TransferTo = toId,
                Amount = amo,
                UserId = userId,
                Date = DateTime.Now.AddHours()
            };
            context.BranchTransformationRequests.InsertOnSubmit(newRequest);
            context.SubmitChanges();
            message.InnerHtml = Tokens.Request_Added_successfully;
            message.Attributes.Add("class", "alert alert-success");
            txtAmount.Text = string.Empty;
        }
    }
}
    
}