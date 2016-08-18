using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerPackagesDiscounts : CustomPage
    {
       
    readonly ResellerDiscountsService _discountsService;

    readonly IspDomian _domian;



    public ResellerPackagesDiscounts(){
        _domian = new IspDomian(IspDataContext);
        _discountsService = new ResellerDiscountsService(IspDataContext);
    }



    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PopulatePage();
        MultiView1.SetActiveView(v_index);
        l_message.Text = "";
        
    }


    void PopulatePage(){
        _domian.PopulateProviders(DdlProvider);
        _domian.PopulateResellers(DdlReseller);
    }



    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
    }



    protected void ResellerPacksDiscounts(object sender, EventArgs e){
        SearchDiscounts();
    }


    void SearchDiscounts(){
        if(DdlReseller.SelectedIndex < 1 || DdlProvider.SelectedIndex < 1){
            gv_index.DataSource = null;
            gv_index.DataBind();
            return;
        }
        var discounts = _discountsService.ResellerDiscounts(Convert.ToInt32(DdlReseller.SelectedItem.Value), Convert.ToInt32(DdlProvider.SelectedItem.Value));
        var models = ResellerDiscountsService.To(discounts);
        gv_index.DataSource = models;
        gv_index.DataBind();


      


    }


    protected void SaveDiscount(object sender, EventArgs e){
        if(string.IsNullOrEmpty(TbDiscount.Text) || string.IsNullOrEmpty(HfRes.Value) || string.IsNullOrEmpty(HfProv.Value)
           || string.IsNullOrEmpty(HfPack.Value))return;
        var discount = Convert.ToDecimal(TbDiscount.Text);
        var resellerId = Convert.ToInt32(HfRes.Value);
        var providerId = Convert.ToInt32(HfProv.Value);
        var packageId = Convert.ToInt32(HfPack.Value);
        _discountsService.SaveDiscount(resellerId,providerId,packageId,discount);
        _discountsService.Commit();
        SearchDiscounts();
        TbDiscount.Text = HfRes.Value = HfProv.Value = HfPack.Value = string.Empty;
    }
}
}