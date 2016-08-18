using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Domain.SearchService;
using NewIspNL.Helpers;
using NewIspNL.Models;

namespace NewIspNL.Pages
{
    public partial class InvoiceByCentral : CustomPage
    {
       
            readonly ICentralRepository _centralRepository;

            readonly DemandSearch _demandSearch;

            readonly IWorkOrderRepository _workOrderRepository;


            public  InvoiceByCentral()
            {
                _workOrderRepository = new WorkOrderRepository();
                _centralRepository = new LCentralRepository();
                _demandSearch = new DemandSearch();
            }


            void PopulateYears()
            {
                var time = DateTime.Now.AddHours();
                var years = Helper.FillYears(time.Year - 5, time.Year + 2).OrderByDescending(x => x);
                ddl_year.DataSource = years;
                ddl_year.DataBind();
                Helper.AddDefaultItem(ddl_year);
                ddl_year.SelectedValue = string.Format("{0}", time.Year);
            }


            void PopulateMonths()
            {
                var months = Helper.FillMonths();
                ddl_months.DataSource = months.Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value
                });
                ddl_months.DataTextField = "Name";
                ddl_months.DataValueField = "Id";
                ddl_months.DataBind();
                Helper.AddDefaultItem(ddl_months);
            }


            void PopulateCentrals()
            {
                var centrals = _centralRepository.Centrals.Select(x => new
                {
                    x.Id,
                    Name =
                        x.Governorate.GovernorateName + " - " + x.Name
                });
                ddl_centrals.DataSource = centrals;
                ddl_centrals.DataTextField = "Name";
                ddl_centrals.DataValueField = "Id";
                ddl_centrals.DataBind();
                Helper.AddDefaultItem(ddl_centrals);
            }
            void PopulateProviders()
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                        var providers = db.ServiceProviders.ToList();
                        DdlProvider.DataSource = providers;
                        DdlProvider.DataTextField = "SPName";
                        DdlProvider.DataValueField = "ID";
                        DdlProvider.DataBind();
                        Helper.AddDefaultItem(DdlProvider);
                      IspDomian _domian = new IspDomian(IspDataContext);
           _domian.PopulateResellerswithDirectUser(DdlReseller, true);
                }
            }
           
        private void PrepareInputs()
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
            {
                if (IsPostBack) return;
                PopulateCentrals();
                PopulateMonths();
                PopulateYears();
                PopulateProviders();
                PrepareInputs();
            }


            protected void bSave_Click(object sender, EventArgs e)
            {
                var centralId = Convert.ToInt32(ddl_centrals.SelectedItem.Value);
                var month = Convert.ToInt32(ddl_months.SelectedItem.Value);
                var year = Convert.ToInt32(ddl_year.SelectedItem.Value);
                var start = new DateTime(year, month, 1);

                var demandResultModels = _demandSearch.SearchDemands(new BasicSearchModel
                {
                    From = start.Date,
                    To = start.AddMonths(1).AddDays(-1).Date,
                    CentralId = centralId
                }, IspDataContext);

                if (DdlProvider.SelectedItem.Value!="")
                {
                   var provider = DdlProvider.SelectedItem.Text;
                   demandResultModels = demandResultModels.Where(x => x.Provider.Contains(provider)).ToList();
                }
                if (DdlReseller.SelectedItem.Value != "")
                {

                    if (DdlReseller.SelectedItem.Value == "0")
                    {
                        demandResultModels = demandResultModels.Where(x => x.ResellerId == null).ToList();
                    }
                    else
                    {
                        var reseller = DdlReseller.SelectedItem.Text;
                        demandResultModels = demandResultModels.Where(x => x.Reseller.Contains(reseller)).ToList();
                    }
                }

                gv_invoices.DataSource = demandResultModels;
                gv_invoices.DataBind();

                net.Text = Helper.FixNumberFormat(demandResultModels.Sum(x => x.DAmount));
            }


            protected void gv_invoices_DataBound(object sender, EventArgs e)
            {
                Helper.GridViewNumbering(gv_invoices, "l_no");
            }

            protected void Exportgrid(object sender, EventArgs e)
            {
                //creating the array of GridViews and calling the Export function
                var gvList = new GridView[] { gv_invoices };
                GridHelper.Export("InvoiceByCentral.xls", gvList);
            }
        }
    }
 