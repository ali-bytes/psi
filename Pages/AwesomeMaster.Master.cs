using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewIspNL.Domain;
using NewIspNL.Helpers;
using NewIspNL.Services;
using Db;
using Resources;

namespace NewIspNL.Pages
{
    public partial class AwesomeMaster : MasterPage
    {
        //readonly ISPDataContext _dataContext;
        public StringBuilder Menu;
        readonly SiteDateRepository _siteDateRepository;
        public  AwesomeMaster()
        {
            var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            //_domain=new IspDomian(_dataContext);
            Menu = new StringBuilder();
            _siteDateRepository = new SiteDateRepository(dataContext);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["cultureid"] == null)
            {
                Response.Redirect("default.aspx");
                return;
            }

            hfRtl.Value = Session["cultureid"].ToString();
            UpdateSiteData();

            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (!IsPostBack)
                {
                   
                    GetQuickSupport(context);
                    /* PopulateGovernorates();
                    var option = OptionsService.GetOptions(dataContext, false);
                    SearchBox.Visible = option != null;
                    DdlGovernorate.Visible = option != null && option.IncludeGovernorateInSearch;
                    if (DdlGovernorate.Visible)
                    {
                         _domain.PopulateGovernorates(DdlGovernorate);
                    }*/
                }
                //new condition
                if (Session["User_ID"] == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }

                Authenticate(context);

                if (IsPostBack) return;
                bool flag = false;

                var users = context.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (!users.Any()) Response.Redirect("default.aspx");
                // counters for system admin only
                var currentuser = users.FirstOrDefault();
                if (currentuser == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }
                var id = currentuser.GroupID;
                var path = HttpContext.Current.Request.Url.AbsolutePath;
                var fileInfo = new FileInfo(path);
                var name = fileInfo.Name;
                if (id == null)
                {
                    Response.Redirect("~/Pages/default.aspx");
                    return;
                }
                var groupId = id.Value;
                var privileges =
                    context.GroupPrivileges.Where(gp => gp.Group.ID == groupId).OrderBy(gp => gp.privilege.PrivOrder);
                if (Enumerable.Any(privileges, privilege => privilege.privilege.Name == name
                                                            || privilege.privilege.Name == "All"
                                                            || name == "home.aspx"
                                                            || privilege.privilege.ParentPageName == name))
                {
                    flag = true;
                }
                if (!flag) Response.Redirect("default.aspx");
                lbl_UserName.Text = currentuser.UserName;
                lblDataLevel.Text = currentuser.Group.GroupName;
                lbl_IP.Text = Tokens.IPMsg + Request.ServerVariables["REMOTE_ADDR"];

                var inbox = context.Messages.Count(x => x.MessageTo == currentuser.ID & !Convert.ToBoolean(x.DoneRead));
                lblInbox.Text = inbox.ToString(CultureInfo.InvariantCulture); //lbltitleInbox.Text
                //  var tick = EditControls.GetTicketCount("ViewTickets.aspx?ts=mELirpUhRYksFj7k8/XBcQ==",
                // Convert.ToInt32(HttpContext.Current.Session["User_ID"]), groupId); //currentuser.GroupID.Value);
                //  var pendingtick = EditControls.GetTicketCount("ViewTickets.aspx?ts=BOII5FUynjpl5RZJJ8nW1g==",
                //  Convert.ToInt32(HttpContext.Current.Session["User_ID"]), groupId); //currentuser.GroupID.Value);
                //  lblTicketCount.Text = (tick + pendingtick).ToString(CultureInfo.InvariantCulture);
                //   var offer = context.OffersDetails.Count();
                //   lblOffersCount.Text = offer.ToString(CultureInfo.InvariantCulture);

