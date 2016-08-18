using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Offers : CustomPage
    {
      
    readonly Loc _loc;

    //readonly ISPDataContext _context;
    readonly IOfferRepository _offerRepository;

    readonly IOffersProviderRepository _offersProviderRepository;

    readonly IOffersResellerRepository _offersReseller;

    readonly IServicePacksRepository _servicePacksRepository;

    readonly OffersBranchRepository _offersBranchRepository;

    readonly IspDomian _domain;


    public Offers(){
       var context = IspDataContext;
        _offersBranchRepository = new OffersBranchRepository();
        _servicePacksRepository = new ServicePacksRepository();
        _offersReseller = new OffersResellerRepository();
        _offerRepository = new OfferRepository();
        _offersProviderRepository = new OffersProviderRepository();
        _domain = new IspDomian(context);
        _loc = new Loc();
    }



    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        MultiView1.SetActiveView(v_index);
        PopulateOffers();
        PopulateResellers();
        PopulateMonths();
        PopulateFreeMonths();

        _domain.PopulateBranches(CblBranches);
        l_message.Text = "";
        TbRouterCost.Text = Tokens.ZeroDefault;
        TbRouterCost.Enabled = CheckRouter.Checked;
    }


    void PopulatePacksForGv(int ? offerId){
        var packPrices = new List<PackagePrice>();
        var packs = _servicePacksRepository.Packages.ToList();

        if(offerId != null){
            packPrices.AddRange(packs.Select(package => new PackagePrice{
                Discount = _offerRepository.GetServicePackageDiscount(Convert.ToInt32(offerId), package.ID),
                Name = package.ServicePackageName,
                PackId = package.ID
            }));
        } else{
            packPrices.AddRange(packs.Select(package => new PackagePrice{
                Discount = new decimal(),
                Name = package.ServicePackageName,
                PackId = package.ID
            }));
        }
    }


    void PopulateMonths(){
        var count = Helper.FillInts(0, 12);
        ddl_monthsCount.DataSource = count;
        ddl_monthsCount.DataBind();
    }


    void PopulateFreeMonths(){
        var count = Helper.FillInts(0, 12);
        DdlFreeMonths.DataSource = count;
        DdlFreeMonths.DataBind();
    }


    void PopulateResellers(){
        var providers = DataLevelClass.GetUserReseller();
        cbl_resellers.DataSource = providers;
        cbl_resellers.DataTextField = "UserName";
        cbl_resellers.DataValueField = "ID";
        cbl_resellers.DataBind();
    }




    void PopulateOffers(){
        var offers = _offerRepository.Offers.ToList().Select(ToOfferPreviewModel);
        gv_index.DataSource = offers;
        gv_index.DataBind();
    }


    OfferPreviewModel ToOfferPreviewModel(Offer x){
        var providers = x.OfferProviders.Any() ? ConcatProviders(x.OfferProviders) : "-";
        var resellers = x.OfferResellers.Any() ? ConcatResellers(x.OfferResellers) : "-";
        var branches = x.OfferBranches.Any() ? ConcateBranches(x.OfferBranches) : "-";
        return new OfferPreviewModel{
            Id = x.Id,
            Title = x.Title,
            Discount = Helper.FixNumberFormat(x.Discount),
            LifeTime = x.LifeTime,
            FreeMonths = x.FreeMonths,
            ByPercent = _loc.IterateResource(x.ByPercent.ToString().ToLower()),
            CalculateOneBill = _loc.IterateResource(x.CalculateOneBill.ToString().ToLower()),
            FreeMonthsFirst = _loc.IterateResource(x.FreeMonthsFirst.ToString().ToLower()),
            CanUpDown = _loc.IterateResource(x.CanUpgradeorDowngrade.ToString().ToLower()),
            Resellers = resellers,
            Branches = branches,

            Providers = providers,
            RouterCost = Helper.FixNumberFormat(x.RouterCost),
            CancelPenalty = Helper.FixNumberFormat(x.CancelPenalty),
            SuspendPenalty = Helper.FixNumberFormat(x.SuspendPenalty),
            PackagesLink = string.Format("~/Pages/OfferPackages.aspx?o={0}", x.Id),
        };
    }


    string ConcateBranches(IEnumerable<OfferBranch> offerBranches){
        var names = new StringBuilder();
        foreach (var reseller in offerBranches)
        {
            names.Append("<div>");
            names.Append(reseller.Branch.BranchName);
            names.Append("</div>");
        }
        return names.ToString();
    }



    string ConcatProviders(IEnumerable<OfferProvider> offerResellers){
        var names = new StringBuilder();
        foreach(var reseller in offerResellers){
            names.Append("<div>");
            names.Append(reseller.ServiceProvider.SPName);
            names.Append("</div>");
        }
        return names.ToString();
    }


    string ConcatResellers(IEnumerable<OfferReseller> offerResellers){
        var names = new StringBuilder();
        foreach(var reseller in offerResellers){
            names.Append("<div>");
            names.Append(reseller.User.UserName);
            names.Append("</div>");
        }
        return names.ToString();
    }


    protected void b_new_Click(object sender, EventArgs e){
        PopulatePacksForGv(null);
        MultiView1.SetActiveView(v_AddEdit);
    }



    protected void b_save_Click(object sender, EventArgs e){
        var discount = Convert.ToDecimal(TbDiscount.Text);
        var byPercent = IsPercent.Checked;
        if(byPercent){
            if(discount < 0 || discount > 100){
                l_message.Text = Tokens.DiscountMustBeBetween100And0;
                return;
            }
        }
        Offer offer;
        var freeMonths = Convert.ToInt32(DdlFreeMonths.SelectedItem.Value);
        var freeMonthsFirst = FreeMonthsFirst.Checked;
        var routerCost = Convert.ToDecimal(TbRouterCost.Text);
        var suspendPenalty = Convert.ToDecimal(TbSuspend.Text);
        var cancelPenalty = Convert.ToDecimal(TbCancel.Text);
        var calculateOneBill = CbCalculateOneBill.Checked;
        var withRouter = CheckRouter.Checked;
        var canupdown = checkUpDownGrade.Checked;
        if(hf_id.Value == string.Empty){
            offer = new Offer{
                Title = tb_title.Text,
                LifeTime = Convert.ToInt32(ddl_monthsCount.SelectedItem.Value),
                Discount = discount,
                ByPercent = byPercent,
                FreeMonths = freeMonths,
                FreeMonthsFirst = freeMonthsFirst,
                RouterCost = routerCost,
                CalculateOneBill = calculateOneBill,
                CancelPenalty = cancelPenalty,
                SuspendPenalty = suspendPenalty,
                withRouter = withRouter,
                CanUpgradeorDowngrade = canupdown
            };
            _offerRepository.Save(offer);

            var resellers = cbl_resellers.Items.OfType<ListItem>()
                .Where(item => item.Selected)
                .Select(item => new OfferReseller{
                    OfferId = offer.Id,
                    UserId = Convert.ToInt32(item.Value)
                }).ToList();
            _offersReseller.SaveMany(resellers);
            
            var branches = CblBranches.Items.OfType<ListItem>()
                .Where(item => item.Selected)
                .Select(item => new OfferBranch{
                    OfferId = offer.Id,
                    BranchId = Convert.ToInt32(item.Value)
                }).ToList();
            _offersBranchRepository.SaveMany(branches);

            
        } else{
            offer = _offerRepository.Offers.FirstOrDefault(o => o.Id == Convert.ToInt32(hf_id.Value));
            if(offer != null){
                offer.Title = tb_title.Text;
                offer.LifeTime = Convert.ToInt32(ddl_monthsCount.SelectedItem.Value);
                offer.Discount = discount;
                offer.ByPercent = byPercent;
                offer.FreeMonths = freeMonths;
                offer.FreeMonthsFirst = freeMonthsFirst;
                offer.RouterCost = routerCost;
                offer.CalculateOneBill = calculateOneBill;
                offer.CancelPenalty = cancelPenalty;
                offer.SuspendPenalty = suspendPenalty;
                offer.withRouter = withRouter;
                offer.CanUpgradeorDowngrade = canupdown;
            }
            _offerRepository.Save(offer);
            if(offer != null){
                var resellers = _offersReseller.GetProviderOffersByOfferId(offer.Id).ToList();
                var newResellers = cbl_resellers.Items.OfType<ListItem>().Where(item => item.Selected).Select(
                                                                                                              item => new OfferReseller{
                                                                                                                  OfferId = offer.Id,
                                                                                                                  UserId = Convert.ToInt32(item.Value)
                                                                                                              }).ToList();

                _offersReseller.DeleteAllAndAddNew(resellers, newResellers);
                
                
                var branches = _offersBranchRepository.GetProviderOffersByOfferId(offer.Id).ToList();
                var offerBranches = CblBranches.Items.OfType<ListItem>().Where(item => item.Selected).Select(
                                                                                                              item => new OfferBranch{
                                                                                                                  OfferId = offer.Id,
                                                                                                                  BranchId = Convert.ToInt32(item.Value)
                                                                                                              }).ToList();

                _offersBranchRepository.DeleteAllAndAddNew(branches, offerBranches);


             
            }
        }
        l_message.Text = string.Format("{0}", Tokens.Saved);
        Clear();
    }


    void Clear(){
        foreach(var text in p_add.Controls.OfType<TextBox>()){
            text.Text = string.Empty;
        }
        foreach(var check in p_add.Controls.OfType<CheckBox>()){
            check.Checked = false;
        }
        foreach(var item in cbl_resellers.Items.OfType<ListItem>()){
            item.Selected = false;
        }
      
        TbRouterCost.Text = Tokens.ZeroDefault;
        PopulateOffers();
        hf_id.Value = string.Empty;
        MultiView1.SetActiveView(v_index);
    }


    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
    }


    protected void gvb_edit_Click(object sender, EventArgs e){
        MultiView1.SetActiveView(v_AddEdit);
        var buttonSender = sender as LinkButton;
        if(buttonSender == null) return;
        var id = Convert.ToInt32(buttonSender.CommandArgument);
        var offer = _offerRepository.Offers.FirstOrDefault(o => o.Id == id);
        if(offer == null) return;
        PopulatePacksForGv(offer.Id);
        tb_title.Text = offer.Title;
        TbDiscount.Text = Helper.FixNumberFormat(offer.Discount);
        TbRouterCost.Text = Helper.FixNumberFormat(offer.RouterCost);
        TbCancel.Text = Helper.FixNumberFormat(offer.CancelPenalty);
        TbSuspend.Text = Helper.FixNumberFormat(offer.SuspendPenalty);
        ddl_monthsCount.SelectedValue = string.Format("{0}", offer.LifeTime);
        hf_id.Value = offer.Id.ToString(CultureInfo.InvariantCulture);
        IsPercent.Checked = offer.ByPercent;
        CbCalculateOneBill.Checked = offer.CalculateOneBill;
        checkUpDownGrade.Checked = offer.CanUpgradeorDowngrade;
        DdlFreeMonths.SelectedValue = string.Format("{0}", offer.FreeMonths);
        FreeMonthsFirst.Checked = offer.FreeMonthsFirst;
        CheckRouter.Checked = offer.withRouter;
        var resellers = _offersReseller.GetProviderOffersByOfferId(id);
        foreach(var item in cbl_resellers
            .Items
            .OfType<ListItem>()
            .Where(item =>
                resellers.
                    Any(p => p.UserId == Convert.ToInt32(item.Value)))){
            item.Selected = true;
        }
        
        var branches = _offersBranchRepository.GetProviderOffersByOfferId(id);
        foreach(var item in CblBranches
            .Items
            .OfType<ListItem>()
            .Where(item =>
                branches.
                    Any(p => p.BranchId == Convert.ToInt32(item.Value)))){
            item.Selected = true;
        }

       
    }


    protected void gvb_delete_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var button = sender as LinkButton;
            if(button != null){
                var offerId = Convert.ToInt32(button.CommandArgument);
                var offer = _offerRepository.Offers.FirstOrDefault(o => o.Id == offerId);
                if(offer == null) return;
              
                var wooffers = (from x in context.WorkOrders
                    where x.OfferId == offerId
                    select x).ToList();
                if(wooffers.Count > 0){
                    l_message.Text = Tokens.CantDelete;
                    return;
                    
                }
            
                var offerdemand = (from a in context.Demands
                    where a.OfferId == offerId
                    select a).ToList();
                if(offerdemand.Count > 0){
                    l_message.Text = Tokens.CantDelete;
                    return;
                }

                var resellers = _offersReseller.GetProviderOffersByOfferId(offerId).ToList();
                _offersReseller.DeleteMany(resellers);

                var branches = _offersBranchRepository.GetProviderOffersByOfferId(offerId).ToList();
                _offersBranchRepository.DeleteMany(branches);

                var providers = _offersProviderRepository.GetProviderOffersByOfferId(offerId).ToList();
                _offersProviderRepository.DeleteMany(providers);
             
                var deleteoferpackages = (from a in context.OfferProviderPackages
                    where a.OfferId == offerId
                    select a).ToList();
                if(deleteoferpackages.Count > 0){
                    context.OfferProviderPackages.DeleteAllOnSubmit(deleteoferpackages);
                    context.SubmitChanges();
                }
                _offerRepository.Delete(offer);
            }
            PopulateOffers();
        }
    }


    protected void ReturnToMainView(object sender, EventArgs e){
        Clear();
        l_message.Text = string.Empty;
    }
    protected void CheckRouter_CheckedChanged(object sender, EventArgs e)
    {
        if(CheckRouter.Checked){
            TbRouterCost.Enabled = true;
            TbRouterCost.Text = Tokens.ZeroDefault;
        } else{
            TbRouterCost.Enabled = false;
            TbRouterCost.Text = Tokens.ZeroDefault;
        }
    }
}



    class OfferPreviewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Discount { get; set; }

        public int LifeTime { get; set; }

        public int FreeMonths { get; set; }

        public string ByPercent { get; set; }

        public string CalculateOneBill { get; set; }

        public string FreeMonthsFirst { get; set; }

        public string Resellers { get; set; }

        public string Branches { get; set; }

        public string Providers { get; set; }

        public string RouterCost { get; set; }

        public string CancelPenalty { get; set; }

        public string SuspendPenalty { get; set; }

        public string PackagesLink { get; set; }

        public string CanUpDown { get; set; }
    }


}