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
    public partial class BranchePaymentReciept : CustomPage
    {
       
            //readonly ISPDataContext _context;
            readonly SiteDateRepository _siteDateRepository;
            public  BranchePaymentReciept()
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
                        if (!string.IsNullOrWhiteSpace(Request.QueryString["RequestBrancheid"]))
                        {
                            var requestBrancheId = Convert.ToInt32(Request.QueryString["RequestBrancheid"]);
                            var item = context.RechargeBranchRequests.FirstOrDefault(x => x.Id == requestBrancheId);
                            //var sitedata = _siteDateRepository.SiteData();
                            if (item != null)
                            {
                                //ImgLogo.ImageUrl = string.Format("~/SiteLogo/{0}", item.VoiceCompany.CompanyImage);
                                //Companyname.InnerHtml = item.VoiceCompany.CompanyName;
                                Branch.InnerHtml = item.Branch.BranchName;
                                Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                                Date.InnerHtml = item.Time.ToString();
                                CustomerName.InnerHtml = item.ClientName;
                                CustomerPhone.InnerHtml = item.ClientTelephone;
                                Notes.InnerHtml = item.Notes;
                                userNameRow.Visible = false;

                            }
                        }
                        if (!string.IsNullOrWhiteSpace(Request.QueryString["voiceid"]))
                        {
                            var creditsId = Convert.ToInt32(Request.QueryString["Voiceid"]);
                            var item = context.BranchCreditVoices.FirstOrDefault(x => x.Id == creditsId);
                            //var sitedata = _siteDateRepository.SiteData();
                            if (item != null)
                            {
                                //Companyname.InnerHtml = sitedata.SiteName;
                                //ImgLogo.ImageUrl = string.Format("~/SiteLogo/{0}", sitedata.LogoUrl);
                                Branch.InnerHtml = item.Branch.BranchName;
                                Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                                Date.InnerHtml = item.Time.ToString();
                                UserName.InnerHtml = item.User.UserName;
                                NameRow.Visible = false;
                                PhoneRow.Visible = false;
                                Notes.InnerHtml = item.Notes;
                                NameRow.Visible = false;
                                PhoneRow.Visible = false;
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
 