                //var cultureService = new CultureService();
                //var culture = cultureService.GetUserCultureName(currentuser.ID);
                //hfRtl.Value = culture == "1" ? "1" : "2";
                //UpdateSiteData();
                PopulateLinks(context);

            }
        }

        
        public string LoadNew()
        {
            using (var context =
                new ISPDataContext(
                    ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture)))
            {
                var sb = new StringBuilder();
                var query = context.News.Where(ns => ns.ID == 1);
                sb.Append("<span>" + query.First().Title + "</span>  :  ");
                sb.Append(query.First().Details);
                return sb.ToString();
            }
        }
        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            using (var context =
               new ISPDataContext(
                   ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture)))
            {
                context.UserTrackings.InsertOnSubmit(new Db.UserTracking()
                {
                    
                    Date = DateTime.Now.AddHours(),
                    // process type 2 for sign out from ProcessType table
                    ProcessTypeId = 2,
                    UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]),
                    Note = "تسجيل خروج"

                });
                context.SubmitChanges();
            }
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
           

            //Session.Abandon();
            FormsAuthentication.RedirectToLoginPage("default.aspx");
        }
        /*void PopulateCultures()
        {
            var cultures = dataContext.Cultures;
            ListBox1.DataSource = cultures;
            ListBox1.DataTextField = "Name";
            ListBox1.DataValueField = "Id";
            ListBox1.DataBind();
        }


         protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e){
            ChangeCulture();
        }


       void ChangeCulture(){
            var selectedCulture = Convert.ToInt32(ListBox1.SelectedItem.Value);
            int userId = Convert.ToInt32(Session["User_ID"]);
            var cultureService = new CultureService();
            cultureService.UpdateUserCulture(selectedCulture, userId);
            Session["redirected"] = true;
            Response.Redirect(Request.Url.ToString());
        }
        void PointDropTOSelectedCulture()
        {
            int userId = Convert.ToInt32(Session["User_ID"]);
            var cultureService = new CultureService();
            ListBox1.SelectedValue = cultureService.GetUserCultureName(userId);
        }*/
        void Authenticate(ISPDataContext context)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }
            var loginName = Request.RequestContext.HttpContext.User.Identity.Name;
            if (loginName == string.Empty)
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }
            var user = context.Users.FirstOrDefault(u => u.LoginName == loginName);
            if (user == null || (user.IsAccountStopped != null && user.IsAccountStopped.Value))
            {
                Response.Redirect("~/Pages/default.aspx");
                return;
            }
            Session["User_ID"] = user.ID;
            var clientIp = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(user.Ip))
            {
                if (!clientIp.Equals(user.Ip))
                {
                    Response.Redirect("~/Pages/default.aspx");
                }
            }
            //MsgIp.InnerHtml = string.Empty;
            // MsgIp.Visible = false;

        }
        void GetQuickSupport(ISPDataContext context)
        {
            var url = Request.Url;
            var lastOrDefault = url.ToString().Split(new string[] { "pages/" }, StringSplitOptions.None).LastOrDefault();
            if (lastOrDefault == null) return;
            var quickSupport = context.QuickSupports.FirstOrDefault(x => x.Url.ToLower() == lastOrDefault.ToLower());
            if (quickSupport == null) return;
            l_current.Text = quickSupport.Body;
        }
        protected void SearchForCustomer(object sender, EventArgs e)
        {
            if (PhoneNumber.Value.Length <= 0) return;
            //var option = OptionsService.GetOptions(dataContext, true);
            const string govId = ""; //option != null && option.IncludeGovernorateInSearch :? DdlGovernorate.SelectedIndex > 0 ? DdlGovernorate.SelectedItem.Value : "" : "";
            Response.Redirect(string.Format("~/Pages/Search.aspx?g={0}&pn={1}&by={2}&sm=s", QueryStringSecurity.Encrypt(govId), QueryStringSecurity.Encrypt(PhoneNumber.Value), QueryStringSecurity.Encrypt("0")));
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["draw"] != null)
            {
                string currentUrl = HttpContext.Current.Request.Url.LocalPath;

                if (currentUrl.EndsWith("home.aspx") || currentUrl.Contains("home.aspx"))
                {
                    DrawMenu();
                }
                else
                {
                    Label1.InnerHtml = Session["draw"].ToString();
                }
               
            }
            else
            {
                DrawMenu();
            }
           
        }
        private void DrawMenu()
        {
            var localizer = new Loc();
            // var divs = new List<string>();
            using (var context =
                new ISPDataContext(
                    ConfigurationManager.AppSettings["ConnectionString"].ToString(CultureInfo.InvariantCulture)))
            {
                var user = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (user != null)
                {
                    var id = user.GroupID;
                    if (id != null)
                    {
                        var groupId = id.Value;
                        var datalevel = Convert.ToInt32(user.Group.DataLevelID);
                        var option = context.Options.First();
                        var showNumbers = option.ShowCounters;
                        var allParent =
                            context.privileges.Where(parent => parent.ParentID == 1).OrderBy(parent => parent.PrivOrder);
                        //int count = 1;

                        foreach (var parent in allParent)
                        {
                            var notInserted = true;
                            var privilege = parent;
                            var parentPrivilageQuery = context.GroupPrivileges
                                .Where(gp => gp.Group.ID == groupId
                                             && gp.privilege.ParentID == privilege.ID)
                                .OrderBy(gp => gp.privilege.PrivOrder).Select(gp =>
                                    new
                                    {
                                        gp.privilege.LinkedName,
                                        gp.privilege.Url,
                                        gp.privilege.ISLinked,
                                        gp.privilege.ParentPageName
                                    });

                            if (!parentPrivilageQuery.Any()) continue;
                            //string divName = "Div" + count;
                            //divs.Add(divName);
                            Menu.Append("<li>");//if (parent.ParentID==1) Menu.Append("<li class='active'>"); else
                            Menu.Append("<a href='#' class='dropdown-toggle'>"//<i class='icon-desktop'></i>
                                        + "<span class='menu-text'> " + localizer.IterateResource(parent.LinkedName) + " </span><b class='arrow icon-angle-down'></b></a>");


                            /*Menu.Append("<div id=" + divName + " class=\"CollapsiblePanel\">");
                                Menu.Append("<div class=\"CollapsiblePanelTab\" tabindex=\"0\">" +
                                            localizer.IterateResource(parent.LinkedName) + "</div>");
                                Menu.Append("<div class=\"CollapsiblePanelContent\">");*/
                            Menu.Append("<ul class='submenu'>");
                            foreach (var child in parentPrivilageQuery)
                            {
                                if (child.ISLinked.Value) Menu.Append("<li>");
                                if (parent.ID == 27) // case new customer 
                                {
                                    var allPrev =
                                        context.GroupPrivileges.Where(gp => gp.GroupID == groupId)
                                            .Select(gp => gp.PrivilegeID);
                                    if (!allPrev.Contains(28) && notInserted)
                                    {
                                        if (allPrev.Contains(29) || allPrev.Contains(30))
                                        {
                                            Menu.Append("&nbsp;<a href=\"AddNewCustomer.aspx?t=t\">" + Tokens.AddNewCustomer +
                                                        "<i class='icon-double-angle-right'></i></a><br/>");
                                            notInserted = false;
                                        }
                                    }
                                }
                                if (parent.ID == 47 && child.ISLinked.Value)
                                // case Pending Requests to add the number of wo
                                {
                                    if (Convert.ToBoolean(showNumbers) && child.Url.Contains("ManageRequests.aspx") || child.Url.Contains("ManageOfferRequest.aspx") || child.Url.Contains("ManageResellerRequests.aspx"))
                                    {
                                        Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                    localizer.IterateResource(child.LinkedName) +
                                                    " (" + EditControls.GetRequestWoCount(child.Url,
                                                        Convert.ToInt32(Session["User_ID"]),
                                                        groupId).ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                    }
                                    else
                                    {
                                        Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                    localizer.IterateResource(child.LinkedName) + "<i class='icon-double-angle-right'></i></a><br/>");
                                    }

                                }
                                else if (parent.ID == 13 && child.ISLinked.Value)
                                // case pending activation to add the number of wo
                                {
                                    if (Convert.ToBoolean(showNumbers) && child.Url.Contains("Reports.aspx"))
                                    {
                                        Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                    localizer.IterateResource(child.LinkedName) +
                                                    " (" + EditControls.GetStateWoCount(child.Url,
                                                        Convert.ToInt32(Session["User_ID"]), groupId)
                                                        .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                    }
                                    else
                                    {
                                        Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                    localizer.IterateResource(child.LinkedName) + "<i class='icon-double-angle-right'></i></a><br/>");
                                    }

                                }
                                else if (parent.ID == 61 && child.ISLinked.Value &&
                                         child.ParentPageName == "ViewTickets.aspx")
                                {
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + " (" +
                                                EditControls.GetTicketCount(child.Url,
                                                    Convert.ToInt32(Session["User_ID"]), groupId)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if (child.Url != null && child.Url.Contains("Inbox.aspx"))
                                {
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetInboxCount(child.Url, Convert.ToInt32(Session["User_ID"]))
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "HandelRechargeRequestes"))
                                {

                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetRechargeRequestsCount(Convert.ToInt32(Session["User_ID"]), datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "HandelRechargeBranch"))
                                {

                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetRechargeRequestsCountBranch(Convert.ToInt32(Session["User_ID"]), datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "HandelCustomerRecharge"))
                                {
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetRechargeClientRequestsCount(Convert.ToInt32(Session["User_ID"]), datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "HandelBrancheRecharge"))
                                {

                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetRechargeBranchRequestsCount(Convert.ToInt32(Session["User_ID"]), datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "ResellerPPR"))
                                {
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetResellerPprCount(user.ID, datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "BranchPPR"))
                                {
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetBranchPprCount(user.ID, datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if ((child.LinkedName == "HandelResellersTransfers"))
                                {
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "(" +
                                                EditControls.GetHandelResellersTransfersCount(user.ID, datalevel)
                                                    .ToString(CultureInfo.InvariantCulture) + ")<i class='icon-double-angle-right'></i></a><br/>");
                                }
                                else if (child.ISLinked.Value)
                                    Menu.Append("&nbsp;<a href=\"" + child.Url + "\">" +
                                                localizer.IterateResource(child.LinkedName) + "<i class='icon-double-angle-right'></i></a><br/>"); Menu.Append("</li>");
                                //Menu.Append("</li>");
                            }
                            Menu.Append("</ul>");
                            //count++;
                        }
                        Menu.Append("<script type=\"text/javascript\">");
                        /*foreach (string divName in divs)
                        {
                            Menu.Append("var" + divName + " = new Spry.Widget.CollapsiblePanel(" + divName +
                                        ",{contentIsOpen:false});");
                        }*/
                    }
                }
                Menu.Append("</script>");
                Session["draw"] = Menu.ToString();
                Label1.InnerHtml = Menu.ToString();
            }
        }
        void PopulateLinks(ISPDataContext context)
        {
            Linklist.DataSource = context.FriendlyLinks.ToList();
            Linklist.DataBind();
        }
        void UpdateSiteData()
        {
            var data = _siteDateRepository.SiteData();
            if (data == null)
            {
                //ImgLogo.ImageUrl = 
                lComapnyName.Text = string.Empty;
                return;
            }
            //ImgLogo.ImageUrl = "../SiteLogo/" + data.LogoUrl;
            lComapnyName.Text = data.SiteName;
            lnkSiteUrl.HRef = "http://";
            lnkSiteUrl.HRef += data.SiteUrl;
            sitename.HRef = "http://";
            sitename.HRef += data.SiteUrl;
            Literal3.Text = data.SiteName;
        }
    }
}