using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;

namespace NewIspNL.Pages
{
    public partial class PayingCustomersReport : CustomPage
    {
     
    private readonly IspDomian _domian;

    public PayingCustomersReport()
    {
        _domian=new IspDomian(IspDataContext);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        _domian.PopulateResellers(DdlReseller);

    }
    protected void NumberGrid(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvResults, "LNo");
    }

    void Search()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            lblTotal.Text = "";
            var demands = context.PayingCustomersResellers.Where(a => a.ResellerId == Helper.GetDropValue(DdlReseller));
            if (!string.IsNullOrWhiteSpace(txtFrom.Text))
            {
                demands = demands.Where(a => a.Date.Date >= Convert.ToDateTime(txtFrom.Text));
            }
            if (!string.IsNullOrWhiteSpace(txtTo.Text))
            {
                demands = demands.Where(a => a.Date.Date <= Convert.ToDateTime(txtTo.Text));
            }
           
            GvResults.DataSource = demands.Select(a=>new
            {
                a.Id,
                demandId=a.Demand.Id,
                a.Date,
                a.Demand.Amount,
                Customer=a.Demand.WorkOrder.CustomerName,
                Phone=a.Demand.WorkOrder.CustomerPhone,
                Status=a.Demand.WorkOrder.Status.StatusName,
                Provider=a.Demand.WorkOrder.ServiceProvider.SPName,
              
                servicepack = a.Demand.WorkOrder.ServicePackage.ServicePackageName,
            });
            GvResults.DataBind();
            var check = demands;
            if (demands.Count() == 0)
            {
                return;
                
            }
            else { 
            if (GvResults.DataSource == null) return;
           if(demands.Any()) 
               lblTotal.Text = demands.Sum(a => a.Demand.Amount).ToString(CultureInfo.InvariantCulture);
         
            divTotal.Visible = true;  }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }
}
}