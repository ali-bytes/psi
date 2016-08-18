using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Resources;

namespace NewIspNL.Pages
{
    public partial class TestCustomer : CustomPage
    {
      

    //readonly ISPDataContext _context;

    readonly IspDomian _domian;

    public bool ShowSucess { get; set; }
    public TestCustomer()
    {
        var context = IspDataContext;
        _domian = new IspDomian(context);
        ShowSucess = false;
    }


    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        _domian.PopulateGovernorates(DdlGovernate);
        ShowSucess = !string.IsNullOrWhiteSpace(Request.QueryString["s"]);
        var fullData = Request.QueryString["fullData"];
        if (ShowSucess && Session["Order"]!=null)
        {
            var order =(WorkOrder) Session["Order"];
            if (!string.IsNullOrEmpty(order.CustomerMobile))
            {
                using (var db5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    if (!string.IsNullOrWhiteSpace(fullData))
                    {
                        if (fullData == "true"|| fullData=="True")
                        {
                            var option = db5.Options.FirstOrDefault();
                            if (option != null && Convert.ToBoolean(option.WidthOfReciept)) datatable.Style["width"] = "8cm";
                            else
                            {
                                imgSite.Style["width"] = "20%";
                                imgSite.Style["height"] = "17%";
                                imgSite.Style["float"] = "left";
                            }
                            imgSite.Style["dispaly"] = "block";
                            div_Receipt.Visible = true;
                            var userId = Convert.ToInt32(Session["User_ID"]);
                            var user = db5.Users.FirstOrDefault(usr => usr.ID == userId);
                            var cnfg = db5.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
                            if (cnfg != null)
                            {
                                imgSite.Src = "../PrintLogos/" + cnfg.LogoUrl;
                                lblCompanyName.Text = cnfg.CompanyName;
                                lblBranch.Text = cnfg.Branch.BranchName;
                            }
                            //hdfWOId.Value = order.ID.ToString(CultureInfo.InvariantCulture);
                          lnkConta.HRef = string.Format("Contarct.aspx?WOID={0}", QueryStringSecurity.Encrypt(order.ID.ToString()));
                            txtContractingCost.Text = order.ContractingCost.ToString();
                            txtPrepaid.Text = order.Prepaid.ToString();
                            txtInstallationCost.Text = order.InstallationCost.ToString();
                            txtCustomerName.Text = order.CustomerName;
                            txtCustomerPhone.Text = order.CustomerPhone;
                            lblDate.Text = DateTime.Now.AddHours().ToShortDateString();
                            if (user != null) lblEmployee.Text = user.UserName;
                            mpe_Receipt.Show();
                        }
                    }
                    var message = SendSms.SendSmsByNotification(db5, order.CustomerMobile, 18);
                    if (!string.IsNullOrEmpty(message))
                    {
                        var myscript = "window.open('" + message + "')";
                        ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript, true);
                    }
                }
            }
        }
    }



    protected void BSearch_OnClick(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var option = OptionsService.GetOptions(context, true);
            var governorate = context.Governorates.FirstOrDefault(x => x.ID == Convert.ToInt32(DdlGovernate.SelectedValue));
            if (governorate == null) { LMessage.Text = Tokens.Error; return; }
            var phone = option != null && option.MergeGovernorateWithPhoneInCreateCustomer ? string.Format("{0}{1}", governorate.Code, TbPhone.Text) : TbPhone.Text;
            var customer = context.WorkOrders.Where(x => x.CustomerPhone.Equals(phone) && x.CustomerGovernorateID == Convert.ToInt32(DdlGovernate.SelectedItem.Value)).ToList();
            ShowSucess = false;
            if(customer.Any())
                LMessage.Text = Tokens.CustomerExists;
            else{
                //if(governorate == null) return;
                //var option = OptionsService.GetOptions(_context, true);
                //var phone = option != null && option.MergeGovernorateWithPhoneInCreateCustomer ? string.Format("{0}{1}", governorate.Code, TbPhone.Text) : TbPhone.Text;
                Response.Redirect(string.Format("AddNewCustomer.aspx?p={0}&g={1}", phone, governorate.ID));
            }
        }
    }

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    var order = (WorkOrder)Session["Order"];
    //    var id = QueryStringSecurity.Encrypt(order.ID.ToString());
    //    Response.Redirect("Contarct.aspx?WOID=" + id);
    //}
}

}