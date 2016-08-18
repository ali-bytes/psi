<%@ Application Language="C#" %>
<%--<%@ Import Namespace="System.DirectoryServices" %>--%>
<%@ Import Namespace="System.IO.Compression" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Db" %>
<%@ Import Namespace="NewIspNL" %>
<%@ Import Namespace="NewIspNL.Domain" %>

<%@ Import Namespace="NewIspNL.Helpers" %>
<%@ Import Namespace="NewIspNL.Models" %>
<%@ Import Namespace="NewIspNL.Pages" %>
<%@ Import Namespace="NewIspNL.Services" %>
<%@ Import Namespace="NewIspNL.Services.DemandServices" %>

<script RunAt="server">

    //public static DateTime checkDate { set; get; }
    private void Application_Start(object sender, EventArgs e)
    {
        BundleConfig.RegisterBundles(BundleTable.Bundles);
        //System.Web.Security.Roles.Enabled;
        //Process(RouteTable.Routes); 
        ThreadStart tsTask = TaskLoop;
        var myTask = new Thread(tsTask);
        myTask.Start();

        ThreadStart tsTask2 = TaskLoop2;
        var myTask2 = new Thread(tsTask2);
        myTask2.Start();

        Susfortempactive();
      OptionsService.CheckResellerPayment();

    }

   
    
    
    
    public void Susfortempactive()
    {
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {


            var allrequest = context.WorkOrderRequests.Where(x => x.RSID == 1 && x.RequestID == 3 && x.PeriodId != null ).ToList();
            foreach (var i in allrequest)
            {
                try
                {

               
                var date = new DateTime();
                if (i.PeriodId == 1)
                {
                    date = i.ProcessDate.Value.AddDays(1);
                }
                else if (i.PeriodId == 2)
                {
                    date = i.ProcessDate.Value.AddDays(2);
                }
                else if (i.PeriodId == 3)
                {
                    date = i.ProcessDate.Value.AddDays(3);
                }

                if (date.Date == DateTime.Now.Date.AddHours() || date.Date < DateTime.Now.Date.AddHours())
                {

                    var  unpaiddemands = context.Demands.Where(x => x.WorkOrderId == i.WorkOrderID && x.Paid != true&&x.Amount>0).ToList();
                    if (unpaiddemands.Count > 0)
                    {
                        //portal suspend
                        var wor = new WorkOrderRequest();
                        var portalList = context.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                        var woproviderList = context.WorkOrders.FirstOrDefault(z => z.ID == i.WorkOrderID);
                        if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
                        {
                            if (woproviderList.UserName != null)
                            {
                                var username = woproviderList.UserName;
                                CookieContainer cookiecon = new CookieContainer();
                                cookiecon = Tedata.Login();
                                if (cookiecon != null)
                                {
                                    var pagetext = Tedata.GetSearchPage(username, cookiecon);
                                    if (pagetext != null)
                                    {
                                         var searchPage = Tedata.CheckSearchPage(pagetext);
                                        if (searchPage)
                                        {
                                            var custStatus = Tedata.CheckCustomerStatus(pagetext);
                                            if (custStatus == "disable")
                                            {
                                                i.PeriodId = null;
                                                context.SubmitChanges();
                                                continue;
                                            }
                                            else
                                            {
                                                var worNote = Tedata.SendTedataSuspendRequest(username, cookiecon, pagetext);
                                                if (worNote == 2)
                                                {
                                                    //فى حالة البورتال واقع
                                                    wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال ";
                                                }
                                                else
                                                {
                                                    //فى حالة نجاح الارسال الى البورتال ننزل الطلب متوافق علية فى اى اس بى
                                                    wor.WorkOrderID = i.WorkOrderID;
                                                    wor.RequestID = 2;
                                                    wor.ConfirmerID = 1;
                                                    wor.RequestDate = DateTime.Now.AddHours();
                                                    wor.ProcessDate = DateTime.Now.AddHours();
                                                    wor.RSID = 1;
                                                    wor.SenderID = 1;
                                                    wor.CurrentPackageID = i.WorkOrder.ServicePackageID;
                                                    wor.NewPackageID = i.WorkOrder.ServicePackageID;
                                                    wor.NewIpPackageID = i.WorkOrder.IpPackageID;
                                                    wor.Notes = "العميل تخطي مدة التشغيل المؤقت ";
                                                    i.PeriodId = null;
                                                    context.WorkOrderRequests.InsertOnSubmit(wor);
                                                    context.SubmitChanges();


                                                    //تغيير الحالة الى suspend
                                                    var current = context.WorkOrders.FirstOrDefault(x => x.ID == i.WorkOrderID);

                                                    if (current != null)
                                                    {
                                                        current.WorkOrderStatusID = 11;

                                                        global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                                                        {
                                                            WorkOrderID = current.ID,
                                                            StatusID = 11,
                                                            UserID = 1,
                                                            UpdateDate = DateTime.Now.AddHours(),
                                                        };
                                                        context.WorkOrderStatus.InsertOnSubmit(wos);
                                                        context.SubmitChanges();
                                                    }

                                                   
                                                    continue;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //فى حالة البورتال واقع
                                            wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال ";
                                        }

                                    }
                                    else
                                    {
                                        //فى حالة البورتال واقع
                                        wor.Notes = "لم يتم ارسال الطلب إلى البورتال تى إى داتا بسبب عدم إستجابة البورتال ";
                                    }
                                }
                                else
                                {
                                    //فى حالة البورتال واقع
                                    wor.Notes = "لم يتم ارسال الطلب إلى البورتال بسبب عدم إستجابة البورتال رجاءً تأكد من Portal User Name or Portal Password";
                                }
                            }
                        }
                        
                        wor.WorkOrderID = i.WorkOrderID;
                        wor.RequestID = 2;
                        wor.RequestDate = DateTime.Now.AddHours();
                        wor.ProcessDate = DateTime.Now.AddHours();
                        wor.RSID = 3;
                        wor.SenderID = 1;
                        wor.CurrentPackageID = i.WorkOrder.ServicePackageID;
                        wor.NewPackageID = i.WorkOrder.ServicePackageID;
                        wor.NewIpPackageID = i.WorkOrder.IpPackageID;
                        wor.Notes += " العميل تخطي مدة التشغيل المؤقت  ";
                        context.WorkOrderRequests.InsertOnSubmit(wor);
                        i.Notes = i.Notes + "تم الفحص ";
                        i.PeriodId = null;
                        context.SubmitChanges();
                       
                        //-----------
                        //var wor = new WorkOrderRequest
                        //{
                        //    WorkOrderID = i.WorkOrderID,
                        //    RequestID = 2,
                        //    RequestDate = DateTime.Now,
                        //    Notes = "العميل تخطي مدة التشغيل المؤقت ",
                        //    RSID = 3,
                        //    SenderID = 1,
                        //    CurrentPackageID = i.WorkOrder.ServicePackageID,
                        //    NewPackageID = i.WorkOrder.ServicePackageID,
                        //    NewIpPackageID = i.WorkOrder.IpPackageID
                            
                        //};
                        //context.WorkOrderRequests.InsertOnSubmit(wor);
                        //i.Notes = i.Notes + "تم الفحص ";
                        //context.SubmitChanges();
                    }
                    else
                    {
                        i.PeriodId = null;
                        context.SubmitChanges();
                    }

                }
                
                }
                catch (Exception)
                {

                   continue;
                }
                
            }
         
                
        }
    }
    
    
    
    
    
    
    
    static void TaskLoop()
    {
        // In this example, task will repeat in infinite loop
        // You can additional parameter if you want to have an option 
        // to stop the task from some page
        //if (myTask.IsAlive)
        while (true)
        {
            // Execute scheduled task
            ScheduledTask();
               
                // Wait for certain time interval
                Thread.Sleep(new TimeSpan(0, 24, 0, 0)); //wait for 24 Hours
        }
    }
    static void TaskLoop2()
    {
        // In this example, task will repeat in infinite loop
        // You can additional parameter if you want to have an option 
        // to stop the task from some page
        //if (myTask.IsAlive)
        while (true)
        {
            // Execute scheduled task
            ScheduledTask2();

            // Wait for certain time interval
            Thread.Sleep(new TimeSpan(0, 1, 0, 0)); //wait for 1 Hours
        }
    }

    private static void ScheduledTask()
    {
        // Task code which is executed periodically
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var demandService = new DemandService( context);
            var page = new Page();
            var now = DateTime.Now.AddHours();
            demandService.ProcessUnpaidDemands(now);
            demandService.ProcessDemands(now, page);
            demandService.ProcessSuspend(now);
          //  demandService.FawryProcess();
        }
    }
    private static void ScheduledTask2()
    {
        // Task code which is executed periodically
        using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        {
            var demandService = new DemandService(context);
           
            demandService.FawryProcess();
        }
    }

    private void Application_End(object sender, EventArgs e)
    {
    }


    private void Application_Error(object sender, EventArgs e)
    {
        try
        {
            string sysName = ConfigurationManager.AppSettings["sysName"];
            string sysVersion = ConfigurationManager.AppSettings["sysVersion"];
            Exception ex = Server.GetLastError().GetBaseException();
            Server.ClearError();
            string funcName = ex.TargetSite.Name;
            var logError = new XmlErrorFile.XmlErrorFile(sysName, sysVersion, "ErrorFile.xml", "~//ErrorFile");

            logError.Insert(XmlErrorFile.XmlErrorFile.GetCurrentPageName(), funcName, ex.Message);
            Session.Add("LastErrorPage", XmlErrorFile.XmlErrorFile.GetCurrentPageName());

        }
        catch
        {
        }
        finally
        {
            ////////HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Redirect(@"~/pages/ErrorPage.aspx");
        }
    }

    protected void Session_End(object sender, EventArgs e)
    {
    }

    protected void Session_Start(object sender, EventArgs e)
    {
        //Session.Timeout = 10;
    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {

       

        // Implement HTTP compression
        //HttpApplication app = (HttpApplication)sender;


        //// Retrieve accepted encodings
        //string encodings = app.Request.Headers.Get("Accept-Encoding");
        //if (encodings != null)
        //{
        //    // Check the browser accepts deflate or gzip (deflate takes preference)
        //    encodings = encodings.ToLower();
        //    if (encodings.Contains("deflate"))
        //    {
        //        app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
        //        app.Response.AppendHeader("Content-Encoding", "deflate");
        //    }
        //    else if (encodings.Contains("gzip"))
        //    {
        //        app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
        //        app.Response.AppendHeader("Content-Encoding", "gzip");
        //    }
        //}
        
        //var path = HttpContext.Current.Request.Path;
        //HttpContext.Current.RewritePath(path);
        //Rewriter.Process();
    }

    
    
        /*public XmlNode ORules;
    public object Create(object parent, object configContext, XmlNode section)
    {
        ORules = section;
        
        return this;
    }
    
    public string GetSubstitution(string zPath)
    {
        var xmlNodeList = ORules.SelectNodes("rule");
        if (xmlNodeList != null)
            foreach (XmlNode oNode in xmlNodeList)
            {
                var selectSingleNode = oNode.SelectSingleNode("url/text()");
                if (selectSingleNode != null)
                {
                    Regex oReg = new Regex(selectSingleNode.Value);
                    Match oMatch = oReg.Match(zPath);

                    if (oMatch.Success)
                    {
                        var xmlNode = oNode.SelectSingleNode("rewrite/text()");
                        if (xmlNode != null)
                            return oReg.Replace(zPath,
                                xmlNode.Value);
                    }
                }
            }

        return zPath;
    }

    void Process()
    {
        Rewriter oRewriter =
       (Rewriter)ConfigurationSettings.GetConfig("system.web/urlrewrites");

        string zSubst = GetSubstitution(HttpContext.Current.Request.Path);//oRewriter.GetSubstitution(HttpContext.Current.Request.Path);
        if (zSubst.Length > 0)
        {
            HttpContext.Current.RewritePath(zSubst);
        }
    }*/
</script>
