using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Db;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.DemandServices;
using Resources;

namespace NewIspNL.Services{
    public class OptionsService{
        readonly static ISPDataContext Context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
        public static Option GetOptions(ISPDataContext context, bool redirect){
            var option = context.Options.FirstOrDefault();
            if(option == null && redirect && HttpContext.Current != null)
                HttpContext.Current.Response.Redirect("~/Pages/Options.aspx");
            return option;
        }
        public static Option SaveOptions(ISPDataContext context,int count,bool merge,bool include,bool reciept,
            bool discound, int from, int to, int timedifference, bool discoundresandbra, bool uploadfielstonewcustomer,
            bool allowminuscredit,bool showstatistic,bool installationdiscound,int daysCount,int alertWay,
            string remindertxt,int userId,bool sendMessage,bool showcounters,bool showrequestinsearch,bool validateCustomerPhone,bool showAlldemand,bool activeFawry, bool autoSuspendCustomerUnderReseller,
            int fromPackageType,int toPackageType,decimal conversionDebt,
            int day1, int day2, int day3, int day4, int day5, int day6, int percentage1, int percentage2, int percentage3, int percentage4, int percentage5, int percentage6, bool isInstallmentActive, bool showDedWithFixedRequestDate, bool preventUnsusForCustomerHasIndebtedness, int portalRelayDays,
            bool preventSuspendBeforeMonth
            )
        {
            var option = GetOptions(context,false);
            if(option == null){
                option=new Option{
                    IncludeGovernorateInSearch = include,
                    MergeGovernorateWithPhoneInCreateCustomer = merge,
                    SuspendDaysCount = count,
                    WidthOfReciept = reciept,
                    DiscoundFromBranchCredit = discound,
                    FromDay = from,
                    ToDay = to,
                    TimeDifference =timedifference,
                    DiscoundfromResellerandBranch = discoundresandbra,
                    UploadFielsToNewCustomer = uploadfielstonewcustomer,
                    Allowminuscredit = allowminuscredit,
                    ShowStatistic = showstatistic,
                    IntallationDiscound = installationdiscound,
                    DaysOfUnpaidDemandsLimit = daysCount,
                    AlertWayOfUnpaidDemand = alertWay,
                    ReminderMessage = remindertxt,
                    ReminderToUserId =userId,
                    SendMessageAfterOperations = sendMessage,
                    ShowCounters = showcounters,
                    ShowRequestsInSearch = showrequestinsearch,
                   ValidationOnCustomerPhone = validateCustomerPhone,
                   ShowAllDemandOfPR = showAlldemand,
                   FawryService = activeFawry,
                    AutoSuspendCustomersUnderReseller = autoSuspendCustomerUnderReseller,
                   ConvertFromPackageType = fromPackageType,
                   ConvertToPackageType = toPackageType,
                   ConversionDebt=conversionDebt,
                   Day1 = day1,
                   Day2 = day2,
                   Day3 = day3,
                   Day4 = day4,
                   Day5 = day5,
                   Day6 = day6,
                   Percentage1 = percentage1,
                   Percentage2 = percentage2,
                   Percentage3 = percentage3,
                   Percentage4 = percentage4,
                   Percentage5 = percentage5,
                   Percentage6 = percentage6,
                   IsResellerPaymentActive = isInstallmentActive,
                   ShowDeductionWithFixedRequestDateInCD = showDedWithFixedRequestDate,
                   PreventUnsusForCustomerHasIndebtedness = preventUnsusForCustomerHasIndebtedness,
                    PortalRelayDays = portalRelayDays,
                    PreventSuspendBeforeMonthFromReActive = preventSuspendBeforeMonth
                };
                context.Options.InsertOnSubmit(option);
            } else{
                option.IncludeGovernorateInSearch = include;
                option.MergeGovernorateWithPhoneInCreateCustomer = merge;
                option.SuspendDaysCount = count;
                option.WidthOfReciept = reciept;
                option.DiscoundFromBranchCredit = discound;
                option.FromDay = from;
                option.ToDay = to;
                option.TimeDifference = timedifference;
                option.DiscoundfromResellerandBranch = discoundresandbra;
                option.UploadFielsToNewCustomer = uploadfielstonewcustomer;
                option.Allowminuscredit = allowminuscredit;
                option.ShowStatistic = showstatistic;
                option.IntallationDiscound = installationdiscound;
                option.AlertWayOfUnpaidDemand = alertWay;
                option.DaysOfUnpaidDemandsLimit = daysCount;
                option.ReminderMessage = remindertxt;
                option.ReminderToUserId = userId;
                option.SendMessageAfterOperations = sendMessage;
                option.ShowCounters = showcounters;
                option.ShowRequestsInSearch = showrequestinsearch;
                option.ValidationOnCustomerPhone = validateCustomerPhone;
                option.ShowAllDemandOfPR = showAlldemand;
                option.FawryService = activeFawry;
                option.AutoSuspendCustomersUnderReseller = autoSuspendCustomerUnderReseller;
                option.ConvertFromPackageType = fromPackageType;
                option.ConvertToPackageType = toPackageType;
                option.ConversionDebt = conversionDebt;
                option.Day1 = day1;
                option.Day2 = day2;
                option.Day3 = day3;
                option.Day4 = day4;
                option.Day5 = day5;
                option.Day6 = day6;
                option.Percentage1 = percentage1;
                option.Percentage2 = percentage2;
                option.Percentage3 = percentage3;
                option.Percentage4 = percentage4;
                option.Percentage5 = percentage5;
                option.Percentage6 = percentage6;
                option.IsResellerPaymentActive = isInstallmentActive;
                option.ShowDeductionWithFixedRequestDateInCD = showDedWithFixedRequestDate;
                option.PreventUnsusForCustomerHasIndebtedness = preventUnsusForCustomerHasIndebtedness;
                option.PortalRelayDays = portalRelayDays;
                option.PreventSuspendBeforeMonthFromReActive = preventSuspendBeforeMonth;
            }
            context.SubmitChanges();
            return option;
        }

