using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AddNewCustomer : CustomPage
    {
        
            readonly ICentralRepository _centralRepository;
            public bool ActiveValidation { get; set; }
            //readonly IspDomian _domian;

            //readonly PackagesRepository _packagesRepository;




            public AddNewCustomer()
            {
                _centralRepository = new LCentralRepository();
                //var context = IspDataContext;
                //_packagesRepository = new PackagesRepository(context);
                //_domian = new IspDomian(context);
            }


            protected void Page_Load(object sender, EventArgs e)
            {
                if (Session["User_ID"] == null)
                {
                    Response.Redirect("~/Pages/default.aspx");
                    return;
                }
                if (Request.QueryString["t"] != null && !string.IsNullOrEmpty(Request.QueryString["t"]) &&
                    Request.QueryString["t"] == "t") Response.Redirect("~/pages/TestCustomer.aspx");
                if (IsPostBack) return;
                //mpe_Receipt.Hide();
                packs.Value = "";
                //Session["FilesList"] = null; //clear List
                UserFile1.Woid = 0;

                Helper.AddDefaultItem(ddl_centrals);
                Helper.AddTextBoxesText(new[] { TbContractingCost, TbInstallationCost, TbPrepaid }, "0");
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    Check_Auth(context);
                    Bind_ddl_ServiceProvider(context);
                    Bind_ddl_ServicePackage(context);
                    Bind_ddl_IpPackage(context);
                    PopulateBranches();
                    PopulateResellers(context);
                    Bind_ddl_CustomerStatus(context);
                    Bind_ddl_Governorates(context);
                    Bind_ddl_PaymentType(context);
                    PopulateOffers(context);
                    TbCreationDate.Text = DateTime.Now.AddHours().ToShortDateString();
                    txt_WorkorderDate.Text = DateTime.Now.AddHours().ToShortDateString();
                    var domian = new IspDomian(context);
                    var user = domian.User(Convert.ToInt32(Session["User_ID"]));
                    if (user != null && user.GroupID != null && user.GroupID == 6)
                    {
                        txt_CustomerEmail.Text = user.UserEmail;
                        TbCreationDate.Enabled = false;
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["p"])) txt_CustomerPhone.Text = Request.QueryString["p"];
                    if (string.IsNullOrEmpty(Request.QueryString["g"])) return;
                    ddl_Governorates.SelectedValue = Request.QueryString["g"];
                    PopulateCentrals();
                    var option = OptionsService.GetOptions(context, true);
                    ActiveValidation = Convert.ToBoolean(option.ValidationOnCustomerPhone);
                }
            }


            void Check_Auth(ISPDataContext context2)
            {
                var simpleFlag = false;
                var fullFlag = false;
                var users = GetGroup(context2);
                var groupIdObject = users.First().GroupID;
                if (groupIdObject != null)
                {
                    var groupId = groupIdObject.Value;
                    var groupPrivileges = context2.GroupPrivileges.Where(gp => gp.Group.ID == groupId);
                    foreach (GroupPrivilege groupPrivilege in groupPrivileges)
                    {
                        if (groupPrivilege.privilege.Name == "Simple Details" || groupPrivilege.privilege.Name == "All")
                        {
                            simpleFlag = true;
                        }
                        if (groupPrivilege.privilege.Name == "Full Details" || groupPrivilege.privilege.Name == "All")
                        {
                            fullFlag = true;
                        }
                    }
                }
                tr_SimpleDetails.Visible = simpleFlag;
                tr_FullDetails.Visible = fullFlag;
                btn_Add.Visible = simpleFlag || fullFlag;
            }


            IQueryable<User> GetGroup(ISPDataContext dataContext)
            {
                return dataContext.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
            }


            void Bind_ddl_ServiceProvider(ISPDataContext db)
            {
                var user = GetGroup(db).FirstOrDefault();
                if (user == null) return;
                List<ServiceProvider> providers;
                if (user.GroupID != null && user.GroupID == 6)
                {
                    var serviceProviders = db.UserProviders.Where(x => x.UserId == user.ID).Select(x => x.ServiceProvider).ToList();
                    providers = serviceProviders.Where(x => x.Active == null || x.Active.Value).ToList();
                }
                else
                    providers = db.ServiceProviders.Where(x => x.Active == null || x.Active.Value).Select(sp => sp).ToList();

                ddl_ServiceProvider.DataSource = providers;
                ddl_ServiceProvider.DataBind();
                Helper.AddDefaultItem(ddl_ServiceProvider, Tokens.Chose);
            }



            private void Bind_ddl_IpPackage(ISPDataContext db)
            {
                var query = db.IpPackages.Select(ip => ip);
                ddl_IpPackage.DataSource = query;
                ddl_IpPackage.DataBind();
            }


            void PopulateResellers(ISPDataContext db1)
            {
                if (string.IsNullOrWhiteSpace(ddl_Branch.SelectedValue))
                {
                    ClearResellersDropItems();
                    Helper.AddDefaultItem(ddl_Reseller, Tokens.DirectCustomer);
                }
                else
                {

                    var branchId = Convert.ToInt32(ddl_Branch.SelectedItem.Value);
                    var resellers = db1.Users.Where(x => x.GroupID == 6 && x.BranchID == branchId && x.IsAccountStopped != true).ToList();
                    //var resellers = DataLevelClass.GetUserReseller();
                    Helper.PopulateDrop(resellers, ddl_Reseller, "ID", "UserName", true, Tokens.DirectCustomer);
                }
            }


            void ClearResellersDropItems()
            {
                Helper.EmptyDrop(ddl_Reseller);
            }


            void PopulateBranches()
            {
                var query = DataLevelClass.GetUserBranches();
                ddl_Branch.DataSource = query;
                ddl_Branch.DataBind();
            }


            private void Bind_ddl_CustomerStatus(ISPDataContext db2)
            {
                var stat = new[] { "New Customer", "Pending TE", "Pending WO", "Pending Splitting", "Pending Installation" };
                var list =
                    (from item in stat where item != null select db2.Status.FirstOrDefault(s => s.StatusName == item)).ToList();
                ddl_CustomerStatus.DataSource = list;
                ddl_CustomerStatus.DataBind();
            }



            void Bind_ddl_Governorates(ISPDataContext db3)
            {
                var query = db3.Governorates.Select(gov => gov);
                ddl_Governorates.DataSource = query;
                ddl_Governorates.DataBind();
                Helper.AddDefaultItem(ddl_Governorates);
            }


            void Bind_ddl_PaymentType(ISPDataContext db4)
            {

                var query = db4.PaymentTypes.Select(paymenttp => paymenttp);
                ddl_PaymentType.DataSource = query;
                ddl_PaymentType.DataBind();
                Helper.AddDefaultItem(ddl_PaymentType);

                var first = db4.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
                if (first == null)
                    return;
                int dataLevel = first.Value;
                if (dataLevel != 3)
                    return;
                ddl_PaymentType.SelectedValue = "1";
                ddl_PaymentType.Enabled = false;
            }


            protected void btn_Add_Click(object sender, EventArgs e)
            {
                if (txt_CustomerPhone.Text.Length <= 0) return;
                var literal = txt_CustomerPhone.Text;
                if (literal[0] == '0')
                {
                    Literal1.Text = string.Format("{0}", "Can't start with \"0\"");
                    txt_CustomerPhone.Focus();
                    txt_CustomerPhone.BackColor = Color.Red;
                    ViewState.Add("PhoneError", true);
                    return;
                }

                bool phoneError;

                if (ViewState["PhoneError"] == null) phoneError = false;
                else phoneError = (bool)ViewState["PhoneError"];
                if (phoneError)
                {
                    lbl_InsertResult.Text = string.Format("{0}", "Phone number already in use or number starts with \"0\"");
                    lbl_InsertResult.ForeColor = Color.Red;
                    return;
                }
                // options validatiom = true
                using (var db5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var check = db5.Options.Select(x => x.ValidationOnCustomerPhone).FirstOrDefault();
                    if (check == true)
                    {
                        if (txt_CustomerPhone.Text.Length < 8)
                        {
                            lbl_InsertResult.Text = @"عدد ارقام التليفون يجب الا يقل عن 8 ارقام";
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                using (var db5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if ((ddl_Governorates.SelectedItem.ToString() == "--Chose--")) return;
                    if (ddl_Governorates.SelectedIndex < 1) return;
                    var customer = db5.WorkOrders.Where(x => x.CustomerPhone.Equals(literal) && x.CustomerGovernorateID == Convert.ToInt32(ddl_Governorates.SelectedItem.Value)).ToList();
                    if (customer.Any())
                    {
                        lbl_InsertResult.Text = Tokens.CustomerExists;
                        lbl_InsertResult.ForeColor = Color.Red;
                        return;
                    }

                    var uploadOption = db5.Options.FirstOrDefault();
                    if (uploadOption != null && uploadOption.UploadFielsToNewCustomer)
                    {
                        if (UserFile1.FilesList.Count == 0)
                        {
                            lbl_InsertResult.Text = Tokens.UploadFile;
                            lbl_InsertResult.ForeColor = Color.Red;
                            return;
                        }
                    }

                    var now = DateTime.Now.AddHours();
                    var cRDate = Convert.ToDateTime(TbCreationDate.Text);
                    var wo = new WorkOrder
                    {
                        PersonalId = TbPersonnalId.Text,
                        CustomerName = txt_CustomerName.Text.Trim(),
                        CustomerGovernorateID = Convert.ToInt32(ddl_Governorates.SelectedItem.Value),
                        CustomerPhone = txt_CustomerPhone.Text.Trim(),
                        CustomerAddress = txt_CustomerAddress.Text.Trim(),
                        CustomerMobile = txt_CustomerMobile.Text.Trim(),
                        CustomerEmail = txt_CustomerEmail.Text.Trim(),
                        ServiceProviderID = Convert.ToInt32(ddl_ServiceProvider.SelectedItem.Value),
                        ServicePackageID = Convert.ToInt32(ddl_ServicePackage.SelectedItem.Value),
                        IpPackageID = Convert.ToInt32(ddl_IpPackage.SelectedItem.Value),
                        PaymentTypeID = Convert.ToInt32(ddl_PaymentType.SelectedItem.Value),
                        BranchID = Convert.ToInt32(ddl_Branch.SelectedItem.Value),
                        VPI = txt_VPI.Text.Trim(),
                        VCI = txt_VCI.Text.Trim(),
                        UserName = txt_UserName.Text.Trim(),
                        Password = txt_Password.Text,
                        Notes = txt_Notes.Text.Trim(),
                        WorkOrderStatusID = Convert.ToInt32(ddl_CustomerStatus.SelectedItem.Value),
                        CreationDate = new DateTime(cRDate.Year, cRDate.Month, cRDate.Day,now.Hour,now.Minute,now.Second),
                        CentralId = Convert.ToInt32(ddl_centrals.SelectedItem.Value),
                        RouterSerial = TbRouterSerail.Text,
                        RequestNumber = TbRequestNumber.Text,
                        CustomerMobile2 = txt_CustomerMobile2.Text.Trim(),
                        LineOwner = TbLineOwner.Text,
                        WorkorderNumbers = txt_WorkorderNumber.Text,
                        WorkorderDate = Convert.ToDateTime(txt_WorkorderDate.Text),
                        PortNumber = TbPortNumber.Text,
                        DslamNumbers = TbDslamNumber.Text,
                        BlockNumber = txt_BlockNumber.Text
                    };
                    if (packs.Value == "show") return;
                    if (ddl_offer.SelectedIndex > 0) wo.OfferId = Convert.ToInt32(ddl_offer.SelectedItem.Value);
                    if (!tr_FullDetails.Visible) wo.ResellerID = Convert.ToInt32(Session["User_ID"]);
                    else
                    {
                        if (ddl_Reseller.SelectedIndex > 0)
                            wo.ResellerID = Convert.ToInt32(ddl_Reseller.SelectedItem.Value);
                    }



                    db5.WorkOrders.InsertOnSubmit(wo);
                    try
                    {
                        db5.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message ==
                           "Violation of UNIQUE KEY constraint 'UQ_WorkOrders_PhoneAndGov'. Cannot insert duplicate key in object 'dbo.WorkOrders'.\r\nThe statement has been terminated.")
                        {
                            HttpContext.Current.Session["Error"] = "Warning : Customer already exsists, DON'T PRESS REFRESH .";
                            throw new Exception("Customer phone already EXIST !!");
                        }
                    }
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    IUserSaveRepository userSave = new UserSaveRepository();
                    var saves = userSave.SavesOfUser(userId, db5).FirstOrDefault();
                    if (!IsZeroOrEmpty(TbInstallationCost.Text))
                    {
                        var amount = Convert.ToDecimal(TbInstallationCost.Text);
                        wo.InstallationCost = amount;
                        var notes = "مصاريف التركيب عند اضافة عميل جديد" + Tokens.Customer_Name + " : " + wo.CustomerName + " - " +
                        Tokens.Customer_Phone + " : " + wo.CustomerPhone;

                        if (saves != null && saves.SaveId != null)
                        {
                            userSave.BranchAndUserSaves(Convert.ToInt32(saves.SaveId), userId, Convert.ToDouble(amount),
                                "مصاريف التركيب", notes, db5);
                        }
                        /*var demandFactory = new DemandFactory(db5);
                        var demand = demandFactory.CreateDemand(wo, now, now.AddMonths(1), amount, userId, now, true, "مصاريف التركيب");
                        demand.PaymentDate = DateTime.Now.AddHours();
                        db5.Demands.InsertOnSubmit(demand);
                        db5.SubmitChanges();*/

                    }
                    if (!IsZeroOrEmpty(TbContractingCost.Text))
                    {
                        var amount = Convert.ToDecimal(TbContractingCost.Text);
                        wo.ContractingCost = amount;
                        var notes = "مصاريف التعاقد عند اضافة عميل جديد" + Tokens.Customer_Name + " : " + wo.CustomerName + " - " +
                        Tokens.Customer_Phone + " : " + wo.CustomerPhone;

                        if (saves != null && saves.SaveId != null)
                        {
                            userSave.BranchAndUserSaves(Convert.ToInt32(saves.SaveId), userId, Convert.ToDouble(amount),
                                "مصاريف التعاقد", notes, db5);
                        }
                        /*var demandFactory = new DemandFactory(db5);
                        var demand = demandFactory.CreateDemand(wo, now, now.AddMonths(1), amount, userId, now, true, "مصاريف التعاقد");
                        demand.PaymentDate = DateTime.Now.AddHours();
                        db5.Demands.InsertOnSubmit(demand);
                        db5.SubmitChanges();*/
                    }
                    if (!IsZeroOrEmpty(TbPrepaid.Text))
                    {
                        var prepaid = Convert.ToDecimal(TbPrepaid.Text);
                        wo.Prepaid = prepaid;
                        //Savestepsinsaves(wo.CustomerName, wo.CustomerPhone, Convert.ToDouble(prepaid));
                        var notes = "المدفوع مقدما عند اضافة عميل جديد" + Tokens.Customer_Name + " : " + wo.CustomerName + " - " +
                                    Tokens.Customer_Phone + " : " + wo.CustomerPhone;
                        if (saves != null && saves.SaveId != null)
                        {
                            userSave.BranchAndUserSaves(Convert.ToInt32(saves.SaveId), userId, Convert.ToDouble(prepaid),
                                "مدفوع مقدما", notes, db5);

                        }
                        //Todo: make new method for creating demand for not connecting to credit - ashraf and try catch here
                        var demandFactory = new DemandFactory(db5);
                        var demand = demandFactory.firstCreateDemand(wo, now, now.AddMonths(1), prepaid, userId, now, true, "مدفوع مقدما");
                        demand.PaymentDate = DateTime.Now.AddHours();
                        demand.IsResellerCommisstions = true;
                        db5.Demands.InsertOnSubmit(demand);
                        db5.SubmitChanges();
                    }
                    var ws = new global::Db.WorkOrderStatus
                    {
                        WorkOrderID = wo.ID,
                        StatusID = Convert.ToInt32(ddl_CustomerStatus.SelectedItem.Value),
                        UserID = userId,//Convert.ToInt32(Session["User_ID"]),
                        UpdateDate = now
                    };
                    db5.WorkOrderStatus.InsertOnSubmit(ws);
                    db5.SubmitChanges();
                    if (uploadOption != null && uploadOption.UploadFielsToNewCustomer) SaveFiles(wo.ID, db5); else if (UserFile1.FilesList.Count > 0) SaveFiles(wo.ID, db5);

                    //var mobile = wo.CustomerMobile;

                    Session.Add("Order", wo);
                    Response.Redirect("TestCustomer.aspx?s=s&fullData=" + tr_FullDetails.Visible);
                    lbl_InsertResult.Text = Tokens.CustomerAddedSuccessfully;
                    ClearControls(this);
                    //Session["FilesList"] = null;

                }
            }


            #region save in branch save
         
            #endregion

            static bool IsZeroOrEmpty(string prepaid)
            {
                return string.IsNullOrEmpty(prepaid) || prepaid.Equals("0");
            }


            void ClearControls(Control ctr)
            {
                if (!ctr.HasControls())
                {
                    var box = ctr as TextBox;
                    if (box != null) box.Text = string.Empty;
                }
                else
                {
                    foreach (Control tmpctr in ctr.Controls)
                    {
                        if (tmpctr.HasControls()) ClearControls(tmpctr);
                        else
                        {
                            var box = tmpctr as TextBox;
                            if (box != null) box.Text = string.Empty;
                        }
                    }
                }
                TbCreationDate.Text = DateTime.Now.AddHours().ToShortDateString();
            }
            protected void txt_CustomerPhone_TextChanged(object sender, EventArgs e)
            {
                if (ddl_Governorates.SelectedIndex < 1) return;
                if (txt_CustomerPhone.Text.Length > 0)
                {
                    var literal = txt_CustomerPhone.Text;
                    if (literal[0] == '0')
                    {
                        Literal1.Text = string.Format("{0}", "Can't start with \"0\"");
                        txt_CustomerPhone.Focus();
                        txt_CustomerPhone.BackColor = Color.Red;
                        ViewState.Add("PhoneError", true);
                    }
                    else
                    {
                        txt_CustomerPhone.BackColor = Color.FromArgb(180, 224, 124);
                        Literal1.Text = string.Empty;
                        ViewState.Add("PhoneError", false);
                    }
                }
                //Todo:Commented  by ashraf
                /*if (txt_CustomerPhone.Text.Length < 8)
                {
                    Literal1.Text = string.Format("{0}", "رقم التليفون يجب الا  يقل عن 8 ارقام");
                    txt_CustomerPhone.Focus();
                    txt_CustomerPhone.BackColor = Color.Red;
                    ViewState.Add("PhoneError", true);
                }*/
                if ((ddl_Governorates.SelectedItem.ToString() == "--Chose--")) return;
                using (var db7 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = db7.WorkOrders.Where(wo => wo.CustomerPhone == txt_CustomerPhone.Text.Trim()
                                                                && wo.CustomerGovernorateID == Convert.ToInt32(ddl_Governorates.SelectedItem.Value));
                    if (query.Any())
                    {
                        txt_CustomerPhone.BackColor = Color.Red;
                        ViewState.Add("PhoneError", true);
                    }
                    else
                    {
                        txt_CustomerPhone.BackColor = Color.FromArgb(180, 224, 124);
                        ViewState.Add("PhoneError", false);
                    }
                }
            }


            private void SaveFiles(int woid, ISPDataContext db8)
            {
                UserFile1.Woid = woid;
                var fl = UserFile1.FilesList;
                if (fl == null) return;
                foreach (WorkOrderFile tempwof in fl) tempwof.WorkOrderID = woid;
                var query = db8.WorkOrderFiles.Where(wof => wof.WorkOrderID == woid);
                db8.WorkOrderFiles.DeleteAllOnSubmit(query);
                db8.SubmitChanges();
                db8.WorkOrderFiles.InsertAllOnSubmit(fl);
                db8.SubmitChanges();
            }


            protected void ddl_Governorates_SelectedIndexChanged(object sender, EventArgs e)
            {
                PopulateCentrals();
            }


            void PopulateCentrals()
            {
                if ((ddl_Governorates.SelectedItem.ToString() == "--Chose--"))
                {
                    ddl_centrals.Items.Clear();
                    Helper.AddDefaultItem(ddl_centrals);
                    return;
                }
                var governateId = Convert.ToInt32(ddl_Governorates.SelectedItem.Value);
                var centrals = _centralRepository.Centrals.Where(c => c.GovernateId == governateId);
                ddl_centrals.DataSource = centrals;
                ddl_centrals.DataTextField = "Name";
                ddl_centrals.DataValueField = "Id";
                ddl_centrals.DataBind();
                Helper.AddDefaultItem(ddl_centrals);
            }


            protected void ddl_ServiceProvider_SelectedIndexChanged(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    packs.Value = "";
                    if (ddl_ServiceProvider.SelectedIndex < 1)
                    {
                        Bind_ddl_ServicePackage(context);
                        PopulateOffers(context);
                        return;
                    }
                    var providerId = Convert.ToInt32(ddl_ServiceProvider.SelectedItem.Value);
                    Bind_ddl_ServicePackage(context, providerId);
                    PopulateOffers(context, providerId);
                }
            }


            void Bind_ddl_ServicePackage(ISPDataContext context, int providerId = 0)
            {
                if (providerId == 0)
                {
                    ddl_ServicePackage.Items.Clear();
                    Helper.AddDefaultItem(ddl_ServicePackage, Tokens.Chose);
                    return;
                }
                var packagesRepository = new PackagesRepository(context);
                var packages = packagesRepository.ProviderPackages(providerId, true);
                ddl_ServicePackage.DataSource = packages;
                ddl_ServicePackage.DataBind();
                Helper.AddDefaultItem(ddl_ServicePackage, Tokens.Chose);
            }


            void PopulateOffers(ISPDataContext db9, int providerId = 0)
            {
                var user = db9.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null) return;
                if (providerId.Equals(0))
                {
                    Helper.BindDrop(ddl_offer, null, "", "");
                    packs.Value = string.Empty;
                    return;
                }
                var provider = db9.ServiceProviders.FirstOrDefault(p => p.ID.Equals(providerId));
                if (provider == null)
                {
                    Helper.BindDrop(ddl_offer, null, "", "");
                    packs.Value = string.Empty;
                    return;
                }
                var domian = new IspDomian(db9);
                var offers = domian.ProviderOffers(provider, user);
                Helper.BindDrop(ddl_offer, offers, "Title", "Id");
            }



          


           


            protected void ddl_offer_SelectedIndexChanged(object sender, EventArgs e)
            {
              
                ISPDataContext db9 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                if (ddl_offer.SelectedIndex<1)
                {
                //    var offers = db9.OfferProviderPackages.Select(z => z.ServicePackage).ToList();
                //Helper.BindDrop(ddl_ServicePackage, offers, "ServicePackageName", "ID");
                   
                        var providerId = Convert.ToInt32(ddl_ServiceProvider.SelectedItem.Value);
                        Bind_ddl_ServicePackage(db9, providerId);
                    
                }
                else{
                var id = Convert.ToInt32(ddl_offer.SelectedValue);

                var offers = db9.OfferProviderPackages.Where(z => z.OfferId == id).Where(x => x.ServicePackage.Active == null || x.ServicePackage.Active.Value).Select(z => z.ServicePackage).ToList();
                Helper.BindDrop(ddl_ServicePackage, offers, "ServicePackageName", "ID");
}

            }


            protected void BranchChanged(object sender, EventArgs e)
            {
                using (var db10 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    PopulateResellers(db10);
                }
            }

        }
    }
 