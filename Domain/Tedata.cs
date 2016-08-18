using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Db;
using HtmlAgilityPack;
using NewIspNL.Helpers;
using NewIspNL.Pages;

namespace NewIspNL.Domain
{
    public class Tedata
    {
     //                   -*- By Ali Sallam -*-

       public static int SendTedataSuspendRequest(string CustomerNumber)
        {
           try
           {
           
               var username = "";
               var pass = "";
               using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
               {
                   var auth = context.authentications.FirstOrDefault();
                   if (auth == null || auth.UserName == null || auth.Password== null)
                   {
                       return 1;
                   }
                   else
                   {
                       username = auth.UserName;
                       pass = auth.Password;
                   }
               }
               WebProxy proxy = new WebProxy();
               bool useProxy = false;
               using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
               {
                   var auth = context.authentications.FirstOrDefault();
                   if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                   {
                       if (auth.UseProxy ?? false)
                       {
                           proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                           proxy.BypassProxyOnLocal = false;
                           useProxy = true;
                       }
                   }

               }
               //string poststring = "txtUsername=firstlineuser&txtPassword=firstlinepass%21%21111020300";
               string poststring = string.Format("txtUsername={0}&txtPassword={1}", username, pass);

            HttpWebRequest httpRequest =
            (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/login.jsp");

            CookieContainer cookieJar = new CookieContainer();
            httpRequest.CookieContainer = cookieJar;



            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
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
            //===
            HttpWebResponse httpWebResponse1 =
           (HttpWebResponse)httpRequest.GetResponse();
            Stream responseStream1 = httpWebResponse1.GetResponseStream();

            //-----------------------------------------
            // get package id value from checkbox in page searchprocess.jsp

            //search by customer number

            //string posts = "slcAreaCode=-1&txtADSLPhoneNumber=2578520&fieldValue=2578520&field=DSL_PHONE";

            string posts = string.Format("fieldValue={0}&field=CUSTOMER_ID", CustomerNumber);
            HttpWebRequest httpRequestadd =
           (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/search/searchprocess.jsp");
            httpRequestadd.CookieContainer = cookieJar;
            httpRequestadd.Method = "POST";
            httpRequestadd.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestadd.Proxy = proxy;
            }
            else
            {
                httpRequestadd.Proxy = null;
            }
            byte[] bytedataadd = Encoding.UTF8.GetBytes(posts);
            httpRequestadd.ContentLength = bytedataadd.Length;

            Stream requestStreamadd = httpRequestadd.GetRequestStream();
            requestStreamadd.Write(bytedataadd, 0, bytedataadd.Length);
            requestStreamadd.Close();

            //-----------------
            HttpWebResponse httpWebResponse =
            (HttpWebResponse)httpRequestadd.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.UTF8);
            string text = sr.ReadToEnd();

            //using (StreamReader reader =
            //new StreamReader(responseStream, System.Text.Encoding.UTF8))
            //{
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        sb.Append(line);
            //    }
            //}

            //return text;
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(text);

            StringBuilder sb3 = new StringBuilder();
            var customerName = "";
            var checkBoxValue = string.Empty;
            if (doc.DocumentNode != null)
            {


                HtmlNode inputChk = doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

                if (inputChk != null)
                {
                    if (inputChk.Attributes["value"] != null)
                    {
                        checkBoxValue = inputChk.Attributes["value"].Value;
                    }
                }
                else
                {
                    HtmlNode inputCh = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");

                    if (inputCh != null)
                    {
                        if (inputCh.Attributes["value"] != null)
                        {
                            checkBoxValue = inputCh.Attributes["value"].Value;
                        }
                    }
                }


                HtmlNode customerNameNode = doc.DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr/td[2]/table/tbody/tr[2]/td/table/tbody/tr[1]/td/table/tbody/tr[3]/td/table/tbody/tr/td[2]/span");

                if (customerNameNode != null)
                {
                    if (customerNameNode.InnerText != null)
                    {
                        customerName = customerNameNode.InnerText;
                    }
                }
            }

            //-----------------------------
            // send suspend request

            string postSuspend = string.Format("txtCustomerID={0}&txtConsumerName={1}&taComment={2}&selectedPackageIDs={3}", CustomerNumber, customerName, "", checkBoxValue);
            HttpWebRequest httpRequestSuspend =
           (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/actions/suspendprocess.jsp");
            httpRequestSuspend.CookieContainer = cookieJar;
            httpRequestSuspend.Method = "POST";
            httpRequestSuspend.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestSuspend.Proxy = proxy;
            }
            else
            {
                httpRequestSuspend.Proxy = null;
            }
            byte[] bytedataSuspend = Encoding.UTF8.GetBytes(postSuspend);
            httpRequestSuspend.ContentLength = bytedataSuspend.Length;

            Stream requestStreamSuspend = httpRequestSuspend.GetRequestStream();
            requestStreamSuspend.Write(bytedataSuspend, 0, bytedataSuspend.Length);
            requestStreamSuspend.Close();

            HttpWebResponse httpWebResponseSuspend =
           (HttpWebResponse)httpRequestSuspend.GetResponse();
            //Stream responseStreamSuspend = httpWebResponse.GetResponseStream();
            //---------------------------



            return 0;

           }
           catch
           {

               return 2;
           }
        }

