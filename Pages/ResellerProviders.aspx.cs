using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Models;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerProviders : CustomPage
    {
     

    readonly IspDomian _domian;


    public ResellerProviders(){
        _domian = new IspDomian(IspDataContext);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack){
            return;
        }
        PopulateResellers();
        PopulateProviders();
    }


    void PopulateProviders(){
        var providers = GetAllProviders();
        GvResults.DataSource = providers;
        GvResults.DataBind();
    }


    List<ProviderModel> GetAllProviders(){
        return _domian.Providers.Select(x => new ProviderModel{
                                                                  ID = x.ID,
                                                                  SPName = x.SPName,
                                                                  IsChecked = false
                                                              }).ToList();
    }


    void PopulateResellers(){
        _domian.PopulateResellers(DdlResellers, true);
    }


    protected void Search(object sender, EventArgs e){
        PerformSearch();
    }


    void PerformSearch(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var allProviders = GetAllProviders();
            if(DdlResellers.SelectedIndex < 1){
                Msg.InnerHtml = Tokens.Chose;
                GvResults.DataSource = allProviders;
                GvResults.DataBind();
                return;
            }
            var id = Convert.ToInt32(DdlResellers.SelectedItem.Value);

            var providers = context.UserProviders.Where(x => x.UserId == id).Select(x => x.ServiceProvider).ToList();
            foreach(var model in allProviders){
                if(providers.Any(x => x.ID == model.ID)){
                    model.IsChecked = true;
                }
            }
            GvResults.DataSource = allProviders;
            GvResults.DataBind();
        }
    }



    protected void UpdateNumbers(object sender, EventArgs e){
        Helper.GridViewNumbering(GvResults, "LNo");
    }


    protected void Save(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(DdlResellers.SelectedIndex < 1){
                Msg.InnerHtml = Tokens.Chose;
                return;
            }
            var resellerId = Convert.ToInt32(DdlResellers.SelectedItem.Value);
            var reseller = context.Users.FirstOrDefault(x => x.ID == resellerId);
            if(reseller == null){
                return;
            }
            var links = new List<UserProvider>();
            foreach(GridViewRow row in GvResults.Rows){
                var check = row.FindControl("CbProvider") as CheckBox;
                if(check == null || !check.Checked) continue;
                var hf = row.FindControl("hfId") as HiddenField;
                if(hf == null) continue;
                var providerId = Convert.ToInt32(hf.Value);

                var provider = context.ServiceProviders.FirstOrDefault(x => x.ID == providerId);
                if(provider == null) continue;

                links.Add(new UserProvider{
                    Provider = providerId,
                    UserId = resellerId
                });
            }

            context.UserProviders.DeleteAllOnSubmit(reseller.UserProviders);
            context.SubmitChanges();

            reseller.UserProviders.AddRange(links);
            context.SubmitChanges();
            PerformSearch();
            Msg.InnerHtml = Tokens.Saved;
        }
    }
}
}