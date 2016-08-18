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
    public partial class Contarct : CustomPage
    {
        
            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                //if (Session["Order"] == null) return;

                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    // var orderId = Convert.ToInt32(Request.QueryString["r"]);
                    if (Session["User_ID"] == null) return;
                    var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                    if (user == null) return;
                    var config = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
                    if (config == null) return;
                    if (string.IsNullOrWhiteSpace(Request.QueryString["WOID"])) return;
                    var que = Request.QueryString["WOID"];
                    var id = QueryStringSecurity.Decrypt(que);
                    var orderId = Convert.ToInt32(id);
                    //var orderId = Convert.ToInt32(Request.QueryString["OrderId"]);//(WorkOrder) Session["Order"]; 
                    var order = context.WorkOrders.FirstOrDefault(a => a.ID == orderId);
                    if (order == null) return;
                    //lblCompanyName.Text=lblfooterComapny.Text = config.CompanyName;
                    //lblBranchName.Text = config.Branch.BranchName;
                    imgCo.ImageUrl = "../PrintLogos/" + config.LogoUrl;
                    lblfooterContact.InnerHtml = config.ContactData;
                    lblDate.Text = DateTime.Now.AddHours().ToShortDateString();
                    lblEmployee.Text = user.UserName;
                    lblCustomerName.Text = order.CustomerName;
                    lblOwner.Text = string.IsNullOrWhiteSpace(order.LineOwner) ? order.CustomerName : order.LineOwner;
                    lblAddress.Text = lblAddress2.Text = order.CustomerAddress;
                    lblServicePhone.Text = order.CustomerPhone;
                    lblMobile1.Text = order.CustomerMobile;
                    //lblMobile2.Text = order.CustomerMobile2;
                    lblPersonalId.Text = order.PersonalId;
                    lblEmail.Text = order.CustomerEmail;
                    lblServiceProvider.Text = order.ServiceProvider.SPName;
                    lblServicePackage.Text = order.ServicePackage.ServicePackageName;
                    /*var pricing = order.ServicePackage.Pricings
                                        .FirstOrDefault(x => x.ServiceProvidersID == order.ServiceProviderID);
                    if (pricing != null)
                    {
                        var offerAmount =Convert.ToDecimal(pricing.Price);
                        lblCostInOffer.Text =(offerAmount - OfferPricingServices.GetOfferPrice(order.Offer, offerAmount)).ToString(CultureInfo.InvariantCulture);
                        lblCostOutOffer.Text = offerAmount.ToString(CultureInfo.InvariantCulture);//order.ServicePackage.Pricings.ToString();
                    }lblIP.Text = order.IpPackage.IpPackageName;*/
                    if (order.Offer != null) lblOffer.Text = order.Offer.Title;

                    //lblPrepaid.Text = order.Prepaid.ToString();
                    //lblInstallationCost.Text = order.InstallationCost.ToString();
                    //lblContractCost.Text = order.ContractingCost.ToString();
                    //lblPaymentType.Text = order.PaymentType.PaymentTypeName;

                }
            }
        }
    }