       public static int SendTedataSuspendRequest(string CustomerNumber, CookieContainer cookieJar, string pageText)
        {
           try
           {
               if (string.IsNullOrEmpty(CustomerNumber) || cookieJar == null || string.IsNullOrEmpty(pageText))
               {
                   return 2;
               }
           
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(pageText);

            var customerName = "";
            var checkBoxValue = string.Empty;
            if (doc.DocumentNode != null)
            {

                HtmlNode inputChk = doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

                if (inputChk != null)
                {
                    if (inputChk.Attributes["value"] != null)
                    {
                        checkBoxValue = inputChk.Attributes["value"].Value;
                    }
                }
                else
                {
                    HtmlNode inputCh = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");

                    if (inputCh != null)
                    {
                        if (inputCh.Attributes["value"] != null)
                        {
                            checkBoxValue = inputCh.Attributes["value"].Value;
                        }
                    }
                }

                HtmlNode customerNameNode =
                    doc.DocumentNode.SelectSingleNode("//table[@class='consumerInfoTable']/tr[3]/td[1]/table/tr/td[2]/span");
                   // "/html/body/table[2]/tbody/tr/td[2]/table/tbody/tr[2]/td/table/tbody/tr[1]/td/table/tbody/tr[3]/td/table/tbody/tr/td[2]/span");

                if (customerNameNode != null)
                {
                    if (customerNameNode.InnerText != null)
                    {
                        customerName = customerNameNode.InnerText;
                    }
                }
            }

            //-----------------------------
            // send suspend request
            WebProxy proxy = new WebProxy();
            bool useProxy = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();
                if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                {
                    if (auth.UseProxy ?? false)
                    {
                        proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                        proxy.BypassProxyOnLocal = false;
                        useProxy = true;
                    }
                }

            }
            string postSuspend = string.Format("txtCustomerID={0}&txtConsumerName={1}&taComment={2}&selectedPackageIDs={3}", CustomerNumber, customerName, "", checkBoxValue);
            HttpWebRequest httpRequestSuspend =
           (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/actions/suspendprocess.jsp");
            httpRequestSuspend.CookieContainer = cookieJar;
            httpRequestSuspend.Method = "POST";
            httpRequestSuspend.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestSuspend.UnsafeAuthenticatedConnectionSharing = true;
                httpRequestSuspend.Proxy = proxy;
            }
            else
            {
                httpRequestSuspend.Proxy = null;
            }
            byte[] bytedataSuspend = Encoding.UTF8.GetBytes(postSuspend);
            httpRequestSuspend.ContentLength = bytedataSuspend.Length;

            Stream requestStreamSuspend = httpRequestSuspend.GetRequestStream();
            requestStreamSuspend.Write(bytedataSuspend, 0, bytedataSuspend.Length);
            requestStreamSuspend.Close();


            string text = string.Empty;
            string checktext = string.Empty;
            using (HttpWebResponse httpWebResponseSuspend = (HttpWebResponse)httpRequestSuspend.GetResponse())
            {
                using (Stream responseStream1 = httpWebResponseSuspend.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }

            HtmlDocument docUsage = new HtmlDocument();
            docUsage.OptionAutoCloseOnEnd = true;
            docUsage.OptionFixNestedTags = true;
            docUsage.LoadHtml(text);

            HtmlNode infox = docUsage.DocumentNode.SelectSingleNode("//table[@class='resumeResultTable']/tbody/tr[2]/td/table/tbody/tr[1]/td/span[@class='errorLabel']");

            if (infox != null)
            {
                return 2;
            }

               return 0;

           }
           catch
           {
               return 2;

           }
        }

        public static int SendTedataUnSuspendRequest(string CustomerNumber)
        {
            
            try
            {
                //login request
                var username = "";
                var pass = "";
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var auth = context.authentications.FirstOrDefault();
                    if (auth == null || auth.UserName == null || auth.Password == null)
                    {
                        return 1;
                    }
                    else
                    {
                        username = auth.UserName;
                        pass = auth.Password;
                    }
                }
                WebProxy proxy = new WebProxy();
                bool useProxy = false;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var auth = context.authentications.FirstOrDefault();
                    if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                    {
                        if (auth.UseProxy ?? false)
                        {
                            proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                            proxy.BypassProxyOnLocal = false;
                            useProxy = true;
                        }
                    }

                }
                string poststring = string.Format("txtUsername={0}&txtPassword={1}", username, pass);

