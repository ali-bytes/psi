using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.ExpensesRevenues;
using NewIspNL.Services.Payment;
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchDailyReport : CustomPage
    {
          readonly DemandsSearchService _demandsSearch;

            readonly ExpensesRevenuesSrevice _expensesRevenuesSrevice;

            readonly PaymentService _paymentService;
            readonly IspDomian _domian;


            public  BranchDailyReport()
            {
                ISPDataContext context = IspDataContext;
                _demandsSearch = new DemandsSearchService(context);
                _paymentService = new PaymentService(context);
                _expensesRevenuesSrevice = new ExpensesRevenuesSrevice(context);
                _domian = new IspDomian(context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                TbDate.Text = DateTime.Now.AddHours().ToShortDateString();
                _domian.PopulateBranches(DdlBranch, true);
            }


            protected void Search(object sender, EventArgs e)
            {
                var date = Convert.ToDateTime(TbDate.Text);
                var searchModel = new BasicSearchModel
                {
                    Date = date,
                    BranchId = Convert.ToInt32(DdlBranch.SelectedItem.Value)
                };


                var demands = _demandsSearch.PaidDemandsForBranchTemplates(searchModel);//.Where(a=>a.Reseller=="-");
                GvCustomers.DataSource = demands;
                GvCustomers.DataBind();


                var resellersPayments = _paymentService.ResellersPaymentsForBranchTemplates(searchModel);
                GvResellers.DataSource = resellersPayments;
                GvResellers.DataBind();

                var rpq = _paymentService.ResellerCredits(searchModel);
                GVRPR.DataSource = rpq;
                GVRPR.DataBind();

                var bpq = _paymentService.BranchCredits(searchModel);
                GVBPR.DataSource = bpq;
                GVBPR.DataBind();

                var branchesPayments = _paymentService.BranchesPaymentsForBranchTemplates(searchModel);
                GvBranches.DataSource = branchesPayments;
                GvBranches.DataBind();

                var revs = _expensesRevenuesSrevice.RevenuesModelsForBranch(searchModel);
                GvRevenues.DataSource = revs;
                GvRevenues.DataBind();

                var exps = _expensesRevenuesSrevice.ExpensesForBranchModels(searchModel);
                GvExpenses.DataSource = exps;
                GvExpenses.DataBind();

               var externalToltal= Bind_grd_Transactions(searchModel);

                var reports = new Dictionary<string, string>();
                var totalDemands = demands.Sum(x => x.Amount);
                var totalResellersPayments = resellersPayments.Sum(x => x.Amount);
                var totalBranchesPayments = branchesPayments.Sum(x => x.Amount);
                var revsTotal = revs.Sum(x => x.Amount);
                var expsTotal = exps.Sum(x => x.Amount);
                var rpqTotal = rpq.Sum(x => x.TAmount);
                var bpqTotal = bpq.Sum(a => a.TAmount);
                var allRevs = totalDemands + totalResellersPayments + totalBranchesPayments + revsTotal + rpqTotal + bpqTotal + externalToltal;
                reports.Add(Tokens.RevenuesTotal, Helper.FixNumberFormat(allRevs));
                reports.Add(Tokens.Expenses, Helper.FixNumberFormat(expsTotal));
                reports.Add(Tokens.Net, Helper.FixNumberFormat(allRevs - expsTotal));

                GvResults.DataSource = reports;
                GvResults.DataBind();

                
            }
            decimal Bind_grd_Transactions(BasicSearchModel conditions)
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = dataContext.CustomerPayments.OrderByDescending(x => x.Time)
                        .Select(x => new
                        {
                            x.User.UserName,
                            x.Notes,
                            x.Box.BoxName,
                            x.CustomerName,
                            x.CustomerTelephone,
                            x.InvoiceAmount,
                            x.BoxAmount,
                            x.ID,
                            x.VoiceCompany.CompanyName,
                            x.BoxId,
                            x.Time,
                            x.VoiceCompanyId,
                            x.User.BranchID
                            
                        }).ToList();
                    if (conditions.Date != null)
                    {
                        query = query.Where(s => s.Time >= conditions.Date).ToList();
                    }
                    if (conditions.BranchId != null)
                    {
                        query = query.Where(s => s.BranchID <= conditions.BranchId).ToList();
                    }
                  
                    grd_Transactions.DataSource = query;
                    grd_Transactions.DataBind();
                    return query.Sum(x => x.InvoiceAmount);

                }
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

            protected void btnExport_Click(object sender, EventArgs e)
            {
                var gridlist = new GridView[] { GvCustomers, GvResellers, GvBranches, GvRevenues, GvExpenses, GvResults };
                GridHelper.Export("BranchDailyReport.xls", gridlist);
            }
        }
    }
 