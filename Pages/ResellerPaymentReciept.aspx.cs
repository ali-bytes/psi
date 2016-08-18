using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerPaymentReciept : CustomPage
    {
      

    protected void Page_Load(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
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
            var sitedata = context.ReceiptCnfgs.FirstOrDefault(a=>a.BranchId==user.BranchID);
            if (sitedata != null)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    var resellerCreditsId = Convert.ToInt32(Request.QueryString["id"]);
                    var item = context.ResellerCredits.FirstOrDefault(x => x.Id == resellerCreditsId);
                  
                    if (item != null)
                    {
                       
                        Reseller.InnerHtml = item.User.UserName;
                        Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                        Date.InnerHtml = item.Time.ToDateTime();
                        Notes.InnerHtml = item.Notes;
                        UserName.InnerHtml = item.User1.UserName;
                        NameRow.Visible = false;
                        PhoneRow.Visible = false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["Voiceid"]))
                {
                    var resellerCreditsId = Convert.ToInt32(Request.QueryString["Voiceid"]);
                    var item = context.ResellerCreditsVoices.FirstOrDefault(x => x.Id == resellerCreditsId);

                    if (item != null)
                    {
                        Reseller.InnerHtml = item.User.UserName;
                        Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                        Date.InnerHtml = item.Time.ToDateTime();
                        Notes.InnerHtml = item.Notes;
                        UserName.InnerHtml = item.User1.UserName;
                        NameRow.Visible = false;
                        PhoneRow.Visible = false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["RequestVoiceid"]))
                {
                    var requestVoiceId = Convert.ToInt32(Request.QueryString["RequestVoiceid"]);
                    var item = context.RechargeClientRequests.FirstOrDefault(x => x.ID == requestVoiceId);
                   
                    if (item != null)
                    {
                    
                        Reseller.InnerHtml = item.User.UserName;
                        Amount.InnerHtml = Helper.FixNumberFormat(item.Amount);
                        Date.InnerHtml = item.Time.ToString();
                        CustomerName.InnerHtml = item.ClientName;
                        CustomerPhone.InnerHtml = item.ClientTelephone;
                        Notes.InnerHtml = item.Notes;
                        userNameRow.Visible = false;

                    }
                }

                if (!string.IsNullOrWhiteSpace(Request.QueryString["Customerpaymentsid"]))
                {
                    var resellerCreditsId = Convert.ToInt32(Request.QueryString["Customerpaymentsid"]);
                    var item = context.CustomerPayments.FirstOrDefault(x => x.ID == resellerCreditsId);

                    if (item != null)
                    {
                       
                        Amount.InnerHtml = Helper.FixNumberFormat(item.InvoiceAmount);
                        Date.InnerHtml = item.Time.ToString();
                        CustomerName.InnerHtml = item.CustomerName;
                        CustomerPhone.InnerHtml = item.CustomerTelephone;
                        Notes.InnerHtml = item.Notes;
                        userNameRow.Visible = false;
                        resellerrow.Visible = false;
                    }
                }





                Companyname.InnerHtml = sitedata.CompanyName;
                ImgLogo.ImageUrl = string.Format("~/PrintLogos/{0}", sitedata.LogoUrl);
                Caution.InnerHtml = sitedata.Caution;
               
            }

        }
    }
}
}