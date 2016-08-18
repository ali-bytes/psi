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
    public partial class RecieptofOutdoorPayment : Page
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

            var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
            if (!string.IsNullOrEmpty(Request.QueryString["Id"])&&cnfg!=null)
            {
                var demandId = Convert.ToInt32(Request.QueryString["Id"]);
                var demand = context.CustomerPayments.FirstOrDefault(d => d.ID == demandId);
                if (demand != null)
                {
                    
                    lblCustomer.Text = demand.CustomerName;
                    lblPhone.Text = demand.CustomerTelephone;
                    lblCompany.Text = demand.VoiceCompany.CompanyName;
                    lblAmount.Text = Helper.FixNumberFormat(demand.InvoiceAmount);
                   
                    lblPayment.Text = demand.Time.ToString("dd-MM-yyyy",CultureInfo.InvariantCulture);
                  
                    lblNotes.Text = demand.Notes;
                    
                    lblEmp.Text = demand.User.UserName;
                }

                LImg.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
                HCompany.InnerHtml = cnfg.CompanyName;
                Caution.InnerHtml = cnfg.Caution;
              
            }
        }
    }
}
}