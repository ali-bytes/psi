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
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class EditCustomer : CustomPage
    {
        private readonly IspEntries _ispEntries;

        readonly ICentralRepository _centralRepository;

        //readonly ISPDataContext _dataContext;

        readonly IspDomian _domian;

      

        readonly PackagesRepository _packagesRepository;
        public bool CanFulledit { get; set; }
        public int GroupId { get; set; }

        public EditCustomer()
        {
             var dataContext = IspDataContext;
             _ispEntries = new IspEntries(dataContext);
            _centralRepository = new LCentralRepository();
          _packagesRepository = new PackagesRepository(dataContext);
            _domian = new IspDomian(dataContext);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var que = Request.QueryString["WOID"];
            if (!string.IsNullOrEmpty(que))
            {
                var id = QueryStringSecurity.Decrypt(que);
                var workOrderId = Convert.ToInt32(id);
                //Session["FilesList"] = populateFiles(workOrderID);
                PopulateFiles(workOrderId);

                //Session.Timeout = 90;
                Bind_ddl_ServiceProvider();
                Bind_ddl_ServicePackage();
                Bind_ddl_IpPackage();
                Bind_ddl_Reseller();
                Bind_ddl_Branch();
                Bind_ddl_CustomerStatus();
                Bind_ddl_Governorates();
                Bind_ddl_PaymentType();
                ProcessQueryString(workOrderId);
            }

            var myid = Request.QueryString["NID"];
            if (!string.IsNullOrEmpty(myid))
            {
                var workOrderId = Convert.ToInt32(myid);
                //Session["FilesList"] = populateFiles(workOrderID);
                PopulateFiles(workOrderId);

                //Session.Timeout = 90;
                Bind_ddl_ServiceProvider();
                Bind_ddl_ServicePackage();
                Bind_ddl_IpPackage();
                Bind_ddl_Reseller();
                Bind_ddl_Branch();
                Bind_ddl_CustomerStatus();
                Bind_ddl_Governorates();
                Bind_ddl_PaymentType();
                ProcessQueryString(workOrderId);
            }

            if (Session["User_ID"] == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            // Commected By ashraf
            //var edit = EditCustomers(userId);
            CanFulledit = _ispEntries.UserHasPrivlidge(userId, "FullCustomerEdit");
            

            //GroupId = edit.GroupId;
            if (!CanFulledit)
            {
                txt_CustomerPhone.Enabled = ddl_IpPackage.Enabled = ddl_ServicePackage.Enabled =
                    ddl_offers.Enabled = LOfferStart.Enabled = ddl_CustomerStatus.Enabled =txt_UserName.Enabled= false;
            }
        }


        /*public CanEditModel EditCustomers(int userId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var model = new CanEditModel();
                var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
                if (user == null)
                {
                    
                    model.GroupId = 0;
                    return model;
                }
                var id = user.GroupID;
                if (id == null)
                {
                    model.GroupId = 0;
                    
                    return model;
                }

                model.GroupId = id.Value;
               return model;
            }
        }*/


        void PopulateFiles(int woid)
        {
            //using(var _dataContext = new ISPDataContext()){
            //return _dataContext.WorkOrderFiles.Where(a => a.WorkOrderID == woid).ToList();
            UserFile1.Woid = woid;
            //}
        }
        void PopulateCentrals()
        {
            ddl_central.Items.Clear();
            if (string.IsNullOrEmpty(ddl_Governorates.SelectedValue))
            {
                ddl_central.Items.Clear();
                ddl_central.DataBind();
                return;
            }

            var governateId = Convert.ToInt32(ddl_Governorates.SelectedItem.Value);
            var centrals = _centralRepository.Centrals.Where(c => c.GovernateId == governateId);
            ddl_central.DataSource = centrals;
            ddl_central.DataTextField = "Name";
            ddl_central.DataValueField = "Id";
            ddl_central.DataBind();
        }


        void ProcessQueryString(int workOrderId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
               
               
                var query = context.WorkOrders.Where(wo => wo.ID == workOrderId);
                if (query.Any())
                {
                    var wo = query.First();
                    ViewState.Add("WorkOrder_ID", workOrderId);
                    txt_CustomerName.Text = wo.CustomerName;
                    ddl_Governorates.SelectedValue = wo.CustomerGovernorateID.ToString();
                    txt_CustomerPhone.Text = wo.CustomerPhone;
                    txt_CustomerAddress.Text = wo.CustomerAddress;
                    txt_CustomerMobile.Text = wo.CustomerMobile;
                    txt_CustomerEmail.Text = wo.CustomerEmail;
                    TbPersonnalId.Text = wo.PersonalId;
                    TbRouterSerail.Text = wo.RouterSerial;
                    TbRequestNumber.Text = wo.RequestNumber;
                    TbLineOwner.Text = wo.LineOwner;
                    txt_VPI.Text = wo.VPI;
                    txt_VCI.Text = wo.VCI;
                    txt_UserName.Text = wo.UserName;
                    txt_Password.Text = wo.Password;
                    txt_Notes.Text = wo.Notes;
                    txt_WorkorderNumber.Text = wo.WorkorderNumbers;
                    txt_WorkorderDate.Text = wo.WorkorderDate.ToString();
                    txt_PortNumber.Text = wo.PortNumber;
                    txt_BlockNumber.Text = wo.BlockNumber;
                    txt_DslamNumber.Text = wo.DslamNumbers;
                    txt_mobile2.Text = wo.CustomerMobile2;

                    Bind_ddl_ServiceProvider();
                    ddl_ServiceProvider.SelectedValue = wo.ServiceProviderID.ToString();
                    if (wo.ServiceProviderID != null)
                    {
                        Bind_ddl_ServicePackage(wo.ServiceProviderID.Value);
                        ddl_ServicePackage.SelectedValue = wo.ServicePackageID.ToString();
                    }
                    if (ddl_ServiceProvider.SelectedIndex < 1)
                    {
                        PopulateOffers();

                    }
                    var providerId = Convert.ToInt32(ddl_ServiceProvider.SelectedItem.Value);
                    PopulateOffers(providerId);
                    ddl_IpPackage.SelectedValue = wo.IpPackageID.ToString();
                    ddl_Reseller.SelectedValue = wo.ResellerID.ToString();
                    ddl_Branch.SelectedValue = wo.BranchID.ToString();
                    ddl_PaymentType.SelectedValue = wo.PaymentTypeID.ToString();

                    ddl_offers.SelectedValue = wo.Offer == null ? string.Empty : wo.Offer.Id.ToString(CultureInfo.InvariantCulture);
                    btn_Update.Enabled = true;
                    //btn_Delete.Enabled = true;

                    PopulateCentrals();
                    if (wo.CentralId != null)
                    {
                        AssignSelectedCentral(wo.CentralId.Value.ToString(CultureInfo.InvariantCulture));
                    }

                    var statusQuery =
                        context.WorkOrderStatus.Where(wos => wos.WorkOrderID == workOrderId).Select(wos => wos.ID);
                    var lastwos = statusQuery.Max();
                    var lastStatus = context.WorkOrderStatus.Where(st => st.ID == lastwos);
                    ddl_CustomerStatus.SelectedValue = lastStatus.First().StatusID.ToString();
                    UserFile1.Woid = wo.ID;
                    if (wo.OfferStart != null)
                    {
                        LOfferStart.Text = wo.OfferStart.Value.ToShortDateString();
                    }
                    else
                    {
                        LOfferStart.Text = "";
                    }
                }
                else
                {
                    btn_Update.Enabled = false;
                    //btn_Delete.Enabled = false;
                    lbl_InsertResult.Text = Tokens.CustomerDoesntExist;
                    lbl_InsertResult.ForeColor = Color.Red;
                }
            }
        }


        void Bind_ddl_ServiceProvider()
        {
            using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var providers = context1.ServiceProviders;
                Helper.BindDrop(ddl_ServiceProvider, providers, "SPName", "ID");
                //ddl_ServiceProvider.DataSource = providers;
                //ddl_ServiceProvider.DataBind();
                Helper.AddDefaultItem(ddl_ServiceProvider, Tokens.Chose);
            }
        }


        void Bind_ddl_ServicePackage(int providerId = 0)
        {
            if (providerId == 0)
            {
                ddl_ServicePackage.Items.Clear();
                Helper.AddDefaultItem(ddl_ServicePackage, Tokens.Chose);
                return;
            }

            var packages = _packagesRepository.ProviderPackages(providerId);
            packages = packages.ToList();//Where(x => x.Active==true).ToList();
            ddl_ServicePackage.DataSource = packages;
            ddl_ServicePackage.DataBind();
            Helper.AddDefaultItem(ddl_ServicePackage, Tokens.Chose);
        }


        void Bind_ddl_IpPackage()
        {
            using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var Query = from IP in context2.IpPackages
                            select IP;
                ddl_IpPackage.DataSource = Query;
                ddl_IpPackage.DataBind();
            }
        }


        void Bind_ddl_Reseller()
        {
            ddl_Reseller.SelectedValue = null;
            ddl_Reseller.Items.Clear();
            ddl_Reseller.Items.Add(new ListItem("--- Chose ---", "-1"));
            ddl_Reseller.AppendDataBoundItems = true;
            ddl_Reseller.DataSource = DataLevelClass.GetUserReseller();
            ddl_Reseller.DataBind();
            ddl_Reseller.SelectedValue = "-1";
        }


        void Bind_ddl_Branch()
        {
            ddl_Branch.DataSource = DataLevelClass.GetUserBranches();
            ddl_Branch.DataBind();
        }


        void Bind_ddl_CustomerStatus()
        {
            using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var query = context3.Status;
                ddl_CustomerStatus.DataSource = query;
                ddl_CustomerStatus.DataBind();
            }
        }


        void Bind_ddl_Governorates()
        {
            using (var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var query = context4.Governorates;
                ddl_Governorates.DataSource = query;
                ddl_Governorates.DataBind();
            }
        }


        void Bind_ddl_PaymentType()
        {
            using (var _context5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var Query = from paymenttp in _context5.PaymentTypes
                            select paymenttp;
                ddl_PaymentType.DataSource = Query;
                ddl_PaymentType.DataBind();
            }
        }


        protected void btn_Update_Click(object sender, EventArgs e)
        {
            using (var context6 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var workOrderID = Convert.ToInt32(ViewState["WorkOrder_ID"]);
                var query = context6.WorkOrders.Where(wor => wor.ID == workOrderID);
                var order = query.First();

                var history = new WorkOrderHistory();
                //Insert row in History
                if (
                    order.ResellerID.ToString() != ddl_Reseller.SelectedItem.Value
                    || order.CustomerGovernorateID.ToString() != ddl_Governorates.SelectedItem.Value
                    || order.ServiceProviderID.ToString() != ddl_ServiceProvider.SelectedItem.Value
                    || order.BranchID.ToString() != ddl_Branch.SelectedItem.Value
                    || order.OfferId.ToString() != ddl_offers.SelectedItem.Value)
                {
                    history.UserID = Convert.ToInt32(Session["User_ID"]);
                    history.UpdateDate = DateTime.Now.AddHours();
                    history.WOID = order.ID;

                    if (order.ResellerID.ToString() != ddl_Reseller.SelectedItem.Value)
                    {
                        if (order.ResellerID != null)
                        {
                            var id = order.ResellerID.Value;
                            history.ResellerID = id;
                        }
                    }
                    if (order.CustomerGovernorateID.ToString() != ddl_Governorates.SelectedItem.Value)
                    {
                        if (order.CustomerGovernorateID != null)
                        {
                            var id = order.CustomerGovernorateID.Value;
                            history.CustomerGovernorateID = id;
                        }
                    }
                    if (order.ServiceProviderID.ToString() != ddl_ServiceProvider.SelectedItem.Value)
                    {
                        if (order.ServiceProviderID != null)
                        {
                            var id = order.ServiceProviderID.Value;
                            history.ServiceProviderID = id;
                        }
                    }
                    if (order.BranchID.ToString() != ddl_Branch.SelectedItem.Value)
                    {
                        if (order.BranchID != null)
                        {
                            var id = order.BranchID.Value;
                            history.BranchID = id;
                        }
                    }
                    if (order.OfferId.ToString() != ddl_offers.SelectedItem.Value)
                    {
                        history.OfferId = order.OfferId;
                    }
                }
                order.PersonalId = TbPersonnalId.Text;
                order.CustomerName = txt_CustomerName.Text.Trim();
                order.CustomerGovernorateID = Convert.ToInt32(ddl_Governorates.SelectedItem.Value);
                order.CustomerPhone = txt_CustomerPhone.Text.Trim();
                order.CustomerAddress = txt_CustomerAddress.Text.Trim();
                order.CustomerMobile = txt_CustomerMobile.Text.Trim();
                order.CustomerEmail = txt_CustomerEmail.Text.Trim();
                order.ServiceProviderID = Convert.ToInt32(ddl_ServiceProvider.SelectedItem.Value);
                order.ServicePackageID = Convert.ToInt32(ddl_ServicePackage.SelectedItem.Value);
                order.IpPackageID = Convert.ToInt32(ddl_IpPackage.SelectedItem.Value);

                order.BranchID = Convert.ToInt32(ddl_Branch.SelectedItem.Value);
                order.PaymentTypeID = Convert.ToInt32(ddl_PaymentType.SelectedItem.Value);
                order.VPI = txt_VPI.Text.Trim();
                order.VCI = txt_VCI.Text.Trim();
                order.UserName = txt_UserName.Text.Trim();
                order.Password = txt_Password.Text;
                order.Notes = txt_Notes.Text.Trim();
                order.LineOwner = TbLineOwner.Text;
                order.WorkorderNumbers = txt_WorkorderNumber.Text;
                order.WorkorderDate = Convert.ToDateTime(txt_WorkorderDate.Text);
                order.PortNumber = txt_PortNumber.Text;
                order.BlockNumber = txt_BlockNumber.Text;
                order.DslamNumbers = txt_DslamNumber.Text;
                order.CustomerMobile2 = txt_mobile2.Text;

                if (ddl_Reseller.SelectedIndex > 0)
                {
                    order.ResellerID = Convert.ToInt32(ddl_Reseller.SelectedItem.Value);
                }
                else
                {
                    order.User = null;
                }
                order.WorkOrderStatusID = Convert.ToInt32(ddl_CustomerStatus.SelectedItem.Value);
                //todo: add drop and assign
                order.ExtraGigaId = 1;
                order.CentralId = Convert.ToInt32(ddl_central.SelectedItem.Value);
                if (ddl_offers.SelectedIndex > 0)
                {
                    order.OfferId = Convert.ToInt32(ddl_offers.SelectedItem.Value);
                }
                else
                {
                    order.Offer = null;
                }
                if (LOfferStart.Enabled && !string.IsNullOrEmpty(LOfferStart.Text))
                {
                    try
                    {
                        var date = Convert.ToDateTime(LOfferStart.Text);
                        order.OfferStart = date;
                    }
                    catch (Exception) { }
                }

                order.RouterSerial = TbRouterSerail.Text;
                order.RequestNumber = TbRequestNumber.Text;
                context6.SubmitChanges();

                SaveFiles(order.ID);


                var StatusQuery = from wos in context6.WorkOrderStatus
                                  where wos.WorkOrderID == workOrderID
                                  select wos.ID;
                var Lastwos = StatusQuery.Max();
                var LastStatus = from st in context6.WorkOrderStatus
                                 where st.ID == Lastwos
                                 select st;

                if (LastStatus.First().StatusID != Convert.ToInt32(ddl_CustomerStatus.SelectedItem.Value))
                {
                    var newwos = new global::Db.WorkOrderStatus
                    {
                        WorkOrderID = workOrderID,
                        StatusID = Convert.ToInt32(ddl_CustomerStatus.SelectedItem.Value),
                        UserID = Convert.ToInt32(Session["User_ID"]),
                        UpdateDate = DateTime.Now.AddHours()
                    };
                    context6.WorkOrderStatus.InsertOnSubmit(newwos);
                    context6.SubmitChanges();
                }

                context6.WorkOrderHistories.InsertOnSubmit(history);
                context6.SubmitChanges();

                //Session["FilesList"] = null;
                lbl_InsertResult.Text = Tokens.CustomerUpdatedSuccess;
                lbl_InsertResult.ForeColor = Color.Green;
                string q = txt_CustomerPhone.Text.Trim().ToString();
                //string sfor=string.Format("g={0}&pn={1}&by={2}&sm=s", QueryStringSecurity.Encrypt(govId), QueryStringSecurity.Encrypt(PhoneNumber.Value), QueryStringSecurity.Encrypt("0"));
                string sfor = string.Format("g={0}&pn={1}&by={2}&sm=s", QueryStringSecurity.Encrypt(ddl_Governorates.SelectedValue), QueryStringSecurity.Encrypt(txt_CustomerPhone.Text), QueryStringSecurity.Encrypt("0"));

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "if(!alert('تم تعديل العميل')) document.location = 'Search.aspx?g="+QueryStringSecurity.Encrypt(ddl_Governorates.SelectedValue)+"&pn="+QueryStringSecurity.Encrypt(txt_CustomerPhone.Text)+"&by="+QueryStringSecurity.Encrypt("0")+"&sm=s';", true);
           
            }
        }


        /*protected void btn_Delete_Click(object sender, EventArgs e){
            using(var context7 = new ISPDataContext()){
                var orderId = Convert.ToInt32(ViewState["WorkOrder_ID"]);
                var order = context7.WorkOrders.FirstOrDefault(w => w.ID == orderId);
                if(DeleteCustomer(context7, order)) return;
                lbl_InsertResult.Text = Tokens.CustomerDeletedSuccess;
                lbl_InsertResult.ForeColor = Color.Green;
                ViewState["WorkOrder_ID"] = null;
                btn_Update.Enabled = false;
                btn_Delete.Enabled = false;
                ClearControls(this);
            }
        }


        public bool DeleteCustomer(ISPDataContext dataContext, WorkOrder order){
            return _orderRepository.Delete(order.ID);
        }*/


        void SaveFiles(int woid)
        {
            using (var context8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                //UserFile1.WOID = WOID;
                var fl = UserFile1.FilesList;
                var fl2 = new List<WorkOrderFile>();
                foreach (WorkOrderFile tempwof in fl)
                {
                    tempwof.WorkOrderID = woid;
                    fl2.Add(new WorkOrderFile
                    {
                        FileName = tempwof.FileName,
                        VirtualName = tempwof.VirtualName,
                        WorkOrderID = tempwof.WorkOrderID,
                        Notes = tempwof.Notes
                    });
                }
                var query = context8.WorkOrderFiles.Where(wof => wof.WorkOrderID == woid);

                context8.WorkOrderFiles.DeleteAllOnSubmit(query);
                context8.SubmitChanges();
                context8.WorkOrderFiles.InsertAllOnSubmit(fl2);
                context8.SubmitChanges();
            }
        }


        protected void ddl_Governorates_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateCentrals();
        }


        void AssignSelectedCentral(string centralId)
        {
            ddl_central.SelectedValue = centralId;
        }


        protected void ddl_ServiceProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ServiceProvider.SelectedIndex < 1)
            {
                PopulateOffers(0);
                return;
            }
            var providerId = Convert.ToInt32(ddl_ServiceProvider.SelectedItem.Value);
            Bind_ddl_ServicePackage(providerId);
            PopulateOffers(providerId);
        }


        void PopulateOffers(int providerId = 0)
        {
            using (var context9 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (providerId.Equals(0))
                {
                    Helper.BindDrop(ddl_offers, null, "", "");
                    packs.Value = string.Empty;
                    return;
                }

                var provider = context9.ServiceProviders.FirstOrDefault(p => p.ID.Equals(providerId));
                if (provider == null)
                {
                    Helper.BindDrop(ddl_offers, null, "", "");
                    packs.Value = string.Empty;
                    return;
                }
                //var user = context9.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                //if(user == null)
                //    return;
                //var offers = _domian.ProviderOffers(provider, user);

                var packages = provider.ServicePackages.ToList();
                var offers = new List<Offer>();
                foreach (var package in packages)
                {
                    var offerProviderPackages = package.OfferProviderPackages.ToList();
                    foreach (var providerPackage in offerProviderPackages)
                    {
                        if (offers.Any(x => x.Id == providerPackage.OfferId))
                            continue;
                        offers.Add(providerPackage.Offer);
                    }
                }
                Helper.BindDrop(ddl_offers, offers, "Title", "Id");
            }
        }


        /*void IsPackageInOffer(){
            using(var _context10 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                if(ddl_ServicePackage.SelectedIndex < 1 || ddl_offers.SelectedIndex < 1) return;
                var package = _context10.ServicePackages.FirstOrDefault(x => x.ID.Equals(Convert.ToInt32(ddl_ServicePackage.SelectedItem.Value)));
                var offer = _context10.Offers.FirstOrDefault(x => x.Id.Equals(Convert.ToInt32(ddl_offers.SelectedItem.Value)));
                if(package == null || offer == null) return;
                foreach(var offerProviderPackage in offer.OfferProviderPackages)
                    if(offerProviderPackage.PackageId == package.ID){
                        packs.Value = "";
                        return;
                    }
                packs.Value = "show";
            }
        }*/


        protected void ddl_ServicePackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // IsPackageInOffer();
        }
    }
}
