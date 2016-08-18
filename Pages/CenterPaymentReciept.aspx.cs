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
    public partial class CenterPaymentReciept : CustomPage
    {
        
            readonly SiteDateRepository _siteDateRepository;


            public  CenterPaymentReciept()
            {
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _siteDateRepository = new SiteDateRepository(context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                    {
                        var centerCreditsId = Convert.ToInt32(Request.QueryString["id"]);
                        var item = context.CenterCredits.FirstOrDefault(x => x.Id == centerCreditsId);
                        var sitedata = _siteDateRepository.SiteData();
                        if (item != null && sitedata != null)
                        {
                            Companyname.InnerHtml = sitedata.SiteName;
                            ImgLogo.ImageUrl = string.Format("~/SiteLogo/{0}", sitedata.LogoUrl);
                            Center.InnerHtml = item.User.UserName;
                            Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                            Date.InnerHtml = item.Time.ToDateTime();
                            Notes.InnerHtml = item.Notes;
                            /*NameRow.Visible = false;
                        PhoneRow.Visible = false;*/
                        }
                    }
                }
            }
        }
    }
 