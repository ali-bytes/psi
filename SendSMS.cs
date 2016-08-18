using System;
using System.Linq;
using Db;

  /// <summary>
    /// Summary description for SendSMS
    /// </summary>
    public static class SendSms
    {
          /* http://smsbox.dawlia.net/api/send.aspx?username=XXXXX&password=XXXXX&language=1&sender=TEST&mobile=2012XXXXXXX&message=Hello */


      public static string SendusingDawlia(
          string userName,
          string password,
          int lang,
        string sender,
          string mobile,
          string msg,
           string url = "http://smsbox.dawlia.net/api/send.aspx?"
          )
      {
          var re = string.Format("{0}userName={1}&password={2}&language{3}&sender={4}&mobile={5}&message={6}",
                url,
                userName,
                password,
             lang,
             sender,
                string.Format("2{0}", mobile),
                msg
               
                );
          return re;
      }



      /* http://www.masrawy.com/SMSService/send.aspx?userName=Pioneers&password=P10neer$&mobile=201XXXX&message=XXX&Sender=XXX */
    

        public static string Sendusingmasrawy(
            string userName , 
            string password,
            string mobile,
            string msg,
            string sender,
            string encoding = "utf-8", string url = "http://www.masrawy.com/SMSService/send.aspx?"
            )
        {
            var re = string.Format("{0}userName={1}&password={2}&mobile={3}&message={4}&sender={5}&encoding={6}",
                url,
                userName,
                password,
                // added 20 to fix mobile number missining digits
                string.Format("2{0}", mobile),
                msg,
                sender
                , encoding);
            return re;
        }

        /*http://www.resalty.net/api/sendSMS.php?userid=YouUser&password=YourPassword&to=MobileNumber&msg=Message&sender=SenderName&encoding=utf-8 */
        public static string SendusingResalaty(string userName, string password, string mobile, string msg, string sender, string encoding = "utf-8", string url = "http://www.resalty.net/api/sendSMS.php?")
        {
            var re = string.Format("{0}userid={1}&password={2}&sender={5}&to={3}&msg={4}&encoding={6}",
                url,
                userName,
                password,
                // added 20 to fix mobile number missining digits
                string.Format("2{0}", mobile),
                msg,
                sender
                , encoding);
            return re;
        }
        public static string SendusingSms4Kw(string userName, string password, string mobile, string msg, string sender, string url = "http://sms4kw.com/sendsms.php?")
        {
            /*http://sms4kw.com/sendsms.php?user=demo&password=demo&numbers=966540000000&sender=sms4kw.com&message=test&lang=en */
            var re = string.Format("{0}user={1}&password={2}&numbers={3}&sender={5}&message={4}&lang=ar",
                url,
                userName,
                password,
                // added 20 to fix mobile number missining digits
                string.Format("2{0}", mobile),
                msg,
                sender);
            return re;
        }

        public static string Sendusingapi(string userName, string password, string mobile, string msg, string sender, string url = "http://api.sms.northstartec.com/api/sendsms/plain?")
        {
            //http://api.sms.northstartec.com/api/sendsms/plain?user=xxx&password=xxxx&sender=Friend&SMSText=messagetext&GSM=38598514674
            var data = string.Format("{0}user={1}&password={2}&sender={3}&SMSText={4}&GSM={5}", url, userName, password,
                sender, msg, string.Format("2{0}", mobile));
            return data;
        }
        public static string Sendunifonicapi(string userName, string password, string mobile, string msg, string sender, string url = "http://api.otsdc.com/wrapper/sendSMS.php?", string encoding = "utf-8")
        {
            //http://api.otsdc.com/wrapper/sendSMS.php?userid=yourEmail&password=yourPassword&to=MobileNumber&msg=TextMessage&sender=SenderId&encoding=utf-8
            var data = string.Format("{0}userid={1}&password={2}&to={3}&msg={4}&sender={5}&encoding={6}", url, userName, password
                , string.Format("2{0}", mobile), msg, sender, encoding);
            return data;
        }
        public static string SendUsingSmsMisr(string userName,string password,string sender,string mobile,string message,int lang=1,string url="http://smsmisr.com/api/send.aspx?")
        {
            //http://smsmisr.com/api/send.aspx?username=XXXXX&password=XXXXX&language=1&sender=TEST&mobile=2012XXXXXXX&message=Hello
            var datasms = string.Format("{0}userName={1}&password={2}&language={3}&sender={4}&mobile={6}&message={5}", url,
                userName, password, lang, sender, message, string.Format("2{0}", mobile));
            return datasms;
        }
        public static string SendUsingSmartEgypt(string userName, string password, string sender, string mobile, string message, string url = "http://smssmartegypt.com/api/?")
        {
            //http://smssmartegypt.com/api/?username=xxxx&password=xxxx&sendername=xxxx&message=xxxx&mobiles=xxxx,xxxx
            var datasms = string.Format("{0}userName={1}&password={2}&sendername={3}&message={4}&mobiles={5}", url,
                userName, password, sender, message, string.Format("2{0}", mobile));
            return datasms;
        }

        public static string Send(string userName, string password, string mobile, string msg, string sender, string url, string encoding = "utf-8")
        {
            if(url.Contains("http://www.masrawy.com/SMSService/send.aspx"))
            {
                return Sendusingmasrawy(userName, password, mobile, msg, sender, encoding,url);
            }
            if(url.Contains("http://www.resalty.net/api/sendSMS.php")){
                return SendusingResalaty(userName, password, mobile, msg, sender, encoding, url);
            }
            if (url.Contains("http://sms4kw.com/sendsms.php"))
            {
                return SendusingSms4Kw(userName, password, mobile, msg, sender, url);
            }
            if (url.Contains("http://api.sms.northstartec.com/api/sendsms/plain"))
            {
                return Sendusingapi(userName, password, mobile, msg, sender, url);
            }
            if (url.Contains("http://smsmisr.com/api/send.aspx"))
            {
                return SendUsingSmsMisr(userName, password, sender, mobile, msg, 1, url);
            }

            if (url.Contains("http://smsbox.dawlia.net/api/send.aspx"))
            {
                return SendusingDawlia(userName, password,1, sender, mobile, msg, url);
            }
            if (url.Contains("http://smssmartegypt.com/api/"))
            {
                return SendUsingSmartEgypt(userName, password, sender, mobile, msg, url);
            }
            if (url.Contains("http://api.otsdc.com/wrapper/sendSMS.php"))
            {
                return Sendunifonicapi(userName, password,mobile, msg,sender, url);
            }
            return "";
        }
        public static string SendSmsByNotification(ISPDataContext context,string mobile, int indexNotification)
        {
            // send sms by ashraf
            try
            {
                var smsdata = context.SMSCnfgs.FirstOrDefault();
                if (smsdata != null && Convert.ToBoolean(smsdata.sendsms))
                {
                    var messagetext = context.SMSCaseNotifications.FirstOrDefault(a => a.Id == indexNotification);
                    var mobil = mobile;
                    if (messagetext != null && Convert.ToBoolean(messagetext.Send))
                    {
                        var message = Send(smsdata.UserName, smsdata.Password, mobil,
                            messagetext.Message,
                            smsdata.Sender, smsdata.UrlAPI);
                        return message;
                    }
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
