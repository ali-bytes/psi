using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.OfferServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Reports : CustomPage
    {
    
   
    #region Repos
    readonly StringBuilder _query;

  
    readonly DemandFactory _demandFactory;

    readonly IspEntries _ispEntries;

    readonly IWorkOrderRepository _orderRepository;

    readonly PriceServices _priceServices;
    private readonly BranchCreditRepository _branchCreditRepository;
    private readonly IResellerCreditRepository _creditRepository;
    readonly RouterRepository _routerRepository = new RouterRepository();
    public bool EditCustomer { get; set; }
    #endregion

    public Reports()
    {
        _priceServices = new PriceServices();
        _ispEntries = new IspEntries();
        _demandFactory = new DemandFactory(new IspEntries());
        _query =
            //new StringBuilder("select WorkOrders.*,StatusName ,PaymentTypes.PaymentTypeName, Branches.BranchName, Users.UserName as ResellerName, ServicePackageName, GovernorateName, SPName from WorkOrders join dbo.Status on WorkOrders.WorkOrderStatusID = Status.ID join dbo.PaymentTypes on dbo.WorkOrders.PaymentTypeID=PaymentTypes.ID join dbo.Branches on dbo.WorkOrders.BranchID=Branches.ID left join dbo.Users on WorkOrders.ResellerID = Users.ID join ServiceProviders on WorkOrders.ServiceProviderID =ServiceProviders.ID join Governorates on WorkOrders.CustomerGovernorateID =Governorates.ID join ServicePackages on WorkOrders.ServicePackageID = ServicePackages.ID where status.ID=");
            new StringBuilder("select WorkOrders.ID,WorkOrders.ServiceProviderID,WorkOrders.CentralId,WorkOrders.CustomerGovernorateID,WorkOrders.ResellerID,WorkOrders.BranchID,WorkOrders.CustomerPhone ,WorkOrders.CustomerName ,WorkOrders.CreationDate,WorkOrders.Notes ,WorkOrders.UserName ,StatusName ,WorkOrders.RequestDate,WorkOrders.RequestNumber ,PaymentTypes.PaymentTypeName, Branches.BranchName, Users.UserName as ResellerName, ServicePackageName, GovernorateName, SPName, Offers.Title as OfferName, Centrals.Name as CentralName from WorkOrders join dbo.Status on WorkOrders.WorkOrderStatusID = Status.ID join dbo.PaymentTypes on dbo.WorkOrders.PaymentTypeID=PaymentTypes.ID join dbo.Branches on dbo.WorkOrders.BranchID=Branches.ID left join dbo.Users on WorkOrders.ResellerID = Users.ID join ServiceProviders on WorkOrders.ServiceProviderID =ServiceProviders.ID join Governorates on WorkOrders.CustomerGovernorateID =Governorates.ID join ServicePackages on WorkOrders.ServicePackageID = ServicePackages.ID FULL join dbo.Offers on  dbo.WorkOrders.OfferId = Offers.Id FULL join dbo.Centrals on  dbo.WorkOrders.CentralId = Centrals.Id where status.ID=");
            _orderRepository = new WorkOrderRepository();
            _branchCreditRepository = new BranchCreditRepository();
            _creditRepository = new ResellerCreditRepository();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Stopwatch sWatch =new Stopwatch();
        sWatch.Start();
        direction.InnerHtml = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "1" : "2";
        if (Session["User_ID"] == null) return;

        if (IsPostBack)
        {
           
            using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                CheckCanEdit(context2);
            }
            return;
        }

        if (Session["messagenew"] == "true")
            {
                lbl_QSStatus.Text = Tokens.OrdersApproved;
                lbl_QSStatus.ForeColor = Color.Green;
                Session["messagenew"] = " ";
            }
        else if (Session["messagenew"] == "false")
        {
            lbl_QSStatus.Text = Tokens.InncorrectLink;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = " ";
        }
        
      ProcessQuery();
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {     
            
            PopulateStatus(context);
            CheckCanEdit(context);
            var domian = new IspDomian(context);
            domian.PopulateResellerswithDirectUser(DdlReseller, true); 
            domian.PopulateBranches(DdlBranch, true);
            domian.PopulateGovernorates(DdlGovernorate);
            domian.PopulateCentrals(DdlCentral);

            PopulateProviders(domian,context);
        }
        sWatch.Stop();
        sWatch.ElapsedMilliseconds.ToString();
        sWatch.Reset();
    }
    protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_wo.PageIndex = e.NewPageIndex;
        ProcessQuery();
    }
    private void PopulateProviders(IspDomian domian, ISPDataContext db)
    {
        var userId = Convert.ToInt32(Session["User_ID"]);
        var user = db.Users.FirstOrDefault(a => a.ID == userId);
        if (user != null && user.GroupID == 6)
        {
            var providers = db.UserProviders.Where(a => a.UserId == userId).Select(x => new
            {
                x.ServiceProvider.SPName,
                x.ServiceProvider.ID,
            }).ToList();
            DdlSeviceProvider.DataSource = providers;
            DdlSeviceProvider.DataTextField = "SPName";
            DdlSeviceProvider.DataValueField = "ID";
            DdlSeviceProvider.DataBind();
            Helper.AddDefaultItem(DdlSeviceProvider);
        }
        else
        {
            domian.PopulateProviders(DdlSeviceProvider);
        }
    }

    void PopulateStatus(ISPDataContext context)
    {
        Helper.PopulateDrop(context.Status.Where(x => (x.ID >= 2 && x.ID <= 5) || x.ID == 7), DdlNewState, "ID",
            "StatusName", false);
    }


    protected DataTable Filter(DataTable table)
    {
        if (DdlReseller.SelectedIndex > 0)
        {
            var resellerFilter = new List<DataRow>();
            var reseller = Convert.ToInt32(DdlReseller.SelectedItem.Value);
            if (reseller > 0)
            {
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    var l = table.Rows[i]["ResellerID"].ToString();
                    if (string.IsNullOrEmpty(l))
                        resellerFilter.Add(table.Rows[i]);
                    else if (!Convert.ToInt32(table.Rows[i]["ResellerID"]).Equals(reseller)) 
                        resellerFilter.Add(table.Rows[i]);
                        
                }
                resellerFilter.ForEach(x => table.Rows.Remove(x));
            }
            else
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if(!string.IsNullOrEmpty(table.Rows[i]["ResellerID"].ToString()))
                        resellerFilter.Add(table.Rows[i]);
                }
                resellerFilter.ForEach(x=>table.Rows.Remove(x));
            }
        }

        if (DdlBranch.SelectedIndex > 0)
        {
            var branchFilter = new List<DataRow>();
            var branch = Convert.ToInt32(DdlBranch.SelectedItem.Value);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(table.Rows[i]["BranchID"].ToString()) &&
                    !Convert.ToInt32(table.Rows[i]["BranchID"]).Equals(branch))
                    branchFilter.Add(table.Rows[i]);
            }
            branchFilter.ForEach(x => table.Rows.Remove(x));
        }

        if (DdlGovernorate.SelectedIndex > 0)
        {
            var govFilter = new List<DataRow>();
            var gov = Convert.ToInt32(DdlGovernorate.SelectedItem.Value);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(table.Rows[i]["CustomerGovernorateID"].ToString()) &&
                    !Convert.ToInt32(table.Rows[i]["CustomerGovernorateID"]).Equals(gov))
                    govFilter.Add(table.Rows[i]);
            }
            govFilter.ForEach(x => table.Rows.Remove(x));
        }

        if (DdlCentral.SelectedIndex > 0)
        {
            var centralFilter = new List<DataRow>();
            var central = Convert.ToInt32(DdlCentral.SelectedItem.Value);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(table.Rows[i]["CentralId"].ToString()) &&
                    !Convert.ToInt32(table.Rows[i]["CentralId"]).Equals(central))
                    centralFilter.Add(table.Rows[i]);
            }
            centralFilter.ForEach(x => table.Rows.Remove(x));
        }
        if (DdlSeviceProvider.SelectedIndex > 0)
        {
            var providerFilter = new List<DataRow>();
            var provider = Convert.ToInt32(DdlSeviceProvider.SelectedItem.Value);
            for (var i = 0; i < table.Rows.Count; i++)
            {

                if (!string.IsNullOrWhiteSpace(table.Rows[i]["ServiceProviderID"].ToString()) &&
                    !Convert.ToInt32(table.Rows[i]["ServiceProviderID"]).Equals(provider))
                    providerFilter.Add(table.Rows[i]);
            }
            providerFilter.ForEach(x => table.Rows.Remove(x));
        }
        if (!string.IsNullOrEmpty(txtCustomerPhone.Text))
        {
            var providerFilter = new List<DataRow>();
            var provider = Convert.ToInt32(txtCustomerPhone.Text);
            for (var i = 0; i < table.Rows.Count; i++)
            {

                if (!string.IsNullOrWhiteSpace(table.Rows[i]["CustomerPhone"].ToString()) &&
                    !Convert.ToInt32(table.Rows[i]["CustomerPhone"]).Equals(provider))
                    providerFilter.Add(table.Rows[i]);
            }
            providerFilter.ForEach(x => table.Rows.Remove(x));
        }
        return table;
    }

    public class UserData
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public string BranchName { get; set; }
        public string ResellerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string GovernorateName { get; set; }
        public string ServicePackageName { get; set; }
        public string SpName { get; set; }
        public string Serial { get; set; }
        public string CreationDate { get; set; }
        public string RequestNumber { get; set; }
        public string Notes { get; set; }
        public string PaymentTypeName { get; set; }
        public string UserName { get; set; }
        public string CentralName { get; set; }
        public string RequestDate { get; set; }
        public string OfferName { get; set; }
    }

    private void GetUserData(int statusId,int userId,string statusName,ISPDataContext context)
    {
        var accountManagerUsers = context.WorkOrders.Where(a => a.WorkOrderStatusID == statusId && a.User.AccountmanagerId != null && a.User.AccountmanagerId == userId).Select(a => new 
        {
            a.ID,
            a.Status.StatusName,
            a.Branch.BranchName,
           RESuserName=  a.User.UserName,
            a.CustomerPhone,
            a.CustomerName,
            a.Governorate.GovernorateName,
            a.ServicePackage.ServicePackageName,
            a.ServiceProvider.SPName,
            a.RouterSerial,
             a.CreationDate,
            a.RequestNumber,
            a.Notes,
            a.PaymentType.PaymentTypeName,
            a.UserName,
            a.Central.Name,
            a.RequestDate ,
             a.Offer.Title
        }).ToList();

        var  res=accountManagerUsers.AsEnumerable().Select(a=> new UserData
        {
            Id = Convert.ToInt32(a.ID),
            StatusName = a.StatusName,
            BranchName = a.BranchName,
            ResellerName = a.RESuserName,
            CustomerPhone = a.CustomerPhone,
            CustomerName = a.CustomerName,
            GovernorateName = a.GovernorateName,
            ServicePackageName = a.ServicePackageName,
            SpName = a.SPName,
            Serial = a.RouterSerial,
            CreationDate = a.CreationDate.ToString(),
            RequestNumber = a.RequestNumber,
            Notes = a.Notes,
            PaymentTypeName = a.PaymentTypeName,
            UserName = a.UserName,
            CentralName = a.Name,
            RequestDate = (a.RequestDate == null ? "_" : a.RequestDate.ToString()),
            OfferName = (a.Title == null ? "_" : a.Title)
        }).ToList();

        
        grd_wo.DataSource = res;
       
        grd_wo.Columns[16].Visible = true;
        grd_wo.Columns[17].Visible = true;
        grd_wo.Columns[18].Visible = false;
        grd_wo.Columns[19].Visible = false;
        grd_wo.Columns[20].Visible = false;

        

        grd_wo.DataBind();

      
        lbl_Legend.Text = accountManagerUsers.Count + @" " + Tokens.CsutomersWithThisStatus + @" " +
                          statusName;
    }







    private void ProcessQuery()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            ClearDialog();
            var query = QueryStringSecurity.Decrypt(Request.QueryString["sid"]);
            var statusId = Convert.ToString(query);
           
            hdnUrl.Value = statusId;
            if (!string.IsNullOrEmpty(statusId))
            {
                HfFlag.Value = statusId;
                DateArea.Visible = statusId == "5";
                TbCreationDate.Text = DateTime.Now.AddHours().ToShortDateString();
                var stQuery = context.Status.Where(st => st.ID == Convert.ToInt32(statusId)).Select(st => st.StatusName);
                var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (groupIdQuery == null)
                {
                    Response.Redirect("UnAuthorized.aspx");
                    return;
                }
                var id = groupIdQuery.GroupID;
                if (id == null)
                {
                    Response.Redirect("UnAuthorized.aspx");
                    return;
                }
                var groupId = id.Value;
                var dataLevelId = groupIdQuery.Group.DataLevelID;
                if (dataLevelId == null)
                {
                    Response.Redirect("UnAuthorized.aspx");
                    return;
                }
                var dataLevel = dataLevelId.Value;
                var groupPrivilegeQuery =
                    context.GroupPrivileges.Where(gp => gp.Group.ID == groupId).Select(gp => gp.privilege.Name);
                if (!(groupPrivilegeQuery.Contains(stQuery.FirstOrDefault()) || groupPrivilegeQuery.Contains("All")))
                {
                    Response.Redirect("UnAuthorized.aspx");
                }
                var acountManager = context.Users.Where(a => a.AccountmanagerId == groupIdQuery.ID).ToList();
                if (acountManager.Count == 0)
                {

                    _query.Append(statusId);
                    _query.Append("ORDER BY WorkOrders.ID");
                    var connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    var cmd = new SqlCommand(_query.ToString(), connection);
                    connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    EditControls.AddSerialField(table);
                    connection.Close();
                    table = Filter(table);

                    switch (dataLevel)
                    {
                        case 1:
                            grd_wo.DataSource = table;
                            ViewState["grdTable"] = table;
                            grd_wo.Columns[16].Visible = true;

                            grd_wo.Columns[18].Visible = true;
                            grd_wo.Columns[19].Visible = true;
                            grd_wo.Columns[20].Visible = false;
                            div_Approve.Visible = table.Rows.Count > 0;
                            Stopwatch sWatch = new Stopwatch();
                            sWatch.Start();

                            grd_wo.DataBind();
                            sWatch.Stop();
                            sWatch.ElapsedMilliseconds.ToString();
                            sWatch.Reset();
                            lbl_Legend.Text = table.Rows.Count + @" " + Tokens.CsutomersWithThisStatus + @" " + stQuery.FirstOrDefault();
                            break;
                        case 2:
                            {

                                List<int?> userBranchs =
                                    DataLevelClass.GetBranchAdminBranchIDs(
                                        Convert.ToInt32(HttpContext.Current.Session["User_ID"]));
                                var tbquery =
                                    table.AsEnumerable()
                                        .Where(dr => (userBranchs.Contains(dr.Field<int?>("BranchID"))))
                                        .Select(dr => new
                                        {
                                            ID = dr.Field<int>("ID"),
                                            StatusName = dr.Field<string>("StatusName"),
                                            BranchName = dr.Field<string>("BranchName"),
                                            ResellerName = dr.Field<string>("ResellerName"),
                                            CustomerPhone = dr.Field<string>("CustomerPhone"),
                                            CustomerName = dr.Field<string>("CustomerName"),
                                            GovernorateName = dr.Field<string>("GovernorateName"),
                                            ServicePackageName = dr.Field<string>("ServicePackageName"),
                                            SPName = dr.Field<string>("SPName"),
                                            Serial = dr.Field<decimal>("Serial"),
                                            CreationDate = dr.Field<DateTime?>("CreationDate"),
                                            RequestNumber = dr.Field<string>("RequestNumber"),
                                            Notes = dr.Field<string>("Notes"),
                                            PaymentTypeName = dr.Field<string>("PaymentTypeName"),
                                            UserName = dr.Field<string>("UserName"),
                                            RequestDate = dr.Field<DateTime?>("RequestDate"),
                                            OfferName = dr.Field<string>("OfferName"),
                                            CentralName = dr.Field<string>("CentralName")
                                        }).ToList();

                              
                                grd_wo.DataSource = tbquery; //this count
                                
                                grd_wo.Columns[18].Visible = false;

                                grd_wo.Columns[19].Visible = false;
                                grd_wo.Columns[20].Visible = false;

                                grd_wo.DataBind();
                                div_Approve.Visible = false;
                                lbl_Legend.Text = tbquery.Count() + @" " + Tokens.CsutomersWithThisStatus + @" " + stQuery.FirstOrDefault();
                            }
                            break;
                        case 3:
                            {
                                var tbquery = table.AsEnumerable()
                                    .Where(dr => dr.Field<int?>("ResellerID") == Convert.ToInt32(Session["User_ID"]))
                                    .Select(dr => new
                                    {
                                        ID = dr.Field<int>("ID"),
                                        StatusName = dr.Field<string>("StatusName"),
                                        BranchName = dr.Field<string>("BranchName"),
                                        ResellerName = dr.Field<string>("ResellerName"),
                                        CustomerPhone = dr.Field<string>("CustomerPhone"),
                                        CustomerName = dr.Field<string>("CustomerName"),
                                        GovernorateName = dr.Field<string>("GovernorateName"),
                                        ServicePackageName = dr.Field<string>("ServicePackageName"),
                                        SPName = dr.Field<string>("SPName"),
                                        Serial = dr.Field<decimal>("Serial"),
                                        CreationDate = dr.Field<DateTime?>("CreationDate"),
                                        RequestNumber = dr.Field<string>("RequestNumber"),
                                        Notes = dr.Field<string>("Notes"),
                                        PaymentTypeName = dr.Field<string>("PaymentTypeName"),
                                        UserName = dr.Field<string>("UserName"),
                                        RequestDate = dr.Field<DateTime?>("RequestDate"),
                                        OfferName = dr.Field<string>("OfferName"),
                                        CentralName = dr.Field<string>("CentralName"),
                                    }).ToList();

                                grd_wo.DataSource = tbquery; //this count
                                
                                grd_wo.Columns[18].Visible = false;
                                grd_wo.Columns[16].Visible = true;

                                grd_wo.Columns[19].Visible = false;
                                grd_wo.Columns[20].Visible = false;

                                grd_wo.DataBind();
                                div_Approve.Visible = false;
                                lbl_Legend.Text = tbquery.Count() + @" " + Tokens.CsutomersWithThisStatus + @" " + stQuery.FirstOrDefault();
                                //GV1.DataSource = tbquery.ToList();

                            }
                            break;
                    }
                }
                else
                {
                    var status = stQuery.FirstOrDefault();
                    GetUserData(Convert.ToInt32(statusId), groupIdQuery.ID,status, context);
                }


                if (statusId == "5")
            {
                if (dataLevel == 1)
                {
                    
                    grd_wo.Columns[19].Visible = true;
                    grd_wo.Columns[20].Visible = true;
                }
            }
            if (statusId == "6")
            {
                grd_wo.Columns[20].Visible = false;
              
                btn_ApproveSelected.Visible = false;
            }
            else if (statusId == "7")
            {
                grd_wo.Columns[18].Visible = false;
                grd_wo.Columns[19].Visible = false;
                grd_wo.Columns[13].Visible = true;
                grd_wo.Columns[20].Visible = false;
               
                btn_ApproveSelected.Visible = false;
            }
            else if (statusId == "9")
            {
                grd_wo.Columns[18].Visible = false;
                grd_wo.Columns[19].Visible = false;
                grd_wo.Columns[20].Visible = false;
                
                btn_ApproveSelected.Visible = false;
            }
            else if (statusId == "10")
            {
                grd_wo.Columns[18].Visible = false;
                grd_wo.Columns[19].Visible = false;
                grd_wo.Columns[20].Visible = false;
               
                btn_ApproveSelected.Visible = false;
            }
            else if (statusId == "11")
            {
                grd_wo.Columns[18].Visible = false;
                grd_wo.Columns[19].Visible = false;
                grd_wo.Columns[20].Visible = false;
               
                btn_ApproveSelected.Visible = false;
            }
        }
        else
        {
            lbl_QSStatus.Text = Tokens.InncorrectLink;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = "false";
        }
    }
}


    void ClearDialog()
    {
        TbPassword.Text = TbSerial.Text = TbUserName.Text = string.Empty;
        TbCreationDate.Text = DateTime.Now.AddHours().ToShortDateString();
    }


    void CheckCanEdit(ISPDataContext context)
    {
       
            var flag=EditCustomer = false;
           
            var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
            if (groupIdQuery != null)
            {
                var groupId = groupIdQuery.GroupID;
                if (groupId != null)
                {
                    var groupIDvalu = groupId.Value;
                    var groupPrivilegeQuery = context.GroupPrivileges.Where(gp => gp.Group.ID == groupIDvalu);
                    foreach (GroupPrivilege tmpgp in groupPrivilegeQuery)
                    {
                        if (tmpgp.privilege.Name == "EditCustomer.aspx")
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
        if (flag) EditCustomer = true;
       

    }

    void SendSms(ISPDataContext context,WorkOrder order)
    {
        
        try
        {
            var mobile = order.CustomerMobile;
            var message = global::SendSms.SendSmsByNotification(context, mobile, 12);
            if (!string.IsNullOrEmpty(message))
            {
                var myscript = "window.open('" + message + "')";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
            }
        }
        catch (Exception)
        {
            lbl_QSStatus.Text = Tokens.Error;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = "false";
        }
    }

    

    protected void btn_ApproveSelected_Click(object sender, EventArgs e)
    {
        var queryString = QueryStringSecurity.Decrypt(Request.QueryString["sid"]);
        int statusId = Convert.ToInt32(queryString);
        if ((statusId <= 0 || statusId >= 6) && statusId != 8)
        {
            lbl_QSStatus.Text = Tokens.CantApproveMsg;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = "false";
            return;
        }
        //todo:loop on selected only
        foreach (GridViewRow row in grd_wo.Rows)
        {
            var control = row.FindControl("SelectItem") as CheckBox;
            if (control == null || !control.Checked) continue;
            var dataKey = grd_wo.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            int id = Convert.ToInt32(dataKey["ID"]);
           
            ProcessRequest(id, statusId);
          
        }
       ProcessQuery();
       lbl_QSStatus.Text = Tokens.OrdersApproved;
       lbl_QSStatus.ForeColor = Color.Green;
       Session["messagenew"] = "true";
        HttpContext.Current.Response.Redirect(Request.RawUrl);
        
    }

    protected void ToSpiliting(object sender, EventArgs e)
    {
        var queryString = QueryStringSecurity.Decrypt(Request.QueryString["sid"]);
        var statusId = Convert.ToInt32(queryString);
        if ( statusId != 3)
        {
            lbl_QSStatus.Text = Tokens.CantApproveMsg;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = "false";
            return;
        }
       
        foreach (GridViewRow row in grd_wo.Rows)
        {
            var check = row.FindControl("SelectItem") as CheckBox;
            
            if (check == null || !check.Checked) continue;
            var dataKey = grd_wo.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var orderId = Convert.ToInt32(dataKey["ID"]);
            var order = _ispEntries.GetWorkOrder(orderId);
            if (!string.IsNullOrEmpty(txtPortNumber.Text)) order.PortNumber = txtPortNumber.Text;
            if (!string.IsNullOrEmpty(txtBlockNumber.Text)) order.BlockNumber = txtBlockNumber.Text;
            if (!string.IsNullOrEmpty(txtDslamNumber.Text)) order.DslamNumbers = txtDslamNumber.Text;
            ProcessRequest(orderId, statusId);
        }
        lbl_QSStatus.Text = Tokens.OrdersApproved;
        lbl_QSStatus.ForeColor = Color.Green;
        Session["messagenew"] = "true";
        ProcessQuery();
        
        HttpContext.Current.Response.Redirect(Request.RawUrl);
    }


    void ProcessRequest(int id, int statusId)
    {
        var oldState = "";
        var order = _ispEntries.GetWorkOrder(id);
        if (order == null) return;
        int userId = Convert.ToInt32(Session["User_ID"]);
        var now = DateTime.Now;
        var updateDate = now.AddHours();
        var requestDate = updateDate;

        var newstateId = statusId + 1;
        var upgradeState = _ispEntries.GetStatus(newstateId);
        oldState = order.Status==null?"": order.Status.StatusName??"";
        var offerStart = Convert.ToDateTime(TbCreationDate.Text);
        var status = UpdateWorkOrderStatus(order, upgradeState, newstateId, userId, statusId == 5 ? offerStart : updateDate);
        if (statusId == 5){
            status.IsNew = true;
        }
       
        _ispEntries.AddStatus(status);
        _ispEntries.Commit();

        var offerStartPlusMonth = offerStart.AddMonths(1);
        order.OfferStart = offerStart;
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var option = OptionsService.GetOptions(context, true);
            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
            {
                CenterMessage.SendMessageReport(order, oldState, upgradeState.StatusName, userId);
            }
        }

        if (statusId == 5)
        {
            
            if (TbUserName.Text!=string.Empty) order.UserName = TbUserName.Text;
            if (TbPassword.Text != string.Empty) order.Password = TbPassword.Text;
            if (TbSerial.Text != string.Empty) order.RouterSerial = TbSerial.Text;
            order.RequestDate = requestDate;
            order.Installed = false;
            var amount = _priceServices.BillDefault(order, requestDate.Month, requestDate.Year, null).Net;
            var basicBill = amount;
            const int daysInMonth = 30; //DateTime.DaysInMonth(offerStart.Year, offerStart.Month);

            var period = (daysInMonth - offerStart.Day);
            if (period < 30)
            {
                period += 1;
            }
            var demandAmount = amount * period / 30;

            if (RblByActivation.SelectedIndex == 0)
            {
                AmountofRouter(order,offerStart,offerStartPlusMonth);
                var activateProcessDemandService = new ProcessDemandsService(_ispEntries,
                    new DemandFactory(_ispEntries));
                activateProcessDemandService.CreateActivationDemandReporsts(order.ID, offerStart,
                    offerStartPlusMonth,
                    amount, offerStart, false, userId, true);
                //اضافة الروتر لو موجود عرض به روتر
                
            }
            else if(RblByActivation.SelectedIndex==1)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var optionPro = context.OptionInvoiceProviders.ToList();
                    foreach (var provider in optionPro)
                    {
                        if (order.ServiceProviderID != provider.ProviderId) continue;
                        var orderRequestDate = new DateTime(offerStartPlusMonth.Year, offerStartPlusMonth.Month, 1);
                        if (RblInOffer.SelectedIndex == 0)
                        {
                            if (order.OfferId != null)
                            {
                                var offer = order.Offer;
                                var monthOfferPrice = OfferPricingServices.GetOfferPrice(offer, amount,basicBill);
                                var netDemand = amount - monthOfferPrice;
                                var netDiscount = period*netDemand/30;
                                demandAmount = demandAmount - netDiscount;
                            }
                            if (order.Demands.Any())
                            {
                                if (order.Demands.Count == 1)
                                {
                                    var firstOrDefault = order.Demands.FirstOrDefault(a => a.Notes.Contains("مدفوع مقدما"));
                                    if (firstOrDefault != null && firstOrDefault.Paid)
                                        demandAmount -= firstOrDefault.Amount;
                                }
                            }
                            AmountofRouter(order,offerStart,orderRequestDate);
                            var demand = _demandFactory.CreateDemand(order, offerStart, orderRequestDate,
                                demandAmount, Convert.ToInt32(Session["User_ID"]), false, "فاتورة");
                            demand.Amount = demandAmount; 
                            demand.StartAt = offerStart;
                            
                            order.RequestDate = orderRequestDate;
                            order.OfferStart = offerStart;
                            _ispEntries.AddDemand(demand);
                                
                        }
                        else
                        {
                            if (order.Demands.Any())
                            {
                                if (order.Demands.Count == 1)
                                {
                                    var firstOrDefault = order.Demands.FirstOrDefault(a => a.Notes.Contains("مدفوع مقدما"));
                                    if (firstOrDefault != null && firstOrDefault.Paid)
                                        demandAmount -= firstOrDefault.Amount;
                                }
                            }
                            AmountofRouter(order,offerStart,orderRequestDate);
                            var demand = _demandFactory.CreateDemand(order, offerStart, orderRequestDate,
                                demandAmount,
                                Convert.ToInt32(Session["User_ID"]));
                            order.RequestDate = orderRequestDate;
                            order.OfferStart = orderRequestDate;
                            _ispEntries.AddDemand(demand);
                            //روتر فى حالة ان العرض به روتر
                                
                        }
                    }
                }

            }
            else if(RblByActivation.SelectedIndex==2)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var optionPro = context.OptionInvoiceProviders.ToList();
                    foreach (var provider in optionPro)
                    {
                        if (order.ServiceProviderID != provider.ProviderId) continue;
                        var orderRequestDate = offerStart.Day < 15 ? new DateTime(offerStart.Year, offerStart.Month, 15) : new DateTime(offerStartPlusMonth.Year,offerStartPlusMonth.Month,15);
                        if (RblInOffer.SelectedIndex == 0)
                        {
                            if (order.OfferId != null)
                            {
                                var offer = order.Offer;
                                var monthOfferPrice = OfferPricingServices.GetOfferPrice(offer, amount, basicBill);
                                var netDemand = amount - monthOfferPrice;
                                var netDiscount = period * netDemand / 30;
                                demandAmount = demandAmount - netDiscount;
                            }
                            if (order.Demands.Any())
                            {
                                if (order.Demands.Count == 1)
                                {
                                    var firstOrDefault = order.Demands.FirstOrDefault(a => a.Notes.Contains("مدفوع مقدما"));
                                    if (firstOrDefault != null && firstOrDefault.Paid)
                                        demandAmount -= firstOrDefault.Amount;
                                }
                            }
                            AmountofRouter(order, offerStart, orderRequestDate);
                            var demand = _demandFactory.CreateDemand(order, offerStart, orderRequestDate,
                                demandAmount, Convert.ToInt32(Session["User_ID"]), false, "فاتورة");
                            demand.Amount = demandAmount; 
                            demand.StartAt = offerStart;
                            
                            order.RequestDate = orderRequestDate;
                            order.OfferStart = offerStart;
                            _ispEntries.AddDemand(demand);

                        }
                        else
                        {
                            if (order.Demands.Any())
                            {
                                if (order.Demands.Count == 1)
                                {
                                    var firstOrDefault = order.Demands.FirstOrDefault(a => a.Notes.Contains("مدفوع مقدما"));
                                    if (firstOrDefault != null && firstOrDefault.Paid)
                                        demandAmount -= firstOrDefault.Amount;
                                }
                            }
                            AmountofRouter(order, offerStart, orderRequestDate);
                            var demand = _demandFactory.CreateDemand(order, offerStart, orderRequestDate,
                                demandAmount,
                                Convert.ToInt32(Session["User_ID"]));
                            order.RequestDate = orderRequestDate;
                            order.OfferStart = orderRequestDate;
                            _ispEntries.AddDemand(demand);
                            //روتر فى حالة ان العرض به روتر

                        }
                    }
                }

            }

            _ispEntries.Commit();

            // Detuct Routers 
            try
            {


                if (order.OfferId != null && order.Offer.withRouter)
                {
                    var reseller = Convert.ToInt32(order.ResellerID);
                    if (order.ResellerID != null)
                    {
                        _routerRepository.SaveResellerRouter(reseller, -1, DateTime.Now.AddHours(), order.ID);
                        _routerRepository.SaveWorkOrderRouter(new WorkOrderRouter
                        {
                            Quantity = 1,
                            Time = Convert.ToDateTime(TbCreationDate.Text),
                            ResellerId = reseller != 0 ? reseller : (int?) null,
                            WorkOrderId = order.ID,
                        });
                    }
                    else
                    {
                        _routerRepository.SaveRouter(new Router
                        {
                            Quantity = -1,
                            Time = Convert.ToDateTime(TbCreationDate.Text),
                            WorkOrderId = order.ID
                        }, 3);
                        _routerRepository.SaveWorkOrderRouter(new WorkOrderRouter
                        {
                            Quantity = 1,
                            Time = Convert.ToDateTime(TbCreationDate.Text),
                            WorkOrderId = order.ID,
                        });
                    }
                }
            }
            catch
            {
                lbl_QSStatus.Text = Tokens.Error;
                lbl_QSStatus.ForeColor = Color.Red;
                Session["messagenew"] = "false";
            }

           


        }
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            
            var option = context.EmailCnfgs.FirstOrDefault();
            if (order.WorkOrderStatusID == 6)
            {
                SendSms(context,order);
                if (order.ResellerID != null)
                {
                    try
                    {
                        if (option != null && option.Active)
                        {
                            var customerActivated = Tokens.CustomerActivated;
                            var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.Customer_Name + "</span></div>" + order.CustomerName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + order.CustomerPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + Tokens.BActive + "</p></div>";
                            var formalmessage = ClsEmail.Body(msg);
                            ClsEmail.SendEmail(order.User.UserEmail, customerActivated,formalmessage , true);
                        }
                    }
                    catch
                    {
                        lbl_QSStatus.Text = Tokens.Error;
                        lbl_QSStatus.ForeColor = Color.Red;
                        Session["messagenew"] = "false";
                    }
                }
                else
                {
                    try
                    {
                        if (option != null && option.Active)
                        {
                            var customerActivated = Tokens.CustomerActivated;
                            var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.Customer_Name + "</span></div>" + order.CustomerName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + order.CustomerPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + Tokens.BActive + "</p></div>";
                            var formalmessage = ClsEmail.Body(msg);
                            ClsEmail.SendEmail(order.CustomerEmail, customerActivated,formalmessage , true);
                        }
                    }
                    catch
                    {
                        lbl_QSStatus.Text = Tokens.Error;
                        lbl_QSStatus.ForeColor = Color.Red;
                        Session["messagenew"] = "false";
                    }
                }
            }
            if (order.WorkOrderStatusID == 8)
            {
                if (order.ResellerID != null)
                {
                    try
                    {
                        if (option != null && option.Active)
                        {
                            var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.Customer_Name + "</span></div>" + order.CustomerName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + order.CustomerPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + Tokens.HasProb2 + "</p></div>";
                            var formalmessage = ClsEmail.Body(msg);
                            ClsEmail.SendEmail(order.User.UserEmail, Tokens.CusProblem,formalmessage , true);
                        }
                    }
                    catch
                    {
                        lbl_QSStatus.Text = Tokens.Error;
                        lbl_QSStatus.ForeColor = Color.Red;
                        Session["messagenew"] = "false";
                    }
                }
                else
                {
                    try
                    {
                        if (option != null && option.Active)
                        {
                            var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.Customer_Name + "</span></div>" + order.CustomerName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + order.CustomerPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + Tokens.HasProb2 + "</p></div>";
                            var formalmessage = ClsEmail.Body(msg);
                            ClsEmail.SendEmail(order.CustomerEmail, Tokens.CusProblem,formalmessage , true);
                        }
                    }
                    catch
                    {
                        lbl_QSStatus.Text = Tokens.Error;
                        lbl_QSStatus.ForeColor = Color.Red;
                        Session["messagenew"] = "false";
                    }
                }
            }

                // Deduct Month invoice from reseller from distributer options
            try
            {
                if (order.WorkOrderStatusID == 5)
                {
                   

                    var distoption = context.DistributorOptions.FirstOrDefault();
                    var resdeduct = Convert.ToBoolean(distoption.ClientActivationSubtract);
                    if (resdeduct)
                    {
                        var amount = _priceServices.CustomerInvoiceDetailsDefault(order, requestDate.Month, requestDate.Year, false).Net;
                        var result = _creditRepository.Save(Convert.ToInt32(order.ResellerID ?? 0), userId,
                                           Convert.ToDecimal((amount > 0 ? amount : 0) * -1),
                                          "تخصيم قيمة فاتورة لعميل جديد" + " - " + order.CustomerName + " - " + order.CustomerPhone,
                                           DateTime.Now.AddHours());

                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
            if (order.WorkOrderStatusID != 8 || order.WorkOrderStatusID != 6)
            {
                try
                {
                    if (option != null && option.Active)
                    {
                        var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.Customer_Name + "</span></div>" + order.CustomerName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + order.CustomerPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + Tokens.NewStatusEmail + "<br/>" + order.Status.StatusName + "</p></div>";
                        var formalmessage = ClsEmail.Body(msg);
                        ClsEmail.SendEmail(order.User.UserEmail,
                            ConfigurationManager.AppSettings["InstallationEmail"]
                            , ConfigurationManager.AppSettings["CC2Email"],
                            Tokens.Customer + Tokens.Colon + order.CustomerPhone,formalmessage , true);
                    }
                }
                catch
                {
                    lbl_QSStatus.Text = Tokens.Error;
                    lbl_QSStatus.ForeColor = Color.Red;
                    Session["messagenew"] = "false";
                }
            }
           
        }
    }

    void AmountofRouter(WorkOrder order,DateTime startAt,DateTime endAt)
    {
        var of = order.Offer;
        if (of!=null && of.RouterCost > 0 && of.withRouter)
        {
            const string notes = "قيمة الروتر"; //"فاتورة + قيمة الروتر";
            
            var routerDemand = _demandFactory.CreateDemand(order, startAt,
                endAt, of.RouterCost, Convert.ToInt32(Session["User_ID"]), false, notes);
            routerDemand.IsResellerCommisstions = false;
            _ispEntries.AddDemand(routerDemand);
            _ispEntries.Commit();
        }
    }

    global::Db.WorkOrderStatus UpdateWorkOrderStatus(WorkOrder order, Status upgradeState, int newstateId, int userId, DateTime updateDate)
    {
        order.Status = upgradeState;
        _ispEntries.Commit();
        var status = new global::Db.WorkOrderStatus
        {
            WorkOrder = order,
            StatusID = newstateId,
            UserID = userId,
            UpdateDate = updateDate
        };
      
        return status;
    }


    protected void SelectHeader_CheckedChanged(object sender, EventArgs e)
    {
        if (((CheckBox)sender).Checked == false)
        {
            foreach (GridViewRow row in grd_wo.Rows)
            {
                ((CheckBox)row.FindControl("SelectItem")).Checked = false; 
            }
        }
        else
        {
            foreach (GridViewRow row in grd_wo.Rows)
            {
                ((CheckBox)row.FindControl("SelectItem")).Checked = true;
            }
        }
    }


    protected void lnb_Reject_Click(object sender, EventArgs e)
    {
        ViewState.Add("woid", ((LinkButton)sender).CommandArgument);
     
    }


    protected void btn_reject_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var id = Convert.ToInt32(hf_woid.Value);
            var workOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == id);//Convert.ToInt32(ViewState["woid"]));
            if(workOrder==null)return;
            workOrder.WorkOrderStatusID = 7;
            var reason = txt_RejectReason.Text.Trim();
            workOrder.Notes = reason;
            context.SubmitChanges();

            //Save History
            var newwos = new global::Db.WorkOrderStatus
            {
                WorkOrderID = Convert.ToInt32(id),
                StatusID = 7,
                UserID = Convert.ToInt32(Session["User_ID"]),
                UpdateDate = DateTime.Now.AddHours(),
                Notes = reason
            };
            context.WorkOrderStatus.InsertOnSubmit(newwos);
            context.SubmitChanges();

            lbl_ProcessResult.Text = Tokens.OrderRejected;
            lbl_ProcessResult.ForeColor = Color.Green;
            Session["messagenew"] = "true";
            ProcessQuery();
            var queryString = QueryStringSecurity.Decrypt(Request.QueryString["sid"]);
            int statusId = Convert.ToInt32(queryString);
            var upgradeState = _ispEntries.GetStatus(statusId);
            var option = OptionsService.GetOptions(context, true);
            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
            {
                CenterMessage.SendRequestReject(workOrder, reason, upgradeState.StatusName,
                    Convert.ToInt32(Session["User_ID"]));
            }
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }
    }
   

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       
        const string attachment = "attachment; filename=Report.xls";
        Response.ClearContent();
        Page.EnableViewState = false;
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        var sw = new StringWriter();
        var htw = new HtmlTextWriter(sw);
        grd_wo.AllowPaging = false;
        DataTable dtS = (DataTable)ViewState["grdTable"];
        grd_wo.DataSource = dtS;
        grd_wo.DataBind();
        grd_wo.RenderControl(htw);
        string style = @"<style> .textmode { } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }


   


    protected void ModalBtnApproveSave(object sender, EventArgs e)
    {
        var queryString = QueryStringSecurity.Decrypt(Request.QueryString["sid"]);
        var statusId = Convert.ToInt32(queryString);
        if ((statusId <= 0 || statusId >= 6) && statusId != 8)
        {
            lbl_QSStatus.Text = Tokens.CantApproveMsg;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = "false";
            return;
        }
        var id = Convert.ToInt32(HfSelected.Value);
        ProcessRequest(id, statusId);
        lbl_QSStatus.Text = Tokens.OrdersApproved;
        lbl_QSStatus.ForeColor = Color.Green;
        Session["messagenew"] = "true";
        ProcessQuery();
        HttpContext.Current.Response.Redirect(Request.RawUrl);
    }
    protected void btn_ApproveSelectToActive_Click(object sender, EventArgs e)
    {
        var queryString = QueryStringSecurity.Decrypt(Request.QueryString["sid"]);
        var statusId = Convert.ToInt32(queryString);
        if (statusId != 5)
        {
            lbl_QSStatus.Text = Tokens.CantApproveMsg;
            lbl_QSStatus.ForeColor = Color.Red;
            Session["messagenew"] = "false";
            return;
        }
        foreach (GridViewRow row in grd_wo.Rows)
        {
            var control = row.FindControl("selecttoapprove") as CheckBox;
            if (control == null || !control.Checked) continue;
            var dataKey = grd_wo.DataKeys[row.RowIndex];
            if (dataKey == null) continue;
            var id = Convert.ToInt32(dataKey["ID"]);
           
            ProcessRequest(id, statusId);
          
        }
        lbl_QSStatus.Text = Tokens.OrdersApproved;
        lbl_QSStatus.ForeColor = Color.Green;
        Session["messagenew"] = "true";
        ProcessQuery();
        HttpContext.Current.Response.Redirect(Request.RawUrl);
    }

    protected void SaveMulti(object sender, EventArgs e)
    {
        var activeUserId = Convert.ToInt32(Session["User_ID"]);
        var updateDate = DateTime.Now.AddHours();
        foreach (GridViewRow row in grd_wo.Rows)
        {
            var check = row.FindControl("SelectItem") as CheckBox;
            var hiddenForId = row.FindControl("hf_id") as HiddenField;
            if (hiddenForId == null || string.IsNullOrEmpty(hiddenForId.Value) || check == null || !check.Checked) continue;
            var orderId = Convert.ToInt32(hiddenForId.Value);
            var order = _ispEntries.GetWorkOrder(orderId);
            var oldstatus = order.Status.StatusName;
           
            var newStstusId = Convert.ToInt32(DdlNewState.SelectedItem.Value);
            var newstatus = _ispEntries.GetStatus(newStstusId);
            var status = UpdateWorkOrderStatus(order, newstatus, newStstusId, activeUserId, updateDate);
            using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var option = OptionsService.GetOptions(context, true);
                if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                {
                    CenterMessage.SendMessageReport(order, oldstatus, newstatus.StatusName, activeUserId);
                }
            }
            
            _ispEntries.AddStatus(status);
            if (!string.IsNullOrEmpty(TbProblem.Text) && newStstusId == 7)
                order.Notes = TbProblem.Text;
            _ispEntries.Commit();
        }
        ProcessQuery();
        lbl_QSStatus.Text = Tokens.OrdersApproved;
        lbl_QSStatus.ForeColor = Color.Green;
        Session["messagenew"] = "true";
        HttpContext.Current.Response.Redirect(Request.RawUrl);
        
    }


    protected void FilterResults(object sender, EventArgs e)
    {
        ProcessQuery(); 
    }
    
        protected void LinkBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect(string.Format("EditCustomer.aspx?WOID={0}", QueryStringSecurity.Encrypt(id.ToString())));
        }

        protected void LinkBtnDetails_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect(string.Format("CustomerDetails.aspx?WOID={0}", QueryStringSecurity.Encrypt(id.ToString())));
            
        }

       
        protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(grd_wo, "l_number");
        }
    }
}