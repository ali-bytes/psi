using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerPR : CustomPage
    {

        private readonly IspEntries _ispEntries;
        private readonly DemandService _demandService;
        private readonly IResellerCreditRepository _creditRepository;

        private readonly BranchCreditRepository _branchCreditRepository;

        private readonly DemandSearch _demandSearch;

        public ResellerPR()
        {

            _branchCreditRepository = new BranchCreditRepository();
            _creditRepository = new ResellerCreditRepository();
            _demandSearch = new DemandSearch();
            _demandService = new DemandService(IspDataContext);
            _ispEntries = new IspEntries();
        }


        private void PopulateReseller()
        {
            if (Session["User_ID"] == null)
            {
                Response.Redirect("default.aspx");
                return;
            }
            var allresellers =
                _creditRepository.GetResellersUponUserGroupWithCredit(Convert.ToInt32(Session["User_ID"])).ToList();
            ddl_reseller.DataSource = allresellers;
            ddl_reseller.DataTextField = "UserName";
            ddl_reseller.DataValueField = "Id";
            ddl_reseller.DataBind();
            Helper.AddAllDefaultItem(ddl_reseller);
        }


        private void ShowResults(bool show)
        {
            resultsPanel.Visible = show;
            searchPanel.Visible = !show;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            PopulateReseller();
            ShowResults(false);
            l_message.Visible = false;
            l_message.InnerHtml = string.Empty;
        }


        protected void b_addRequest_Click(object sender, EventArgs e)
        {
            PopulateCustomers();
            ShowResults(true);
        }


        protected void gv_customers_DataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(gv_customers, "gv_lNumber");
        }


        protected void b_save_Click(object sender, EventArgs e)
        {
            HandleRequests();
            ShowResults(false);
            ddl_reseller.SelectedIndex = -1;
            gv_customers.DataSource = null;
            gv_customers.DataBind();
        }

        private void PopulateCustomers()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                l_message.Visible = false;
                l_message.InnerHtml = string.Empty;
                var resellerId = Convert.ToInt32(ddl_reseller.SelectedItem.Value);

                var resellerUnpaidDemands = _demandSearch.ResellerDemandsAdo(resellerId);

                var customerWithRequests = context.WorkOrderRequests
                    .Where(r =>
                        r.RequestID == 11 &&
                        r.RSID == 3 && r.WorkOrder.WorkOrderStatusID != 9 && r.WorkOrder.WorkOrderStatusID != 8 &&
                        r.WorkOrder.ResellerID == resellerId).ToList();

                var demandsToShow = resellerUnpaidDemands;
                //resellerUnpaidDemands.Where(d => customerWithRequests.All(x => x.DemandId != d.Id)).ToList();

                var resellerCredit = Convert.ToDouble(_creditRepository.GetNetCredit(resellerId));
                var newlist = new List<DemandResultModel2>();
                var lastlist = new List<DemandResultModel2>();
                var foundedproviders = context.OptionProviders.ToList();
                var option = OptionsService.GetOptions(context, true);
                foreach (var item in foundedproviders)
                {
                    var data = demandsToShow.Where(a => a.Provider == item.ServiceProvider.SPName).ToList();

                    newlist.AddRange(data);
                }
                if (Convert.ToBoolean(option.ShowAllDemandOfPR))
                {
                    lastlist = newlist;
                }
                else
                {
                    foreach (var demandResultModel in newlist)
                    {
                        var check = lastlist.Where(r => r.Phone == demandResultModel.Phone).ToList();
                        if (check.Count == 0)
                        {
                            var sametouser = newlist.Where(s => s.Phone == demandResultModel.Phone).ToList();
                            var lastdemand = sametouser.OrderByDescending(f => f.Id).FirstOrDefault(); //();
                            if (lastdemand != null && !lastdemand.Paid) lastlist.Add(lastdemand);
                        }
                    }
                }



                var l =
                    lastlist.Where(
                        w =>
                            w.Isrequested != true && !w.Paid &&
                            customerWithRequests.All(x => x.WorkOrderID != w.WorkorderId)).ToList();
                gv_customers.DataSource = l.OrderBy(x => x.WorkorderId);
                gv_customers.DataBind();



                var totalCustomersDue = l.Sum(x => x.DAmount);
                L_ResellerCredit.Text = Helper.FixNumberFormat(resellerCredit);
                l_customersDue.Text = Helper.FixNumberFormat(totalCustomersDue);
                var resellerrequests =
                    context.WorkOrderRequests.Where(
                        a => a.WorkOrder.ResellerID == resellerId && a.RSID == 3 && a.RequestID == 11).ToList();
                var amount = resellerrequests.Sum(s => s.Demand.Amount);
                lblavailableCredit.Text = Helper.FixNumberFormat(resellerCredit - Convert.ToDouble(amount));

            }
        }



        private double CalculateSelected()
        {
            var total = 0.0;
            foreach (GridViewRow row in gv_customers.Rows)
            {
                var selectedCheckBox = row.FindControl("gv_cbRequested") as CheckBox;

                if (selectedCheckBox == null)
                {
                    return 0;
                }

                var dueLabel = row.FindControl("gv_lDue") as Label;
                if (dueLabel == null)
                    return 0;

                if (selectedCheckBox.Checked)
                {
                    total += Convert.ToDouble(dueLabel.Text);
                }
            }
            return total;
        }


        private void HandleRequests()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var selectedReseller = ddl_reseller.SelectedItem.Value;
                var resellerId = Convert.ToInt32(selectedReseller);
                var resellerCredit = Convert.ToDouble(_creditRepository.GetNetCredit(resellerId));
                var x = context.Users.FirstOrDefault(a => a.ID == resellerId);
                if (x == null) return;
                var branchid = Convert.ToInt32(x.BranchID);
                var branchCredit = Convert.ToDouble(_branchCreditRepository.GetNetCredit(branchid));
                var customersDue = CalculateSelected();
                var resellerrequests =
                    context.WorkOrderRequests.Where(
                        a => a.WorkOrder.ResellerID == resellerId && a.RSID == 3 && a.RequestID == 11).ToList();

                var branchrrq =
                    context.WorkOrderRequests.Where(
                        a =>
                            a.WorkOrder.BranchID == branchid && a.RSID == 3 && a.RequestID == 11 &&
                            a.WorkOrder.ResellerID == null).ToList();
                var branchamount = branchrrq.Sum(s => s.Demand.Amount);
                var amount = resellerrequests.Sum(s => s.Demand.Amount);
                var newresellercredit = resellerCredit - Convert.ToDouble(amount);
                var newbranchcredit = branchCredit - Convert.ToDouble(branchamount);
                if (context.Options.First().DiscoundfromResellerandBranch)
                {
                    if (newresellercredit < customersDue || newbranchcredit < customersDue)
                    {
                        l_message.Visible = true;
                        l_message.InnerHtml = Tokens.NotEnoughtCreditMsg;
                        l_message.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        CreateRquest();
                    }
                }
                else
                {
                    if (newresellercredit < customersDue)
                    {
                        l_message.Visible = true;
                        l_message.InnerHtml = Tokens.ResellerCreditIsnotEnough;
                        l_message.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        CreateRquest();
                    }
                }

            }

        }



        private void CreateRquest()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var userid = Convert.ToInt32(Session["User_ID"]);
                var requeststoAdd = new List<WorkOrderRequest>();

                foreach (GridViewRow row in gv_customers.Rows)
                {
                    var selected = row.FindControl("gv_cbRequested") as CheckBox;
                    var demandHf = row.FindControl("gv_hfId") as HiddenField;
                    var totalLabel = row.FindControl("gv_lDue") as Label;
                    if (totalLabel == null || demandHf == null || selected == null || !selected.Checked) continue;
                    var demandId = Convert.ToInt32(demandHf.Value);
                    var demand = context.Demands.FirstOrDefault(d => d.Id == demandId);
                    if (demand == null) continue;

                    var check =
                        context.WorkOrderRequests.Where(
                            a => a.RequestID == 11 && a.WorkOrderID == demand.WorkOrderId && a.RSID == 3);

                    var option = OptionsService.GetOptions(context, false);
                    var op = option.AutoResellerPayment != null && Convert.ToBoolean(option.AutoResellerPayment);
                    if (op)
                    {
                        //&& a.DemandId == demandId
                        //if (check.Any()) continue;
                        var firstOrDefault =
                            context.ResellerPackagesDiscounts.FirstOrDefault(
                                r =>
                                    r.ResellerId == demand.WorkOrder.ResellerID &&
                                    r.ProviderId == demand.WorkOrder.ServiceProviderID &&
                                    r.PackageId == demand.WorkOrder.ServicePackageID);

                        var discount = firstOrDefault != null ? firstOrDefault.Discount : 0;

                        var netdscount = demand.Amount*discount/100;
                        var amount = demand.Amount - netdscount;

                        if (context.Options.First().DiscoundfromResellerandBranch)
                        {
                            var branchCredit =
                                Convert.ToDouble(
                                    _branchCreditRepository.GetNetCredit(Convert.ToInt32(demand.WorkOrder.User.BranchID)));
                            if (branchCredit >= Convert.ToDouble(demand.Amount))
                            {
                                decimal branchamount = 0;
                                if (demand != null && demand.IsResellerCommisstions != null &&
                                    demand.IsResellerCommisstions != false)
                                {
                                    var branchdiscound =
                                        context.BranchPackagesDiscounts.FirstOrDefault(
                                            r =>
                                                r.BranchId == demand.WorkOrder.BranchID &&
                                                r.ProviderId == demand.WorkOrder.ServiceProviderID &&
                                                r.PackageId == demand.WorkOrder.ServicePackageID);
                                    var dis = branchdiscound != null ? branchdiscound.Discount : 0;
                                    var netdiscount = demand.Amount*dis/100;
                                    branchamount = demand.Amount - netdiscount;


                                }

                                _branchCreditRepository.Save(Convert.ToInt32(demand.WorkOrder.BranchID), userid,
                                    Convert.ToDecimal(branchamount)*-1,
                                    demand.WorkOrder.CustomerName + " - " + demand.WorkOrder.CustomerPhone,
                                    DateTime.Now.AddHours());

                            }
                            else
                            {

                                l_message.Visible = true;
                                l_message.InnerHtml = Tokens.NotEnoughtCreditMsg;
                                l_message.Attributes.Add("class", "alert alert-danger");
                                return;

                            }
                        }

                          var result = _creditRepository.Save(Convert.ToInt32(demand.WorkOrder.ResellerID), userid, Convert.ToDecimal(amount * -1), demand.WorkOrder.CustomerName + " - " + demand.WorkOrder.CustomerPhone, DateTime.Now.AddHours());
                        switch (result)
                        {
                            case SaveResult.Saved:
                                demand.Paid = true;
                                demand.IsRequested = true;
                                demand.PaymentDate = DateTime.Now.AddHours();
                                demand.UserId = userid;

                                l_message.InnerHtml = Tokens.Saved;

                                     var requestOrder2 = new WorkOrderRequest
                        {
                            WorkOrder = demand.WorkOrder,
                            RequestID = 11,
                            RequestDate = DateTime.Now.AddHours(),
                            RSID = 1,
                            SenderID = userid,
                            CurrentPackageID = demand.WorkOrder.ServicePackageID,
                            NewPackageID = demand.WorkOrder.ServicePackageID,
                            Total = demand.Amount,
                            ExtraGigaId = demand.WorkOrder.ExtraGigaId,
                            NewIpPackageID = demand.WorkOrder.IpPackageID,
                            DemandId = demandId,
                            RejectReason = "تم الموافقة تلقائيا",
                            ProcessDate = DateTime.Now.AddHours(),
                            ConfirmerID = userid

                        };

                        //requests.Add(requestOrder2);
                                // if (requests.Count == 0) return;
                                 context.WorkOrderRequests.InsertOnSubmit(requestOrder2);
                    context.SubmitChanges();

                                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                                {
                                    var gov =
                                        context.Governorates.FirstOrDefault(
                                            a => a.ID == demand.WorkOrder.CustomerGovernorateID);
                                    CenterMessage.SendRequestApproval(demand.WorkOrder.CustomerName,
                                        demand.WorkOrder.CustomerPhone, gov != null ? gov.GovernorateName : " ",
                                        Convert.ToInt32(demand.WorkOrder.ResellerID), Tokens.ResellerPR, userid);
                                }

                                //--------------
                                var workOrderRequest2 =
                                    context.WorkOrderRequests.Where(
                                        wor =>
                                            wor.WorkOrderID == demand.WorkOrder.ID && wor.RequestID == 2 &&
                                            wor.RSID == 3 &&
                                            (wor.IsProviderRequest == true || wor.IsProviderRequest == null))
                                        .Select(z => z)
                                        .ToList();
                                var workOrderRequest = workOrderRequest2.LastOrDefault();
                                if (workOrderRequest != null)
                                {
                                    workOrderRequest.RSID = 2;
                                    workOrderRequest.RejectReason = " تم الغاء الطلب بدفع فاتورة العميل ";
                                    workOrderRequest.ConfirmerID = userid;
                                    workOrderRequest.ProcessDate = DateTime.Now.AddHours();
                                    context.SubmitChanges();
                                }
                                //---------------------
                                using (
                                    var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                                {
                                    if (demand != null)
                                    {
                                        var wid = demand.WorkOrderId;
                                        var demands = _demandService.CustomerDemands(wid );
                                        var unpaid = demands.Where(x => !x.Paid).OrderBy(a => a.Id).ToList();
                                        var order = db8.WorkOrders.FirstOrDefault(x => x.ID == wid);
                                        try
                                        {
                                            bool portalIsStoped = false;
                                            if (unpaid.Count == 0 && order.WorkOrderStatusID == 11)
                                            {
                                                var portalList2 =
                                                    db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                                                var woproviderList2 = db8.WorkOrders.FirstOrDefault(z => z.ID == wid);
                                                if (woproviderList2 != null &&
                                                    portalList2.Contains(woproviderList2.ServiceProviderID))
                                                {
                                                    var username = woproviderList2.UserName;
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
                                                                    Div1.Visible = true;
                                                                    Div1.InnerHtml =
                                                                        "هذا العميل مفعل بالفعل على البورتال";
                                                                    Div1.Attributes.Add("class", "alert alert-danger");
                                                                    portalIsStoped = true;
                                                                }
                                                                else
                                                                {
                                                                    var worNote = Tedata.SendTedataUnSuspendRequest(
                                                                        username, cookiecon,
                                                                        pagetext);
                                                                    if (worNote == 2)
                                                                    {
                                                                        Div1.Visible = true;
                                                                        Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                                                        Div1.Attributes.Add("class",
                                                                            "alert alert-danger");
                                                                        //فى حالة البورتال واقع
                                                                        //ينزل الطلب معلق فى اى اس بى
                                                                        portalIsStoped = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        //ينزل الطلب متوافق علية فى اى اس بى
                                                                        var request2 = new WorkOrderRequest
                                                                        {
                                                                            WorkOrderID = wid,
                                                                            CurrentPackageID = order.ServicePackageID,
                                                                            ExtraGigaId = order.ExtraGigaId,
                                                                            NewPackageID = order.ServicePackageID,
                                                                            RequestDate = DateTime.Now.AddHours(),
                                                                            RequestID = 3,
                                                                            RSID = 1,
                                                                            NewIpPackageID = order.IpPackageID,
                                                                            SenderID = userid,
                                                                            ProcessDate = DateTime.Now.AddHours(),
                                                                            ConfirmerID = userid,
                                                                            Notes = "تم تشغيل العميل بدفع فاتورة"
                                                                        };
                                                                        db8.WorkOrderRequests.InsertOnSubmit(request2);
                                                                        db8.SubmitChanges();

                                                                        //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                                        var current =
                                                                            db8.WorkOrders.FirstOrDefault(
                                                                                x => x.ID == wid);
                                                                        if (current != null)
                                                                        {
                                                                            current.WorkOrderStatusID = 6;
                                                                            global::Db.WorkOrderStatus wos = new global
                                                                                ::Db.
                                                                                WorkOrderStatus
                                                                            {
                                                                                WorkOrderID = current.ID,
                                                                                StatusID = 6,
                                                                                UserID = userid,
                                                                                UpdateDate = DateTime.Now.AddHours(),
                                                                            };
                                                                            db8.WorkOrderStatus.InsertOnSubmit(wos);

                                                                            // ترحيل ايام السسبند
                                                                            int daysCount = _ispEntries.DaysForCustomerAtStatus(order.ID, 11);
                                                                           
                                                                            if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                                                            {
                                                                                var date33 =
                                                                            order.RequestDate.Value.AddDays(daysCount);
                                                                                order.RequestDate = date33.Date;
                                                                                db8.SubmitChanges();
                                                                            }

                                                                            db8.SubmitChanges();
                                                                        }

                                                                        Div1.Visible = true;
                                                                        Div1.InnerHtml =
                                                                            "تم إرسال طلب التشغيل الى البورتال بنجاح";
                                                                        Div1.Attributes.Add("class",
                                                                            "alert alert-success");
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                portalIsStoped = true;
                                                                Div1.Visible = true;
                                                                Div1.InnerHtml =
                                                                    "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                                                Div1.Attributes.Add("class", "alert alert-danger");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            portalIsStoped = true;
                                                            Div1.Visible = true;
                                                            Div1.InnerHtml = "فشل الأتصال بالبورتال";
                                                            Div1.Attributes.Add("class", "alert alert-danger");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        portalIsStoped = true;
                                                        Div1.Visible = true;
                                                        Div1.InnerHtml =
                                                            "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                                        Div1.Attributes.Add("class", "alert alert-danger");
                                                        //فى حالة البورتال واقع
                                                        //ينزل الطلب معلق فى اى اس بى 
                                                    }
                                                }
                                                else
                                                {
                                                    portalIsStoped = true;
                                                }

                                                if (portalIsStoped)
                                                {
                                                    var wrq =
                                                        db8.WorkOrderRequests.Where(
                                                            a => a.WorkOrderID == wid && a.RequestID == 3 && a.RSID == 3)
                                                            .ToList();
                                                    if (wrq.Count == 0)
                                                    {
                                                        var request3 = new WorkOrderRequest
                                                        {
                                                            WorkOrderID = wid,
                                                            CurrentPackageID = order.ServicePackageID,
                                                            ExtraGigaId = order.ExtraGigaId,
                                                            NewPackageID = order.ServicePackageID,
                                                            RequestDate = DateTime.Now.AddHours(),
                                                            RequestID = 3,
                                                            RSID = 3,
                                                            NewIpPackageID = order.IpPackageID,
                                                            SenderID = userid,
                                                            Notes = "طلب تشغيل عن طريق دفع فاتورة"
                                                        };
                                                        db8.WorkOrderRequests.InsertOnSubmit(request3);
                                                        db8.SubmitChanges();

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                }
                                break;
                            case SaveResult.NoCredit:
                                l_message.InnerHtml = Tokens.NotEnoughtCreditMsg;
                                return;
                                break;
                        }




                   
                    }
                    else
                    {

                        //if (check.Any()) continue;
                        var requestOrder = new WorkOrderRequest
                        {
                            WorkOrder = demand.WorkOrder,
                            RequestID = 11,
                            RequestDate = DateTime.Now.AddHours(),
                            RSID = 3,
                            SenderID = userid,
                            CurrentPackageID = demand.WorkOrder.ServicePackageID,
                            NewPackageID = demand.WorkOrder.ServicePackageID,
                            Total = demand.Amount,
                            ExtraGigaId = demand.WorkOrder.ExtraGigaId,
                            NewIpPackageID = demand.WorkOrder.IpPackageID,
                            DemandId = demandId
                        };

                        //requests.Add(requestOrder);
                        //if (requests.Count == 0) return;
                        context.WorkOrderRequests.InsertOnSubmit(requestOrder);
                        context.SubmitChanges();
                    }

                    l_message.Visible = true;
                    l_message.InnerHtml = Tokens.Saved;
                    l_message.Attributes.Add("class", "alert alert-success");
                    ShowResults(false);







                }
            }


        }
    }
}