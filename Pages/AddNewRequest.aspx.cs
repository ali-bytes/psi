using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Concrete;
using NewIspNL.Services;
using Resources;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class AddNewRequest : CustomPage
    {
        private readonly IspEntries _ispEntries;
          

            //readonly IspDomian _domian;

            /*public Pages_AddNewRequest(){
                _ispEntries = new IspEntries();
                var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _domian = new IspDomian(context);
            }*/

        public AddNewRequest()
        {
            var context = IspDataContext;
            _ispEntries = new IspEntries(context);
        }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    portalResult.Visible = false;
                }
                using (var db1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    ddl_ServicePackage.Enabled = true;
                    var option = OptionsService.GetOptions(db1, true);//db1.Options.FirstOrDefault();
                    var thisday = DateTime.Now.Day;
                    if (option != null)
                    {
                        var domain = new IspDomian(db1);
                        if (option.FromDay == 0 && option.ToDay == 0)
                        {
                           

                               
                                    if (Convert.ToBoolean(Session["portalsuccess"]))
                                    {
                                        portalResult.Visible = true;
                                        portalResult.InnerHtml = Session["portalmsg"].ToString();
                                        portalResult.Attributes.Add("class", "alert alert-success");
                                        Session["portalsuccess"] = false;
                                    }

                                    if (Convert.ToBoolean(Session["portalfail"]))
                                    {
                                        portalResult.Visible = true;
                                        portalResult.InnerHtml = Session["portalmsg"].ToString();
                                        portalResult.Attributes.Add("class", "alert alert-danger");
                                        Session["portalfail"] = false;
                                    }

                               
                               
                            

                            if (Convert.ToBoolean(Session["reloadrepquestPage"]))
                            {

                                Session["reloadrepquestPage"] = true;
                                lbl_InsertResult.Text = Tokens.Request_Added_successfully;
                                lbl_InsertResult.ForeColor = Color.Green;
                                Session["reloadrepquestPage"] = false;
                            }
                            if (Session["User_ID"] == null) return;
                            ProcessQuery(db1);
                            if (IsPostBack) return;

                            //var option = OptionsService.GetOptions(db1, true);
                            if (option.IncludeGovernorateInSearch)
                            {
                                GovBox.Visible = true;
                                domain.PopulateGovernorates(DdlGovernorate);
                            }
                            else GovBox.Visible = false;
                            Bind_ddl_Requests(db1);
                            Bind_ddl_IpPackage(db1);
                            PopulateExtraGigas(db1);
                            lbl_RequestDate.Text = DateTime.Now.AddHours().ToString(CultureInfo.InvariantCulture);//.ToShortDateString();
                            SwitchNameUponRequestType(db1);
                            Helper.AddAllDefaultItem(this);
                            lblRequestDuration.Visible = false;
                        }
                        else
                        {
                            if (thisday >= option.FromDay && thisday <= option.ToDay)
                            {
                               
                                    if (Convert.ToBoolean(Session["portalsuccess"]))
                                    {
                                        portalResult.Visible = true;
                                        portalResult.InnerHtml = Session["portalmsg"].ToString();
                                        portalResult.Attributes.Add("class", "alert alert-success");
                                        Session["portalsuccess"] = false;
                                    }

                                    if (Convert.ToBoolean(Session["portalfail"]))
                                    {
                                        portalResult.Visible = true;
                                        portalResult.InnerHtml = Session["portalmsg"].ToString();
                                        portalResult.Attributes.Add("class", "alert alert-danger");
                                        Session["portalfail"] = false;
                                    }

                                

                                if (Convert.ToBoolean(Session["reloadrepquestPage"]))
                                {
                                    Session["reloadrepquestPage"] = true;
                                    lbl_InsertResult.Text = Tokens.Request_Added_successfully;
                                    lbl_InsertResult.ForeColor = Color.Green;
                                    Session["reloadrepquestPage"] = false;
                                }
                                if (Session["User_ID"] == null) return;
                                ProcessQuery(db1);
                                if (IsPostBack) return;
                                //var option = OptionsService.GetOptions(db1, true);
                                if (option.IncludeGovernorateInSearch)
                                {
                                    GovBox.Visible = true;
                                    domain.PopulateGovernorates(DdlGovernorate);
                                }
                                else GovBox.Visible = false;
                                Bind_ddl_Requests(db1);
                                Bind_ddl_IpPackage(db1);
                                PopulateExtraGigas(db1);
                                lbl_RequestDate.Text = DateTime.Now.AddHours().ToString(CultureInfo.InvariantCulture);//.ToShortDateString();
                                SwitchNameUponRequestType(db1);
                                Helper.AddAllDefaultItem(this);
                                lblRequestDuration.Visible = false;
                            }
                            else
                            {
                                lblRequestDuration.Visible = true;
                                ser.Visible = false;
                                //Request.QueryString["rid"] = null;
                                return;
                            }
                            txt_CustomerPhone.TabIndex = 0;
                        }

                    }
                }
               
            }


            void PopulateExtraGigas(ISPDataContext db)
            {
                var ispEntries = new IspEntries(db);
                ispEntries.PopulateExtraGigas(DdlExtraGigas);
            }



            private void SwitchNameUponRequestType(ISPDataContext db2)
            {
                if (Request.QueryString["rid"] == null) return;
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var contextId = Convert.ToInt32(que);
                var request = db2.Requests.FirstOrDefault(r => r.ID == contextId);
                var locer = new Loc();
                if (request != null) btn_Search.Text = locer.IterateResource(request.RequestName);
            }


            void ProcessQuery(ISPDataContext db3)
            {
                //using(var db3 = new ISPDataContext()){
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestId = Convert.ToString(que);
                if (!string.IsNullOrEmpty(requestId))
                {
                    ViewState.Add("RequestID", requestId);
                    var reqQuery = db3.Requests.Where(req => req.ID == Convert.ToInt32(requestId)).Select(req => req.RequestName);
                    var groupIdQuery = db3.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                    var id = groupIdQuery.First().GroupID;
                    if (id == null) return;
                    var groupId = id.Value;
                    var groupPrivilegeQuery =
                        db3.GroupPrivileges.Where(a => a.Group.ID == groupId).Select(a => a.privilege.Name);
                    /*from gp in db3.GroupPrivileges
                    where gp.Group.ID == groupId
                    select gp.privilege.Name;*/
                    if (!(groupPrivilegeQuery.Contains(reqQuery.First()) || groupPrivilegeQuery.Contains("All")))
                    {
                        Response.Redirect("UnAuthorized.aspx");
                    }

                    if (requestId == "1")
                    {
                        tr_ExtraGiga.Visible = false;
                        tr_NewIPPackage.Visible = false;
                        tr_NewServicePackage.Visible = true;
                        ddl_Requests.SelectedValue = "1";
                    }
                    else if (requestId == "8")
                    {
                        tr_ExtraGiga.Visible = false;
                        tr_NewIPPackage.Visible = true;
                        tr_NewServicePackage.Visible = false;
                        ddl_Requests.SelectedValue = "8";
                    }
                    else if (requestId == "9")
                    {
                        tr_ExtraGiga.Visible = true;
                        tr_NewIPPackage.Visible = false;
                        tr_NewServicePackage.Visible = false;
                        ddl_Requests.SelectedValue = "9";
                    }
                    else
                    {
                        ddl_Requests.SelectedValue = requestId;
                        tr_ExtraGiga.Visible = false;
                        tr_NewIPPackage.Visible = false;
                        tr_NewServicePackage.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("ErrorPage.aspx");
                }
                //}
            }


            void Bind_ddl_ServicePackage(WorkOrder orderId, ISPDataContext db4)
            {
                var packages = orderId.OfferId == null ? db4.ServicePackages.Where(x => x.ProviderId == orderId.ServiceProviderID.Value).Select(sp => sp) : db4.OfferProviderPackages.Where(a => a.OfferId == orderId.OfferId).Select(sp => sp.ServicePackage);
                packages = packages.Where(x => x.Active == true && x.ID != orderId.ServicePackageID);
                ddl_ServicePackage.DataSource = packages;
                ddl_ServicePackage.DataBind();
                Helper.AddDefaultItem(ddl_ServicePackage);
            }


            void Bind_ddl_Requests(ISPDataContext db5)
            {
                var requests = db5.Requests.Select(req => req);
                ddl_Requests.DataSource = requests;
                ddl_Requests.DataBind();
            }


            void Bind_ddl_IpPackage(ISPDataContext db6)
            {
                var query = db6.IpPackages.Select(ip => ip);
                ddl_IpPackage.DataSource = query;
                ddl_IpPackage.DataBind();
                ddl_IpPackage.SelectedValue = "-1";
            }


            protected void btn_Search_Click(object sender, EventArgs e)
            {
                portalResult.Visible = false;
                using (var db7 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (Session["User_ID"] == null) return;
                    var user = db7.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                    if (user == null)
                    {
                        Response.Redirect("UnAuthorized.aspx");
                        return;
                    }
                    //List<WorkOrder> userWorkOrders;
                    var acountManager = db7.Users.Where(a => a.AccountmanagerId == user.ID).ToList();
                    // فلتر للعملاء , اذا كان المستخدم اللى داخل مدير حسابات لمستخدمين فلن يرى الا هؤلاء المستخدم فقط
                    var userWorkOrders = acountManager.Count == 0 ? DataLevelClass.GetUserWorkOrder() : DataLevelClass.GetUserWorkOrderByAccountManager(user.ID, db7);
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
                    Bind_ddl_ServicePackage(order, db7);

                    IEnumerable<bool> matchedList = userWorkOrders.Select(tmpwo => tmpwo.ID == order.ID);
                    if (!matchedList.Contains(true))
                    {
                        lbl_SearchResult.Text = Tokens.User_doesn_t_exsists_;
                        lbl_SearchResult.ForeColor = Color.Red;
                        tr_Details.Visible = false;
                        tr_Request.Visible = false;
                        return;
                    }
                    var woQuery = db7.WorkOrders.Where(wo => wo.CustomerPhone == txt_CustomerPhone.Text).Select(wo => new
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
                    if (!woQuery.Any()) return;
                    ViewState.Add("woid", woQuery.First().ID);
                    ViewState.Add("ServicePackageID", woQuery.First().ServicePackageID);
                    tr_Details.Visible = true;
                    tr_Request.Visible = true;
                    //if(woQuery.First().OfferId!=null)
                    lbl_CustomerName.Text = woQuery.First().CustomerName;
                    lbl_CustomerPhone.Text = woQuery.First().CustomerPhone;
                    lbl_BranchName.Text = woQuery.First().BranchName;
                    lbl_ServicePackageName.Text = woQuery.First().ServicePackageName;
                    lbl_IpPackageName.Text = woQuery.First().IpPackageName;
                    lbl_CustomerStatus.Text = woQuery.First().StatusName;
                    if (ViewState["RequestID"].ToString() == "1") //Disalbe the current service package
                    {
                        foreach (ListItem li in ddl_ServicePackage.Items)
                        {
                            if (li.Value == woQuery.First().ServicePackageID.ToString())
                                li.Enabled = false;
                        }
                        var canupdown = woQuery.First().CanUp;
                        if (!canupdown)
                        {
                            ddl_ServicePackage.Enabled = btn_AddRequest.Disabled = false;
                            lbl_SearchResult.Text = Tokens.CantUpgradeDowngrade;
                        }

                    }
                }
            }


            protected void btn_AddRequest_Click(object sender, EventArgs e)
            {
                using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                   
                    if (!IsValid) return;
                    var requestId = ViewState["RequestID"].ToString();
                    var orderRequests = db8.WorkOrderRequests
                        .Where(woreq => woreq.WorkOrderID == Convert.ToInt32(ViewState["woid"]) && woreq.RSID == 3);

                    if (orderRequests.Any())
                    {
                        lbl_InsertResult.Text = Tokens.User_Has_Request;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }

                    var orders = db8.WorkOrders.Where(wo => wo.ID == Convert.ToInt32(ViewState["woid"])
                                                            &&
                                                            (wo.WorkOrderStatusID == 6 || wo.WorkOrderStatusID == 11 ||
                                                             wo.WorkOrderStatusID == 10
                                                             || wo.WorkOrderStatusID == 8 || wo.WorkOrderStatusID == 9 ||
                                                             wo.WorkOrderStatusID == 7));
                    if (!orders.Any())
                    {
                        lbl_InsertResult.Text = Tokens.Active_Suspend_Active_Only;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                    var firstOrder = orders.FirstOrDefault();
                    if (firstOrder == null) return;

                    if (firstOrder.WorkOrderStatusID == 11 && (requestId != "3" && requestId != "6"))
                    {
                        lbl_InsertResult.Text = Tokens.Duplicate_Request_Suspend;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }

                    //حالة الكانسليشن بروسس او كانسل ملوش غير ريأكتيف فقط
                    if ((firstOrder.WorkOrderStatusID == 8 || firstOrder.WorkOrderStatusID == 9) && (requestId != "7"))
                    {
                        lbl_InsertResult.Text = Tokens.Cancelation_process_or_canceled_user;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }

                    //حالة العميل يجب ان تكون اكتف عند عمل سسبند للعميل
                    if (firstOrder.WorkOrderStatusID != 6 && requestId == "2")
                    {
                        lbl_InsertResult.Text = Tokens.ToSuspendShouldbeActive;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                    //حالة العميل يجب ان تكون اكتف عند عمل طلب خفض رفع
                    if (firstOrder.WorkOrderStatusID != 6 && requestId == "1")
                    {
                        lbl_InsertResult.Text = Tokens.ToUpgradeDowngradShouldbeActive;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                    //حالة العميل يجب ان تكون اكتيف عند عمل طلب تعليق عميل Hold
                    if (firstOrder.WorkOrderStatusID != 6 && requestId == "4")
                    {
                        lbl_InsertResult.Text = Tokens.ToHoldShouldbeActive;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                    //حالة العميل يجب ان تكون اكتف عند عمل طلب تغير اى بى
                    if (firstOrder.WorkOrderStatusID != 6 && requestId == "8")
                    {
                        lbl_InsertResult.Text = Tokens.ToaddIPShouldbeActive;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                    // حالة العميل يجب ان تكون اكتف عند عمل طلب جيجات اضافية
                    if (firstOrder.WorkOrderStatusID != 6 && requestId == "9")
                    {
                        lbl_InsertResult.Text = Tokens.ToAddGigaShouldbeActive;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                   
                    // في حالة السسبند يجب مرور شهر علي اعادة التفعيل
                    var option = OptionsService.GetOptions(db8, true);
                    if (option != null && (option.PreventSuspendBeforeMonthFromReActive ?? false))
                    {
                        if (firstOrder.WorkOrderStatusID == 6 && requestId == "2")
                        {

                            var woid = firstOrder.ID;
                            var req =
                                db8.WorkOrderRequests.Where(
                                    x => x.WorkOrderID == woid && x.RSID == 1 && x.RequestID == 7)
                                    .ToList();
                            var lastreq = req.LastOrDefault();
                            if (lastreq != null && lastreq.ProcessDate != null &&
                                lastreq.ProcessDate.Value.AddMonths(1) > DateTime.Now)
                            {
                                lbl_InsertResult.Text = Tokens.CantSuspend;

                                lbl_InsertResult.ForeColor = Color.Red;
                                return;
                            }


                        }
                    }




                    if (requestId == "2") //Suspend
                    {
                        var order = db8.WorkOrders.FirstOrDefault(wo => wo.ID == Convert.ToInt32(ViewState["woid"])
                                                                        && wo.WorkOrderStatusID == 11);

                        if (order != null)
                        {
                            lbl_InsertResult.Text = Tokens.Duplicate_Suspend2;
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }
                    }

                    if (requestId == "3") //UnSuspended
                    {
                        var query3 = db8.WorkOrders.Where(wo => wo.ID == Convert.ToInt32(ViewState["woid"])
                                                                && (wo.WorkOrderStatusID == 11));
                        if (!query3.Any())
                        {
                            lbl_InsertResult.Text = Tokens.Cant_Unsuspend;
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }

                        //فى حالة العميل علية مديونية
                        //var option = db8.Options.FirstOrDefault();
                        if (option != null && (option.PreventUnsusForCustomerHasIndebtedness ?? false))
                        {
                            var demands = db8.Demands.Where(x => x.WorkOrderId == Convert.ToInt32(ViewState["woid"]) && x.Paid == false && x.Amount > 0).ToList();
                            if (demands.Count > 0)
                            {
                                lbl_InsertResult.Text = Tokens.CantUnsuspendCustomerHasIndebtedness;
                                lbl_InsertResult.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }
                    /* else if(ViewState["RequestID"].ToString() == "9") //Request Extra Giga
                {
                    var Query3 = from wo in db8.WorkOrders
                        where
                            wo.ID == Convert.ToInt32(ViewState["woid"])
                        select wo;
                    if(Query3.First().ServicePackage.ServicePackagesType.ID == 2) //Unlimited
                    {
                        lbl_InsertResult.Text = Tokens.Cant_RequestForUnlimitedCustomers;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                
                }}*/
                    else if (requestId == "5") //Hold
                    {
                        var query3 = db8.WorkOrders.Where(wo => wo.ID == Convert.ToInt32(ViewState["woid"])
                                                                && (wo.WorkOrderStatusID == 10));
                        if (!query3.Any())
                        {
                            lbl_InsertResult.Text = Tokens.Cant_Unhold;
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }
                    }

                    //Case Cancel requests change the work order state to cancelation process

                    if (ddl_Requests.SelectedItem.Value == @"6") //Cancel Request
                    {
                        var query = from wo in db8.WorkOrders
                            where wo.ID == Convert.ToInt32(ViewState["woid"])
                                  &&
                                  (wo.WorkOrderStatusID == 6 || wo.WorkOrderStatusID == 10 || wo.WorkOrderStatusID == 11)
                            //Active , suspended or hold
                            select wo;
                        if (query.Any())
                        {
                            //WorkOrder uwo = query.First();
                            //uwo.WorkOrderStatusID = 8;
                            //db8.SubmitChanges();
                        }
                        else
                        {
                            lbl_InsertResult.Text = Tokens.Cant_Cancel;
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }
                    }

                    //Case ReActive requests occur to cancelled or cancelation process

                    if (ddl_Requests.SelectedItem.Value == @"7") //ReActive Request
                    {
                        var query = from wo in db8.WorkOrders
                            where wo.ID == Convert.ToInt32(ViewState["woid"])
                                  && (wo.WorkOrderStatusID == 8 //cacelation process , cancelled 
                                      || wo.WorkOrderStatusID == 9
                                      || wo.WorkOrderStatusID == 7)
                            select wo;

                        if (!query.Any())
                        {
                            lbl_InsertResult.Text = Tokens.Cant_Reactivate;
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }
                    }

                    var wor = new WorkOrderRequest
                    {
                        WorkOrderID = Convert.ToInt32(ViewState["woid"]),
                        CurrentPackageID = Convert.ToInt32(ViewState["ServicePackageID"])
                    };
                    if (DdlExtraGigas.SelectedIndex > 0)
                    {
                        wor.ExtraGiga =
                            db8.ExtraGigas.FirstOrDefault(x => x.Id == Convert.ToInt32(DdlExtraGigas.SelectedItem.Value));
                    }

                    wor.NewPackageID = wor.CurrentPackageID;
                    //Suspended
                    if (requestId == "1")
                    {
                        wor.NewPackageID = Convert.ToInt32(ddl_ServicePackage.SelectedItem.Value);
                    }

                    var date = DateTime.Parse(lbl_RequestDate.Text, CultureInfo.InvariantCulture);
                    wor.RequestDate = date;
                    wor.RequestID = Convert.ToInt32(ddl_Requests.SelectedItem.Value);
                    wor.RSID = 3;
                    if (ddl_IpPackage.SelectedIndex > 0)
                    {
                        wor.NewIpPackageID = Convert.ToInt32(ddl_IpPackage.SelectedItem.Value);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hf_woid.Value))
                        {
                            var workOrder = db8.WorkOrders.FirstOrDefault(w => w.ID == Convert.ToInt32(hf_woid.Value));
                            if (workOrder != null)
                            {
                                wor.NewIpPackageID = workOrder.IpPackageID;
                            }
                        }
                    }
                    var portalMsg = string.Empty;
                    //send suspend request to portal
                    if (requestId == "2")
                    {
                        var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                        var woproviderList =
                            db8.WorkOrders.FirstOrDefault(z => z.ID == Convert.ToInt32(ViewState["woid"]));
                        if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                        {
                            var username = woproviderList.UserName;
                            CookieContainer cookiecon = new CookieContainer();
                            cookiecon = Tedata.Login();
                            if (cookiecon != null)
                            {
                                var pagetext = Tedata.GetSearchPage(username, cookiecon);
                                if (pagetext != null)
                                {
                                    var searchPage = Tedata.CheckSearchPage(pagetext);
                                    if (searchPage)
                                    {
                                        var custStatus = Tedata.CheckCustomerStatus(pagetext);
                                        if (custStatus == "disable")
                                        {
                                            Session["portalmsg"] = "هذا العميل موقوف بالفعل على البورتال";
                                            Session["portalfail"] = true;
                                            portalResult.Visible = true;
                                            portalResult.InnerHtml = "هذا العميل موقوف بالفعل على البورتال";
                                            portalResult.Attributes.Add("class", "alert alert-danger");
                                            return;
                                        }
                                        else
                                        {
                                            var worNote = Tedata.SendTedataSuspendRequest(username, cookiecon, pagetext);
                                            if (worNote == 2)
                                            {
                                                Session["portalfail"] = true;
                                                Session["portalmsg"] =
                                                    "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                                //فى حالة البورتال واقع
                                                wor.Notes =
                                                    "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                            }
                                            else
                                            {
                                                //فى حالة نجاح الارسال الى البورتال
                                                //ينزل الطلب متوافق علية فى اى اس بى
                                                wor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                                wor.ProcessDate = DateTime.Now.AddHours();
                                                wor.RSID = 1;
                                                wor.SenderID = Convert.ToInt32(Session["User_ID"]);
                                                wor.Notes = notes.Value;
                                                db8.WorkOrderRequests.InsertOnSubmit(wor);
                                                //db8.SubmitChanges();

                                                //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                var current = db8.WorkOrders.FirstOrDefault(x => x.ID == wor.WorkOrderID);

                                                if (current != null)
                                                {
                                                    current.WorkOrderStatusID = 11;

                                                    global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                                                    {
                                                        WorkOrderID = current.ID,
                                                        StatusID = 11,
                                                        UserID = 1,
                                                        UpdateDate = DateTime.Now.AddHours(),
                                                    };
                                                    db8.WorkOrderStatus.InsertOnSubmit(wos);
                                                }

                                                db8.SubmitChanges();
                                                Session["portalsuccess"] = true;
                                                Session["portalmsg"] = "تم إرسال الطلب الى البورتال بنجاح";
                                                portalResult.Visible = true;
                                                portalResult.InnerHtml = Tokens.Request_Added_successfully;
                                                portalResult.Attributes.Add("class", "alert alert-success");
                                                Session["reloadrepquestPage"] = true;
                                                if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
                                                {
                                                    portalMsg = "تم إرسال الطلب الى البورتال بنجاح";
                                                    var s = Request.QueryString["rid"];
                                                    Response.Redirect("~/Pages/AddNewRequest.aspx?rid=" + s);
                                                }

                                                
                                                return;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Session["portalfail"] = true;
                                        Session["portalmsg"] = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                        portalResult.Visible = true;
                                        portalResult.InnerHtml =
                                            "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                        portalResult.Attributes.Add("class", "alert alert-danger");
                                        wor.Notes =
                                            "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                    }

                                }
                                else
                                {
                                    Session["portalfail"] = true;
                                    Session["portalmsg"] = "تعذر الوصول الى البورتال";
                                    portalResult.Visible = true;
                                    portalResult.InnerHtml = "تعذر الوصول الى البورتال";
                                    portalResult.Attributes.Add("class", "alert alert-danger");
                                    wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                }
                            }
                            else
                            {
                                Session["portalfail"] = true;
                                //فى حالة البورتال واقع
                                Session["portalmsg"] = "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                portalResult.Visible = true;
                                portalResult.InnerHtml = "تعذر الوصول الى البورتال";
                                portalResult.Attributes.Add("class", "alert alert-danger");
                                wor.Notes = "لم يتم ارسال الطلب إلى البورتال بسبب عدم إستجابة البورتال رجاءً تأكد من Portal User Name or Portal Password";
                            }
                        }
                    }

                    //send Un suspend request to tedata
                    if (requestId == "3")
                    {
                
                        var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                        var woproviderList =
                            db8.WorkOrders.FirstOrDefault(z => z.ID == Convert.ToInt32(ViewState["woid"]));
                        if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                        {
                            var username = woproviderList.UserName;
                            CookieContainer cookiecon = new CookieContainer();
                            cookiecon = Tedata.Login();
                            if (cookiecon != null)
                            {
                                var pagetext = Tedata.GetSearchPage(username, cookiecon);
                                if (pagetext != null)
                                {
                                    var searchPage = Tedata.CheckSearchPage(pagetext);
                                    if (searchPage)
                                    {

                                        var custStatus = Tedata.CheckCustomerStatus(pagetext);
                                        if (custStatus == "enable")
                                        {
                                            Session["portalfail"] = true;
                                            Session["portalmsg"] = "هذا العميل موقوف بالفعل على البورتال";
                                            portalResult.Visible = true;
                                            portalResult.InnerHtml = "هذا العميل مفعل بالفعل على البورتال";
                                            portalResult.Attributes.Add("class", "alert alert-danger");
                                            return;
                                        }
                                        else
                                        {
                                            var worNote = Tedata.SendTedataUnSuspendRequest(username, cookiecon,
                                                pagetext);
                                            if (worNote == 2)
                                            {
                                                Session["portalfail"] = true;
                                                Session["portalmsg"] =
                                                   "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                                //فى حالة البورتال واقع
                                                wor.Notes =
                                                    "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                            }
                                            else
                                            {
                                                //ينزل الطلب متوافق علية فى اى اس بى
                                                wor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                                wor.ProcessDate = DateTime.Now.AddHours();
                                                wor.RSID = 1;
                                                wor.SenderID = Convert.ToInt32(Session["User_ID"]);
                                                wor.Notes = notes.Value;
                                                db8.WorkOrderRequests.InsertOnSubmit(wor);
                                                //db8.SubmitChanges();

                                                //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                var current = db8.WorkOrders.FirstOrDefault(x => x.ID == wor.WorkOrderID);
                                             
                                                //تحديث تاريخ المطالبة باضافة عدد ايام السسبند
                                                try
                                                {
                                                var susday = CountSuspenddays(current.ID);
                                                var curDate = current.RequestDate.Value;
                                                current.RequestDate = curDate.AddDays(susday);
                                                 db8.SubmitChanges();
                                                }
                                                catch 
                                                {
                                                    
                                                   
                                                }
                                              
                                                //
                                                
                                                if (current != null)
                                                {
                                                    current.WorkOrderStatusID = 6;

                                                    global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                                                    {
                                                        WorkOrderID = current.ID,
                                                        StatusID = 6,
                                                        UserID = 1,
                                                        UpdateDate = DateTime.Now.AddHours(),
                                                    };
                                                    db8.WorkOrderStatus.InsertOnSubmit(wos);
                                                }

                                                db8.SubmitChanges();
                                                Session["portalsuccess"] = true;
                                                Session["portalmsg"] = "تم إرسال الطلب الى البورتال بنجاح";
                                                Session["reloadrepquestPage"] = true;
                                                if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
                                                {
                                                    portalMsg = "تم إرسال الطلب الى البورتال بنجاح";
                                                    var s = Request.QueryString["rid"];
                                                    Response.Redirect("~/Pages/AddNewRequest.aspx?rid=" + s);
                                                }


                                                // ترحيل ايام السسبند
                                                int daysCount = _ispEntries.DaysForCustomerAtStatus(wor.ID, 11);
                                                //var option = OptionsService.GetOptions(db8, true);
                                                if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                                {
                                                    wor.RequestDate.Value.AddDays(daysCount);
                                                    _ispEntries.Commit();
                                                }


                                                portalResult.Visible = true;
                                                portalResult.InnerHtml = Tokens.Request_Added_successfully;
                                                portalResult.Attributes.Add("class", "alert alert-success");
                                                return;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Session["portalfail"] = true;
                                        Session["portalmsg"] = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                        portalResult.Visible = true;
                                        portalResult.InnerHtml =
                                            "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                        portalResult.Attributes.Add("class", "alert alert-danger");
                                        wor.Notes =
                                            "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                    }

                                }
                                else
                                {
                                    Session["portalfail"] = true;
                                    Session["portalmsg"] = "تعذر الوصول الى البورتال";
                                    portalResult.Visible = true;
                                    portalResult.InnerHtml = "لا يمكن الاتصال بسيرفر البورتال";
                                    portalResult.Attributes.Add("class", "alert alert-danger");
                                    //فى حالة البورتال واقع
                                    wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                }
                            }
                            else
                            {
                                Session["portalfail"] = true;
                                //فى حالة البورتال واقع
                                Session["portalmsg"] = "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                portalResult.Visible = true;
                                portalResult.InnerHtml = "لا يمكن الاتصال بسيرفر البورتال";
                                portalResult.Attributes.Add("class", "alert alert-danger");
                                wor.Notes = "لم يتم ارسال الطلب إلى البورتال بسبب عدم إستجابة البورتال رجاءً تأكد من Portal User Name or Portal Password  ";
                            }
                        }





                    }

                    wor.Notes += notes.Value;
                    wor.SenderID = Convert.ToInt32(Session["User_ID"]);
                    db8.WorkOrderRequests.InsertOnSubmit(wor);
                    db8.SubmitChanges();

                     
                    Session["reloadrepquestPage"] = true;
                    if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
                    {
                        var s = Request.QueryString["rid"];
                        Response.Redirect("~/Pages/AddNewRequest.aspx?rid=" + s);
                    }
                }
            }
            protected int CountSuspenddays(int workorderId)
            {
                var ispEntries = new IspEntries();
                return ispEntries.DaysForCustomerAtStatus(workorderId, 11);
            }
        }
    }
 