                HttpWebRequest httpRequest =
                (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/login.jsp");

                CookieContainer cookieJar = new CookieContainer();
                httpRequest.CookieContainer = cookieJar;

                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
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
                HttpWebResponse httpWebResponse1 =
               (HttpWebResponse)httpRequest.GetResponse();
                Stream responseStream1 = httpWebResponse1.GetResponseStream();
                
              //-------------------
            // get package id value from checkbox in page searchprocess.jsp
            //search by customer number

            string posts = string.Format("fieldValue={0}&field=CUSTOMER_ID", CustomerNumber);
            HttpWebRequest httpRequestadd =
           (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/search/searchprocess.jsp");
            httpRequestadd.CookieContainer = cookieJar;
            httpRequestadd.Method = "POST";
            httpRequestadd.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestadd.Proxy = proxy;
            }
            else
            {
                httpRequestadd.Proxy = null;
            }
            byte[] bytedataadd = Encoding.UTF8.GetBytes(posts);
            httpRequestadd.ContentLength = bytedataadd.Length;

            Stream requestStreamadd = httpRequestadd.GetRequestStream();
            requestStreamadd.Write(bytedataadd, 0, bytedataadd.Length);
            requestStreamadd.Close();

            //-----------------
            HttpWebResponse httpWebResponse =
            (HttpWebResponse)httpRequestadd.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();

           
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            string text = sr.ReadToEnd();

          
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(text);

            var customerName = "";
            var checkBoxValue = string.Empty;
            if (doc.DocumentNode != null)
            {
               

                HtmlNode inputChk = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");

                if (inputChk != null)
                {
                    if (inputChk.Attributes["value"] != null)
                    {
                        checkBoxValue = inputChk.Attributes["value"].Value;
                    }
                }
                else
                {
                    HtmlNode inputCh = doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

                    if (inputCh != null)
                    {
                        if (inputCh.Attributes["value"] != null)
                        {
                            checkBoxValue = inputCh.Attributes["value"].Value;
                        }
                    }
                }

                HtmlNode customerNameNode = doc.DocumentNode.SelectSingleNode("//table[@class='consumerInfoTable']/tr[3]/td[1]/table/tr/td[2]/span");

                if (customerNameNode != null)
                {
                    if (customerNameNode.InnerText != null)
                    {
                        customerName = customerNameNode.InnerText;
                    }
                }
            }
            //-----------------------------
            // send Un Suspend Request
           
            string postSuspend = string.Format("txtCustomerID={0}&txtConsumerName={1}&taComment={2}&selectedPackageIDs={3}&btnRequest=Request", CustomerNumber ?? "", customerName ?? "", "", checkBoxValue ?? "");
            HttpWebRequest httpRequestSuspend =
           (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/actions/resumeprocess.jsp");
            httpRequestSuspend.CookieContainer = cookieJar;
            httpRequestSuspend.Method = "POST";
            httpRequestSuspend.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestSuspend.Proxy = proxy;
            }
            else
            {
                httpRequestSuspend.Proxy = null;
            }
            byte[] bytedataSuspend = Encoding.UTF8.GetBytes(postSuspend);
            httpRequestSuspend.ContentLength = bytedataSuspend.Length;

            Stream requestStreamSuspend = httpRequestSuspend.GetRequestStream();
            requestStreamSuspend.Write(bytedataSuspend, 0, bytedataSuspend.Length);
            requestStreamSuspend.Close();
            HttpWebResponse httpWebResponseSuspend =
           (HttpWebResponse)httpRequestSuspend.GetResponse();
            //---------------------------
            return 0;
            }
            catch
            {

                return 2;
            }
        }


