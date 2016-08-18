using System;
using System.Collections.Generic;
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
    public partial class OfferPackages : CustomPage
    {
      

    readonly IspDomian _domian;


    public OfferPackages(){
        _domian = new IspDomian(IspDataContext);
       
    }



    protected void Page_Load(object sender, EventArgs e){
            if (IsPostBack) return;
            if (string.IsNullOrEmpty(Request.QueryString["o"]))
            {
                Response.Redirect("~/Pages/Offers.aspx");
                return;
            }
            var offerId = Convert.ToInt32(Request.QueryString["o"]);
            HfOfferId.Value = Request.QueryString["o"];
            var offer = _domian.Offer(offerId);

            if (offer == null)
            {
                Response.Redirect("~/Pages/Offers.aspx");
                return;
            }
            H3.InnerHtml = string.Format("{0} - {1}", offer.Title, Tokens.Packages);

            MultiView1.SetActiveView(v_index);
            Populate(offerId);
            l_message.Text = "";
        
    }


    void Populate(int offerId){
        var offerProvidersPackages = _domian.OfferProvidersPackages(offerId).Select(x => new{
            Provider = x.Provider.SPName,
            Package = x.Package.ServicePackageName,
            x.Checked,
            Concate = string.Format("{0}-{1}", x.Provider.ID, x.Package.ID)
        });
        gv_index.DataSource = offerProvidersPackages;
        gv_index.DataBind();
    }



    protected void gv_index_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_index, "l_number");
    }


    protected void Save(object sender, EventArgs e)
    {
        using (var dataContext = IspDataContext)
        {
            l_message.Text = string.Empty;
            if (string.IsNullOrEmpty(HfOfferId.Value))
            {
                return;
            }
            var offerId = Convert.ToInt32(HfOfferId.Value);
            var offer = _domian.Offer(offerId);
            if (offer == null) return;

            foreach (GridViewRow row in gv_index.Rows)
            {
                var checkBox = row.FindControl("CbSelected") as CheckBox;
                if (checkBox == null) continue;
                var concate = checkBox.CssClass.Split('-');
                var packageId = Convert.ToInt32(concate[1]);
                if (checkBox.Checked)
                {
                    var exist =
                        offer.OfferProviderPackages.FirstOrDefault(x => x.PackageId == packageId && x.OfferId == offerId);
                    if (exist != null)
                    {
                        continue;
                    }
                    offer.OfferProviderPackages.Add(new OfferProviderPackage()
                    {
                        OfferId = offerId,
                        PackageId = packageId
                    });

                }
                else
                {
                    var offerProviderPackage =
                        offer.OfferProviderPackages.FirstOrDefault(x => x.PackageId == packageId && x.OfferId == offerId);
                    if (offerProviderPackage == null)
                    {
                        continue;
                    }
                    dataContext.OfferProviderPackages.DeleteOnSubmit(offerProviderPackage);
                }
            }
            dataContext.SubmitChanges();
            Populate(offerId);
            l_message.Text = Tokens.Saved;
        }
    }

}
}