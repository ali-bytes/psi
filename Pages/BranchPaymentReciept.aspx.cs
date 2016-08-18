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

namespace NewIspNL.Pages
{
    public partial class BranchPaymentReciept : CustomPage
    {
        //readonly  _context;

        readonly SiteDateRepository _siteDateRepository;


        public BranchPaymentReciept()
        {
            ISPDataContext context = IspDataContext;
            _siteDateRepository = new SiteDateRepository(context);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                var option = context.Options.FirstOrDefault();
                if (option != null && Convert.ToBoolean(option.WidthOfReciept))
                {
                    all.Style["width"] = "8cm";
                    logo.Attributes.Add("style", "width: 90%;");
                    title.Attributes.Add("style", "width: 90%;margin: 0% 5%;");
                }
                var sitedata = context.ReceiptCnfgs.FirstOrDefault(a => a.BranchId == user.BranchID);//_siteDateRepository.SiteData();
                if (sitedata != null)
                {
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                    {
                        var resellerCreditsId = Convert.ToInt32(Request.QueryString["id"]);
                        var item = context.BranchCredits.FirstOrDefault(x => x.Id == resellerCreditsId);
                        //var sitedata = _siteDateRepository.SiteData();
                        if (item != null)
                        {
                            //Companyname.InnerHtml = sitedata.SiteName;
                            //ImgLogo.ImageUrl = string.Format("~/SiteLogo/{0}", sitedata.LogoUrl);
                            Branch.InnerHtml = item.Branch.BranchName;
                            Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                            Date.InnerHtml = item.Time.ToDateTime();
                            Notes.InnerHtml = item.Notes;
                            UserName.InnerHtml = item.User.UserName;
                        }
                    }
                    Companyname.InnerHtml = sitedata.CompanyName;
                    ImgLogo.ImageUrl = string.Format("~/PrintLogos/{0}", sitedata.LogoUrl);
                    Caution.InnerHtml = sitedata.Caution;
                    //Address.Text = sitedata.ContactData;
                }
            }
        }
    }
}
