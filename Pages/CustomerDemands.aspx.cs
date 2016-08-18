using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.OfferServices;
using NewIspNL.Services.Requests;
using Resources;
using NewIspNL;

namespace NewIspNL.Pages
{
    public partial class CustomerDemands : CustomPage
    {

        #region repos

        // readonly ISPDataContext _context;

        private readonly DemandService _demandService;

        private readonly IspEntries _ispEntries;

        private readonly RequestsService _requestsService;

        private readonly SearchEngine _searchEngine;

        private readonly IspDomian _domian;
        private readonly IRequestNotifiy _requestNotifiy;

        private readonly IBoxCreditRepository _boxCreditRepository;
        public Option WidthOption { get; set; }
        //readonly DemandFactory _demandFactory;
        private readonly BranchCreditRepository _branchcreditRepository; //= new BranchCreditRepository();
        private readonly IUserSaveRepository _userSave;
        private readonly PriceServices _priceServices;
        private IWorkOrderCredit _orderCredit;

        public CustomerDemands()
        {
            var context = IspDataContext;
            _ispEntries = new IspEntries(context);
            _demandService = new DemandService(context);
            _searchEngine = new SearchEngine(context);
            _domian = new IspDomian(context);
            _requestsService = new RequestsService(context);
            DetailsUrl = "CustomerDetails.aspx?c=";
            CanDelete = CanEdit = false;
            _boxCreditRepository = new BoxCreditRepository();
            WidthOption = context.Options.Any() ? context.Options.FirstOrDefault() : new Option();
            _userSave = new UserSaveRepository();
            //_demandFactory=new DemandFactory(_ispEntries);
            _branchcreditRepository = new BranchCreditRepository();
            _requestNotifiy = new RequestNotifiy();
            _priceServices = new PriceServices();
            _orderCredit = new Domain.Abstract.WorkOrderCredit();
        }

        #endregion

        #region Privs


        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanUnpay { get; set; }

        public bool CanPay { get; set; }

        public bool CanAdd { get; set; }

        public bool CanAddNext { get; set; }
        public bool CanSuspend { get; set; }


        public bool CanEditPendingRequestDate { get; set; }

        public string DetailsUrl { get; set; }

        #endregion

        public void Direct(object sender, EventArgs eventArgs)
        {

            var stringId = HfCustomerId.Value;
            var id = QueryStringSecurity.Encrypt(stringId);
            Response.Redirect("CustomerDetails.aspx?WOID=" + stringId);


        }

        public bool CanActive { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
//for no resending data when refresh page
            if (!IsPostBack)
            { 
              
                Session["CheckRefresh3"] =
                    Server.UrlDecode(System.DateTime.Now.ToString());
               
            }
           
            history.Visible = false;
            BNextD2.Visible = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                if (Session["User_ID"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                HandlePrivildges();
               
                if (IsPostBack)
                {

                    //var wor = GetWorkOrder(context);

                    //if (wor != null && wor.WorkOrderStatusID == 11)
                    //{
                       
                    //    Suspsend.Visible = false;
                    //    handleCanSuspend.Visible = false;
                    //    if (CanActive) handelcanActive.Visible = true;
                    //}
                    //else if (wor != null && wor.WorkOrderStatusID == 6)
                    //{
                    //    btnUpDown.Visible = true;
                    //    btnChangeIPPackage.Visible = true;
                      
                    //    if (CanSuspend) handleCanSuspend.Visible = true;
                    //    handelcanActive.Visible = false;

                    //}
                }
                if (IsPostBack) return;
                //TbUpDwonDate.Text = DateTime.Now.AddHours().ToShortDateString();
                //TbMiscDate.Text = DateTime.Now.AddHours().ToShortDateString();
                Bind_ddl_IpPackage(context);
                TbUnsuspendDate.Text = DateTime.Now.AddHours().ToShortDateString();
                var option = OptionsService.GetOptions(context, true);
                if (option == null || option.IncludeGovernorateInSearch == false) GovBox.Visible = false;
                else
                {
                    _domian.PopulateGovernorates(DdGovernorates);
                    GovBox.Visible = true;
                }
                if (option != null && Convert.ToBoolean(option.ShowDeductionWithFixedRequestDateInCD))
                {
                    DeductionWithFixedRequestDate.Visible = true;
                }
                else
                {
                    DeductionWithFixedRequestDate.Visible = false;
                }
                suspendMsg.Visible = false;
                Helper.Populate1To12(DdlMonths);
                handleCanSuspend.Visible = false;
                handelcanActive.Visible = false;
                btnUpDown.Visible = false;
                btnChangeIPPackage.Visible = false;
                PopulateBoxes();
                PopulateSvaes();
                PopulateUnsus();




                _ispEntries.PopulateExtraGigas(DdlExtraGigas);
                Helper.AddDefaultItem(DdlExtraGigas);
                /*var user = context.Users.FirstOrDefault(a => a.ID == Convert.ToInt32(Session["User_ID"]));
                    var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
                    if (cnfg != null)
                    {
                        imgSite.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
                    }*/
                //Request From Customer Demand
                if (!string.IsNullOrWhiteSpace(Request.QueryString["c"]) &&
                    !string.IsNullOrWhiteSpace(Request.QueryString["g"]))
                {

                    var phone = QueryStringSecurity.Decrypt(Request.QueryString["c"]);
                    var gov = QueryStringSecurity.Decrypt(Request.QueryString["g"]);
                    if (option != null && option.IncludeGovernorateInSearch)
                    {
                        DdGovernorates.SelectedValue = gov;
                    }
                    TPhone.Text = phone;
                    SearchDemands();
                }





            }
            if (!IsPostBack)
            {
                HandleShowAndHideElements();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh3"] = Session["CheckRefresh3"];
           
        }

        private void Bind_ddl_ServicePackage(WorkOrder orderId, ISPDataContext db4)
        {
            var packages = orderId.OfferId == null
                ? db4.ServicePackages.Where(x => x.ProviderId == orderId.ServiceProviderID.Value).Select(sp => sp)
                : db4.OfferProviderPackages.Where(a => a.OfferId == orderId.OfferId).Select(sp => sp.ServicePackage);
            packages = packages.Where(x => x.Active == true && x.ID != orderId.ServicePackageID);
            ddl_ServicePackage.DataSource = packages;
            ddl_ServicePackage.DataValueField = "ID";
            ddl_ServicePackage.DataTextField = "ServicePackageName";
            ddl_ServicePackage.DataBind();
            Helper.AddDefaultItem(ddl_ServicePackage);
        }

        private void PopulateSvaes()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                var userId = Convert.ToInt32(Session["User_ID"]); //ddlSaves.DataSource =
                ddlPrePay.DataSource = ddlSavesCancelPay.DataSource =
                    ddlSavesPay.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
                    {
                        a.Save.SaveName,
                        a.Save.Id
                    });
                //ddlSaves.DataBind();
                ddlSavesPay.DataBind();
                ddlPrePay.DataBind();
                ddlSavesCancelPay.DataBind();
                //Helper.AddDefaultItem(ddlSaves);
                Helper.AddDefaultItem(ddlSavesPay);
                Helper.AddDefaultItem(ddlSavesCancelPay);
                Helper.AddDefaultItem(ddlPrePay);
            }
        }

        private void PopulateUnsus()
        {
            //List<string> add = new List<string> {"تشغيل 24 ساعة", "تشغيل 48 ساعة", "تشغيل 72 ساعة"};
            //unsusduration.DataSource = add;

            unsusduration.Items.Insert(0, new ListItem("تشغيل 24 ساعة", "1"));

            unsusduration.Items.Insert(1, new ListItem("تشغيل 48 ساعة", "2"));

            unsusduration.Items.Insert(2, new ListItem("تشغيل 72 ساعة", "3"));


            unsusduration.DataBind();
            Helper.AddDefaultItem(unsusduration);
        }

