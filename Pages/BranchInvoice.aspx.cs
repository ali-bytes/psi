using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class BranchInvoice : CustomPage
    {

        //readonly  _context;

        readonly DemandsSearchService _demandsSearchService;

        readonly IspDomian _domian;
        private readonly IspEntries _ispEntries;
        public bool CanCreateInvoice { get; set; }

        public BranchInvoice()
        {
            var context = IspDataContext;
            _demandsSearchService = new DemandsSearchService(context);
            _domian = new IspDomian(context);
            _ispEntries = new IspEntries(context);
        }
        void HandlePrivildges()
        {
            var userId = Convert.ToInt32(Session["User_ID"]);
            CanCreateInvoice = _ispEntries.UserHasPrivlidge(userId, "CreateInvoice");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PrepareInputs();
            PopulateProvider();
            HandlePrivildges();
            Button1.Visible = CanCreateInvoice;
            searchPanel.Visible = true;
            resultPanel.Visible = false;
        }



        void PrepareInputs()
        {
            _domian.PopulateBranches(DdlBranch, true);
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
            Msg.InnerHtml = string.Empty;
            SearchDemands();
            HfSerched.Value = "1";
            searchPanel.Visible = false;
            resultPanel.Visible = true;
        }


        void SearchDemands()
        {
            var searchDemands = _demandsSearchService.BranchUnpaidDemandsPreview(new BasicSearchModel
            {
                //Paid = false,
                BranchPaid = false,
                BranchId = Helper.GetDropValue(DdlBranch),
                Month = Helper.GetDropValue(DdlMonth),
                Year = Helper.GetDropValue(DdlYear),
                WithBranchDiscount = true
            });

            var newlist = new List<DemandPreviewModel>();
            if (providerlist.Items.Count > 0)
            {
                foreach (ListItem item in providerlist.Items)
                {
                    if (!item.Selected) continue;
                    var data = searchDemands.Where(a => a.Provider == item.ToString()).ToList();
                    // newlist+=data;
                    newlist.AddRange(data);
                }
            }
            else
            {
                newlist = searchDemands;
            }
            GvResults.DataSource = newlist;//searchDemands;
            GvResults.DataBind();

            var report = new Dictionary<string, string>{
            { Tokens.Total, Helper.FixNumberFormat((newlist.Sum(x => x.Amount))) },
            { Tokens.TotalBranchDiscount, Helper.FixNumberFormat((newlist.Sum(x => x.BranchDiscount))) },
            { Tokens.Net, Helper.FixNumberFormat((newlist.Sum(x => x.BranchNet))) }
        };
            GvReport.DataSource = report;
            GvReport.DataBind();

            List<DemandPreviewModelToExel> exelList = new List<DemandPreviewModelToExel>();

            exelList = newlist.ConvertAll(x => new DemandPreviewModelToExel()
            {
                Id = x.Id,
                Customer = x.Customer,
                Address = x.Address,
                TAmount = x.TAmount,
                TStartAt = x.TStartAt,
                EndAt = x.EndAt,
                TEndAt = x.TEndAt,
                StartAt = x.StartAt,
                BranchNet = x.BranchNet,
                
            }).ToList();
            ViewState["newlist"] = null;
            ViewState["newlist"] = new List<DemandPreviewModelToExel>();
            ViewState["newlist"] = exelList.ToList();
           
        }


        protected void NumberGrid(object sender, EventArgs e)
        {
            Helper.GridViewNumbering(GvResults, "LNo");
        }



        protected void CreateInvoice(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
               

                var newlist = new List<DemandPreviewModelToExel>();
                newlist = (List<DemandPreviewModelToExel>)ViewState["newlist"];
                ViewState["newlist"] = null;

                if (!newlist.Any()) return;
                //var total = newlist.Sum(x => x.Amount);
                var totalwith = newlist.Sum(x => x.BranchNet);
                var branchId = Convert.ToInt32(DdlBranch.SelectedItem.Value);
                var branch = context.Branches.FirstOrDefault(x => x.ID == branchId);
                if (branch == null) return;

                //var completePath = HttpContext.Current.Server.MapPath("~/ExcelTemplates/ResellerPaidDemands.xls");
                var time = DateTime.Now.AddHours();
                var timeName = time.Day + "_" + time.Month + "_" + time.Year + "_" + time.Hour + "_" + time.Minute +
                               "_" + time.Millisecond;

                GvResults.Columns[18].Visible = false;
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                // Render grid view control.
                GvResults.RenderControl(htw);

                // Write the rendered content to a file.
                string renderedGridView = sw.ToString();
                File.WriteAllText(HttpContext.Current.Server.MapPath(string.Format("~/ExcelTemplates/BranchPaidDemands/{0}_{1}.xls", branchId, timeName)), renderedGridView);




             
                var transaction = new UsersTransaction
                {
                    Branch = branch,
                    DepitAmmount = Convert.ToDouble(totalwith),
                    Total = Billing.GetLastBalance(branchId, "Branch") + Convert.ToDouble(totalwith),//newlist.Sum(x => x.BranchNet)),
                    CreditAmmount = 0,
                    CreationDate = time,
                    IsInvoice = true,
                    IsPaid = false,
                    Description = "Invoice",
                    Notes = "اصدار فاتورة بتاريخ" + time.ToShortDateString(),
                    UserId = Convert.ToInt32(Session["User_ID"]),
                    FileUrl = string.Format("{0}_{1}.xls", branchId, timeName)
                };
                context.UsersTransactions.InsertOnSubmit(transaction);
                //searchDemands = searchDemands.Where(a => !string.IsNullOrWhiteSpace(a.Reseller)).ToList();
                //newlist.ForEach(x =>
                //{
                //    //x.Demand.Month =month==null?x.Demand.StartAt.Month: month.Value;
                //    //x.Demand.Year =year==null?x.Demand.StartAt.Year: year.Value;
                //    x.Demand.BranchPaid = true;
                //});

                foreach (var list in newlist)
                {
                    var demand = context.Demands.FirstOrDefault(a => a.Id == list.Id);
                    if (demand != null && demand.StartAt.Date == Convert.ToDateTime(list.TStartAt).Date && demand.EndAt.Date == Convert.ToDateTime(list.EndAt).Date)
                    {
                        demand.BranchPaid = true;
                    }
                }
              

                context.SubmitChanges();
                _demandsSearchService.Commit();
                Msg.InnerHtml = Tokens.Saved;
                ResetPage();
                var mobile = branch.Mobile1;
                if (!string.IsNullOrEmpty(mobile))
                {
                    var message = SendSms.SendSmsByNotification(context, mobile, 4);
                    if (!string.IsNullOrEmpty(message))
                    {
                        var myscript = "window.open('" + message + "')";
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                    }
                }
            }
        }


        protected void SearchAgain(object sender, EventArgs e)
        {
            ResetPage();
        }


        void ResetPage()
        {
            GvResults.DataSource = null;
            GvResults.DataBind();
            Helper.Reset(Forsearch);
            HfSerched.Value = string.Empty;
            searchPanel.Visible = true;
            resultPanel.Visible = false;
        }

        [WebMethod]
        public static string GetHistory(int id)
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var history = db.BranchInvoiceComments.Where(x => x.DemandId == id).Select(z => new CustomerHistory
                {
                    User = z.User.UserName,
                    Comment = z.Comment,
                    Date = z.Date.Value.Date
                }).ToList();
             
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(history);
            }

        }

        protected void GvResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton allcomment = (LinkButton)e.Row.FindControl("lnb_AllComment");
                var id = Convert.ToInt32(allcomment.CommandArgument);
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var history =
                        db.BranchInvoiceComments.FirstOrDefault(x => x.DemandId == id);

                    if (history == null)
                    {
                        allcomment.Visible = false;
                    }
                    else
                    {
                        allcomment.Visible = true;
                    }

                }

            }
        }
        public void AddComment(object sender, EventArgs eventArgs)
        {
            int userId = Convert.ToInt32(Session["User_ID"]);
            var demandId = Convert.ToInt32(demmandId.Value ?? "0");
            if (demandId > 0)
            {
                using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    BranchInvoiceComment comm = new BranchInvoiceComment()
                    {
                        DemandId = demandId,
                        UserId = userId,
                        Comment = txtComment.Text,
                        Date = DateTime.Now.AddHours()
                    };
                    db.BranchInvoiceComments.InsertOnSubmit(comm);
                    db.SubmitChanges();
                    SearchDemands();
                }
            }

        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            GvResults.Columns[18].Visible = false;

            string attachment = string.Format("attachment; filename={0}.xls", Tokens.BranchInvoices);
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            GvResults.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();


        }
    }
}
