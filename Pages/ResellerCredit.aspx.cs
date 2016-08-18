using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
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
    public partial class ResellerCredit :CustomPage
    {

        readonly DemandsSearchService _searchService;
    readonly IResellerCreditRepository _creditRepository;

    readonly IResellerCreditVoiceRepository _creditVoice;

    public ResellerCredit(){
        _creditRepository = new ResellerCreditRepository();
        _creditVoice=new ResellerCreditVoiceRepository();
        _searchService = new DemandsSearchService(IspDataContext);
    }


    #region Page Helpers


    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        BuildUponUserType();
        
    }


    List<User> GetAvailableResellers(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var id = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(u => u.ID == id);
            List<User> resellers = null;
            if(user != null){
                switch(user.GroupID){
                    case 1 : // admin
                        resellers = GetResellers(null);
                        Button1.Visible = true;
                        break;
                    case 6 : // reseller
                        resellers = context.Users.Where(u => u.ID == id).ToList();
                        break;
                    default :
                        resellers = GetResellers(id);
                        break;
                }
            }
            return resellers;
        }
    }


    List<User> GetResellers(int? id)
    {
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(id != null){
                var user = context.Users.FirstOrDefault(x => x.ID == id);
                if(user == null){
                    Response.Redirect("default.aspx");
                }
                var resellers = context.Users.Where(g => g.GroupID == 6 && g.BranchID == user.BranchID).ToList();
                return resellers;
            }
            return context.Users.Where(x => x.GroupID == 6).ToList();
        }
    }
    
    void BuildUponUserType()
    {
        var resellers = GetAvailableResellers();
        ddl_reseller.DataSource = resellers;
        ddl_reseller.DataValueField = "ID";
        ddl_reseller.DataTextField = "UserName";
        ddl_reseller.DataBind();
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var id = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(u => u.ID == id);
            if (user != null && user.GroupID == 1 || user.GroupID == 4)
            {
                Helper.AddDefaultItem(ddl_reseller, Tokens.All);
            }
        }
        

    }


    #endregion


    #region Events


    protected void b_addRequest_Click(object sender, EventArgs e)
    {
        sum.Visible = false;
        var credits = new List<ResellerCreditTemp>();
        var credits2 = new List<ResellerCreditTemp>();
       
        if (HiddenField2.Value == string.Empty && ddl_reseller.SelectedItem.Text == string.Empty && HiddenField1.Value == string.Empty)
        {
            VMessage.Text = Tokens.SelectOptionFirst;
            return;
        }
        VMessage.Text = string.Empty;
        var from = rblFrom.SelectedIndex;
        if (ddl_reseller.SelectedIndex == 0 && ddl_reseller.SelectedItem.ToString() == Tokens.All)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var id = Convert.ToInt32(Session["User_ID"]);
                var branchid = context.Users.FirstOrDefault(x => x.ID == id);
                if (branchid.GroupID == 4)
                {
                    credits = GetAvailableResellers()
                    .Select(x => new ResellerCreditTemp
                    {
                        Credit = GetCredit(from, x.ID),
                        Reseller = x.UserName,
                        Branchid=x.BranchID,
                        Resellerid = x.ID,
                        Branchname = context.Branches.Where(s => s.ID == x.BranchID).Select(s=>s.BranchName).FirstOrDefault()

                    }).Where(x => x.Branchid == branchid.BranchID).ToList();


                    var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


                    var sp = context2.SPoptionReselleraccounts.Select(z => z).ToList();
                    foreach (var i in credits)
                    {
                        var add = new ResellerCreditTemp();
                        var newlist = new List<DemandPreviewModel>();
                        var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
                        {
                            Paid = false,
                            ResellerId = i.Resellerid,
                            Month = Convert.ToInt32(DateTime.Now.Month),
                            Year = Convert.ToInt32(DateTime.Now.Year),
                            WithResellerDiscount = true

                        });
                        foreach (var ii in sp)
                        {
                            var data = searchDemands.Where(a => a.Provider == ii.ServiceProvider.SPName && a.Reseller == i.Reseller).ToList();
                            newlist.AddRange(data);




                        }
                        add.Credit = i.Credit;
                        add.Reseller = i.Reseller;
                        add.currentpill = (double)newlist.Sum(x => x.ResellerNet);

                        add.MenuResellerCredit = Math.Round(
                          Convert.ToDouble(Convert.ToDecimal(i.Credit) +
                                           Convert.ToDecimal(newlist.Sum(x => x.ResellerNet))), 2);
                        add.Branchname = i.Branchname;
                        credits2.Add(add);
                    }


                    var total = credits.Sum(s => s.Credit);
                    LTotal.Text = string.Format("{0} : {1}", Tokens.Total, total);
                }
                else if (branchid.GroupID == 1) { 
                    credits = GetAvailableResellers()
                        .Select(x => new ResellerCreditTemp
                        {
                            Credit = GetCredit(from, x.ID),
                            Reseller = x.UserName,
                            Resellerid = x.ID,

                            Branchname  = context.Branches.Where(s => s.ID == x.BranchID).Select(s => s.BranchName).FirstOrDefault()
                        }).ToList();
                  
                 
                

                    var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

          
                    var sp = context2.SPoptionReselleraccounts.Select(z => z).ToList();
                    foreach (var i in credits)
                    {
                        var add = new ResellerCreditTemp();
                        var newlist = new List<DemandPreviewModel>();
                        var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
                        {
                            Paid = false,
                            ResellerId = i.Resellerid,
                            Month = Convert.ToInt32(DateTime.Now.Month),
                            Year = Convert.ToInt32(DateTime.Now.Year),
                            WithResellerDiscount = true

                        });
                    foreach (var ii in sp)
                    {
                        var data = searchDemands.Where(a => a.Provider == ii.ServiceProvider.SPName&&a.Reseller==i.Reseller).ToList();
                        newlist.AddRange(data);

                      


                    }
                    add.Credit = i.Credit;
                    add.Reseller = i.Reseller;
                    add.currentpill = (double)newlist.Sum(x => x.ResellerNet);
                  
                    add.MenuResellerCredit = Math.Round(
                      Convert.ToDouble(Convert.ToDecimal(i.Credit )+
                                       Convert.ToDecimal(newlist.Sum(x => x.ResellerNet))), 2);
                    add.Branchname = i.Branchname;
                    credits2.Add(add);
                    }


                var total = credits.Sum(s => s.Credit);
                LTotal.Text = string.Format("{0} : {1}", Tokens.Total, total);


                 

                }
            }
        }
        else
        {
            var resellerId = Convert.ToInt32(ddl_reseller.SelectedValue);
            credits.Add(new ResellerCreditTemp
            {

                Credit = GetCredit(from, resellerId),
                Reseller = ddl_reseller.SelectedItem.ToString()
            });
            LTotal.Text = string.Empty;
        }
        if (ddl_reseller.SelectedIndex == 0 && ddl_reseller.SelectedItem.ToString() == Tokens.All)
        {
            if (rblFrom.SelectedIndex != 1)
            {
                one.Visible = true;
                all.Visible = false;
                GCredits.DataSource = credits2.OrderByDescending(x => x.Credit);
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
        else {
            one.Visible = true;
            all.Visible = false;
        GCredits.DataSource = credits.OrderByDescending(x => x.Credit);
        GCredits.DataBind();
        }
        //----
        if (rblFrom.SelectedIndex == 1 && ddl_reseller.SelectedIndex != 0 && ddl_reseller.SelectedItem.ToString() != Tokens.All)
        {
            sum.Visible = true;
            var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
            {
                Paid = false,
                ResellerId = Helper.GetDropValue(ddl_reseller),
                Month = Convert.ToInt32(DateTime.Now.Month),
                Year = Convert.ToInt32(DateTime.Now.Year),
                WithResellerDiscount = true

            });

            var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

            var newlist = new List<DemandPreviewModel>();
            var sp = context2.SPoptionReselleraccounts.Select(z => z).ToList();
            foreach (var i in sp)
            {
                var data = searchDemands.Where(a => a.Provider == i.ServiceProvider.SPName).ToList();
                newlist.AddRange(data);
            }

            var report = (Tokens.currentpill + " : " + Helper.FixNumberFormat((newlist.Sum(x => x.ResellerNet))));
            lblcurrentdemand.Visible = true;
            lblcurrentdemand.Text = report;


            var res = Tokens.MenuResellerCredit + " : " +
                      Math.Round(
                          Convert.ToDouble(Convert.ToDecimal(credits.Sum(s => s.Credit)) +
                                           Convert.ToDecimal(newlist.Sum(x => x.ResellerNet))), 2);

            saf.Visible = true;
            saf.Text = res;
        }
        //-----
    }

    protected void b_addRequest1_Click(object sender, EventArgs e)
    {
        using (var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var allusers = _context.Users.ToList().Where(x => x.GroupID == 6);

            foreach (var user in allusers)
            {
                var lastCredit =
                    _context.ResellerCredits.Where(c => c.ResellerId == user.ID)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
                if (lastCredit != null) lastCredit.Net = 0;
                _context.SubmitChanges();
            }

        }
    }

    public double GetCredit(int @from,int resellerId){
        switch(@from){
            case 0:
                var ResellerCredit = Convert.ToDouble(_creditRepository.GetNetCredit(resellerId));
                return ResellerCredit;
                //break;
            case 1:
                var UserTransaction = Billing.GetLastBalance(resellerId, "Reseller");
                return UserTransaction;
                
            case 2:
                var ResellerCreditVoice =Convert.ToDouble(_creditVoice.GetNetCredit(resellerId));
                return ResellerCreditVoice;
                
            case 3:
                using(var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                    var UpdateResellerBs = _context.UpdatedResellerBs.Where(p => p.ResellerId == resellerId).Sum(p => p.InvoiceAfterReview);
                    return Convert.ToDouble(UpdateResellerBs);
                }
            default:
                return 0;

        }
    }

    #endregion
}
}