using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class ResellerPPR : CustomPage
    {
        private readonly DemandService _demandService;
    readonly IResellerCreditRepository _creditRepository;

    readonly BranchCreditRepository _branchCreditRepository;

    readonly IBoxCreditRepository _boxCreditRepository;
    public bool ManageRequests { get; set; }
    private readonly IspEntries _ispEntries;
    public ResellerPPR(){
        _creditRepository = new ResellerCreditRepository();
        _branchCreditRepository=new BranchCreditRepository();
        _boxCreditRepository=new BoxCreditRepository();
        _ispEntries=new IspEntries();
        _demandService = new DemandService(IspDataContext);
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


    protected void Page_Load(object sender, EventArgs e){
      //for no resending data when refresh page  
        if (!IsPostBack)
        {
            reload.Value = RandomString();
            string ses = reload.Value;
            Session[ses] =
            Server.UrlDecode(System.DateTime.Now.ToString());

        }

        mv_container.SetActiveView(v_search);
        BuildUponUserType();
        PopulateBoxes();
            var userId = Convert.ToInt32(Session["User_ID"]);
            ManageRequests = _ispEntries.UserHasPrivlidge(userId, "ManageResellerRequest");
            Div1.Visible = false;
            Div1.InnerHtml = string.Empty;
            
           
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        string ses = reload.Value;
        ViewState[ses] = Session[ses];
    }


    void PopulateBoxes(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            ddlBox.DataSource = context.Boxes.Where(a => a.ShowBoxInResellerPPR == true);
            ddlBox.DataTextField = "BoxName";
            ddlBox.DataValueField = "ID";
            ddlBox.DataBind();
            Helper.AddDefaultItem(ddlBox);
        }
    }


    protected void b_addRequest_Click(object sender, EventArgs e){
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            FindRequests(context);
        }
    }

    private List<WorkOrderRequest> Requests(int requestId,ISPDataContext context)
    {
        if(HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");

        var first = context.Users.First(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]));//Select(usr => usr.Group.DataLevelID)

        if(first != null && first.Group.DataLevelID!=null)
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
    

    void FindRequests(ISPDataContext context){
            if (string.IsNullOrEmpty(HiddenField1.Value)) return;
        var req = Requests(11, context);
        var requests = req.Where(r =>
            r.ProcessDate == null &&
            r.WorkOrder.ResellerID == Convert.ToInt32(HiddenField1.Value))
            .Select(x => new
            {
                x.ID,
                x.WorkOrder.CustomerName,
                x.WorkOrder.CustomerPhone,
                x.RequestDate,
                x.Total,
                x.WorkOrderID,
                x.WorkOrder.ServiceProvider.SPName,
                x.WorkOrder.Governorate.GovernorateName,
                x.WorkOrder.Status.StatusName,
                x.WorkOrder.ServicePackage.ServicePackageName,
                x.WorkOrder.User.UserName,
                x.WorkOrder.Branch.BranchName,
                Title=x.WorkOrder.Offer!=null?x.WorkOrder.Offer.Title:" ",
                user = x.WorkOrder.UserName,
                x.WorkOrder.Password,
                Start = (x.DemandId != null) ? x.Demand.StartAt.Date.ToString() : "",
                End = (x.DemandId != null) ? x.Demand.EndAt.Date.ToString() : ""
            }).ToList();
            gv_customers.DataSource = requests;
            gv_customers.DataBind();
            mv_container.SetActiveView(v_results);
    }


    void BuildUponUserType(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var id = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(u => u.ID == id);
            if(user == null) return;

            List<User> resellers;
            switch(user.Group.DataLevelID){
                case 1 : // admin
                    resellers = GetResellers(null,context);
                    break;
                case 3 : // reseller
                    resellers = context.Users.Where(u => u.ID == id).ToList();
                 
                    break;
                default :
                    resellers = GetResellers(id,context);
                    break;
            }
            ddl_reseller.DataSource = resellers;
            ddl_reseller.DataValueField = "ID";
            ddl_reseller.DataTextField = "UserName";
            ddl_reseller.DataBind();
            Helper.AddDefaultItem(ddl_reseller);
        }
    }


    List<User> GetResellers(int ? id,ISPDataContext context){
       
            if(id != null){
                var user = context.Users.FirstOrDefault(x => x.ID == id);
                if(user == null){
                    Response.Redirect("default.aspx");
                    return null;
                }
                //filter by Branch Id of Current User
                List<int?> userBranchs = DataLevelClass.GetBranchAdminBranchIDs(user.ID);
                var resellers = context.Users.Where(g => g.GroupID == 6 &&  userBranchs.Contains(g.BranchID)).ToList();//g.BranchID ==user.BranchID).ToList();

                return resellerhasRequest(resellers,context);
            } else{
                var allresellers = context.Users.Where(x => x.GroupID == 6).ToList();
                var reseller = new List<User>();
                foreach(var res in allresellers){
                    var check = reseller.Where(s => s.ID == res.ID).ToList();
                    if(check.Count == 0){
                        var founded = context.WorkOrderRequests.Where(x => x.WorkOrder.ResellerID == res.ID && x.RequestID == 11 && x.RSID == 3).ToList();
                        if(founded.Count != 0){
                            reseller.Add(res);

                        }
                    }
                }
                return resellerhasRequest(reseller,context);
            }
          
    }


    List<User> resellerhasRequest(List<User> reseller,ISPDataContext context){
       
            var resellerHasRequest = new List<User>();
            foreach(var item in reseller){
                var check = context.WorkOrderRequests.FirstOrDefault(ch => ch.WorkOrder.ResellerID == item.ID && ch.RSID == 3 && ch.RequestID == 11);
                if(check != null){
                    resellerHasRequest.Add(item);
                }
            }
            return resellerHasRequest;
       
    }


    protected void gv_customers_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_customers, "gv_lNumber");

        if(!ManageRequests){
            var cols = gv_customers.Columns;
            cols[16].Visible = false;
            b_changeReseller.Visible = false;
        }
    }

    protected void gv_bConfirm_Click(object sender, EventArgs e){

        string ses = reload.Value;
        if (Session[ses].ToString() == ViewState[ses].ToString())
        {

        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var id = Convert.ToInt32(hf_ApprovedId.Value);
            var boxid = Convert.ToInt32(hf_boxId.Value);
            var userId = Convert.ToInt32(Session["User_ID"]);
           
            var request = context.WorkOrderRequests.FirstOrDefault(r => r.ID == id);
            if(request != null){
                var resellerCredit = Convert.ToDouble(_creditRepository.GetNetCredit(Convert.ToInt32(request.WorkOrder.ResellerID)));
                if(resellerCredit >= Convert.ToDouble(request.Total)){
                    request.Demand.Paid = true;
                    request.Demand.IsRequested = true;
                    request.Demand.PaymentDate = DateTime.Now.AddHours();
                    request.Demand.UserId = userId;
                    request.RSID = 1;
                    request.RejectReason = txtApprovComment.Text;
                    request.ProcessDate = DateTime.Now.AddHours();
                    request.ConfirmerID = userId;
                    var amount = request.Total;
                    var branchamount = request.Total;
                    //context.SubmitChanges();
                    var curdem1 = context.Demands.FirstOrDefault(x => x.Id == request.DemandId);
                    // deleted condition from IF
                    /*&& curdem1.IsResellerCommisstions != null && curdem1.IsResellerCommisstions != false*/
                    if (curdem1 != null)
                    {
                        var firstOrDefault =
                            context.ResellerPackagesDiscounts.FirstOrDefault(
                                r =>
                                    r.ResellerId == request.WorkOrder.ResellerID &&
                                    r.ProviderId == request.WorkOrder.ServiceProviderID &&
                                    r.PackageId == request.WorkOrder.ServicePackageID);

                        var discount = firstOrDefault != null ? firstOrDefault.Discount : 0;
                        //added by ashraf to get net discount
                        var netdscount = request.Total * discount / 100;
                        amount = request.Total - netdscount;
                    }


                    if(context.Options.First().DiscoundfromResellerandBranch){
                        var branchCredit = Convert.ToDouble(_branchCreditRepository.GetNetCredit(Convert.ToInt32(request.WorkOrder.User.BranchID)));
                        if (branchCredit >= Convert.ToDouble(request.Total))
                        {
                            var curdem = context.Demands.FirstOrDefault(x => x.Id == request.DemandId);
                            if (curdem != null && curdem.IsResellerCommisstions!=null && curdem.IsResellerCommisstions!=false)
                            {
                                var branchdiscound =
                               context.BranchPackagesDiscounts.FirstOrDefault(
                                   r =>
                                       r.BranchId == request.WorkOrder.BranchID &&
                                       r.ProviderId == request.WorkOrder.ServiceProviderID &&
                                       r.PackageId == request.WorkOrder.ServicePackageID);
                                var dis = branchdiscound != null ? branchdiscound.Discount : 0;
                                var netdiscount = request.Total * dis / 100;
                                 branchamount = request.Total - netdiscount;

                               
                            }

                            _branchCreditRepository.Save(Convert.ToInt32(request.WorkOrder.BranchID), userId,
                                   Convert.ToDecimal(branchamount) * -1,
                                   request.WorkOrder.CustomerName + " - " + request.WorkOrder.CustomerPhone,
                                   DateTime.Now.AddHours());
                           
                        }
                        else
                        {
                            l_message.Text = Tokens.NotEnoughtCreditMsg;
                            Div1.Visible = true;
                            Div1.InnerHtml = Tokens.NotEnoughtCreditMsg;
                            Div1.Attributes.Add("class", "alert alert-danger");
                            return;

                        }

                    }

                    var result = _creditRepository.Save(Convert.ToInt32(request.WorkOrder.ResellerID), userId, Convert.ToDecimal(amount * -1), request.WorkOrder.CustomerName + " - " + request.WorkOrder.CustomerPhone, DateTime.Now.AddHours());
                    switch(result){
                        case SaveResult.Saved :
                            if(boxid > 0)
                            {
                                var notes = "طلب سداد موزع" + " " +request.WorkOrder.CustomerName + " - " +
                                            request.WorkOrder.CustomerPhone;
                                _boxCreditRepository.SaveBox(boxid, userId, Convert.ToDecimal(txtDiscoundBox.Text) * -1, notes, DateTime.Now.AddHours());
                            }
             /*               var userTransaction = new UsersTransaction{
                                CreationDate = DateTime.Now.AddHours(),
                                DepitAmmount = 0,
                                CreditAmmount = Convert.ToDouble(request.Total),
                                IsInvoice = false,
                                WorkOrderID = request.WorkOrderID,
                                Total = Billing.GetLastBalance(Convert.ToInt32(request.WorkOrderID), "WorkOrder") - Convert.ToDouble(request.Total),
                                Description = "payment"
                            };
                            context.UsersTransactions.InsertOnSubmit(userTransaction);*/
                            context.SubmitChanges();
                            l_message.Text = Tokens.Saved;


                            var option = OptionsService.GetOptions(context, true);
                            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                            {
                                var gov =
                                    context.Governorates.FirstOrDefault(
                                        a => a.ID == request.WorkOrder.CustomerGovernorateID);
                                CenterMessage.SendRequestApproval(request.WorkOrder.CustomerName,
                                    request.WorkOrder.CustomerPhone, gov != null ? gov.GovernorateName : " ",
                                    Convert.ToInt32(request.WorkOrder.ResellerID), Tokens.ResellerPR, userId);
                            }

                            //--------------
                             var workOrderRequest2 =
                            context.WorkOrderRequests.Where(
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
                                context.SubmitChanges();
                            }
                            //---------------------
                            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                            {
                                if (request != null)
                                {

                                  
                                    
                                  
                                    var wid = request.WorkOrderID;
                                    var demands = _demandService.CustomerDemands(wid ?? 0);
                                    var unpaid = demands.Where(x => !x.Paid).OrderBy(a => a.Id).ToList();
                                    var order = db8.WorkOrders.FirstOrDefault(x=>x.ID==wid);


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
                                                                Div1.Visible = true;
                                                                Div1.InnerHtml = "هذا العميل مفعل بالفعل على البورتال";
                                                                Div1.Attributes.Add("class", "alert alert-danger");
                                                                portalIsStoped = true;
                                                            }
                                                            else
                                                            {
                                                                var worNote = Tedata.SendTedataUnSuspendRequest(username, cookiecon,
                                                                    pagetext);
                                                                if (worNote == 2)
                                                                {
                                                                    Div1.Visible = true;
                                                                    Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                                                    Div1.Attributes.Add("class", "alert alert-danger");
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
                                                                        //_ispEntries.Commit();
                                                                    }

                                                                    Div1.Visible = true;
                                                                    Div1.InnerHtml = "تم إرسال طلب التشغيل الى البورتال بنجاح";
                                                                    Div1.Attributes.Add("class", "alert alert-success");
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            portalIsStoped = true;
                                                            Div1.Visible = true;
                                                            Div1.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
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
                                                    Div1.InnerHtml = "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
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
                                                        SenderID = userId,
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
                        case SaveResult.NoCredit :
                            l_message.Text = Tokens.NotEnoughtCreditMsg;
                            return;
                            break;
                    }
                    FindRequests(context);
                   
                }
            }

           
        }
        Session[ses] =
Server.UrlDecode(System.DateTime.Now.ToString());
        }
        else
        {
            Response.Redirect("ResellerPPR.aspx");
        }



    }


    protected void b_changeReseller_Click(object sender, EventArgs e){
        mv_container.SetActiveView(v_search);
        BuildUponUserType();
        gv_customers.DataSource = null;
        gv_customers.DataBind();
    }


    protected void btn_reject_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var userId = Convert.ToInt32(Session["User_ID"]);
            var request = context.WorkOrderRequests.FirstOrDefault(r => r.ID == Convert.ToInt32(hf_rejectionId.Value));
            if(request != null){
                request.RejectReason = txt_RejectReason.Text;
                request.RSID = 2;
                request.ConfirmerID = userId;
                request.ProcessDate = DateTime.Now.AddHours();
            }
            var demand = context.Demands.FirstOrDefault(d => d.WorkOrderId == request.WorkOrderID && d.Id == request.DemandId);
            if(demand != null){
                demand.IsRequested = false;
            }

            context.SubmitChanges();
            FindRequests(context);
            mv_container.SetActiveView(v_results);
            
            l_message.Text = Tokens.Rejected;
            if (request != null)
            {
                var option = OptionsService.GetOptions(context, true);
                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                {
                    var gov = context.Governorates.FirstOrDefault(a => a.ID == request.WorkOrder.CustomerGovernorateID);
                    CenterMessage.SendRequestReject(request.WorkOrder.CustomerName, request.WorkOrder.CustomerPhone,
                        gov != null ? gov.GovernorateName : " ", Convert.ToInt32(request.WorkOrder.ResellerID),
                        Tokens.ResellerPR, txt_RejectReason.Text, userId);
                }
            }
            txt_RejectReason.Text = string.Empty;
        }
    }


    
}
}