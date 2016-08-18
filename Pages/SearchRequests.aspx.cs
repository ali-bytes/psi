using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using Resources;

namespace NewIspNL.Pages
{
    public partial class SearchRequests : CustomPage
    {
        readonly List<int?> _portalList;
        public CookieContainer Cookiecon;
        public SearchRequests()
        {
          //Cookiecon=new CookieContainer();
            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                _portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e){
        if(IsPostBack) return;
            //using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            //{
            //    var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
            //    if (portalList.Count > 0)
            //    {
            //        Cookiecon = Tedata.Login();
            //    }
               
            //}
            PortalData();
            PopulateRequests();
        Populateuser();
       PopulateReseller();
        var today = DateTime.Now.AddHours();
        var first = new DateTime(today.Year, today.Month, 1);
        var last = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        tb_from.Text = first.ToShortDateString();
        tb_to.Text = last.ToShortDateString();
    }
        void PortalData()
        {
            using (var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var portalList = dataContext.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
               
                if (portalList.Count >0){
                    ShowPortalStatus.Visible = true;
                }
                else
                {
                    ShowPortalStatus.Visible = false;
                }
            }
        }
    void PopulateReseller()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var domian = new IspDomian(context);
            domian.PopulateResellerswithDirectUser(ddlReseller, true);
        }
    }
    void Populateuser(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var users = context.Users.Where(u => u.GroupID != 6);
            var userId = Convert.ToInt32(Session["User_ID"]);
            var usr = context.Users.FirstOrDefault(a => a.ID == userId);
            if (usr != null)
            {
                var datalevel = usr.Group.DataLevelID;
                switch (datalevel)
                {
                    case 1:
                        break;
                    case 2:
                        users = users.Where(a => a.BranchID == usr.BranchID);
                        break;
                }
                Helper.BindDrop(DdlUser, users, "UserName", "ID", true, Tokens.All);
            }
            
        }
    }


    void PopulateRequests(){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var requestTypes = context.Requests.Where(r => r.ID != 10);
            Helper.BindDrop(ddl_requestsTypes, requestTypes, "RequestName", "ID", true, Tokens.All);
        }
    }


    protected void b_search_Click(object sender, EventArgs e){
        
        {
            var requestId = ddl_requestsTypes.SelectedIndex > 0 ? Convert.ToInt32(ddl_requestsTypes.SelectedValue) : 0;
            var userId = DdlUser.SelectedIndex > 0 ? Convert.ToInt32(DdlUser.SelectedValue) : 0;
            var resellerId = ddlReseller.SelectedIndex > 0 ? Convert.ToInt32(ddlReseller.SelectedItem.Value) : -1;

            var intactRequests = DataLevelClass.GetCofirmedWoRequests();
           
            if(intactRequests==null)return;
            if(requestId != 0){
                intactRequests = intactRequests.Where(r => r.RequestID == requestId).ToList();
            }

            if(userId != 0){
                intactRequests = intactRequests.Where(r => r.ConfirmerID == userId).ToList();
            }
            if (resellerId != -1)
            {
                if(resellerId!=0)
                    intactRequests= intactRequests.Where(r => r.WorkOrder.ResellerID == resellerId).ToList();
                else if (resellerId == 0)
                    intactRequests = intactRequests.Where(r => r.WorkOrder.ResellerID == null).ToList();
            }
            var results = intactRequests
                .Where(r => r.ProcessDate != null && (r.ProcessDate.Value.Date <= Convert.ToDateTime(tb_to.Text).Date
                                                      &&
                                                      r.ProcessDate.Value.Date >= Convert.ToDateTime(tb_from.Text).Date))
                .Select(w => new
                {
                    w.WorkOrder.CustomerName,
                    w.WorkOrder.CustomerPhone,
                   ServicePackage= w.ServicePackage == null ? "-": w.ServicePackage.ServicePackageName,
                    w.RequestDate,
                    w.ProcessDate,
                    w.WorkOrder.CustomerMobile,
                    w.WorkOrder.CustomerEmail,
                   GovernorateName= w.WorkOrder.Governorate == null ? "-" : w.WorkOrder.Governorate.GovernorateName,
                    Provider = w.WorkOrder.ServiceProvider == null ? "-" : w.WorkOrder.ServiceProvider.SPName,
                    Branch = w.WorkOrder.Branch == null ? "-" : w.WorkOrder.Branch.BranchName,
                    Resseller = w.WorkOrder.ResellerID == null ? "-" : w.WorkOrder.User.UserName,
                    OrderStatus = w.RequestStatus == null ? "-" : w.RequestStatus.RSName,
                    Request = w.Request == null ? "-" : w.Request.RequestName,
                    User = w.ConfirmerID != null ? w.User1.UserName : "-",
                    newspeed = w.ServicePackage1 == null ? "-" :w.ServicePackage1.ServicePackageName,
                    newip = w.IpPackage == null ? "-":w.IpPackage.IpPackageName,
                  extragiga=w.ExtraGigaId==null?"-":w.ExtraGiga.Name,
                    status = w.WorkOrder.Status == null ? "-" : w.WorkOrder.Status.StatusName,
                PortalStatus= CkShowPortalStatus.Checked? GetPortalStatus(w.WorkOrder) :"-"

                }).ToList();

            gv_results.DataSource = results;
            gv_results.DataBind();
        }
    }

        string GetPortalStatus(WorkOrder wor)
        {
            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                //var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                //var woproviderList = db8.WorkOrders.FirstOrDefault(z => z.ID == workOrderId);
                //var portalList = _portalList.Select(z => z.PortalProvidersId).ToList();
          
               

                if (wor != null && _portalList.Contains(wor.ServiceProviderID))
                {
                    if (Cookiecon == null)
                    {
                        Cookiecon = new CookieContainer();
                        Cookiecon = Tedata.Login();  
                    }
                    var username = wor.UserName;
                    //CookieContainer cookiecon = new CookieContainer();
                    //cookiecon = Tedata.Login();
                    if (Cookiecon != null)
                    {
                        var pagetext = Tedata.GetSearchPage(username, Cookiecon);
                        if (pagetext != null)
                        {
                            var searchPage = Tedata.CheckSearchPage(pagetext);
                            if (searchPage)
                            {
                                var custStatus = Tedata.GetCustomerStatus(pagetext);
                                if (custStatus !=null )
                                {
                                    //Div1.InnerHtml = "هذا العميل موقوف بالفعل على البورتال";
                                    return custStatus;
                                }
                            }
                            else
                            {
                                //فى حالة البورتال واقع
                                return "InvalidCustomerUserName";
                                //Div1.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                            }

                        }
                        else
                        {
                            return "CannotConnectToPortal";
                            //Div1.InnerHtml = "تعذر الوصول الى البورتال";
                         

                        }
                    }
                    else
                    {
                        return "InvalidPoratlUserNameOrPassword";
                            //"فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                    }
                }
                else
                {
                    return "-";
                }
                return "-";
            }
        }
    protected void gv_results_DataBound(object sender, EventArgs e){
        foreach(GridViewRow row in gv_results.Rows){
            var label = row.FindControl("gv_l_number") as Label;
            if(label != null) label.Text = (row.RowIndex + 1).ToString(CultureInfo.InvariantCulture);
        }
    }
}
}