        public static void CheckResellerPayment()
        {
            var dateNow = DateTime.Now.AddHours().Day;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                DemandsSearchService _searchService;
                _searchService = new DemandsSearchService(context);

                var option = context.Options.FirstOrDefault();
                if (option == null || option.IsResellerPaymentActive == false)
                {
                    return;
                }

                if (option.Day1 == dateNow || option.Day2 == dateNow || option.Day3 == dateNow || option.Day4 == dateNow || option.Day5 == dateNow || option.Day6 == dateNow)
                {
                    //Branches implement reseller payment 
                    var branchesWpay = context.BranchesForResellerPayments.ToList();
                    // Branches
                    var resellers = new List<User>();
                    foreach (var b in branchesWpay)
                    {
                        var reseller = context.Users.Where(x => x.GroupID == 6 && x.BranchID == b.BranchId).ToList();
                        resellers.AddRange(reseller);
                    }

                    if (resellers.Count <= 0)
                    {
                        return;
                    }

                    string stopedResellersMsg = "الموزعين الموجودين بالرسالة تم ايقافهم لعدم تخطى نسبة التحصيل المقررة";
                    string stopedResellersSubject = "تنبية بموزعين تم ايقافهم  ";

                    string notifySubject = "تنبية بإيقاف عملاء موزعين  ";
                    string notifyMsg = "الموزعين الموجودين بالرسالة يجب إيقاف عملائهم لعدم تخطى نسبة التحصيل المقررة";

                   //فى حالة اليوم الأول والثالث والخامس يتم ايقاف الموزع
                    //فى حالة اليوم الثانى والرابع والسادس لا يتم توقيف الموزع (يتم تنبية الادمن فقط)-على سلام
                    //اليوم الأول
                    if (option.Day1 == dateNow)
                    {
                        if (option.Percentage1 !=null)
                        {
                            int percentage = option.Percentage1??0;
                            if (percentage>0)
                            {
                                var reseller = new List<User>();
                                reseller = CheckInstallment(resellers, _searchService, percentage, context,false);
                                if (reseller.Count <= 0) return;
                               //ارسال رسالة للأدمنز لمركز الرسايل الداخلى
                                SendInternalMessageCenterReportToAdmin(reseller, stopedResellersSubject, stopedResellersMsg);
                                //ارسال رسالة للموزعين الذين تم ايقافهم للإيميل الخارجى
                                NotifyResellerAfterAccountStoped(reseller);
                            }
                           
                        }
                       

                    }//اليوم الثانى
                    else if (option.Day2 == dateNow)
                    {
                        if (option.Percentage2 != null)
                        {
                            int percentage = option.Percentage2 ?? 0;
                            if (percentage > 0)
                            {
                                var reselleres = new List<User>();
                                reselleres = CheckInstallment(resellers, _searchService, percentage, context,true);
                                if (reselleres.Count<=0)return;
                               
                                //ارسال رسالة  تنبية للأدمنز لمركز الرسايل الداخلى
                                SendInternalMessageCenterReportToAdmin(reselleres, notifySubject, notifyMsg);
                                //ارسال رسالة تنبية للأدمنز بالموزعين لايقاف حساباتهم - للإيميل الخارجى
                                NotifyAdminOutSideMail(reselleres, notifySubject, notifyMsg);
                               
                            }

                        }
                       
                    }
                    else if (option.Day3 == dateNow)
                    {
                        if (option.Percentage3 != null)
                        {
                            int percentage = option.Percentage3 ?? 0;
                            if (percentage > 0)
                            {
                                var reseller = new List<User>();
                                reseller = CheckInstallment(resellers, _searchService, percentage, context, false);
                                if (reseller.Count <= 0) return;
                                //ارسال رسالة للأدمنز لمركز الرسايل الداخلى
                                SendInternalMessageCenterReportToAdmin(reseller, stopedResellersSubject, stopedResellersMsg);
                                //ارسال رسالة للموزعين الذين تم ايقافهم للإيميل الخارجى
                                NotifyResellerAfterAccountStoped(reseller);
                            }

                        }
                    }
                    else if (option.Day4 == dateNow)
                    {
                        if (option.Percentage4 != null)
                        {
                            int percentage = option.Percentage4 ?? 0;
                            if (percentage > 0)
                            {
                                var reselleres = new List<User>();
                                reselleres = CheckInstallment(resellers, _searchService, percentage, context, true);
                                if (reselleres.Count <= 0) return;
                                //ارسال رسالة  تنبية للأدمنز لمركز الرسايل الداخلى
                                SendInternalMessageCenterReportToAdmin(reselleres, notifySubject, notifyMsg);
                                //ارسال رسالة تنبية للأدمنز بالموزعين لايقاف حساباتهم - للإيميل الخارجى
                                NotifyAdminOutSideMail(reselleres, notifySubject, notifyMsg);
                              
                            }

                        }
                    }
                    else if (option.Day5 == dateNow)
                    {
                        if (option.Percentage5 != null)
                        {
                            int percentage = option.Percentage5 ?? 0;
                            if (percentage > 0)
                            {
                                var reseller = new List<User>();
                                reseller = CheckInstallment(resellers, _searchService, percentage, context, false);
                                if (reseller.Count <= 0) return;
                                //ارسال رسالة للأدمنز لمركز الرسايل الداخلى
                                SendInternalMessageCenterReportToAdmin(reseller, stopedResellersSubject, stopedResellersMsg);
                                //ارسال رسالة للموزعين الذين تم ايقافهم للإيميل الخارجى
                                NotifyResellerAfterAccountStoped(reseller);
                            }

                        }
                    }
                    else if (option.Day6 == dateNow)
                    {
                        if (option.Percentage6 != null)
                        {
                            int percentage = option.Percentage6 ?? 0;
                            if (percentage > 0)
                            {
                                var reselleres = new List<User>();
                                reselleres = CheckInstallment(resellers, _searchService, percentage, context, true);
                                if (reselleres.Count <= 0) return;
                                //ارسال رسالة  تنبية للأدمنز لمركز الرسايل الداخلى
                                SendInternalMessageCenterReportToAdmin(reselleres, notifySubject, notifyMsg);
                                //ارسال رسالة تنبية للأدمنز بالموزعين لايقاف حساباتهم - للإيميل الخارجى
                                NotifyAdminOutSideMail(reselleres, notifySubject, notifyMsg);
                               
                            }

                        }
                    }

                }
            }
        }

