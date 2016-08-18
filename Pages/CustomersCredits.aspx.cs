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
    public partial class CustomersCredits : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            fill_grid();
        }
        protected void payhis_OnDataBound(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvCustomerData, "LNo");

        }

     

        public class DataGv 
        {
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string StatusName { get; set; }
            public string ServicePackageName { get; set; }
            public string BranchName { get; set; }
            public string SPName { get; set; }

            public string OfferTitle { get; set; }
            public string CentralName { get; set; }
            public decimal CreditAmount { get; set; }
        }

        public void fill_grid()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {



                var groupIdQuery = context.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                var userid = Convert.ToInt32(Session["User_ID"]);
                var gId = groupIdQuery.FirstOrDefault();
                var branchs = DataLevelClass.GetBranchAdminBranchIDs(userid);
                var id = gId.Group.DataLevelID;
                var datalevel = id.Value;







                var newlist = new List<DataGv>();
                var allCredit = context.WorkOrderCredits.Select(z => z.WorkOrderId).Distinct().ToList();


                if (datalevel == 1)
                {
                    foreach (var i in allCredit)
                    {
                        var add =
                            context.WorkOrderCredits.Where(z => z.WorkOrderId == i).Select(z => new
                            {
                                z.WorkOrder.CustomerName,
                                z.WorkOrder.CustomerPhone,
                                z.WorkOrder.Status.StatusName,
                                z.WorkOrder.ServicePackage.ServicePackageName,
                                z.WorkOrder.ServiceProvider.SPName,
                                OfferTitle = z.WorkOrder.Offer.Title,
                                CentralName = z.WorkOrder.Central.Name,
                                z.CreditAmount,
                                z.WorkOrder.Branch.BranchName


                            }).ToList();
                        var addlist = add.LastOrDefault();
                        if (addlist == null || addlist.CreditAmount == 0) continue;
                        var newcustomer = new DataGv
                        {
                            BranchName = addlist.BranchName,
                            CentralName = addlist.CentralName,
                            CreditAmount = addlist.CreditAmount,
                            CustomerName = addlist.CustomerName,
                            CustomerPhone = addlist.CustomerPhone,
                            OfferTitle = addlist.OfferTitle,
                            SPName = addlist.SPName,
                            ServicePackageName = addlist.ServicePackageName,
                            StatusName = addlist.StatusName
                        };
                        newlist.Add(newcustomer);

                    }

                    GvCustomerData.DataSource = newlist;
                    GvCustomerData.DataBind();
                }
           
            
            else if (datalevel == 2)
            {
                foreach (var i in allCredit)
                {
                    var add =
                        context.WorkOrderCredits.Where(z => z.WorkOrderId == i&& branchs.Contains(z.WorkOrder.BranchID )).Select(z => new
                        {
                            z.WorkOrder.CustomerName,
                            z.WorkOrder.CustomerPhone,
                            z.WorkOrder.Status.StatusName,
                            z.WorkOrder.ServicePackage.ServicePackageName,
                            z.WorkOrder.ServiceProvider.SPName,
                            OfferTitle = z.WorkOrder.Offer.Title,
                            CentralName = z.WorkOrder.Central.Name,
                            z.CreditAmount,
                            z.WorkOrder.Branch.BranchName


                        }).ToList();
                    var addlist = add.LastOrDefault();
                    if (addlist == null || addlist.CreditAmount == 0) continue;
                    var newcustomer = new DataGv
                    {
                        BranchName = addlist.BranchName,
                        CentralName = addlist.CentralName,
                        CreditAmount = addlist.CreditAmount,
                        CustomerName = addlist.CustomerName,
                        CustomerPhone = addlist.CustomerPhone,
                        OfferTitle = addlist.OfferTitle,
                        SPName = addlist.SPName,
                        ServicePackageName = addlist.ServicePackageName,
                        StatusName = addlist.StatusName
                    };
                    newlist.Add(newcustomer);

                }

                GvCustomerData.DataSource = newlist;
                GvCustomerData.DataBind();


            }
 else if (datalevel == 3)
 {

     foreach (var i in allCredit)
     {
         var add =
             context.WorkOrderCredits.Where(z => z.WorkOrderId == i && z.WorkOrder.ResellerID == gId.ID).Select(z => new
             {
                 z.WorkOrder.CustomerName,
                 z.WorkOrder.CustomerPhone,
                 z.WorkOrder.Status.StatusName,
                 z.WorkOrder.ServicePackage.ServicePackageName,
                 z.WorkOrder.ServiceProvider.SPName,
                 OfferTitle = z.WorkOrder.Offer.Title,
                 CentralName = z.WorkOrder.Central.Name,
                 z.CreditAmount,
                 z.WorkOrder.Branch.BranchName


             }).ToList();
         var addlist = add.LastOrDefault();
         if (addlist == null || addlist.CreditAmount == 0) continue;
         var newcustomer = new DataGv
         {
             BranchName = addlist.BranchName,
             CentralName = addlist.CentralName,
             CreditAmount = addlist.CreditAmount,
             CustomerName = addlist.CustomerName,
             CustomerPhone = addlist.CustomerPhone,
             OfferTitle = addlist.OfferTitle,
             SPName = addlist.SPName,
             ServicePackageName = addlist.ServicePackageName,
             StatusName = addlist.StatusName
         };
         newlist.Add(newcustomer);

     }

     GvCustomerData.DataSource = newlist;
     GvCustomerData.DataBind();

 }

            }







        }
    }
}