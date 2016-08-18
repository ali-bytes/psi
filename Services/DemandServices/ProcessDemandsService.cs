using System;
using System.Configuration;
using System.Linq;
using BL.Concrete;
using Db;
using NewIspNL.Services.OfferServices;

namespace NewIspNL.Services.DemandServices{
    public class ProcessDemandsService{
        readonly DemandFactory _demandFactory;

        readonly IspEntries _ispEntries;


        public ProcessDemandsService(IspEntries ispEntries, DemandFactory demandFactory){
            _ispEntries = ispEntries;
            _demandFactory = demandFactory;
        }


        public void CreateActivationDemand(int orderId, DateTime startAt,
            DateTime endAt,decimal amount, DateTime offerStart, bool isPaid,
            int userId, bool payRouterCost = false){//, DateTime calculationDate
            string notes = string.Empty;
            var order = _ispEntries.GetWorkOrder(orderId);
            if(order == null) return;
            var deleteOffer = false;
            decimal demandAmount = 0;
            var endDate = new DateTime();
            var calculationDate = startAt;
            if(order.OfferId == null){
                //done
                // without offer
                endDate = startAt.AddMonths(1);
                demandAmount = amount;
            } else{
                var offer = order.Offer;
                if(order.OfferStart != null){
                    var offerEndDate = order.OfferStart.Value.AddMonths(offer.LifeTime);
                    var plusMonth = startAt.AddMonths(1);
                    if(calculationDate.Date >= offerStart.Date && calculationDate.Date < offerEndDate.Date){
                        var monthOfferPrice = OfferPricingServices.GetOfferPrice(offer, amount,amount);
                        var netDemand = amount - monthOfferPrice;
                        if(offer.FreeMonths > 0){
                            if(offer.CalculateOneBill){
                                notes = "فاتور مجمعة";
                                var offerMonthCount = offer.LifeTime - offer.FreeMonths;
                                demandAmount = netDemand * offerMonthCount;
                                endDate = startAt.AddMonths(offer.LifeTime);
                            } else{
                                int freeMonthsCount = order.Offer.FreeMonths;
                                int previousDemandsCount = order.Demands.Count;
                                if(offer.FreeMonthsFirst){
                                    if(freeMonthsCount > previousDemandsCount && previousDemandsCount < order.Offer.LifeTime){
                                        notes = "فاتورة مجانية حسب العرض";
                                        demandAmount = 0;
                                        endDate = plusMonth;
                                    } else{
                                        demandAmount = netDemand;
                                        endDate = plusMonth;
                                    }
                                } else{
                                    //if(freeMonthsCount <= previousDemandsCount && previousDemandsCount < order.Offer.LifeTime)
                                    // change condition
                                    if (previousDemandsCount < (order.Offer.LifeTime - freeMonthsCount))
                                    {  demandAmount = netDemand;
                                        endDate = plusMonth;
                                      
                                    } else 
                                    {
                                        // compare freemonthinoffer with current free demands with workorder
                                        int previousfreeDemandsCount = order.Demands.Count(x => x.Notes == "فاتورة مجانية حسب العرض");
                                        if (previousfreeDemandsCount < freeMonthsCount)
                                        {
                                             notes = "فاتورة مجانية حسب العرض";
                                        demandAmount = 0;
                                        endDate = plusMonth;
                                        }
                                        else
                                        {
                                            demandAmount = netDemand;
                                            endDate = plusMonth;
                                        }
                                      
                                    }
                                }
                            }
                        } else{
                            if(offer.CalculateOneBill){
                                notes = "فاتورة مجمعة";
                                demandAmount = offer.LifeTime * netDemand;
                                endDate = startAt.AddMonths(offer.LifeTime);
                            } else{
                                demandAmount = netDemand;
                                endDate = plusMonth;
                            }
                        }
                        /*if(payRouterCost && offer.RouterCost > 0){
                            notes = "فاتورة + قيمة الروتر";
                            demandAmount += offer.RouterCost;
                        }*/
                    } else{
                        endDate = plusMonth;
                        demandAmount = amount;
                        deleteOffer = true;
                    }
                }
            }

            order.RequestDate = endDate;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var paymentAmount = Convert.ToDecimal(order.PaymentType.Amount);
                demandAmount += paymentAmount;
                var optionPro = context.OptionInvoiceProviders.ToList();
                foreach (var provider in optionPro)
                {
                    if (order.ServiceProviderID == provider.ProviderId)
                    {
                        var demand = _demandFactory.CreateDemand(order, startAt, endDate, demandAmount, userId,
                            notes: notes);
                        _ispEntries.AddDemands(demand);
                    }
                }
                _ispEntries.Commit();
                if (!deleteOffer) return;
                order.Offer = null;
                _ispEntries.Commit();
                // الغاء العرض على العميل
                if (order.ResellerID != null && order.ResellerID != -1)
                {
                    var option = OptionsService.GetOptions(context, true);
                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                    {
                        const string offermessage = "تم الغاء العرض لهذا العميل";
                        CenterMessage.SendPublicMessageReport(order, offermessage, userId);
                    }
                }
            }
        }
        public void CreateActivationDemandReporsts(int orderId, DateTime startAt,
    DateTime endAt, decimal amount, DateTime offerStart, bool isPaid,
    int userId, bool payRouterCost = false)//, DateTime 
        {
            string notes = string.Empty;
            var order = _ispEntries.GetWorkOrder(orderId);
            if (order == null) return;
            var deleteOffer = false;
            decimal demandAmount = 0;
            var endDate = new DateTime();
            var calculationDate = startAt;
            if (order.OfferId == null)
            {
                //done
                // without offer
                endDate = startAt.AddMonths(1);
                demandAmount = amount;
            }
            else
            {
                Offer offer = order.Offer;
                if (order.OfferStart != null)
                {
                    DateTime offerEndDate = order.OfferStart.Value.AddMonths(offer.LifeTime);
                    DateTime plusMonth = startAt.AddMonths(1);
                    if (calculationDate.Date >= offerStart.Date && calculationDate.Date < offerEndDate.Date)
                    {
                        var monthOfferPrice = OfferPricingServices.GetOfferPrice(offer, amount,amount);
                        var netDemand = amount - monthOfferPrice;
                        if (offer.FreeMonths > 0)
                        {
                            if (offer.CalculateOneBill)
                            {
                                notes = "فاتور مجمعة";
                                int offerMonthCount = offer.LifeTime - offer.FreeMonths;
                                demandAmount = netDemand * offerMonthCount;
                                endDate = startAt.AddMonths(offer.LifeTime);
                            }
                            else
                            {
                                int freeMonthsCount = order.Offer.FreeMonths;
                                int previousDemandsCount = order.Demands.Count;
                                if (offer.FreeMonthsFirst)
                                {
                                    if (freeMonthsCount > previousDemandsCount && previousDemandsCount < order.Offer.LifeTime)
                                    {
                                        notes = "فاتورة مجانية حسب العرض";
                                        demandAmount = 0;
                                        endDate = plusMonth;
                                    }
                                    else
                                    {
                                        demandAmount = netDemand;
                                        endDate = plusMonth;
                                    }
                                }
                                else
                                {
                                    if (freeMonthsCount <= previousDemandsCount && previousDemandsCount < order.Offer.LifeTime)
                                    {
                                        notes = "فاتورة مجانية حسب العرض";
                                        demandAmount = 0;
                                        endDate = plusMonth;
                                    }
                                    else
                                    {
                                        demandAmount = netDemand;
                                        endDate = plusMonth;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (offer.CalculateOneBill)
                            {
                                notes = "فاتورة مجمعة";
                                demandAmount = offer.LifeTime * netDemand;
                                endDate = startAt.AddMonths(offer.LifeTime);
                            }
                            else
                            {
                                demandAmount = netDemand;
                                endDate = plusMonth;
                            }
                        }
                        /*if(payRouterCost && offer.RouterCost > 0){
                            notes = "فاتورة + قيمة الروتر";
                            demandAmount += offer.RouterCost;
                        }*/
                    }
                    else
                    {
                        endDate = plusMonth;
                        demandAmount = amount;
                        deleteOffer = true;
                    }
                }
            }

            order.RequestDate = endDate;
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var paymentAmount = Convert.ToDecimal(order.PaymentType.Amount);
                demandAmount += paymentAmount;
                var optionPro = context.OptionInvoiceProviders.ToList();
                foreach (var provider in optionPro)
                {
                    if (order.ServiceProviderID == provider.ProviderId)
                    {
                        if (order.Demands.Count == 1)
                        {
                            var firstDemand = order.Demands.FirstOrDefault(a => a.Notes.Contains("مدفوع مقدما"));
                            if (firstDemand != null && firstDemand.Paid) demandAmount -= firstDemand.Amount;
                        }

                        var demand = _demandFactory.CreateDemand(order, startAt, endDate, demandAmount, userId,
                            notes: notes);
                        _ispEntries.AddDemands(demand);
                    }
                }
                _ispEntries.Commit();
                if (!deleteOffer) return;
                order.Offer = null;
                _ispEntries.Commit();
                // الغاء العرض على العميل
                if (order.ResellerID != null && order.ResellerID != -1)
                {
                    var option = OptionsService.GetOptions(context, true);
                    if (option != null && Convert.ToBoolean(option.SendMessageAfterOperations))
                    {
                        const string offermessage = "تم الغاء العرض لهذا العميل";
                        CenterMessage.SendPublicMessageReport(order, offermessage, userId);
                    }
                }
            }
        }
    }
}
