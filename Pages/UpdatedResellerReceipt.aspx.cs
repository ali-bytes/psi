using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class UpdatedResellerReceipt : CustomPage
    {
       

    readonly ISPDataContext _context;

    readonly SiteDateRepository _siteDateRepository;


    public UpdatedResellerReceipt()
    {
        _context = IspDataContext;
        _siteDateRepository = new SiteDateRepository(_context);
    }


    protected void Page_Load(object sender, EventArgs e){
        if(!string.IsNullOrWhiteSpace(Request.QueryString["id"])){
            var resellerCreditsId = Convert.ToInt32(Request.QueryString["id"]);
            var item = _context.UpdatedResellerPayments.FirstOrDefault(x => x.Id == resellerCreditsId);
            var sitedata = _siteDateRepository.SiteData();
            if(item != null && sitedata != null){
                Companyname.InnerHtml = sitedata.SiteName;
                ImgLogo.ImageUrl = string.Format("~/SiteLogo/{0}", sitedata.LogoUrl);
                Reseller.InnerHtml = item.User.UserName;
                Amount.InnerHtml = Helper.FixNumberFormat(item.Total);
                Date.InnerHtml = item.Time.ToString();
                Notes.InnerHtml = item.Notes;
            }
        }
    }
}
}