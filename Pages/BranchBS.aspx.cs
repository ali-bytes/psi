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
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchBS : CustomPage
    {
        static readonly ISPDataContext context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);

        DemandsSearchService _searchService = new DemandsSearchService(context);
            readonly IUserSaveRepository _userSave = new UserSaveRepository();
            //system admin only
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        Bind_ddl_Branchs();
                        PopulateReciepCnfg(context);
                        PopulateSaves(context);
                        var option = context.Options.FirstOrDefault();
                        if (option != null && !Convert.ToBoolean(option.WidthOfReciept)) tb_Receipt.Style["width"] = "8cm";
                    }
                }
            }

            void PopulateSaves(ISPDataContext context)
            {
                //using (var context=new ISPDataContext())
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

            void PopulateReciepCnfg(ISPDataContext context)
            {
                //using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null) return;
                var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
                if (cnfg != null)
                {
                    ImgLogo.ImageUrl = "../PrintLogos/" + cnfg.LogoUrl;
                    LCaution.Text = cnfg.Caution;
                    DAddress.InnerHtml = cnfg.ContactData;
                }
                // }
            }


            void Bind_ddl_Branchs()
            {
                ddl_Branchs.DataSource = DataLevelClass.GetUserBranches();
                ddl_Branchs.DataBind();
                Helper.AddDefaultItem(ddl_Branchs);
                //ddl_Branchs.SelectedValue = "-1";
            }


            protected void btn_search_Click(object sender, EventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var branch = context1.Branches.FirstOrDefault(brnch => brnch.ID == Convert.ToInt32(ddl_Branchs.SelectedItem.Value));

                    if (branch == null) return;
                    tb_SearchResult.Visible = true;
                    ViewState.Add("ID", branch.ID);
                    ViewState.Add("BranchID", branch.ID);
                    Bind_grd_Transactions();
                    //rbl_Distination.Items[1].Enabled = rbl_Distination.Items[2].Enabled = true;
                    GetBranchDiscount(branch.ID);

                    var first = context1.Users.Where(usr => usr.ID == Convert.ToInt32(HttpContext.Current.Session["User_ID"])).Select(usr => usr.Group.DataLevelID).First();
                    if (first == null) return;
                    int DataLevel = first.Value;
                    tb_ProcessPayment.Visible = DataLevel == 1;









                    var searchDemands = _searchService.BranchUnpaidDemandsPreview(new BasicSearchModel
                    {
                        BranchPaid = false,
                        BranchId = Helper.GetDropValue(ddl_Branchs),
                        Month = Convert.ToInt32(DateTime.Now.Month),
                        Year = Convert.ToInt32(DateTime.Now.Year),
                        WithBranchDiscount = true

                    });
                    var newlist = new List<DemandPreviewModel>();
                    var sp = context1.SPoptionReselleraccounts.Select(z => z).ToList();
                    foreach (var i in sp)
                    {

                        var data = searchDemands.Where(a => a.Provider == i.ServiceProvider.SPName).ToList();

                        newlist.AddRange(data);
                    }
                    var report = (Tokens.currentpill + " : " + Helper.FixNumberFormat((newlist.Sum(x => x.BranchNet))));
                    lblcurrentdemand.Visible = true;
                    lblcurrentdemand.Text = report;


                    var query = context1
                       .UsersTransactions
                       .Where(t => t.BranchID == Convert.ToInt32(ViewState["ID"]))
                       .OrderByDescending(x => x.ID)
                       .Select(x => new
                       {
                           Total = Helper.FixNumberFormat(Convert.ToDouble(x.Total)),
                           DepitAmmount = Helper.FixNumberFormat(x.DepitAmmount),
                           CreditAmmount = Helper.FixNumberFormat(x.CreditAmmount),
                           x.Description,
                           x.CreationDate,
                           User = context1.Users.FirstOrDefault(u => u.ID == x.UserId) == null ? "-" : context1.Users.FirstOrDefault(u => u.ID == x.UserId).UserName,
                           FileUrl = x.FileUrl
                       }).ToList(); ;
                    decimal lasttoal = 0;
                    if (query.Count != 0)
                    {
                        lasttoal = Convert.ToDecimal(query.First().Total);

                    }
                    var res = Tokens.MenuBranchCredit + " : " + Convert.ToDouble(lasttoal + Convert.ToDecimal(newlist.Sum(x => x.BranchNet)));



                    saf.Visible = true;
                    saf.Text = res;
                }
            }


            void GetBranchDiscount(int BranchID)
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = context2.BranchesDiscounts.Where(branchDiscount => branchDiscount.BranchID == BranchID).Select(d => new
                    {
                        d.ServiceProvider.SPName,
                        d.ServicePackagesType.SPTName,
                        d.DiscountPercent
                    });
                    grd_Discount.DataSource = query;
                    grd_Discount.DataBind();
                }
            }


            protected void grd_Transactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                grd_Transactions.PageIndex = e.NewPageIndex;
                Bind_grd_Transactions();
            }


            void Bind_grd_Transactions()
            {
                using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var query = dataContext
                        .UsersTransactions
                        .Where(t => t.BranchID == Convert.ToInt32(ViewState["ID"]))
                        .OrderByDescending(x => x.ID)
                        .Select(x => new
                        {
                            Total = Helper.FixNumberFormat(Convert.ToDouble(x.Total)),
                            DepitAmmount = Helper.FixNumberFormat(x.DepitAmmount),
                            CreditAmmount = Helper.FixNumberFormat(x.CreditAmmount),
                            x.Description,
                            x.CreationDate,
                            User = dataContext.Users.FirstOrDefault(u => u.ID == x.UserId) == null ? "-" : dataContext.Users.FirstOrDefault(u => u.ID == x.UserId).UserName,
                            FileUrl = x.FileUrl
                        });
                    grd_Transactions.DataSource = query;
                    grd_Transactions.DataBind();
                }
            }


            protected void btn_Add_Click(object sender, EventArgs e)
            {
                using (var dataContext3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userTransaction = new UsersTransaction
                    {
                        CreationDate = DateTime.Now.AddHours(),
                        DepitAmmount = Convert.ToDouble(txt_Add.Text),
                        CreditAmmount = 0,
                        IsInvoice = false,
                        BranchID = Convert.ToInt32(ViewState["ID"]),
                        Total = Billing.GetLastBalance(Convert.ToInt32(ViewState["ID"]), "Branch") + Convert.ToDouble(txt_Add.Text),
                        Description = txt_AddComment.Text,
                        UserId = Convert.ToInt32(Session["User_ID"])
                    };
                    dataContext3.UsersTransactions.InsertOnSubmit(userTransaction);
                    dataContext3.SubmitChanges();
                    Bind_grd_Transactions();
                }
            }


            protected void btn_Sub_Click(object sender, EventArgs e)
            {
                using (var DataContext4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var userTransaction = new UsersTransaction
                    {
                        CreationDate = DateTime.Now.AddHours(),
                        DepitAmmount = 0,
                        CreditAmmount = Convert.ToDouble(txt_Sub.Text),
                        IsInvoice = false,
                        BranchID = Convert.ToInt32(ViewState["ID"]),
                        Total = Billing.GetLastBalance(Convert.ToInt32(ViewState["ID"]), "Branch") - Convert.ToDouble(txt_Sub.Text),
                        Description = txt_SubComment.Text,
                        UserId = Convert.ToInt32(Session["User_ID"])
                    };
                    DataContext4.UsersTransactions.InsertOnSubmit(userTransaction);
                    DataContext4.SubmitChanges();
                    Bind_grd_Transactions();
                }
            }


            protected void btn_Payment_Click(object sender, EventArgs e)
            {
                using (var DataContext5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    ViewState["ReceiptID"] = null;
                    btn_ViewReceipt.Enabled = false;
                    string VirtualName = DateTime.Now.AddHours().ToFileTime().ToString();
                    string FileName = "";
                   
                    var userTransaction = new UsersTransaction
                    {
                        CreationDate = DateTime.Now.AddHours(),
                        DepitAmmount = 0,
                        CreditAmmount = Convert.ToDouble(txt_Paid.Text),
                        IsInvoice = false,
                        BranchID = Convert.ToInt32(ViewState["ID"]),
                        Total = Billing.GetLastBalance(Convert.ToInt32(ViewState["ID"]), "Branch") - Convert.ToDouble(txt_Paid.Text),
                        UserId = Convert.ToInt32(Session["User_ID"]),
                        Description = "payment"
                    };

                    userTransaction.Notes = txt_PaidComment.Text;
                    btn_ViewReceipt.Enabled = true; //Enabled viewReceipt Button
                    tb_Paid.Style.Add("display", "block");

                    DataContext5.UsersTransactions.InsertOnSubmit(userTransaction);
                    DataContext5.SubmitChanges();
                    Bind_grd_Transactions();

                    //Save payment in save or banks  & keep history
                    var branchname = DataContext5.Branches.FirstOrDefault(x => x.ID == Convert.ToInt32(ViewState["ID"]));
                 
                        var notes = " كشف حساب فرع " + txt_PaidComment.Text + " - " + branchname.BranchName;
                      
                        DataContext5.SubmitChanges();
                        var userId = Convert.ToInt32(Session["User_ID"]);
                        var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                        _userSave.BranchAndUserSaves(saveId, userId, Convert.ToDouble(txt_Paid.Text), "payment",
                            notes, DataContext5);
                  
                    //Save Receipt
                    var Receipt_New = new Receipt
                    {
                        PrcessDate = DateTime.Now.AddHours(),
                        UserTransationID = userTransaction.ID,
                        Notes = txt_PaidComment.Text
                    };
                    DataContext5.Receipts.InsertOnSubmit(Receipt_New);
                    DataContext5.SubmitChanges();
                    ViewState["ReceiptID"] = Receipt_New.ID;
                    if (branchname != null && !string.IsNullOrEmpty(branchname.Mobile1))
                    {
                        var message = SendSms.SendSmsByNotification(DataContext5, branchname.Mobile1, 9);
                        if (!string.IsNullOrEmpty(message))
                        {
                            var myscript = "window.open('" + message + "')";
                            ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                        }
                    }
                }
            }


            protected void btn_ViewReceipt_Click(object sender, EventArgs e)
            {
                using (var dataContext6 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    decimal receiptId = Convert.ToDecimal(ViewState["ReceiptID"]);
                    Receipt currentReceipt = dataContext6.Receipts.First(res => res.ID == receiptId);
                    UsersTransaction currentUsersTransaction = dataContext6.UsersTransactions.First(ut => ut.ID == currentReceipt.UserTransationID);
                    lbl_CustomerName.Text = currentUsersTransaction.Branch.BranchName;
                    LBL_Amount.Text = currentUsersTransaction.CreditAmmount.ToString();
                    lbl_For.Text = currentReceipt.Notes;
                    lbl_ReceiptNo.Text = currentReceipt.ID.ToString(CultureInfo.InvariantCulture);
                    lbl_dt.Text = currentReceipt.PrcessDate.ToString();
                    lbl_User.Text = currentUsersTransaction.User1.UserName;
                    mpe_Receipt.Show();
                }
            }
            protected void PutDownloadLinks(object sender, EventArgs e)
            {
                foreach (GridViewRow row in grd_Transactions.Rows)
                {
                    var link = row.FindControl("hl") as HyperLink;
                    if (link != null)
                    {
                        if (!string.IsNullOrEmpty(link.ToolTip))
                        {
                            link.Visible = true;
                            link.NavigateUrl = string.Format("../ExcelTemplates/BranchPaidDemands/{0}", link.ToolTip);
                            link.ToolTip = string.Empty;
                        }
                        else
                        {
                            link.Visible = false;
                        }
                    }
                }
            }
        }
    }
 