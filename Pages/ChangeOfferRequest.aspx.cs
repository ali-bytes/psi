using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ChangeOfferRequest : CustomPage
    {
          private readonly IspEntries _ispEntries;
            private readonly IspDomian _domian;
            public  ChangeOfferRequest()
            {
                _ispEntries = new IspEntries();
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _domian = new IspDomian(context);
            }

            protected void Page_Load(object sender, EventArgs e)
            {
                using (var db1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var option = OptionsService.GetOptions(db1, true);
                    if (Convert.ToBoolean(Session["reloadrepquestPage"]))
                    {
                        Session["reloadrepquestPage"] = true;
                        lbl_InsertResult.Text = Tokens.Request_Added_successfully;
                        lbl_InsertResult.ForeColor = Color.Green;
                        Session["reloadrepquestPage"] = false;
                    }
                    if (Session["User_ID"] == null) return;
                    if (IsPostBack) return;
                    //var option = OptionsService.GetOptions(db1, true);
                    if (option.IncludeGovernorateInSearch)
                    {
                        GovBox.Visible = true;
                        _domian.PopulateGovernorates(DdlGovernorate);
                    }
                    else GovBox.Visible = false;
                    PopulateOffers();
                    lbl_RequestDate.Text = DateTime.Now.AddHours().ToShortDateString();
                    Helper.AddAllDefaultItem(this);
                }
            }
            void PopulateOffers(int providerId = 0)
            {
                using (var db9 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    var user = db9.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                    if (user == null) return;
                    if (providerId.Equals(0))
                    {
                        Helper.BindDrop(ddlOffers, null, "", "");
                        //packs.Value = string.Empty;
                        return;
                    }
                    var provider = db9.ServiceProviders.FirstOrDefault(p => p.ID.Equals(providerId));
                    if (provider == null)
                    {
                        Helper.BindDrop(ddlOffers, null, "", "");
                        //packs.Value = string.Empty;
                        return;
                    }
                    var offers = _domian.ProviderOffers(provider, user);
                    Helper.BindDrop(ddlOffers, offers, "Title", "Id");
                }
            }

            protected void btn_Search_Click(object sender, EventArgs e)
            {
                using (var db7 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userWorkOrders = DataLevelClass.GetUserWorkOrder();
                    var option = OptionsService.GetOptions(db7, true);
                    WorkOrder order;
                    if (option != null && option.IncludeGovernorateInSearch)
                    {
                        order = DdlGovernorate.SelectedIndex > 0 ? db7.WorkOrders.FirstOrDefault(wo => wo.CustomerPhone == txt_CustomerPhone.Text.Trim() && wo.CustomerGovernorateID == Convert.ToInt32(DdlGovernorate.SelectedItem.Value)) : null;
                    }
                    else order = db7.WorkOrders.FirstOrDefault(wo => wo.CustomerPhone == txt_CustomerPhone.Text.Trim());
                    if (order == null)
                    {
                        lbl_SearchResult.Text = Tokens.User_doesn_t_exsists_;
                        lbl_SearchResult.ForeColor = Color.Red;
                        tr_Details.Visible = false;
                        tr_Request.Visible = false;
                        return;
                    }

                    hf_woid.Value = order.ID.ToString(CultureInfo.InvariantCulture);
                    PopulateOffers(Convert.ToInt32(order.ServiceProviderID));

                    IEnumerable<bool> matchedList = userWorkOrders.Select(tmpwo => tmpwo.ID == order.ID);
                    if (!matchedList.Contains(true))
                    {
                        lbl_SearchResult.Text = Tokens.User_doesn_t_exsists_;
                        lbl_SearchResult.ForeColor = Color.Red;
                        tr_Details.Visible = false;
                        tr_Request.Visible = false;
                        return;
                    }
                    /*var woQuery = db7.WorkOrders.Where(wo => wo.CustomerPhone == txt_CustomerPhone.Text).Select(wo => new
                    {
                        wo.CustomerName,
                        wo.CustomerPhone,
                        wo.ID,
                        wo.Branch.BranchName,
                        wo.ServicePackage.ServicePackageName,
                        wo.IpPackage.IpPackageName,
                        wo.ServicePackageID,
                        wo.Status.StatusName,
                        CanUp = wo.Offer == null || wo.Offer.CanUpgradeorDowngrade,
                        wo.OfferId,
                    });
                    if (!woQuery.Any()) return;*/
                    ViewState.Add("woid", order.ID);
                    ViewState.Add("ServicePackageID", order.ServicePackageID);
                    tr_Details.Visible = true;
                    tr_Request.Visible = true;
                    //if(woQuery.First().OfferId!=null)
                    lbl_CustomerName.Text = order.CustomerName;
                    lbl_CustomerPhone.Text = order.CustomerPhone;
                    lbl_BranchName.Text = order.Branch.BranchName;
                    lbl_ServicePackageName.Text = order.ServicePackage.ServicePackageName;
                    lbl_IpPackageName.Text = order.IpPackage.IpPackageName;
                    lbl_CustomerStatus.Text = order.Status.StatusName;
                }
            }
            protected void btn_AddRequest_Click(object sender, EventArgs e)
            {
                using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (!IsValid) return;
                    var orders = db8.WorkOrders.FirstOrDefault(wo => wo.ID == Convert.ToInt32(ViewState["woid"]));
                    if (orders == null) return;

                    //حالة العميل يجب ان تكون اكتف عند تغير العرض 
                    if (orders.WorkOrderStatusID != 6)
                    {
                        lbl_InsertResult.Text = "لتغير العرض يلزم ان يكون العميل اكتف";//Tokens.ToSuspendShouldbeActive;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                    var orderRequests = db8.WorkOrderRequests
               .Where(woreq => woreq.WorkOrderID == orders.ID && woreq.RSID == 3);

                    if (orderRequests.Any())
                    {
                        lbl_InsertResult.Text = Tokens.User_Has_Request;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }


                    var wor = new WorkOrderRequest
                    {
                        WorkOrderID = orders.ID,
                        CurrentPackageID = orders.ServicePackageID,
                        NewPackageID = orders.ServicePackageID,
                        RequestDate = Convert.ToDateTime(lbl_RequestDate.Text),
                        RequestID = 12,
                        RSID = 3,
                        NewIpPackageID = orders.IpPackageID,
                        SenderID = Convert.ToInt32(Session["User_ID"]),
                        NewOfferId = Convert.ToInt32(ddlOffers.SelectedItem.Value)
                    };

                    db8.WorkOrderRequests.InsertOnSubmit(wor);
                    db8.SubmitChanges();
                    Session["reloadrepquestPage"] = true;
                    Response.Redirect("~/Pages/ChangeOfferRequest.aspx");

                }
            }
        }
    }
 