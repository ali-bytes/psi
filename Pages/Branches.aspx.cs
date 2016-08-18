using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Branches : CustomPage
    {
        
            // readonly ISPDataContext _context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);


            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    Bind_grd();
                    Bind_ddl_Admins();
                }
            }


            protected void btn_Insert_Click(object sender, EventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    var item = new Branch
                    {
                        BranchName = txt_BranchName.Text.Trim(),
                        AdminID = Convert.ToInt32(ddl_Admins.SelectedItem.Value),
                        BranchAddress = txt_BranchAddress.Text.Trim(),
                        Fax = txt_Fax.Text.Trim(),
                        Mobile1 = txt_Mobile1.Text.Trim(),
                        Mobile2 = txt_Mobile2.Text.Trim(),
                        Phone1 = txt_Phone1.Text.Trim(),
                        Phone2 = txt_Phone2.Text.Trim()
                    };
                    context.Branches.InsertOnSubmit(item);
                    context.SubmitChanges();

                    //Add Branch Save
                    var branchSave = new BranchesSave
                    {
                        BranchID = item.ID,
                        SaveValue = 0,
                        UpdateDate = DateTime.Now.AddHours()
                    };
                    context.BranchesSaves.InsertOnSubmit(branchSave);
                    context.SubmitChanges();

                    //Add Bank1 & Bank2 for the first branch only
                    if (item.ID == 1)
                    {
                        var bank1 = new BranchesBank
                        {
                            BankName = "Bank1",
                            BankValue = 0,
                            BranchID = 1,
                            UpdateDate = DateTime.Now.AddHours()
                        };
                        context.BranchesBanks.InsertOnSubmit(bank1);
                        context.SubmitChanges();

                        var bank2 = new BranchesBank
                        {
                            BankName = "Bank2",
                            BankValue = 0,
                            BranchID = 1,
                            UpdateDate = DateTime.Now.AddHours()
                        };
                        context.BranchesBanks.InsertOnSubmit(bank2);
                        context.SubmitChanges();
                    }

                    //Update BranchAdmin 
                    var branchAdminUser = context.Users.First(usr => usr.ID == Convert.ToInt32(ddl_Admins.SelectedItem.Value));
                    branchAdminUser.BranchID = item.ID;
                    context.SubmitChanges();

                    lbl_InsertResult.Text = Tokens.ItemAdded;
                    lbl_InsertResult.ForeColor = Color.Green;
                    Bind_grd();
                }
            }



            protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
            {
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    grd.EditIndex = e.NewEditIndex;
                    Bind_grd();
                    var ddlusers = ((DropDownList)(grd.Rows[e.NewEditIndex].FindControl("ddl_Users_Grd")));
                    var query = context.Users.Where(item => item.Group.ID == 1 || item.Group.ID == 4).Select(item => new
                    {
                        item.ID,
                        item.UserName
                    });
                    ddlusers.DataSource = query;
                    ddlusers.DataBind();
                    var dataKey = grd.DataKeys[e.NewEditIndex];
                    if (dataKey != null)
                        ddlusers.SelectedValue = dataKey["UserID"].ToString();
                }
            }


            protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var dataKey = grd.DataKeys[e.RowIndex];
                    if (dataKey == null) return;
                    var id = Convert.ToInt32(dataKey["ID"]);
                    DeleteBranch(context1, id);
                }
            }


            void DeleteBranch(ISPDataContext context, int id)
            {
                var query = context.Branches.Where(item => item.ID == id);
                var branch = query.First();
                //var users = context.Users.Where(a => a.BranchID == id);
                if (branch.WorkOrders.Any() || branch.Users.Any())
                {//||users.Any()){
                    lbl_InsertResult.Text = Tokens.CantDelete;
                    return;
                }
                try
                {
                    //context.ResellerCredits.DeleteAllOnSubmit(branch.ResellerCredits);
                    context.ReceiptCnfgs.DeleteAllOnSubmit(branch.ReceiptCnfgs);
                    context.UserBranches.DeleteAllOnSubmit(branch.UserBranches);
                    context.SubmitChanges();

                    List<Receipt> addrec = new List<Receipt>();
                    var userinres = context.UsersTransactions.Where(x => x.BranchID == branch.ID).ToList();
                    foreach (var usersTransaction in userinres)
                    {
                        var resrec =
                            context.Receipts.FirstOrDefault(x => x.UserTransationID == usersTransaction.ID);

                        if (resrec != null) addrec.Add(resrec);
                    }

                    context.Receipts.DeleteAllOnSubmit(addrec);

                    context.SubmitChanges();



                    context.UsersTransactions.DeleteAllOnSubmit(branch.UsersTransactions);
                    context.SubmitChanges();
                    //context.WorkOrderHistories.DeleteAllOnSubmit(branch.WorkOrderHistories);
                    context.OutgoingExpenses.DeleteAllOnSubmit(branch.OutgoingExpenses);
                    context.BranchesSaves.DeleteAllOnSubmit(branch.BranchesSaves);
                    context.BranchesBanksHistories.DeleteAllOnSubmit(branch.BranchesBanksHistories);
                  
                    context.BranchesBanks.DeleteAllOnSubmit(branch.BranchesBanks);
                    context.IncomingExpenses.DeleteAllOnSubmit(branch.IncomingExpenses);
                    context.BranchesDiscounts.DeleteAllOnSubmit(branch.BranchesDiscounts);
                    context.BranchCredits.DeleteAllOnSubmit(branch.BranchCredits);


                   var savesList= context.Saves.Where(a => a.BranchId == branch.ID).ToList();

                   List<UserSavesHistory> userSavesHistory = new List<UserSavesHistory>();
                   foreach (var sve in savesList)
                    {
                        var sav =
                            context.UserSavesHistories.Where(x => x.SaveId==sve.Id).ToList();

                        if (sav.Count > 0) userSavesHistory.AddRange(sav);
                    }
                   context.UserSavesHistories.DeleteAllOnSubmit(userSavesHistory);
                   context.SubmitChanges();


                   List<UserSave> userSaves = new List<UserSave>();
                    foreach (var usersve in savesList)
                    {
                        var usav =
                            context.UserSaves.Where(x => x.SaveId == usersve.Id).ToList();

                        if (usav.Count > 0) userSaves.AddRange(usav);
                    }
                    context.UserSaves.DeleteAllOnSubmit(userSaves);
                    context.SubmitChanges();



                    context.Saves.DeleteAllOnSubmit(branch.Saves);
                    //context.Users.DeleteAllOnSubmit(branch.Users);
                    context.SubmitChanges();
                    context.RechargeRequestBranches.DeleteAllOnSubmit(branch.RechargeRequestBranches);
                    context.BranchCreditVoices.DeleteAllOnSubmit(branch.BranchCreditVoices);
                    context.RechargeBranchRequests.DeleteAllOnSubmit(branch.RechargeBranchRequests);
                    context.BranchPackagesDiscounts.DeleteAllOnSubmit(branch.BranchPackagesDiscounts);
                    context.OfferBranches.DeleteAllOnSubmit(branch.OfferBranches);
                   

                    context.Branches.DeleteOnSubmit(branch);
                    context.SubmitChanges();
                    lbl_InsertResult.Text = Tokens.Deleted;
                }
                catch (Exception ex)
                {

                    lbl_InsertResult.Text = string.Format("Error Details :{0}", ex.Message);
                }

                Bind_grd();
            }


            protected void grd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
            {
                grd.EditIndex = -1;
                Bind_grd();
            }


            protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    int id = Convert.ToInt32(grd.DataKeys[e.RowIndex]["ID"]);
                    var Query = context2.Branches.Where(Item => Item.ID == id);
                    Branch Entity = Query.First();
                    Entity.BranchName = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox1"))).Text.Trim();
                    Entity.AdminID = Convert.ToInt32(((DropDownList)(grd.Rows[e.RowIndex].FindControl("ddl_Users_Grd"))).SelectedItem.Value);
                    Entity.BranchAddress = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox2"))).Text.Trim();
                    Entity.Fax = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox7"))).Text.Trim();
                    Entity.Mobile1 = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox5"))).Text.Trim();
                    Entity.Mobile2 = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox6"))).Text.Trim();
                    Entity.Phone1 = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox3"))).Text.Trim();
                    Entity.Phone2 = ((TextBox)(grd.Rows[e.RowIndex].FindControl("TextBox4"))).Text.Trim();
                    context2.SubmitChanges();

                    var userid = Convert.ToInt32(((DropDownList)(grd.Rows[e.RowIndex].FindControl("ddl_Users_Grd"))).SelectedItem.Value);
                    User BranchAdminUser = context2.Users.First(usr => usr.ID == userid);
                    BranchAdminUser.BranchID = Entity.ID;
                    context2.SubmitChanges();

                    grd.EditIndex = -1;
                    Bind_grd();

                    // update user branch id

                }
            }



            void Bind_ddl_Admins()
            {
                using (var context3 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Query = context3.Users.Where(Item => Item.Group.ID == 4).Select(Item => new
                    {
                        Item.ID,
                        Item.UserName
                    });
                    ddl_Admins.DataSource = Query;
                    ddl_Admins.DataBind();
                    Helper.AddDefaultItem(ddl_Admins);
                }
            }


            void Bind_grd()
            {
                using (var context4 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var Query = from Item in context4.Branches
                                select new
                                {
                                    Admin = Item.User.UserName,
                                    UserID = Item.User.ID,
                                    Item.AdminID,
                                    Item.BranchAddress,
                                    Item.BranchName,
                                    Item.Fax,
                                    Item.ID,
                                    Item.Mobile1,
                                    Item.Mobile2,
                                    Item.Phone1,
                                    Item.Phone2
                                };
                    grd.DataSource = Query;
                    grd.DataBind();
                }
            }



            protected void BDel_OnClick(object sender, EventArgs e)
            {
                using (var context5 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var btn = sender as LinkButton;
                    if (btn == null) return;

                    var id = Convert.ToInt32(btn.CommandArgument);
                    DeleteBranch(context5, id);
                }
            }
        }
    }
 