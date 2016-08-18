using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class OutgoingReport : CustomPage
    {
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                PopulateOutgoing(context);
            }
        }
    }

    void PopulateOutgoing(ISPDataContext context)
    {
        var userId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
        var first = context.Users.Where(usr => usr.ID == userId).
            Select(usr => usr.Group.DataLevelID).First();
        var incoming = context.OutgoingTypes;
        ddlIncomeType.DataSource = incoming;
        ddlIncomeType.DataBind();
        if (first != null && first.Value == 1) Helper.AddDefaultItem(ddlIncomeType, "All");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (ddlIncomeType.SelectedIndex == 0)
            {
                Getdata(null, context);
            }
            if (ddlIncomeType.SelectedIndex != 0)
            {
                var outgoingId = Convert.ToInt32(ddlIncomeType.SelectedValue);
                Getdata(outgoingId, context);
            }
        }
    }

    private void Getdata(int? id, ISPDataContext context)
    {
        var incoming =
            GetByDataLevel(context).Where(a => a.Date != null && a.Date.Value.Date <= Convert.ToDateTime(tb_to.Text)
                                               && a.Date.Value.Date >= Convert.ToDateTime(tb_from.Text)).ToList();
        if (id != null)
        {
             incoming = incoming.Where(a => a.OutgoingTypeID == id).ToList();
        }
        var inc = incoming.Select(a => new
        {
            a.ID,
            a.Branch.BranchName,
            Amount = a.Value,
            a.Comment,
            a.Date,
            User = a.User.UserName,
            a.OutgoingType.Name
        }).ToList();
        grd.DataSource = inc;
        grd.DataBind();
        var sum = inc.Sum(a => a.Amount);
        lblTotal.Text = sum > 0 ? sum.ToString() : "0";

    }

    protected void GetTotal(object sender, EventArgs e)
    {
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var incoming = context.OutgoingTypes.ToList();
            var dataTotals=new List<ExpTotal>();
            foreach (var type in incoming)
            {
                var type1 = type;
                var expences =
                    GetByDataLevel(context).Where(a => a.OutgoingTypeID == type1.ID && a.Date!=null && a.Date.Value.Date <= Convert.ToDateTime(tb_to.Text)
                             && a.Date.Value.Date >= Convert.ToDateTime(tb_from.Text)).ToList();
                var expdata = new ExpTotal
                {
                    Name = type1.Name,
                    Total = Convert.ToDouble(expences.Sum(a => a.Value))
                };
                dataTotals.Add(expdata);
            }
            GVTotals.DataSource = dataTotals;
            GVTotals.DataBind();
        }
        
    }
    protected  List<OutgoingExpense> GetByDataLevel(ISPDataContext context)
    {
        if (HttpContext.Current.Session["User_ID"] == null) HttpContext.Current.Response.Redirect("../default.aspx");
        var userId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]);
        var first = context.Users.Where(usr => usr.ID == userId).
            Select(usr => usr.Group.DataLevelID).First();
        var requests = new List<OutgoingExpense>();//= null;
        if (first == null) return requests;
        var accountManager = context.Users.Where(a => a.AccountmanagerId == userId).ToList();
        if (accountManager.Count == 0)
        {
            var dataLevel = first.Value;
            switch (dataLevel)
            {
                case 1:
                    requests = context.OutgoingExpenses.ToList();
                    break;
                case 2:
                    requests =
                        context
                            .OutgoingExpenses
                            .Where(
                                wor =>DataLevelClass.GetBranchAdminBranchIDs(userId)
                                        .Contains(wor.BranchID)).ToList();
                    break;
                case 3:
                    requests = context.OutgoingExpenses.Where(wor =>wor.User.ID == userId).ToList();
                    break;
            }

        }
        else
        {
            //var c = 0;
            foreach (var user in accountManager)
            {
                var data = context.OutgoingExpenses.Where(wor =>wor.User.ID == user.ID).ToList();
                if (data.Count > 0) requests.AddRange(data);
            }
        }
        return requests;
    }
    protected void grd_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(grd, "l_Number");
    }

    protected void GVT_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GVTotals, "l_Number");
    }
}

    public class ExpTotal
    {
        public string Name { get; set; }
        public double Total { get; set; }
    }

}