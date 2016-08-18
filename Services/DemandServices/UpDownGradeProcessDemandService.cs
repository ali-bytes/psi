using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Concrete;
using Db;
using NewIspNL.Domain.Concrete;
using NewIspNL.Helpers;
using NewIspNL.Models;
using NewIspNL.Services.OfferServices;
using Resources;

namespace NewIspNL.Services.DemandServices{
    public class UpDownGradeProcessDemandService{
        readonly DemandFactory _demandFactory;



        readonly PriceServices _priceServices;
        private readonly IspEntries _ispEntries;

        public UpDownGradeProcessDemandService(DemandFactory demandFactory, PriceServices priceServices){
            _demandFactory = demandFactory;

            _priceServices = priceServices;
            _ispEntries = new IspEntries();
        }

        public DemandsReportContainer HandleUpDownGradeFirstChoise(DateTime date, int orderId, int newPackageId, int userId,
             bool previewOnly = false, bool twoInvoices = false)
        {

            var ispEntries = new IspEntries();

            var order = ispEntries.GetWorkOrder(orderId);
            /*var demand = ispEntries.OrderDemand(orderId).OrderByDescending(x => x.Id).FirstOrDefault();*/
            var dem = ispEntries.OrderDemand2(orderId, date).ToList();
            var demand2 = dem.LastOrDefault();

            var container = new DemandsReportContainer
            {
                After = new List<Demand>()
            };

            if (order == null) return container;
            var newPack = ispEntries.ServicePackages().FirstOrDefault(x => x.ID == newPackageId);
            var basicBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, order.ServicePackageID??0);
            if (demand2 == null)
            {
                order.ServicePackage = newPack;
                ispEntries.Commit();
                return container;
            }
            
            if (order.OfferId != null && basicBill.Net !=0) basicBill.Net = basicBill.Net - OfferPricingServices.GetOfferPrice(order.Offer, basicBill.Net, basicBill.Net);

            int days = (date.Date - demand2.StartAt.Date).Days ;
            const int daysInMonth = 30;
            #region PeriodsInfo


            var packageInfo1 = new PackageInfo
            {
                Period = days,
                Price = 0,
                Name = basicBill.PackageName
            };

            var packageInfo2 = new PackageInfo
            {
                Period = 30,
                Price = 0,
                Name = ""
            };


            #endregion

            // calculate old package periode
            var passedPeriodPercent = Convert.ToDecimal(days) / Convert.ToDecimal(daysInMonth);
            var passedPart = basicBill.Net * passedPeriodPercent;
            // calculate new package period
            var newPackBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, newPackageId);

            var newPackageNet = newPackBill.Net;
            packageInfo2.Name = newPackBill.PackageName;

            var endPlusDay = date;
            if (order.OfferId != null && newPackageNet != 0)
            {
                newPackageNet = newPackageNet - OfferPricingServices.GetOfferPrice(order.Offer, newPackageNet, newPackageNet);
            }

