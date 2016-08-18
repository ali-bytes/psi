using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.ExpensesRevenues;
using NewIspNL.Services.Payment;
using Resources;

namespace NewIspNL.Pages
{
    public partial class MonthlyReport : CustomPage
    {
      
    readonly DemandsSearchService _demandsSearch;

    readonly ExpensesRevenuesSrevice _expensesRevenuesSrevice;

    readonly PaymentService _paymentService;


    public MonthlyReport()
    {
        ISPDataContext context = IspDataContext;
        _demandsSearch = new DemandsSearchService(context);
        _paymentService = new PaymentService(context);
        _expensesRevenuesSrevice = new ExpensesRevenuesSrevice(context);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        var now = DateTime.Now.AddHours();
        var year = now.Year;
        var month = now.Month;
        var start = new DateTime(year, month, 1);
        var end = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        TbFrom.Text = start.ToShortDateString();
        TbTo.Text = end.ToShortDateString();
    }


    protected void Search(object sender, EventArgs e){
        var startAt = Convert.ToDateTime(TbFrom.Text);
        var endAt = Convert.ToDateTime(TbTo.Text);
        var demands = _demandsSearch.PaidDemandsTemplates(startAt, endAt).Where(a=>string.IsNullOrWhiteSpace(a.Reseller));

        GvCustomers.DataSource = demands;
        GvCustomers.DataBind();

        var resellersPayments = _paymentService.ResellersPaymentsTemplates(startAt, endAt);
        GvResellers.DataSource = resellersPayments;
        GvResellers.DataBind();

        var branchesPayments = _paymentService.BranchesPaymentsTemplates(startAt, endAt);
        GvBranches.DataSource = branchesPayments;
        GvBranches.DataBind();

        var revs = _expensesRevenuesSrevice.RevenuesModels(startAt, endAt);
        GvRevenues.DataSource = revs;
        GvRevenues.DataBind();

        var exps = _expensesRevenuesSrevice.ExpensesModels(startAt, endAt);
        GvExpenses.DataSource = exps;
        GvExpenses.DataBind();

        var RPQ = _paymentService.ResellerCredits(startAt, endAt);
        GVRPR.DataSource = RPQ;
        GVRPR.DataBind();

        var BPQ = _paymentService.BranchCredits(startAt, endAt);
        GVBPR.DataSource = BPQ;
        GVBPR.DataBind();

        var reports = new Dictionary<string, string>();
        var totalDemands = demands.Sum(x => x.Amount);
        var totalResellersPayments = resellersPayments.Sum(x => x.Amount);
        var totalBranchesPayments = branchesPayments.Sum(x => x.Amount);
        var revsTotal = revs.Sum(x => x.Amount);
        var expsTotal = exps.Sum(x => x.Amount);
        var RPQTotal = RPQ.Sum(x => x.TAmount);
        var BPQTotla = BPQ.Sum(x => x.TAmount);
        var allRevs = totalDemands + totalResellersPayments + totalBranchesPayments + revsTotal+RPQTotal+BPQTotla;
        reports.Add(Tokens.RevenuesTotal, Helper.FixNumberFormat(allRevs));
        reports.Add(Tokens.Expenses, Helper.FixNumberFormat(expsTotal));
        reports.Add(Tokens.Net, Helper.FixNumberFormat(allRevs - expsTotal));
        GvResults.DataSource = reports;
        GvResults.DataBind();
    }


    protected void BindCustomersNumber(object sender, EventArgs e){
        Helper.GridViewNumbering(GvCustomers, "LNo");
    }


    protected void BindResellersNumber(object sender, EventArgs e){
        Helper.GridViewNumbering(GvResellers, "LNo");
    }


    protected void BindBranchesNumber(object sender, EventArgs e){
        Helper.GridViewNumbering(GvBranches, "LNo");
    }


    protected void NumberRevs(object sender, EventArgs e){
        Helper.GridViewNumbering(GvRevenues, "LNo");
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        var gridlist = new GridView[] { GvCustomers, GvResellers, GvBranches,GVRPR,GVBPR, GvRevenues, GvExpenses, GvResults };
        GridHelper.Export("MonthlyReport.xls", gridlist);
    }
}
}