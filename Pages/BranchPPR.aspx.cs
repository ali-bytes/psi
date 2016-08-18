using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchPPR : CustomPage
    {
         //static bool BranchPrintExcel; //it should be static
        private readonly DemandService _demandService;
            readonly BranchCreditRepository _creditRepository;

            readonly IBoxCreditRepository _boxCreditRepository;

            public bool ManageRequests { get; set; }
            private readonly IspEntries _ispEntries;
            public  BranchPPR()
            {
                _demandService = new DemandService(IspDataContext);
                //var _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                _creditRepository = new BranchCreditRepository();
                _boxCreditRepository = new BoxCreditRepository();
                _ispEntries = new IspEntries();
            }
            private Random _random = new Random(Environment.TickCount);
            //   public string ses;
            public string RandomString()
            {
                string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
                StringBuilder builder = new StringBuilder(5);

                for (int i = 0; i < 5; ++i)
                    builder.Append(chars[_random.Next(chars.Length)]);

                return builder.ToString();
            }



            protected void Page_Load(object sender, EventArgs e)
            {
                //for no resending data when refresh page
                if (!IsPostBack)
                {
                    reload.Value = RandomString();
                    string ses = reload.Value;
                    Session[ses] =
                    Server.UrlDecode(System.DateTime.Now.ToString());

                }
                BuildUponUserType();
                PopulateBoxes();
                var userId = Convert.ToInt32(Session["User_ID"]);
                ManageRequests = _ispEntries.UserHasPrivlidge(userId, "ManagrBrancheRequest");
                Div1.Visible = false;
                Div1.InnerHtml = string.Empty;
                

            }
            protected void Page_PreRender(object sender, EventArgs e)
            {
                string ses = reload.Value;
                ViewState[ses] = Session[ses];
            }



            void PopulateBoxes()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ddlBox.DataSource = context.Boxes.Where(a => a.ShowBoxInResellerPPR == true);
                    ddlBox.DataTextField = "BoxName";
                    ddlBox.DataValueField = "ID";
                    ddlBox.DataBind();
                    Helper.AddDefaultItem(ddlBox);
                }
            }


            void BuildUponUserType()
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var id = Convert.ToInt32(Session["User_ID"]);
                    var user = context.Users.FirstOrDefault(u => u.ID == id);
                    var requests = context.WorkOrderRequests.Where(r => r.RequestID == 11 && r.RSID == 3 && r.ProcessDate == null && r.WorkOrder.ResellerID == null);
                    if (user == null) return;
                    var branches = new List<Branch>();
                    switch (user.Group.DataLevelID)
                    {
                        case 1: // admin
                            branches = GetBranches(id, context);
                            break;
                        case 2://old code==>groupId==4
                            var all = context.UserBranches.Where(b => b.UserID == id);
                            if (all.Any())
                            {
                                all.ToList().ForEach(x => branches.Add(x.Branch));
                            }
                            break;
                        default:
                            branches = GetBranches(id, context);
                            break;
                    }
                    //hf_user.Value = user.Group.DataLevelID.ToString();
                    ddl_branchs.DataSource = branches.Where(branch => requests.Any(r => r.WorkOrder.BranchID == branch.ID && r.RSID == 3 && r.RequestID == 11 && r.WorkOrder.ResellerID == null)).ToList();
                    ddl_branchs.DataValueField = "ID";
                    ddl_branchs.DataTextField = "BranchName";
                    ddl_branchs.DataBind();
                    Helper.AddDefaultItem(ddl_branchs);
                }
            }


            List<Branch> GetBranches(int? id, ISPDataContext context1)
            {
                //using(var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                var branches = new List<Branch>();
                if (id != null)
                {
                    var user = context1.Users.FirstOrDefault(x => x.ID == id);
                    if (user == null)
                    {
                        Response.Redirect("default.aspx");
                    }
                    else
                    {
                        if (user.Group.DataLevelID != 1)
                        {
                            branches.Add(user.Branch);
                        }
                        else
                        {
                            branches = context1.Branches.ToList();
                        }
                    }
                }
                return branches;
                //}
            }


            protected void b_addRequest_Click(object sender, EventArgs e)
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    FindRequests(context2);
                }
            }
            private List<WorkOrderRequest> Requests(int requestId, ISPDataContext context)
            {
                if (HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");

                var first = context.Users.First(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]));//Select(usr => usr.Group.DataLevelID)

                if (first != null && first.Group.DataLevelID != null)
                {
                    var resultItems = new List<WorkOrderRequest>();
                    var dataLevel = first.Group.DataLevelID.Value;
                    switch (dataLevel)
                    {
                        case 1:
                            resultItems = context.WorkOrderRequests
                                .Where(wor => wor.RequestID == requestId && wor.RSID == 3).ToList()
                                .ToList();
                            break;
                        case 2:
                            resultItems =
                                context
                                    .WorkOrderRequests
                                    .Where(
                                        wor =>
                                            wor.RequestID == requestId && wor.RSID == 3 &&
                                            DataLevelClass.GetBranchAdminBranchIDs(Convert.ToInt32(HttpContext.Current.Session["User_ID"]))
                                                .Contains(wor.WorkOrder.BranchID)).ToList();
                            break;
                        case 3:
                            resultItems = context.WorkOrderRequests.Where(wor => wor.RequestID == requestId
                                                                                 && wor.RSID == 3
                                                                                 &&
                                                                                 wor.WorkOrder.User.ID ==
                                                                                 Convert.ToInt32(
                                                                                     HttpContext.Current.Session["User_ID"]))
                                .ToList();
                            break;
                    }
                    return resultItems;
                }
                return null;
            }

            void FindRequests(ISPDataContext context2)
            {

                if (string.IsNullOrEmpty(HiddenField1.Value)) return;
                var requ = Requests(11, context2);
                var requests = requ.Where(r =>
                                r.ProcessDate == null &&
                                (r.WorkOrder.ResellerID == -1 || r.WorkOrder.ResellerID == null) &&
                                r.WorkOrder.BranchID == Convert.ToInt32(HiddenField1.Value))
                    .Select(x =>
                        new
                        {
                            Branch = x.WorkOrder.Branch.BranchName,
                            x.ID,
                            x.WorkOrder.CustomerName,
                            x.WorkOrder.CustomerPhone,
                            RequestDate = x.RequestDate != null ? x.RequestDate.Value.ToShortDateString() : " ",
                            Total = Helper.FixNumberFormat(x.Total),
                            x.WorkOrderID,
                            x.WorkOrder.ServiceProvider.SPName,
                            x.WorkOrder.Governorate.GovernorateName,
                            x.WorkOrder.Status.StatusName,
                            x.WorkOrder.ServicePackage.ServicePackageName,
                            Title = x.WorkOrder.Offer != null ? x.WorkOrder.Offer.Title : " ",
                            user = x.WorkOrder.UserName,
                            x.WorkOrder.Password,
                            Start = (x.DemandId != null) ? x.Demand.StartAt.Date.ToString() : "",
                            End = (x.DemandId != null) ? x.Demand.EndAt.Date.ToString() : ""
                        }).ToList();
                gv_customers.DataSource = requests;
                gv_customers.DataBind();
            }


            protected void b_confirm_Click(object sender, EventArgs e)
            {
                string ses = reload.Value;
                if (Session[ses].ToString() == ViewState[ses].ToString())
                {

                    using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var id = Convert.ToInt32(hf_confirm.Value);
                        var boxid = Convert.ToInt32(hf_boxId.Value);
                        var activeUserId = Convert.ToInt32(Session["User_ID"]);
                        var request = context3.WorkOrderRequests.FirstOrDefault(r => r.ID == id);
                        if (request != null && request.WorkOrder.BranchID != null)
                        {
                            var credit = _creditRepository.GetNetCredit(request.WorkOrder.BranchID.Value);
                            if (request.Total != null && credit < request.Total.Value)
                            {
                                Div1.Visible = true;
                                Div1.InnerHtml = Tokens.NotEnoughtCreditMsg;
                                Div1.Attributes.Add("class", "alert alert-danger");
                                return;
                            }
                            var firstOrDefault =
                                context3.BranchPackagesDiscounts.FirstOrDefault(
                                    r =>
                                        r.BranchId == request.WorkOrder.BranchID &&
                                        r.ProviderId == request.WorkOrder.ServiceProviderID &&
                                        r.PackageId == request.NewPackageID);

                            var discount = firstOrDefault != null ? firstOrDefault.Discount : 0;
                            var netdiscount = request.Total*discount/100;
                            var amount = request.Total - netdiscount;

                            var result = _creditRepository.Save(Convert.ToInt32(request.WorkOrder.BranchID.Value),
                                activeUserId, Convert.ToDecimal(amount)*-1,
                                request.WorkOrder.CustomerName + " - " + request.WorkOrder.CustomerPhone,
                                DateTime.Now.AddHours());
                            if (result == SaveResult.NoCredit) return;
                            if (boxid > 0)
                            {
                                var notes = "طلب سداد فرع" + " " + request.WorkOrder.CustomerName + " - " +
                                            request.WorkOrder.CustomerPhone;
                                _boxCreditRepository.SaveBox(boxid, Convert.ToInt32(Session["User_ID"]),
                                    Convert.ToDecimal(txtDiscoundBox.Text)*-1, notes, DateTime.Now.AddHours());
                            }
                            request.Demand.Paid = true;
                            request.Demand.UserId = activeUserId;
                            request.Demand.PaymentDate = DateTime.Now.AddHours();
                            request.Demand.IsRequested = true;
                            request.ConfirmerID = activeUserId;
                            var userTransaction = new UsersTransaction
                            {
                                CreationDate = DateTime.Now.AddHours(),
                                DepitAmmount = 0,
                                CreditAmmount = Convert.ToDouble(request.Total),
                                IsInvoice = false,
                                WorkOrderID = request.WorkOrderID,
                                Total =
                                    Billing.GetLastBalance(Convert.ToInt32(request.WorkOrderID), "WorkOrder") -
                                    Convert.ToDouble(request.Total),
                                Description = "payment",
                                UserId = activeUserId
                            };

                            context3.UsersTransactions.InsertOnSubmit(userTransaction);
                            /*  var order = _context.WorkOrders.FirstOrDefault(w => w.ID == request.WorkOrderID);

                   if(order != null){
                        var ispEntries = new IspEntries();
                        var nextMonthRequestDate = ispEntries.GetNextMonthRequestDate(order, _context);
                        order.RequestDate = Convert.ToDateTime(nextMonthRequestDate);
                        _context.SubmitChanges();
                    }*/

                            //l_message.Text = "";
                            request.RSID = 1;
                            request.ProcessDate = DateTime.Now.AddHours();

                            request.RejectReason = TextBox1.Text;
                            context3.SubmitChanges();
                            FindRequests(context3);
                            var option = context3.Options.FirstOrDefault();
                            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                            {
                                CenterMessage.SendRequestApproval(request.WorkOrder, Tokens.BranchPR, activeUserId);
                            }
                        }
                          //--------------
                             var workOrderRequest2 =
                            context3.WorkOrderRequests.Where(
                                wor =>
                                    wor.WorkOrderID == request.WorkOrderID && wor.RequestID == 2 && wor.RSID == 3 &&
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
                            context3.SubmitChanges();
                        }

                        //---------------------
                        using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                        {
                            var userId = Convert.ToInt32(Session["User_ID"]);
                            if (request != null)
                            {
                                var wid = request.WorkOrderID;
                                var demands = _demandService.CustomerDemands(wid ?? 0);
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
                                                                var option = OptionsService.GetOptions(db8, true);
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
                    Session[ses] =
                    Server.UrlDecode(System.DateTime.Now.ToString());
                }


                else
                {
                    Response.Redirect("BranchPPR.aspx");
                }

                //HttpContext.Current.Response.Redirect(Request.RawUrl);
            }


            protected void gv_customers_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_customers, "gv_lNumber");
                if (ManageRequests) return; //hf_user.Value != "1"){
                var cols = gv_customers.Columns;
                cols[15].Visible = false;
            }


            protected void btn_reject_Click(object sender, EventArgs e)
            {
                using (var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var request = context4.WorkOrderRequests.FirstOrDefault(r => r.ID == Convert.ToInt32(hf_rejectionId.Value));
                    if (request != null)
                    {
                        request.RejectReason = txt_RejectReason.Text;
                        request.RSID = 2;
                        // request.RejectReason = txt_RejectReason.Text;
                        request.ConfirmerID = userId;
                        request.ProcessDate = DateTime.Now.AddHours();
                        var demand = request.Demand;
                        demand.IsRequested = false;
                    }

                    context4.SubmitChanges();
                    FindRequests(context4);

                    l_message.Text = Tokens.Rejected;
                    Div1.Visible = true;
                    Div1.InnerHtml = Tokens.Rejected;
                    Div1.Attributes.Add("class", "alert alert-danger");
                    if (request != null)
                    {
                        var option = context4.Options.FirstOrDefault();
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestReject(request.WorkOrder, txt_RejectReason.Text, Tokens.BranchPR, userId);
                        }
                    }
                    txt_RejectReason.Text = string.Empty;
                }
            }


            protected void Export_OnClick(object sender, EventArgs e)
            {
                //BranchPrintExcel = true;
                string attachment = string.Format("attachment; filename={0}.xls",
                    "BranchRequestedPayment" + "@__"
                    + DateTime.Now.AddHours().Date.ToShortDateString());
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";

                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                gv_customers.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }



            public override void VerifyRenderingInServerForm(Control control) { }
        }
    }
 