            var newSpeedPrice = newPackageNet;
            packageInfo1.Price = passedPart;
            packageInfo2.Price = newSpeedPrice;
            var allNotes = new StringBuilder().Append(Tokens.Upgrade_Downgrade).Append("<br/>").Append(packageInfo1.PrintInfo()).Append(packageInfo2.PrintInfo()).Append("تصفية السرعة القديمة وشهر كامل بالسرعة الجديدة").ToString();
            var paymentId = Convert.ToInt32(demand2.WorkOrder.PaymentTypeID);
            if (!demand2.Paid)
            {
                if (twoInvoices)
                {
                    // الحالة الاولى تصفية الفترة وشهر كامل
                    container.Title = Tokens.PartialInvoiceForOldSpeed;
                    demand2.Amount = passedPart;
                    demand2.Notes = allNotes;
                    var newAmount = AddPaymentWay(newPackageNet, paymentId);
                    var fullDemand = _demandFactory.CreateDemand(order, endPlusDay, endPlusDay.AddMonths(1),
                        newAmount, userId, notes: allNotes);
                    demand2.EndAt = date;
                    if (previewOnly)
                    {
                        container.After.Add(_demandFactory.ToFakeDemand(demand2, order));
                        container.After.Add(_demandFactory.ToFakeDemand(fullDemand, order));
                    }
                    else
                    {
                        order.ServicePackage = newPack;
                        order.RequestDate = endPlusDay.AddMonths(1);
                        ispEntries.AddDemand(fullDemand);
                        ispEntries.Commit();
                    }
                }
            }
            else
            {
                var oldAmountForCustomer = basicBill.Net - passedPart;
                var netRequired = newPackageNet - oldAmountForCustomer;
                var neaMount = AddPaymentWay(netRequired, paymentId);
                var remainingDemand =
                    _demandFactory.CreateDemand(order, endPlusDay, date.AddDays(1).AddMonths(1),
                        neaMount, userId, notes: allNotes);
                
                // الحالى الاولى
                if (twoInvoices)
                {
                    container.Title = Tokens.MergeSpeedsInOneInvoice;
                    remainingDemand.Amount = neaMount;

                }
                if (previewOnly)
                {
                    container.After.Add(_demandFactory.ToFakeDemand(remainingDemand, order));
                }
                else
                {
                    order.ServicePackage = newPack;
                    order.RequestDate = endPlusDay.AddMonths(1);
                    ispEntries.AddDemand(remainingDemand);
                    ispEntries.Commit();
                }
            }


