using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ServicePackages : CustomPage
    {
     
    readonly PackagesRepository _packagesRepository;


    public ServicePackages(){
        var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        _packagesRepository = new PackagesRepository(dataContext);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        MultiView1.SetActiveView(v_index);
        PopulatePackages();
        PopulateProviders();
        PopulateTypes();
        l_message.Text = "";
    }


    void PopulateTypes(){
        var packageTypes = _packagesRepository.PackageTypes();

        DdTypes.DataSource = packageTypes;
        DdTypes.DataTextField = "SPTName";
        DdTypes.DataValueField = "ID";
        DdTypes.DataBind();
        Helper.AddDefaultItem(DdTypes, Tokens.Chose);
    }


    void PopulateProviders(){
        var providers = _packagesRepository.Providers();
        DdProviders.DataSource = providers;
        DdProviders.DataTextField = "SPName";
        DdProviders.DataValueField = "ID";
        DdProviders.DataBind();
        Helper.AddDefaultItem(DdProviders, Tokens.Chose);
    }


    void PopulatePackages(){
        var packages = _packagesRepository.ToPreview();
        gv_index.DataSource = packages;
        gv_index.DataBind();
    }


    protected void b_new_Click(object sender, EventArgs e){
        MultiView1.SetActiveView(v_AddEdit);
    }


    protected void b_save_Click(object sender, EventArgs e){
        ServicePackage package;
        var providerId = Convert.ToInt32(DdProviders.SelectedItem.Value);
        var typeId = Convert.ToInt32(DdTypes.SelectedItem.Value);
        var name = TbName.Text;
        var notes = TbNotes.Text;
        var price = Convert.ToDouble(TbPrice.Text);
        var active = CbActive.Checked;
        var purchasePrice = Convert.ToDecimal(TbPurchasePrice.Text);
        if(hf_id.Value == string.Empty){
            package = new ServicePackage{
                                            ProviderId = providerId,
                                            ServicePackageName = name,
                                            ServicePackageTypeID = typeId,
                                            Notes = notes,
                                            Active = active,
                                            PurchasePrice = purchasePrice
                                        };
            _packagesRepository.Save(package);

            var pricing = CreartePricing(package, price);
            _packagesRepository.SavePricing(pricing);
        } else{
            package = _packagesRepository.GetPackage(Convert.ToInt32(hf_id.Value));
            if(package != null){
                var pricing = _packagesRepository.GetPricing(package.ProviderId, package.ID);
                package.ProviderId = providerId;
                package.ServicePackageName = name;
                package.ServicePackageTypeID = typeId;
                package.Notes = notes;
                package.Active = active;
                package.PurchasePrice = purchasePrice;
                _packagesRepository.Save(package);
                if(pricing != null){
                    pricing.Price = price;
                    pricing.ServiceProvidersID = providerId;
                    _packagesRepository.SavePricing(pricing);
                } else{
                     pricing = CreartePricing(package, price);
                    _packagesRepository.SavePricing(pricing);
                }   
            }
        }

        PopulatePackages();
        if(package != null) l_message.Text = string.Format(Tokens.Saved);
        hf_id.Value = string.Empty;
        Clear();
        MultiView1.SetActiveView(v_index);
    }


    static Pricing CreartePricing(ServicePackage package, double price){
        return new Pricing{
                              ServicePackagesID = package.ID,
                              ServiceProvidersID = package.ProviderId,
                              Price = price
                          };
    }


    void Clear(){
        DdProviders.SelectedIndex = DdTypes.SelectedIndex = -1;
        TbName.Text = TbNotes.Text = string.Empty;
    }


    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
    }


    protected void gvb_edit_Click(object sender, EventArgs e){
        MultiView1.SetActiveView(v_AddEdit);
        var buttonSender = sender as LinkButton;
        if(buttonSender == null) return;
        var id = Convert.ToInt32(buttonSender.CommandArgument);
        var package = _packagesRepository.GetPackage(id);
        if(package == null) return;
        hf_id.Value = package.ID.ToString(CultureInfo.InvariantCulture);
        DdTypes.SelectedValue = string.Format("{0}", package.ServicePackageTypeID);
        DdProviders.SelectedValue = string.Format("{0}", package.ProviderId);
        TbName.Text = package.ServicePackageName;
        TbNotes.Text = package.Notes;
        l_message.Text = string.Empty;
        var pricing = _packagesRepository.GetPrice(package.ProviderId, package.ID);
        TbPrice.Text = Helper.FixNumberFormat(pricing);
        TbPurchasePrice.Text = Helper.FixNumberFormat(package.PurchasePrice);
        CbActive.Checked = package.Active == null || package.Active.Value;
    }


    protected void gvb_delete_Click(object sender, EventArgs e){
        var button = sender as LinkButton;
        if(button == null) return;
        var package = _packagesRepository.GetPackage(Convert.ToInt32(button.CommandArgument));
        if(package == null) return;
        var deleted=_packagesRepository.Delete(package);
        if (!deleted) {l_message.Text = Tokens.CantDelete; return;}
        l_message.Text = string.Format(Tokens.Saved);
        PopulatePackages();
    }
}

}