        private static List<User> CheckInstallment(List<User> resellers, DemandsSearchService _searchService, 
            int Percentage, ISPDataContext context, bool toMail )
        {
            var resToProcess = new List<User>();
            foreach (var res in resellers)
            {
                //رصيد الموزع الحالى لو بالسالب يبق لة
                decimal balanc =Math.Round(Convert.ToDecimal(Billing.GetLastBalance(res.ID, "Reseller")),2);

                var searchDemands = _searchService.SearchDemandsToPreview(new BasicSearchModel
                {
                    Paid = false,
                    ResellerId = res.ID,
                    Month = Convert.ToInt32(DateTime.Now.Month),
                    Year = Convert.ToInt32(DateTime.Now.Year),
                    WithResellerDiscount = true
                });
                var newlist = new List<DemandPreviewModel>();
                var sp = context.SPoptionReselleraccounts.Select(z => z).ToList();
                foreach (var i in sp)
                {
                    var data = searchDemands.Where(a => a.Provider == i.ServiceProvider.SPName).ToList();
                    newlist.AddRange(data);
                }

                // اجمالى قيمة الفواتير على الموزع
                var totalDemand = Helper.FixNumberFormat((newlist.Sum(x => x.ResellerNet)));

                if (Convert.ToDecimal(totalDemand) <= 0)continue;
               
                if (balanc < 0)
                {
                    decimal tD = Convert.ToDecimal(totalDemand);

                    //القيمة المطلوب دفعها بناءً على النسبة
                    var valueToPay = Math.Round((Convert.ToDecimal(Percentage)*tD))/100;

                    //تحدد اذا كان رصيد الموزع يغطى القيمة المطلوب دفعها
                    if ((balanc*-1) < valueToPay)
                    {
                        var userToProcess = context.Users.FirstOrDefault(x => x.ID == res.ID);
                        resToProcess.Add(userToProcess);

                        if (!toMail)
                        {
                            //stop the reseller
                            var userToStop = context.Users.FirstOrDefault(x => x.ID == res.ID);
                            if (userToStop != null)
                            {
                                StopReseller(userToStop, context);
                            }
                        }
                       
                       
                    }
                }
                else
                {
                    var userToProcess = context.Users.FirstOrDefault(x => x.ID == res.ID);
                    resToProcess.Add(userToProcess);

                    if (!toMail)
                    {
                        //stop the reseller
                        var userToStop = context.Users.FirstOrDefault(x => x.ID == res.ID);
                        if (userToStop != null)
                        {
                            StopReseller(userToStop, context);
                        }
                    }
                    
                }
            }

            return resToProcess.ToList();
        }

