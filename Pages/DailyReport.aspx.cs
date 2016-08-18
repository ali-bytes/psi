using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Helpers;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.ExpensesRevenues;
using NewIspNL.Services.Payment;
using Resources;

namespace NewIspNL.Pages
{
    public partial class DailyReport : CustomPage
    {
       
            readonly DemandsSearchService _demandsSearch;

            readonly ExpensesRevenuesSrevice _expensesRevenuesSrevice;

            readonly PaymentService _paymentService;

            //readonly ISPDataContext context;

            public  DailyReport()
            {
                var context = IspDataContext;
                _demandsSearch = new DemandsSearchService(context);
                _paymentService = new PaymentService(context);
                _expensesRevenuesSrevice = new ExpensesRevenuesSrevice(context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                TbDate.Text = DateTime.Now.AddHours().ToShortDateString();
            }


            protected void Search(object sender, EventArgs e)
            {
                //using(var context = new ISPDataContext()){
                var userId = Convert.ToInt32(Session["User_ID"]);
                var date = Convert.ToDateTime(TbDate.Text);
                // var user = context.Users.FirstOrDefault(a => a.ID == userId);
                var demands = _demandsSearch.PaidDemandsTemplates(date, userId).OrderBy(a => a.PaymentDate);//.Where(a=>a.Reseller=="-")

                GvCustomers.DataSource = demands;
                GvCustomers.DataBind();

                var resellersPayments = _paymentService.ResellersPaymentsTemplates(date, userId);
                GvResellers.DataSource = resellersPayments;
                GvResellers.DataBind();

                var branchesPayments = _paymentService.BranchesPaymentsTemplates(date, userId);
                GvBranches.DataSource = branchesPayments;
                GvBranches.DataBind();

                var revs = _expensesRevenuesSrevice.RevenuesModels(date, userId);
                GvRevenues.DataSource = revs;
                GvRevenues.DataBind();

                var exps = _expensesRevenuesSrevice.ExpensesModels(date, userId);
                GvExpenses.DataSource = exps;
                GvExpenses.DataBind();
                // Edited by ashraf
                // todo: review code
                var rpq = _paymentService.ResellerCredits(date, userId);
                GVRPR.DataSource = rpq;
                GVRPR.DataBind();

                var bpq = _paymentService.BranchCredits(date, userId);
                GVBPR.DataSource = bpq;
                GVBPR.DataBind();

                var reports = new Dictionary<string, string>();
                var totalDemands = demands.Sum(x => x.Amount);
                var totalResellersPayments = resellersPayments.Sum(x => x.Amount);
                var totalBranchesPayments = branchesPayments.Sum(x => x.Amount);
                var revsTotal = revs.Sum(x => x.Amount);
                var expsTotal = exps.Sum(x => x.Amount);
                var totalrpr = rpq.Sum(x => x.TAmount);
                var totalbpr = bpq.Sum(x => x.TAmount);
                var allRevs = totalDemands + totalResellersPayments + totalBranchesPayments + revsTotal + totalrpr + totalbpr;

                reports.Add(Tokens.RevenuesTotal, Helper.FixNumberFormat(allRevs));
                reports.Add(Tokens.Expenses, Helper.FixNumberFormat(expsTotal));
                reports.Add(Tokens.Net, Helper.FixNumberFormat(allRevs - expsTotal));

                GvResults.DataSource = reports;
                GvResults.DataBind();
                // }
            }


            protected void BindCustomersNumber(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvCustomers, "LNo");
            }


            protected void BindResellersNumber(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvResellers, "LNo");
            }


            protected void BindBranchesNumber(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvBranches, "LNo");
            }


            protected void NumberRevs(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(GvRevenues, "LNo");
            }
            protected void btnExport_click(object sender, EventArgs e)
            {
                //creating the array of GridViews and calling the Export function
                var gvList = new GridView[] { GvCustomers, GvResellers, GvBranches, GvRevenues, GvExpenses, GVRPR, GVBPR, GvResults };
                GridHelper.Export("DailyReport.xls", gvList);
            }

        }
    }
