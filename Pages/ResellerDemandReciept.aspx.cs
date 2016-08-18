using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class ResellerDemandReciept : Page
    {
 
    protected void Page_Load(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            if (user == null)
            {
                return;
            }
            var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.ResellerId == user.ID);
            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && cnfg != null)
            {
                var que = QueryStringSecurity.Decrypt(Request.QueryString["id"]);
                var demandId = Convert.ToInt32(que);
                var demand = context.Demands.FirstOrDefault(d => d.Id == demandId);
                if (demand != null)
                {
                    var order = demand.WorkOrder;
                    Customer.Text = order.CustomerName;
                    Phone.Text = order.CustomerPhone;
                    Central.Text = order.Central == null ? "-" : order.Central.Name;
                    Gov.Text = order.Governorate.GovernorateName;
                    Pack.Text = order.ServicePackage.ServicePackageName;
                    Amount.Text = Helper.FixNumberFormat(demand.Amount);
                    StartAt.Text = demand.StartAt.ToString("dd-MM-yyyy",CultureInfo.InvariantCulture);
                    EndAt.Text = demand.EndAt.ToString("dd-MM-yyyy",CultureInfo.InvariantCulture);
                    lblServiceProvider.Text = demand.WorkOrder.ServiceProvider.SPName;
                    Notes.Text = demand.Notes;
                    Emp.Text = user.UserName;
                    var firstOrDefault = demand.Receipts.FirstOrDefault(a => a.DemandId == demandId);
                    if (firstOrDefault != null) RecieptNum.Text = firstOrDefault.ID.ToString(CultureInfo.InvariantCulture);
                }

                LImg.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
                HCompany.InnerHtml = cnfg.CompanyName;
                Caution.InnerHtml = cnfg.Caution;
                Address.Text = cnfg.ContactData;
            }
        }
    }
}
}