        //**********************
       public static int SendTedataUnSuspendRequest(string customerNumber, CookieContainer cookieJar, string pageText)
        {
           try
           {
               if (string.IsNullOrEmpty(customerNumber) || cookieJar == null || string.IsNullOrEmpty(pageText))
               {
                   return 2;
               }
               HtmlDocument doc = new HtmlDocument();
               doc.OptionAutoCloseOnEnd = true;
               doc.OptionFixNestedTags = true;
               doc.LoadHtml(pageText);
               var customerName = "";
               var checkBoxValue = string.Empty;
               if (doc.DocumentNode != null)
               {
                   HtmlNode inputChk = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");

                   if (inputChk != null)
                   {
                       if (inputChk.Attributes["value"] != null)
                       {
                           checkBoxValue = inputChk.Attributes["value"].Value;
                       }
                   }
                   else
                   {
                       HtmlNode inputCh = doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

                       if (inputCh != null)
                       {
                           if (inputCh.Attributes["value"] != null)
                           {
                               checkBoxValue = inputCh.Attributes["value"].Value;
                           }
                       }
                   }
                   HtmlNode customerNameNode =
                       doc.DocumentNode.SelectSingleNode(
                           "//table[@class='consumerInfoTable']/tr[3]/td[1]/table/tr/td[2]/span");
                   if (customerNameNode != null)
                   {
                       if (customerNameNode.InnerText != null)
                       {
                           customerName = customerNameNode.InnerText;
                       }
                   }
               }
               //-----------------------------
               // send Un Suspend Request
               WebProxy proxy = new WebProxy();
               bool useProxy = false;
               using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
               {
                   var auth = context.authentications.FirstOrDefault();
                   if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                   {
                       if (auth.UseProxy ?? false)
                       {
                           proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                           proxy.BypassProxyOnLocal = false;
                           useProxy = true;
                       }
                   }

               }
               string postSuspend =
                   string.Format(
                       "txtCustomerID={0}&txtConsumerName={1}&taComment={2}&selectedPackageIDs={3}&btnRequest=Request",
                       customerNumber ?? "", customerName ?? "", "", checkBoxValue ?? "");
               HttpWebRequest httpRequestunSuspend =
                   (HttpWebRequest) WebRequest.Create("https://partners.tedata.net/actions/resumeprocess.jsp");
               httpRequestunSuspend.CookieContainer = cookieJar;
               httpRequestunSuspend.Method = "POST";
               httpRequestunSuspend.ContentType = "application/x-www-form-urlencoded";
               if (useProxy)
               {
                   httpRequestunSuspend.UnsafeAuthenticatedConnectionSharing = true;
                   httpRequestunSuspend.Proxy = proxy;
               }
               else
               {
                   httpRequestunSuspend.Proxy = null;
               }
               byte[] bytedataSuspend = Encoding.UTF8.GetBytes(postSuspend);
               httpRequestunSuspend.ContentLength = bytedataSuspend.Length;

               Stream requestStreamSuspend = httpRequestunSuspend.GetRequestStream();
               requestStreamSuspend.Write(bytedataSuspend, 0, bytedataSuspend.Length);
               requestStreamSuspend.Close();

             

            string text = string.Empty;
            using (HttpWebResponse httpWebResponseunSuspend = (HttpWebResponse)httpRequestunSuspend.GetResponse())
            {
                using (Stream responseStream1 = httpWebResponseunSuspend.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }

            HtmlDocument docUsage = new HtmlDocument();
            docUsage.OptionAutoCloseOnEnd = true;
            docUsage.OptionFixNestedTags = true;
            docUsage.LoadHtml(text);

            HtmlNode infox = docUsage.DocumentNode.SelectSingleNode("//table[@class='resumeResultTable']/tbody/tr[2]/td/table/tbody/tr[1]/td/span[@class='errorLabel']");

            if (infox != null)
            {
                return 2;
            }
           
              return 0;
           }
           catch
           {
               return 2;
           }
        }

        public static Tedatalist GetCustomerDetails(string customerNumber, string page, CookieContainer cookiecon)
        {
            //CookieContainer cookiecon=new CookieContainer();
            //cookiecon = Login(customerNumber);
            //if (cookiecon==null)
            //{
            //    return null;
            //}
            //var page = GetSearchPage(customerNumber, cookiecon);
           
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);
           
            var LineSpeedValue = string.Empty;
            var LimitedOrUnlimitedValue = string.Empty;
            var LineStatusValue = string.Empty;
            var RouterStatusValue = string.Empty;
            var OperationalStatusValue = string.Empty;
            var CustomerPasswordValue = string.Empty;
            var StartDateValue = string.Empty;
            var EndDateValue = string.Empty;
            var NetAmountValue = string.Empty;

            if (doc.DocumentNode == null)
            {
                return null;
            }
           //--------------------------------------------------------
             HtmlNode suspendPage = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");
                HtmlNode lineSpeed = doc.DocumentNode.SelectSingleNode("//table[@class='consumerInfoTable']/tr[15]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");
                if (lineSpeed != null)
                {
                    if (lineSpeed.InnerText != null)
                    {
                        LineSpeedValue = lineSpeed.InnerText;
                    }
                }
                HtmlNode limitedOrUnlimited =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[17]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");
                if (limitedOrUnlimited != null)
                {
                    if (limitedOrUnlimited.InnerText != null)
                    {
                        LimitedOrUnlimitedValue = limitedOrUnlimited.InnerText;
                    }
                }
                HtmlNode lineStatus =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[18]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");
                if (lineStatus != null)
                {
                    if (lineStatus.InnerText != null)
                    {
                        LineStatusValue = lineStatus.InnerText;
                    }
                }
                HtmlNode routerStatus =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[19]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");
                if (routerStatus != null)
                {
                    if (routerStatus.InnerText != null)
                    {
                        RouterStatusValue = routerStatus.InnerText;
                    }
                }
                HtmlNode operationalStatus =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[20]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");

                if (operationalStatus != null)
                {
                    if (operationalStatus.InnerText != null)
                    {
                        OperationalStatusValue = operationalStatus.InnerText;
                    }
                }

                HtmlNode customerPassword =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[22]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");

                if (customerPassword != null)
                {
                    if (customerPassword.InnerText != null)
                    {
                        CustomerPasswordValue = customerPassword.InnerText;
                    }
                }

                if (suspendPage != null)
                {
                HtmlNode startDate =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[27]/td/table/tr/td/table/tr[2]/td[3]/span[@class='consumerInfoValue']");

                if (startDate != null)
                {
                    if (startDate.InnerText != null)
                    {
                        StartDateValue = startDate.InnerText.Replace("&nbsp;","");
                    }
                }
                HtmlNode endDate =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[27]/td/table/tr/td/table/tr[2]/td[4]/span[@class='consumerInfoValue']");

                if (endDate != null)
                {
                    if (endDate.InnerText != null)
                    {
                        EndDateValue = endDate.InnerText.Replace("&nbsp;", ""); 
                    }
                }
                HtmlNode netAmount =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@class='consumerInfoTable']/tr[27]/td/table/tr/td/table/tr[2]/td[6]/span[@class='consumerInfoValue']");

                if (netAmount != null)
                {
                    if (netAmount.InnerText != null)
                    {
                        NetAmountValue = netAmount.InnerText.Replace("&nbsp;", "");
                    }
                }
            }
                else
                {

                    HtmlNode startDate =
                        doc.DocumentNode.SelectSingleNode("//*[@name='frmPackages']");
                    HtmlNode startDate2 = startDate.SelectSingleNode("//table[@class='consumerActivePackagesContentTable']/tr[2]/td[3]/span['consumerInfoValue']");
                    if (startDate2 != null)
                    {
                        if (startDate2.InnerText != null)
                        {
                            StartDateValue = startDate2.InnerText.Replace("&nbsp;", "");
                        }
                    }
                    HtmlNode endDate =
                        doc.DocumentNode.SelectSingleNode("//*[@name='frmPackages']");
                    HtmlNode endDate2 =
                        endDate.SelectSingleNode(
                            "//table[@class='consumerActivePackagesContentTable']/tr[2]/td[4]/span['consumerInfoValue']");
                    if (endDate2 != null)
                    {
                        if (endDate2.InnerText != null)
                        {
                            EndDateValue = endDate2.InnerText.Replace("&nbsp;", "");
                        }
                    }
                    HtmlNode netAmount =
                        startDate.SelectSingleNode(
                            "//table[@class='consumerActivePackagesContentTable']/tr[2]/td[6]/span['consumerInfoValue']");
                    if (netAmount != null)
                    {
                        if (netAmount.InnerText != null)
                        {
                            NetAmountValue = netAmount.InnerText.Replace("&nbsp;", "");
                        }
                    }
                }

