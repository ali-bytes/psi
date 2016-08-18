﻿using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class smallDemandReciept : CustomPage
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
                if (!string.IsNullOrEmpty(Request.QueryString["d"]) && cnfg != null)
                {
                    var que = QueryStringSecurity.Decrypt(Request.QueryString["d"]);
                    var demandId = Convert.ToInt32(que);
                    var demand = context.Demands.FirstOrDefault(d => d.Id == demandId);
                    if (demand != null)
                    {

                        var order = demand.WorkOrder;
                        Customer.InnerHtml = order.CustomerName;
                        Phone.Text = order.CustomerPhone;
                        //lblMobile.Text = order.CustomerMobile;
                        //lblAddress.Text = order.CustomerAddress;
                        //Central.Text = order.Central == null ? "-" : order.Central.Name;
                        //Gov.Text = order.Governorate.GovernorateName;
                        Pack.Text = order.ServicePackage.ServicePackageName;
                        Amount.Text = Helper.FixNumberFormat(demand.Amount);
                        StartAt.Text = demand.StartAt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        EndAt.Text = demand.EndAt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        lblServiceProvider.Text = demand.WorkOrder.ServiceProvider.SPName;
                        Notes.Text = demand.PaymentComment;
                        Emp.Text = user.UserName;//demand.User.UserName;
                        var firstOrDefault = demand.Receipts.FirstOrDefault(a => a.DemandId == demandId);
                        if (firstOrDefault != null)
                            RecieptNum.Text = firstOrDefault.ID.ToString(CultureInfo.InvariantCulture);
                        else lblRecieptNum.Visible = false;
                    }

                    LImg.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
                    HCompany.Text = cnfg.CompanyName;
                    Caution.InnerHtml = cnfg.Caution;
                    Address3.InnerHtml = cnfg.ContactData;
                }
            }
        }
    }
}