using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain.Abstract;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class Expenses : CustomPage
    {

        private readonly IUserSaveRepository _userSave;

        public Expenses()
        {
            _userSave = new UserSaveRepository();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var query = Request.QueryString["tp"];
                switch (query)
                {
                    case "in":
                        Page.Title = lblTitle.Text = Tokens.Expensesout;
                        Label7.Text = Tokens.IncomeTypes;
                        break;
                    case "out":
                        Page.Title = lblTitle.Text = Tokens.Expenses;
                        Label7.Text = Tokens.OutgoingTypes;
                        break;
                }
                if (IsPostBack) return;
                switch (query)
                {
                    case "in":
                        tr_OutGoingType.Visible = true;
                        Bind_ddlIncomeType(context);
                        break;
                    case "out":
                        tr_OutGoingType.Visible = true;
                        Bind_ddl_OutgoingTypes(context);
                        break;
                    default:
                        Response.Redirect("ErrorPage.aspx");
                        break;
                }

             var user = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null) return;
               ViewState.Add("BranchID", user.BranchID);
               
                PopulateSvaes();
            }
        }

        void PopulateSvaes()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var userId = Convert.ToInt32(Session["User_ID"]);
                ddlSaves.DataSource = _userSave.SavesOfUser(userId, context).Select(a => new
                {
                    a.Save.SaveName,
                    a.Save.Id
                });
                ddlSaves.DataBind();
                Helper.AddDefaultItem(ddlSaves);
            }
        }
        void Bind_ddl_OutgoingTypes(ISPDataContext context)
        {
            var outgoingTypes = context.OutgoingTypes;
            ddl_OutgoingTypes.DataSource = outgoingTypes;
            ddl_OutgoingTypes.DataBind();
            Helper.AddDefaultItem(ddl_OutgoingTypes);
        }

        void Bind_ddlIncomeType(ISPDataContext context)
        {
            var incomingtypes = context.RevenueTypes;
            ddl_OutgoingTypes.DataSource = incomingtypes;
            ddl_OutgoingTypes.DataBind();
            Helper.AddAllDefaultItem(ddl_OutgoingTypes);
        }


        protected void btn_Payment_Click(object sender, EventArgs e)
        {
            using (var context1 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var now = DateTime.Now.AddHours();
                var activeUser = Convert.ToInt32(Session["User_ID"]);
                switch (Request.QueryString["tp"])
                {

                    case "in":
                        {
                            var incom = new IncomingExpense
                            {
                                BranchID = Convert.ToInt32(ViewState["BranchID"]),
                                Comment = txt_PaidComment.Text + ddl_OutgoingTypes.SelectedItem,
                                Value = Convert.ToDouble(txt_Paid.Text),
                                UserId = activeUser,
                                Date = now,
                                RevenueTypeId = Convert.ToInt32(ddl_OutgoingTypes.SelectedValue)

                            };
                            context1.IncomingExpenses.InsertOnSubmit(incom);
                            context1.SubmitChanges();
                        }
                        break;
                    case "out":
                        {

                            var saveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);
                            if (_userSave.CanSave(saveId, Convert.ToDecimal(txt_Paid.Text) * -1, context1))
                            {
                                var outgo = new OutgoingExpense
                                {
                                    BranchID = Convert.ToInt32(ViewState["BranchID"]),
                                    Comment = txt_PaidComment.Text + ddl_OutgoingTypes.SelectedItem,
                                    Value = Convert.ToDouble(txt_Paid.Text),
                                    OutgoingTypeID = Convert.ToInt32(ddl_OutgoingTypes.SelectedValue),
                                    UserId = activeUser,
                                    Date = now
                                };
                                context1.OutgoingExpenses.InsertOnSubmit(outgo);
                                context1.SubmitChanges();
                            }
                        }
                        break;
                }
               
                            var amount = Convert.ToDouble(txt_Paid.Text);
                              double am = 0;
                                var note2 = string.Empty;
                                var note = string.Empty;
                                switch (Request.QueryString["tp"])
                                {
                                    case "in":
                                        note = "ايرادات اخرى";
                                        am = amount;
                                       
                                        break;
                                    case "out":
                                        note = "مصروفات";
                                        am = amount * -1;

                                        break;
                                }
                                note2 = txt_PaidComment.Text + "  _  " + ddl_OutgoingTypes.SelectedItem;
                                var eSaveId = Convert.ToInt32(ddlSaves.SelectedItem.Value);


                                if (Request.QueryString["tp"] == "out")
                                {
                                    if (!_userSave.CanSave(eSaveId, Convert.ToDecimal(am), context1))
                                    {
                                        lbl_Process.Text = Tokens.CreditIsntEnough;
                                        return;
                                    }
                                }
                                _userSave.BranchAndUserSaves(eSaveId, activeUser, am, note, note2, context1);
                            
                            context1.SubmitChanges();
                          
                switch (Request.QueryString["tp"])
                {
                    case "in":
                        {
                            lbl_Process.Text = Tokens.Saved;
                        }
                        break;
                    case "out":
                        {
                            lbl_Process.Text = Tokens.Saved;
                           
                        }
                        break;
                }
                lbl_Process.ForeColor = Color.Green;
                PopulateModal(context1);
                mpe_Receipt.Show();
            }
        }

        void PopulateModal(ISPDataContext context)
        {
            var option = context.Options.FirstOrDefault();
            if (option != null && Convert.ToBoolean(option.WidthOfReciept)) datatable.Style["width"] = "8cm";
            var userId = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
            if (user == null) return;
            var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
            if (cnfg != null)
            {
                imglogo.Src = "../PrintLogos/" + cnfg.LogoUrl;
                lblCompanyName.Text = string.Format("الشركة : {0}", cnfg.CompanyName);
                lblBranch.Text = string.Format("الفرع : {0}", cnfg.Branch.BranchName);
            }
            lblDate.Text = string.Format("التاريخ : {0}", DateTime.Now.AddHours().ToShortDateString());
            lblEmployeName.Text = user.UserName;
            lblType.Text = Label7.Text;
            txtType.Text = ddl_OutgoingTypes.SelectedItem.Text;
            txtValue.Text = txt_Paid.Text;
            txtComment.Text = txt_PaidComment.Text;
            txtSaves.Text = ddlSaves.SelectedItem.Text;
        }
    }
}
