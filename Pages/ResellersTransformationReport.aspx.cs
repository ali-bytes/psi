using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellersTransformationReport : CustomPage
    {
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        BuildUponUserType();
    }
    void BuildUponUserType()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var id = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(u => u.ID == id);
            if (user != null)
            {
                List<User> resellers;
                switch (user.GroupID)
                {
                    case 1: // admin
                        resellers = GetResellers(null,context);
                        break;
                    case 6: // reseller
                        resellers = context.Users.Where(u => u.ID == id).ToList();
                        break;
                    default:
                        resellers = GetResellers(id,context);
                        break;
                }
                ddlReseller.DataSource = resellers;
                ddlReseller.DataValueField = "ID";
                ddlReseller.DataTextField = "UserName";
                ddlReseller.DataBind();

                Helper.AddDefaultItem(ddlReseller);
                if (user.GroupID == 1) Helper.AddDropDownItem(ddlReseller, 1, Tokens.All);
             
            }
        }
    }


    List<User> GetResellers(int? id,ISPDataContext context)
    {
      
        {
            if (id != null)
            {
                var user = context.Users.FirstOrDefault(x => x.ID == id);
                if (user == null)
                {
                    Response.Redirect("default.aspx");
                }
                var resellers = context.Users.Where(g => g.GroupID == 6 && g.BranchID == user.BranchID).ToList();
                return resellers;
            }
            return context.Users.Where(x => x.GroupID == 6).ToList();
        }
    }


    protected void gv_customers_DataBound(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(gvRequests, "gv_lNumber");
        var rows = gvRequests.Rows;
        foreach (GridViewRow item in rows)
        {
            var lblStatus = item.FindControl("lblStatus") as Label;
          
            if (lblStatus != null) 
            {
                 PassStyle(lblStatus);
                
            }
            var lblfr = item.FindControl("lblFrom") as Label;
            var lblto = item.FindControl("lblTo") as Label;
            if (lblfr != null && lblto != null)
            {
                lblfr.Text = PassText(Convert.ToInt32(lblfr.Text));
                lblto.Text = PassText(Convert.ToInt32(lblto.Text));
            }
        }
    }

    static void PassStyle(Label lbl)
    {
        var tar = lbl.Text;
       
        switch (tar)
        {
            case "":
                lbl.Text = Tokens.PendingRequest;
                lbl.CssClass = "label label-warning arrowed-in-right arrowed";
                break;
            case "True":
                lbl.Text = Tokens.Approved;
                lbl.CssClass = "label label-success arrowed";
                break;
            case "False":
                lbl.Text = Tokens.RequestRejected;
                lbl.CssClass = "label label-danger arrowed-in";
                break;
        }
       // return text;
    }
    static string PassText(int tar)
    {
        var text = string.Empty;
        switch (tar)
        {
            case 1:
                text = Tokens.ResellerPaymentCredit;
                break;
            case 2:
                text = Tokens.ResellerVoiceCredit;
                break;
            case 3:
                text = Tokens.ResellerBalanceSheet;
                break;
        }
        return text;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            PopulateRequests(context);
        }
    }

    void PopulateRequests(ISPDataContext context)
    {
        var requests =context.ResellerTransformationRequests.ToList();
        if (ddlReseller.SelectedIndex > 1)
        {
            var resellerId = Helper.GetDropValue(ddlReseller);
            requests = requests.Where(a => a.ResellerId == resellerId).ToList();
        }
        if (!string.IsNullOrWhiteSpace(txtFrom.Text))
        {
            var dateFrom = Convert.ToDateTime(txtFrom.Text);
            requests = requests.Where(a =>a.date!=null && a.date.Value.Date >= dateFrom.Date).ToList();
        }
        if (!string.IsNullOrWhiteSpace(txtTo.Text))
        {
            var dateTo = Convert.ToDateTime(txtTo.Text);
            requests = requests.Where(a => a.date != null && a.date.Value.Date <= dateTo.Date).ToList();
        }
        var endRequests=requests.Select(a => new
                {
                    Reseller = a.User.UserName,
                    a.User1.UserName,
                    a.TransferFrom,
                    a.TransferTo,
                    RequestDate = a.date,
                    Amount = Helper.FixNumberFormat(a.Amount),
                    a.Id,
                    a.Status
                }).ToList();
        gvRequests.DataSource = endRequests;
        gvRequests.DataBind();
    }
}
}