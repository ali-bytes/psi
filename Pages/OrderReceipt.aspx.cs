using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using NewIspNL.Services;

namespace NewIspNL.Pages
{
    public partial class OrderReceipt : CustomPage
    {
     
    private readonly IWorkOrderCredit _orderCredit;

    public OrderReceipt()
    {
        _orderCredit=new Domain.Abstract.WorkOrderCredit();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (string.IsNullOrEmpty(Request.QueryString["receiptId"]) || string.IsNullOrEmpty(Request.QueryString["orderId"]) ||string.IsNullOrEmpty(Request.QueryString["amount"])) return;
            var rec =Convert.ToInt32(QueryStringSecurity.Decrypt(Request.QueryString["receiptId"]));
            var ord =Convert.ToInt32(QueryStringSecurity.Decrypt(Request.QueryString["orderId"]));
            var amo =Convert.ToDecimal(QueryStringSecurity.Decrypt(Request.QueryString["amount"]));
            PopulateAjaxPop(rec,ord,amo,context);
        }

    }
    void PopulateAjaxPop(int receiptId, int orderId, decimal amount, ISPDataContext context)
    {
        var receipt = _orderCredit.GetCredit(receiptId, context);
        if (receipt == null) return;
        var order = receipt.WorkOrder;

        if (order == null) return;
        if (order.ID != orderId)
        {
            Msg2.InnerHtml = @"المستخدم صاحب الايصال ليس مطابق للمستخدم الذى تبحث عنه";
            Msg2.Visible = true;
            Msg.Visible = false;
            return;
        }
        var option = OptionsService.GetOptions(context, true);
        if (option != null && Convert.ToBoolean(option.WidthOfReciept)) datatable.Style["width"] = "8cm";
        else
        {
            imgSite.Style["width"] = "20%";
            imgSite.Style["height"] = "17%";
            imgSite.Style["float"] = "left";
        }
        imgSite.Style["dispaly"] = "block";
       
        var userId = Convert.ToInt32(Session["User_ID"]);
        var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
        var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
        if (cnfg != null)
        {
            imgSite.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
            lblCompanyName.Text = cnfg.CompanyName;
            lblBranch.Text = cnfg.Branch.BranchName;
        }
        lblReceiptNumber.Text = receipt.Id.ToString();
        txtPrepaid.Text = amount.ToString(CultureInfo.InvariantCulture);

        txtCustomerName.Text = order.CustomerName;
        txtCustomerPhone.Text = order.CustomerPhone;
        lblNotes.Text = receipt.Notes;
        txtDate.Text = receipt.Time.ToShortDateString();
        lblEmployee.Text = receipt.User.UserName;
       
    }
}
}