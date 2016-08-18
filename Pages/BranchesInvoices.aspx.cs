using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchesInvoices : CustomPage
    {

        readonly DemandsSearchService _demandsSearchService;

        //readonly IspDomian _domian;
        private readonly IspEntries _ispEntries;

        public BranchesInvoices()
        {
            var context = IspDataContext;
            _demandsSearchService = new DemandsSearchService(context);
            //_domian = new IspDomian(context);
            _ispEntries = new IspEntries(context);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PrepareInputs();
            PopulateProvider();
        }



        void PrepareInputs()
        {
            var currentYear = DateTime.Now.Year;
            Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear + 2).OrderBy(x => x), DdlYear);
            Helper.PopulateMonths(DdlMonth);
        }
        void PopulateProvider()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = context.Users.FirstOrDefault(a => a.ID == userId);
                List<ServiceProvider> providers;
                if (user != null && user.GroupID != 6)
                {
                    providers = _ispEntries.ServiceProviders();
                }
                else
                {
                    providers = _ispEntries.ServiceProviders();
                    var pro = new List<ServiceProvider>();
                    var pro2 = new List<ServiceProvider>();
                    var userProviders = context.UserProviders.Where(a => a.UserId == userId).ToList();
                    foreach (var userprovid in userProviders)
                    {
                        var v = providers.FirstOrDefault(a => a.ID == Convert.ToInt32(userprovid.Provider));
                        if (v != null) pro.Add(v);
                    }
                    var invoiceProvider = context.OptionInvoiceProviders.ToList();
                    foreach (var invoiceprovider in invoiceProvider)
                    {
                        var l = pro.FirstOrDefault(a => a.ID == invoiceprovider.ProviderId);
                        if (l != null) pro2.Add(l);
                    }
                    providers = pro2;
                }
                providerlist.DataSource = providers;
                providerlist.DataTextField = "SPName";
                providerlist.DataValueField = "ID";
                providerlist.DataBind();
            }
        }

        protected void SearchDemands(object sender, EventArgs e)
        {

            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                resellerDiv.Visible = true;
                var branches = context.Branches.ToList();
                var branchList = new List<QulaifiedData>();
                var newlist = new List<DemandPreviewModel>();
                foreach (var branch in branches)
                {
                    var searchDemands = _demandsSearchService.BranchUnpaidDemandsPreview(new BasicSearchModel
                    {
                        //Paid = false,
                        BranchPaid = false,
                        BranchId = branch.ID,//Helper.GetDropValue(DdlBranch),
                        Month = Helper.GetDropValue(DdlMonth),
                        Year = Helper.GetDropValue(DdlYear),
                        WithBranchDiscount = true
                    });


                    if (providerlist.Items.Count > 0)
                    {
                        var totalcount = providerlist.Items.Cast<ListItem>().Count(item => item.Selected);
                        if (totalcount == 0)
                        {
                            Msg.InnerText = Tokens.SelectServiceProvider;
                            continue;
                        }
                        foreach (ListItem item in providerlist.Items)
                        {
                            if (item.Selected)
                            {
                                var item1 = item;
                                var data = searchDemands.Where(a => a.Provider == item1.ToString()).ToList();
                                // newlist+=data;
                                newlist.AddRange(data);
                            }
                        }
                    }
                    else
                    {
                        newlist = searchDemands;
                    }

                    var total = newlist.Sum(x => x.BranchNet); //Amount);
                    var ld = new QulaifiedData
                    {
                        Branch = branch.BranchName,
                        Reseller = "",
                        Total = total
                    };
                    branchList.Add(ld);
                    newlist.Clear();
                }
                GridView1.DataSource = branchList;
                GridView1.DataBind();
                //var gvList = new GridView[] {GridView1};
                //GridHelper.Export("BranchesInvoices.xls", gvList);

                const string attachment = "attachment; filename=BranchesInvoices.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");



                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);
                GridView1.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();


                //GridHelper.ExportOneGrid("BranchesInvoices", GridView1);
                resellerDiv.Visible = false;
            }
        }
  

        protected void NumberGrid(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GridView1, "LNo");
        }

    }
}
