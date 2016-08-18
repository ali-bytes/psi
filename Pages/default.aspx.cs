using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using BL.Concrete;
using Resources;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Services;

namespace NewIspNL.Pages
{
    public partial class Default : CustomDefaultPage
    {

        readonly IspEntries _ispEntries;

        public Default()
        {
            _ispEntries = new IspEntries();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //PopulateCultures();
                /* Create a thread to throw an exception
                var thread = new Thread(() => { throw new ArgumentException(); });

                // Start the thread to throw the exception
                thread.Start();

                // Wait a short while to give the thread time to start and throw
                Thread.Sleep(50);
                //RestartApplicationPool();*/

            }

            /*if(Session["cultureChoice"] != null && !string.IsNullOrEmpty(Session["cultureChoice"].ToString())){
                DdlCultures.SelectedValue = Session["cultureChoice"].ToString();
            }*/

        }
        #region Hashed
        /*protected void RestartApplicationPool()
    {
        string name = HttpContext.Current.Request.ServerVariables["APP_POOL_ID"];

        if (String.IsNullOrEmpty(name))
            name = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
        string appPoolName = name;
        string appPoolPath = @"IIS://" + Environment.MachineName + "/W3SVC/AppPools/" + appPoolName;//"IIS://localhost/W3SVC/AppPools/"+appPoolName;
        int intStatus = 0;
        try
        {
            var con = new LdapConnection(new LdapDirectoryIdentifier("server",80));
            con.SessionOptions.SecureSocketLayer = true;
            con.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback(true);
            con.Credential = new NetworkCredential(String.Empty, String.Empty);
            con.AuthType = AuthType.Basic;
            con.Bind();
            var w3Svc = new DirectoryEntry(appPoolPath, null, null, AuthenticationTypes.Anonymous);// {Path = appPoolPath, AuthenticationType = AuthenticationTypes.Secure};
            
            /*var search = new DirectorySearcher {SearchRoot = w3Svc};
            var result = search.FindAll();
            if (result.Count > 0)
            {
                //intStatus = (int)w3Svc.InvokeGet("AppPoolState");
            //w3Svc.RefreshCache();
            //ServerManager manager = new ServerManager();
                //w3Svc.Invoke("Stop", null);
                //w3Svc.Invoke("Start", null);
                //w3Svc.Invoke("start", e);
                //status();
              //var server = new ServerManager();
            //}
        
           } ServerManager manager = new ServerManager();
        foreach (Site site in manager.Sites)
        {
            foreach (Application app in site.Applications)
            {

                if (app.ApplicationPoolName.ToString() == AppPoolName)
                {
                     string appname = app.Path;
                }
            }
        }
        
            try
        {
            DirectoryEntry w3svc = new DirectoryEntry(appPoolPath);
            w3svc.Invoke("Start", null);
            status();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    public bool ServerCallBack(LdapConnection connection, X509Certificate certificate)
    {
        return true;
    }

*/
        /*void PopulateCultures(){
            using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
                var cultures = dataContext.Cultures;
                DdlCultures.DataSource = cultures;
                DdlCultures.DataTextField = "Name";
                DdlCultures.DataValueField = "Id";
                DdlCultures.DataBind();
            }
        }*/
        #endregion
        #region OldPage
        /*
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e){
        using(var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            e.Authenticated = Authentication(Login1.UserName, Login1.Password);
            if(!e.Authenticated) return;
            var ticket = new FormsAuthenticationTicket(2,
                Login1.UserName,
                DateTime.Now.AddHours(),
                DateTime.Now.AddHours().AddMinutes(720), Login1.RememberMeSet, null, FormsAuthentication.FormsCookiePath);
            string hashCookies = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SetAuthCookie(Login1.UserName, true);
            var userId = Convert.ToInt32(Session["User_ID"]);
            var user = context.Users.FirstOrDefault(u => u.ID ==userId);
            if(user == null){
                return;
            }
            string clientIp = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            if(user.Ip != null && !string.IsNullOrWhiteSpace(user.Ip) && !clientIp.Equals(user.Ip)){

                Session["loginIpError"] = Tokens.UserIp + " " + Tokens.DoesntMatch;
                return;
            }
            Session["loginIpError"] = "";
            //context.GetType().InvokeMember("ClearCache",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,null, context, null);
            Response.Redirect("home.aspx");
        }
    }


    public Boolean Authentication(string username, string password){
        using(var dataContext = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"])){
            var user = dataContext.Users
                .FirstOrDefault(usr => usr.LoginName == username.Trim() && usr.LoginPassword == Security.EncodePassword(password));
            if(user == null){
                return false;
            }
            if(user.IsAccountStopped != null){
                if(user.IsAccountStopped.Value){
                    return false;
                }
            }
           /* if(Session["cultureChoice"] != null && !string.IsNullOrEmpty(Session["cultureChoice"].ToString())){
              var cultureService = new CultureService();
              cultureService.UpdateUserCulture(Convert.ToInt32(Session["cultureChoice"]), user.ID);
               Session["User_ID"] = user.ID;
            }
            Session.Add("User_ID", user.ID);
            return true;
        }
    }



    protected void lb_subscribe_Click(object sender, EventArgs e){
        Session.Add("LoginUser", "external");
        Session.Add("User_ID", -1);
        Response.Redirect("SubscripeNewService.aspx");
    }


    protected void LinkButton1_Click(object sender, EventArgs e){
        Response.Redirect("AboutUs.aspx");
    }


    protected void LinkButton2_Click(object sender, EventArgs e){
        Response.Redirect("ContactUs.aspx");
    }


    protected void DdlCultures_SelectedIndexChanged(object sender, EventArgs e){
        ChangeCulture();
    }


    void ChangeCulture(){
       // Session["cultureChoice"] = DdlCultures.SelectedItem.Value;
    }
   /* public override void VerifyRenderingInServerForm(Control control) { }

    public VerifyServerCertificateCallback ServerCallback { get; set; }*/
        #endregion

