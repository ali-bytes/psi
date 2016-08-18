using System;
using System.Configuration;
using System.Text;
using Db;
using NewIspNL.Helpers;

namespace NewIspNL
{
    /// <summary>
    /// Summary description for CenterMessage
    /// </summary>
    public static class CenterMessage
    {
        readonly static ISPDataContext Context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        public static void SendPublicMessageReport(WorkOrder uwo, string message, int userid)
        {
            var body = new StringBuilder();
            body.Append(message);
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerPhone));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" محافظة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.Governorate.GovernorateName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append("<br/>");


            if (uwo.ResellerID != null && uwo.ResellerID != -1)
            {
                FillMessage(Convert.ToInt32(uwo.ResellerID), 1, body.ToString(), string.Format("{0} {1}", "بخصوص العميل", uwo.CustomerName));
            }
            FillMessage(1, userid, body.ToString(), string.Format("{0} {1}", "العميل", uwo.CustomerName));
        }
        public static void SendPublic(string name,string phone, string message, int sendfrom,int sendTo)
        {
            var body = new StringBuilder();
            body.Append(message);
            body.Append("<br/>");
            body.Append(string.Format("{0}", name));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", phone));
            body.Append("<br/>");
            body.Append("<hr/>");
            FillMessage(sendTo, 1, body.ToString(), string.Format("{0} {1}", "بخصوص", name));

            FillMessage(1, sendfrom, body.ToString(), string.Format("{0} {1}", "بخصوص", name));
        }
        public static void SendMessageReport(WorkOrder uwo, string oldState, string newState, int userid)
        {
            var body = new StringBuilder();
            body.Append(" تم تحويل العميل");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerPhone));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" محافظة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.Governorate.GovernorateName));
            body.Append("<hr/>");
            body.Append(" من الحالة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", oldState));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" الى الحالة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", newState));
            body.Append("<br/>");

        
            if (uwo.ResellerID != null && uwo.ResellerID != -1)
            {
                FillMessage(Convert.ToInt32(uwo.ResellerID), 1,body.ToString(),string.Format("{0} {1}", "تغير حالة عميل", uwo.CustomerName));
            }
            FillMessage(1,userid,body.ToString(),string.Format("{0} {1}", "تغير حالة عميل", uwo.CustomerName));
        }

        public static void SendRequestApproval(WorkOrder uwo, string requestType, int userid)
        {
            var body = new StringBuilder();
            body.Append(" تم موافقة الطلب الخاص  بالعميل");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerPhone));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" محافظة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.Governorate.GovernorateName));
            body.Append("<hr/>");
            body.Append(" نوع الطلب");
            body.Append("<br/>");
            body.Append(string.Format("{0}", requestType));

            if (uwo.ResellerID != null && uwo.ResellerID != -1)
            {
                FillMessage(Convert.ToInt32(uwo.ResellerID), 1, body.ToString(), string.Format("{0} {1}", "موافقة طلب العميل", uwo.CustomerName));
            }
            FillMessage(1, userid, body.ToString(), string.Format("{0} {1}", "موافقة طلب العميل", uwo.CustomerName));
        }
        public static void SendRequestApproval(string customerName,string customerPhone,string governrate,int resellerId, string requestType, int userid)
        {
            var body = new StringBuilder();
            body.Append(" تم موافقة الطلب الخاص  بالعميل");
            body.Append("<br/>");
            body.Append(string.Format("{0}", customerName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", customerPhone));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" محافظة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", governrate));
            body.Append("<hr/>");
            body.Append(" نوع الطلب");
            body.Append("<br/>");
            body.Append(string.Format("{0}", requestType));

            if (resellerId != -1 && resellerId!=0)
            {
                FillMessage(Convert.ToInt32(resellerId), 1, body.ToString(), string.Format("{0} {1}", "موافقة طلب العميل", customerName));
            }
            FillMessage(1, userid, body.ToString(), string.Format("{0} {1}", "موافقة طلب العميل", customerName));
        }
        public static void SendRequestReject(WorkOrder uwo, string rejectReason, string requestType, int userid)
        {
            var body = new StringBuilder();
            body.Append(" تم رفض الطلب الخاص  بالعميل");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.CustomerPhone));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" محافظة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", uwo.Governorate.GovernorateName));
            body.Append("<hr/>");
            body.Append(" نوع الطلب");
            body.Append("<br/>");
            body.Append(string.Format("{0}", requestType));
            body.Append("<hr/>");
            body.Append(" سبب الرفض");
            body.Append("<br/>");
            body.Append(rejectReason);
            if (uwo.ResellerID != null && uwo.ResellerID != -1)
            {
                FillMessage(Convert.ToInt32(uwo.ResellerID), 1, body.ToString(), string.Format("{0} {1}", "رفض طلب العميل ", uwo.CustomerName));
            }
            FillMessage(1, userid, body.ToString(), string.Format("{0} {1}", "رفض طلب العميل ", uwo.CustomerName));
        }
        public static void SendRequestReject(string customerName, string customerPhone, string governrate, int resellerId, string requestType, string rejectReason, int userid)
        {
            var body = new StringBuilder();
            body.Append(" تم رفض الطلب الخاص  بالعميل");
            body.Append("<br/>");
            body.Append(string.Format("{0}", customerName));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" تليفون");
            body.Append("<br/>");
            body.Append(string.Format("{0}", customerPhone));
            body.Append("<br/>");
            body.Append("<hr/>");
            body.Append(" محافظة");
            body.Append("<br/>");
            body.Append(string.Format("{0}", governrate));
            body.Append("<hr/>");
            body.Append(" نوع الطلب");
            body.Append("<br/>");
            body.Append(string.Format("{0}", requestType));
            body.Append("<hr/>");
            body.Append(" سبب الرفض");
            body.Append("<br/>");
            body.Append(rejectReason);
            if (resellerId != 0 && resellerId != -1)
            {
                FillMessage(Convert.ToInt32(resellerId), 1, body.ToString(), string.Format("{0} {1}", "رفض طلب العميل ", customerName));
            }
            FillMessage(1, userid, body.ToString(), string.Format("{0} {1}", "رفض طلب العميل ",customerName));
        }

        public static void FillMessage(int messageto, int messagefrom, string body, string subject)
        {
            var message = new Message{
                MessageTo = messageto,
                DoneRead = false,
                MessageFrom = messagefrom,
                Message1 = body,
                Subject = subject,
                Time = DateTime.Now.AddHours()
            };
            Context.Messages.InsertOnSubmit(message);
            Context.SubmitChanges();
        }
    }
}