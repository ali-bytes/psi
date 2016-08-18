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
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;
using Resources;

namespace NewIspNL.Pages
{
    public partial class PrintDemands : CustomPage
    {
   
    public PrintModel PrintModel { get; set; }

    readonly DemandSearch _demandSearch;

    readonly IspDomian _domian;


    public PrintDemands()
    {
        var context = IspDataContext;
        _domian = new IspDomian(context);
        _demandSearch = new DemandSearch();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Activate();
        if (IsPostBack) return;
        _domian.PopulatePaymentTypes(DdlPaymentTypes);
        var currentYear = DateTime.Now.Year;
        Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear + 2).OrderBy(x => x), DdlYear);
        Helper.PopulateMonths(DdlMonth);
    }


    void Activate()
    {
        BSearch.ServerClick += (o, e) => Search();
    }


    void Search()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var year = Convert.ToInt32(DdlYear.SelectedItem.Value);
            var month = Convert.ToInt32(DdlMonth.SelectedItem.Value);
            var first = new DateTime(year, month, 1);
            var end = first.AddMonths(1).AddDays(-1);
            var searchModel = new BasicSearchModel
            {
                Paid = false,
                PaymentTypeId = Convert.ToInt32(DdlPaymentTypes.SelectedItem.Value),
                From = first,
                To = end
            };
            var demandResultModels =
                _demandSearch.SearchDemandsByPaid(searchModel, context).Select(DemandResultModel.To2).ToList();

            GvDemands.DataSource = demandResultModels;
            GvDemands.DataBind();
            if (demandResultModels.Count > 0)
            {
                btnExport.Visible = true;
                var total = demandResultModels.Sum(a=>a.DAmount);
                lblTotal.InnerText = string.Format(Tokens.Total + " : {0}",total.ToString(CultureInfo.InvariantCulture));
                lblTotal.Visible = true;
            }
            var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            if (user == null) return;

            var cnfg = context.ReceiptCnfgs.FirstOrDefault(x => x.BranchId == user.BranchID);
            if (cnfg != null)
                PrintModel = new PrintModel
                {
                    LogoUrl = "../SiteLogo/" + cnfg.LogoUrl,
                    CompanyName = cnfg.CompanyName,
                    Caution = cnfg.Caution,
                    ContactData = cnfg.ContactData
                };
        }
    }

    protected void Export(object sender, EventArgs e)
    {
        GridHelper.ExportOneGrid("Demands",GvDemands);
    }

    protected void BindGrid(object sender, EventArgs e)
    {
        Helper.GridViewNumbering(GvDemands, "lblNu");
    }
}
}