        private void PopulateBoxes()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ddlbox.DataSource = context.Boxes.Where(a => a.ShowInCustomerDemands == true);
                ddlbox.DataTextField = "BoxName";
                ddlbox.DataValueField = "ID";
                ddlbox.DataBind();
                Helper.AddDefaultItem(ddlbox);
            }
        }


        private void HandlePrivildges()
        {
            if (Session["User_ID"] == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            //------
            var user = _ispEntries.GetUser(userId);
            if (user == null)
            {
                return ;
            }
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var groupId = user.GroupID.Value;
                var groupPrivileges = context.GroupPrivileges.Where(gp => gp.Group.ID == groupId).ToList();
                CanEdit = _ispEntries.UserHasPrivlidge(groupPrivileges, "EditDemand");
                CanDelete = _ispEntries.UserHasPrivlidge(groupPrivileges, "DeleteDemand");
                CanUnpay = _ispEntries.UserHasPrivlidge(groupPrivileges, "UnpayDemand");
                CanPay = _ispEntries.UserHasPrivlidge(groupPrivileges, "PayDemand");
                CanAdd = _ispEntries.UserHasPrivlidge(groupPrivileges, "AddDemand");
                CanAddNext = false;
                CanSuspend = _ispEntries.UserHasPrivlidge(groupPrivileges, "CanSuspendOnDemandPage");
                addDemand.Visible = CanAdd;
                CanActive = _ispEntries.UserHasPrivlidge(groupPrivileges, "ActiveCustomerDemand");
                CanEditPendingRequestDate = _ispEntries.UserHasPrivlidge(groupPrivileges, "EditPendingRequestDate");
            }
            //---


            //CanEdit = _ispEntries.UserHasPrivlidge(userId, "EditDemand");
            //CanDelete = _ispEntries.UserHasPrivlidge(userId, "DeleteDemand");
            //CanUnpay = _ispEntries.UserHasPrivlidge(userId, "UnpayDemand");
            //CanPay = _ispEntries.UserHasPrivlidge(userId, "PayDemand");
            //CanAdd = _ispEntries.UserHasPrivlidge(userId, "AddDemand");
            //CanAddNext = false;
            //CanSuspend = _ispEntries.UserHasPrivlidge(userId, "CanSuspendOnDemandPage");
            //addDemand.Visible = CanAdd;
            //CanActive = _ispEntries.UserHasPrivlidge(userId, "ActiveCustomerDemand");
        }


        /*void Activate()
            {
                GvNotes.DataBound += (o, e) => Helper.GridViewNumbering(GvNotes, "LNo");
                grd_Tickets.DataBound += (o, e) => PopulateTicketDays();
            }*/


        protected void BSearch_OnServerClick(object sender, EventArgs e)
        {
           
            ResetSearch();
            //HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        private void ResetSearch()
        {
            Reset();
            SearchDemands();

        }


        private void SearchDemands()
        {
            using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var order = GetWorkOrder(context2);
                if (order == null)
                {
                    Reset();
                    Msg.Visible = false;
                    Msg2.Visible = true;
                    Msg2.InnerHtml = Tokens.CustomerNotFound;
                    HfCustomerId.Value = "0";
                    GvCustomerData.DataSource = null;
                    GvCustomerData.DataBind();
                    return;
                }

                fill_payhisGV(order.ID);


                woid.Value = order.ID.ToString();
                if (order.WorkOrderStatusID != null && order.WorkOrderStatusID.Value == 11)
                {
                    int daysCount = _ispEntries.DaysForCustomerAtStatus(order.ID, 11);
                    hdfDaysOfSuspend.Value = daysCount.ToString(CultureInfo.InvariantCulture);
                    suspendMsg.Visible = true;
                    suspendMsg.InnerHtml = string.Format("{0} : ( {1} ) , {2}", Tokens.SuspendDaysCount, daysCount,
                        Tokens.CustomerShouldConvertToActive);
                    handleCanSuspend.Visible = false;
                    handelcanActive.Visible = CanActive;
                }
                else
                {
                    suspendMsg.Visible = false;
                    suspendMsg.InnerHtml = string.Empty;
                    handleCanSuspend.Visible = CanSuspend;
                    handelcanActive.Visible = false;
                }

                if (order != null && order.WorkOrderStatusID == 11)
                {
                    //if (CanActive)  handelcanActive.Visible = true;
                    Suspsend.Visible = false;
                    handleCanSuspend.Visible = false;
                    if (CanActive) handelcanActive.Visible = true;
                }
                else if (order != null && order.WorkOrderStatusID == 6)
                {
                    btnUpDown.Visible = true;
                    btnChangeIPPackage.Visible = true;
                    //Suspsend.Visible = true;
                    if (CanSuspend) handleCanSuspend.Visible = true;
                    handelcanActive.Visible = false;

                }

                ExtractOrderData(order);
                GetDemands(order, context2);
                var fakeOrderList = new List<WorkOrder>
                {
                    order
                };

                Bind_ddl_ServicePackage(order, context2);
                //TbUnsuspendDate.Visible = CanEditPendingRequestDate;
                UnsesuspendContainer.Visible = CanEditPendingRequestDate;
                GvCustomerData.DataSource = fakeOrderList.Select(x => _searchEngine.ToCustomerResult(x));
                GvCustomerData.DataBind();

            }
        }


        private void ExtractOrderData(WorkOrder order)
        {
            HFcheck.Value = order.ID.ToString();
            HfCustomerId.Value = string.Format("{0}", QueryStringSecurity.Encrypt(order.ID.ToString()));
            SCustomer.InnerHtml = order.CustomerName;
            SOffer.InnerHtml = order.Offer == null ? "-" : order.Offer.Title;
            SPhone.InnerHtml = order.CustomerPhone;
        }


        private WorkOrder GetWorkOrder(ISPDataContext context1)
        {
            var userOrders = DataLevelClass.GetUserWorkOrder(Convert.ToInt32(Session["User_ID"]), context1);
            var option = OptionsService.GetOptions(context1, true);
            WorkOrder order;
            if (option != null && option.IncludeGovernorateInSearch)
                order = DdGovernorates.SelectedIndex > 0
                    ? userOrders.FirstOrDefault(
                        wo =>
                            wo.CustomerPhone == TPhone.Text.Trim() &&
                            wo.CustomerGovernorateID == Convert.ToInt32(DdGovernorates.SelectedItem.Value))
                    : null;
            else order = userOrders.FirstOrDefault(wo => wo.CustomerPhone == TPhone.Text.Trim());

            return order;

        }


        private void GetDemands(WorkOrder order, ISPDataContext context)
        {
            var demands = _demandService.CustomerDemands(order.ID);

            var unpaid = demands.Where(x => !x.Paid).OrderBy(a => a.Id).ToList().Select(Transformer.DemandToGridPreview);
            var paid =
                demands.Where(x => x.Paid).OrderByDescending(a => a.Id).ToList().Select(Transformer.DemandToGridPreview);
            var userId = Convert.ToInt32(Session["User_ID"]);
            if (order.WorkOrderStatusID == 6 && _ispEntries.UserHasPrivlidge(userId, "AddNextDemand"))
                CanAddNext = BNextD2.Visible = true;
            //  CanAddNext = BNextD2.Visible = _ispEntries.UserHasPrivlidge(userId, "AddNextDemand");
            if (unpaid.Any()) CanAddNext = false;

            GvUnpaid.DataSource = unpaid;
            GvUnpaid.DataBind();
            divCredits.Visible = true;
            var orderCredit = _orderCredit.GetLastCredit(order.ID, context);
            lblTotalUnpaid.InnerHtml = string.Format("{0}",
                orderCredit > 0
                    ? Helper.FixNumberFormat(demands.Where(a => !a.Paid).Sum(a => a.Amount) - orderCredit)
                    : Helper.FixNumberFormat(demands.Where(a => !a.Paid).Sum(a => a.Amount) + orderCredit));
            lblCredit.InnerHtml = string.Format("{0}", Helper.FixNumberFormat(orderCredit));
            hdnCredit.Value = Helper.FixNumberFormat(orderCredit);
            //if(unpaid.Count()!=0)TbAmountincomment.Value = GvUnpaid.Rows[0].Cells[9].Text;
            GvPaid.DataSource = paid;
            GvPaid.DataBind();
        }


        private void Reset()
        {
            foreach (var control in Mother.Controls)
            {
                var grd = control as GridView;
                if (grd != null)
                {
                    grd.DataSource = null;
                    grd.DataBind();
                }
                Msg2.Visible = Msg2.Visible = false;
                Msg.InnerHtml = Msg2.InnerHtml = string.Empty;
            }
        }



        protected void GvUnpaid_OnDataBound(object sender, EventArgs e)
        {

            Helper.HideShowControl(GvUnpaid, "EditBtn", CanEdit);
            Helper.HideShowControl(GvUnpaid, "DeleteBtn", CanDelete);
            Helper.GridViewNumbering(GvUnpaid, "LNo");
            if (GvUnpaid.Rows.Count <= 0) return;
            foreach (GridViewRow row in GvUnpaid.Rows)
            {
                var button = row.FindControl("BPay") as LinkButton;
                    //GvUnpaid.Rows[0].FindControl("BPay") as LinkButton;
                if (button == null) return;
                button.Visible = CanPay;
            }
        }


        protected void GvPaid_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvPaid, "LNo");
            Helper.GridViewNumbering(GvUnpaid, "LNo");
            Helper.HideShowControl(GvPaid, "Unpay", CanUnpay);
        }

        protected void payhis_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(payhis, "LNo");

        }


        public void fill_payhisGV(int id)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var order = id;

                var his = context.WorkOrderCredits.Where(z => z.WorkOrderId == order).Select(z => new
                {
                    z.Id,
                    z.CreditAmount,
                    z.User.UserName,
                    z.Time,
                    z.Notes,



                }).ToList();
                if (his.Count > 0)
                {
                    history.Visible = true;
                    //BNextD2.Visible = true;
                }
                payhis.DataSource = his;
                payhis.DataBind();
            }

        }




      


        protected void SaveDemand(object sender, EventArgs e)
        {
            //using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                var orderId = Convert.ToInt32(que);
                var order = _ispEntries.GetWorkOrder(orderId);
                if (order == null) return;

                var demandFactory = new DemandFactory(_ispEntries);
                var demand = demandFactory
                    .CreateDemand(order,
                        Convert.ToDateTime(TbFrom.Text),
                        Convert.ToDateTime(TbTo.Text),
                        Convert.ToDecimal(TbAmount.Text),
                        Convert.ToInt32(Session["User_ID"]),
                        DateTime.Now.AddHours(),
                        false,
                        TbNotes.Text
                    );
                demand.IsResellerCommisstions = cbCommission.Checked;
                _ispEntries.AddDemands(demand);
                _ispEntries.Commit();
                
                SearchDemands();
                Msg2.Visible = false;
                Msg.Visible = true;
                Msg.InnerHtml = Tokens.Saved;
                TbNotes.Text =
                    SPhone.InnerHtml =
                        SCustomer.InnerHtml = SOffer.InnerHtml = TbFrom.Text = TbTo.Text = TbAmount.Text = string.Empty;
               
            }
        }


        protected void PayDemand(object sender, EventArgs e)
        {

            if (Session["CheckRefresh3"].ToString() == ViewState["CheckRefresh3"].ToString())
            {

                 var userId = Convert.ToInt32(Session["User_ID"]);
                            var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                            var orderId = Convert.ToInt32(que);
                            var order = _ispEntries.GetWorkOrder(orderId);
                using (var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    try
                    {
                        var demandId = Convert.ToInt32(HfDemandId.Value);
                        if (Session["User_ID"] == null) return;
                        if (demandId == 0) return;
                        // check branch credit
                        var optiondis = context4.Options.FirstOrDefault();
                        if (optiondis == null) return;
                        var discoundoption = optiondis.DiscoundFromBranchCredit;
                        if (discoundoption)
                        {
                            if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                            //var userId = Convert.ToInt32(Session["User_ID"]);
                            //var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                            //var orderId = Convert.ToInt32(que);
                            //var order = _ispEntries.GetWorkOrder(orderId);

                            var branchcredit =
                                context4.BranchCredits.Where(x => x.BranchId == order.BranchID)
                                    .OrderByDescending(x => x.Id)
                                    .FirstOrDefault();
                            if (branchcredit != null && order != null)
                            {
                                var lastCredit = branchcredit.Net;
                                var demand = _ispEntries.GetDemand(demandId);
                                var barnchid = Convert.ToInt16(order.BranchID);
                                if (demand.Amount <= lastCredit)
                                {
                                    var amount = Convert.ToDecimal(TbAmountincomment.Value)*-1; //demand.Amount * -1;
                                    _branchcreditRepository.Save(barnchid, userId, amount, TbComment.Text,
                                        DateTime.Now.AddHours());

                                    PayDemand(demandId);
                                    Msg.Visible = true;
                                    Msg2.Visible = false;
                                    Msg.InnerHtml += Tokens.Saved;

                                }
                                else
                                {
                                    Msg.Visible = false;
                                    Msg2.Visible = true;
                                    Msg2.InnerHtml = Tokens.NotEnoughtCrefit;
                                }
                            }
                        }
                        else
                        {
                            PayDemand(demandId);

                        }
                        ddlbox.SelectedIndex = -1;
                    }
                    catch (Exception)
                    {
                        Msg.Visible = false;
                        Msg2.Visible = true;
                        Msg2.InnerHtml = Tokens.Error;
                    }
                   

                    //HttpContext.Current.Response.Redirect(Request.RawUrl);
                }
                Session["CheckRefresh3"] =
                    Server.UrlDecode(System.DateTime.Now.ToString());
            }
            else
            {
                Response.Redirect("CustomerDemands.aspx");
            }
        }

        private bool CreateRquest(int demandId, int branchId, ISPDataContext context)
        {

            var requests = new List<WorkOrderRequest>();
            var demand = context.Demands.FirstOrDefault(d => d.Id == demandId);
            if (demand == null) return false;
            //chcek if this request already exists in WorkOrderRequest
            var check =
                context.WorkOrderRequests.Where(a => a.RequestID == 11 && a.DemandId == demand.Id && a.RSID == 3);
            //&& a.DemandId == demandId
            if (check.Any()) return false;
            if (Session["User_ID"] == null) return false;
            var requestOrder = new WorkOrderRequest
            {
                WorkOrder = demand.WorkOrder,
                RequestID = 11,
                RequestDate = DateTime.Now.AddHours(),
                RSID = 3,
                SenderID = Convert.ToInt32(Session["User_ID"]),
                CurrentPackageID = demand.WorkOrder.ServicePackageID,
                NewPackageID = demand.WorkOrder.ServicePackageID,
                Total = demand.Amount,
                ExtraGigaId = demand.WorkOrder.ExtraGigaId,
                NewIpPackageID = demand.WorkOrder.IpPackageID,
                DemandId = demandId
            };
            requests.Add(requestOrder);

            //_context.SubmitChanges();
            var creditRepository = new ResellerCreditRepository();
            var resellerId = Convert.ToInt32(demand.WorkOrder.ResellerID);
            var branchCredit = _branchcreditRepository.GetNetCredit(branchId);

            var customersDue = requests.Sum(x => x.Total != null ? x.Total.Value : 0);

            var branchrequests =
                context.WorkOrderRequests.Where(
                    a =>
                        a.WorkOrder.BranchID == branchId && a.RSID == 3 && a.RequestID == 11 &&
                        a.WorkOrder.ResellerID == null).ToList();

            var amount = branchrequests.Sum(s => s.Demand.Amount);
            var newbranchcredit = branchCredit - amount;
            var newresellercredit = customersDue;
            if (resellerId != 0)
            {
                var resellerrequests =
                    context.WorkOrderRequests.Where(
                        a => a.WorkOrder.ResellerID == resellerId && a.RSID == 3 && a.RequestID == 11).ToList();
                var resamount = resellerrequests.Sum(a => a.Demand.Amount);
                var resellerCredit = Convert.ToDecimal(creditRepository.GetNetCredit(resellerId));
                newresellercredit = resellerCredit - resamount;
            }
            if (context.Options.First().DiscoundfromResellerandBranch)
            {
                if (newbranchcredit < customersDue || newresellercredit < customersDue)
                {
                    Msg2.Visible = true;
                    Msg.Visible = false;
                    Msg2.InnerHtml = string.Format("    ,{0} {1}", Tokens.CreditIsntEnough, "لعمل طلب سداد");
                    return false;
                }
            }
            else
            {
                if (newbranchcredit < customersDue)
                {
                    Msg2.Visible = true;
                    Msg.Visible = false;
                    Msg2.InnerHtml = string.Format("    ,{0} {1}", Tokens.CreditIsntEnough, "لعمل طلب سداد");
                    return false;
                }
            }
            if (requests.Count == 0) return false;
            //demand.IsRequested = true;
            context.WorkOrderRequests.InsertAllOnSubmit(requests);
            context.SubmitChanges();
            return true;
        }

        private void SendSms(ISPDataContext context, WorkOrder order, int indexnotification)
        {
            // send sms by Ahmed Saied
            try
            {
                var mobile = order.CustomerMobile;
                if (string.IsNullOrEmpty(mobile)) return;
                var message = global::SendSms.SendSmsByNotification(context, mobile, indexnotification);
                if (string.IsNullOrEmpty(message)) return;
                var myscript = "window.open('" + message + "')";
                ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript, true);
            }
            catch (Exception)
            {
                return;
            }
        }


        private void PayDemand(int demandId)
        {


            using (var context7 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                var demand = _ispEntries.GetDemand(demandId);
                var userId = Convert.ToInt32(Session["User_ID"]);
                var proccess = true;
                if (CheckAddRequestPayment.Checked)
                {
                    var cussrentuser = context7.Users.FirstOrDefault(x => x.ID == userId);
                    if (cussrentuser != null)
                    {
                        var branchId = Convert.ToInt32(demand.WorkOrder.BranchID);
                        proccess = CreateRquest(demandId, branchId, context7);
                    }
                }
                if (proccess)
                {
                    var boxid = hf_boxId.Value != string.Empty ? Convert.ToInt32(hf_boxId.Value) : 0;
                    var box = context7.Boxes.FirstOrDefault(a => a.ID == boxid);
                    if (boxid > 0)
                    {
                        var notes = "دفع فاتورة عميل" + " " + demand.WorkOrder.CustomerName + " - " +
                                    demand.WorkOrder.CustomerPhone;
                        _boxCreditRepository.SaveBox(boxid, userId, Convert.ToDecimal(txtDiscoundBox.Text)*-1, notes,
                            DateTime.Now.AddHours());
                        var lastdata = _boxCreditRepository.GetLastBoxCredit();
                        demand.BoxCreditId = lastdata.ID;
                    }
                    if (box != null)
                    {
                        var nts = "تخصيم من صندوق : " + box.BoxName+" - "+demand.Notes;
                        _demandService.Pay(demandId, userId, nts, TbComment.Text);
                    }
                    var amount = Convert.ToDecimal(TbAmountincomment.Value);
                    var notes2 = TbComment.Text + " - " + demand.WorkOrder.CustomerName + " - " +
                                 demand.WorkOrder.CustomerPhone + " - " + " فاتورة شهر  " + "(" + demand.StartAt.Month +
                                 " - " + demand.StartAt.Year + ")";
                    var saveId = Convert.ToInt32(ddlSavesPay.SelectedItem.Value);
                    var orderCredit = _orderCredit.GetLastCredit(demand.WorkOrderId, context7);


                    //if (orderCredit < 0)
                    //{
                    //    orderCredit *= -1;
                    //    if (done.Equals(SaveResult.Saved) && orderCredit <= Convert.ToDecimal(demand.Amount))
                    //    {
                    //        //فى حالة ان الرصيد اقل من قيمة المطالبة الى فى تكست المبلغ --الحالة الثانية 
                    //        if (amount > 0) amount *= -1;
                    //        _orderCredit.AddCredit(userId, demand.WorkOrderId, amount,
                    //            "دفع مطالبة من صفحة فواتير العملاء", DateTime.Now.AddHours(), context7);
                    //    }
                    //}
                    //

                    //-----------------------
                    if (orderCredit < 0)
                    {
                        orderCredit *= -1;
                        if (orderCredit >= Convert.ToDecimal(demand.Amount))
                        {
                            //فى حالة ان الرصيد اكبر من او يساوى قيمة المطالبة 

                            var netCredit = orderCredit - demand.Amount;

                            if (netCredit > 0)
                            {
                                netCredit *= -1;
                                _orderCredit.AddCredit(userId, demand.WorkOrderId, netCredit,
                                    "دفع مطالبة من صفحة فواتير العملاء", DateTime.Now.AddHours(), context7);
                            }
                        }
                        else if (orderCredit < Convert.ToDecimal(demand.Amount))
                        {
                            //فى حالة ان (الرصيد + المبلغ المدفوع) (الى فى تكست المبلغ) يساوى قيمة المطالبة يتم تصفير الكريدت 
                            if (Convert.ToDecimal(orderCredit + amount) == Convert.ToDecimal(demand.Amount))
                            {
                                _orderCredit.AddCredit(userId, demand.WorkOrderId, 0,
                                    "دفع مطالبة من صفحة فواتير العملاء", DateTime.Now.AddHours(), context7);
                                var done = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount),
                                    "دفع مطالبة من صفحة فواتير العملاء",
                                    notes2, context7);
                            }
                            else if (Convert.ToDecimal(orderCredit + amount) < Convert.ToDecimal(demand.Amount))
                            {
                                //فى حالة ان (المبلغ المدفوع + رصيد العميل) اصغر من قيمة المطالبة اضيف دين على العميل
                                context7.DebtsInvoices.InsertOnSubmit(new DebtsInvoice
                                {
                                    DemandId = demandId,
                                    Amount = demand.Amount - (orderCredit + amount),
                                    paid = false
                                });
                                context7.SubmitChanges();

                                _orderCredit.AddCredit(userId, demand.WorkOrderId, 0,
                                    "دفع مطالبة من صفحة فواتير العملاء", DateTime.Now.AddHours(), context7);
                                var done = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount),
                                    "دفع مطالبة من صفحة فواتير العملاء",
                                    notes2, context7);
                            }
                            else if (orderCredit < Convert.ToDecimal(demand.Amount) &&
                                     amount == Convert.ToDecimal(demand.Amount))
                            {
                                _orderCredit.AddCredit(userId, demand.WorkOrderId, 0,
                                    "دفع مطالبة من صفحة فواتير العملاء", DateTime.Now.AddHours(), context7);
                                var saveamount = Convert.ToDecimal(demand.Amount) - orderCredit;
                                var done = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(saveamount),
                                    "دفع مطالبة من صفحة فواتير العملاء",
                                    notes2, context7);
                            }

                        }
                    }
                    else
                    {
                        var done = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(amount),
                            "دفع مطالبة من صفحة فواتير العملاء",
                            notes2, context7);
                        var realAmount = Convert.ToDecimal(TbAmountincomment.Value);
                        if (demand.Amount > realAmount)
                        {
                            context7.DebtsInvoices.InsertOnSubmit(new DebtsInvoice
                            {
                                DemandId = demandId,
                                Amount = demand.Amount - realAmount,
                                paid = false
                            });
                            context7.SubmitChanges();
                        }
                    }

                    Msg2.Visible = false;
                    Msg.Visible = true;
                    Msg.InnerHtml = Tokens.Saved;
                    context7.Receipts.InsertOnSubmit(new Receipt
                    {
                        DemandId = demandId,
                        PrcessDate = DateTime.Now.AddHours(),
                        Notes = TbComment.Text
                    });
                    context7.SubmitChanges();
                    SearchDemands();

                


                    // workorderrequests requestid=2 rsid=3
                    try
                    {
                        var workOrderRequest2 =
                            context7.WorkOrderRequests.Where(
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
                            context7.SubmitChanges();
                            var currentWorkOrder =
                               context7.WorkOrders.FirstOrDefault(wo => wo.ID == workOrderRequest.WorkOrderID);


                            var option = OptionsService.GetOptions(context7, true);
                            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                            {
                                CenterMessage.SendRequestReject(currentWorkOrder, " تم الغاء الطلب بدفع فاتورة العميل ",
                                    workOrderRequest.Request.RequestName, Convert.ToInt32(Session["User_ID"]));
                            }

                        }

                      



                        using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                        {
                            //var userId = Convert.ToInt32(Session["User_ID"]);
                            var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                            var orderId = Convert.ToInt32(que);
                            var order = _ispEntries.GetWorkOrder(orderId);
                            var demands = _demandService.CustomerDemands(orderId);
                            var unpaid = demands.Where(x => !x.Paid).OrderBy(a => a.Id).ToList();
                            try
                            {
                                bool portalIsStoped = false;
                                if (unpaid.Count == 0 && order.WorkOrderStatusID == 11)
                                {
                                    var portalList2 = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                                    var woproviderList2 = db8.WorkOrders.FirstOrDefault(z => z.ID == orderId);
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
                                                            var request = new WorkOrderRequest
                                                            {
                                                                WorkOrderID = orderId,
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
                                                            db8.WorkOrderRequests.InsertOnSubmit(request);
                                                            db8.SubmitChanges();

                                                            //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                            var current = db8.WorkOrders.FirstOrDefault(x => x.ID == orderId);
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
                                                            if (option != null && option.PortalRelayDays !=null&& daysCount > option.PortalRelayDays)
                                                            {
                                                                order.RequestDate.Value.AddDays(daysCount);
                                                                _ispEntries.Commit();
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
                                                        a => a.WorkOrderID == orderId && a.RequestID == 3 && a.RSID == 3)
                                                        .ToList();
                                        if (wrq.Count == 0)
                                        {
                                            var request3 = new WorkOrderRequest
                                            {
                                                WorkOrderID = orderId,
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




                        try
                        {
                            //ارسال رسالة بعد اتمام العمليات على مركز الرسائل الداخلى
                            if (WidthOption != null && Convert.ToBoolean(WidthOption.SendMessageAfterOperations))
                            {
                                CenterMessage.SendPublicMessageReport(demand.WorkOrder,
                                    "تم دفع المطالبة من صفحة فواتير العملاء",
                                    userId);
                            }
                            NotifyAdmin(demand.WorkOrder, "تم دفع فاتورة العميل من صفحة فواتير العملاء", context7);
                            SendSms(context7, demand.WorkOrder, 5);


                        }
                        catch (Exception)
                        {
                            Msg.Visible = false;
                            Msg2.Visible = true;
                            Msg2.InnerHtml = Tokens.ErrorMsg;
                        }


                    }
                    catch (Exception)
                    {


                    }


                }
                else
                {
                    Msg.Visible = false;
                    Msg2.Visible = true;
                    Msg2.InnerHtml = Tokens.CreditIsntEnough;
                }
            }
        }

        private static void NotifyAdmin(WorkOrder updatedwo, string message, ISPDataContext context)
        {
            var active = context.EmailCnfgs.FirstOrDefault();
            if (active == null || !active.Active) return;
            var admin = context.Users.FirstOrDefault(a => a.ID == 1);
            if (admin == null) return;
            //var msg =
            //    "<div style='margin-left:30%'><table style=' background-color: #CEF6EC;border: 1px solid black;' ><tr ><th>" + updatedwo.CustomerName + "</th> <td>" + ":" + Tokens.Customer_Name + "</tr><tr><th>" + updatedwo.CustomerPhone + "</th><td>" + ":" + Tokens.PhoneNo + "</td</tr></table>  <br/><br/></div>" + "<div style='margin-right:35%'><h4>" + message + "</h4></div>";

            var msg =
                "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                ":" + Tokens.Customer_Name + "</span></div>" + updatedwo.CustomerName +
                "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo +
                "</div> <span><br/> " + updatedwo.CustomerPhone +
                "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" +
                message + "</p></div>";





            var formalmessage = ClsEmail.Body(msg);
            ClsEmail.SendEmail(admin.UserEmail,
                ConfigurationManager.AppSettings["InstallationEmail"]
                , ConfigurationManager.AppSettings["CC2Email"],
                "Customer: " + updatedwo.CustomerPhone, formalmessage
                , true);
        }

        protected void EditUnpaidDemand(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HfEditDemandId.Value)) return;
            var demandId = Convert.ToInt32(HfEditDemandId.Value);

            _demandService.EditDemand(demandId, Convert.ToDateTime(TbEFrom.Text), Convert.ToDateTime(TbETo.Text),
                Convert.ToDecimal(TbEAmount.Text), TbENotes.Text, CBResellerCommession.Checked);
            Msg.InnerHtml = Tokens.Saved;
            Msg.Visible = true;
            Msg2.Visible = false;
            SearchDemands();
            //HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        protected void CancelPayment(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hdfId.Value)) return;
            if (Session["User_ID"] == null) return;
            var demandId = Convert.ToInt32(hdfId.Value);
            var userId = Convert.ToInt32(Session["User_ID"]);
            _demandService.CancelPayment(demandId);
            var demand = _ispEntries.GetDemand(demandId);
            var box = _boxCreditRepository.GetCredit(Convert.ToInt32(demand.BoxCreditId));
            if (box != null)
                _boxCreditRepository.SaveBox(Convert.ToInt32(box.BoxId), userId, Convert.ToDecimal(box.Amount)*-1,
                    "الغاء فاتورة مدفوعة" + demand.WorkOrder.CustomerName + " - " + demand.WorkOrder.CustomerPhone,
                    DateTime.Now.AddHours());
            //Unpaidstepsinsaves(demand);

            var saveId = Convert.ToInt32(ddlSavesCancelPay.SelectedItem.Value);
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var done = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(demand.Amount)*-1,
                    " تخصيم قيمة مطالبة من صفحة فواتير العملاء" + " " + demand.WorkOrder.CustomerName + " - " +
                    demand.WorkOrder.CustomerPhone, "", context);

                //commented for not changing order credit
                //if (done.Equals(SaveResult.Saved))
                //{
                //    var orderCredit = _orderCredit.GetLastCredit(demand.WorkOrderId, context);
                //    var amount = demand.Amount;
                //    amount *= -1;
                //    var newamount =  orderCredit;
                //    _orderCredit.AddCredit(userId, demand.WorkOrderId, newamount, "تخصيم قيمة مطالبة من صفحة فواتير العملاء", DateTime.Now.AddHours(), context);
                //}
                // _userSave.UpdateSave(userId, saveId, Convert.ToDouble(demand.Amount)*-1,
                // " تخصيم قيمة مطالبة من صفحة فواتير العملاء" +" "+ demand.WorkOrder.CustomerName + " - " + demand.WorkOrder.CustomerPhone, "", context);
            }
            Msg.InnerHtml = Tokens.Saved;
            Msg.Visible = true;
            Msg2.Visible = false;
            SearchDemands();
            //HttpContext.Current.Response.Redirect(Request.RawUrl);
        }

        protected void PrePayment(object sender, EventArgs e)
        {
            if (Session["User_ID"] == null) Response.Redirect("default.aspx");
            if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
            var orderId = Convert.ToInt32(que);
            var order = _ispEntries.GetWorkOrder(orderId);
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (order == null || ddlPrePay.SelectedIndex == 0 || string.IsNullOrEmpty(txtPrePayAmount.Text)) return;
                var not = order.CustomerName + " " + order.CustomerPhone + " " + txtNotes.Text;

                var saveId = Convert.ToInt32(ddlPrePay.SelectedItem.Value);
                var done = _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(txtPrePayAmount.Text),
                    "دفع مقدم من فواتير العملاء", not, context);
                if (done.Equals(SaveResult.Saved))
                {
                    var lastCredit = _orderCredit.GetLastCredit(order.ID, context);
                    if (lastCredit == 0)
                        lastCredit = Convert.ToDecimal(txtPrePayAmount.Text)*(-1);
                    else
                        lastCredit -= Convert.ToDecimal(txtPrePayAmount.Text);
                    _orderCredit.AddCredit(userId, order.ID, lastCredit, not,
                        DateTime.Now.AddHours(), context);
                    var lastOrderCredit = _orderCredit.GetLastRow(order.ID, context);

                    //PopulateAjaxPop(lastOrderCredit.Id,order.ID,Convert.ToDecimal(txtPrePayAmount.Text),context);
                    var url = string.Format("OrderReceipt.aspx?receiptId={0}&orderId={1}&amount={2}",
                        QueryStringSecurity.Encrypt(lastOrderCredit.Id.ToString()),
                        QueryStringSecurity.Encrypt(orderId.ToString()),
                        QueryStringSecurity.Encrypt(txtPrePayAmount.Text));
                    var myscript = "window.open('" + url + "')";
                    ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript, true);
                }
            }
            Msg.InnerHtml = Tokens.Saved;
            Msg.Visible = true;
            Msg2.Visible = false;
            SearchDemands();
        }

        protected void NextMonthDemand(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
            var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
            var orderId = Convert.ToInt32(que);
            var lastDemand = _demandService.GetLastDemand(Convert.ToInt32(orderId));
            if (lastDemand == null)
            {
                return;
            }
            var processDemandsService = new ProcessDemandsService(_ispEntries, new DemandFactory(_ispEntries));
            var priceService = new PriceServices();
            if (lastDemand.WorkOrder.RequestDate == null) return;
            var endAt = lastDemand.WorkOrder.RequestDate.Value;
            var order = lastDemand.WorkOrder;
            if (Session["User_ID"] == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            if (order.WorkOrderStatusID == 8 || order.WorkOrderStatusID == 9 || order.WorkOrderStatusID == 11)
            {
                Msg2.InnerHtml = @"حالة العميل لا تسمح باضافة مطالبة جديدة";
                Msg2.Visible = true;
                Msg.Visible = false;
                return;
            }
            //ippackage
            if (order.IpPackageID != null && order.IpPackage.IpPackageName != "0")
            {
                try
                {
                    var ipAmount = Convert.ToInt32(order.IpPackage.IpPackageName)*10;
                    // required += ipAmount;
                    var requestDate = Convert.ToDateTime(order.RequestDate);
                    //غيرنا بداية و تهاية المطالبة بدلالة تاريخ مطالبة العميل
                    var demandFactory = new DemandFactory(_ispEntries);
                    var demand = demandFactory.CreateDemand(order, requestDate, requestDate.AddMonths(1), ipAmount,
                        userId,
                        paid: false,
                        notes: "IP Package", isCommesstion: false);
                    _ispEntries.AddDemands(demand);
                    _ispEntries.Commit();

                }
                catch
                {
                    Msg2.Visible = true;
                    Msg.Visible = false;
                    Msg2.InnerHtml = Tokens.ErrorMsg;
                }
            }

            var basic = priceService.BillDefault(lastDemand.WorkOrder, endAt.Month, endAt.Year, null).Net;
            if (lastDemand.WorkOrder.OfferStart == null) return;
            if (Session["User_ID"] == null) return;
            processDemandsService.CreateActivationDemand(orderId, endAt, endAt.AddMonths(1), basic,
                lastDemand.WorkOrder.OfferStart.Value, false, Convert.ToInt32(Session["User_Id"]));
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                SendSms(context, order, 1);
            }
            Msg2.Visible = false;
            Msg.Visible = true;
            Msg.InnerHtml = Tokens.Saved;
            SearchDemands();
            //HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        protected void DeleteCurrentDemand(object sender, EventArgs e)
        {
            using (var context7 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var button = sender as LinkButton;
                if (button != null)
                {
                    var deleted = false;
                    var demandId = Convert.ToInt32(button.CommandArgument);
                    var workid = context7.Demands.FirstOrDefault(z => z.Id == demandId);
                    var alldemondid =
                        context7.Demands.Where(z => z.WorkOrderId == workid.WorkOrderId && z.Paid == false).Select(z => z.Id).ToList();

                    var lastdemond = alldemondid.LastOrDefault();
                    if (lastdemond == demandId)
                    {
                        deleted = _ispEntries.DeleteDemand(demandId, true);
                    }
                    else
                    {
                        deleted = _ispEntries.DeleteDemand(demandId, false);
                    }

                    if (deleted)
                    {
                        try
                        {
                            _ispEntries.Commit();
                            Msg.InnerHtml = Tokens.Deleted;
                            Msg.Visible = true;
                            Msg2.Visible = false;
                            SearchDemands();
                        }
                        catch
                        {
                            Msg2.InnerHtml = Tokens.DemandCantBeDeleted;
                            Msg2.Visible = true;
                            Msg.Visible = false;
                        }

                    }
                    else
                    {
                        Msg2.InnerHtml = Tokens.DemandCantBeDeleted;
                        Msg2.Visible = true;
                        Msg.Visible = false;
                    }
                    //HttpContext.Current.Response.Redirect(Request.RawUrl);
                }
            }
        }


        protected void SuspendOrder(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
            var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
            var orderId = Convert.ToInt32(que);
            if (Session["User_ID"] == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            var order = _requestsService.GetOrder(orderId);
            if (order.WorkOrderStatusID == 6)
            {
                var suspended = _requestsService.SuspendByOrderId(orderId, userId);
                if (suspended)
                {
                    Msg.InnerHtml = Tokens.Saved;
                    Msg.Visible = true;
                    Msg2.Visible = false;
                }
                else
                {
                    Msg2.InnerHtml = Tokens.CantSuspend;
                    Msg2.Visible = true;
                    Msg.Visible = false;
                }
                if (suspended)
                {
                    using (var context8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        _requestNotifiy.AddNotification(order.ID, 2, false, DateTime.Now.AddHours(), userId, context8);
                    }
                }
                ResetSearch();
            }
            //HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        //protected void Activeorder(object sender, EventArgs e){
        protected void HandleUnsuspendRequest(string whichbtn)
        {

            // using (var context8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                var orderid = Convert.ToInt32(que);
                var order = _ispEntries.GetWorkOrder(orderid);


                if (order == null || order.WorkOrderStatusID != 11)
                {
                    Msg2.InnerHtml = Tokens.Cant_Unsuspend;
                    Msg2.Visible = true;
                    Msg.Visible = false;
                    return;
                }
                var oldRequests = _ispEntries.GetOrderRequest(order.ID);
                    //context8.WorkOrderRequests.Where(a => a.WorkOrderID == order.ID && a.RSID == 3);
                if (oldRequests.Any())
                {
                    Msg2.InnerHtml = Tokens.User_Has_Request;
                    Msg2.Visible = true;
                    Msg.Visible = false;
                    return;
                }
                var userId = Convert.ToInt32(Session["User_ID"]);
                var thisday = DateTime.Now.AddHours();

                // check customer state in portal
                CookieContainer cookieJar = new CookieContainer();
                var pageString = string.Empty;
                var username1 = string.Empty;
                using (var db9 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var portalList = db9.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                    var woproviderList = db9.WorkOrders.FirstOrDefault(z => z.ID == orderid);
                    if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                    {


                        var username = woproviderList.UserName;
                        if (username != null)
                        {
                            username1 = username;
                            CookieContainer cookiecon = new CookieContainer();
                            cookiecon = Tedata.Login();
                            if (cookiecon != null)
                            {
                                cookieJar = cookiecon;
                                var pagetext = Tedata.GetSearchPage(username, cookiecon);
                                if (pagetext != null)
                                {
                                    var searchPage = Tedata.CheckSearchPage(pagetext);
                                    if (searchPage)
                                    {
                                        pageString = pagetext;
                                        var custStatus = Tedata.CheckCustomerStatus(pagetext);
                                        if (custStatus == "enable")
                                        {
                                            portalRequest.Visible = true;
                                            portalRequest.InnerHtml = "هذا العميل مفعل بالفعل على البورتال";
                                            portalRequest.Attributes.Add("class", "alert alert-danger");

                                            return;
                                        }
                                    }
                                    else
                                    {
                                        portalRequest.Visible = true;
                                        portalRequest.InnerHtml =
                                            "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                        portalRequest.Attributes.Add("class", "alert alert-danger");
                                    }
                                }
                            }
                            else
                            {
                                portalRequest.Visible = true;
                                portalRequest.InnerHtml =
                                    "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                portalRequest.Attributes.Add("class", "alert alert-danger");

                            }
                        }
                    }
                }

                if (whichbtn == "4")
                {
                    using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        var orderRequests = db8.WorkOrderRequests
                            .Where(
                                woreq => woreq.WorkOrderID == orderid && woreq.RSID == 3);

                        if (orderRequests.Any())
                        {
                            Msg.InnerHtml = Tokens.User_Has_Request;
                            Msg.Visible = false;
                            Msg2.Visible = true;
                            return;
                        }
                        var worNote = 5;



                        if (!string.IsNullOrEmpty(pageString) && !string.IsNullOrEmpty(username1))
                        {

                            worNote = Tedata.SendTedataUnSuspendRequest(username1, cookieJar, pageString);
                            if (worNote == 0)
                            {
                                var request = new WorkOrderRequest
                                {
                                    WorkOrderID = orderid,
                                    CurrentPackageID = order.ServicePackageID,
                                    ExtraGigaId = order.ExtraGigaId,
                                    NewPackageID = order.ServicePackageID,
                                    RequestDate = thisday,
                                    RequestID = 3,
                                    RSID = 1,
                                    NewIpPackageID = order.IpPackageID,
                                    SenderID = userId,
                                    ProcessDate = thisday,
                                    ConfirmerID = userId,
                                    PeriodId = Convert.ToInt32(unsusduration.SelectedItem.Value),
                                    Notes = unsusduration.SelectedItem.Text

                                };
                                //context8.WorkOrderRequests.InsertOnSubmit(request);
                                _ispEntries.SaveRequest(request);
                                _ispEntries.Commit();
                               

                                //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                var current = db8.WorkOrders.FirstOrDefault(x => x.ID == orderid);

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
                                }

                                db8.SubmitChanges();


                                // ترحيل ايام السسبند
                                int daysCount = _ispEntries.DaysForCustomerAtStatus(order.ID, 11);
                                var option = OptionsService.GetOptions(db8, true);
                                if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                {
                                    order.RequestDate.Value.AddDays(daysCount);
                                    _ispEntries.Commit();
                                }

                            }

                            if (worNote == 2)
                            {
                                portalRequest.Visible = true;
                                portalRequest.InnerHtml = "لا يمكن الاتصال بسيرفر البورتال";
                                portalRequest.Attributes.Add("class", "alert alert-danger");
                            }
                        }





                        if (worNote == 2 || worNote == 5)
                        {
                            var request = new WorkOrderRequest
                            {
                                WorkOrderID = orderid,
                                CurrentPackageID = order.ServicePackageID,
                                ExtraGigaId = order.ExtraGigaId,
                                NewPackageID = order.ServicePackageID,
                                RequestDate = thisday,
                                RequestID = 3,
                                RSID = 3,
                                NewIpPackageID = order.IpPackageID,
                                SenderID = userId,
                                ProcessDate = thisday,
                                ConfirmerID = userId,
                                PeriodId = Convert.ToInt32(unsusduration.SelectedItem.Value),
                                Notes = unsusduration.SelectedItem.Text
                            };
                            _ispEntries.SaveRequest(request);
                            _ispEntries.Commit();

                        }




                    }
                    Msg2.InnerHtml = Tokens.Saved;
                    Msg2.Visible = true;
                    Msg.Visible = false;
                }
                else
                {


                    //add Unsuspend Request
                    var request = new WorkOrderRequest
                    {
                        WorkOrderID = orderid,
                        CurrentPackageID = order.ServicePackageID,
                        ExtraGigaId = order.ExtraGigaId,
                        NewPackageID = order.ServicePackageID,
                        RequestDate = thisday,
                        RequestID = 3,
                        RSID = 1,
                        NewIpPackageID = order.IpPackageID,
                        SenderID = userId,
                        ProcessDate = thisday,
                        ConfirmerID = userId
                    };
                    //context8.WorkOrderRequests.InsertOnSubmit(request);
                    _ispEntries.SaveRequest(request);
                    _ispEntries.Commit();
                    //context8.SubmitChanges();
                   
                    //Approve this Request

                    var now = Convert.ToDateTime(TbUnsuspendDate.Text);
                    var period = Convert.ToInt32(hdfDaysOfSuspend.Value);
                    // Convert.ToInt32(TbDaysCount.Text);
                    //var ispEntries = new IspEntries(context8);
                    request.ProcessDate = now;
                    if (order.OfferStart == null) order.OfferStart = now;
                    var checkdemand = CheckDemand(order, request, now,Convert.ToInt32(whichbtn));
                    order.WorkOrderStatusID = 6;
                    //workOrder.WorkOrderStatusID = 6;
                    if (!checkdemand)
                    {

                        var demand = _ispEntries.OrderDemand(order.ID).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (demand != null)
                        {
                            if (!demand.Paid && demand.Amount < 0)
                            {
                                //فى حالة ان الفاتورة غير مدفوعة و قيمة الفاتورة بالسالب 
                                switch (whichbtn)
                                {
                                    case "1":
                                        //فى حالة اختار ترحيل ايام السسبند مش هيعمل حاجة
                                        var endAt = demand.EndAt.AddDays(period);
                                        order.RequestDate = endAt;
                                        demand.Amount = 0;
                                        demand.Paid = true;
                                        demand.PaymentDate = DateTime.Now.AddHours();
                                        demand.PaymentComment = "دفعت بطلب تشغيل";
                                        break;
                                    case "2":
                                        //فى حالة اختيار تخصيم مع ثبات تاريخ المطالبة.. هيضرب عدد الايام اللى هيخصمها فى تمن السرعة لليوم الواحد
                                        var newPackBill =
                                            _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(
                                                order, now.Month,
                                                now.Year, Convert.ToInt32(order.ServicePackageID));
                                        var priceForDayinDiscoundDays = (newPackBill.Net/30)*period;
                                        demand.Amount = priceForDayinDiscoundDays*-1;
                                        break;
                                    case "3":
                                        //فى حالة اختار فاتورة كاملة هيحط المطالبة بصفر
                                        demand.Amount = 0;
                                        demand.Paid = true;
                                        demand.PaymentDate = DateTime.Now.AddHours();
                                        demand.PaymentComment = "دفعت بطلب تشغيل";
                                        break;
                                }
                                _ispEntries.Commit();
                            }
                            else if (!demand.Paid && demand.Amount > 0)
                            {
                                if (demand.CaseDetectSuspend != null)
                                {
                                    demand.Amount = Convert.ToDecimal(demand.CaseDetectSuspend);
                                    _ispEntries.Commit();
                                }
                                // demand > 0
                                var daysCount = period; //Convert.ToInt32(TbDaysCount.Text);
                                switch (whichbtn)
                                {
                                    case "1":
                                        UpdateRequestDatePostpone(order, daysCount);
                                        break;
                                    case "2":
                                        UpdateRequestDate(order, daysCount);

                                        break;
                                    case "3":
                                        break;
                                }
                            }
                            else if (demand.Paid)
                            {
                                var newPackBill =
                                    _priceServices.CustomerInvoiceDetailsDefaultbByPackage(order,
                                        now.Month, now.Year, Convert.ToInt32(order.ServicePackageID));
                                decimal amount;
                                var allAmount = newPackBill.Net + Convert.ToDecimal(order.PaymentType.Amount);
                                var periodOfDemand = (demand.EndAt.Date - demand.StartAt.Date).Days;
                                if (periodOfDemand == 30 || periodOfDemand == 31 ||
                                    (periodOfDemand == 28 && demand.StartAt.Month == 2)) amount = allAmount;
                                else amount = (allAmount/30)*periodOfDemand;
                                if (amount > demand.Amount)
                                {
                                    // logic here لو فى فرق بين الاثنين ينزل مطالبة بالفرق غير مدفوعة - لو مفيش فرق بريك 
                                    //var am = newPackBill.Net - demand.Amount;
                                    var am = amount - demand.Amount;
                                    var factory = new DemandFactory(_ispEntries);
                                    var newdemand = factory.CreateDemand(order, now, demand.EndAt, am,
                                        Convert.ToInt32(Session["User_ID"]),
                                        notes:
                                            " الفرق بين الفاتورة المدفوعة و فاتورة الشهر اذا كان هناك خصم على الفاتورة القديم");
                                    _ispEntries.AddDemands(newdemand); //context8.Demands.InsertOnSubmit(newdemand);
                                }
                            }
                        }


                        _ispEntries.Commit();
                        //context8.SubmitChanges();

                    }

                    //CenterMessage.SendMessageReport(workOrder, request.ServicePackage.ServicePackageName, request.ServicePackage1.ServicePackageName, userId);

                    //context8.SubmitChanges();
                    //      _ispEntries.Commit();
                    var wos = new global::Db.WorkOrderStatus
                    {
                        WorkOrderID = order.ID,
                        StatusID = 6,
                        UserID = userId,
                        UpdateDate = thisday
                    };
                    _ispEntries.AddStatus(wos);
                    //context8.WorkOrderStatus.InsertOnSubmit(wos);
                    //context8.SubmitChanges();
                    _ispEntries.Commit();
                }
                if (whichbtn != "4")
                {
                    var worNote = 11;
                    if (!string.IsNullOrEmpty(username1) && !string.IsNullOrEmpty(pageString))
                    {

                        worNote = Tedata.SendTedataUnSuspendRequest(username1, cookieJar, pageString);
                        if (worNote == 2)
                        {
                            //فى حالة عدم الوصول الى البورتال
                            using (
                                var context8 =
                                    new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                            {
                                _requestNotifiy
                                    .AddNotification(order.ID, 3, false, thisday, userId, context8);
                                var option = OptionsService.GetOptions(context8, true);
                                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                                {
                                    CenterMessage.SendRequestApproval(order, "Unsuspend", userId);
                                }
                            }

                            portalRequest.Visible = true;
                            portalRequest.InnerHtml = "لا يمكن الاتصال بسيرفر البورتال";
                            portalRequest.Attributes.Add("class", "alert alert-danger");

                        }
                        /*else
                        {
                            using (var context8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                            {
                                // ترحيل ايام السسبند
                                int daysCount = _ispEntries.DaysForCustomerAtStatus(order.ID, 11);
                                var option = OptionsService.GetOptions(context8, true);
                                if (option != null && option.PortalRelayDays != null &&
                                    daysCount > option.PortalRelayDays)
                                {
                                    order.RequestDate.Value.AddDays(daysCount);
                                    _ispEntries.Commit();
                                }
                            }

                        }*/
                    }
                    else
                    {
                        //فى حالة عدم الوصول الى البورتال
                        using (
                            var context8 =
                                new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                        {
                            _requestNotifiy
                                .AddNotification(order.ID, 3, false, thisday, userId, context8);
                            var option = OptionsService.GetOptions(context8, true);
                            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                            {
                                CenterMessage.SendRequestApproval(order, "Unsuspend", userId);
                            }
                        }
                        if (!string.IsNullOrEmpty(username1) && !string.IsNullOrEmpty(pageString) && worNote == 2)
                        {
                            portalRequest.Visible = true;
                            portalRequest.InnerHtml = "لا يمكن الاتصال بسيرفر البورتال";
                            portalRequest.Attributes.Add("class", "alert alert-danger");
                        }
                    }
                }

                var workOrderRepository = new WorkOrderRepository();
                var activation = workOrderRepository.GetActivationDate(order.ID);
                if (activation == null)
                {
                    Msg.InnerHtml = @"تم حفظ البيانات و اتمام العمليات بنجاح لكن هذا العميل ليس له تاريخ تفعيل";
                    Msg.Visible = true;
                    Msg2.Visible = false;
                    ResetSearch();
                }
                else
                {
                    Msg.InnerHtml = Tokens.Saved;
                    Msg.Visible = true;
                    Msg2.Visible = false;
                }

            }
            ResetSearch();
        }

        protected void UnsuspendWithPostponeSuspendDays(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("1");
        }


        protected void UnsusPendWithDeductionWithFixedRequestDate(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("2");
        }


        protected void UnsusPendWithCompleteInvoice(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("3");
        }

        protected void Unsuslimited(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("4");
        }

        //فحص اذا كان العميل مالوش فواتير قبل كدة او تاريخ التنفيذ بعد تاريخ المطالبة
        protected bool CheckDemand(WorkOrder order, WorkOrderRequest request, DateTime date,int btn)
        {
            var daysinmonth = DateTime.DaysInMonth(date.Year, date.Month);
            //var nextemonth = date.AddMonths(1);
            var lastday = new DateTime(date.Year, date.Month, daysinmonth);
                //new DateTime(date.Year, nextemonth.Month, 1);
            //var ispEntries = new IspEntries();
            var priceServices = new PriceServices();
            var packBill = priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month,
                date.Year, Convert.ToInt32(order.ServicePackageID));

            if (order.OfferStart != null && order.Offer != null)
            {
                Offer offer = order.Offer;
                DateTime offerEndDate = order.OfferStart.Value.AddMonths(offer.LifeTime);
                //DateTime plusMonth = startAt.AddMonths(1);
                if (date.Date >= order.OfferStart && date.Date < offerEndDate.Date)
                {
                    packBill.Net = packBill.Net -
                                   OfferPricingServices.GetOfferPrice(order.Offer, packBill.Net, packBill.Net);
                }
                else
                {
                    order.Offer = null;
                }
            }

            var userId = Convert.ToInt32(Session["User_ID"]);
            var demand = _ispEntries.OrderDemand(order.ID).OrderByDescending(x => x.Id).FirstOrDefault();

            if (demand == null)
            {
                if (btn == 1)
                {
                    if (!string.IsNullOrEmpty(TbDaysCount.Text))
                    {
                        var val = Convert.ToDouble(TbDaysCount.Text);
                        var curDate = order.RequestDate.Value;
                        order.RequestDate = curDate.AddDays(val);
                        return true;
                    }
                }
                else
                {
                    AddNewDemand(order, date, lastday, packBill, _ispEntries, userId);
                    return true;
                }
            }
            if (request.ProcessDate > demand.EndAt)
            {
                if (btn == 1)
                {
                    if (!string.IsNullOrEmpty(TbDaysCount.Text))
                    {
                        var val = Convert.ToDouble(TbDaysCount.Text);
                        var curDate = order.RequestDate.Value;
                        order.RequestDate = curDate.AddDays(val);
                        return true;
                    }
                }
                else
                {
                    AddNewDemand(order, date, lastday, packBill, _ispEntries, userId);
                    return true;
                }
            }
            if (request.ProcessDate <= demand.EndAt)
            {
                return false;
            }
            return false;
        }

        private void AddNewDemand(WorkOrder order, DateTime date, DateTime lastDay, BillDetails packageBill,
            IspEntries ispEntries, int userId)
        {
            var demandFactory = new DemandFactory(ispEntries);

            if (order.RequestDate != null && order.RequestDate.Value.Day == 1)
            {
                var newdatemonth = date.AddMonths(1);
                var newenddate = new DateTime(newdatemonth.Year, newdatemonth.Month, 1);
                var period = (lastDay.Date - date.Date).Days + 1;
                var periodPercent = Convert.ToDecimal(period)/Convert.ToDecimal(30);
                var amount = packageBill.Net*periodPercent;
                const string notes =
                    "  اضافة مطالبة جديدة من اول الشهر لمستخدم لم يكن لها مطالبات قبل ذلك و تاريخ المطالبة كان فى اول الشهر";
                var newdemand = demandFactory.CreateDemand(order, date, newenddate, amount, userId, notes: notes);
                order.RequestDate = newenddate;
                ispEntries.AddDemand(newdemand);
            }
            else if (order.RequestDate != null && order.RequestDate.Value.Day > 1)
            {
                var enddate = date.AddMonths(1);
                const string notes =
                    " اضافة مطالبة جديدة لمستخدم لم يكن له مطالبات قبل ذلك وتاريخ المطالبة كان خلال الشهر";
                var newdemand = demandFactory.CreateDemand(order, date, enddate, packageBill.Net, userId, notes: notes);
                    //date.AddMonths(1)
                order.RequestDate = enddate;
                ispEntries.AddDemand(newdemand);
            }
        }

        private void UpdateRequestDate(WorkOrder updatedwo, int daysCount = 0)
        {
            _demandService.UpdateRequestDate(updatedwo.ID, Convert.ToDateTime(TbUnsuspendDate.Text), 11, daysCount);
        }


        private void UpdateRequestDatePostpone(WorkOrder updatedwo, int postponed = 0)
        {
            _demandService.UpdateRequestDate(updatedwo.ID, Convert.ToDateTime(TbUnsuspendDate.Text), 11,
                postponed: postponed);
        }

        protected void AddExtraGiga(object sender, EventArgs e)
        {
            using (var context9 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                var orderid = Convert.ToInt32(que);
                var order = context9.WorkOrders.FirstOrDefault(p => p.ID == orderid);
                if (order != null && order.WorkOrderStatusID == 6)
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var now = DateTime.Now.AddHours();
                    //add ExtraGiga Request
                    var request = new WorkOrderRequest
                    {
                        WorkOrderID = orderid,
                        CurrentPackageID = order.ServicePackageID,
                        ExtraGigaId = Convert.ToInt32(DdlExtraGigas.SelectedItem.Value),
                        NewPackageID = order.ServicePackageID,
                        RequestDate = now,
                        RequestID = 9,
                        RSID = 1,
                        NewIpPackageID = order.IpPackageID,
                        SenderID = userId,
                        ProcessDate = now,
                        ConfirmerID = userId
                    };
                    context9.WorkOrderRequests.InsertOnSubmit(request);
                    context9.SubmitChanges();

                    var notes = string.Format("{0}: {1}", Tokens.ExtraGigas, request.ExtraGiga.Name);
                    _demandService.AddDemandForWorkOrderService(order.ID, notes,
                        Convert.ToDecimal(request.ExtraGiga.Price), userId, false, false);

                    _requestNotifiy
                        .AddNotification(order.ID, 9, false, now, userId, context9, request.ExtraGiga.Name);
                    var option = OptionsService.GetOptions(context9, true);
                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                    {
                        CenterMessage.SendRequestApproval(order, Tokens.ExtraGigas, userId);
                    }
                    Msg.InnerHtml = Tokens.Saved;
                    Msg.Visible = true;
                    Msg2.Visible = false;
                    ResetSearch();
                }
            }

        }

        protected void SearchPrePaidReceipt(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;

                if (string.IsNullOrWhiteSpace(txtReceiptNumber.Text)) return;
                var receiptId = Convert.ToInt32(txtReceiptNumber.Text);
                if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                var orderId = Convert.ToInt32(que);
                var last = context.WorkOrderCredits.FirstOrDefault(a => a.Id == receiptId);
                    //_orderCredit.GetLastRow(orderId, context);
                var orderCredit = //context.WorkOrderCredits.FirstOrDefault(a => a.Id == receiptId);
                    context.WorkOrderCredits.Where(a => a.WorkOrderId == orderId).OrderByDescending(a => a.Id).ToList();
                if (last == null) return;
                decimal afterLast = 0;
                foreach (var cred in orderCredit)
                {
                    if (cred.CreditAmount == last.CreditAmount || last.Id <= cred.Id) continue;
                    if (afterLast != 0) continue;
                    afterLast = cred.CreditAmount;
                }
                var amount = last.CreditAmount - afterLast;
                // PopulateAjaxPop(receiptId,orderId, (amount*-1), context);
                Response.Redirect(string.Format("OrderReceipt.aspx?receiptId={0}&orderId={1}&amount={2}",
                    QueryStringSecurity.Encrypt(receiptId.ToString()), QueryStringSecurity.Encrypt(orderId.ToString()),
                    QueryStringSecurity.Encrypt(amount.ToString())));
            }
        }

        /*void PopulateAjaxPop(int receiptId,int orderId,decimal amount,ISPDataContext context)
            {
                var receipt = _orderCredit.GetCredit(receiptId, context);
                if (receipt == null) return;
                var order = receipt.WorkOrder;//context.WorkOrders.FirstOrDefault(p => p.ID == orderid);
        
                if (order == null) return;
                if (order.ID != orderId)
                {
                    Msg2.InnerHtml = @"المستخدم صاحب الايصال ليس مطابق للمستخدم الذى تبحث عنه";
                    Msg2.Visible = true;
                    Msg.Visible = false;
                    return;
                }
                var option = OptionsService.GetOptions(context, true);//db5.Options.FirstOrDefault();
                if (option != null && Convert.ToBoolean(option.WidthOfReciept)) datatable.Style["width"] = "8cm";
                else
                {
                    imgSite.Style["width"] = "20%";
                    imgSite.Style["height"] = "17%";
                    imgSite.Style["float"] = "left";
                }
                imgSite.Style["dispaly"] = "block";
                div_Receipt.Visible = true;
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
                var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
                if (cnfg != null)
                {
                    //imgSite.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
                    lblCompanyName.Text = cnfg.CompanyName;
                    lblBranch.Text = cnfg.Branch.BranchName;
                }
                lblReceiptNumber.Text = receipt.Id.ToString();
                txtPrepaid.Text = amount.ToString(CultureInfo.InvariantCulture);//order.Prepaid.ToString();

                txtCustomerName.Text = order.CustomerName;
                txtCustomerPhone.Text = order.CustomerPhone;
                lblNotes.Text = receipt.Notes;
                txtDate.Text = receipt.Time.ToShortDateString();
                lblEmployee.Text = receipt.User.UserName;
                mpe_PrePaidReceipt.Show();
                //ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
            }
            */

        protected void HideAjaxModal(object sender, EventArgs e)
        {
            const string myscript =
                "var mywindow = window.open('', 'new div');mywindow.document.write('<html><head><title> إيصال دفع مقدم</title>');mywindow.document.write('</head><body >');mywindow.document.write($('#receptmodal').prop('outerHTML'));mywindow.document.write('</body></html>');mywindow.document.target = '_blank';$('table img').src($('#imgSite').src);mywindow.print();mywindow.close();return true;";
            ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript, true);

        }

        protected void btn_ApproveSelected_Click(object sender, EventArgs e)
        {
            Msg.InnerHtml = "";
            Msg.Attributes.Remove("class");
            Msg.Visible = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                var orderid = Convert.ToInt32(que);

                var orderRequests = context.WorkOrderRequests
                    .Where(woreq => woreq.WorkOrderID == orderid && woreq.RSID == 3);

                if (orderRequests.Any())
                {
                    Msg.InnerHtml = Tokens.User_Has_Request;
                    Msg.Attributes.Add("class", "alert alert-danger");
                    Msg.Visible = true;
                    return;
                }

                var orders = context.WorkOrders.FirstOrDefault(wo => wo.ID == orderid);


                if (orders == null) return;

                //حالة العميل يجب ان تكون اكتف عند عمل طلب خفض رفع
                if (orders.WorkOrderStatusID != 6)
                {
                    Msg.InnerHtml = Tokens.ToUpgradeDowngradShouldbeActive;
                    Msg.Attributes.Add("class", "alert alert-danger");
                    Msg.Visible = true;
                    return;
                }

                if (orders.WorkOrderStatusID == 6)
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var now = DateTime.Now.AddHours();
                    //add UpgradeDowngrade Request
                    var request = new WorkOrderRequest
                    {
                        WorkOrderID = orderid,
                        CurrentPackageID = orders.ServicePackageID,
                       NewPackageID = Convert.ToInt32(ddl_ServicePackage.SelectedItem.Value),
                        RequestDate = now,
                        RequestID = 1,
                        RSID = 1,
                        NewIpPackageID = orders.IpPackageID,
                        SenderID = userId,
                        ProcessDate = now,
                        ConfirmerID = userId
                    };
                    context.WorkOrderRequests.InsertOnSubmit(request);
                    context.SubmitChanges();

                    //UpgradeDowngrade from handle request in MR
                    var demandService = new DemandService(context);
                    var option = OptionsService.GetOptions(context, true);
                    if (RblUpDwonOptions.SelectedIndex == 0)
                    {
                        if (request.NewPackageID != null)
                            demandService.ProcessUpDownGradeDemand(orders.ID, now, request.NewPackageID.Value,
                                userId,
                                false, true, RblUpDwonOptions.SelectedIndex);
                    }
                    else
                    {
                        if (request.NewPackageID != null)
                            demandService.ProcessUpDownGradeDemand(orders.ID, now, request.NewPackageID.Value,
                                userId,
                                false, false, RblUpDwonOptions.SelectedIndex);
                    }
                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                    {
                        CenterMessage.SendRequestApproval(orders, Tokens.Upgrade_Downgrade, userId);
                    }
                    context.SubmitChanges();
                    SendSms(context, orders, 15);
                    var currentPackageType =
                           context.ServicePackages.FirstOrDefault(x => x.ID == request.CurrentPackageID);
                    var newPackageType = context.ServicePackages.FirstOrDefault(x => x.ID == request.NewPackageID);
                    if (option != null)
                    {
                        var fromType = option.ConvertFromPackageType;
                        var toType = option.ConvertToPackageType;
                        var debt = option.ConversionDebt;

                       
                            if (debt > 0 && newPackageType != null && currentPackageType != null &&
                                currentPackageType.ServicePackageTypeID == fromType &&
                                newPackageType.ServicePackageTypeID == toType && fromType > 0 && toType > 0)
                            {
                                IspEntries _ispEntries = new IspEntries(context);
                                //var dateD = Convert.ToDateTime(TbUpDwonDate.Text);
                                var demandFactory = new DemandFactory(_ispEntries);
                                var demand = demandFactory
                                    .CreateDemand(orders,
                                        Convert.ToDateTime(now.Date),
                                        Convert.ToDateTime(now.Date),
                                        Convert.ToDecimal(debt),
                                        Convert.ToInt32(Session["User_ID"]),
                                        DateTime.Now.AddHours(),
                                        false,
                                        string.Format("قيمة التحويل من {0} الى {1} ",
                                            currentPackageType.ServicePackageName,
                                            newPackageType.ServicePackageName)
                                    );
                                demand.IsResellerCommisstions = false;
                                _ispEntries.AddDemands(demand);
                                _ispEntries.Commit();
                            }
                        
                    }
                    _requestNotifiy
                      .AddNotification(orders.ID, 1, false, now, userId, context, string.Format(" التحويل من {0} الى {1} ",
                                            currentPackageType.ServicePackageName,
                                            newPackageType.ServicePackageName));
                    try
                    {
                        var requ = context.Requests.FirstOrDefault(r => r.ID == 1);
                        if (requ != null)
                        {
                            NotifyUserByProcess(orders, requ, context);
                        }
                    }
                    catch
                    {
                        Msg.InnerHtml = Tokens.ErrorMsg;
                        Msg.Attributes.Add("class", "alert alert-danger");
                        Msg.Visible = true;
                    }
                    Msg.InnerHtml = Tokens.SelectedRequestsApproved;
                    Msg.Attributes.Add("class", "alert alert-success");
                    Msg.Visible = true;
                    ResetSearch();
                }
            }
        }
        private static void NotifyUserByProcess(WorkOrder updatedwo, Request requ, ISPDataContext context)
        {
            var active = context.EmailCnfgs.FirstOrDefault();
            if (active == null || !active.Active) return;


            var msg =
                "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                ":" + Tokens.Customer_Name + "</span></div>" + updatedwo.CustomerName +
                "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo +
                "</div> <span><br/> " + updatedwo.CustomerPhone +
                "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" +
                Tokens.ApprovedLiteral + "<br/>" + requ.RequestName + "</p></div>";

           
            var formalmessage =
                ClsEmail.Body(msg);
            if (updatedwo.User != null)
            {
                ClsEmail.SendEmail(updatedwo.User.UserEmail,
                    ConfigurationManager.AppSettings["InstallationEmail"]
                    , ConfigurationManager.AppSettings["CC2Email"],
                    "Customer: " + updatedwo.CustomerPhone, formalmessage
                    , true);
            }
            else
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var wors2 = context2.WorkOrderRequests.Select(x => x).ToList();
                    var wors = wors2.LastOrDefault(x => x.WorkOrderID == updatedwo.ID);
                    if (wors != null)
                    {
                        var usr = context2.Users.FirstOrDefault(x => x.ID == wors.SenderID);
                        if (usr != null)
                        {
                            var sendr = usr.UserEmail;
                            if (sendr != null)
                            {
                                ClsEmail.SendEmail(sendr,
                                 ConfigurationManager.AppSettings["InstallationEmail"]
                                 , ConfigurationManager.AppSettings["CC2Email"],
                                 "Customer: " + updatedwo.CustomerPhone, formalmessage
                                 , true);
                            }
                        }
                    }
                }
            }

        }
        void Bind_ddl_IpPackage(ISPDataContext db6)
        {
            var query = db6.IpPackages.ToList();
            ddl_IpPackage.DataSource = query;
            ddl_IpPackage.DataTextField = "IpPackageName";
            ddl_IpPackage.DataValueField = "ID";
            ddl_IpPackage.DataBind();
            Helper.AddDefaultItem(ddl_IpPackage,"--choose--",0);
        }

        protected void BtnSaveIpPack_OnServerClick(object sender, EventArgs e)
        {
            Msg.InnerHtml = "";
            Msg.Attributes.Remove("class");
            Msg.Visible = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                if (string.IsNullOrEmpty(HfCustomerId.Value)) return;
                var que = QueryStringSecurity.Decrypt(HfCustomerId.Value);
                var orderid = Convert.ToInt32(que);

                var orderRequests = context.WorkOrderRequests
                    .Where(woreq => woreq.WorkOrderID == orderid && woreq.RSID == 3);

                if (orderRequests.Any())
                {
                    Msg.InnerHtml = Tokens.User_Has_Request;
                    Msg.Attributes.Add("class", "alert alert-danger");
                    Msg.Visible = true;
                    return;
                }

                var orders = context.WorkOrders.FirstOrDefault(wo => wo.ID == orderid);
                if (orders == null) return;
                //حالة العميل يجب ان تكون اكتف عند عمل طلب تغير اى بى
                if (orders.WorkOrderStatusID != 6)
                {
                    Msg.InnerHtml = Tokens.ToaddIPShouldbeActive;
                    Msg.Attributes.Add("class", "alert alert-danger");
                    Msg.Visible = true;
                    return;
                }
                if (ddl_IpPackage.SelectedIndex<=0)
                {
                    Msg.InnerHtml = "إختر باقة أى بى";
                    Msg.Attributes.Add("class", "alert alert-danger");
                    Msg.Visible = true;
                    return;
                }
                if (orders.WorkOrderStatusID == 6)
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var now = DateTime.Now.AddHours();

                    var newIpPackId = Convert.ToInt32(ddl_IpPackage.SelectedItem.Value);
                    //add change ip Request
                    var request = new WorkOrderRequest
                    {
                        WorkOrderID = orderid,
                        CurrentPackageID = orders.ServicePackageID,
                        NewPackageID = orders.ServicePackageID,
                        RequestDate = now,
                        RequestID = 8,
                        RSID = 1,
                        NewIpPackageID = newIpPackId,
                        SenderID = userId,
                        ProcessDate = now,
                        ConfirmerID = userId
                    };
                   
                    context.WorkOrderRequests.InsertOnSubmit(request);  
                    orders.IpPackageID = request.NewIpPackageID;
                    context.SubmitChanges();


                    var demandService = new DemandService(context);
                    var option = OptionsService.GetOptions(context, true);
                  
                        var package = context.IpPackages.FirstOrDefault(x => x.ID == request.NewIpPackageID); 
                        if (package != null)
                        {
                            var ipPackageName = package.IpPackageName;
                            var ipNotes = string.Format("{0}: {1}", Tokens.IpPackages, ipPackageName);
                            if (ipPackageName!="0")
                            {
                                demandService.AddDemandForWorkOrderService(now, orders.ID, ipNotes,
                               Convert.ToDecimal(Convert.ToDecimal(ipPackageName) * 10), Convert.ToInt32(Session["User_ID"]),
                               false,
                               false);
                            }
                           
                            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                            {
                                CenterMessage.SendRequestApproval(orders, Tokens.MenuChangeIPPackage, userId);
                            }
                            SendSms(context, orders, 16);
                        }

                        _requestNotifiy
                        .AddNotification(orders.ID, 8, false, now, userId, context,"IP Package : "+ package.IpPackageName);

                try
                {
                    var query = context.WorkOrders.Where(wo => wo.ID == request.WorkOrderID);
                    var updatedwo = query.FirstOrDefault();
                    var requ = context.Requests.FirstOrDefault(r => r.ID == 8);
                    if (requ != null)
                    {
                        NotifyUserByProcess(updatedwo, requ, context);
                    }
                }
                catch
                {
                    Msg.InnerHtml = Tokens.ErrorMsg;
                    Msg.Attributes.Add("class", "alert alert-danger");
                    Msg.Visible = true;
                }

                Msg.InnerHtml = Tokens.RequestAdded;
                Msg.Attributes.Add("class", "alert alert-success");
                Msg.Visible = true;
                
                context.SubmitChanges();
                }
                ResetSearch();
            }

        }

        private void HandleShowAndHideElements()
        {
            PostponeSuspendDays.Visible = false;
            DeductionWithFixedRequestDate.Visible = false;
            ModalBtnApprove.Visible = false;

            //BtnSave.Visible = false;
           
            foreach (ListItem pr in RblUpDwonOptions.Items)
            {
                //pr.Attributes.CssStyle.Add("visibility", "hidden");
                pr.Attributes.CssStyle.Add("display", "none");
                pr.Enabled = false;
            }
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var pList = context.ShowPendingRequestsOptions.ToList();
                if (pList.Count > 0)
                {
                    foreach (var lst in pList)
                    {
                        foreach (ListItem pr in RblUpDwonOptions.Items)
                        {
                            if (lst.RequestType == "updown" && lst.Name == pr.Value)
                            {
                                //pr.Attributes.CssStyle.Remove("visibility");
                                pr.Attributes.CssStyle.Remove("display");
                                pr.Enabled = true;
                                pr.Selected = true;
                            }
                        }
                        if (lst.RequestType == "unsus" && (lst.Name == "ترحيل ايام السسبند" || lst.Name == Tokens.PostponeSuspendDays))
                        {
                            PostponeSuspendDays.Visible = true;
                        }
                        if (lst.RequestType == "unsus" && (lst.Name == "تخصيم مع ثبات تاريخ المطالبة" || lst.Name == Tokens.DeductionWithFixedRequestDate))
                        {
                            DeductionWithFixedRequestDate.Visible = true;
                        }
                        if (lst.RequestType == "unsus" && (lst.Name == "فاتورة كاملة" || lst.Name == Tokens.CompleteInvoice))
                        {
                            ModalBtnApprove.Visible = true;
                        }
                     
                    }
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
