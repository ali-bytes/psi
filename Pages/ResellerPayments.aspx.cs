using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services.Discounts;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ResellerPayments : CustomPage
    {
      
    private readonly ResellerDiscountCalculator _resellerDiscound;

    public ResellerPayments()
    {
        var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        _resellerDiscound = new ResellerDiscountCalculator(context);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BuildUponUserType();
        }
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
                        resellers = GetResellers(null);
                        break;
                    case 6: // reseller
                        resellers = context.Users.Where(u => u.ID == id).ToList();
                        break;
                    default:
                        resellers = GetResellers(id);
                        break;
                }
                ddl_reseller.DataSource = resellers;
                ddl_reseller.DataValueField = "ID";
                ddl_reseller.DataTextField = "UserName";
                ddl_reseller.DataBind();

                Helper.AddDefaultItem(ddl_reseller);
                if(user.GroupID==1)Helper.AddDropDownItem(ddl_reseller, 1, Tokens.All);
                //AddDefaultItem(ddl_reseller,Tokens.All);
            }
        }
    }


    List<User> GetResellers(int? id)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
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

    protected void b_search_Click(object sender, EventArgs e)
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            if (RblSearch.SelectedIndex == 0)
            {
                if (ddl_reseller.SelectedIndex == 1 && ddl_reseller.SelectedItem.Value == "0")
                {
                    SearchInWorkOrderRequests(context, null);
                }
                else
                {
                    var resellerId = Convert.ToInt32(ddl_reseller.SelectedItem.Value);
                    SearchInWorkOrderRequests(context, resellerId);
                }

            }
            else
            {
              
                if (ddl_reseller.SelectedIndex == 1 && ddl_reseller.SelectedItem.Value == "0")
                {
                    SearchInDemand(context,null);
                }
                else
                {
                    var id = Convert.ToInt32(ddl_reseller.SelectedItem.Value);
                    SearchInDemand(context,id);

                }
            }
        }
    }


    protected void gv_results_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gv_results.Rows)
        {
            var label = row.FindControl("gv_l_number") as Label;
            if (label != null)
            {
                label.Text = (row.RowIndex + 1).ToString(CultureInfo.InvariantCulture);
            }
        }
    }


    protected void OnClick(object sender, EventArgs e)
    {
        //var BranchPrintExcel = true;
        string attachment = string.Format("attachment; filename={0}.xls", Tokens.ResellerPayments);
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

        var sw = new StringWriter();
        var htw = new HtmlTextWriter(sw);
        gv_results.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    void SearchInWorkOrderRequests(ISPDataContext context, int? resellerId)
    {
        var newworkrequest = new List<WorkOrderRequest>();
        var worequest = context.WorkOrderRequests
            .Where(
                r => r.ProcessDate.Value.Date >= Convert.ToDateTime(tb_from.Text)
                     && r.ProcessDate.Value.Date <= Convert.ToDateTime(tb_to.Text)
                     && r.RequestStatus.ID == 1
                     && r.RequestID == 11).ToList();
        foreach (var wok in worequest)
        {
            if (wok.WorkOrder.ResellerID != null)
            {
                newworkrequest.Add(wok);
            }
        }
           var results=newworkrequest.Select(w => new
            {
                 w.WorkOrder.ResellerID,
                w.WorkOrder.CustomerName,
                w.WorkOrder.CustomerPhone,
                w.ServicePackage.ServicePackageName,
   
                w.User1.UserName,
                w.WorkOrder.CustomerMobile,
                w.WorkOrder.CustomerEmail,
                w.WorkOrder.Governorate.GovernorateName,
                Provider = w.WorkOrder.ServiceProvider.SPName,
                Branch = w.WorkOrder.Branch.BranchName,
                w.ProcessDate,
                Resseller =  w.WorkOrder.User.UserName,
                Status = w.RequestStatus.RSName,
                w.Demand.StartAt,
                w.Demand.EndAt,
                Total = Helper.FixNumberFormat(w.Total),
                Discound = Helper.FixNumberFormat(
                            _resellerDiscound.CalculateDiscount(w.WorkOrder.ResellerID.Value,
                                w.WorkOrder.ServiceProviderID.Value, w.WorkOrder.ServicePackageID.Value, w.Demand.Amount)
                                .Discount),
                Net = Helper.FixNumberFormat(
                            _resellerDiscound.CalculateDiscount(w.WorkOrder.ResellerID.Value,
                                w.WorkOrder.ServiceProviderID.Value, w.WorkOrder.ServicePackageID.Value, w.Demand.Amount)
                                .Net)
            })
            .ToList();
        if (resellerId != null)
        {
            var res = results.Where(r => r.ResellerID == resellerId);
            gv_results.DataSource = res;
            gv_results.DataBind();
            
        }
        else
        {
            gv_results.DataSource = results;
            gv_results.DataBind();
        }

    }

    void SearchInDemand(ISPDataContext context, int? resellerId)
    {
        var newdemands = new List<Demand>();
        var demands = context.Demands.Where(
                                       r => r.PaymentDate != null
                                            && r.PaymentDate.Value.Date >= Convert.ToDateTime(tb_from.Text)
                                            && r.PaymentDate.Value.Date <= Convert.ToDateTime(tb_to.Text)
                                            && r.Paid).ToList();
        foreach (var dem in demands)
        {
            if (dem.WorkOrder.ResellerID != null)
            {
                newdemands.Add(dem);
            }
        }
        var resultses = newdemands.Select(w => new
        {

            w.WorkOrder.ResellerID,
            w.WorkOrder.CustomerName,
            w.WorkOrder.CustomerPhone,
            w.WorkOrder.ServicePackage.ServicePackageName,

            w.User.UserName,
            w.WorkOrder.CustomerMobile,
            w.WorkOrder.CustomerEmail,
            ProcessDate = w.PaymentDate,
            w.WorkOrder.Governorate.GovernorateName,
            Provider = w.WorkOrder.ServiceProvider.SPName,
            Branch = w.WorkOrder.Branch.BranchName,
            Resseller = w.WorkOrder.User.UserName,
            Status = Tokens.NoResults,
            Total = Helper.FixNumberFormat(w.Amount),
            w.StartAt,
            w.EndAt,
            Discound =
                Helper.FixNumberFormat(
                    _resellerDiscound.CalculateDiscount(w.WorkOrder.ResellerID.Value,
                        w.WorkOrder.ServiceProviderID.Value, w.WorkOrder.ServicePackageID.Value, w.Amount).Discount),
            Net =
                Helper.FixNumberFormat(
                    _resellerDiscound.CalculateDiscount(w.WorkOrder.ResellerID.Value,
                        w.WorkOrder.ServiceProviderID.Value, w.WorkOrder.ServicePackageID.Value, w.Amount).Net)
        }).ToList();
        if (resellerId != null)
        {
            var res = resultses.Where(a => a.ResellerID == resellerId);
            gv_results.DataSource = res;
            gv_results.DataBind();
        }
        else
        {
            gv_results.DataSource = resultses;
            gv_results.DataBind();
        }

    }
}
}