using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Db;
using HtmlAgilityPack;
using NewIspNL.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewIspNL.Domain
{
    public class Distributor
    {
        private static string viewStateToken = string.Empty;
        private static string idt = string.Empty;
        static WebProxy proxy = new WebProxy();
        static bool useProxy = false;

        public Distributor()
        {
            PopulateProxy();
        }
        public static CookieContainer GetLoginCookie()
        {
            try
            {
                CookieContainer cookieJar = new CookieContainer();
                HttpWebRequest httpRequest =
                           (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/index.ted");
                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequest.CookieContainer = cookieJar;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                PopulateProxy();
                if (useProxy)
                {
                    httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequest.Proxy = proxy;
                }
                else
                {
                    httpRequest.Proxy = null;
                }
                string text = "";
                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                        text = sr.ReadToEnd();
                    }
                }

                if (IsFirstLoginPage(text))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.OptionAutoCloseOnEnd = true;
                    doc.OptionFixNestedTags = true;
                    doc.LoadHtml(text);
                    HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@id='javax.faces.ViewState']");
                    if (vStateNode != null)
                    {
                        viewStateToken = vStateNode.GetAttributeValue("value", "");
                    }
                    HtmlNode idtNode = doc.DocumentNode.SelectSingleNode("//*[@value='Sign In']");
                    if (idtNode != null)
                    {
                        idt = idtNode.GetAttributeValue("name", "").Replace("signin:j_idt","").Trim();
                    }
                    return cookieJar;
                }
                else
                {
                    HttpContext.Current.Session["errormsg"] = "Try again later or Please check your device authorization on distributor";
                    return null;
                }
            }
            catch
            {
                HttpContext.Current.Session["errormsg"] = "Try again later or Please check your device authorization on distributor";
                return null;
            }
        }

        public static CookieContainer Login(CookieContainer cookieJar)
        {
            //login request
            var username = string.Empty;
            var pass = string.Empty;

            //CookieContainer cookieJar = new CookieContainer();
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();
                if (auth == null)
                {
                    HttpContext.Current.Session["errormsg"] = "Please check Distributor User name or password (LG)";
                    return null;
                }

                if (auth.DisUserName.Length > 0 && auth.DisPassword.Length > 0)
                {
                    username = auth.DisUserName;
                    pass = QueryStringSecurity.Decrypt(auth.DisPassword);
                }
                else
                {
                    HttpContext.Current.Session["errormsg"] = "Please check Distributor User name or password (LG)";
                    return null;
                }
                if (auth.DisUseProxy ?? false)
                {
                    if (!string.IsNullOrEmpty(auth.DisProxy) && auth.DisPort.HasValue)
                    {
                        proxy = new WebProxy(auth.DisProxy, auth.DisPort ?? 80);
                        proxy.BypassProxyOnLocal = false;
                        useProxy = true;
                    }
                }
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
            {
                HttpContext.Current.Session["errormsg"] = "Please check Distributor User name or password (LG)";
                return null;
            }
            try
            {
                // signin=signin&signin%3Ausername=smartqueenadmin&signin%3Apassword=123456789&signin%3Aj_idt18=Sign+In&javax.faces.ViewState=-4154174852824765831%3A1753199359541538591
                // signin=signin&signin%3Ausername={0}&signin%3Apassword={1}&signin%3Aj_idt18=Sign+In&javax.faces.ViewState=-2624786525568649188%3A7537472255374654693
                string poststring =
                    string.Format(
                       //signin=signin&signin%3Ausername={0}&signin%3Apassword={1}&signin%3Aj_idt25=Sign+In&javax.faces.ViewState=-7230246125115000109%3A-7531590997029558091
                        "signin=signin&signin%3Ausername={0}&signin%3Apassword={1}&signin%3Aj_idt{2}=Sign+In&javax.faces.ViewState={3}",
                        username, pass, idt, viewStateToken);

                //HttpWebRequest httpRequest =
                //    (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/index.ted;jsessionid=" + sessionId+"");
                HttpWebRequest httpRequest =
                    (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/index.ted");

                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequest.CookieContainer = cookieJar;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequest.Proxy = proxy;
                }
                else
                {
                    httpRequest.Proxy = null;
                }

                byte[] bytedata = Encoding.UTF8.GetBytes(poststring);
                httpRequest.ContentLength = bytedata.Length;

                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytedata, 0, bytedata.Length);
                requestStream.Close();

                string text = "";
                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                        text = sr.ReadToEnd();
                    }
                }

                if (CheckIfUserLogedIn(text))
                {
                    return cookieJar;
                }
                else
                {
                    HttpContext.Current.Session["errormsg"] = "Try again later or Please check customer phone or Distributor User name or password (LG)";
                    return null;
                }

            }
            catch (WebException wex)
            {
                var pageContent = new StreamReader(wex.Response.GetResponseStream())
                          .ReadToEnd();
                HttpContext.Current.Session["errormsg"] = "Try again later or Please check customer phone or Distributor User name or password (LG)";
                return null;
            }
        }

        public static string GetCustomerDetailsPage(CookieContainer cookieJar)
        {
            HttpWebRequest httpRequest =
                      (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            httpRequest.CookieContainer = cookieJar;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                httpRequest.Proxy = proxy;
            }
            else
            {
                httpRequest.Proxy = null;
            }
            string text = "";
            using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
            {
                using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }

            if (IsSearchForCustomerPage(text))
            {
                return text;
            }
            else
            {
                return null;
            }
        }

        public static string GetCustomerDetailsToken(string page)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            string token = string.Empty;
            HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@id='javax.faces.ViewState']");
            if (vStateNode != null)
            {
                token = vStateNode.GetAttributeValue("value", "");
            }

            return token;
        }

        public static string GetCustomerDetailsToken(CookieContainer cookieJar)
        {
            try
            {
                HttpWebRequest httpRequest =
                          (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");
                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequest.CookieContainer = cookieJar;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequest.Proxy = proxy;
                }
                else
                {
                    httpRequest.Proxy = null;
                }
                string text = "";
                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                        text = sr.ReadToEnd();
                    }
                }

                if (IsSearchForCustomerPage(text))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.OptionAutoCloseOnEnd = true;
                    doc.OptionFixNestedTags = true;
                    doc.LoadHtml(text);

                    string token = string.Empty;
                    HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@id='javax.faces.ViewState']");
                    if (vStateNode != null)
                    {
                        token = vStateNode.GetAttributeValue("value", "");
                    }

                    return token;
                }
                else
                {
                    HttpContext.Current.Session["errormsg"] = "Try again later or Please check customer phone or Distributor User name or password (DT)";
                    return null;
                }
            }
            catch
            {
                HttpContext.Current.Session["errormsg"] = "Try again later or Please check customer phone or Distributor User name or password (DT)";
                return null;
            }
        }

        public static DisCustomerDetails GetCustomerDetailsByPhone(string customerPhone, string govNumber, string token, CookieContainer cookieJar)
        {
            try
            {
                //javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=customerDetails&renewcustomer%3AsearchOption=0&renewcustomer%3AareaCodeList=48&renewcustomer%3Adslnum=3549041&javax.faces.ViewState=2922775790908731162%3A752776558526767326
                string poststring =
                      string.Format(
                          "javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=customerDetails&renewcustomer%3AsearchOption=0&renewcustomer%3AareaCodeList={0}&renewcustomer%3Adslnum={1}&javax.faces.ViewState={2}",
                          govNumber, customerPhone, token);
                HttpWebRequest httpRequest =
                       (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");

                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequest.CookieContainer = cookieJar;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequest.Proxy = proxy;
                }
                else
                {
                    httpRequest.Proxy = null;
                }

                byte[] bytedata = Encoding.UTF8.GetBytes(poststring);
                httpRequest.ContentLength = bytedata.Length;

                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytedata, 0, bytedata.Length);
                requestStream.Close();

                string text = "";
                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                        text = sr.ReadToEnd();
                    }
                }
                if (IsSearchResaultPage(text))
                {
                    HtmlDocument details = new HtmlDocument();
                    details.OptionAutoCloseOnEnd = true;
                    details.OptionFixNestedTags = true;
                    details.LoadHtml(text);
                    var customerNumber = string.Empty;
                    var customerName = string.Empty;
                    var disCustomerPhone = string.Empty;
                    var speed = string.Empty;
                    var startDate = string.Empty;
                    var endDate = string.Empty;
                    var amount = string.Empty;

                    HtmlNode customerNum = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[2]/p");
                    if (customerNum != null)
                    {
                        if (customerNum.InnerText != null)
                        {
                            customerNumber = customerNum.InnerText.Trim();
                        }
                    }

                    HtmlNode name = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[3]/p");
                    if (name != null)
                    {
                        if (name.InnerText != null)
                        {
                            customerName = name.InnerText.Trim();
                        }
                    }

                    HtmlNode speedNode = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[6]/p");
                    if (speedNode != null)
                    {
                        if (speedNode.InnerText != null)
                        {
                            speed = speedNode.InnerText.Trim();
                        }
                    }
                    HtmlNode startDateNode = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[9]/p/label");
                    if (startDateNode != null)
                    {
                        if (startDateNode.InnerText != null)
                        {
                            startDate = startDateNode.InnerText.Trim();
                        }
                    }

                    HtmlNode endDateNode = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[10]/p/label");
                    if (endDateNode != null)
                    {
                        if (endDateNode.InnerText != null)
                        {
                            endDate = endDateNode.InnerText.Trim();
                        }
                    }
                    HtmlNode amountNode = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[12]/label[2]");
                    if (amountNode != null)
                    {
                        if (amountNode.InnerText != null)
                        {
                            amount = amountNode.InnerText.Replace("LE", "").Trim();
                        }
                    }
                    HtmlNode phoneNode = details.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[4]/p");
                    if (phoneNode != null)
                    {
                        if (phoneNode.InnerText != null)
                        {
                            disCustomerPhone = phoneNode.InnerText.Substring(4).Trim();
                        }
                    }

                    DisCustomerDetails custDetails = new DisCustomerDetails()
                        {

                            CustomerNumber = customerNumber,
                            Name = customerName,
                            Phone = disCustomerPhone,
                            Speed = speed,
                            StartDate = startDate,
                            EndDate = endDate,
                            Amount = amount

                        };

                    return custDetails;
                }
                else
                {
                    ErrorMsg(text);

                    return null;
                }

            }
            catch (WebException wex)
            {
                var pageContent = new StreamReader(wex.Response.GetResponseStream())
                          .ReadToEnd();
                HttpContext.Current.Session["errormsg"] = "Try again later or Please check customer phone or Distributor User name or password (DBP)";
                return null;
            }
        }

        public static string ErrorMsg(string page)
        {
            HtmlDocument doc = new HtmlDocument();
            string errorMsg = string.Empty;
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode element = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:InquiryError']");
            if (element != null)
            {
                if (element.InnerText != null)
                {
                    errorMsg = element.InnerText.Trim();
                    HttpContext.Current.Session["errormsg"] = errorMsg;
                }
            }

            HtmlNode element2 = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:searchType']/li[1]/div/label[2]");
            if (element2 != null)
            {
                if (element2.InnerText != null)
                {
                    errorMsg = element2.InnerText.Trim();
                    if (HttpContext.Current.Session["errormsg"] == null)
                    {
                        HttpContext.Current.Session["errormsg"] = errorMsg;
                    }
                    else
                    {
                        HttpContext.Current.Session["errormsg"] +=" - " +errorMsg;
                    }
                    
                }
            }
            HtmlNode element3 = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:PaymentError']");
            if (element3 != null)
            {
                if (element3.InnerText != null)
                {
                    errorMsg = element3.InnerText.Trim();
                    if (HttpContext.Current.Session["errormsg"] == null)
                    {
                        HttpContext.Current.Session["errormsg"] = errorMsg;
                    }
                    else
                    {
                        HttpContext.Current.Session["errormsg"] += " - " + errorMsg;
                    }

                }
            }

            return errorMsg;
        }



        public static string GetPayToken(CookieContainer cookieJar)
        {
            HttpWebRequest httpRequest =
                      (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            httpRequest.CookieContainer = cookieJar;
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                httpRequest.Proxy = proxy;
            }
            else
            {
                httpRequest.Proxy = null;
            }
            string text = "";
            using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
            {
                using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }

            if (IsSearchForCustomerPage(text))
            {
                HtmlDocument doc = new HtmlDocument();

                doc.OptionAutoCloseOnEnd = true;
                doc.OptionFixNestedTags = true;
                doc.LoadHtml(text);

                string token = string.Empty;
                HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@id='javax.faces.ViewState']");
                if (vStateNode != null)
                {
                    token = vStateNode.GetAttributeValue("value", "");
                }


                return token;
            }
            else
            {
                return null;
            }
        }
        public static int PayDemand(string customerPhone, string govNumber, CookieContainer cookieJar, string token)
        {
            try
            {
                // SEARCH STEP
                string postSearch =
                           string.Format(
                               "javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=customerDetails&renewcustomer%3AsearchOption=0&renewcustomer%3AareaCodeList={0}&renewcustomer%3Adslnum={1}&javax.faces.ViewState={2}",
                               govNumber, customerPhone, token);
                HttpWebRequest httpRequestSearch =
                       (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");
                httpRequestSearch.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequestSearch.CookieContainer = cookieJar;
                httpRequestSearch.Method = "POST";
                httpRequestSearch.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequestSearch.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequestSearch.Proxy = proxy;
                }
                else
                {
                    httpRequestSearch.Proxy = null;
                }

                byte[] bytedataSearch = Encoding.UTF8.GetBytes(postSearch);
                httpRequestSearch.ContentLength = bytedataSearch.Length;

                Stream requestStreamSearch = httpRequestSearch.GetRequestStream();
                requestStreamSearch.Write(bytedataSearch, 0, bytedataSearch.Length);
                requestStreamSearch.Close();

                string textSearch = "";
                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequestSearch.GetResponse())
                {
                    using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                        textSearch = sr.ReadToEnd();
                    }
                }
                if (!IsSearchResaultPage(textSearch))
                {
                    HttpContext.Current.Session["errormsg"] = "Try again later or Please check customer phone or Distributor User name or password";
                    return 2;
                }
                //CONFIRMATION STEP
                //javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=confirmationStep&renewcustomer%3ApaymentMethodOption=9&renewcustomer%3AaddVoucherMethod=true&renewcustomer%3Avoucheridtxt=&javax.faces.ViewState=647155676351021906%3A5228043360568418416
                string postConfirmation =
                           string.Format(
                               "javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=confirmationStep&renewcustomer%3ApaymentMethodOption=9&renewcustomer%3AaddVoucherMethod=true&renewcustomer%3Avoucheridtxt=&javax.faces.ViewState={0}",
                                token);
                HttpWebRequest httpRequestConfirmation =
                          (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");

                httpRequestConfirmation.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequestConfirmation.CookieContainer = cookieJar;
                httpRequestConfirmation.Method = "POST";
                httpRequestConfirmation.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequestConfirmation.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequestConfirmation.Proxy = proxy;
                }
                else
                {
                    httpRequestConfirmation.Proxy = null;
                }
                byte[] byteDataConf = Encoding.UTF8.GetBytes(postConfirmation);
                httpRequestConfirmation.ContentLength = byteDataConf.Length;

                Stream requestStreamConf = httpRequestConfirmation.GetRequestStream();
                requestStreamConf.Write(byteDataConf, 0, byteDataConf.Length);
                requestStreamConf.Close();


                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequestConfirmation.GetResponse())
                {
                }
                //PAY STEP
                //javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=printReceipt&javax.faces.ViewState=647155676351021906%3A5228043360568418416
                string postPay =
                          string.Format(
                              "javax.faces.partial.ajax=true&javax.faces.source=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.execute=renewcustomer%3Arenewcustomerwzr&javax.faces.partial.render=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr=renewcustomer%3Arenewcustomerwzr&renewcustomer%3Arenewcustomerwzr_wizardRequest=true&renewcustomer%3Arenewcustomerwzr_stepToGo=printReceipt&javax.faces.ViewState={0}",
                               token);
                HttpWebRequest httpRequest =
                          (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/customers/renewcustomer.ted");

                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpRequest.CookieContainer = cookieJar;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequest.Proxy = proxy;
                }
                else
                {
                    httpRequest.Proxy = null;
                }
                byte[] bytedata = Encoding.UTF8.GetBytes(postPay);
                httpRequest.ContentLength = bytedata.Length;

                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytedata, 0, bytedata.Length);
                requestStream.Close();

                string text = "";
                using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                        text = sr.ReadToEnd();
                    }
                }

                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.OptionFixNestedTags = true;
                doc.LoadHtml(text);
                HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renewcustomerwzr']/a");
                if (vStateNode != null)
                {
                    return 1;
                }
                ErrorMsg(text);
                return 2;

            }
            catch
            {
                HttpContext.Current.Session["errormsg"] = "Try again later, Please check Distributor User name or password (PD)";
                return 2;
            }

        }
        public static string GetTransActionNumber(CookieContainer cookieJar)
        {
            HttpWebRequest httpRequest =
                      (HttpWebRequest)WebRequest.Create("http://distributors.tedata.net/receipt/receipttemplate.ted");
            httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            httpRequest.CookieContainer = cookieJar;
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequest.UnsafeAuthenticatedConnectionSharing = true;
                httpRequest.Proxy = proxy;
            }
            else
            {
                httpRequest.Proxy = null;
            }
            string text = "";
            using (HttpWebResponse httpWebResponse1 = (HttpWebResponse)httpRequest.GetResponse())
            {
                using (Stream responseStream1 = httpWebResponse1.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }

            if (IsPrintPage(text))
            {
                HtmlDocument doc = new HtmlDocument();

                doc.OptionAutoCloseOnEnd = true;
                doc.OptionFixNestedTags = true;
                doc.LoadHtml(text);

                string token = string.Empty;
                // /html/body/div/div[3]/p
                HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@class='trans_no']/p");
                if (vStateNode != null)
                {
                    token = vStateNode.InnerText.Trim();
                }


                return token;
            }
            else
            {
                return null;
            }
        }
        public static string GetCustomerAmount(string page)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            string token = string.Empty;
            HtmlNode vStateNode = doc.DocumentNode.SelectSingleNode("//*[@id='javax.faces.ViewState']");
            if (vStateNode != null)
            {
                token = vStateNode.GetAttributeValue("value", "");
            }

            return token;
        }
        public static bool IsFirstLoginPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@id='signin:username']");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }
        public static bool CheckIfUserLogedIn(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@class='logout']");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }
        public static bool IsCustomerDetailsPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:dslnum']");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }
        public static bool IsSearchForCustomerPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:searchOption:0']");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }
        public static bool IsSearchResaultPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@id='renewcustomer:renew_content']/fieldset/ul[1]/li[2]/p");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }
        public static bool IsPrintPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@class='trans_no']");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }

        public static bool PopulateProxy()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();
                if (auth == null)
                {
                    return false;
                }

                if (auth.DisUseProxy ?? false)
                {
                    if (!string.IsNullOrEmpty(auth.DisProxy) && auth.DisPort.HasValue)
                    {
                        proxy = new WebProxy(auth.DisProxy, auth.DisPort ?? 80);
                        proxy.BypassProxyOnLocal = false;
                        useProxy = true;
                        return true;
                    }
                }
            }
            return false;

        }
    }
    public class DisCustomerDetails
    {
        public string CustomerNumber { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Speed { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Amount { get; set; }

    }
}