        //تنبية للأدمنز بإيقاف عملاء موزعين للإيميل الخارجى
        static void NotifyAdminOutSideMail(List<User> resellersToStop,string subject, string message)
        {

            var active = Context.EmailCnfgs.FirstOrDefault();
            if (active == null || !active.Active) return;
            var admins = Context.Users.Where(a => a.GroupID == 1).ToList();
            if (admins.Count<=0) return;

            var body = new StringBuilder();
            body.Append("<div style='margin: 20px auto;width: 80%;text-align:center;'>");
            foreach (var user in resellersToStop)
            {
                body.Append(
                    "<h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                    ":" + Tokens.Reseller + "</span></div>" + user.UserName +
                    "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo +
                    "</div> <span><br/> " + user.UserPhone + "</h3>");
                body.Append("<hr/>");

               // resellersNames.Append("الموزع");
               // resellersNames.Append("<br/>");
               // resellersNames.Append(string.Format("{0}", user.UserName));
               // resellersNames.Append("<br/>");
               //// resellersNames.Append("<hr/>");
               // resellersNames.Append(" تليفون");
               // resellersNames.Append("<br/>");
               // resellersNames.Append(string.Format("{0}", user.UserPhone));
               // resellersNames.Append("<br/>");
               // resellersNames.Append("<hr/>");
            }

            body.Append("<p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + message + "</p></div>");


            //var msg =
            //    "<div style='margin-left:30%'><table style=' background-color: #CEF6EC;border: 1px solid black;' ><tr ><th>" + updatedwo.CustomerName + "</th> <td>" + ":" + Tokens.Customer_Name + "</tr><tr><th>" + updatedwo.CustomerPhone + "</th><td>" + ":" + Tokens.PhoneNo + "</td</tr></table>  <br/><br/></div>" + "<div style='margin-right:35%'><h4>" + message + "</h4></div>";

            //var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.reseller + "</span></div>" + user.UserName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + user.UserPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + message + "</p></div>";

            var formalmessage = ClsEmail.Body(body.ToString());
            foreach (var admin in admins)
            {
                ClsEmail.SendEmail(admin.UserEmail,
               subject, formalmessage
               , true);
            }
           
        }


        //تنبية للأدمنز بإيقاف عملاء موزعين لمركز الرسايل الداخلى
        public static void SendInternalMessageCenterReportToAdmin(List<User> resellersToStop, string subject, string message)
        {
            var admins = Context.Users.Where(a => a.GroupID == 1).ToList();
            if (admins.Count <= 0) return;

            var body = new StringBuilder();
            body.Append(message);
            body.Append("<br/>");
            foreach (var res in resellersToStop)
            {
                body.Append(string.Format("{0}", res.UserName));
                body.Append("<br/>");
                //body.Append("<hr/>");
                body.Append(" تليفون");
                body.Append("<br/>");
                body.Append(string.Format("{0}", res.UserPhone));
                body.Append("<br/>");
                body.Append("<hr/>");
                body.Append("<br/>");
            }

            foreach (var admin in admins)
            {
                CenterMessage.FillMessage(admin.ID, 1, body.ToString(), subject);
            }
                     
        }
        //تنبية للموزعين الذين تم ايقافهم للإيميل الخارجى
        static void NotifyResellerAfterAccountStoped(List<User> stopedResellers)
        {
            var active = Context.EmailCnfgs.FirstOrDefault();
            if (active == null || !active.Active) return;
            string message = "تم ايقاف حسابك لعدم السداد";

            foreach (var res in stopedResellers)
            {
                var msg = "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" + ":" + Tokens.Reseller + "</span></div>" + res.UserName + "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo + "</div> <span><br/> " + res.UserPhone + "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" + message + "</p></div>";

                var formalmessage = ClsEmail.Body(msg);
                try
                {
                    ClsEmail.SendEmail(res.UserEmail,
                  "تم ايقاف حسابك لعدم السداد", formalmessage
                  , true);
                }
                catch
                {
                    continue;
                   
                }
               
            }
          
           
        }

        static void StopReseller(User user, ISPDataContext context)
        {
            user.IsAccountStopped = true;
            context.SubmitChanges();
        }

        public static void SaveDistributorOptions(ISPDataContext context, decimal collectionCommission, int boxid, bool clientActivationSubtract, bool subtractResellerCommission)
        {
             var option = GetDistributorOptions(context,false);
            if (option == null)
            {
                option = new DistributorOption
                {
                    CollectionCommission = collectionCommission,
                    ClientActivationSubtract = clientActivationSubtract,
                    BoxId = boxid,
                    SubtractResellerCommission = subtractResellerCommission
                };
                context.DistributorOptions.InsertOnSubmit(option);
            }
            else
            {
                option.CollectionCommission = collectionCommission > 0 ? collectionCommission : 0;
                option.ClientActivationSubtract = clientActivationSubtract;
                option.SubtractResellerCommission = subtractResellerCommission;
                option.BoxId = boxid;
            }

            context.SubmitChanges();
        }
        public static DistributorOption GetDistributorOptions(ISPDataContext context, bool redirect)
        {
            var option = context.DistributorOptions.FirstOrDefault();
            if (option == null && redirect && HttpContext.Current != null)
                HttpContext.Current.Response.Redirect("~/Pages/Options.aspx");
            return option;
        }
    }
}

        
