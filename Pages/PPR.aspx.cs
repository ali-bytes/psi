using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class PPR : CustomPage
    {
     
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        using(var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
        
            FindRequests(context3);
            
        }
        
    }
    void FindRequests(ISPDataContext context2)
    { 
        var groupIdQuery = context2.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
        var userid = Convert.ToInt32(Session["User_ID"]);
        var gId = groupIdQuery.FirstOrDefault();
        var branchs = DataLevelClass.GetBranchAdminBranchIDs(userid);
        var id = gId.Group.DataLevelID;
        var datalevel = id.Value;
        if (datalevel == 1)
        { 
        
            var requests = context2.WorkOrderRequests
            .Where(r => r.RequestID == 11 &&
                        r.RSID == 3 &&
                        r.ProcessDate == null 
                        
                        )
            .Select(x =>
                new
                {
                    Branch = x.WorkOrder.Branch.BranchName,
                    x.ID,
                    x.WorkOrder.CustomerName,
                    x.WorkOrder.CustomerPhone,
                    RequestDate = x.RequestDate.Value.ToShortDateString(),
                    Total = Helper.FixNumberFormat(x.Total),
                    x.WorkOrderID,
                    x.WorkOrder.ServiceProvider.SPName,
                    x.WorkOrder.Governorate.GovernorateName,
                    x.WorkOrder.Status.StatusName,
                    x.WorkOrder.ServicePackage.ServicePackageName,
                    x.WorkOrder.Offer.Title,
                    user = x.WorkOrder.UserName,
                    Reseller=x.WorkOrder.User==null?" ":x.WorkOrder.User.UserName,
                    x.WorkOrder.Password,
                    Start = (x.DemandId != null) ? x.Demand.StartAt.Date.ToString() : "",
                    End = (x.DemandId != null) ? x.Demand.EndAt.Date.ToString() : ""
                }).ToList();
        gv_customers.DataSource = requests;
        gv_customers.DataBind();
        }
        else if (datalevel == 2)
        {
            var requests = context2.WorkOrderRequests
           .Where(r => r.RequestID == 11 &&
                       r.RSID == 3 &&
                       r.ProcessDate == null
                       && branchs.Contains(r.WorkOrder.BranchID )
                       )
           .Select(x =>
               new
               {
                   Branch = x.WorkOrder.Branch.BranchName,
                   x.ID,
                   x.WorkOrder.CustomerName,
                   x.WorkOrder.CustomerPhone,
                   RequestDate = x.RequestDate.Value.ToShortDateString(),
                   Total = Helper.FixNumberFormat(x.Total),
                   x.WorkOrderID,
                   x.WorkOrder.ServiceProvider.SPName,
                   x.WorkOrder.Governorate.GovernorateName,
                   x.WorkOrder.Status.StatusName,
                   x.WorkOrder.ServicePackage.ServicePackageName,
                   x.WorkOrder.Offer.Title,
                   user = x.WorkOrder.UserName,
                   Reseller = x.WorkOrder.User == null ? " " : x.WorkOrder.User.UserName,
                   x.WorkOrder.Password,
                   Start = (x.DemandId != null) ? x.Demand.StartAt.Date.ToString() : "",
                   End = (x.DemandId != null) ? x.Demand.EndAt.Date.ToString() : ""
               }).ToList();
            gv_customers.DataSource = requests;
            gv_customers.DataBind();


        }
        else if (datalevel == 3)
         {
             var requests = context2.WorkOrderRequests
          .Where(r => r.RequestID == 11 &&
                      r.RSID == 3 &&
                      r.ProcessDate == null
                      && r.WorkOrder.ResellerID == gId.ID
                      )
          .Select(x =>
              new
              {
                  Branch = x.WorkOrder.Branch.BranchName,
                  x.ID,
                  x.WorkOrder.CustomerName,
                  x.WorkOrder.CustomerPhone,
                  RequestDate = x.RequestDate.Value.ToShortDateString(),
                  Total = Helper.FixNumberFormat(x.Total),
                  x.WorkOrderID,
                  x.WorkOrder.ServiceProvider.SPName,
                  x.WorkOrder.Governorate.GovernorateName,
                  x.WorkOrder.Status.StatusName,
                  x.WorkOrder.ServicePackage.ServicePackageName,
                  x.WorkOrder.Offer.Title,
                  user = x.WorkOrder.UserName,
                  Reseller = x.WorkOrder.User == null ? " " : x.WorkOrder.User.UserName,
                  x.WorkOrder.Password,
                  Start = (x.DemandId != null) ? x.Demand.StartAt.Date.ToString() : "",
                  End = (x.DemandId != null) ? x.Demand.EndAt.Date.ToString() : ""
              }).ToList();
             gv_customers.DataSource = requests;
             gv_customers.DataBind();


         }


    }
    protected void gv_customers_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(gv_customers, "gv_lNumber");
      
    }

    protected void Export_OnClick(object sender, EventArgs e)
    {
        GridHelper.ExportOneGrid("PPR",gv_customers);
    }
}
}