        protected void Login1_Authenticate(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var userName = txtUsername.Value;
                var password = txtPassword.Text;
                //var rememberMe = CheckMemberMe.Checked;
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) return;
                var authenticated = Authentication(userName, password, context);
                if (!authenticated)
                {
                    lblError.Visible = true;
                    //lblError.InnerHtml = Tokens.User_doesn_t_exsists_;
                    return;
                }
                var ticket = new FormsAuthenticationTicket(2,
                    userName,
                    DateTime.Now.AddHours(),
                    DateTime.Now.AddHours().AddMinutes(720), false, null, FormsAuthentication.FormsCookiePath);
                var hashCookies = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(userName, true);
                var userId = Convert.ToInt32(Session["User_ID"]);
                var user = context.Users.FirstOrDefault(u => u.ID == userId);
                if (user == null)
                {
                    return;
                }
                var clientIp = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
                if (user.Ip != null && !string.IsNullOrWhiteSpace(user.Ip) && !clientIp.Equals(user.Ip))
                {

                    Session["loginIpError"] = Tokens.UserIp + " " + Tokens.DoesntMatch;
                    return;
                }
                Session["loginIpError"] = "";
                //context.GetType().InvokeMember("ClearCache",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,null, context, null);

                var cultureService = new CultureService();
                var culture = cultureService.GetUserCultureName(user.ID);
                Session["cultureid"] = culture == "1" ? "1" : "2";

