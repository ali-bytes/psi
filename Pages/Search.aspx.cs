using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
using Resources;

using System.Net;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace NewIspNL.Pages
{
    public partial class Search : CustomPage
    {
        private readonly IspEntries _ispEntries;

        #region srvcs

        private readonly IspDomian _domian;

        private readonly IWorkOrderStatusServices _orderStatusServices;

        private readonly IWorkOrderRepository _workOrderRepository;

        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool AddTicket { get; set; }
        public bool CustomerDemand { get; set; }
        public int GroupId { get; set; }
        public bool ShowPortalButton { get; set; }
        public bool ShowSendRoutersButton { get; set; }
        public bool ShowReciveroutersButton { get; set; }
        public Search()
        {
            var context = IspDataContext;
            _orderStatusServices = new WorkOrderStatusServices();
            _workOrderRepository = new WorkOrderRepository();
            _domian = new IspDomian(context);
            _ispEntries = new IspEntries(context);

        }


        #endregion


        public bool IsNotReseller { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            HandlePrivildges();
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null)
                {
                    Response.Redirect("~/Pages/default.aspx");
                    return;
                }
                var userId = Convert.ToInt32(Session["User_ID"]);
                if (userId == 0) return;
                var edit = EditCustomer(userId, context);
                CanEdit = edit.CanEdit;
                CanDelete = edit.CanDelete;
                GroupId = edit.GroupId;
                AddTicket = edit.AddTicket;
                CustomerDemand = edit.CustomerDemand;
                Activate();
                IsNotReseller = !UserServices.UserIs(Convert.ToInt32(Session["User_Id"]), context, 6);
                Div1.Visible = false;
                if (IsPostBack) return;
                PopulateRouters();
                PopulateStore();
                SendRouters.Visible = false;
                reciverouters.Visible = false;
                grd_resellerData.Visible = false;
                btnNotDirect.Visible = false;
                external_data.Visible = false;
                UserFile1.Woid = 0;
                PopulateReceiptOptions(context);
                var siteDateRepository = new SiteDateRepository(context);
                var siteData = siteDateRepository.SiteData();
                if (siteData != null)
                {
                    LCompany.Text = siteData.SiteName;
                    Company2.InnerHtml = siteData.SiteName;

                }
                if (string.IsNullOrEmpty(Request.QueryString["pn"]) || string.IsNullOrEmpty(Request.QueryString["by"]))
                {
                    ProcessQueryString(context);
                    return;
                }
                RunOrderTEData.Visible = false;
                RunOrderEtisalat.Visible = false;

                var option = OptionsService.GetOptions(context, true);
                if (option != null && option.IncludeGovernorateInSearch) _domian.PopulateGovernorates(DdlGovernorate);
                else GovDiv.Visible = option != null && option.IncludeGovernorateInSearch;
                var phoneName = QueryStringSecurity.Decrypt(Request.QueryString["pn"]);
                var searchBy = QueryStringSecurity.Decrypt(Request.QueryString["by"]);
                rbl_searchType.SelectedIndex = Convert.ToInt32(searchBy);
                if (!string.IsNullOrEmpty(Request.QueryString["g"]))
                    DdlGovernorate.SelectedValue = QueryStringSecurity.Decrypt(Request.QueryString["g"]);
                txt_CustomerPhone0.Text = phoneName;
                tr_CustomerDetails.Visible = false;
                tr_Status.Visible = false;
                tr_Requests.Visible = false;
                tr_Tickets.Visible = false;
                tr_Files.Visible = false;
                tr_woInfo.Visible = false;
                CNotes.Visible = false;
                customerRouter.Visible = false;
                divCallMessages.Visible = false;
                RequestDateHistory.Visible = false;
                if (GroupId == 6)
                {
                    divRejectRequest.Visible = option != null && Convert.ToBoolean(option.ShowRequestsInSearch);
                }
                PerformSearch();
                

            }


        }
        void PopulateStore()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var allstores = context.Stores.ToList();
                ddlStores.DataSource = allstores;
                ddlStores.DataTextField = "StoreName";
                ddlStores.DataValueField = "Id";
                ddlStores.DataBind();
                Helper.AddDefaultItem(ddlStores);
            }
        }
        void PopulateRouters()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var allstores = context.RecieveRouters.Where(a => a.IsRecieved == false).ToList();
                ddlRouters.DataSource = allstores;
                ddlRouters.DataTextField = "RouterSerial";
                ddlRouters.DataValueField = "Id";
                ddlRouters.DataBind();
                Helper.AddDefaultItem(ddlRouters);
            }
        }
        private void HandlePrivildges()
        {
            var context = IspDataContext;

            if (Session["User_ID"] == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            //"Upgrade/Downgrade"
            bool upDown = _ispEntries.UserHasPrivlidge(userId, 38);
            //"Suspend"
            bool suspend = _ispEntries.UserHasPrivlidge(userId, 39);
            //"Unsuspend"
            bool unsuspend = _ispEntries.UserHasPrivlidge(userId, 40);
            //"Cancel"
            bool cancel = _ispEntries.UserHasPrivlidge(userId, 43);
            //"Reactivate"
            bool reactivate = _ispEntries.UserHasPrivlidge(userId, 44);
            //"Hold"
            bool hold = _ispEntries.UserHasPrivlidge(userId, 41);
            //"Unhold"
            bool unhold = _ispEntries.UserHasPrivlidge(userId, 42);
            //"Change IP Package"
            bool changeIp = _ispEntries.UserHasPrivlidge(userId, 45);
            //"Request Extra Giga"
            bool requestExtraGiga = _ispEntries.UserHasPrivlidge(userId, 46);
            bool changeOffer = _ispEntries.UserHasPrivlidge(userId, "ChangeOfferRequest.aspx");
            bool changeRequestDate = _ispEntries.UserHasPrivlidge(userId, "ChangeRequestDate.aspx");
            bool changeActivationDate = _ispEntries.UserHasPrivlidge(userId, "ChangeActivationDate.aspx");
            ShowPortalButton = _ispEntries.UserHasPrivlidge(userId, "ShowPortalData");
            btnUpDoenGrade.Visible = upDown ? true : false;
            btnSuspend.Visible = suspend ? true : false;
            btnUnSuspend.Visible = unsuspend ? true : false;
            btnCancel.Visible = cancel ? true : false;
            btnReactive.Visible = reactivate ? true : false;
            btnHold.Visible = hold ? true : false;
            btnUnHold.Visible = unhold ? true : false;
            btnchangeIP.Visible = changeIp ? true : false;
            btnExtraGiga.Visible = requestExtraGiga ? true : false;
            btnChangeOffer.Visible = changeOffer ? true : false;
            btnChangeRequestDate.Visible = changeRequestDate;
            btnChangeActivationDate.Visible = changeActivationDate;

        }

        private void PopulateReceiptOptions(ISPDataContext context)
        {

            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            if (user == null) return;
            var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
            if (cnfg == null) return;
            LAddress.Text = cnfg.ContactData;
            LAddress2.Text = cnfg.ContactData;
            lblAdreessEtisalat.Text = cnfg.ContactData;

        }

        protected RecieveRouter CheckIfAlreadyExist(int id, ISPDataContext context)
        {
            var router = context.RecieveRouters.FirstOrDefault(a => a.WorkOrderId == id);
            return router;
        }
        protected Db.RecieveRouter CheckIfCustomerNotReceive(int id, Db.ISPDataContext context)
        {
            var router = context.RecieveRouters.FirstOrDefault(a => a.WorkOrderIdRecive == id);
            return router;
        }
        private void Bind_grd_wo()
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var userWorkOrders = DataLevelClass.GetUserWorkOrder();
                List<WorkOrder> orders;
                var option = OptionsService.GetOptions(dataContext, true);
                if (option.IncludeGovernorateInSearch)
                {
                    var govId = DdlGovernorate.SelectedIndex > 0
                        ? Convert.ToInt32(DdlGovernorate.SelectedItem.Value)
                        : 0;
                    if (rbl_searchType.SelectedValue == "1")
                    {
                        orders = dataContext.WorkOrders.Where
                            (x => x.CustomerPhone == txt_CustomerPhone0.Text.Trim() && x.CustomerGovernorateID == govId)
                            .ToList();
                    }
                    else if (rbl_searchType.SelectedValue == "2")
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo =>
                                wo.CustomerName.Contains(txt_CustomerPhone0.Text) && wo.CustomerGovernorateID == govId)
                            .ToList();
                    }
                    else if (rbl_searchType.SelectedValue == "3")
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo =>
                                wo.CustomerMobile == txt_CustomerPhone0.Text.Trim() && wo.CustomerGovernorateID == govId)
                            .ToList();
                    }
                    else
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo => wo.UserName.Contains(txt_CustomerPhone0.Text) && wo.CustomerGovernorateID == govId)
                            .ToList();
                    }
                }
                else
                {
                    if (rbl_searchType.SelectedValue == "1")
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo => wo.CustomerPhone == txt_CustomerPhone0.Text.Trim()).ToList();
                    }
                    else if (rbl_searchType.SelectedValue == "2")
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo => wo.CustomerName.Contains(txt_CustomerPhone0.Text)).ToList();
                    }
                    else if (rbl_searchType.SelectedValue == "3")
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo => wo.CustomerMobile == txt_CustomerPhone0.Text.Trim()).ToList();
                    }
                    else
                    {
                        orders = dataContext.WorkOrders.Where
                            (wo => wo.UserName.Contains(txt_CustomerPhone0.Text)).ToList();
                    }
                }

                var finalList =
                    orders.Where(currentTwo => userWorkOrders.Any(currentUwo => currentUwo.ID == currentTwo.ID))
                        .ToList();
                if (finalList.Count > 0)
                {
                    var grdSrc = from wo in dataContext.WorkOrders
                        where (from idlist in finalList
                            select idlist.ID).Contains(wo.ID)
                        select new
                        {
                            wo.ID,
                            wo.CustomerPhone,
                            wo.CustomerName,
                            wo.Status.StatusName,
                            wo.Branch.BranchName,
                            ResellerName = wo.User.UserName,
                            wo.IpPackage.IpPackageName,
                            wo.UserName,
                            wo.Password,
                           lastStatusDate= wo.WorkOrderStatus.Max(d=>d.UpdateDate),
                           PayType=wo.PaymentType.PaymentTypeName
                        };

                    grd_wo.DataSource = grdSrc;
                    grd_wo.DataBind();
                    var idd = grdSrc.First().ID;
                    var orderexist = CheckIfAlreadyExist(grdSrc.First().ID, dataContext);
                    if (orderexist == null || !Convert.ToBoolean(orderexist.IsRecieved))
                    {
                        SendRouters.Visible = true;
                    }

                    var orderexist2 = CheckIfCustomerNotReceive(grdSrc.First().ID, dataContext);
                    if (orderexist2 == null || orderexist2.CompanyConfirmerUserId == null || orderexist2.CompanyProcessDate == null)
                    {
                        reciverouters.Visible = true;
                    }


                    linkRouter.Visible =
                        toExcel.Visible = divRequests.Visible = btn_AddTicket.Visible = ADemanedsLink.Visible = true;


                    if (GroupId == 6)
                    {
                        divRejectRequest.Visible = option != null && Convert.ToBoolean(option.ShowRequestsInSearch);
                    }

                    if (divRejectRequest.Visible && Convert.ToBoolean(option.ShowRequestsInSearch))
                        PopulateRequests(grdSrc.First().ID, dataContext);

                    Bind_ddl_IpPackage(dataContext);
                    PopulateExtraGigas();
                    var firstOrDefault = grdSrc.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        var order = _domian.GetWorkOrder(firstOrDefault.ID);
                        Bind_ddl_ServicePackage(order, dataContext);
                        PopulateOffers(dataContext, Convert.ToInt32(order.ServiceProviderID));
                        ViewState.Add("ID", Convert.ToInt32(order.ID));
                        lbl_InsertResult.Text = string.Empty;
                        if (CustomerDemand)
                            ADemanedsLink.HRef = string.Format("~/Pages/CustomerDemands.aspx?c={0}&g={1}",
                                QueryStringSecurity.Encrypt(order.CustomerPhone),
                                QueryStringSecurity.Encrypt(order.CustomerGovernorateID.ToString()));


                        btnChangeRequestDate.HRef = string.Format("~/Pages/ChangeRequestDate.aspx?c={0}&g={1}",
                               QueryStringSecurity.Encrypt(order.CustomerPhone),
                               QueryStringSecurity.Encrypt(order.CustomerGovernorateID.ToString()));
                        btnChangeActivationDate.HRef = string.Format("~/Pages/ChangeActivationDate.aspx?c={0}&g={1}",
                             QueryStringSecurity.Encrypt(order.CustomerPhone),
                             QueryStringSecurity.Encrypt(order.CustomerGovernorateID.ToString()));

                        btnNotDirect.Visible = IsDirectCustomer(order.ID) ? false : true ;
                        grd_resellerData.Visible = false;
                        if (ShowPortalButton)
                        {
                            var portalList = dataContext.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                            var woproviderList = dataContext.WorkOrders.FirstOrDefault(z => z.ID == firstOrDefault.ID);
                            if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                            {
                                external_data.Visible = true;
                            }
                            else
                            {
                                external_data.Visible = false;
                            }
                        }
                    }


                    
                }
                else
                {
                    grd_resellerData.Visible = false;
                    btnNotDirect.Visible = false;
                    grd_wo.DataSource = null;
                    grd_wo.DataBind();
                    linkRouter.Visible =
                        toExcel.Visible = divRequests.Visible = btn_AddTicket.Visible = ADemanedsLink.Visible = false;
                        //=tr_Edit.Visible
                }
                Fillrouterdiv(orders.FirstOrDefault());
               
            }
        }



        private void ProcessQueryString(ISPDataContext context)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                var query = QueryStringSecurity.Decrypt(Request.QueryString["id"]);
                var id = Convert.ToInt32(query);
                ProcessWo(id);

            }
            var option = OptionsService.GetOptions(context, true);
            if (option != null && option.IncludeGovernorateInSearch) _domian.PopulateGovernorates(DdlGovernorate);
            else GovDiv.Visible = option != null && option.IncludeGovernorateInSearch;

        }


        private void ProcessWo(int id)
        {
            var order = _workOrderRepository.Get(id);
            if (order == null) return;
            if (order.CustomerGovernorateID != null)
            {
                var selectedValue = string.Format("{0}", order.CustomerGovernorateID.Value);
                UserFile1.Woid = order.ID;
                txt_CustomerPhone0.Text = order.CustomerPhone;
                PerformSearch(selectedValue);
            }

        }



        protected void btn_search_Click(object sender, EventArgs e)
        {
            var d = Session["dz"].ToString();
            grd_externalData.Visible = false;
            grd_usage.Visible = false;
            external_data.Visible = false;
            Div1.Visible = false;
            
            PerformSearch();
        }

        protected void btnCallMessage_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (hf_Woid == null) return;
                var userId = Session["User_ID"];
                if (userId == null) return;
                var id = Convert.ToInt32(hf_Woid.Value);
                var message = new CallMessage
                {
                    MessageText = txtCallMassage.Text,
                    User_Id = Convert.ToInt32(userId),
                    WorkOrder_Id = id,
                    Date = DateTime.Now.Add9Hours()
                };
                context.CallMessages.InsertOnSubmit(message);
                context.SubmitChanges();
            }
        }

        private void PerformSearch(string govId = "")
        {
            tr_CustomerDetails.Visible = false;
            tr_Status.Visible = false;
            tr_Requests.Visible = false;
            tr_Tickets.Visible = false;
            tr_Files.Visible = false;
            tr_woInfo.Visible = false;
            CNotes.Visible = false;
            customerRouter.Visible = false;
            divCallMessages.Visible = false;
            RequestDateHistory.Visible = false;
            Div2.Visible = false;
            Bind_grd_wo();
        }

        protected void btnRecieveToCustomer_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
               
                var woid = Convert.ToInt32(ViewState["ID"]);
                var userId = Convert.ToInt32(Session["User_ID"]);
                var routerId = Convert.ToInt32(ddlRouters.SelectedItem.Value);
                var router = context.RecieveRouters.FirstOrDefault(a => a.Id == routerId); //CheckIfAlreadyExist(routerId, context);

                if (router != null)
                {
                    if (!string.IsNullOrWhiteSpace(fileUpload1.FileName))
                    {
                        var file = fileUpload1.FileName;
                        router.Attachments = file;
                        var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                        string ex = Path.GetExtension(fileUpload1.FileName);

                        if (!string.IsNullOrEmpty(ex) && extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
                        {
                            fileUpload1.SaveAs(Server.MapPath(string.Format("~/Attachments/{0}", file)));
                            if (!string.IsNullOrWhiteSpace(fileUpload2.FileName))
                            {
                                string ex2 = Path.GetExtension(fileUpload2.FileName);

                                if (!string.IsNullOrEmpty(ex2) &&
                                    extensions.Any(currentExtention => currentExtention == ex2.ToUpper()))
                                {
                                    var file2 = fileUpload2.FileName;
                                    router.Attachments2 = file2;
                                    fileUpload2.SaveAs(Server.MapPath(string.Format("~/Attachments/{0}", file2)));
                                }
                            }
                            var cuorder = context.WorkOrders.FirstOrDefault(c => c.ID == woid);
                            if (cuorder != null) cuorder.RouterSerial = router.RouterSerial;
                            router.WorkOrderId = woid;
                            router.CustomerConfirmerUserId = userId;
                            router.CustomerProcessDate = DateTime.Now.AddHours();
                            router.IsRecieved = true;
                            context.SubmitChanges();
                            //Msgsuccess.Visible = true;

                            //ProcessQuery();
                            //PopulateRouters();
                            Div2.InnerHtml = Tokens.Saved;
                            Div2.Attributes.Add("class", "alert alert-success");
                            Div2.Visible = true;
                            SendRouters.Visible = false;
                        }
                       
                    }

                }
                else
                {
                    Div2.InnerHtml = Tokens.NoResults;
                    Div2.Attributes.Add("class", "alert alert-danger");
                    Div2.Visible = true;
                    
                }
            }
        }

        protected void btnRecieveFromCompany_Click(object sender, EventArgs e)
        {
            using (var context = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var check = context.RecieveRouters.Where(z => z.RouterSerial == txtRouterSerial.Text).Select(z => z).ToList();
                if (check.Count > 0)
                {
                    Div2.InnerHtml = Tokens.SavingError + " سيريال الراوتر موجود من قبل";
                    Div2.Attributes.Add("class", "alert alert-success");
                    Div2.Visible = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(txtRouterSerial.Text) || string.IsNullOrEmpty(txtRouterType.Text)) return;
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var storeId = Convert.ToInt32(ddlStores.SelectedItem.Value);
                    var woid = Convert.ToInt32(ViewState["ID"]);
                    var receivefromcompany = new Db.RecieveRouter
                    {
                        RouterSerial = txtRouterSerial.Text.Trim(),
                        RouterType = txtRouterType.Text.Trim(),
                        CompanyConfirmerUserId = userId,
                        CompanyProcessDate = DateTime.Now.AddHours(),
                        StoreId = storeId,
                        WorkOrderIdRecive = woid,
                        IsRecieved = false

                    };
                    context.RecieveRouters.InsertOnSubmit(receivefromcompany);
                    context.SubmitChanges();
                    Div2.InnerHtml = Tokens.Saved;
                    Div2.Attributes.Add("class", "alert alert-success");
                    Div2.Visible = true;
                    reciverouters.Visible = false;
                    PopulateRouters();
                }
            }

           
        }


        protected void lnb_Edit_Click(object sender, EventArgs e)
        {

            ViewWoDetails(Convert.ToInt32(((LinkButton) sender).CommandArgument));
            tr_CustomerDetails.Visible = true;
            tr_Status.Visible = true;
            tr_Requests.Visible = true;
            tr_Tickets.Visible = true;
            tr_Files.Visible = true;
            tr_woInfo.Visible = true;
            CNotes.Visible = true;
            customerRouter.Visible = true;
            divCallMessages.Visible = true;
            RequestDateHistory.Visible = true;
            ViewState.Add("ID", Convert.ToInt32(((LinkButton) sender).CommandArgument));
        }






        public SearchPrivilage EditCustomer(int userId, ISPDataContext context)
        {

            var model = new SearchPrivilage();
            var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
            if (user == null)
            {
                model.CanEdit = false;
                model.CanDelete = false;
                model.AddTicket = false;
                model.CustomerDemand = false;
                model.GroupId = 0;
                return model;
            }
            var id = user.GroupID;
            if (id == null)
            {
                model.GroupId = 0;
                model.CanEdit = false;
                model.CanDelete = false;
                model.AddTicket = false;
                model.CustomerDemand = false;
                return model;
            }

            model.GroupId = id.Value;
            var groupPrivilegeQuery =
                context.GroupPrivileges.Where(gp => gp.Group.ID == model.GroupId).Select(gp => gp.privilege.Name);
            model.CanEdit = groupPrivilegeQuery.Contains("EditCustomer.aspx");
            model.CanDelete = groupPrivilegeQuery.Contains("DeleteCustomer");
            model.AddTicket = groupPrivilegeQuery.Contains("AddTicket.aspx");
            model.CustomerDemand = groupPrivilegeQuery.Contains("CustomerDemands.aspx");
            return model;

        }


        protected void btn_DirectEdit_Click(object sender, EventArgs e)
        {

            var stringId = ((LinkButton) sender).CommandArgument;
            var id = QueryStringSecurity.Encrypt(stringId);
            Response.Redirect("EditCustomer.aspx?woid=" + id);
        }

        protected void btn_Directcan_Click(object sender, EventArgs e)
        {

            var stringId = ((LinkButton) sender).CommandArgument;
            var id = QueryStringSecurity.Encrypt(stringId);
            Response.Redirect("Contarct.aspx?woid=" + id);
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            var orderId = Convert.ToInt32(((LinkButton) sender).CommandArgument);
            Div2.Visible = false;
            var order = _workOrderRepository.Get(orderId);
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
               bool noti= context.RequestsNotitfications.Any(x => x.WorkOrderId == order.ID);
                if (noti)
                {
                    Div2.InnerHtml = Tokens.customerHasRequest;
                    Div2.Attributes.Add("class", "alert alert-danger");
                    Div2.Visible = true;
                    return;
                }
            }


            if (DeleteCustomer(order))
            {
                Div2.InnerHtml = Tokens.customerHasRequest;
                Div2.Attributes.Add("class", "alert alert-danger");
                Div2.Visible = true;
                lbl_InsertResult.Text = Tokens.CantDeleteCustomer;
                lbl_InsertResult.ForeColor = Color.Red;
                return;
            }
            Div2.InnerHtml = Tokens.CustomerDeletedSuccess;
            Div2.Attributes.Add("class", "alert alert-success");
            Div2.Visible = true;
            lbl_InsertResult.Text = Tokens.CustomerDeletedSuccess;
            lbl_InsertResult.ForeColor = Color.Green;
            Bind_grd_wo();
        }

        public bool DeleteCustomer(WorkOrder order)
        {
            return _workOrderRepository.Delete(order.ID);
        }

        protected void btn_AddTicket_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddTicket.aspx?woid=" + QueryStringSecurity.Encrypt(ViewState["ID"].ToString()));
        }


        protected void grd_wo_DataBound(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                Helper.GridViewNumbering(grd_wo, "no");
                foreach (GridViewRow row in grd_wo.Rows)
                {
                    var hf = row.FindControl("hf_id") as HiddenField;
                    if (hf == null) continue;
                    if (string.IsNullOrEmpty(hf.Value)) continue;
                    var id = Convert.ToInt32(hf.Value);
                    var order = context.WorkOrders.FirstOrDefault(o => o.ID == id);
                    if (order == null) continue;
                    var lOffer = row.FindControl("l_offer") as Label;
                    var lActivation = row.FindControl("l_activation") as Label;
                    var lBranch = row.FindControl("l_branch") as Label;
                    var lGovernate = row.FindControl("l_governate") as Label;
                    var lProvider = row.FindControl("l_provider") as Label;
                    var lSpeed = row.FindControl("l_speed") as Label;
                    var lcentral = row.FindControl("central") as Label;
                    var lrequestDate = row.FindControl("lRequestDate") as Label;
                    if (lOffer != null) lOffer.Text = order.Offer == null ? "-" : order.Offer.Title;
                    if (lBranch != null) lBranch.Text = order.Branch.BranchName;
                    if (lGovernate != null) lGovernate.Text = order.Governorate.GovernorateName;
                    if (lProvider != null) lProvider.Text = order.ServiceProvider.SPName;
                    if (lSpeed != null) lSpeed.Text = order.ServicePackage.ServicePackageName;
                    if (lcentral != null) lcentral.Text = order.Central == null ? "-" : order.Central.Name;
                    if (lrequestDate != null)
                    {
                        lrequestDate.Text = order.RequestDate == null
                            ? string.Empty
                            : order.RequestDate.Value.ToShortDateString();
                    }
                    var activationDate = _orderStatusServices.GetStatusStartDate(order.ID, 6);
                    if (lActivation != null)
                        lActivation.Text = activationDate == null ? "-" : activationDate.Value.ToShortDateString();
                }
            }
        }


        private void ViewWoDetails(int id)
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var dataContext2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                var orders = dataContext.WorkOrders.Where(wo => wo.ID == id);

                var selectedOrder = orders.FirstOrDefault();
                UserFile1.CanEdit = false;
                if (selectedOrder == null) return;
                UserFile1.Woid = selectedOrder.ID;
                UserFile1.BindGrd(selectedOrder.ID);


                {
                    var fake = new List<WorkOrder>
                    {
                        selectedOrder
                    };
                    Fillrouterdiv(selectedOrder);
                    FillRunOrder(fake);

                }

                if (selectedOrder.WorkOrderStatusID != null && selectedOrder.WorkOrderStatusID.Value == 11)
                {
                    var request =
                        selectedOrder.WorkOrderRequests.Where(x => x.RequestID == 2)
                            .OrderByDescending(x => x.ID)
                            .FirstOrDefault();
                    if (request != null)
                    {
                        if (request.ProcessDate != null)
                        {
                            var period = (DateTime.Now.AddHours().Date - request.ProcessDate.Value.Date).Days;
                            LSuspendedDaysCount.Text = string.Format("{0}", period);
                        }
                    }
                }
                var order = dataContext.WorkOrders.FirstOrDefault(wo => wo.ID == id);
                if (order != null)
                {
                    var activationDate = _workOrderRepository.GetActivationDate(order.ID);
                    if (activationDate != null)
                    {
                        LOfferStart.InnerHtml = order.OfferStart == null
                            ? ""
                            : order.OfferStart.Value.ToShortDateString();
                        if (order.Offer != null && order.OfferStart != null)
                        {
                            var offerLastDate = order.OfferStart.Value.AddMonths(Convert.ToInt32(order.Offer.LifeTime));
                            LOfferEnd.InnerHtml = offerLastDate.ToShortDateString();
                        }
                    }
                    else
                    {
                        LOfferStart.InnerHtml = LOfferEnd.InnerHtml = string.Empty;
                    }
                }
                var query = orders.Select(wo => new
                {
                    wo.CustomerName,
                    wo.Governorate.GovernorateName,
                    wo.CustomerAddress,
                    wo.CustomerPhone,
                    wo.IpPackage.IpPackageName,
                    wo.CustomerMobile,
                    wo.CustomerEmail,
                    wo.ServiceProvider.SPName,
                    wo.ServicePackage.ServicePackageName,
                    reseller = wo.User.UserName,
                    wo.Branch.BranchName,
                    wo.VPI,
                    wo.VCI,
                    wo.UserName,
                    wo.Status.StatusName,
                    wo.Password,
                    ExtraGigas = wo.ExtraGiga.Name,
                    wo.PaymentType.PaymentTypeName,
                    wo.Notes,
                    Central = wo.Central.Name,
                    wo.Offer.Title,
                    wo.ID,
                    wo.PersonalId,
                    wo.RequestNumber,
                    wo.RouterSerial,
                    Installer = wo.User1.UserName,
                    InstallationTime =
                        wo.InstallationTime == null
                            ? "-"
                            : wo.InstallationTime.Value.ToString(CultureInfo.InvariantCulture),
                    wo.WorkorderNumbers,
                    wo.WorkorderDate,
                    wo.PortNumber,
                    wo.BlockNumber,
                    wo.DslamNumbers,
                    wo.LineOwner,
                    wo.Prepaid,
                    wo.InstallationCost,
                    wo.ContractingCost,
                    wo.RecieveRouters,
                    wo.CreationDate,
                    wo.CustomerMobile2
                });
                var workOrderStatus = dataContext.WorkOrderStatus.Where(wos => wos.WorkOrderID == id).Select(wos => new
                {
                    wos.Status.StatusName,
                    wos.User.UserName,
                    wos.UpdateDate
                });

                var workOrderRequests =
                    dataContext.WorkOrderRequests.Where(wor => wor.WorkOrderID == id).Select(wor => new
                    {
                        wor.RequestDate,
                        wor.RequestStatus.RSName,
                        wor.Request.RequestName,
                        wor.User.UserName,
                        UserName2 = wor.User1.UserName,
                        wor.ServicePackage.ServicePackageName,
                        ServicePackageName2 = wor.ServicePackage1.ServicePackageName,
                        wor.RejectReason,
                        wor.ProcessDate,
                        wor.Total,
                        wor.IpPackage.IpPackageName,
                        wor.ExtraGiga.Name,
                        wor.ID,
                        wor.Notes
                    });
                grd_Requests.DataSource = workOrderRequests.OrderBy(a => a.ID);
                grd_Requests.DataBind();
                var hintService = new OrderHintService(dataContext);
                var notes = hintService.OrderNotes(id);
                GvNotes.DataSource = notes.Select(x => new
                {
                    x.Text,
                    x.Time,
                    x.User.UserName,
                    Done = x.Processed ? Tokens.Yes : Tokens.No,
                    Employee = x.User1.LoginName,
                    x.Comment
                });
                GvNotes.DataBind();

                var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                var cmd = new SqlCommand("PROC_GET_TICKETS", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@TICKETSTATUSID", SqlDbType.Int)).Value = DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.Int)).Value = DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.DateTime)).Value = DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.DateTime)).Value = DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@WorkOrderID", SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@BRANCHID", SqlDbType.Int)).Value = DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@BRANCHADMINID", SqlDbType.Int)).Value = DBNull.Value;
                connection.Open();
                var table = new DataTable();
                table.Load(cmd.ExecuteReader());
                connection.Close();

                grd_Tickets.DataSource = table;
                grd_Tickets.DataBind();

                var woinfoQuery = dataContext.WorkOrderHistories
                    .Where(woh => woh.WOID == id)
                    .Select(woh =>
                        new
                        {
                            woh.Governorate.GovernorateName,
                            woh.ServiceProvider.SPName,
                            woh.UpdateDate,
                            Reseller = woh.User.UserName,
                            woh.User1.UserName,
                            woh.Branch.BranchName,
                            Offer = woh.Offer == null ? "-" : woh.Offer.Title
                        }).ToList();
                grd_Info.DataSource = woinfoQuery;
                grd_Info.DataBind();
                var first = query.FirstOrDefault();
                if (first != null)
                {
                    SpanInstallTime.InnerHtml = first.InstallationTime;
                    SpanInstaller.InnerHtml = first.Installer;
                    lbl_BranchName.Text = first.BranchName;
                    lbl_Client_UserName.Text = first.UserName;
                    lbl_CustomerAddress.Text = first.CustomerAddress;
                    lbl_CustomerEmail.Text = first.CustomerEmail;
                    lbl_Mobil2.Text = first.CustomerMobile2;
                    lbl_CustomerMobile.Text = first.CustomerMobile;
                    lbl_CustomerName.Text = first.CustomerName;
                    lbl_CustomerPhone.Text = first.CustomerPhone;
                    lbl_ExtraGigas.Text = first.ExtraGigas;
                    lbl_GovernorateName.Text = first.GovernorateName;
                    lbl_IpPackageName.Text = first.IpPackageName;
                    lbl_Password.Text = first.Password;
                    lbl_ResellerName.Text = first.reseller;
                    lbl_ServicePackageName.Text = first.ServicePackageName;
                    lbl_SPName.Text = first.SPName;
                    lbl_StatusName.Text = first.StatusName;
                    lbl_VCI.Text = first.VCI;
                    lbl_VPI.Text = first.VPI;
                    lbl_PaymentType.Text = first.PaymentTypeName;
                    lbl_Notes.Text = first.Notes;
                    l_central.Text = first.Central;
                    l_offer.Text = first.Title;
                    lPersonalId.Text = first.PersonalId;
                    LRouterSerial.Text = first.RouterSerial;
                    LRequestNumber.Text = first.RequestNumber;
                    lblWorkorderNumber.Text = first.WorkorderNumbers;
                    lblWorkorderDate.Text = first.WorkorderDate.ToString();
                    lblPortNumber.Text = first.PortNumber;
                    lblBlock.Text = first.BlockNumber;
                    lblDslam.Text = first.DslamNumbers;
                    lblLineOwner.Text = first.LineOwner;
                    lblprepaid.Text = first.Prepaid.ToString();
                    lblinstallationcost.Text = first.InstallationCost.ToString();
                    lblcontractingcost.Text = first.ContractingCost.ToString();
                    lblCreationDate.Text = first.CreationDate.ToString();
                    GridView1.DataSource = workOrderStatus;
                    GridView1.DataBind();
                    Grd_CallMessages.DataSource =
                        dataContext.CallMessages.Where(a => a.WorkOrder_Id == first.ID)
                            .Select(a => new {a.MessageText, a.User.UserName, a.Date});
                    Grd_CallMessages.DataBind();

                    var routerData =
                        dataContext2.RecieveRouters.Where(a => a.WorkOrderId == id).ToList().Select(a => new
                        {
                            a.Id,
                            a.RouterSerial,
                            a.Store.StoreName,
                            CompanyUserName = a.User != null ? a.User.UserName : "-",
                            CustomerUserName = a.User1 != null ? a.User1.UserName : "-",
                            CompanyDate = Convert.ToDateTime(a.CompanyProcessDate).ToShortDateString(),
                            CustomerDate =
                                a.CustomerProcessDate != null
                                    ? Convert.ToDateTime(a.CustomerProcessDate).ToShortDateString()
                                    : "",
                            Attach = a.Attachments != null ? string.Format("../Attachments/{0}", a.Attachments) : "",
                            Attach2 = a.Attachments2 != null ? string.Format("../Attachments/{0}", a.Attachments2) : "",
                            a.IsRecieved
                        });
                    GVRouter.DataSource = routerData;
                }
                GVRouter.DataBind();
                GVRequestDateHistory.DataSource =
                    dataContext.RequestDateHistories.Where(a => a.WorkOrderId == id).Select(a => new
                    {
                        a.WorkOrder.CustomerName,
                        a.User.UserName,
                        a.newRequestDate,
                        a.oldRequestDate,
                        a.ChangeDate
                    });
                GVRequestDateHistory.DataBind();

            }
        }

        protected void GVRouter_DataBound(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                foreach (GridViewRow row in GVRouter.Rows)
                {
                    var id = GVRouter.DataKeys[row.RowIndex];
                    if (id != null)
                    {
                        var routerid = Convert.ToInt32(id.Value);
                        var rowdata = context.RecieveRouters.FirstOrDefault(a => a.Id == routerid);
                        var btn1 = row.FindControl("link") as HtmlAnchor;
                        var btn2 = row.FindControl("link2") as HtmlAnchor;
                        if (rowdata != null && rowdata.Attachments != null && btn1 != null)
                        {
                            btn1.Visible = true;
                        }
                        if (rowdata != null && rowdata.Attachments2 != null && btn2 != null)
                        {
                            btn2.Visible = true;
                        }
                    }
                }
            }
        }

        private void Fillrouterdiv(WorkOrder query)
        {

            if (query == null) return;
            lblName.Text = query.CustomerName;
            lblPhone.Text = query.CustomerPhone;
            lblSerial.Text = query.RouterSerial;
            lblDate.Text = DateTime.Now.AddHours().Date.ToShortDateString();
            lblNationalNumber.Text = query.PersonalId;
        }

        private void FillRunOrder(IEnumerable<WorkOrder> orders)
        {
            var data = (orders.Select(a => new
            {
                a.CreationDate,
                a.CustomerPhone,
                a.CustomerName,
                a.LineOwner,
                a.CustomerMobile,
                a.CustomerAddress,
                a.ServicePackage.ServicePackageName,
                a.UserName,
                a.Password,
                Title = a.Offer == null ? "-" : a.Offer.Title,
                a.ServiceProvider.SPName,
                PersonalId = a.PersonalId ?? "-",
                Prepaid = a.Prepaid ?? 0,
                InstallationCost = a.InstallationCost ?? 0,
                ProviderId = a.ServiceProviderID ?? 0
            })).FirstOrDefault();
            if (data == null) return;
            switch (data.ProviderId)
            {
                case 8:

                    lblDateTEdata.Text = DateTime.Now.AddHours().Date.ToShortDateString();
                    lblADSLTEdata.Text = data.CustomerPhone;
                    lblNameTEdata.Text = data.CustomerName;
                    lblonerNameTEdata.Text = data.LineOwner;
                    lblMobileTEdata.Text = data.CustomerMobile;
                    lblAddressTEdata.Text = data.CustomerAddress;
                    lblSpeedTEdata.Text = data.ServicePackageName;
                    lblUserNameTEdata.Text = data.UserName;
                    lblPassTEdata.Text = data.Password;
                    lblOfferTEdata.Text = data.Title;
                    lblprePaidTeData.Text = data.Prepaid.ToString(CultureInfo.InvariantCulture);
                    lblInstallationfeesTedata.Text = Helper.FixNumberFormat(data.InstallationCost);
                    break;
                case 4:

                    lblDateEtisalat.Text = DateTime.Now.AddHours().Date.ToShortDateString();
                    lblADSLEtisalat.Text = data.CustomerPhone;
                    lblNameEtisalat.Text = data.CustomerName;
                    lblOwnerEtisalat.Text = data.LineOwner;
                    lblMobileEtisalat.Text = data.CustomerMobile;
                    lblAdreessEtisalat.Text = data.CustomerAddress;
                    lblSpeedEtisalat.Text = data.ServicePackageName;
                    lblUsernameEtisalat.Text = data.UserName;
                    lblPassEtisalat.Text = data.Password;
                    lblOfferEtisalat.Text = data.Title;
                    lblNationalIDEtisalat.Text = data.PersonalId;
                    lblPrePaidEtisalat.Text = data.Prepaid.ToString(CultureInfo.InvariantCulture);
                    lblInstallationFees.Text = Helper.FixNumberFormat(data.InstallationCost);
                    break;
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
        }


        protected void b2_OnClick(object sender, EventArgs e)
        {

            var attachment = string.Format("attachment; filename={0}_{1}.xls", "Search_Results", DateTime.Now.AddHours());
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grd_wo.RenderControl(htw);
            Response.Write(sw.ToString());

            Response.End();

        }


        private void Activate()
        {
            GvNotes.DataBound += (o, e) => Helper.GridViewNumbering(GvNotes, "LNo");
            grd_Tickets.DataBound += (o, e) => PopulateTicketDays();
            Grd_CallMessages.DataBound += (o, e) => Helper.GridViewNumbering(Grd_CallMessages, "LNo");
            GVRequestDateHistory.DataBound += (o, e) => Helper.GridViewNumbering(GVRequestDateHistory, "LNo");
        }


        private void PopulateTicketDays()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                foreach (GridViewRow row in grd_Tickets.Rows)
                {
                    var literal = row.FindControl("DaysCount") as Literal;
                    if (literal == null) return;
                    var tickectId = Convert.ToInt32(literal.Text);
                    var ticket = context.Tickets.FirstOrDefault(t => t.ID == tickectId);
                    if (ticket == null)
                    {
                        literal.Text = "";
                        return;
                    }
                    var comment =
                        context.TicketComments.Where(x => x.TicketID == tickectId)
                            .OrderByDescending(x => x.CommentDate.Value)
                            .FirstOrDefault();
                    if (ticket.TicketDate != null && comment != null && comment.CommentDate != null)
                    {
                        literal.Text = string.Format("{0}",
                            (comment.CommentDate.Value.Date - ticket.TicketDate.Value.Date).Days);
                    }
                    if (ticket.TicketDate != null && (comment == null || comment.CommentDate == null))
                    {
                        literal.Text = string.Format("{0}",
                            (DateTime.Now.AddHours().Date - ticket.TicketDate.Value.Date).Days);
                    }
                }
            }
        }


        protected void linkRunOrderTEdata_Click(object sender, EventArgs e)
        {
            RunOrderTEData.Visible = RunOrderTEData.Visible == false;
        }


        protected void linkRunOrderEtisalate_Click(object sender, EventArgs e)
        {
            RunOrderEtisalat.Visible = RunOrderEtisalat.Visible == false;
        }


        protected void linkRouter_Click(object sender, EventArgs e)
        {
            router.Visible = router.Visible == false;
        }

        private void Bind_ddl_ServicePackage(WorkOrder orderId, ISPDataContext db4)
        {
            // var packages = db4.ServicePackages.Where(x => x.ProviderId == orderId.ServiceProviderID.Value && x.Active.Value).Select(sp => sp);
            IQueryable<ServicePackage> packages;
            if (orderId.OfferId == null)
            {

                packages =
                    db4.ServicePackages.Where(x => x.ProviderId == orderId.ServiceProviderID.Value).Select(sp => sp);
            }
            else
            {
                packages =
                    db4.OfferProviderPackages.Where(a => a.OfferId == orderId.OfferId).Select(sp => sp.ServicePackage);

            }

            packages = packages.Where(x => x.Active == true && x.ID != orderId.ServicePackageID);


            ddl_ServicePackage.DataSource = packages;
            ddl_ServicePackage.DataBind();
            Helper.AddDefaultItem(ddl_ServicePackage);
        }

        private void Bind_ddl_IpPackage(ISPDataContext db6)
        {
            var query = db6.IpPackages.Select(ip => ip);
            ddl_IpPackage.DataSource = query;
            ddl_IpPackage.DataBind();

        }

        private void PopulateExtraGigas()
        {

            _domian.PopulateExtraGigas(DdlExtraGigas);

        }

        private void PopulateOffers(ISPDataContext db9, int providerId = 0)
        {

            {
                var user = db9.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null) return;
                if (providerId.Equals(0))
                {
                    Helper.BindDrop(ddlOffers, null, "", "");

                    return;
                }
                var provider = db9.ServiceProviders.FirstOrDefault(p => p.ID.Equals(providerId));
                if (provider == null)
                {
                    Helper.BindDrop(ddlOffers, null, "", "");

                    return;
                }
                var offers = _domian.ProviderOffers(provider, user);
                Helper.BindDrop(ddlOffers, offers, "Title", "Id");
            }
        }

        public void AddRequest(int requestId)
        {
            
            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var woId = Convert.ToInt32(ViewState["ID"]);

                if (!IsValid) return;
                if (woId == 0) return;

                var orderRequests = db8.WorkOrderRequests
                    .Where(woreq => woreq.WorkOrderID == woId && woreq.RSID == 3);

                if (orderRequests.Any())
                {
                    lbl_InsertResult.Text = Tokens.User_Has_Request;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }

                var orders = db8.WorkOrders.Where(wo => wo.ID == woId
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

                if (firstOrder.WorkOrderStatusID == 11 && (requestId != 3 && requestId != 6))
                {
                    lbl_InsertResult.Text = Tokens.Duplicate_Request_Suspend;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }

                //حالة العرض غير مسموح له الخفض والرفع
                var worder = db8.WorkOrders.FirstOrDefault(z => z.ID == woId);
                var woQuery = db8.WorkOrders.Where(wo => wo.CustomerPhone == worder.CustomerPhone).Select(wo => new
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
                var canupdown = woQuery.First().CanUp;
                if (requestId == 1 && !canupdown)
                {
                    lbl_InsertResult.Text = Tokens.CantUpgradeDowngrade;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }


                // في حالة الساسبند يجب مرور شهر علي اعادة التفعيل
                 var option = OptionsService.GetOptions(db8, true);
                if (option != null && (option.PreventSuspendBeforeMonthFromReActive ?? false))
                {
                    if (firstOrder.WorkOrderStatusID == 6 && requestId == 2)
                    {

                        var woid = firstOrder.ID;
                        var req =
                            db8.WorkOrderRequests.Where(x => x.WorkOrderID == woid && x.RSID == 1 && x.RequestID == 7)
                                .ToList();
                        var lastreq = req.LastOrDefault();
                        if (lastreq != null && lastreq.ProcessDate != null &&
                            lastreq.ProcessDate.Value.AddMonths(1) > DateTime.Now)
                        {
                            lbl_InsertResult.Text = Tokens.CantSuspend;

                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }

                        //var workOrderRepository = new WorkOrderRepository();
                        //var activation = workOrderRepository.GetActivationDate(woid);
                        //if (activation != null)
                        //{
                        //    var activationmonth = activation.Value.AddMonths(1);
                        //    if (DateTime.Now < activationmonth)
                        //    {
                        //        lbl_InsertResult.Text = Tokens.cantcancel;

                        //        lbl_InsertResult.ForeColor = Color.Red;
                        //        return;
                        //    }
                        //}
                    }
                }

                //حالة الكانسليشن بروسس او كانسل ملوش غير ريأكتيف فقط
                if ((firstOrder.WorkOrderStatusID == 8 || firstOrder.WorkOrderStatusID == 9) && (requestId != 7))
                {
                    lbl_InsertResult.Text = Tokens.Cancelation_process_or_canceled_user;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }

                //حالة العميل يجب ان تكون اكتف عند عمل سسبند للعميل
                if (firstOrder.WorkOrderStatusID != 6 && requestId == 2)
                {
                    lbl_InsertResult.Text = Tokens.ToSuspendShouldbeActive;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }
                //حالة العميل يجب ان تكون اكتف عند عمل طلب خفض رفع
                if (firstOrder.WorkOrderStatusID != 6 && requestId == 1)
                {
                    lbl_InsertResult.Text = Tokens.ToUpgradeDowngradShouldbeActive;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }
                //حالة العميل يجب ان تكون اكتيف عند عمل طلب تعليق عميل Hold
                if (firstOrder.WorkOrderStatusID != 6 && requestId == 4)
                {
                    lbl_InsertResult.Text = Tokens.ToHoldShouldbeActive;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }
                //حالة العميل يجب ان تكون اكتف عند عمل طلب تغير اى بى
                if (firstOrder.WorkOrderStatusID != 6 && requestId == 8)
                {
                    lbl_InsertResult.Text = Tokens.ToaddIPShouldbeActive;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }
                // حالة العميل يجب ان تكون اكتف عند عمل طلب جيجات اضافية
                if (firstOrder.WorkOrderStatusID != 6 && requestId == 9)
                {
                    lbl_InsertResult.Text = Tokens.ToAddGigaShouldbeActive;
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }
                //حالة العميل يجب ان تكون اكتف عند تغير العرض 
                if (firstOrder.WorkOrderStatusID != 6 && requestId == 12)
                {
                    lbl_InsertResult.Text = @"لتغير العرض يلزم ان يكون العميل اكتف";
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }

                if (requestId == 2) //Suspend
                {
                    var order = db8.WorkOrders.FirstOrDefault(wo => wo.ID == woId
                                                                    && wo.WorkOrderStatusID == 11);

                    if (order != null)
                    {
                        lbl_InsertResult.Text = Tokens.Duplicate_Suspend2;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                }

                if (requestId == 3) //UnSuspended
                {
                    var query3 = db8.WorkOrders.Where(wo => wo.ID == woId
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
                        var demands = db8.Demands.Where(x => x.WorkOrderId == woId && x.Paid == false && x.Amount > 0).ToList();
                        if (demands.Count > 0)
                        {
                            lbl_InsertResult.Text = Tokens.CantUnsuspendCustomerHasIndebtedness;
                            lbl_InsertResult.ForeColor = Color.Red;
                            Div1.Visible = true;
                            Div1.InnerHtml = Tokens.CantUnsuspendCustomerHasIndebtedness;
                            Div1.Attributes.Add("class", "alert alert-danger");
                            return;
                        }
                    }
                }
                else if (requestId == 5) //Hold
                {
                    var query3 = db8.WorkOrders.Where(wo => wo.ID == woId
                                                            && (wo.WorkOrderStatusID == 10));
                    if (!query3.Any())
                    {
                        lbl_InsertResult.Text = Tokens.Cant_Unhold;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                }

                //Case Cancel requests change the work order state to cancelation process

                if (requestId == 6) //Cancel Request
                {
                    var query = from wo in db8.WorkOrders
                        where wo.ID == woId
                              &&
                              (wo.WorkOrderStatusID == 6 || wo.WorkOrderStatusID == 10 || wo.WorkOrderStatusID == 11)
                        //Active , suspended or hold
                        select wo;
                    if (query.Any())
                    {

                    }
                    else
                    {
                        lbl_InsertResult.Text = Tokens.Cant_Cancel;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }
                }
                //Case ReActive requests occur to cancelled or cancelation process

                if (requestId == 7) //ReActive Request
                {
                    var query = from wo in db8.WorkOrders
                        where wo.ID == woId
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
                    WorkOrderID = woId,
                    CurrentPackageID = Convert.ToInt32(firstOrder.ServicePackageID)
                };
                if (DdlExtraGigas.SelectedIndex > 0)
                {
                    wor.ExtraGiga =
                        db8.ExtraGigas.FirstOrDefault(x => x.Id == Convert.ToInt32(DdlExtraGigas.SelectedItem.Value));
                }

                wor.NewPackageID = wor.CurrentPackageID;

                //Suspended
                if (requestId == 1)
                {
                    wor.NewPackageID = Convert.ToInt32(ddl_ServicePackage.SelectedItem.Value);
                }
                if (requestId == 12)
                {
                    wor.NewOfferId = Convert.ToInt32(ddlOffers.SelectedItem.Value);
                }


                wor.RequestDate = Convert.ToDateTime(DateTime.Now.AddHours());
                wor.RequestID = Convert.ToInt32(requestId);
                wor.RSID = 3;

                if (requestId == 7)
                {
                    wor.Notes = notes.Value;
                }
                if (ddl_IpPackage.SelectedIndex > -1)
                {
                    wor.NewIpPackageID = Convert.ToInt32(ddl_IpPackage.SelectedItem.Value);
                }
                else
                {
                    var workOrder = firstOrder;
                    if (workOrder.IpPackage != null)
                    {
                        wor.NewIpPackageID = workOrder.IpPackageID;
                    }

                }




                //send suspend request to tedata
                if (requestId == 2)
                {
                    var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                    var woproviderList = db8.WorkOrders.FirstOrDefault(z => z.ID == woId);
                    if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                    {
                           external_data.Visible = true;
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
                                         Div1.Visible = true;
                                         Div1.InnerHtml = "هذا العميل موقوف بالفعل على البورتال";
                                         Div1.Attributes.Add("class", "alert alert-danger");
                                         return;
                                     }
                                     else
                                     {
                                         var worNote = Tedata.SendTedataSuspendRequest(username, cookiecon, pagetext);
                                         if (worNote == 2)
                                         {
                                             //فى حالة البورتال واقع
                                             Div1.Visible = true;
                                             Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                             Div1.Attributes.Add("class", "alert alert-danger");
                                             wor.Notes =
                                                 "لم يتم ارسال الطلب إلى البورتال بسبب عدم إستجابة البورتال";
                                             //ينزل الطلب معلق فى اى اس بى
                                         }
                                         else
                                         {
                                             //فى حالة نجاح الارسال الى البورتال ننزل الطلب متوافق علية فى اى اس بى
                                             wor.SenderID = Convert.ToInt32(Session["User_ID"]);
                                             wor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                             wor.ProcessDate = DateTime.Now.AddHours();
                                             wor.RSID = 1;
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

                                             Div1.Visible = true;
                                             Div1.InnerHtml = "تم إرسال الطلب الى البورتال بنجاح";
                                             Div1.Attributes.Add("class", "alert alert-success");
                                             lbl_InsertResult.Text = Tokens.Request_Added_successfully;
                                             lbl_InsertResult.ForeColor = Color.Green;
                                             return;
                                         }

                                     }

                                 }
                                 else
                                 {
                                     //فى حالة البورتال واقع
                                     Div1.Visible = true;
                                     Div1.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                     Div1.Attributes.Add("class", "alert alert-danger");
                                     wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                 }

                             }
                             else
                             {
                                 Div1.Visible = true;
                                 Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                 Div1.Attributes.Add("class", "alert alert-danger");
                                 wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                             }
                         }
                        else
                        {
                            Div1.Visible = true;
                            Div1.InnerHtml = "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                            Div1.Attributes.Add("class", "alert alert-danger");
                            wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                        }
                    }
                }

                //send Un suspend request to tedata
                if (requestId == 3)
                {
                    var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                    var woproviderList = db8.WorkOrders.FirstOrDefault(z => z.ID == woId);
                    if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                    {

                             external_data.Visible = true;
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
                                        Div1.Visible = true;
                                        Div1.InnerHtml = "هذا العميل مفعل بالفعل على البورتال";
                                        Div1.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                    else
                                    {
                                        var worNote = Tedata.SendTedataUnSuspendRequest(username, cookiecon, pagetext);
                                        if (worNote == 2)
                                        {
                                            Div1.Visible = true;
                                            Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                            Div1.Attributes.Add("class", "alert alert-danger");
                                            //فى حالة البورتال واقع
                                            wor.Notes =
                                                "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                            //ينزل الطلب معلق فى اى اس بى
                                        }
                                        else
                                        {
                                            //ينزل الطلب متوافق علية فى اى اس بى
                                            wor.SenderID = Convert.ToInt32(Session["User_ID"]);
                                            wor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                            wor.ProcessDate = DateTime.Now.AddHours();
                                            wor.RSID = 1;
                                            db8.WorkOrderRequests.InsertOnSubmit(wor);
                                            //db8.SubmitChanges();


                                            //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                            var current = db8.WorkOrders.FirstOrDefault(x => x.ID == wor.WorkOrderID);
                                            try
                                            {
                                                if (current != null)
                                                {
                                                    var susday = CountSuspenddays(current.ID);
                                                    var curDate = current.RequestDate.Value;
                                                    current.RequestDate = curDate.AddDays(susday);
                                                    db8.SubmitChanges();
                                                }
                                               
                                            }
                                            catch (Exception)
                                            {

                                                throw;
                                            }
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



                                            // ترحيل ايام السسبند
                                            int daysCount = _ispEntries.DaysForCustomerAtStatus(wor.ID, 11);
                                            //var option = OptionsService.GetOptions(db8, true);
                                            if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                            {
                                                wor.RequestDate.Value.AddDays(daysCount);
                                                _ispEntries.Commit();
                                            }

                                            db8.SubmitChanges();

                                            Div1.Visible = true;
                                            Div1.InnerHtml = "تم إرسال الطلب الى البورتال بنجاح";
                                            Div1.Attributes.Add("class", "alert alert-success");
                                            lbl_InsertResult.Text = Tokens.Request_Added_successfully;
                                            lbl_InsertResult.ForeColor = Color.Green;
                                            return;
                                        }
                                    }

                                }
                                else
                                {
                                    //فى حالة البورتال واقع
                                    Div1.Visible = true;
                                    Div1.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                    Div1.Attributes.Add("class", "alert alert-danger");
                                    wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                    
                                }

                            }
                            else
                            {
                                //فى حالة البورتال واقع
                                Div1.Visible = true;
                                Div1.InnerHtml = "فشل الأتصال بالبورتال";
                                Div1.Attributes.Add("class", "alert alert-danger");
                                wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                                
                            }
                        }
                        else
                        {
                            Div1.Visible = true;
                            Div1.InnerHtml = "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                            Div1.Attributes.Add("class", "alert alert-danger");
                            //فى حالة البورتال واقع
                            wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال";
                            //ينزل الطلب معلق فى اى اس بى 
                        }
                    }
                }

                wor.SenderID = Convert.ToInt32(Session["User_ID"]);
                db8.WorkOrderRequests.InsertOnSubmit(wor);
                db8.SubmitChanges();
                lbl_InsertResult.Text = Tokens.Request_Added_successfully;
                lbl_InsertResult.ForeColor = Color.Green;

            }
        }

        protected void Send(object sender, EventArgs e)
        {
            var buttonId = ((Button) sender).ID;
            var request = 0;
            switch (buttonId)
            {
                case "btnmodalupdown":
                    request = 1;
                    break;
                case "btnSuspend":
                    request = 2;
                    break;
                case "btnUnSuspend":
                    request = 3;
                    break;
                case "btnCancel":
                    request = 6;
                    break;
                case "btnReactive":
                    request = 7;
                    break;
                case "btnHold":
                    request = 4;
                    break;
                case "btnUnHold":
                    request = 5;
                    break;
                case "btnchangeIpModal":
                    request = 8;
                    break;
                case "btnExtraModal":
                    request = 9;
                    break;
                case "btnChangeOfferModal":
                    request = 12;
                    break;
            }



            AddRequest(request);
        }

        protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(GridView2, "lbl_No");
        }

        private void PopulateRequests(int workorderId, ISPDataContext context)
        {

            {
                var requests = DataLevelClass.GetNonCofirmedRequestsToUser(workorderId);
                if (requests.Count > 0)
                {
                    GridView2.DataSource = requests;
                    GridView2.DataBind();
                    var re = requests.First().RequestTypeId;
                    var requestType = context.Requests.FirstOrDefault(a => a.ID == Convert.ToInt32(re));
                    if (requestType != null)
                        lblrequest.Text = Tokens.Requests + @" ( " + requestType.RequestName + @" ) ";

                    if (requests.First().RequestTypeId == null)
                    {
                        GridView2.Visible = false;
                    }
                    else
                    {
                        var idInQueryString = Convert.ToInt32(requests.First().RequestTypeId);
                        var columns = GridView2.Columns;
                        GridHelper.HideAllColumns(columns);
                        var columnNames = new List<string>
                        {
                            "#",
                            Tokens.Customer,
                            Tokens.Phone,
                            Tokens.Governrate,
                            Tokens.Central,
                            Tokens.CurrentSpeed,
                            Tokens.Status,
                            Tokens.Provider,
                            Tokens.Reseller,
                            Tokens.Branch,
                            Tokens.SenderName,
                            Tokens.Activation_Date,
                            Tokens.Offer,
                            Tokens.Request_Date,
                            Tokens.PaymentType,
                            Tokens.Reject,
                        };
                        switch (idInQueryString)
                        {
                            // Upgrade-Downgrade
                            case 1:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.New_Service_Package,
                                    Tokens.CurrentSpeed
                                });
                                break;
                            case 3:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.SuspendDaysCount
                                });
                                break;
                            // Ip Package
                            case 8:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.New_IP_Package
                                });
                                break;

                            case 9:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.Extra_Gigas
                                });
                                break;
                        }

                        //sys admin, sys employee


                        GridHelper.ShowExactColumns(columns, columnNames);
                    }
                }
                else
                {
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblrequest.Text = Tokens.Requests;
                }
            }
        }

        protected void RejectSelectedRequest(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var orderRequestId = Convert.ToInt32(RejectedRequestId.Value);
                var workOrderRequest = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == orderRequestId);
                if (workOrderRequest != null && workOrderRequest.IsProviderRequest != false)
                {
                    RejectRequest(orderRequestId);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox",
                        "alert('لا يمكن الغاء  الطلب لان  تم ارسالة لمزود الخدمة ');", true);

                }
            }
        }

        private void RejectRequest(int orderRequestId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var workOrderRequest = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == orderRequestId);
                if (workOrderRequest == null) return;
                workOrderRequest.RSID = 2;
                var orderId = Convert.ToInt32(workOrderRequest.WorkOrderID);
                workOrderRequest.RejectReason = TbRejectReason.Text + " تم الغاء الطلب من قبل الموزع ";
                workOrderRequest.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                workOrderRequest.ProcessDate = DateTime.Now.AddHours();
                context.SubmitChanges();

                var workOrderStatuses =
                    context.WorkOrderStatus.Where(wost => wost.WorkOrderID == workOrderRequest.WorkOrderID).ToList();
                var statusId = workOrderStatuses.Last().StatusID;
                if (statusId != null)
                {
                    int lastStatusId = statusId.Value;
                    var currentWorkOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == workOrderRequest.WorkOrderID);
                    if (currentWorkOrder != null)
                    {
                        currentWorkOrder.WorkOrderStatusID = lastStatusId;
                        var option = OptionsService.GetOptions(context, true);
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestReject(currentWorkOrder, TbRejectReason.Text,
                                workOrderRequest.Request.RequestName, Convert.ToInt32(Session["User_ID"]));
                        }
                    }
                }
                context.SubmitChanges();
                lbl_InsertResult.Text = Tokens.RequestRejected;
                lbl_InsertResult.ForeColor = Color.Green;
                if (orderId != 0) PopulateRequests(orderId, context);

            }
        }

       
        protected void external_data_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (ViewState["ID"] == null)
                {
                  return;  
                }
                
                var id = Convert.ToInt32(ViewState["ID"]);
                var portalList = context.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                var woproviderList = context.WorkOrders.FirstOrDefault(z => z.ID == id);
                if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                {
                    List<Tedatalist> exList = new List<Tedatalist>();
                    List<Usagelist> usgelist = new List<Usagelist>();
                    var username = woproviderList.UserName;

                    CookieContainer cookiecon = new CookieContainer();
                    cookiecon = Tedata.Login();
                    if (cookiecon == null)
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml = "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    var pagetext = Tedata.GetSearchPage(username, cookiecon);
                    if (pagetext==null)
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml = "فشل الأتصال بالسيرفر";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        return;  
                    }
                    var searchPage = Tedata.CheckSearchPage(pagetext);
                    if (!searchPage)
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml = "رجاء تأكد من حقل (User Name)";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    Tedatalist dList=new Tedatalist();
                    dList = Tedata.GetCustomerDetails(username, pagetext, cookiecon);

                    Usagelist usgeList= new Usagelist();
                    usgeList = Tedata.GetCustomerUsage(username, pagetext, cookiecon);

                    if (dList != null)
                    {
                        exList.Add(dList);
                        grd_externalData.Visible = true;
                        grd_externalData.DataSource = exList;
                        grd_externalData.DataBind();
                    }

                    if (usgeList != null)
                    {
                        usgelist.Add(usgeList);
                        grd_usage.Visible = true;
                        grd_usage.DataSource = usgelist;
                        grd_usage.DataBind();
                    }
                }
            }
        }

        bool IsDirectCustomer(int customerWId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var cust = context.WorkOrders.FirstOrDefault(w => w.ID == customerWId);
                if (cust != null && cust.ResellerID.HasValue)
                {
                    return false;
                }
            }
            return true;
        }

        protected void btnNotDirect_Click(object sender, EventArgs e)
        {
         var worId = Convert.ToInt32(ViewState["ID"]);
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var cust = context.WorkOrders.FirstOrDefault(w => w.ID == worId);
                var reseller = context.Users.FirstOrDefault(w => w.ID == cust.ResellerID);
                if (reseller==null)
                {
                    return;
                }
                Resellerlist lreseller = new Resellerlist()
                {
                    Name = reseller.UserName,
                    Phone = reseller.UserPhone,
                    Mobile = reseller.UserMobile,
                    Email = reseller.UserEmail,
                    Address = reseller.UserAddress
                };
                List<Resellerlist> resellerlist=new List<Resellerlist>();
                resellerlist.Add(lreseller);
                grd_resellerData.Visible = true;
                grd_resellerData1.DataSource = resellerlist;
                grd_resellerData1.DataBind();
            }
        }

        protected void grd_wo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = "<b>" + e.Row.Cells[6].Text + "</b>";
                if (e.Row.Cells[6].Text.Trim() == "<b>Active</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.Green;
                }
                if (e.Row.Cells[6].Text.Trim() == "<b>Suspend</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.OrangeRed;
                }
                if (e.Row.Cells[6].Text.Trim() == "<b>Cancelled</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.Red;
                }
                if (e.Row.Cells[6].Text.Trim() == "<b>Cancellation Process</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.Red;
                }
            }
        } 
        protected int CountSuspenddays(int workorderId)
            {
                var ispEntries = new IspEntries();
                return ispEntries.DaysForCustomerAtStatus(workorderId, 11);
            }
    }
    public class Resellerlist
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
   
}