using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.Discounts;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerBS : CustomPage
    {
     
    readonly IUserSaveRepository _userSave;
    readonly DemandsSearchService _searchService;
    public ResellerBS(){
        _userSave = new UserSaveRepository();
     
       
        _searchService = new DemandsSearchService(IspDataContext);
       // _domian = new IspDomian(IspDataContext);
      //  _ispEntries=new IspEntries(IspDataContext);
    
    }


    protected void Page_Load(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            if(IsPostBack) return;
            Bind_ddl_Reseller(context);
            PopulateReciepCnfg(context);
            PopulateSaves();
            var option = context.Options.FirstOrDefault();
            if (option != null && Convert.ToBoolean(option.WidthOfReciept)) tb_Receipt.Style["width"] = "8cm";
            var first =
                context.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]))
                    .Select(usr => usr.Group.DataLevelID)
                    .FirstOrDefault();
            if(first == null) return;

            var dataLevel = first.Value;
            if(dataLevel != 3) return;
            lbl_SelectProcess.Visible = false;
            Select1.Visible = false;

        }
    }
    void PopulateSaves()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
            {
                a.Save.Id,
                a.Save.SaveName
            });
            ddlSaves.DataBind();
            Helper.AddDefaultItem(ddlSaves);
        }
    }

    void PopulateReciepCnfg(ISPDataContext context){
        var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
        if(user == null)return;
        var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
        if(cnfg != null){
            ImgLogo.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
            LCaution.Text = cnfg.Caution;
            DAddress.InnerHtml = cnfg.ContactData;
        }
    }


    void Bind_ddl_Reseller(ISPDataContext context){
        var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
        if(user == null)return;
        ddl_Reseller.SelectedValue = null;
        ddl_Reseller.Items.Clear();

        ddl_Reseller.AppendDataBoundItems = true;
        var query = DataLevelClass.GetUserReseller();

        var q = query.Where(x => x.AccountmanagerId == user.ID).ToList();
        if (q.Count > 0)
        {
            query = query.Where(x => x.AccountmanagerId == user.ID).ToList();
        }

        ddl_Reseller.DataSource = query;
        ddl_Reseller.DataBind();
        Helper.AddDefaultItem(ddl_Reseller);
    }


    protected void btn_search_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (ddl_Reseller.SelectedIndex == 0) return;
            var reseller = context.Users.FirstOrDefault(user => user.ID == Convert.ToInt32(ddl_Reseller.SelectedItem.Value));
            if(reseller == null) return;
            tb_SearchResult.Visible = true;
            ViewState.Add("ID", reseller.ID);
            ViewState.Add("BranchID", reseller.BranchID);
            ChkboxActiveResellerDiv.Visible = Convert.ToBoolean(reseller.IsAccountStopped) ? true : false;
            Bind_grd_Transactions();
            //rbl_Distination.Items[1].Enabled = reseller.BranchID != 1 ? (rbl_Distination.Items[2].Enabled = false) : (rbl_Distination.Items[2].Enabled = true);
            GetResellerDiscount(reseller.ID,context);
            var first = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"]));//.Select(usr => usr.Group.DataLevelID).First();
            if(first == null) return;
            if (first.Group.DataLevelID == null) return;
            var dataLevel = first.Group.DataLevelID.Value;
            switch (dataLevel)
            {
                case 1:
                    tb_ProcessPayment.Visible = true;
                    break;
                case 2:
                    tb_ProcessPayment.Visible = true;
                    break;
                default:
                    tb_ProcessPayment.Visible = false;
                    break;
            }
            if (tb_ProcessPayment.Visible)
            {
                var groupPrivilegeQuery =
                    context.GroupPrivileges.Where(gp => gp.Group.ID == first.GroupID).Select(gp => gp.privilege.Name);
                var payment = groupPrivilegeQuery.Contains("Payment");
                if (!payment)
                {
                    Select1.Items.Clear();
                    Select1.Items.Add(Tokens.Chose);
                    Select1.Items.Add(Tokens.Payment);
                }
                else
                {
                    Select1.Items.Clear();
                    Select1.Items.Add(Tokens.Chose);
                    Select1.Items.Add(Tokens.Add);
                    Select1.Items.Add(Tokens.Subtract);
                    Select1.Items.Add(Tokens.Payment);
                }
                
            }
        }
    }


    void GetResellerDiscount(int resellerId,ISPDataContext context){
        var query = from resellerDiscount in context.ResellerPackagesDiscounts
            where resellerDiscount.ResellerId == resellerId
            select new{
                resellerDiscount.ServiceProvider.SPName,
                SPTName=resellerDiscount.ServicePackage.ServicePackageName,
              DiscountPercent=  resellerDiscount.Discount
            };
        grd_Discount.DataSource = query;
        grd_Discount.DataBind();
    }


    protected void grd_Transactions_PageIndexChanging(object sender, GridViewPageEventArgs e){
        grd_Transactions.PageIndex = e.NewPageIndex;
        Bind_grd_Transactions();
    }

    void Bind_grd_Transactions(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
           
            var query = context.UsersTransactions
                .Where(userTransaction => userTransaction.ResellerID == Convert.ToInt32(ViewState["ID"]))
                .OrderByDescending(x => x.ID)
                .Select(x => new{
                    DepitAmmount = Helper.FixNumberFormat(x.DepitAmmount),
                    CreditAmmount = Helper.FixNumberFormat(x.CreditAmmount),
                    x.CreationDate,
                    note=x.Notes,
                    des=x.Description,
                    Descriptions=string.IsNullOrWhiteSpace(x.Notes)?x.Description:x.Description+" - "+x.Notes,
                    Total = Helper.FixNumberFormat(Convert.ToDouble(x.Total)),
                    User = x.UserId==null?"-":x.User1.UserName,
                    x.FileUrl
                }).ToList();
            grd_Transactions.DataSource = query;
            grd_Transactions.DataBind();

            //todo: task 28-4-2015
        
            var context2 = new Db.ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);



            var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
            {
                Paid = false,
                ResellerId = Helper.GetDropValue(ddl_Reseller),
                Month = Convert.ToInt32(DateTime.Now.Month),
                Year = Convert.ToInt32(DateTime.Now.Year),
                WithResellerDiscount = true

            });
            //var ResellerId =Convert.ToInt32( Helper.GetDropValue(ddl_Reseller));
            //var Year = Convert.ToInt32(DateTime.Now.Year);
            //var Month = Convert.ToInt32(DateTime.Now.Month);
            //var searchDemands2 =
            //    context.Demands.Where(z => z.IsResellerCommisstions == true && z.Paid == false && z.Month == Month
            //                               && z.Year == Year && z.WorkOrder.ResellerID == ResellerId)
            //        .Select(z => z)
            //        .ToList();

            var newlist = new List<DemandPreviewModel>();
        var sp = context2.SPoptionReselleraccounts.Select(z => z).ToList();
            foreach (var i in sp)
            {

                var data = searchDemands.Where(a => a.Provider == i.ServiceProvider.SPName).ToList();
             
                    newlist.AddRange(data);
                }



            var report = (Tokens.currentpill +" : "+ Helper.FixNumberFormat((newlist.Sum(x => x.ResellerNet))));
            lblcurrentdemand.Visible = true;
            lblcurrentdemand.Text = report;

          /*  var query2 = context.UsersTransactions
                .Where(userTransaction => userTransaction.ResellerID == Convert.ToInt32(ViewState["ID"]))
                .OrderByDescending(x => x.ID)
                .Select(x => x.CreditAmmount).ToList().Sum();
            var report2 = (Tokens.CreditBS + " : " + Helper.FixNumberFormat((query2)));
            dayen.Visible = true;
            dayen.Text = report2;


            var query3 = context.UsersTransactions
              .Where(userTransaction => userTransaction.ResellerID == Convert.ToInt32(ViewState["ID"]))
              .OrderByDescending(x => x.ID)
              .Select(x => x.DepitAmmount).ToList().Sum();
            var report3 = (Tokens.DepitBS + " : " + Helper.FixNumberFormat((query3)));
            maden.Visible = true;
            maden.Text = report3;*/

            //added by mr.ashraf
            decimal lasttoal = 0;
            if (query.Count != 0)
            {
                lasttoal = Convert.ToDecimal(query.First().Total);

            }
            var res = Tokens.MenuResellerCredit + " : " + Convert.ToDouble(lasttoal + Convert.ToDecimal(newlist.Sum(x => x.ResellerNet)));



           // var res = Tokens.MenuResellerCredit + " : " + Convert.ToDouble(Convert.ToDecimal(query.First().Total) + Convert.ToDecimal(newlist.Sum(x => x.ResellerNet)));
            saf.Visible = true;
            saf.Text = res;


            //if (query.Count > 0)
            //{
            //    lblResellerCredit.InnerHtml = string.Format(Tokens.MenuResellerCredit + " : {0}", query.First().Total);
            //    lblResellerCredit.Attributes.Add("class", "alert alert-info");
                
            //}
          
        }
    }


    protected void btn_Add_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var userTransaction = new UsersTransaction{
                CreationDate = DateTime.Now.AddHours(),
                DepitAmmount = Convert.ToDouble(txt_Add.Text),
                CreditAmmount = 0,
                IsInvoice = false,
                ResellerID = Convert.ToInt32(ViewState["ID"]),
                Total = Billing.GetLastBalance(Convert.ToInt32(ViewState["ID"]), "Reseller") + Convert.ToDouble(txt_Add.Text),
                Description = txt_AddComment.Text,
               
                UserId = Convert.ToInt32(Session["User_ID"])
            };
            context.UsersTransactions.InsertOnSubmit(userTransaction);
            context.SubmitChanges();
            Bind_grd_Transactions();
            Clear();
            divMessage.Visible = true;
        }
    }


    void Clear(){
        txt_Add.Text = string.Empty;
        txt_AddComment.Text = string.Empty;
        txt_Paid.Text = string.Empty;
        txt_Sub.Text = string.Empty;
        txt_PaidComment.Text = string.Empty;
        txt_SubComment.Text = string.Empty;


    }


    protected void btn_Sub_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var userTransaction = new UsersTransaction{
                CreationDate = DateTime.Now.AddHours(),
                DepitAmmount = 0,
                CreditAmmount = Convert.ToDouble(txt_Sub.Text),
                IsInvoice = false,
                ResellerID = Convert.ToInt32(ViewState["ID"]),
                Total = Billing.GetLastBalance(Convert.ToInt32(ViewState["ID"]), "Reseller") - Convert.ToDouble(txt_Sub.Text),
                Description = txt_SubComment.Text,
                
                UserId = Convert.ToInt32(Session["User_ID"])
            };
            context.UsersTransactions.InsertOnSubmit(userTransaction);
            context.SubmitChanges();
            Bind_grd_Transactions();
            Clear();
            divMessage.Visible = true;
        }
    }


    protected void btn_Payment_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            ViewState["ReceiptID"] = null;
            btn_ViewReceipt.Enabled = false;
           
            var userTransaction = new UsersTransaction{
                CreationDate = DateTime.Now.AddHours(),
                DepitAmmount = 0,
                CreditAmmount = Convert.ToDouble(txt_Paid.Text),
                IsInvoice = false,
                ResellerID = Convert.ToInt32(ViewState["ID"]),
                Total = Billing.GetLastBalance(Convert.ToInt32(ViewState["ID"]), "Reseller") - Convert.ToDouble(txt_Paid.Text),
                UserId = Convert.ToInt32(Session["User_ID"]),
                Description = "payment"
            };

            userTransaction.Notes = txt_PaidComment.Text;
            btn_ViewReceipt.Enabled = true; 
            tb_Paid.Style.Add("display", "block");
            
            context.UsersTransactions.InsertOnSubmit(userTransaction);
            context.SubmitChanges();
            

            //Save payment in save or banks  & keep history
            var resellername = context.Users.FirstOrDefault(x => x.ID == Convert.ToInt32(ViewState["ID"]));
          var notes =resellername!=null? txt_PaidComment.Text + " - " + resellername.UserName:txt_PaidComment.Text;
               
                var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                var userId = Convert.ToInt32(Session["User_ID"]);
               
                _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(txt_Paid.Text),
                    "Payment - من صفحة كشف حساب موزع", notes, context);

            if (resellername !=null && ChkboxActiveReseller.Checked)
            {
                resellername.IsAccountStopped = false;
                ChkboxActiveResellerDiv.Visible = false;
                ChkboxActiveReseller.Checked = false;
            }

            //Save Receipt
            var receiptNew = new Receipt{
                PrcessDate = DateTime.Now.AddHours(),
                UserTransationID = userTransaction.ID,
                Notes = txt_PaidComment.Text
            };
            context.Receipts.InsertOnSubmit(receiptNew);
            context.SubmitChanges();
            ViewState["ReceiptID"] = receiptNew.ID;
            Clear();
            divMessage.Visible = true;
            Bind_grd_Transactions();
            if (resellername != null && !string.IsNullOrEmpty(resellername.UserMobile))
            {
                var message = SendSms.SendSmsByNotification(context, resellername.UserMobile, 8);
                if (!string.IsNullOrEmpty(message))
                {
                    var myscript = "window.open('" + message + "')";
                    ClientScript.RegisterClientScriptBlock(typeof (Page), "myscript", myscript, true);
                }
            }
        }
    }


    protected void btn_ViewReceipt_Click(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var receiptId = Convert.ToDecimal(ViewState["ReceiptID"]);
            var currentReceipt = context.Receipts.First(res => res.ID == receiptId);
            var currentUsersTransaction = context.UsersTransactions.First(ut => ut.ID == currentReceipt.UserTransationID);
            lbl_CustomerName.Text = currentUsersTransaction.User.UserName;
            LBL_Amount.Text = currentUsersTransaction.CreditAmmount.ToString();
            lbl_For.Text = currentReceipt.Notes;
            lbl_ReceiptNo.Text = currentReceipt.ID.ToString(CultureInfo.InvariantCulture);
            lbl_dt.Text = currentReceipt.PrcessDate.ToString();
            lbl_User.Text = currentUsersTransaction.User1.UserName;
            mpe_Receipt.Show();
        }
    }

    protected void PutDownloadLinks(object sender, EventArgs e){
        foreach(GridViewRow row in grd_Transactions.Rows){
            var link = row.FindControl("hl") as HyperLink;
            if(link != null){
                if(!string.IsNullOrEmpty(link.ToolTip)){
                    link.Visible = true;
                    link.NavigateUrl = string.Format("../ExcelTemplates/ResselerPaidDemands/{0}", link.ToolTip);
                    link.ToolTip = string.Empty;
                } else{
                    link.Visible = false;
                }
            }
        }
    }
}
}