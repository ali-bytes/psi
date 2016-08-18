using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
    public partial class BranchPR : CustomPage
    {
        
            //static bool BranchPrintExcel; //it should be static

            // readonly ISPDataContext _context;
        private readonly DemandService _demandService;
        private readonly IspEntries _ispEntries;
            readonly BranchCreditRepository _creditRepository;

            readonly DemandSearch _demandSearch;
            public Option WidthOption { get; set; }


            void PopulateCustomers()
            {

                if (string.IsNullOrEmpty(HiddenField1.Value)) return;
                var branchId = Convert.ToInt32(HiddenField1.Value);

                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
           var demands = _demandSearch.BranchDemandsAdo(branchId);
               
                    var previousRequests =
                        context.WorkOrderRequests
                            .Where(r =>
                                r.RequestID == 11 &&
                                r.RSID == 3 &&
                                r.WorkOrder.BranchID == branchId && (r.WorkOrder.WorkOrderStatusID != 8 || r.WorkOrder.WorkOrderStatusID != 9) &&
                                (r.WorkOrder.ResellerID == null || r.WorkOrder.ResellerID == -1))
                            .ToList();

                    var credit = _creditRepository.GetNetCredit(branchId);
                    var resultModels = new List<DemandResultModel2>();
                    var newlist = new List<DemandResultModel2>();
                    var lastlist = new List<DemandResultModel2>();
                   
                    resultModels = demands.Where(w=> previousRequests.All(x => x.WorkOrderID != w.WorkorderId)).ToList();
                    var foundedproviders = context.OptionProviders.ToList();
                    foreach (var item in foundedproviders)
                    {
                        var item1 = item;
                        var data = resultModels.Where(a => a.Provider == item1.ServiceProvider.SPName).ToList();
                        newlist.AddRange(data);
                    }
                    var option = OptionsService.GetOptions(context, true);
                    if (Convert.ToBoolean(option.ShowAllDemandOfPR))
                    {
                        lastlist = newlist;
                    }
                    else
                    {
                        foreach (var demandResultModel in newlist)
                        {
                            var model = demandResultModel;
                            var check = lastlist.Where(r => r.Phone == model.Phone).ToList();
                            if (check.Count == 0)
                            {
                                var resultModel = demandResultModel;
                                var sametouser = newlist.Where(s => s.Phone == resultModel.Phone).ToList();
                                var lastdemand = sametouser.OrderByDescending(f => f.Id).FirstOrDefault(); //();
                                if (lastdemand != null && !lastdemand.Paid) lastlist.Add(lastdemand);
                            }
                        }
                    }
                    var l = lastlist.Where(w => w.Isrequested != true && !w.Paid && previousRequests.All(x => x.WorkOrderID != w.WorkorderId)).ToList();

                  
                    gv_customers.DataSource = l.OrderBy(x => x.WorkorderId);
                    gv_customers.DataBind();
                    L_branchCredit.Text = Helper.FixNumberFormat(credit);
                    l_customersDue.Text = Helper.FixNumberFormat(l.Sum(x => x.DAmount));
                    var amount = previousRequests.Sum(s => s.Demand.Amount);
                    lblavailableCredit.Text = Helper.FixNumberFormat(credit - amount);
                  
                }
            }

        private void HandleRequests()
        {

            using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var selectedBranch = ddl_branches.SelectedItem.Value;
                var branchId = Convert.ToInt32(selectedBranch);
                var resellerCredit = _creditRepository.GetNetCredit(branchId);
                var customersDue = CalculateSelected();

                var branchrequests =
                    context1.WorkOrderRequests.Where(
                        a =>
                            a.WorkOrder.BranchID == branchId && a.RSID == 3 && a.RequestID == 11 &&
                            a.WorkOrder.ResellerID == null).ToList();
                var amount2 = branchrequests.Sum(s => s.Demand.Amount);
                var newbranchcredit = resellerCredit - amount2;

                if (newbranchcredit < customersDue)
                {
                    l_message.Visible = true;
                    l_message.Text = Tokens.CreditIsntEnough;

                    foreach (GridViewRow row in gv_customers.Rows)
                    {
                        var selected = row.FindControl("gv_cbRequested") as CheckBox;
                        selected.Checked = false;
                    }
                    return;
                }
                else
                {
                    CreateRequests();
                }
            }
        }

        void CreateRequests()
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Userid = Convert.ToInt32(Session["User_ID"]);
                    var requests = new List<WorkOrderRequest>();
                    foreach (GridViewRow row in gv_customers.Rows)
                    {
                        var selected = row.FindControl("gv_cbRequested") as CheckBox;
                        var totalLabel = row.FindControl("gv_lDue") as Label;
                        var workOrderIdHiddenField = row.FindControl("gv_hfId") as HiddenField;
                        if (workOrderIdHiddenField == null || selected == null || !selected.Checked ||
                            totalLabel == null) continue;
                        var demand =
                            context1.Demands.FirstOrDefault(x => x.Id == Convert.ToInt32(workOrderIdHiddenField.Value));
                        if (demand == null) continue;
                        if (Session["User_ID"] == null) continue;
                        //chcek if this request already exists in WorkOrderRequest
                        var check =
                            context1.WorkOrderRequests.Where(
                                a => a.RequestID == 11 && a.RSID == 3 && a.WorkOrderID == demand.WorkOrderId && a.DemandId == demand.Id);
                        //if (check.Any()) continue;
                        var option = OptionsService.GetOptions(context1, false);
                        var op = option.AutoBranchPayment != null && Convert.ToBoolean(option.AutoBranchPayment);

                        if (op)
                         {


                            if (demand != null && demand.WorkOrder.BranchID != null)
                            {
                                var credit = _creditRepository.GetNetCredit(demand.WorkOrder.BranchID.Value);
                                if (demand.Amount != null && credit < demand.Amount)
                                {
                                    Div1.Visible = true;
                                    Div1.InnerHtml = Tokens.NotEnoughtCreditMsg;
                                    Div1.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                                var firstOrDefault =
                                    context1.BranchPackagesDiscounts.FirstOrDefault(
                                        r =>
                                            r.BranchId == demand.WorkOrder.BranchID &&
                                            r.ProviderId == demand.WorkOrder.ServiceProviderID &&
                                            r.PackageId == demand.WorkOrder.ServicePackageID);

                                var discount = firstOrDefault != null ? firstOrDefault.Discount : 0;
                                var netdiscount = demand.Amount * discount / 100;
                                var amount = demand.Amount - netdiscount;

                                var result = _creditRepository.Save(Convert.ToInt32(demand.WorkOrder.BranchID.Value),
                                    Userid, Convert.ToDecimal(amount) * -1,
                                    demand.WorkOrder.CustomerName + " - " + demand.WorkOrder.CustomerPhone,
                                    DateTime.Now.AddHours());
                                if (result == SaveResult.NoCredit)
                                {
                                    Div1.Visible = true;
                                    Div1.InnerHtml = Tokens.NotEnoughtCreditMsg;
                                    Div1.Attributes.Add("class", "alert alert-danger");
                                    return;
                                   }
                                demand.Paid = true;
                                demand.UserId = Userid;
                                demand.PaymentDate = DateTime.Now.AddHours();
                                demand.IsRequested = true;

                                var requestOrder2 = new WorkOrderRequest
                                {
                                    WorkOrderID = demand.WorkOrderId,
                                    RequestID = 11,
                                    RequestDate = DateTime.Now.AddHours(),
                                    RSID = 1,
                                    SenderID = Userid,
                                    CurrentPackageID = demand.WorkOrder.ServicePackageID,
                                    NewPackageID = demand.WorkOrder.ServicePackageID,
                                    Total = demand.Amount,
                                    ExtraGigaId = demand.WorkOrder.ExtraGigaId,
                                    NewIpPackageID = demand.WorkOrder.IpPackageID,
                                    Demand = demand,
                                    RejectReason = "تم الموافقة تلقائيا",
                                    ProcessDate = DateTime.Now.AddHours(),
                                    ConfirmerID = Userid
                                };
                                //requests.Add(requestOrder);
                                context1.WorkOrderRequests.InsertOnSubmit(requestOrder2);
                                context1.SubmitChanges();
                        /*        context1.WorkOrderRequests.InsertAllOnSubmit(requests);
                                context1.SubmitChanges();*/
                       
                             
                            
                                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                                {
                                    CenterMessage.SendRequestApproval(demand.WorkOrder, Tokens.BranchPR, Userid);
                                }
                            }
                            //--------------
                            var workOrderRequest2 =
                           context1.WorkOrderRequests.Where(
                               wor =>
                                   wor.WorkOrderID == demand.WorkOrderId && wor.RequestID == 2 && wor.RSID == 3 &&
                                   (wor.IsProviderRequest == true || wor.IsProviderRequest == null))
                               .Select(z => z)
                               .ToList();
                            var workOrderRequest = workOrderRequest2.LastOrDefault();
                            if (workOrderRequest != null)
                            {
                                workOrderRequest.RSID = 2;
                                workOrderRequest.RejectReason = " تم الغاء الطلب بدفع فاتورة العميل ";
                                workOrderRequest.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                workOrderRequest.ProcessDate = DateTime.Now.AddHours();
                                context1.SubmitChanges();
                            }

                            //---------------------
                            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                            {
                                var userId = Convert.ToInt32(Session["User_ID"]);
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
                                            var portalList2 = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                                            var woproviderList2 = db8.WorkOrders.FirstOrDefault(z => z.ID == wid);
                                            if (woproviderList2 != null && portalList2.Contains(woproviderList2.ServiceProviderID))
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
                                                                portalRequest.Visible = true;
                                                                portalRequest.InnerHtml = "هذا العميل مفعل بالفعل على البورتال";
                                                                portalRequest.Attributes.Add("class", "alert alert-danger");
                                                                portalIsStoped = true;
                                                            }
                                                            else
                                                            {
                                                                var worNote = Tedata.SendTedataUnSuspendRequest(username, cookiecon,
                                                                    pagetext);
                                                                if (worNote == 2)
                                                                {
                                                                    portalRequest.Visible = true;
                                                                    portalRequest.InnerHtml = "تعذر الوصول الى البورتال";
                                                                    portalRequest.Attributes.Add("class", "alert alert-danger");
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
                                                                        SenderID = userId,
                                                                        ProcessDate = DateTime.Now.AddHours(),
                                                                        ConfirmerID = userId,
                                                                        Notes = "تم تشغيل العميل بدفع فاتورة"
                                                                    };
                                                                    db8.WorkOrderRequests.InsertOnSubmit(request2);
                                                                    db8.SubmitChanges();

                                                                    //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                                    var current = db8.WorkOrders.FirstOrDefault(x => x.ID == wid);
                                                                    if (current != null)
                                                                    {
                                                                        current.WorkOrderStatusID = 6;
                                                                        global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                                                                        {
                                                                            WorkOrderID = current.ID,
                                                                            StatusID = 6,
                                                                            UserID = userId,
                                                                            UpdateDate = DateTime.Now.AddHours(),
                                                                        };
                                                                        db8.WorkOrderStatus.InsertOnSubmit(wos);
                                                                        db8.SubmitChanges();
                                                                    }


                                                                    // ترحيل ايام السسبند
                                                                    int daysCount = _ispEntries.DaysForCustomerAtStatus(order.ID, 11);
                                                                   
                                                                    if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                                                    {
                                                                        var date33 =
                                                                           order.RequestDate.Value.AddDays(daysCount);
                                                                        order.RequestDate = date33.Date;
                                                                        db8.SubmitChanges();
                                                                    }

                                                                    portalRequest.Visible = true;
                                                                    portalRequest.InnerHtml = "تم إرسال طلب التشغيل الى البورتال بنجاح";
                                                                    portalRequest.Attributes.Add("class", "alert alert-success");
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            portalIsStoped = true;
                                                            portalRequest.Visible = true;
                                                            portalRequest.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                                            portalRequest.Attributes.Add("class", "alert alert-danger");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        portalIsStoped = true;
                                                        portalRequest.Visible = true;
                                                        portalRequest.InnerHtml = "فشل الأتصال بالبورتال";
                                                        portalRequest.Attributes.Add("class", "alert alert-danger");
                                                    }
                                                }
                                                else
                                                {
                                                    portalIsStoped = true;
                                                    portalRequest.Visible = true;
                                                    portalRequest.InnerHtml =
                                                        "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                                    portalRequest.Attributes.Add("class", "alert alert-danger");
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
                                                        SenderID = userId,
                                                        Notes = "طلب تشغيل عن طريق دفع فاتورة"
                                                    };
                                                    db8.WorkOrderRequests.InsertOnSubmit(request3);
                                                    db8.SubmitChanges();

                                                }

                                                //wor.IsProviderRequest = false;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }






                         
                           
                        }
                        else
                        {


                           
                            var requestOrder = new WorkOrderRequest
                            {
                                WorkOrderID = demand.WorkOrderId,
                                RequestID = 11,
                                RequestDate = DateTime.Now.AddHours(),
                                RSID = 3,
                                SenderID = Userid,
                                CurrentPackageID = demand.WorkOrder.ServicePackageID,
                                NewPackageID = demand.WorkOrder.ServicePackageID,
                                Total = demand.Amount,
                                ExtraGigaId = demand.WorkOrder.ExtraGigaId,
                                NewIpPackageID = demand.WorkOrder.IpPackageID,
                                Demand = demand
                            };
                            //requests.Add(requestOrder);
                            context1.WorkOrderRequests.InsertOnSubmit(requestOrder);
                            context1.SubmitChanges();
                    /*        context1.WorkOrderRequests.InsertAllOnSubmit(requests);
                            context1.SubmitChanges();*/
                        }
                    }
                    //if (requests.Count == 0) return;
                    //var selectedBranch = ddl_branches.SelectedItem.Value;
                    //var branchId = Convert.ToInt32(selectedBranch);
                    //var resellerCredit = _creditRepository.GetNetCredit(branchId);
                    //var customersDue = requests.Sum(x => x.Total != null ? x.Total.Value : 0);

                    //var branchrequests = context1.WorkOrderRequests.Where(a => a.WorkOrder.BranchID == branchId && a.RSID == 3 && a.RequestID == 11 && a.WorkOrder.ResellerID == null).ToList();
                    //var amount2 = branchrequests.Sum(s => s.Demand.Amount);
                    //var newbranchcredit = resellerCredit - amount2;
                
                    //if (newbranchcredit < customersDue)
                    //{
                    //    l_message.Visible = true;
                    //    l_message.Text = Tokens.CreditIsntEnough;

                    //    foreach (GridViewRow row in gv_customers.Rows)
                    //    {
                    //        var selected = row.FindControl("gv_cbRequested") as CheckBox;
                    //        selected.Checked = false;
                    //    }
                    //    return;
                    //}
                    //else
                    //{
                    //    context1.WorkOrderRequests.InsertAllOnSubmit(requests);
                    //    context1.SubmitChanges();
                    //}
               

                    Response.Redirect("~/Pages/BranchPR.aspx");
                }
            }

        private decimal CalculateSelected()
        {
            decimal total = 0;
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
                    total += Convert.ToDecimal(dueLabel.Text);
                }
            }
            return total;
        }


            public  BranchPR()
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    _demandService = new DemandService(IspDataContext);
                    _ispEntries = new IspEntries();
                    _demandSearch = new DemandSearch();
                    _creditRepository = new BranchCreditRepository();
                    WidthOption = context2.Options.Any() ? context2.Options.FirstOrDefault() : new Option();
                }
            }


            void BuildUponUserType()
            {
                using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (Session["User_ID"] == null) return;
                    var id = Convert.ToInt32(Session["User_ID"]);
                    var user = context3.Users.FirstOrDefault(u => u.ID == id);
                    if (user == null) return;
                    var branches = new List<Branch>();
                    switch (user.GroupID)
                    {
                        case 1: // admin
                            branches = GetBranches(id);
                            break;
                        case 4:
                            var all = context3.UserBranches.Where(b => b.UserID == id);
                            if (all.Any())
                            {
                                all.ToList().ForEach(x => branches.Add(x.Branch));
                            }
                            break;
                        default:
                            branches = GetBranches(id);
                            break;
                    }
                    ddl_branches.DataSource = branches;
                    ddl_branches.DataValueField = "ID";
                    ddl_branches.DataTextField = "BranchName";
                    ddl_branches.DataBind();
                    Helper.AddDefaultItem(ddl_branches);
                }
            }


            List<Branch> GetBranches(int? id)
            {
                using (var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var branches = new List<Branch>();
                    if (id != null)
                    {
                        var user = context4.Users.FirstOrDefault(x => x.ID == id);
                        if (user == null)
                        {
                            Response.Redirect("default.aspx");
                        }
                        else
                        {
                            if (user.GroupID != 1)
                            {
                                branches.Add(user.Branch);
                                return branches;
                            }
                            branches = context4.Branches.ToList();
                            return branches;
                        }
                    }
                    return branches;
                }
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                BuildUponUserType();
            }


            protected void b_addRequest_Click(object sender, EventArgs e)
            {
                resellerId.Value = ddl_branches.SelectedItem.Value;
                PopulateCustomers();
                HiddenField1.Value = ddl_branches.SelectedItem.Value;
                l_message.Text = string.Empty;
            }


            protected void gv_customers_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_customers, "gv_lNumber");
            }


            protected void b_save_Click(object sender, EventArgs e)
            {
                HandleRequests();
            }

            protected void Export_OnClick(object sender, EventArgs e)
            {
                //BranchPrintExcel = true;
                string attachment = string.Format("attachment; filename={0}.xls",
                    "available_" + Title + "s" + "@__"
                    + DateTime.Now.AddHours().Date.ToShortDateString());
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                gv_customers.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }



            public override void VerifyRenderingInServerForm(Control control) { }

            protected void LinkBtnDemandReciept_Command(object sender, CommandEventArgs e)
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect(string.Format("DemandReciept.aspx?d={0}", QueryStringSecurity.Encrypt(id.ToString())));
            }
            protected void LinkBtnsmallDemandReciept_Command(object sender, CommandEventArgs e)
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect(string.Format("smallDemandReciept.aspx?d={0}", QueryStringSecurity.Encrypt(id.ToString())));
            }
        }
    }
 