                _ispEntries.AddUserTrack(new Db.UserTracking()
                {
                    Date = DateTime.Now.AddHours(),
                    // process type 1 for sign out from ProcessType table
                    ProcessTypeId = 1,
                    UserId = Convert.ToInt32(HttpContext.Current.Session["User_ID"]),
                    Note = "تسجيل دخول - " + clientIp
                });
                _ispEntries.Commit();
                if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) && !Request.QueryString["ReturnUrl"].Contains("default.aspx") && Request.QueryString["ReturnUrl"].Contains(".aspx"))
                {
                   Response.Redirect(Request.QueryString["ReturnUrl"]);
                }
                else
                {
                    Response.Redirect("/pages/home.aspx");
                }
            }
        }


        public Boolean Authentication(string username, string password, ISPDataContext dataContext)
        {
            var user = dataContext.Users
                .Where(usr => usr.LoginName == username.Trim()).ToList();
            if (user.Count == 0)
            {
                lblError.InnerHtml = Tokens.UserNotFounded;
                return false;
            }
            var us = user.FirstOrDefault(usr => usr.LoginPassword == Security.EncodePassword(password));

            if (us == null)
            {
                lblError.InnerHtml = Tokens.PassWordError;
                return false;
            }
            if (us.IsAccountStopped != null)
            {
                if (us.IsAccountStopped.Value)
                {
                    lblError.InnerHtml = Tokens.UserIsStoped;
                    return false;
                }
            }
            /* if(Session["cultureChoice"] != null && !string.IsNullOrEmpty(Session["cultureChoice"].ToString())){
               var cultureService = new CultureService();
               cultureService.UpdateUserCulture(Convert.ToInt32(Session["cultureChoice"]), user.ID);
                Session["User_ID"] = user.ID;
             }*/
            Session.Add("User_ID", us.ID);
            return true;
        }

        protected void BtnSave(object sender, EventArgs e)
        {
            var culture = radioArabic.Checked ? 1 : 2;
            AddNewResellerRequest(txtArabicName.Value, txtEnglishName.Value, ddlCompanyType.SelectedItem.Value, ddlEmployeeNumbers.SelectedItem.Value,
                txtCompanyActivity.Value, txtComapnyAddress.Value, txtCompanyTele.Value, txtfax.Value, txtCompanyEmail.Value,
                txtCompanyMobile.Value, txtResellerName.Value, txtNationalNumber.Value, txtResellerMobile.Value,
                txtResellerEmail.Value, txtResellerUsername.Value, txtResellerPassword.Text.Trim(), culture);
        }

        //[WebMethod]
        public int AddNewResellerRequest(string arabicName, string englishName, string companyType, string employeeNumber,
        string companyActiviy, string companyAddress, string companyTelephone, string fax, string companyEmail,
        string companyMobile, string resellerName, string national, string resellerMobile, string resellerEmail,
        string userName, string passWord, int culture)
        {
            try
            {
                int status;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var obj = new NewResellerRequest
                    {
                        CompanyArabicName = arabicName,
                        CompanyEnglishName = englishName,
                        CompanyType = companyType,
                        EmployeeNumbers = employeeNumber,
                        CompanyActivities = companyActiviy,
                        CompanyAddress = companyAddress,
                        CompanyTelephone = companyTelephone,
                        FaxNumber = fax,
                        CompanyEmail = companyEmail,
                        CompanyMobile = companyMobile,
                        ResellerName = resellerName,
                        ResellerNationalNumber = national,
                        ResellerMobile = resellerMobile,
                        ResellerEmail = resellerEmail,
                        ResellerUsername = userName,
                        ResellerPassword = passWord,
                        ResellerCulture = culture
                    };
                    context.NewResellerRequests.InsertOnSubmit(obj);
                    context.SubmitChanges();
                    status = obj.Id;
                }
                return status;
            }
            catch
            {
                return -1;
            }
        }

        protected void btn_ForgotPass_Click(object sender, EventArgs e)
        {
            forgotMsg.Visible = false;
            if (string.IsNullOrEmpty(ForgotPassEmail.Value) && string.IsNullOrEmpty(ForgotPassUserName.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('لم يتم الإرسال خطأ فى الأيميل او أسم المستخدم')", true);
                return;
            }
            var email = ForgotPassEmail.Value;
            var userName = ForgotPassUserName.Value;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var user = context.Users.FirstOrDefault(x => x.UserEmail.Equals(email) && x.LoginName.Equals(userName));
                if (user != null)
                {
                    var n = Membership.GeneratePassword(6, 0);
                    user.LoginPassword = Security.EncodePassword(n);
                    context.SubmitChanges();
                    SendPasswordToUser(user.LoginName, n, user.UserEmail, context);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('تم إرسال رسالة بالباسورد الجديد')", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('لم يتم الإرسال خطأ فى الأيميل او أسم المستخدم')", true);
                }
            }
        }

        private static void SendPasswordToUser(string name, string newPassword, string email, ISPDataContext context)
        {
            var active = context.EmailCnfgs.FirstOrDefault();
            if (active == null || !active.Active) return;
            var msg =
                "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                ":" + Tokens.Dear + "</span></div>" + name +
                "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.NewPassword +
                "</div> <span><br/> " + newPassword +
                "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" +
                Tokens.NowYouCanSignin + "<br/> Thank you </p></div>";
            var formalmessage =
                ClsEmail.Body(msg);
            ClsEmail.SendEmail(email, "Dear : " + name + " Your new password", formalmessage, true);

        }

    }
}