            return container;
        }
        public DemandsReportContainer HandleUpDownGradeSecoundChoise(DateTime date, int orderId, int newPackageId, int userId,
            bool previewOnly = false, bool twoInvoices = false)
        {

            var allNotes = new StringBuilder();
            var ispEntries = new IspEntries();

            var order = ispEntries.GetWorkOrder(orderId);
            var dem = ispEntries.OrderDemand2(orderId, date).ToList();
            var demand2 = dem.LastOrDefault();
            #region PeriodsInfo

            // فى الحالة الثانية دى هتبقى  الفترة من اول الشهر لحد الطلب
            var packageInfo1 = new PackageInfo
            {
                Period = 0,
                Price = 0,
                Name = ""//basicBill.ServicePackageName
            };
            // فى الحالة الثانية هتبقى الفترة على السرعة الجديدة لاخر الشهر
            var packageInfo2 = new PackageInfo
            {
                Period = 0,
                Price = 0,
                Name = ""//basicBill.ServicePackageName,
            };

            var packageInfo3 = new PackageInfo
            {
                Period = 0,
                Price = 0,
                Name = ""
            };

            #endregion

            var container = new DemandsReportContainer
            {
                After = new List<Demand>()
            };

            if (order == null) return container;
            const int daysInMonth = 30; //DateTime.DaysInMonth(date.Year, date.Month);
            var newPack = ispEntries.ServicePackages().FirstOrDefault(x => x.ID == newPackageId);
            var basicBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, order.ServicePackageID??0);

            if (demand2 == null)
            {
                order.ServicePackage = newPack;
                ispEntries.Commit();
                return container;
            }
            //من بداية المطالبة مش من بداية الشهر
            var monthStart = demand2.StartAt;//new DateTime(date.Year, date.Month, 1);
            var monthEnd = demand2.EndAt;//monthStart.AddMonths(1).AddDays(-1);
            var newPackBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, newPackageId);
            var newPackageNet = newPackBill.Net;
            if (order.OfferId != null && newPackageNet !=0)
            {
                newPackageNet = newPackageNet - OfferPricingServices.GetOfferPrice(order.Offer, newPackageNet, newPackageNet);
            }

            if (order.OfferId != null && basicBill.Net !=0) basicBill.Net = basicBill.Net - OfferPricingServices.GetOfferPrice(order.Offer, basicBill.Net, newPackageNet);

            int firestPerioddays, secoundPerioddays, thirdPerioddays;
            decimal firstpassedPart = 0, secoundpassedPart = 0, thirstpassedpart = 0, firstpassedPeriodPercent = 0, secoundpassedPeriodPercent = 0, remainingpercent = 0;

            var lastUpDownRequest = ispEntries
                .GetOrderRequests(orderId)
                .Where(x => x.RequestID == 1 && x.ProcessDate != null && x.ProcessDate.Value.Date <= monthEnd.Date && x.ProcessDate.Value.Date >= monthStart.Date)
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            if (lastUpDownRequest != null && lastUpDownRequest.ProcessDate != null && lastUpDownRequest.CurrentPackageID != null)
            {
                // الفترة بين الطلب الحالى واخر طلب فى حالة وجود طلب قديم فى نفس الشهر
                var p3Bill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, lastUpDownRequest.CurrentPackageID.Value);
                var p3 = p3Bill.Net;
                if (order.OfferId != null && p3 !=0)
                {
                    p3 = p3 - OfferPricingServices.GetOfferPrice(order.Offer, lastUpDownRequest.CurrentPackageID.Value, p3);

                }
                firestPerioddays = (lastUpDownRequest.ProcessDate.Value.Date - monthStart.Date).Days;
                secoundPerioddays = (date.Date - lastUpDownRequest.ProcessDate.Value.Date).Days + 1;

                firstpassedPeriodPercent = Convert.ToDecimal(firestPerioddays) / Convert.ToDecimal(daysInMonth);
                firstpassedPart = p3 * firstpassedPeriodPercent;

                secoundpassedPeriodPercent = Convert.ToDecimal(secoundPerioddays) / Convert.ToDecimal(daysInMonth);
                secoundpassedPart = basicBill.Net * secoundpassedPeriodPercent;

                thirdPerioddays = (monthEnd.Date - date.Date).Days - 1;
                var allperiod = (monthEnd.Date - monthStart.Date).Days;
                if (allperiod == 31) thirdPerioddays = thirdPerioddays - 1;
                if (allperiod == 28) thirdPerioddays += 2;
                //if (monthEnd.Day==1 && monthEnd.AddDays(-1).Day == 31) thirdPerioddays = thirdPerioddays-1;
                remainingpercent = Convert.ToDecimal(thirdPerioddays) / daysInMonth;
                thirstpassedpart = newPackageNet * remainingpercent;

                packageInfo1.Period = firestPerioddays;
                packageInfo1.Price = firstpassedPart;
                packageInfo1.Name = p3Bill.PackageName;

                packageInfo2.Period = secoundPerioddays;
                packageInfo2.Price = secoundpassedPart;
                packageInfo2.Name = basicBill.PackageName;

                packageInfo3.Name = newPackBill.PackageName;
                packageInfo3.Period = thirdPerioddays;
                packageInfo3.Price = thirstpassedpart;
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                allNotes.Append(Tokens.Upgrade_Downgrade).Append("<br/>").Append(packageInfo1.PrintInfo()).Append(packageInfo2.PrintInfo()).Append(packageInfo3.PrintInfo()).ToString();
            }
            else
            {
                // الفترة بين اول المطالبة والطلب الحالى
                firestPerioddays = (date.Date - monthStart.Date).Days;

                firstpassedPeriodPercent = Convert.ToDecimal(firestPerioddays) / Convert.ToDecimal(daysInMonth);
                firstpassedPart = basicBill.Net * firstpassedPeriodPercent;

                thirdPerioddays = (monthEnd.Date - date.Date).Days;
                var allperiod = (monthEnd.Date - monthStart.Date).Days;
                if (allperiod == 31) thirdPerioddays = thirdPerioddays - 1;
                if (allperiod == 28) thirdPerioddays += 2;
                //if (monthEnd.Day == 1 && monthEnd.AddDays(-1).Day == 31) thirdPerioddays = thirdPerioddays - 1;
                remainingpercent = Convert.ToDecimal(thirdPerioddays) / daysInMonth;
                thirstpassedpart = newPackageNet * remainingpercent;


                packageInfo1.Period = firestPerioddays;
                packageInfo1.Price = firstpassedPart;
                packageInfo1.Name = basicBill.PackageName;

                packageInfo3.Name = newPackBill.PackageName;
                packageInfo3.Period = thirdPerioddays;
                packageInfo3.Price = thirstpassedpart;
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                allNotes.Append(Tokens.Upgrade_Downgrade).Append("<br/>").Append(packageInfo1.PrintInfo()).Append(packageInfo3.PrintInfo()).ToString();
            }

            var bothPackagesRequired = firstpassedPart + secoundpassedPart + thirstpassedpart;
            var paymentId = Convert.ToInt32(demand2.WorkOrder.PaymentTypeID);
            if (!demand2.Paid)
            {
                //  الحالة الثانية مجموع الكل فى فاتورة واحدة
                container.Title = Tokens.MergeSpeedsInOneInvoice;
                var newAmount = AddPaymentWay(bothPackagesRequired, paymentId);
                demand2.Amount = newAmount;
                demand2.Notes = allNotes.ToString();
                if (previewOnly)
                {
                    container.After.Add(_demandFactory.ToFakeDemand(demand2, order));
                }
                else
                {
                    order.ServicePackage = newPack;
                    ispEntries.Commit();
                }
            }
            else
            {
                var oldAmountForCustomer = demand2.Amount;
                var netRequired = bothPackagesRequired - oldAmountForCustomer;
                var amout = AddPaymentWay(netRequired, paymentId);
                var remainingDemand =
                    _demandFactory.CreateDemand(order, monthStart, monthEnd,
                        amout, userId, notes: allNotes.ToString());
                if (previewOnly)
                {
                    container.After.Add(_demandFactory.ToFakeDemand(remainingDemand, order));
                }
                else
                {
                    order.ServicePackage = newPack;
                    ispEntries.AddDemand(remainingDemand);
                    ispEntries.Commit();
                }
            }


            return container;
        }
        public DemandsReportContainer HandleUpDownGradeThirdChoice(DateTime date, int orderId, int newPackageId, int userId,
           bool previewOnly = false, bool twoInvoices = false)
        {
            // فاتورة كاملة بالشهر القديم وفاتورة كاملة بالشهر الجديد
            var notes = new StringBuilder();
            var ispEntries = new IspEntries();

            var order = ispEntries.GetWorkOrder(orderId);
            var dem = ispEntries.OrderDemand2(orderId, date).ToList();
            var demand2 = dem.LastOrDefault();

            var container = new DemandsReportContainer
            {
                After = new List<Demand>()
            };
            var newPack = ispEntries.ServicePackages().FirstOrDefault(x => x.ID == newPackageId);
            if (demand2 == null)
            {
                order.ServicePackage = newPack;
                ispEntries.Commit();
                return container;
            }
           
            #region PeriodsInfo


          
            var packageInfo2 = new PackageInfo
            {
                Period = 30,
                Price = 0,
                Name = ""
            };

           

            #endregion

            var newPackBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, newPackageId);
            var newPackageNet = newPackBill.Net;
            packageInfo2.Name = newPackBill.PackageName;

            var endPlusDay = date;
            if (order.OfferId != null && newPackageNet !=0)
            {
                newPackageNet = newPackageNet - OfferPricingServices.GetOfferPrice(order.Offer, newPackageNet, newPackageNet);
            }

           
            var newSpeedNotes = new StringBuilder();

            if (newPack != null) newSpeedNotes.Append(", " + Tokens.Month);
            var paymentId = Convert.ToInt32(demand2.WorkOrder.PaymentTypeID);
            var amo = AddPaymentWay(newPackageNet, paymentId);

             packageInfo2.Price = amo;
             var allNotes = new StringBuilder().Append(Tokens.Upgrade_Downgrade).Append("<br/>").Append(packageInfo2.PrintInfo()).Append("فاتورة كاملة بالشهر القديم وفاتورة كاملة بالشهر الجديد").ToString();
            var fullDemand =
                _demandFactory.CreateDemand(order, endPlusDay, endPlusDay.AddMonths(1),
                    amo, userId, notes: allNotes);
            demand2.EndAt = date;
            if (previewOnly)
            {
                container.After.Add(_demandFactory.ToFakeDemand(demand2, order));
                container.After.Add(_demandFactory.ToFakeDemand(fullDemand, order));
            }
            else
            {
                order.ServicePackage = newPack;
                order.RequestDate = endPlusDay.AddMonths(1);
                ispEntries.AddDemand(fullDemand);
                ispEntries.Commit();
            }
            return container;
        }
        public DemandsReportContainer HandleUpDownGradeFourthChoice(DateTime date, int orderId, int newPackageId, int userId,
           bool previewOnly = false, bool twoInvoices = false)
        {

           
            var ispEntries = new IspEntries();

            var order = ispEntries.GetWorkOrder(orderId);
            var dem = ispEntries.OrderDemand2(orderId, date).ToList();
            var demand2 = dem.LastOrDefault();

            var container = new DemandsReportContainer
            {
                After = new List<Demand>()
            };
            var newPack = ispEntries.ServicePackages().FirstOrDefault(x => x.ID == newPackageId);
            if (demand2 == null)
            {
                order.ServicePackage = newPack;
                ispEntries.Commit();
                return container;
            }

      
           const int daysInMonth = 30;
            var spentdays = (demand2.EndAt - date).Days + 1;
           
            var allperiod = (demand2.EndAt.Date - demand2.StartAt.Date).Days;
            if (allperiod == 31) spentdays = spentdays - 1;
            if (allperiod == 28) spentdays += 2;

            #region PeriodsInfo


          
            var packageInfo2 = new PackageInfo
            {
                Period = spentdays,
                Price = 0,
                Name = ""
            };

            #endregion

            var newPackBill = _priceServices.CustomerInvoiceDetailsByPackageWithoutResellerDiscount(order, date.Month, date.Year, newPackageId);
            var newPackageNet = newPackBill.Net;
            packageInfo2.Name = newPackBill.PackageName;
          
            if (order.OfferId != null && newPackageNet !=0)
            {
                newPackageNet = newPackageNet - OfferPricingServices.GetOfferPrice(order.Offer, newPackageNet, spentdays);
            }
            
            var newSpeedNotes = new StringBuilder();

            if (newPack != null) newSpeedNotes.Append(", " + Tokens.Month);

            var paymentId = Convert.ToInt32(demand2.WorkOrder.PaymentTypeID);
            var netRequired = newPackageNet / daysInMonth * spentdays;
            var amoun = AddPaymentWay(netRequired, paymentId);
            packageInfo2.Price = amoun;
            var allNotes = new StringBuilder().Append(Tokens.Upgrade_Downgrade).Append("<br/>").Append(packageInfo2.PrintInfo()).Append("<br/> شهر كامل للسرعة القديمة وتصفية السرعة الجديدة من : ").Append(date).ToString();
            var remainingDemand =
                _demandFactory.CreateDemand(order, date, demand2.EndAt,
                amoun, userId, notes: allNotes);
            container.Title = Tokens.PartialInvoiceForOldSpeed;
          
            if (previewOnly)
            {
                container.After.Add(_demandFactory.ToFakeDemand(remainingDemand, order));
            }
            else
            {
                
                order.ServicePackage = newPack;
                ispEntries.AddDemand(remainingDemand);
                ispEntries.Commit();
            }


            return container;
        }

        public class PackageInfo{
            public string Name { get; set; } 
            public decimal Price { get; set; }

            public int Period { get; set; }


            public string PrintInfo(){
                if(string.IsNullOrWhiteSpace(Name)) return string.Empty;

                var builder = new StringBuilder();
                const string dashPeriod = ": ";
                const string comma = ", ";
                const string br = "</br>";
                var tPrice = Helper.FixNumberFormat(Price);
                builder.Append(Name).Append(dashPeriod)
                    .Append(Tokens.Price).Append(dashPeriod).Append(tPrice)
                    .Append(comma).Append(Tokens.Period).Append(dashPeriod).Append(Period)
                    .Append(br);

                return builder.ToString();
            }
        }

        public decimal AddPaymentWay(decimal amount,int paymentId)
        {
            var am = _ispEntries.PaymentType(paymentId);
            return amount + Convert.ToDecimal(am.Amount);
        }
    }
}