            Tedatalist Tlist=new Tedatalist()
            {
                LineSpeed = LineSpeedValue,LimitedOrUnlimited = LimitedOrUnlimitedValue,
                LineStatus = LineStatusValue,RouterStatus = RouterStatusValue,
                CustomerPassword = CustomerPasswordValue,OperationalStatus = OperationalStatusValue,
                StartDate = StartDateValue,EndDate = EndDateValue,NetAmount = NetAmountValue
            };
            if (Tlist==null)
            {
                return null;
            }
            return Tlist;
        }

        public static CookieContainer Login()
        {
            //login request
            var username = "";
            var pass = "";
            WebProxy proxy = new WebProxy();
            bool useProxy = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();
                if (auth == null)
                {
                    return null;
                }

                if(auth.UserName.Length>0 && auth.Password.Length>0)
                {

                    username = auth.UserName;
                    pass = QueryStringSecurity.Decrypt(auth.Password);
                }
                else
                {
                    return null;
                }
                if (auth.Proxy.Length > 0 && auth.Port.HasValue)
                {
                    if (auth.UseProxy ?? false)
                    {
                        proxy = new WebProxy(auth.Proxy,auth.Port ?? 80);
                        proxy.BypassProxyOnLocal = false;
                        useProxy = true;
                    }
                }
            }
            try
            {
                //string poststring = "txtUsername=firstlineuser&txtPassword=firstlinepass!!111020300";
                string poststring = string.Format("txtUsername={0}&txtPassword={1}", username, pass);

                HttpWebRequest httpRequest =
                    (HttpWebRequest) WebRequest.Create("https://partners.tedata.net/login.jsp");

                CookieContainer cookieJar = new CookieContainer();
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
               

                //HttpWebResponse httpWebResponse1 =
                //    (HttpWebResponse) httpRequest.GetResponse();
                //Stream responseStream1 = httpWebResponse1.GetResponseStream();
                //StreamReader sr = new StreamReader(responseStream1, Encoding.UTF8);
                //string text = sr.ReadToEnd();

                if (CheckIfUserLogedIn(text))
                {
                    return cookieJar;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        //************************
        public static string GetSearchPage(string customerNumber, CookieContainer cookieJar)
        {
            // get package id value from checkbox in page searchprocess.jsp
            //search by customer number
             WebProxy proxy = new WebProxy();
            bool useProxy = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();

                if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                {
                    if (auth.UseProxy ?? false)
                    {
                        proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                        proxy.BypassProxyOnLocal = false;
                        useProxy = true;
                    }
                }
            }
            try
            {
                string posts = string.Format("fieldValue={0}&field=CUSTOMER_ID", customerNumber);
                HttpWebRequest httpRequestadd =
                    (HttpWebRequest) WebRequest.Create("https://partners.tedata.net/search/searchprocess.jsp");
                httpRequestadd.CookieContainer = cookieJar;
                httpRequestadd.Method = "POST";
                httpRequestadd.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequestadd.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequestadd.Proxy = proxy;
                }
                else
                {
                    httpRequestadd.Proxy = null;
                }
                byte[] bytedataadd = Encoding.UTF8.GetBytes(posts);
                httpRequestadd.ContentLength = bytedataadd.Length;

                Stream requestStreamadd = httpRequestadd.GetRequestStream();
                requestStreamadd.Write(bytedataadd, 0, bytedataadd.Length);
                requestStreamadd.Close();

                //-----------------
                string text = "";
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequestadd.GetResponse())
                {
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                        text = sr.ReadToEnd();
                    }
                }
               

                //HttpWebResponse httpWebResponse =
                //    (HttpWebResponse) httpRequestadd.GetResponse();
                //Stream responseStream = httpWebResponse.GetResponseStream();
                //StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                //string text = sr.ReadToEnd();


                return text;
            }
            catch
            {

                return null;
            }
        }


        //*****************************
        public static Usagelist GetCustomerUsage(string customerNumber, string page, CookieContainer cookieJar)
        {
            try
            {
            WebProxy proxy = new WebProxy();
            bool useProxy = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();

                if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                {
                   if (auth.UseProxy ?? false)
                   {
                       proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                       proxy.BypassProxyOnLocal = false;
                       useProxy = true;
                   }
                }
            }
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            var ResellerCustomerIDValue = string.Empty;
            HtmlNode ResellerCustomerID =
                   doc.DocumentNode.SelectSingleNode(
                //table[@class='consumerInfoTable']/tr[17]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");
                       "//table[@class='consumerInfoTable']/script/text()");

            if (ResellerCustomerID != null)
            {
                if (ResellerCustomerID.InnerText != null)
                {
                    var s = ResellerCustomerID.InnerText.Substring(0, ResellerCustomerID.InnerText.IndexOf("&"));
                    var Findex = s.Length;
                    var Sindex = s.IndexOf("=");
                    var Lindex = Findex - Sindex;
                    ResellerCustomerIDValue = (s.Substring(s.IndexOf("="), Lindex)).Replace("=","");
                }
            }
            //string posts = string.Format("?resellerCustomerID=593616&CustomerID=3373156", customerNumber);
            HttpWebRequest httpRequestadd =
              (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/search/CustomerUsage.jsp?resellerCustomerID=" + ResellerCustomerIDValue + "&CustomerID=" + customerNumber + "");

            httpRequestadd.CookieContainer = cookieJar;
            httpRequestadd.Method = "POST";
            httpRequestadd.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestadd.UnsafeAuthenticatedConnectionSharing = true;
                httpRequestadd.Proxy = proxy;
            }
            else
            {
                httpRequestadd.Proxy = null;
            }
            
            //-----------------
            string text = "";
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequestadd.GetResponse())
            {
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }
               
            //HttpWebResponse httpWebResponse =
            //(HttpWebResponse)httpRequestadd.GetResponse();

            //Stream responseStream = httpWebResponse.GetResponseStream();
            //StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //string text = sr.ReadToEnd();
           
            var mx = GetMaxAttainableSpeed(customerNumber, doc, cookieJar);

            HtmlDocument docUsage = new HtmlDocument();
            docUsage.OptionAutoCloseOnEnd = true;
            docUsage.OptionFixNestedTags = true;
            docUsage.LoadHtml(text);
            var cpeedTypeValue = string.Empty;
            var totaldownValue = string.Empty;
            var totalUpValue = string.Empty;
            var totalUsageValue = string.Empty;
            var maxAttainableSpeed = string.Empty;

            HtmlNode speedType = docUsage.DocumentNode.SelectSingleNode("//table[1]/tr[1]/td[2]/span[@class='consumerInfoValue']");

            if (speedType != null)
            {
                if (speedType.InnerText != null)
                {
                    cpeedTypeValue = speedType.InnerText;
                }
            }
            HtmlNode totaldown = docUsage.DocumentNode.SelectSingleNode("//table/tr[2]/td[2]/span[@class='consumerInfoValue']/table/tr[1]");

            if (totaldown != null)
            {
                if (totaldown.InnerText != null)
                {
                    totaldownValue = totaldown.InnerText.Replace("Total:", "");
                }
            }
            HtmlNode totalUp = docUsage.DocumentNode.SelectSingleNode("//table/tr[2]/td[2]/span[@class='consumerInfoValue']/table/tr[2]");

            if (totalUp != null)
            {
                if (totalUp.InnerText != null)
                {
                    totalUpValue = totalUp.InnerText.Replace("Total:","");
                }
            }

            HtmlNode totalUsage = docUsage.DocumentNode.SelectSingleNode("//table/tr[2]/td[2]/span[@class='consumerInfoValue']/table/tr[3]");

            if (totalUsage != null)
            {
                if (totalUsage.InnerText != null)
                {
                    totalUsageValue = totalUsage.InnerText.Replace("Total:","");
                }
            }

            if (Math.Abs(Convert.ToDouble(mx)) < 1)
            {
                maxAttainableSpeed = "N/A (Router must be up)";
            }
            else if (Math.Abs(Convert.ToDouble(mx)) > 0)
            {
                maxAttainableSpeed = mx;
            }

            Usagelist uList = new Usagelist()
            {
                MaxAttainableSpeed = maxAttainableSpeed,
                SpeedType = cpeedTypeValue,
                TotalDownload = totaldownValue,
                TotalUpload = totalUpValue,
                TotalUsage = totalUsageValue
            };
            return uList;
            }
            catch
            {
                return null;
            }

        }

        public static string GetMaxAttainableSpeed(string customerNumber, HtmlDocument doc, CookieContainer cookieJar)
        {
            try
            {
            // get customer Max attainable speed ----------------------------------
            WebProxy proxy = new WebProxy();
            bool useProxy = false;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var auth = context.authentications.FirstOrDefault();

                if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                {
                    if (auth.UseProxy ?? false)
                    {
                        proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                        proxy.BypassProxyOnLocal = false;
                        useProxy = true;
                    }
                }
            }
            string maxPost = string.Format("customerno={0}", customerNumber);
            HttpWebRequest httpRequestMax =
                (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/search/ajax.jsp");
            httpRequestMax.CookieContainer = cookieJar;
            httpRequestMax.Method = "POST";
            httpRequestMax.ContentType = "application/x-www-form-urlencoded";
            if (useProxy)
            {
                httpRequestMax.UnsafeAuthenticatedConnectionSharing = true;
                httpRequestMax.Proxy = proxy;
            }
            else
            {
                httpRequestMax.Proxy = null;
            }
            byte[] bytedatamax = Encoding.UTF8.GetBytes(maxPost);
            httpRequestMax.ContentLength = bytedatamax.Length;

            Stream requestStreamMax = httpRequestMax.GetRequestStream();
            requestStreamMax.Write(bytedatamax, 0, bytedatamax.Length);
            requestStreamMax.Close();

            string text = "";
            using (HttpWebResponse httpWebResponseMax = (HttpWebResponse)httpRequestMax.GetResponse())
            {
                using (Stream responseStreammax = httpWebResponseMax.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStreammax, Encoding.UTF8);
                    text = sr.ReadToEnd();
                }
            }
               

            //HttpWebResponse httpWebResponseMax =
            //(HttpWebResponse)httpRequestMax.GetResponse();

            //Stream responseStreammax = httpWebResponseMax.GetResponseStream();
            //StreamReader sr = new StreamReader(responseStreammax, Encoding.UTF8);
            //string text = sr.ReadToEnd();
            
            return (text.Replace("TRUE{:}", "")).Trim();
            }
            catch
            {

                return null;
            }
        }

        public static string CheckCustomerStatus(string page)
        {
            var LineStatusValue = string.Empty;

            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode suspendPage = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");
            if (suspendPage != null)
            {
                return "disable";
            }
            return "enable";
        }
        public static string GetCustomerStatus(string page)
        {
            var LineStatusValue = string.Empty;

            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);
            HtmlNode lineStatus =
                   doc.DocumentNode.SelectSingleNode(
                       "//table[@class='consumerInfoTable']/tr[18]/td/table/tr/td[@class='consumerInfoValueCell']/span[@class='consumerInfoValue']");
            if (lineStatus != null)
            {
                if (lineStatus.InnerText != null)
                {
                    return lineStatus.InnerText;
                }
            }

            //HtmlNode suspendPage = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");
            //if (suspendPage != null)
            //{
            //    return "disable";
            //}
            return "-";
        }
        public static bool CheckSearchPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode suspendList = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");
            HtmlNode unSuspendList= doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

            if (suspendList != null || unSuspendList != null)
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

            HtmlNode logInElement = doc.DocumentNode.SelectSingleNode("//*[@href='/logout.jsp']");
            if (logInElement != null)
            {
                return true;
            }
            return false;
        }
        public static bool CheckIfCancelPage(string page)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(page);

            HtmlNode suspendList = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");
            HtmlNode unSuspendList = doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

            if (suspendList == null && unSuspendList == null)
            {
                return true;
            }
            return false;
        }

        public static int SendTedataCancelRequest(string customerNumber, CookieContainer cookieJar, string pageText)
        {
            try
            {
                if (string.IsNullOrEmpty(customerNumber) || cookieJar == null || string.IsNullOrEmpty(pageText))
                {
                    return 2;
                }

                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.OptionFixNestedTags = true;
                doc.LoadHtml(pageText);
                var customerName = "";
                var checkBoxValue = string.Empty;
                if (doc.DocumentNode != null)
                {
                    HtmlNode inputChk = doc.DocumentNode.SelectSingleNode("//*[@id='chkSuspendedPackageSelect']");

                    if (inputChk != null)
                    {
                        if (inputChk.Attributes["value"] != null)
                        {
                            checkBoxValue = inputChk.Attributes["value"].Value;
                        }
                    }
                    else
                    {
                        HtmlNode inputCh = doc.DocumentNode.SelectSingleNode("//*[@id='chkActivePackageSelect']");

                        if (inputCh != null)
                        {
                            if (inputCh.Attributes["value"] != null)
                            {
                                checkBoxValue = inputCh.Attributes["value"].Value;
                            }
                        }
                    }
                    HtmlNode customerNameNode =
                        doc.DocumentNode.SelectSingleNode(
                            "//table[@class='consumerInfoTable']/tr[3]/td[1]/table/tr/td[2]/span");
                    if (customerNameNode != null)
                    {
                        if (customerNameNode.InnerText != null)
                        {
                            customerName = customerNameNode.InnerText;
                        }
                    }
                }
                //-----------------------------
                // send Cancel Request
                WebProxy proxy = new WebProxy();
                bool useProxy = false;
                using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var auth = context.authentications.FirstOrDefault();
                    if (auth != null && auth.Proxy.Length > 0 && auth.Port.HasValue)
                    {
                        if (auth.UseProxy ?? false)
                        {
                            proxy = new WebProxy(auth.Proxy, auth.Port ?? 80);
                            proxy.BypassProxyOnLocal = false;
                            useProxy = true;
                        }
                    }

                }
                //txtCustomerID=649202&txtConsumerName=&taComment=test+p&selectedPackageIDs=7416
                string postC =
                    string.Format(
                        "txtCustomerID={0}&txtConsumerName={1}&taComment={2}&selectedPackageIDs={3}",
                        customerNumber ?? "", customerName ?? "", "", checkBoxValue ?? "");
                HttpWebRequest httpRequestCancel =
                    (HttpWebRequest)WebRequest.Create("https://partners.tedata.net/actions/terminateprocess.jsp");
                httpRequestCancel.CookieContainer = cookieJar;
                httpRequestCancel.Method = "POST";
                httpRequestCancel.ContentType = "application/x-www-form-urlencoded";
                if (useProxy)
                {
                    httpRequestCancel.UnsafeAuthenticatedConnectionSharing = true;
                    httpRequestCancel.Proxy = proxy;
                }
                else
                {
                    httpRequestCancel.Proxy = null;
                }
                byte[] bytedataC = Encoding.UTF8.GetBytes(postC);
                httpRequestCancel.ContentLength = bytedataC.Length;

                Stream requestStreamC = httpRequestCancel.GetRequestStream();
                requestStreamC.Write(bytedataC, 0, bytedataC.Length);
                requestStreamC.Close();
                using (HttpWebResponse httpWebResponsec = (HttpWebResponse)httpRequestCancel.GetResponse())
                {
                }
                //HttpWebResponse httpWebResponsec =
                //    (HttpWebResponse)httpRequestCancel.GetResponse();
               
                
                return 0;
            }
            catch 
            {
                return 2;
            }
        }
    }

    public class Tedatalist
    {
        public string LineSpeed { get; set; }
        public string LimitedOrUnlimited { get; set; }
        public string LineStatus { get; set; }
        public string RouterStatus { get; set; }
        public string OperationalStatus { get; set; }
        public string CustomerPassword { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string NetAmount { get; set; }
    }

    public class Usagelist
    {
        public string MaxAttainableSpeed { get; set; }
        public string SpeedType { get; set; }
        public string TotalDownload { get; set; }
        public string TotalUpload{ get; set; }
        public string TotalUsage { get; set; }
       
    }
}