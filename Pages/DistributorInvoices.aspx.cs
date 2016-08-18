using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class DistributorInvoices : CustomPage
    {
        public ReceiptCnfg Cnfg { get; set; }
        private readonly IUserSaveRepository _userSave;
        private readonly BranchCreditRepository _branchCreditRepository;
        private readonly IResellerCreditRepository _creditRepository;
        private readonly IBoxCreditRepository _boxCreditRepository;
        readonly RecieptCnfgRepository _cnfgRepository;
        public DistributorInvoices()
        {
            _userSave = new UserSaveRepository();
            _branchCreditRepository = new BranchCreditRepository();
            _creditRepository = new ResellerCreditRepository();
            _boxCreditRepository = new BoxCreditRepository();
            var _context = IspDataContext;
            _cnfgRepository = new RecieptCnfgRepository(_context);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateSaves();
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            Div1.Visible = false;
            externalCust.Visible = false;
            ispCust.Visible = false;
            Div2.Visible = false;

            if (string.IsNullOrEmpty(txtPhone.Text) || areaCode.SelectedIndex == 0)
            {
                Div1.Visible = true;
                Div1.InnerHtml = "Error Please enter phone and choose area";
                Div1.Attributes.Add("class", "alert alert-danger");
                return;
            }
            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                var distributorList = db8.DistributorProviders.Select(z => z.ProviderForDistributorID).ToList();
                var woProvider =
                    db8.WorkOrders.FirstOrDefault(z => z.CustomerPhone == txtPhone.Text);

                if (woProvider != null && !distributorList.Contains(woProvider.ServiceProviderID))
                {
                    Div1.Visible = true;
                    Div1.InnerHtml = "مزود الخدمة لهذا العميل غير مربوط بالدستربيوتر";
                    Div1.Attributes.Add("class", "alert alert-danger");
                    return;
                }
                if (woProvider == null)
                {
                    externalCust.Visible = true;
                }
                if (woProvider != null)
                {
                    ispCust.Visible = true;
                }
            }
            DisCustomerDetails customerDetails = new DisCustomerDetails();
            CookieContainer cok = new CookieContainer();
            string token = string.Empty;

            if (ViewState["cookie"] == null)
            {
                cok = Distributor.GetLoginCookie();
                if (cok.Count > 0)
                {
                    cok = Distributor.Login(cok);
                    if (cok != null)
                    {
                        token = Distributor.GetCustomerDetailsToken(cok);
                        ViewState.Add("cookie", cok);
                    }
                    else
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml = "Please check Distributor User name or password";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        return;
                    }

                }
            }
            else
            {
                cok = (CookieContainer)ViewState["cookie"];
                token = Distributor.GetCustomerDetailsToken(cok);
                if (string.IsNullOrEmpty(token))
                {
                    // in case the session in viewState is expired
                    cok = Distributor.GetLoginCookie();
                    if (cok.Count > 0)
                    {
                        cok = Distributor.Login(cok);
                        if (cok != null)
                        {
                            token = Distributor.GetCustomerDetailsToken(cok);
                            ViewState.Add("cookie", cok);
                        }
                        else
                        {
                            Div1.Visible = true;
                            Div1.InnerHtml = "Please check Distributor User name or password";
                            Div1.Attributes.Add("class", "alert alert-danger");
                            return;
                        }

                    }
                }
            }

            if (cok != null && cok.Count > 0 && !string.IsNullOrEmpty(token))
            {
                customerDetails = Distributor.GetCustomerDetailsByPhone(txtPhone.Text, areaCode.Value, token, cok);
                if (customerDetails != null)
                {
                    ViewState.Add("customerPhone", customerDetails.Phone);
                    ViewState.Add("customerName", customerDetails.Name);
                    ViewState.Add("customerAmount", customerDetails.Amount);
                }

            }

            if (customerDetails == null)
            {
                Div1.Visible = true;
                Div1.InnerHtml = Session["errormsg"].ToString();
                Div1.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                List<DisCustomerDetails> customerDetails2 = new List<DisCustomerDetails>();
                customerDetails2.Add(customerDetails);
                grd_invoice.DataSource = customerDetails2;
                grd_invoice.DataBind();
            }

        }

        protected void lnb_Pay_Click(object sender, EventArgs e)
        {
            var phone = hdCustomerPhone.Value;
            var govNumber = HdGovNumber.Value;
            var userId = Convert.ToInt32(Session["User_ID"]);
            string transactionNumber = "0";
            var amount = Convert.ToDecimal(ViewState["customerAmount"] ?? 0);
            var customerPhone = ViewState["customerPhone"] != null ? ViewState["customerPhone"].ToString() : null;
            var customerName = ViewState["customerName"] != null ? ViewState["customerName"].ToString() : "-";
            int saveId = Convert.ToInt32(string.IsNullOrEmpty(ddlSavesPay.SelectedValue) ? "0" : ddlSavesPay.SelectedValue);
            Div1.Visible = false;
            Div1.InnerHtml = "";
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(govNumber))
            {
                Div1.Visible = true;
                Div1.InnerHtml = "Error Please check phone or area";
                Div1.Attributes.Add("class", "alert alert-danger");
                return;
            }
            try
            {


                // check credit
                var option = new DistributorOption();
                var existedWo = new WorkOrder();
                var branchCredit = default(double);
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var cuser = context.Users.FirstOrDefault(x => x.ID == userId);
                    if (cuser != null && cuser.GroupID != 6 && ddlSavesPay.SelectedIndex == 0)
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml = "Error Please check the save";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    option = context.DistributorOptions.FirstOrDefault();
                    existedWo = context.WorkOrders.FirstOrDefault(x => x.CustomerPhone == phone);
                    var curuser = context.Users.FirstOrDefault(x => x.ID == userId);
                    branchCredit = Convert.ToDouble(_branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0)));
                    if (curuser == null) return;
                    // Our Customer
                    if (option != null)
                    {
                        var curbox = option.BoxId ?? 0;
                        if (curbox > 0)
                        {
                            var net = _boxCreditRepository.GetNetBoxCredit(curbox);
                            if (net < amount)
                            {
                                Div2.Visible = true;
                                Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                Div2.Attributes.Add("class", "alert alert-danger");
                                return;
                            }
                        }
                    }

                    #region Our Customer

                    if (existedWo != null)
                    {

                        if (option == null)
                        {
                            return;
                        }


                        // reseller
                        if (curuser.GroupID == 6)
                        {
                            if (existedWo.ResellerID != null && existedWo.ResellerID == userId)
                            {

                               
                                if (context.Options.First().DiscoundfromResellerandBranch)
                                {
                                    if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                    {
                                        Div2.Visible = true;
                                        Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                        Div2.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                }

                                var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                                var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                                if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                            }
                            else if (existedWo.ResellerID != null && existedWo.ResellerID != userId)
                            {
                                if (context.Options.First().DiscoundfromResellerandBranch)
                                {
                                    if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                    {
                                        Div2.Visible = true;
                                        Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                        Div2.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                }

                                var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                                var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                                if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                            }
                            else if (existedWo.ResellerID == null)
                            {
                                if (context.Options.First().DiscoundfromResellerandBranch)
                                {
                                    if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                    {
                                        Div2.Visible = true;
                                        Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                        Div2.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                }

                                var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                                var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                                if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                            }


                        }

                        else

                        {
                            var bc = _branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0));
                            branchCredit =
                                Convert.ToDouble(bc > 0 ? bc : 0);
                            if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                            {
                                Div2.Visible = true;
                                Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                Div2.Attributes.Add("class", "alert alert-danger");
                                return;
                            }
                        }







                    }
                    else
                    {
                        // external

                        #region external

                        if (curuser.GroupID == 6)
                        {

                            if (context.Options.First().DiscoundfromResellerandBranch)
                            {
                                var bc = _branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0));
                                branchCredit = Convert.ToDouble(bc > 0 ? bc : 0);
                                if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                            }
                            var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                            var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                            if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                            {
                                Div2.Visible = true;
                                Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                Div2.Attributes.Add("class", "alert alert-danger");
                                return;
                            }
                        }
                        else
                        {

                           
                                var bc = _branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0));
                                branchCredit = Convert.ToDouble(bc > 0 ? bc : 0);
                                if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                            

                        }

                        #endregion
                    }

                    #endregion
                }

                #region Pay in Distriputer

                CookieContainer cok = new CookieContainer();
                string token = string.Empty;

                if (ViewState["cookie"] == null)
                {
                    cok = Distributor.GetLoginCookie();
                    if (cok.Count > 0)
                    {
                        cok = Distributor.Login(cok);
                        token = Distributor.GetCustomerDetailsToken(cok);
                        ViewState.Add("cookie", cok);
                    }
                }
                else
                {
                    cok = (CookieContainer) ViewState["cookie"];
                    token = Distributor.GetCustomerDetailsToken(cok);
                    if (string.IsNullOrEmpty(token))
                    {
                        // in case the session in viewState is expired
                        cok = Distributor.GetLoginCookie();
                        if (cok.Count > 0)
                        {
                            cok = Distributor.Login(cok);
                            token = Distributor.GetCustomerDetailsToken(cok);
                            ViewState.Add("cookie", cok);
                        }
                    }
                }

                //Pay in Distributor
                if (cok != null && cok.Count > 0 && !string.IsNullOrEmpty(token))
                {
                    int check = Distributor.PayDemand(customerPhone, govNumber, cok, token);
                    if (check == 2)
                    {
                        Div1.Visible = true;
                        Div1.InnerHtml = Session["errormsg"].ToString() ?? "Try again later";
                        Div1.Attributes.Add("class", "alert alert-danger");
                        return;
                    }
                    else
                    {
                        transactionNumber = Distributor.GetTransActionNumber(cok) ?? "0";
                        Div1.Visible = true;
                        Div1.InnerHtml = "تم تدفيع العميل على الدستربيوتر";
                        Div1.Attributes.Add("class", "alert alert-success");
                    }
                }
                else
                {
                    Div1.Visible = true;
                    Div1.InnerHtml = Session["errormsg"] != null ? Session["errormsg"].ToString() : "Try again later";
                    Div1.Attributes.Add("class", "alert alert-danger");
                    return;
                }

                #endregion

                // Pay in isp
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    // Our Customer
                    var curuser = context.Users.FirstOrDefault(x => x.ID == userId);

                    #region Our Customer


                    if (option == null)
                    {
                        return;
                    }
                    if (curuser == null) return;

                    if (existedWo != null)
                    {

                        // reseller
                        if (curuser.GroupID == 6)
                        {
                            if (existedWo.ResellerID != null && existedWo.ResellerID == userId)
                            {

                                // detuct reseller commission from amount before subtract from reseller credit
                                var firstOrDefault =
                                    context.ResellerPackagesDiscounts.FirstOrDefault(
                                        r =>
                                            r.ResellerId == userId &&
                                            r.ProviderId == existedWo.ServiceProviderID &&
                                            r.PackageId == existedWo.ServicePackageID);
                                var discount = firstOrDefault != null ? firstOrDefault.Discount : 0;

                                if (option.SubtractResellerCommission != null &&
                                    option.SubtractResellerCommission == true)
                                {
                                    var netdscount = amount*discount/100;
                                    amount = amount - netdscount;
                                }
                                if (context.Options.First().DiscoundfromResellerandBranch)
                                {
                                    if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                    {
                                        Div2.Visible = true;
                                        Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                        Div2.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                    else
                                    {
                                        var branchdiscound =
                                            context.BranchPackagesDiscounts.FirstOrDefault(
                                                r =>
                                                    r.BranchId == curuser.BranchID &&
                                                    r.ProviderId == existedWo.ServiceProviderID &&
                                                    r.PackageId == existedWo.ServicePackageID);
                                        var dis = branchdiscound != null ? branchdiscound.Discount : 0;
                                        var netdiscount = amount*dis/100;
                                        var branchamount = amount - netdiscount;

                                        _branchCreditRepository.Save(
                                            Convert.ToInt32(curuser.BranchID ?? 0), userId,
                                            Convert.ToDecimal(branchamount > 0 ? branchamount : 0)*-1,
                                            "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                            existedWo.CustomerPhone + " - " +
                                            TbComment.Text,
                                            DateTime.Now.AddHours());
                                    }
                                }

                                var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                                var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                                if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                                else
                                {

                                    var result = _creditRepository.Save(Convert.ToInt32(userId),
                                        userId,
                                        Convert.ToDecimal((amount > 0 ? amount : 0)*-1),
                                        "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                        existedWo.CustomerPhone + " - " +
                                        TbComment.Text,
                                        DateTime.Now.AddHours());
                                }
                            }
                            else if (existedWo.ResellerID != null && existedWo.ResellerID != userId)
                            {
                                if (context.Options.First().DiscoundfromResellerandBranch)
                                {
                                    if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                    {
                                        Div2.Visible = true;
                                        Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                        Div2.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                    else
                                    {


                                        _branchCreditRepository.Save(
                                            Convert.ToInt32(curuser.BranchID ?? 0), userId,
                                            Convert.ToDecimal(amount > 0 ? amount : 0)*-1,
                                            "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                            existedWo.CustomerPhone + " - " +
                                            TbComment.Text,
                                            DateTime.Now.AddHours());
                                    }
                                }

                                var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                                var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                                if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                                else
                                {
                                    var result = _creditRepository.Save(Convert.ToInt32(userId),
                                        userId,
                                        Convert.ToDecimal((amount > 0 ? amount : 0)*-1),
                                        "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                        existedWo.CustomerPhone + " - " +
                                        TbComment.Text,
                                        DateTime.Now.AddHours());
                                }
                            }
                            else if (existedWo.ResellerID == null)
                            {
                                if (context.Options.First().DiscoundfromResellerandBranch)
                                {
                                    if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                    {
                                        Div2.Visible = true;
                                        Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                        Div2.Attributes.Add("class", "alert alert-danger");
                                        return;
                                    }
                                    else
                                    {
                                        _branchCreditRepository.Save(
                                            Convert.ToInt32(curuser.BranchID ?? 0), userId,
                                            Convert.ToDecimal(amount > 0 ? amount : 0)*-1,
                                            "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                            existedWo.CustomerPhone + " - " +
                                            TbComment.Text,
                                            DateTime.Now.AddHours());
                                    }
                                }

                                var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                                var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                                if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                                else
                                {
                                    var result = _creditRepository.Save(Convert.ToInt32(userId),
                                        userId,
                                        Convert.ToDecimal((amount > 0 ? amount : 0)*-1),
                                        "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                        existedWo.CustomerPhone + " - " +
                                        TbComment.Text,
                                        DateTime.Now.AddHours());
                                }
                            }


                        }

                        else

                        {
                            var bc = _branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0));
                            branchCredit =
                                Convert.ToDouble(bc > 0 ? bc : 0);
                            if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                            {
                                Div2.Visible = true;
                                Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                Div2.Attributes.Add("class", "alert alert-danger");
                                return;
                            }
                            else
                            {
                                _branchCreditRepository.Save(
                                    Convert.ToInt32(curuser.BranchID ?? 0), userId,
                                    Convert.ToDecimal(amount > 0 ? amount : 0)*-1,
                                    "دفع عميل فى الدستربيوتر" + " - " + existedWo.CustomerName + " - " +
                                    existedWo.CustomerPhone + " - " +
                                    TbComment.Text,
                                    DateTime.Now.AddHours());
                            }

                        }


                    }
                    else
                    {
                        // external

                        #region external

                        if (curuser.GroupID == 6)
                        {

                            if (context.Options.First().DiscoundfromResellerandBranch)
                            {
                                var bc = _branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0));
                                branchCredit = Convert.ToDouble(bc > 0 ? bc : 0);
                                if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                                else
                                {
                                    _branchCreditRepository.Save(
                                        Convert.ToInt32(curuser.BranchID ?? 0), userId,
                                        Convert.ToDecimal(amount > 0 ? amount : 0)*-1,
                                        "دفع عميل فى الدستربيوتر" + " - " + customerName + " - " +
                                        customerPhone + " - " +
                                        TbComment.Text,
                                        DateTime.Now.AddHours());
                                }
                            }
                            var rc = _creditRepository.GetNetCredit(Convert.ToInt32(userId));
                            var resellerCredit = Convert.ToDouble(rc > 0 ? rc : 0);
                            if (resellerCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                            {
                                Div2.Visible = true;
                                Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                Div2.Attributes.Add("class", "alert alert-danger");
                                return;
                            }
                            else
                            {
                                var result = _creditRepository.Save(Convert.ToInt32(userId),
                                    userId,
                                    Convert.ToDecimal((amount > 0 ? amount : 0)*-1),
                                    "دفع عميل فى الدستربيوتر" + " - " + customerName + " - " +
                                    customerPhone + " - " +
                                    TbComment.Text,
                                    DateTime.Now.AddHours());
                            }
                        }
                        else
                        {

                            if (context.Options.First().DiscoundfromResellerandBranch)
                            {
                                var bc = _branchCreditRepository.GetNetCredit(Convert.ToInt32(curuser.BranchID ?? 0));
                                branchCredit = Convert.ToDouble(bc > 0 ? bc : 0);
                                if (branchCredit < Convert.ToDouble(amount > 0 ? amount : 0))
                                {
                                    Div2.Visible = true;
                                    Div2.InnerHtml = "Isp error " + Tokens.NotEnoughtCreditMsg;
                                    Div2.Attributes.Add("class", "alert alert-danger");
                                    return;
                                }
                                else
                                {
                                    _branchCreditRepository.Save(
                                        Convert.ToInt32(curuser.BranchID ?? 0), userId,
                                        Convert.ToDecimal(amount > 0 ? amount : 0)*-1,
                                        "دفع عميل فى الدستربيوتر" + " - " + customerName + " - " +
                                        customerPhone + " - " +
                                        TbComment.Text,
                                        DateTime.Now.AddHours());
                                }
                            }

                        }

                        #endregion
                    }


                    #endregion

                    // collection commission for reseller to hois credit

                    if (curuser.GroupID == 6)
                    {
                        if (existedWo != null)
                        {
                            if (amount > 50)
                            {
                               
                            var resellerDistributorCommision = context.ResellerDistributorCommisions.FirstOrDefault(
                                x => x.ResellerID == userId);
                            if (resellerDistributorCommision != null)
                            {
                                var rescollcomm =
                                    resellerDistributorCommision.HisClientCommission;
                                var result = _creditRepository.Save(Convert.ToInt32(userId), userId,
                                    Convert.ToDecimal(rescollcomm > 0 ? rescollcomm : 0),
                                    customerName + " - " + customerPhone + " - " + "عمولة تحصيل" +
                                    " - " + TbComment.Text,
                                    DateTime.Now.AddHours());
                            }
                            }
                        }
                        else
                        {
                            if (amount > 50)
                            {
                                var resellerDistributorCommision = context.ResellerDistributorCommisions.FirstOrDefault(
                                    x => x.ResellerID == userId);
                                if (resellerDistributorCommision != null)
                                {
                                    var rescollcomm =
                                        resellerDistributorCommision.OtherClientCommission;
                                    var result = _creditRepository.Save(Convert.ToInt32(userId), userId,
                                        Convert.ToDecimal(rescollcomm > 0 ? rescollcomm : 0),
                                        customerName + " - " + customerPhone + " - " + "   عمولة تحصيل عميل خارجى" +
                                        " - " +
                                        TbComment.Text,
                                        DateTime.Now.AddHours());
                                }
                            }

                        }
                    }



                    // deduct from box
                    var curbox = option.BoxId ?? 0;
                    if (curbox > 0)
                    {
                        var notes = "دفع عميل فى الدستربيوتر" + " - " + customerName + " - " +
                                    customerPhone + " - " + TbComment.Text;
                        _boxCreditRepository.SaveBox(curbox, userId, Convert.ToDecimal(amount > 0 ? amount : 0)*-1,
                            notes, DateTime.Now.AddHours());

                        if (amount > 50)
                            {
                        var ourcoollcomm = option.CollectionCommission;
                        var notes2 = "عمولة تحصيل" + " " + customerName + " - " +
                                     customerPhone + " - " + TbComment.Text;
                        _boxCreditRepository.SaveBox(curbox, userId,
                            Convert.ToDecimal(ourcoollcomm > 0 ? ourcoollcomm : 0), notes2, DateTime.Now.AddHours());
}
                    }

                    if (curuser.GroupID != 6)
                    {
                        // save move
                       
                            string notes2 = customerName + " - " +
                                            customerPhone + " - " + TbComment.Text;
                            var done = _userSave.BranchAndUserSaves(saveId, userId,
                                Convert.ToDouble(amount > 0 ? amount : 0),
                                "دفع مطالبة من صفحة الدستربيوتر",
                                notes2, context);

                      
                    }

                    if (existedWo != null)
                    {
                        using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                        {
                            DistributorPaymentRecord record = new DistributorPaymentRecord();
                            record.CustomerPhone = customerPhone;
                            record.CustomerName = customerName;
                            record.Amount = amount;
                            record.PaidDate = DateTime.Now.AddHours();
                            record.UserId = userId;
                            record.WorkOrderId = existedWo.ID;
                            record.BoxId = curbox;
                            record.TransactionNumber = Convert.ToInt64(transactionNumber);
                            db.DistributorPaymentRecords.InsertOnSubmit(record);
                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                        {
                            DistributorPaymentRecord record = new DistributorPaymentRecord();
                            record.CustomerPhone = customerPhone;
                            record.CustomerName = customerName;
                            record.Amount = amount;
                            record.PaidDate = DateTime.Now.AddHours();
                            record.UserId = userId;
                            record.BoxId = curbox;
                            record.TransactionNumber = Convert.ToInt64(transactionNumber);
                            db.DistributorPaymentRecords.InsertOnSubmit(record);
                            db.SubmitChanges();
                        }
                    }


                    // print recipt 


                    Cnfg = _cnfgRepository.GetCnfg(userId);
                    LCustomer.Text = Convert.ToString(ViewState[("customerName")]);
                    LFor.Text = "Tedata Invoice";
                    LNumber.Text = transactionNumber ?? "NO";
                    var user1 = context.Users.FirstOrDefault(c => c.ID == userId);
                    LBy.Text = user1 == null ? "" : user1.UserName;
                    LPackage.Text = Convert.ToString(ViewState[("customerAmount")]);
                    LDate.Text = Convert.ToString(DateTime.Now);




                }
           
        }
                catch
                (Exception ex)
            {
                Div2.Visible = true;
                Div2.InnerHtml = ex.ToString();
                Div2.Attributes.Add("class", "alert alert-danger");
            }

        }


        protected void grd_invoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Helper.GridViewNumbering(grd_invoice, "no");
        }
        private void PopulateSaves()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (Session["User_ID"] == null) return;
                var userId = Convert.ToInt32(Session["User_ID"]);
                var cuser = context.Users.FirstOrDefault(x => x.ID == userId);
                if (cuser != null && cuser.GroupID == 6)
                {
                    savesDiv.Visible = false;
                }
                else
                {
                    ddlSavesPay.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
                    {
                        a.Save.SaveName,
                        a.Save.Id
                    });
                    ddlSavesPay.DataBind();
                    Helper.AddDefaultItem(ddlSavesPay);
                }

            }
        }

    }

    public class DistributorData
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Amount { get; set; }
    }
}