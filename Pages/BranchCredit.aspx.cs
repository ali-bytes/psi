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
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using NewIspNL.Templates;
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchCredit : CustomPage
    {
           //readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

            public List<Branch> Branches { get; set; }
            readonly DemandsSearchService _searchService;
            readonly BranchCreditRepository _creditRepository;
            //readonly IResellerCreditRepository _creditRepository;
            readonly IBranchCreditVoiceRepository _creditVoice;
            //readonly IResellerCreditVoiceRepository _creditVoice;

            public  BranchCredit()
            {
                _searchService = new DemandsSearchService(IspDataContext);
                _creditRepository = new BranchCreditRepository();
                _creditVoice = new BranchCreditVoiceRepository();
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                BuildUponUserType();
            }


            void BuildUponUserType()
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var id = Convert.ToInt32(Session["User_ID"]);
                    var user = db.Users.FirstOrDefault(u => u.ID == id);
                    if (user == null) return;
                    var branches = new List<Branch>();
                    switch (user.GroupID)
                    {
                        case 1: // admin
                            branches = GetBranches(id);
                            Button1.Visible = true;
                            break;
                        case 4:
                            var all = db.UserBranches.Where(b => b.UserID == id);
                            if (all.Any())
                            {
                                all.ToList().ForEach(x => branches.Add(x.Branch));
                            }
                            break;
                        default:
                            branches = GetBranches(id);
                            break;
                    }
                    Branches = branches;
                    ddl_branchs.DataSource = branches;
                    ddl_branchs.DataValueField = "ID";
                    ddl_branchs.DataTextField = "BranchName";
                    ddl_branchs.DataBind();
                    if (user.GroupID == 1) Helper.AddDefaultItem(ddl_branchs, Tokens.All);
                    // Helper.AddDefaultItem(ddl_branchs);
                }
            }


            List<Branch> GetBranches(int? id)
            {
                using (var db1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var branches = new List<Branch>();
                    if (id != null)
                    {
                        var user = db1.Users.FirstOrDefault(x => x.ID == id);
                        if (user == null)
                        {
                            Response.Redirect("default.aspx");
                        }
                        else
                        {
                            if (user.GroupID != 1)
                            {
                                branches.Add(user.Branch);
                            }
                            else
                            {
                                branches = db1.Branches.ToList();
                            }
                        }
                    }
                    return branches;
                }
            }

            protected void b_addRequest1_Click(object sender, EventArgs e)
            {
                using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var allbranches = _context.Branches.ToList();

                    foreach (var branch in allbranches)
                    {
                        var lastCredit =
                            _context.BranchCredits.Where(c => c.BranchId == branch.ID)
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefault();
                        if (lastCredit != null) lastCredit.Net = 0;
                        _context.SubmitChanges();
                    }

                }
            }
            protected void BSearch_OnClick(object sender, EventArgs e)
            {
                var credits = new List<ResellerCreditTemp>();
                var credits2 = new List<ResellerCreditTemp>();
                var from = rblFrom.SelectedIndex;
                if (ddl_branchs.SelectedItem.Text == Tokens.All)
                {
                    var currentBranches = ddl_branchs.Items.Cast<ListItem>().Where(item => item.Text != "empty" && item.Text != Tokens.All).ToDictionary(item => Convert.ToInt32(item.Value), item => item.Text);
                    credits = currentBranches
                        .Select(x => new ResellerCreditTemp
                        {
                            Credit = GetCredit(from, x.Key),//Billing.SubString2Digits(Billing.GetLastBalance(x.Key, "Branch")),
                            Reseller = x.Value,
                            Branchid = x.Key
                        }).ToList();


                    var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


                    var sp = context2.SPoptionReselleraccounts.Select(z => z).ToList();
                    foreach (var i in credits)
                    {
                        var add = new ResellerCreditTemp();
                        var newlist = new List<DemandPreviewModel>();
                        var searchDemands = _searchService.BranchUnpaidDemandsPreview(new BasicSearchModel
                        {
                            BranchPaid = false,
                            BranchId = i.Branchid,
                            Month = Convert.ToInt32(DateTime.Now.Month),
                            Year = Convert.ToInt32(DateTime.Now.Year),
                            WithBranchDiscount = true 

                        });
                        foreach (var ii in sp)
                        {
                            var data = searchDemands.Where(a => a.Provider == ii.ServiceProvider.SPName).ToList();
                            newlist.AddRange(data);




                        }
                        add.Credit = i.Credit;
                        add.Reseller = i.Reseller;
                        add.currentpill = (double)newlist.Sum(x => x.BranchNet);

                        add.MenuResellerCredit = Math.Round(
                          Convert.ToDouble(Convert.ToDecimal(i.Credit) +
                                           Convert.ToDecimal(newlist.Sum(x => x.BranchNet))), 2);
                        add.Branchname = i.Branchname;
                        credits2.Add(add);
                    }


                    var total = credits.Sum(s => s.Credit);
                    LTotal.Text = string.Format("{0} :  {1}", Tokens.Total, Billing.SubString2Digits(total));
                }
                else
                {
                    credits.Add(new ResellerCreditTemp
                    {
                        Credit = GetCredit(from, Convert.ToInt32(ddl_branchs.SelectedItem.Value)),//Billing.SubString2Digits(Billing.GetLastBalance(Convert.ToInt32(ddl_branchs.SelectedItem.Value), "Branch")),
                        Reseller = ddl_branchs.SelectedItem.Text,
                        Branchid = Convert.ToInt32(ddl_branchs.SelectedItem.Value)
                    });



                    var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


                    var sp = context2.SPoptionReselleraccounts.Select(z => z).ToList();
                    foreach (var i in credits)
                    {
                        var add = new ResellerCreditTemp();
                        var newlist = new List<DemandPreviewModel>();
                        var searchDemands = _searchService.BranchUnpaidDemandsPreview(new BasicSearchModel
                        {
                            BranchPaid = false,
                            BranchId = i.Branchid,
                            Month = Convert.ToInt32(DateTime.Now.Month),
                            Year = Convert.ToInt32(DateTime.Now.Year),
                            WithBranchDiscount = true

                        });
                        foreach (var ii in sp)
                        {
                            var data = searchDemands.Where(a => a.Provider == ii.ServiceProvider.SPName ).ToList();
                            newlist.AddRange(data);




                        }
                        add.Credit = i.Credit;
                        add.Reseller = i.Reseller;
                        add.currentpill = (double)newlist.Sum(x => x.BranchNet);

                        add.MenuResellerCredit = Math.Round(
                          Convert.ToDouble(Convert.ToDecimal(i.Credit) +
                                           Convert.ToDecimal(newlist.Sum(x => x.BranchNet))), 2);
                        add.Branchname = i.Branchname;
                        credits2.Add(add);
                    }


                    LTotal.Text = string.Empty;
                }
                if (rblFrom.SelectedIndex != 1)
                {
                    one.Visible = true;
                    all.Visible = false;
                    GCredits.DataSource = credits.OrderByDescending(x => x.Credit);
                    GCredits.DataBind();
                }
                else
                {
                    one.Visible = false;
                    all.Visible = true;
                    Allreseleer.DataSource = credits2.OrderByDescending(x => x.Credit);
                    Allreseleer.DataBind();
                }
            }
            public double GetCredit(int From, int BranchId)
            {
                switch (From)
                {
                    case 0:
                        var BranchCredit = Convert.ToDouble(_creditRepository.GetNetCredit(BranchId));
                        return BranchCredit;
                    //break;
                    case 1:
                        var UserTransaction = Billing.SubString2Digits(Billing.GetLastBalance(BranchId, "Branch"));
                        return UserTransaction;

                    case 2:
                        var ResellerCreditVoice = Convert.ToDouble(_creditVoice.GetNetCredit(BranchId));
                        return ResellerCreditVoice;

                    /*case 3:
                        var UpdateResellerBs = _context.UpdatedResellerBs.Where(p => p.BranchId == BranchId).Sum(p => p.InvoiceAfterReview);
                        return Convert.ToDouble(UpdateResellerBs);*/
                    default:
                        return 0;

                }
            }
        }
    }
 