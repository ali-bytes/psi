using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BL.Concrete;
using Db;
using NewIspNL.Domain;
using NewIspNL.Domain.Abstract;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services;
using NewIspNL.Services.DemandServices;
using NewIspNL.Services.OfferServices;
using Resources;

namespace NewIspNL.Pages
{
    public partial class ManageRequests : CustomPage
    {
        private readonly IspEntries _ispEntries;
        private readonly IResellerCreditRepository _creditRepository;
        private readonly IspDomian _domian;
        private readonly PriceServices _priceServices;
        private readonly IWorkOrderRepository _work;

        public ManageRequests()
        {
            var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
            _creditRepository = new ResellerCreditRepository();
            _domian = new IspDomian(context);
            _priceServices = new PriceServices();
            _work = new WorkOrderRepository();
            _ispEntries = new IspEntries(context);
        }


        private void HandleRequest(int requestTypeId, WorkOrder workOrder, WorkOrderRequest request,
            ISPDataContext context,
            int userId)
        {
            var ispEntries = new IspEntries(context);
            var demandService = new DemandService(context);
            var option = OptionsService.GetOptions(context, true);
            switch (requestTypeId)
            {
                //UpgradeDowngrade
                case 1:
                    var hour = DateTime.Now.AddHours();
                    var date = Convert.ToDateTime(TbUpDwonDate.Text).AddHours(hour.Hour).AddMinutes(hour.Minute);

                    request.ProcessDate = date;
                    if (RblUpDwonOptions.SelectedIndex == 0)
                    {
                        if (request.NewPackageID != null)
                            demandService.ProcessUpDownGradeDemand(workOrder.ID, date, request.NewPackageID.Value,
                                userId,
                                false, true, RblUpDwonOptions.SelectedIndex);

                    }
                    else
                    {
                        if (request.NewPackageID != null)
                            demandService.ProcessUpDownGradeDemand(workOrder.ID, date, request.NewPackageID.Value,
                                userId,
                                false, false, RblUpDwonOptions.SelectedIndex);
                    }
                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                    {
                        CenterMessage.SendRequestApproval(workOrder, Tokens.Upgrade_Downgrade, userId);
                    }
                    context.SubmitChanges();
                    SendSms(context, workOrder, 15);
                    break;
                //Suspend
                case 2:
                    {
                        var hour2 = DateTime.Now.AddHours();
                        var uiNow = Convert.ToDateTime(TbMiscDate.Text).AddHours(hour2.Hour).AddMinutes(hour2.Minute);


                        request.ProcessDate = uiNow;
                        workOrder.WorkOrderStatusID = 11;

                        global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                        {
                            WorkOrderID = workOrder.ID,
                            StatusID = workOrder.WorkOrderStatusID,
                            UserID = Convert.ToInt32(Session["User_ID"]),
                            UpdateDate = uiNow
                        };
                        context.WorkOrderStatus.InsertOnSubmit(wos);
                        context.SubmitChanges();

                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, Tokens.Suspend, userId);
                        }
                        SendSms(context, workOrder, 13);
                    }
                    break;
                //Unsuspend
                case 3:
                    {

                        var hour3 = DateTime.Now.AddHours();
                        var now = Convert.ToDateTime(TbUnsuspendDate.Text).AddHours(hour3.Hour).AddMinutes(hour3.Minute);



                        var period = Convert.ToInt32(TbDaysCount.Text);
                        request.ProcessDate = now;
                        workOrder.WorkOrderStatusID = 6;
                        context.SubmitChanges();
                        if (workOrder.OfferStart == null) workOrder.OfferStart = now;
                        //------------


                        var checkdemand = CheckDemand(workOrder, request, ispEntries, now);
                       

                        //-----------
                      
                       
                        if (!checkdemand)
                        {

                            var dem = ispEntries.OrderDemand2(workOrder.ID, now).ToList();
                            var demand2 = dem.LastOrDefault();

                            if (demand2 != null)
                            {

                                if (!demand2.Paid && demand2.Amount < 0)
                                {
                                    //فى حالة ان الفاتورة غير مدفوعة و قيمة الفاتورة بالسالب 
                                    switch (ClickedBtn.Value)
                                    {
                                        case "1":
                                            //فى حالة اختار ترحيل ايام السسبند
                                            var endAt = demand2.EndAt.AddDays(period);
                                            demand2.WorkOrder.RequestDate = endAt;
                                            demand2.Amount = 0;
                                            demand2.Paid = true;
                                            demand2.PaymentDate = DateTime.Now.AddHours();
                                            demand2.PaymentComment = "دفعت بطلب تشغيل";
                                           
                                               var curDate = workOrder.RequestDate.Value;
                                               workOrder.RequestDate = curDate.AddDays(period);
                                               
                                            break;
                                        case "2":
                                            //فى حالة اختيار تخصيم مع ثبات تاريخ المطالبة.. هيضرب عدد الايام اللى هيخصمها فى تمن السرعة لليوم الواحد
                                            var newPackBill =
                                                _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(
                                                    workOrder, now.Month, now.Year,
                                                    Convert.ToInt32(workOrder.ServicePackageID));
                                            var priceForDayinDiscoundDays = (newPackBill.Net / 30) * period;
                                            demand2.Amount = priceForDayinDiscoundDays * -1;
                                            break;
                                        case "3":
                                            //فى حالة اختار فاتورة كاملة هيحط المطالبة بصفر
                                            demand2.Amount = 0;
                                            demand2.Paid = true;
                                            demand2.PaymentDate = DateTime.Now.AddHours();
                                            demand2.PaymentComment = "دفعت بطلب تشغيل";
                                            break;
                                    }
                                    ispEntries.Commit();
                                }
                                else if (!demand2.Paid && demand2.Amount > 0)
                                {
                                    if (demand2.CaseDetectSuspend != null)
                                    {
                                        demand2.Amount = Convert.ToDecimal(demand2.CaseDetectSuspend);
                                        ispEntries.Commit();
                                        // demand > 0
                                    }
                                    var daysCount = period;
                                    switch (ClickedBtn.Value)
                                    {
                                        case "1":
                                            UpdateRequestDatePostpone(workOrder, demandService, daysCount);
                                            break;
                                        case "2":

                                            UpdateRequestDate(workOrder, demandService, daysCount);
                                            break;
                                        case "3":

                                            break;
                                    }

                                    context.SubmitChanges();
                                }
                                else if (demand2.Paid)
                                {
                                    // ترحيل
                                    if (Convert.ToInt32(ClickedBtn.Value) == 1)
                                    {
                                        
                                            var curDate = workOrder.RequestDate.Value;
                                            workOrder.RequestDate = curDate.AddDays(period);
                                           
                                    }

                                    //-------------------------
                                    var newPackBill =
                                        _priceServices.CustomerInvoiceDetailsDefaultbByPackage(workOrder,
                                            now.Month, now.Year, Convert.ToInt32(workOrder.ServicePackageID));
                                    decimal amount;
                                    var allAmount = newPackBill.Net + Convert.ToDecimal(workOrder.PaymentType.Amount);
                                    var periodOfDemand = (demand2.EndAt.Date - demand2.StartAt.Date).Days;
                                    if (periodOfDemand == 30 || periodOfDemand == 31 ||
                                        (periodOfDemand == 28 && demand2.StartAt.Month == 2)) amount = allAmount;
                                    else amount = (allAmount / 30) * periodOfDemand;
                                    if (amount > demand2.Amount)
                                    {
                                        // logic here لو فى فرق بين الاثنين ينزل مطالبة بالفرق غير مدفوعة - لو مفيش فرق بريك 
                                        var am = amount - demand2.Amount;
                                        var factory = new DemandFactory(ispEntries);
                                        var newdemand = factory.CreateDemand(workOrder, now, demand2.EndAt, am,
                                            Convert.ToInt32(Session["User_ID"]),
                                            notes:
                                                " الفرق بين الفاتورة المدفوعة و فاتورة باقى الشهر اذا كانت الفاتورة المدفوعة تم تصفيتها");
                                        ispEntries.AddDemand(newdemand);
                                        ispEntries.Commit();
                                    }
                                }

                            }

                        }
                        var wos = new global::Db.WorkOrderStatus
                        {
                            WorkOrderID = workOrder.ID,
                            StatusID = workOrder.WorkOrderStatusID,
                            UserID = Convert.ToInt32(Session["User_ID"]),
                            UpdateDate = now
                        };
                        context.WorkOrderStatus.InsertOnSubmit(wos);
                        context.SubmitChanges();

                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, "Unsuspend", userId);
                        }
                    }
                    break;
                //Hold
                case 4:
                    {
                        var hour3 = DateTime.Now.AddHours();
                        var now = Convert.ToDateTime(TbMiscDate.Text).AddHours(hour3.Hour).AddMinutes(hour3.Minute);



                        request.ProcessDate = now;
                        workOrder.WorkOrderStatusID = 10;
                        context.SubmitChanges();
                        var wos = new global::Db.WorkOrderStatus
                        {
                            WorkOrderID = workOrder.ID,
                            StatusID = workOrder.WorkOrderStatusID,
                            UserID = Convert.ToInt32(Session["User_ID"]),
                            UpdateDate = now
                        };
                        context.WorkOrderStatus.InsertOnSubmit(wos);
                        context.SubmitChanges();
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, Tokens.MenuHold, userId);
                        }
                    }
                    break;
                //Unhold
                case 5:
                    {
                        var hour3 = DateTime.Now.AddHours();
                        var now = Convert.ToDateTime(TbMiscDate.Text).AddHours(hour3.Hour).AddMinutes(hour3.Minute);



                        request.ProcessDate = now;
                        if (workOrder.OfferStart == null) workOrder.OfferStart = now;

                        CheckDemand(workOrder, request, ispEntries, now);

                        workOrder.WorkOrderStatusID = 6;

                        var wos = new global::Db.WorkOrderStatus
                        {
                            WorkOrderID = workOrder.ID,
                            StatusID = workOrder.WorkOrderStatusID,
                            UserID = Convert.ToInt32(Session["User_ID"]),
                            UpdateDate = now
                        };
                        context.WorkOrderStatus.InsertOnSubmit(wos);
                        context.SubmitChanges();
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, "Unhold", userId);
                        }
                    }
                    break;
                //Cancel
                case 6:
                    {
                        var hour3 = DateTime.Now.AddHours();
                        var now = Convert.ToDateTime(TbCancelDate.Text).AddHours(hour3.Hour).AddMinutes(hour3.Minute);



                        request.ProcessDate = now;
                        if (workOrder.WorkOrderStatusID != null)
                        {
                            var originalStatusId = workOrder.WorkOrderStatusID.Value;
                            workOrder.Status = context.Status.FirstOrDefault(s => s.ID == 8);
                            workOrder.OfferId = null;
                            context.SubmitChanges();
                            var wos = new global::Db.WorkOrderStatus
                            {
                                WorkOrderID = workOrder.ID,
                                StatusID = workOrder.WorkOrderStatusID,
                                UserID = Convert.ToInt32(Session["User_ID"]),
                                UpdateDate = now
                            };
                            context.WorkOrderStatus.InsertOnSubmit(wos);
                            context.SubmitChanges();
                            UpdateDemonds(now, request.WorkOrderID, userId, originalStatusId, demandService,
                                RblCancelOptions.SelectedIndex);
                            var wor = new WorkOrderRepository();
                            var activationDate = wor.GetActivationDate(workOrder.ID, 6);
                            if (workOrder.CreationDate != null && workOrder.Offer != null && activationDate != null)
                            {
                                var period = (now.Date - activationDate.Value.Date).Days;
                                if (period <= 365)
                                {
                                    if (workOrder.Offer.CancelPenalty > 0)
                                    {
                                        var factory = new DemandFactory(new IspEntries(context));
                                        var demand = factory.CreateDemand(workOrder, now, now.AddMonths(1),
                                            workOrder.Offer.CancelPenalty,
                                            Convert.ToInt32(Session["User_ID"]), notes: "غرامة الكانسل خلال السنة الأولى",
                                            isCommesstion: false);
                                        context.Demands.InsertOnSubmit(demand);
                                        context.SubmitChanges();
                                    }

                                }
                            }
                            else if (workOrder.CreationDate != null && workOrder.Offer == null && activationDate != null)
                            {
                                var offerid = context.Demands.Where(a => a.WorkOrderId == workOrder.ID && a.OfferId != null)
                                    .OrderByDescending(s => s.EndAt)
                                    .Select(o => o.OfferId).FirstOrDefault();

                                if (offerid != null)
                                {
                                    var penalty =
                                        context.Offers.Where(s => s.Id == offerid)
                                            .Select(f => f.CancelPenalty)
                                            .FirstOrDefault();
                                    if (penalty > 0)
                                    {
                                        var factory = new DemandFactory(new IspEntries(context));
                                        var demand = factory.CreateDemand(workOrder, now, now.AddMonths(1),
                                            penalty,
                                            Convert.ToInt32(Session["User_ID"]), notes: "غرامة الكانسل خلال السنة الأولى",
                                            isCommesstion: false);
                                        context.Demands.InsertOnSubmit(demand);
                                        context.SubmitChanges();
                                    }
                                }
                            }


                            if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                            {
                                CenterMessage.SendRequestApproval(workOrder, Tokens.Cancel, userId);
                            }
                            SendSms(context, workOrder, 14);
                        }
                    }
                    break;
                //Reactivate
                case 7:
                    {
                        var hour3 = DateTime.Now.AddHours();
                        var updateDate =
                            Convert.ToDateTime(TbReactiveDate.Text).AddHours(hour3.Hour).AddMinutes(hour3.Minute);



                        request.ProcessDate = updateDate;
                        if (workOrder.OfferStart == null) workOrder.OfferStart = updateDate;

                        if (RbToNew.Checked)
                        {
                            workOrder.WorkOrderStatusID = 1;
                            workOrder.Installed = false;
                            workOrder.Installer = null;
                            workOrder.InstallationTime = null;
                            // delete from recive router by worid
                            var rt = workOrder.RecieveRouters.FirstOrDefault(x => x.WorkOrderId == workOrder.ID);
                            if (rt != null)
                            {
                                context.RecieveRouters.DeleteOnSubmit(rt);
                            }
                            if (ddlServiceProvider.SelectedIndex > 0)
                            {
                                workOrder.ServiceProviderID = Convert.ToInt32(ddlServiceProvider.SelectedItem.Value);
                            }

                            if (DdlPackage.SelectedIndex > 0)
                            {
                                workOrder.ServicePackageID = Convert.ToInt32(DdlPackage.SelectedItem.Value);
                            }
                            if (DdlOffer.SelectedIndex > 0)
                            {
                                workOrder.OfferId = Convert.ToInt32(DdlOffer.SelectedItem.Value);
                            }
                            if (DdlIpPackage.SelectedIndex > 0)
                            {
                                workOrder.IpPackageID = Convert.ToInt32(DdlIpPackage.SelectedItem.Value);
                            }
                        }
                        context.SubmitChanges();
                        if (RbToActive.Checked)
                        {
                            if (ddlServiceProvider.SelectedIndex > 0)
                            {
                                workOrder.ServiceProviderID = Convert.ToInt32(ddlServiceProvider.SelectedItem.Value);
                            }
                            if (DdlPackage.SelectedIndex > 0)
                            {
                                workOrder.ServicePackageID = Convert.ToInt32(DdlPackage.SelectedItem.Value);
                            }
                            if (DdlOffer.SelectedIndex > 0)
                            {
                                workOrder.OfferId = Convert.ToInt32(DdlOffer.SelectedItem.Value);
                                workOrder.OfferStart = updateDate.Date;
                            }
                            if (DdlIpPackage.SelectedIndex > 0)
                            {
                                workOrder.IpPackageID = Convert.ToInt32(DdlIpPackage.SelectedItem.Value);
                            }
                            context.SubmitChanges();
                            var factory = new DemandFactory(new IspEntries(context));
                            var pricing =
                                workOrder.ServicePackage.Pricings
                                    .FirstOrDefault(x => x.ServiceProvidersID == workOrder.ServiceProviderID);

                            if (pricing != null)
                            {
                                var notes = string.Empty;
                                var amount = Convert.ToDecimal(pricing.Price);
                                var basicBill = amount;
                                var index = RblDemand.SelectedIndex;
                                var addMonths = new DateTime();
                                if (request.WorkOrder.ResellerID != null)
                                {

                                    var discoption = OptionsService.GetOptions(context, true);
                                    if (discoption != null && Convert.ToBoolean(discoption.IntallationDiscound))
                                    {
                                        var firstOrDefault =
                                            context.ResellerPackagesDiscounts.FirstOrDefault(
                                                r =>
                                                    r.ResellerId == request.WorkOrder.ResellerID &&
                                                    r.ProviderId == request.WorkOrder.ServiceProviderID &&
                                                    r.PackageId == request.WorkOrder.ServicePackageID);
                                        var discount = firstOrDefault != null ? firstOrDefault.Discount : 0;
                                        //added by ashraf to get net discount
                                        var netdscount = amount * discount / 100;
                                        var amountfordisc = amount - netdscount;
                                        var result = _creditRepository.Save(Convert.ToInt32(request.WorkOrder.ResellerID),
                                            userId, Convert.ToDecimal(amountfordisc * -1),
                                            request.WorkOrder.CustomerName + " - " + request.WorkOrder.CustomerPhone,
                                            DateTime.Now.AddHours());
                                    }
                                }

                                switch (index)
                                {
                                    case 0:
                                        //فاتورة كاملة 
                                        addMonths = updateDate.AddMonths(1);
                                        break;
                                    case 1:
                                        //تصفية الى بداية الشهر
                                        var nextmonth = updateDate.AddMonths(1);

                                        addMonths = new DateTime(updateDate.Year, nextmonth.Month, 1);
                                        const int daysInMonth = 30;
                                        var spentDays = (30 - (updateDate.Date.Day)) + 1;
                                        amount = amount * Convert.ToDecimal(spentDays) / Convert.ToDecimal(daysInMonth);
                                        break;
                                }
                                if (workOrder.Offer != null)
                                {
                                    var offer = workOrder.Offer;
                                    amount = amount - OfferPricingServices.GetOfferPrice(offer, amount, basicBill);
                                    if (offer != null && offer.RouterCost > 0)
                                    {
                                        var routerDemand = factory.CreateDemand(workOrder, updateDate,
                                            addMonths, offer.RouterCost, Convert.ToInt32(Session["User_ID"]),
                                            notes: "قيمة الروتر"
                                            , isCommesstion: false);
                                        context.Demands.InsertOnSubmit(routerDemand);
                                    }
                                }

                                if (DdlIpPackage.SelectedIndex > 0)
                                {
                                    workOrder.IpPackageID = Convert.ToInt32(DdlIpPackage.SelectedItem.Value);

                                    var ipPackage =
                                        context.IpPackages.FirstOrDefault(
                                            x => x.ID == Convert.ToInt32(DdlIpPackage.SelectedItem.Value)); //_context
                                    if (ipPackage != null)
                                    {
                                        var ipPackageName = ipPackage.IpPackageName;
                                        var ipNotes = string.Format("{0}: {1}", Tokens.IpPackages, ipPackageName);
                                        var ipPackageDemand =
                                            factory.CreateDemand(workOrder, updateDate,
                                                addMonths,
                                                Convert.ToInt32(ipPackage.IpPackageName) * 10,
                                                Convert.ToInt32(Session["User_ID"]), notes: ipNotes, isCommesstion: false);
                                        context.Demands.InsertOnSubmit(ipPackageDemand);
                                    }
                                }
                                amount = amount + Convert.ToDecimal(workOrder.PaymentType.Amount);
                                notes += "مطالبة جديدة بعد اعادة التشغيل";
                                var demand = factory.CreateDemand(workOrder, updateDate,
                                    addMonths, amount,
                                    Convert.ToInt32(Session["User_ID"]), notes: notes);

                                context.Demands.InsertOnSubmit(demand);
                                workOrder.RequestDate = addMonths;

                            }
                            workOrder.WorkOrderStatusID = 6;
                        }
                        context.SubmitChanges();

                        var wos = new global::Db.WorkOrderStatus
                        {
                            WorkOrderID = workOrder.ID,
                            StatusID = workOrder.WorkOrderStatusID,
                            UserID = Convert.ToInt32(Session["User_ID"]),
                            UpdateDate = updateDate
                        };
                        context.WorkOrderStatus.InsertOnSubmit(wos);
                        context.SubmitChanges();
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, "Reactivate", userId);
                        }
                    }
                    break;
                //Change IP Package
                case 8:

                    var hour4 = DateTime.Now.AddHours();
                    var ipnow = Convert.ToDateTime(TbMiscDate.Text).AddHours(hour4.Hour).AddMinutes(hour4.Minute);


                    request.ProcessDate = ipnow;

                    workOrder.IpPackageID = request.NewIpPackageID;
                    context.SubmitChanges();
                    var package = context.IpPackages.FirstOrDefault(x => x.ID == request.NewIpPackageID); //_context
                    if (package != null)
                    {
                        var ipPackageName = package.IpPackageName;
                        var ipNotes = string.Format("{0}: {1}", Tokens.IpPackages, ipPackageName);

                        demandService.AddDemandForWorkOrderService(ipnow, workOrder.ID, ipNotes,
                            Convert.ToDecimal(Convert.ToDecimal(ipPackageName) * 10), Convert.ToInt32(Session["User_ID"]),
                            false,
                            false);
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, Tokens.MenuChangeIPPackage, userId);
                        }
                        SendSms(context, workOrder, 16);
                    }
                    break;
                //Request Extra Giga
                case 9:
                    {
                        var hour5 = DateTime.Now.AddHours();
                        var giganow = Convert.ToDateTime(TbMiscDate.Text).AddHours(hour5.Hour).AddMinutes(hour5.Minute);


                        request.ProcessDate = giganow;
                        var updatedwo = workOrder;
                        if (request.ExtraGiga == null) return;
                        updatedwo.ExtraGiga = request.ExtraGiga;
                        context.SubmitChanges();
                        var notes = string.Format("{0}: {1}", Tokens.ExtraGigas, request.ExtraGiga.Name);
                        demandService.AddDemandForWorkOrderService(giganow, workOrder.ID, notes,
                            Convert.ToDecimal(request.ExtraGiga.Price), Convert.ToInt32(Session["User_ID"]), false, false);
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestApproval(workOrder, Tokens.ExtraGigas, userId);
                        }
                        SendSms(context, workOrder, 17);
                    }
                    break;

            }
        }

        private void SendSms(ISPDataContext context, WorkOrder order, int indexnotification)
        {
            // send sms by Ahmed Saied
            try
            {
                var mobile = order.CustomerMobile;
                if (!string.IsNullOrEmpty(mobile))
                {
                    var message = global::SendSms.SendSmsByNotification(context, mobile, indexnotification);
                    if (!string.IsNullOrEmpty(message))
                    {
                        var myscript = "window.open('" + message + "')";
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "myscript", myscript, true);
                    }
                }
            }
            catch
            {

                lbl_ProcessResult.Text = Tokens.ErrorMsg;
                lbl_ProcessResult.ForeColor = Color.Red;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HandlePrivildges();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            direction.InnerHtml = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "1" : "2";
            if (Session["User_ID"] == null)
            {
                Response.Redirect("default.aspx");
                return;
            }


            ProcessQuery();
            if (!IsPostBack)
            {
                HandleShowAndHideElements();
                PopulateDdlPaymentType();
                CheckGroupIdAndBindGrid();

                _domian.PopulateProviders(ddlServiceProvider, Tokens.WithOut);

                _domian.PopulateIpPackages(DdlIpPackage, Tokens.WithOut);
                Div1.Visible = false;
            }
        }

        private void HandleShowAndHideElements()
        {
            PostponeSuspendDays.Visible = false;
            DeductionWithFixedRequestDate.Visible = false;
            ModalBtnApprove.Visible = false;

            BtnSave.Visible = false;
            BtnSaveWithLiquidation.Visible = false;
            foreach (ListItem pr in RblUpDwonOptions.Items)
            {
                //pr.Attributes.CssStyle.Add("visibility", "hidden");
                pr.Attributes.CssStyle.Add("display", "none");
            }
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var pList = context.ShowPendingRequestsOptions.ToList();
                if (pList.Count > 0)
                {
                    foreach (var lst in pList)
                    {
                         foreach (ListItem pr in RblUpDwonOptions.Items)
                        {
                            if (lst.RequestType == "updown" && lst.Name == pr.Value)
                            {
                                //pr.Attributes.CssStyle.Remove("visibility");
                                pr.Attributes.CssStyle.Remove("display");
                            }
                        }
                         if (lst.RequestType == "unsus" && (lst.Name == "ترحيل ايام السسبند" || lst.Name == Tokens.PostponeSuspendDays))
                         {
                             PostponeSuspendDays.Visible=true;
                         }
                         if (lst.RequestType == "unsus" && (lst.Name == "تخصيم مع ثبات تاريخ المطالبة" || lst.Name == Tokens.DeductionWithFixedRequestDate))
                         {
                             DeductionWithFixedRequestDate.Visible = true;
                         }
                         if (lst.RequestType == "unsus" && (lst.Name == "فاتورة كاملة" || lst.Name == Tokens.CompleteInvoice))
                         {
                             ModalBtnApprove.Visible = true;
                         }
                         if (lst.RequestType == "sus" && (lst.Name == "تاكيد" || lst.Name == Tokens.Confirm))
                         {
                             BtnSave.Visible = true;
                         }
                         if (lst.RequestType == "sus" && (lst.Name == "تأكيد مع التصفية" || lst.Name == Tokens.ConfirmWithLiquidation))
                         {
                             BtnSaveWithLiquidation.Visible = true;
                         }
                    }
                }
            }
        }

        private void HandlePrivildges()
        {
            if (Session["User_ID"] == null) return;
            var userId = Convert.ToInt32(Session["User_ID"]);
            bool changeRequestDate = _ispEntries.UserHasPrivlidge(userId, "EditPendingRequestDate");
            if (changeRequestDate)
            {
                //TbMiscDate.ReadOnly = false;
                EditRequestDate.Value = "0";
            }
            else
            {
                //TbMiscDate.ReadOnly = true;
                EditRequestDate.Value = "1";
            }
            
        }

        private void PopulateDdlPaymentType()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var payType = context.PaymentTypes.ToList();
                if (payType.Count > 0)
                {
                    ddlPayType.DataSource = payType;
                    ddlPayType.DataValueField = "ID";
                    ddlPayType.DataTextField = "PaymentTypeName";
                    ddlPayType.DataBind();
                    Helper.AddDefaultItem(ddlPayType);
                }
            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_Requests.PageIndex = e.NewPageIndex;
            CheckGroupIdAndBindGrid();
        }

        private void PopulateSuspendDays(IEnumerable<ManageRequestTemplate> requests)
        {
            var ispEntries = new IspEntries();
            var req = requests.OrderBy(a => a.SuspenDaysCount).ToList();
            var numbers = ispEntries.ListOfDaysForCustomerAtStatus(11, req);
            ddlSuspendCount.DataSource = numbers;
            ddlSuspendCount.DataTextField = "Value";
            ddlSuspendCount.DataValueField = "Value";
            ddlSuspendCount.DataBind();
            Helper.AddDefaultItem(ddlSuspendCount);
        }

        protected void ddlSuspendDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (ddlSuspendCount.SelectedIndex > 0)
                {
                    var dayId = Convert.ToInt32(ddlSuspendCount.SelectedItem.Value);
                    var requestList = (List<ManageRequestTemplate>)Session["Requests"];
                    var filter = requestList.Where(a => CountSuspenddays(a.WorkOrderID) == dayId).ToList();
                    grd_Requests.DataSource = filter;
                    grd_Requests.DataBind();
                    var groupIdQuery = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                    if (groupIdQuery == null) return;
                    var id = groupIdQuery.Group.DataLevelID; //.GroupID;
                    if (id == null) return;
                    var groupId = id.Value;
                    if (string.IsNullOrEmpty(Request.QueryString["rid"]))
                    {
                        grd_Requests.Visible = false;
                    }
                    else
                    {
                        var que = Request.QueryString["rid"];
                        var deid = QueryStringSecurity.Decrypt(que);
                        var idInQueryString = Convert.ToInt32(deid);
                        var columns = grd_Requests.Columns;
                        GridHelper.HideAllColumns(columns);
                        var columnNames = new List<string>
                        {
                            "#",
                            Tokens.Customer,
                            Tokens.Phone,
                            Tokens.Governrate,
                            Tokens.Central,
                            Tokens.CurrentSpeed,
                            Tokens.Status,
                            Tokens.Provider,
                            Tokens.Reseller,
                            Tokens.Branch,
                            Tokens.SenderName,
                            Tokens.Activation_Date,
                            Tokens.Offer,
                            Tokens.Request_Date,
                            Tokens.PaymentType,
                            Tokens.Notes,
                             Tokens.InvoiceDueDate,
                            Tokens.isprorequest
                        };
                        switch (idInQueryString)
                        {
                            // Upgrade-Downgrade
                            case 1:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.New_Service_Package,
                                    Tokens.CurrentSpeed
                                });
                                break;
                            case 3:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.SuspendDaysCount
                                });
                                break;
                            // Ip Package
                            case 8:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.New_IP_Package
                                });
                                break;

                            case 9:
                                columnNames.AddRange(new List<string>
                                {
                                    Tokens.Extra_Gigas
                                });
                                break;
                        }

                        //sys admin, sys employee

                        if (groupId == 1)
                        {

                            columnNames.AddRange(new List<string>
                            {
                                Tokens.Approve,
                                Tokens.Reject,
                                Tokens.Select
                            });

                            tbl_Control.Visible = filter.Count > 0;

                        }
                        else
                        {
                            tbl_Control.Visible =
                                provrequestall.Visible =
                                    btn_ApproveSelected.Visible = btn_RejectSelected.Visible = false;
                        }
                        GridHelper.ShowExactColumns(columns, columnNames);
                    }
                }
            }
        }

        protected int CountSuspenddays(int workorderId)
        {
            var ispEntries = new IspEntries();
            return ispEntries.DaysForCustomerAtStatus(workorderId, 11);
        }


        private void ProcessQuery()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (string.IsNullOrEmpty(Request.QueryString["rid"]))
                {
                    Response.Redirect("ErrorPage.aspx");
                    return;
                }
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestId = Convert.ToString(que);
                HfIsUnsuspend.Value = requestId;
                ViewState.Add("requestId", requestId);
                if (requestId == "3") suspendStatus.Visible = true;
                var requests = context.Requests
                    .Where(req => req.ID == Convert.ToInt32(requestId))
                    .Select(req => req.RequestName);
                var user = context.Users.FirstOrDefault(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                if (user == null)
                {
                    Response.Redirect("ErrorPage.aspx");
                    return;
                }
                var id = user.GroupID;
                if (id == null)
                {
                    Response.Redirect("ErrorPage.aspx");
                    return;
                }
                var groupId = id.Value;
                if (!IsPostBack)
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    bool changeRequestDate = _ispEntries.UserHasPrivlidge(userId, "EditPendingRequestDate");

                    ReactivateDateBox.Visible =
                        MiscTbContainer.Visible =
                            UnsesuspendContainer.Visible =
                                containerForUpDown.Visible = containerForCancel.Visible = changeRequestDate;
                    TbReactiveDate.Text =
                        TbUpDwonDate.Text =
                            TbUnsuspendDate.Text =
                                TbMiscDate.Text = TbCancelDate.Text = DateTime.Now.AddHours().ToShortDateString();
                }
                var privilegeQuery =
                    context.GroupPrivileges.Where(gp => gp.Group.ID == groupId).Select(gp => gp.privilege.Name);
                if (!(privilegeQuery.Contains(requests.FirstOrDefault()) || privilegeQuery.Contains("All")))
                {
                    Response.Redirect("UnAuthorized.aspx");
                    return;
                }
                lbl_Gridlabel.Text = requests.FirstOrDefault() + Tokens.Requests;
            }
        }


        protected void btn_ApproveSelected_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ClickedBtn.Value = (RBLUnsuspend.SelectedIndex + 1).ToString(CultureInfo.InvariantCulture);
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);
                foreach (GridViewRow row in grd_Requests.Rows)
                {
                    var checkBox = row.FindControl("SelectItem") as CheckBox;
                    if (checkBox == null || !checkBox.Checked) continue;
                    var dataKey = grd_Requests.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var workOrderRequestId = Convert.ToInt32(dataKey["ID"]);
                    var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                    if (request == null) continue;
                    request.RSID = 1;
                    var confirmerId = Convert.ToInt32(Session["User_ID"]);
                    request.ConfirmerID = confirmerId;

                    var workOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == request.WorkOrderID);
                    HandleRequest(requestTypeId, workOrder, request, context, confirmerId);
                    context.SubmitChanges();
                    try
                    {
                        var requ = context.Requests.FirstOrDefault(r => r.ID == requestTypeId);
                        if (requ != null)
                        {
                            NotifyUserByProcess(workOrder, requ, context);
                        }
                    }
                    catch
                    {
                        lbl_ProcessResult.Text = Tokens.ErrorMsg;
                        lbl_ProcessResult.ForeColor = Color.Red;
                    }

                }
                CheckGroupIdAndBindGrid();
                lbl_ProcessResult.Text = Tokens.SelectedRequestsApproved;
                lbl_ProcessResult.ForeColor = Color.Green;
                HttpContext.Current.Response.Redirect(Request.RawUrl);

            }
        }





        protected void btn_SuspendSelected_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                foreach (GridViewRow row in grd_Requests.Rows)
                {
                    var checkBox = row.FindControl("SelectItem") as CheckBox;
                    if (checkBox == null || !checkBox.Checked) continue;
                    var dataKey = grd_Requests.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var workOrderRequestId = Convert.ToInt32(dataKey["ID"]);
                    var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                    if (request == null) continue;
                    var orderId = Convert.ToInt32(request.WorkOrderID);
                    var requestId = Convert.ToInt32(request.ID);
                    ConfirmSuspend(requestId, orderId);
                    context.SubmitChanges();
                }
                CheckGroupIdAndBindGrid();
                HttpContext.Current.Response.Redirect(Request.RawUrl);
            }
        }

        protected void btn_SuspendwithLiquedationSelected_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

                foreach (GridViewRow row in grd_Requests.Rows)
                {
                    var checkBox = row.FindControl("SelectItem") as CheckBox;
                    if (checkBox == null || !checkBox.Checked) continue;
                    var dataKey = grd_Requests.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var workOrderRequestId = Convert.ToInt32(dataKey["ID"]);
                    var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                    if (request == null) continue;
                    var orderId = Convert.ToInt32(request.WorkOrderID);
                    var requestId = Convert.ToInt32(request.ID);
                    ConfirmSuspend(requestId, orderId);
                    var date = Convert.ToDateTime(TbMiscDate.Text);
                    Liquidation(orderId, date);
                    context.SubmitChanges();
                }
                CheckGroupIdAndBindGrid();
                HttpContext.Current.Response.Redirect(Request.RawUrl);
            }
        }

        protected void CancelProcess(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);
                var dataKey = UnsuspendId.Value;
                var workOrderRequestId = Convert.ToInt32(dataKey);
                var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                if (request == null) return;
                request.RSID = 1;
                var confirmerId = Convert.ToInt32(Session["User_ID"]);
                request.ConfirmerID = confirmerId;
                context.SubmitChanges();
                var workOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == request.WorkOrderID);
                HandleRequest(requestTypeId, workOrder, request, context, confirmerId);
                try
                {
                    var requ = context.Requests.FirstOrDefault(r => r.ID == requestTypeId);
                    if (requ != null)
                    {
                        NotifyUserByProcess(workOrder, requ, context);
                    }
                }
                catch
                {
                    lbl_ProcessResult.Text = Tokens.ErrorMsg;
                    lbl_ProcessResult.ForeColor = Color.Red;
                }
                lbl_ProcessResult.Text = Tokens.SelectedRequestsApproved;
                lbl_ProcessResult.ForeColor = Color.Green;
                CheckGroupIdAndBindGrid();
                HttpContext.Current.Response.Redirect(Request.RawUrl);
            }
        }

        //فحص اذا كان العميل مالوش فواتير قبل كدة او تاريخ التنفيذ بعد تاريخ المطالبة
        protected bool CheckDemand(WorkOrder order, WorkOrderRequest request, IspEntries context, DateTime date)
        {
            
            var daysinmonth = DateTime.DaysInMonth(date.Year, date.Month);

            var lastday = new DateTime(date.Year, date.Month, daysinmonth);
            var ispEntries = new IspEntries();
            var packBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month,
                date.Year, Convert.ToInt32(order.ServicePackageID));
            
           if (order.OfferStart != null && order.Offer != null)
            {
                Offer offer = order.Offer;
                DateTime offerEndDate = order.OfferStart.Value.AddMonths(offer.LifeTime);
                //DateTime plusMonth = startAt.AddMonths(1);
                if (date.Date >= order.OfferStart && date.Date < offerEndDate.Date)
                {
                    packBill.Net = packBill.Net -
                              OfferPricingServices.GetOfferPrice(order.Offer, packBill.Net, packBill.Net);
                }
                else
                {
                    order.Offer = null;
                }
            }
        
            var userId = Convert.ToInt32(Session["User_ID"]);

            var dem = ispEntries.OrderDemand2(order.ID, date).ToList();
            var demand2 = dem.LastOrDefault();
            if (demand2 == null)
            {
                if (Convert.ToInt32(ClickedBtn.Value) == 1)
                {
                    if (!string.IsNullOrEmpty(TbDaysCount.Text))
                    {
                        var val = Convert.ToDouble(TbDaysCount.Text);
                        var curDate = order.RequestDate.Value;
                        order.RequestDate = curDate.AddDays(val);
                        return true;
                    }
                }
                else
                {
                    AddNewDemand(context, order, date, lastday, packBill, ispEntries, userId);
                    return true;
                }
            }
            if (request.ProcessDate > demand2.EndAt)
            {
                if (Convert.ToInt32(ClickedBtn.Value) == 1)
                {
                    if (!string.IsNullOrEmpty(TbDaysCount.Text))
                    {
                        var val = Convert.ToDouble(TbDaysCount.Text);
                        var curDate = order.RequestDate.Value;
                        order.RequestDate = curDate.AddDays(val);
                        return true;
                    }
                }
                else
                {
                    AddNewDemand(context, order, date, lastday, packBill, ispEntries, userId);
                    return true;
                }
            }
            if (request.ProcessDate <= demand2.EndAt)
            {
                return false;
            }
            return false;
        }

        private static void AddNewDemand(IspEntries context, WorkOrder order, DateTime date, DateTime lastDay,
            BillDetails packageBill, IspEntries ispEntries, int userId)
        {
            var demandFactory = new DemandFactory(context);
            var paymentTypeAmount = Convert.ToDecimal(order.PaymentType.Amount);
            if (order.RequestDate != null && order.RequestDate.Value.Day == 1)
            {
                var newdatemonth = date.AddMonths(1);
                var newenddate = new DateTime(newdatemonth.Year, newdatemonth.Month, 1);
                var period = (lastDay.Date - date.Date).Days + 1;
                var periodPercent = Convert.ToDecimal(period) / Convert.ToDecimal(30);
                var amount = packageBill.Net * periodPercent;
                amount += paymentTypeAmount;
                const string notes =
                    "  اضافة مطالبة جديدة من اول الشهر لمستخدم لم يكن لها مطالبات قبل ذلك و تاريخ المطالبة كان فى اول الشهر";
                var newdemand = demandFactory.CreateDemand(order, date, newenddate, amount, userId, notes: notes);
                order.RequestDate = newenddate;
                ispEntries.AddDemand(newdemand);
            }
            else if (order.RequestDate != null && order.RequestDate.Value.Day > 1)
            {
                var enddate = date.AddMonths(1);
                var amount2 = packageBill.Net + paymentTypeAmount;
                const string notes =
                    " اضافة مطالبة جديدة لمستخدم لم يكن له مطالبات قبل ذلك وتاريخ المطالبة كان خلال الشهر";
                var newdemand = demandFactory.CreateDemand(order, date, enddate, amount2, userId, notes: notes);
                //date.AddMonths(1)
                order.RequestDate = enddate;
                ispEntries.AddDemand(newdemand);
            }
        }

        protected void lnb_Approve_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var requestId = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == requestId);
                var orderId = Convert.ToInt32(((LinkButton)sender).ValidationGroup);
                var order = context.WorkOrders.FirstOrDefault(wo => wo.ID == orderId);
                if (order == null || request == null) return;
                request.RSID = 1;
                request.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                context.SubmitChanges();
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);

                HandleRequest(requestTypeId, order, request, context, Convert.ToInt32(Session["User_ID"]));
                try
                {
                    var query = context.WorkOrders.Where(wo => wo.ID == request.WorkOrderID);
                    var updatedwo = query.FirstOrDefault();
                    var requ = context.Requests.FirstOrDefault(r => r.ID == requestTypeId);
                    if (requ != null)
                    {
                        NotifyUserByProcess(updatedwo, requ, context);
                    }
                }
                catch
                {
                    lbl_ProcessResult.Text = Tokens.ErrorMsg;
                    lbl_ProcessResult.ForeColor = Color.Red;
                }
                lbl_ProcessResult.Text = Tokens.RequestAdded;
                lbl_ProcessResult.ForeColor = Color.Green;
                CheckGroupIdAndBindGrid();
                HttpContext.Current.Response.Redirect(Request.RawUrl);
            }
        }

        public void PeoviderRequestSent(object sender, EventArgs eventArgs)
        {
            var obj = (LinkButton)sender;
            int id = Convert.ToInt32(obj.CommandArgument);
            HandleProviderRequest(id);
            CheckGroupIdAndBindGrid();
        }

        private void HandleProviderRequest(int id)
        {
            using (var db8 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {

               
                var wor = db8.WorkOrderRequests.FirstOrDefault(x => x.ID == id);

                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);
                switch (requestTypeId)
                {
                    //Suspend
                    case 2:
                        {
                            var portalList = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                            var woproviderList = db8.WorkOrders.FirstOrDefault(z => z.ID == wor.WorkOrderID);
                            if (woproviderList != null && portalList.Contains(woproviderList.ServiceProviderID))
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
                                                Div1.Visible = true;
                                                Div1.InnerHtml = "هذا العميل موقوف بالفعل على البورتال";
                                                Div1.Attributes.Add("class", "alert alert-danger");
                                                return;
                                            }
                                            else
                                            {
                                                var worNote = Tedata.SendTedataSuspendRequest(username, cookiecon, pagetext);
                                                if (worNote == 2)
                                                {
                                                    //فى حالة البورتال واقع
                                                    Div1.Visible = true;
                                                    Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                                    Div1.Attributes.Add("class", "alert alert-danger");

                                                    //ينزل الطلب معلق فى اى اس بى
                                                }
                                                else
                                                {
                                                    //فى حالة نجاح الارسال الى البورتال ننزل الطلب متوافق علية فى اى اس بى

                                                    wor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                                    wor.ProcessDate = DateTime.Now.AddHours();
                                                    wor.RSID = 1;
                                                    wor.IsProviderRequest = false;
                                                    db8.SubmitChanges();

                                                    //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                    var current = db8.WorkOrders.FirstOrDefault(x => x.ID == wor.WorkOrderID);

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
                                                        db8.WorkOrderStatus.InsertOnSubmit(wos);
                                                        db8.SubmitChanges();
                                                    }



                                                    Div1.Visible = true;
                                                    Div1.InnerHtml = "تم إرسال الطلب الى البورتال بنجاح";
                                                    Div1.Attributes.Add("class", "alert alert-success");

                                                    return;
                                                }

                                            }

                                        }
                                        else
                                        {
                                            //فى حالة البورتال واقع
                                            Div1.Visible = true;
                                            Div1.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                            Div1.Attributes.Add("class", "alert alert-danger");

                                        }

                                    }
                                    else
                                    {
                                        Div1.Visible = true;
                                        Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                        Div1.Attributes.Add("class", "alert alert-danger");

                                    }
                                }
                                else
                                {
                                    Div1.Visible = true;
                                    Div1.InnerHtml =
                                        "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                    Div1.Attributes.Add("class", "alert alert-danger");

                                }
                            }
                            else
                            {
                                wor.IsProviderRequest = false;
                                db8.SubmitChanges();
                            }

                            break;
                        }
                    //Unsuspend
                    case 3:
                        {
                            var portalList2 = db8.OptionPortalProviders.Select(z => z.PortalProvidersId).ToList();
                            var woproviderList2 = db8.WorkOrders.FirstOrDefault(z => z.ID == wor.WorkOrderID);
                            if (woproviderList2 != null && portalList2.Contains(woproviderList2.ServiceProviderID))
                            {

                                var username = woproviderList2.UserName;
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
                                            if (custStatus == "enable")
                                            {
                                                Div1.Visible = true;
                                                Div1.InnerHtml = "هذا العميل مفعل بالفعل على البورتال";
                                                Div1.Attributes.Add("class", "alert alert-danger");
                                                return;
                                            }
                                            else
                                            {
                                                var worNote = Tedata.SendTedataUnSuspendRequest(username, cookiecon,
                                                    pagetext);
                                                if (worNote == 2)
                                                {
                                                    Div1.Visible = true;
                                                    Div1.InnerHtml = "تعذر الوصول الى البورتال";
                                                    Div1.Attributes.Add("class", "alert alert-danger");
                                                    //فى حالة البورتال واقع
                                                    //ينزل الطلب معلق فى اى اس بى
                                                }
                                                else
                                                {
                                                    //ينزل الطلب متوافق علية فى اى اس بى

                                                    wor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                                                    wor.ProcessDate = DateTime.Now.AddHours();
                                                    wor.RSID = 1;
                                                    wor.IsProviderRequest = false;
                                                    db8.SubmitChanges();

                                                    //تغيير الحالة الى(WorkOrders,WorkOrderStatus) suspend
                                                    var current = db8.WorkOrders.FirstOrDefault(x => x.ID == wor.WorkOrderID);

                                                    if (current != null)
                                                    {
                                                        current.WorkOrderStatusID = 6;

                                                        global::Db.WorkOrderStatus wos = new global::Db.WorkOrderStatus
                                                        {
                                                            WorkOrderID = current.ID,
                                                            StatusID = 6,
                                                            UserID = 1,
                                                            UpdateDate = DateTime.Now.AddHours(),
                                                        };
                                                        db8.WorkOrderStatus.InsertOnSubmit(wos);
                                                        db8.SubmitChanges();
                                                    }


                                                    // ترحيل ايام السسبند
                                                    int daysCount = _ispEntries.DaysForCustomerAtStatus(wor.ID, 11);
                                                    var option = OptionsService.GetOptions(db8, true);
                                                    if (option != null && option.PortalRelayDays != null && daysCount > option.PortalRelayDays)
                                                    {
                                                        wor.RequestDate.Value.AddDays(daysCount);
                                                        db8.SubmitChanges();
                                                        _ispEntries.Commit();
                                                    }

                                                    Div1.Visible = true;
                                                    Div1.InnerHtml = "تم إرسال الطلب الى البورتال بنجاح";
                                                    Div1.Attributes.Add("class", "alert alert-success");
                                                    return;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //فى حالة البورتال واقع
                                            Div1.Visible = true;
                                            Div1.InnerHtml = "تعذر الوصول الى البورتال الرجاء تأكد من حقل (User Name)";
                                            Div1.Attributes.Add("class", "alert alert-danger");

                                        }

                                    }
                                    else
                                    {
                                        //فى حالة البورتال واقع
                                        Div1.Visible = true;
                                        Div1.InnerHtml = "فشل الأتصال بالبورتال";
                                        Div1.Attributes.Add("class", "alert alert-danger");

                                    }
                                }
                                else
                                {
                                    Div1.Visible = true;
                                    Div1.InnerHtml =
                                        "فشل الأتصال بالسيرفر رجاءً تأكد من Portal User Name or Portal Password";
                                    Div1.Attributes.Add("class", "alert alert-danger");
                                    //فى حالة البورتال واقع
                                    //ينزل الطلب معلق فى اى اس بى 
                                }
                            }
                            else
                            {
                                wor.IsProviderRequest = false;
                                db8.SubmitChanges();
                            }

                            break;
                        }
                    default:
                        {
                            wor.IsProviderRequest = false;
                            db8.SubmitChanges();
                            break;
                        }
                }

               
            }
        }

        private void CheckGroupIdAndBindGrid()
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {


                PopulateOffers(context);
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);

                var woQuery = DataLevelClass.GetUserNonCofirmedWoRequests(Convert.ToInt32(que));



                hdnreq.Value = que;
                ViewState["No"] = null;

                grd_Requests.DataSource = woQuery;
                grd_Requests.DataBind();
                var groupIdQuery = context.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                var gId = groupIdQuery.FirstOrDefault();
                if (gId == null) return;
                var id = gId.Group.DataLevelID;
                if (id == null) return;
                var groupId = id.Value;
                if (string.IsNullOrEmpty(Request.QueryString["rid"]))
                {
                    grd_Requests.Visible = false;
                }
                else
                {
                    var que2 = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                    var idInQueryString = Convert.ToInt32(que2);
                    var columns = grd_Requests.Columns;
                    GridHelper.HideAllColumns(columns);
                    var columnNames = new List<string>
                    {
                        "#",
                        Tokens.Customer,
                        Tokens.Phone,
                        Tokens.Governrate,
                        Tokens.Central,
                        Tokens.CurrentSpeed,
                        Tokens.Status,
                        Tokens.Provider,
                        Tokens.Reseller,
                        Tokens.Branch,
                        Tokens.SenderName,
                        Tokens.Activation_Date,
                        Tokens.Offer,
                        Tokens.Request_Date,
                        Tokens.PaymentType,
                        Tokens.UserName,
                        Tokens.Notes,
                        Tokens.InvoiceDueDate

                    };
                    switch (idInQueryString)
                    {
                        // Upgrade-Downgrade
                        case 1:
                            columnNames.AddRange(new List<string>
                            {
                                Tokens.New_Service_Package,
                                Tokens.CurrentSpeed
                            });
                            break;
                        case 3:
                            columnNames.AddRange(new List<string>
                            {
                                Tokens.SuspendDaysCount
                            });
                            break;
                        // Ip Package
                        case 8:
                            columnNames.AddRange(new List<string>
                            {
                                Tokens.New_IP_Package
                            });
                            break;

                        case 9:
                            columnNames.AddRange(new List<string>
                            {
                                Tokens.Extra_Gigas
                            });
                            break;
                    }





                    //sys admin, sys employee

                    if (groupId == 1)
                    {
                        //|| groupId == 2){
                        columnNames.AddRange(new List<string>
                        {
                            Tokens.Approve,
                            Tokens.Reject,
                            Tokens.Select,
                            Tokens.isprorequest
                        });

                        tbl_Control.Visible = woQuery.Count > 0;
                        //btn_ApproveSelected.Visible = btn_RejectSelected.Visible = true;
                    }
                    else
                    {
                        tbl_Control.Visible =
                            provrequestall.Visible = btn_ApproveSelected.Visible = btn_RejectSelected.Visible = false;
                    }
                    GridHelper.ShowExactColumns(columns, columnNames);
                    Session["AllRequests"] = woQuery;
                    if (ddlSuspendCount.Visible)
                    {
                        PopulateSuspendDays(woQuery);
                        Session["Requests"] = woQuery;
                    }
                }
            }
        }

        protected void btn_PeoviderRequestSentSelected_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ClickedBtn.Value = (RBLUnsuspend.SelectedIndex + 1).ToString(CultureInfo.InvariantCulture);


                foreach (GridViewRow row in grd_Requests.Rows)
                {
                    var checkBox = row.FindControl("SelectItem") as CheckBox;
                    if (checkBox == null || !checkBox.Checked) continue;
                    var dataKey = grd_Requests.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var workOrderRequestId = Convert.ToInt32(dataKey["ID"]);
                    var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                    if (request == null) continue;



                    //request.IsProviderRequest = false;
                    //context.SubmitChanges();
                    HandleProviderRequest(workOrderRequestId);


                }
                CheckGroupIdAndBindGrid();
                lbl_ProcessResult.Text = Tokens.SelectedRequestsApproved;
                lbl_ProcessResult.ForeColor = Color.Green;
                HttpContext.Current.Response.Redirect(Request.RawUrl);

            }
        }

       


        private void UpdateDemonds(DateTime time, int? workOrderId, int userId, int workOrderStatusId,
            DemandService demandService, int cancelOption = -1)
        {
            if (workOrderId == null) return;
            demandService.HandleCancelRequest(workOrderId.Value, time, userId, workOrderStatusId,
                cancelOption: cancelOption);
        }



        private static void NotifyUserByProcess(WorkOrder updatedwo, Request requ, ISPDataContext context)
        {
            var active = context.EmailCnfgs.FirstOrDefault();
            if (active == null || !active.Active) return;


            var msg =
                "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                ":" + Tokens.Customer_Name + "</span></div>" + updatedwo.CustomerName +
                "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + ":" + Tokens.PhoneNo +
                "</div> <span><br/> " + updatedwo.CustomerPhone +
                "</h3><p style='padding: 15px;border: 1px solid #ddd;display: inline-block;margin: 0px auto;'>" +
                Tokens.ApprovedLiteral + "<br/>" + requ.RequestName + "</p></div>";

            var branch = updatedwo.BranchID;
            var branchAdmin = context.Users.FirstOrDefault(x => x.BranchID == branch && x.GroupID == 4);
            var brAdEmail = branchAdmin!=null? branchAdmin.UserEmail:"";
            var formalmessage =
                ClsEmail.Body(msg);
            if (updatedwo.User != null)
            {
                ClsEmail.SendEmail(updatedwo.User.UserEmail,
                    ConfigurationManager.AppSettings["InstallationEmail"]
                    , ConfigurationManager.AppSettings["CC2Email"],
                    "Customer: " + updatedwo.CustomerPhone, formalmessage
                    , true, brAdEmail);
            }
            else
            {
                using (var context2 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    var wors2 = context2.WorkOrderRequests.Select(x => x).ToList();
                    var wors = wors2.LastOrDefault(x => x.WorkOrderID == updatedwo.ID);
                    if (wors != null)
                    {
                        var usr = context2.Users.FirstOrDefault(x => x.ID == wors.SenderID);
                        if (usr != null)
                        {
                            var sendr = usr.UserEmail;
                            if (sendr != null)
                            {
                                ClsEmail.SendEmail(sendr,
                                 ConfigurationManager.AppSettings["InstallationEmail"]
                                 , ConfigurationManager.AppSettings["CC2Email"],
                                 "Customer: " + updatedwo.CustomerPhone, formalmessage
                                 , true, brAdEmail);
                            }
                        }
                    }
                }
            }

        }


        protected void lnb_Reject_Click(object sender, EventArgs e)
        {
            var rejectedId = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            ViewState.Add("RejectedID", rejectedId);
        }


        protected void btn_reject_Click(object sender, EventArgs e)
        {
            var orderRequestId = Convert.ToInt32(ViewState["RejectedID"]);
            RejectRequest(orderRequestId);
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        private void RejectRequest(int orderRequestId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var workOrderRequest = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == orderRequestId);
                if (workOrderRequest == null) return;
                workOrderRequest.RSID = 2;
                workOrderRequest.RejectReason = TbRejectReason.Text;
                workOrderRequest.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                workOrderRequest.ProcessDate = DateTime.Now.AddHours();
                context.SubmitChanges();

                var workOrderStatuses =
                    context.WorkOrderStatus.Where(wost => wost.WorkOrderID == workOrderRequest.WorkOrderID).ToList();
                var statusId = workOrderStatuses.Last().StatusID;
                if (statusId != null)
                {
                    var lastStatusId = statusId.Value;
                    var currentWorkOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == workOrderRequest.WorkOrderID);
                    if (currentWorkOrder != null)
                    {
                        currentWorkOrder.WorkOrderStatusID = lastStatusId;
                        var option = OptionsService.GetOptions(context, true);
                        if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                        {
                            CenterMessage.SendRequestReject(currentWorkOrder, TbRejectReason.Text,
                                workOrderRequest.Request.RequestName, Convert.ToInt32(Session["User_ID"]));
                        }
                    }
                }
                context.SubmitChanges();
                lbl_ProcessResult.Text = Tokens.RequestRejected;
                lbl_ProcessResult.ForeColor = Color.Green;
                CheckGroupIdAndBindGrid();
                ViewState["RejectedID"] = null;

            }
        }


        private void UpdateRequestDate(WorkOrder updatedwo, DemandService demandService, int daysCount = 0)
        {
            demandService.UpdateRequestDate(updatedwo.ID, Convert.ToDateTime(TbUnsuspendDate.Text), 11, daysCount);
        }


        private void UpdateRequestDatePostpone(WorkOrder updatedwo, DemandService demandService, int postponed = 0)
        {
            demandService.UpdateRequestDate(updatedwo.ID, Convert.ToDateTime(TbUnsuspendDate.Text), 11,
                postponed: postponed);
        }



        protected void btn_RejectSelected_Click(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var option = OptionsService.GetOptions(context, true);
                foreach (GridViewRow row in grd_Requests.Rows)
                {
                    var checkBox = row.FindControl("SelectItem") as CheckBox;
                    var chk = checkBox;
                    if (chk == null || !chk.Checked) continue;
                    var dataKey = grd_Requests.DataKeys[row.RowIndex];
                    if (dataKey == null) continue;
                    var id = Convert.ToInt32(dataKey["ID"]);
                    var worQuery = context.WorkOrderRequests.Where(wor => wor.ID == id);
                    var updatedwor = worQuery.FirstOrDefault();
                    if (updatedwor == null) continue;
                    updatedwor.RSID = 2;
                    updatedwor.ConfirmerID = Convert.ToInt32(Session["User_ID"]);
                    updatedwor.ProcessDate = DateTime.Now.AddHours();
                    context.SubmitChanges();
                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                    {
                        CenterMessage.SendRequestReject(updatedwor.WorkOrder, TbRejectReason.Text,
                            updatedwor.Request.RequestName, Convert.ToInt32(Session["User_ID"]));
                    }
                }
                CheckGroupIdAndBindGrid();
                lbl_ProcessResult.Text = Tokens.SelectedRequestsRejected;
                lbl_ProcessResult.ForeColor = Color.Green;
                HttpContext.Current.Response.Redirect(Request.RawUrl);
            }
        }




        protected void grd_Requests_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            Helper.GridViewNumbering(grd_Requests, "lbl_No");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = "<b>" + e.Row.Cells[6].Text + "</b>";
                if (e.Row.Cells[6].Text.Trim() == "<b>Active</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.Green;
                }
                if (e.Row.Cells[6].Text.Trim() == "<b>Suspend</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.OrangeRed;
                }
                if (e.Row.Cells[6].Text.Trim() == "<b>Cancelled</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.Red;
                }
                if (e.Row.Cells[6].Text.Trim() == "<b>Cancellation Process</b>")
                {
                    e.Row.Cells[6].ForeColor = Color.Red;
                }


                if (ViewState["requestId"] != null && Convert.ToInt32(ViewState["requestId"]) > 0)
                {
                    HtmlButton btn = (HtmlButton)e.Row.FindControl("btnapproved");
                    HtmlAnchor lnkBtn = (HtmlAnchor)e.Row.FindControl("lnb_Approve");
                    //CheckBox chckbx = (CheckBox) e.Row.FindControl("SelectItem");
                    var sId = Convert.ToInt32(ViewState["requestId"]);

                    if (sId == 3)
                    {
                        lnkBtn.Visible = false;
                        //chckbx.Visible = false;
                        btn.Visible = true;

                    }
                    else
                    {
                        lnkBtn.Visible = true;
                        //chckbx.Visible = true;
                        btn.Visible = false;

                    }

                }

                //HtmlAnchor lnka = (HtmlAnchor)e.Row.FindControl("lnb_Approve");
                //lnka.Attributes.Add("onclick", "addDialog(" + lnka + ")");

            }

        }

        //unhold
        protected void BtnSave_OnServerClick(object sender, EventArgs e)
        {
            var ids = SelectedStuff.Value.Split(',');
            var requestId = Convert.ToInt32(ids[0]);
            var orderId = Convert.ToInt32(ids[1]);
            ConfirmSuspend(requestId, orderId);
            CheckGroupIdAndBindGrid();
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }

        //unhold with Liquidation
        protected void BtnSaveWithLiquidation_Click(object sender, EventArgs e)
        {
            var ids = SelectedStuff.Value.Split(',');
            var requestId = Convert.ToInt32(ids[0]);
            var orderId = Convert.ToInt32(ids[1]);
            ConfirmSuspend(requestId, orderId);
            var date = Convert.ToDateTime(TbMiscDate.Text);
            Liquidation(orderId, date);
            CheckGroupIdAndBindGrid();
            HttpContext.Current.Response.Redirect(Request.RawUrl);

        }


        private void ConfirmSuspend(int workorderRequest, int workorderId)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workorderRequest);
                if (request == null) return;
                request.RSID = 1;
                int confirmerId = Convert.ToInt32(Session["User_ID"]);
                request.ConfirmerID = confirmerId;

                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);

                var order = context.WorkOrders.FirstOrDefault(wo => wo.ID == workorderId);
                if (order == null) return;
                HandleRequest(requestTypeId, order, request, context, confirmerId);
                try
                {
                    var query = context.WorkOrders.Where(wo => wo.ID == request.WorkOrderID);
                    var updatedwo = query.FirstOrDefault();
                    var requ = context.Requests.FirstOrDefault(r => r.ID == requestTypeId);
                    if (requ != null)
                    {
                        NotifyUserByProcess(updatedwo, requ, context);
                    }
                }
                catch
                {
                    lbl_ProcessResult.Text = Tokens.ErrorMsg;
                    lbl_ProcessResult.ForeColor = Color.Red;
                }
                lbl_ProcessResult.Text = Tokens.RequestAdded;
                lbl_ProcessResult.ForeColor = Color.Green;
                context.SubmitChanges();
            }
        }


        private void Liquidation(int orderId, DateTime date)
        {
            var ispEntries = new IspEntries();
            var order = ispEntries.GetWorkOrder(orderId);
            var demand = ispEntries.OrderDemand(orderId).OrderByDescending(x => x.Id).FirstOrDefault();
            var dem = ispEntries.OrderDemand2(orderId, date).ToList();
            var demand2 = dem.LastOrDefault();

            if (demand2 == null || order == null) return;
            demand2.CaseDetectSuspend = Convert.ToDecimal(demand2.Amount);
            var startAt = demand2.StartAt;
            var activation = _work.GetActivationDate(orderId);
            decimal basicBill = 0;
            if (activation != null)
            {
                basicBill = _priceServices.BillDefault(order, startAt.Month, startAt.Year, null).Net;
            }

            const int daysInMonth = 30;
            var oldPaid = demand2.Paid;

            var spentDays = (date.Date - demand2.StartAt.Date).Days + 1;

            var netRequired = basicBill * Convert.ToDecimal(spentDays) / Convert.ToDecimal(daysInMonth);
            if (order.Offer != null)
            {
                var offer = order.Offer;
                var offerStart = order.OfferStart;
                if (offerStart != null)
                {

                    var offerEnd = offerStart.Value.AddMonths(order.Offer.LifeTime);
                    if (date.Date >= offerStart.Value.Date && date.Date < offerEnd.Date)
                    {
                        netRequired -= OfferPricingServices.GetOfferPrice(offer, netRequired, basicBill);
                    }
                }
            }

            // demand2.Notes = "حساب الفترة حتى ايقاف مؤقت بتاريخ : " + date.Date;
            if (!oldPaid)
            {
                demand2.Amount = netRequired;
                demand2.Notes = "حساب الفترة حتى ايقاف مؤقت بتاريخ : " + date.Date;
            }
            else
            {
                var amount = netRequired - demand2.Amount;
                var start = demand2.StartAt;
                var end = demand2.EndAt;
                var workOrder = demand2.WorkOrder;
                var not = "حساب الفترة حتى ايقاف مؤقت بتاريخ : " + date.Date;
                ;
                var factory = new DemandFactory(new IspEntries());
                var newdemand = factory.CreateDemand(workOrder, start, end,
                    amount,
                    Convert.ToInt32(Session["User_ID"]), notes: not);
                ispEntries.AddDemand(newdemand);
            }

            ispEntries.Commit();

        }


        protected void UnsuspendWithPostponeSuspendDays(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("1");
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        protected void UnsusPendWithDeductionWithFixedRequestDate(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("2");
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        protected void UnsusPendWithCompleteInvoice(object sender, EventArgs e)
        {
            HandleUnsuspendRequest("3");
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }


        private void HandleUnsuspendRequest(string whichbtn)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ClickedBtn.Value = whichbtn;
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);
                var dataKey = UnsuspendId.Value;
                var workOrderRequestId = Convert.ToInt32(dataKey);
                var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                if (request == null) return;
                request.RSID = 1;
                var confirmerId = Convert.ToInt32(Session["User_ID"]);
                request.ConfirmerID = confirmerId;
                context.SubmitChanges();
                var workOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == request.WorkOrderID);
                HandleRequest(requestTypeId, workOrder, request, context, confirmerId);
                try
                {
                    var requ = context.Requests.FirstOrDefault(r => r.ID == requestTypeId);
                    if (requ != null)
                    {
                        NotifyUserByProcess(workOrder, requ, context);
                    }
                }
                catch
                {
                    lbl_ProcessResult.Text = Tokens.ErrorMsg;
                    lbl_ProcessResult.ForeColor = Color.Red;
                }
                lbl_ProcessResult.Text = Tokens.SelectedRequestsApproved;
                lbl_ProcessResult.ForeColor = Color.Green;
                CheckGroupIdAndBindGrid();
            }
        }


        protected void UpGradeDwon(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                var requestTypeId = Convert.ToInt32(que);
                var dataKey = UnsuspendId.Value;
                var workOrderRequestId = Convert.ToInt32(dataKey);
                var request = context.WorkOrderRequests.FirstOrDefault(wor => wor.ID == workOrderRequestId);
                var workOrder = context.WorkOrders.FirstOrDefault(wo => wo.ID == request.WorkOrderID);
                var confirmerId = Convert.ToInt32(Session["User_ID"]);
                if (request == null) return;
                HandleRequest(requestTypeId, workOrder, request, context, confirmerId);
                request.RSID = 1;

                request.ConfirmerID = confirmerId;


                var option = context.Options.FirstOrDefault();
                if (option != null)
                {
                    var fromType = option.ConvertFromPackageType;
                    var toType = option.ConvertToPackageType;
                    var debt = option.ConversionDebt;

                    //type
                    var currentPackageType =
                        context.ServicePackages.FirstOrDefault(x => x.ID == request.CurrentPackageID);
                    var newPackageType = context.ServicePackages.FirstOrDefault(x => x.ID == request.NewPackageID);
                    if (debt > 0 && newPackageType != null && currentPackageType != null &&
                        currentPackageType.ServicePackageTypeID == fromType &&
                        newPackageType.ServicePackageTypeID == toType && fromType > 0 && toType > 0)
                    {
                        IspEntries _ispEntries = new IspEntries(context);
                        var dateD = Convert.ToDateTime(TbUpDwonDate.Text);
                        var demandFactory = new DemandFactory(_ispEntries);
                        var demand = demandFactory
                            .CreateDemand(workOrder,
                                Convert.ToDateTime(dateD.Date),
                                Convert.ToDateTime(dateD.Date),
                                Convert.ToDecimal(debt),
                                Convert.ToInt32(Session["User_ID"]),
                                DateTime.Now.AddHours(),
                                false,
                                string.Format("قيمة التحويل من {0} الى {1} ",
                                    currentPackageType.ServicePackagesType.SPTName,
                                    newPackageType.ServicePackagesType.SPTName)
                            );
                        demand.IsResellerCommisstions = false;
                        _ispEntries.AddDemands(demand);
                        _ispEntries.Commit();
                    }

                }


                context.SubmitChanges();

                try
                {
                    var requ = context.Requests.FirstOrDefault(r => r.ID == requestTypeId);
                    if (requ != null)
                    {
                        NotifyUserByProcess(workOrder, requ, context);
                    }
                }
                catch
                {
                    lbl_ProcessResult.Text = Tokens.ErrorMsg;
                    lbl_ProcessResult.ForeColor = Color.Red;
                }
                lbl_ProcessResult.Text = Tokens.SelectedRequestsApproved;
                lbl_ProcessResult.ForeColor = Color.Green;
                CheckGroupIdAndBindGrid();
                HttpContext.Current.Response.Redirect(Request.RawUrl);
            }
        }


        protected void RejectSelectedRequest(object sender, EventArgs e)
        {
            var orderRequestId = Convert.ToInt32(RejectedRequestId.Value);
            RejectRequest(orderRequestId);
            HttpContext.Current.Response.Redirect(Request.RawUrl);
        }

        protected void ddlServiceProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                if (ddlServiceProvider.SelectedIndex > 0)
                {
                    var providerId = Convert.ToInt32(ddlServiceProvider.SelectedItem.Value);
                    var packages = context.ServicePackages.Where(a => a.ProviderId == providerId).ToList();
                    DdlPackage.DataSource = packages;
                    DdlPackage.DataTextField = "ServicePackageName";
                    DdlPackage.DataValueField = "ID";
                    DdlPackage.DataBind();
                    PopulateOffers(context, providerId);
                }
                else
                {
                    ddlServiceProvider.SelectedIndex = -1;
                    ddlServiceProvider.DataSource = null;
                    ddlServiceProvider.DataBind();
                    PopulateOffers(context);
                }
            }
        }


        protected void DdlOffer_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                ISPDataContext db9 = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]);
                if (DdlOffer.SelectedIndex < 1)
                {
                    var offers = db9.OfferProviderPackages.Select(z => z.ServicePackage).ToList();
                    Helper.BindDrop(DdlPackage, offers, "ServicePackageName", "ID");
                }
                else
                {
                    var id = Convert.ToInt32(DdlOffer.SelectedValue);

                    var offers =
                        db9.OfferProviderPackages.Where(z => z.OfferId == id).Select(z => z.ServicePackage).ToList();
                    Helper.BindDrop(DdlPackage, offers, "ServicePackageName", "ID");
                }
            }
        }

        private void PopulateOffers(ISPDataContext db9, int providerId = 0)
        {
            var user = db9.Users.FirstOrDefault(u => u.ID == Convert.ToInt32(Session["User_ID"]));
            if (user == null) return;
            if (providerId.Equals(0))
            {
                Helper.BindDrop(DdlOffer, null, "", "");

                return;
            }
            var provider = db9.ServiceProviders.FirstOrDefault(p => p.ID == providerId);
            if (provider == null)
            {
                Helper.BindDrop(DdlOffer, null, "", "");

                return;
            }
            var domian = new IspDomian(db9);
            var offers = domian.ProviderOffers(provider, user);
            Helper.BindDrop(DdlOffer, offers, "Title", "Id");
        }

        protected void ddlPayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var woQuery = new List<ManageRequestTemplate>();
                if (ddlPayType.SelectedIndex > 0)
                {
                    var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);

                    woQuery = DataLevelClass.GetUserNonCofirmedWoRequests(Convert.ToInt32(que));

                    hdnreq.Value = que;

                    var PType = Convert.ToInt32(ddlPayType.SelectedItem.Value);
                    //var requestList = (List<ManageRequestTemplate>)Session["AllRequests"];

                    grd_Requests.DataSource = woQuery.Where(a => a.PaymentTypeId == PType).ToList();
                    grd_Requests.DataBind();


                }
                else
                {
                    var que = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);

                    woQuery = DataLevelClass.GetUserNonCofirmedWoRequests(Convert.ToInt32(que));

                    hdnreq.Value = que;


                    grd_Requests.DataSource = woQuery;
                    grd_Requests.DataBind();
                }
                var groupIdQuery = context.Users.Where(usr => usr.ID == Convert.ToInt32(Session["User_ID"]));
                var gId = groupIdQuery.FirstOrDefault();
                if (gId == null) return;
                var id = gId.Group.DataLevelID;
                if (id == null) return;
                var groupId = id.Value;
                if (string.IsNullOrEmpty(Request.QueryString["rid"]))
                {
                    grd_Requests.Visible = false;
                }
                else
                {
                    var que2 = QueryStringSecurity.Decrypt(Request.QueryString["rid"]);
                    var idInQueryString = Convert.ToInt32(que2);
                    var columns = grd_Requests.Columns;
                    GridHelper.HideAllColumns(columns);
                    var columnNames = new List<string>
                        {
                            "#",
                            Tokens.Customer,
                            Tokens.Phone,
                            Tokens.Governrate,
                            Tokens.Central,
                            Tokens.CurrentSpeed,
                            Tokens.Status,
                            Tokens.Provider,
                            Tokens.Reseller,
                            Tokens.Branch,
                            Tokens.SenderName,
                            Tokens.Activation_Date,
                            Tokens.Offer,
                            Tokens.Request_Date,
                            Tokens.PaymentType,
                            Tokens.UserName,
                            Tokens.Notes,
                            Tokens.InvoiceDueDate

                        };
                    switch (idInQueryString)
                    {
                        // Upgrade-Downgrade
                        case 1:
                            columnNames.AddRange(new List<string>
                                {
                                    Tokens.New_Service_Package,
                                    Tokens.CurrentSpeed
                                });
                            break;
                        case 3:
                            columnNames.AddRange(new List<string>
                                {
                                    Tokens.SuspendDaysCount
                                });
                            break;
                        // Ip Package
                        case 8:
                            columnNames.AddRange(new List<string>
                                {
                                    Tokens.New_IP_Package
                                });
                            break;

                        case 9:
                            columnNames.AddRange(new List<string>
                                {
                                    Tokens.Extra_Gigas
                                });
                            break;
                    }
                    //sys admin, sys employee
                    if (groupId == 1)
                    {
                        columnNames.AddRange(new List<string>
                            {
                                Tokens.Approve,
                                Tokens.Reject,
                                Tokens.Select,
                                Tokens.isprorequest
                            });

                        tbl_Control.Visible = woQuery.Count > 0;
                    }
                    else
                    {
                        tbl_Control.Visible =
                            provrequestall.Visible =
                                btn_ApproveSelected.Visible = btn_RejectSelected.Visible = false;
                    }
                    GridHelper.ShowExactColumns(columns, columnNames);
                }
            }
        }
    }
}