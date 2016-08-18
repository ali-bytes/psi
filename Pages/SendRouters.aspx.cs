using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SendRouters : CustomPage
    {
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PopulateGrid();
            PopulateRouters();
           filldrops();  
            //ViewState["num"] = 10;
        }
       
    }

        public static List<int?> GetBranchAdminBranchIDs(int userId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (HttpContext.Current.Session["User_ID"] == null)
                    HttpContext.Current.Response.Redirect("../default.aspx");
                var userBranchs =
                    context.UserBranches.Where(ub => ub.UserID == userId).Select(ub => ub.BranchID).ToList();
                if (userBranchs.Count <= 0)
                    userBranchs = context.Users.Where(u => u.ID == userId).Select(u => u.BranchID).ToList();
                return userBranchs;
                
            }
        }

        public virtual List<User> ResellersOfUser(int userId)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var resellerList = new List<User>();
            var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
            if (user == null) return resellerList;
            var id = user.Group.DataLevelID;
            if (id == null) return resellerList;
            var level = id.Value;
            var i = context.Users.Where(usr => usr.ID == userId).Select(usr => usr.BranchID).First();
            if (i == null)
            {
            }
            else
            {
                var userBranchId = i.Value;
                switch (level)
                {
                    case 1:
                        resellerList = context.Users.Where(usr => usr.GroupID == 6).ToList();
                        break;
                    case 2:
                        resellerList =
                            context.Users.Where(
                                usr => usr.GroupID == 6 && GetBranchAdminBranchIDs(userId).Contains(usr.BranchID))
                                .ToList();
                        break;
                    case 3:
                        resellerList =
                            context.Users.Where(
                                usr => usr.GroupID == 6 && usr.BranchID == userBranchId && usr.ID == userId).ToList();
                        break;
                }
            }
            return resellerList;
        }
    }
    public void filldrops()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var statues = context.Status;
            Ddlstate.DataSource = statues;
            Ddlstate.DataTextField = "StatusName";
            Ddlstate.DataValueField = "ID";
            Ddlstate.DataBind();
            Helper.AddDefaultItem(Ddlstate);

            var userId = Convert.ToInt32(Session["User_ID"]);
            var resller = ResellersOfUser(userId);
            DdlReseller.DataSource = resller;
            DdlReseller.DataTextField = "UserName";
            DdlReseller.DataValueField = "ID";
            DdlReseller.DataBind();
            Helper.AddDefaultItem(DdlReseller);


            var bran = DataLevelClass.GetUserBranches();
            DdlBranch.DataSource = bran;
            DdlBranch.DataTextField = "BranchName";
            DdlBranch.DataValueField = "ID";
            DdlBranch.DataBind();
            Helper.AddDefaultItem(DdlBranch);


            var pro = context.ServiceProviders;
            DdlSeviceProvider.DataSource = pro;
            DdlSeviceProvider.DataTextField = "SPName";
            DdlSeviceProvider.DataValueField = "ID";
            DdlSeviceProvider.DataBind();
            Helper.AddDefaultItem(DdlSeviceProvider);
        }
    }



  void PopulateRouters()
    {
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var allstores = context.RecieveRouters.Where(a=>a.IsRecieved==false).ToList();
            ddlRouters.DataSource = allstores;
            ddlRouters.DataTextField = "RouterSerial";
            ddlRouters.DataValueField = "Id";
            ddlRouters.DataBind();
            Helper.AddDefaultItem(ddlRouters);
        }
    }
    void PopulateGrid()
    {
        // Edited by ashraf - select all WO instead of active only
        //var orderRepository = new IspEntries();
       /*var orderList=orderRepository.GetWorkOrderOrdersByStatusId(6);*/
            
        //orderList = orderList.Where(x => x.OfferId != null && x.Offer.withRouter).ToList();
            using (var contex = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var orderList = contex.WorkOrders.Where(x => x.OfferId != null && x.Offer.withRouter).ToList();
                var newOrderList = new List<WorkOrder>();
                foreach (var order in orderList)
                {
                    var orderexist = CheckIfAlreadyExist(order.ID, contex);
                    if (orderexist == null || !Convert.ToBoolean(orderexist.IsRecieved))
                    {
                        newOrderList.Add(order);
                    }
                }
                var data = newOrderList.Select(a => new
                {
                    a.CustomerName,
                    a.Governorate.GovernorateName,
                    a.CustomerPhone,
                    a.RequestNumber,
                    CentralName = a.Central.Name,
                    a.ServicePackage.ServicePackageName,
                    a.ServiceProvider.SPName,
                    a.Status.StatusName,
                    a.Branch.BranchName,
                    Reseller = a.User != null ? a.User.UserName : "_",
                    a.CreationDate,
                    a.Offer.Title,
                    a.RequestDate,
                    a.ID,
                    activationdate = a.WorkOrderStatus.Where(x => x.WorkOrderID == a.ID && x.StatusID == 6).OrderByDescending(x => x.ID).Select(x => x.UpdateDate).FirstOrDefault()
                }).ToList();
                grd_wo.DataSource = data; //.Take(rowNumber);
                grd_wo.DataBind();
                /*if (rowNumber >= data.Count)
            {
                btnLoadMore.Visible = false;
            }*/
            }
        
    }
    protected void grd_wo_DataBound(object sender, EventArgs e)
    {
        
        {
            Helper.GridViewNumbering(grd_wo, "l_number");
           
        }
    }
   

    protected void btnRecieveToCustomer_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var woid = Convert.ToInt32(hdfId.Value);
            var userId = Convert.ToInt32(Session["User_ID"]);
            var routerId = Convert.ToInt32(ddlRouters.SelectedItem.Value);
            var router = context.RecieveRouters.FirstOrDefault(a => a.Id == routerId); //CheckIfAlreadyExist(routerId, context);
            if (router != null && router.CompanyProcessDate != null)
            {
                if (!string.IsNullOrWhiteSpace(fileUpload1.FileName))
                {
                    var file = fileUpload1.FileName;
                    router.Attachments = file;

                    var extensions = new List<string> { ".JPG", ".GIF",".JPEG",".PNG" };
                        string ex = Path.GetExtension(fileUpload1.FileName);

                    if (!string.IsNullOrEmpty(ex) &&
                        extensions.Any(currentExtention => currentExtention == ex.ToUpper()))
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
                        Msgsuccess.Visible = true;
                        //PopulateGrid();
                        ProcessQuery();
                        PopulateRouters();
                    }
                }
               
            }
            else
            {
                MsgError.Visible = true;
                Msgsuccess.InnerText = Tokens.NoResults;
            }
        }
    }

    protected RecieveRouter CheckIfAlreadyExist(int id,ISPDataContext context)
    {
        var router = context.RecieveRouters.FirstOrDefault(a => a.WorkOrderId == id);
        return router;
    }

  
    protected void FilterResults(object sender, EventArgs e)
    {
        PopulateGrid();
        PopulateRouters();
    }

  

    protected void FilterResults2(object sender, EventArgs e)
    {
        ProcessQuery();
    }
   
    readonly StringBuilder _query;
    private void ProcessQuery()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
          
           
           
                
               var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
               var id = groupIdQuery.GroupID;
               var groupId = id.Value;
               var dataLevelId = groupIdQuery.Group.DataLevelID;
               var dataLevel = dataLevelId.Value;
               var groupPrivilegeQuery =
                   context.GroupPrivileges.Where(gp => gp.Group.ID == groupId).Select(gp => gp.privilege.Name);
               
            
            //if (groupIdQuery == null)
                //{
                //    Response.Redirect("UnAuthorized.aspx");
                //    return;
                //}
                //if (id == null)
                //{
                //    Response.Redirect("UnAuthorized.aspx");
                //    return;
                //}
                //
                //if (dataLevelId == null)
                //{
                //    Response.Redirect("UnAuthorized.aspx");
                //    return;
                //}
               //if (!(groupPrivilegeQuery.Contains("All")))
                //{
                //    Response.Redirect("UnAuthorized.aspx");
                //}

                var orderList = context.WorkOrders.Where(x => x.OfferId != null && x.Offer.withRouter).ToList();
                var newOrderList = new List<WorkOrder>();
                foreach (var order in orderList)
                {
                    var orderexist = CheckIfAlreadyExist(order.ID, context);
                    if (orderexist == null || !Convert.ToBoolean(orderexist.IsRecieved))
                    {
                        newOrderList.Add(order);
                    }
                }
                var data = newOrderList.Select(a => new
                {
                    a.CustomerName,
                    a.ResellerID,
                    a.ServiceProviderID,
                    a.WorkOrderStatusID,
                    a.Governorate.GovernorateName,
                    a.CustomerPhone,
                    a.RequestNumber,
                    CentralName = a.Central.Name,
                    a.ServicePackage.ServicePackageName,
                    a.ServiceProvider.SPName,
                    a.Status.StatusName,
                    a.Branch.BranchName,
                    Reseller = a.User != null ? a.User.UserName : "_",
                    a.CreationDate,
                    a.Offer.Title,
                    a.RequestDate,
                    a.BranchID,
                    a.ID,
                    activationdate = a.WorkOrderStatus.Where(x => x.WorkOrderID == a.ID && x.StatusID == 6).OrderByDescending(x => x.ID).Select(x => x.UpdateDate).FirstOrDefault()
                }).ToList();
                DataTable table2 = ToDataTable(data);
                   
                  
                    EditControls.AddSerialField(table2);
         
                    table2 = Filter(table2);

                    switch (dataLevel)
                    {
                        case 1:
                            grd_wo.DataSource = table2;

                          
                          
                            grd_wo.DataBind();
                             break;
                        case 2:
                            {

                                List<int?> userBranchs =
                                    DataLevelClass.GetBranchAdminBranchIDs(
                                        Convert.ToInt32(HttpContext.Current.Session["User_ID"]));
                                var tbquery =
                                    table2.AsEnumerable()
                                        .Where(dr => (userBranchs.Contains(dr.Field<int?>("BranchID"))))
                                        .Select(dr => new
                                        {

CustomerName=  dr.Field<string>("CustomerName"),
GovernorateName= dr.Field<string>("GovernorateName"),
 CustomerPhone= dr.Field<string>("CustomerPhone"),
 RequestNumber= dr.Field<string>("RequestNumber"),
 CentralName= dr.Field<string>("CentralName"),
ServicePackageName= dr.Field<string>("ServicePackageName"),
SPName= dr.Field<string>("SPName"),
              StatusName= dr.Field<string>("StatusName"),
              Reseller= dr.Field<string>("Reseller")!=null ? dr.Field<string>("Reseller") : "-",
CreationDate = dr.Field<DateTime>("CreationDate"),
                Title= dr.Field<string>("Title"),
ID = dr.Field<int>("ID"),
RequestDate = dr.Field<DateTime>("RequestDate"),
activationdate = dr.Field<DateTime>("activationdate"),
   
             
                                         
                                              }).ToList();

                                grd_wo.DataSource = tbquery; //this count
                               

                                grd_wo.DataBind();
                                  }
                            break;
                        case 3:
                            {
                                var tbquery = table2.AsEnumerable()
                                    .Where(dr => dr.Field<int?>("ResellerID") == Convert.ToInt32(Session["User_ID"]))
                                    .Select(dr => new
                                    {
                                        CustomerName = dr.Field<string>("CustomerName"),
                                        GovernorateName = dr.Field<string>("GovernorateName"),
                                        CustomerPhone = dr.Field<string>("CustomerPhone"),
                                        RequestNumber = dr.Field<string>("RequestNumber"),
                                        CentralName = dr.Field<string>("CentralName"),
                                        ServicePackageName = dr.Field<string>("ServicePackageName"),
                                        SPName = dr.Field<string>("SPName"),
                                        StatusName = dr.Field<string>("StatusName"),
                                        Reseller = dr.Field<string>("Reseller") != null ? dr.Field<string>("Reseller") : "-",
                                        CreationDate = dr.Field<DateTime>("CreationDate"),
                                        Title = dr.Field<string>("Title"),
                                        ID = dr.Field<int>("ID"),
                                        RequestDate = dr.Field<DateTime>("RequestDate"),
                                        activationdate = dr.Field<DateTime>("activationdate"),
                                    }).ToList();

                                grd_wo.DataSource = tbquery; //this count
                               

                                grd_wo.DataBind();
                                 }
                            break;
                    }
          
             


              
           
        }
    }
    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
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
                    if (!string.IsNullOrEmpty(table.Rows[i]["ResellerID"].ToString()))
                        resellerFilter.Add(table.Rows[i]);
                }
                resellerFilter.ForEach(x => table.Rows.Remove(x));
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

        if (Ddlstate.SelectedIndex > 0)
        {
            var govFilter = new List<DataRow>();
            var gov = Convert.ToInt32(Ddlstate.SelectedItem.Value);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(table.Rows[i]["WorkOrderStatusID"].ToString()) &&
                    !Convert.ToInt32(table.Rows[i]["WorkOrderStatusID"]).Equals(gov))
                    govFilter.Add(table.Rows[i]);
            }
            govFilter.ForEach(x => table.Rows.Remove(x));
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

        return table;
    }



}
}