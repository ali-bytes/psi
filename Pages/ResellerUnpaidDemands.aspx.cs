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
    public partial class ResellerUnpaidDemands : CustomPage
    {
    

    readonly IspDomian _domian;
    readonly DemandsSearchService _searchService;
    readonly IspEntries _ispEntries;
    public bool canCreateInvoice { get; set; }


    public ResellerUnpaidDemands(){
       
        _searchService = new DemandsSearchService(IspDataContext);
        _domian = new IspDomian(IspDataContext);
        _ispEntries=new IspEntries(IspDataContext);
    }

    void HandlePrivildges()
    {
        int userId = Convert.ToInt32(Session["User_ID"]);
        canCreateInvoice = _ispEntries.UserHasPrivlidge(userId, "CreateInvoice");
    }
    protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
        PrepareInputs();
        HandlePrivildges();
        PopulateProvider();
      
        btnCreatInvoice.Visible = canCreateInvoice;
        searchPanel.Visible = true;
        resultPanel.Visible = false;
    }

       


        void PopulateProvider(){
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            int userId = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(a => a.ID == userId);
            List<ServiceProvider> providers;
            if (user != null && user.GroupID != 6)
            {
                 providers= _ispEntries.ServiceProviders();
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
                        if(v!=null)pro.Add(v);
                    }
                var invoiceProvider = context.OptionInvoiceProviders.ToList();
                foreach (var invoiceprovider in invoiceProvider)
                {
                    var l = pro.FirstOrDefault(a => a.ID == invoiceprovider.ProviderId);
                    if(l!=null)pro2.Add(l);
                }
                providers = pro2;
            }
            providerlist.DataSource = providers;
            providerlist.DataTextField = "SPName";
            providerlist.DataValueField = "ID";
            providerlist.DataBind();
        }
    }

        void Bind_ddl_Reseller()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var user = context.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null) return;
                DdlReseller.SelectedValue = null;
                DdlReseller.Items.Clear();

                DdlReseller.AppendDataBoundItems = true;
                var query = DataLevelClass.GetUserReseller();

                var q = query.Where(x => x.AccountmanagerId == user.ID).ToList();
                if (q.Count > 0)
                {
                    query = query.Where(x => x.AccountmanagerId == user.ID).ToList();
                }

                DdlReseller.DataSource = query;
                DdlReseller.DataBind();
                Helper.AddDefaultItem(DdlReseller);
            }
        }
    void PrepareInputs(){
        //_domian.PopulateResellers(DdlReseller, true);
        Bind_ddl_Reseller();
        var currentYear = DateTime.Now.Year;
        Helper.PopulateDrop(Helper.FillYears(currentYear - 5, currentYear+2).OrderBy(x => x), DdlYear);
        Helper.PopulateMonths(DdlMonth);
    }


    protected void SearchDemands(object sender, EventArgs e){
        Msg.InnerHtml = string.Empty;
        SearchDemands();
        HfSerched.Value="1";
        searchPanel.Visible = false;
        resultPanel.Visible = true;
    }

        [WebMethod]
    public static string GetHistory(int id)
    {
        //var id = Convert.ToInt32(((Button)sender).CommandArgument);
        using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var history = db.ResellerUnpaidDemandComments.Where(x => x.DemandId == id).Select(z=>new CustomerHistory
            {
                User = z.User.UserName,
                Comment = z.Comment,
                Date = z.Date.Value.Date
            }).ToList();
            //List<CustomerHistory> chl = new List<CustomerHistory>();
            //foreach (var h in history)
            //{
            //    CustomerHistory ch=new CustomerHistory();
            //    ch.User = h.User.UserName;
            //    ch.Comment = h.Comment;
            //    ch.Date = h.Date;
            //    chl.Add(ch);
            //}
            //grdHistory.DataSource = history;
            //grdHistory.DataBind();
           JavaScriptSerializer js=new JavaScriptSerializer();
           return js.Serialize(history);
         
            //return chl;

        }

    }

        void SearchDemands(){
        var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel{
            Paid = false,
            ResellerId = Helper.GetDropValue(DdlReseller),
            Month = Helper.GetDropValue(DdlMonth),
            Year = Helper.GetDropValue(DdlYear),
            WithResellerDiscount = true,
            
        });
        var newlist = new List<DemandPreviewModel>();
        if(providerlist.Items.Count > 0){
            foreach(ListItem item in providerlist.Items){
                if(item.Selected){
                    var data = searchDemands.Where(a => a.Provider == item.ToString()).ToList();
             
                    newlist.AddRange(data);
                }
            }
        } else{
            newlist = searchDemands;
        }
     
        GvResults.DataSource = newlist;//searchDemands;
        GvResults.DataBind();
        var report=new Dictionary<string, string>{
           
            {Tokens.Total,Helper.FixNumberFormat((newlist.Sum(x=>x.Amount)))},
            { Tokens.TotalResellerDiscount, Helper.FixNumberFormat((newlist.Sum(x => x.ResellerDiscount))) },
            { Tokens.Net, Helper.FixNumberFormat((newlist.Sum(x => x.ResellerNet))) }
        };
        GvReport.DataSource = report;
        GvReport.DataBind();

            List<DemandPreviewModelToExel> exelList=new List<DemandPreviewModelToExel>();

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
                ResellerNet = x.ResellerNet
            }).ToList();

            ViewState["newlist"] = null;
            ViewState["newlist"] = new List<DemandPreviewModelToExel>();
            ViewState["newlist"] = exelList.ToList();
    }


    protected void NumberGrid(object sender, EventArgs e){
        Helper.GridViewNumbering(GvResults, "LNo");
    }



    protected void CreateInvoice(object sender, EventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
           

            var newlist = new List<DemandPreviewModelToExel>();
            newlist = (List<DemandPreviewModelToExel>)ViewState["newlist"];
            ViewState["newlist"] = null;

            if(!newlist.Any()) return;
            IspDataContext.SubmitChanges();
            var total = newlist.Sum(x => x.ResellerNet);//Amount);
            var resellerId = Convert.ToInt32(DdlReseller.SelectedItem.Value);
            var reseller = context.Users.FirstOrDefault(x => x.ID == resellerId);
            if(reseller == null) return;
            var time = DateTime.Now.AddHours();
            var timeName = time.Day + "_" + time.Month + "_" + time.Year + "_" + time.Hour + "_" + time.Minute + "_" + time.Millisecond;


            GvResults.Columns[17].Visible = false;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            // Render grid view control.
            GvResults.RenderControl(htw);

            // Write the rendered content to a file.
            string renderedGridView = sw.ToString();
            File.WriteAllText(HttpContext.Current.Server.MapPath(string.Format("~/ExcelTemplates/ResselerPaidDemands/{0}_{1}.xls", resellerId, timeName)), renderedGridView);



            var userId = Convert.ToInt32(Session["User_ID"]);
            var transaction = new UsersTransaction{
                ResellerID = resellerId,
                DepitAmmount = Convert.ToDouble(total),
                Total = Billing.GetLastBalance(resellerId, "Reseller") + Convert.ToDouble(total),
                CreditAmmount = 0,
                CreationDate = time,
                IsInvoice = true,
                IsPaid = false,
                Description = "Invoice",
                Notes = "اصدار فاتورة بتاريخ" + time.ToShortDateString(),
                UserId = userId,
                FileUrl = string.Format("{0}_{1}.xls", resellerId, timeName)
            };
            context.UsersTransactions.InsertOnSubmit(transaction);
            
          
            foreach (var list in newlist)
            {
                var demand=context.Demands.FirstOrDefault(a => a.Id == list.Id);
                if (demand != null &&demand.StartAt.Date==Convert.ToDateTime(list.TStartAt).Date&&demand.EndAt.Date==Convert.ToDateTime(list.EndAt).Date)
                {
                    demand.Paid = true;
                    demand.PaymentDate = DateTime.Now.AddHours();
                    demand.UserId = userId;
                    demand.PaymentComment = "اصدار فاتورة موزع";
                    context.SubmitChanges();
                }
            }
            context.SubmitChanges();
            IspDataContext.SubmitChanges();
            Msg.InnerHtml = Tokens.Saved;
            ResetPage();
           
            var mobile = reseller.UserMobile;
            if (!string.IsNullOrEmpty(mobile))
            {
                var message = SendSms.SendSmsByNotification(context, mobile, 3);
                if (!string.IsNullOrEmpty(message))
                {
                    var myscript = "window.open('" + message + "')";
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                }
            }
           
        }
        GvResults.Columns[17].Visible = true;
    }


    protected void SearchAgain(object sender, EventArgs e){
        ResetPage();
    }


    void ResetPage(){
        GvResults.DataSource = null;
        GvResults.DataBind();
        Helper.Reset(Forsearch);
        HfSerched.Value = string.Empty;
        searchPanel.Visible = true;
        resultPanel.Visible = false;
    }
    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        GvResults.Columns[17].Visible = false;

        string attachment = string.Format("attachment; filename={0}.xls", Tokens.UnpaidResellerDemands);
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
    public void AddComment(object sender, EventArgs eventArgs)
    {
         int userId = Convert.ToInt32(Session["User_ID"]);
        var demandId =Convert.ToInt32(demmandId.Value ?? "0");
        if (demandId > 0)
        {
            using (var db = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ResellerUnpaidDemandComment comm = new ResellerUnpaidDemandComment()
                {
                    DemandId = demandId,
                    UserId = userId,
                    Comment = txtComment.Text,
                    Date = DateTime.Now.AddHours()
                };
                db.ResellerUnpaidDemandComments.InsertOnSubmit(comm);
                db.SubmitChanges();
                SearchDemands();
            }
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
                    db.ResellerUnpaidDemandComments.FirstOrDefault(x => x.DemandId == id);

                if (history ==null)
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
    //protected void grdHistory_DataBound(object sender, EventArgs e)
    //{
    //    foreach (GridViewRow row in grdHistory.Rows)
    //    {
    //        var label = row.FindControl("gv_l_number") as Label;
    //        if (label != null)
    //        {
    //            label.Text = (row.RowIndex + 1).ToString(CultureInfo.InvariantCulture);
    //        }
    //    }
    //}
}

    public class CustomerHistory
    {
        public string User { get; set; }

        public string Comment { get; set; }

        public DateTime? Date { get; set; }
    }
}