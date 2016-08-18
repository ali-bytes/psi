using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;

namespace NewIspNL.Pages
{
    public partial class PayingCustomers : CustomPage
    {
     
    private readonly IspDomian _domian;
    readonly DemandsSearchService _searchService;
    readonly IspEntries _ispEntries;
    public PayingCustomers()
    {
        _domian=new IspDomian(IspDataContext);
        _searchService=new DemandsSearchService(IspDataContext);
        _ispEntries=new IspEntries(IspDataContext);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)return;
        _domian.PopulateResellers(DdlReseller);
        PrepareInputs();
    }
    void PrepareInputs()
    {
        var currentYear = DateTime.Now.Year;
        Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear + 2).OrderBy(x => x), DdlYear);
        Helper.PopulateMonths(DdlMonth);
    }
    protected void NumberGrid(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvResults, "LNo");
    }
    protected void SearchDemands(object sender, EventArgs e)
    {
        
        SearchDemands();
        Msg.Visible = false;
       
    }


    void SearchDemands()
    {
        var year = Helper.GetDropValue(DdlYear);
        var month = Helper.GetDropValue(DdlMonth);
        var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
        {
            Paid = false,
            ResellerId = Helper.GetDropValue(DdlReseller),
        });
        if (year != null && year > 0)
        {
            searchDemands = searchDemands.Where(x => x.StartAt.Year == year).ToList();
        }
        if (month != null && month > 0)
        {
            searchDemands = searchDemands.Where(x => x.StartAt.Month == month).ToList();
        }
        using (var context=new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var allpaied = context.PayingCustomersResellers.ToList();
           
            foreach (var p in allpaied)
            {
                PayingCustomersReseller p1 = p;
                var demand = searchDemands.FirstOrDefault(a => a.Id == p1.DemandId);
                if(demand!=null)searchDemands.Remove(demand);
            }
            GvResults.DataSource = searchDemands;//searchDemands;
            GvResults.DataBind();
        }


    }

    protected void Pay(object sender, EventArgs e)
    {
        var btn = sender as LinkButton;
        if(btn==null)return;
        var id = Convert.ToInt32(btn.CommandArgument);
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var demand = _ispEntries.GetDemand(id);
            var resellerId = demand.WorkOrder.ResellerID;
            var paynewDemand = new PayingCustomersReseller
            {
                DemandId = id,
                Date = DateTime.Now.Add9Hours(),
                ResellerId = Convert.ToInt32(resellerId),
            };
            context.PayingCustomersResellers.InsertOnSubmit(paynewDemand);
            context.SubmitChanges();
           
            var myscript = "window.open('ResellerDemandReciept.aspx?id=" + QueryStringSecurity.Encrypt(paynewDemand.DemandId.ToString()) + "')";
            ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
            SearchDemands();
        }
    }

}
}