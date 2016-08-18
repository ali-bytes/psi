using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Pages
{
    public partial class RevenuesAndExpenses : CustomPage
    {
     

    protected void Page_Load(object sender, EventArgs e) {}


    protected void b_search_Click(object sender, EventArgs e){
        PopulateRevenuesAndExpenses();
    }


    void PopulateRevenuesAndExpenses(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var userId = Session["User_ID"];
            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(userId));

            // if user doesn't exist or has no branch
            if(user == null || user.BranchID == null)
                return;

            var revenuesExpensesList = new List<RevenuesExpenses>();
            var revenueCash = context.IncomingExpenses.Where(ex => ex.BranchID == user.BranchID &&
                                                                   ex.Date.Value.Date <= Convert.ToDateTime(tb_to.Text)&&
                                                                   ex.Date.Value.Date >=Convert.ToDateTime(tb_from.Text)).ToList();
         

            var expenseCash = context.OutgoingExpenses.Where(ex => ex.BranchID == user.BranchID
                             && ex.Date.Value.Date <= Convert.ToDateTime(tb_to.Text)
                             && ex.Date.Value.Date >= Convert.ToDateTime(tb_from.Text)).ToList();


            if(expenseCash.Any()){
                foreach(var cashRevenue in expenseCash){
                    if(cashRevenue.Date != null)
                        
                            revenuesExpensesList.Add(new RevenuesExpenses{
                                Amount = cashRevenue.Value,
                                BranchName = cashRevenue.Branch.BranchName,
                                CashBank = CashBank.Cash,
                                Comment = cashRevenue.Comment,
                                Date = cashRevenue.Date.Value.Date,
                                Effect = Effect.Expense,
                                TableId = cashRevenue.ID,
                                BranchId = Convert.ToInt32(cashRevenue.BranchID),
                            UserId = Convert.ToInt32(cashRevenue.UserId)
                            });
                }
            }



            if(revenueCash.Any()){
                foreach(var cashExpense in revenueCash){
                    if(cashExpense.Date != null)
                        revenuesExpensesList.Add(new RevenuesExpenses{
                            Amount = cashExpense.Value,
                            BranchName = cashExpense.Branch.BranchName,
                            CashBank = CashBank.Cash,
                            Comment = cashExpense.Comment,
                            Date = cashExpense.Date.Value.Date,
                            Effect = Effect.Revenue,
                            TableId = cashExpense.ID,
                            BranchId = Convert.ToInt32(cashExpense.BranchID),
                            UserId = Convert.ToInt32(cashExpense.UserId)
                        });
                }
            }
           
            switch (user.Group.DataLevelID)
            {
                case 1:
                    break;
                default:
                    revenuesExpensesList =
                        revenuesExpensesList.Where(a => DataLevelClass.GetBranchAdminBranchIDs(user.ID)
                            .Contains(a.BranchId)).ToList();
                    break;
            }


            gv_RevenueExpenses.DataSource = revenuesExpensesList;
            gv_RevenueExpenses.DataBind();

            l_ExpensesTotal.Text = revenuesExpensesList
                .Where(re => re.Effect == Effect.Expense)
                .Sum(r => r.Amount)
                .ToString();
            l_RevenuesTotal.Text =
                revenuesExpensesList
                    .Where(re => re.Effect == Effect.Revenue)
                    .Sum(r => r.Amount).ToString();
        }
    }


    protected void gv_RevenueExpenses_DataBound(object sender, EventArgs e){
        Helper.GridViewNumbering(gv_RevenueExpenses, "l_Number");
    }
}
}